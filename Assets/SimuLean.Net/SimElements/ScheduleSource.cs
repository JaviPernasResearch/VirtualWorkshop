using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace simProcess
{
    class ScheduleSource : Element, Eventcs
    {
        Item lastItem;

        int numberIterms;

        String fileName;
        TextReader dataFile;
        //int itemType;
        //double itemCreationTime;

        Queue<Item> itemsInQueue;

        public ScheduleSource(String name, SimClock state, String fileName) : base(name, state)
        {
            this.fileName = fileName;
            dataFile = File.OpenText(fileName);
            itemsInQueue = new Queue<Item>();
        }

        public override void start()
        {
            numberIterms = 0;
            scheduleNext();
        }

        public override bool unblock()
        {
            Item theItem;
            if (itemsInQueue.Count > 0)
            {
                theItem = itemsInQueue.Peek();
                if (this.getOutput().sendItem(theItem, this))
                {
                    itemsInQueue.Dequeue();
                    scheduleNext();
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

            if (this.getOutput().sendItem(lastItem, this))
            {
                scheduleNext();
            }
            else
            {
                itemsInQueue.Enqueue(lastItem);
                scheduleNext();
            }
        }

        Item createItem()
        {
            Item nItem = new Item(simClock.getSimulationTime());

            return nItem;
        }

        public override bool checkAvaliability(Item theItem)
        {
            return false;
        }

        void scheduleNext()
        {
            lastItem = createItem();
            readFileAndSet();
            lastItem.vItem = vElement.generateItem(lastItem.getId());
            numberIterms++;

            if (simClock.getSimulationTime() > lastItem.getCreationTime())
                simClock.scheduleEvent(this, simClock.getSimulationTime());
            else
                simClock.scheduleEvent(this, lastItem.getCreationTime());

        }

        void readFileAndSet() //Lee línea de fichero
        {

            string line;
            line = dataFile.ReadLine();
            string[] bits = line.Split(' ');  

            lastItem.setId(bits[0], int.Parse(bits[1]), int.Parse(bits[2]));
            lastItem.setcreationTime(double.Parse(bits[3]));
            for (int i = 4; i < bits.Length - 1; i = i + 2)
            {
                //lastItem.setDoubleAttrib(bits[i], Double.Parse(bits[i + 1]));

            }

        }

        public Queue<Item> getItems()
        {
            return itemsInQueue;

        }

    }
}

