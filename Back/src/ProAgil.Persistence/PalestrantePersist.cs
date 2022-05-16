using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Persistence.Contextos;
using ProAgil.Persistence.Models;

namespace ProAgil.Persistence.Contratos
{
    public class PalestrantePersist : GeralPersist, IPalestrantePersist
    {
        private readonly ProAgilContext _context;

        public PalestrantePersist(ProAgilContext context): base(context)
        {
            _context = context;

        }
        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p=>p.User)
            .Include(p=>p.RedesSociais);

            if(includeEventos){
                query = query.Include(p=>p.PalestranteEventos).ThenInclude(pe=>pe.Evento);
            }

            query = query.AsNoTracking()
                        .Where(p=> (p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                                    p.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                                    p.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower())) &&
                                    p.User.Funcao == Domain.Enum.Funcao.Palestrante)

                        .OrderBy(p=>p.Id);

            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }



        public async Task<Palestrante> GetPalestranteByUserIdAsync(int UserId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                   .Include(p=>p.User)
                   .Include(p=>p.RedesSociais);

            if(includeEventos){
                query = query.Include(p=>p.PalestranteEventos).ThenInclude(pe=>pe.Evento);
            }

            query=query.AsNoTracking().OrderBy(p=>p.Id).Where(p=>p.UserId == UserId);

            return await query.FirstOrDefaultAsync();
        }


    }
}