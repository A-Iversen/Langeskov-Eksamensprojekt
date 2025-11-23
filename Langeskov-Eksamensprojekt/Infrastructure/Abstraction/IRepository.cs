
namespace Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        // CRUD: Read
        IEnumerable<T> GetAll();
        T? GetById(int id);

        // CRUD: Create (Returnerer den oprettede entitet med tildelt ID fra DB)
        T Add(T entity);

        // CRUD: Update
        void Update(T entity);

        // CRUD: Delete
        void Delete(int id);
    }
}
