//LocationEntry.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Entities
{
    public class LocationEntry
    {
        #region Properties

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime VisitedOn { get; set; }
        public bool IsKnown { get; set; }
        public string KnownKey { get; set; }

        #endregion

        #region Methods

        public LocationEntry()
        {
            this.Latitude = double.MaxValue;
            this.Longitude = double.MaxValue;
            this.VisitedOn = default(DateTime);
            this.IsKnown = false;
            this.KnownKey = string.Empty;
        }

        #endregion
    }
}
