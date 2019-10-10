//Recipe.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Entities
{
    public class Recipe
    {
        #region Fields

        public long GroupUID { get; set; }
        public string Name { get; set; }
        public string Tagline { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsEnabled { get; set; }
        public double ScopeHours { get; set; }

        public string DisplayDescription
        {
            get
            {
                switch (this.Name)
                {
                    case "Temperature Drop":
                        return string.Format(this.Description, Constants.TEMPERATURE_ALERT_THRESHOLD);
                    case "Car Service":
                        return string.Format(this.Description, Constants.CAR_SERVICE_INTERVAL);
                    case "Gym":
                        return string.Format(this.Description, Constants.MAX_GYM_VACATION);
                    case "Busy":
                        return string.Format(this.Description, Constants.MEETING_AUTOREPLY_MESSAGE);
                    case "Battery Low":
                        return string.Format(this.Description, Constants.BATTERYLOW_AUTOREPLY_MESSAGE, Constants.BATTERY_WARNING_THRESHOLD);
                    case "Follow Topics":
                        return string.Format(this.Description, Constants.NEWS_TOPICS);
                    case "Follow People":
                        return string.Format(this.Description, Constants.NEWS_PEOPLE);
                    case "Meeting":
                        return string.Format(this.Description, Constants.MEETING_ALERT_HOUR);
                    case "Favorite App":
                        return string.Format(this.Description, Constants.APP_TO_LAUNCH_ON_SHAKE);
                    case "Landscape":
                        return string.Format(this.Description, Constants.AppComments[Constants.APP_TO_LAUNCH_ON_LANDSCAPE]);
                    default:
                        return this.Description;
                }
            }
        }

        public string ScopeDescription
        {
            get
            {
                if (this.ScopeHours == -1) return string.Empty;
                return "Scope: " + this.ScopeHours + " hours";
            }
        }

        public Trigger Trigger { get; set; }
        public Activity Activity { get; set; }

        #endregion

        #region Constructors

        public Recipe()
        {
            this.GroupUID = -1;
            this.Name = string.Empty;
            this.Tagline = string.Empty;
            this.Description = string.Empty;
            this.ImagePath = string.Empty;
            this.IsAvailable = false;
            this.IsEnabled = false;
            this.ScopeHours = -1;

            this.Trigger = new Trigger();
            this.Activity = new Activity();
        }

        #endregion
    }
}
