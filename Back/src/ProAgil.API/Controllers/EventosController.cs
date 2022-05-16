using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.API.Extensions;
using ProAgil.API.Helpers;
using ProAgil.Application.Contratos;
using ProAgil.Application.Dtos;

using ProAgil.Persistence.Contextos;
using ProAgil.Persistence.Models;

namespace ProAgil.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService eventoService;
        private readonly IUtil util;
        private readonly IAccountService accountService;
        private readonly string _destino = "images";

        public EventosController(
                    IEventoService eventoService, 
                    IUtil util,
                    IAccountService accountService
                    )
        {
            this.util = util;
            this.accountService = accountService;
            this.eventoService = eventoService;



        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
        {
            try
            {
                var eventos = await eventoService.GetAllEventosAsync(User.GetUserId(), pageParams, true);
                
                if (eventos == null) return NoContent();

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar evento. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar evento. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await eventoService.AddEvento(User.GetUserId(), model);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar evento. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
                if (evento == null)
                    return NoContent();
                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    util.DeleteImage(evento.ImagemURL, _destino);
                    evento.ImagemURL = await util.SaveImage(file, _destino);
                }
                var eventoRetorno = await eventoService.UpdateEvento(User.GetUserId(), eventoId, evento);
                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar carregar foto do evento. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await eventoService.UpdateEvento(User.GetUserId(), id, model);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar evento. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();

                if (await eventoService.DeleteEvento(User.GetUserId(), id))
                {
                    util.DeleteImage(evento.ImagemURL, _destino);
                    return Ok(new { message = "Deletado" });
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao tentar deletar Evento");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar evento. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }
        
    }
}

