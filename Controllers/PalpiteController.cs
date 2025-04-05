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
    }
}