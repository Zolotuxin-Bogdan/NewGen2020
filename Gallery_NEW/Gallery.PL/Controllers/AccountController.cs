using Gallery.BLL;
using Gallery.BLL.Contracts;
using Gallery.DAL;
using Gallery.DAL.Model;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Gallery.PL.Interfaces;
using Gallery.PL.Services;

namespace Gallery.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IUsersService usersService, IAuthenticationService authenticationService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        public AccountController() : this(new UsersService(new UsersRepository(new UserContext())), new AuthenticationService()) { }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var canAuthorize = await _usersService.IsUserExistAsync(model.Name, model.Password);

                if (canAuthorize)
                {
                    var userId = _usersService.GetUserId(model.Name);
                    var claim = _authenticationService.ClaimsСreation(userId.ToString());

                    _authenticationService.AuthByOwinCookies(HttpContext.GetOwinContext(), claim);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "User not found");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var isUserExist = await _usersService.IsUserExistAsync(model.Name, model.Password);

                if (isUserExist == false)
                {
                    CreateUserDTO userDTO = new CreateUserDTO(model.Name, model.Password);
                    await _usersService.RegisterUserAsync(userDTO);

                    var userId = _usersService.GetUserId(model.Name);
                    var claim = _authenticationService.ClaimsСreation(userId.ToString());
                    _authenticationService.AuthByOwinCookies(HttpContext.GetOwinContext(), claim);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "User already exists");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            _authenticationService.LogOut(HttpContext.GetOwinContext());
            return RedirectToAction("Index", "Home");
        }

    }
}