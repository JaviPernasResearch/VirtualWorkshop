using System;

namespace simProcess
{
	public interface ArrivalListener
	{
		void itemReceived(Item theItem, int source);

        VElement getVElement();
    }
}

