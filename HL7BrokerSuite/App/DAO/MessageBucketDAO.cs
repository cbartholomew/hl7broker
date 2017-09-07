using HL7BrokerSuite.App.Model;
using HL7BrokerSuite.Settings;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.App.DAO
{
    public class MessageBucketDAO : GenericUtility
    {
        public const string ID = "ID";
        public const string AT_MHI_SENDING_APPLICATION  = "@MHI_SENDING_APPLICATION";
        public const string AT_MHI_PROCESSED = "@MHI_PROCESSED";
        public const string AT_ID = "@ID";
        public const string AT_MESSAGE_CONTROL_ID = "@MESSAGE_CONTROL_ID";
        public const string AT_MESSAGE_ID = "@MESSAGE_ID";
        public const string AT_ISREPROCESS = "@ISREPROCESS";
        public const string AT_ACCESSION_NO = "@ACCESSION_NO";

        public static List<MessageBucket> GetUnprocessedMessageHeaderInstancesByApplication(Application application)
        {
            List<MessageBucket> brokerMessages = new List<MessageBucket>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();
            
            // set process flag
            bool isProcessed = false;

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_MESSAGE_HEADER_INSTANCES;
                command.Parameters.AddWithValue(MessageBucketDAO.AT_MHI_SENDING_APPLICATION, application.name);
                command.Parameters.AddWithValue(MessageBucketDAO.AT_MHI_PROCESSED, isProcessed);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            brokerMessages.Add(new MessageBucket(reader,
                                MessageBucket.View.GetMessageHeaderInstances));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetUnprocessedMessageHeaderInstancesByApplication(Application application)", application.id.ToString());
                }
            }
            return brokerMessages;
        }

        public static List<MessageBucket> GetProcessedMessageHeaderInstancesByApplication(Application application)
        {
            List<MessageBucket> brokerMessages = new List<MessageBucket>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            // set process flag
            bool isProcessed = true;

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_MESSAGE_HEADER_INSTANCES;
                command.Parameters.AddWithValue(MessageBucketDAO.AT_MHI_SENDING_APPLICATION, application.name);
                command.Parameters.AddWithValue(MessageBucketDAO.AT_MHI_PROCESSED, isProcessed);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            brokerMessages.Add(new MessageBucket(reader,
                                MessageBucket.View.GetMessageHeaderInstances));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetProcessedMessageHeaderInstancesByApplication(Application application)", application.id.ToString());
                }
            }

            return brokerMessages;
        }

        public static List<MessageBucket> GetUnprocessedMessageHeaderInstances()
        {
            List<MessageBucket> brokerMessages = new List<MessageBucket>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            // set process flag
            bool isProcessed = false;

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_MESSAGE_HEADER_INSTANCES;
                command.Parameters.AddWithValue(MessageBucketDAO.AT_MHI_PROCESSED, isProcessed);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            brokerMessages.Add(new MessageBucket(reader,
                                MessageBucket.View.GetMessageHeaderInstances));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetUnprocessedMessageHeaderInstances()");
                }
            }
            return brokerMessages;
        }

        public static List<MessageBucket> GetProcessedMessageHeaderInstances()
        {
            List<MessageBucket> brokerMessages = new List<MessageBucket>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            // set process flag
            bool isProcessed = true;

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_MESSAGE_HEADER_INSTANCES;
                command.Parameters.AddWithValue(MessageBucketDAO.AT_MHI_PROCESSED, isProcessed);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            brokerMessages.Add(new MessageBucket(reader,
                                MessageBucket.View.GetMessageHeaderInstances));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetProcessedMessageHeaderInstances()");
                }
            }
            return brokerMessages;
        }

        public static bool UpdateProcessedFlagAndMessageLog(MessageHeaderInstance messageHeaderInstance, 
                                                            Message message, 
                                                            bool isReprocess = false)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            // set the flag which determines the reprocess
            bool isProcessed = false;

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.UPDATE_MESSAGE_HEADER_INSTANCE_PROCESS;
                command.Parameters.AddWithValue(MessageBucketDAO.AT_ID, messageHeaderInstance.id);
                command.Parameters.AddWithValue(MessageBucketDAO.AT_MESSAGE_ID,   message.id);
                command.Parameters.AddWithValue(MessageBucketDAO.AT_ISREPROCESS, isReprocess);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            isProcessed = DAOUtility.GetData<bool>(reader, MessageBucketDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "MessageHeaderInstance messageHeaderInstance, Message message", 
                        messageHeaderInstance.id.ToString() + "|" + message.id.ToString());

                    ErrorLogger.LogError(new Exception("Message UpdateProcessedFlagAndMessageLog Retry"),
                       "MessageHeaderInstance messageHeaderInstance, Message message", messageHeaderInstance.id.ToString() + "|" + message.id.ToString());

                    isProcessed = UpdateProcessedFlagAndMessageLog(messageHeaderInstance, message, isReprocess);   
                }               
            }

            return isProcessed;       
        }

        public static bool ReprocessMessage(int messageHeaderInstanceId)
        {
            return reprocessMessage(messageHeaderInstanceId, BLANK);
        }

        public static bool ReprocessMessage(string messageControlId)
        {
            return reprocessMessage(0, messageControlId);
        }

        public static bool RisResendReport()
        {
            return risResendAllMissing();
        }

        public static bool RisResendReport(string accessionNo)
        {
            return RisResendReport(accessionNo);
        }

        private static bool reprocessMessage(int messageHeaderInstanceId, string messageControlId)
        {
            if (messageHeaderInstanceId == Convert.ToInt32(ZERO) && messageControlId == BLANK)
            {
                throw new ArgumentNullException("There is no message header instance id or message control id, can't reprocess");
            }

            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            // set the flag which determines the reprocess
            bool isProcessed = false;
    
            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.UPDATE_MESSAGE_HEADER_INSTANCE_FOR_REPROCESS;
                command.Parameters.AddWithValue(MessageBucketDAO.AT_ID, messageHeaderInstanceId);
                command.Parameters.AddWithValue(MessageBucketDAO.AT_MESSAGE_CONTROL_ID, messageControlId);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            isProcessed = DAOUtility.GetData<bool>(reader, MessageBucketDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "reprocessMessage(int messageHeaderInstanceId, string messageControlId)",messageHeaderInstanceId.ToString() + "|" + messageControlId);
                }
            }
            return isProcessed;
        } 

        private static bool risResendSpecificReport(string accessionNo)
        {
            // set the flag which determines the reprocess
            bool isProcessed = false;

            try
            {
                if (accessionNo == BLANK)
                {
                    throw new ArgumentException("Accession number can't be blank");
                }

                SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
                DataAccess das = new DataAccess(); 

                using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = das.EXTERNAL_REPROCESS_RIS_REPORT;
                    command.Parameters.AddWithValue(MessageBucketDAO.AT_ACCESSION_NO, accessionNo);
                    command.ExecuteNonQuery();

                    isProcessed = true;
                }            

            }
            catch (Exception e)
            {                
                ErrorLogger.LogError(e, "BrokerDAO.risResendSpecificaReport(string accessionNo)", accessionNo);
            }

            return isProcessed;
        }

        private static bool risResendAllMissing()
        {
            // set the flag which determines the reprocess
            bool isProcessed = false;

            try
            {
                SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
                DataAccess das = new DataAccess();

                using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = das.EXTERNAL_REPROCESS_RIS_REPORT;
                    command.Parameters.AddWithValue(MessageBucketDAO.AT_ACCESSION_NO, DBNull.Value);
                    command.ExecuteNonQuery();

                    isProcessed = true;
                }

            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, "BrokerDAO.risResendSpecificaReport()");
            }

            return isProcessed;
        }
    }
}
