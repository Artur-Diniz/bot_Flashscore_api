﻿IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
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
    [horaErro] datetime2 NOT NULL,
    [QualPageFoi] varchar(250) NULL,
    [QualUrl] varchar(250) NULL,
    [OqueProvavelmenteAConteceu] varchar(510) NULL,
    CONSTRAINT [PK_TB_ERROSLOGS] PRIMARY KEY ([Id])
);

CREATE TABLE [TB_ESTATISTICA] (
    [Id_Estatistica] int NOT NULL,
    [Id_Partida] int NOT NULL,
    [CasaOuFora] varchar(250) NULL,
    [TipoPartida] varchar(250) NULL,
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
    [Gol_HT] int NULL,
    [GolSofrido_HT] int NULL,
    [Posse_Bola_HT] int NULL,
    [Total_Finalizacao_HT] int NULL,
    [Chances_Claras_HT] int NULL,
    [Escanteios_HT] int NULL,
    [Bolas_trave_HT] int NULL,
    [Gols_de_cabeça_HT] int NULL,
    [Defesas_Goleiro_HT] int NULL,
    [Impedimentos_HT] int NULL,
    [Faltas_HT] int NULL,
    [Cartoes_Amarelos_HT] int NULL,
    [Cartoes_Vermelhos_HT] int NULL,
    [Laterais_Cobrados_HT] int NULL,
    [Toque_Area_Adversaria_HT] int NULL,
    [Passes_HT] int NULL,
    [Passes_Totais_HT] int NULL,
    [Precisao_Passes_HT] int NULL,
    [Passes_terco_Final_HT] int NULL,
    [Cruzamentos_HT] int NULL,
    [Desarmes_HT] int NULL,
    [Bolas_Afastadas_HT] int NULL,
    [Interceptacoes_HT] int NULL,
    CONSTRAINT [PK_TB_ESTATISTICA] PRIMARY KEY ([Id_Estatistica])
);

CREATE TABLE [TB_ESTATISTICA_BASEMODEL] (
    [Id] int NOT NULL,
    [Gol] real NULL,
    [Gol_Slope] real NULL,
    [Gol_DP] real NULL,
    [GolSofrido] real NULL,
    [GolSofrido_Slope] real NULL,
    [GolSofrido_DP] real NULL,
    [Posse_Bola] real NULL,
    [Posse_Bola_Slope] real NULL,
    [Posse_Bola_DP] real NULL,
    [Total_Finalizacao] real NULL,
    [Total_Finalizacao_Slope] real NULL,
    [Total_Finalizacao_DP] real NULL,
    [Chances_Claras] real NULL,
    [Escanteios] real NULL,
    [Escanteios_Slope] real NULL,
    [Escanteios_DP] real NULL,
    [Bolas_trave] real NULL,
    [Gols_de_cabeca] real NULL,
    [Defesas_Goleiro] real NULL,
    [Impedimentos] real NULL,
    [Impedimentos_Slope] real NULL,
    [Impedimentos_DP] real NULL,
    [Faltas] real NULL,
    [Faltas_Slope] real NULL,
    [Faltas_DP] real NULL,
    [Cartoes_Amarelos] real NULL,
    [Cartoes_Amarelos_Slope] real NULL,
    [Cartoes_Amarelos_DP] real NULL,
    [Cartoes_Vermelhos] real NULL,
    [Cartoes_Vermelhos_Slope] real NULL,
    [Cartoes_Vermelhos_DP] real NULL,
    [Laterais_Cobrados] real NULL,
    [Toque_Area_Adversaria] real NULL,
    [Passes] real NULL,
    [Passes_Totais] real NULL,
    [Precisao_Passes] real NULL,
    [Passes_terco_Final] real NULL,
    [Cruzamentos] real NULL,
    [Desarmes] real NULL,
    [Desarmes_Slope] real NULL,
    [Desarmes_DP] real NULL,
    [Bolas_Afastadas] real NULL,
    [Interceptacoes] real NULL,
    CONSTRAINT [PK_TB_ESTATISTICA_BASEMODEL] PRIMARY KEY ([Id])
);

