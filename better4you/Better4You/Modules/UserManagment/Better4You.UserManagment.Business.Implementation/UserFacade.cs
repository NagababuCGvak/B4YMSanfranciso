using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Better4You.Core;
using Better4You.Core.Repositories;
using Better4You.EntityModel;
using Better4You.UserManagment.Business.PasswordValidation;
using Better4You.UserManagment.EntityModel;
using Better4You.UserManagment.ViewModel;
using Tar.Business;
using Tar.Core.Exceptions;

namespace Better4You.UserManagment.Business.Implementation
{
    public class UserFacade : IUserFacade
    {
        private readonly IConfigRepository _repository;
        private readonly IUserNotifications _userNotifications;

        public UserFacade(IConfigRepository repository, IUserNotifications userNotifications)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            if (userNotifications == null) throw new ArgumentNullException("userNotifications");
            _repository = repository;
            _userNotifications = userNotifications;
        }

        public void ValidateUser(string userName, string password)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (password == null) throw new ArgumentNullException("password");
            var user = _repository.Query<User>().FirstOrDefault(u => u.UserName == userName);

            if (user == null ||
                ! PasswordManager.IsValid(new PasswordInfo(user.PasswordFormatType.Id.ToString(CultureInfo.InvariantCulture), password, user.Password)))
                throw new WarningException("Invalid user name or password!");
            
            ValidateUser(user);

            if ((user.IsCompanyUser() || user.IsSchoolUser()) && user.Applications.Count == 0)
                throw new WarningException("Require application! Please, contact with system administrator.");

