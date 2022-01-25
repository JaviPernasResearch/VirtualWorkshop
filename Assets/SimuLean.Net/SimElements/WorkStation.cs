using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace simProcess
{
    public interface WorkStation
    {
        void completeServerProcess(ServerProcess theProcess);
        string getName();
    }
}
