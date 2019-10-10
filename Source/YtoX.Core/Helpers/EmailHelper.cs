//EmailUtility.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace YtoX.Core.Helpers
{
    public class EmailHelper
    {
        #region Methods

        public static bool SendEmail(string to, string subject, string body)
        {
            try
            {
                //set smtp-settings
                SmtpClient smtp = new SmtpClient("mail.ytox.net", 25);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential("ytox@ytox.net", "Temp_Temp");
                smtp.EnableSsl = false;

                //build mail-message and send
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("ytox@ytox.net", "YtoX");
                mail.To.Add(to);
                mail.Subject = subject;

                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Body = body;

                //send email
                smtp.Send(mail);
                return true;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                return false;
            }
        }

        #endregion
    }
}
