using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.Service.Model.HL7
{
    public class SubComponent : Generic
    {
        public List<Repetition> repetitions { get; set; }

        public SubComponent() 
        {
            initialize();        
        }

        private void initialize()
        {
            this.repetitions = new List<Repetition>();
        }
    }
}
