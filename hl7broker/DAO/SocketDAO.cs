using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using HL7Broker.Model;
using HL7Broker.Model.HL7;
using HL7Broker.Utility;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Utility;
using HL7BrokerSuite.App.DAO;
using HL7BrokerSuite.App.Model;
using System.Net.Sockets;

namespace HL7Broker.DAO
{
    public class SocketDAO : Generic
    {        

        public static void handleIncomingHL7MessageToBroker(string hl7Input, 
                                                    Communication interfaceCommunication, 
                                                    HL7Broker.Model.Socket socket) 
        {
            // attempt to scrub message
            hl7Input = HL7MessageUtility.scrubHL7MessageForParse(hl7Input);

            // Get Incoming HL7 Message Configuration
            HL7Message hl7Message = HL7MessageDAO.getMessage(hl7Input);

            // if the message is not null
            if(hl7Message != null)
            {
                // set the hl7 message to the socket object
                socket.setHL7Message(hl7Message);
            }
        }


        public static void handleOutgoingHL7MessageToBroker(string hl7Input,
                                           Communication databaseCommunication,
                                           HL7Broker.Model.Socket socket)
        { 
            Dictionary<int, int> databaseTableRelationDictionary
               = new Dictionary<int, int>();

            Configuration masterConfig = socket.masterConfiguration;
            List<Credential> credentials = masterConfig.credentials;
            List<DatabaseTable> databaseTables = masterConfig.databaseTables;
            List<DatabaseInstance> databaseInstances = masterConfig.databaseInstances;
            List<ColumnSet> columnSets = masterConfig.columnSets;
            List<DatabaseTableRelation> relations = masterConfig.databaseTableRelations;

            // this identity is used for multiple inserts
            int identityNo = -1;

            // text
            string baseCommandText = BLANK;
            string tableName = BLANK;
            // these will help build the custom query
            List<string> columns = new List<string>();
            List<string> columnValues = new List<string>();

            // get message
            HL7Message hl7Message = socket.getHL7Message();

            // get configurations for the database comminucation
            DatabaseInstance databaseInstance
                = ConfigurationUtility.GetDatabaseInstance(databaseCommunication, databaseInstances);

            // credential 
            Credential credential
                = ConfigurationUtility.GetDatabseCredential(databaseInstance, credentials);

            // build the connection string
            string connectionString = DAOUtility.GetConnectionString(credential,
                                                                     databaseInstance, true);


            // build database query to insert data into tables based on configuration
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    List<DatabaseTable> configDatabaseTables
                        = ConfigurationUtility.GetDatabaseTables(databaseInstance, databaseTables);

                    // pick up all the tables for the config
                    configDatabaseTables.ForEach(delegate(DatabaseTable tableConfig)
                    {
                        // empty the db value
                        string dbValue = BLANK;
                        // reset variables
                        baseCommandText = BLANK;
                        // empty the stores
                        columns.Clear();
                        columnValues.Clear();

                        SqlCommand command = new SqlCommand();

                        // set the database table
                        DatabaseTable table = tableConfig;

                        List<ColumnSet> configDatabaseColumns
                            = ConfigurationUtility.GetColumnSets(table, columnSets);

                        // set the table name
                        tableName = table.name;

                        // for each column, get the column name
                        configDatabaseColumns.ForEach(delegate(ColumnSet configColumn)
                        {
                            // set the column
                            ColumnSet column = configColumn;

                            // get the data type to use
                            SqlDbType sqlDbType = DAOUtility.GetSqlDataType(column.columnDataType);

                            // if it's 0, there must be some special condition here, such as a primary key or handling a specific data type
                            if (column.messageGroupInstanceId == ZERO &&
                                column.columnDataType == INTEGER_TYPE)
                            {
                                // if the source table does have a relationship, then we need to get it
                                if (relations.Count(x => x.sourceDatabaseTableId == table.id) > ZERO)
                                {
                                    if (column.isPrimaryKey)
                                    {
                                        // do something here
                                        int keyValue = -1;

                                        // get the target table based on the source table
                                        int targetTableId = relations.Find(r => r.sourceDatabaseTableId ==
                                                                           table.id).targetDatabaseTableId;

                                        // get the identity
                                        databaseTableRelationDictionary.TryGetValue(targetTableId, out keyValue);

                                        // set the keyvalue to the db value
                                        dbValue = keyValue.ToString();
                                    }
                                    else
                                    {
                                        // Throw Error - there is an issue if the column value is of interger type but no 
                                        // key is assoicated it it.
                                    }
                                }
                            }
                            // if this isn't bound to a column and it's of type text - then it should ALWAYS be an HL7 Message
                            else if (column.messageGroupInstanceId == ZERO &&
                                    column.columnDataType == TEXT_TYPE)
                            {
                                // apply hl7 message
                                dbValue = hl7Input;
                            }
                            // database column is of current date type, so we set the value to the current date time
                            else if (column.messageGroupInstanceId == ZERO &&
                                     column.columnDataType == DATE_TYPE)
                            {
                                // set the value to the current date time
                                dbValue = DateTime.Now.ToString();
                            }
                            // search for it by hl7 position
                            else
                            {
                                // init a message group instance
                                MessageGroupInstance messageGroupInstance = new MessageGroupInstance()
                                {
                                    id = column.messageGroupInstanceId
                                };

                                // get the message position for this specific column
                                List<Configuration> configMessagePoisition
                                    = ConfigurationDAO.GetMessagePosition(messageGroupInstance);

                                // get the coordinates for the message item
                                string messageCoordinates = HL7MessageUtility.getItemCoordinates(configMessagePoisition);

                                // get segment string type
                                string columnSegment = configMessagePoisition[ZERO_ELEMENT].segmentType.name;

                                // get the value for the database inside of the value position
                                string hl7Value = HL7MessageUtility.getValueByPosition(
                                                                    hl7Message,
                                                                    HL7MessageUtility.getSegmentType(columnSegment),
                                                                    messageCoordinates);

                                // generate the value
                                dbValue = (!String.IsNullOrEmpty(hl7Value) ? hl7Value : DBNull.Value.ToString());
                            }

                            // initialize for each new table
                            baseCommandText
                                = DAOUtility.GetBaseQuery(DAOUtility.SERVICE_QUERY_TYPE.INSERT);

                            // add to columns statement
                            columns.Add(column.name);

                            // add to values statement
                            columnValues.Add(AT_SIGN + column.name);

                            // set the command string
                            command.Parameters.Add(AT_SIGN + column.name, sqlDbType);

                            // set the values
                            command.Parameters[AT_SIGN + column.name].Value = dbValue;

                        });

                        // open the connection
                        connection.Open();

                        // grab command text
                        command.CommandText = DAOUtility.GetStatement(DAOUtility.SERVICE_QUERY_TYPE.INSERT,
                                                                                            baseCommandText,
                                                                                            tableName,
                                                                                            columns,
                                                                                            columnValues);

                        // make new transaction to allow roll back
                        SqlTransaction transaction = connection.BeginTransaction();

                        // initialize the connection
                        command.Connection = connection;

                        // initialize the transaction
                        command.Transaction = transaction;

                        // handle the database insert for message header instance
                        handleDatabaseTableCaller(connection,
                                             command,
                                             transaction,
                                             hl7Message,
                                             table,
                                             hl7Input,
                                             socket,
                                             out identityNo);

                        DatabaseTableRelation databaseTableRelation
                            = ConfigurationUtility.GetDatabaseRelation(table, relations);

                        if (databaseTableRelation == null)
                        {
                            // override object
                            databaseTableRelation = new DatabaseTableRelation() { id = 0 };
                        }

                        // if the "database table has dependencies" - store the relation id w/ the table id
                        if (databaseTableRelation.id != ZERO)
                        {
                            // or in better terms - has a dependency that this ide requires 
                            if (databaseTableRelation.requiresIdentity)
                            {
                                // add to the dictionary if it does require depdents
                                databaseTableRelationDictionary.Add(table.id, identityNo);
                            }
                            else
                            {
                                // reset if it doesn't need it
                                identityNo = NEG_ONE;
                            }
                        }
                    });
                }
                catch (SqlException sqlex)
                {
                    ErrorLogger.LogError(sqlex, "handleOutgoingHL7MessageToBroker()", hl7Input);
                    connection.Close();
                }
            }
        }

        public static void handleDatabaseTableCaller(SqlConnection connection, 
                                                    SqlCommand command, 
                                                    SqlTransaction transaction,
                                                    HL7Message hl7Message,
                                                    DatabaseTable databaseTable, 
                                                    string hl7Input,
                                                    HL7Broker.Model.Socket socket,
                                                    out int identity)
        {
            // set to negative one
            identity = -1;

            switch (databaseTable.id)    
            {
                case APP_MESSAGE_HEADER_INSTANCE:
                    identity = MessageHeaderInstanceDAO.insertIntoMessageHeaderInstance(
                        connection, 
                        command, 
                        transaction);
                    break;
                case APP_MESSAGE:
                    identity = MessageDAO.insertIntoMessage(
                        connection,
                        command,
                        transaction);

                    // get the type of log we are suppose to log
                    MessageLogDAO.MessageLogType logType
                        = handleAcknowledgement(hl7Message, hl7Input, identity, socket);

                    // pass it to the message log so we can log it
                    handleMessageLog(hl7Message, hl7Input, identity, logType);

                    // update the app broker stats
                    handleUpdateBrokerStats(identity, socket);
                    break;
                default:
                    break;
            }        
        }

        public static MessageLogDAO.MessageLogType handleAcknowledgement(HL7Message hl7Message, 
                                                                         string hl7Input, 
                                                                         int messageIdentity,
                                                                         HL7Broker.Model.Socket socket)
        {
            string ackResponseMessage = BLANK;

            AcknowledgementDAO.AcknowledgementType ackType 
                = AcknowledgementDAO.AcknowledgementType.UNKNOWN;

            MessageLogDAO.MessageLogType logType 
                = MessageLogDAO.MessageLogType.UNKNOWN;

            if (messageIdentity == NEG_ONE)
            {
                // swap it to 9999 so we don't insert a neg value
                messageIdentity = ERROR_CODE_NINE;
                ackType = AcknowledgementDAO.AcknowledgementType.AE;
                ackResponseMessage = ERROR_MESSAGE_FROM_BROKER;
            }
            else if (messageIdentity == ZERO) 
            {
                ackType = AcknowledgementDAO.AcknowledgementType.AR;
                ackResponseMessage = ERROR_MESSAGE_REJECTED;
            }
            else
            {
                ackType = AcknowledgementDAO.AcknowledgementType.AA;
                ackResponseMessage = MESSAGE_NO_ERROR;
            }

            // get the message control id to build the ack
            string messageControlId = HL7MessageUtility.getValueByPosition(hl7Message,
                                                                           SegmentType.MSH,
                                                                           MESSAGE_HEADER_CONTROL_ID);

            string messageDateStamp = HL7MessageUtility.getValueByPosition(hl7Message,
                                                                           SegmentType.MSH,
                                                                           MESSAGE_DATE_STAMP_POSITION);
            // get ack response
            string strAckResponse = HL7MessageUtility.getAck(ackType.ToString(),
                                                             messageControlId,
                                                             messageDateStamp,                                                             
                                                             MESSAGE_HEADER_APPLICATION_NAME,
                                                             MESSAGE_HEADER_FACILITY_NAME, 
                                                             ackResponseMessage);
            
            try
            {
                // build ack response
                Acknowledgement ackResponse = new Acknowledgement()
                {
                    id = NEG_ONE,
                    acknowledgementTypeId = (int)ackType,
                    messageId = messageIdentity,
                    raw = strAckResponse,
                    createdDttm = DateTime.Now
                };

                // insert into the acknowledgement 
                ackResponse = AcknowledgementDAO.insertIntoAcknowledgement(ackResponse);

                logType = (ackResponse.id != NEG_ONE) ? MessageLogDAO.MessageLogType.ORIGINAL
                                                      : MessageLogDAO.MessageLogType.ERRORED;
                
                
                // get the network stream to send the ack back
                NetworkStream stream = socket.getNetworkStream();
                
                // pad the hl7 msesage for transfer
                strAckResponse = HL7MessageUtility.padHL7MessageForTransfer(strAckResponse);

                stream.Write(Encoding.UTF8.GetBytes(strAckResponse), 0, strAckResponse.Length);

            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, "handleAcknowledgement()", messageControlId);
            }

            // send the ack message

            // return the type of log we are submitting
            return logType;
        }


        public static void handleUpdateBrokerStats(int messageIdentity,HL7Broker.Model.Socket socket)
        {
            socket.broker.lastMessageDTTM = DateTime.Now;
            socket.broker.lastMessageId = messageIdentity;
            BrokerDAO.UpdateAppBrokerProperty(socket.broker, BrokerDAO.Property.Stats);        
        }

        public static void handleMessageLog(HL7Message hl7Message, 
                                            string hl7Input,
                                            int messageIdentity, 
                                            MessageLogDAO.MessageLogType logType)
        {
            try
            {
                // build MessageLog Entry
                MessageLog messageLogEntry = new MessageLog()
                {
                    id = NEG_ONE,
                    messageLogTypeId = (int)logType,
                    messageId = messageIdentity,          
                    createdDttm = DateTime.Now
                };

                // insert into the MessageLog 
                messageLogEntry = MessageLogDAO.insertIntoMessageLog(messageLogEntry);
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, "handleMessageLog()", messageIdentity.ToString());
            }        
        }

        [Obsolete("This was the orignal method which would call DB for each mesage to get config - do not use", true)]
        public static void handleOutgoingHL7MessageToBrokerV1(string hl7Input,
                                                    Communication databaseCommunication,
                                                    HL7Broker.Model.Socket socket)
        {
            Dictionary<int, int> databaseTableRelationDictionary
                = new Dictionary<int, int>();

            // get all database table relations 
            List<DatabaseTableRelation> relations = DatabaseTableRelationDAO.Get();

            // this identity is used for multiple inserts
            int identityNo = -1;

            // text
            string baseCommandText = BLANK;
            string tableName = BLANK;
            // these will help build the custom query
            List<string> columns = new List<string>();
            List<string> columnValues = new List<string>();

            // get message
            HL7Message hl7Message = socket.getHL7Message();

            // get configurations for the database comminucation
            List<Configuration> configurations
                = ConfigurationDAO.GetDatabaseConfiguration(databaseCommunication);

            // get the credential
            Credential credential = configurations[ZERO_ELEMENT].credential;

            // get the database instance
            DatabaseInstance databaseInstance = configurations[ZERO_ELEMENT].databaseInstance;

            // build the connection string
            string connectionString = DAOUtility.GetConnectionString(credential,
                                                                     databaseInstance, true);


            // build database query to insert data into tables based on configuration
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // get a list of databses tables
                    List<Configuration> configDatabaseTables
                        = ConfigurationDAO.GetDatabaseTables(databaseInstance);

                    // pick up all the tables for the config
                    configDatabaseTables.ForEach(delegate(Configuration tableConfig)
                    {
                        // empty the db value
                        string dbValue = BLANK;
                        // reset variables
                        baseCommandText = BLANK;
                        // empty the stores
                        columns.Clear();
                        columnValues.Clear();

                        SqlCommand command = new SqlCommand();

                        // set the database table
                        DatabaseTable table = tableConfig.databaseTable;

                        // init database table relation
                        DatabaseTableRelation databaseTableRelation = tableConfig.databaseTableRelation;

                        // based on the table instance id , get the column set
                        List<Configuration> configDatabaseColumns
                                 = ConfigurationDAO.GetDatabaseColumns(table);

                        // set the table name
                        tableName = table.name;

                        // for each column, get the column name
                        configDatabaseColumns.ForEach(delegate(Configuration configColumn)
                        {
                            // set the column
                            ColumnSet column = configColumn.columnSet;

                            // get the data type to use
                            SqlDbType sqlDbType = DAOUtility.GetSqlDataType(column.columnDataType);

                            // if it's 0, there must be some special condition here, such as a primary key or handling a specific data type
                            if (column.messageGroupInstanceId == ZERO &&
                                column.columnDataType == INTEGER_TYPE)
                            {
                                // if the source table does have a relationship, then we need to get it
                                if (relations.Count(x => x.sourceDatabaseTableId == table.id) > ZERO)
                                {
                                    if (column.isPrimaryKey)
                                    {
                                        // do something here
                                        int keyValue = -1;

                                        // get the target table based on the source table
                                        int targetTableId = relations.Find(r => r.sourceDatabaseTableId ==
                                                                           table.id).targetDatabaseTableId;

                                        // get the identity
                                        databaseTableRelationDictionary.TryGetValue(targetTableId, out keyValue);

                                        // set the keyvalue to the db value
                                        dbValue = keyValue.ToString();
                                    }
                                    else
                                    {
                                        // Throw Error - there is an issue if the column value is of interger type but no 
                                        // key is assoicated it it.

                                    }
                                }
                            }
                            // if this isn't bound to a column and it's of type text - then it should ALWAYS be an HL7 Message
                            else if (column.messageGroupInstanceId == ZERO &&
                                    column.columnDataType == TEXT_TYPE)
                            {
                                // apply hl7 message
                                dbValue = hl7Input;
                            }
                            // database column is of current date type, so we set the value to the current date time
                            else if (column.messageGroupInstanceId == ZERO &&
                                     column.columnDataType == DATE_TYPE)
                            {
                                // set the value to the current date time
                                dbValue = DateTime.Now.ToString();
                            }
                            // search for it by hl7 position
                            else
                            {
                                // init a message group instance
                                MessageGroupInstance messageGroupInstance = new MessageGroupInstance()
                                {
                                    id = column.messageGroupInstanceId
                                };

                                // reset the message group instance
                                configColumn.messageGroupInstance = messageGroupInstance;

                                // get the message position for this specific column
                                List<Configuration> configMessagePoisition
                                    = ConfigurationDAO.GetMessagePosition(configColumn.messageGroupInstance);

                                // get the coordinates for the message item
                                string messageCoordinates = HL7MessageUtility.getItemCoordinates(configMessagePoisition);

                                // get segment string type
                                string columnSegment = configMessagePoisition[ZERO_ELEMENT].segmentType.name;

                                // get the value for the database inside of the value position
                                string hl7Value = HL7MessageUtility.getValueByPosition(
                                                                    hl7Message,
                                                                    HL7MessageUtility.getSegmentType(columnSegment),
                                                                    messageCoordinates);

                                // generate the value
                                dbValue = (!String.IsNullOrEmpty(hl7Value) ? hl7Value : DBNull.Value.ToString());
                            }

                            // initialize for each new table
                            baseCommandText
                                = DAOUtility.GetBaseQuery(DAOUtility.SERVICE_QUERY_TYPE.INSERT);

                            // add to columns statement
                            columns.Add(column.name);

                            // add to values statement
                            columnValues.Add(AT_SIGN + column.name);

                            // set the command string
                            command.Parameters.Add(AT_SIGN + column.name, sqlDbType);

                            // set the values
                            command.Parameters[AT_SIGN + column.name].Value = dbValue;

                        });

                        // open the connection
                        connection.Open();

                        // grab command text
                        command.CommandText = DAOUtility.GetStatement(DAOUtility.SERVICE_QUERY_TYPE.INSERT,
                                                                                            baseCommandText,
                                                                                            tableName,
                                                                                            columns,
                                                                                            columnValues);

                        // make new transaction to allow roll back
                        SqlTransaction transaction = connection.BeginTransaction();

                        // initialize the connection
                        command.Connection = connection;

                        // initialize the transaction
                        command.Transaction = transaction;

                        // handle the database insert for message header instance
                        handleDatabaseTableCaller(connection,
                                             command,
                                             transaction,
                                             hl7Message,
                                             table,
                                             hl7Input,
                                             socket,
                                             out identityNo);

                        // if the "database table has dependencies" - store the relation id w/ the table id
                        if (databaseTableRelation.id != ZERO)
                        {
                            // or in better terms - has a dependency that this ide requires 
                            if (databaseTableRelation.requiresIdentity)
                            {
                                // add to the dictionary if it does require depdents
                                databaseTableRelationDictionary.Add(table.id, identityNo);
                            }
                            else
                            {
                                // reset if it doesn't need it
                                identityNo = NEG_ONE;
                            }
                        }
                    });
                }
                catch (SqlException sqlex)
                {
                    ErrorLogger.LogError(sqlex, "handleOutgoingHL7MessageToBroker()", hl7Input);
                    connection.Close();
                }
            }
        }

    }
}
