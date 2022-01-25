using System;
using System.Collections.Generic;
using System.Text;

namespace simProcess
{
    public class SimClock
    {
        double simTime;



        DoubleMinBinaryHeap events;

        public SimClock()
        {
            events = new DoubleMinBinaryHeap(10);
            simTime = 0;
        }

        public void scheduleEvent(Eventcs theEvent, double time)
        {
            events.add(simTime + time, theEvent);
        }

        public bool advanceClock(double time)
        {
            double t;
            Eventcs nextEvent;
            
            if (events.count() == 0)
            {
                return false;
            }

            t = events.getMinValue();
            
            while (t <= time)
            {
                SimCosts.addCost((t-simTime) * Element.getInventory() * SimCosts.inventoryUnitCost);

                simTime = t;
                
                
                nextEvent = (Eventcs) events.retrieveFirst();
                nextEvent.execute();

                if (events.count() == 0)
                {
                    return false;
                }
                t = events.getMinValue();
            }

            return true;
        }

        public void reset()
        {
            simTime = 0;
            events.reset();
        }

        public double getSimulationTime()
        {
            return simTime;
        }

    }
}