CREATE TABLE [TB_ESTATISTICA_TIME] (
    [Id] int NOT NULL,
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
    [Gol_HT] real NULL,
    [GolSofrido_HT] real NULL,
    [Posse_Bola_HT] real NULL,
    [Total_Finalizacao_HT] real NULL,
    [Chances_Claras_HT] real NULL,
    [Escanteios_HT] real NULL,
    [Bolas_trave_HT] real NULL,
    [Gols_de_cabeça_HT] real NULL,
    [Defesas_Goleiro_HT] real NULL,
    [Impedimentos_HT] real NULL,
    [Faltas_HT] real NULL,
    [Cartoes_Amarelos_HT] real NULL,
    [Cartoes_Vermelhos_HT] real NULL,
    [Laterais_Cobrados_HT] real NULL,
    [Toque_Area_Adversaria_HT] real NULL,
    [Passes_HT] real NULL,
    [Passes_Totais_HT] real NULL,
    [Precisao_Passes_HT] real NULL,
    [Passes_terco_Final_HT] real NULL,
    [Cruzamentos_HT] real NULL,
    [Desarmes_HT] real NULL,
    [Bolas_Afastadas_HT] real NULL,
    [Interceptacoes_HT] real NULL,
    [Gol_Adversaria_HT] real NULL,
    [GolSofrido_Adversaria_HT] real NULL,
    [Posse_Bola_Adversaria_HT] real NULL,
    [Total_Finalizacao_Adversaria_HT] real NULL,
    [Chances_Claras_Adversaria_HT] real NULL,
    [Escanteios_Adversaria_HT] real NULL,
    [Bolas_trave_Adversaria_HT] real NULL,
    [Gols_de_cabeça_Adversaria_HT] real NULL,
    [Defesas_Goleiro_Adversaria_HT] real NULL,
    [Impedimentos_Adversaria_HT] real NULL,
    [Faltas_Adversaria_HT] real NULL,
    [Cartoes_Amarelos_Adversaria_HT] real NULL,
    [Cartoes_Vermelhos_Adversaria_HT] real NULL,
    [Laterais_Cobrados_Adversaria_HT] real NULL,
    [Toque_Area_Adversaria_Adversaria_HT] real NULL,
    [Passes_Adversaria_HT] real NULL,
    [Passes_Totais_Adversaria_HT] real NULL,
    [Precisao_Passes_Adversaria_HT] real NULL,
    [Passes_terco_Final_Adversaria_HT] real NULL,
    [Cruzamentos_Adversaria_HT] real NULL,
    [Desarmes_Adversaria_HT] real NULL,
    [Bolas_Afastadas_Adversaria_HT] real NULL,
    [Interceptacoes_Adversaria_HT] real NULL,
    [Gol_Confrontos_HT] real NULL,
    [GolSofrido_Confrontos_HT] real NULL,
    [Posse_Bola_Confrontos_HT] real NULL,
    [Total_Finalizacao_Confrontos_HT] real NULL,
    [Chances_Claras_Confrontos_HT] real NULL,
    [Escanteios_Confrontos_HT] real NULL,
    [Bolas_trave_Confrontos_HT] real NULL,
    [Gols_de_cabeça_Confrontos_HT] real NULL,
    [Defesas_Goleiro_Confrontos_HT] real NULL,
    [Impedimentos_Confrontos_HT] real NULL,
    [Faltas_Confrontos_HT] real NULL,
    [Cartoes_Amarelos_Confrontos_HT] real NULL,
    [Cartoes_Vermelhos_Confrontos_HT] real NULL,
    [Laterais_Cobrados_Confrontos_HT] real NULL,
    [Toque_Area_Adversaria_Confrontos_HT] real NULL,
    [Passes_Confrontos_HT] real NULL,
    [Passes_Totais_Confrontos_HT] real NULL,
    [Precisao_Passes_Confrontos_HT] real NULL,
    [Passes_terco_Final_Confrontos_HT] real NULL,
    [Cruzamentos_Confrontos_HT] real NULL,
    [Desarmes_Confrontos_HT] real NULL,
    [Bolas_Afastadas_Confrontos_HT] real NULL,
    [Interceptacoes_Confrontos_HT] real NULL,
    CONSTRAINT [PK_TB_ESTATISTICA_TIME] PRIMARY KEY ([Id])
);

