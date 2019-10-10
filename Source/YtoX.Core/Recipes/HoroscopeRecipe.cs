//HoroscopeRecipe.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YtoX.Core.Activities;
using YtoX.Core.Helpers;

namespace YtoX.Core.Recipes
{
    public class HoroscopeRecipe : IRecipe
    {
        #region Methods

        //show my horoscope everyday morning at 10am
        public bool Execute(Dictionary<string, object> arguments)
        {
            int hoursPast10am = DateTime.Now.Hour - 10;
            if (hoursPast10am > 0 && hoursPast10am <= 3) //if time is past 10am
            {
                string zodiacString = ((string)arguments["Zodiac"]).ToUpper();
                Zodiac zodiac = (Zodiac)Enum.Parse(typeof(Zodiac), zodiacString);
                string predictionText = HoroscopeHelper.GetTodayPrediction(zodiac);

                string alertText = "Hey " + zodiac.ToString() + "! " + predictionText;
                new NotifyActivity().Execute(new Dictionary<string, object>() { { "HeaderText", "YtoX Alert!" }, { "AlertText", alertText }, { "Persistent", true } });
                return true;
            }

            return false;
        }

        #endregion
    }
}
