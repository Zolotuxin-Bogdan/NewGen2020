using Gallery.BLL;
using Gallery.BLL.Contracts;
using Gallery.DAL;
using Gallery.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Gallery.PL.Controllers
{
    public class AccountController : Controller
    {
        private IUsersService _usersService;
        public AccountController(IUsersService usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public AccountController() : this(new UsersService(new UsersRepository(new UserContext()))) { }

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
                    FormsAuthentication.SetAuthCookie(model.Name, true);
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

                    FormsAuthentication.SetAuthCookie(model.Name, true);
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
            return View();
        }

    }
}