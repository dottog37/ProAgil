using System.Threading.Tasks;
using ProAgil.Domain;
using ProAgil.Persistence.Models;

namespace ProAgil.Persistence.Contratos
{
    public interface IEventoPersist
    {

         Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrate = false );
         Task<Evento> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrate = false);

    }
}