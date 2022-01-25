using System;
using System.Collections.Generic;
using System.Text;

namespace simProcess
{
    public interface DoubleRandomProcess
    {
        void initialize(double initialValue, double[] parameters);

        double nextValue();
    }
}
