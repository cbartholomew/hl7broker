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
    public class InterfaceDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string COMMUNICATION_ID = "COMMUNICATION_ID";
        public const string CREDENTIAL_ID = "CREDENTIAL_ID";
        public const string IP_ADDRESS = "IP_ADDRESS";
        public const string PORT = "PORT";
        public const string MAX_CONNECTIONS = "MAX_CONNECTIONS";

        // COLUMNS THAT ARE BASED ON CUSTOM VIEWS
        public const string VIEW_INTERFACE_IP_ADDRESS = "INTERFACE_IP_ADDRESS";
        public const string VIEW_INTERFACE_PORT = "INTERFACE_PORT";
        public const string VIEW_INTERFACE_MAX_CONNECTIONS = "INTERFACE_MAX_CONNECTIONS";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_COMMUNICATION_ID = "@COMMUNICATION_ID";
        public const string AT_CREDENTIAL_ID = "@CREDENTIAL_ID";
        public const string AT_IP_ADDRESS = "@IP_ADDRESS";
        public const string AT_PORT = "@PORT";
        public const string AT_MAX_CONNECTIONS = "@MAX_CONNECTIONS";

        public static List<Interface> Get()
        {
            return getInterfaces();
        }

        public static List<Interface> Get(Interface _interface)
        {
            return getSingleInterface(_interface);
        }

        public static Interface PostUpdate(Interface _interface)
        {
            return postUpdateInterface(_interface);
        }

        public static bool Delete(Interface _interface)
        {
            return deleteInterface(_interface);
        }

        private static List<Interface> getInterfaces()
        {
            List<Interface> _interfaces = new List<Interface>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_INTERFACE;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            _interfaces.Add(new Interface(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getInterfaces()");
                }
            }

            return _interfaces;
        }

        private static List<Interface> getSingleInterface(Interface _interface)
        {
            List<Interface> _interfaces = new List<Interface>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_INTERFACE;
                command.Parameters.AddWithValue(InterfaceDAO.AT_ID, _interface.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            _interfaces.Add(new Interface(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleInterface(Interface Interface)", _interface.id.ToString());
                }
            }

            return _interfaces;
        }

        private static Interface postUpdateInterface(Interface _interface)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_INTERFACE;
                command.Parameters.AddWithValue(InterfaceDAO.AT_COMMUNICATION_ID, _interface.communicationId);
                command.Parameters.AddWithValue(InterfaceDAO.AT_CREDENTIAL_ID, _interface.credentialId);
                command.Parameters.AddWithValue(InterfaceDAO.AT_IP_ADDRESS, _interface.ipAddress);
                command.Parameters.AddWithValue(InterfaceDAO.AT_PORT, _interface.port);
                command.Parameters.AddWithValue(InterfaceDAO.AT_MAX_CONNECTIONS, _interface.maxConnections);
                command.Parameters.AddWithValue(InterfaceDAO.AT_ID, _interface.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            _interface.id = DAOUtility.GetData<int>(reader, InterfaceDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateInterface()", _interface.id.ToString());
                }
            }
            return _interface;
        }

        private static bool deleteInterface(Interface _interface)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_INTERFACE;
                command.Parameters.AddWithValue(InterfaceDAO.AT_ID, _interface.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            _interface.id = DAOUtility.GetData<int>(reader, InterfaceDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteInterface()", _interface.id.ToString());
                }
            }
            return true;
        }
    }
}
