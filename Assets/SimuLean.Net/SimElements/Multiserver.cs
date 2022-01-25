using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace simProcess
{
    public class MultiServer : Element, WorkStation
    {
        Queue<ServerProcess> idleProccesses;
        public List<ServerProcess> workInProgress;
        Queue<ServerProcess> completed;

        int currentItems;
        int capacity;

        DoubleRandomProcess[] randomTimes;
        string name;


        public MultiServer(DoubleRandomProcess[] randomTimes, String name, SimClock sClock) : base(name, sClock) //Name me identifica el tiempo que pasa el item
        {
            idleProccesses = new Queue<ServerProcess>(randomTimes.Length); //No empleo Randomtimes para nada, los tiempos vienen en los Items
            workInProgress = new List<ServerProcess>(randomTimes.Length);
            completed = new Queue<ServerProcess>(randomTimes.Length);

            this.randomTimes = randomTimes;
            this.name = name;

            this.capacity = randomTimes.Length;
        }

        public override void start()
        {

            ServerProcess theServer;
            idleProccesses.Clear();
            workInProgress.Clear();
            completed.Clear();

            for (int i = 0; i < capacity; i++)
            {
                theServer = new ServerProcess(this, randomTimes[i], 1);
                idleProccesses.Enqueue(theServer);
            }

            currentItems = 0;
            //pendingRequests = 0;
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
            if (completed.Count > 0)
            {
                ServerProcess theProcess;
                Item theItem;

                theProcess = completed.Dequeue();
                idleProccesses.Enqueue(theProcess);
                theItem = theProcess.theItem;
                currentItems--;
                vElement.reportState("Exit");

                getOutput().sendItem(theItem, this);
                getInput().notifyAvaliable(this);

                return true;
            }
            else
            {
                return false;
            }
        }


        public override bool receive(Item theItem)
        {
            double delay;
            if (currentItems >= capacity)
            {
                return false;
            }
            else
            {
                ServerProcess theProcess;

                currentItems++;

                theProcess = idleProccesses.Dequeue();
                theProcess.theItem = theItem;
                theProcess.loadTime = simClock.getSimulationTime();
                workInProgress.Add(theProcess);

                vElement.loadItem(theItem);

                delay = theProcess.getDelay(); //me devuelve el delay correspondiente según el nombre del multiserver
                theProcess.lastDelay = delay;
                simClock.scheduleEvent(theProcess, delay);

                return true;
            }
        }
        void WorkStation.completeServerProcess(ServerProcess theProcess)
        {
            Item theItem = theProcess.theItem;

            workInProgress.Remove(theProcess);

            if (getOutput().sendItem(theItem, this))
            {
                idleProccesses.Enqueue(theProcess);
                currentItems--;
                vElement.reportState("Exit");
                getInput().notifyAvaliable(this);

            }
            else
            {
                completed.Enqueue(theProcess);
            }
        }

        public override bool checkAvaliability(Item theItem)
        {
            return !(currentItems >= capacity);
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
