using HL7BrokerSuite.App.DAO;
using HL7BrokerSuite.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HL7BrokerSuite.App.Model
{
    public class MessageLog
    {
        public int id { get; set; }
        public int messageId { get; set; }
        public int messageLogTypeId { get; set; }
        public DateTime createdDttm { get; set; }

        public MessageLog()
        { 
                
        }

        public MessageLog(IDataRecord reader)
        {
            fillDataReader(reader);        
        }

        public void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, MessageLogDAO.ID);
            this.messageId = DAOUtility.GetData<int>(reader, MessageLogDAO.MESSAGE_ID);
            this.messageLogTypeId = DAOUtility.GetData<int>(reader, MessageLogDAO.MESSAGE_LOG_TYPE_ID);
            this.createdDttm = DAOUtility.GetData<DateTime>(reader, MessageLogDAO.CREATED_DTTM);
        }
    }
}