            if (user.IsSchoolUser() && user.Schools.Count == 0)
                throw new WarningException("Require school! Please, contact with system administrator.");
        }

        private void ValidateUser(User user)
        {
            if (user.UserLoginInfo.IsLocked) throw new WarningException("User is locked! Please contact with system administrator.");

            if (!user.UserLoginInfo.IsApprove) throw new WarningException("User is not approve! Please contact with system administrator.");
        }

        public UserViewModel GetUser(long userId)
        {
            var user = _repository.Query<User>().FirstOrDefault(x => x.Id == userId);
            if (user == null)
                throw new Exception("User not found! UserId:" + userId);
            return user.ToViewModel();
        }

        public UserViewModel GetUser(string userName)
        {
            var user = _repository.Query<User>().FirstOrDefault(u => u.UserName == userName);
            return user.ToViewModel();
        }

        public void SaveUser(UserViewModel user)
        {
            var isNew = user.UserId == 0;

            var entity = isNew ? new User() : _repository.GetById<User>(user.UserId);

            if (isNew && _repository.Query<User>().Any(u => u.UserName == user.UserName))
                throw new Exception("This email address already exists in our system.");

            if (entity.AlgorithmType == null) entity.AlgorithmType = new AlgorithmType {Id = 105001};
            if (entity.PasswordFormatType == null) entity.PasswordFormatType = new PasswordFormatType {Id = 103001};
            entity.UserType = new UserType { Id = user.UserTypeId };

            if (entity.Password == null) entity.Password = "";
            if (entity.PasswordSalt == null) entity.PasswordSalt = "";

            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;
            entity.UserName = user.UserName;

            
            if(entity.UserLoginInfo==null)
                entity.SetUserLoginInfo(new UserLoginInfo());

            var loginInfo = entity.UserLoginInfo;
            loginInfo.IsApprove = true;
            loginInfo.IsLocked = false;

            if (isNew)
            {
                _repository.Create(entity);
                user.UserId = entity.Id;
            }
            else _repository.Update(entity);

            var email = entity.Communications.FirstOrDefault(
                c => c.IsPrimary && c.CommunicationType.Id == (long)CommunicationTypes.Email) ??
                        new Communication
                            {
                                CommunicationType = new CommunicationType { Id = (long)CommunicationTypes.Email },
                                Description = "Email",
                                Phone = "",
                                IsPrimary = true,
                                RecordStatus = new RecordStatus { Id = (long)RecordStatuses.Active }
                            };
            email.Email = user.UserName;
            if (email.Id == 0)
            {
                email.Users.Add(entity);
                _repository.Create(email);
            }
            else
                _repository.Update(email);

            var phone = entity.Communications.FirstOrDefault(
                c => c.IsPrimary && c.CommunicationType.Id == (long)CommunicationTypes.WorkPhone) ??
                        new Communication
                            {
                                CommunicationType = new CommunicationType { Id = (long)CommunicationTypes.WorkPhone },
                                Description = "WorkPhone",
                                Email = "",
                                IsPrimary = true,
                                RecordStatus = new RecordStatus { Id = (long)RecordStatuses.Active }
                            };
            phone.Phone = user.Phone;
            if (phone.Id == 0)
            {
                phone.Users.Add(entity);
                _repository.Create(phone);
            }
            else
                _repository.Update(phone);
        }

        public void AddUserToApplication(long applicationId, long userId)
        {
            var application = _repository.GetById<Application>(applicationId);
            var user = _repository.GetById<User>(userId);
            user.Applications.Add(application);
            _repository.Update(application);
        }

        public IList<GeneralItemView> GetUserApplications(long userId)
        {
            var user = _repository.Query<User>().FirstOrDefault(u => u.Id == userId);
            if (user == null) throw new Exception(string.Format("User not found! UserId:{0}", userId));
            return user.Applications.Select(
                app => new GeneralItemView(app.Id.ToString(CultureInfo.InvariantCulture), app.Name))
                .ToList();
        }

        public List<Tar.ViewModel.GeneralItemView> GetUserSchools(long userId)
        {
            var schools = _repository.Query<School>().Where(s => s.Users.Any(u => u.Id == userId)).ToList();

            return schools.Select(d=>d.ToView<Tar.ViewModel.GeneralItemView>()).ToList();
        }

        public IList<UserViewModel> GetActiveSchoolUsers(long schoolId)
        {
            School school = _repository.Query<School>().Single(c => c.Id == schoolId);
            IList<User> users = school.Users;
            var activeUsers = users.Select(c => c.UserLoginInfo).Where(c => c.IsLocked == false).Select(c => c.User);
            return activeUsers.Select(u => u.ToViewModel()).ToList();
        }

        public string GenerateActivationCode(long userId)
        {
            var user = _repository.GetById<User>(userId);
            var activationCode = Guid.NewGuid();
            user.UserLoginInfo.ActivationCode = activationCode.ToString();
            return user.UserLoginInfo.ActivationCode;
        }

        public IList<UserViewModel> GetApplicationUsers(long applicationId)
        {
            return _repository.GetById<Application>(applicationId).Users.ToList().Select(u => u.ToViewModel()).ToList();
        }

        public IList<UserViewModel> GetApplicationUsersByFilter(long applicationId, string firstName, string lastName, string userName)
        {
            var query = _repository.GetById<Application>(applicationId).Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(c => c.FirstName.ToLower().StartsWith(firstName.ToLower()));
            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(c => c.LastName.ToLower().StartsWith(lastName.ToLower()));
            if (!String.IsNullOrWhiteSpace(userName))
                query = query.Where(c => c.UserName.ToLower().StartsWith(userName.ToLower()));

            return query.ToList().Select(c => c.ToViewModel()).ToList();
        }

        public IList<UserViewModel> GetApplicationUsersByName(long applicationId, string firstName)
        {
            return
                _repository.Query<User>()
                    .Where(c => c.FirstName.StartsWith(firstName))
                    .ToList()
                    .Select(u => u.ToViewModel())
                    .ToList();
        }

        public void ResetPassword(long userId)
        {
            var user = _repository.GetById<User>(userId);
            if(user==null) throw new Exception("User not found! User Id:" + userId);

            ValidateUser(user);

            var actionCode = Guid.NewGuid().ToString();
            var expireActivationDate = DateTime.Now.AddDays(1);
            user.UserLoginInfo.ActivationCode = actionCode;
            user.UserLoginInfo.ExpireActivationDate = expireActivationDate;
            _repository.Update(user);
            _userNotifications.ResetPassword(userId, actionCode, expireActivationDate);
        }

        public void ChangePassword(string userName, string activationCode, string newPassword)
        {
            var user = _repository.Query<User>().FirstOrDefault(u => u.UserName == userName);
            if (user == null) throw new Exception("User not found!");

            if (string.IsNullOrEmpty(activationCode) || !activationCode.Equals(user.UserLoginInfo.ActivationCode,
                StringComparison.InvariantCultureIgnoreCase)) throw new Exception("Invalid activation code!");

            if (user.UserLoginInfo.ExpireActivationDate.HasValue
                && user.UserLoginInfo.ExpireActivationDate.Value < DateTime.Now)
                throw new Exception("Expired activation code!");

            ValidateUser(user);

            user.UserLoginInfo.ActivationCode = null;
            user.UserLoginInfo.ExpireActivationDate = null;
            user.Password = PasswordManager.Encrypt(user.PasswordFormatType.Id.ToString(CultureInfo.InvariantCulture), newPassword);
            _repository.Update(user);
        }

        public void ForgotPassword(string userName)
        {
            var user = _repository.Query<User>().FirstOrDefault(u => u.UserName == userName);
            if (user == null) throw new Exception("User not found! User Name:" + userName);
            ResetPassword(user.Id);
        }

        public void AddRoleToUser(long roleId, long userId)
        {
            var user = _repository.GetById<User>(userId);
            if (user == null) throw new Exception(string.Format("User not found! UserId:{0}", userId));

            var role = _repository.GetById<Role>(roleId);
            if (role == null) throw new Exception(string.Format("Role not found! RoleId:{0}", roleId));

            if (user.Roles.Any(r => r.Id == roleId))
                throw new Exception(string.Format("Has same role! UserId:{0},RoleId:{1}", userId, roleId));

            user.Roles.Add(role);
            _repository.Update(user);
        }

        public IList<GeneralItemView> GetAllRoles()
        {
            return _repository.Query<Role>().ToList()
                .Select(r => new GeneralItemView(r.Id.ToString(CultureInfo.InvariantCulture), r.Name)).ToList();
        }

        public void LockUser(long userId)
        {
            var user = _repository.GetById<User>(userId);
            if (user == null) throw new Exception(string.Format("User not found! UserId:{0}", userId));

            user.UserLoginInfo.IsLocked = true;
            _repository.Update(user);
        }

        public void UnLockUser(long userId)
        {
            var user = _repository.GetById<User>(userId);
            if (user == null) throw new Exception(string.Format("User not found! UserId:{0}", userId));

            user.UserLoginInfo.IsLocked = false;
            _repository.Update(user);
        }

        public IList <Tar.ViewModel.GeneralItemView> GetUserRoles(long userId)
        {
            var user = _repository.GetById<User>(userId);
            if (user == null) throw new Exception(string.Format("User not found! UserId:{0}", userId));

            return user.Roles.Select(
                r => new Tar.ViewModel.GeneralItemView(r.Id, r.Code, r.Name)).ToList();
        }

    }
}
