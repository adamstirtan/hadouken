using System.Collections.Generic;
using System.Threading.Tasks;

using Hadouken.ObjectModel;

namespace Hadouken.Database.Services
{
    public interface IServiceAsync<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(long id);

        Task<T> CreateAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(long id);

        Task<bool> DeleteAsync(IEnumerable<T> entities);
    }
}