using System;
using UnityEngine;

namespace simProcess
{
	public class ProviderSource  : Element
	{

		public bool useSocket = false;
		
		int numberIterms;
        int totOrders;
        int pendingItems;

		DoubleRandomProcess leadTime;

		Item blockedItem;



		public ProviderSource (bool useSocket, String name, SimClock state, double minTime = 30.0, double maxTime = 60.0) : base(name, state)
		{
			this.useSocket = useSocket;

			if (!useSocket) {
				this.leadTime = new UniformDistribution (minTime, maxTime);
			}
		}


		public override void start()
		{
			numberIterms = 0;
			pendingItems = 0;
            totOrders = 0;

            blockedItem = null;
		}

		public void order(int quantity) {
            if (quantity > 0)
            {
                totOrders += quantity;

                pendingItems += quantity;

                OrderArrival newArrival = new OrderArrival(quantity, this);

                double time = leadTime.nextValue();


                simClock.scheduleEvent(newArrival, time);
                Debug.Log(time);
            }
 		}

		public void createItems(int quantity) {
			Item newItem;

			do {
				newItem = createItem();
				if (!getOutput().sendItem(newItem, this)) {
					blockedItem = newItem;
					break;
                }
                else /*if (pendingItems > 0)*/
                {
                    pendingItems--;
                }/* else
                {
                    blockedItem = newItem;
                    break;
                }*/
			} while (pendingItems > 0);
		}

		public override bool unblock()
		{
			Item newItem;
			//newItem = blockedItem
			//while (pendingItems > 0)
			//{
				if (blockedItem == null) {
                //newItem = createItem ();  //JP         
                    this.order(1);
                    return true; 
				} else {
					newItem = blockedItem;
					blockedItem = null;
				}

				if (!getOutput().sendItem(newItem, this)) {
					blockedItem = newItem;
					//break;
				} else {
					pendingItems--;
				}
			//}
			return true;
		}

		public int getNumberItems()
		{
			return numberIterms;
		}
        /*
        public int getPendingItems()
        {
            return pendingItems;
        }*/

        public int getPendingArrivals()
        {
            return totOrders - numberIterms;
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

		Item createItem()
		{
            numberIterms++;
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

