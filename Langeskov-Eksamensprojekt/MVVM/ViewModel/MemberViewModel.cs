using MVVM.Model;
using Infrastructure.Repository;
using System;
using System.Linq;
using SubsidyGroupName = MVVM.Model.SubsidyGroup.SubsidyGroupName;

namespace MVVM.ViewModel
{
    public class MemberViewModel
    {
        private readonly SQLMemberRepository _repository;

        // Simulerer den valgte medlemskabstype fra UI
        public string SelectedMemberGroup { get; set; }

        // Properties for dataindtastning (binder til View/UI)
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }


        public MemberViewModel(SQLMemberRepository repository)
        {
            _repository = repository;
        }

        // Automatisk tildeling af Tilskudsgruppe baseret på fødselsdato.
        private SubsidyGroupName CalculateSubsidyGroup(DateTime dateOfBirth)
        {
            int age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

            if (age <= 12)
                return SubsidyGroupName.Child_0_12;
            else if (age <= 18)
                return SubsidyGroupName.Youth_13_18;
            else if (age <= 24)
                return SubsidyGroupName.YoungAdult_19_24;
            else if (age <= 59)
                return SubsidyGroupName.Adult_25_59;
            else
                return SubsidyGroupName.Senior_60_Plus;
        }

        // Tjekker om medlemmet er bosiddende i Kerteminde Kommune.
        private bool IsResidentInMunicipality(string postalCode)
        {
            string[] kertemindePostalCodes = { "5300", "5550", "5380", "5390" /* ... tilføj flere */ };
            return kertemindePostalCodes.Contains(postalCode);
        }

        // Tilmelding af nyt medlem
        public Member EnrollNewMember()
        {
            if (string.IsNullOrWhiteSpace(Name) || DateOfBirth == default || string.IsNullOrWhiteSpace(SelectedMemberGroup))
            {
                throw new ArgumentException("Navn, fødselsdato og medlemskabstype skal udfyldes.");
            }

            if (_repository.MemberExists(Name, DateOfBirth))
            {
                throw new InvalidOperationException("Medlemmet findes allerede. Kontakt administrator for genaktivering.");
            }

            var newMember = new Member(name: Name, email: Email, address: Address, postalCode: PostalCode, phoneNumber: PhoneNumber, gender: Gender, dateOfBirth: DateOfBirth, memberGroupName: SelectedMemberGroup);

            // Automatisk Gruppeberegning
            var allocatedGroupEnum = CalculateSubsidyGroup(newMember.DateOfBirth);

            // Konvertering til database INT Primary Key.
            var allocatedGroupID = (int)allocatedGroupEnum;

            newMember.SetSubsidyGroup(allocatedGroupID);

            // Persistens via Repository
            var createdMember = _repository.Add(newMember);

            //Simulation af Bekræftelse/Mail
            Console.WriteLine($"Bekræftelse sendt til {createdMember.Email}. Tildelt tilskudsgruppe ID: {createdMember.SubsidyGroupID}");

            return createdMember;
        }
    }
}