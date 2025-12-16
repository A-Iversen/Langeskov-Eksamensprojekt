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
    // This wrapper handles the validation logic for the DataGrid rows
    public class RunnerValidationWrapper : ViewModelBase, IDataErrorInfo
    {
        // Reference to the actual Model
        public Runner Model { get; }

        public RunnerValidationWrapper(Runner runner)
        {
            Model = runner;
        }

        // Expose properties you want to edit and validate
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

        // Expose other properties needed for display (read-only or passthrough)
        public int RunnerID => Model.RunnerID;


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

        // --- IDataErrorInfo Implementation (Logic in ViewModel!) ---
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

        // Den valgte medlemskabstype fra UI - bruger nu integer ID i stedet for string navn
        public int SelectedRunnerGroupID { get; set; }

        //Properties & ObservableCollection for dataindtastning (binder til View/UI)
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (_dateOfBirth != value)
                {
                    _dateOfBirth = value;
                    OnPropertyChanged();
                }
            }
        }

        private Gender? _gender;
        public Gender? Gender
        {
            get => _gender;
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _address = string.Empty;
        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _postalCode = string.Empty;
        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (_postalCode != value)
                {
                    _postalCode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _phoneNumber = string.Empty;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
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


        //Constructor
        public RunnerViewModel(IRunnerRepository repository, IRunnerGroupRepository? runnerGroupRepository = null)
        {
            _repository = repository;
            _runnerGroupRepository = runnerGroupRepository;

            
            CreateRunnerCommand = new RelayCommand(_ =>
            {
                try
                {
                    var runner = CreateNewRunner();
                    Runners.Add(new RunnerValidationWrapper(runner)); // Optional: show in list
                    MessageBox.Show($"Bruger '{runner.Name}' blev oprettet.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fejl: {ex.Message}", "Oprettelse mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            LoadRunners();
            LoadRunnerGroups();
        }


        // Indlæs eksisterende medlemmer
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


        // Kommando til at oprette et nyt medlem
        public ICommand CreateRunnerCommand { get; }

        //--Oprettelse af Runner---
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
        


        //---Editing af Runner---
        public ICommand UpdateRunnerCommand => new RelayCommand(param =>
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
        });



        //---Sletning af Runner---
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

        public ICommand DeleteRunnerCommand => new RelayCommand(execute =>
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
        });

    }
}