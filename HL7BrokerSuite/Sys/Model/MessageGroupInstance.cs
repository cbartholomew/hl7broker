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
    public class MessageGroupInstance
    {
        public int id { get; set; }
        public int messageTypeId { get; set; }
        public int segmentTypeId { get; set; }
        public DateTime createdDttm { get; set; }
        public DateTime updatedDttm { get; set; }
        public string description { get; set; }

        public MessageGroupInstance()
        { 
        
        
        }

        public MessageGroupInstance(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id                = DAOUtility.GetData<int>(reader,MessageGroupInstanceDAO.ID);
            this.messageTypeId     = DAOUtility.GetData<int>(reader,MessageGroupInstanceDAO.MESSAGE_TYPE_ID);
            this.segmentTypeId     = DAOUtility.GetData<int>(reader,MessageGroupInstanceDAO.SEGMENT_TYPE_ID);
            this.createdDttm       = DAOUtility.GetData<DateTime>(reader,MessageGroupInstanceDAO.CREATED_DTTM);
            this.updatedDttm       = DAOUtility.GetData<DateTime>(reader, MessageGroupInstanceDAO.UPDATED_DTTM);
            this.description       = DAOUtility.GetData<string>(reader, MessageGroupInstanceDAO.DESCRIPTION);
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, MessageGroupInstanceDAO.VIEW_MESSAGE_GROUP_INSTANCE_ID);
            this.description = DAOUtility.GetData<string>(reader, MessageGroupInstanceDAO.VIEW_MESSAGE_GROUP_INSTANCE_DESCRIPTION);
        }
    }
}
