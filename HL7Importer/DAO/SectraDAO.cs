using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Importer.Model;
using System.Data.SqlClient;
using System.Data;

namespace HL7Importer.DAO
{
    public class SectraDAO
    {
        public const string CONNNECTION_STRING = "Data Source=SHCSCTDB;Initial Catalog=RLOGIC;User Id=ReadingRadPooledUser;Password=ReadingRadPooledUser";

        // EXAM COLUMNS
        public const string EXAM_ID           ="EXAM_ID";               
        public const string EXAM_PATIENT_NO   ="EXAM_PATIENT_NO";
        public const string EXAM_REQUESTED_BY ="EXAM_REQUESTED_BY";
        public const string EXAM_ACCESSION_NO ="EXAM_ACCESSION_NO";
        public const string EXAM_CODE_NAME    ="EXAM_CODE_NAME"; 
        public const string EXAM_TYPE_NAME    ="EXAM_TYPE_NAME"; 
        public const string EXAM_STATUS_NAME  ="EXAM_STATUS_NAME";
        public const string EXAM_DATE_REQUESTED = "EXAM_DATE_REQUESTED";
        public const string EXAM_DATE = "EXAM_DATE";
        public const string EXAM_SIDE = "EXAM_SIDE";

        // SOURCE OF REFERRL COLUMNS
        public const string SOURCE_OF_REFERRAL_NAME = "SOURCE_OF_REFERRAL_NAME";

        // PATIENT COLUMNS
        public const string PATIENT_FULL_NAME       = "PATIENT_FULL_NAME";
        public const string PATIENT_MRN             = "PATIENT_MRN";
        public const string PATIENT_DATE_OF_BIRTH   = "PATIENT_DATE_OF_BIRTH";

        // ORGANIZATION COLUMNS       
        public const string ORGANIZATION_NAME = "ORGANIZATION_NAME"; 

        // REPORT COLUMNS
        public const string REPORT_EXAMID             ="EXAMID";         
        public const string REPORT_INTERP_NO          ="INTERP_NO"; 
        public const string REPORT_INTERPRETATION     ="INTERPRETATION"; 
        public const string REPORT_RADIOLOGIST        ="RADIOLOGIST"; 
        public const string REPORT_INTERPTIME         ="INTERPTIME"; 
        public const string REPORT_SIGNEDDATE         ="SIGNEDDATE"; 
        public const string REPORT_SIGNEDTIME         ="SIGNEDTIME";
        public const string REPORT_DICTATEDBY         = "DICTATEDBY";
        public const string REPORT_HTML_HEADER = "<HTML><HEAD><TITLE>REPORT</TITLE></HEAD>";
        public const string REPORT_HTML_FOOTER = "</HTML>";

        // USER COLUMNS
        public const string User_No             ="User_No";
        public const string UserCode            ="UserCode";
        public const string Name                ="Name";
        public const string License_No          ="License_No";
        public const string License_No2         ="License_No2";
        public const string Phone               ="Phone";
        public const string FaxNumber           ="FaxNumber";
        public const string Street1             ="Street1";
        public const string Street2             ="Street2";
        public const string City                ="City";
        public const string State               ="State";
        public const string Zip                 ="Zip";
        public const string NPI                 ="NPI";
        public const string TypeOfUser          ="TypeOfUser";
        public const string SPECIALITY_DESC     ="SPECIALITY_DESC";
        public const string Speciality_No       ="Speciality_No";
        public const string SpecialityCode      = "SpecialityCode";

        // NEW VIEW FOR THIS ONE BECAUSE OLDER EXAMS DONT HAVE THE CORRECT LINKS FOR THE ROBUST VIEW, WHICH CAUSES NO RETURNS
        public const string EXAM_PATIENT_REFERRING_ORGANIZATION_QUERY = "SELECT * FROM [dbo].[vShields_HL7InterfaceBroker_GetExamPatientImportSpecial] WHERE EXAM_ACCESSION_NO = @ACCESSION_NO";

        // THIS WILL GET THE DOCTOR LOCATION INFORMATION BY USER_NO
        public const string DOCTOR_LOCATION_QUERY = "SELECT * FROM [dbo].[vShields_HL7InterfaceBroker_GetDoctorsInformationByLicNo] WHERE USER_NO = @USER_NO";

