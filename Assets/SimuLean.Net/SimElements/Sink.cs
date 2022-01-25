using System;
using System.Collections.Generic;

using System.Text;

namespace simProcess
{
    public class Sink : Element
    {
        int numberIterms;

        public Sink(String name, SimClock state) : base(name, state)
        {
        }

        public int getNumberIterms()
        {
            return numberIterms;
        }

        public override void start()
        {
            numberIterms = 0;
        }

        public override bool unblock()
        {
            throw new System.InvalidOperationException("The Sink cannot receive notifications."); //To change body of generated methods, choose Tools | Templates.
        }

        /*public override bool notifyRequest() {
            throw new System.InvalidOperationException("The Sink cannot receive notifications."); //To change body of generated methods, choose Tools | Templates.
        }*/

        override public int getQueueLength()
        {
            return 0;
        }
        override public int getFreeCapacity()
        {
            return -1;
        }


        public override bool receive(Item theItem)
        {
            vElement.loadItem(theItem);
            numberIterms++;
            return true;
        }

        public override bool checkAvaliability(Item theItem)
        {
            return true;
        }

        /*public override bool cancelRequest() {
            return true;
        }*/
    }
}
