using Es_sett18_NicolasO.Models;
using Es_sett18_NicolasO.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Es_sett18_NicolasO.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Artista> Artisti { get; set; }
        public DbSet<Evento> Eventi { get; set; }
        public DbSet<Biglietto> Biglietti { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Artista>()
                .HasMany(a => a.Eventi)
                .WithOne(e => e.Artista)
                .HasForeignKey(e => e.ArtistaId)
                .OnDelete(DeleteBehavior.Cascade);
           modelBuilder.Entity<Evento>()
                .HasMany(e => e.Biglietti)
                .WithOne(b => b.Evento)
                .HasForeignKey(b => b.EventoId)
                .OnDelete(DeleteBehavior.Cascade); 
            
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Biglietti)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

           
            modelBuilder.Entity<Evento>()
                .HasIndex(e => e.Data);

            modelBuilder.Entity<Evento>()
                .HasIndex(e => e.Luogo);

            modelBuilder.Entity<Artista>()
                .HasIndex(a => a.Nome)
                .IsUnique();

           
            modelBuilder.Entity<Evento>()
                .Property(e => e.Titolo)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Evento>()
                .Property(e => e.Luogo)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Artista>()
                .Property(a => a.Nome)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Artista>()
                .Property(a => a.Genere)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Biglietto>()
                .Property(b => b.DataAcquisto)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}