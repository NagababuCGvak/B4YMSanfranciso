
using System.Linq;
using System.Web.Mvc;
using Better4You.Meal.Config;
using SysMngConfig = Better4You.UserManagement.Config;
using Better4You.Service;
using Better4You.UserManagment.Service;
using Better4You.UserManagment.Service.Messages;
using Better4You.UserManagment.ViewModel;
using Better4You.Meal.Service;
using Tar.Service.Messages;

namespace Better4You.UI.Mvc.Controllers
{
    [Authorize]
    public class SchoolController : ControllerBase
    {
        public ISchoolService SchoolService { get; set; }
        public ILookupService LookupService { get; set; }
        public IMealMenuService MealMenuService { get; set; }
        public IUserService UserService { get; set; }
        //
        // GET: /School/
        public ActionResult Index()
        {
            var schoolTypeId = (long)SysMngConfig.SchoolTypes.None;
            if (!string.IsNullOrWhiteSpace(Request["SchoolTypeId"]))
                schoolTypeId = long.Parse(Request["SchoolTypeId"]);
            var schoolTypes = SysMngConfig.Lookups.GetItems<SysMngConfig.SchoolTypes>();
            ViewBag.SchoolTypes = schoolTypes.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == schoolTypeId) }).ToList();


            var recordStatusId = (long)SysMngConfig.RecordStatuses.None;
            if (!string.IsNullOrWhiteSpace(Request["RecordStatusId"]))
                recordStatusId = long.Parse(Request["RecordStatusId"]);
            var recordStatuses = SysMngConfig.Lookups.GetItems<SysMngConfig.RecordStatuses>();

            ViewBag.RecordStatuses = recordStatuses.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == recordStatusId) }).ToList();

            var request = new SchoolGetAllRequest
                {
                    //Filter = new SchoolFilterView { RecordStatusId = recordStatusId ,SchoolTypeId = schoolTypeId},
                    Filter = new SchoolFilterView (),
                    OrderByAsc = true,
                    OrderByField = "Name",
                    PageIndex = 1,
                    PageSize = 2000
                };
            TryUpdateModel(request.Filter);

            var result = SchoolService.GetAllByFilter(request);
            return View(result.Schools);
        }

        [HttpPost]
        public JsonResult List()
        {
            var schoolTypes = SysMngConfig.Lookups.GetItems<SysMngConfig.SchoolTypes>();
            ViewBag.SchoolTypes = schoolTypes.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == 0) }).ToList();
            var request = new SchoolGetAllRequest
            {
                Filter = new SchoolFilterView(),
                OrderByField = "Name",
                PageIndex = 1,
                PageSize = 2000,
                OrderByAsc = true,
            };
            TryUpdateModel(request.Filter);
            var result = SchoolService.GetAllByFilter(request);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        public ActionResult Edit(int? id)
        {

            var school = id.HasValue
                ? SchoolService.Get(new SchoolGetRequest {Id = id.Value}).School
                : new SchoolView{SchoolType = (long)SysMngConfig.SchoolTypes.None,RecordStatus = (long)SysMngConfig.RecordStatuses.Active};

            var schoolTypes = SysMngConfig.Lookups.GetItems<SysMngConfig.SchoolTypes>();
            ViewBag.SchoolTypes = schoolTypes.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == school.SchoolType) }).ToList();

            var foodServiceTypes = SysMngConfig.Lookups.GetItems<SysMngConfig.FoodServiceType>();
            ViewBag.FoodServiceTypes = foodServiceTypes.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == school.FoodServiceType) }).ToList();

            var ovsTypes = SysMngConfig.Lookups.GetItems<SysMngConfig.OVSType>();
            ViewBag.OVSTypes = ovsTypes.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == school.OVSType) }).ToList();
            
            var recordStatuses = SysMngConfig.Lookups.GetItems<SysMngConfig.RecordStatuses>();
            ViewBag.RecordStatuses = recordStatuses.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == school.RecordStatus) }).ToList();

            return View(school);
        }

        [HttpPost]
        public ActionResult Edit(SchoolView school)
        {

            if (!ModelState.IsValid) return View(school);
            var response = SchoolService.Save(new SchoolSaveRequest
            {
                School = school
            });

            if (response.Result == Result.Success)
                InfoMessage = response.Message;
            else
                ErrorMessage = response.Message;

            var schoolTypes = SysMngConfig.Lookups.GetItems<SysMngConfig.SchoolTypes>();
            ViewBag.SchoolTypes = schoolTypes.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == school.SchoolType) }).ToList();

            var foodServiceTypes = SysMngConfig.Lookups.GetItems<SysMngConfig.FoodServiceType>();
            ViewBag.FoodServiceTypes = foodServiceTypes.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == school.FoodServiceType) }).ToList();

            var ovsTypes = SysMngConfig.Lookups.GetItems<SysMngConfig.OVSType>();
            ViewBag.OVSTypes = ovsTypes.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == school.OVSType) }).ToList();
           
            var recordStatuses = SysMngConfig.Lookups.GetItems<SysMngConfig.RecordStatuses>();
            ViewBag.RecordStatuses = recordStatuses.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == school.RecordStatus) }).ToList();

            return View(response.School);
            
        }



        [HttpPost]
        public JsonResult GetUsers(int id, string filter)
        {

            return Json(SchoolService.GetSchoolUser(new SchoolUserFilterRequest {FilterString = filter}),
                 JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public ActionResult EditAnnualAgreement(int id, int? annualAgreementId)
        {
            var annualAgreement = new SchoolAnnualAgreementRequest
            {
                SchoolId = id,
                SchoolAnnualAgreement = new SchoolAnnualAgreementView { ItemType = Lookups.GetItem<AnnualItemTypes>((long)AnnualItemTypes.None), RecordStatus = SysMngConfig.Lookups.GetItem<SysMngConfig.RecordStatuses>((long)SysMngConfig.RecordStatuses.Active) }
            };
            if (annualAgreementId.HasValue && annualAgreementId.Value > 0)
                annualAgreement.SchoolAnnualAgreement = SchoolService.GetSchoolAnnualAgreement(
                    new SchoolAnnualAgreementRequest
                    {
                        SchoolId = id,
                        SchoolAnnualAgreement = new SchoolAnnualAgreementView { Id = annualAgreementId.Value }
                    }).SchoolAnnualAgreement;
            
            //ViewBag.MealTypes =Lookups.GetItems<MealTypes>().Select(d=>new SelectListItem{Selected = d.Id==annualAgreement.SchoolAnnualAgreement.MealType.Id,Text=d.Text,Value = d.Id.ToString()}).ToList();
            ViewBag.AnnualItemTypes = Lookups.GetItems<AnnualItemTypes>().Select(d => new SelectListItem { Selected = (d.Id == annualAgreement.SchoolAnnualAgreement.ItemType.Id), Text = d.Text, Value = d.Id.ToString() }).ToList();
            
            return View(annualAgreement);
        }

        [HttpPost]
        public ActionResult EditAnnualAgreement(SchoolAnnualAgreementRequest request)
        {
            var actionParam = Request["actionParam"];
            if (ModelState.IsValid)
            {
                if (request.SchoolAnnualAgreement.Id > 0)
                {
                    if (actionParam == "S")
                    {
                        var updateResponse = SchoolService.UpdateSchoolAnnualAgreement(request);
                        if (updateResponse.Result == Result.Failed)
                            ErrorMessage = updateResponse.Message;
                    }
                    else
                    {
                        var deleteResponse = SchoolService.DeleteSchoolAnnualAgreement(request);
                        if (deleteResponse.Result == Result.Failed)
                            ErrorMessage = deleteResponse.Message;                        
                    }
                }
                else
                {
                    var createResponse = SchoolService.CreateSchoolAnnualAgreement(request);
                    if (createResponse.Result == Result.Failed)
                        ErrorMessage = createResponse.Message;
                }
            }
            return RedirectToAction("Edit", "School", new { id = request.SchoolId });

        }



        [HttpGet]
        public ActionResult EditRoute(int id, int? schoolRouteId)
        {
            var schoolRoute = new SchoolRouteRequest
                              {
                SchoolId = id,
                SchoolRoute = new SchoolRouteView { MealType = Lookups.GetItem<MealTypes>((long)MealTypes.None), RecordStatus = SysMngConfig.Lookups.GetItem<SysMngConfig.RecordStatuses>((long)SysMngConfig.RecordStatuses.Active) }
            };
            if (schoolRouteId.HasValue && schoolRouteId.Value > 0)
                schoolRoute.SchoolRoute = SchoolService.GetSchoolRoute(
                    new SchoolRouteRequest
                    {
                        SchoolId = id,
                        SchoolRoute = new SchoolRouteView { Id = schoolRouteId.Value }
                    }).SchoolRoute;
            ViewBag.MealTypes = Lookups.GetItems<MealTypes>().Select(d => new SelectListItem { Selected = d.Id == schoolRoute.SchoolRoute.MealType.Id, Text = d.Text, Value = d.Id.ToString() }).ToList();

            return View(schoolRoute);
        }

        [HttpPost]
        public ActionResult EditRoute(SchoolRouteRequest request)
        {
            var actionParam = Request["actionParam"];
            if (ModelState.IsValid)
            {
                if (request.SchoolRoute.Id > 0)
                {
                    if (actionParam == "S")
                    {
                        var updateResponse = SchoolService.UpdateSchoolRoute(request);
                        if (updateResponse.Result != Result.Success)
                            ErrorMessage = updateResponse.Message;
                    }
                    else
                    {
                        var deleteResponse = SchoolService.DeleteSchoolRoute(request);
                        if (deleteResponse.Result != Result.Success)
                            ErrorMessage = deleteResponse.Message;
                    }
                }
                else
                {
                    var createResponse = SchoolService.CreateSchoolRoute(request);
                    if (createResponse.Result != Result.Success)
                        ErrorMessage = createResponse.Message;
                }

            }
            return RedirectToAction("Edit", "School", new { id = request.SchoolId });
        }

        #region EditUser
        [HttpGet]
        public ActionResult EditUser(long userId, long schoolId, string actionParam)
        {
            ViewBag.ActionParam = actionParam;
            var schoolUserRequest = new SchoolUserRequest
            {
                SchoolId = schoolId,
                UserId = userId
            };
            if (userId > 0)
            {
                var user = UserService.GetUser(new GetUserRequest { UserId = userId });
                ViewBag.FullName = user.Result == Result.Success && user.User != null
                    ? string.Format("{0}, {1}", user.User.LastName, user.User.FirstName)
                    : "User Not Found!";
            }
            
            ViewBag.Title = actionParam == "C" ? "Add User" : "Delete User";
            return PartialView(schoolUserRequest);
        }

        [HttpPost]
        public ActionResult EditUser(SchoolUserRequest request, string actionParam)
        {
            if (request.UserId > 0 ||request.SchoolId>0)
            {
                if (actionParam == "C")
                {
                    var response = SchoolService.CreateUser(new SchoolUserRequest { SchoolId = request.SchoolId, UserId = request.UserId });
                    if (response.Result == Result.Success)
                    {
                        //InfoMessage = response.Message;
                        return RedirectToAction("Edit", "School", new { id = request.SchoolId });
                    }
                    ErrorMessage = response.Message;
                }
                else
                {
                    var response =
                        SchoolService.DeleteUser(new SchoolUserRequest
                        {
                            SchoolId = request.SchoolId,
                            UserId = request.UserId
                        });
                    if (response.Result == Result.Success)
                    {
                        //InfoMessage = response.Message;
                        return RedirectToAction("Edit", "School", new { id = request.SchoolId });
                    }
                    ErrorMessage = response.Message;
                }
                
                
            }
            ErrorMessage = "User or School Not Exist in system";
            return RedirectToAction("Edit", "School", new { id = request.SchoolId });
        }
        #endregion

        [HttpPost]
        public ActionResult RemoveUserFromSchool(int schoolId, int userId)
        {
            if (userId > 0)
            {                
                SchoolService.DeleteUser(new SchoolUserRequest { SchoolId = schoolId, UserId = userId });
            }

            return RedirectToAction("Edit", "School", new { id = schoolId });
        }
    }
}
