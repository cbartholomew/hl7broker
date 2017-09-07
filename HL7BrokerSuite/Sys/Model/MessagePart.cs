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
    public class MessagePart
    {
        public int id { get; set; }
        public string name { get; set; }
        public char delimiter { get; set; }

        public MessagePart()
        { 
        
        }

        public MessagePart(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id         = DAOUtility.GetData<int>(reader,MessagePartDAO.ID);
            this.name       = DAOUtility.GetData<string>(reader,MessagePartDAO.NAME);
            this.delimiter  = DAOUtility.GetData<char>(reader, MessagePartDAO.DELIMITER);
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, MessagePartDAO.VIEW_MESSAGE_PART_ID);
            this.name = DAOUtility.GetData<string>(reader, MessagePartDAO.VIEW_MESSAGE_PART_NAME);
            this.delimiter = DAOUtility.GetData<char>(reader, MessagePartDAO.VIEW_MESSAGE_PART_DELIMITER);
        }
    }
}
