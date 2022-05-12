using System.Threading.Tasks;
using ProAgil.Application.Dtos;

namespace ProAgil.Application.Contratos
{
    public interface ILoteService
    {

         Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models);
         Task<bool> DeleteLote(int eventoId, int loteId);

   
         Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);
         Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}