using System.Threading.Tasks;

namespace Entities
{
	public interface IRepository<T> where T : BaseEntity
	{
		Task<T> AddAsync(T entity);
		Task<T> GetAsync(string id);
		Task<T> UpdateAsync(T entity);
		Task<T> DeleteAsync(string id);
		Task<T> DeleteAsync(T entity);
	}
}
