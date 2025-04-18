using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;

namespace botAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Estatistica> TB_ESTATISTICA { get; set; }
        public DbSet<Estatistica_Times> TB_ESTATISTICA_TIME { get; set; }
        public DbSet<Palpites> TB_PALPITES { get; set; }
        public DbSet<Partida> TB_PARTIDAS { get; set; }
        public DbSet<ErrosLogs> TB_ERROSLOGS { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<ErrosLogs>(entity =>
            {
                entity.Property(e => e.OqueProvavelmenteAConteceu)
                    .HasMaxLength(510)
                    .HasColumnType("varchar(510)");
            });

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar").HaveMaxLength(250);
        }
    }
}