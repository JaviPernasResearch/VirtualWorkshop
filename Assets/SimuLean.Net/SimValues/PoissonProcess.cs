using System;
using System.Collections.Generic;
using System.Text;

namespace simProcess
{
    public class PoissonProcess : DoubleRandomProcess, DoubleProvider
    {
        double mean;

        RandomGenerator rg;

        public PoissonProcess(double mean) 
        {
            this.mean = mean;
            rg = new RandomGenerator();
        }

        public double getMean() 
        {
            return mean;
        }

        public void setMean(double mean) 
        {
            this.mean = mean;
        }

        double DoubleProvider.provideValue() 
        {
            return -Math.Log(1 - rg.nextDouble()) * mean;
        }
    
        double DoubleRandomProcess.nextValue() 
        {
            return -Math.Log(1 - rg.nextDouble()) * mean;
        }

        void DoubleRandomProcess.initialize(double initialValue, double[] parameters) 
        {
            mean = parameters[0];
        }
    }
}
