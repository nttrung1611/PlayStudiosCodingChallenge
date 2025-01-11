using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayStudiosCodingChallenge.Services;
using PlayStudiosCodingChallenge.Services.Models;

namespace PlayStudiosCodingChallenge.Server.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly IPlayerQuestService _playerQuestService;

        public QuestController(IPlayerQuestService playerQuestService)
        {
            _playerQuestService = playerQuestService;   
        }

        [HttpPost]
        public async Task<IActionResult> Progress(QuestProgressionRequest request)
        {
            try
            {
                if (request.ChipAmountBet <= 0)
                    return BadRequest("ChipAmountBet must be greater than 0");

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
                var questState = await _playerQuestService.GetQuestState(playerId);

                if (questState == null) { return NotFound("Player quest state not found!"); }

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
