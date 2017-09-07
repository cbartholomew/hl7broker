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
    public class ColumnSetDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID      = "ID";
        public const string DATABASE_TABLE_ID = "DATABASE_TABLE_ID";
        public const string NAME    = "NAME";
        public const string IS_PRIMAR_KEY = "IS_PRIMARY_KEY";
        public const string MESSAGE_GROUP_INSTANCE_ID = "MESSAGE_GROUP_INSTANCE_ID";
        public const string COLUMN_DATA_TYPE = "COLUMN_DATA_TYPE";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_DATABASE_ID     = "DATABASE_COLUMN_ID";
        public const string VIEW_DATABASE_COLUMN = "DATABASE_COLUMN";
        public const string VIEW_DATABASE_COLUMN_IS_PRIMARY_KEY = "DATABASE_COLUMN_IS_PRIMARY_KEY";
        public const string VIEW_DATABASE_COLUMN_MESSAGE_GROUP_INSTANCE_ID = "DATABASE_COLUMN_MESSAGE_GROUP_INSTANCE_ID";
        public const string VIEW_DATABASE_COLUMN_DATA_TYPE = "DATABASE_COLUMN_DATA_TYPE";

        // PARAMETERS THAT ARE IN THE STORE PROCEDURE
        public const string AT_DATABASE_TABLE_ID = "@DATABASE_TABLE_ID";
        public const string AT_NAME = "@NAME";
        public const string AT_MESSAGE_GROUP_INSTANCE_ID = "@MESSAGE_GROUP_INSTANCE_ID";
        public const string AT_IS_PRIMAR_KEY = "@IS_PRIMARY_KEY";
        public const string AT_COLUMN_DATA_TYPE = "@COLUMN_DATA_TYPE";
        public const string AT_ID = "@ID";



        public static List<ColumnSet> Get()
        {
            return getColumnSets();
        }

        public static List<ColumnSet> Get(ColumnSet columnSet)
        {
            return getSingleColumnSet(columnSet);
        }

        public static ColumnSet PostUpdate(ColumnSet columnSet)
        {
            return postUpdateColumnSet(columnSet);
        }

        public static bool Delete(ColumnSet columnSet)
        {
            return deleteColumnSet(columnSet);
        }

        private static List<ColumnSet> getColumnSets()
        {
            List<ColumnSet> columnSets = new List<ColumnSet>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_COLUMN_SET;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            columnSets.Add(new ColumnSet(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getColumnSets()");
                }
            }

            return columnSets;
        }

        private static List<ColumnSet> getSingleColumnSet(ColumnSet ColumnSet)
        {
            List<ColumnSet> columnSets = new List<ColumnSet>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_COLUMN_SET;
                command.Parameters.AddWithValue(ColumnSetDAO.AT_ID, ColumnSet.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            columnSets.Add(new ColumnSet(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleColumnSet(ColumnSet ColumnSet)", ColumnSet.id.ToString());
                }
            }

            return columnSets;
        }

        private static ColumnSet postUpdateColumnSet(ColumnSet columnSet)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_COLUMN_SET;
                command.Parameters.AddWithValue(ColumnSetDAO.AT_DATABASE_TABLE_ID, columnSet.databaseTableId);
                command.Parameters.AddWithValue(ColumnSetDAO.AT_NAME, columnSet.name);
                command.Parameters.AddWithValue(ColumnSetDAO.AT_MESSAGE_GROUP_INSTANCE_ID, columnSet.messageGroupInstanceId);
                command.Parameters.AddWithValue(ColumnSetDAO.AT_IS_PRIMAR_KEY, columnSet.isPrimaryKey);
                command.Parameters.AddWithValue(ColumnSetDAO.AT_COLUMN_DATA_TYPE, columnSet.columnDataType);
                command.Parameters.AddWithValue(ColumnSetDAO.AT_ID, columnSet.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            columnSet.id = DAOUtility.GetData<int>(reader, ColumnSetDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateColumnSet()", columnSet.name);
                }
            }
            return columnSet;
        }

        private static bool deleteColumnSet(ColumnSet ColumnSet)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_COLUMN_SET;
                command.Parameters.AddWithValue(ColumnSetDAO.AT_ID, ColumnSet.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ColumnSet.id = DAOUtility.GetData<int>(reader, ColumnSetDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteColumnSet()", ColumnSet.name);
                }
            }
            return true;
        }
    }
}
