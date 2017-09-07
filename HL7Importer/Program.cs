using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Importer.DAO;
using HL7Importer.Model;
using HL7Importer.Utility;
using WSShieldsApps;
using System.IO;
using System.Threading;


namespace HL7Importer
{
    class Program
    {
        public static Dictionary<string, string> textDisplay = new Dictionary<string, string>()
        { 
                {"Welcome","Welcome to HL7Importer. This application is meant to import any accession or batches of accessions from SELv1 into SELv2. You will provide your own query. Let's get started\n"},
                {"Menu", "Please choose an option below:\n 1. Import new report by custom query\n 2. Force Import of existing report by query\n 3. Quit"},  
                {"Prompt",">>"},
                {"Error","Invalid Query - Press any key to quit\n"},
                {"Query","Please provide a query to obtain your data.\n Your format should look like the following:\n SELECT <COLUMN> AS ACCESSIONNO FROM <TABLE> WHERE <COLUMN> <CONDITION> <EXP>\n Example: SELECT ID AS ACCESSIONNO FROM SCANS WHERE ID = '1010200101.1' (Or Date Range)\n"}
        };


        public const string FILE_NAME = "ImportLog.txt";
        /*
            1.	Get all $accessions from dbo.seloru
            2.	FE $accession in $accessions
                a.	IF $accession IS NOT in RIS_DB 
                    i.	Call SHCSCTDB Get Exam Information, Get Referring Provider Information, 
                        Get Radiologist Information, and Report Information. 
                    ii.	Populate Exam Object
                    iii.Populate Doctor Object
                    iv.	Populate Patient Object
                    v.	Populate Report Object
                    vi.	InsertUpdateExam/InsertUpdateDoctor/InsertUpdateReport/etc
                b.	ELSE IF $accession IS IN RIS_DB
                    i.	IF $accession Exam Status IS NOT IN REPORTED //because this is seloru accession, 
                        then it should be reported. If it’s not reported in sel, then it must have been an 
                        exam edit that didn’t update our system.
            1.	Do 2.a.i.
                ii.	ELSE
                    1.	Skip Accession

         */

        static void Main(string[] args)
        {
            DisplayMenu();
        }

        private static void DisplayMenu()
        {

            Console.WriteLine(textDisplay["Welcome"]);
            Console.WriteLine(textDisplay["Query"]);
            Console.Write(textDisplay["Prompt"]);

            string userValue = Console.ReadLine().ToString();

            userValue = userValue.ToString().Replace(textDisplay["Prompt"], "");

            if (String.IsNullOrEmpty(userValue))
            {
                Console.Write(textDisplay["Error"]);
                Console.Read();
                System.Environment.Exit(1);
            }

            // true == force report even if the exam status is 4
            // false == if the exam status is 4 - skip report
            Begin(userValue, true);
        }

