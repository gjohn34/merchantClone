using System.Collections.Generic;
using System.Xml.Serialization;

namespace merchantClone.Models
{
    public interface ITask
    {
        int Cost { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        int RequiredLevel { get; set; }
        int Time { get; set; }
        int ExperienceGain { get; set; }
        List<RewardItem> RewardItems { get; set; }
    }
}