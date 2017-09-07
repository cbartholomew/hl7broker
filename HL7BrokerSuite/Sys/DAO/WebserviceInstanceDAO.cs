using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Settings;
using HL7BrokerSuite.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.Sys.DAO
{
    public class WebserviceInstanceDAO
    {       
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string COMMUNICATION_ID = "COMMUNICATION_ID";
        public const string CREDENTIAL_ID = "CREDENTIAL_ID";
        public const string NAME = "NAME";
        public const string SERVER = "SERVER";
        public const string IP_ADDRESS = "IP_ADDRESS";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_WEBSERVICE_NAME = "WEBSERVICE_NAME";
        public const string VIEW_WEBSERVICE_SERVER = "WEBSERVICE_SERVER";
        public const string VIEW_WEBSERVICE_IP_ADDRESS = "WEBSERVICE_IP_ADDRESS";
        public const string VIEW_WEBSERVICE_INSTANCE_ID = "WEBSERVICE_INSTANCE_ID";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_COMMUNICATION_ID = "@COMMUNICATION_ID";
        public const string AT_CREDENTIAL_ID = "@CREDENTIAL_ID";
        public const string AT_NAME = "@NAME";
        public const string AT_SERVER = "@SERVER";
        public const string AT_IP_ADDRESS = "@IP_ADDRESS";

        public static List<WebserviceInstance> Get()
        {
            return getWebserviceInstances();
        }

        public static List<WebserviceInstance> Get(WebserviceInstance webserviceInstance)
        {
            return getSingleWebserviceInstance(webserviceInstance);
        }

        public static WebserviceInstance PostUpdate(WebserviceInstance webserviceInstance)
        {
            return postUpdateWebserviceInstance(webserviceInstance);
        }

        public static bool Delete(WebserviceInstance webserviceInstance)
        {
            return deleteWebserviceInstance(webserviceInstance);
        }

        private static List<WebserviceInstance> getWebserviceInstances()
        {
            List<WebserviceInstance> webserviceInstances = new List<WebserviceInstance>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_WEBSERVICE_INSTANCE;
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            webserviceInstances.Add(new WebserviceInstance(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getWebserviceInstances()");
                }
            }

            return webserviceInstances;
        }

        private static List<WebserviceInstance> getSingleWebserviceInstance(WebserviceInstance webserviceInstance)
        {
            List<WebserviceInstance> webserviceInstances = new List<WebserviceInstance>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_WEBSERVICE_INSTANCE;
                command.Parameters.AddWithValue(WebserviceInstanceDAO.AT_ID, webserviceInstance.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            webserviceInstances.Add(new WebserviceInstance(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleWebserviceInstance(WebserviceInstance WebserviceInstance)", webserviceInstance.id.ToString());
                }
            }

            return webserviceInstances;
        }

        private static WebserviceInstance postUpdateWebserviceInstance(WebserviceInstance webserviceInstance)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_WEBSERVICE_INSTANCE;
                command.Parameters.AddWithValue(WebserviceInstanceDAO.COMMUNICATION_ID, webserviceInstance.communicationId);
                command.Parameters.AddWithValue(WebserviceInstanceDAO.CREDENTIAL_ID, webserviceInstance.credentialId);
                command.Parameters.AddWithValue(WebserviceInstanceDAO.NAME, webserviceInstance.name);
                command.Parameters.AddWithValue(WebserviceInstanceDAO.SERVER, webserviceInstance.server);
                command.Parameters.AddWithValue(WebserviceInstanceDAO.IP_ADDRESS, webserviceInstance.ipAddress);
                command.Parameters.AddWithValue(WebserviceInstanceDAO.AT_ID, webserviceInstance.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            webserviceInstance.id = DAOUtility.GetData<int>(reader, WebserviceInstanceDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateWebserviceInstance()", webserviceInstance.id.ToString());
                }
            }
            return webserviceInstance;
        }

        private static bool deleteWebserviceInstance(WebserviceInstance webserviceInstance)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_WEBSERVICE_INSTANCE;
                command.Parameters.AddWithValue(WebserviceInstanceDAO.AT_ID, webserviceInstance.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            webserviceInstance.id = DAOUtility.GetData<int>(reader, WebserviceInstanceDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteWebserviceInstance()", webserviceInstance.id.ToString());
                }
            }
            return true;
        }
    }
}
