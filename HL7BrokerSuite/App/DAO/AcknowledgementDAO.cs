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
    public class AcknowledgementDAO
    {
        public enum AcknowledgementType
        {
            AA      = 1,
            AE      = 2,
            AR      = 3,
            UNKNOWN = 4
        }
        public const string ID = "ID";
        public const string MESSAGE_ID = "MESSAGE_ID";
        public const string ACKNOWLEDGEMENT_TYPE_ID = "ACKNOWLEDGEMENT_TYPE_ID";
        public const string RAW = "RAW";
        public const string CREATED_DTTM = "CREATED_DTTM";

        public const string AT_MESSAGE_ID              = "@MESSAGE_ID";
        public const string AT_ACKNOWLEDGEMENT_TYPE_ID = "@ACKNOWLEDGEMENT_TYPE_ID";
        public const string AT_RAW                     = "@RAW";
        public const string AT_CREATED_DTTM            = "@CREATED_DTTM";

        public static Acknowledgement insertIntoAcknowledgement(Acknowledgement ack) 
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.POST_ACKNOWLEDGEMENT_PROCEDURE;
                command.Parameters.AddWithValue(AcknowledgementDAO.AT_MESSAGE_ID, ack.messageId);
                command.Parameters.AddWithValue(AcknowledgementDAO.AT_ACKNOWLEDGEMENT_TYPE_ID, ack.acknowledgementTypeId);
                command.Parameters.AddWithValue(AcknowledgementDAO.AT_RAW, ack.raw);
                command.Parameters.AddWithValue(AcknowledgementDAO.AT_CREATED_DTTM, ack.createdDttm);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ack = new Acknowledgement(reader);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "insertIntoAcknowledgement(Acknowledgement ack)", ack.messageId.ToString());
                }
            }
            return ack;
        }
    }
}
