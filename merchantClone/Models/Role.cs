using System.Collections.Generic;

namespace merchantClone.Models
{
    public enum Roles
    {
        Carpenter,
        Armorer,
        Blacksmith
    }
    public class Role
    {
        #region fields
        protected List<Recipe> AllRecipes = new List<Recipe> {
                new Recipe("Iron Dagger", 1, Roles.Blacksmith),
                new Recipe("Iron Short Sword", 1, Roles.Blacksmith),
                new Recipe("Iron Long Sword", 2, Roles.Blacksmith),
                new Recipe("Iron Wand", 1, Roles.Carpenter),
                new Recipe("Iron Club", 1, Roles.Carpenter),
                new Recipe("Iron Staff", 2, Roles.Carpenter),
                new Recipe("Iron Boots", 1, Roles.Armorer),
                new Recipe("Iron Gloves", 1, Roles.Armorer),
                new Recipe("Iron Helmet", 2, Roles.Armorer),
            };
        #endregion
        #region properties
        public List<Recipe> RoleRecipes { get; set; }
        #endregion
        #region methods
        #endregion

    }
}