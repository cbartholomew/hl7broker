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
    public class MessageTypeDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string NAME = "NAME";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_MESSAGE_TYPE_NAME = "MESSAGE_TYPE_NAME";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_NAME = "@NAME";

        public static List<MessageType> Get()
        {
            return getMessageTypes();
        }

        public static List<MessageType> Get(MessageType messageType)
        {
            return getSingleMessageType(messageType);
        }

        public static MessageType PostUpdate(MessageType messageType)
        {
            return postUpdateMessageType(messageType);
        }

        public static bool Delete(MessageType messageType)
        {
            return deleteMessageType(messageType);
        }

        private static List<MessageType> getMessageTypes()
        {
            List<MessageType> messageTypes = new List<MessageType>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_MESSAGE_TYPE;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageTypes.Add(new MessageType(reader));
                        }
                    }

                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getMessageTypes()");
                }
            }

            return messageTypes;
        }

        private static List<MessageType> getSingleMessageType(MessageType messageType)
        {
            List<MessageType> messageTypes = new List<MessageType>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_MESSAGE_TYPE;
                command.Parameters.AddWithValue(MessageTypeDAO.AT_ID, messageType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageTypes.Add(new MessageType(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleMessageType(MessageType MessageType)", messageType.id.ToString());
                }
            }

            return messageTypes;
        }

        private static MessageType postUpdateMessageType(MessageType messageType)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_MESSAGE_TYPE;
                command.Parameters.AddWithValue(MessageTypeDAO.AT_NAME, messageType.name);
                command.Parameters.AddWithValue(MessageTypeDAO.AT_ID, messageType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageType.id = DAOUtility.GetData<int>(reader, MessageTypeDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateMessageType()", messageType.id.ToString());
                }
            }
            return messageType;
        }

        private static bool deleteMessageType(MessageType messageType)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_MESSAGE_TYPE;
                command.Parameters.AddWithValue(MessageTypeDAO.AT_ID, messageType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageType.id = DAOUtility.GetData<int>(reader, MessageTypeDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteMessageType()", messageType.id.ToString());
                }
            }
            return true;
        }


    }
}
