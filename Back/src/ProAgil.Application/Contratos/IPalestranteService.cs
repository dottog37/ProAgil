using System.Threading.Tasks;
using ProAgil.Application.Dtos;
using ProAgil.Persistence.Models;

namespace ProAgil.Application.Contratos
{
    public interface IPalestranteService
    {
         Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto model);
         Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model);

         Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
         Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);
    }
}