
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.Service.Model.HL7
{
    public class Segment : Generic
    {
        public List<Field> fields { get; set; }

        public Segment() 
        {
            initialize();        
        }

        private void initialize()
        {
            this.fields = new List<Field>();
        }
    }
}
