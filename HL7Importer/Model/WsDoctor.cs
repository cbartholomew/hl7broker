using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using WSShieldsApps;
using HL7Importer.DAO;
using HL7Importer.Utility;
using System.Data;

namespace HL7Importer.Model
{
    public class WsDoctor : Doctor
    {
        public string fullName { get; set; }
        // constants for name part indicies 
        public const int LASTNAME = 0;
        public const int FIRSTNAME = 1;
        public const int MIDDLENAME = 2;

        public WsDoctor()
        { 
        
        }

        public WsDoctor(IDataRecord reader)
        {
            fillDataReader(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {       
            this.fullName = DAOUtility.GetData<string>(reader, SectraDAO.Name);

            string[] arrParsedName = SectraUtility.handleName(this.fullName);

            // set the name
            this.LastName = arrParsedName[LASTNAME];
            this.FirstName = arrParsedName[FIRSTNAME];

            // does patient have middle name?
            if (arrParsedName.Length > 2)
            {
                this.MiddleName = arrParsedName[MIDDLENAME];
            }

            this.Npi  = DAOUtility.GetData<string>(reader, SectraDAO.NPI);
            this.Type = DAOUtility.GetData<string>(reader, SectraDAO.TypeOfUser);
            this.EnterpriseId = 1;
        }
    }
}
