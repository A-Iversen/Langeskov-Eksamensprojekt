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
    public class RunnerViewModel
    {
        private readonly IRunnerRepository _repository;

        // Den valgte medlemskabstype fra UI - bruger nu integer ID i stedet for string navn
        public int SelectedRunnerGroupID { get; set; }

        // Properties for dataindtastning (binder til View/UI)
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public RunnerViewModel(IRunnerRepository repository)
        {
            _repository = repository;
            LoadRunners();

        }

        private void LoadRunners()
        {
            var runners = _repository.GetAll();
            _runners.Clear();
            foreach (var runner in runners)
            {
                _runners.Add(runner);
            }
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
        public Runner CreateNewRunner()
        {
            if (string.IsNullOrWhiteSpace(Name) || DateOfBirth == default || SelectedRunnerGroupID <= 0)
            {
                throw new ArgumentException("Navn, fødselsdato og medlemskabstype skal udfyldes.");
            }

            if (_repository.RunnerExists(Name, DateOfBirth))
            {
                throw new InvalidOperationException("Medlemmet findes allerede. Kontakt administrator for genaktivering.");
            }

            var newRunner = new Runner(name: Name, email: Email, address: Address, postalCode: PostalCode, phoneNumber: PhoneNumber, gender: Gender, dateOfBirth: DateOfBirth, runnerGroupID: SelectedRunnerGroupID);

            // Automatisk Gruppeberegning
            var allocatedGroupEnum = CalculateSubsidyGroup(newRunner.DateOfBirth);

            // Konvertering til database INT Primary Key.
            var allocatedGroupID = (int)allocatedGroupEnum;
            newRunner.SetSubsidyGroup(allocatedGroupID);

            var createdRunner = _repository.Add(newRunner);

            //Simulation af Bekræftelse/Mail
            Console.WriteLine($"Bekræftelse sendt til {createdRunner.Email ?? "ingen email"}. Tildelt tilskudsgruppe ID: {createdRunner.SubsidyGroupID}");

            return createdRunner;
        }

        private ObservableCollection<Runner> _runners { get; set; } = new ObservableCollection<Runner>();
        public ObservableCollection<Runner> Runners
        {
            get { return _runners; }
            set { _runners = value; }
        }




    }
}