CREATE TABLE [TB_METODOPALPITES] (
    [Id] int NOT NULL IDENTITY,
    [Nome] varchar(250) NULL,
    [Versao] varchar(250) NULL,
    [Descricao] varchar(250) NULL,
    [Condicoes] varchar(250) NULL,
    CONSTRAINT [PK_TB_METODOPALPITES] PRIMARY KEY ([Id])
);

CREATE TABLE [TB_PARTIDAS] (
    [id] int NOT NULL,
    [Id_EstatisticaCasa] int NOT NULL,
    [Id_EstatisticaFora] int NOT NULL,
    [NomeTimeCasa] varchar(250) NULL,
    [NomeTimeFora] varchar(250) NULL,
    [Url_Partida] varchar(250) NULL,
    [DataPartida] datetime2 NOT NULL,
    [Campeonato] varchar(250) NULL,
    [PartidaAnalise] bit NOT NULL,
    [TipoPartida] varchar(250) NULL,
    CONSTRAINT [PK_TB_PARTIDAS] PRIMARY KEY ([id])
);

CREATE TABLE [TB_ESTATISTICA_ESPERADAS] (
    [Id] int NOT NULL,
    [NomeTime] varchar(250) NULL,
    [FT_Id] int NOT NULL,
    [FTId] int NULL,
    [HT_Id] int NOT NULL,
    [HTId] int NULL,
    [FT_Adversario_Id] int NOT NULL,
    [FT_AdversarioId] int NULL,
    [HT_Adversario_Id] int NOT NULL,
    [HT_AdversarioId] int NULL,
    [FT_Confronto_Id] int NOT NULL,
    [FT_ConfrontoId] int NULL,
    [HT_Confronto_Id] int NOT NULL,
    [HT_ConfrontoId] int NULL,
    CONSTRAINT [PK_TB_ESTATISTICA_ESPERADAS] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TB_ESTATISTICA_ESPERADAS_TB_ESTATISTICA_BASEMODEL_FTId] FOREIGN KEY ([FTId]) REFERENCES [TB_ESTATISTICA_BASEMODEL] ([Id]),
    CONSTRAINT [FK_TB_ESTATISTICA_ESPERADAS_TB_ESTATISTICA_BASEMODEL_FT_AdversarioId] FOREIGN KEY ([FT_AdversarioId]) REFERENCES [TB_ESTATISTICA_BASEMODEL] ([Id]),
    CONSTRAINT [FK_TB_ESTATISTICA_ESPERADAS_TB_ESTATISTICA_BASEMODEL_FT_ConfrontoId] FOREIGN KEY ([FT_ConfrontoId]) REFERENCES [TB_ESTATISTICA_BASEMODEL] ([Id]),
    CONSTRAINT [FK_TB_ESTATISTICA_ESPERADAS_TB_ESTATISTICA_BASEMODEL_HTId] FOREIGN KEY ([HTId]) REFERENCES [TB_ESTATISTICA_BASEMODEL] ([Id]),
    CONSTRAINT [FK_TB_ESTATISTICA_ESPERADAS_TB_ESTATISTICA_BASEMODEL_HT_AdversarioId] FOREIGN KEY ([HT_AdversarioId]) REFERENCES [TB_ESTATISTICA_BASEMODEL] ([Id]),
    CONSTRAINT [FK_TB_ESTATISTICA_ESPERADAS_TB_ESTATISTICA_BASEMODEL_HT_ConfrontoId] FOREIGN KEY ([HT_ConfrontoId]) REFERENCES [TB_ESTATISTICA_BASEMODEL] ([Id])
);

