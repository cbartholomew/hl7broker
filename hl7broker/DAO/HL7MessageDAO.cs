using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Broker.Model.HL7;
using HL7Broker.Utility;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Sys.DAO;
namespace HL7Broker.DAO
{
    public class HL7MessageDAO : Generic
    {
        public static HL7Message getMessage(string hl7Input) {

            HL7Message hl7 = new HL7Message();

            HL7Message.Parse(hl7, hl7Input);

            return hl7;
        }
    }
}
