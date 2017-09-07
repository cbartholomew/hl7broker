using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Importer.DAO;
using HL7Importer.Utility;

namespace HL7Importer.Model
{
    public class SelOru
    {
        public string accessionNo { get; set; }

        public SelOru()
        { 
        
        }

        public SelOru(IDataRecord reader)
        {
            fillDataReader(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.accessionNo = DAOUtility.GetData<string>(reader,SelOruDAO.ACCESSION_NO);
        }
    }

}
