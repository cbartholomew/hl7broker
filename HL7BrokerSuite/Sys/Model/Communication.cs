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
    public class Communication
    {
        public int id { get; set; }
        public int directionTypeId { get; set; }
        public int communicationTypeId { get; set; }
        public int applicationId { get; set; }

        public Communication()
        { 
                
        }

        public Communication(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, CommunicationDAO.ID);
            this.directionTypeId = DAOUtility.GetData<int>(reader, CommunicationDAO.DIRECTION_TYPE_ID);
            this.communicationTypeId = DAOUtility.GetData<int>(reader, CommunicationDAO.COMMUNICATION_TYPE_ID);
            this.applicationId = DAOUtility.GetData<int>(reader, CommunicationDAO.APPLICATION_ID);        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, CommunicationDAO.VIEW_COMMUNICATION_ID);
            this.directionTypeId = DAOUtility.GetData<int>(reader, CommunicationDAO.VIEW_COMMUNICATION_DIRECTION_TYPE_ID);
            this.communicationTypeId = DAOUtility.GetData<int>(reader, CommunicationDAO.VIEW_COMMUNICATION_COMMUNICATION_TYPE_ID);
            this.applicationId = DAOUtility.GetData<int>(reader, CommunicationDAO.VIEW_COMMUNICATION_APPLICATION_ID);  
        }
    }
}
