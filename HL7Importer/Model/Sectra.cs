using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Importer.Model;

namespace HL7Importer.Model
{
    public class Sectra
    {
        public WsExam wsExam { get; set; }
        public WsPatient wsPatient{ get; set; }
        public WsOrganization wsOrganization { get; set; }
        public WsDoctor referringDoctor { get; set; }
        public WsDoctor radiologistDoctor { get; set; }
        public WsLocation wsReferringDoctorLocation { get; set; }
        public WsReport wsReport { get; set; }

        public Sectra()
        {
            this.wsExam = new WsExam();
            this.wsPatient = new WsPatient();
            this.wsOrganization = new WsOrganization();
            this.referringDoctor = new WsDoctor();
            this.radiologistDoctor = new WsDoctor();
            this.wsReferringDoctorLocation = new WsLocation();
            this.wsReport = new WsReport();
        }
    }
}
