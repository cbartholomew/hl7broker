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
    public class DatabaseInstance
    {
        public int id { get; set; }
        public int communicationId { get; set; }
        public int credentialId { get; set; }
        public string name { get; set; }
        public string server { get; set; }
        public string ipAddress { get; set; }

        public DatabaseInstance()
        { 
        
        
        }

        public DatabaseInstance ShallowCopy()
        {
            return (DatabaseInstance)this.MemberwiseClone();
        }

        public DatabaseInstance(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, DatabaseInstanceDAO.ID);
            this.communicationId = DAOUtility.GetData<int>(reader, DatabaseInstanceDAO.COMMUNICATION_ID);
            this.credentialId = DAOUtility.GetData<int>(reader, DatabaseInstanceDAO.CREDENTIAL_ID);
            this.name = DAOUtility.GetData<string>(reader, DatabaseInstanceDAO.NAME);
            this.server = DAOUtility.GetData<string>(reader, DatabaseInstanceDAO.SERVER);
            this.ipAddress = DAOUtility.GetData<string>(reader, DatabaseInstanceDAO.IP_ADDRESS);        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.name = DAOUtility.GetData<string>(reader, DatabaseInstanceDAO.VIEW_DATABASE_NAME);
            this.server = DAOUtility.GetData<string>(reader, DatabaseInstanceDAO.VIEW_DATABASE_SERVER);
            this.ipAddress = DAOUtility.GetData<string>(reader, DatabaseInstanceDAO.VIEW_DATABASE_IP_ADDRESS);
            this.id = DAOUtility.GetData<int>(reader, DatabaseInstanceDAO.VIEW_DATABASE_INSTANCE_ID);
        }
    }
}
