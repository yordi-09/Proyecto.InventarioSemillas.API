using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecto.InventarioSemillas.API.Models;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Especie> Especies => Set<Especie>();
    public DbSet<Semilla> Semillas => Set<Semilla>();
    public DbSet<Ubicacion> Ubicaciones => Set<Ubicacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Especie>().ToTable("Especie");

        modelBuilder.Entity<Semilla>().ToTable("Semilla");

        modelBuilder.Entity<Ubicacion>().ToTable("Ubicacion");

        base.OnModelCreating(modelBuilder);
    }
}