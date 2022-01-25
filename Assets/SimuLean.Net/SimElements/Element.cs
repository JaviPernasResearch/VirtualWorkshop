using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace simProcess
{
    public abstract class Element
    {

        private Link input, output;

        protected SimClock simClock;

        public VElement vElement;

        readonly string name;

        private int type;
        //0:no type, no item in process

        static ArrayList elements;

        public static ArrayList getElements()
        {
            if (elements == null)
            {
                elements = new ArrayList();
            }
            return elements;
        }

        public static int getInventory()
        {
            ArrayList elems = getElements();
            int inv = 0;

            foreach (Element e in elements)
            {
                inv += e.getQueueLength();
            }

            return inv;
        }


        public Element(string name, SimClock simClock)
        {
            this.name = name;
            this.simClock = simClock;

            getElements().Add(this);

        }

        public Link getInput()
        {
            return input;
        }

        public void setInput(Link input)
        {
            this.input = input;
        }

        public Link getOutput()
        {
            return output;
        }

        public void setOutput(Link output)
        {
            this.output = output;
        }

        public int getType()
        {
            return type;
        }

        public void setType(int type)
        {
            this.type = type;
        }

        public abstract void start();

        // Get the current items in the element

        public abstract int getQueueLength();

        // Get the free slots for receiving items, -1 if infinite

        public abstract int getFreeCapacity();

        // Input connector methods

        public abstract bool checkAvaliability(Item theItem);

        public abstract bool receive(Item theItem);


        // Output connector methods

        public abstract bool unblock();

        /*public abstract bool notifyRequest();*/

    }
}
