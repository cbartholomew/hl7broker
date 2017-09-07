using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Utility;
using HL7BrokerSuite.App.DAO;

namespace HL7BrokerSuite.App.Model
{
    public class MessageHeaderInstance
    {
        public int      id                      { get; set; }
        public string   sendingApplication      { get; set; }
        public string   receivingApplication    { get; set; }
        public string   sendingFacility         { get; set; }
        public string   receivingFacility       { get; set; }
        public string   messageDTTM             { get; set; }
        public string   messageControlId        { get; set; }
        public string   messageType             { get; set; }
        public string   versionId               { get; set; }
        public string   applicationAckType      { get; set; }
        public string   acceptAckType           { get; set; }
        public string   patientIdentifier       { get; set; }
        public string   orderControlCode        { get; set; }
        public string   processed               { get; set; }
        public DateTime pendingReprocessDttm    { get; set; }
        public DateTime processedDttm           { get; set; }
        public DateTime createdDttm             { get; set; }
        public int      processedCount          { get; set; }

        public MessageHeaderInstance()
        { 
                   
        }

        public MessageHeaderInstance(IDataRecord reader, bool isFromView = false)
        {
            if (!isFromView)
            {
                fillDataReader(reader);
            }
            else
            {
                fillDataReaderCustomView(reader);
            }            
        }

        public void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, MessageHeaderInstanceDAO.ID);
            this.sendingApplication = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.SENDING_APPLICATION);
            this.receivingApplication = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.RECEIVING_APPLICATION);
            this.sendingFacility = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.SENDING_FACILITY);
            this.receivingFacility = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.RECEIVING_FACILTIY);
            this.messageDTTM = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.MESSAGE_DTTM);
            this.messageControlId = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.MESSAGE_CONTROL_ID);
            this.messageType = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.MESSAGE_TYPE);
            this.versionId = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VERSION_ID);
            this.applicationAckType = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.APPLICATION_ACK_TYPE);
            this.acceptAckType = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.ACCEPT_ACK_TYPE);
            this.patientIdentifier = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.PATIENT_IDENTIFIER);
            this.orderControlCode = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.ORDER_CONTROL_CODE);            
            this.processed = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.PROCESSED);
            this.processedDttm = DAOUtility.GetData<DateTime>(reader, MessageHeaderInstanceDAO.PROCESSED_DTTM);
            this.pendingReprocessDttm = DAOUtility.GetData<DateTime>(reader, MessageHeaderInstanceDAO.PENDING_REPROCESS_DTTM);
            this.createdDttm = DAOUtility.GetData<DateTime>(reader, MessageHeaderInstanceDAO.CREATED_DTTM);
            this.processedCount = DAOUtility.GetData<int>(reader, MessageHeaderInstanceDAO.PROCESSED_COUNT);  
        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, MessageHeaderInstanceDAO.VIEW_MHI_ID);
            this.sendingApplication = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_SENDING_APPLICATION);
            this.receivingApplication = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_RECEIVING_APPLICATION);
            this.sendingFacility = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_SENDING_FACILITY);
            this.receivingFacility = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_RECEIVING_FACILTIY);
            this.messageDTTM = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_MESSAGE_DTTM);
            this.messageControlId = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_MESSAGE_CONTROL_ID);
            this.messageType = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_MESSAGE_TYPE);
            this.versionId = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_VERSION_ID);
            this.applicationAckType = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_APPLICATION_ACK_TYPE);
            this.acceptAckType = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_ACCEPT_ACK_TYPE);
            this.patientIdentifier = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_PATIENT_IDENTIFIER);
            this.orderControlCode = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_ORDER_CONTROL_CODE);
            this.processed = DAOUtility.GetData<string>(reader, MessageHeaderInstanceDAO.VIEW_MHI_PENDING_REPROCESS_DTTM);
            this.processedDttm = DAOUtility.GetData<DateTime>(reader, MessageHeaderInstanceDAO.VIEW_MHI_PROCESSED_DTTM);
            this.pendingReprocessDttm = DAOUtility.GetData<DateTime>(reader, MessageHeaderInstanceDAO.PENDING_REPROCESS_DTTM);
            this.createdDttm = DAOUtility.GetData<DateTime>(reader, MessageHeaderInstanceDAO.VIEW_MHI_CREATED_DTTM);
            this.processedCount = DAOUtility.GetData<int>(reader, MessageHeaderInstanceDAO.VIEW_MHI_PROCESSED_COUNT);  
        }
    }
}
