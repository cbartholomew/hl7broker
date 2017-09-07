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
    public class MessageType
    {
        public int id { get; set; }
        public string name { get; set; }

        public MessageType()
        { 
        
        
        }

        public MessageType(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, MessageTypeDAO.ID);
            this.name = DAOUtility.GetData<string>(reader, MessageTypeDAO.NAME);        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.name = DAOUtility.GetData<string>(reader, MessageTypeDAO.VIEW_MESSAGE_TYPE_NAME);        
        }
    }
}
