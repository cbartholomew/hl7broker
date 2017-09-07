using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Importer.Model;

namespace HL7Importer.Utility
{
    public class SectraUtility
    {

        public const string ROUTINE = "ROUT";

        public static string[] handleName(string name)
        {
            string[] arrName = null;
            try
            {
                // replace comma space
                name = (name.Contains(",")) ? name.Replace(",", " ") : name;

                // split person name emply blanks
                arrName = (name != "") ? name.Split(new string[] {" "},
                                                    StringSplitOptions.RemoveEmptyEntries)
                                                       : new string[] { "Unable to Retrieve Lastname", "Unable to Retrieve Firstname" };
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "handleName()");
            }

            return arrName;
        }

        public static void handleSourceOfReferral(WsExam wsExam)
        {
            wsExam.SourceOfReferralName = (wsExam.SourceOfReferralName == null || wsExam.SourceOfReferralName == "") 
                ? ROUTINE : wsExam.SourceOfReferralName;
        }
    }
}
