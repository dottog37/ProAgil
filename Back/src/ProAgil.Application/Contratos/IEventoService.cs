using System.Threading.Tasks;
using ProAgil.Application.Dtos;
using ProAgil.Persistence.Models;

namespace ProAgil.Application.Contratos
{
    public interface IEventoService
    {
         Task<EventoDto> AddEvento(int userId, EventoDto model);
         Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
         Task<bool> DeleteEvento(int userId, int eventoId);

         Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrate = false);
         Task<EventoDto> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrate = false);
    }
}