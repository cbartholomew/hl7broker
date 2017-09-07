using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Settings;


namespace HL7BrokerSuite.Utility
{
    public class CopyConfigurationUtility
    {
        public const string DIRECTION_TYPE_OUTGOING = "OUTGOING";
        public const string DIRECTION_TYPE_INCOMING = "INCOMING";
        public const string COMM_TYPE_DATABASE      = "DATABASE";
        public const string COMM_TYPE_INTERFACE     = "INTERFACE";
        public const string COMM_TYPE_WEBSERVICE    = "WEBSERVICE";

        public enum CopyType
        {
            OUTGOING_DATABASE,            
            INCOMING_INTERFACE
        }

        public static bool Copy(Application fromApplication, Application toApplication, CopyType copyType)
        {
            bool processedFlag = false;

            switch (copyType)
            {
                case CopyType.OUTGOING_DATABASE:
                    // this will be used to copy outgoing database configuration
                    processedFlag = copyOutgoingDatabase(fromApplication, 
                        toApplication, 
                        DIRECTION_TYPE_OUTGOING, 
                        COMM_TYPE_DATABASE);
                    break;
                case CopyType.INCOMING_INTERFACE:
                    break;
                default:
                    break;
            }   

            return processedFlag;
        }

        private static bool copyOutgoingDatabase(Application inFromApplication,
                                                 Application inToApplication, 
                                                 string directionType, 
                                                 string communicationType)
        {
            try
            {
                int toApplicationIdentity = inFromApplication.id;
                int fromApplicationIdentity = inToApplication.id;
                // get all applications
                List<Configuration> applicationList = ConfigurationDAO.GetApplications(new Application() { id = fromApplicationIdentity });

                // get all communuication and direction Types
                Configuration masterConfigration = ConfigurationDAO.GetAllConfigurations();

                // get the specific application direction and comm you want
                Configuration fromApplication = applicationList.Find(a => a.communicationType.name == communicationType
                                                                            && a.directionType.name == directionType);


                // communication translation identities
                int toDirectionTypeId =
                    masterConfigration.directionTypes.Find(d => d.name == fromApplication.directionType.name).id;
                int toCommunicationTypeId =
                    masterConfigration.communicationTypes.Find(c => c.name == fromApplication.communicationType.name).id;

                // create a new application object with your current application identity
                Application toApplication = new Application()
                {
                    id = toApplicationIdentity
                };

                // insert a new communication with your exsisting application 
                Communication toCommunication =
                    CommunicationDAO.PostUpdate(new Communication()
                    {
                        applicationId = toApplication.id,
                        communicationTypeId = toCommunicationTypeId,
                        directionTypeId = toDirectionTypeId
                    });


                // get the database_instance information (credential id, name, server, ip) of the communication identity.
                DatabaseInstance fromDatabaseInstance =
                    masterConfigration.databaseInstances.Find(i => i.communicationId == fromApplication.communication.id);

                // get database_instance id of the copy from insert into new database_instance with info prior step
                DatabaseInstance toDatabaseInstance = new DatabaseInstance();

                // copy individual values as not top copy the reference (we need it later)
                toDatabaseInstance = fromDatabaseInstance.ShallowCopy();

                // override the communication id w/ the "to" communication id
                toDatabaseInstance.id = 0;
                toDatabaseInstance.communicationId = toCommunication.id;
                // insert new database instance - get the id from this request
                toDatabaseInstance = DatabaseInstanceDAO.PostUpdate(toDatabaseInstance);

                // get all database tables from the fromDatabaseInstance 
                List<Configuration> fromDatabaseTables = ConfigurationDAO.GetDatabaseTables(fromDatabaseInstance);

                // get the database table relations
                List<DatabaseTableRelation> databaseTableRelations = masterConfigration.databaseTableRelations;

                // create a new database relation object
                DatabaseTableRelation databaseTableRelation = new DatabaseTableRelation()
                {
                    id = 0,
                    requiresIdentity = true,
                    sourceDatabaseTableId = -1,
                    targetDatabaseTableId = -1
                };

                // foreach table that belongs to the from database_instance, get the from database_table information
                fromDatabaseTables.ForEach(delegate(Configuration configurationTable)
                {
                    // extract the database table from the configuration
                    DatabaseTable fromDatabaseTable = new DatabaseTable()
                    {
                        id = configurationTable.databaseTable.id,
                        databaseInstanceId = fromDatabaseInstance.id,
                        name = configurationTable.databaseTable.name
                    };

                    // create new database table
                    DatabaseTable toDatabaseTable = new DatabaseTable()
                    {
                        databaseInstanceId = toDatabaseInstance.id,
                        name = fromDatabaseTable.name
                    };

                    // insert new table into database_table with fromDatabaseTable information but use toDatabaseInstanceId
                    toDatabaseTable = DatabaseTableDAO.PostUpdate(toDatabaseTable);

                    // check for prior source relation
                    if (ConfigurationUtility.IsDatabaseTableRelation(configurationTable.databaseTable, databaseTableRelations))
                    {
                        databaseTableRelation.sourceDatabaseTableId = toDatabaseTable.id;
                    }

                    // check for prior target relation
                    if (ConfigurationUtility.IsDatabaseTableRelation(configurationTable.databaseTable, databaseTableRelations, true))
                    {
                        databaseTableRelation.targetDatabaseTableId = toDatabaseTable.id;
                    }

                    // based on the fromDatabaseTable get all column sets
                    List<Configuration> fromColumnSets = ConfigurationDAO.GetDatabaseColumns(fromDatabaseTable);

                    // foreach columnset that belongs to fromDatabaseColumn copy all information (except for the fromDatabaseTableId)
                    fromColumnSets.ForEach(delegate(Configuration configurationColumnSet)
                    {
                        // define the column set
                        ColumnSet fromColumnSet = new ColumnSet();
                        ColumnSet toColumnSet = new ColumnSet();

                        // get the column set from the configuration list
                        fromColumnSet = configurationColumnSet.columnSet;
                        fromColumnSet.databaseTableId = configurationColumnSet.databaseTable.id;

                        // do a shallow copy of its properties and override the ones you need
                        toColumnSet = fromColumnSet.ShallowCopy();
                        toColumnSet.id = 0;
                        toColumnSet.databaseTableId = toDatabaseTable.id;

                        // insert new toColumnSet using new toDAtabaseTable.id
                        toColumnSet = ColumnSetDAO.PostUpdate(toColumnSet);
                    });
                });

                // if relation source or target is not negative one - insert new relation
                if (databaseTableRelation.sourceDatabaseTableId != -1 &&
                   databaseTableRelation.targetDatabaseTableId != -1)
                {
                    databaseTableRelation = DatabaseTableRelationDAO.PostUpdate(databaseTableRelation);
                }
            }
            catch (Exception ex)
            {
                string fromApplicationId = inFromApplication.id.ToString();
                string toApplicationId = inToApplication.id.ToString();
                ErrorLogger.LogError(ex,
                    "CopyConfigurationUtility.CopyOutgoingDatabase(fromApplication, toApplication, directionType,communicationType)",
                    fromApplicationId + "|" +
                    toApplicationId   + "|" +
                    directionType     +  "|" +
                    communicationType);

                return false;
            }

            return true;
        }
    }
}
