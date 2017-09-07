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
    public class ApplicationSettingDAO
    {

        // REGULAR COLUMN NAMES
        public const string ID                  = "ID";
        public const string APPLICATION_ID      = "APPLICATION_ID";
        public const string COMMUNICATION_ID    = "COMMUNICATION_ID";
        public const string AUTOSTART           = "AUTOSTART";
        public const string DISABLED            = "DISABLED";
            
        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_APP_SETTING_ID                 = "APP_SETTING_ID";
        public const string VIEW_APP_SETTING_APPLICATION_ID     = "APP_SETTING_APPLICATION_ID";
        public const string VIEW_APP_SETTING_COMMUNICATION_ID   = "APP_SETTING_COMMUNICATION_ID";
        public const string VIEW_APP_SETTING_AUTOSTART          = "APP_SETTING_AUTOSTART";
        public const string VIEW_APP_SETTING_DISABLED           = "APP_SETTING_DISABLED";
                
        // PARAMETERS THAT ARE IN THE STORE PROCEDURE
        public const string AT_APPLICATION_ID = "@APPLICATION_ID";
        public const string AT_COMMUNICATION_ID = "@COMMUNICATION_ID";
        public const string AT_AUTOSTART = "@AUTOSTART";
        public const string AT_DISABLED = "@DISABLED";
        public const string AT_ID = "@ID";

        public static List<ApplicationSetting> Get()
        {
            return getApplicationSettings();
        }

        public static List<ApplicationSetting> Get(ApplicationSetting applicationSetting)
        {
            return getSingleApplicationSetting(applicationSetting);
        }

        public static ApplicationSetting PostUpdate(ApplicationSetting applicationSetting)
        {
            return postUpdateApplicationSetting(applicationSetting);
        }

        public static bool Delete(ApplicationSetting applicationSetting)
        {
            return deleteApplicationSetting(applicationSetting);
        }

        private static List<ApplicationSetting> getApplicationSettings()
        {
            List<ApplicationSetting> applicationSettings = new List<ApplicationSetting>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_APPLICATION_SETTING;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            applicationSettings.Add(new ApplicationSetting(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetApplicationSettings()");
                }
            }

            return applicationSettings;
        }

        private static List<ApplicationSetting> getSingleApplicationSetting(ApplicationSetting applicationSetting)
        {
            List<ApplicationSetting> applicationSettings = new List<ApplicationSetting>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_APPLICATION_SETTING;
                command.Parameters.AddWithValue(ApplicationSettingDAO.AT_ID, applicationSetting.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            applicationSettings.Add(new ApplicationSetting(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleApplicationSetting(ApplicationSetting applicationSetting)", applicationSetting.id.ToString());
                }
            }

            return applicationSettings;
        }

        private static ApplicationSetting postUpdateApplicationSetting(ApplicationSetting applicationSetting)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_APPLICATION_SETTING;
                command.Parameters.AddWithValue(ApplicationSettingDAO.AT_APPLICATION_ID, applicationSetting.applicationId);
                command.Parameters.AddWithValue(ApplicationSettingDAO.AT_COMMUNICATION_ID, applicationSetting.communicationId);
                command.Parameters.AddWithValue(ApplicationSettingDAO.AT_DISABLED, applicationSetting.disabled);
                command.Parameters.AddWithValue(ApplicationSettingDAO.AT_AUTOSTART, applicationSetting.autoStart);
                command.Parameters.AddWithValue(ApplicationSettingDAO.AT_ID, applicationSetting.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            applicationSetting.id = DAOUtility.GetData<int>(reader, ApplicationSettingDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateApplicationSetting()", applicationSetting.id.ToString());
                }
            }
            return applicationSetting;
        }

        private static bool deleteApplicationSetting(ApplicationSetting applicationSetting)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_APPLICATION_SETTING;
                command.Parameters.AddWithValue(ApplicationSettingDAO.AT_ID, applicationSetting.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            applicationSetting.id = DAOUtility.GetData<int>(reader, ApplicationSettingDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteApplicationSetting()", applicationSetting.id.ToString());
                }
            }
            return true;
        }
    }
}