CREATE TABLE [TB_PALPITES] (
    [Id] int NOT NULL,
    [IdPartida] int NOT NULL,
    [TipoAposta] int NOT NULL,
    [Num] float NOT NULL,
    [Descricao] varchar(510) NULL,
    [GreenRed] varchar(250) NULL,
    [ODD] real NULL,
    [DataPalpite] datetime2 NOT NULL,
    [MetodoGeradorPalpite_Id] int NOT NULL,
    CONSTRAINT [PK_TB_PALPITES] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TB_PALPITES_TB_METODOPALPITES_MetodoGeradorPalpite_Id] FOREIGN KEY ([MetodoGeradorPalpite_Id]) REFERENCES [TB_METODOPALPITES] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [TB_PARTIDA_ESTAITSTICA_ESPERADAS] (
    [Id] int NOT NULL,
    [Id_Partida] int NOT NULL,
    [PartidaId] int NULL,
    [Id_Estatisticas_Esperadas_Casa] int NOT NULL,
    [Estatisticas_Esperadas_CasaId] int NULL,
    [Id_Estatisticas_Esperadas_Fora] int NOT NULL,
    [Estatisticas_Esperadas_ForaId] int NULL,
    [Id_Partida_FT] int NOT NULL,
    [Partida_FTId] int NULL,
    [Id_Partida_HT] int NOT NULL,
    [Partida_HTId] int NULL,
    [Id_Partida_FT_Confronto] int NOT NULL,
    [Partida_FT_ConfrontoId] int NULL,
    [Id_Partida_HT_Confronto] int NOT NULL,
    [Partida_HT_ConfrontoId] int NULL,
    CONSTRAINT [PK_TB_PARTIDA_ESTAITSTICA_ESPERADAS] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_TB_ESTATISTICA_BASEMODEL_Partida_FTId] FOREIGN KEY ([Partida_FTId]) REFERENCES [TB_ESTATISTICA_BASEMODEL] ([Id]),
    CONSTRAINT [FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_TB_ESTATISTICA_BASEMODEL_Partida_FT_ConfrontoId] FOREIGN KEY ([Partida_FT_ConfrontoId]) REFERENCES [TB_ESTATISTICA_BASEMODEL] ([Id]),
    CONSTRAINT [FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_TB_ESTATISTICA_BASEMODEL_Partida_HTId] FOREIGN KEY ([Partida_HTId]) REFERENCES [TB_ESTATISTICA_BASEMODEL] ([Id]),
    CONSTRAINT [FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_TB_ESTATISTICA_BASEMODEL_Partida_HT_ConfrontoId] FOREIGN KEY ([Partida_HT_ConfrontoId]) REFERENCES [TB_ESTATISTICA_BASEMODEL] ([Id]),
    CONSTRAINT [FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_TB_ESTATISTICA_ESPERADAS_Estatisticas_Esperadas_CasaId] FOREIGN KEY ([Estatisticas_Esperadas_CasaId]) REFERENCES [TB_ESTATISTICA_ESPERADAS] ([Id]),
    CONSTRAINT [FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_TB_ESTATISTICA_ESPERADAS_Estatisticas_Esperadas_ForaId] FOREIGN KEY ([Estatisticas_Esperadas_ForaId]) REFERENCES [TB_ESTATISTICA_ESPERADAS] ([Id]),
    CONSTRAINT [FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_TB_PARTIDAS_PartidaId] FOREIGN KEY ([PartidaId]) REFERENCES [TB_PARTIDAS] ([id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Condicoes', N'Descricao', N'Nome', N'Versao') AND [object_id] = OBJECT_ID(N'[TB_METODOPALPITES]'))
    SET IDENTITY_INSERT [TB_METODOPALPITES] ON;
INSERT INTO [TB_METODOPALPITES] ([Id], [Condicoes], [Descricao], [Nome], [Versao])
VALUES (1, 'media de gols feitos casa mais media de gols sofridos fora /2  o mesmo para visitante', 'Para partidas que houver menos de 4 gols em tempo regulamentar', 'Under 4 gols', '1.0'),
(2, 'media de gols feitos casa mais media de gols sofridos fora /2  o mesmo para visitante', 'Para partidas que houver mais de 2 gols em tempo regulamentar', 'Over 2 gols', '1.0'),
(3, 'baseado em 4 características para definir quem tem a maior probabilidade de vencer, sendo eles posse de bola,Precisão dos passes,gols e jogos sem sofre gol', 'Para definir quem sera o vencedor da partida em termpo regulamentar', 'Vencedor do Encontro', '1.0'),
(4, 'baseado na media de escanteios feitos e  sofridos de cada time /4 ', 'Para definir a linha de over escanteios da Partida em termpo regulamentar', 'Over escanteios Variaveis', '1.0'),
(5, 'baseado na media de escanteios feitos e  sofridos de cada time /4 ', 'Para definir a linha de Under escanteios da Partida em termpo regulamentar', 'Under escanteios Variaveis', '1.0'),
(6, 'media de gols feitos casa mais media de gols sofridos fora /2  ', 'Para definir se um time faz um gol no adversario em termpo regulamentar', 'Over 0.5 Time', '1.0');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Condicoes', N'Descricao', N'Nome', N'Versao') AND [object_id] = OBJECT_ID(N'[TB_METODOPALPITES]'))
    SET IDENTITY_INSERT [TB_METODOPALPITES] OFF;

CREATE INDEX [IX_TB_ESTATISTICA_ESPERADAS_FT_AdversarioId] ON [TB_ESTATISTICA_ESPERADAS] ([FT_AdversarioId]);

CREATE INDEX [IX_TB_ESTATISTICA_ESPERADAS_FT_ConfrontoId] ON [TB_ESTATISTICA_ESPERADAS] ([FT_ConfrontoId]);

CREATE INDEX [IX_TB_ESTATISTICA_ESPERADAS_FTId] ON [TB_ESTATISTICA_ESPERADAS] ([FTId]);

CREATE INDEX [IX_TB_ESTATISTICA_ESPERADAS_HT_AdversarioId] ON [TB_ESTATISTICA_ESPERADAS] ([HT_AdversarioId]);

CREATE INDEX [IX_TB_ESTATISTICA_ESPERADAS_HT_ConfrontoId] ON [TB_ESTATISTICA_ESPERADAS] ([HT_ConfrontoId]);

CREATE INDEX [IX_TB_ESTATISTICA_ESPERADAS_HTId] ON [TB_ESTATISTICA_ESPERADAS] ([HTId]);

CREATE INDEX [IX_TB_PALPITES_MetodoGeradorPalpite_Id] ON [TB_PALPITES] ([MetodoGeradorPalpite_Id]);

CREATE INDEX [IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Estatisticas_Esperadas_CasaId] ON [TB_PARTIDA_ESTAITSTICA_ESPERADAS] ([Estatisticas_Esperadas_CasaId]);

CREATE INDEX [IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Estatisticas_Esperadas_ForaId] ON [TB_PARTIDA_ESTAITSTICA_ESPERADAS] ([Estatisticas_Esperadas_ForaId]);

CREATE INDEX [IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Partida_FT_ConfrontoId] ON [TB_PARTIDA_ESTAITSTICA_ESPERADAS] ([Partida_FT_ConfrontoId]);

CREATE INDEX [IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Partida_FTId] ON [TB_PARTIDA_ESTAITSTICA_ESPERADAS] ([Partida_FTId]);

CREATE INDEX [IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Partida_HT_ConfrontoId] ON [TB_PARTIDA_ESTAITSTICA_ESPERADAS] ([Partida_HT_ConfrontoId]);

CREATE INDEX [IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Partida_HTId] ON [TB_PARTIDA_ESTAITSTICA_ESPERADAS] ([Partida_HTId]);

CREATE INDEX [IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_PartidaId] ON [TB_PARTIDA_ESTAITSTICA_ESPERADAS] ([PartidaId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250630003726_migrationsdatacontext', N'9.0.6');

COMMIT;
GO

