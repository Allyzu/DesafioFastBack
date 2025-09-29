using DesafioFast.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioFast.Data
{
    public class ApppDbContext : DbContext
    {
        public ApppDbContext(DbContextOptions<ApppDbContext> options ) : base(options)
        {
        }

        public DbSet<Models.ColaboradorModels> Colaboradores { get; set; }
        public DbSet<Models.WorkshopModels> Workshops { get; set; }
        public DbSet<Models.AtaModels> Atas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Workshop ↔ Colaborador (N:N)
            modelBuilder.Entity<ColaboradorModels>()
                .HasMany(c => c.Workshops)
                .WithMany(w => w.Colaboradores)
                .UsingEntity<Dictionary<string, object>>(
                    "ColaboradorWorkshop",
                    j => j.HasOne<WorkshopModels>()
                          .WithMany()
                          .HasForeignKey("WorkshopId")
                          .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<ColaboradorModels>()
                          .WithMany()
                          .HasForeignKey("ColaboradorId")
                          .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey("ColaboradorId", "WorkshopId");
                        j.ToTable("ColaboradorWorkshops");
                    });

            // Ata ↔ Workshop (N:1)
            modelBuilder.Entity<AtaModels>()
                .HasOne(a => a.Workshop)
                .WithMany(w => w.Atas)
                .HasForeignKey(a => a.WorkshopId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ata ↔ Colaborador (N:N)
            modelBuilder.Entity<AtaModels>()
                .HasMany(a => a.ColaboradoresList)
                .WithMany(c => c.Atas)
                .UsingEntity<Dictionary<string, object>>(
                    "AtaColaborador",
                    j => j.HasOne<ColaboradorModels>()
                          .WithMany()
                          .HasForeignKey("ColaboradorId")
                          .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<AtaModels>()
                          .WithMany()
                          .HasForeignKey("AtaId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("AtaId", "ColaboradorId");
                        j.ToTable("AtaColaboradores");
                    });
        }
    }
}
