using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSShieldsApps;
using HL7Importer.DAO;
using HL7Importer.Utility;
using System.Data;

namespace HL7Importer.Model
{
    public class WsReport : Report
    {
        public int radiologistUserNo { get; set; }

        public WsReport()
        { 
        
        }

        public WsReport(IDataRecord reader)
        {
            fillDataReader(reader);
        }

        private void fillDataReader(IDataRecord reader)
        {
            this.radiologistUserNo  = DAOUtility.GetData<int>(reader, SectraDAO.REPORT_RADIOLOGIST);
            this.ObservationDate    = DAOUtility.GetData<DateTime>(reader, SectraDAO.REPORT_SIGNEDDATE);
            this.ReportText         = DAOUtility.GetData<string>(reader, SectraDAO.REPORT_INTERPRETATION);
        }
    }
}
