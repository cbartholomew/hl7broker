using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Settings;
using HL7BrokerSuite.Utility;
using System.Data.SqlClient;

namespace HL7BrokerSuite.Sys.DAO
{
    public class ConfigurationDAO
    {
        public const string AT_ID = "@ID";
        public const string AT_CONFIG_ID = "@CONFIG_ID";
        public const string AT_COMMUNICATION_ID = "@COMMUNICATION_ID";
        public const string AT_DATABASE_INSTANCE_ID = "@DATABASE_INSTANCE_ID";
        public const string AT_DATABASE_TABLE_ID = "@DATABASE_TABLE_ID";
        public const string AT_MESSAGE_GROUP_INSTANCE_ID = "@MESSAGE_GROUP_INSTANCE_ID";
        public const string AT_WEBSERVICE_INSTANCE_ID = "@WEBSERVICE_INSTANCE_ID";
        public const string AT_WEBSERVICE_OBJECT_ID = "@WEBSERVICE_OBJECT_ID";        
        public const string COMM_TYPE_INTERFACE = "INTERFACE";
        public const string COMM_TYPE_DATABASE = "DATABASE";
        public const string COMM_TYPE_WEBSERVICE = "WEBSERVICE";

        public const string DIRECTION_TYPE_INCOMING = "INCOMING";
        public const string DIRECTION_TYPE_OUTGOING = "OUTGOING";

        public const int DIRECTION_TYPE_INCOMING_NO = 1;
        public const int DIRECTION_TYPE_OUTGOING_NO = 2;      

        public const int COMM_TYPE_INTERFACE_NO     = 1;
        public const int COMM_TYPE_DATABASE_NO      = 2;
        public const int COMM_TYPE_WEBSERVICE_NO    = 9;

        private const string KEY_APPLICATION  = "APPLICATION";
        private const string KEY_DATABASE     = "DATABASE";
        private const string KEY_INTERFACE    = "INTERFACE";
        private const string KEY_TABLES       = "TABLES";
        private const string KEY_COLUMNS      = "COLUMNS";
        private const string KEY_MESSAGE      = "MESSAGE";
      
        public static List<Configuration> GetApplications()
        {
            return getApplications();
        }

        public static List<Configuration> GetApplications(Application application)
        {
            return GetConfigurationByApplicationId(application);
        }

