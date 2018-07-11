using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Better4You.Core;
using Better4You.Service;
using Better4You.UI.Mvc.ViewModels;
using Better4You.UserManagment.Service;
using Better4You.UserManagment.Service.Messages;
using Better4You.UserManagment.ViewModel;
using Tar.Security;
using Tar.Service.Messages;
using System.Text;
using Tar.Core.Configuration;

namespace Better4You.UI.Mvc.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthentication _authentication;
        public ISchoolService SchoolService { get; set; }
        public ILookupService LookupService {
            get {return ServiceLocator.Get<ILookupService>(); }
        }
        public ISettings Settings { get; set; }
        public AccountController(IUserService userService, IAuthentication authentication)
        {
            if (userService == null) throw new ArgumentNullException("userService");
            if (authentication == null) throw new ArgumentNullException("authentication");

            _userService = userService;
            _authentication = authentication;
        }

        public ActionResult Login()
        {
            if (CurrentUser.IsAuthenticated) return RedirectToAction("Index", "Home");
            Session["ReturnUrl"] = Request.Params["ReturnUrl"];
            return View("Edit", new LoginRequest());
        }

        [HttpPost]
        public ActionResult Login(LoginRequest request)
        {
            var response = _userService.Login(request);
            if (response.Result == Result.Success)
            {
                _authentication.SignIn(request.UserName, request.Password);
                FormsAuthentication.SetAuthCookie(request.UserName, false);
                return RedirectToAction("SelectSchool");
            }
            ErrorMessage = response.Message;

            return View("Edit", request);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            CurrentUser = new CurrentUser();
            return RedirectToAction("Login");
        }

        public ActionResult SelectSchool()
        {
            if (Session["ReturnUrl"] == null)
                if (Request.UrlReferrer != null)
                    Session["ReturnUrl"] = Request.UrlReferrer.ToString();
                else
                    Session["ReturnUrl"] = Url.Action("Index", "Home");

            var model = new SelectSchoolViewModel(
                CurrentUser,
                Session["ReturnUrl"] == null ? null : Session["ReturnUrl"].ToString());

            if (model.CanSelect()) return View(model);

            var school = CurrentUser.Schools().FirstOrDefault(d => d.Id == CurrentUser.CurrentSchoolId());
            if (school != null)
                CurrentUser.Data["SchoolName"] = school.Text;
            return Redirect(model.ReturnUrl);
        }

        [HttpPost]
        public ActionResult SelectSchool(string returnUrl, int? applicationId, int? schoolId)
        {
            if (applicationId.HasValue)
                CurrentUser.CurrentApplicationId(applicationId.Value);

            if (schoolId.HasValue)
            {
                CurrentUser.CurrentSchoolId(schoolId.Value);
                CurrentUser.Data["SchoolName"] = "";
                var school = CurrentUser.Schools().FirstOrDefault(d => d.Id == CurrentUser.CurrentSchoolId());
                if (school != null)CurrentUser.Data["SchoolName"] = school.Text;
            }
            //if (!string.IsNullOrEmpty(returnUrl) 
            //    && returnUrl != "/") return Redirect(returnUrl);

            return RedirectToHomeIndex();
        }


        public ActionResult ResetPassword(int? id)
        {
            if (id.HasValue)
            {
                var response = _userService.ResetPassword(new ResetPasswordRequest { UserId = id.Value });
                if (response.Result == Result.Success)
                {
                    var user = _userService.GetUser(new GetUserRequest { UserId = id.Value });
                    StringBuilder message = new StringBuilder()
                .AppendFormat("{0}/Account/ChangePassword?ActivationCode={1}", Settings.GetSetting<string>("HostServer"), user.User.ActivitationCode)
                .AppendLine("<br/>")
                .AppendFormat("ValidDate:{0}", user.User.ExpireActivationDate);
                    InfoMessage = string.Format("{0}. Link: {1}", response.Message, message);
                }
                else
                    ErrorMessage = response.Message;
            }


            return RedirectToAction("Edit", "User",new {id});
        }

        public ActionResult ChangePassword(string activationCode)
        {
            return View("Edit", new ChangePasswordRequest { ActivationCode = activationCode });
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordRequest request)
        {
            if (!ModelState.IsValid) return View("Edit", request);

            var response = _userService.ChangePassword(request);
            if (response.Result == Result.Success)
            {
                InfoMessage = "Your password has been updated successfully.";
                return RedirectToAction("Login", "Account");
            }

            ErrorMessage = response.Message;
            return View("Edit", request);
        }

        #region Forgot Password
        public ActionResult ForgotPassword()
        {
            return View("Edit", new ForgotPasswordRequest());
        }
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid) return View("Edit", request);

            var response = _userService.ForgotPassword(request);
            if (response.Result == Result.Success)
            {
                InfoMessage = "If your email account exists in the system, you should receive an email that explains you how to reset your password.";
                return View("ForgotPassword");
            }

            ErrorMessage = response.Message;
            return View("Edit", request);
        }
        #endregion Forgot Password

        #region Contact Info
        public ActionResult ContactInfo()
        {
            if (!CurrentUser.IsAuthenticated) return RedirectToAction("Login");
            var contactRegards = LookupService.ContactRegards().List.ToList();
            contactRegards.Insert(0, new KeyValuePair<long, string>(0, "What is this in regard to?"));
            ViewBag.ContactRegards = contactRegards;
            var view = new ContactInfoView
                {
                    Email = CurrentUser.Name,
                    FirstName = CurrentUser.Data["FirstName"].ToString(),
                    LastName = CurrentUser.Data["LastName"].ToString()
                };
            return View(view);
        }
        [HttpPost]
        public ActionResult ContactInfo(ContactInfoView request)
        {
            if (!CurrentUser.IsAuthenticated) return RedirectToAction("Login");
            request.ContactRegard =
                    new KeyValuePair<long, string>(Convert.ToInt64(Request["ContactRegard.Key"]), "");
            /*
            var school = CurrentUser.Schools()
                    .FirstOrDefault(d => d.Value == CurrentUser.CurrentSchoolId().ToString(CultureInfo.InvariantCulture));
            
            request.School =  new KeyValuePair<long, string>(CurrentUser.CurrentSchoolId(), school != null ? school.Text : "");
            */

            request.School = new KeyValuePair<long, string>(CurrentUser.CurrentSchoolId(), CurrentUser.Data["SchoolName"].ToString());
            request.UserId = Convert.ToInt32(CurrentUser.Data["UserId"]);
            request.Email = CurrentUser.Name;
            request.FirstName = CurrentUser.Data["FirstName"].ToString();
            request.LastName = CurrentUser.Data["LastName"].ToString();
            var response = SchoolService.CreateContactInfo(new CreateContactInfoRequest { ContactInfo = request });
            if (response.Result == Result.Success)
            {
                InfoMessage = "Message sent!";
                return RedirectToAction("ContactInfo");
            }
            var contactRegards = LookupService.ContactRegards().List.ToList();
            contactRegards.Insert(0, new KeyValuePair<long, string>(0, "Select Regards To"));
            ViewBag.ContactRegards = contactRegards;
            ErrorMessage = response.Message;
            return View(request);
        }
        #endregion Contact Info
    }
}
