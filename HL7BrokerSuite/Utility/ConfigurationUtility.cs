using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.App.Model;
using HL7BrokerSuite.App.DAO;
using HL7BrokerSuite.Settings;

namespace HL7BrokerSuite.Utility
{
    public class ConfigurationUtility : GenericUtility
    {
        public static DatabaseTableRelation GetDatabaseRelation(DatabaseTable databaseTable, List<DatabaseTableRelation> databaseRelations)
        {
            DatabaseTableRelation databaseRelation = new DatabaseTableRelation();

            databaseRelation = databaseRelations.Find(r => r.targetDatabaseTableId == databaseTable.id);

            return databaseRelation;

        }

        public static DatabaseInstance GetDatabaseInstance(Communication databaseCommunication, List<DatabaseInstance> databaseInstances)
        {
            DatabaseInstance databaseInstance = new DatabaseInstance();

            databaseInstance = databaseInstances.Find(i => i.communicationId == databaseCommunication.id);

            return databaseInstance;
        }

        public static Credential GetDatabseCredential(DatabaseInstance databaseInstance, List<Credential> credentials)
        {
            Credential credential = new Credential();

            credential = credentials.Find(c => c.id == databaseInstance.credentialId);

            return credential;
        }

        public static List<DatabaseTable> GetDatabaseTables(DatabaseInstance databaseInstance, List<DatabaseTable> databaseTables)
        {
            List<DatabaseTable> tempTables = new List<DatabaseTable>();

            tempTables = databaseTables.FindAll(t => t.databaseInstanceId == databaseInstance.id);

            return tempTables;
        }

        public static List<ColumnSet> GetColumnSets(DatabaseTable databaseTable, List<ColumnSet> columnSets)
        {
            List<ColumnSet> tempColumnSets = new List<ColumnSet>();

            tempColumnSets = columnSets.FindAll(c => c.databaseTableId == databaseTable.id);

            return tempColumnSets;
        }

        public static List<Configuration> GetIncomingInterfaces(List<Configuration> configs)
        {
            List<Configuration> tempConfig = new List<Configuration>();

            tempConfig = configs.FindAll(c => c.communicationType.name != ConfigurationDAO.COMM_TYPE_DATABASE
                                && c.communicationType.name != ConfigurationDAO.COMM_TYPE_WEBSERVICE
                                && c.directionType.name != ConfigurationDAO.DIRECTION_TYPE_OUTGOING);

            return tempConfig;
         
        }

        public static List<Configuration> GetOutgoingInterfaces(List<Configuration> configs)
        {
            List<Configuration> tempConfig = new List<Configuration>();

            tempConfig = configs.FindAll(c => c.communicationType.name != ConfigurationDAO.COMM_TYPE_DATABASE
                                && c.communicationType.name != ConfigurationDAO.COMM_TYPE_WEBSERVICE
                                && c.directionType.name != ConfigurationDAO.DIRECTION_TYPE_INCOMING);

            return tempConfig;
        }

        public static Configuration GetOutgoingDatabasesByApplicationId(List<Configuration> configs, int applicationId)
        {
            Configuration tempConfig = new Configuration();

            tempConfig = configs.Find(c => c.communicationType.name == ConfigurationDAO.COMM_TYPE_DATABASE
                                && c.communicationType.name != ConfigurationDAO.COMM_TYPE_WEBSERVICE
                                && c.directionType.name == ConfigurationDAO.DIRECTION_TYPE_OUTGOING
                                && c.application.id == applicationId);

            return tempConfig;
        }

        public static List<Configuration> GetIncomingDatabases(List<Configuration> configs)
        {
            List<Configuration> tempConfig = new List<Configuration>();

            tempConfig = configs.FindAll(c => c.communicationType.name != ConfigurationDAO.COMM_TYPE_INTERFACE
                                && c.communicationType.name != ConfigurationDAO.COMM_TYPE_WEBSERVICE
                                && c.directionType.name != ConfigurationDAO.DIRECTION_TYPE_OUTGOING);

            return tempConfig;
        }

