//IActivity.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Core.Activities
{
    interface IActivity
    {
        void Execute(Dictionary<string, object> arguments);
    }
}
