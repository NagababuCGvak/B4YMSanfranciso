/*-- BEGIN Communication Type --*/
insert into GNRL_LOOKUPGROUP (LOOKUPGROUPID,NAME) select 100,'Communication Type'
insert into GNRL_LOOKUPITEM (LOOKUPITEMID,FIELDVALUE,FIELDTEXT,DESCRIPTION,ITEMORDER,ISACTIVE,LOOKUPGROUPID) select 100001,'','Home Phone','Home Phone',1,1,100
insert into GNRL_LOOKUPITEM (LOOKUPITEMID,FIELDVALUE,FIELDTEXT,DESCRIPTION,ITEMORDER,ISACTIVE,LOOKUPGROUPID) select 100002,'','Work Phone','Work Phone',2,1,100
insert into GNRL_LOOKUPITEM (LOOKUPITEMID,FIELDVALUE,FIELDTEXT,DESCRIPTION,ITEMORDER,ISACTIVE,LOOKUPGROUPID) select 100003,'','Cell','Cell',3,1,100
insert into GNRL_LOOKUPITEM (LOOKUPITEMID,FIELDVALUE,FIELDTEXT,DESCRIPTION,ITEMORDER,ISACTIVE,LOOKUPGROUPID) select 100004,'','Fax','Fax',4,1,100
insert into GNRL_LOOKUPITEM (LOOKUPITEMID,FIELDVALUE,FIELDTEXT,DESCRIPTION,ITEMORDER,ISACTIVE,LOOKUPGROUPID) select 100005,'','Email','Email',5,1,100
/*-- END Communication Type --*/