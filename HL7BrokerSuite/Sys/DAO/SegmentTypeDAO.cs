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
    public class SegmentTypeDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string NAME = "NAME";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_SEGMENT_TYPE_NAME = "SEGMENT_TYPE_NAME";
        public const string VIEW_SEGMENT_TYPE_ID   = "SEGMENT_TYPE_ID";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_NAME = "@NAME";

        public static List<SegmentType> Get()
        {
            return getSegmentTypes();
        }

        public static List<SegmentType> Get(SegmentType segmentType)
        {
            return getSingleSegmentType(segmentType);
        }

        public static SegmentType PostUpdate(SegmentType segmentType)
        {
            return postUpdateSegmentType(segmentType);
        }

        public static bool Delete(SegmentType segmentType)
        {
            return deleteSegmentType(segmentType);
        }

        private static List<SegmentType> getSegmentTypes()
        {
            List<SegmentType> segmentTypes = new List<SegmentType>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_SEGMENT_TYPE;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            segmentTypes.Add(new SegmentType(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSegmentTypes()");
                }
            }

            return segmentTypes;
        }

        private static List<SegmentType> getSingleSegmentType(SegmentType segmentType)
        {
            List<SegmentType> segmentTypes = new List<SegmentType>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_SEGMENT_TYPE;
                command.Parameters.AddWithValue(SegmentTypeDAO.AT_ID, segmentType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            segmentTypes.Add(new SegmentType(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleSegmentType(SegmentType SegmentType)", segmentType.id.ToString());
                }
            }

            return segmentTypes;
        }

        private static SegmentType postUpdateSegmentType(SegmentType segmentType)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_SEGMENT_TYPE;
                command.Parameters.AddWithValue(SegmentTypeDAO.AT_NAME, segmentType.name);
                command.Parameters.AddWithValue(SegmentTypeDAO.AT_ID, segmentType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            segmentType.id = DAOUtility.GetData<int>(reader, SegmentTypeDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateSegmentType()", segmentType.id.ToString());
                }
            }
            return segmentType;
        }

        private static bool deleteSegmentType(SegmentType segmentType)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_SEGMENT_TYPE;
                command.Parameters.AddWithValue(SegmentTypeDAO.AT_ID, segmentType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            segmentType.id = DAOUtility.GetData<int>(reader, SegmentTypeDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteSegmentType()", segmentType.id.ToString());
                }
            }
            return true;
        }


    }
}
