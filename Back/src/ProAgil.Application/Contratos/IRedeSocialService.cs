using System.Threading.Tasks;
using ProAgil.Application.Dtos;

namespace ProAgil.Persistence.Contratos
{
    public interface IRedeSocialService
    {
         Task<RedeSocialDto[]> SaveByEvento(int id, RedeSocialDto[] models);

         Task<bool> DeleteByEvento(int eventoId, int redeSocialId);

         Task<RedeSocialDto[]> SaveByPalestrante(int id, RedeSocialDto[] models);

         Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId);

         Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId);
         
         Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId);

         Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId);

         Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId);
    }
}