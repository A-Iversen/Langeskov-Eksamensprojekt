using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Model
{
    public class Member
    {
        public int MemberID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        // FK fra SubsidyGroup
        public int SubsidyGroupID { get; private set; }

        // FK fra MemberGroup
        public string MemberGroupName { get; private set; }

        public Member() { }

        public Member(string name, string email, string address, string postalCode, string phoneNumber, string gender, DateTime dateOfBirth, string memberGroupName)
        {
            Name = name;
            Email = email;
            Address = address;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            MemberGroupName = memberGroupName;
        }

        // bruges af ViewModel
        public void SetSubsidyGroup(int groupID)
        {
            SubsidyGroupID = groupID;
        }
    }
}