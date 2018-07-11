go
/*-- BEGIN Roles --*/
set identity_insert UM_ROLE on
insert into UM_ROLE (ROLEID,NAME,CODE) select 1,'Administrator','administrator'
insert into UM_ROLE (ROLEID,NAME,CODE) select 2,'Driver','driver'
insert into UM_ROLE (ROLEID,NAME,CODE) select 3,'Shipping','shipping'
insert into UM_ROLE (ROLEID,NAME,CODE) select 4,'School - Billing','schl_billing'
insert into UM_ROLE (ROLEID,NAME,CODE) select 5,'School - Meal Ordering','schl_mealordering'
set identity_insert UM_ROLE off
/*-- END Roles --*/

go