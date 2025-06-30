using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;

namespace botAPI.Data
{
    public class MLDbContext : DbContext
    {
        public MLDbContext(DbContextOptions<MLDbContext> options) : base(options)
        {

        }
        public DbSet<Estatistica> TB_ESTATISTICA { get; set; }
        public DbSet<Estatistica_Times> TB_ESTATISTICA_TIME { get; set; }
        public DbSet<Palpites> TB_PALPITES { get; set; }
        public DbSet<Partida> TB_PARTIDAS { get; set; }
        public DbSet<ErrosLogs> TB_ERROSLOGS { get; set; }
        public DbSet<Estatistica_BaseModel> TB_ESTATISTICA_BASEMODEL { get; set; }

        public DbSet<Partida_Estatistica_Esperadas> TB_PARTIDA_ESTAITSTICA_ESPERADAS { get; set; }
        public DbSet<Estatistica_Esperadas> TB_ESTATISTICA_ESPERADAS { get; set; }
        public DbSet<MetodoGeradorPalpites> TB_METODOPALPITES { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estatistica>().Property(e => e.Id_Estatistica).ValueGeneratedOnAdd();
            modelBuilder.Entity<Estatistica_Times>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Palpites>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Partida>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Estatistica_BaseModel>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Partida_Estatistica_Esperadas>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Estatistica_Esperadas>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<MetodoGeradorPalpites>().Property(e => e.Id).ValueGeneratedOnAdd();

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

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MetodoGeradorPalpites>().HasData(
            new MetodoGeradorPalpites { Id = 1, Nome = "Under 4 gols", Versao = "1.0", Descricao = "Para partidas que houver menos de 4 gols em tempo regulamentar", Condicoes = "media de gols feitos casa mais media de gols sofridos fora /2  o mesmo para visitante" },
            new MetodoGeradorPalpites { Id = 2, Nome = "Over 2 gols", Versao = "1.0", Descricao = "Para partidas que houver mais de 2 gols em tempo regulamentar", Condicoes = "media de gols feitos casa mais media de gols sofridos fora /2  o mesmo para visitante" },
            new MetodoGeradorPalpites { Id = 3, Nome = "Vencedor", Versao = "1.0", Descricao = "Para definir quem sera o vencedor da partida em termpo regulamentar", Condicoes = "baseado em 4 características para definir quem tem a maior probabilidade de vencer, sendo eles posse de bola,Precisão dos passes,gols e jogos sem sofre gol" },
            new MetodoGeradorPalpites { Id = 4, Nome = "Over escanteios Variaveis", Versao = "1.0", Descricao = "Para definir a linha de over escanteios da Partida em termpo regulamentar", Condicoes = "baseado na media de escanteios feitos e  sofridos de cada time /4 " },
            new MetodoGeradorPalpites { Id = 5, Nome = "Under escanteios Variaveis", Versao = "1.0", Descricao = "Para definir a linha de Under escanteios da Partida em termpo regulamentar", Condicoes = "baseado na media de escanteios feitos e  sofridos de cada time /4 " },
            new MetodoGeradorPalpites { Id = 6, Nome = "Over 0.5 Time", Versao = "1.0", Descricao = "Para definir se um time faz um gol no adversario em termpo regulamentar", Condicoes = "media de gols feitos casa mais media de gols sofridos fora /2  " }
            );

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar").HaveMaxLength(250);
        }
    }
}