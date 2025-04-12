using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EstatisticaController : ControllerBase
    {
        private readonly DataContext _context;

        public EstatisticaController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetId(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("Estatistica Não Encontrada");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Estatistica e = await _context.TB_ESTATISTICA
                .FirstOrDefaultAsync(es => es.Id_Estatistica == id);
                if (e == null)
                    throw new System.Exception("Estatistica Não Encontrada");

                return Ok(e);
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
                List<Estatistica> estatisticas = await _context
                .TB_ESTATISTICA.ToListAsync();

                return Ok(estatisticas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Estatistica e)
        {
            try
            {
                await _context.TB_ESTATISTICA.AddAsync(e);
                await _context.SaveChangesAsync();

                return Ok(e.Id_Estatistica);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("Partida")]
        public async Task<IActionResult> Post(Partida_Estatistica_DTO dTO)
        {
            try{
                await _context.TB_ESTATISTICA.AddRangeAsync(dTO.EstatisticaCasa, dTO.EstatisticaFora);
                await _context.SaveChangesAsync();

                dTO.Partida.Id_EstatisticaCasa = dTO.EstatisticaCasa.Id_Estatistica;
                dTO.Partida.Id_EstatisticaFora = dTO.EstatisticaFora.Id_Estatistica;

                await _context.TB_PARTIDAS.AddAsync(dTO.Partida);
                await _context.SaveChangesAsync(); 

                dTO.EstatisticaCasa.Id_Partida = dTO.Partida.Id;
                dTO.EstatisticaFora.Id_Partida = dTO.Partida.Id;
                await _context.SaveChangesAsync();

                _context.TB_ESTATISTICA.UpdateRange(dTO.EstatisticaCasa, dTO.EstatisticaFora);
                _context.TB_PARTIDAS.Update(dTO.Partida);

                await _context.SaveChangesAsync();

                string mensagem = $"Estatísticas salvas - Casa: {dTO.EstatisticaCasa.Id_Estatistica}, " +
                                 $"Fora: {dTO.EstatisticaFora.Id_Estatistica}, " +
                                 $"Partida ID: {dTO.Partida.Id}";
                return Ok(mensagem);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put(Estatistica e)
        {
            try
            {
                _context.TB_ESTATISTICA.Update(e);
                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("O Id não pode ser igual a zero");

                Estatistica e = await _context.TB_ESTATISTICA
                .FirstOrDefaultAsync(es => es.Id_Estatistica == id);
                if (e == null)
                    throw new System.Exception("Estatistica Não Encontrada");
                _context.TB_ESTATISTICA.Remove(e);


                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}