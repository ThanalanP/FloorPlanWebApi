using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FloorsPlanGGGWebApi.DataModels
{
    public interface IUnitOfWork: IDisposable
    {
        DbContext Context { get; }

        Task SaveAsync();
    }
}
