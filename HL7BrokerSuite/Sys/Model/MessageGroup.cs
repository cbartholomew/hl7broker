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
    public class MessageGroup
    {
        public int id { get; set; }
        public int messageGroupInstanceId { get; set; }
        public int messagePartId { get; set; }
        public int position { get; set; }

        public MessageGroup()
        { 
        
        
        }

        public MessageGroup(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id                       = DAOUtility.GetData<int>(reader,MessageGroupDAO.ID);
            this.messageGroupInstanceId   = DAOUtility.GetData<int>(reader,MessageGroupDAO.MESSAGE_GROUP_INSTANCE_ID);
            this.messagePartId            = DAOUtility.GetData<int>(reader,MessageGroupDAO.MESSAGE_PART_ID);
            this.position                 = DAOUtility.GetData<int>(reader, MessageGroupDAO.POSITION);
        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.position = DAOUtility.GetData<int>(reader, MessageGroupDAO.VIEW_MESSAGE_GROUP_POSITION);
        }
    }
}
