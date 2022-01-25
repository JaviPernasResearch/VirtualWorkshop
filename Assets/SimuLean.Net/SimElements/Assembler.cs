using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
/* @author Javi Pernas  */

namespace simProcess
{
    public class Assembler : Element, WorkStation
    {
        ServerProcess theServer;
        DoubleRandomProcess randomTimes;

        Item theItem;

        int requirements;
        int itemsCompleted;
        int[] serverId;

        protected Dictionary<int, Item[]> matchedItems;
        string name;


        public Assembler(string name, SimClock sClock, DoubleRandomProcess randomTimes, int requirements) : base(name, sClock)
        {
            this.randomTimes = randomTimes;
            this.name = name;
            this.requirements = requirements; 
        }

        public override void start()
        {

            theServer = new ServerProcess(this, randomTimes, requirements);

            itemsCompleted = 0;

        }

        override public int getQueueLength() 
        {
            return theServer.getQueueLength();
        }
        override public int getFreeCapacity()
        {
            return requirements - theServer.getQueueLength();
        }

        string WorkStation.getName()
        {
            return name;
        }

        public override bool unblock()
        {
            if (theServer.state == 2)
            {
                if (getOutput().sendItem(theServer.theItem, this))
                {

                    this.setType(0);
                    theServer.state = 0;
                    theServer.theItem = null;
                    theServer.clearList();
                    getInput().notifyAvaliable(this);
                    theItem = null;

                    itemsCompleted++;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        public override bool receive(Item theItem)
        {
            double delay;
            bool notFound = true;

            if (theServer.state == 0)
            {
                if (this.getType().Equals(theItem.getId()))
                {
                    notFound = !notFound;
                }
                else if (this.getType() == 0)
                {
                    this.setType(theItem.getId());
                    notFound = !notFound;
                }
            }
            if (notFound == true)
            {
                return false;
            }
            else
            {
                this.theItem = theItem;
                theServer.addItem(theItem);
                vElement.loadItem(theItem);

                if (theServer.getQueueLength() == requirements)
                {
                    theServer.state = 1;
                    theServer.theItem = theItem;

                    delay = this.randomTimes.nextValue();
                    theServer.lastDelay = delay; //For SimpleTransporter
                    simClock.scheduleEvent(theServer, delay);
                }
                else
                    this.getInput().notifyAvaliable(this);

                return true;
            }
        }

        void WorkStation.completeServerProcess(ServerProcess theProcess)
        {

            if (theServer.state == 1)
            {
                Item theItem;
                ArrayList itemsStoraged = theServer.getItems();

                theItem = (Item)itemsStoraged[0];
                theServer.loadTime = Time.time; // SimpleTransporter
                theItem.vItem = vElement.generateItem(theItem.getId());

                for (int i = 0; i < itemsStoraged.Count; i++)  //Saving items setting the first one as container (previously ordered)
                {
                    theItem.addItem((Item)itemsStoraged[i]);
                }

                if (getOutput().sendItem(theItem, this))
                {

                    this.setType(0);
                    theServer.state = 0;
                    theServer.theItem = null;
                    theServer.clearList();
                    theItem = null;

                    itemsCompleted++;

                    getInput().notifyAvaliable(this);
                }
                else
                {
                    theServer.state = 2;
                    theServer.theItem = theItem;
                    theServer.clearList();
                }
            }
            else if (!this.getInput().notifyAvaliable(this))
            {
                simClock.scheduleEvent(theServer, 2);
                //Case the item is not in the queue, scheduling question
            }
        }

        public override bool checkAvaliability(Item theItem)
        {
            if (this.getType() == theItem.getId()&&theServer.state == 0)
                return true;
            else if (this.getType() ==  0)
                return true;
            else
                return false;
        }

        public Item getItem()
        {
            return theItem;
        }

        //Restarting
        public void setCapacity(int capacity)
        {
            this.requirements = capacity;
        }
    }
}
