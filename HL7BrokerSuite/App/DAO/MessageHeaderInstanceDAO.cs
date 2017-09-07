using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using HL7BrokerSuite.Utility;
using HL7BrokerSuite.Settings;
using HL7BrokerSuite.App.Model;
using HL7BrokerSuite.Sys.Model;


namespace HL7BrokerSuite.App.DAO
{
    public class MessageHeaderInstanceDAO
    {
        // insert statement specific to get identity
        public const string SCOPE_IDENTITY = " SET @id=SCOPE_IDENTITY();";
        public const string AT_SET_ID = "@id";

        // regular column names
        public const string ID = "ID";
        public const string SENDING_APPLICATION = "SENDING_APPLICATION";
        public const string RECEIVING_APPLICATION = "RECEIVING_APPLICATION";
        public const string SENDING_FACILITY = "SENDING_FACILITY";
        public const string RECEIVING_FACILTIY = "RECEIVING_FACILTIY";
        public const string MESSAGE_DTTM = "MESSAGE_DTTM";
        public const string MESSAGE_CONTROL_ID = "MESSAGE_CONTROL_ID";
        public const string MESSAGE_TYPE = "MESSAGE_TYPE";
        public const string VERSION_ID = "VERSION_ID";
        public const string APPLICATION_ACK_TYPE = "APPLICATION_ACK_TYPE";
        public const string ACCEPT_ACK_TYPE = "ACCEPT_ACK_TYPE";
        public const string ORDER_CONTROL_CODE = "ORDER_CONTROL_CODE";
        public const string PATIENT_IDENTIFIER = "PATIENT_IDENTIFIER";
        public const string PROCESSED = "PROCESSED";
        public const string PROCESSED_DTTM = "PROCESSED_DTTM";
        public const string PROCESSED_COUNT = "PROCESSED_COUNT";
        public const string PENDING_REPROCESS_DTTM = "PENDING_REPROCESS_DTTM";
        public const string CREATED_DTTM = "CREATED_DTTM";
        

        // view column names
        public const string VIEW_MHI_ID                     = "MHI_ID";
        public const string VIEW_MHI_SENDING_APPLICATION    = "MHI_SENDING_APPLICATION";
        public const string VIEW_MHI_RECEIVING_APPLICATION  = "MHI_RECEIVING_APPLICATION";
        public const string VIEW_MHI_SENDING_FACILITY       = "MHI_SENDING_FACILITY";
        public const string VIEW_MHI_RECEIVING_FACILTIY     = "MHI_RECEIVING_FACILTIY";
        public const string VIEW_MHI_MESSAGE_DTTM           = "MHI_MESSAGE_DTTM";
        public const string VIEW_MHI_MESSAGE_CONTROL_ID     = "MHI_MESSAGE_CONTROL_ID";
        public const string VIEW_MHI_MESSAGE_TYPE           = "MHI_MESSAGE_TYPE";
        public const string VIEW_MHI_VERSION_ID             = "MHI_VERSION_ID";
        public const string VIEW_MHI_APPLICATION_ACK_TYPE   = "MHI_APPLICATION_ACK_TYPE";
        public const string VIEW_MHI_ACCEPT_ACK_TYPE        = "MHI_ACCEPT_ACK_TYPE";
        public const string VIEW_MHI_ORDER_CONTROL_CODE     = "MHI_ORDER_CONTROL_CODE";
        public const string VIEW_MHI_PATIENT_IDENTIFIER     = "MHI_PATIENT_IDENTIFIER";
        public const string VIEW_MHI_PROCESSED              = "MHI_PROCESSED";
        public const string VIEW_MHI_PROCESSED_DTTM         = "MHI_PROCESSED_DTTM";
        public const string VIEW_MHI_PROCESSED_COUNT        = "MHI_PROCESSED_COUNT";
        public const string VIEW_MHI_CREATED_DTTM           = "MHI_CREATED_DTTM";
        public const string VIEW_MHI_PENDING_REPROCESS_DTTM = "MHI_PENDING_REPROCESS_DTTM";

