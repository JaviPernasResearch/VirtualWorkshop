using System;

namespace simProcess
{
	public class OrderArrival : Eventcs
	{
		ProviderSource source;

		int quantity;
		
		public OrderArrival (int quantity, ProviderSource source)
		{
			this.quantity = quantity;
			this.source = source;
		}

		void Eventcs.execute()
		{
			source.createItems(quantity);
		}
	}
}

