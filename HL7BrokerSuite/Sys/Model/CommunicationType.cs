using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Utility;
using System.Data;

namespace HL7BrokerSuite.Sys.Model
{
    public class CommunicationType
    {
        public int id { get; set; }
        public string name { get; set; }

        public CommunicationType()
        { 
                
        }

        public CommunicationType(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, CommunicationTypeDAO.ID);
            this.name = DAOUtility.GetData<string>(reader, CommunicationTypeDAO.NAME);   
        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, CommunicationTypeDAO.VIEW_COMMUNICATION_TYPE_ID);
            this.name = DAOUtility.GetData<string>(reader, CommunicationTypeDAO.VIEW_COMMUNICATION_TYPE_NAME);   
        }
    }
}
