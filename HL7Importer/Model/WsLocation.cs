using HL7Importer.DAO;
using HL7Importer.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSShieldsApps;

namespace HL7Importer.Model
{
    public class WsLocation : Location
    {
        public WsLocation()
        { 
        
        }

        public WsLocation(IDataRecord reader)
        {
            fillDataReader(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {        
            this.StreetOne      = DAOUtility.GetData<string>(reader, SectraDAO.Street1);
            this.StreetTwo      = DAOUtility.GetData<string>(reader, SectraDAO.Street2);
            this.City           = DAOUtility.GetData<string>(reader, SectraDAO.City);
            this.State          = DAOUtility.GetData<string>(reader, SectraDAO.State);
            this.ZipCode        = DAOUtility.GetData<string>(reader, SectraDAO.Zip);
            this.EnterpriseId   = 1;
        }
    }
}
