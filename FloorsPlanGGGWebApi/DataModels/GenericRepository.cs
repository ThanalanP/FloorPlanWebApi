using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FloorsPlanGGGWebApi.DataModels
{
    public class GenericRepository<T> : IRepository<T> where T : class 
    {
        private readonly IUnitOfWork m_unitOfWork;

        public GenericRepository() : this(new UnitOfWork())
        {
        }

        public GenericRepository(UnitOfWork unitOfWork)
        {
            m_unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return m_unitOfWork.Context.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new List<T>();
            }
        }

        public T Get(int id)
        {
            try
            {
                return m_unitOfWork.Context.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public T Get(T entity)
        {
            try
            {
                return m_unitOfWork.Context.Set<T>().Find(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public async Task<bool> Add(T entity)
        {
            try
            {
                m_unitOfWork.Context.Set<T>().Add(entity);
                await m_unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                m_unitOfWork.Context.Set<T>().Attach(entity);
                m_unitOfWork.Context.Entry(entity).State = EntityState.Modified;
                await m_unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> Delete(T entity)
        {
            try
            {
                var entityToRemove = Get(entity);
                m_unitOfWork.Context.Set<T>().Remove(entityToRemove);
                await m_unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                var entityToRemove = Get(id);
                m_unitOfWork.Context.Set<T>().Remove(entityToRemove);
                await m_unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
    }
}