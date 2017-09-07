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
    public class CommunicationDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string DIRECTION_TYPE_ID = "DIRECTION_TYPE_ID";
        public const string COMMUNICATION_TYPE_ID = "COMMUNICATION_TYPE_ID";
        public const string APPLICATION_ID = "APPLICATION_ID";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_COMMUNICATION_COMMUNICATION_TYPE_ID = "COMMUNICATION_COMMUNICATION_TYPE_ID";
        public const string VIEW_COMMUNICATION_APPLICATION_ID = "COMMUNICATION_APPLICATION_ID";
        public const string VIEW_COMMUNICATION_DIRECTION_TYPE_ID = "COMMUNICATION_DIRECTION_TYPE_ID";
        public const string VIEW_COMMUNICATION_ID = "COMMUNICATION_ID";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_DIRECTION_TYPE_ID = "@DIRECTION_TYPE_ID";
        public const string AT_COMMUNICATION_TYPE_ID = "@COMMUNICATION_TYPE_ID";
        public const string AT_APPLICATION_ID = "@APPLICATION_ID";

        public static List<Communication> Get()
        {
            return getCommunications();
        }

        public static List<Communication> Get(Communication communication)
        {
            return getSingleCommunication(communication);
        }

        public static Communication PostUpdate(Communication communication)
        {
            return postUpdateCommunication(communication);
        }

        public static bool Delete(Communication communication)
        {
            return deleteCommunication(communication);
        }

        private static List<Communication> getCommunications()
        {
            List<Communication> communications = new List<Communication>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_COMMUNICATION;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            communications.Add(new Communication(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getCommunications()");
                }
            }

            return communications;
        }

        private static List<Communication> getSingleCommunication(Communication communication)
        {
            List<Communication> communications = new List<Communication>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_COMMUNICATION;
                command.Parameters.AddWithValue(CommunicationDAO.AT_ID, communication.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            communications.Add(new Communication(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleCommunication(Communication Communication)", communication.id.ToString());
                }
            }

            return communications;
        }

        private static Communication postUpdateCommunication(Communication communication)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_COMMUNICATION;
                command.Parameters.AddWithValue(CommunicationDAO.AT_DIRECTION_TYPE_ID, communication.directionTypeId);
                command.Parameters.AddWithValue(CommunicationDAO.AT_COMMUNICATION_TYPE_ID, communication.communicationTypeId);
                command.Parameters.AddWithValue(CommunicationDAO.AT_APPLICATION_ID, communication.applicationId);
                command.Parameters.AddWithValue(CommunicationDAO.AT_ID, communication.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            communication.id = DAOUtility.GetData<int>(reader, CommunicationDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateCommunication()", communication.id.ToString());
                }
            }
            return communication;
        }

        private static bool deleteCommunication(Communication communication)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_COMMUNICATION;
                command.Parameters.AddWithValue(CommunicationDAO.AT_ID, communication.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            communication.id = DAOUtility.GetData<int>(reader, CommunicationDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteCommunication()", communication.id.ToString());
                }
            }
            return true;
        }
    }
}
