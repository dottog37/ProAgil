using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Persistence.Contratos
{
    public interface ILotePersist
    {
        /// <summary>
        /// Método get que retornará uma lista de lotes por eventoId
        /// </summary>
        /// <param name="eventoId">Código chave da tabela Eventos</param>
        /// <returns>Array de lotes</returns>
         Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
         /// <summary>
         /// Método get que retornará apenas um lote
         /// </summary>
         /// <param name="eventoId">Código chave da tabela Evento</param>
         /// <param name="loteId">Código chave da tabela Lote</param>
         /// <returns>Apenas 1 lote</returns>
         Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId);

    }
}