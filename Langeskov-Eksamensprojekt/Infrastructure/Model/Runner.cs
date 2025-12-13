using System;

namespace Infrastructure.Model
{
    //Runner kan ikke have to Gender variabler, så enum Gender laves udenfor klassen Runner,
    //og så laves den som en property i Runner klassen hvor der ikke er en defination for Gender.
    public enum Gender
    {
        Mand,
        Kvinde,
        Andet
    }

    public class Runner
    {
        public int RunnerID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        // FK fra SubsidyGroup
        public int SubsidyGroupID { get; private set; }

        // FK fra MemberGroup - make setter public so WPF can two-way bind SelectedValue to this property
        public int RunnerGroupID { get; set; }

        //
        public Runner() { }

        //
        public Runner(string name, string? email, string? address, string? postalCode, string? phoneNumber, Gender? gender, DateTime dateOfBirth, int runnerGroupID)
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

        // bruges af Repository til at sætte MemberGroupID fra database (keeps compatibility)
        public void SetRunnerGroupID(int runnerGroupID)
        {
            RunnerGroupID = runnerGroupID;
        }
    }
}

