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
    public class MessageGroupInstanceDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string MESSAGE_TYPE_ID = "MESSAGE_TYPE_ID";
        public const string SEGMENT_TYPE_ID = "SEGMENT_TYPE_ID";
        public const string CREATED_DTTM = "CREATED_DTTM";
        public const string UPDATED_DTTM = "UPDATED_DTTM";
        public const string DESCRIPTION = "DESCRIPTION";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_MESSAGE_GROUP_INSTANCE_DESCRIPTION = "MESSAGE_GROUP_INSTANCE_DESCRIPTION";
        public const string VIEW_MESSAGE_GROUP_INSTANCE_ID = "MESSAGE_GROUP_INSTANCE_ID";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_MESSAGE_TYPE_ID = "@MESSAGE_TYPE_ID";
        public const string AT_SEGMENT_TYPE_ID = "@SEGMENT_TYPE_ID";
        public const string AT_CREATED_DTTM = "@CREATED_DTTM";
        public const string AT_UPDATED_DTTM = "@UPDATED_DTTM";
        public const string AT_DESCRIPTION = "@DESCRIPTION";


        public static List<MessageGroupInstance> Get()
        {
            return getMessageGroupInstances();
        }

        public static List<MessageGroupInstance> Get(MessageGroupInstance messageGroupInstance)
        {
            return getSingleMessageGroupInstance(messageGroupInstance);
        }

        public static MessageGroupInstance PostUpdate(MessageGroupInstance messageGroupInstance)
        {
            return postUpdateMessageGroupInstance(messageGroupInstance);
        }

        public static bool Delete(MessageGroupInstance messageGroupInstance)
        {
            return deleteMessageGroupInstance(messageGroupInstance);
        }

        private static List<MessageGroupInstance> getMessageGroupInstances()
        {
            List<MessageGroupInstance> messageGroupInstances = new List<MessageGroupInstance>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_MESSAGE_GROUP_INSTANCE;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageGroupInstances.Add(new MessageGroupInstance(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getMessageGroupInstances()");
                }
            }

            return messageGroupInstances;
        }

        private static List<MessageGroupInstance> getSingleMessageGroupInstance(MessageGroupInstance messageGroupInstance)
        {
            List<MessageGroupInstance> messageGroupInstances = new List<MessageGroupInstance>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_MESSAGE_GROUP_INSTANCE;
                command.Parameters.AddWithValue(MessageGroupInstanceDAO.AT_ID, messageGroupInstance.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageGroupInstances.Add(new MessageGroupInstance(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleMessageGroupInstance(MessageGroupInstance MessageGroupInstance)", messageGroupInstance.id.ToString());
                }
            }

            return messageGroupInstances;
        }

        private static MessageGroupInstance postUpdateMessageGroupInstance(MessageGroupInstance messageGroupInstance)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_MESSAGE_GROUP_INSTANCE;
                command.Parameters.AddWithValue(MessageGroupInstanceDAO.AT_MESSAGE_TYPE_ID, messageGroupInstance.messageTypeId);
                command.Parameters.AddWithValue(MessageGroupInstanceDAO.AT_SEGMENT_TYPE_ID, messageGroupInstance.segmentTypeId);
                command.Parameters.AddWithValue(MessageGroupInstanceDAO.AT_DESCRIPTION, messageGroupInstance.description);
                command.Parameters.AddWithValue(MessageGroupInstanceDAO.AT_ID, messageGroupInstance.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageGroupInstance.id = DAOUtility.GetData<int>(reader, MessageGroupInstanceDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateMessageGroupInstance()", messageGroupInstance.id.ToString());
                }
            }
            return messageGroupInstance;
        }

        private static bool deleteMessageGroupInstance(MessageGroupInstance messageGroupInstance)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_MESSAGE_GROUP_INSTANCE;
                command.Parameters.AddWithValue(MessageGroupInstanceDAO.AT_ID, messageGroupInstance.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageGroupInstance.id = DAOUtility.GetData<int>(reader, MessageGroupInstanceDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteMessageGroupInstance()", messageGroupInstance.id.ToString());
                }
            }
            return true;
        }

    }
}
