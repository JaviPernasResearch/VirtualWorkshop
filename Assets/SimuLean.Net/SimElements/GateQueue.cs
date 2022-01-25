using System;
using System.Collections.Generic;
using System.Collections;

namespace simProcess
{
	public class GateQueue : Element
	{
		int capacity;
		int pendingRelease;
		int currentItems;

		Item blockedItem;

		Queue<Item> itemsQ;

		public GateQueue(int capacity, String myName, SimClock sClock) : base(myName, sClock)
		{
			this.capacity = capacity;
			itemsQ = new Queue<Item>(capacity);
		}

		public override void start()
		{
			itemsQ.Clear();
			currentItems = 0;
			pendingRelease = 0;

			blockedItem = null;
		}

		public void release (int quantity) {
			pendingRelease += quantity;

            doTransfers();

        }

		void doTransfers() {
			Item theItem;

            while (pendingRelease > 0 && currentItems > 0)
            {
                /**if (currentItems > 0) {*/
                if (blockedItem != null)
                {
                    theItem = blockedItem;
                    blockedItem = null;
                }
                else
                {
                    theItem = itemsQ.Dequeue();
                }

                pendingRelease--;
                currentItems--;

                if (getOutput().sendItem(theItem, this))
                {

                    vElement.reportState("Exit");
                    //getInput().notifyAvaliable(this);
                }
                else
                {
                    pendingRelease++;
                    currentItems++;
                    blockedItem = theItem;
                    break;
                }
                /*}*/
            }         
			
		}

		override public int getQueueLength()
		{
            if (blockedItem == null)
            {
                return currentItems;
            }
            else
            {
                return currentItems + 1;
            }
			
		}

		public int getPendingItems()
		{
			return pendingRelease;
		}

		override public int getFreeCapacity()
		{
			return capacity - currentItems;
		}

		public override bool unblock()
		{
			if (currentItems > 0)
			{
				doTransfers ();
				return true;
			}
            else
			{
				return false;
			}
		}


		public override bool receive(Item theItem)
		{
			if (currentItems < capacity)
			{
				currentItems++;
				itemsQ.Enqueue(theItem);
				vElement.loadItem(theItem);

				doTransfers ();
				return true;
			}
			else
			{
				return false;
			}
		}

		public override bool checkAvaliability(Item theItem)
		{
			return !(currentItems >= capacity);
		}

		public Queue<Item> getItems()
		{
			Queue<Item> wholeItems = new Queue<Item>();

			if (blockedItem != null) {
				wholeItems.Enqueue (blockedItem);
			}

			foreach (Item it in  itemsQ) {
				wholeItems.Enqueue (it);
			}

			return wholeItems;
		}

        //Restarting
        public void setCapacity(int capacity)
        {
            this.capacity = capacity;
        }
    }
}

