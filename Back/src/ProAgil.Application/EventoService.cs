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
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersist _eventoPersist;
        private readonly IMapper _mapper;
        public EventoService(IGeralPersist geralPersist,
                             IEventoPersist eventoPersist,
                             IMapper mapper)
        {
            this._eventoPersist = eventoPersist;
            this._geralPersist = geralPersist;
            this._mapper = mapper;

        }
        public async Task<EventoDto> AddEvento(int userId, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;
                _geralPersist.Add<Evento>(evento);
                if (await _geralPersist.SaveChangesAsync()){
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(userId, evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, false);
                if (evento==null) return null;
                
                model.Id = evento.Id;
                model.UserId = userId;
                _mapper.Map(model, evento);
                _geralPersist.Update<Evento>(evento);
                if (await _geralPersist.SaveChangesAsync()){
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(userId, evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, false);
                if (evento==null) throw new Exception("Evento para delete não encontrado");
                
                _geralPersist.Delete<Evento>(evento);
                return await _geralPersist.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrate)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(userId, pageParams, includePalestrate);
                if (eventos==null) return null;
                var resultado = _mapper.Map<PageList<EventoDto>>(eventos);
                resultado.PageSize = eventos.PageSize;
                resultado.CurrentPage = eventos.CurrentPage;
                resultado.TotalCount = eventos.TotalCount;
                resultado.TotalPages = eventos.TotalPages;
                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }



        public async Task<EventoDto> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrate)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(userId, EventoId, includePalestrate);
                if (evento==null) return null;

                var resultado = _mapper.Map<EventoDto>(evento);
                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }


    }
}