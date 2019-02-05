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
        private UIBarButtonItem _buttonAdd;
        private UIBarButtonItem _buttonLogOut;
        private readonly string _textTitle = "To-do List";
        private MvxUIRefreshControl _refreshControl;

        public CollectionOfDoneTasksView () : base (nameof(CollectionOfDoneTasksView), null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = _textTitle;
            _refreshControl = new MvxUIRefreshControl();
            CollectionOfDoneTasksTableView.AddSubview(_refreshControl);
            _buttonAdd = new UIBarButtonItem(UIBarButtonSystemItem.Add, null);
            NavigationItem.SetRightBarButtonItem(_buttonAdd, false);
            _buttonLogOut = new UIBarButtonItem(UIBarButtonSystemItem.Stop, null);
            NavigationItem.SetLeftBarButtonItem(_buttonLogOut, false);
            var source = new TodoTasksTableViewSource(CollectionOfDoneTasksTableView);
            CollectionOfDoneTasksTableView.Source = source;
            var set = this.CreateBindingSet<CollectionOfDoneTasksView, CollectionOfDoneTasksViewModel>();
            set.Bind(source).To(vm => vm.Goals);
            set.Bind(source).For(v => v.SelectionChangedCommand).To(vm => vm.FillingDataActivityCommand);
            set.Bind(_buttonAdd).For("Clicked").To(vm => vm.FillingDataActivityCommand);
            set.Bind(_buttonLogOut).For("Clicked").To(vm => vm.LogoutCommand);
            set.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsRefreshLayoutRefreshing);
            set.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.UpdateDataCommand);
            set.Apply();
            CollectionOfDoneTasksTableView.ReloadData();
        }
    }
}