using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Persistence.Contextos;
using ProAgil.Persistence.Contratos;

namespace ProAgil.Persistence
{
    public class RedeSocialPersist: GeralPersist, IRedeSocialPersist
    {
        private readonly ProAgilContext context;
        public RedeSocialPersist(ProAgilContext context): base(context)
        {
            this.context = context;

        }
        public async Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int id){
            IQueryable<RedeSocial> query = context.RedesSociais;
            
            query = query.AsNoTracking()
                    .Where(rs => rs.EventoId == eventoId && rs.Id == id);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id){
            IQueryable<RedeSocial> query = context.RedesSociais;
            query = query.AsNoTracking()
                    .Where(rs => rs.PalestranteId == palestranteId && rs.Id == id);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId){
            IQueryable<RedeSocial> query = context.RedesSociais;
            query = query.AsNoTracking()
                .Where(rs => rs.EventoId == eventoId);
            return await query.ToArrayAsync();
        }
        public async Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId){
            IQueryable<RedeSocial> query = context.RedesSociais;

            query = query.AsNoTracking()
                        .Where(rs => rs.PalestranteId == palestranteId);
            return await query.ToArrayAsync();
        }
    }
}