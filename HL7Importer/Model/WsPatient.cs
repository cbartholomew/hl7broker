using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSShieldsApps;
using HL7Importer.DAO;
using HL7Importer.Utility;
using System.Data;

namespace HL7Importer.Model
{
    public class WsPatient : Patient
    {
        // sectra returns full name not parsed name
        public string fullName { get; set; }

        // constants for name part indicies 
        public const int LASTNAME = 0;
        public const int FIRSTNAME = 1;
        public const int MIDDLENAME = 2;

        public WsPatient()
        { 
        
        }

        public WsPatient(IDataRecord reader)
        {
            fillDataReader(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.fullName = DAOUtility.GetData<string>(reader, SectraDAO.PATIENT_FULL_NAME);
            
            // parse the full name
            string[] arrParsedName = SectraUtility.handleName(this.fullName);

            // set the name
            this.LastName = arrParsedName[LASTNAME];
            this.FirstName = arrParsedName[FIRSTNAME];

            // does patient have middle name?
            if (arrParsedName.Length > 2)
            {
                this.MiddleName = arrParsedName[MIDDLENAME];
            }
            
            // get rest of properties
            this.MedicalRecordNo = DAOUtility.GetData<string>(reader, SectraDAO.PATIENT_MRN);
            this.DateOfBirth        = DAOUtility.GetData<DateTime>(reader, SectraDAO.PATIENT_DATE_OF_BIRTH);
        }
    }
}
