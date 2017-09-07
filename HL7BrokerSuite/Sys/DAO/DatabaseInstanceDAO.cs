using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Utility;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Settings;
using System.Data.SqlClient;

namespace HL7BrokerSuite.Sys.DAO
{
    public class DatabaseInstanceDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string COMMUNICATION_ID = "COMMUNICATION_ID";
        public const string CREDENTIAL_ID = "CREDENTIAL_ID";
        public const string NAME = "NAME";
        public const string SERVER = "SERVER";
        public const string IP_ADDRESS = "IP_ADDRESS";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_DATABASE_NAME = "DATABASE_NAME";
        public const string VIEW_DATABASE_SERVER = "DATABASE_SERVER";
        public const string VIEW_DATABASE_IP_ADDRESS = "DATABASE_IP_ADDRESS";
        public const string VIEW_DATABASE_INSTANCE_ID = "DATABASE_INSTANCE_ID";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_COMMUNICATION_ID = "@COMMUNICATION_ID";
        public const string AT_CREDENTIAL_ID = "@CREDENTIAL_ID";
        public const string AT_NAME = "@NAME";
        public const string AT_SERVER = "@SERVER";
        public const string AT_IP_ADDRESS = "@IP_ADDRESS";


        public static List<DatabaseInstance> Get()
        {
            return getDatabaseInstances();
        }

        public static List<DatabaseInstance> Get(DatabaseInstance databaseInstance)
        {
            return getSingleDatabaseInstance(databaseInstance);
        }

        public static DatabaseInstance PostUpdate(DatabaseInstance databaseInstance)
        {
            return postUpdateDatabaseInstance(databaseInstance);
        }

        public static bool Delete(DatabaseInstance databaseInstance)
        {
            return deleteDatabaseInstance(databaseInstance);
        }

        private static List<DatabaseInstance> getDatabaseInstances()
        {
            List<DatabaseInstance> databaseInstances = new List<DatabaseInstance>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_DATABASE_INSTANCE;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseInstances.Add(new DatabaseInstance(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getDatabaseInstances()");
                }
            }

            return databaseInstances;
        }

        private static List<DatabaseInstance> getSingleDatabaseInstance(DatabaseInstance databaseInstance)
        {
            List<DatabaseInstance> databaseInstances = new List<DatabaseInstance>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_DATABASE_INSTANCE;
                command.Parameters.AddWithValue(DatabaseInstanceDAO.AT_ID, databaseInstance.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseInstances.Add(new DatabaseInstance(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleDatabaseInstance(DatabaseInstance DatabaseInstance)", databaseInstance.id.ToString());
                }
            }

            return databaseInstances;
        }

        private static DatabaseInstance postUpdateDatabaseInstance(DatabaseInstance databaseInstance)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_DATABASE_INSTANCE;
                command.Parameters.AddWithValue(DatabaseInstanceDAO.COMMUNICATION_ID, databaseInstance.communicationId);
                command.Parameters.AddWithValue(DatabaseInstanceDAO.CREDENTIAL_ID, databaseInstance.credentialId);
                command.Parameters.AddWithValue(DatabaseInstanceDAO.NAME, databaseInstance.name);
                command.Parameters.AddWithValue(DatabaseInstanceDAO.SERVER, databaseInstance.server);
                command.Parameters.AddWithValue(DatabaseInstanceDAO.IP_ADDRESS, databaseInstance.ipAddress);
                command.Parameters.AddWithValue(DatabaseInstanceDAO.AT_ID, databaseInstance.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseInstance.id = DAOUtility.GetData<int>(reader, DatabaseInstanceDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateDatabaseInstance()", databaseInstance.id.ToString());
                }
            }
            return databaseInstance;
        }

        private static bool deleteDatabaseInstance(DatabaseInstance databaseInstance)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_DATABASE_INSTANCE;
                command.Parameters.AddWithValue(DatabaseInstanceDAO.AT_ID, databaseInstance.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseInstance.id = DAOUtility.GetData<int>(reader, DatabaseInstanceDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteDatabaseInstance()", databaseInstance.id.ToString());
                }
            }
            return true;
        }
    }
}
