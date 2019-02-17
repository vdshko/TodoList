﻿using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;
using TodoList.Core.Interfaces;
using TodoList.Core.Models;
using Xamarin.Essentials;

namespace TodoList.Core.ViewModels
{
    public class FillingDataViewModel : MvxViewModel<Goal>
    {
        private string _goalName;
        private string _goalDescription;
        private bool _goalStatus = false;
        private bool _goalNameEnableStatus;
        private readonly IMvxNavigationService _navigationService;
        private ITaskService _taskService;
        private ILoginService _loginService;
        private IWebApiService _webApiService;
        private int _goalId;
        private bool _saveButtonEnableStatus = false;
        private string _userId;
        private string _deleteCanselButtonText;
        private bool _isNetAvailable;

        public FillingDataViewModel(IMvxNavigationService navigationService, ITaskService taskService, ILoginService loginService, IWebApiService webApiService)
        {
            _navigationService = navigationService;
            _taskService = taskService;
            _loginService = loginService;
            _webApiService = webApiService;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsNetAvailable = true;
            }
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                IsNetAvailable = true;
                return;
            }
            IsNetAvailable = false;
        }

        public int GoalId
        {
            get
            {
                return _goalId;
            }

            set
            {
                _goalId = value;
                RaisePropertyChanged(() => GoalId);
            }
        }

        public string GoalName
        {
            get
            {
                return _goalName;
            }

            set
            {
                _goalName = value;
                RaisePropertyChanged(() => GoalName);
                RaisePropertyChanged(() => SaveButtonEnableStatus);
            }
        }

        public string GoalDescription
        {
            get
            {
                return _goalDescription;
            }

            set
            {
                _goalDescription = value;
                RaisePropertyChanged(() => GoalDescription);
            }
        }

        public bool GoalStatus
        {
            get
            {
                return _goalStatus;
            }

            set
            {
                _goalStatus = value;
                RaisePropertyChanged(() => GoalStatus);
            }
        }

        public bool GoalNameEnableStatus
        {
            get
            {
                return _goalNameEnableStatus;
            }

            set
            {
                _goalNameEnableStatus = value;
                RaisePropertyChanged(() => GoalNameEnableStatus);
            }
        }

        public bool SaveButtonEnableStatus
        {
            get
            {
                if (GoalName == null | GoalName.Trim() == string.Empty)
                {
                    return _saveButtonEnableStatus = false;
                }
                return _saveButtonEnableStatus = true;
            }

            set
            {
                _saveButtonEnableStatus = value;
                RaisePropertyChanged(() => SaveButtonEnableStatus);
            }
        }

        public MvxAsyncCommand SendBackCommand
        {
            get
            {
                return new MvxAsyncCommand(NavigateToPreviousActivity);
            }
        }

        private async Task NavigateToPreviousActivity()
        {
            await _navigationService.Close(this);
        }

        public MvxAsyncCommand SaveDataCommand
        {
            get
            {
                return new MvxAsyncCommand(SaveDataToDB);
            }
        }

        private async Task SaveDataToDB()
        {
            if (IsNetAvailable)
            {
                Goal goal = new Goal(GoalId, GoalName.Trim(), GoalDescription, GoalStatus, UserId);
                await _webApiService.InsertOrUpdateDataAsync(goal);
                await _navigationService.Close(this);
            }
        }

        public string DeleteCanselButtonText
        {
            get
            {
                if (GoalName == null)
                {
                    return _deleteCanselButtonText = "Cancel";
                }
                return _deleteCanselButtonText = "Delete";
            }

            set
            {
                _deleteCanselButtonText = value;
                RaisePropertyChanged(() => DeleteCanselButtonText);
            }
        }

        public MvxAsyncCommand DeleteDataCommand
        {
            get
            {
                return new MvxAsyncCommand(DeleteDataFromDB);
            }
        }

        public string UserId
        {
            get
            {
                return _userId = _loginService.CurrentUser.UserId;
            }

            set
            {
                _userId = value;
                RaisePropertyChanged(() => UserId);
            }
        }

        private async Task DeleteDataFromDB()
        {
            if (IsNetAvailable)
            {
                var position = GoalId;
                await _webApiService.DeleteDataAsync(position);
                await _navigationService.Close(this);
            }
        }

        public override void Prepare(Goal parameter)
        {
            if (parameter != null)
            {
                GoalNameEnableStatus = false;
                GoalId = parameter.Id;
                GoalName = parameter.GoalName;
                GoalDescription = parameter.GoalDescription;
                GoalStatus = parameter.GoalStatus;
                UserId = parameter.UserId;
                return;
            }
            GoalNameEnableStatus = true;
        }

        public bool IsNetAvailable
        {
            get
            {
                return _isNetAvailable;
            }

            set
            {
                _isNetAvailable = value;
                RaisePropertyChanged(() => IsNetAvailable);
            }
        }
    }
}