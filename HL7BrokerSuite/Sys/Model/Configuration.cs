using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Sys;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Sys.Model;
using System.Data;

namespace HL7BrokerSuite.Sys.Model
{
    public class Configuration
    {
        public List<Application> applications { get; set; }
        public List<ApplicationSetting> applicationSettings { get; set; }
        public List<Communication> communications { get; set; }
        public List<CommunicationType> communicationTypes { get; set; }
        public List<Credential> credentials { get; set; }
        public List<CredentialType> credentialTypes { get; set; }
        public List<DatabaseTable> databaseTables { get; set; }
        public List<DatabaseInstance> databaseInstances { get; set; }
        public List<DatabaseTableRelation> databaseTableRelations { get; set; }
        public List<DirectionType> directionTypes { get; set; }
        public List<Interface> interfaceThreads { get; set; }
        public List<MessageGroupInstance> messageGroupInstances { get; set; }
        public List<MessagePart> messageParts { get; set; }
        public List<MessageType> messageTypes { get; set; }
        public List<SegmentType> segmentTypes { get; set; }        
        public List<MessageGroup> messageGroups { get; set; }
        public List<ColumnSet> columnSets { get; set; }
        public List<WebserviceInstance> webserviceInstances { get; set; }
        public List<WebserviceObject> webserviceObjects { get; set; }
        public List<WebservicePropertySet> webservicePropertySets { get; set; }

        public Application application { get; set; }
        public ApplicationSetting applicationSetting { get; set; }
        public Communication communication { get; set; }
        public CommunicationType communicationType { get; set; }
        public Credential credential { get; set; }
        public CredentialType credentialType { get; set; }
        public DatabaseInstance databaseInstance { get; set; }
        public DirectionType directionType { get; set; }
        public Interface interfaceThread { get; set; }
        public MessageGroupInstance messageGroupInstance { get; set; }
        public MessagePart messagePart { get; set; }
        public MessageType messageType { get; set; }
        public SegmentType segmentType { get; set; }
        public DatabaseTable databaseTable { get; set; }
        public DatabaseTableRelation databaseTableRelation { get; set; }
        public MessageGroup messageGroup { get; set; }
        public ColumnSet columnSet { get; set; }
        public WebserviceInstance webserviceInstance { get; set; }
        public WebserviceObject webserviceObject { get; set; }
        public WebservicePropertySet webserviceProperty { get; set; }

       // these are the name of the views that are responsible for retrieving data. 
       public enum View
       {
           GetApplicationConfiguration,
           GetApplicationInterfaceConfig,
           GetApplicationDatabasesConfig,
           GetDatabaseTables,
           GetDatabaseColumns,
           GetMessagePosition,
           GetApplicationWebserviceConfig,
           GetWebseviceObjects,
           GetWebserviceObjectProperties
       };

        public Configuration()
        {
            initializeLists();
        }

        public Configuration(IDataRecord reader, View view)
        {
            initializeWithView(reader, view);
        }

