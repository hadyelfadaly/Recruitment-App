if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('APPLICATION') and o.name = 'FK_APPLICAT_APPLICATI_JOBSEEKE')
alter table APPLICATION
   drop constraint FK_APPLICAT_APPLICATI_JOBSEEKE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('APPLICATION') and o.name = 'FK_APPLICAT_APPLICATI_VACANCY')
alter table APPLICATION
   drop constraint FK_APPLICAT_APPLICATI_VACANCY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BELONGSTOINDUSTRY') and o.name = 'FK_BELONGST_BELONGSTO_INDUSTRY')
alter table BELONGSTOINDUSTRY
   drop constraint FK_BELONGST_BELONGSTO_INDUSTRY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BELONGSTOINDUSTRY') and o.name = 'FK_BELONGST_BELONGSTO_VACANCY')
alter table BELONGSTOINDUSTRY
   drop constraint FK_BELONGST_BELONGSTO_VACANCY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('EMPLOYER') and o.name = 'FK_EMPLOYER_USER_IS_E_USER')
alter table EMPLOYER
   drop constraint FK_EMPLOYER_USER_IS_E_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('JOBSEEKER') and o.name = 'FK_JOBSEEKE_USER_IS_J_USER')
alter table JOBSEEKER
   drop constraint FK_JOBSEEKE_USER_IS_J_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SAVEDJOBS') and o.name = 'FK_SAVEDJOB_SAVEDJOBS_VACANCY')
alter table SAVEDJOBS
   drop constraint FK_SAVEDJOB_SAVEDJOBS_VACANCY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SAVEDJOBS') and o.name = 'FK_SAVEDJOB_SAVEDJOBS_JOBSEEKE')
alter table SAVEDJOBS
   drop constraint FK_SAVEDJOB_SAVEDJOBS_JOBSEEKE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('USERSKILL') and o.name = 'FK_USERSKIL_USERSKILL_JOBSEEKE')
alter table USERSKILL
   drop constraint FK_USERSKIL_USERSKILL_JOBSEEKE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('USERSKILL') and o.name = 'FK_USERSKIL_USERSKILL_SKILL')
alter table USERSKILL
   drop constraint FK_USERSKIL_USERSKILL_SKILL
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('VACANCY') and o.name = 'FK_VACANCY_LOCATEDIN_LOCATION')
alter table VACANCY
   drop constraint FK_VACANCY_LOCATEDIN_LOCATION
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('VACANCYSKILL') and o.name = 'FK_VACANCYS_VACANCYSK_VACANCY')
alter table VACANCYSKILL
   drop constraint FK_VACANCYS_VACANCYSK_VACANCY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('VACANCYSKILL') and o.name = 'FK_VACANCYS_VACANCYSK_SKILL')
alter table VACANCYSKILL
   drop constraint FK_VACANCYS_VACANCYSK_SKILL
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('APPLICATION')
            and   name  = 'APPLICATION2_FK'
            and   indid > 0
            and   indid < 255)
   drop index APPLICATION.APPLICATION2_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('APPLICATION')
            and   name  = 'APPLICATION_FK'
            and   indid > 0
            and   indid < 255)
   drop index APPLICATION.APPLICATION_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('APPLICATION')
            and   type = 'U')
   drop table APPLICATION
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BELONGSTOINDUSTRY')
            and   name  = 'BELONGSTOINDUSTRY2_FK'
            and   indid > 0
            and   indid < 255)
   drop index BELONGSTOINDUSTRY.BELONGSTOINDUSTRY2_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BELONGSTOINDUSTRY')
            and   name  = 'BELONGSTOINDUSTRY_FK'
            and   indid > 0
            and   indid < 255)
   drop index BELONGSTOINDUSTRY.BELONGSTOINDUSTRY_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BELONGSTOINDUSTRY')
            and   type = 'U')
   drop table BELONGSTOINDUSTRY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('EMPLOYER')
            and   type = 'U')
   drop table EMPLOYER
go

if exists (select 1
            from  sysobjects
           where  id = object_id('INDUSTRY')
            and   type = 'U')
   drop table INDUSTRY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('JOBSEEKER')
            and   type = 'U')
   drop table JOBSEEKER
go

