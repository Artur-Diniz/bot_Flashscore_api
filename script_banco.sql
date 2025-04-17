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
CREATE TABLE [TB_ERROSLOGS] (
    [Id] int NOT NULL IDENTITY,
    [QualPageFoi] varchar(250) NULL,
    [QualUrl] varchar(250) NULL,
    [OqueProvavelmenteAConteceu] varchar(510) NULL,
    CONSTRAINT [PK_TB_ERROSLOGS] PRIMARY KEY ([Id])
);

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
    [Gol] real NULL,
    [GolSofrido] real NULL,
    [Posse_Bola] real NULL,
    [Total_Finalizacao] real NULL,
    [Chances_Claras] real NULL,
    [Escanteios] real NULL,
    [Bolas_trave] real NULL,
    [Gols_de_cabeça] real NULL,
    [Defesas_Goleiro] real NULL,
    [Impedimentos] real NULL,
    [Faltas] real NULL,
    [Cartoes_Amarelos] real NULL,
    [Cartoes_Vermelhos] real NULL,
    [Laterais_Cobrados] real NULL,
    [Toque_Area_Adversaria] real NULL,
    [Passes] real NULL,
    [Passes_Totais] real NULL,
    [Precisao_Passes] real NULL,
    [Passes_terco_Final] real NULL,
    [Cruzamentos] real NULL,
    [Desarmes] real NULL,
    [Bolas_Afastadas] real NULL,
    [Interceptacoes] real NULL,
    [Gol_Adversaria] real NULL,
    [GolSofrido_Adversaria] real NULL,
    [Posse_Bola_Adversaria] real NULL,
    [Total_Finalizacao_Adversaria] real NULL,
    [Chances_Claras_Adversaria] real NULL,
    [Escanteios_Adversaria] real NULL,
    [Bolas_trave_Adversaria] real NULL,
    [Gols_de_cabeça_Adversaria] real NULL,
    [Defesas_Goleiro_Adversaria] real NULL,
    [Impedimentos_Adversaria] real NULL,
    [Faltas_Adversaria] real NULL,
    [Cartoes_Amarelos_Adversaria] real NULL,
    [Cartoes_Vermelhos_Adversaria] real NULL,
    [Laterais_Cobrados_Adversaria] real NULL,
    [Toque_Area_Adversaria_Adversaria] real NULL,
    [Passes_Adversaria] real NULL,
    [Passes_Totais_Adversaria] real NULL,
    [Precisao_Passes_Adversaria] real NULL,
    [Passes_terco_Final_Adversaria] real NULL,
    [Cruzamentos_Adversaria] real NULL,
    [Desarmes_Adversaria] real NULL,
    [Bolas_Afastadas_Adversaria] real NULL,
    [Interceptacoes_Adversaria] real NULL,
    [Gol_Confrontos] real NULL,
    [GolSofrido_Confrontos] real NULL,
    [Posse_Bola_Confrontos] real NULL,
    [Total_Finalizacao_Confrontos] real NULL,
    [Chances_Claras_Confrontos] real NULL,
    [Escanteios_Confrontos] real NULL,
    [Bolas_trave_Confrontos] real NULL,
    [Gols_de_cabeça_Confrontos] real NULL,
    [Defesas_Goleiro_Confrontos] real NULL,
    [Impedimentos_Confrontos] real NULL,
    [Faltas_Confrontos] real NULL,
    [Cartoes_Amarelos_Confrontos] real NULL,
    [Cartoes_Vermelhos_Confrontos] real NULL,
    [Laterais_Cobrados_Confrontos] real NULL,
    [Toque_Area_Adversaria_Confrontos] real NULL,
    [Passes_Confrontos] real NULL,
    [Passes_Totais_Confrontos] real NULL,
    [Precisao_Passes_Confrontos] real NULL,
    [Passes_terco_Final_Confrontos] real NULL,
    [Cruzamentos_Confrontos] real NULL,
    [Desarmes_Confrontos] real NULL,
    [Bolas_Afastadas_Confrontos] real NULL,
    [Interceptacoes_Confrontos] real NULL,
    CONSTRAINT [PK_TB_ESTATISTICA_TIME] PRIMARY KEY ([Id])
);

CREATE TABLE [TB_PALPITES] (
    [Id] int NOT NULL IDENTITY,
    [IdPartida] int NOT NULL,
    [TipoAposta] int NOT NULL,
    [Num] float NOT NULL,
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
VALUES (N'20250417162906_InitialCreate', N'9.0.3');

COMMIT;
GO

