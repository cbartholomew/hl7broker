using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.Service.Model.HL7
{
    public class Field : Generic
    {
        public List<Component> components { get; set; }

        public Field() 
        {
            initialize();        
        }

        private void initialize()
        {
            this.components = new List<Component>();
        }
    }
}
