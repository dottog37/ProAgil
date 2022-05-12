using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Persistence.Contratos
{
    public interface IPalestrantePersist
    {
         Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos);
         Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos);
         Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos);

    }
}