using HaversineDistanceCalculator;
using Microsoft.Extensions.Logging;
using RedisAPI.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisAPI.Services
{
    public class RedisControllerService : IRedisControllerService
    {
        private readonly IRedisConnection _redisConnection;
        private readonly IDistanceCalculator _distanceCalculator;
        private readonly ILogger<RedisControllerService> _logger;

        public RedisControllerService(IRedisConnection redisConnection, IDistanceCalculator distanceCalculator, ILogger<RedisControllerService> logger)
        {
            _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
            _distanceCalculator = distanceCalculator ?? throw new ArgumentNullException(nameof(distanceCalculator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> AddUserToRedisAsync(UserGpsInformation userGpsInformation)
        {
            return await _redisConnection.AddAsync(userGpsInformation);
        }

        public async Task<Dictionary<string, double>> GetClosestUsersAsync(UserGpsInformation userGpsInformation, int selectedDistance = 100)
        {
            IEnumerable<UserGpsInformation> userGpsInformations = await _redisConnection.GetAllAsync();

            Dictionary<string, double> closestUsers = new Dictionary<string, double>();
            foreach (var otherUserCoordinates in userGpsInformations)
            {
                if (String.Equals(otherUserCoordinates.UserNickname, userGpsInformation.UserNickname))
                    continue;

                double distance = _distanceCalculator.Calculate(userGpsInformation.Latitude, userGpsInformation.Longitude, otherUserCoordinates.Latitude, otherUserCoordinates.Longitude);
                if (closestUsers.ContainsKey(otherUserCoordinates.UserNickname) && distance <= selectedDistance)
                {
                    closestUsers[otherUserCoordinates.UserNickname] = distance;
                }
                else
                {
                    if (distance <= selectedDistance)
                        closestUsers.Add(otherUserCoordinates.UserNickname, distance);
                }
            }

            return closestUsers;
        }
    }
}
