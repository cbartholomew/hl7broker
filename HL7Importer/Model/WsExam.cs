using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSShieldsApps;
using HL7Importer.DAO;
using HL7Importer.Utility;

namespace HL7Importer.Model
{
    public class WsExam : Exam
    {
        public int examId { get; set; }
        public int patientNo { get; set; }
        public int requestedBy { get; set; }
        public string accessionNo { get; set; }

        public WsExam()
        { 
        
        }

        public WsExam(IDataRecord reader)
        {
            fillDataReader(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.examId         = DAOUtility.GetData<int>(reader, SectraDAO.EXAM_ID);
            this.patientNo      = DAOUtility.GetData<int>(reader, SectraDAO.EXAM_PATIENT_NO);
            this.requestedBy    = DAOUtility.GetData<int>(reader, SectraDAO.EXAM_REQUESTED_BY);
            this.accessionNo    = DAOUtility.GetData<string>(reader, SectraDAO.EXAM_ACCESSION_NO);
            this.AccessionNo    = DAOUtility.GetData<string>(reader, SectraDAO.EXAM_ACCESSION_NO);
            this.ExamCodeName   = DAOUtility.GetData<string>(reader, SectraDAO.EXAM_CODE_NAME);
            this.ExamTypeName   = DAOUtility.GetData<string>(reader, SectraDAO.EXAM_TYPE_NAME);
            this.ExamStatusName = DAOUtility.GetData<string>(reader, SectraDAO.EXAM_STATUS_NAME);
            this.ExamSideName   = DAOUtility.GetData<string>(reader, SectraDAO.EXAM_SIDE);
            this.ExamDate       = DAOUtility.GetData<DateTime>(reader, SectraDAO.EXAM_DATE);
            this.DateRequested  = DAOUtility.GetData<DateTime>(reader, SectraDAO.EXAM_DATE_REQUESTED);
            this.SourceOfReferralName  = DAOUtility.GetData<string>(reader, SectraDAO.SOURCE_OF_REFERRAL_NAME);

            // handle source of referral name for older exams
            SectraUtility.handleSourceOfReferral(this);

        }
    }
}
