namespace Exchange.Service.Interfaces;

public interface ICacheService
{
    Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> funcToGetData, int durationInMinutes = 60);
}