        // view parameter names
        public const string AT_VIEW_MHI_ID = "@MHI_ID";
        public const string AT_VIEW_MHI_SENDING_APPLICATION = "@MHI_SENDING_APPLICATION";
        public const string AT_VIEW_MHI_RECEIVING_APPLICATION = "@MHI_RECEIVING_APPLICATION";
        public const string AT_VIEW_MHI_SENDING_FACILITY = "@MHI_SENDING_FACILITY";
        public const string AT_VIEW_MHI_RECEIVING_FACILTIY = "@MHI_RECEIVING_FACILTIY";
        public const string AT_VIEW_MHI_MESSAGE_DTTM = "@MHI_MESSAGE_DTTM";
        public const string AT_VIEW_MHI_MESSAGE_CONTROL_ID = "@MHI_MESSAGE_CONTROL_ID";
        public const string AT_VIEW_MHI_MESSAGE_TYPE = "@MHI_MESSAGE_TYPE";
        public const string AT_VIEW_MHI_VERSION_ID = "@MHI_VERSION_ID";
        public const string AT_VIEW_MHI_APPLICATION_ACK_TYPE = "@MHI_APPLICATION_ACK_TYPE";
        public const string AT_VIEW_MHI_ACCEPT_ACK_TYPE = "@MHI_ACCEPT_ACK_TYPE";
        public const string AT_VIEW_MHI_ORDER_CONTROL_CODE = "@MHI_ORDER_CONTROL_CODE";
        public const string AT_VIEW_MHI_PATIENT_IDENTIFIER = "@MHI_PATIENT_IDENTIFIER";
        public const string AT_VIEW_MHI_PROCESSED = "@MHI_PROCESSED";
        public const string AT_VIEW_MHI_PROCESSED_DTTM = "@MHI_PROCESSED_DTTM";
        public const string AT_VIEW_MHI_PROCESSED_COUNT = "@MHI_PROCESSED_COUNT";
        public const string AT_VIEW_MHI_CREATED_DTTM = "@MHI_CREATED_DTTM"; 	

        // parameter names 
        public const string AT_ID = "@ID";
        public const string AT_SENDING_APPLICATION = "@SENDING_APPLICATION";
        public const string AT_RECEIVING_APPLICATION = "@RECEIVING_APPLICATION";
        public const string AT_SENDING_FACILITY = "@SENDING_FACILITY";
        public const string AT_RECEIVING_FACILTIY = "@RECEIVING_FACILTIY";
        public const string AT_MESSAGE_DTTM = "@MESSAGE_DTTM";
        public const string AT_MESSAGE_CONTROL_ID = "@MESSAGE_CONTROL_ID";
        public const string AT_MESSAGE_TYPE = "@MESSAGE_TYPE";
        public const string AT_VERSION_ID = "@VERSION_ID";
        public const string AT_APPLICATION_ACK_TYPE = "@APPLICATION_ACK_TYPE";
        public const string AT_ACCEPT_ACK_TYPE = "@ACCEPT_ACK_TYPE";
        public const string AT_ORDER_CONTROL_CODE = "@ORDER_CONTROL_CODE";
        public const string AT_PATIENT_IDENTIFIER = "@PATIENT_IDENTIFIER";
        public const string AT_PROCSSED = "@PROCESSED";
        public const string AT_PROCESSED_DTTM = "@PROCESSED_DTTM";
        public const string AT_PROCESSED_COUNT = "@PROCESSED_COUNT";
        public const string AT_CREATED_DTTM = "@CREATED_DTTM"; 	
        public const string AT_MESSAGE_ID   = "@MESSAGE_ID";
        public const string AT_ISREPROCESS  = "@ISREPROCESS";

