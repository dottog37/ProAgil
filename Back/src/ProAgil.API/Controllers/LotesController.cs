using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Application.Contratos;
using ProAgil.Application.Dtos;

using ProAgil.Persistence.Contextos;

namespace ProAgil.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService loteService;

        public LotesController(ILoteService LoteService)
        {
            this.loteService = LoteService;



        }


        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var lotes = await loteService.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return NoContent();
                
                                
                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar Lotes. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

      
        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
              try
            {
                var lotes = await loteService.SaveLotes(eventoId, models);
                if(lotes==null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar salvar Lotes. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await loteService.GetLoteByIdsAsync(eventoId, loteId);
                if(lote==null) return NoContent();
                
                if(await loteService.DeleteLote(lote.EventoId, lote.Id)){
                    return Ok(new { message = "Lote Deletado" });
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao tentar deletar Lote");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar Lotes. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}

