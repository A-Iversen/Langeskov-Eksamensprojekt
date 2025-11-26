using Infrastructure.Model;
using Infrastructure.Repository;
using System;
using System.Linq;
using SubsidyGroupName = Infrastructure.Model.SubsidyGroup.SubsidyGroupName;
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.ViewModel
{
    public class MemberViewModel
    {
        private readonly IMemberRepository _repository;

        // Den valgte medlemskabstype fra UI - bruger nu integer ID i stedet for string navn
        public int SelectedMemberGroupID { get; set; }

        // Properties for dataindtastning (binder til View/UI)
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public MemberViewModel(IMemberRepository repository)
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
        private bool IsResidentInMunicipality(string? postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode))
                return false;
            string[] kertemindePostalCodes = { "5300", "5550", "5380", "5390" /* ... tilføj flere */ };
            return kertemindePostalCodes.Contains(postalCode);
        }

        // Tilmelding af nyt medlem
        public Member CreateNewMember()
        {
            if (string.IsNullOrWhiteSpace(Name) || DateOfBirth == default || SelectedMemberGroupID <= 0)
            {
                throw new ArgumentException("Navn, fødselsdato og medlemskabstype skal udfyldes.");
            }

            if (_repository.MemberExists(Name, DateOfBirth))
            {
                throw new InvalidOperationException("Medlemmet findes allerede. Kontakt administrator for genaktivering.");
            }

            var newMember = new Member(name: Name, email: Email, address: Address, postalCode: PostalCode, phoneNumber: PhoneNumber, gender: Gender, dateOfBirth: DateOfBirth, memberGroupID: SelectedMemberGroupID);

            // Automatisk Gruppeberegning
            var allocatedGroupEnum = CalculateSubsidyGroup(newMember.DateOfBirth);

            // Konvertering til database INT Primary Key.
            var allocatedGroupID = (int)allocatedGroupEnum;
            newMember.SetSubsidyGroup(allocatedGroupID);

            var createdMember = _repository.Add(newMember);

            //Simulation af Bekræftelse/Mail
            Console.WriteLine($"Bekræftelse sendt til {createdMember.Email ?? "ingen email"}. Tildelt tilskudsgruppe ID: {createdMember.SubsidyGroupID}");

            return createdMember;
        }

        private ObservableCollection<Member> _members { get; set; } = new ObservableCollection<Member>();
        public ObservableCollection<Member> Members
        {
            get { return _members; }
            set { _members = value; }
        }


    }
}