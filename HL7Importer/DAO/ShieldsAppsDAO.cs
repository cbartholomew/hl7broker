using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSShieldsApps;

namespace HL7Importer.DAO
{
    public class ShieldsAppsDAO
    {
        public static Exam GetExamByAccessionNo(WSShieldsApps.Exam exam)
        {
            using (ShieldsAppsClient client = new ShieldsAppsClient())
            {
                try
                {
                    exam = client.GetExamByAccessionNo(exam.AccessionNo);
                }
                catch (Exception ex)
                {
                    client.Close();
                }
            }
            return exam;
        }

        public static string PutExam(Exam exam)
        {
            WSShieldsApps.ENUMMESSAGE output
                = WSShieldsApps.ENUMMESSAGE.UNKNOWN;

            string exceptionText = "";

            using (ShieldsAppsClient client = new ShieldsAppsClient())
            {
                try
                {
                    exam.ExamId = client.InsertExamByHl7(out output, exam, 1);
                }
                catch (Exception ex)
                {
                    exceptionText = ex.ToString();
                    client.Close();
                }
            }

            return output.ToString() + " " + exceptionText + " " + exam.ExamId.ToString();
        }

        public static string PostReport(WSShieldsApps.Report report)
        {
            ENUMMESSAGE output
                = ENUMMESSAGE.UNKNOWN;

            string exceptionText = "";

            int reportId = -1;

            using (ShieldsAppsClient client = new ShieldsAppsClient())
            {
                try
                {
                    reportId = client.InsertReport(out output, report);
                }
                catch (Exception ex)
                {
                    exceptionText = ex.ToString();
                    client.Close();
                }
            }

            return output.ToString() + " " + exceptionText + " " + reportId.ToString();
        }

        public static string PostReport(WSShieldsApps.Report report, int addendumIteration)
        {
            ENUMMESSAGE output
                = ENUMMESSAGE.UNKNOWN;

            string exceptionText = "";

            int reportId = -1;

            using (ShieldsAppsClient client = new ShieldsAppsClient())
            {
                try
                {
                    reportId = client.InsertReport(out output, report);
                }
                catch (Exception ex)
                {
                    exceptionText = ex.ToString();
                    client.Close();
                }
            }

            return output.ToString() + " " + exceptionText + " " + reportId.ToString();
        }

        public static string PutDoctor(WSShieldsApps.Doctor doctor)
        {
            ENUMMESSAGE output
                = ENUMMESSAGE.UNKNOWN;

            string exceptionText = "";

            using (ShieldsAppsClient client = new ShieldsAppsClient())
            {
                try
                {
                    doctor.DoctorId = client.InsertUpdateDoctor(out output, doctor);
                }
                catch (Exception ex)
                {
                    client.Close();
                }
            }

            return output.ToString() + " " + exceptionText + " " + doctor.DoctorId.ToString();
        }
    }
}
