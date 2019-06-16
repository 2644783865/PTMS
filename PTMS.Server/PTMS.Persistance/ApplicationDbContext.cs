using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PTMS.Domain.Entities;
using PTMS.Persistance.EntityConfigurations;

namespace PTMS.Persistance
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public virtual DbSet<ArmUsers> ArmUsers { get; set; }
        public virtual DbSet<BlockTypes> BlockTypes { get; set; }
        public virtual DbSet<Bs> Bs { get; set; }
        public virtual DbSet<BsRoute> BsRoute { get; set; }
        public virtual DbSet<CarBrand> CarBrand { get; set; }
        public virtual DbSet<CarType> CarType { get; set; }
        public virtual DbSet<Changed> Changed { get; set; }
        public virtual DbSet<Days> Days { get; set; }
        public virtual DbSet<DefaultPlans> DefaultPlans { get; set; }
        public virtual DbSet<Granits> Granits { get; set; }
        public virtual DbSet<IbeTodo> IbeTodo { get; set; }
        public virtual DbSet<Objects> Objects { get; set; }
        public virtual DbSet<Plans> Plans { get; set; }
        public virtual DbSet<ProjContracts> ProjContracts { get; set; }
        public virtual DbSet<ProjectRoute> ProjRouts { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<Reports> Reports { get; set; }
        public virtual DbSet<ReportsControl> ReportsControl { get; set; }
        public virtual DbSet<Routs> Routs { get; set; }
        public virtual DbSet<Sim> Sim { get; set; }
        public virtual DbSet<WebMapUser> WebMapUsers { get; set; }
        public virtual DbSet<UserProject> UsersProjs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Remove Pluralizing Table Name Convention
            foreach (IMutableEntityType entity in builder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }

            builder.ConfigureAppUsers();

            builder.Entity<ArmUsers>(entity =>
            {
                entity.HasKey(e => e.AuId)
                    .HasName("IDX_PK_ARM_USERS_ID_");

                entity.ToTable("ARM_USERS                      ");

                entity.HasIndex(e => e.AuId)
                    .HasName("IDX_PK_ARM_USERS_ID_");

                entity.HasIndex(e => e.AuLogin)
                    .HasName("UNQ1_ARM_USERS");

                entity.Property(e => e.AuId).HasColumnName("AU_ID_");

                entity.Property(e => e.AuAccess)
                    .HasColumnName("AU_ACCESS")
                    .HasMaxLength(20);

                entity.Property(e => e.AuDiscription)
                    .HasColumnName("AU_DISCRIPTION")
                    .HasMaxLength(50);

                entity.Property(e => e.AuLogin)
                    .IsRequired()
                    .HasColumnName("AU_LOGIN")
                    .HasMaxLength(100);
            });

            builder.Entity<BlockTypes>(entity =>
            {
                entity.HasKey(e => e.BtId)
                    .HasName("PK_BLOCK_TYPES_1");

                entity.ToTable("BLOCK_TYPES                    ");

                entity.HasIndex(e => e.BtId)
                    .HasName("PK_BLOCK_TYPES_1");

                entity.HasIndex(e => e.BtName)
                    .HasName("UNQ1_BLOCK_TYPES");

                entity.Property(e => e.BtId).HasColumnName("BT_ID_");

                entity.Property(e => e.BtName)
                    .IsRequired()
                    .HasColumnName("BT_NAME_")
                    .HasMaxLength(50)
                    .HasAnnotation("Description", "Тип блока");
            });

            builder.Entity<Bs>(entity =>
            {
                entity.ToTable("BS                             ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_BS_1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Azmth).HasColumnName("AZMTH");

                entity.Property(e => e.Lat).HasColumnName("LAT");

                entity.Property(e => e.Lon).HasColumnName("LON");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(110);
            });

            builder.Entity<BsRoute>(entity =>
            {
                entity.ToTable("BS_ROUTE                       ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_BS_ROUTE_1");

                entity.HasIndex(e => new { e.Num, e.RouteId })
                    .HasName("BS_ROUTE_IDX")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BsId).HasColumnName("BS_ID");

                entity.Property(e => e.Num).HasColumnName("NUM");

                entity.Property(e => e.RouteId).HasColumnName("ROUTE_ID");
            });

            builder.ConfigureCarBrands();

            builder.Entity<CarType>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_CAR_TYPE_");

                entity.ToTable("CAR_TYPE_                      ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_CAR_TYPE_");

                entity.HasIndex(e => e.Name)
                    .HasName("UNQ1_CAR_TYPE_");

                entity.Property(e => e.Id).HasColumnName("CT_ID_");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME_")
                    .HasMaxLength(50)
                    .HasAnnotation("Description", "Тип автобуса");

                entity.Property(e => e.ShortName)
                    .HasColumnName("SHORT_NAME_")
                    .HasMaxLength(20)
                    .HasAnnotation("Description", "Сокращенное название типа автобуса");
            });

            builder.Entity<Changed>(entity =>
            {
                entity.HasKey(e => e.ChgIds)
                    .HasName("PK_CHANGED_");

                entity.ToTable("CHANGED_                       ");

                entity.HasIndex(e => e.ChgIds)
                    .HasName("PK_CHANGED_");

                entity.Property(e => e.ChgIds).HasColumnName("CHG_IDS");

                entity.Property(e => e.FieldName)
                    .IsRequired()
                    .HasColumnName("FIELD_NAME_")
                    .HasMaxLength(100)
                    .HasAnnotation("Description", "Поле");

                entity.Property(e => e.Ids).HasColumnName("IDS_");

                entity.Property(e => e.NewFieldValue)
                    .IsRequired()
                    .HasColumnName("NEW_FIELD_VALUE_")
                    .HasMaxLength(255)
                    .HasAnnotation("Description", "Новое значение поля");

                entity.Property(e => e.OldFieldValue)
                    .IsRequired()
                    .HasColumnName("OLD_FIELD_VALUE_")
                    .HasMaxLength(255)
                    .HasAnnotation("Description", "Старое значение поля");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasColumnName("TABLE_NAME_")
                    .HasMaxLength(100)
                    .HasAnnotation("Description", "Таблица");

                entity.Property(e => e.Time)
                    .HasColumnName("TIME_")
                    .HasAnnotation("Description", "Время изменения");

                entity.Property(e => e.TypeChaged)
                    .HasColumnName("TYPE_CHAGED_")
                    .HasAnnotation("Description", "Тип изменения");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasColumnName("USER_")
                    .HasMaxLength(100)
                    .HasAnnotation("Description", "Пользователь");
            });

            builder.Entity<Days>(entity =>
            {
                entity.HasKey(e => e.DId)
                    .HasName("PK_DAYS__1");

                entity.ToTable("DAYS_                          ");

                entity.HasIndex(e => e.DId)
                    .HasName("PK_DAYS__1");

                entity.Property(e => e.DId).HasColumnName("D_ID_");

                entity.Property(e => e.DayName)
                    .IsRequired()
                    .HasColumnName("DAY_NAME_")
                    .HasMaxLength(20)
                    .HasAnnotation("Description", "День недели");

                entity.Property(e => e.DayShortName)
                    .IsRequired()
                    .HasColumnName("DAY_SHORT_NAME_")
                    .HasMaxLength(5)
                    .HasAnnotation("Description", "Сокращенное название дня недели");

                entity.Property(e => e.DayType)
                    .HasColumnName("DAY_TYPE_")
                    .HasAnnotation("Description", "Признак выходного дня");
            });

            builder.Entity<DefaultPlans>(entity =>
            {
                entity.HasKey(e => e.Ids)
                    .HasName("PK_DEFAULT_PLANS");

                entity.ToTable("DEFAULT_PLANS                  ");

                entity.HasIndex(e => e.Ids)
                    .HasName("PK_DEFAULT_PLANS");

                entity.HasIndex(e => e.Pid)
                    .HasName("FK_DEFAULT_PLANS_1")
                    .IsUnique();

                entity.HasIndex(e => e.Rid)
                    .HasName("FK_DEFAULT_PLANS_2")
                    .IsUnique();

                entity.Property(e => e.Ids).HasColumnName("IDS_");

                entity.Property(e => e.Pid)
                    .HasColumnName("PID_")
                    .HasAnnotation("Description", "Перевозчик");

                entity.Property(e => e.PlannedCount)
                    .HasColumnName("PLANNED_COUNT_")
                    .HasAnnotation("Description", "Плановое количество");

                entity.Property(e => e.PlannedCountHol)
                    .HasColumnName("PLANNED_COUNT_HOL_")
                    .HasAnnotation("Description", "Плановое количество в выходные");

                entity.Property(e => e.Race1Planned)
                    .HasColumnName("RACE1_PLANNED_")
                    .HasDefaultValueSql("DEFAULT 0")
                    .HasAnnotation("Description", "Плановое количество рейсов ТС по типу");

                entity.Property(e => e.Race1PlannedHol)
                    .HasColumnName("RACE1_PLANNED_HOL_")
                    .HasDefaultValueSql("DEFAULT 0")
                    .HasAnnotation("Description", "Плановое количество рейсов ТС по типу");

                entity.Property(e => e.Race2Planned)
                    .HasColumnName("RACE2_PLANNED_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Race2PlannedHol)
                    .HasColumnName("RACE2_PLANNED_HOL_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Race3Planned)
                    .HasColumnName("RACE3_PLANNED_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Race3PlannedHol)
                    .HasColumnName("RACE3_PLANNED_HOL_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Race4Planned)
                    .HasColumnName("RACE4_PLANNED_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Race4PlannedHol)
                    .HasColumnName("RACE4_PLANNED_HOL_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.RaceCount)
                    .HasColumnName("RACE_COUNT_")
                    .HasAnnotation("Description", "Количество рейсов в выходные");

                entity.Property(e => e.RaceCountHol)
                    .HasColumnName("RACE_COUNT_HOL_")
                    .HasAnnotation("Description", "Количество рейсов в выходные");

                entity.Property(e => e.Rid)
                    .HasColumnName("RID_")
                    .HasAnnotation("Description", "Маршрут");

                entity.Property(e => e.Type1Planned)
                    .HasColumnName("TYPE1_PLANNED_")
                    .HasDefaultValueSql("DEFAULT 0")
                    .HasAnnotation("Description", "Плановое количество ТС по типу");

                entity.Property(e => e.Type1PlannedHol)
                    .HasColumnName("TYPE1_PLANNED_HOL_")
                    .HasDefaultValueSql("DEFAULT 0")
                    .HasAnnotation("Description", "Плановое количество ТС по типу в выходные");

                entity.Property(e => e.Type2Planned)
                    .HasColumnName("TYPE2_PLANNED_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Type2PlannedHol)
                    .HasColumnName("TYPE2_PLANNED_HOL_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Type3Planned)
                    .HasColumnName("TYPE3_PLANNED_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Type3PlannedHol)
                    .HasColumnName("TYPE3_PLANNED_HOL_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Type4Planned)
                    .HasColumnName("TYPE4_PLANNED_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Type4PlannedHol)
                    .HasColumnName("TYPE4_PLANNED_HOL_")
                    .HasDefaultValueSql("DEFAULT 0");
            });

            builder.Entity<Granits>(entity =>
            {
                entity.ToTable("GRANITS                        ");

                entity.HasIndex(e => e.BlockNumber)
                    .HasName("UNQ1_BLOCK_NUM");

                entity.HasIndex(e => e.BlockType)
                    .HasName("FK_GRANITS_2")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("PK_GRANITS");

                entity.HasIndex(e => e.Oids)
                    .HasName("GRANITS_IDX1");

                entity.Property(e => e.Id).HasColumnName("ID_");

                entity.Property(e => e.BlockNumber)
                    .HasColumnName("BLOCK_NUMBER")
                    .HasAnnotation("Description", "Номер блока");

                entity.Property(e => e.BlockType)
                    .HasColumnName("BLOCK_TYPE")
                    .HasAnnotation("Description", "Тип блока");

                entity.Property(e => e.Oids)
                    .HasColumnName("OIDS_")
                    .HasAnnotation("Description", "ID автобуса");
            });

            builder.Entity<IbeTodo>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("RDB$PRIMARY1");

                entity.ToTable("IBE$TODO                       ");

                entity.HasIndex(e => e.ItemId)
                    .HasName("RDB$PRIMARY1");

                entity.HasIndex(e => e.ObjectName)
                    .HasName("IBE$TODO_BY_OBJECTNAME")
                    .IsUnique();

                entity.Property(e => e.ItemId).HasColumnName("ITEM_ID");

                entity.Property(e => e.ItemCaption)
                    .IsRequired()
                    .HasColumnName("ITEM_CAPTION")
                    .HasMaxLength(255);

                entity.Property(e => e.ItemCategory)
                    .HasColumnName("ITEM_CATEGORY")
                    .HasMaxLength(64);

                entity.Property(e => e.ItemDeadline)
                    .HasColumnName("ITEM_DEADLINE")
                    .HasColumnType("DATE");

                entity.Property(e => e.ItemDescription)
                    .HasColumnName("ITEM_DESCRIPTION")
                    .HasColumnType("BLOB SUB_TYPE TEXT");

                entity.Property(e => e.ItemOwner)
                    .IsRequired()
                    .HasColumnName("ITEM_OWNER")
                    .HasMaxLength(64);

                entity.Property(e => e.ItemPriority)
                    .HasColumnName("ITEM_PRIORITY")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.ItemState)
                    .HasColumnName("ITEM_STATE")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.ItemTimestamp).HasColumnName("ITEM_TIMESTAMP");

                entity.Property(e => e.ObjectName)
                    .HasColumnName("OBJECT_NAME")
                    .HasMaxLength(64);

                entity.Property(e => e.ObjectType).HasColumnName("OBJECT_TYPE");

                entity.Property(e => e.ResponsiblePerson)
                    .HasColumnName("RESPONSIBLE_PERSON")
                    .HasMaxLength(64);
            });

            builder.ConfigureObjects();

            builder.Entity<Plans>(entity =>
            {
                entity.ToTable("PLANS                          ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_PLANS");

                entity.HasIndex(e => new { e.Date, e.ProjId, e.RoutId })
                    .HasName("PLANS_IDX1")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID_");

                entity.Property(e => e.Date)
                    .HasColumnName("DATE_")
                    .HasAnnotation("Description", "Дата, на которую сформированы планы");

                entity.Property(e => e.DateModify)
                    .HasColumnName("DATE_MODIFY")
                    .HasAnnotation("Description", "Время формирования планов");

                entity.Property(e => e.ProjId)
                    .HasColumnName("PROJ_ID_")
                    .HasAnnotation("Description", "Перевозчик");

                entity.Property(e => e.Race1Planned)
                    .HasColumnName("RACE1_PLANNED_")
                    .HasAnnotation("Description", "Плановое количество ТС по типу в выходные");

                entity.Property(e => e.Race2Planned)
                    .HasColumnName("RACE2_PLANNED_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Race3Planned)
                    .HasColumnName("RACE3_PLANNED_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.Race4Planned)
                    .HasColumnName("RACE4_PLANNED_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.RoutId)
                    .HasColumnName("ROUT_ID_")
                    .HasAnnotation("Description", "Маршрут");

                entity.Property(e => e.Type1Planned)
                    .HasColumnName("TYPE1_PLANNED_")
                    .HasAnnotation("Description", "Плановое количество ТС по типу");

                entity.Property(e => e.Type2Planned).HasColumnName("TYPE2_PLANNED_");

                entity.Property(e => e.Type3Planned).HasColumnName("TYPE3_PLANNED_");

                entity.Property(e => e.Type4Planned).HasColumnName("TYPE4_PLANNED_");
            });

            builder.Entity<ProjContracts>(entity =>
            {
                entity.HasKey(e => e.Ids)
                    .HasName("PK_PROJ_CONTRACTS_1");

                entity.ToTable("PROJ_CONTRACTS                 ");

                entity.HasIndex(e => e.Ids)
                    .HasName("PK_PROJ_CONTRACTS_1");

                entity.HasIndex(e => e.ProjId)
                    .HasName("INDX_FK_PROJ_CONTRACTS_1")
                    .IsUnique();

                entity.Property(e => e.Ids).HasColumnName("IDS_");

                entity.Property(e => e.Atribut)
                    .HasColumnName("ATRIBUT")
                    .HasMaxLength(220)
                    .HasAnnotation("Description", "Юридическая информация");

                entity.Property(e => e.DateStart)
                    .HasColumnName("DATE_START_")
                    .HasColumnType("DATE")
                    .HasAnnotation("Description", "Дата подписания контракта");

                entity.Property(e => e.Number)
                    .HasColumnName("NUMBER_")
                    .HasMaxLength(50)
                    .HasAnnotation("Description", "Номер контракта");

                entity.Property(e => e.ProjId)
                    .HasColumnName("PROJ_ID_")
                    .HasAnnotation("Description", "Перевозчик");
            });

            builder.Entity<ProjectRoute>(entity =>
            {
                entity.HasKey(e => e.Ids)
                    .HasName("PK_PROJ_ROUTS_1");

                entity.ToTable("PROJ_ROUTS                     ");

                entity.HasIndex(e => e.Ids)
                    .HasName("PK_PROJ_ROUTS_1");

                entity.HasIndex(e => e.ProjId)
                    .HasName("FK_PROJ_ROUTS_2")
                    .IsUnique();

                entity.HasIndex(e => e.RoutId)
                    .HasName("FK_PROJ_ROUTS_1")
                    .IsUnique();

                entity.HasIndex(e => new { e.ProjId, e.RoutId })
                    .HasName("UNQ_PROJ_ROUTS_2");

                entity.Property(e => e.Ids).HasColumnName("IDS_");

                entity.Property(e => e.ProjId)
                    .HasColumnName("PROJ_ID_")
                    .HasAnnotation("Description", "Маршрут");

                entity.Property(e => e.RoutId)
                    .HasColumnName("ROUT_ID_")
                    .HasAnnotation("Description", "Перевозчик");
            });

            builder.ConfigureProjects();

            builder.Entity<Provider>(entity =>
            {
                entity.ToTable("PROVIDERS                      ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_PROVIDERS");

                entity.HasIndex(e => e.Name)
                    .HasName("UNQ1_PROVIDERS");

                entity.Property(e => e.Id).HasColumnName("ID_");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME_")
                    .HasMaxLength(50)
                    .HasAnnotation("Description", "Установщик");
            });

            builder.Entity<Reports>(entity =>
            {
                entity.ToTable("REPORTS                        ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_REPORTS");

                entity.Property(e => e.Id).HasColumnName("ID_");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME_")
                    .HasMaxLength(20);

                entity.Property(e => e.Sql)
                    .IsRequired()
                    .HasColumnName("SQL_")
                    .HasMaxLength(300);
            });

            builder.Entity<ReportsControl>(entity =>
            {
                entity.HasKey(e => e.RcId)
                    .HasName("PK_REPORTS_CONTROL");

                entity.ToTable("REPORTS_CONTROL                ");

                entity.HasIndex(e => e.RcId)
                    .HasName("PK_REPORTS_CONTROL");

                entity.Property(e => e.RcId).HasColumnName("RC_ID_");

                entity.Property(e => e.Comments)
                    .HasColumnName("COMMENTS")
                    .HasMaxLength(30)
                    .HasAnnotation("Description", "Комментарии");

                entity.Property(e => e.RepCheck)
                    .HasColumnName("REP_CHECK_")
                    .HasAnnotation("Description", "Статус проверки отчета");

                entity.Property(e => e.RepCheckDate)
                    .HasColumnName("REP_CHECK_DATE_")
                    .HasAnnotation("Description", "Время проверки");

                entity.Property(e => e.RepCheckUser)
                    .HasColumnName("REP_CHECK_USER")
                    .HasDefaultValueSql("DEFAULT 5")
                    .HasAnnotation("Description", "Кто проверил отчет");

                entity.Property(e => e.RepCreateDate)
                    .HasColumnName("REP_CREATE_DATE_")
                    .HasAnnotation("Description", "Время создания");

                entity.Property(e => e.RepCreateUser)
                    .HasColumnName("REP_CREATE_USER")
                    .HasDefaultValueSql("DEFAULT 5")
                    .HasAnnotation("Description", "Кто создал отчет");

                entity.Property(e => e.RepDataFr3)
                    .HasColumnName("REP_DATA_FR3_")
                    .HasAnnotation("Description", "Файл отчета");

                entity.Property(e => e.RepMail)
                    .HasColumnName("REP_MAIL")
                    .HasAnnotation("Description", "Статус отправки отчета");

                entity.Property(e => e.RepMailDate)
                    .HasColumnName("REP_MAIL_DATE_")
                    .HasAnnotation("Description", "Время отправки");

                entity.Property(e => e.RepMailTo)
                    .HasColumnName("REP_MAIL_TO")
                    .HasMaxLength(800)
                    .HasAnnotation("Description", "Список пользователей");

                entity.Property(e => e.RepMailUser)
                    .HasColumnName("REP_MAIL_USER_")
                    .HasDefaultValueSql("DEFAULT 5")
                    .HasAnnotation("Description", "Кто отправил");
            });

            builder.ConfigureRoutes();

            builder.Entity<Sim>(entity =>
            {
                entity.ToTable("SIM                            ");

                entity.HasIndex(e => e.Associated)
                    .HasName("UNQ_SIM_ASSOCIATED");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SIM");

                entity.HasIndex(e => e.Phone)
                    .HasName("UNQ_SIM_PHONE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Associated)
                    .HasColumnName("ASSOCIATED")
                    .HasMaxLength(20);

                entity.Property(e => e.Comment)
                    .HasColumnName("COMMENT")
                    .HasMaxLength(200)
                    .HasAnnotation("Description", "Комментарий");

                entity.Property(e => e.Phone)
                    .HasColumnName("PHONE")
                    .HasAnnotation("Description", "Телефон");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("DEFAULT 1")
                    .HasAnnotation("Description", "0 : на маршруте; 1 : у нас; 2 : неизвестно; 3 : камеры; 4 : на блок");
            });

            builder.Entity<WebMapUser>(entity =>
            {
                entity.ToTable("USERS                          ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_USERS");

                entity.Property(e => e.Id).HasColumnName("ID_");

                entity.Property(e => e.FactAddrLat)
                    .HasColumnName("FACT_ADDR_LAT")
                    .HasMaxLength(25)
                    .HasAnnotation("Description", "Для карты - маркер баз коммунальщиков");

                entity.Property(e => e.FactAddrLon)
                    .HasColumnName("FACT_ADDR_LON")
                    .HasMaxLength(25)
                    .HasAnnotation("Description", "Для карты - маркер баз коммунальщиков");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME_")
                    .HasMaxLength(50)
                    .HasAnnotation("Description", "Логин");

                entity.Property(e => e.Pass)
                    .HasColumnName("PASS_")
                    .HasMaxLength(50)
                    .HasAnnotation("Description", "Пароль");
            });

            builder.Entity<UserProject>(entity =>
            {
                entity.ToTable("USERS_PROJS                    ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_USERS_PROJS");

                entity.HasIndex(e => e.ProjectId)
                    .HasName("FK_USERS_PROJS")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_USERS_PROJS2")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID_");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("PROJ_ID_")
                    .HasAnnotation("Description", "Перевозчик");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_")
                    .HasAnnotation("Description", "Пользователь");
            });
        }
    }
}
