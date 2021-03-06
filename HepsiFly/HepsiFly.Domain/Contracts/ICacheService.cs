namespace HepsiFly.Domain.Contracts;

public interface ICacheService
{
    T Get<T>(string key);

    void Set(string key, object data, int cacheTime);

    bool IsSet(string key);

    bool Remove(string key);

    void RemoveByPattern(string pattern);

    void Clear();
}