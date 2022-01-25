using System;
using UnityEngine;

namespace simProcess
{
	public class UniformDistribution: DoubleRandomProcess, DoubleProvider
	{
		double min, width;

		RandomGenerator rg;

		public UniformDistribution(double min, double max) 
		{
			this.min = min;
			this.width = max - min;
			rg = new RandomGenerator();
		}

		public double getMin() 
		{
			return min;
		}

		public void setMin(double min) 
		{
			this.min = min;
		}

		public double getMax() 
		{
			return min + width;
		}

		public void setMax(double max) 
		{
			this.width = max - min;
		}

		double DoubleProvider.provideValue() 
		{
			return min + width * rg.nextDouble();
		}

		double DoubleRandomProcess.nextValue() 
		{
			return min + width * rg.nextDouble();
		}

		void DoubleRandomProcess.initialize(double initialValue, double[] parameters) 
		{
			min = parameters[0];
			width = parameters[1] - min;
		}
	}
}

