using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Configuration;

namespace HL7Broker.Utility
{
    public class ErrorLogger
    {

        public static void LogError(Exception exc,string methodName,string parameters = "",string username = "")
        {
            String hostName = Dns.GetHostName();

            String CONNNECTION_STRING = "Data Source=SHCAPPPRODDB;Initial Catalog=SHIELDS_ERROR_LOGGER;User Id=ReadingRadPooledUser;Password=ReadingRadPooledUser";
            int APP_ID = 16;
            using (SqlConnection myConnection = new SqlConnection(CONNNECTION_STRING))
            {
                SqlCommand myCommand = new SqlCommand("INSERT_ERROR", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;

                // db will generate auto new id
                myCommand.Parameters.AddWithValue("@APPLICATION_ID", APP_ID);
                myCommand.Parameters.AddWithValue("@HOST", hostName);
                myCommand.Parameters.AddWithValue("@MESSAGE", exc.Message);
                myCommand.Parameters.AddWithValue("@STACKTRACE", exc.StackTrace);
                myCommand.Parameters.AddWithValue("@METHODNAME", methodName);
                myCommand.Parameters.AddWithValue("@PARAMETERS", parameters);
                myCommand.Parameters.AddWithValue("@USER", username);
                myCommand.Parameters.AddWithValue("@DATE", DateTime.Now);

                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }

        public static void WriteErrorToEventViewer(Exception exc)
        {
            String AppName = "HL7Broker";
            EventLog eventLog = new EventLog("");
            eventLog.Source = AppName;
            eventLog.WriteEntry("Error: " + exc.Message + " Stack Trace: " + exc.StackTrace, EventLogEntryType.Error);
        }

        public static void WriteMessageToEventViewer(string message)
        {
            String AppName = "HL7Broker";
            EventLog eventLog = new EventLog("");
            eventLog.Source = AppName;
            eventLog.WriteEntry("Message: " + message, EventLogEntryType.Information);
        }

        // make log type
        public enum LogType
        {
            CONNECTED,
            PENDING,
            STARTED,
            STOPPED,
            ERRORED,
            UNKNOWN,
            MESSAGE_RECIEVED,
            MESSAGE_SENT,
            ACK_GENERATED,
            NACK_GENERATED,
            INFORMATION,
            WAITING,
            INBOUND_SERVICE_STARTING,
            INBOUND_SERVICE_STOPPING, 
            OUTBOUND_SERVICE_STARTING,
            OUTBOUND_SERVICE_STOPPING
        }

        public static void Log(LogType type, String Message, String Method = "", String HL7Raw = "")
        {
            EventLog eventLog = new EventLog("");
            eventLog.Source = "HL7Broker";
            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendLine("Log Type: " + type.ToString());
            logMessage.AppendLine("Message: "  + Message);
            logMessage.AppendLine("Method: " + Method);
            logMessage.AppendLine("HL7: " + HL7Raw);
            eventLog.WriteEntry(logMessage.ToString(), EventLogEntryType.Information);
        }
        
    }
}