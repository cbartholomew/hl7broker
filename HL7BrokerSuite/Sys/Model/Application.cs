using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HL7BrokerSuite.Utility;
using HL7BrokerSuite.Sys.DAO;

namespace HL7BrokerSuite.Sys.Model
{
    public class Application
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public Application()
        { 
        
        }


        public Application(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, ApplicationDAO.ID);
            this.name = DAOUtility.GetData<string>(reader, ApplicationDAO.NAME);
            this.description = DAOUtility.GetData<string>(reader, ApplicationDAO.DESCRIPTION);
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, ApplicationDAO.VIEW_CONFIG_ID);
            this.name = DAOUtility.GetData<string>(reader, ApplicationDAO.VIEW_CONFIG_NAME);
            this.description = DAOUtility.GetData<string>(reader, ApplicationDAO.VIEW_CONFIG_DESCRIPTION);
        }
    }
}
