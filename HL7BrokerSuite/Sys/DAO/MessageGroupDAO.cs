using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Utility;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Settings;
using System.Data.SqlClient;

namespace HL7BrokerSuite.Sys.Model
{
    public class MessageGroupDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string MESSAGE_GROUP_INSTANCE_ID = "MESSAGE_GROUP_INSTANCE_ID";
        public const string MESSAGE_PART_ID = "MESSAGE_PART_ID";
        public const string POSITION = "POSITION";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_MESSAGE_GROUP_POSITION = "MESSAGE_GROUP_POSITION";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_MESSAGE_GROUP_INSTANCE_ID = "@MESSAGE_GROUP_INSTANCE_ID";
        public const string AT_MESSAGE_PART_ID = "@MESSAGE_PART_ID";
        public const string AT_POSITION = "@POSITION";


        public static List<MessageGroup> Get()
        {
            return getMessageGroups();
        }

        public static List<MessageGroup> Get(MessageGroup messageGroup)
        {
            return getSingleMessageGroup(messageGroup);
        }

        public static MessageGroup PostUpdate(MessageGroup messageGroup)
        {
            return postUpdateMessageGroup(messageGroup);
        }

        public static bool Delete(MessageGroup messageGroup)
        {
            return deleteMessageGroup(messageGroup);
        }

        private static List<MessageGroup> getMessageGroups()
        {
            List<MessageGroup> messageGroups = new List<MessageGroup>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_MESSAGE_GROUP;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageGroups.Add(new MessageGroup(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getMessageGroups()");
                }
            }

            return messageGroups;
        }

        private static List<MessageGroup> getSingleMessageGroup(MessageGroup messageGroup)
        {
            List<MessageGroup> messageGroups = new List<MessageGroup>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_MESSAGE_GROUP;
                command.Parameters.AddWithValue(MessageGroupDAO.AT_ID, messageGroup.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageGroups.Add(new MessageGroup(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleMessageGroup(MessageGroup MessageGroup)", messageGroup.id.ToString());
                }
            }

            return messageGroups;
        }

        private static MessageGroup postUpdateMessageGroup(MessageGroup messageGroup)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_MESSAGE_GROUP;
                command.Parameters.AddWithValue(MessageGroupDAO.AT_MESSAGE_GROUP_INSTANCE_ID, messageGroup.messageGroupInstanceId);
                command.Parameters.AddWithValue(MessageGroupDAO.AT_MESSAGE_PART_ID, messageGroup.messagePartId);
                command.Parameters.AddWithValue(MessageGroupDAO.AT_POSITION, messageGroup.position);
                command.Parameters.AddWithValue(MessageGroupDAO.AT_ID, messageGroup.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageGroup.id = DAOUtility.GetData<int>(reader, MessageGroupDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateMessageGroup()", messageGroup.id.ToString());
                }
            }
            return messageGroup;
        }

        private static bool deleteMessageGroup(MessageGroup messageGroup)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_MESSAGE_GROUP;
                command.Parameters.AddWithValue(MessageGroupDAO.AT_ID, messageGroup.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageGroup.id = DAOUtility.GetData<int>(reader, MessageGroupDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteMessageGroup()", messageGroup.id.ToString());
                }
            }
            return true;
        }
    }
}
