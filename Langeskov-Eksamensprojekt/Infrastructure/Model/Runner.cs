using System;

namespace Infrastructure.Model
{
    public class Runner
    {
        public int RunnerID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        // FK fra SubsidyGroup
        public int SubsidyGroupID { get; private set; }

        // FK fra MemberGroup - bruger nu integer ID i stedet for string navn
        public int RunnerGroupID { get; private set; }

        public Runner() { }

        public Runner(string name, string? email, string? address, string? postalCode, string? phoneNumber, string? gender, DateTime dateOfBirth, int runnerGroupID)
        {
            Name = name;
            Email = email;
            Address = address;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            RunnerGroupID = runnerGroupID;
        }

        // bruges af ViewModel
        public void SetSubsidyGroup(int groupID)
        {
            SubsidyGroupID = groupID;
        }

        // bruges af Repository til at sætte MemberGroupID fra database
        public void SetRunnerGroupID(int runnerGroupID)
        {
            RunnerGroupID = runnerGroupID;
        }
    }
}

