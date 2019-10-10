//NewsEntry.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Entities
{
    public class NewsEntry
    {
        #region Properties

        public string Provider { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }

        public string EmailId { get; set; }
        public bool EmailSent { get; set; }

        #endregion

        #region Constructors

        public NewsEntry()
        {
            Provider = string.Empty;
            Title = string.Empty;
            Summary = string.Empty;
            Url = string.Empty;

            EmailId = string.Empty;
            EmailSent = false;
        }

        #endregion
    }
}
