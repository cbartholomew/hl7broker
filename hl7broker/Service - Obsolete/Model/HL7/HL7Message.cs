using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.Service.Model.HL7
{
    public class HL7Message 
    {
        public string messageType { get; set; }
        public List<Segment> segments { get; set; }

        public HL7Message() 
        {
            initialize();        
        }

        private void initialize()
        {            
            this.segments = new List<Segment>();
        }
    }
}
