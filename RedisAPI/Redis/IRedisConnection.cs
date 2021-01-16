using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisAPI.Redis
{
    public interface IRedisConnection
    {
        void Connect();
        void Close();
        Task ConnectAsync();
        Task CloseAsync();
        IDatabase GetRedisDatabase();
        Task<bool> AddAsync(UserGpsInformation userGpsInformation);
        Task<bool> DeleteAsync(UserGpsInformation userGpsInformation);
        Task<bool> UpdateAsync(UserGpsInformation userGpsInformation);
        Task<IEnumerable<UserGpsInformation>> GetAllAsync();
    }
}
