//RecipeGroupDAC.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using YtoX.Entities;

namespace YtoX.Core.Storage
{
    public static class RecipeGroupDAC
    {
        #region Methods

        public static RecipeGroup FetchByUid(int uid)
        {
            RecipeGroup group = null;
            using (DAC dac = new DAC())
            {
                using (DbDataReader reader = dac.FetchRecords("SELECT * FROM RecipeGrou] WHERE UID = " + uid + ";"))
                {
                    if (reader.Read())
                    {
                        group = Fetch(reader);
                    }
                }
            }
            return group;
        }

        public static List<RecipeGroup> FetchAll()
        {
            List<RecipeGroup> all = new List<RecipeGroup>();
            using (DAC dac = new DAC())
            {
                using (DbDataReader reader = dac.FetchRecords("SELECT * FROM RecipeGroup ORDER BY UID;"))
                {
                    while (reader.Read())
                    {
                        all.Add(Fetch(reader));
                    }
                }
            }
            return all;
        }

        #endregion

        #region Internal-Methods

        private static RecipeGroup Fetch(DbDataReader reader)
        {
            RecipeGroup group = new RecipeGroup();
            group.UID = (long)reader["UID"];
            group.Name = (string)reader["Name"];
            group.Tagline = (string)reader["Tagline"];
            group.Description = (string)reader["Description"];
            group.ImagePath = (string)reader["ImagePath"];
            group.Recipes = RecipeDAC.FetchByGroupUid(group.UID);
            return group;
        }

        #endregion
    }
}
