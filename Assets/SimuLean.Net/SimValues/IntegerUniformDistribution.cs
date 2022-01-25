using System;

namespace simProcess
{
	public class IntegerUniformDistribution: IntegerProvider
	{
		int min, max;

		RandomGenerator rg;

		public IntegerUniformDistribution(int min, int max) 
		{
			this.min = min;
			this.max = max;
			rg = new RandomGenerator();
		}

		public int getMin() 
		{
			return min;
		}

		public void setMin(int min) 
		{
			this.min = min;
		}

		public int getMax() 
		{
			return max;
		}

		public void setMax(int max) 
		{
			this.max = max;
		}

		int IntegerProvider.provideValue() 
		{
			return (int) rg.nextInt(min, max);
		}

	}
}

