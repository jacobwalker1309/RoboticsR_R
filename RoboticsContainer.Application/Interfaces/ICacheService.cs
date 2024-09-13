namespace RoboticsContainer.Application.Interfaces
{
    /// <summary>
    /// Caching Service
    /// </summary>
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        Task RemoveAsync(string key);
    }

}
