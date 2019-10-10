//Activity.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Entities
{
    /// <summary>
    /// Represents an action that can take place in the Ultrabook in response to triggers.
    /// Examples:
    ///     show a notification
    ///     open an app
    ///     open a URL
    ///     call a web service that does something cool
    ///     create a shortcut/pinnable-item on Start screen
    ///     create a shortcut on the desktop
    ///     mute the volume
    ///     blackout the screen
    ///     go to start screen
    ///     set the volume to 100% (when i play a movie)
    ///     copy to dropbox
    ///     import photos (copy to pictures folder) when i connect my iPhone
    ///     start audio recording when i am in a meeting
    ///     start webcam
    ///     
    /// </summary>
    public class Activity
    {
        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        #endregion

        #region Constructors

        public Activity()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.ImagePath = string.Empty;
        }

        #endregion
    }
}
