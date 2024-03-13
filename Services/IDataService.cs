using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEstimaterBackend.Services
{
    public interface IDataService<T>
    {
        T Add(T entity);
        IList<T> GetAll();
        T GetById(string id);
        T Update(T entity, string id);
        void Delete(string id);

    }
}
