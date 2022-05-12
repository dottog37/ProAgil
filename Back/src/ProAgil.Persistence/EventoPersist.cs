using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Persistence.Contextos;

namespace ProAgil.Persistence.Contratos
{
    public class EventoPersist : IEventoPersist
    {
        private readonly ProAgilContext _context;

        public EventoPersist(ProAgilContext context)
        {
            _context = context;

        }
        
        public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrate = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e=>e.Lote).Include(e=>e.RedesSociais);

            if(includePalestrate){
                query = query.Include(e=>e.PalestranteEventos).ThenInclude(pe=>pe.Palestrante);
            }

            query=query.AsNoTracking().Where(e=>e.UserId==userId).OrderBy(e=>e.Id);

            return await query.ToArrayAsync();

        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrate = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e=>e.Lote).Include(e=>e.RedesSociais);

            if(includePalestrate){
                query = query.Include(e=>e.PalestranteEventos).ThenInclude(pe=>pe.Palestrante);
            }

            query=query.AsNoTracking()
                        .OrderBy(e=>e.Id)
                        .Where(e=>e.Tema.ToLower().Contains(tema.ToLower()) && e.UserId==userId);
            
            return await query.ToArrayAsync();       
        }
        public async Task<Evento> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrate = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e=>e.Lote).Include(e=>e.RedesSociais);

            if(includePalestrate){
                query = query.Include(e=>e.PalestranteEventos).ThenInclude(pe=>pe.Palestrante);
            }

            query=query.AsNoTracking().OrderBy(e=>e.Id)
            .Where(e=>e.Id== EventoId && e.UserId==userId);
            
            return await query.FirstOrDefaultAsync(); 
        }

  

    }
}