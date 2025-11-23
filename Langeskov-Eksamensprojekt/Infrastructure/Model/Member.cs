using System;

namespace Infrastructure.Model
{
    public class Member
    {
        public int MemberID { get; set; }
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
        public int MemberGroupID { get; private set; }

        public Member() { }

        public Member(string name, string? email, string? address, string? postalCode, string? phoneNumber, string? gender, DateTime dateOfBirth, int memberGroupID)
        {
            Name = name;
            Email = email;
            Address = address;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            MemberGroupID = memberGroupID;
        }

        // bruges af ViewModel
        public void SetSubsidyGroup(int groupID)
        {
            SubsidyGroupID = groupID;
        }

        // bruges af Repository til at sætte MemberGroupID fra database
        public void SetMemberGroupID(int memberGroupID)
        {
            MemberGroupID = memberGroupID;
        }
    }
}

