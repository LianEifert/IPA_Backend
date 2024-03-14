
namespace ProjectEstimaterBackend.Services
{
    public interface IDataService<T>
    {
        T Add(T entity);
        T Update(T entity, string id);
    }
}
