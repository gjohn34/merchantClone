using merchantClone.Controls;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace merchantClone.Models
{
    public interface IPerson
    {
        string Name { get; }
        int CurrentXp { get; set; }
        int TotalXp { get; set; }

        void FinishTask();
        //List<Recipe> GetJobs();
        //List<ComponentRow> ShowJobs(Texture2D texture, SpriteFont spriteFont);
    }
}