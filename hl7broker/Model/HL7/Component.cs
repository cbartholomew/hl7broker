using HL7Broker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Broker.Model.HL7
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

        public void Parse(string component, int position)
        {
            try
            {
                this.value = component;
                this.position = position;

                if (component.Contains(AMP))
                {
                    List<string> delimitedResult = component.Split(AMP).ToList();

                    // place a buffer in field 0
                    delimitedResult.Insert(ZERO_ELEMENT, "");

                    for (int subComponentIndex = 1; subComponentIndex < delimitedResult.Count; subComponentIndex++)
                    {
                        SubComponent sc = new SubComponent();
                        sc.Parse(delimitedResult[subComponentIndex], subComponentIndex);
                        this.subComponents.Add(sc);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, "Component.Parse(string component, int position)",
                    component + "|" + position.ToString());
            }            
        }

        public static Component getComponentByPos(Field singleField, int position)
        {
            Component c = new Component();
            try
            {
                if(singleField.components.Count >= position)
                { 
                    int componentIndex = singleField.components.FindIndex(x => x.position == position);

                    c = singleField.components[componentIndex];
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, "getComponentByPos(Field singleField, int position)",
                singleField.components.Count.ToString() + " " + position.ToString());
                
            }           
            return c;
        }
    }
}
