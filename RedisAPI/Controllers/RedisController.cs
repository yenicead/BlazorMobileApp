using HaversineDistanceCalculator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedisAPI.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : ControllerBase
    {
        private readonly IRedisConnection _redisConnection;
        private readonly IDistanceCalculator _distanceCalculator;
        private readonly ILogger<RedisController> _logger;

        public RedisController(IRedisConnection redisConnection, IDistanceCalculator distanceCalculator, ILogger<RedisController> logger)
        {
            _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
            _distanceCalculator = distanceCalculator ?? throw new ArgumentNullException(nameof(distanceCalculator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<bool> SaveUserInformationAsync(UserGpsInformation userGpsInformation)
        {
            bool iserUserAdded = await _redisConnection.AddAsync(userGpsInformation);
            return iserUserAdded;
        }


        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IEnumerable<UserGpsInformation>> GetClosestUsersAsync([FromQuery] UserGpsInformation userGpsInformation)
        {
            IEnumerable<UserGpsInformation> userGpsInformations = await _redisConnection.GetAllAsync();
            userGpsInformations = userGpsInformations.Where(x => !String.Equals(x.UserNickname, userGpsInformation.UserNickname));
            var first = userGpsInformations.First();
            var second = userGpsInformations.Last();

            var result = _distanceCalculator.Calculate(first.Latitude, first.Longitude, second.Latitude, second.Longitude);
            var test = _distanceCalculator.Calculate(51.5007, 0.1246, 40.6892, 74.0445);
            return userGpsInformations;
        }
    }
}
