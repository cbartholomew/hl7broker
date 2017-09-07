using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HL7BrokerSuite.Service.Utility.Data
{
    public class FileStoreUtility : GenericUtility
    {

        public static bool AppendLog(string message,string logFileName, bool overwrite = false)
        {            
            try
            {
                string fileName = logFileName;

                if (overwrite)
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }                
                }

                if (!File.Exists(fileName))
                {
                    if (!FileStoreUtility.makeFile(fileName))
                    {
                        return false;
                    }
                }

                string output = LOG_TEMPLATE
                    .Replace("[DATE_TIME]",DateTime.Now.ToShortDateString())
                    .Replace("[MESSAGE]", message) + Environment.NewLine;

                File.AppendAllText(fileName,output);
            }
            catch (Exception exception)
            {                
                string parameters = message;
                ErrorLogger.LogError(exception, "AppendLocalWebLog(string message)", parameters);
                return false;
            }

            return true;
        }

        private static bool makeFile(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {                            
                    // make new stream
                    FileStream stream = File.Create(fileName);
                    // force close the stream
                    stream.Close();
                }
            }
            catch (Exception exception)
            {
                string parameters = fileName;
                ErrorLogger.LogError(exception, "MakeFile(string fileName)", parameters);
                return false;
            }

            return true;
        }

    }
}
