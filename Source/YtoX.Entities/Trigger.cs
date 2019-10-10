//Trigger.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using YtoX.Entities.Expression;

namespace YtoX.Entities
{
    /// <summary>
    /// Represents an event that occurs on the Ultrabook which can be based on weather, time, news, WiFi, battery, location, app-contracts, etc.
    /// Events can originate from various devices available in the Ultrabook, namely:
    ///     GPS
    ///         - post the location to FB (or tweet the location) when i'm in my favorite location/restaurant for more than 20 minutes
    ///         - turn wifi off when i leave from work
    ///         - turn wifi on when i arrive at work
    ///         - launch calendar/outlook when i arrive at work
    ///         - remind me to fill timesheet by 6pm every weekday
    ///         - when i start from work, remind me to buy milk
    ///         - launch the music app while i am driving
    ///         - remind me to visit the gym if haven't been there for 3 days
    ///         - remind me to leave the bike/car for service, if i haven't visited the showroom/workshop for 3 months
    ///         - notify me if i'm not traveling in my usual/known paths
    ///     Ambient Light Sensor
    ///     Accelerometer
    ///     Gyroscope
    ///     Compass
    ///         - shutdown the PC when i am traveling in north direction for 1 hour.
    ///     Mic
    ///         - when i am in a noisy place and i am not driving, suggest me to turn the music on & to wear the headphones
    ///     Speaker
    ///         - mute the pc, when an appointment starts, and revert back when the appointments ends
    ///         - start playback, when a wired headset is connected
    ///     WiFi
    ///         - every mrng, post 'happy birthday' on wall of frnds whose birthday is today
    ///         - show me the scorpio horoscope everyday at 8am
    ///         - email me alerts related to President Obama every day at 9am
    ///         - show me the weather forecast every day at 8am if the expected temperature is below 50*F
    ///         - remind me to take umbrella every day the first time i turn on the laptop, if it is going to be rainy
    ///     Ethernet Adapter
    ///     USB ports
    ///     Power Socket
    ///     Card reader slot
    ///     CD/DVD drive
    ///     Laptop lid/display
    ///         - mute the system if i close (or tilt to certain degrees) the ultrabook lid/display
    ///         - turn off the wifi when i close the lid and turn it on when i open the lid
    ///     Mouse and mouse-buttons
    ///     Keyboard, numpad
    ///         - launch the calculator when i turn on the num-lock.
    ///         - create a shortcut on the desktop to the current website i am looking in the browser, on pressing a shortcut key
    ///     Trackpad and trackpad-buttons
    ///     Clock/calendar
    ///         - remind me of my first appointment tomorrow when i open the lid after 10pm
    ///         - automatically reply 'i am in a meeting, will ping you back', when one of the contacts pings me during a meeting.
    /// </summary>
    public class Trigger
    {
        #region Fields

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public System.Linq.Expressions.Expression Condition { get; set; }
        //public List<Condition> Conditions { get; set; }
        //public List<LogicalOp> Operators { get; set; }

        #endregion

        #region Constructors

        public Trigger()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.ImagePath = string.Empty;

            this.Condition = null;
            //this.Conditions = new List<Condition>();
            //this.Operators = new List<LogicalOp>();
        }

        #endregion
    }
}
