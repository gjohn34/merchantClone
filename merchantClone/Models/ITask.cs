using System.Collections.Generic;

namespace merchantClone.Models
{
    public interface ITask
    {
        int Cost { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        int RequiredLevel { get; set; }
        int Time { get; set; }
        List<Item> Reward { get; set; }
    }
}