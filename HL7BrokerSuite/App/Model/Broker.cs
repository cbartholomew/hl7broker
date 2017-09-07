using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.App.DAO;
using HL7BrokerSuite.Utility;
using System.Data;

namespace HL7BrokerSuite.App.Model
{
    public class Broker
    {
        // local db properties
        public int id { get; set; }
        public int interfaceStatusId { get; set; }
        public int communicationId { get; set; }
        public int processId { get; set; }
        public int lastMessageId { get; set; }
        public int queueCount { get; set; }
        public DateTime lastMessageDTTM { get; set; }

        // extended properties via vShieldsHL7Broker_GetAppBrokerMainList view
        // applied to this model only to ease coding process w/ MVC website
        public int brokerId { get; set; }
        public string applicationDescription { get; set; }
        public string applicationName { get; set; }
        public int brokerQueueCount { get; set; }
        public string interfaceStatusName { get; set; }
        public string interfaceStatusColor { get; set; }
        public string communicationTypeName { get; set; }
        public string directionTypeName { get; set; }
        public int brokerProcessId { get; set; }
        public DateTime brokerLastMessageDTTM { get; set; }
        public int brokerLastMessageId { get; set; }
        public string messageHl7Raw { get; set; }
        public int messageHeaderInstanceId { get; set; }
        public int directionTypeId { get; set; }
        public int communicationTypeId { get; set; }
        public int brokerCommunicationId { get; set; }
        public int brokerInterfaceStatusId { get; set; }
        public int communicationApplicationId { get; set; }

        public Broker()
        { 
        
        }

        public Broker ShallowCopy()
        {
            return (Broker)this.MemberwiseClone();
        }

        public Broker(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, BrokerDAO.ID);
            this.interfaceStatusId = DAOUtility.GetData<int>(reader, BrokerDAO.INTERFACE_STATUS_ID);
            this.communicationId = DAOUtility.GetData<int>(reader, BrokerDAO.COMMUNICATION_ID);
            this.processId = DAOUtility.GetData<int>(reader, BrokerDAO.PROCESS_ID);
            this.lastMessageId = DAOUtility.GetData<int>(reader, BrokerDAO.LAST_MESSAGE_ID);
            this.queueCount = DAOUtility.GetData<int>(reader, BrokerDAO.QUEUE_COUNT);
            this.lastMessageDTTM = DAOUtility.GetData<DateTime>(reader, BrokerDAO.LAST_MESSAGE_DTTM); 
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.brokerId = DAOUtility.GetData<int>(reader, BrokerDAO.VIEW_BROKER_ID);
            this.applicationDescription = DAOUtility.GetData<string>(reader, BrokerDAO.VIEW_APPLICATION_DESCRIPTION);
            this.applicationName = DAOUtility.GetData<string>(reader, BrokerDAO.VIEW_APPLICATION_NAME);
            this.brokerQueueCount = DAOUtility.GetData<int>(reader, BrokerDAO.VIEW_BROKER_QUEUE_COUNT);
            this.interfaceStatusName = DAOUtility.GetData<string>(reader, BrokerDAO.VIEW_INTERFACE_STATUS_NAME);
            this.interfaceStatusColor = DAOUtility.GetData<string>(reader, BrokerDAO.VIEW_INTERFACE_STATUS_COLOR);
            this.communicationTypeName = DAOUtility.GetData<string>(reader, BrokerDAO.VIEW_COMMUNICATION_TYPE_NAME);
            this.directionTypeName = DAOUtility.GetData<string>(reader, BrokerDAO.VIEW_DIRECTION_TYPE_NAME);
            this.brokerProcessId = DAOUtility.GetData<int>(reader, BrokerDAO.VIEW_BROKER_PROCESS_ID);
            this.brokerLastMessageDTTM = DAOUtility.GetData<DateTime>(reader, BrokerDAO.VIEW_BROKER_LAST_MESSAGE_DTTM);
            this.brokerLastMessageId = DAOUtility.GetData<int>(reader, BrokerDAO.VIEW_BROKER_LAST_MESSAGE_ID);
            this.messageHl7Raw = DAOUtility.GetData<string>(reader, BrokerDAO.VIEW_MESSAGE_HL7_RAW);
            this.messageHeaderInstanceId = DAOUtility.GetData<int>(reader, BrokerDAO.VIEW_MESSAGE_HEADER_INSTANCE_ID);
            this.directionTypeId = DAOUtility.GetData<int>(reader, BrokerDAO.VIEW_DIRECTION_TYPE_ID);
            this.communicationTypeId = DAOUtility.GetData<int>(reader, BrokerDAO.VIEW_COMMUNICATION_TYPE_ID);
            this.brokerCommunicationId = DAOUtility.GetData<int>(reader, BrokerDAO.VIEW_BROKER_COMMUNICATION_ID);
            this.brokerInterfaceStatusId = DAOUtility.GetData<int>(reader, BrokerDAO.VIEW_BROKER_INTERFACE_STATUS_ID);
            this.communicationApplicationId = DAOUtility.GetData<int>(reader, BrokerDAO.VIEW_COMMUNICATION_APPLICATION_ID);
        }
    }
}