if exists (select 1
            from  sysobjects
           where  id = object_id('LOCATION')
            and   type = 'U')
   drop table LOCATION
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('SAVEDJOBS')
            and   name  = 'SAVEDJOBS2_FK'
            and   indid > 0
            and   indid < 255)
   drop index SAVEDJOBS.SAVEDJOBS2_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('SAVEDJOBS')
            and   name  = 'SAVEDJOBS_FK'
            and   indid > 0
            and   indid < 255)
   drop index SAVEDJOBS.SAVEDJOBS_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SAVEDJOBS')
            and   type = 'U')
   drop table SAVEDJOBS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SKILL')
            and   type = 'U')
   drop table SKILL
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"USER"')
            and   type = 'U')
   drop table "USER"
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('USERSKILL')
            and   name  = 'USERSKILL2_FK'
            and   indid > 0
            and   indid < 255)
   drop index USERSKILL.USERSKILL2_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('USERSKILL')
            and   name  = 'USERSKILL_FK'
            and   indid > 0
            and   indid < 255)
   drop index USERSKILL.USERSKILL_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('USERSKILL')
            and   type = 'U')
   drop table USERSKILL
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('VACANCY')
            and   name  = 'LOCATEDIN_FK'
            and   indid > 0
            and   indid < 255)
   drop index VACANCY.LOCATEDIN_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('VACANCY')
            and   type = 'U')
   drop table VACANCY
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('VACANCYSKILL')
            and   name  = 'VACANCYSKILL2_FK'
            and   indid > 0
            and   indid < 255)
   drop index VACANCYSKILL.VACANCYSKILL2_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('VACANCYSKILL')
            and   name  = 'VACANCYSKILL_FK'
            and   indid > 0
            and   indid < 255)
   drop index VACANCYSKILL.VACANCYSKILL_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('VACANCYSKILL')
            and   type = 'U')
   drop table VACANCYSKILL
go

/*==============================================================*/
/* Table: APPLICATION                                           */
/*==============================================================*/
create table APPLICATION (
   USERID               int                  not null,
   VACANCYID            int                  not null,
   DATEAPPLIED          datetime     DEFAULT GETDATE(),
   STATUS               varchar(1024)        null,
   constraint PK_APPLICATION primary key (USERID, VACANCYID)
)
go

/*==============================================================*/
/* Index: APPLICATION_FK                                        */
/*==============================================================*/
create index APPLICATION_FK on APPLICATION (
USERID ASC
)
go

/*==============================================================*/
/* Index: APPLICATION2_FK                                       */
/*==============================================================*/
create index APPLICATION2_FK on APPLICATION (
VACANCYID ASC
)
go


/*==============================================================*/
/* Table: EMPLOYER                                              */
/*==============================================================*/
create table EMPLOYER (
   USERID               int                  not null,
   NAME                 varchar(1024)        null,
   EMAIL                varchar(1024)        null,
   PASSWORD             varchar(1024)        null,
   USERTYPE             varchar(1024)        null,
   PHONENUMBER          int                  null,
   REGISTRATIONDATE     datetime             DEFAULT GETDATE(),
   COMPANYNAME          varchar(1024)        null,
   constraint PK_EMPLOYER primary key (USERID)
)
go

/*==============================================================*/
/* Table: INDUSTRY                                              */
/*==============================================================*/
create table INDUSTRY (
   INDUSTRYID           int IDENTITY(1,1)               not null,
   NAME                 varchar(1024)        null,
   constraint PK_INDUSTRY primary key nonclustered (INDUSTRYID)
)
go

/*==============================================================*/
/* Table: JOBSEEKER                                             */
/*==============================================================*/
create table JOBSEEKER (
   USERID               int                  not null,
   NAME                 varchar(1024)        null,
   EMAIL                varchar(1024)        null,
   PASSWORD             varchar(1024)        null,
   USERTYPE             varchar(1024)        null,
   PHONENUMBER          int                  null,
   REGISTRATIONDATE     datetime             DEFAULT GETDATE(),
   EXPRIENCE            int                  null,
   constraint PK_JOBSEEKER primary key (USERID)
)
go

/*==============================================================*/
/* Table: LOCATION                                              */
/*==============================================================*/
create table LOCATION (
   LOCATIONID           int IDENTITY(1,1)     not null,
   CITY                 varchar(1024)        null,
   COUNTRY              varchar(1024)        null,
   constraint PK_LOCATION primary key nonclustered (LOCATIONID)
)
go

/*==============================================================*/
/* Table: SAVEDJOBS                                             */
/*==============================================================*/
create table SAVEDJOBS (
   VACANCYID            int                  not null,
   USERID               int                  not null,
   constraint PK_SAVEDJOBS primary key (VACANCYID, USERID)
)
go

/*==============================================================*/
/* Index: SAVEDJOBS_FK                                          */
/*==============================================================*/
create index SAVEDJOBS_FK on SAVEDJOBS (
VACANCYID ASC
)
go

/*==============================================================*/
/* Index: SAVEDJOBS2_FK                                         */
/*==============================================================*/
create index SAVEDJOBS2_FK on SAVEDJOBS (
USERID ASC
)
go

/*==============================================================*/
/* Table: SKILL                                                 */
/*==============================================================*/
create table SKILL (
   SKILLID              int IDENTITY(1,1)    not null,
   NAME                 varchar(1024)        null,
   constraint PK_SKILL primary key nonclustered (SKILLID)
)
go

