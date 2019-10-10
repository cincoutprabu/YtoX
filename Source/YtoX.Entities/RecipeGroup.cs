//RecipeGroup.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Entities
{
    public class RecipeGroup
    {
        #region Properties

        public long UID { get; set; }
        public string Name { get; set; }
        public string Tagline { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public List<Recipe> Recipes { get; set; }

        #endregion

        #region Constructors

        public RecipeGroup()
        {
            this.UID = -1;
            this.Name = string.Empty;
            this.Tagline = string.Empty;
            this.Description = string.Empty;
            this.ImagePath = string.Empty;
            this.Recipes = new List<Recipe>();
        }

        #endregion
    }
}
