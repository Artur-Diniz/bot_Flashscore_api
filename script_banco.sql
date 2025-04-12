IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [TB_ESTATISTICA] (
    [Id_Estatistica] int NOT NULL IDENTITY,
    [Id_Partida] int NOT NULL,
    [CasaOuFora] varchar(250) NULL,
    [NomeTime] varchar(250) NULL,
    [NomeTimeRival] varchar(250) NULL,
    [Gol] int NULL,
    [GolSofrido] int NULL,
    [Posse_Bola] int NULL,
    [Total_Finalizacao] int NULL,
    [Chances_Claras] int NULL,
    [Escanteios] int NULL,
    [Bolas_trave] int NULL,
    [Gols_de_cabeça] int NULL,
    [Defesas_Goleiro] int NULL,
    [Impedimentos] int NULL,
    [Faltas] int NULL,
    [Cartoes_Amarelos] int NULL,
    [Cartoes_Vermelhos] int NULL,
    [Laterais_Cobrados] int NULL,
    [Toque_Area_Adversaria] int NULL,
    [Passes] int NULL,
    [Passes_Totais] int NULL,
    [Precisao_Passes] int NULL,
    [Passes_terco_Final] int NULL,
    [Cruzamentos] int NULL,
    [Desarmes] int NULL,
    [Bolas_Afastadas] int NULL,
    [Interceptacoes] int NULL,
    CONSTRAINT [PK_TB_ESTATISTICA] PRIMARY KEY ([Id_Estatistica])
);

CREATE TABLE [TB_ESTATISTICA_TIME] (
    [Id] int NOT NULL IDENTITY,
    [CasaOuFora] varchar(250) NULL,
    [NomeTime] varchar(250) NULL,
    [Gol] real NOT NULL,
    [GolSofrido] real NOT NULL,
    [Posse_Bola] real NOT NULL,
    [Total_Finalizacao] real NOT NULL,
    [Chances_Claras] real NOT NULL,
    [Escanteios] real NOT NULL,
    [Bolas_trave] real NOT NULL,
    [Gols_de_cabeça] real NOT NULL,
    [Defesas_Goleiro] real NOT NULL,
    [Impedimentos] real NOT NULL,
    [Faltas] real NOT NULL,
    [Cartoes_Amarelos] real NOT NULL,
    [Cartoes_Vermelhos] real NOT NULL,
    [Laterais_Cobrados] real NOT NULL,
    [Toque_Area_Adversaria] real NOT NULL,
    [Passes] real NOT NULL,
    [Passes_Totais] real NOT NULL,
    [Precisao_Passes] real NOT NULL,
    [Passes_terco_Final] real NOT NULL,
    [Cruzamentos] real NOT NULL,
    [Desarmes] real NOT NULL,
    [Bolas_Afastadas] real NOT NULL,
    [Interceptacoes] real NOT NULL,
    [Gol_Adversaria] real NOT NULL,
    [GolSofrido_Adversaria] real NOT NULL,
    [Posse_Bola_Adversaria] real NOT NULL,
    [Total_Finalizacao_Adversaria] real NOT NULL,
    [Chances_Claras_Adversaria] real NOT NULL,
    [Escanteios_Adversaria] real NOT NULL,
    [Bolas_trave_Adversaria] real NOT NULL,
    [Gols_de_cabeça_Adversaria] real NOT NULL,
    [Defesas_Goleiro_Adversaria] real NOT NULL,
    [Impedimentos_Adversaria] real NOT NULL,
    [Faltas_Adversaria] real NOT NULL,
    [Cartoes_Amarelos_Adversaria] real NOT NULL,
    [Cartoes_Vermelhos_Adversaria] real NOT NULL,
    [Laterais_Cobrados_Adversaria] real NOT NULL,
    [Toque_Area_Adversaria_Adversaria] real NOT NULL,
    [Passes_Adversaria] real NOT NULL,
    [Passes_Totais_Adversaria] real NOT NULL,
    [Precisao_Passes_Adversaria] real NOT NULL,
    [Passes_terco_Final_Adversaria] real NOT NULL,
    [Cruzamentos_Adversaria] real NOT NULL,
    [Desarmes_Adversaria] real NOT NULL,
    [Bolas_Afastadas_Adversaria] real NOT NULL,
    [Interceptacoes_Adversaria] real NOT NULL,
    [Gol_Confrontos] real NOT NULL,
    [GolSofrido_Confrontos] real NOT NULL,
    [Posse_Bola_Confrontos] real NOT NULL,
    [Total_Finalizacao_Confrontos] real NOT NULL,
    [Chances_Claras_Confrontos] real NOT NULL,
    [Escanteios_Confrontos] real NOT NULL,
    [Bolas_trave_Confrontos] real NOT NULL,
    [Gols_de_cabeça_Confrontos] real NOT NULL,
    [Defesas_Goleiro_Confrontos] real NOT NULL,
    [Impedimentos_Confrontos] real NOT NULL,
    [Faltas_Confrontos] real NOT NULL,
    [Cartoes_Amarelos_Confrontos] real NOT NULL,
    [Cartoes_Vermelhos_Confrontos] real NOT NULL,
    [Laterais_Cobrados_Confrontos] real NOT NULL,
    [Toque_Area_Adversaria_Confrontos] real NOT NULL,
    [Passes_Confrontos] real NOT NULL,
    [Passes_Totais_Confrontos] real NOT NULL,
    [Precisao_Passes_Confrontos] real NOT NULL,
    [Passes_terco_Final_Confrontos] real NOT NULL,
    [Cruzamentos_Confrontos] real NOT NULL,
    [Desarmes_Confrontos] real NOT NULL,
    [Bolas_Afastadas_Confrontos] real NOT NULL,
    [Interceptacoes_Confrontos] real NOT NULL,
    CONSTRAINT [PK_TB_ESTATISTICA_TIME] PRIMARY KEY ([Id])
);

CREATE TABLE [TB_PALPITES] (
    [Id] int NOT NULL IDENTITY,
    [IdPartida] int NOT NULL,
    [TipoAposta] int NOT NULL,
    [Num] float NULL,
    [Descricao] varchar(250) NULL,
    CONSTRAINT [PK_TB_PALPITES] PRIMARY KEY ([Id])
);

CREATE TABLE [TB_PARTIDAS] (
    [Id] int NOT NULL IDENTITY,
    [Id_EstatisticaCasa] int NOT NULL,
    [Id_EstatisticaFora] int NOT NULL,
    [NomeTimeCasa] varchar(250) NULL,
    [NomeTimeFora] varchar(250) NULL,
    [DataPartida] datetime2 NOT NULL,
    [Campeonato] varchar(250) NULL,
    [PartidaAnalise] bit NOT NULL,
    [TipoPartida] varchar(250) NULL,
    CONSTRAINT [PK_TB_PARTIDAS] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250408010857_InitialCreate', N'9.0.3');

COMMIT;
GO

