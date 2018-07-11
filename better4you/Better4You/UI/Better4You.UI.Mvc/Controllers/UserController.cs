using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Better4You.UserManagment.Service;
using Better4You.UserManagment.Service.Messages;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;
using Better4You.Core;

namespace Better4You.UI.Mvc.Controllers
{
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            if (userService == null) throw new ArgumentNullException("userService");
            _userService = userService;
        }



        public ActionResult Edit(int? id)
        {

            var response = id.HasValue ? _userService.GetUser(new GetUserRequest { UserId = id.Value }) :
                new GetUserResponse
                {
                    User = new UserViewModel
                    {
                        UserTypeId = (int)UserTypes.School
                    }
                };

            ViewBag.UserTypes = Enum.GetValues(typeof(UserTypes)).Cast<UserTypes>().Select(
                x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = ((int)x).ToString(System.Globalization.CultureInfo.InvariantCulture),
                    Selected = ((int)x) == response.User.UserTypeId
                }).ToList();

            var selectListItems = (List<SelectListItem>)TempData["Roles"];
            if (selectListItems == null || !selectListItems.Any())
            {
                var roles = _userService.GetAllRoles(new GetAllRolesRequest()).List.Select(d => new SelectListItem { Selected = false, Text = d.Text, Value = d.Value }).ToList();
                TempData.Add("Roles", roles);
            }
            return View("Edit", new UpdateUserRequest
            {
                ApplicationId = CurrentUser.CurrentApplicationId(),
                User = response.User


            });
        }
        [HttpPost]
        public ActionResult Edit(UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.UserTypes = Enum.GetValues(typeof(UserTypes)).Cast<UserTypes>().Select(
                x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = ((int)x).ToString(System.Globalization.CultureInfo.InvariantCulture),
                    Selected = ((int)x) == request.User.UserTypeId
                }).ToList();

                var selectListItems1 = (List<SelectListItem>)TempData["Roles"];
                if (selectListItems1 == null || !selectListItems1.Any())
                {
                    var roles = _userService.GetAllRoles(new GetAllRolesRequest()).List.Select(d => new SelectListItem { Selected = false, Text = d.Text, Value = d.Value }).ToList();
                    TempData.Add("Roles", roles);
                }

                return View(request);
            }

            var response = _userService.UpdateUser(request);

            if (response.Result == Result.Success)
            {
                InfoMessage = response.Message;
                return RedirectToAction("Index");
            }

            ViewBag.UserTypes = Enum.GetValues(typeof(UserTypes)).Cast<UserTypes>().Select(
                x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = ((int)x).ToString(System.Globalization.CultureInfo.InvariantCulture),
                    Selected = ((int)x) == request.User.UserTypeId
                }).ToList();


            var selectListItems = (List<SelectListItem>)TempData["Roles"];
            if (selectListItems == null || !selectListItems.Any())
            {
                var roles = _userService.GetAllRoles(new GetAllRolesRequest()).List.Select(d => new SelectListItem { Selected = false, Text = d.Text, Value = d.Value }).ToList();
                TempData.Add("Roles", roles);
            }
            //ViewBag.Roles = _userService.GetAllRoles(new GetAllRolesRequest()).List.Select(d => new SelectListItem { Selected = false, Text = d.Text, Value = d.Value }).ToList();
            ViewBag.Roles = TempData["Roles"];
            ErrorMessage = response.Message;

            return View(request);
        }
        public ActionResult UserViewModel(UserViewModel validate)
        {
            if (string.IsNullOrEmpty(validate.UserName))
            { ModelState.AddModelError("UserName", "UserName is requied"); }

            if (string.IsNullOrEmpty(validate.FirstName))
            { ModelState.AddModelError("FirstName", "FirstName is requied"); }

            if (string.IsNullOrEmpty(validate.LastName))
            { ModelState.AddModelError("LastName", "LastName is requied"); }

            if (string.IsNullOrEmpty(validate.Phone))
            { ModelState.AddModelError("Phone", "LastName is requied"); }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Edit");
            }

            return View(validate);
        }
        public ActionResult Index()
        {
            GetApplicationUsersRequest request = new GetApplicationUsersRequest
            {
                ApplicationId = CurrentUser.CurrentApplicationId(),
                Filter = new UserViewModelFilter()

            };
            TryUpdateModel(request.Filter);
            var result = _userService.GetApplicationUsersByFilter(request);

            return View(result.Users.OrderBy(c => c.LastName));
        }

        [HttpPost]
        public ActionResult ChangeStatus(int id, bool isLocked)
        {

            var response = isLocked ? _userService.UnLockUser(new UnLockUserRequest { UserId = id }) : _userService.LockUser(new LockUserRequest { UserId = id });
            if (response.Result == Result.Success)
                InfoMessage = response.Message;
            else
                ErrorMessage = response.Message;
            return RedirectToAction("Edit", "User", new { id });
        }



        public ActionResult Details(int? id)
        {
            if (!id.HasValue) return Index();
            var response = _userService.GetUser(new GetUserRequest { UserId = id.Value });
            return View("Display", response.User);
        }



        [HttpPost]
        public ActionResult AssignRole(int id, int roleId)
        {

            var response = _userService.AddRoleToUser(new AddRoleToUserRequest { RoleId = roleId, UserId = id });
            if (response.Result == Result.Success)
                InfoMessage = response.Message;
            else
                ErrorMessage = response.Message;

            return RedirectToAction("Edit", "User", new { id });
        }

    }
}
