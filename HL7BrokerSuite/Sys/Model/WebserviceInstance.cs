using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.Sys.Model
{
    public class WebserviceInstance
    {
        public int id { get; set; }
        public int communicationId { get; set; }
        public int credentialId { get; set; }
        public string name { get; set; }
        public string server { get; set; }
        public string ipAddress { get; set; }

        public WebserviceInstance()
        {

        }

        public WebserviceInstance(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, WebserviceInstanceDAO.ID);
            this.communicationId = DAOUtility.GetData<int>(reader, WebserviceInstanceDAO.COMMUNICATION_ID);
            this.credentialId = DAOUtility.GetData<int>(reader, WebserviceInstanceDAO.CREDENTIAL_ID);
            this.name = DAOUtility.GetData<string>(reader, WebserviceInstanceDAO.NAME);
            this.server = DAOUtility.GetData<string>(reader, WebserviceInstanceDAO.SERVER);
            this.ipAddress = DAOUtility.GetData<string>(reader, WebserviceInstanceDAO.IP_ADDRESS);        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.name = DAOUtility.GetData<string>(reader, WebserviceInstanceDAO.VIEW_WEBSERVICE_NAME);
            this.server = DAOUtility.GetData<string>(reader, WebserviceInstanceDAO.VIEW_WEBSERVICE_SERVER);
            this.ipAddress = DAOUtility.GetData<string>(reader, WebserviceInstanceDAO.IP_ADDRESS);
            this.id = DAOUtility.GetData<int>(reader, WebserviceInstanceDAO.VIEW_WEBSERVICE_INSTANCE_ID);
        }
    }
}
