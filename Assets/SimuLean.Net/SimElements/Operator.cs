using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace simProcess
{
    public class Operator : Element, WorkStation
    {
        ServerProcess theProcess;

		int currentItems;
		int capacity;

        string name;

        public bool atPickPoint;




        public Operator(String name, SimClock sClock, int capacity) : base(name, sClock)
        {
            this.name = name;
			this.capacity = capacity;
        }

        public override void start()
        {

            theProcess = new ServerProcess(this, new PoissonProcess(1), 1);

            currentItems = 0;
            atPickPoint = false;
        }

        override public int getQueueLength()
        {
			return currentItems;
        }
        override public int getFreeCapacity()
        {
            return capacity - currentItems;
        }

        string WorkStation.getName()
        {
            return name;
        }

        //Desbloquea la estación de trabajo cuando aguas abajo queda libre
        public override bool unblock()
        {
            if (theProcess.state == 2)
            {
                ArrayList itemsStoraged = theProcess.getItems();
                ArrayList itemsToRemove = new ArrayList();

                foreach (Item it in itemsStoraged)
                {
                    if (getOutput().sendItem(it, this))
                    {
                        itemsToRemove.Add(it);
                        currentItems--;
                        vElement.reportState("Exit 1"); //Tiene que liberar un item
                    }
                    else
                    {
                        foreach (Item itt in itemsToRemove)
                        {
                            itemsStoraged.Remove(itt);
                        }
                        theProcess.state = 2;
                        return false;
                    }

                    vElement.reportState("Exit all");    
                }
            }
            else
            {
                return false;
            }

            theProcess.state = 0;
            theProcess.clearList();
            getInput().notifyAvaliable(this);

            return true;
        }


        public override bool receive(Item theItem)
        {

            if (currentItems >= capacity || theProcess.state == 2)
            {
                return false;
            }
            else
            {
				if(atPickPoint == true)  //Entra cuando el operador está en el punto de recogida
				{
                    theProcess.addItem(theItem);
                    currentItems++;

                    if (currentItems >= capacity)
                    {
                        Item myItems = theItem;
                        foreach (Item it in theProcess.getItems())
                        {
                            myItems.addItem(it);
                        }

                        vElement.unloadItem(myItems);
                    }

                    return true;
				}
	
				vElement.loadItem(theItem);	

                return false;
            }
        }

        void WorkStation.completeServerProcess(ServerProcess theProcess)
        {

            ArrayList itemsStoraged = theProcess.getItems();
            ArrayList itemsToRemove = new ArrayList();


            foreach (Item it in theProcess.getItems())
            {
				
				if (getOutput().sendItem(it, this))
				{
                    itemsToRemove.Add(it);
					currentItems--;
					vElement.reportState("Exit 1"); //Tiene que liberar un item
				}
				else
				{
                    foreach (Item itt in itemsToRemove)
                    {
                        itemsStoraged.Remove(itt);
                    }
                    theProcess.state = 2;
					return;
				}

			}

            theProcess.clearList();
            getInput().notifyAvaliable(this);

            return;

        }

        public override bool checkAvaliability(Item theItem)
        {
            return !(currentItems >= capacity);
        }


        //Getting all the current items to display them
        //public Queue<Item> getItems()
        //{
        //    Queue<Item> myItems = new Queue<Item>();
        //    foreach (ServerProcess sp in workInProgress)
        //    {
        //        myItems.Enqueue(sp.getCurrentItem());
        //    }

        //    foreach (ServerProcess sp in completed)
        //    {
        //        myItems.Enqueue(sp.getCurrentItem());
        //    }

        //    return myItems;

        //}
		
		public void pickItem() //Called once the operator arrives at the origin
		{
			getInput().notifyAvaliable(this);
				
		}
		
		public void leaveItem() //Called once the operator arrives at the destination
		{
            simClock.scheduleEvent(theProcess, 0.0);

        }

    }
}
