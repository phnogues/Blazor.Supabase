using FluentResults;

namespace BlazorSupabase.Data;

public interface IDataRepository<T> where T : class
{
	Task<T?> GetById(long id);

	Task<IEnumerable<T>> GetAll();

	Task<T?> Insert(T entity);

	Task<Result<T?>> Update(T entity);

	Task<Result> Delete(T entity);
}
