using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisAPI.Redis
{
    public class RedisConnection : IRedisConnection
    {
        private ConnectionMultiplexer _redis;

        public RedisConnection()
        {
            this.Connect();
        }

        ~RedisConnection()
        {
            this.Close();
        }

        public void Connect()
        {
            try
            {
                // TODO<Adem>: Read connection, port and other connection information from appsettings.json.
                _redis = ConnectionMultiplexer.Connect("localhost:6379");
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"An exception occured, message: { exception }");
            }
        }

        public void Close()
        {
            try
            {
                _redis.Close();
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"An exception occured, message: { exception }");
            }
        }

        public async Task ConnectAsync() 
        {
            try
            {
                // TODO<Adem>: Read connection, port and other connection information from appsettings.json.
                _redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"An exception occured, message: { exception }");
            }
           
        }

        public async Task CloseAsync()
        {
            try
            {
                await _redis.CloseAsync();
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"An exception occured, message: { exception }");
            }
        }

        public IDatabase GetRedisDatabase() 
        {
            try
            {
                IDatabase redisDatabase = _redis.GetDatabase();
                return redisDatabase;
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"An exception occured, message: { exception }");
            }
        }

        public async Task<bool> AddAsync(UserGpsInformation userGpsInformation)
        {
            try
            {
                IDatabase redisDatabase = GetRedisDatabase();
                string key = userGpsInformation.UserNickname;
                string value = JsonSerializer.Serialize(userGpsInformation);

                bool isUserDeleted = await DeleteAsync(userGpsInformation);
                if (isUserDeleted)
                    return await redisDatabase.StringSetAsync(key, value);

                return false;
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"An exception occured, message: { exception }");
            }
        }

        public async Task<bool> DeleteAsync(UserGpsInformation userGpsInformation)
        {
            try
            {
                IDatabase redisDatabase = GetRedisDatabase();
                bool isKeyExists = await redisDatabase.KeyExistsAsync(userGpsInformation.UserNickname);
                if (isKeyExists)
                {
                    return await redisDatabase.KeyDeleteAsync(userGpsInformation.UserNickname);
                }
                return true;
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"An exception occured, message: { exception }");
            }
        }

        public async Task<bool> UpdateAsync(UserGpsInformation userGpsInformation)
        {
            try
            {
                IDatabase redisDatabase = GetRedisDatabase();
                return await AddAsync(userGpsInformation);
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"An exception occured, message: { exception }");
            }
        }

        public async Task<IEnumerable<UserGpsInformation>> GetAllAsync() 
        {
            try
            {
                IDatabase redisDatabase = GetRedisDatabase();
                EndPoint[] endpoints = redisDatabase.Multiplexer.GetEndPoints();
                IServer server = redisDatabase.Multiplexer.GetServer(endpoints.First());

                List<UserGpsInformation> userGpsInformations = new List<UserGpsInformation>();
                IEnumerable<RedisKey> keys = server.Keys(redisDatabase.Database);

                foreach (RedisKey key in keys)
                {
                    RedisValue value = await redisDatabase.StringGetAsync(key);
                    UserGpsInformation userGps = JsonSerializer.Deserialize<UserGpsInformation>(value);
                    userGpsInformations.Add(userGps);
                }

                return userGpsInformations;
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"An exception occured, message: { exception }");
            }
        }
    }
}
