using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SkachkiWebApp.Models;
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


        UserModel user = new UserModel { Id = 1, Email = "gleb@gmail.com", Password = "qwerty", CreationDate = DateTime.Now, RoleId = 1 };
        modelBuilder.Entity<UserModel>().HasData(user);

        //modelBuilder.Entity<Ippodrom>().HasData(new Ippodrom { Id=2, Address="Krasnaya 23", Description="Nice map"}); // test data
    }
}