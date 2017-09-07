using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Utility;
using HL7BrokerSuite.App.Model;
using System.Data.SqlClient;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Settings;

namespace HL7BrokerSuite.App.DAO
{
    public class MessageDAO
    {
        // insert statement specific to get identity
        public const string SCOPE_IDENTITY = " SET @id=SCOPE_IDENTITY();";
        public const string AT_SET_ID = "@id";

        // regular column names
        public const string ID = "ID";
        public const string MESSAGE_HEADER_INSTANCE_ID = "MESSAGE_HEADER_INSTANCE_ID";
        public const string HL7_RAW = "HL7_RAW";

        // view column names
        public const string VIEW_MESSAGE_ID = "MESSAGE_ID";
        public const string VIEW_MESSAGE_HL7_RAW = "MESSAGE_HL7_RAW";

        // parameter names
        public const string AT_ID = "@ID";
        public const string AT_MESSAGE_HEADER_INSTANCE_ID = "@MESSAGE_HEADER_INSTANCE_ID";
        public const string AT_HL7_RAW = "@HL7_RAW";

        // custom which is not normally like the other ones cause it's auto generated
        public static int insertIntoMessage(SqlConnection connection,
                                            SqlCommand command,
                                            SqlTransaction transaction)
        {
            int identityResult = -1;
            try
            {
                // create sql parameter for return value
                SqlParameter identityParm = new SqlParameter();

                // append command text to get identity
                command.CommandText += SCOPE_IDENTITY;

                // create identity parameter
                identityParm.ParameterName = AT_SET_ID;
                identityParm.DbType = System.Data.DbType.Int32;
                identityParm.Direction = System.Data.ParameterDirection.Output;

                // add identity param
                command.Parameters.Add(identityParm);

                // execute non query to set transaction
                command.ExecuteNonQuery();

                // commit all database transactions
                transaction.Commit();

                // return the scope identity
                identityResult = (int)identityParm.Value;
            }
            catch (SqlException sqlEx)
            {
                ErrorLogger.LogError(sqlEx, "insertIntoMessage()", command.CommandText);
                identityResult = -1;
                transaction.Rollback();
            }
            finally
            {     
                connection.Close();
            }
            return identityResult;
        }

        public static List<Message> Get()
        {
            return getMessages();
        }

        public static List<Message> Get(Message messagePart)
        {
            return getSingleMessage(messagePart);
        }

        public static bool Delete(Message messagePart)
        {
            return deleteMessage(messagePart);
        }

        private static List<Message> getMessages()
        {
            List<Message> messages = new List<Message>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_MESSAGE;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messages.Add(new Message(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getMessages()");
                }
            }

            return messages;
        }

        private static List<Message> getSingleMessage(Message message)
        {
            List<Message> messages = new List<Message>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_MESSAGE;
                command.Parameters.AddWithValue(MessageDAO.AT_ID, message.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messages.Add(new Message(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleMessage(Message Message)", message.id.ToString());
                }
            }

            return messages;
        }

        private static bool deleteMessage(Message message)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_MESSAGE;
                command.Parameters.AddWithValue(MessageDAO.AT_ID, message.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            message.id = DAOUtility.GetData<int>(reader, MessageDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteMessage()", message.id.ToString());
                }
            }
            return true;
        }
    }
}
