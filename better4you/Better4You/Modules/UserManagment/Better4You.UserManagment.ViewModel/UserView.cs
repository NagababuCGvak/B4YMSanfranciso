using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class UserView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public KeyValuePair<long, string> RecordStatus { get; set; }

        [DataMember]
        public KeyValuePair<long, string> UserType { get; set; }

        [DataMember]
        public KeyValuePair<long, string> PasswordFormatType { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string PasswordSalt { get; set; }

        [DataMember]
        public KeyValuePair<long, string> AlgorithmType { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        //public IList<Application> Applications { get; set; }

        [DataMember]
        public UserLoginInfoView UserLoginInfo { get; set; }
        
        [DataMember]
        public IList<RoleView> Roles { get; set; }
        
        /*
        [DataMember]
        public IList<SchoolView> Schools { get; set; }
        */

        [DataMember]
        public IList<CommunicationView> Communications { get; set; }

        public UserView()
        {

            Roles = new List<RoleView>();
            //Schools = new List<SchoolView>();
            Communications = new List<CommunicationView>();
            //Applications = new List<Application>();

        }

    }
}
