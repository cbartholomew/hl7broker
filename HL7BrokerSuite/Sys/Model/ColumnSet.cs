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
    public class ColumnSet 
    {
        public int      id { get; set; }
        public int      databaseTableId { get; set; }
        public string   name { get; set; }
        public bool     isPrimaryKey { get; set; }
        public int      messageGroupInstanceId { get; set; }
        public string   columnDataType { get; set; }

        public ColumnSet()
        { 
        
        }

        public ColumnSet(IDataRecord reader, bool isFromView = false)
        {
             if(!isFromView)
                 fillDataReader(reader);
             else
                 fillDataReaderCustomView(reader);             
        }

        public ColumnSet ShallowCopy()
        {
            return (ColumnSet)this.MemberwiseClone();
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, ColumnSetDAO.ID);
            this.databaseTableId = DAOUtility.GetData<int>(reader, ColumnSetDAO.DATABASE_TABLE_ID);
            this.name = DAOUtility.GetData<string>(reader, ColumnSetDAO.NAME);
            this.isPrimaryKey = DAOUtility.GetData<bool>(reader, ColumnSetDAO.IS_PRIMAR_KEY);
            this.messageGroupInstanceId = DAOUtility.GetData<int>(reader, ColumnSetDAO.MESSAGE_GROUP_INSTANCE_ID);
            this.columnDataType = DAOUtility.GetData<string>(reader, ColumnSetDAO.COLUMN_DATA_TYPE);
        }

        private void fillDataReaderCustomView(IDataRecord reader)
        {
            this.id = DAOUtility.GetData<int>(reader, ColumnSetDAO.VIEW_DATABASE_ID);
            this.name = DAOUtility.GetData<string>(reader, ColumnSetDAO.VIEW_DATABASE_COLUMN);
            this.isPrimaryKey = DAOUtility.GetData<bool>(reader, ColumnSetDAO.VIEW_DATABASE_COLUMN_IS_PRIMARY_KEY);
            this.messageGroupInstanceId = DAOUtility.GetData<int>(reader, ColumnSetDAO.VIEW_DATABASE_COLUMN_MESSAGE_GROUP_INSTANCE_ID);
            this.columnDataType = DAOUtility.GetData<string>(reader, ColumnSetDAO.VIEW_DATABASE_COLUMN_DATA_TYPE);
        }

    }
}
