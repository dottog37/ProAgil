using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProAgil.Application.Contratos;
using ProAgil.Application.Dtos;
using ProAgil.Domain;
using ProAgil.Persistence.Contratos;

namespace ProAgil.Application
{
    public class LoteService : ILoteService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly ILotePersist _lotePersist;
        private readonly IMapper _mapper;
        public LoteService(IGeralPersist geralPersist,
                             ILotePersist lotePersist,
                             IMapper mapper)
        {
            this._lotePersist = lotePersist;
            this._geralPersist = geralPersist;
            this._mapper = mapper;

        }

        public async Task AddLotes(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;
                _geralPersist.Add<Lote>(lote);
                await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                if (lotes==null) return null;
                foreach (var model in models)
                {
                    if (model.Id == 0){ //insert
                        await AddLotes(eventoId, model);
                    }
                    else //update
                    {
                        var lote = lotes.FirstOrDefault(l => l.Id ==  model.Id);
                        model.EventoId = eventoId;
                        _mapper.Map(model, lote);
                        _geralPersist.Update<Lote>(lote);
                        await _geralPersist.SaveChangesAsync();
                    }
               
                    var loteRetorno = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                    return _mapper.Map<LoteDto[]>(loteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);
                if (lote==null) throw new Exception("Lote para delete não encontrado");
                
                _geralPersist.Delete<Lote>(lote);
                return await _geralPersist.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

 

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                if (lotes==null) return null;
                
                var resultado = _mapper.Map<LoteDto[]>(lotes);
                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);
                if (lote==null) return null;

                var resultado = _mapper.Map<LoteDto>(lote);
                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }


    }
}