        private static void Begin(string Query, bool forceReport = false)
        {
            int totalCount = 0;
            int currentIndex = 0;
            int percentComplete = 0;
            DateTime startDate = DateTime.Now;

            // get all sel oru exams to date
            List<SelOru> accessions = SelOruDAO.GetAllAccessionsFromSELORU(Query);

            totalCount = accessions.Count();

            Console.WriteLine("Starting! {0}",startDate.ToString());

            // set the MOD from here
            //var options = new ParallelOptions
            //{
            //    MaxDegreeOfParallelism = 2
            //};
            // parallel For each 
            //Parallel.ForEach(accessions, options, oldSELExam =>
            // for each exam do the following
            accessions.ForEach(delegate(SelOru oldSELExam)
            {
                currentIndex++;
                percentComplete = (int)Math.Round((double)(100 * currentIndex) / totalCount);
                Console.Write("\rProgress: {0}%", percentComplete);

                // check ris db for exam
                Exam tempExam = ShieldsAppsDAO.GetExamByAccessionNo(new Exam()
                {
                    AccessionNo = oldSELExam.accessionNo
                });

                // is the exam in ris db?
                if (tempExam.ExamId == 0)
                {
                    // build sectra exam object
                    DoSectraBuild(oldSELExam);
                }
                else
                {
                    // REPORT STATUS FOR RIS_DB IS 4
                    if (tempExam.ExamStatusId != 4)
                    {
                        // before inserting into db, i'll do another check for the insert
                        DoSectraBuild(oldSELExam);
                    }
                    else if (tempExam.ExamStatusId == 4 && forceReport)
                    {
                        DoSectraBuild(oldSELExam);
                    }
                    else
                    {
                        return;
                    }
                }
         
            });
            Console.WriteLine("\nDone! Press any key to continue! {0}", DateTime.Now.ToString());
            Console.Read();
            Console.Clear();

        }
        private static void DoSectraBuild(SelOru oldSELExam)
        {
            // make new sectra object
            Sectra patientObj = new Sectra();

            // begin by populatig the main exam/patient/org objects
            SectraDAO.GetSectraExamPatientOrganizationDetail(patientObj, oldSELExam.accessionNo);

            // gather the doctor info
            SectraDAO.GetSectraDoctorLocationDetail(patientObj, patientObj.wsExam.requestedBy);

            // gather the report
            List<WsReport> reports = SectraDAO.GetSectraReportDetail(patientObj.wsExam.examId);

            // let's see if this is a grouped account
            if (reports.Count == 0)
            {
                List<int> examids = SectraDAO.GetGroupedExamsSectra(patientObj.wsExam.examId);

                // if there are grouped exams, search for correct grouped report
                if (examids.Count > 0)
                {
                    foreach (int examid_temp in examids)
                    {
                        reports = SectraDAO.GetSectraReportDetail(examid_temp);

                        // we found the grouping w/ the reports
                        if (reports.Count > 0)
                            break;
                    }
                }

            }

            // get the first and only report in the list - we handle addendums sperately
            if (reports.Count != 0) 
            {
                patientObj.wsReport = reports[0];
            }         

            // set the radiologist if you have a report
            if (reports.Count != 0)
            {
                SectraDAO.GetSectraDoctorLocationDetail(patientObj, patientObj.wsReport.radiologistUserNo, true);
            }

            // pass the patient object and reports for inserting
            DoInsertUpdate(patientObj, reports);
        }

        private static void DoInsertUpdate(Sectra sectraPatient, List<WsReport> Reports)
        {
            StringBuilder serviceMessage = new StringBuilder();

            // make SEL exam object
            WSShieldsApps.Exam exam
                = new WSShieldsApps.Exam();
            WSShieldsApps.Location location
                = new WSShieldsApps.Location();
            WSShieldsApps.Patient patient
                = new WSShieldsApps.Patient();
            WSShieldsApps.Organization organization
                = new WSShieldsApps.Organization();
            WSShieldsApps.Doctor referring
                = new WSShieldsApps.Doctor();
            // make SEL report object
            WSShieldsApps.Report report
                = new WSShieldsApps.Report();
            WSShieldsApps.Doctor radiologist
                = new WSShieldsApps.Doctor();

            // even though we use inhertiance, it doesn't appear the WS can serialize the object
            setExam(exam, sectraPatient.wsExam);
            setLocation(location, sectraPatient.wsReferringDoctorLocation);
            setPatient(patient, sectraPatient.wsPatient);                       
            setDoctor(radiologist,sectraPatient.radiologistDoctor);
            setDoctor(referring, sectraPatient.referringDoctor);
            setReport(report, sectraPatient.wsReport);
            setOrganization(organization, sectraPatient.wsOrganization);

            serviceMessage.AppendLine("ACCESSION: " + exam.AccessionNo);

            if (referring.Npi == "" || referring.Npi == null)
            {
                // apply fake npi
                referring.Npi = "9999999998";
                referring.FirstName = "Doctor";
                referring.LastName = "Exception";
            }

            if (radiologist.Npi == "" || radiologist.Npi == null)
            {
                // apply fake npi
                radiologist.EnterpriseId = 1;
                radiologist.Npi = "9999999999";
                radiologist.FirstName = "Radiologist";
                radiologist.LastName = "Exception";
            }

            // assign the correct attributes
            exam.Location = location;
            exam.Doctor = referring;
            exam.Organization = organization;
            exam.Patient = patient;

            // make a new exam object
            WSShieldsApps.Exam exam2Object = new WSShieldsApps.Exam();

            // pass the accession so we don't override
            exam2Object.AccessionNo = exam.AccessionNo;

            // get the updated exam id only
            exam.ExamId = ShieldsAppsDAO.GetExamByAccessionNo(exam2Object).ExamId;

            // put radiologist
            serviceMessage.AppendLine("Radiologist: " + ShieldsAppsDAO.PutDoctor(radiologist));

            // put exam
            serviceMessage.AppendLine("Exam: " + ShieldsAppsDAO.PutExam(exam));

            // post orignal report, but make sure you update the exam id and rad id
            report.ExamId = exam.ExamId;
            report.RadiologistId = radiologist.DoctorId;           

            if(Reports.Count > 0)
                serviceMessage.AppendLine("Original Report:" + ShieldsAppsDAO.PostReport(report));

            // if there is more than 1 report, then there is an addendum we need to get
            if(Reports.Count > 1)
            {
                // for each report text starting at "1" (0 == orignal report) get all addendums after
                for (int reportIteration = 1, n = Reports.Count; 
                    reportIteration < n; 
                    reportIteration++)
                {
                    // set the addendum flag
                    report.IsAddendum = true;

                    // re-define report text and observation date
                    report.ReportText = Reports[reportIteration].ReportText;

                    // handle inserting of addendum report 
                    serviceMessage.AppendLine("Report Addendum " + reportIteration.ToString() + ":" + ShieldsAppsDAO.PostReport(report, reportIteration));
                }
            }
        }

