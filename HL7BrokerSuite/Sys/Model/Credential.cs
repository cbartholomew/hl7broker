using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Utility;

namespace HL7BrokerSuite.Sys.Model
{
    public class Credential
    {
        public int id { get; set; }
        public int credentialTypeId { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public Credential()
        { 
        
        
        }

        public Credential(IDataRecord reader, bool isFromView = false)
        {
            if (!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, CredentialDAO.ID);
            this.credentialTypeId = DAOUtility.GetData<int>(reader, CredentialDAO.CREDENTIAL_TYPE_ID);
            this.username = DAOUtility.GetData<string>(reader, CredentialDAO.USERNAME);
            this.password = DAOUtility.GetData<string>(reader, CredentialDAO.PASSWORD);        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, CredentialDAO.VIEW_CREDENTIAL_ID);
            this.username = DAOUtility.GetData<string>(reader, CredentialDAO.VIEW_CREDENTIAL_USERNAME);
            this.password = DAOUtility.GetData<string>(reader, CredentialDAO.VIEW_CREDENTIAL_PASSWORD);  
        }
    }
}
