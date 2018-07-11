using System;
using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class ChangePasswordRequest : Request
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string ActivationCode { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string ConfirmPassword { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(UserName)) throw new Exception("User Name cannot be empty!");
            if (string.IsNullOrEmpty(ActivationCode)) throw new Exception("Activation Code cannot be empty!");
            if (string.IsNullOrEmpty(Password)) throw new Exception("Password cannot be empty!");
            if (string.IsNullOrEmpty(ConfirmPassword)) throw new Exception("Confirm Password cannot be empty!");
            if (Password != ConfirmPassword) throw new Exception("Password and password confirm must equal!");

            base.Validate();
        }
    }
}