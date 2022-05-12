using System.Threading.Tasks;
using ProAgil.Persistence.Contextos;

namespace ProAgil.Persistence.Contratos
{
    public class GeralPersist : IGeralPersist
    {
        private readonly ProAgilContext _context;

        public GeralPersist(ProAgilContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await( _context.SaveChangesAsync()) > 0;
        }

        


    }
}