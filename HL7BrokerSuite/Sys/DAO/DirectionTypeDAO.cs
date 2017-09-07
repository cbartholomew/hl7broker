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
    public class DirectionTypeDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string NAME = "NAME";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_DIRECTION_TYPE_NAME = "DIRECTION_TYPE_NAME";
        public const string VIEW_DIRECTION_TYPE_ID = "DIRECTION_TYPE_ID";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_NAME = "@NAME";



        public static List<DirectionType> Get()
        {
            return getDirectionTypes();
        }

        public static List<DirectionType> Get(DirectionType directionType)
        {
            return getSingleDirectionType(directionType);
        }

        public static DirectionType PostUpdate(DirectionType directionType)
        {
            return postUpdateDirectionType(directionType);
        }

        public static bool Delete(DirectionType directionType)
        {
            return deleteDirectionType(directionType);
        }

        private static List<DirectionType> getDirectionTypes()
        {
            List<DirectionType> directionTypes = new List<DirectionType>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_DIRECTION_TYPE;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            directionTypes.Add(new DirectionType(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getDirectionTypes()");
                }
            }

            return directionTypes;
        }

        private static List<DirectionType> getSingleDirectionType(DirectionType directionType)
        {
            List<DirectionType> directionTypes = new List<DirectionType>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_DIRECTION_TYPE;
                command.Parameters.AddWithValue(DirectionTypeDAO.AT_ID, directionType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            directionTypes.Add(new DirectionType(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleDirectionType(DirectionType DirectionType)", directionType.id.ToString());
                }
            }

            return directionTypes;
        }

        private static DirectionType postUpdateDirectionType(DirectionType directionType)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_DIRECTION_TYPE;
                command.Parameters.AddWithValue(DirectionTypeDAO.AT_NAME, directionType.name);
                command.Parameters.AddWithValue(DirectionTypeDAO.AT_ID, directionType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            directionType.id = DAOUtility.GetData<int>(reader, DirectionTypeDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateDirectionType()", directionType.id.ToString());
                }
            }
            return directionType;
        }

        private static bool deleteDirectionType(DirectionType directionType)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_DIRECTION_TYPE;
                command.Parameters.AddWithValue(DirectionTypeDAO.AT_ID, directionType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            directionType.id = DAOUtility.GetData<int>(reader, DirectionTypeDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteDirectionType()", directionType.id.ToString());
                }
            }
            return true;
        }


    }
}
