using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;

namespace botAPI.Data
{

    public class MLDbContext : DbContext
    {
        public MLDbContext(DbContextOptions<MLDbContext> options) : base(options) { }

        // Tabelas do banco de ML (exatamente como no seu schema)
        public DbSet<Estatistica_BaseModel> estatistica_basemodel { get; set; }
        public DbSet<Estatistica_Esperadas> tb_estatistica_esperadas { get; set; }
        public DbSet<Partida> tb_partidas { get; set; }
        public DbSet<Partida_Estatistica_Esperadas> tb_partida_estatistica_esperadas { get; set; }
        public DbSet<ml_features> ml_features { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da tabela estatistica_basemodel
            modelBuilder.Entity<Estatistica_BaseModel>(entity =>
            {
                entity.ToTable("estatistica_basemodel");
                entity.HasKey(e => e.Id);
            });

            // Configuração da tabela tb_partidas
            modelBuilder.Entity<Partida>(entity =>
            {
                entity.ToTable("tb_partidas");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.NomeTimeCasa).IsRequired().HasMaxLength(250);
                entity.Property(e => e.NomeTimeFora).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Campeonato).IsRequired().HasMaxLength(250);
                entity.Property(e => e.PartidaAnalise).HasDefaultValue(false);
            });

            // Configuração da tabela tb_estatistica_esperadas
            modelBuilder.Entity<Estatistica_Esperadas>(entity =>
            {
                entity.ToTable("tb_estatistica_esperadas");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.NomeTime).IsRequired().HasMaxLength(250);

                // Relacionamentos
                entity.HasOne(e => e.FT)
                     .WithMany()
                     .HasForeignKey(e => e.FT_Id)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.HT)
                     .WithMany()
                     .HasForeignKey(e => e.HT_Id)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FT_Adversario)
                     .WithMany()
                     .HasForeignKey(e => e.FT_Adversario_Id)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.HT_Adversario)
                     .WithMany()
                     .HasForeignKey(e => e.HT_Adversario_Id)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FT_Confronto)
                     .WithMany()
                     .HasForeignKey(e => e.FT_Confronto_Id)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.HT_Confronto)
                     .WithMany()
                     .HasForeignKey(e => e.HT_Confronto_Id)
                     .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração da tabela tb_partida_estatistica_esperadas
            modelBuilder.Entity<Partida_Estatistica_Esperadas>(entity =>
            {
                entity.ToTable("tb_partida_estatistica_esperadas");
                entity.HasKey(e => e.Id);

                // Relacionamentos
                entity.HasOne(e => e.Partida)
                     .WithMany()
                     .HasForeignKey(e => e.Id_Partida)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Estatisticas_Esperadas_Casa)
                     .WithMany()
                     .HasForeignKey(e => e.Id_Estatisticas_Esperadas_Casa)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Estatisticas_Esperadas_Fora)
                     .WithMany()
                     .HasForeignKey(e => e.Id_Estatisticas_Esperadas_Fora)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Partida_FT)
                     .WithMany()
                     .HasForeignKey(e => e.Id_Partida_FT)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Partida_HT)
                     .WithMany()
                     .HasForeignKey(e => e.Id_Partida_HT)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Partida_FT_Confronto)
                     .WithMany()
                     .HasForeignKey(e => e.Id_Partida_FT_Confronto)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Partida_HT_Confronto)
                     .WithMany()
                     .HasForeignKey(e => e.Id_Partida_HT_Confronto)
                     .OnDelete(DeleteBehavior.Restrict);

                // Garante uma entrada única por partida
                entity.HasIndex(e => e.Id_Partida).IsUnique();
            });

            // Configuração da tabela ml_features
            modelBuilder.Entity<ml_features>(entity =>
            {
                entity.ToTable("ml_features");
                entity.HasKey(e => e.id);

                entity.Property(e => e.time_id).IsRequired().HasMaxLength(100);
                entity.Property(e => e.features).IsRequired();
                entity.Property(e => e.versao_modelo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.data_processamento).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(e => e.partida)
                     .WithMany()
                     .HasForeignKey(e => e.partida_id)
                     .OnDelete(DeleteBehavior.Restrict);
            });


        }
    }
}