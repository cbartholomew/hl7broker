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
    public class CredentialDAO
    {
        // REGULAR COLUMN NAMES
        public const string ID = "ID";
        public const string CREDENTIAL_TYPE_ID = "CREDENTIAL_TYPE_ID";
        public const string USERNAME = "USERNAME";
        public const string PASSWORD = "PASSWORD";

        // PARAMETERS
        public const string AT_ID = "@ID";
        public const string AT_CREDENTIAL_TYPE_ID = "@CREDENTIAL_TYPE_ID";
        public const string AT_USERNAME = "@USERNAME";
        public const string AT_PASSWORD = "@PASSWORD";

        // VIEW COLUMN NAMES
        public const string VIEW_CREDENTIAL_ID = "CREDENTIAL_ID";
        public const string VIEW_CREDENTIAL_USERNAME = "CREDENTIAL_USERNAME";
        public const string VIEW_CREDENTIAL_PASSWORD = "CREDENTIAL_PASSWORD";

        public static List<Credential> Get()
        {
            return getCredentials();
        }

        public static List<Credential> Get(Credential credential)
        {
            return getSingleCredential(credential);
        }

        public static Credential PostUpdate(Credential credential)
        {
            return postUpdateCredential(credential);
        }

        public static bool Delete(Credential credential)
        {
            return deleteCredential(credential);
        }

        private static List<Credential> getCredentials()
        {
            List<Credential> credentials = new List<Credential>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_ALL_CREDENTIAL;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            credentials.Add(new Credential(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getCredentials()");
                }
            }

            return credentials;
        }

        private static List<Credential> getSingleCredential(Credential credential)
        {
            List<Credential> credentials = new List<Credential>();
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.GET_SINGLE_CREDENTIAL_TYPE;
                command.Parameters.AddWithValue(CredentialDAO.AT_ID, credential.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            credentials.Add(new Credential(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "getSingleCredential(Credential Credential)", credential.id.ToString());
                }
            }

            return credentials;
        }

        private static Credential postUpdateCredential(Credential credential)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = das.PUT_CREDENTIAL;
                command.Parameters.AddWithValue(CredentialDAO.AT_CREDENTIAL_TYPE_ID, credential.credentialTypeId);
                command.Parameters.AddWithValue(CredentialDAO.AT_USERNAME, credential.username);
                command.Parameters.AddWithValue(CredentialDAO.AT_PASSWORD, credential.password);
                command.Parameters.AddWithValue(CredentialDAO.AT_ID, credential.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            credential.id = DAOUtility.GetData<int>(reader, CredentialDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "postUpdateCredential()", credential.id.ToString());
                }
            }
            return credential;
        }

        private static bool deleteCredential(Credential credential)
        {
            SysDataAccessCredential dac = DAOUtility.GetSysCredentials();
            DataAccess das = new DataAccess();

            using (SqlConnection conn = new SqlConnection(DAOUtility.GetConnectionString(dac)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = das.DELETE_CREDENTIAL;
                command.Parameters.AddWithValue(CredentialDAO.AT_ID, credential.id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            credential.id = DAOUtility.GetData<int>(reader, CredentialDAO.ID);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "DeleteCredential()", credential.id.ToString());
                }
            }
            return true;
        }
    }
}
