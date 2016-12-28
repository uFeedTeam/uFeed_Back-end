using System;
using System.Web;
using System.Web.Mvc;
using uFeed.WEB.Account.Interfaces;

namespace uFeed.WEB.Modules
{
    public class AuthHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Authenticate;
        }

        public void Dispose()
        {
        }

        private static void Authenticate(object sender, EventArgs args)
        {
            var app = (HttpApplication)sender;
            var context = app.Context;

            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            auth.HttpContext = context;
            context.User = auth.CurrentUser;
        }
    }
}