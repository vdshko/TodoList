﻿using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System.Threading.Tasks;
using TodoList.Core.ViewModels;
using Xamarin.Essentials;
using Xamarin.Facebook;

namespace TodoList.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [Register("TodoList.Droid.Views.LoginView")]
    public class LoginView : BaseFragment<LoginViewModel>
    {
        private Button _facebookLoginButton;

        protected override int FragmentId
        {
            get
            {
                return Resource.Layout.LoginLayout;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            _facebookLoginButton = view.FindViewById<Button>(Resource.Id.facebook_login_button);
            _facebookLoginButton.Click += delegate { LoggedInOrOutFacebook(); };
            return view;
        }

        private async void LoggedInOrOutFacebook()
        {
            if (string.IsNullOrEmpty(this.ViewModel.UserId))
            {
                this.ViewModel.LoginFacebookCommand.Execute();
                await Task.Run(() =>
                 {
                     if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                     {
                         StartActivity(this.ViewModel.Authenticator.GetUI(View.Context));
                         Activity.OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
                     }
                 });
                return;
            }
            this.ViewModel.LogoutFacebookCommand.Execute();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Activity.OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }
    }
}