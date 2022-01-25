using System;
using System.Collections.Generic;
using System.Collections;

namespace simProcess
{
    public class ItemsQueue : Element
    {
        int capacity;
        int currentItems;

        Queue<Item> itemsQ;

        public ItemsQueue(int capacity, String myName, SimClock sClock) : base(myName, sClock)
        {
            this.capacity = capacity;
            itemsQ = new Queue<Item>(capacity);
        }

        public override void start()
        {
            itemsQ.Clear();
            currentItems = 0;
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
            if (itemsQ.Count > 0)
            {
                Item theItem = itemsQ.Dequeue();
                currentItems--;
                vElement.reportState("Exit");

                getOutput().sendItem(theItem, this);

                getInput().notifyAvaliable(this);
                return true;
            }
            else if (capacity == 0)
            {

                return getInput().notifyAvaliable(this);
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
                if (!getOutput().sendItem(theItem, this))
                {
                    itemsQ.Enqueue(theItem);
                    currentItems++;
                    vElement.loadItem(theItem);
                }
                return true;
            }
            else if (capacity == 0)
            {
                if (getOutput().sendItem(theItem, this))
                {
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

        public override bool checkAvaliability(Item theItem)
        {
            return !(currentItems >= capacity);
        }

        public Queue<Item> getItems()
        {
            return itemsQ;
        }

        //Restarting
        public void setCapacity(int capacity)
        {
            this.capacity = capacity;
        }
    }
}
