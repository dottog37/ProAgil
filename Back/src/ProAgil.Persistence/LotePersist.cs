using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Persistence.Contextos;

namespace ProAgil.Persistence.Contratos
{
    public class LotePersist : ILotePersist
    {
        private readonly ProAgilContext _context;

        public LotePersist(ProAgilContext context)
        {
            _context = context;

        }

        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking().Where(l => l.EventoId == eventoId && l.Id == loteId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking().Where(l => l.EventoId == eventoId);
            return await query.ToArrayAsync();
        }
    }
}