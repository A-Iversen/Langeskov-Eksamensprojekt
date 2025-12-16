using Infrastructure.Model;
using Infrastructure.Repository;
using MVVM.Core;
using System;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SubsidyGroupName = Infrastructure.Model.SubsidyGroup.SubsidyGroupName;


namespace MVVM.ViewModel
{
    // Runner Wrapper
    // bruges som et stop mellem ViewModel og Model for at implementere validering af DataGrid felterne.
    public class RunnerValidationWrapper : ViewModelBase, IDataErrorInfo
    {
        // Reference til Runner.cs modellen
        public Runner Model { get; }

        public RunnerValidationWrapper(Runner runner)
        {
            Model = runner;
        }

        // Exposer properties til validering
        public string Name
        {
            get => Model.Name;
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Email
        {
            get => Model.Email;
            set
            {
                if (Model.Email != value)
                {
                    Model.Email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? PostalCode
        {
            get => Model.PostalCode;
            set
            {
                if (Model.PostalCode != value)
                {
                    Model.PostalCode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? PhoneNumber
        {
            get => Model.PhoneNumber;
            set
            {
                if (Model.PhoneNumber != value)
                {
                    Model.PhoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Address
        {
            get => Model.Address;
            set
            {
                if (Model.Address != value)
                {
                    Model.Address = value;
                    OnPropertyChanged();
                }
            }
        }

        public Gender? Gender
        {
            get => Model.Gender;
            set
            {
                if (Model.Gender != value)
                {
                    Model.Gender = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DateOfBirth
        {
            get => Model.DateOfBirth;
            set
            {
                if (Model.DateOfBirth != value)
                {
                    Model.DateOfBirth = value;
                    OnPropertyChanged();
                }
            }
        }

        public int RunnerID
        {
            get => Model.RunnerID;
            set
            {
                if (Model.RunnerID != value)
                {
                    Model.RunnerID = value;
                    OnPropertyChanged();
                }
            }
        }

        public int RunnerGroupID
        {
            get => Model.RunnerGroupID;
            set
            {
                if (Model.RunnerGroupID != value)
                {
                    Model.RunnerGroupID = value;
                    OnPropertyChanged();
                }
            }
        }

        // --- IDataErrorInfo Implementering (Validerings conditions) ---
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrWhiteSpace(Name))
                            result = "Navn er påkrævet.";
                        else if (Name.Length < 2)
                            result = "Navn skal være mindst 2 tegn.";
                        break;

                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email))
                            result = "Email er påkrævet.";
                        else if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                            result = "Ugyldigt email format.";
                        break;

                    case nameof(PostalCode):
                        if (string.IsNullOrWhiteSpace(PostalCode))
                            result = "Post Nr. er påkrævet.";
                        else if (!Regex.IsMatch(PostalCode, @"^\d{4}$"))
                            result = "Post Nr. skal være 4 cifre.";
                        break;

                    case nameof(PhoneNumber):
                        if (!string.IsNullOrWhiteSpace(PhoneNumber) && !Regex.IsMatch(PhoneNumber, @"^\d{8}$"))
                            result = "Telefon skal være 8 cifre.";
                        break;
                }
                return result;
            }
        }
    }
    




    public class RunnerViewModel : ViewModelBase
    {
        private readonly IRunnerRepository _repository;
        private readonly IRunnerGroupRepository? _runnerGroupRepository;

        //Properties & ObservableCollection for dataindtastning (binder til View/UI)
        private RunnerValidationWrapper _newRunner;
        public RunnerValidationWrapper NewRunner
        {
            get => _newRunner;
            set
            {
                _newRunner = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<RunnerValidationWrapper> _runners { get; set; } = new ObservableCollection<RunnerValidationWrapper>();
        public ObservableCollection<RunnerValidationWrapper> Runners
        {
            get { return _runners; }
            set { _runners = value; }
        }

        private ObservableCollection<RunnerGroup> _runnerGroups { get; set; } = new ObservableCollection<RunnerGroup>();
        public ObservableCollection<RunnerGroup> RunnerGroups
        {
            get { return _runnerGroups; }
            set { _runnerGroups = value; }
        }

        public IEnumerable<Gender> GenderOptions => Enum.GetValues(typeof(Gender)).Cast<Gender>();


        private RunnerValidationWrapper _selectedRunner;
        public RunnerValidationWrapper SelectedRunner
        {
            get { return _selectedRunner; }
            set
            {
                _selectedRunner = value;
                OnPropertyChanged();
            }
        }

        //Constructor
        public RunnerViewModel(IRunnerRepository repository, IRunnerGroupRepository? runnerGroupRepository = null)
        {
            _repository = repository;
            _runnerGroupRepository = runnerGroupRepository;

            // Initializere wrapperen med en blank Runner model.
            NewRunner = new RunnerValidationWrapper(new Runner());

            LoadRunners();
            LoadRunnerGroups();
        }


        // Indlæs eksisterende medlemmer fra databasen
        private void LoadRunners()
        {
            var runners = _repository.GetAll();
            _runners.Clear();
            foreach (var runner in runners)
            {
                _runners.Add(new RunnerValidationWrapper(runner));
            }
        }
        private void LoadRunnerGroups()
        {
            _runnerGroups.Clear();
            if (_runnerGroupRepository == null) return;
            var groups = _runnerGroupRepository.GetAll();
            foreach (var g in groups)
            {
                _runnerGroups.Add(g);
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



        //--Oprettelse af Runner---
        public ICommand CreateRunnerCommand => new RelayCommand(execute => CreateRunner());
        private void CreateRunner()
        {
            // Validerings logik
            if (string.IsNullOrWhiteSpace(NewRunner.Name) || NewRunner.DateOfBirth == default)
            {
                MessageBox.Show("Navn og fødselsdato skal udfyldes.", "Valideringsfejl");
                return;
            }

            try
            {
                if (_repository.RunnerExists(NewRunner.Name, NewRunner.DateOfBirth))
                {
                    MessageBox.Show("Medlemmet findes allerede.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Oprettelse af ny Runner fra Wrapper
                // Model attributen er af typen Runner så det er "stortset" det samme som en type: new Runner
                var runnerModel = NewRunner.Model;

                // Automatisk Gruppeberegning & Konvertering til database INT Primary Key.
                var allocatedGroupEnum = CalculateSubsidyGroup(runnerModel.DateOfBirth);
                runnerModel.SetSubsidyGroup((int)allocatedGroupEnum);

                // Oprettelse i database
                var createdRunner = _repository.Add(runnerModel);

                //Tilføjelse til ObservableCollection for UI opdatering
                Runners.Add(new RunnerValidationWrapper(createdRunner));

                //Simulation af Bekræftelse/Mail
                Console.WriteLine($"Bekræftelse sendt til {createdRunner.Email ?? "ingen email"}. Tildelt tilskudsgruppe ID: {createdRunner.SubsidyGroupID}");


                //Laver et
                NewRunner = new RunnerValidationWrapper(new Runner());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl: {ex.Message}", "Oprettelse mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //---Editing af Runner---
        public ICommand UpdateRunnerCommand => new RelayCommand(EditRunner);

        private void EditRunner(object param)
        {
            var runner = param as RunnerValidationWrapper ?? SelectedRunner;
            if (runner == null)
            {
                MessageBox.Show("Ingen løber valgt til opdatering.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                _repository.Update(runner.Model);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved opdatering: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //---Sletning af Runner---
        public ICommand DeleteRunnerCommand => new RelayCommand(execute => DeleteRunner());
        private void DeleteRunner()
        {
            if (SelectedRunner == null)
            {
                MessageBox.Show("Ingen løber valgt til sletning.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                _repository.Delete(SelectedRunner.Model.RunnerID);
                Runners.Remove(SelectedRunner);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved sletning: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}