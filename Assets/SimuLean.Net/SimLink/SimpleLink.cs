﻿using System;
using System.Collections.Generic;
using System.Text;

namespace simProcess
{
    public class SimpleLink : Link
    {
        Element origin;
        Element destination;

        Link thislink;

        static public void createLink(Element origin, Element destination)
        {
            SimpleLink theLink = new SimpleLink(origin, destination);

            origin.setOutput(theLink);
            destination.setInput(theLink);
        }

        public SimpleLink(Element origin, Element destination)
        {
            thislink = this;
            this.origin = origin;
            this.destination = destination;

        }

        bool Link.sendItem(Item theItem, Element source)
        {
            if (destination.receive(theItem))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool Link.notifyAvaliable(Element source)
        {
            return origin.unblock();
        }
    }
}
