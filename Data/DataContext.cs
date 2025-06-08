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
        public DbSet<ErrosLogs> TB_ERROSLOGS { get; set; }

        public DbSet<Estatistica> TB_ESTATISTICA { get; set; }
        public DbSet<Estatistica_Times> TB_ESTATISTICA_TIME { get; set; }
        public DbSet<Palpites> TB_PALPITES { get; set; }
        public DbSet<Partida> TB_PARTIDAS { get; set; }
        public DbSet<Estatistica_BaseModel> TB_ESTATISTICA_BASEMODEL { get; set; }

        public DbSet<Partida_Estatistica_Esperadas> TB_PARTIDA_ESTAITSTICA_ESPERADAS { get; set; }
        public DbSet<Estatistica_Esperadas> TB_ESTATISTICA_ESPERADAS { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IDs serão definidos manualmente
            modelBuilder.Entity<Estatistica>().Property(e => e.Id_Estatistica).ValueGeneratedNever();
            modelBuilder.Entity<Estatistica_Times>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Palpites>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Partida>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Estatistica_BaseModel>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Partida_Estatistica_Esperadas>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Estatistica_Esperadas>().Property(e => e.Id).ValueGeneratedNever();

            modelBuilder.Entity<ErrosLogs>(entity =>
            {
                entity.Property(e => e.OqueProvavelmenteAConteceu)
                    .HasMaxLength(510)
                    .HasColumnType("varchar(510)");
            });
            modelBuilder.Entity<Palpites>(entity =>
            {
                entity.Property(e => e.Descricao)
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