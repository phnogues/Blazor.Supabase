namespace Blazor.Supabase.Data;

public interface IDataRepository<T> where T : class
{
	Task<T?> GetById(long id);

	Task<IEnumerable<T>> GetAll();

	Task<T?> Insert(T entity);

	Task<T?> Update(T entity);

	Task Delete(T entity);
}
