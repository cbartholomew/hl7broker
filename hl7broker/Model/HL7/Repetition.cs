using HL7Broker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Broker.Model.HL7
{
    public class Repetition : Generic
    {
        public Repetition() 
        {

        }

        public static Repetition getRepetitionByPos(SubComponent singleSubComponent, int position)
        {
            Repetition r = new Repetition();
            try
            {
                int repetitionIndex = singleSubComponent.repetitions.FindIndex(x => x.position == position);

                r = singleSubComponent.repetitions[repetitionIndex];
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, "Repetition.getRepetitionByPos(SubComponent singleSubComponent, int position)",
                    singleSubComponent.repetitions.Count().ToString() + "|" + position.ToString());
            }         
            return r;
        }
    }
}
