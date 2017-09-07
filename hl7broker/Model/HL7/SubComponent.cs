using HL7Broker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Broker.Model.HL7
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

        public void Parse(string subComponent, int position)
        {
            try
            {
                this.value = subComponent;
                this.position = position;

                if (subComponent.Contains(TILDE))
                {
                    List<string> delimitedResult = subComponent.Split(TILDE).ToList();

                    // place a buffer in field 0
                    delimitedResult.Insert(ZERO_ELEMENT, "");

                    for (int repIndex = 1; repIndex < delimitedResult.Count; repIndex++)
                    {
                        Repetition r = new Repetition();
                        r.value = delimitedResult[repIndex];
                        r.position = repIndex;
                        this.repetitions.Add(r);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, "SubComponent.Parse(string subComponent, int position)",
                    subComponent + "|" + position.ToString());
            }            
        }

        public static SubComponent getSubComponentByPos(Component singleComponent, int position)
        {
            SubComponent sc = new SubComponent();
            try
            {                 
                int subComponentIndex = singleComponent.subComponents.FindIndex(x => x.position == position);

                sc = singleComponent.subComponents[subComponentIndex];
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, "getSubComponentByPos(Component singleComponent, int position)",
                    singleComponent.subComponents.Count.ToString() + " " + position.ToString());
            }          
            return sc;
        }
    }
}
