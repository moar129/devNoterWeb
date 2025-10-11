using devNoter.EFDbContext;
using Microsoft.EntityFrameworkCore;

namespace devNoter.Service
{
    public class dbService<T> : IService<T> where T : class
    {
        private readonly DevNoterDbContext _context;

        public dbService(DevNoterDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Generic method to get all objects from the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetObjectsAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Generic method to get an object by its id from the database 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetObjectByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Generic method to add an object to the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> AddObjectAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Generic method to update an object in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task UpdateObjectAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Generic method to save a list of objects to the database
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task SaveObjectsAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Set<T>().Add(entity);
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Generic method to delete an object from the database by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteObjectAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