        private void initializeWithView(IDataRecord reader, View view)
        {
            switch (view)
            {
                case View.GetApplicationConfiguration:
                    this.application = new Application(reader, true);
                    this.communication = new Communication(reader, true);
                    this.communicationType = new CommunicationType(reader, true);
                    this.directionType = new DirectionType(reader, true);
                    this.applicationSetting = new ApplicationSetting(reader,true);
                    break;
                case View.GetApplicationInterfaceConfig:
                    this.application = new Application(reader, true);
                    this.communication = new Communication(reader, true);
                    this.communicationType = new CommunicationType(reader, true);
                    this.interfaceThread = new Interface(reader, true);
                    this.directionType = new DirectionType(reader, true);
                    this.applicationSetting = new ApplicationSetting(reader, true);
                    break;
                case View.GetApplicationDatabasesConfig:
                    this.application = new Application(reader, true);                 
                    this.communication = new Communication(reader, true);
                    this.communicationType = new CommunicationType(reader, true);
                    this.credential = new Credential(reader, true);
                    this.databaseInstance = new DatabaseInstance(reader, true);
                    this.interfaceThread = new Interface(reader, true);
                    this.directionType = new DirectionType(reader, true);
                    this.applicationSetting = new ApplicationSetting(reader, true);
                    break;
                case View.GetDatabaseTables:
                    this.application = new Application(reader, true);
                    this.communication = new Communication(reader, true);
                    this.communicationType = new CommunicationType(reader, true);
                    this.databaseInstance = new DatabaseInstance(reader, true);
                    this.databaseTable = new DatabaseTable(reader, true);
                    this.databaseTableRelation = new DatabaseTableRelation(reader, true);
                    break;
                case View.GetDatabaseColumns:
                    this.application = new Application(reader, true);
                    this.columnSet = new ColumnSet(reader, true);
                    this.communication = new Communication(reader, true);
                    this.communicationType = new CommunicationType(reader, true);
                    this.directionType = new DirectionType(reader, true);
                    this.databaseTable = new DatabaseTable(reader, true);
                    break;
                case View.GetMessagePosition:
                    this.messageGroupInstance = new MessageGroupInstance(reader, true);
                    this.messageGroup = new MessageGroup(reader, true);
                    this.messageType = new MessageType(reader, true);
                    this.segmentType = new SegmentType(reader, true);
                    this.messagePart = new MessagePart(reader, true);
                    break;
                case View.GetApplicationWebserviceConfig: 
                    this.application = new Application(reader, true);                 
                    this.communication = new Communication(reader, true);
                    this.communicationType = new CommunicationType(reader, true);
                    this.credential = new Credential(reader, true);
                    this.webserviceInstance = new WebserviceInstance(reader, true);
                    this.directionType = new DirectionType(reader, true);
                    this.applicationSetting = new ApplicationSetting(reader, true);
                    break;
                case View.GetWebseviceObjects:
                    this.application = new Application(reader, true);
                    this.communication = new Communication(reader, true);
                    this.communicationType = new CommunicationType(reader, true);
                    this.webserviceInstance = new WebserviceInstance(reader, true);
                    this.webserviceObject = new WebserviceObject(reader, true);                    
                    break;
                case View.GetWebserviceObjectProperties:
                    this.application = new Application(reader, true);
                    this.webserviceProperty = new WebservicePropertySet(reader, true);
                    this.communication = new Communication(reader, true);
                    this.communicationType = new CommunicationType(reader, true);
                    this.directionType = new DirectionType(reader, true);
                    this.webserviceObject = new WebserviceObject(reader, true);
                    break;
                default:
                    break;

            }        
        }

        private void initializeLists()
        {
            this.applications = new List<Application>();
            this.applicationSettings = new List<ApplicationSetting>();
            this.communications = new List<Communication>();
            this.communicationTypes = new List<CommunicationType>();
            this.credentials = new List<Credential>();
            this.credentialTypes = new List<CredentialType>();
            this.databaseInstances = new List<DatabaseInstance>();
            this.databaseTables = new List<DatabaseTable>();
            this.databaseTableRelations = new List<DatabaseTableRelation>();
            this.directionTypes = new List<DirectionType>();
            this.interfaceThreads = new List<Interface>();
            this.messageGroupInstances = new List<MessageGroupInstance>();
            this.messageParts = new List<MessagePart>();
            this.messageTypes = new List<MessageType>();
            this.segmentTypes = new List<SegmentType>();
            this.databaseTables = new List<DatabaseTable>();
            this.messageGroups = new List<MessageGroup>();
            this.columnSets = new List<ColumnSet>();
            this.webserviceInstances = new List<WebserviceInstance>();
            this.webserviceObjects = new List<WebserviceObject>();
            this.webservicePropertySets = new List<WebservicePropertySet>();
        }
    }
}