        private static void setDoctor(Doctor doctor, WsDoctor wsDoctor)
        {
            doctor.FirstName = wsDoctor.FirstName;
            doctor.LastName = wsDoctor.LastName;
            doctor.MiddleName = wsDoctor.MiddleName;
            doctor.Type = wsDoctor.Type;
            doctor.Npi = wsDoctor.Npi;
            doctor.EnterpriseId = wsDoctor.EnterpriseId;
        }

        private static void setOrganization(Organization organization, WsOrganization wsOrganization)
        {
            organization.OrganizationName = wsOrganization.OrganizationName;
        }

        private static void setExam(Exam exam, WsExam wsExam)
        {
            exam.AccessionNo            =  wsExam.AccessionNo;         
            exam.ExamCodeName           =  wsExam.ExamCodeName;        
            exam.ExamTypeName           =  wsExam.ExamTypeName;        
            exam.ExamStatusName         =  wsExam.ExamStatusName;      
            exam.ExamSideName           =  wsExam.ExamSideName;        
            exam.ExamDate               =  wsExam.ExamDate;            
            exam.DateRequested          =  wsExam.DateRequested;
            exam.SourceOfReferralName = wsExam.SourceOfReferralName;
        }

        private static void setLocation(Location location, WsLocation wsLocation)
        { 
            location.StreetOne      = wsLocation.StreetOne;   
            location.StreetTwo      = wsLocation.StreetTwo;   
            location.City           = wsLocation.City;        
            location.State          = wsLocation.State;       
            location.ZipCode        = wsLocation.ZipCode;
            location.EnterpriseId   = wsLocation.EnterpriseId;
        }

        private static void setReport(Report report, WsReport wsReport)
        {
            // handle faulty date instruction
            report.ObservationDate = (wsReport.ObservationDate == Convert.ToDateTime("1/1/0001 12:00:00 AM")) ? DateTime.Now : report.ObservationDate;
            report.ReportText = wsReport.ReportText;        
        }

        private static void setPatient(Patient patient, WsPatient wsPatient)
        { 
           patient.LastName         = wsPatient.LastName;       
           patient.FirstName        = wsPatient.FirstName;      
           patient.MiddleName       = wsPatient.MiddleName;     
           patient.MedicalRecordNo  = wsPatient.MedicalRecordNo;
           patient.DateOfBirth = wsPatient.DateOfBirth;
           
        }

    }
}
