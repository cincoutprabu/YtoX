//HoroscopeHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Core.Helpers
{
    public enum Zodiac
    {
        Aeries,
        Taurus,
        Gemini,
        Cancer,
        Leo,
        Virgo,
        Libra,
        Scorpio,
        Sagittarius,
        Capricorn,
        Aquarius,
        Pisces
    }

    public enum Planets
    {
        Mercury,
        Venus,
        Mars,
        Jupiter,
        Saturn,
        Sun,
        Moon
    }

    public class HoroscopeHelper
    {
        #region Methods

        public static string GetTodayPrediction(Zodiac moonSign)
        {
            //TODO: invoke a web-service to get today's prediction
            //

            return string.Empty;
        }

        #endregion
    }
}
