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
            if(string.IsNullOrEmpty(req.Channel_id) || string.IsNullOrEmpty(req.Return_url))
            {
                return StatusCode(400, new {message = "Channel id or return url cannot be null or empty"});
            }          

            var response = await _telex.BingTelex(req);
            if(!response.Equals("success"))
            {
                return BadRequest(response);
            }
            //TODO:: add validation
            return StatusCode(202, new {
                status = "accepted"
            });
        }
    }
}