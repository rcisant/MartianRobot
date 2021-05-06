using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MartianRobot.Domain.SeedWork;
using MartianRobot.Api.Helpers;
using MartianRobot.Application.Services.UriService;
using MartianRobot.Application.Services.MartianRobot;
using MartianRobot.Domain.Entities;

namespace MartianRobot.Api.Controllers.v1
{
    /// <summary>
    /// The MartianRobot's controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion(ApiVersioning._1_0)]
    public class MartianRobotController : ControllerBase
    {
        private readonly ILogger<MartianRobotController> _logger;
        private readonly IMartianRobotService _martianRobotService;
        private readonly IUriService _uriService;

        public MartianRobotController(IMartianRobotService martianRobotService
                                    , ILogger<MartianRobotController> logger
                                    , IUriService uriService)
        {
            _martianRobotService = martianRobotService ?? throw new ArgumentNullException(nameof(martianRobotService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService)); ;
        }


        /// <summary>
        /// Calculate instructions.
        /// </summary>
        /// 
        /// <returns>
        ///     If OK, then returns <see cref="Domain.Entities.MartianRobotDTO"></see> object, otherwise returns error.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<MartianRobotDTO>), 200)]
        public async Task<ActionResult> GetMartianRobot(decimal xLimit, decimal yLimit, [FromQuery] List<InstructionDTO> instructions)
        {
            
            _logger.LogDebug("Requesting a MartianRobot by Instructions");

            var result = await _martianRobotService.GetAsync(xLimit, yLimit, instructions);

            _logger.LogDebug("The MartianRobot has been requested");

            return Ok(result);
        }



    }
}
