using System.Runtime.CompilerServices;
using HepsiFly.Domain.Contracts;
using HepsiFly.Infrastructure.Base;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace HepsiFly.Infrastructure.Cache;

public class RedisCacheService : CacheHelper, ICacheService
    {
        private static IDatabase _db;
        private readonly RedisSettings settings;

        public RedisCacheService(IOptions<RedisSettings> options)
        {
            settings = options.Value;
            
            CreateRedisDb(settings);
        }

        private static IDatabase CreateRedisDb(RedisSettings settings)
        {
            var connect = ConnectionMultiplexer.Connect(settings.ConnectionString);
            _db = connect.GetDatabase();
            return _db;
        }

        public void Clear()
        {
            var server = _db.Multiplexer.GetServer(settings.Host, settings.Port);
            foreach (var item in server.Keys())
                _db.KeyDelete(item);
        }

        public T Get<T>(string key)
        {
            var rValue = _db.SetMembers(key);
            if (rValue.Length == 0)
                return default(T);

            var result = Deserialize<T>(rValue.ToStringArray());
            return result;
        }

        public bool IsSet(string key)
        {
            try
            {
                return _db.KeyExists(key);
            }
            catch
            {
                return false;
            }
        }

        public bool Remove(string key)
        {
            return _db.KeyDelete(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var server = _db.Multiplexer.GetServer(settings.Host, settings.Port);
            foreach (var item in server.Keys(pattern: "*" + pattern + "*"))
                _db.KeyDelete(item);
        }

        public void Set(string key, object data, int cacheTime)
        {
            var entryBytes = Serialize(data);
            _db.SetAdd(key, entryBytes);

            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            if (cacheTime > 0)
                _db.KeyExpire(key, expiresIn);
        }
    }

