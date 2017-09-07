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
    public class WebserviceObjectDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string WEBSERVICE_INSTANCE_ID = "WEBSERVICE_INSTANCE_ID";
        public const string NAME = "NAME";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_WEBSERVICE_OBJECT_ID = "WEBSERVICE_OBJECT_ID";
        public const string VIEW_WEBSERVICE_OBJECT = "WEBSERVICE_OBJECT";
        public const string VIEW_WEBSERVICE_OBJECT_NAME = "WEBSERVICE_OBJECT_NAME";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_WEBSERVICE_INSTANCE_ID = "@WEBSERVICE_INSTANCE_ID";
        public const string AT_NAME = "@NAME";


        public static List<WebserviceObject> Get()
        {
            return getWebserviceObjects();
        }

        public static List<WebserviceObject> Get(WebserviceObject webserviceObject)
        {
            return getSingleWebserviceObject(webserviceObject);
        }

        public static WebserviceObject PostUpdate(WebserviceObject webserviceObject)
        {
            return postUpdateWebserviceObject(webserviceObject);
        }

        public static bool Delete(WebserviceObject webserviceObject)
        {
            return deleteWebserviceObject(webserviceObject);
        }

        private static List<WebserviceObject> getWebserviceObjects()
        {
            List<WebserviceObject> webserviceObjects = new List<WebserviceObject>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_WEBSERVICE_OBJECT;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            webserviceObjects.Add(new WebserviceObject(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getWebserviceObjects()");
                }
            }

            return webserviceObjects;
        }

        private static List<WebserviceObject> getSingleWebserviceObject(WebserviceObject webserviceObject)
        {
            List<WebserviceObject> webserviceObjects = new List<WebserviceObject>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_WEBSERVICE_OBJECT;
                command.Parameters.AddWithValue(WebserviceObjectDAO.AT_ID, webserviceObject.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            webserviceObjects.Add(new WebserviceObject(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleWebserviceObject(WebserviceObject WebserviceObject)", webserviceObject.id.ToString());
                }
            }

            return webserviceObjects;
        }

        private static WebserviceObject postUpdateWebserviceObject(WebserviceObject webserviceObject)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_WEBSERVICE_OBJECT;
                command.Parameters.AddWithValue(WebserviceObjectDAO.WEBSERVICE_INSTANCE_ID, webserviceObject.webserviceInstanceId);
                command.Parameters.AddWithValue(WebserviceObjectDAO.AT_NAME, webserviceObject.name);
                command.Parameters.AddWithValue(WebserviceObjectDAO.AT_ID, webserviceObject.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            webserviceObject.id = DAOUtility.GetData<int>(reader, WebserviceObjectDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateWebserviceObject()", webserviceObject.id.ToString());
                }
            }
            return webserviceObject;
        }

        private static bool deleteWebserviceObject(WebserviceObject webserviceObject)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_WEBSERVICE_OBJECT;
                command.Parameters.AddWithValue(WebserviceObjectDAO.AT_ID, webserviceObject.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            webserviceObject.id = DAOUtility.GetData<int>(reader, WebserviceObjectDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteWebserviceObject()", webserviceObject.id.ToString());
                }
            }
            return true;
        }

    }
}
