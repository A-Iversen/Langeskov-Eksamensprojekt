using System;

namespace Infrastructure.Model
{
    public class SubsidyGroup
    {
        public int SubsidyGroupID { get; set; }
        public string SubsidyGroupNameText { get; set; } = string.Empty;
        public string AgeRange { get; set; } = string.Empty;

        public enum SubsidyGroupName
        {
            Child_0_12 = 1,
            Youth_13_18 = 2,
            YoungAdult_19_24 = 3,
            Adult_25_59 = 4,
            Senior_60_Plus = 5
        }

        public SubsidyGroup(int id, string name, string ageRange)
        {
            SubsidyGroupID = id;
            SubsidyGroupNameText = name;
            AgeRange = ageRange;
        }

        public SubsidyGroup() { }
    }
}

