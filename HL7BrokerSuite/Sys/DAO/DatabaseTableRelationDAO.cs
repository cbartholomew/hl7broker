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
    public class DatabaseTableRelationDAO
    {
        public const string ID = "ID";
        public const string SOURCE_DATABASE_TABLE_ID = "SOURCE_DATABASE_TABLE_ID";
        public const string TARGET_DATABASE_TABLE_ID = "TARGET_DATABASE_TABLE_ID";
        public const string REQUIRES_IDENTITY = "REQUIRES_IDENTITY";


        public const string VIEW_DATABASE_TABLE_RELATION_HAS_DEPENDENCIES = "DATABASE_TABLE_RELATION_HAS_DEPENDENCIES";
        public const string VIEW_DATABASE_TABLE_RELATION_ID = "DATABASE_TABLE_RELATION_ID";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_SOURCE_DATABASE_TABLE_ID = "@SOURCE_DATABASE_TABLE_ID";
        public const string AT_TARGET_DATABASE_TABLE_ID = "@TARGET_DATABASE_TABLE_ID";
        public const string AT_REQUIRES_IDENTITY = "@REQUIRES_IDENTITY";


        public static List<DatabaseTableRelation> Get()
        {
            return getDatabaseTableRelations();
        }

        public static List<DatabaseTableRelation> Get(DatabaseTableRelation databaseTableRelation)
        {
            return getSingleDatabaseTable(databaseTableRelation);
        }

        public static DatabaseTableRelation PostUpdate(DatabaseTableRelation databaseTableRelation)
        {
            return postUpdateDatabaseTableRelation(databaseTableRelation);
        }

        public static bool Delete(DatabaseTableRelation databaseTableRelation)
        {
            return deleteDatabaseTableRelation(databaseTableRelation);
        }

        private static List<DatabaseTableRelation> getDatabaseTableRelations()
        {
            List<DatabaseTableRelation> databaseTableRelations = new List<DatabaseTableRelation>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_DATABASE_TABLE_RELATION;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseTableRelations.Add(new DatabaseTableRelation(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getDatabaseTableRelations()");
                }
            }

            return databaseTableRelations;
        }

        private static List<DatabaseTableRelation> getSingleDatabaseTable(DatabaseTableRelation databaseTableRelation)
        {
            List<DatabaseTableRelation> databaseTableRelations = new List<DatabaseTableRelation>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_DATABASE_TABLE_RELATION;
                command.Parameters.AddWithValue(DatabaseTableRelationDAO.AT_ID, databaseTableRelation.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseTableRelations.Add(new DatabaseTableRelation(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleDatabaseTableRelation(DatabaseTableRelation DatabaseTableRelation)", databaseTableRelation.id.ToString());
                }
            }

            return databaseTableRelations;
        }

        private static DatabaseTableRelation postUpdateDatabaseTableRelation(DatabaseTableRelation databaseTableRelation)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_DATABASE_TABLE_RELATION;
                command.Parameters.AddWithValue(DatabaseTableRelationDAO.SOURCE_DATABASE_TABLE_ID, databaseTableRelation.sourceDatabaseTableId);
                command.Parameters.AddWithValue(DatabaseTableRelationDAO.TARGET_DATABASE_TABLE_ID, databaseTableRelation.targetDatabaseTableId);
                command.Parameters.AddWithValue(DatabaseTableRelationDAO.REQUIRES_IDENTITY, databaseTableRelation.requiresIdentity);
                command.Parameters.AddWithValue(DatabaseTableRelationDAO.AT_ID, databaseTableRelation.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseTableRelation.id = DAOUtility.GetData<int>(reader, DatabaseTableDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateDatabaseTableRelation()", databaseTableRelation.id.ToString());
                }
            }
            return databaseTableRelation;
        }

        private static bool deleteDatabaseTableRelation(DatabaseTableRelation databaseTableRelation)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_DATABASE_TABLE_RELATION;
                command.Parameters.AddWithValue(DatabaseTableRelationDAO.AT_ID, databaseTableRelation.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            databaseTableRelation.id = DAOUtility.GetData<int>(reader, DatabaseTableDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteDatabaseTableRelation()", databaseTableRelation.id.ToString());
                }
            }
            return true;
        }
    }
}
