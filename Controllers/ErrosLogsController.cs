using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ErrosLogsController : ControllerBase
    {
        private readonly DataContext _context;

        public ErrosLogsController(DataContext context)
        {
            _context = context;
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> getId(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("Log Não Encontrada");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                ErrosLogs e = await _context.TB_ERROSLOGS
                .FirstOrDefaultAsync(es => es.Id == id);
                if (e == null)
                    throw new System.Exception("Log Não Encontrada");

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
                List<ErrosLogs> erros = await _context
                .TB_ERROSLOGS.ToListAsync();

                return Ok(erros);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(ErrosLogs e)
        {
            try
            {
                await _context.TB_ERROSLOGS.AddAsync(e);
                await _context.SaveChangesAsync();

                return Ok(e.Id);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put(ErrosLogs e)
        {
            try
            {
                _context.TB_ERROSLOGS.Update(e);
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


                ErrosLogs e = await _context.TB_ERROSLOGS
                .FirstOrDefaultAsync(es => es.Id == id);
                if (e == null)
                    throw new System.Exception("Log Não Encontrada");
                _context.TB_ERROSLOGS.Remove(e);


                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Apague")]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                List<ErrosLogs> erros = await _context
                .TB_ERROSLOGS.ToListAsync();
                if (erros == null)
                    throw new System.Exception("Logs Não Encontrada");
                _context.TB_ERROSLOGS.RemoveRange(erros);


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