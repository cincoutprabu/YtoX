//IRecipe.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Core.Recipes
{
    public interface IRecipe
    {
        bool Execute(Dictionary<string, object> arguments);
    }
}
