using System.Threading.Tasks;
using ProAgil.Domain;
using ProAgil.Persistence.Models;

namespace ProAgil.Persistence.Contratos
{
    public interface IPalestrantePersist: IGeralPersist
    {

         Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
         Task<Palestrante> GetPalestranteByUserIdAsync(int UserId, bool includeEventos = false);

    }
}