
using HL7Broker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Broker.Model.HL7
{
    public class Segment : Generic
    {
        public SegmentType segmentType { get; set; }
        public List<Field> fields { get; set; }

        public Segment() 
        {
            initialize();        
        }

        private void initialize()
        {
            this.fields = new List<Field>();
        }

        public void Parse(string segmentedLine)
        {
            try
            {
                List<string> delimitedResult = segmentedLine.Split(PIPE).ToList();

                this.segmentType = Segment.getSegmentType(delimitedResult[ZERO_ELEMENT]);

                if (this.segmentType == SegmentType.MSH)
                {
                    // insert pipe in MSH.1
                    delimitedResult.Insert(FIRST_ELEMENT, Generic.PIPE.ToString());
                }

                for (int fieldIndex = 0;
                    fieldIndex < delimitedResult.Count;
                    fieldIndex++)
                {
                    Field f = new Field();

                    // get field alone
                    string tempField = delimitedResult[fieldIndex];

                    // parse the field
                    f.Parse(tempField, fieldIndex);

                    // add to the segment's field set
                    this.fields.Add(f);
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, "Segement.Parse(string segmentedLine)", segmentedLine);                
            }           
        }

        public static SegmentType getSegmentType(string segmentType)
        {
            Dictionary<string, SegmentType> segmentTypeDictionary = new Dictionary<string, SegmentType>() 
            { 
                 {"MSH", SegmentType.MSH },
                 {"PID", SegmentType.PID },
                 {"PV1", SegmentType.PV1 },
                 {"PV2", SegmentType.PV2 },
                 {"ORC", SegmentType.ORC },
                 {"OBR", SegmentType.OBR },
                 {"OBX", SegmentType.OBX },
                 {"IN1", SegmentType.IN1 },
                 {"IN2", SegmentType.IN2 },
                 {"IN3", SegmentType.IN3 },
                 {"ZD1", SegmentType.ZD1 },
                 {"ZD2", SegmentType.ZD2 },
                 {"UNK", SegmentType.UNKNOWN }
            };

            SegmentType result = SegmentType.UNKNOWN;
            segmentTypeDictionary.TryGetValue(segmentType, out result);

            // return the segment type
            return result;
        }
    }
}
