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
    public class BrokerDAO
    {

        public enum Property
        {
            Status,
            Process,
            Stats
        }

        public const int STOPPED = 2;
        public const int CONNECTED = 3;
        public const int WAITING = 4;

        // normal columns
        public const string ID                  = "ID";
        public const string INTERFACE_STATUS_ID = "INTERFACE_STATUS_ID";
        public const string COMMUNICATION_ID    = "COMMUNICATION_ID";
        public const string PROCESS_ID          = "PROCESS_ID";
        public const string LAST_MESSAGE_ID     = "LAST_MESSAGE_ID";
        public const string QUEUE_COUNT         = "QUEUE_COUNT";
        public const string LAST_MESSAGE_DTTM   = "LAST_MESSAGE_DTTM";

        // view columns
        public const string VIEW_BROKER_ID                       = "BROKER_ID";
        public const string VIEW_APPLICATION_DESCRIPTION         = "APPLICATION_DESCRIPTION";
        public const string VIEW_APPLICATION_NAME                = "APPLICATION_NAME";
        public const string VIEW_BROKER_QUEUE_COUNT              = "BROKER_QUEUE_COUNT";
        public const string VIEW_INTERFACE_STATUS_NAME           = "INTERFACE_STATUS_NAME";
        public const string VIEW_INTERFACE_STATUS_COLOR          = "INTERFACE_STATUS_COLOR";
        public const string VIEW_COMMUNICATION_TYPE_NAME         = "COMMUNICATION_TYPE_NAME";
        public const string VIEW_DIRECTION_TYPE_NAME             = "DIRECTION_TYPE_NAME";
        public const string VIEW_BROKER_PROCESS_ID               = "BROKER_PROCESS_ID";
        public const string VIEW_BROKER_LAST_MESSAGE_DTTM        = "BROKER_LAST_MESSAGE_DTTM";
        public const string VIEW_BROKER_LAST_MESSAGE_ID          = "BROKER_LAST_MESSAGE_ID";
        public const string VIEW_MESSAGE_HL7_RAW                 = "MESSAGE_HL7_RAW";
        public const string VIEW_MESSAGE_HEADER_INSTANCE_ID      = "MESSAGE_HEADER_INSTANCE_ID";
        public const string VIEW_DIRECTION_TYPE_ID               = "DIRECTION_TYPE_ID";
        public const string VIEW_COMMUNICATION_TYPE_ID           = "COMMUNICATION_TYPE_ID";
        public const string VIEW_BROKER_COMMUNICATION_ID         = "BROKER_COMMUNICATION_ID";
        public const string VIEW_BROKER_INTERFACE_STATUS_ID      = "BROKER_INTERFACE_STATUS_ID";
        public const string VIEW_COMMUNICATION_APPLICATION_ID    = "COMMUNICATION_APPLICATION_ID";

        // parameter constants
        public const string AT_ID = "@ID";
        public const string AT_INTERFACE_STATUS_ID = "@INTERFACE_STATUS_ID";
        public const string AT_COMMUNICATION_ID = "@COMMUNICATION_ID";
        public const string AT_PROCESS_ID = "@PROCESS_ID";
        public const string AT_LAST_MESSAGE_ID = "@LAST_MESSAGE_ID";
        public const string AT_QUEUE_COUNT = "@QUEUE_COUNT";
        public const string AT_LAST_MESSAGE_DTTM = "@LAST_MESSAGE_DTTM";

        //
        public static List<Broker> Get()
        {
            return getBrokers();
        }

        public static List<Broker> Get(Broker broker)
        {
            return getSingleBroker(broker);
        }

        public static List<Broker> GetBrokerWorklist()
        {
            List<Broker> brokers = new List<Broker>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.SERVICE_VIEW_GET_BROKER_WORKLIST;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            brokers.Add(new Broker(reader,true));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetBrokerWorklist()");
                }
            }

            return brokers;
        }

        public static Broker PostUpdate(Broker broker)
        {
            return postUpdateBroker(broker);
        }

        public static bool UpdateAppBrokerProperty(Broker broker, Property property) 
        {
            try
            {
                switch (property)
                {
                    case Property.Status:
                        updateInterfaceStatus(broker);
                        break;
                    case Property.Process:
                        updateProcessIdentity(broker);
                        break;
                    case Property.Stats:
                        updateLastMessageStats(broker);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "UpdateAppBroker(Broker broker, Property property)", broker.id.ToString());
                return false;
            }          
            return true;
        }

        private static void updateInterfaceStatus(Broker broker)
        {   
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();
            try
            {
                using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac))) 
                {                    
                    SqlCommand command = new SqlCommand();
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = das.UPDATE_INTERFACE_STATUS;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(BrokerDAO.AT_INTERFACE_STATUS_ID, broker.interfaceStatusId);
                    command.Parameters.AddWithValue(BrokerDAO.AT_ID, broker.id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "BrokerDAO.updateInterfaceStatus(Broker broker", broker.id.ToString());
            }              
        }

        private static void updateProcessIdentity(Broker broker)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();
            try
            {
                using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
                {
                    SqlCommand command = new SqlCommand();
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = das.UPDATE_PROCESS_IDENTITY;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(BrokerDAO.AT_PROCESS_ID,broker.processId);
                    command.Parameters.AddWithValue(BrokerDAO.AT_ID, broker.id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "BrokerDAO.updateProcessIdentity(Broker broker", broker.id.ToString());
            }              
        
        }

        private static void updateLastMessageStats(Broker broker)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();
            try
            {
                using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
                {
                    SqlCommand command = new SqlCommand();
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = das.UPDATE_MESSAGE_STATS;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(BrokerDAO.AT_LAST_MESSAGE_ID, broker.lastMessageId);
                    command.Parameters.AddWithValue(BrokerDAO.AT_LAST_MESSAGE_DTTM, broker.lastMessageDTTM);
                    command.Parameters.AddWithValue(BrokerDAO.AT_QUEUE_COUNT, broker.queueCount);
                    command.Parameters.AddWithValue(BrokerDAO.ID, broker.id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "BrokerDAO.updateLastMessageStats(Broker broker", broker.id.ToString() + "|" + broker.lastMessageDTTM.ToString());
            }              
        
        }

        public static bool Delete(Broker broker)
        {
            return deleteBroker(broker);
        }

        private static List<Broker> getBrokers()
        {
            List<Broker> brokers = new List<Broker>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_BROKER;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            brokers.Add(new Broker(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetBrokers()");
                }
            }

            return brokers;
        }

        private static List<Broker> getSingleBroker(Broker broker)
        {
            List<Broker> brokers = new List<Broker>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_BROKER;
                command.Parameters.AddWithValue(BrokerDAO.AT_ID, broker.id);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            brokers.Add(new Broker(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleBroker(Broker broker)", broker.id.ToString());
                }
            }

            return brokers;
        }

        private static Broker postUpdateBroker(Broker broker)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_BROKER;
                command.Parameters.AddWithValue(BrokerDAO.AT_INTERFACE_STATUS_ID, broker.interfaceStatusId);
                command.Parameters.AddWithValue(BrokerDAO.AT_COMMUNICATION_ID, broker.communicationId);
                command.Parameters.AddWithValue(BrokerDAO.PROCESS_ID, broker.processId);
                command.Parameters.AddWithValue(BrokerDAO.LAST_MESSAGE_ID, broker.lastMessageId);
                command.Parameters.AddWithValue(BrokerDAO.QUEUE_COUNT, broker.queueCount);
                command.Parameters.AddWithValue(BrokerDAO.LAST_MESSAGE_DTTM, broker.lastMessageDTTM);                
                command.Parameters.AddWithValue(BrokerDAO.AT_ID, broker.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            broker.id = DAOUtility.GetData<int>(reader, BrokerDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateBroker()", broker.id.ToString());
                }
            }
            return broker;
        }

        private static bool deleteBroker(Broker broker)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_BROKER;
                command.Parameters.AddWithValue(BrokerDAO.AT_ID, broker.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            broker.id = DAOUtility.GetData<int>(reader, BrokerDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteBroker()", broker.id.ToString());
                }
            }
            return true;
        }






    }
}
