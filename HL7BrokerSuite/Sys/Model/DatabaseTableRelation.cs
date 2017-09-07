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
    public class DatabaseTableRelation
    {
        public int id { get; set; }
        public int sourceDatabaseTableId { get; set; }
        public int targetDatabaseTableId { get; set; }
        public bool requiresIdentity { get; set; }

        public DatabaseTableRelation()
        { 
        
        
        }

        public DatabaseTableRelation(IDataRecord reader, bool isFromView = false)
        {
            if(!isFromView)
                fillDataReader(reader);
            else
                fillDataReaderCustomView(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, DatabaseTableRelationDAO.ID);
            this.sourceDatabaseTableId = DAOUtility.GetData<int>(reader, DatabaseTableRelationDAO.SOURCE_DATABASE_TABLE_ID);
            this.targetDatabaseTableId = DAOUtility.GetData<int>(reader, DatabaseTableRelationDAO.TARGET_DATABASE_TABLE_ID);
            this.requiresIdentity = DAOUtility.GetData<bool>(reader, DatabaseTableRelationDAO.REQUIRES_IDENTITY);
        
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, DatabaseTableRelationDAO.VIEW_DATABASE_TABLE_RELATION_ID);
            this.requiresIdentity = DAOUtility.GetData<bool>(reader, DatabaseTableRelationDAO.VIEW_DATABASE_TABLE_RELATION_HAS_DEPENDENCIES);
        }
    }
}
