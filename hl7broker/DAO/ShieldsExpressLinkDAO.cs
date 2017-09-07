using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSShieldsApps;
using HL7Broker.Utility;

namespace HL7Broker.DAO
{
    
    public class ShieldsExpressLinkDAO
    {
        // WS OBJECTS
        public const string REFERRING       = "Referring";
        public const string EXAM            = "Exam";
        public const string LOCATION        = "Location";
        public const string ORGANIZATION    = "Organization";
        public const string PATIENT         = "Patient";
        public const string REPORT          = "Report";
        public const string RADIOLOGIST     = "Radiologist";
        
        // LOCATION PROPERTIES
        public const string PROPERTY_LOCATION_STREET_ONE = "LOCATION_STREET_ONE";
        public const string PROPERTY_LOCATION_STREET_TWO = "LOCATION_STREET_TWO";
        public const string PROPERTY_LOCATION_CITY       = "LOCATION_CITY";
        public const string PROPERTY_LOCATION_STATE      = "LOCATION_STATE";
        public const string PROPERTY_LOCATION_ZIP        = "LOCATION_ZIP";

        // PATIENT PROPERTIES
        public const string PROPERTY_PATIENT_FIRST_NAME = "PATIENT_FIRST_NAME";
        public const string PROPERTY_PATIENT_LAST_NAME = "PATIENT_LAST_NAME";
        public const string PROPERTY_PATIENT_MIDDLE_NAME = "PATIENT_MIDDLE_NAME";
        public const string PROPERTY_PATIENT_MEDICAL_RECORD_NO = "PATIENT_MEDICAL_RECORD_NO";
        public const string PROPERTY_PATIENT_DATE_OF_BIRTH = "PATIENT_DATE_OF_BIRTH";
        
        // ORGANIZATION PROPERTIES
        public const string PROPERTY_ORGANIZATION_NAME = "ORGANIZATION_NAME";

        // EXAM PROPERTIES
        public const string PROPERTY_EXAM_ACCESSION_NO   = "EXAM_ACCESSION_NO";
        public const string PROPERTY_EXAM_CODE_NAME      = "EXAM_CODE_NAME";
        public const string PROPERTY_EXAM_TYPE_NAME      = "EXAM_TYPE_NAME";
        public const string PROPERTY_EXAM_SIDE_NAME      = "EXAM_SIDE_NAME";
        public const string PROPERTY_EXAM_STATUS_NAME    = "EXAM_STATUS_NAME";
        public const string PROPERTY_EXAM_DATE           = "EXAM_DATE";
        public const string PROPERTY_EXAM_DATE_REQUESTED = "EXAM_DATE_REQUESTED";
        public const string PROPERTY_SOURCE_OF_REFERRAL  = "SOURCE_OF_REFERRAL";

        // RADIOLOGIST PROPERTIES
        public const string PROPERTY_RADIOLOGIST_NPI         ="RADIOLOGIST_NPI";
        public const string PROPERTY_RADIOLOGIST_FIRST_NAME  ="RADIOLOGIST_FIRST_NAME";
        public const string PROPERTY_RADIOLOGIST_LAST_NAME   ="RADIOLOGIST_LAST_NAME";
        public const string PROPERTY_RADIOLOGIST_MIDDLE_NAME ="RADIOLOGIST_MIDDLE_NAME";

         // REFERRING PROPERTIES
        public const string PROPERTY_REFERRING_NPI           ="REFERRING_NPI";
        public const string PROPERTY_REFERRING_LAST_NAME     ="REFERRING_LAST_NAME";
        public const string PROPERTY_REFERRING_FIRST_NAME    ="REFERRING_FIRST_NAME";
        public const string PROPERTY_REFERRING_MIDDLE_NAME   ="REFERRING_MIDDLE_NAME";
        public const string PROPERTY_REFERRING_SUFFIX        ="REFERRING_SUFFIX";

