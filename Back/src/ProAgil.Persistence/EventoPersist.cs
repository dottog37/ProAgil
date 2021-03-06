using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Persistence.Contextos;
using ProAgil.Persistence.Models;

namespace ProAgil.Persistence.Contratos
{
    public class EventoPersist : IEventoPersist
    {
        private readonly ProAgilContext _context;

        public EventoPersist(ProAgilContext context)
        {
            _context = context;

        }
        
        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrate = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e=>e.Lote).Include(e=>e.RedesSociais);

            if(includePalestrate){
                query = query.Include(e=>e.PalestranteEventos).ThenInclude(pe=>pe.Palestrante);
            }

            query=query.AsNoTracking()
            .Where(e=>e.Tema.ToLower().Contains(pageParams.Term.ToLower()) && e.UserId==userId &&
                                         e.UserId==userId)
                                         .OrderBy(e=>e.Id);

            return await PageList<Evento>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);

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