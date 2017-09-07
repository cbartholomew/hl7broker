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
    public class Message
    {
        public int id { get; set; }
        public int messageHeaderInstaceId { get; set; }
        public string hl7Raw { get; set; }

        public Message()
        { 
                
        }

        public Message(IDataRecord reader, bool isFromView = false)
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
            this.id = DAOUtility.GetData<int>(reader, MessageDAO.ID);
            this.messageHeaderInstaceId = DAOUtility.GetData<int>(reader, MessageDAO.MESSAGE_HEADER_INSTANCE_ID);
            this.hl7Raw = DAOUtility.GetData<string>(reader, MessageDAO.HL7_RAW);
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, MessageDAO.VIEW_MESSAGE_ID);
            this.hl7Raw = DAOUtility.GetData<string>(reader, MessageDAO.VIEW_MESSAGE_HL7_RAW);
        }

    }
}
