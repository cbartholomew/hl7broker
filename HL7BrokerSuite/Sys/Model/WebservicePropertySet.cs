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
    public class WebservicePropertySet
    {
        public int      id { get; set; }
        public int      webserviceObjectId { get; set; }
        public string   name { get; set; }
        public int      messageGroupInstanceId { get; set; }
        public string   columnDataType { get; set; }

        public WebservicePropertySet()
        { 
        
        }

        public WebservicePropertySet(IDataRecord reader, bool isFromView = false)
        {
             if(!isFromView)
                 fillDataReader(reader);
             else
                 fillDataReaderCustomView(reader);             
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, WebservicePropertySetDAO.ID);
            this.webserviceObjectId = DAOUtility.GetData<int>(reader, WebservicePropertySetDAO.WEBSERVICE_OBJECT_ID);
            this.name = DAOUtility.GetData<string>(reader, WebservicePropertySetDAO.NAME);
            this.messageGroupInstanceId = DAOUtility.GetData<int>(reader, WebservicePropertySetDAO.MESSAGE_GROUP_INSTANCE_ID);
            this.columnDataType = DAOUtility.GetData<string>(reader, WebservicePropertySetDAO.COLUMN_DATA_TYPE);
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, WebservicePropertySetDAO.VIEW_WEBSERVICE_PROPERTY_ID);
            this.name = DAOUtility.GetData<string>(reader, WebservicePropertySetDAO.VIEW_WEBSERVICE_PROPERTY);            
            this.messageGroupInstanceId = DAOUtility.GetData<int>(reader, WebservicePropertySetDAO.VIEW_WEBSERVICE_PROPERTY_MESSAGE_GROUP_INSTANCE_ID);
            this.columnDataType = DAOUtility.GetData<string>(reader, WebservicePropertySetDAO.VIEW_WEBSERVICE_PROPERTY_COLUMN_DATA_TYPE);
        }
    }
}
