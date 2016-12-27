using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ams
{
    public enum TranState
    {
        New = 1,
        Accepted,
        Rejected,
        Finished,
    }
}