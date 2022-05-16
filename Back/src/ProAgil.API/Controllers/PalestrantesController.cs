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
using ProAgil.Application.Contratos;
using ProAgil.Application.Dtos;

using ProAgil.Persistence.Contextos;
using ProAgil.Persistence.Models;

namespace ProAgil.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController : ControllerBase
    {
        private readonly IPalestranteService palestranteService;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IAccountService accountService;

        public PalestrantesController(
                    IPalestranteService palestranteService, 
                    IWebHostEnvironment hostEnvironment,
                    IAccountService accountService
                    )
        {
            this.hostEnvironment = hostEnvironment;
            this.accountService = accountService;
            this.palestranteService = palestranteService;



        }


        [HttpGet("all")]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
        {
            try
            {
                var palestrantes = await palestranteService.GetAllPalestrantesAsync(pageParams, true);
                
                if (palestrantes == null) return NoContent();

                Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPages);
                return Ok(palestrantes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar palestrantes. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetPalestrantes(int id)
        {
            try
            {
                var palestrante = await palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(),true);
                if (palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar palestrantes. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto model)
        {
            try
            {
                var palestrante = await palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);
                if (palestrante ==null)
                    palestrante = await palestranteService.AddPalestrante(User.GetUserId(), model);
                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar Palestrante. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        
        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto model)
        {
            try
            {
                var Palestrante = await palestranteService.UpdatePalestrante(User.GetUserId(), model);
                if (Palestrante == null) return NoContent();

                return Ok(Palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar Palestrante. Erro{ex.Message}");
                throw new Exception(ex.Message);
            }
        }


        
    }
}

