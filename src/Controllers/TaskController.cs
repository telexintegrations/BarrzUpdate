using Microsoft.AspNetCore.Mvc;

using src.Models;
using src.Models.Request;
using src.Repository.Interface;

namespace src.TaskController
{
    [Route("/")]
    [ApiController]
    public class TaskController : ControllerBase{
        private readonly ITelex _telex;
        public TaskController(ITelex telex)
        {
            _telex = telex;
        }        

        [HttpGet]
        public async Task<ActionResult> Bing()
        {
            var response = await _telex.GetToken();
            return Ok($"api is active!, Token - { response}");
        }

        [HttpGet("integration.json")]
        public ActionResult GetIntegration()
        {
            var configSettings = _telex.GetTelexConfiguration();
            return Ok(configSettings);
        }

        [HttpPost("/tick")]
        public async Task<ActionResult> BingTelex(TickRequest req)
        {
            var response = await _telex.BingTelex(req);
            //TODO:: add validation
            return StatusCode(202, new {
                status = "accepted"
            });
        }
    }
}