using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EstatisticaTimesController : ControllerBase
    {
        private readonly DataContext _context;

        public EstatisticaTimesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getId(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("Estatistica Não Encontrada");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Estatistica_Times e = await _context.TB_ESTATISTICA_TIME
                .FirstOrDefaultAsync(es => es.Id == id);
                if (e == null)
                    throw new System.Exception("Estatistica do Times Não Encontrada");

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
                List<Estatistica_Times> estatisticas = await _context
                .TB_ESTATISTICA_TIME.ToListAsync();

                return Ok(estatisticas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Estatistica_Times e)
        {
            try
            {
                await _context.TB_ESTATISTICA_TIME.AddAsync(e);
                await _context.SaveChangesAsync();

                return Ok(e.Id);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put(Estatistica_Times e)
        {
            try
            {
                _context.TB_ESTATISTICA_TIME.Update(e);
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

                Estatistica_Times e = await _context.TB_ESTATISTICA_TIME
                .FirstOrDefaultAsync(es => es.Id == id);
                if (e == null)
                    throw new System.Exception("Estatistica Não Encontrada");
                _context.TB_ESTATISTICA_TIME.Remove(e);


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