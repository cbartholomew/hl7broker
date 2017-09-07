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
    public class MessagePartDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string NAME = "NAME";
        public const string DELIMITER = "DELIMITER";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_MESSAGE_PART_ID = "MESSAGE_PART_ID";
        public const string VIEW_MESSAGE_PART_NAME = "MESSAGE_PART_NAME";
        public const string VIEW_MESSAGE_PART_DELIMITER = "MESSAGE_PART_DELIMITER";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_NAME = "@NAME";
        public const string AT_DELIMITER = "@DELIMITER";

        public static List<MessagePart> Get()
        {
            return getMessageParts();
        }

        public static List<MessagePart> Get(MessagePart messagePart)
        {
            return getSingleMessagePart(messagePart);
        }

        public static MessagePart PostUpdate(MessagePart messagePart)
        {
            return postUpdateMessagePart(messagePart);
        }

        public static bool Delete(MessagePart messagePart)
        {
            return deleteMessagePart(messagePart);
        }

        private static List<MessagePart> getMessageParts()
        {
            List<MessagePart> messageParts = new List<MessagePart>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_MESSAGE_PART;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageParts.Add(new MessagePart(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getMessageParts()");
                }
            }

            return messageParts;
        }

        private static List<MessagePart> getSingleMessagePart(MessagePart messagePart)
        {
            List<MessagePart> messageParts = new List<MessagePart>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_MESSAGE_PART;
                command.Parameters.AddWithValue(MessagePartDAO.AT_ID, messagePart.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageParts.Add(new MessagePart(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleMessagePart(MessagePart MessagePart)", messagePart.id.ToString());
                }
            }

            return messageParts;
        }

        private static MessagePart postUpdateMessagePart(MessagePart messagePart)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_MESSAGE_PART;
                command.Parameters.AddWithValue(MessagePartDAO.AT_NAME, messagePart.name);
                command.Parameters.AddWithValue(MessagePartDAO.AT_DELIMITER, messagePart.delimiter);
                command.Parameters.AddWithValue(MessagePartDAO.AT_ID, messagePart.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messagePart.id = DAOUtility.GetData<int>(reader, MessagePartDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateMessagePart()", messagePart.id.ToString());
                }
            }
            return messagePart;
        }

        private static bool deleteMessagePart(MessagePart messagePart)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_MESSAGE_PART;
                command.Parameters.AddWithValue(MessagePartDAO.AT_ID, messagePart.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messagePart.id = DAOUtility.GetData<int>(reader, MessagePartDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteMessagePart()", messagePart.id.ToString());
                }
            }
            return true;
        }
    }
}
