using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPSLS.Applications.Contracts;
using RPSLS.Applications.Models;

namespace RPSLS.Controllers
{
	[Authorize]
	[ApiController]
	public class PlayController : Controller
	{
		private readonly IRulesService _rulesService;

		public PlayController(IRulesService rulesService)
		{
			_rulesService = rulesService;
		}

		/// <summary>
		///   Play with player into rock paper scissors lizard Spock game
		/// </summary>
		/// <returns>Game result</returns>
		[HttpPost]
		[Route("~/play")]
		[ProducesResponseType(typeof(GameResultsModel), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetAllChoicesAsync([FromBody] PlayInputModel playInputModel,
			CancellationToken cancellationToken)
		{
			try
			{
				if(playInputModel == null)
				{
					return BadRequest("Please, input your choice");
				}
				if(playInputModel.Player <= 0 || playInputModel.Player > 5) 
				{
					return BadRequest("Only numbers from 1 to 5 are allowed");
				}
				var gameResults = await _rulesService.GetGameResultsAsync(playInputModel.Player, cancellationToken);

				return Ok(gameResults);
			}
			catch (Exception exception)
			{
				var result = new ObjectResult(exception.Message);
				result.StatusCode = StatusCodes.Status500InternalServerError;
				return result;
			}
		}
	}
}
