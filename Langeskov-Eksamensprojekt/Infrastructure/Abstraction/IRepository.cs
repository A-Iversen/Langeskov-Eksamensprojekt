    
namespace Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        // CRUD: Read
        IEnumerable<T> GetAll();
        T? GetById(int id);

        // CRUD: Create
        T Add(T entity);

        // CRUD: Update
        void Update(T entity);

        // CRUD: Delete
        void Delete(int id);
    }
}
