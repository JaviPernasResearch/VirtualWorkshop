using System;
using System.Collections.Generic;
using System.Text;

namespace simProcess
{
    public class UpdateTime : Eventcs
    {
        
        protected SimClock simClock;


        public void start(SimClock sc)
        {
            simClock = sc;
            simClock.scheduleEvent(this, 0.1);
        }


        void Eventcs.execute()
        {
            simClock.scheduleEvent(this, 0.1);
        }

    }
}
