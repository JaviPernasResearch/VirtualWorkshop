using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace simProcess
{
    public class ServerProcess : Eventcs
    {
        WorkStation myServer;

        public Item theItem;

        public double loadTime = 0.0;
        public double lastDelay = 0.0;

        int capacity;

        DoubleRandomProcess delay;
        ArrayList itemsOrdered;

        public int state = 0;
        //0: idle
        //1: bussy
        //2: blocked

        int typeItem;
        //0:no type

        public ServerProcess(WorkStation myServer, DoubleRandomProcess randomDelay, int capacity)
        {
            this.myServer = myServer;

            delay = randomDelay;

            state = 0;

            this.capacity = capacity;

            itemsOrdered = new ArrayList(capacity); //debería recibir capacity y establecer su capacidad de inicio

            typeItem = 0;
        }

        public double getDelay()
        {
            double delay = this.delay.nextValue();

            return delay;
        }

        void Eventcs.execute()
        {
            myServer.completeServerProcess(this);
        }


        //Hecho para Assembler

        public void addItem(Item theItem) //Sorting itens according to priority
        {

            if (typeItem == 0)
            {
                itemsOrdered.Clear();
                typeItem = theItem.getId();
            }

            itemsOrdered.Add(theItem);

            if (itemsOrdered.Count > 1)
            {
                for (int i = itemsOrdered.Count - 2; i >= 0; i--)
                {
                    Item oneItem = (Item)itemsOrdered[i];
                    if (theItem.priority < oneItem.priority)
                    {
                        swap(i);
                    }
                    else
                        i = -1;
                }
            }
        }

        private void swap(int i)
        {
            Item intermItem = (Item)itemsOrdered[i];
            itemsOrdered[i] = itemsOrdered[i + 1];
            itemsOrdered[i + 1] = intermItem;
        }

        //Case Assembler
        public ArrayList getItems()
        {
            typeItem = 0;

            return itemsOrdered;
        }

        //Case Multiserver (itemsOrdered is always null)
        public Item getCurrentItem()
        {
            return theItem;
        }

        public int getQueueLength()
        {
            return itemsOrdered.Count;
        }

        public int getTypeProcess()
        {
            return typeItem;
        }

        public void clearList()
        {
            itemsOrdered.Clear();
        }

    }
}
