using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.Sys.Model
{
    public class SysDataAccessCredential
    {
        public string username { get; set; }
        public string password { get; set; }
        public string server { get; set; }
        public string database { get; set; }
    }
}
