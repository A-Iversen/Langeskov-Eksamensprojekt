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
        public string Gender { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }

        public Member(int memberID, string gender, string name, string address, string postalCode, string phoneNumber, string email, string dateOfBirth)
        {
            MemberID = memberID;
            Gender = gender;
            Name = name;
            Address = address;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            Email = email;
            DateOfBirth = dateOfBirth;
        }
    }
}