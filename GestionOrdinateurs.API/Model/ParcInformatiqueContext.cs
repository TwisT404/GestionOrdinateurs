using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API.Data;

public class ParcInformatiqueContext : DbContext
{
    //Propriétés
    public DbSet<Ordinateurs> Ordinateurs { get; set; }
    public DbSet<Reparation> Reparations {get;set;}
    public DbSet<User> Utilisateurs {get;set;}

    //Configuration
    public ParcInformatiqueContext(DbContextOptions<ParcInformatiqueContext> options): base(options) {
        
     }


    //Configuration de la db SqLite
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=parcinfo.db");
        }
    }
    
    //Au build : Liaison des entités
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Ordinateurs>()
            .OwnsOne(o => o.Caracteristiques);

        modelBuilder.Entity<Ordinateurs>()
            .HasMany(o => o.Reparations)
            .WithOne(r => r.UnOrdinateur);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();

    }

    

}