using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medico.Service.DynamicForm.Interfaces
{
    public interface IReadOnlyRepository<T> where T : class
    {
        T Find(string id);
        Task<T> FindAsync(string id);
        string FindByValue(string value);
        Task<string> FindByValueAsync(string value);

        //no async for IEnumerable
        IEnumerable<T> GetAll();
        IQueryable<T> AsQueryable();

        IEnumerable<T> GetAllFromProc(string id);
        IEnumerable<T> GetAllFromProcByTenantSite(string tenantId, string siteId, string roomId);
    }

    public interface IRepository<T> : IReadOnlyRepository<T> where T : class
    {
        //IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        int Remove(int entity);
        int Delete(string id);
        Tuple<T, T> Update(T entity);
        bool Edit(T entity);
    }
}
