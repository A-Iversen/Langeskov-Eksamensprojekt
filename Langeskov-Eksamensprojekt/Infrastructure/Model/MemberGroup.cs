using System;

namespace Infrastructure.Model
{
    public class MemberGroup
    {
        public int MemberGroupID { get; set; }
        public string MemberGroupName { get; set; } = string.Empty;
        public decimal SubscriptionFee { get; set; }

        public MemberGroup(int id, string name, decimal fee)
        {
            MemberGroupID = id;
            MemberGroupName = name;
            SubscriptionFee = fee;
        }

        public MemberGroup() { }
    }
}

