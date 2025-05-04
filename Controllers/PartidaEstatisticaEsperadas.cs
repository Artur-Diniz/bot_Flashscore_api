using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PartidaEstatisticaEsperadasController : ControllerBase
    {
        private readonly DataContext _context;

        public PartidaEstatisticaEsperadasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getsingle(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("O Id não pode ser igual a zero");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Partida_Estatistica_Esperadas p = await _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS
                .FirstOrDefaultAsync(pa => pa.Id == id);
                if (p == null)
                    throw new System.Exception("Partida  Estatistica Esperdas Não Encontrada");
                return Ok(p);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Partida_Estatistica_Esperadas> partidas = await _context
                .TB_PARTIDA_ESTAITSTICA_ESPERADAS.ToListAsync();
                if (partidas == null)
                    throw new System.Exception("Partida Não Encontrada");

                return Ok(partidas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(Partida_Estatistica_Esperadas partida)
        {
            try
            {
                await _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS.AddAsync(partida);
                await _context.SaveChangesAsync();

                return Ok(partida.Id);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("GerarEstatisticasEsperadas/{IdPartida}")]
        public async Task<IActionResult> GerarPartidasestatistcasEsperadas(Partida_Estatistica_Esperadas partida)
        {
            try
            {
                await _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS.AddAsync(partida);
                await _context.SaveChangesAsync();

                return Ok(partida.Id);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Partida_Estatistica_Esperadas p)
        {
            try
            {
                _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS.Update(p);
                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delte(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("O Id não pode ser igual a zero");

                Partida_Estatistica_Esperadas p = await _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS
              .FirstOrDefaultAsync(pa => pa.Id == id);

                if (p == null)
                    throw new System.Exception("Partida Não Encontrada");
                _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS.Remove(p);


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