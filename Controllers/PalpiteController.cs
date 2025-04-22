using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PalpiteController : ControllerBase
    {
        private readonly DataContext _context;

        public PalpiteController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getId(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("Id não pode ser igual a Zero");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Palpites p = await _context.TB_PALPITES.FirstOrDefaultAsync(pa => pa.Id == id);
                if (p == null)
                    throw new System.Exception("Palpite Não Encontrada");

                return Ok(p);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Palpites> palpites = await _context
                .TB_PALPITES.ToListAsync();

                return Ok(palpites);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Palpites p)
        {
            try
            {
                await _context.TB_PALPITES.AddAsync(p);
                await _context.SaveChangesAsync();

                return Ok(p.Id);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Gerar_Palpites")]
        public async Task<IActionResult> GerarPalpites()
        {
            try
            {
                List<Partida> partidas = await _context
                .TB_PARTIDAS.Where(p => p.PartidaAnalise == true).ToListAsync();
                if (partidas.Count() == 0)
                    throw new System.Exception("Sem Partidas Para Analisar Hoje");


                List<Palpites> palpites = await MetodosPalpites(partidas);

                if (palpites.Count() > 0)
                    return Ok($"FOI hoje tem {palpites.Count()} palpites, Bora Forrar!!!");

                return Ok("Não há Palpites para o dia de Hoje");

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Palpites palpites)
        {
            try
            {
                _context.TB_PALPITES.Update(palpites);
                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> delte(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("Id não pode ser igual a Zero");

                Palpites p = await _context.TB_PALPITES.FirstOrDefaultAsync(pa => pa.Id == id);
                if (p == null)
                    throw new System.Exception("Palpite Não Encontrada");

                _context.TB_PALPITES.Remove(p);
                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private async Task<List<Palpites>> MetodosPalpites(List<Partida> partidas)
        {
            List<Palpites> palpites = new List<Palpites>();
            List<Palpites> apostas = new List<Palpites>();

            foreach (var item in partidas)
            {
                palpites = await Palpitesgenerate(item);
                apostas.AddRange(palpites);
            }
            return apostas;
        }


        private async Task<List<Palpites>> Palpitesgenerate(Partida partida)
        {
            List<Palpites> palpites = new List<Palpites>();

            Estatistica_Times casa = await _context.TB_ESTATISTICA_TIME.FirstOrDefaultAsync(c => c.NomeTime == partida.NomeTimeCasa);
            Estatistica_Times fora = await _context.TB_ESTATISTICA_TIME.FirstOrDefaultAsync(c => c.NomeTime == partida.NomeTimeFora);

            List<Partida> Partidas_casa = await _context.TB_PARTIDAS.Where(e => e.NomeTimeCasa == casa.NomeTime && e.TipoPartida == "Casa").ToListAsync();
            List<Partida> Partidas_fora = await _context.TB_PARTIDAS.Where(e => e.NomeTimeFora == fora.NomeTime && e.TipoPartida == "Fora").ToListAsync();


            if (partida.Campeonato != "Eurocopa" && partida.Campeonato != "Copa America"
                    && partida.Campeonato != "Copa Sul-Americana" && partida.Campeonato != "Copa Libertadores"
                     && partida.Campeonato != "Copa do Mundo" && partida.Campeonato != "Liga dos Campeoes")
            {
                Palpites under4 = await MetodoUnder4(casa, fora, Partidas_casa, Partidas_fora, partida.Id);
                if (under4.Descricao != "")
                    palpites.Add(under4);
                Palpites Over2 = await MetodoOver2(casa, fora, Partidas_casa, Partidas_fora, partida.Id);
                if (Over2.Descricao != "")
                    palpites.Add(Over2);
                Palpites Winner = await MetodoWinner(casa, fora, Partidas_casa, Partidas_fora, partida.Id);
                if (Winner.Descricao != "")
                    palpites.Add(Winner);
                Palpites OverCantos = await OverCantosVariaveis(casa, fora, Partidas_casa, Partidas_fora, partida.Id);
                if (OverCantos.Descricao != "")
                    palpites.Add(OverCantos);
                Palpites UnderCantos = await UnderCantosVariaveis(casa, fora, Partidas_casa, Partidas_fora, partida.Id);
                if (UnderCantos.Descricao != "")
                    palpites.Add(UnderCantos);
                Palpites Golteam = await GolTime(casa, fora, Partidas_casa, Partidas_fora, partida.Id);
                if (Golteam.Descricao != "")
                    palpites.Add(Golteam);


            }

            if (palpites.Count() != 0)
            {
                await _context.TB_PALPITES.AddRangeAsync(palpites);
                await _context.SaveChangesAsync();
            }

            return palpites;
        }

        private async Task<Palpites> MetodoUnder4(Estatistica_Times c, Estatistica_Times f, List<Partida> casa, List<Partida> fora, int IdPartida)
        {
            Palpites palpite = new Palpites();

            if (c == null || f == null)
            {
                return palpite;
            }


            if (casa.Count < 3 && fora.Count < 3)
            {
                return palpite;
            }

            int numOverGols4Home = 0;
            foreach (var item in casa)
            {
                Estatistica home = await _context.TB_ESTATISTICA
                      .FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaCasa);
                int mais4 = (int)(home.Gol + home.GolSofrido);

                if (mais4 >= 4)
                    numOverGols4Home = +1;
            }
            int numOverGols4Fora = 0;
            foreach (var item in fora)
            {
                Estatistica Fora = await _context.TB_ESTATISTICA
                      .FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaFora);
                int mais4 = (int)(Fora.Gol + Fora.GolSofrido);

                if (mais4 >= 4)
                    numOverGols4Fora = +1;
            }

            if (numOverGols4Fora > 1 || numOverGols4Home > 1)
                return palpite;



            if (c.Gol <= 2.4 && f.Gol <= 2.4 && c.GolSofrido <= 2.0 && f.GolSofrido <= 2)
            {
                int medidor = 0;
                if (c.Gol > 1.6)
                    medidor += 1;
                if (f.Gol > 1.6)
                    medidor += 1;
                if (c.GolSofrido > 1.2)
                    medidor += 1;
                if (f.GolSofrido > 1.2)
                    medidor += 1;

                if (medidor > 1)
                    return palpite;
            }
            float mediaGolsEsperados = (float)(c.Gol + f.Gol + c.GolSofrido + f.GolSofrido);


            if (mediaGolsEsperados <= 4.2)
            {
                palpite.IdPartida = IdPartida;
                palpite.TipoAposta = TipoAposta.Gols;
                palpite.Num = 3.5;

                palpite.Descricao = $"Menos de 3.5 Gols para Essa Partida por Representarem" +
                $" uma espectativa de Gols abaixo de 4 gols Esperadas para esse Partida entre {c.NomeTime} e {f.NomeTime} ";
            }


            return palpite;
        }

        private async Task<Palpites> MetodoOver2(Estatistica_Times c, Estatistica_Times f, List<Partida> casa, List<Partida> fora, int IdPartida)
        {
            Palpites palpite = new Palpites();

            if (c == null || f == null)
                return palpite;


            int numOverGols4Home = 0;
            foreach (var item in casa)
            {
                Estatistica home = await _context.TB_ESTATISTICA
                      .FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaCasa);
                int mais4 = (int)(home.Gol + home.GolSofrido);

                if (mais4 >= 2)
                    numOverGols4Home = +1;
            }
            int numOverGols4Fora = 0;
            foreach (var item in fora)
            {
                Estatistica home = await _context.TB_ESTATISTICA
                      .FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaFora);
                int mais4 = (int)(home.Gol + home.GolSofrido);

                if (mais4 >= 2)
                    numOverGols4Fora = +1;
            }
            if (numOverGols4Fora < 1 || numOverGols4Home < 1)
            { return palpite; }

            float mediacasa = (float)(c.Gol + c.GolSofrido);
            float mediafora = (float)(f.Gol + f.GolSofrido);

            if (mediacasa < 1.6 && mediafora < 1.6)
            {
                return palpite;
            }

            float mediaAtaque = (float)(c.Gol + f.Gol);
            if (c.Gol < 0.8 || f.Gol < 0.8)
                return palpite;

            if (mediaAtaque < 2.2)
                return palpite;

            float mediaGolsEsperados = mediacasa + mediafora;
            if (mediaGolsEsperados >= 3.8)
            {
                palpite.IdPartida = IdPartida;
                palpite.TipoAposta = TipoAposta.Gols;
                palpite.Num = 1.5;

                palpite.Descricao = $"Mais de 1.5 Gols para Essa Partida por Representarem" +
                $" Constante  Media de Gols Esperadas para esse Partida entre {c.NomeTime} e {f.NomeTime} ";
            }

            return palpite;
        }


        private async Task<Palpites> OverCantosVariaveis(Estatistica_Times c, Estatistica_Times f, List<Partida> casa, List<Partida> fora, int IdPartida)
        {

            Palpites palpite = new Palpites();

            if (c == null || f == null)
                return palpite;

            if (casa.Count < 3 || fora.Count < 3)
                return palpite;

            float espectaticaCasa = (float)(c.Escanteios + f.Escanteios_Adversaria) / 2;
            float espectaticaFora = (float)(f.Escanteios + c.Escanteios_Adversaria) / 2;

            float espectativaCantos = (float)(espectaticaCasa + espectaticaFora) / 2;
            bool escanteios = false;
            float cantosEsperados = (float)Math.Floor(espectativaCantos * 0.7);

            if (espectativaCantos >= 10)
            {
                List<Partida> confrontos = await _context.TB_PARTIDAS.Where(e => e.NomeTimeCasa == c.NomeTime
            && e.TipoPartida == "ConfrontoDireto" && e.NomeTimeFora == f.NomeTime || e.NomeTimeCasa == f.NomeTime
            && e.TipoPartida == "ConfrontoDireto" && e.NomeTimeFora == c.NomeTime).ToListAsync();

                int OverCantos = 0;
                if (confrontos.Count() > 2)
                {
                    foreach (var item in confrontos)
                    {
                        Estatistica home = await _context.TB_ESTATISTICA.FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaCasa);

                        Estatistica adversario = await _context.TB_ESTATISTICA.FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaFora);

                        int CastosTotal = (int)home.Escanteios + (int)adversario.Escanteios;

                        if (CastosTotal >= cantosEsperados + 1)
                            OverCantos++;
                    }
                }
                if (OverCantos > 2 && confrontos.Count == 3)
                    escanteios = true;
                if (OverCantos > 2 && confrontos.Count == 4)
                    escanteios = true;
                if (OverCantos > 3 && confrontos.Count == 5)
                    escanteios = true;
            }
            if (escanteios)
            {
                palpite.TipoAposta = TipoAposta.Escanteios;
                palpite.Num = cantosEsperados - 0.5;

                palpite.Descricao = $"over {cantosEsperados - 0.5}" +
                $" os confronstos entre {c.NomeTime} e {f.NomeTime} apresentaram Alta media de escanteios " +
                $" alem de apresentarem uma media alta em suas partidas";
            }

            return palpite;
        }


        private async Task<Palpites> UnderCantosVariaveis(Estatistica_Times c, Estatistica_Times f, List<Partida> casa, List<Partida> fora, int IdPartida)
        {
            Palpites palpite = new Palpites();

            if (c == null || f == null)
                return palpite;

            if (casa.Count < 3 || fora.Count < 3)
                return palpite;

            float espectaticaCasa = (float)(c.Escanteios + f.Escanteios_Adversaria) / 2;
            float espectaticaFora = (float)(f.Escanteios + c.Escanteios_Adversaria) / 2;

            float espectativaCantos = (float)(espectaticaCasa + espectaticaFora) / 2;
            bool escanteios = false;
            float cantosEsperados = (float)Math.Ceiling(espectativaCantos * 1.3);

            if (espectativaCantos <= 7)
            {
                List<Partida> confrontos = await _context.TB_PARTIDAS.Where(e => e.NomeTimeCasa == c.NomeTime
            && e.TipoPartida == "ConfrontoDireto" && e.NomeTimeFora == f.NomeTime || e.NomeTimeCasa == f.NomeTime
            && e.TipoPartida == "ConfrontoDireto" && e.NomeTimeFora == c.NomeTime).ToListAsync();

                int UnderCantos = 0;
                if (confrontos.Count() > 2)
                {
                    foreach (var item in confrontos)
                    {
                        Estatistica home = await _context.TB_ESTATISTICA.FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaCasa);

                        Estatistica adversario = await _context.TB_ESTATISTICA.FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaFora);

                        int CastosTotal = (int)(home.Escanteios + adversario.Escanteios);

                        if (CastosTotal <= cantosEsperados - 1)
                            UnderCantos++;
                    }
                }
                if (UnderCantos > 2 && confrontos.Count == 3)
                    escanteios = true;
                if (UnderCantos > 2 && confrontos.Count == 4)
                    escanteios = true;
                if (UnderCantos > 3 && confrontos.Count == 5)
                    escanteios = true;
            }
            if (escanteios)
            {
                palpite.TipoAposta = TipoAposta.Escanteios;
                palpite.IdPartida = IdPartida;
                palpite.Num = cantosEsperados + 0.5;

                palpite.Descricao = $"Under {cantosEsperados + 0.5}" +
                $" os confronstos entre {c.NomeTime} e {f.NomeTime} apresentaram Baixa media de escanteios " +
                $" alem de apresentarem uma media alta em suas partidas";
            }
            return palpite;

        }


        private async Task<Palpites> GolTime(Estatistica_Times c, Estatistica_Times f, List<Partida> casa, List<Partida> fora, int IdPartida)
        {
            Palpites palpite = new Palpites();

            if (c == null || f == null)
                return palpite;

            if (casa.Count < 3 || fora.Count < 3)
                return palpite;

            bool CasaMarca = false;
            bool ForaMarca = false;

            int golcasa = 0, golsofridoCasa = 0;
            foreach (var item in casa)
            {
                Estatistica home = await _context.TB_ESTATISTICA
                      .FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaCasa);
                if (home.Gol > 0)
                    golcasa++;

                if (home.GolSofrido > 0)
                    golsofridoCasa++;
            }
            int golfora = 0, golssofridosfora = 0;
            foreach (var item in fora)
            {
                Estatistica home = await _context.TB_ESTATISTICA
                      .FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaFora);
                if (home.Gol > 0)
                    golfora++;

                if (home.GolSofrido > 0)
                    golssofridosfora++;
            }

            if (golcasa > 3 && golssofridosfora > 2)
                if (c.Gol > 1.15)
                    CasaMarca = true;
            if (golfora > 3 && golsofridoCasa > 2)
                if (f.Gol > 1.2)
                    ForaMarca = true;

            if (CasaMarca && ForaMarca)
            {
                palpite.IdPartida = IdPartida;

                palpite.TipoAposta = TipoAposta.Gols;
                palpite.Num = 2;

                palpite.Descricao = $"Ambas Marcam pela" +
                $" Constante  Media de Gols Esperadas para esse Partida entre {c.NomeTime} e {f.NomeTime} ";
            }
            else
           if (CasaMarca)
            {
                palpite.IdPartida = IdPartida;

                palpite.TipoAposta = TipoAposta.Gols;
                palpite.Num = 0.5;

                palpite.Descricao = $"Casa Marca pela" +
                $" Constante  Media de Gols Em Casa  na Partida entre {c.NomeTime} e {f.NomeTime} ";
            }
            else
           if (ForaMarca)
            {
                palpite.IdPartida = IdPartida;

                palpite.TipoAposta = TipoAposta.Gols;
                palpite.Num = 0.5;

                palpite.Descricao = $"Fora Marca pela" +
                $" Constante  Media de Gols Jogando Fora na Partida entre {c.NomeTime} e {f.NomeTime} ";
            }


            return palpite;
        }

        private async Task<Palpites> MetodoWinner(Estatistica_Times c, Estatistica_Times f, List<Partida> casa, List<Partida> fora, int IdPartida)
        {

            Palpites palpite = new Palpites();

            if (c == null || f == null)
                return palpite;

            if (casa.Count < 3 || fora.Count < 3)
                return palpite;

            int golsSemGolCasa = 0, aprovetamentocasaWin = 0, vitoraempatecasa = 0;
            foreach (var item in casa)
            {
                Estatistica ca = await _context.TB_ESTATISTICA.FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaCasa);

                if (ca.GolSofrido < 0.5)
                    golsSemGolCasa++;

                if (ca.Gol > ca.GolSofrido)
                {
                    aprovetamentocasaWin++;
                    vitoraempatecasa++;
                }
                else if (ca.Gol >= ca.GolSofrido)
                    vitoraempatecasa++;
            }

            double porcentegemWinRateCasa = Porcentagemcount(aprovetamentocasaWin, casa.Count);
            double vitoraempatecasaDWRateCasa = Porcentagemcount(vitoraempatecasa, casa.Count);
            double porcentagemCasa = Porcentagemcount(golsSemGolCasa, casa.Count);


            float posseCasa = (float)c.Posse_Bola / 100;

            float chancedeVitoriaCasa = (float)(posseCasa * 0.25 + c.Precisao_Passes * 0.25 + c.Gol * 0.25 + porcentagemCasa * 0.25);

            int golsSemGolFora = 0, aproveitamentoWinfora = 0, vitoriaemaptefora = 0;

            foreach (var item in casa)
            {
                Estatistica ca = await _context.TB_ESTATISTICA.FirstOrDefaultAsync(e => e.Id_Estatistica == item.Id_EstatisticaFora);

                if (ca.GolSofrido < 0.5)
                    golsSemGolFora++;

                if (ca.Gol > ca.GolSofrido)
                {
                    aproveitamentoWinfora++;
                    vitoriaemaptefora++;
                }
                else if (ca.Gol >= ca.GolSofrido)
                    vitoriaemaptefora++;
            }

            double porcentegemWinRateFora = Porcentagemcount(aproveitamentoWinfora, casa.Count);
            double vitoraempatecasaDWRateFora = Porcentagemcount(vitoriaemaptefora, casa.Count);
            double porcentagemFora = Porcentagemcount(golsSemGolFora, fora.Count);




            float possefora = (float)c.Posse_Bola / 100;

            float chancedeVitoriafora = (float)(possefora * 0.25 + c.Precisao_Passes * 0.25 + c.Gol * 0.25 + porcentagemFora * 0.25);

            float Partida = chancedeVitoriafora + chancedeVitoriaCasa;

            float VitoriaCasa = chancedeVitoriaCasa / Partida * 100;
            float VitoriaFora = chancedeVitoriafora / Partida * 100;

            float DiferençaWin = VitoriaCasa - VitoriaFora;

            if (DiferençaWin > 10 && vitoraempatecasaDWRateCasa >= 0.70)
            {
                if (DiferençaWin < 24.99 && aprovetamentocasaWin >= 0.60)
                {
                    palpite.TipoAposta = TipoAposta.CasaVence;
                    palpite.Num = 0;
                    palpite.IdPartida = IdPartida;
                    palpite.Descricao = $"Casa Vitoria Empate" +
                   $" o Mandante {c.NomeTime} tem se mostrar um adversário melhor em comparação ao {f.NomeTime}, tendo tendencia Maior de vitoria o Mandante";

                }
                else
                {
                    palpite.TipoAposta = TipoAposta.CasaVence;
                    palpite.Num = 0;
                    palpite.IdPartida = IdPartida;
                    palpite.Descricao = $"Casa Vitoria " +
                   $"o Mandante {c.NomeTime} tem se mostrar um adversário Superior em comparação ao {f.NomeTime}, tendo tendencia Maior de vitoria o Mandante";

                }

            }
            if (DiferençaWin < -10 && vitoraempatecasaDWRateFora >= 0.70)
            {
                if (DiferençaWin > -24.99 && aproveitamentoWinfora >= 0.60)
                {
                    palpite.TipoAposta = TipoAposta.ForaVence;
                    palpite.Num = 0;
                    palpite.IdPartida = IdPartida;
                    palpite.Descricao = $"Fora Vitoria Empate " +
                   $"o Mandante {c.NomeTime} tem se mostrar um adversário Fraco em comparação ao {f.NomeTime}, tendo tendencia Maior de vitoria  o Visitante";

                }
                else
                {
                    palpite.TipoAposta = TipoAposta.ForaVence;
                    palpite.Num = 0;
                    palpite.IdPartida = IdPartida;
                    palpite.Descricao = $"Fora Vitoria " +
                   $"o Mandante {c.NomeTime} tem se mostrar um adversário Fraco em comparação ao {f.NomeTime}, tendo tendencia Maior de vitoria o Visitante";

                }
            }
            return palpite;

        }




        private static double Porcentagemcount(int ocorrencia, int total)
        {
            double input = 0;
            if (total == 5)
            {
                if (ocorrencia == 5)
                    input = 1;
                else if (ocorrencia == 4)
                    input = 0.8;
                else if (ocorrencia == 3)
                    input = 0.6;
                else if (ocorrencia == 2)
                    input = 0.4;
                else if (ocorrencia == 1)
                    input = 0.2;
            }
            else if (total == 4)
            {
                if (ocorrencia == 4)
                    input = 1;
                else if (ocorrencia == 3)
                    input = 0.75;
                else if (ocorrencia == 2)
                    input = 0.5;
                else if (ocorrencia == 1)
                    input = 0.25;
            }
            else if (total == 3)
            {
                if (ocorrencia == 3)
                    input = 1;
                else if (ocorrencia == 2)
                    input = 0.66;
                else if (ocorrencia == 1)
                    input = 0.33;
            }

            return input;
        }
    }
}