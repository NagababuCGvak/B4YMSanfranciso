using System;
using System.Runtime.Serialization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tar.Security;

namespace Tar.Security.Tests
{
    [TestClass]
    public class AuthenticationTestCase
    {
        [TestMethod]
        public void SignIn()
        {
            const string userName = "zahir";
            const string password = "123456";
            var currentUser = new CurrentUser
                                  {
                                      Name = userName,
                                      FullName = "M. Zahir Solak",
                                      IsAuthenticated = true
                                  };

            IAuthentication authentication =
                new Authentication(new AuthenticationProvider(userName, password, currentUser));

            var currentUser2 = authentication.SignIn(userName, password);
            Assert.IsTrue(currentUser.IsAuthenticated);
            Assert.AreEqual(currentUser, currentUser2);

            Assert.AreEqual(true, Thread.CurrentPrincipal.Identity.IsAuthenticated);
            Assert.AreEqual(currentUser, Thread.CurrentPrincipal);

            var asCurrentUser = Thread.CurrentPrincipal.AsCurrentUser();
            Assert.IsNotNull(asCurrentUser);
            Assert.IsTrue(asCurrentUser.IsAuthenticated);
        }

        class AuthenticationProvider : IAuthenticationProvider
        {
            private readonly string _userName;
            private readonly string _password;
            private readonly CurrentUser _currentUser;

            public AuthenticationProvider(string userName,string password,CurrentUser currentUser)
            {
                if (currentUser == null) throw new ArgumentNullException("currentUser");
                _userName = userName;
                _password = password;
                _currentUser = currentUser;
            }

            public CurrentUser GetUser(string name, string password)
            {
                if (name==_userName&& password==_password) return _currentUser;
                return new CurrentUser {IsAuthenticated = false};
            }

            public CurrentUser GetUser(string name)
            {
                if (name == _userName) return _currentUser;
                return new CurrentUser { IsAuthenticated = false };
            }
        }

        [Serializable]
        public class NotFoundUserException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public NotFoundUserException()
                :base("Not found user.")
            {
            }

            public NotFoundUserException(string userName)
                : base(string.Format("Not found user. UserName:{0}",userName))
            {
            }

            public NotFoundUserException(string message, Exception inner) : base(message, inner)
            {
            }

            protected NotFoundUserException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
