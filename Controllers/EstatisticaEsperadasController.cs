using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EstatisticaEsperadasController : ControllerBase
    {
        private readonly DataContext _context;

        public EstatisticaEsperadasController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("id")]
        public async Task<IActionResult> getId(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("Estatistica Não Encontrada");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Estatistica_Esperadas e = await _context.TB_ESTATISTICA_ESPERADAS
                .FirstOrDefaultAsync(es => es.Id == id);
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
                List<Estatistica_Esperadas> estatisticas = await _context
                .TB_ESTATISTICA_ESPERADAS.ToListAsync();

                return Ok(estatisticas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Estatistica_Esperadas e)
        {
            try
            {
                await _context.TB_ESTATISTICA_ESPERADAS.AddAsync(e);
                await _context.SaveChangesAsync();

                return Ok(e.Id);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put(Estatistica_Esperadas e)
        {
            try
            {
                _context.TB_ESTATISTICA_ESPERADAS.Update(e);
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

                Estatistica_Esperadas e = await _context.TB_ESTATISTICA_ESPERADAS
                .FirstOrDefaultAsync(es => es.Id == id);

                if (e == null)
                    throw new System.Exception("Estatistica Não Encontrada");
                _context.TB_ESTATISTICA_ESPERADAS.Remove(e);


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