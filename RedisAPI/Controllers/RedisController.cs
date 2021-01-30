using HaversineDistanceCalculator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedisAPI.Services;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace RedisAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : ControllerBase
    {
        private readonly IRedisControllerService _redisControllerService;
        private readonly ILogger<RedisController> _logger;

        public RedisController(IRedisControllerService redisControllerService, ILogger<RedisController> logger)
        {
            _redisControllerService = redisControllerService ?? throw new ArgumentNullException(nameof(logger));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("SaveUserInformationAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<bool> SaveUserInformationAsync(UserGpsInformation userGpsInformation)
        {
            return await _redisControllerService.AddUserToRedisAsync(userGpsInformation);
        }

        [HttpGet]
        [Route("GetClosestUsersAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<Dictionary<string, double>> GetClosestUsersAsync([FromQuery] UserGpsInformation userGpsInformation, int selectedDistance = 100)
        {
            return await _redisControllerService.GetClosestUsersAsync(userGpsInformation, selectedDistance);
        }
    }
}
