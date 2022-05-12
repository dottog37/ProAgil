using System.Threading.Tasks;
using ProAgil.Application.Dtos;

namespace ProAgil.Application.Contratos
{
    public interface IEventoService
    {
         Task<EventoDto> AddEvento(int userId, EventoDto model);
         Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
         Task<bool> DeleteEvento(int userId, int eventoId);

         Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrate = false);
         Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrate = false);
         Task<EventoDto> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrate = false);
    }
}