        public static List<Configuration> GetOutgoingDatabases(List<Configuration> configs)
        {
            List<Configuration> tempConfig = new List<Configuration>();

            tempConfig = configs.FindAll(c => c.communicationType.name != ConfigurationDAO.COMM_TYPE_INTERFACE
                                && c.communicationType.name != ConfigurationDAO.COMM_TYPE_WEBSERVICE
                                && c.directionType.name != ConfigurationDAO.DIRECTION_TYPE_INCOMING);

            return tempConfig;
        }

        public static List<Configuration> GetOutgoingWebservice(List<Configuration> configs)
        {
            List<Configuration> tempConfig = new List<Configuration>();

            tempConfig = configs.FindAll(c => c.communicationType.name != ConfigurationDAO.COMM_TYPE_DATABASE
                                && c.communicationType.name != ConfigurationDAO.COMM_TYPE_INTERFACE
                                && c.directionType.name != ConfigurationDAO.DIRECTION_TYPE_INCOMING);

            return tempConfig;
        }

        public static List<Configuration> GetIncomingWebservice(List<Configuration> configs)
        {
            List<Configuration> tempConfig = new List<Configuration>();
            tempConfig = configs.FindAll(c => c.communicationType.name != ConfigurationDAO.COMM_TYPE_DATABASE
                                && c.communicationType.name != ConfigurationDAO.COMM_TYPE_INTERFACE
                                && c.directionType.name != ConfigurationDAO.DIRECTION_TYPE_OUTGOING);

            return tempConfig;
        }

        public static Communication GetIncomingWebserviceCommunication(Application application, List<Communication> communications)
        {
            Communication communication = new Communication();

            communication = communications.Find(c => c.applicationId == application.id
                                          && c.directionTypeId == ConfigurationDAO.DIRECTION_TYPE_INCOMING_NO
                                          && c.communicationTypeId == ConfigurationDAO.COMM_TYPE_WEBSERVICE_NO);

            return communication;
        }

        public static WebserviceInstance GetIncomingWebserviceInstance(Communication communication, List<WebserviceInstance> webserviceInstances)
        {
            WebserviceInstance tempInstance = new WebserviceInstance();

            tempInstance = webserviceInstances.Find(c => c.communicationId == communication.id);

            return tempInstance;
        }

        public static List<WebserviceObject> GetIncomingWebserviceObjects(WebserviceInstance webserviceIsntance, List<WebserviceObject> webserviceObjects)
        {
            List<WebserviceObject> tempWebserviceObjects = new List<WebserviceObject>();

            tempWebserviceObjects = webserviceObjects.FindAll(o => o.webserviceInstanceId == webserviceIsntance.id);

            return tempWebserviceObjects;
        }

        public static List<WebservicePropertySet> GetIncomingWebservicePropertySets(WebserviceObject webserviceObject, List<WebservicePropertySet> webservicePropertySets)
        {
            List<WebservicePropertySet> tempWebservicePropertySets = new List<WebservicePropertySet>();

            tempWebservicePropertySets = webservicePropertySets;

            tempWebservicePropertySets = tempWebservicePropertySets.FindAll(p => p.webserviceObjectId == webserviceObject.id);

            return tempWebservicePropertySets;
        }

        public static List<MessageGroup> GetIncomingWebserviceMessageGroup(WebservicePropertySet webservicePropertySet, List<MessageGroup> messageGroups)
        {
            List<MessageGroup> tempGroups = new List<MessageGroup>();

            tempGroups = messageGroups;

            tempGroups = tempGroups.FindAll(m => m.messageGroupInstanceId == webservicePropertySet.messageGroupInstanceId);

            return tempGroups;
        }

        public static bool IsDatabaseTableRelation(DatabaseTable databaseTable, List<DatabaseTableRelation> databaseTableRelations, bool isTarget = false)
        {
            int id = databaseTable.id;
            bool isRelation = false;

            if (!isTarget)
            {
                // source
                isRelation = (databaseTableRelations.FindIndex(s => s.sourceDatabaseTableId == id) >= 0);
            }
            else
            {
                // target
                isRelation = (databaseTableRelations.FindIndex(t=>t.targetDatabaseTableId == id) >= 0);
            }

            return isRelation;
        }

        public static Broker GetBrokerByApplicationAndCommunication(Communication communication, Broker broker)
        {            
            List<Broker> brokers = BrokerDAO.Get();

            broker = brokers.Find(b => b.communicationId == communication.id);

            return broker;
        }
    }
}
