using System;
using System.Collections.Generic;
using System.Collections;

namespace simProcess
{
	public class ConstrainedInput : Element
	{
		int capacity;
		int currentItems;

        int inputId;

		Queue<Item> itemsQ;

		ArrivalListener aListener;

		public ConstrainedInput(int capacity, ArrivalListener aListener, int inputId, String myName, SimClock sClock) : base(myName, sClock)
		{
			this.aListener = aListener;
			this.capacity = capacity;
            this.inputId = inputId;

            itemsQ = new Queue<Item>(capacity);
		}

		public override void start()
		{
			itemsQ.Clear();
			currentItems = 0;
		}

		public Queue<Item> release (int quantity) {
			Queue<Item> wholeItems = new Queue<Item>();

			for (int i = 0; i < quantity; i++) {
				wholeItems.Enqueue (itemsQ.Dequeue());
				currentItems--;

                getInput().notifyAvaliable(this);
			}

			return wholeItems;
		}

		override public int getQueueLength()
		{
			return currentItems;
		}
		override public int getFreeCapacity()
		{
			return capacity - currentItems;
		}

		public override bool unblock()
		{
			return true;
		}


		public override bool receive(Item theItem)
		{
			if (currentItems < capacity || capacity < 0)
			{
				currentItems++;
                theItem.setConstrainedInput(this.inputId); //For item placement
				itemsQ.Enqueue(theItem);
				aListener.getVElement().loadItem(theItem);

                aListener.itemReceived(theItem, inputId);


                return true;
			}
			else
			{
				return false;
			}
		}

		public override bool checkAvaliability(Item theItem)
		{
			return !(currentItems >= capacity || capacity < 0);
		}

        public int getCapacity()
        {
            return capacity;
        }

        public Queue<Item> getItems()
        {
            return itemsQ;
        }
    }
}

