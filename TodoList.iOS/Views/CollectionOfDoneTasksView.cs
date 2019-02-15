﻿using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using TodoList.Core.ViewModels;
using TodoList.iOS.Sources;
using UIKit;

namespace TodoList.iOS.Views
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "DONE", TabIconName = "TabBarChecked")]
    public partial class CollectionOfDoneTasksView : MvxViewController<CollectionOfDoneTasksViewModel>
    {
        private UIButton _buttonFillingData;
        private UIButton _buttonLogOut;
        private readonly string _textTitle = "To-do List";
        private MvxUIRefreshControl _refreshControl;

        public CollectionOfDoneTasksView () : base (nameof(CollectionOfDoneTasksView), null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationItem.Title = _textTitle;
            var titleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White };
            NavigationController.NavigationBar.TitleTextAttributes = titleTextAttributes;
            _buttonFillingData = new UIButton(UIButtonType.Custom);
            _buttonFillingData.Frame = new CGRect(0, 0, 40, 40);
            _buttonFillingData.SetImage(UIImage.FromBundle("TabBarFillingDataIcon"), UIControlState.Normal);
            this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(_buttonFillingData), false);
            _buttonLogOut = new UIButton(UIButtonType.Custom);
            _buttonLogOut.Frame = new CGRect(0, 0, 40, 40);
            _buttonLogOut.SetImage(UIImage.FromBundle("TabBarLogOutIcon"), UIControlState.Normal);
            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(_buttonLogOut), false);
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() { TextColor = UIColor.White });
            NavigationController.NavigationBar.BarTintColor = new UIColor(0.17f, 0.24f, 0.31f, 1.0f);
            _refreshControl = new MvxUIRefreshControl();
            CollectionOfDoneTasksTableView.AddSubview(_refreshControl);
            var source = new TodoTasksTableViewSource(CollectionOfDoneTasksTableView);
            CollectionOfDoneTasksTableView.Source = source;
            source.OnShareHandlerSource = (currentTask) =>
            {
                this.ViewModel.PlatformName = false; //if false -> iOS, if true -> Android
                this.ViewModel.ShareMessageCommand.Execute(currentTask);
            };
            var set = this.CreateBindingSet<CollectionOfDoneTasksView, CollectionOfDoneTasksViewModel>();
            set.Bind(source).To(vm => vm.Goals);
            set.Bind(source).For(v => v.SelectionChangedCommand).To(vm => vm.FillingDataActivityCommand);
            set.Bind(_buttonFillingData).To(vm => vm.FillingDataActivityCommand);
            set.Bind(_buttonLogOut).To(vm => vm.LogoutCommand);
            set.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsRefreshLayoutRefreshing);
            set.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.UpdateDataCommand);
            set.Apply();
            CollectionOfDoneTasksTableView.ReloadData();
        }
    }
}