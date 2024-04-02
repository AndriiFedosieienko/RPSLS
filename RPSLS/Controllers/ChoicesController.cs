using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPSLS.Applications.Contracts;
using RPSLS.Applications.Models;

namespace RPSLS.Controllers
{
	[Authorize]
	[ApiController]
	public class ChoicesController : Controller
	{
		private readonly IChoicesService _choicesService;

		public ChoicesController(IChoicesService choicesService)
		{
			_choicesService = choicesService;	
		}

		/// <summary>
		///     Return all possible choices
		/// </summary>
		/// <returns>All possible choices</returns>
		[HttpGet]
		[Route("~/choices")]
		[ProducesResponseType(typeof(IEnumerable<ChoiceModel>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllChoicesAsync(CancellationToken cancellationToken)
		{
			try
			{
				var choicesResponse = await _choicesService.GetAllChoicesAsync(cancellationToken);

				return Ok(choicesResponse);
			}
			catch (Exception exception)
			{
				var result = new ObjectResult(exception.Message);
				result.StatusCode = StatusCodes.Status500InternalServerError;
				return result;
			}
		}

		/// <summary>
		///     Return randomly selected choice
		/// </summary>
		/// <returns>Randomly selected choice</returns>
		[HttpGet]
		[Route("~/choice")]
		[ProducesResponseType(typeof(ChoiceModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetRandomChoicesAsync(CancellationToken cancellationToken)
		{
			try
			{
				var choicesResponse = await _choicesService.GetSingeChoice(cancellationToken);

				if(choicesResponse == null)
				{
					var result = new ObjectResult("There are some issues, they will be fixed soon");
					result.StatusCode = StatusCodes.Status500InternalServerError;
					return result;
				}

				return Ok(choicesResponse);
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
