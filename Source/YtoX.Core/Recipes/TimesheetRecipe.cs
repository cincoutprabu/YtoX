//TimesheetRecipe.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YtoX.Core.Activities;

namespace YtoX.Core.Recipes
{
    public class TimesheetRecipe : IRecipe
    {
        #region Methods

        //remind me to fill the timesheet every weekday at 6pm evening
        public bool Execute(Dictionary<string, object> arguments)
        {
            if (DateTime.Today.DayOfWeek != DayOfWeek.Saturday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
            {
                int hoursPast6pm = DateTime.Now.Hour - 18;
                if (hoursPast6pm > 0 && hoursPast6pm <= 3) //if time is past 6pm
                {
                    new NotifyActivity().Execute(new Dictionary<string, object>() { { "HeaderText", "YtoX Alert!" }, { "AlertText", "Have you filled your Timesheet today ?" }, { "Persistent", true } });
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
