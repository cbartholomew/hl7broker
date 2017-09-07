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
    public class CredentialTypeDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string NAME = "NAME";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_NAME = "@NAME";
        public static List<CredentialType> Get()
        {
            return getCredentialTypes();
        }

        public static List<CredentialType> Get(CredentialType credentialType)
        {
            return getSingleCredentialType(credentialType);
        }

        public static CredentialType PostUpdate(CredentialType credentialType)
        {
            return postUpdateCredentialType(credentialType);
        }

        public static bool Delete(CredentialType credentialType)
        {
            return deleteCredentialType(credentialType);
        }

        private static List<CredentialType> getCredentialTypes()
        {
            List<CredentialType> credentialTypes = new List<CredentialType>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_CREDENTIAL_TYPE;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            credentialTypes.Add(new CredentialType(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getCredentialTypes()");
                }
            }

            return credentialTypes;
        }

        private static List<CredentialType> getSingleCredentialType(CredentialType credentialType)
        {
            List<CredentialType> credentialTypes = new List<CredentialType>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_CREDENTIAL_TYPE;
                command.Parameters.AddWithValue(CredentialTypeDAO.AT_ID, credentialType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            credentialTypes.Add(new CredentialType(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleCredentialType(CredentialType CredentialType)", credentialType.id.ToString());
                }
            }

            return credentialTypes;
        }

        private static CredentialType postUpdateCredentialType(CredentialType credentialType)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_CREDENTIAL_TYPE;
                command.Parameters.AddWithValue(CredentialTypeDAO.AT_NAME, credentialType.name);
                command.Parameters.AddWithValue(CredentialTypeDAO.AT_ID, credentialType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            credentialType.id = DAOUtility.GetData<int>(reader, CredentialTypeDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateCredentialType()", credentialType.id.ToString());
                }
            }
            return credentialType;
        }

        private static bool deleteCredentialType(CredentialType credentialType)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_CREDENTIAL_TYPE;
                command.Parameters.AddWithValue(CredentialTypeDAO.AT_ID, credentialType.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            credentialType.id = DAOUtility.GetData<int>(reader, CredentialTypeDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteCredentialType()", credentialType.id.ToString());
                }
            }
            return true;
        }
    }
}
