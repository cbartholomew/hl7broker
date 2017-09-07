using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Settings;

namespace HL7BrokerSuite.Utility
{
    public class DAOUtility : GenericUtility
    {
        public enum SERVICE_QUERY_TYPE
        {
            INSERT,
            UPDATE,
            SELECT,
            UNKNOWN
        }

        public static SqlDbType GetSqlDataType(string dataType) 
        {
            SqlDbType sqlDataType;
            switch (dataType)
            {
                case DATA_TYPE_INTEGER:
                    sqlDataType = SqlDbType.BigInt;
                    break;
                case DATA_TYPE_TEXT:
                    sqlDataType = SqlDbType.Text;
                    break;
                case DATA_TYPE_CURRENT_DATE:
                    sqlDataType = SqlDbType.DateTime;
                    break;
                default:
                    sqlDataType = SqlDbType.Text;
                    break;
            }
            return sqlDataType;
        }

        public static string GetBaseQuery(SERVICE_QUERY_TYPE queryType)
        {
            // new data access
            DataAccess settings 
                = new DataAccess();

            string queryValue = BLANK;

            switch (queryType)
            {
                case SERVICE_QUERY_TYPE.INSERT:
                    queryValue = settings.SERVICE_BASE_QUERY_INSERT;
                    break;
                case SERVICE_QUERY_TYPE.UPDATE:
                    break;
                case SERVICE_QUERY_TYPE.SELECT:
                    break;
                default:
                    break;
            }

            return queryValue;
        }

        public static string GetStatement(SERVICE_QUERY_TYPE queryType,
                                          string baseCommandText,
                                          string table,
                                          List<string> columns,
                                          List<string> values)
        {
            string queryStatement = BLANK;

            switch (queryType)
            {
                case SERVICE_QUERY_TYPE.INSERT:
                    queryStatement = getInsertStatement(baseCommandText, 
                                                        table, 
                                                        columns, 
                                                        values);
                    break;
                case SERVICE_QUERY_TYPE.UPDATE:
                    break;
                case SERVICE_QUERY_TYPE.SELECT:
                    break;
                case SERVICE_QUERY_TYPE.UNKNOWN:
                    break;
                default:
                    break;
            }

            return queryStatement;
        }

        private static string getInsertStatement(string baseCommandText, 
                                                 string table, 
                                                 List<string> columns,
                                                 List<string> values)
        {
            try
            {
                // replace table
                baseCommandText = baseCommandText.Replace(DAOUtility.TABLE, 
                                                            table);

                // replace columns
                baseCommandText = baseCommandText.Replace(DAOUtility.COLUMNS, 
                                                            String.Join(COMMA_STR, columns.ToArray()));

                // replace values
                baseCommandText = baseCommandText.Replace(DAOUtility.COLUMN_VALUES, 
                                                            String.Join(COMMA_STR, values.ToArray()));


            }
            catch (Exception ex)
            {
                throw;
            }

            return baseCommandText;
        }

        public static T GetData<T>(IDataRecord reader, 
                                   string columnName, 
                                   bool ignoreException = true)
        {
            try
            {
                return (T)Convert.ChangeType(((reader[columnName] == DBNull.Value) ? default(T) : reader[columnName]), typeof(T));
            }
            catch (Exception e)
            {
                if (ignoreException)
                {
                    return default(T);
                }
                ErrorLogger.LogError(e, "getData(IDataRecord reader, string columnName)", reader + PIPE.ToString() + columnName);
                throw e;
            }
        }

        public static SysDataAccessCredential GetSysCredentials()
        {
            DataAccessSysCredentials dataAccessSysCredentials = new DataAccessSysCredentials();
            return new SysDataAccessCredential()
            {
                username = dataAccessSysCredentials.USERNAME,
                password = dataAccessSysCredentials.PASSWORD,
                server = dataAccessSysCredentials.SERVER,
                database = dataAccessSysCredentials.DATABASE
            };
        }

        public static string GetConnectionString(SysDataAccessCredential credential)
        {
            DataAccess dataAccessSettings = new DataAccess();

            string connection_string = dataAccessSettings.BASE_CONNECTION_STRING;
            //// build custom connectionstring
            connection_string = connection_string.Replace("[SERVER]", credential.server);
            connection_string = connection_string.Replace("[DATABASE]", credential.database);
            connection_string = connection_string.Replace("[USERNAME]", credential.username);
            connection_string = connection_string.Replace("[PASSWORD]", credential.password);
            return connection_string;
        }

        public static string GetConnectionString(Credential credential, 
                                                 DatabaseInstance databaseInstance,
                                                 bool useIpAddress = false)
        {
            DataAccess dataAccessSettings = new DataAccess();
            string connection_string = dataAccessSettings.BASE_CONNECTION_STRING;

            // provide the option to use ip address instead of server name
            string server = (!useIpAddress) ? databaseInstance.server 
                                            : databaseInstance.ipAddress;

            // build custom connectionstring
            connection_string = connection_string.Replace("[SERVER]",   server);
            connection_string = connection_string.Replace("[DATABASE]", databaseInstance.name);
            connection_string = connection_string.Replace("[USERNAME]", credential.username);
            connection_string = connection_string.Replace("[PASSWORD]", credential.password);
            return connection_string;
        }


    }
}
