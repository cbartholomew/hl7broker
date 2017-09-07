using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSShieldsApps;
using HL7Importer.Utility;
using HL7Importer.DAO;
namespace HL7Importer.Model
{
    public class WsOrganization : Organization
    {
        public WsOrganization()
        { 
        
        }

        public WsOrganization(IDataRecord reader)
        {
            fillDataReader(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {        
            this.OrganizationName = DAOUtility.GetData<string>(reader, SectraDAO.ORGANIZATION_NAME);
        }
    }
}
