using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Importer.Utility
{
    public class DAOUtility
    {
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
                ErrorLogger.LogError(e, "getData(IDataRecord reader, string columnName)", reader + "|" + columnName);
                throw e;
            }
        }
    }
}
