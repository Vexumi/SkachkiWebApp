using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class ApplicationContext : DbContext
{
    public DbSet<CompetitionModel> Competitions { get; set; } = null!;
    public DbSet<CompetitionTicketModel> CompetitionTickets { get; set; } = null!;
    public DbSet<HorseModel> Horses { get; set; } = null!;
    public DbSet<HorseOwnerModel> HorseOwners { get; set; } = null!;
    public DbSet<IppodromModel> Ippodroms { get; set; } = null!;
    public DbSet<JokeyModel> Jokeys { get; set; } = null!;

    public ApplicationContext(DbContextOptions options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlite("Data Source=DataBases/Skachki.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompetitionTicketModel>().HasKey(o => new { o.TiketId, o.CompetitionId, o.HorseId, o.JokeyId});
        //modelBuilder.Entity<Ippodrom>().HasData(new Ippodrom { Id=2, Address="Krasnaya 23", Description="Nice map"});
    }
}