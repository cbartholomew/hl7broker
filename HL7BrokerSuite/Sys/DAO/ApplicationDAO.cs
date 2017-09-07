using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Utility;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Settings;

namespace HL7BrokerSuite.Sys.DAO
{
    public class ApplicationDAO
    {
        
        // REGULAR COLUMN NAMES
        public const string ID          = "ID";
        public const string NAME        = "NAME";
        public const string DESCRIPTION = "DESCRIPTION";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_CONFIG_ID          = "CONFIG_ID";
        public const string VIEW_CONFIG_NAME        = "CONFIG_NAME";
        public const string VIEW_CONFIG_DESCRIPTION = "CONFIG_DESCRIPTION";

        // PARAMETERS THAT ARE IN THE STORE PROCEDURE
        public const string AT_NAME         = "@NAME";
        public const string AT_DESCRIPTION  = "@DESCRIPTION";
        public const string AT_ID           = "@ID";
        
        public static List<Application> Get()
        {
            return getApplications();
        }

        public static List<Application> Get(Application application)
        {
            return getSingleApplication(application);
        }

        public static Application PostUpdate(Application application)
        {
            return postUpdateApplication(application);
        }

        public static bool Delete(Application application)
        {
            return deleteApplication(application);
        }

        private static List<Application> getApplications()
        {
            List<Application> applications = new List<Application>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {  
                conn.Open();

                SqlCommand command = new SqlCommand();               
                command.Connection  = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_APPLICATION;

                try 
	            {	        
		             SqlDataReader reader = command.ExecuteReader();

                     if (reader.HasRows)
                     {
                         while (reader.Read())
                         {
                            applications.Add(new Application(reader));   
                         }                                          
                     }
	            }
	            catch (Exception e)
	            {
                    ErrorLogger.LogError(e, "GetApplications()");	
	            }
            }

            return applications;
        }

        private static List<Application> getSingleApplication(Application application)
        {
            List<Application> applications = new List<Application>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_APPLICATION;
                command.Parameters.AddWithValue(ApplicationDAO.AT_ID, application.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            applications.Add(new Application(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleApplication(Application application)", application.id.ToString());
                }
            }

            return applications;
        }

        private static Application postUpdateApplication(Application application)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_APPLICATION;
                command.Parameters.AddWithValue(ApplicationDAO.AT_NAME, application.name);
                command.Parameters.AddWithValue(ApplicationDAO.AT_DESCRIPTION, application.description);
                command.Parameters.AddWithValue(ApplicationDAO.AT_ID, application.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            application.id = DAOUtility.GetData<int>(reader, ApplicationDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateApplication()", application.name);
                }
            }
            return application;
        }

        private static bool deleteApplication(Application application)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_APPLICATION;
                command.Parameters.AddWithValue(ApplicationDAO.AT_ID, application.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            application.id = DAOUtility.GetData<int>(reader, ApplicationDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteApplication()", application.name);
                }
            }
            return true;
        }
    }
}
