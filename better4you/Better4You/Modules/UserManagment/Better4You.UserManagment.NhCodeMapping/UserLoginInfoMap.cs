using Better4You.UserManagment.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.UserManagment.NhCodeMapping
{
    public class UserLoginInfoMap:ClassMapping<UserLoginInfo>
    {
        public UserLoginInfoMap()
        {
            Table("UM_USERLOGININFO");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Generator(Generators.Native);
                   map.Column("USERLOGININFOID");
               });

            Property(x => x.IsApprove,
                     map => map.Column(c =>
                                           {
                                               c.Name("ISAPPROVE");
                                               c.SqlType("bit");
                                               c.NotNullable(true);
                                           }
                                )
                );
            Property(x => x.IsLocked,
                     map => map.Column(c =>
                                           {
                                               c.Name("ISLOCKED");
                                               c.SqlType("bit");
                                               c.NotNullable(true);
                                           }
                                )
                );
            Property(x => x.IsOnline,
                     map => map.Column(c =>
                                           {
                                               c.Name("ISONLINE");
                                               c.SqlType("bit");
                                               c.NotNullable(true);
                                           }
                                )
                );

            Property(x => x.LastLoginDate,
                     map => map.Column(c =>
                                           {
                                               c.Name("LASTLOGINDATE");
                                               c.SqlType("datetime");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.PasswordAttemptCount,
                     map => map.Column(c =>
                                           {
                                               c.Name("PASSWORDATTEMPTCOUNT");
                                               c.SqlType("int");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.LastPasswordAttemptDate,
                     map => map.Column(c =>
                                           {
                                               c.Name("LASTPASSWORDATTEMPTDATE");
                                               c.SqlType("datetime");
                                               c.NotNullable(false);
                                           }
                                )
                );
            Property(x => x.ActivationCode,
                     map => map.Column(c =>
                                           {
                                               c.Name("ACTIVATIONCODE");
                                               c.SqlType("nvarchar(128)");
                                               c.NotNullable(false);
                                           }
                                )
                );
            Property(x => x.ExpireActivationDate,
                     map => map.Column(c =>
                                           {
                                               c.Name("EXPIREACTIVATIONDATE");
                                               c.SqlType("datetime");
                                               c.NotNullable(false);
                                           }
                                )
                );
            Property(x => x.Description,
                     map => map.Column(c =>
                                           {
                                               c.Name("DESCRIPTION");
                                               c.SqlType("nvarchar(512)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            //ManyToOne(p => p.User,
            //          map =>
            //              {
            //                  map.Column("USERID");
            //                  map.Unique(true);
            //              }
            //    );
            ManyToOne(o => o.User,
                      o =>
                          {
                              o.Column("USERID");
                              o.Unique(true);
                              o.ForeignKey("FK_User_UserLogInfo");
                          });
        }
    }
}
