using Microsoft.AspNetCore.Mvc;
using PreparationTaskService.DataTransfer.Streets;
using static PreparationTaskService.Common.Statics;
using PreparationTaskService.Services.Interfaces;

namespace PreparationTaskService.Controller
{
    [ApiController]
    [Produces("application/json")]
    public class PreparationTaskController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ILogger<PreparationTaskController> _logger;
        private readonly IStreetService _streetService;

        public PreparationTaskController(ILogger<PreparationTaskController> logger, IStreetService streetService)
        {
            _logger = logger;
            _streetService = streetService;
        }

        [HttpPost]
        [Route(STREET_CREATE)]
        [ProducesResponseType(typeof(StreetResponseHolderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async ValueTask<IActionResult> PostStreetCreate([FromBody] StreetCreateRequestHolderDto requestModel)
        {
            try
            {
                var result = await _streetService.FetchAndProcessingStreetCreationAsync(requestModel.StreetCreateRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("PreparationTaskController. Error in PostStreetCreate.", ex);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route(STREET_DELETE)]
        [ProducesResponseType(typeof(StreetResponseHolderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async ValueTask<IActionResult> PostStreetDelete([FromBody] StreetDeleteRequestHolderDto requestModel)
        {
            try
            {
                var result = await _streetService.FetchAndProcessingStreetDeletionsAsync(requestModel.StreetDeleteRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("PreparationTaskController. Error in PostStreetDelete.", ex);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route(STREET_ADD_POINT)]
        [ProducesResponseType(typeof(StreetResponseHolderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async ValueTask<IActionResult> PostStreetAddPoint([FromBody] StreetAddPointRequestHolderDto requestModel)
        {
            try
            {
                var result = await _streetService.FetchAndProcessingAddingNewPointAsync(requestModel.StreetAddPointRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("PreparationTaskController. Error in PostStreetAddPoint.", ex);
            }
            return BadRequest();
        }
    }
}
