using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Persistence.Contratos
{
    public interface IEventoPersist
    {

         Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrate = false);
         Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrate = false );
         Task<Evento> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrate = false);

    }
}