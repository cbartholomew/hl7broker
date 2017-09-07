using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Utility;
using HL7BrokerSuite.Sys.DAO;

namespace HL7BrokerSuite.Sys.Model
{
    public class ApplicationSetting
    {
        public int id { get; set; }
        public int applicationId { get; set; }
        public int communicationId { get; set; }
        public bool disabled { get; set; }
        public bool autoStart { get; set; }

        public ApplicationSetting()
        { 
        
        }
        
        public ApplicationSetting(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, ApplicationSettingDAO.ID);
            this.applicationId = DAOUtility.GetData<int>(reader, ApplicationSettingDAO.APPLICATION_ID);
            this.communicationId = DAOUtility.GetData<int>(reader, ApplicationSettingDAO.COMMUNICATION_ID);
            this.autoStart = DAOUtility.GetData<bool>(reader, ApplicationSettingDAO.AUTOSTART);
            this.disabled = DAOUtility.GetData<bool>(reader, ApplicationSettingDAO.DISABLED);
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, ApplicationSettingDAO.VIEW_APP_SETTING_ID);
            this.autoStart = DAOUtility.GetData<bool>(reader, ApplicationSettingDAO.VIEW_APP_SETTING_AUTOSTART);
            this.disabled = DAOUtility.GetData<bool>(reader, ApplicationSettingDAO.VIEW_APP_SETTING_DISABLED);
        }
    }
}
