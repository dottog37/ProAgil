using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Application.Contratos;
using ProAgil.Application.Dtos;

using ProAgil.Persistence.Contextos;
using ProAgil.Persistence.Contratos;
using ProAgil.API.Extensions;

namespace ProAgil.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : ControllerBase
    {
        private readonly IRedeSocialService redeSocialService;
        private readonly IEventoService eventoService;
        private readonly IPalestranteService palestranteService;

        public RedesSociaisController(
                                    IRedeSocialService redeSocialService,
                                    IEventoService eventoService,
                                    IPalestranteService palestranteService)
        {
            this.eventoService = eventoService;
            this.palestranteService = palestranteService;
            this.redeSocialService = redeSocialService;


        }


        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId)
        {
            try
            {
                if (!(await AutorEvento(eventoId)))
                    return Unauthorized();
                var RedeSocial = await redeSocialService.GetAllByEventoIdAsync(eventoId);
                if (RedeSocial == null) return NoContent();


                return Ok(RedeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar Redes Sociais por evento. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            try
            {
                var palestrante = await palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null)
                    return Unauthorized();
                var RedeSocial = await redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
                if (RedeSocial == null) return NoContent();


                return Ok(RedeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar Redes Sociais do palestrante. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }


        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                if (!(await AutorEvento(eventoId)))
                    return Unauthorized();
                var redeSocial = await redeSocialService.SaveByEvento(eventoId, models);
                if (redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar salvar Rede Social por evento. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
        {
            try
            {
                var palestrante =  await palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null)
                    return Unauthorized();
                var redeSocial = await redeSocialService.SaveByPalestrante(palestrante.Id, models);
                if (redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar salvar Rede Social por palestrante. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                if (!(await AutorEvento(eventoId)))
                    return Unauthorized();

                var RedeSocial = await redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (RedeSocial == null)
                    return NoContent();

                if (await redeSocialService.DeleteByEvento(eventoId, RedeSocial.Id))
                {
                    return Ok(new { message = "Rede Social Deletada" });
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao tentar deletar Rede Social por evento");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar Rede Social por evento. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante =  await palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null)
                    return Unauthorized();

                var RedeSocial = await redeSocialService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, redeSocialId);
                if (RedeSocial == null)
                    return NoContent();

                if (await redeSocialService.DeleteByPalestrante(palestrante.Id, RedeSocial.Id))
                {
                    return Ok(new { message = "Rede Social Deletada" });
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao tentar deletar Rede Social por palestrante");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar Rede Social por palestrante. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId){
            var evento = await eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
            if (evento == null)
                return false;
            else
                return true;
        }
    }
}

