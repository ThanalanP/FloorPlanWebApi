using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FloorsPlanGGGWebApi.DataModels
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool m_disposed;

        public DbContext Context { get; }

        public UnitOfWork()
        {
            Context = new GggDataContext();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }

            m_disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}