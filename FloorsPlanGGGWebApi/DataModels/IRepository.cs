using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloorsPlanGGGWebApi.DataModels
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T Get(int id);

        Task<bool> Add(T entity);

        Task<bool> Update(T entity);

        Task<bool> Delete(T entity);

        Task<bool> DeleteById(int id);
    }
}