        // REPORT PROPERTIES
        public const string PROPERTY_REPORT_OBSERVATION_DATE = "REPORT_OBSERVATION_DATE";
        public const string PROPERTY_REPORT_TEXT = "REPORT_TEXT";

        // RETURN VARIABLES
        public const string EXAM_RETURN_TEXT    = "Exam Inserted Result";
        public const string DOCTOR_UPDATE       = "Doctor Insert/Update Result";
        public const string EXAM_UPDATE         = "Exam Updated Result";
        public const string REPORT_INSERTED     = "Report Inserted/Update Result";
        public const string ADDENDUM_INSERTED   = "Addendum Inserted/Updated Result";
        public const string ADDENDUM_FOUND      = "Addendum(s) are Found, Inserting Addendum(s) only: ";
        public const string EXCEPTION_FOUND     = " Exception Found: ";

        public const int ENTERPRISE_DEFAULT = 1;

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

            int examId = -1;

            string exceptionText = GenericUtility.BLANK;

            using (ShieldsAppsClient client = new ShieldsAppsClient())
            {
                try
                {
                    examId = client.InsertExamByHl7(out output, exam, ENTERPRISE_DEFAULT);
                }
                catch (Exception ex)
                {
                    exceptionText = EXCEPTION_FOUND + ex.ToString();
                    client.Close();
                }
            }

            return GenericUtility.GetHL7TextForServiceMessage(EXAM_UPDATE, 
                                                             (exceptionText == GenericUtility.BLANK) ? 
                                                                output.ToString() 
                                                                : output.ToString() + exceptionText,
                                                                examId.ToString(),
                                                                ENTERPRISE_DEFAULT);
        }

        public static string PostReport(WSShieldsApps.Report report)
        {
            ENUMMESSAGE output
                = ENUMMESSAGE.UNKNOWN;

            string exceptionText = GenericUtility.BLANK;

            int reportId = -1;

            using (ShieldsAppsClient client = new ShieldsAppsClient())
            {
                try
                {
                    reportId = client.InsertReport(out output, report);
                }
                catch (Exception ex)
                {
                    exceptionText = EXCEPTION_FOUND + ex.ToString();
                    client.Close();
                }
            }

            return GenericUtility.GetHL7TextForServiceMessage(REPORT_INSERTED,
                                                      (exceptionText == GenericUtility.BLANK) ?
                                                               output.ToString()
                                                               : output.ToString() + exceptionText,
                                                               reportId.ToString(),
                                                               ENTERPRISE_DEFAULT);          
        }

        public static string PostReport(WSShieldsApps.Report report, int addendumIteration)
        {
            ENUMMESSAGE output
                = ENUMMESSAGE.UNKNOWN;

            string exceptionText = GenericUtility.BLANK;

            int reportId = -1;

            using (ShieldsAppsClient client = new ShieldsAppsClient())
            {
                try
                {
                    reportId = client.InsertReport(out output, report);
                }
                catch (Exception ex)
                {
                    exceptionText = EXCEPTION_FOUND + ex.ToString();
                    client.Close();
                }
            }

            return GenericUtility.GetHL7TextForServiceMessage(ADDENDUM_INSERTED + "&" + addendumIteration,
                                                                    (exceptionText == GenericUtility.BLANK) ?
                                                                    output.ToString()
                                                                    : output.ToString() + exceptionText,
                                                                    reportId.ToString(),
                                                                    ENTERPRISE_DEFAULT);
        }

        public static string PutDoctor(WSShieldsApps.Doctor doctor)
        {
            ENUMMESSAGE output
                = ENUMMESSAGE.UNKNOWN;

            string exceptionText = GenericUtility.BLANK;

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

            return GenericUtility.GetHL7TextForServiceMessage(DOCTOR_UPDATE,
                                                         (exceptionText == GenericUtility.BLANK) ?
                                                         output.ToString()
                                                         : output.ToString() + exceptionText,
                                                         doctor.DoctorId.ToString(),
                                                         ENTERPRISE_DEFAULT);                            
        }

    }                      
}                          
                           
                           