        public static List<Configuration> getApplications()
        {
            List<Configuration> configurations = new List<Configuration>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_ALL_APPLICATION_CONFIG;
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            configurations.Add(new Configuration(reader,
                                Configuration.View.GetApplicationConfiguration));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleApplication(Application application)", "-9");
                }
            }
            return configurations;
        }

        public static Configuration GetAllConfigurations()
        {
            // make a new configuration
            Configuration configuration
                = new Configuration();

            // get all configurations
            configuration.applications = ApplicationDAO.Get();
            configuration.applicationSettings = ApplicationSettingDAO.Get();
            configuration.columnSets = ColumnSetDAO.Get();
            configuration.communications = CommunicationDAO.Get();
            configuration.communicationTypes = CommunicationTypeDAO.Get();
            configuration.credentials = CredentialDAO.Get();
            configuration.credentialTypes = CredentialTypeDAO.Get();
            configuration.databaseTableRelations = DatabaseTableRelationDAO.Get();
            configuration.databaseInstances = DatabaseInstanceDAO.Get();
            configuration.databaseTables = DatabaseTableDAO.Get();
            configuration.directionTypes = DirectionTypeDAO.Get();
            configuration.interfaceThreads = InterfaceDAO.Get();
            configuration.messageGroups = MessageGroupDAO.Get();
            configuration.messageGroupInstances = MessageGroupInstanceDAO.Get();
            configuration.messageParts = MessagePartDAO.Get();
            configuration.messageTypes = MessageTypeDAO.Get();
            configuration.segmentTypes = SegmentTypeDAO.Get();
            configuration.webserviceInstances = WebserviceInstanceDAO.Get();
            configuration.webserviceObjects = WebserviceObjectDAO.Get();
            configuration.webservicePropertySets = WebservicePropertySetDAO.Get();

            return configuration;
        }

        public static List<Configuration> GetConfigurationByApplicationId(Application application)
        {
            List<Configuration> configurations = new List<Configuration>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_APPLICATION_CONFIG;
                command.Parameters.AddWithValue(ConfigurationDAO.AT_CONFIG_ID, application.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            configurations.Add(new Configuration(reader, 
                                Configuration.View.GetApplicationConfiguration));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getConfigurationByApplicationId(Application application)", application.id.ToString());
                }
            }
            return configurations;
        }

        public static List<Configuration> GetDatabaseConfiguration(Communication communucation)
        {
            List<Configuration> configurations = new List<Configuration>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_APPLICATION_DATABASE_CONFIG;
                command.Parameters.AddWithValue(ConfigurationDAO.AT_COMMUNICATION_ID, communucation.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            configurations.Add(new Configuration(reader, 
                                Configuration.View.GetApplicationDatabasesConfig));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getDatabaseConfiguration(Communication communucation)", communucation.id.ToString());
                }
            }
            return configurations;
        }

        public static List<Configuration> GetApplicationWebserviceConfig(Communication communication)
        {
            List<Configuration> configurations = new List<Configuration>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_WEBSERVICE_CONFIG;
                command.Parameters.AddWithValue(ConfigurationDAO.AT_COMMUNICATION_ID, communication.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            configurations.Add(new Configuration(reader,
                                Configuration.View.GetApplicationWebserviceConfig));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetApplicationWebserviceConfig(Communication communucation)", communication.id.ToString());
                }
            }
            return configurations;
        }

        public static List<Configuration> GetInterfaceConfigration(Communication communucation)
        {
            List<Configuration> configurations = new List<Configuration>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_APPLICATION_INTERFACE_CONFIG;
                command.Parameters.AddWithValue(ConfigurationDAO.AT_COMMUNICATION_ID, communucation.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            configurations.Add(new Configuration(reader,
                                Configuration.View.GetApplicationInterfaceConfig));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getInterfaceConfigration(Communication communucation)", communucation.id.ToString());
                }
            }
            return configurations;
        }

        public static List<Configuration> GetDatabaseTables(DatabaseInstance databaseInstance)
        {
            List<Configuration> configurations = new List<Configuration>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_DATABASE_TABLES;
                command.Parameters.AddWithValue(ConfigurationDAO.AT_DATABASE_INSTANCE_ID, 
                                                databaseInstance.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            configurations.Add(new Configuration(reader,
                                Configuration.View.GetDatabaseTables));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getDatabaseTables(DatabaseInstance databaseInstance)", databaseInstance.id.ToString());
                }
            }
            return configurations;    
        }

        public static List<Configuration> GetDatabaseColumns(DatabaseTable databaseTable)
        {
            List<Configuration> configurations = new List<Configuration>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_DATABASE_COLUMNS;
                command.Parameters.AddWithValue(ConfigurationDAO.AT_DATABASE_TABLE_ID,
                                                databaseTable.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            configurations.Add(new Configuration(reader,
                                Configuration.View.GetDatabaseColumns));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getDatabaseColumns(DatabaseTable databaseTable)", databaseTable.id.ToString());
                }
            }
            return configurations;         
        }

        public static List<Configuration> GetWebserviceObjects(WebserviceInstance webserviceInstance)
        {
            List<Configuration> configurations = new List<Configuration>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_WEBSERVICE_OBJECTS;
                command.Parameters.AddWithValue(ConfigurationDAO.AT_WEBSERVICE_INSTANCE_ID,
                                                webserviceInstance.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            configurations.Add(new Configuration(reader,
                                Configuration.View.GetWebseviceObjects));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetWebserviceObjects(WebserviceInstance webserviceInstance)", webserviceInstance.id.ToString());
                }
            }
            return configurations;

        }

        public static List<Configuration> GetWebserviceObjectProperties(WebserviceObject webserviceObject)
        {
            List<Configuration> configurations = new List<Configuration>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_WEBSERVICE_PROPERTIES;
                command.Parameters.AddWithValue(ConfigurationDAO.AT_WEBSERVICE_OBJECT_ID,
                                                webserviceObject.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            configurations.Add(new Configuration(reader,
                                Configuration.View.GetWebserviceObjectProperties));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetWebserviceObjectProperties(WebserviceObject webserviceObject)", webserviceObject.id.ToString());
                }
            }
            return configurations;         
        
        }

        public static List<Configuration> GetMessagePosition(MessageGroupInstance messageGroupInstance)
        {
            List<Configuration> configurations = new List<Configuration>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_MESSAGE_POSITION;
                command.Parameters.AddWithValue(ConfigurationDAO.AT_MESSAGE_GROUP_INSTANCE_ID,
                                                messageGroupInstance.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            configurations.Add(new Configuration(reader,
                                Configuration.View.GetMessagePosition));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getMessagePosition(MessageGroupInstance messageGroupInstance)", messageGroupInstance.id.ToString());
                }
            }
            return configurations;         
        }

        public static bool CopyApplicationConfiguration(List<Configuration> applicationFrom,
                                                        List<Configuration> applicationTo)
        {


            return true;
        }
    }
}
