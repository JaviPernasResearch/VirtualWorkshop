using System;
using System.Collections.Generic;
using System.Text;

namespace simProcess
{
    public class InfiniteSource : Element, Eventcs
    {
        Item lastItem;

        int numberIterms;

        public InfiniteSource(string name, SimClock state) : base(name, state)
        {
        }

        public override void start()
        {
            numberIterms = 0;

            simClock.scheduleEvent(this, 0.0);
        }

        public override bool unblock()
        {

            if (this.getOutput().sendItem(lastItem, this))
            {
                lastItem = createItem();
                numberIterms++;
                return true;
            }

            else
                return false;
        }
        
        public int getNumberItems()
        {
            return numberIterms;
        }

        public override bool receive(Item theItem)
        {
            throw new System.InvalidOperationException("The Source cannot receive Items."); //To change body of generated methods, choose Tools | Templates.
        }

        override public int getQueueLength()
        {
            return 0;
        }
        override public int getFreeCapacity()
        {
            return 0;
        }


        void Eventcs.execute()
        {
            int i = 0;
            do
            {
                lastItem = createItem();
                numberIterms++;
            }
            while (this.getOutput().sendItem(lastItem, this));
        }

        Item createItem()
        {
            Item nItem = new Item(simClock.getSimulationTime());
            nItem.setId("type", 1, 1);
            nItem.vItem = vElement.generateItem(nItem.getId());

            return nItem;
        }

        public override bool checkAvaliability(Item theItem)
        {
            return false;
        }
    }
}
