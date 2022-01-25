using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace simProcess
{
	public class MultiAssembler: Element, WorkStation, ArrivalListener
	{
		//Realiza las operaciones de subloques y bloques, exceptuando las de ensamblado
		Queue<ServerProcess> idleProccesses;
		public List<ServerProcess> workInProgress;
		Queue<ServerProcess> completed;

		bool batchMode;
		int completedItems;
		int capacity;
        
		ConstrainedInput [] inputs;
		int [] requirements;

		DoubleRandomProcess delay; //Es 1
		string name;

        bool receivingItems = false;

		public MultiAssembler(int capacity, int [] requirements, DoubleRandomProcess delay, String name, SimClock sClock, bool batchMode = false) : base(name, sClock) //Name me identifica el tiempo que pasa el item
		{
			idleProccesses = new Queue<ServerProcess>(capacity);
			workInProgress = new List<ServerProcess>(capacity);
			completed = new Queue<ServerProcess>(capacity);

			this.requirements = requirements;

			this.delay = delay;
			this.name = name;
			this.batchMode = batchMode;

			this.capacity = capacity;

			inputs = new ConstrainedInput[requirements.Length];
			for (int i = 0; i < inputs.Length; i++) {
				inputs [i] = new ConstrainedInput (requirements [i], this, i, this.name + ".Input" + i, simClock);
			}
		}

		public override void start()
		{
			ServerProcess theServer;

			idleProccesses.Clear();
			workInProgress.Clear();
			completed.Clear();

			for (int i = 0; i < capacity; i++)
			{
				theServer = new ServerProcess(this, delay, 1);
				idleProccesses.Enqueue(theServer);
			}


			for (int i = 0; i < inputs.Length; i++)
            {
				inputs [i].start ();
			}

            completedItems = 0;

            //pendingRequests = 0;
        }

		public ConstrainedInput getInput(int i) {
			return inputs [i];
		}

        public int getInputsCount() {
			return inputs.Length;
		}


		override public int getQueueLength()
		{
            int q = 0;

            foreach (ConstrainedInput ci in inputs)
            {
                q += ci.getQueueLength();
            }

			return workInProgress.Count + completed.Count + q;
		}
		override public int getFreeCapacity()
		{
			return capacity;
		}

        public int getCompletedItems()
        {
            return completedItems;
        }

		string WorkStation.getName()
		{
			return name;
		}

		//Desbloquea la estación de trabajo cuando aguas abajo queda libre
		public override bool unblock()
		{
			if (completed.Count > 0)
			{
				ServerProcess theProcess;
				Item theItem;

				theProcess = completed.Peek();
				theItem = theProcess.theItem;

				if(getOutput().sendItem(theItem, this))
                {

                    completed.Dequeue();

                    idleProccesses.Enqueue(theProcess);

                    vElement.reportState("Exit");

                    checkRequirements();

                    return true;
                }
                else
                {
                    return false;
                }

			}
			else
			{
				return false;
			}
		}


		public override bool receive(Item theItem)
		{
			return true;
		}

		void ArrivalListener.itemReceived(Item theItem, int source) {
            
            //Not to overlap two deliveries
            if (!receivingItems)
            {
                checkRequirements();
            }
		}

        VElement ArrivalListener.getVElement()
        {
            return vElement;
        }

        void checkRequirements() {
			ServerProcess theProcess;
			bool ready = true;
			Item newItem; 
			Queue<Item> items;
			double thisDelay;

			if (idleProccesses.Count > 0) {
				for (int i = 0; i < inputs.Length; i++) {
					if (inputs [i].getQueueLength () < requirements [i]) {
						ready = false;
					}
				}

				if (ready) {
                    completedItems++;
                    receivingItems = true;
                    newItem = createNewItem();

                    theProcess = idleProccesses.Dequeue();

                    for (int i = 0; i < inputs.Length; i++) {
						items = inputs [i].release (requirements [i]);


						foreach (Item it in items) {
							if (batchMode) {
								vElement.unloadItem (it);
								newItem.addItem (it);
							} else {
								vElement.unloadItem (it);
							}
						} 

						items.Clear ();
					}

                    
                    receivingItems = false;
                    checkRequirements();

					theProcess.theItem = newItem;


					theProcess.loadTime = simClock.getSimulationTime();
					workInProgress.Add(theProcess);

                    //vElement.loadItem(newItem);
                    vElement.reportState("Sort");

                    thisDelay = this.delay.nextValue();

                    Debug.Log("Assembler delay " + thisDelay);

					theProcess.lastDelay = thisDelay;
					simClock.scheduleEvent(theProcess, thisDelay);
                    SimCosts.addCost(SimCosts.processingCost);


                }
			}
		}

		Item createNewItem() {
			Item newItem = new Item (simClock.getSimulationTime());
            newItem.setId("type", 1, 1);

            newItem.vItem = vElement.generateItem (0);

			return newItem;
		}


		void WorkStation.completeServerProcess(ServerProcess theProcess)
		{
			Item theItem = theProcess.theItem;

			workInProgress.Remove(theProcess);


            Debug.Log("Process completed ");

            if (getOutput().sendItem(theItem, this))
			{
                Debug.Log("Assembler Sent Item ");

                idleProccesses.Enqueue(theProcess);

				vElement.reportState("Exit");

				checkRequirements ();

			}
			else
			{
                Debug.Log("Assembler blocked ");
                completed.Enqueue(theProcess);
			}
		}

		public override bool checkAvaliability(Item theItem)
		{
			return true;
		}


		//Getting all the current items to display them
		public Queue<Item> getItems()
		{
			Queue<Item> myItems = new Queue<Item>() ;
			foreach (ServerProcess sp in workInProgress)
			{
				myItems.Enqueue(sp.getCurrentItem());
			}

			foreach(ServerProcess sp in completed)
			{
				myItems.Enqueue(sp.getCurrentItem());
			}

			return myItems;

		}

        //Restarting
        public void setCapacity(int capacity)
        {
            this.capacity = capacity;
        }

	}
}

