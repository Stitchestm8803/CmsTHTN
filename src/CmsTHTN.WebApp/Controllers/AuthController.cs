using CmsTHTN.Core.ConfigOptions;
using CmsTHTN.Core.Domain.Identity;
using CmsTHTN.Core.Events.LoginSuccessed;
using CmsTHTN.Core.Events.RegisterSuccessed;
using CmsTHTN.Core.SeedWorks.Constants;
using CmsTHTN.WebApp.Extensions;
using CmsTHTN.WebApp.Models;
using CmsTHTN.WebApp.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CmsTHTN.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMediator _mediator;
        private readonly IEmailSender _emailSender;
        private readonly SystemConfig _systemConfig;
        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IMediator mediator,
            IEmailSender emailSender, IOptions<SystemConfig> systemConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
            _emailSender = emailSender;
            _systemConfig = systemConfig.Value;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegsiterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userManager.CreateAsync(new AppUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
            }, model.Password);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                await _signInManager.SignInAsync(user, true);
                await _mediator.Publish(new RegisterSuccessedEvent(model.Email));
                return Redirect(UrlConsts.Profile);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email nhập không đúng hoặc không tồn tại!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _mediator.Publish(new LoginSuccessedEvent(model.Email));

                return Redirect(UrlConsts.Profile);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Đăng nhập thất bại!");
            }
            return View();
        }
        [HttpGet]
        [Route("forgot-password")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("forgot-password")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không thể tìm được người dùng có Email này");
            }

            // For more information on how to enable account confirmation and password reset please
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);

            //var emailData = new EmailData
            //{
            //    ToEmail = user.Email ?? string.Empty,
            //    Subject = $"{_systemConfig.AppName} - Lấy lại mật khẩu",
            //    Content = $"Chào {user.FirstName}. Bạn vừa gửi yêu cầu lấy lại mật khẩu tại {_systemConfig.AppName}. Click: <a href='{callbackUrl}'>vào đây</a> để đặt lại mật khẩu. Trân trọng."
            //};
            //await _emailSender.SendEmail(emailData);


            TempData[SystemConsts.FormSuccessMsg] = "Kiểm tra email của bạn để lấy mã";
            return Redirect(UrlConsts.Login);
        }

        [HttpGet]
        [Route("reset-password")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("Bạn phải nhập mã xác nhận");
            }
            return View(new ResetPasswordViewModel { Code = code });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ModelState.AddModelError(string.Empty, "Email không tồn tại");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                TempData[SystemConsts.FormSuccessMsg] = "Đổi mật khẩu thành công";
                return Redirect(UrlConsts.Login);
            }
            return View();
        }

    }
}
