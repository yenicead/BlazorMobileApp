using HaversineDistanceCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisAPI.Services
{
    public interface IRedisControllerService
    {
        Task<bool> AddUserToRedisAsync(UserGpsInformation userGpsInformation);
        Task<Dictionary<string, double>> GetClosestUsersAsync(UserGpsInformation userGpsInformation, int selectedDistance = 100);
    }
}
