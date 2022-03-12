using StackExchange.Redis;

namespace HepsiFly.Business.Cache;

public class RedisCacheService : CacheHelper, ICacheService
    {
        private static IDatabase _db;
        private static readonly string Host = "localhost";
        private static readonly int Port = 6379;

        public RedisCacheService()
        {
            CreateRedisDb();
        }

        private static IDatabase CreateRedisDb()
        {
            try
            {
                if (null == _db)
                {
                    var connect = ConnectionMultiplexer.Connect(@"localhost:6379,password=eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81");
                    _db = connect.GetDatabase();
                }

                return _db;
            }
            catch
            {
                return null;
            }
        }

        public void Clear()
        {
            var server = _db.Multiplexer.GetServer(Host, Port);
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
            var server = _db.Multiplexer.GetServer(Host, Port);
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

