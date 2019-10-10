//BuyMilkRecipe.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YtoX.Core.Activities;
using YtoX.Core.Location;

namespace YtoX.Core.Recipes
{
    public class BuyMilkRecipe : IRecipe
    {
        #region Methods

        //remind me to buy milk every weekday when i leave from work
        public bool Execute(Dictionary<string, object> arguments)
        {
            //if (KnownLocations.Current != null && KnownLocations.Work != null)
            //{
            //    if (DateTime.Today.DayOfWeek != DayOfWeek.Saturday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
            //    {
            //        if (true) //TODO: condition to check whether I've been to work today
            //        {
            //            double distance = PathFinding.FindDistance(KnownLocations.Current, KnownLocations.Work);
            //            if (distance >= 10)
            //            {
            //                string alertText = "Hope you had a great day at Work! Don't get under the nose of your wife, buy Milk now!!";
            //                new NotifyActivity().Execute(new Dictionary<string, object>() { { "HeaderText", "YtoX Alert!" }, { "AlertText", alertText } });
            //                return true;
            //            }
            //        }
            //    }
            //}

            return false;
        }

        #endregion
    }
}
