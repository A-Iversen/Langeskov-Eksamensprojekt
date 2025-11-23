using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MVVM.Core;
using MVVM.View.UserControls;

namespace MVVM.ViewModel
{
    class MainViewModel : ViewModelBase
    {
		public string View { get; set; }

        private object _currentView = new MemberControl();

		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value;
				OnPropertyChanged();
			}
		}

        public RelayCommand ShowMemberViewCommand => new RelayCommand(execute => CurrentView = new MemberControl());
		public RelayCommand ShowAccountingViewCommand => new RelayCommand(execute => CurrentView = new AccountingControl());


	}
}
