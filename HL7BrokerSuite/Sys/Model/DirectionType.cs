using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Utility;
using System.Data;

namespace HL7BrokerSuite.Sys.Model
{
    public class DirectionType
    {
        public int id { get; set; }
        public string name { get; set; }

        public DirectionType()
        { 
                
        }

        public DirectionType(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, DirectionTypeDAO.ID);
            this.name = DAOUtility.GetData<string>(reader, DirectionTypeDAO.NAME);   
        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, DirectionTypeDAO.VIEW_DIRECTION_TYPE_ID);
            this.name = DAOUtility.GetData<string>(reader, DirectionTypeDAO.VIEW_DIRECTION_TYPE_NAME);   
        }
    }
}
