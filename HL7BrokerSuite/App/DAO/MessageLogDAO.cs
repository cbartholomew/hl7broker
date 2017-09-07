using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using HL7BrokerSuite.App.Model;
using HL7BrokerSuite.Utility;

namespace HL7BrokerSuite.App.DAO
{
    public class MessageLogDAO
    {
        public enum MessageLogType
        {
            ORIGINAL        = 1,
            REPROCESS       = 2,
            ERRORED         = 3,
            UNKNOWN         = 4,
            APP_REPROCESS   = 5,
            WEB_SERVICE     = 6
        }

        public const string ID = "ID";
        public const string MESSAGE_ID = "MESSAGE_ID";
        public const string MESSAGE_LOG_TYPE_ID = "MESSAGE_LOG_TYPE_ID";
        public const string CREATED_DTTM = "CREATED_DTTM";

        public const string AT_MESSAGE_ID = "@MESSAGE_ID";
        public const string AT_MESSAGE_LOG_TYPE_ID = "@MESSAGE_LOG_TYPE_ID";
        public const string AT_CREATED_DTTM = "@CREATED_DTTM";

        public static MessageLog insertIntoMessageLog(MessageLog messageLog)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.POST_MESSAGE_LOG_PROCEDURE;
                command.Parameters.AddWithValue(MessageLogDAO.AT_MESSAGE_ID, messageLog.messageId);
                command.Parameters.AddWithValue(MessageLogDAO.AT_MESSAGE_LOG_TYPE_ID, messageLog.messageLogTypeId);
                command.Parameters.AddWithValue(MessageLogDAO.AT_CREATED_DTTM, messageLog.createdDttm);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            messageLog = new MessageLog(reader);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "insertIntoMessageLog(MessageLog messageLog)", messageLog.messageId.ToString());
                }
            }
            return messageLog;
        }
    }
}
