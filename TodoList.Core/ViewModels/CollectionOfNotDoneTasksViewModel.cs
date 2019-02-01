﻿using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;
using TodoList.Core.Interfaces;
using TodoList.Core.Models;

namespace TodoList.Core.ViewModels
{
    public class CollectionOfNotDoneTasksViewModel : MvxViewModel<Action>
    {
        private bool _isRefreshLayoutRefreshing;
        private MvxObservableCollection<Goal> _goals;
        private MvxCommand _updateDataCommand;
        private ITaskService _taskService;
        private ILoginService _loginService;
        private readonly IMvxNavigationService _navigationService;

        public CollectionOfNotDoneTasksViewModel(IMvxNavigationService navigationService, ITaskService taskService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _taskService = taskService;
            _loginService = loginService;
            Goals = new MvxObservableCollection<Goal>();
            FillingDataActivityCommand = new MvxAsyncCommand<Goal>(CreateNewGoal);
        }

        public IMvxCommand<Goal> FillingDataActivityCommand { get; set; }
        public IMvxCommand LogoutCommand => new MvxCommand(Logout);
        public Action OnLoggedOutHandler { get; set; }

        private void Logout()
        {
            OnLoggedOutHandler();
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            MakeListOfGoals();
            RaisePropertyChanged(() => Goals);
        }

        public MvxObservableCollection<Goal> Goals
        {
            get
            {
                return _goals;
            }
            set
            {
                _goals = value;
                RaisePropertyChanged(() => Goals);
            }
        }

        public async Task CreateNewGoal(Goal goal)
        {
            var result = await _navigationService.Navigate<FillingDataViewModel, Goal>(goal);
        }

        public MvxCommand UpdateDataCommand
        {
            get
            {
                return _updateDataCommand = _updateDataCommand ?? new MvxCommand(UpdateDataFromDB);
            }
        }

        private void UpdateDataFromDB()
        {
            IsRefreshLayoutRefreshing = true;
            MakeListOfGoals();
            IsRefreshLayoutRefreshing = false;
        }

        private void MakeListOfGoals()
        {
            User user = _loginService.CurrentUser;
            var list = _taskService.GetNotDoneUserGoal(user.UserId);
            Goals = new MvxObservableCollection<Goal>(list);
        }

        public bool IsRefreshLayoutRefreshing
        {
            get
            {
                return _isRefreshLayoutRefreshing;
            }

            set
            {
                _isRefreshLayoutRefreshing = value;
                RaisePropertyChanged(() => IsRefreshLayoutRefreshing);
            }
        }

        public override void Prepare(Action parameter)
        {
            OnLoggedOutHandler = parameter;
        }
    }
}
