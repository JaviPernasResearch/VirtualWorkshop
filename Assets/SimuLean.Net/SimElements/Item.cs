using System;
using System.Collections.Generic;
using System.Text;

namespace simProcess
{
    public class Item
    {
        static int ITEM_NUMBER = 0;

        double creationTime;

        private string type;
        private int myId;
        private int myConstrainedInput;

        public int priority;

        public object vItem;

        public List<Item> subItems; //en uso

        protected Dictionary<string, double> attribDouble;


        //protected Dictionary<string, int> attribInt;
        //protected Dictionary<string, string> attribStrings;


        public Item(double creationTime)
        {
            Item.ITEM_NUMBER++;

            this.creationTime = creationTime;
        }

        public void setId(string type, int myId, int priority) //tres atributos fundamentales del Item
        {

            this.type = type;
            this.myId = myId;
            this.priority = priority;
        }

        public int getId()
        {
            return myId;
        }

        public void setcreationTime(double creationTime)
        {
            this.creationTime = creationTime;
        }

        public double getCreationTime()
        {
            return creationTime;
        }

        public void setConstrainedInput(int myConstrainedInput)
        {
            this.myConstrainedInput = myConstrainedInput;
        }

        public int getConstrainedInput()
        {
            return myConstrainedInput;
        }

        public void addItem(Item theItem)
        {
            if (subItems == null)
            {
                subItems = new List<Item>();
            }
            subItems.Add(theItem);
        }

        public List<Item> getSubItems()
        {
            return subItems;
        }

        //Case Deterministic MODEL
        //public void setDoubleAttrib(string attribName, double value)
        //{
        //    if (attribDouble == null)
        //    {
        //        attribDouble = new Dictionary<string, double>();
        //    }
        //    attribDouble.Add(attribName, value);
        //}
        //public double getDoubleAttrib(string aName)
        //{
        //    if (attribDouble == null)
        //    {
        //        return 0.0;
        //    }
        //    return attribDouble[aName];
        //}
        //public Dictionary<string, double> getDoubleAttributes()
        //{
        //    if (attribDouble == null)
        //    {
        //        return null;
        //    }
        //    return attribDouble;
        //}


    }
}
