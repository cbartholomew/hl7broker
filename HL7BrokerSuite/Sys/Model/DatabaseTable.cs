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
    public class DatabaseTable
    {
        public int id { get; set; }
        public int databaseInstanceId { get; set; }
        public string name { get; set; }

        public DatabaseTable()
        { 
        
        
        }

        public DatabaseTable ShallowCopy()
        {
            return (DatabaseTable)this.MemberwiseClone();
        }

        public DatabaseTable(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, DatabaseTableDAO.ID);
            this.databaseInstanceId = DAOUtility.GetData<int>(reader, DatabaseTableDAO.DATABASE_INSTANCE_ID);
            this.name = DAOUtility.GetData<string>(reader, DatabaseTableDAO.NAME);
        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, DatabaseTableDAO.VIEW_DATABASE_TABLE_ID);
            this.name = DAOUtility.GetData<string>(reader, DatabaseTableDAO.VIEW_DATABASE_TABLE);
        }
    }
}
