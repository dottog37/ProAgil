using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ProAgil.Application.Contratos;
using ProAgil.Application.Dtos;
using ProAgil.Domain;
using ProAgil.Persistence.Contratos;
using ProAgil.Persistence.Models;

namespace ProAgil.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantePersist _palestrantePersist;
        private readonly IMapper _mapper;
        public PalestranteService(
                             IPalestrantePersist palestrantePersist,
                             IMapper mapper)
        {
            this._palestrantePersist = palestrantePersist;
            this._mapper = mapper;

        }
        public async Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto model)
        {
            try
            {
                var Palestrante = _mapper.Map<Palestrante>(model);
                Palestrante.UserId = userId;
                _palestrantePersist.Add<Palestrante>(Palestrante);
                if (await _palestrantePersist.SaveChangesAsync()){
                    var PalestranteRetorno = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false);
                    return _mapper.Map<PalestranteDto>(PalestranteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {
            try
            {
                var Palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false);
                if (Palestrante==null) return null;
                
                model.Id = Palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, Palestrante);
                _palestrantePersist.Update<Palestrante>(Palestrante);
                if (await _palestrantePersist.SaveChangesAsync()){
                    var PalestranteRetorno = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false);
                    return _mapper.Map<PalestranteDto>(PalestranteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos)
        {
            try
            {
                var Palestrantes = await _palestrantePersist.GetAllPalestrantesAsync(pageParams, includeEventos);
                if (Palestrantes==null) return null;
                var resultado = _mapper.Map<PageList<PalestranteDto>>(Palestrantes);
                resultado.PageSize = Palestrantes.PageSize;
                resultado.CurrentPage = Palestrantes.CurrentPage;
                resultado.TotalCount = Palestrantes.TotalCount;
                resultado.TotalPages = Palestrantes.TotalPages;
                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }



        public async Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos)
        {
            try
            {
                var Palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, includeEventos);
                if (Palestrante==null) return null;

                var resultado = _mapper.Map<PalestranteDto>(Palestrante);
                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }


    }
}