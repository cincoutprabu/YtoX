//ITrigger.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Core.Triggers
{
    interface ITrigger
    {
        bool Execute(Dictionary<string, object> arguments);
    }
}
