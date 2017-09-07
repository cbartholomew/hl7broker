using HL7BrokerSuite.Settings;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.Sys.DAO
{
    public class WebservicePropertySetDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string WEBSERVICE_OBJECT_ID = "WEBSERVICE_OBJECT_ID";
        public const string NAME = "NAME";
        public const string MESSAGE_GROUP_INSTANCE_ID = "MESSAGE_GROUP_INSTANCE_ID";
        public const string COLUMN_DATA_TYPE = "COLUMN_DATA_TYPE";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_WEBSERVICE_PROPERTY_ID = "WEBSERVICE_PROPERTY_ID";
        public const string VIEW_WEBSERVICE_PROPERTY = "WEBSERVICE_PROPERTY";
        public const string VIEW_WEBSERVICE_PROPERTY_MESSAGE_GROUP_INSTANCE_ID = "WEBSERVICE_PROPERTY_MESSAGE_GROUP_INSTANCE_ID";
        public const string VIEW_WEBSERVICE_PROPERTY_COLUMN_DATA_TYPE = "WEBSERVICE_PROPERTY_COLUMN_DATA_TYPE";

        // PARAMETERS THAT ARE IN THE STORE PROCEDURE
        public const string AT_WEBSERVICE_OBJECT_ID = "@WEBSERVICE_OBJECT_ID";
        public const string AT_NAME = "@NAME";
        public const string AT_MESSAGE_GROUP_INSTANCE_ID = "@MESSAGE_GROUP_INSTANCE_ID";
        public const string AT_COLUMN_DATA_TYPE = "@COLUMN_DATA_TYPE";
        public const string AT_ID = "@ID";

        public static List<WebservicePropertySet> Get()
        {
            return getWebservicePropertySets();
        }

        public static List<WebservicePropertySet> Get(WebservicePropertySet webservicePropertySet)
        {
            return getSingleWebservicePropertySet(webservicePropertySet);
        }

        public static WebservicePropertySet PostUpdate(WebservicePropertySet webservicePropertySet)
        {
            return postUpdateWebservicePropertySet(webservicePropertySet);
        }

        public static bool Delete(WebservicePropertySet webservicePropertySet)
        {
            return deleteWebservicePropertySet(webservicePropertySet);
        }

        private static List<WebservicePropertySet> getWebservicePropertySets()
        {
            List<WebservicePropertySet> webservicePropertySets = new List<WebservicePropertySet>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_WEBSERVICE_PROPERTY_SET;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            webservicePropertySets.Add(new WebservicePropertySet(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getWebservicePropertySets()");
                }
            }

            return webservicePropertySets;
        }

        private static List<WebservicePropertySet> getSingleWebservicePropertySet(WebservicePropertySet WebservicePropertySet)
        {
            List<WebservicePropertySet> webservicePropertySets = new List<WebservicePropertySet>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_WEBSERVICE_PROPERTY_SET;
                command.Parameters.AddWithValue(WebservicePropertySetDAO.AT_ID, WebservicePropertySet.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            webservicePropertySets.Add(new WebservicePropertySet(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleWebservicePropertySet(WebservicePropertySet WebservicePropertySet)", WebservicePropertySet.id.ToString());
                }
            }

            return webservicePropertySets;
        }

        private static WebservicePropertySet postUpdateWebservicePropertySet(WebservicePropertySet webservicePropertySet)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_WEBSERVICE_PROPERTY;
                command.Parameters.AddWithValue(WebservicePropertySetDAO.AT_WEBSERVICE_OBJECT_ID, webservicePropertySet.webserviceObjectId);
                command.Parameters.AddWithValue(WebservicePropertySetDAO.AT_NAME, webservicePropertySet.name);
                command.Parameters.AddWithValue(WebservicePropertySetDAO.AT_MESSAGE_GROUP_INSTANCE_ID, webservicePropertySet.messageGroupInstanceId);                
                command.Parameters.AddWithValue(WebservicePropertySetDAO.AT_COLUMN_DATA_TYPE, webservicePropertySet.columnDataType);
                command.Parameters.AddWithValue(WebservicePropertySetDAO.AT_ID, webservicePropertySet.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            webservicePropertySet.id = DAOUtility.GetData<int>(reader, WebservicePropertySetDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateWebservicePropertySet()", webservicePropertySet.name);
                }
            }
            return webservicePropertySet;
        }

        private static bool deleteWebservicePropertySet(WebservicePropertySet WebservicePropertySet)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_WEBSERVICE_PROPERTY_SET;
                command.Parameters.AddWithValue(WebservicePropertySetDAO.AT_ID, WebservicePropertySet.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            WebservicePropertySet.id = DAOUtility.GetData<int>(reader, WebservicePropertySetDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteWebservicePropertySet()", WebservicePropertySet.name);
                }
            }
            return true;
        }
    }
}
