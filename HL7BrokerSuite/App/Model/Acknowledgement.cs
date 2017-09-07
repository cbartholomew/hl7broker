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
    public class Acknowledgement
    {
        public int id { get; set; }
        public int messageId { get; set; }
        public int acknowledgementTypeId { get; set; }
        public string raw { get; set; }
        public DateTime createdDttm { get; set; }

        public Acknowledgement()
        { 
                
        }

        public Acknowledgement(IDataRecord reader)
        {
            fillDataReader(reader);        
        }

        public void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, AcknowledgementDAO.ID);
            this.messageId = DAOUtility.GetData<int>(reader, AcknowledgementDAO.MESSAGE_ID);
            this.acknowledgementTypeId = DAOUtility.GetData<int>(reader, AcknowledgementDAO.ACKNOWLEDGEMENT_TYPE_ID);
            this.raw = DAOUtility.GetData<string>(reader, AcknowledgementDAO.RAW);
            this.createdDttm = DAOUtility.GetData<DateTime>(reader, AcknowledgementDAO.CREATED_DTTM);
        }
    }
}
