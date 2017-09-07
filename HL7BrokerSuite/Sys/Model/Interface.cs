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
    public class Interface
    {
        public int id { get; set; }
        public int communicationId { get; set; }
        public int credentialId { get; set; }
        public string ipAddress { get; set; }
        public string port { get; set; }
        public int maxConnections { get; set; }

        public Interface()
        { 
        
        
        }

        public Interface(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, InterfaceDAO.ID);
            this.communicationId = DAOUtility.GetData<int>(reader, InterfaceDAO.COMMUNICATION_ID);
            this.credentialId = DAOUtility.GetData<int>(reader, InterfaceDAO.CREDENTIAL_ID);
            this.ipAddress = DAOUtility.GetData<string>(reader, InterfaceDAO.IP_ADDRESS);
            this.port = DAOUtility.GetData<string>(reader, InterfaceDAO.PORT);
            this.maxConnections = DAOUtility.GetData<int>(reader, InterfaceDAO.MAX_CONNECTIONS);        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.ipAddress = DAOUtility.GetData<string>(reader, InterfaceDAO.VIEW_INTERFACE_IP_ADDRESS);
            this.port = DAOUtility.GetData<string>(reader, InterfaceDAO.VIEW_INTERFACE_PORT);
            this.maxConnections = DAOUtility.GetData<int>(reader, InterfaceDAO.VIEW_INTERFACE_MAX_CONNECTIONS);
        }
    }
}