        // custom which is not normally like the other ones cause it's auto generated
        public static int insertIntoMessageHeaderInstance(SqlConnection connection, 
                                                          SqlCommand command, 
                                                          SqlTransaction transaction)
        {
            int identityResult = -1;
            try
            {
                // create sql parameter for return value
                SqlParameter identityParm = new SqlParameter();

                // append command text to get identity
                command.CommandText += SCOPE_IDENTITY;

                // create identity parameter
                identityParm.ParameterName = AT_SET_ID;
                identityParm.DbType = System.Data.DbType.Int32;
                identityParm.Direction = System.Data.ParameterDirection.Output;

                // add identity param
                command.Parameters.Add(identityParm);

                // execute non query to set transaction
                command.ExecuteNonQuery();

                // commit all database transactions
                transaction.Commit();

                // return the scope identity
                identityResult = (int)identityParm.Value;
            }
            catch (SqlException sqlEx)
            {
                ErrorLogger.LogError(sqlEx, "insertIntoMessageHeaderInstance()", command.CommandText);
                identityResult = -1;
                transaction.Rollback();
            }
            finally 
            {
                connection.Close();
            }
            return identityResult;
        }


        public static List<MessageHeaderInstance> Get()
        {
            return getMessageHeaderInstances();
        }

        public static List<MessageHeaderInstance> Get(MessageHeaderInstance messageHeaderInstance)
        {
            return getSingleMessageHeaderInstance(messageHeaderInstance);
        }

        public static MessageHeaderInstance PostUpdate(MessageHeaderInstance messageHeaderInstance,
                                                                                    Message message)
        {
            return updateMessageHeaderInstanceToProcessed(messageHeaderInstance, message);
        }

        public static MessageHeaderInstance ReprocessMessage(MessageHeaderInstance messageHeaderInstance,
                                                                                    Message message)
        {
            return updateMessageHeaderInstanceToProcessed(messageHeaderInstance, message,true);
        }

        public static bool Delete(MessageHeaderInstance messageHeaderInstance)
        {
            return deleteMessageHeaderInstance(messageHeaderInstance);
        }

        private static MessageHeaderInstance updateMessageHeaderInstanceToProcessed(MessageHeaderInstance messageHeaderInstance, 
                                                                                    Message message, 
                                                                                    bool isReprocess = false)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.UPDATE_MESSAGE_HEADER_INSTANCE_PROCESS;
                command.Parameters.AddWithValue(MessageHeaderInstanceDAO.AT_ID, messageHeaderInstance.id);
                command.Parameters.AddWithValue(MessageHeaderInstanceDAO.AT_MESSAGE_ID, message.id);
                command.Parameters.AddWithValue(MessageHeaderInstanceDAO.AT_ISREPROCESS,isReprocess);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageHeaderInstance.id = DAOUtility.GetData<int>(reader, MessageHeaderInstanceDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "updateMessageHeaderInstanceToProcessed()", messageHeaderInstance.id.ToString());
                }
            }
            return messageHeaderInstance;
        }

        private static List<MessageHeaderInstance> getMessageHeaderInstances()
        {
            List<MessageHeaderInstance> messageHeaderInstances = new List<MessageHeaderInstance>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_MESSAGE_HEADER_INSTANCE;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageHeaderInstances.Add(new MessageHeaderInstance(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getMessageHeaderInstances()");
                }
            }

            return messageHeaderInstances;
        }

        private static List<MessageHeaderInstance> getSingleMessageHeaderInstance(MessageHeaderInstance messageHeaderInstance)
        {
            List<MessageHeaderInstance> messageHeaderInstances = new List<MessageHeaderInstance>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_MESSAGE_HEADER_INSTANCE;
                command.Parameters.AddWithValue(MessageHeaderInstanceDAO.AT_ID, messageHeaderInstance.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageHeaderInstances.Add(new MessageHeaderInstance(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleMessageHeaderInstance(MessageHeaderInstance MessageHeaderInstance)", messageHeaderInstance.id.ToString());
                }
            }

            return messageHeaderInstances;
        }
  
        private static bool deleteMessageHeaderInstance(MessageHeaderInstance messageHeaderInstance)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_MESSAGE_HEADER_INSTANCE;
                command.Parameters.AddWithValue(MessageHeaderInstanceDAO.AT_ID, messageHeaderInstance.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageHeaderInstance.id = DAOUtility.GetData<int>(reader, MessageHeaderInstanceDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteMessageHeaderInstance()", messageHeaderInstance.id.ToString());
                }
            }
            return true;
        }
        
 
    }
}
