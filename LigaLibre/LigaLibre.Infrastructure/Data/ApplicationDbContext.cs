
using LigaLibre.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LigaLibre.Infrastructure.Data;


public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // contstructor que recibe opciones y las pasa a la clase base
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    // DbSet para cada entidad
    public DbSet<Club> Club { get; set; }
    public DbSet<Player> Player { get; set; }
    public DbSet<Match> Match { get; set; }
    public DbSet<Referee> Referee { get; set; }

    // Configuración del modelo
    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        base.OnModelCreating(modelbuilder);

        modelbuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.City).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.StadiumName).HasMaxLength(100);
            entity.Property(e => e.NumberOfPartners).IsRequired();
        });
        modelbuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Position).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Nationality).IsRequired().HasMaxLength(50);
            entity.Property(e => e.JerseyNumber).IsRequired();
            entity.Property(e => e.Height).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Weight).HasColumnType("decimal(5, 2)");

            entity.HasOne(e => e.Club).WithMany(p => p.Players)
            .HasForeignKey(e => e.ClubId);


        });
        modelbuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Stadium).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(e => e.HomeClub)
            .WithMany(c => c.HomeMatches)
            .HasForeignKey(c=>c.HomeClubId)
            .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.AwayClub)
            .WithMany(c => c.AwayMatches)
            .HasForeignKey(c=>c.AwayClubId)
            .OnDelete(DeleteBehavior.NoAction);


            entity.HasOne(e => e.Referee)
            .WithMany(r => r.Matches)
            .HasForeignKey(e => e.RefereeId);

        });
        modelbuilder.Entity<Referee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.LicenseNumber).IsUnique();

        });

    }
}


