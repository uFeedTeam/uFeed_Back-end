using System;
using System.Web.Http;
using uFeed.BLL.Infrastructure.Exceptions;
using uFeed.BLL.Interfaces;
using uFeed.WEB.Account.Interfaces;
using uFeed.WEB.ViewModels.Account;

namespace uFeed.WEB.Controllers
{
    [RoutePrefix("api")]
    public class AccountController : BaseController
    {
        private readonly IAuthentication _auth;
        private readonly IUserService _userService;

        public AccountController(IAuthentication authentication, IUserService userService)
        {
            _auth = authentication;
            _userService = userService;
        }

        [HttpPost, Route("token")]
        public IHttpActionResult Token(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = _auth.Token(loginViewModel.Email, loginViewModel.Password);
                    return Ok(token);
                }
                catch (EntityNotFoundException)
                {
                    ModelState.AddModelError(string.Empty, "Email or password are invalid");
                }
                catch (ArgumentException)
                {
                    ModelState.AddModelError(string.Empty, "Email or password are invalid");
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost, Route("register")]
        public IHttpActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userService.Register(registerViewModel.Email, registerViewModel.Name, registerViewModel.Password);
                    return Ok();
                }
                catch (ArgumentException)
                {
                    ModelState.AddModelError(string.Empty, "Email or password are invalid");
                }
            }

            return BadRequest(ModelState);
        }
    }
}