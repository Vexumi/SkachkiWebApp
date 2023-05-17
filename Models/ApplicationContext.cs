using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SkachkiWebApp.Areas.user.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class ApplicationContext : DbContext
{
    public DbSet<CompetitionModel> Competitions { get; set; } = null!;
    public DbSet<CompetitionTicketModel> CompetitionTickets { get; set; } = null!;
    public DbSet<HorseModel> Horses { get; set; } = null!;
    public DbSet<HorseOwnerModel> HorseOwners { get; set; } = null!;
    public DbSet<IppodromModel> Ippodroms { get; set; } = null!;
    public DbSet<JokeyModel> Jokeys { get; set; } = null!;
    public DbSet<UserModel> Users { get; set; } = null!;
    public DbSet<RoleModel> Roles { get; set; } = null;

    public ApplicationContext(DbContextOptions options) : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlite("Data Source=DataBases/Skachki.db");
        optionsBuilder.EnableSensitiveDataLogging();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        string adminRoleName = "admin";
        string jokeyRoleName = "jokey";
        string horseOwnerRoleName = "howner";

        // Добавление ролей
        RoleModel adminRole = new RoleModel { Id = 1, Name = adminRoleName };
        RoleModel jokeyRole = new RoleModel { Id = 2, Name = jokeyRoleName };
        RoleModel horseOwnerRole = new RoleModel { Id = 3, Name = horseOwnerRoleName };

        modelBuilder.Entity<RoleModel>().HasData(new RoleModel[] { adminRole, jokeyRole, horseOwnerRole });
        modelBuilder.Entity<CompetitionTicketModel>().HasKey(o => o.TiketId );


        // Delete on prod
        UserModel user = new UserModel { Id = 1, Email = "admin@admin", Password = "admin", CreationDate = DateTime.Now, RoleId = 1 };
        modelBuilder.Entity<UserModel>().HasData(user);

        IppodromModel ippodrom = new IppodromModel { Id = 1, Address = "Ул.Молодости 10", Description = "Ипподром в центре гэса!" };
        modelBuilder.Entity<IppodromModel>().HasData(ippodrom);

        CompetitionModel competition = new CompetitionModel { Id = 1, Name="Всероссийские гонки на лошадях", Date=DateTime.Now.AddDays(10), IppodromId=1,  PublicationDate=DateTime.Now, Recruiting=true, 
            Description= "Кубок Победы – серия турниров по конному спорту, " +
            "которая на протяжении 5 лет проводится группой компаний MAXIMA. " +
            "Цель турнира - объединить спортсменов-конников из регионов России, а также дружественных стран, " +
            "и почтить память героев Великой Отечественной Войны новыми спортивными победами. \r\n\r\n9 " +
            "мая станет одним из важнейших дней турнира. Участников и гостей соревнований ждет яркая и насыщенная программа," +
            " посвященная празднику Великой Победы. \r\n\r\nВ этот день будут определены лидеры финала Кубка Победы -2023 по" +
            " конкуру и выездке, а также состоится праздничный ланч для ветеранов конного спорта России – самых заслуженных всадников, " +
            "составляющих славу отечественного конного спорта.\r\n\r\nВ турнире по выездке спортсмены выступят в программах для детей, " +
            "юношей и юниоров, а также Малых ездах, конкуристы разыграют медали в маршрутах с высотами препятствий от 90 см до 150 см. " +
            "В рамках конкурного турнира определяться победители личного и командного первенства «Команда Победы». \r\n\r\nТакже " +
            "9 мая на Олимпийском поле MAXIMA PARK пройдет торжественный парад в честь Великой Победы. Участники турнира и гости" +
            " отдадут дань памяти героям минутой молчания и примут участие в акции «Бессмертный Полк».\r\n\r\nТурнир состоится на" +
            " боевых полях MAXIMA PARK по адресу - Московская область, Дмитровский район, дер. Горки Сухаревские.\r\n\r\nВход на " +
            "территорию MAXIMA PARK и доступ на трибуны спортивных площадок открыт для всех желающих!" };
        modelBuilder.Entity<CompetitionModel>().HasData(competition);
        //modelBuilder.Entity<Ippodrom>().HasData(new Ippodrom { Id=2, Address="Krasnaya 23", Description="Nice map"}); // test data
    }
}