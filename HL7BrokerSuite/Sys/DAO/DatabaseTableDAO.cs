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
    public class DatabaseTableDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string DATABASE_INSTANCE_ID = "DATABASE_INSTANCE_ID";
        public const string NAME = "NAME";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_DATABASE_TABLE_ID = "DATABASE_TABLE_ID";
        public const string VIEW_DATABASE_TABLE = "DATABASE_TABLE";
        public const string VIEW_DATABASE_TABLE_NAME = "DATABASE_TABLE_NAME";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_DATABASE_INSTANCE_ID = "@DATABASE_INSTANCE_ID";
        public const string AT_NAME = "@NAME";


        public static List<DatabaseTable> Get()
        {
            return getDatabaseTables();
        }

        public static List<DatabaseTable> Get(DatabaseTable databaseTable)
        {
            return getSingleDatabaseTable(databaseTable);
        }

        public static DatabaseTable PostUpdate(DatabaseTable databaseTable)
        {
            return postUpdateDatabaseTable(databaseTable);
        }

        public static bool Delete(DatabaseTable databaseTable)
        {
            return deleteDatabaseTable(databaseTable);
        }

        private static List<DatabaseTable> getDatabaseTables()
        {
            List<DatabaseTable> databaseTables = new List<DatabaseTable>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_DATABASE_TABLE;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseTables.Add(new DatabaseTable(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getDatabaseTables()");
                }
            }

            return databaseTables;
        }

        private static List<DatabaseTable> getSingleDatabaseTable(DatabaseTable databaseTable)
        {
            List<DatabaseTable> databaseTables = new List<DatabaseTable>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_DATABASE_TABLE;
                command.Parameters.AddWithValue(DatabaseTableDAO.AT_ID, databaseTable.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseTables.Add(new DatabaseTable(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleDatabaseTable(DatabaseTable DatabaseTable)", databaseTable.id.ToString());
                }
            }

            return databaseTables;
        }

        private static DatabaseTable postUpdateDatabaseTable(DatabaseTable databaseTable)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_DATABASE_TABLE;                
                command.Parameters.AddWithValue(DatabaseTableDAO.AT_DATABASE_INSTANCE_ID, databaseTable.databaseInstanceId);
                command.Parameters.AddWithValue(DatabaseTableDAO.AT_NAME, databaseTable.name);
                command.Parameters.AddWithValue(DatabaseTableDAO.AT_ID, databaseTable.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseTable.id = DAOUtility.GetData<int>(reader, DatabaseTableDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateDatabaseTable()", databaseTable.id.ToString());
                }
            }
            return databaseTable;
        }

        private static bool deleteDatabaseTable(DatabaseTable databaseTable)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_DATABASE_TABLE;
                command.Parameters.AddWithValue(DatabaseTableDAO.AT_ID, databaseTable.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseTable.id = DAOUtility.GetData<int>(reader, DatabaseTableDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteDatabaseTable()", databaseTable.id.ToString());
                }
            }
            return true;
        }
    }
}
