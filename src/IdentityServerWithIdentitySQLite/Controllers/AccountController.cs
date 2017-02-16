using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Models;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Http.Authentication;
using IdentityServer4.Extensions;
using ASPNETCoreIdentitySample.Services.Contracts.Identity;
using ASPNETCoreIdentitySample.ViewModels.Identity;
using ASPNETCoreIdentitySample.ViewModels.Identity.Server4Vms;
using ASPNETCoreIdentitySample.Entities.Identity;
using DNTBreadCrumb.Core;
using ASPNETCoreIdentitySample.ViewModels.Identity.Settings;
using Microsoft.Extensions.Options;
using DNTCaptcha.Core;
using IdentityServerWithAspNetIdentitySqlite.Controllers;
using ASPNETCoreIdentitySample.Common.WebToolkit;
using ASPNETCoreIdentitySample.Common.IdentityToolkit;
using ASPNETCoreIdentitySample.ViewModels.Identity.Emails;
using DNTPersianUtils.Core;
using ASPNETCoreIdentitySample.Common.GuardToolkit;

namespace IdentityServerWithAspNetIdentity.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        #region #Settings
        private readonly IApplicationUserManager _userManager;
        private readonly IPasswordValidator<User> _passwordValidator;
        private readonly IUserValidator<User> _userValidator;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IPersistedGrantService _persistedGrantService;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;

        public AccountController(
            IApplicationUserManager userManager,
            IPasswordValidator<User> passwordValidator,
            IUserValidator<User> userValidator,
            IPersistedGrantService persistedGrantService,
            IApplicationSignInManager signInManager,
            IIdentityServerInteractionService interaction,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,
            IClientStore clientStore,
            IOptionsSnapshot<SiteSettings> siteOptions)
        {
            _userManager = userManager;
            _persistedGrantService = persistedGrantService;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _interaction = interaction;
            _clientStore = clientStore;


            _passwordValidator = passwordValidator;
            _passwordValidator.CheckArgumentIsNull(nameof(_passwordValidator));

            _userValidator = userValidator;
            _userValidator.CheckArgumentIsNull(nameof(_userValidator));

            _siteOptions = siteOptions;
            _siteOptions.CheckArgumentIsNull(nameof(_siteOptions));
        }
        #endregion

        #region #Login & LogOut
        //
        // GET: /Account/Login
        [HttpGet]
        [BreadCrumb(Title = "ورود", Order = 1)]

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            //if (context?.IdP != null)
            //{
            //    // if IdP is passed, then bypass showing the login screen
            //    return ExternalLogin(context.IdP, returnUrl);
            //}

            var vm = await BuildLoginViewModelAsync(returnUrl, context);

            //if (vm.EnableLocalLogin == false && vm.ExternalProviders.Count() == 1)
            //{
            //    // only one option for logging in
            //    return ExternalLogin(vm.ExternalProviders.First().AuthenticationScheme, returnUrl);
            //}

            return View(vm);
        }


        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(CaptchaGeneratorLanguage = DNTCaptcha.Core.Providers.Language.Persian)]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "نام کاربری و یا کلمه‌ی عبور وارد شده معتبر نیستند.");
                    return View(await BuildLoginViewModelAsync(model));
                }

                if (!user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "اکانت شما غیرفعال شده‌است.");
                    return View(await BuildLoginViewModelAsync(model));
                }

                if (_siteOptions.Value.EnableEmailConfirmation &&
                    !await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false))
                {
                    ModelState.AddModelError("", "لطفا به پست الکترونیک خود مراجعه کرده و ایمیل خود را تائید کنید!");
                    return View(await BuildLoginViewModelAsync(model));
                }

                var result = await _signInManager.PasswordSignInAsync(
                                        model.Username,
                                        model.Password,
                                        model.RememberLogin,
                                        lockoutOnFailure: true).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"{model.Username} logged in.");
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(
                        nameof(TwoFactorController.SendCode),
                        "TwoFactor",
                        new { ReturnUrl = returnUrl, RememberMe = model.RememberLogin });
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, $"{model.Username} قفل شده‌است.");
                    return View("~/Views/TwoFactor/Lockout.cshtml");
                }

                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "عدم دسترسی ورود.");
                    return View(await BuildLoginViewModelAsync(model));
                }

                ModelState.AddModelError(string.Empty, "نام کاربری و یا کلمه‌ی عبور وارد شده معتبر نیستند.");
                return View(await BuildLoginViewModelAsync(model));
            }

            // If we got this far, something failed, redisplay form
            return View(await BuildLoginViewModelAsync(model));
        }



        #region #BuildLoginViewModelAsync
        async Task<LoginExtraViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {
            var providers = HttpContext.Authentication.GetAuthenticationSchemes()
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.AuthenticationScheme
                });

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme));
                    }
                }
            }

            return new LoginExtraViewModel
            {
                EnableLocalLogin = allowLocal,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl, context);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }
        #endregion

        /// <summary>
        /// Show logout page
        /// </summary>
        /// 
        [HttpGet]
        [BreadCrumb(Title = "خروج", Order = 1)]

        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                // if the user is not authenticated, then just show logged out page
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            var vm = new LogoutViewModel
            {
                LogoutId = logoutId
            };

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            var subjectId = HttpContext.User.Identity.GetSubjectId();

            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                if (model.LogoutId == null)
                {
                    // if there's no current logout context, we need to create one
                    // this captures necessary info from the current logged in user
                    // before we signout and redirect away to the external IdP for signout
                    model.LogoutId = await _interaction.CreateLogoutContextAsync();
                }

                string url = "/Account/Logout?logoutId=" + model.LogoutId;
                try
                {
                    // hack: try/catch to handle social providers that throw
                    await HttpContext.Authentication.SignOutAsync(idp, new AuthenticationProperties { RedirectUri = url });
                }
                catch(NotSupportedException)
                {
                }
            }

            // delete authentication cookie
            await _signInManager.SignOutAsync();

            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

            var vm = new LoggedOutViewModel
            {
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = logout?.ClientId,
                SignOutIframeUrl = logout?.SignOutIFrameUrl
            };

            await _persistedGrantService.RemoveAllGrantsAsync(subjectId, "angular2client");

            return View("LoggedOut", vm);
        }
        #endregion

        #region #Register
        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateUsername(string username, string email)
        {
            var result = await _userValidator.ValidateAsync(
                (UserManager<User>)_userManager, new User { UserName = username, Email = email }).ConfigureAwait(false);
            return Json(result.Succeeded ? "true" : result.DumpErrors(useHtmlNewLine: true));
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidatePassword(string password, string username)
        {
            var result = await _passwordValidator.ValidateAsync(
                (UserManager<User>)_userManager, new User { UserName = username }, password).ConfigureAwait(false);
            return Json(result.Succeeded ? "true" : result.DumpErrors(useHtmlNewLine: true));
        }
        //
        // GET: /Account/Register
        [HttpGet]
        [BreadCrumb(Title = "ثبت نام", Order = 1)]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(CaptchaGeneratorLanguage = DNTCaptcha.Core.Providers.Language.Persian)]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(3, $"{user.UserName} created a new account with password.");

                    if (_siteOptions.Value.EnableEmailConfirmation)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                        //ControllerExtensions.ShortControllerName<RegisterController>(), //todo: use everywhere .................

                        await _emailSender.SendEmailAsync(
                           email: user.Email,
                           subject: "لطفا اکانت خود را تائید کنید",
                           viewNameOrPath: "~/Views/EmailTemplates/_RegisterEmailConfirmation.cshtml",
                           model: new RegisterEmailConfirmationViewModel
                           {
                               User = user,
                               EmailConfirmationToken = code,
                               EmailSignature = _siteOptions.Value.Smtp.FromName,
                               MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                           }).ConfigureAwait(false);

                        return RedirectToAction(nameof(ConfirmYourEmail));
                    }
                    return RedirectToAction(nameof(ConfirmedRegisteration));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        [BreadCrumb(Title = "ایمیل خود را تائید کنید", Order = 1)]
        public IActionResult ConfirmYourEmail()
        {
            return View();
        }

        [BreadCrumb(Title = "تائیدیه ایمیل", Order = 1)]
        public IActionResult ConfirmedRegisteration()
        {
            return View();
        }

        [BreadCrumb(Title = "تائید ایمیل", Order = 1)]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code).ConfigureAwait(false);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        #endregion
        
        
    }
}
