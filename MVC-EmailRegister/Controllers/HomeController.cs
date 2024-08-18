
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_EmailRegister.MailServices;
using MVC_EmailRegister.Models;
using System.Diagnostics;

namespace MVC_EmailRegister.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMailService _mailService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IMailService mailService, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _mailService = mailService;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var user = new IdentityUser { UserName = registerVM.Email, Email = registerVM.Email };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (result.Succeeded)
            {
                var codeToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callBackURL = Url.Action("ConfirmEmail", "Account", new { userýd = user.Id, code = codeToken }, protocol: Request.Scheme);
                var mailMessage = $"Please confirm your account by"  +
                    $"<a href='{callBackURL}'>Click here</a>.";
                await _mailService.SendMailAsync(registerVM.Email, "Confim your email", mailMessage);
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ConfirmEmail(string userId,string code)
        {
            if (userId is null || code is null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Kullanýcý geçersiz");

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "Index" : "Eror");
        }


        //public async Task<IActionResult> Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginVM loginVM)
        //{var user = await _userManager.FindByEmailAsync(loginVM.Email);

        //    if (user == null)
        //    {
        //        return View(loginVM);
        //    }
        //    var chechkPassword = await _signInManager.
        //    return View(loginVM);
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
