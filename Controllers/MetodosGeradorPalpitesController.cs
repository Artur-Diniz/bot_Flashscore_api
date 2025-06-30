using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;
using System.Collections.Generic;
using System.Transactions;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MetodoGeradorPalpitesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly MLDbContext _MLDb;

        public MetodoGeradorPalpitesController(DataContext context, MLDbContext mLDb)
        {
            _context = context;
            _MLDb = mLDb;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getId(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("Id não pode ser igual a Zero");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                MetodoGeradorPalpites p = await _MLDb.TB_METODOPALPITES.FirstOrDefaultAsync(pa => pa.Id == id);
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
                List<MetodoGeradorPalpites> palpites = await _context
                .TB_METODOPALPITES.ToListAsync();
                if (palpites.Count() == 0)
                    return Ok($"Sem Palpites Para o Hoje");
                return Ok(palpites);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllComplete")]
        public async Task<IActionResult> GetAllComplete()
        {
            try
            {
                List<MetodoGeradorPalpites> palpites = await _MLDb
                .TB_METODOPALPITES.ToListAsync();
                if (palpites.Count() == 0)
                    return Ok($"Sem Palpites Para o Hoje");
                return Ok(palpites);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(MetodoGeradorPalpites mp)
        {
            try
            {
                await _MLDb.TB_METODOPALPITES.AddAsync(mp);
                await _MLDb.SaveChangesAsync();
                int idPrincipal = mp.Id;

                mp.Id = idPrincipal;
                _MLDb.Entry(mp).State = EntityState.Detached;
                _context.Entry(mp).State = EntityState.Added;
                await _context.SaveChangesAsync();

                int idSecundario = mp.Id;

                return Ok(new { PrimaryId = idPrincipal, SecondaryId = idSecundario });
            }
            catch (System.Exception ex)
            {
                try
                {
                    var inserted = await _MLDb.TB_METODOPALPITES.FindAsync(mp.Id);
                    if (inserted != null)
                    {
                        _MLDb.TB_METODOPALPITES.Remove(inserted);
                        await _MLDb.SaveChangesAsync();
                    }
                }
                catch (Exception rollbackEx)
                {
                    return BadRequest($"Erro ao inserir estatística: {ex.Message} | Erro ao desfazer no primeiro banco: {rollbackEx.Message}");
                }

                return BadRequest($"Erro ao inserir estatística: {ex.Message}. Inserção no primeiro banco foi revertida com sucesso.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, MetodoGeradorPalpites palpites)
        {
            try
            {
                if (id == 0)
                    return BadRequest("O Id não pode ser igual a zero");

                var palpitesML = await _MLDb.TB_METODOPALPITES
                              .FirstOrDefaultAsync(es => es.Id == id);

                var palpitesSecundarios = await _context.TB_METODOPALPITES
                    .FirstOrDefaultAsync(es => es.Id == id);

                if (palpitesML == null || palpitesSecundarios == null)
                    return NotFound("Estatística não encontrada em um dos bancos.");


                _MLDb.Entry(palpitesML).CurrentValues.SetValues(palpites);
                _context.Entry(palpitesSecundarios).CurrentValues.SetValues(palpites);

                palpitesML.Id = id;
                palpitesSecundarios.Id = id;
                await _MLDb.SaveChangesAsync();

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Tenta desfazer alteração no primeiro banco
                    _MLDb.Entry(palpitesML).State = EntityState.Unchanged;
                    return BadRequest($"Erro ao salvar no segundo banco: {ex.Message}");
                }

                return Ok(new { Message = "Atualização realizada com sucesso em ambos os bancos" });

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

                MetodoGeradorPalpites p = await _MLDb.TB_METODOPALPITES.FirstOrDefaultAsync(pa => pa.Id == id);
                MetodoGeradorPalpites p_repeat = await _context.TB_METODOPALPITES.FirstOrDefaultAsync(pa => pa.Id == id);

                if (p == null || p_repeat == null)
                    throw new System.Exception("Palpite Não Encontrada");


                _context.TB_METODOPALPITES.Remove(p_repeat);
                _MLDb.TB_METODOPALPITES.Remove(p);

                await _MLDb.SaveChangesAsync();
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