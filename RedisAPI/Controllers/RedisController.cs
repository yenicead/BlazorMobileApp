using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : ControllerBase
    {
        private readonly ILogger<RedisController> _logger;
        private readonly ConnectionMultiplexer _redis;

        private static readonly IEnumerable<UserGpsInformation> UserGpsInformations =
            new List<UserGpsInformation>
            {
                new UserGpsInformation
                {
                    UserNickname = "UserA",
                    Latitude = 42.58745,
                    Longitude = 31.12487,
                    Altitude = 0.0
                },
                new UserGpsInformation
                {
                    UserNickname = "UserB",
                    Latitude = 42.58745,
                    Longitude = 31.12487,
                    Altitude = 0.0
                },
                new UserGpsInformation
                {
                    UserNickname = "UserC",
                    Latitude = 42.58745,
                    Longitude = 31.12487,
                    Altitude = 0.0
                },
                new UserGpsInformation
                {
                    UserNickname = "UserD",
                    Latitude = 42.58745,
                    Longitude = 31.12487,
                    Altitude = 0.0
                }
            };

        public RedisController(ILogger<RedisController> logger)
        {
            _logger = logger;
            _redis = ConnectionMultiplexer.Connect("localhost:6379");
        }

        // TODO: 2 endpoint'e ihtiyacim var. 
        // 1. POST bool SaveUserInformationAsync(GspInformation)
        // 2. GET Enumerable<GspInformation> GetClosestUsersAsync(GspInformation)

        [HttpPost]
        public async Task<bool> SaveUserInformationAsync(UserGpsInformation userGpsInformation)
        {
            IDatabase db = _redis.GetDatabase();

            var key = userGpsInformation.UserNickname;
            string json = JsonSerializer.Serialize(UserGpsInformations);

            RedisValue isUserExist = await db.StringGetAsync(key);
            if (isUserExist.HasValue)
            {
                bool isUserDeleted = await db.KeyDeleteAsync(key);
                if (isUserDeleted)
                {
                    bool result = await db.StringSetAsync(userGpsInformation.UserNickname, json);
                    return result;
                }

                return false;
            }
            else
            {
                bool result = await db.StringSetAsync(userGpsInformation.UserNickname, json);
                return result;
            }
        }


        [HttpGet]
        public async Task<IEnumerable<UserGpsInformation>> GetClosestUsersAsync([FromQuery] UserGpsInformation userGpsInformation)
        {
            // Burada butun kayitlari redis'den al ve sonra istek atan kullaniciyi bu listeden cikart.
            IDatabase db = _redis.GetDatabase();
            RedisValue result = await db.StringGetAsync(userGpsInformation.UserNickname);
            var list = JsonSerializer.Deserialize<List<UserGpsInformation>>(result);


            System.Net.EndPoint[] endpoints = db.Multiplexer.GetEndPoints();
            IServer server = db.Multiplexer.GetServer(endpoints.First());

            var keys = server.Keys(db.Database);
            foreach (var key in keys)
            {
                string tempKey = key;
            }

            return list;
        }
    }
}
