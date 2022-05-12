using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Persistence.Contextos;

namespace ProAgil.Persistence.Contratos
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly ProAgilContext _context;

        public PalestrantePersist(ProAgilContext context)
        {
            _context = context;

        }
        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(p=>p.RedesSociais);

            if(includeEventos){
                query = query.Include(p=>p.PalestranteEventos).ThenInclude(pe=>pe.Evento);
            }

            query=query.AsNoTracking().OrderBy(p=>p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(p=>p.RedesSociais);

            if(includeEventos){
                query = query.Include(p=>p.PalestranteEventos).ThenInclude(pe=>pe.Evento);
            }

            query=query.AsNoTracking().OrderBy(p=>p.Id).Where(p=>p.User.PrimeiroNome.ToLower().Contains(nome.ToLower()) &&
             p.User.UltimoNome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }


        public async Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(p=>p.RedesSociais);

            if(includeEventos){
                query = query.Include(p=>p.PalestranteEventos).ThenInclude(pe=>pe.Evento);
            }

            query=query.AsNoTracking().OrderBy(p=>p.Id).Where(p=>p.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }


    }
}