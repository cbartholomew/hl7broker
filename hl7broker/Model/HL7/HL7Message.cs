using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Broker.Utility;
using System.IO;

namespace HL7Broker.Model.HL7
{
    public class HL7Message : Generic
    {
        public MessageType messageType { get; set; }
        public List<Segment> segments { get; set; }

        public HL7Message() 
        {
            initialize();        
        }

        private void initialize()
        {            
            this.segments = new List<Segment>();
        }

        public static void Parse(HL7Message hl7Message, string hl7Input)
        {
            try
            {
                List<string> delimitedResult = hl7Input.Split(Convert.ToChar(CR)).ToList();

                foreach (string segment in delimitedResult)
                {
                    Segment s = new Segment();

                    s.Parse(segment);

                    hl7Message.segments.Add(s);
                }

                // get the message type after you've processed the message
                hl7Message.messageType = HL7MessageUtility.getMessageType(hl7Message, hl7Input);
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, "HL7Message.Parse(HL7Message hl7Message, string hl7Input)", hl7Input);
            }

        }

        public static string ReadHL7FromFile(string pathToFile)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                byte[] hl7Message = File.ReadAllBytes(pathToFile);

                foreach (byte character in hl7Message)
                {
                    sb.Append(Convert.ToChar(character));
                }
            }
            catch (Exception ex)
            {
                sb.Append(BLANK);
                throw ex;
            }

            return sb.ToString().Replace(Convert.ToChar(Generic.LF).ToString(), BLANK);
        }

       
    }
}