/*==============================================================*/
/* Table: "USER"                                                */
/*==============================================================*/
create table "USER" (
   USERID               int IDENTITY(1,1)    not null,
   NAME                 varchar(1024)        null,
   EMAIL                varchar(1024)        null,
   PASSWORD             varchar(1024)        null,
   USERTYPE             varchar(1024)        null,
   PHONENUMBER          int                  null,
   REGISTRATIONDATE     datetime             DEFAULT GETDATE(),
   constraint PK_USER primary key nonclustered (USERID)
)
go

/*==============================================================*/
/* Table: USERSKILL                                             */
/*==============================================================*/
create table USERSKILL (
   USERID               int                  not null,
   SKILLID              int                  not null,
   EXPERIENCEYEARS      int                  null,
   constraint PK_USERSKILL primary key (USERID, SKILLID)
)
go

/*==============================================================*/
/* Index: USERSKILL_FK                                          */
/*==============================================================*/
create index USERSKILL_FK on USERSKILL (
USERID ASC
)
go

/*==============================================================*/
/* Index: USERSKILL2_FK                                         */
/*==============================================================*/
create index USERSKILL2_FK on USERSKILL (
SKILLID ASC
)
go

/*==============================================================*/
/* Table: VACANCY                                               */
/*==============================================================*/
create table VACANCY (
   VACANCYID            int IDENTITY(1,1)                 not null,
   LOCATIONID           int                  not null,
   INDUSTRYID           int                  null,
   TITLE                varchar(1024)        null,
   DESCRIPTION          varchar(1024)        null,
   REQUIREDEXPERIENCE   int                  null,
   EMPLOYERID           int                  null,
   SALARYRANGE          varchar(1024)        null,
   POSTINGDATE          datetime             DEFAULT GETDATE(),
   EXPIRATIONDATE       datetime             null,
   COMPANYNAME          varchar(1024)        null,
   constraint PK_VACANCY primary key nonclustered (VACANCYID)
)
go

/*==============================================================*/
/* Index: LOCATEDIN_FK                                          */
/*==============================================================*/
create index LOCATEDIN_FK on VACANCY (
LOCATIONID ASC
)
go

/*==============================================================*/
/* Table: VACANCYSKILL                                          */
/*==============================================================*/
create table VACANCYSKILL (
   VACANCYID            int                  not null,
   SKILLID              int                  not null,
   constraint PK_VACANCYSKILL primary key (VACANCYID, SKILLID)
)
go

/*==============================================================*/
/* Index: VACANCYSKILL_FK                                       */
/*==============================================================*/
create index VACANCYSKILL_FK on VACANCYSKILL (
VACANCYID ASC
)
go

/*==============================================================*/
/* Index: VACANCYSKILL2_FK                                      */
/*==============================================================*/
create index VACANCYSKILL2_FK on VACANCYSKILL (
SKILLID ASC
)
go

alter table APPLICATION
   add constraint FK_APPLICAT_APPLICATI_JOBSEEKE foreign key (USERID)
      references JOBSEEKER (USERID)
go

alter table APPLICATION
   add constraint FK_APPLICAT_APPLICATI_VACANCY foreign key (VACANCYID)
      references VACANCY (VACANCYID)
go

alter table EMPLOYER
   add constraint FK_EMPLOYER_USER_IS_E_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table JOBSEEKER
   add constraint FK_JOBSEEKE_USER_IS_J_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table SAVEDJOBS
   add constraint FK_SAVEDJOB_SAVEDJOBS_VACANCY foreign key (VACANCYID)
      references VACANCY (VACANCYID)
go

alter table SAVEDJOBS
   add constraint FK_SAVEDJOB_SAVEDJOBS_JOBSEEKE foreign key (USERID)
      references JOBSEEKER (USERID)
go

alter table USERSKILL
   add constraint FK_USERSKIL_USERSKILL_JOBSEEKE foreign key (USERID)
      references JOBSEEKER (USERID)
go

alter table USERSKILL
   add constraint FK_USERSKIL_USERSKILL_SKILL foreign key (SKILLID)
      references SKILL (SKILLID)
go

alter table VACANCY
   add constraint FK_VACANCY_LOCATEDIN_LOCATION foreign key (LOCATIONID)
      references LOCATION (LOCATIONID)
go

alter table VACANCY
   add constraint FK_VACANCY_ISIN_INDUSTRY foreign key (INDUSTRYID)
      references INDUSTRY (INDUSTRYID)
go

alter table VACANCY
   add constraint FK_VACANCY_POSTEDBY_EMPLOYER foreign key (EMPLOYERID)
      references EMPLOYER (USERID)
go

alter table VACANCYSKILL
   add constraint FK_VACANCYS_VACANCYSK_VACANCY foreign key (VACANCYID)
      references VACANCY (VACANCYID)
go

alter table VACANCYSKILL
   add constraint FK_VACANCYS_VACANCYSK_SKILL foreign key (SKILLID)
      references SKILL (SKILLID)
go

