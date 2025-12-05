using System;

namespace Infrastructure.Model
{
    public class RunnerGroup
    {
        public int RunnerGroupID { get; set; }
        public string RunnerGroupName { get; set; } = string.Empty;
        public decimal SubscriptionFee { get; set; }

        public RunnerGroup(int id, string name, decimal fee)
        {
            RunnerGroupID = id;
            RunnerGroupName = name;
            SubscriptionFee = fee;
        }

        public RunnerGroup() { }
    }
}

