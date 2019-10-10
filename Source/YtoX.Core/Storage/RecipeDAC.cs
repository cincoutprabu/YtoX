//RecipeDAC.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using YtoX.Entities;
using YtoX.Core.Helpers;

namespace YtoX.Core.Storage
{
    public static class RecipeDAC
    {
        #region Methods

        public static List<Recipe> FetchByGroupUid(long groupUid)
        {
            List<Recipe> recipeList = new List<Recipe>();
            using (DAC dac = new DAC())
            {
                using (DbDataReader reader = dac.FetchRecords("SELECT * FROM Recipe WHERE GroupUID = " + groupUid + ";"))
                {
                    while (reader.Read())
                    {
                        recipeList.Add(Fetch(reader));
                    }
                }
            }
            return recipeList;
        }

        public static void SetEnabled(Recipe recipe)
        {
            using (DAC dac = new DAC())
            {
                string statement = string.Format("UPDATE Recipe SET IsEnabled = {0} WHERE Name = '{1}';", recipe.IsEnabled ? 1 : 0, recipe.Name);
                dac.ExecuteCommand(statement);
            }
        }

        #endregion

        #region Internal-Methods

        private static Recipe Fetch(DbDataReader reader)
        {
            Recipe recipe = new Recipe();
            try
            {
                recipe.GroupUID = (long)reader["GroupUID"];
                recipe.Name = (string)reader["Name"];
                recipe.Tagline = (string)reader["Tagline"];
                recipe.Description = (string)reader["Description"];
                recipe.ImagePath = (string)reader["ImagePath"];
                recipe.IsAvailable = (bool)reader["IsAvailable"];
                recipe.IsEnabled = (bool)reader["IsEnabled"];
                if (!Convert.IsDBNull(reader["ScopeHours"])) recipe.ScopeHours = (double)reader["ScopeHours"];
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
            return recipe;
        }

        #endregion
    }
}
