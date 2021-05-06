using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MartianRobot.Api.Controllers.v1
{
    /// <summary>
    /// The version controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion(ApiVersioning._1_0)]
    public class VersionController : ControllerBase
    {
        public VersionController() { }

        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult Get()
        {
            var assemblyInformationalVersionAttribute = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            var responseContent = JsonConvert.SerializeObject(assemblyInformationalVersionAttribute, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return Ok(responseContent);
        }
    }
}
