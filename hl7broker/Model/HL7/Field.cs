using HL7Broker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Broker.Model.HL7
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

        public void Parse(string field, int position)
        {
            try
            {
                this.value = field;
                this.position = position;

                if (field.Contains(CARROT))
                {
                    List<string> delimiterResult = field.Split(CARROT).ToList();

                    // place a buffer in field 0
                    delimiterResult.Insert(ZERO_ELEMENT, "");

                    for (int componentIndex = 1; 
                        componentIndex < delimiterResult.Count; 
                        componentIndex++)
                    {
                        Component c = new Component();
                        
                        c.Parse(delimiterResult[componentIndex], componentIndex);

                        this.components.Add(c);
                    }
                }
            }
            catch (Exception e)
            {
                
                ErrorLogger.LogError(e,"Field.Parse(string field, int position)"
                    , field + "|" + position.ToString());
            }            
        }

        public static Field getFieldByPos(HL7Message message, 
                                        SegmentType segmentType, 
                                        int position, 
                                        int segmentOrder = -1)
        {
            Field f = new Field();
            List<Segment> segments = new List<Segment>();

            try
            {
                if (segmentOrder == NEG_ONE)
                {
                    segments = message.segments.FindAll(x => x.segmentType == segmentType);
                }
                else
                { 
                    segments = message.segments.FindAll(x => x.segmentType == segmentType 
                    && x.fields[FIRST_ELEMENT].value == segmentOrder.ToString());
                }

                int fieldIndex = NEG_ONE;
                foreach (Segment s in segments)
                {
                    if (fieldIndex == NEG_ONE)
                    {
                        fieldIndex = s.fields.FindIndex(x => x.position == position);                            
                        // adding to avoid bad locations
                        if (fieldIndex == NEG_ONE && segmentType == SegmentType.PV1 && position == 11)
                        {
                            // build empty location
                            f = HL7MessageUtility.getEmptyLocationNotFound();               
                        } 
                        else                 
                        {
                            f = s.fields[fieldIndex];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                List<string> parameters = new List<string>();

                parameters.Add(message.segments.Count.ToString());
                parameters.Add(segmentType.ToString());
                parameters.Add(position.ToString());
                parameters.Add(segmentOrder.ToString());

                ErrorLogger.LogError(e,"Field.getFieldByPos(HL7Message message, SegmentType segmentType, int position, int segmentOrder = -1)"
                    , String.Join(Generic.PIPE.ToString(),parameters.ToArray()));
            }

            return f;
        }
    }
}
