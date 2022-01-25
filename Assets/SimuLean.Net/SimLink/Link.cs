using System;
using System.Collections.Generic;
using System.Text;

namespace simProcess
{
    public interface Link
    {
        bool sendItem(Item theItem, Element source);

        bool notifyAvaliable(Element source);

    }
}
