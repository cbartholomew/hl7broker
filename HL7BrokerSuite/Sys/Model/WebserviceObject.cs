using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.Sys.Model
{
    public class WebserviceObject
    {
        public int id { get; set; }
        public int webserviceInstanceId { get; set; }
        public string name { get; set; }

        public WebserviceObject()
        { 
        
        
        }

        public WebserviceObject(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, WebserviceObjectDAO.ID);
            this.webserviceInstanceId = DAOUtility.GetData<int>(reader, WebserviceObjectDAO.WEBSERVICE_INSTANCE_ID);
            this.name = DAOUtility.GetData<string>(reader, WebserviceObjectDAO.NAME);
        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, WebserviceObjectDAO.VIEW_WEBSERVICE_OBJECT_ID);
            this.name = DAOUtility.GetData<string>(reader, WebserviceObjectDAO.VIEW_WEBSERVICE_OBJECT_NAME);
        }
    }
}
