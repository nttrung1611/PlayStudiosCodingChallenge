using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayStudiosCodingChallenge.Services;
using PlayStudiosCodingChallenge.Services.Models;

namespace PlayStudiosCodingChallenge.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestStateController : ControllerBase
    {
        private readonly IPlayerQuestService _playerQuestService;

        public QuestStateController(IPlayerQuestService playerQuestService)
        {
            _playerQuestService = playerQuestService;   
        }

        [HttpPost]
        public async Task<IActionResult> Progress(QuestProgressionRequest request)
        {
            try
            {
                var response = await _playerQuestService.UpdateQuestProgress(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> State(Guid playerId)
        {
            try
            {
                //var isGuid = Guid.TryParse(playerId, out Guid playerIdGuid);
                //if (!isGuid) { return BadRequest(); }

                var questState = await _playerQuestService.GetQuestState(playerId);

                if (questState == null) { return NotFound(); }

                return Ok(questState);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetPlayerId()
        {
            try
            {
                var playerId = _playerQuestService.GetPlayerId();

                return Ok(playerId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
