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
        private readonly ILogger<RedisController> _logger;

        public RedisController(IRedisConnection redisConnection, ILogger<RedisController> logger)
        {
            _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
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
            return userGpsInformations;
        }
    }
}
