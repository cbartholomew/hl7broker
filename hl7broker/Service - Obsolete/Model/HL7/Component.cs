using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.Service.Model.HL7
{
    public class Component : Generic
    {
        public List<SubComponent> subComponents { get; set; }

        public Component() 
        {
            initialize();        
        }

        private void initialize()
        {
            this.subComponents = new List<SubComponent>();
        }
    }
}
