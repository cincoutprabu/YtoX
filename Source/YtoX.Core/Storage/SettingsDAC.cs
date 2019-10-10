//SettingsDAC.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YtoX.Core.Storage
{
    public class SettingsDAC
    {
        #region Methods

        public static string Get(string key)
        {
            try
            {
                string statement = string.Format("SELECT SettingValue FROM Settings WHERE SettingKey = '{0}';", key);
                using (DAC dac = new DAC())
                {
                    return dac.ExecuteScalar(statement).ToString();
                }
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public static int Set(string key, string value)
        {
            value = value.Replace("'", "''");

            string statement = string.Format("UPDATE Settings SET SettingValue = '{0}', UpdatedOn = '{1}' WHERE [SettingKey] = '{2}';", value, DateTime.Now.ToString(), key);
            using (DAC dac = new DAC())
            {
                return dac.ExecuteCommand(statement);
            }
        }

        #endregion
    }
}
