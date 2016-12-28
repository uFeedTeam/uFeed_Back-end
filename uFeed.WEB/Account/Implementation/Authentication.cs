using System;
using System.Net;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.Infrastructure.Exceptions;
using uFeed.BLL.Interfaces;
using uFeed.WEB.Account.Interfaces;
using uFeed.WEB.ViewModels;

namespace uFeed.WEB.Account.Implementation
{
    public class Authentication : IAuthentication
    {
        private const string CookieName = "__AUTH";

        private readonly IUserService _userService;

        private IPrincipal _currentUser;

        public Authentication(IUserService userService)
        {
            _userService = userService;
        }

        public HttpContext HttpContext { get; set; }

        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser != null)
                {
                    return _currentUser;
                }

                var authCookie = HttpContext.Request.Cookies.Get(CookieName);
                if (!string.IsNullOrEmpty(authCookie?.Value))
                {
                    ProcessTicket(authCookie.Value);
                }
                else if (HttpContext.Request.Headers["Authorization"] != null)
                {
                    var token = HttpContext.Request.Headers["Authorization"];
                    ProcessTicket(token);
                }
                else if (HttpContext.Request.Headers["UserEmail"] != null &&
                         HttpContext.Request.Headers["UserPassword"] != null)
                {
                    var email = HttpContext.Request.Headers["UserEmail"];
                    var password = HttpContext.Request.Headers["UserPassword"];

                    LoginUserByCredentials(email, password);
                }
                else
                {
                    _currentUser = new UserProvider(null, null);
                }

                return _currentUser;
            }
        }

        public UserViewModel Login(string email, string password, bool isPersistent)
        {
            var user = _userService.Login(email, password);

            if (user != null)
            {
                CreateCookie(user.Email, isPersistent);
            }

            var userViewModel = Mapper.Map<UserDTO, UserViewModel>(user);

            return userViewModel;
        }

        public TokenInfo Token(string email, string password)
        {
            var user = _userService.Login(email, password);
            var tokenInfo = new TokenInfo();

            if (user != null)
            {
                tokenInfo = new TokenInfo
                {
                    Token = CreateToken(user.Email),
                    ExpiresIn = DateTime.Now.Add(FormsAuthentication.Timeout)
                };
            }

            return tokenInfo;
        }

        public void LogOut()
        {
            var httpCookie = HttpContext.Response.Cookies[CookieName];

            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        private static string CreateToken(string userEmail)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                userEmail,
                DateTime.UtcNow,
                DateTime.UtcNow.Add(FormsAuthentication.Timeout),
                false,
                string.Empty,
                FormsAuthentication.FormsCookiePath);

            var encTicket = FormsAuthentication.Encrypt(ticket);

            return encTicket;
        }

        private void ProcessTicket(string encryptedTicket)
        {
            try
            {
                var ticket = FormsAuthentication.Decrypt(encryptedTicket);
                if (ticket != null && !ticket.Expired)
                {
                    _currentUser = new UserProvider(ticket.Name, _userService);
                }
            }
            catch (CryptographicException)
            {
                _currentUser = new UserProvider(null, null);
            }
        }

        private void CreateCookie(string email, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                email,
                DateTime.UtcNow,
                DateTime.UtcNow.Add(FormsAuthentication.Timeout),
                isPersistent,
                string.Empty,
                FormsAuthentication.FormsCookiePath);

            var encTicket = FormsAuthentication.Encrypt(ticket);

            var authCookie = new HttpCookie(CookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };

            HttpContext.Response.Cookies.Set(authCookie);
        }

        private void LoginUserByCredentials(string email, string password)
        {
            try
            {
                if (Login(email, password, false) != null)
                {
                    _currentUser = new UserProvider(email, _userService);
                }
            }
            catch (ArgumentException ex)
            {
                AbortRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                AbortRequest(ex.Message);
            }
        }

        private void AbortRequest(string message)
        {
            HttpContext.Response.Write(message);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            HttpContext.Response.End();
        }
    }
}