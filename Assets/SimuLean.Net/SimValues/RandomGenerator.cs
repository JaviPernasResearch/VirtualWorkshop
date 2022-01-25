using System;
using System.Collections.Generic;
using System.Text;

namespace simProcess
{
    class RandomGenerator
    {
        System.Random r;

        public RandomGenerator()
        {
            r = new Random(DateTime.Now.Millisecond);
        }

        public double nextDouble()
        {
            return r.NextDouble();
        }

        public double nextInt(int minValue, int maxValue)
        {
            return r.Next(minValue, maxValue);
        }

    }
}
