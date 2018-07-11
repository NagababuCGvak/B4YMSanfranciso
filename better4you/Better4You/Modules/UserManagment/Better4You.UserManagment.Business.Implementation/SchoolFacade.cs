using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Better4You.Core;
using Better4You.Core.Repositories;

using Better4You.UserManagment.EntityModel;
using Better4You.UserManagment.ViewModel;
using SysMngConfig=Better4You.UserManagement.Config;

using Tar.Business;
using Tar.Core.Configuration;
using Tar.Core.Mail;


namespace Better4You.UserManagment.Business.Implementation
{
    public class SchoolFacade:ISchoolFacade
    {
        private readonly IConfigRepository _repository;
        public ISettings Settings { get; set; }
        public IMailService MailService { get; set; }

        public SchoolFacade(IConfigRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            _repository = repository;
        }

        public List<SchoolListItemView> GetByFilter(SchoolFilterView filter, int pageSize, int pageIndex, string orderByField, bool orderByAsc, out int totalCount)
        {
            var query = _repository.Query<School>();

            if(!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(d => d.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrWhiteSpace(filter.Code))
            {
                query = query.Where(d => d.Code.Contains(filter.Code));
            }
            if(filter.CityId>0)
            {
                query=query.Where(d => d.CityId == filter.CityId);
            }
            if (filter.StateId > 0)
            {
                query = query.Where(d => d.FirstAdminDivisionId == filter.StateId);
            }

            if (!string.IsNullOrWhiteSpace(filter.Email) )
            {
                query = query.Where(d => d.Email.Contains(filter.Email));
            }
            if (!string.IsNullOrWhiteSpace(filter.Phone))
            {
                query = query.Where(d => d.BusinessPhone.Contains(filter.Phone) || d.CellPhone.Contains(filter.Phone) || d.Fax.Contains(filter.Phone));
            }
            if(filter.SchoolTypeId>0)
            {
                query = query.Where(d => d.SchoolType == filter.SchoolTypeId);
            }
            if (filter.RecordStatusId > 0)
            {
                query = query.Where(d => d.RecordStatus == filter.RecordStatusId);
            }

            switch (orderByField)
            {
                case "Id":
                    query = orderByAsc ? query.OrderBy(c => c.Id) : query.OrderByDescending(c => c.Id);
                    break;
                case "Name":
                    query = orderByAsc ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
                    break;
                case "Code":
                    query = orderByAsc ? query.OrderBy(c => c.Code) : query.OrderByDescending(c => c.Code);
                    break;
                default:
                    query = orderByAsc ? query.OrderBy(c => c.Id) : query.OrderByDescending(c => c.Id);
                    break;
            }
            
            if (!string.IsNullOrWhiteSpace(filter.Route))
            {
                query = (from s in query
                         join sr in _repository.Query<SchoolRoute>()
                         on s.Id equals sr.School.Id
                         where sr.Route == filter.Route
                         select s);
            }
            


            totalCount = query.Count();

            if (pageSize > 0) query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var resultQuery = query.AsEnumerable();

            return resultQuery.Select(d => d.ToView<SchoolListItemView>()).ToList();
        }

        public SchoolView Get(long id)
        {
            return _repository.GetById<School>(id).ToView<SchoolView>();
        }

        public SchoolListItemView GetById(long id)
        {
            /*
            var model = _repository.GetById<School>(id);

            var view = new SchoolListItemView
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code,
                //Route = model.Route,
                SchoolType = model.SchoolType.FieldText,
                SchoolTypeId = model.SchoolType.Id
            };
            #region Addresses
            var address = model.Addresses.FirstOrDefault(
                d => d.AddressType.Id == (long) AddressTypes.Work &&
                     d.RecordStatus.Id == (long) RecordStatuses.Active);
            if(address!=null)
            {
                view.Address1 = address.Address1;
                view.Address2 = address.Address2;
                view.District = address.District;
                view.City = address.City.Name;
                view.CityId = address.City.Id;
                view.StateId = address.FirstAdminDivision.Id;
                view.State = address.FirstAdminDivision.Name;
                view.PostalCode = address.PostalCode;
            }
            #endregion Addresses
            #region Communications
            var communications = model.Communications.ToList();
            var workPhone = communications.FirstOrDefault(
                d => d.CommunicationType.Id == (long) CommunicationTypes.WorkPhone &&
                     d.RecordStatus.Id == (long) RecordStatuses.Active && !string.IsNullOrWhiteSpace(d.Phone));
            if (workPhone != null)
            {
                view.BusinessPhone = workPhone.Phone;
            }
            var homePhone = communications.FirstOrDefault(
                d => d.CommunicationType.Id == (long)CommunicationTypes.HomePhone &&
                     d.RecordStatus.Id == (long)RecordStatuses.Active && !string.IsNullOrWhiteSpace(d.Phone));
            if (homePhone != null)
            {
                view.HomePhone = homePhone.Phone;
            }
            var cellPhone = communications.FirstOrDefault(
                d => d.CommunicationType.Id == (long)CommunicationTypes.Cell &&
                     d.RecordStatus.Id == (long)RecordStatuses.Active && !string.IsNullOrWhiteSpace(d.Phone));
            if (cellPhone != null)
            {
                view.CellPhone = cellPhone.Phone;
            }
            var fax = communications.FirstOrDefault(
                d => d.CommunicationType.Id == (long) CommunicationTypes.Fax &&
                     d.RecordStatus.Id == (long)RecordStatuses.Active && !string.IsNullOrWhiteSpace(d.Phone));
            if (fax != null)
            {
                view.Fax = fax.Phone;
            }
            var email = communications.FirstOrDefault(
                d => d.CommunicationType.Id == (long)CommunicationTypes.Email &&
                     d.RecordStatus.Id == (long)RecordStatuses.Active && !string.IsNullOrWhiteSpace(d.Email));
            if (email != null)
            {
                view.Email = email.Email;
            }
            #endregion Communications
            return view;
            */
            throw new Exception("not handled");
        }

        public SchoolView Save(SchoolView view)
        {
            var dtNow = DateTime.Now;
            var model = view.Id > 0
                ? _repository.GetById<School>(view.Id)
                : new School{RecordStatus = (long)SysMngConfig.RecordStatuses.Active};

            view.SetTo(model);

            var companySchoolUser = Settings.GetSetting<string>("CompanySchoolUser");
            if (model.Users.All(k => k.UserName != companySchoolUser))
            {
                var user = _repository.Query<User>().FirstOrDefault(d => d.UserName == companySchoolUser);
                if (user != null)
                {
                    model.AddUser(user);
                }
            }            

            if (view.Id > 0)
                _repository.Update(model);
            else
                _repository.Create(model);
            return model.ToView<SchoolView>();
        }

        public void Delete(long id)
        {            
            var model = _repository.GetById<School>(id);
            model.RecordStatus = (long)SysMngConfig.RecordStatuses.Deleted ;
            _repository.Update(model);
            
        }

 

        public void CreateUser(long schoolId, long userId)
        {
            var school = _repository.GetById<School>(schoolId);
            school.Users.Add(new User{Id = userId});
            _repository.Update(school);
        }

        public void DeleteUser(long schoolId, long userId)
        {
            var school = _repository.GetById<School>(schoolId);
            List<User> users = new List<User>();
            foreach (var user in school.Users)
            {
                if (user.Id != userId)
                    users.Add(user);
            }
            //school.Users.Remove(new User { Id = userId });
            school.Users = users;
            _repository.Update(school);
        }

        public List<UserItemView> GetUserByFilter(string filter)
        {
            var query =
                _repository.Query<User>().Where(d => (d.LastName+d.FirstName).Contains(filter) && d.UserType.Id==(long)UserTypes.School);                                
            query = query.Skip((DefaultDefinition.DefaultPageIndex - 1) * DefaultDefinition.DefaultPageSize)
                .Take(DefaultDefinition.DefaultPageSize);
            return query.AsEnumerable().Select(d => d.ToView()).ToList();

        }

       // public List<KeyValuePair<int, double>> AnnualAgreementsByFilter(long schoolId, int? year, long mealTypeId)
         public List<KeyValuePair<int, double>> AnnualAgreementsByFilter(long schoolId, int? year, long  itemTypeId)
        {
            var query = _repository.Query<SchoolAnnualAgreement>().Where(d => d.School.Id == schoolId && d.ItemType == itemTypeId && d.RecordStatus == (long)SysMngConfig.RecordStatuses.Active);
            if (year.HasValue)
                query = query.Where(d => d.Year == year.Value);
            return query.Select(d => new KeyValuePair<int, double>(d.Year, d.Price)).ToList();
        }

        public List<KeyValuePair<int, double>> AnnualAgreementsByYearAndId(long schoolId, int? year)
        {
            var query = _repository.Query<SchoolAnnualAgreement>().Where(d => d.School.Id == schoolId && d.RecordStatus == (long)SysMngConfig.RecordStatuses.Active);
            if (year.HasValue)
                query = query.Where(d => d.Year == year.Value);
            return query.Select(d => new KeyValuePair<int, double>(d.Year, d.Price)).ToList();
        } 

        public List<SchoolAnnualAgreementView>AllAnnualAgreementsByYear(int year){
            var query = _repository.Query<SchoolAnnualAgreement>().Where(d => d.Year == year && d.RecordStatus == (long)SysMngConfig.RecordStatuses.Active);
            return query.Select(c => c.ToView()).ToList();
        }
        
        public void CreateAnnualAgreement(long schoolId, SchoolAnnualAgreementView view)
        {
            var school = _repository.GetById<School>(schoolId);
            view.RecordStatus = SysMngConfig.Lookups.GetItem<SysMngConfig.RecordStatuses>((long)SysMngConfig.RecordStatuses.Active);
            var model = view.ToModel();
            model.RecordStatus = (long) SysMngConfig.RecordStatuses.Active;
            model.School = school;
            school.SchoolAnnualAgreements.Add(model);
        }

        public void UpdateAnnualAgreement(long schoolId, SchoolAnnualAgreementView view)
        {
            var model = _repository.GetById<SchoolAnnualAgreement>(view.Id);
            view.SetTo(model);
            _repository.Update(model);
        }

        public void DeleteAnnualAgreement(long schoolId, SchoolAnnualAgreementView view)
        {
            view.RecordStatus = SysMngConfig.Lookups.GetItem<SysMngConfig.RecordStatuses>((long)SysMngConfig.RecordStatuses.Deleted);

            UpdateAnnualAgreement(schoolId, view);
        }

        public SchoolAnnualAgreementView GetSchoolAnnualAgreement(long id)
        {
            return _repository.GetById<SchoolAnnualAgreement>(id).ToView();
        }

        public void CreateContactInfo(ContactInfoView view)
        {
            //TODO Get related Mail Address From Configuration
            var mailAddress = Settings.GetSetting<string>("EmailContactRegardsTo");
            var subject = string.Format("Contact Info - {0},{1}", view.LastName.ToUpperInvariant(), view.FirstName);
            var body = new StringBuilder()
                .AppendLine(string.Format("School : {0}", view.School.Value))
                .AppendLine(string.Format("Email : <{0},{1}>{2}", view.LastName.ToUpperInvariant(), view.FirstName,view.Email))
                .AppendLine(string.Format("Regards To: {0}", view.ContactRegard.Value))
                .AppendLine(string.Format("Message: {0}", view.Message));

            MailService.SendMail(mailAddress, subject, body.ToString());
        }

        public void CreateSchoolRoute(long schoolId, SchoolRouteView view)
        {
            view.RecordStatus = SysMngConfig.Lookups.GetItem<SysMngConfig.RecordStatuses>((long)SysMngConfig.RecordStatuses.Active);
            var school = _repository.GetById<School>(schoolId);
            var model = view.ToModel();
            model.School = school;
            school.SchoolRoutes.Add(model);
        }

        public void DeleteSchoolRoute(long schoolId, SchoolRouteView view) {

            view.RecordStatus = SysMngConfig.Lookups.GetItem<SysMngConfig.RecordStatuses>((long)SysMngConfig.RecordStatuses.Deleted);
            var model = _repository.GetById<SchoolRoute>(view.Id);
            view.SetTo(model);
            _repository.Update(model);
        }

        public void UpdateSchoolRoute(long schoolId, SchoolRouteView view) {
            var existingRoute = (from route in _repository.Query<SchoolRoute>()
                                 where
                                    route.MealType == view.MealType.Id
                                    && route.RecordStatus == (long)SysMngConfig.RecordStatuses.Active
                                    && route.Id != view.Id
                                    && route.School.Id == schoolId
                                 select route).FirstOrDefault();
            if (existingRoute != null)
                throw new ApplicationException("The Meal Type and Route definition already exists.");

            view.RecordStatus = SysMngConfig.Lookups.GetItem<SysMngConfig.RecordStatuses>((long)SysMngConfig.RecordStatuses.Active);

            var model = _repository.GetById<SchoolRoute>(view.Id);
            view.SetTo(model);
            _repository.Update(model);
        }

        public SchoolRouteView GetSchoolRoute(long id)
        {
            var schoolRoute = _repository.GetById<SchoolRoute>(id).ToView();
            return schoolRoute;
        }
    
    }
}