        // This stored procedure will contain info about that report and radiologist
        public const string REPORT_RADIOLOGIST_QUERY = "[dbo].[Shields_WSAcceleratorApps_GetPatientReportByExamID]";

        // sorry so long
        public const string REPORT_CHECK_GROUPING = "select examid from rlogic.examgm where examgroupno = (select examgroupno from rlogic.examgm where examid = @EXAMID)";

        public static List<WsReport> GetSectraReportDetail(int examId)
        {
            List<WsReport> reports = new List<WsReport>();

            SqlCommand sqlCommand = new SqlCommand();
            using (SqlConnection sqlConnection = new SqlConnection(CONNNECTION_STRING))
            {
                try
                {
                    // open 
                    sqlConnection.Open();
                    // set command attributes
                    sqlCommand.CommandText = REPORT_RADIOLOGIST_QUERY;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Connection = sqlConnection;

                    // add the parameter to the view
                    sqlCommand.Parameters.AddWithValue("@EXAMID", examId);

                    // run query
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            reports.Add(new WsReport(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetSectraReportDetail()");
                }
            }

            return reports;
        }

        public static Sectra GetSectraDoctorLocationDetail(Sectra patientObject, int userNo, bool isRadiologist = false)
        {
            SqlCommand sqlCommand = new SqlCommand();
            using (SqlConnection sqlConnection = new SqlConnection(CONNNECTION_STRING))
            {
                try
                {
                    // open 
                    sqlConnection.Open();
                    // set command attributes
                    sqlCommand.CommandText = DOCTOR_LOCATION_QUERY;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = sqlConnection;

                    // add the parameter to the view
                    sqlCommand.Parameters.AddWithValue("@USER_NO", userNo);

                    // run query
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // if it's not a radiologist, then it's a referring - get the location as well
                            if (!isRadiologist)
                            {
                                // populate objects
                                patientObject.referringDoctor = new WsDoctor(reader);
                                patientObject.wsReferringDoctorLocation = new WsLocation(reader);
                            }
                            else
                            {
                                patientObject.radiologistDoctor = new WsDoctor(reader);
                            }                          
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetSectraDoctorLocationDetail()");
                }
            }

            return patientObject;
        }

        // returns a sectra object that contains the exam, patient, and organization details
        public static Sectra GetSectraExamPatientOrganizationDetail(Sectra patientObject, string accessionNo) 
        {
            SqlCommand sqlCommand = new SqlCommand();
            using (SqlConnection sqlConnection = new SqlConnection(CONNNECTION_STRING))
            {
                try
                {
                    // open 
                    sqlConnection.Open();
                    // set command attributes
                    sqlCommand.CommandText = EXAM_PATIENT_REFERRING_ORGANIZATION_QUERY;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = sqlConnection;

                    // add the parameter to the view
                    sqlCommand.Parameters.AddWithValue("@ACCESSION_NO", accessionNo);

                    // run query
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // populate objects
                            patientObject.wsExam = new WsExam(reader);
                            patientObject.wsPatient = new WsPatient(reader);
                            patientObject.wsOrganization = new WsOrganization(reader);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetSectraExamPatientOrganizationDetail()");
                }
            }

            return patientObject;
        }

        // returns a list of accessions that are grouped to this exam, which is not itself
        public static List<int> GetGroupedExamsSectra(int examId)
        {
            List<int> examids = new List<int>();
            SqlCommand sqlCommand = new SqlCommand();
            using (SqlConnection sqlConnection = new SqlConnection(CONNNECTION_STRING))
            {
                try
                {
                    // open 
                    sqlConnection.Open();
                    // set command attributes
                    sqlCommand.CommandText = REPORT_CHECK_GROUPING;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = sqlConnection;

                    // add the parameter to the view
                    sqlCommand.Parameters.AddWithValue("@EXAMID", examId);

                    // run query
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int temp = Convert.ToInt32(reader["examid"]);
                            if(temp != examId)
                                // populate objects
                                examids.Add(temp);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetGroupedExamsSectra()");
                }
            }

            return examids;
            
        }
    }
}
