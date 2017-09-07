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
    public class CommunicationTypeDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string NAME = "NAME";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_COMMUNICATION_TYPE_ID = "COMMUNICATION_TYPE_ID";
        public const string VIEW_COMMUNICATION_TYPE_NAME = "COMMUNICATION_TYPE_NAME";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_NAME = "@NAME";

        public static List<CommunicationType> Get()
        {
            return getCommunicationTypes();
        }

        public static List<CommunicationType> Get(CommunicationType communicationType)
        {
            return getSingleCommunicationType(communicationType);
        }

        public static CommunicationType PostUpdate(CommunicationType communicationType)
        {
            return postUpdateCommunicationType(communicationType);
        }

        public static bool Delete(CommunicationType communicationType)
        {
            return deleteCommunicationType(communicationType);
        }

        private static List<CommunicationType> getCommunicationTypes()
        {
            List<CommunicationType> communicationTypes = new List<CommunicationType>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_COMMUNICATION_TYPE;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            communicationTypes.Add(new CommunicationType(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getCommunicationTypes()");
                }
            }

            return communicationTypes;
        }

        private static List<CommunicationType> getSingleCommunicationType(CommunicationType communicationType)
        {
            List<CommunicationType> communicationTypes = new List<CommunicationType>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_COMMUNICATION_TYPE;
                command.Parameters.AddWithValue(CommunicationTypeDAO.AT_ID, communicationType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            communicationTypes.Add(new CommunicationType(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleCommunicationType(CommunicationType CommunicationType)", communicationType.id.ToString());
                }
            }

            return communicationTypes;
        }

        private static CommunicationType postUpdateCommunicationType(CommunicationType communicationType)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_COMMUNICATION_TYPE;
                command.Parameters.AddWithValue(CommunicationTypeDAO.AT_NAME, communicationType.name);
                command.Parameters.AddWithValue(CommunicationTypeDAO.AT_ID, communicationType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            communicationType.id = DAOUtility.GetData<int>(reader, CommunicationTypeDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateCommunicationType()", communicationType.id.ToString());
                }
            }
            return communicationType;
        }

        private static bool deleteCommunicationType(CommunicationType communicationType)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_COMMUNICATION_TYPE;
                command.Parameters.AddWithValue(CommunicationTypeDAO.AT_ID, communicationType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            communicationType.id = DAOUtility.GetData<int>(reader, CommunicationTypeDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteCommunicationType()", communicationType.id.ToString());
                }
            }
            return true;
        }

    }
}
