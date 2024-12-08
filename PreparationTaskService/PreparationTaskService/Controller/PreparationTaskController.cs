using Microsoft.AspNetCore.Mvc;
using PreparationTaskService.Services;
using PreparationTaskService.DataTransfer.Streets;
using static PreparationTaskService.Common.Statics;

namespace PreparationTaskService.Controller
{
    [ApiController]
    //[Route("[controller]")]
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
                var resp = new StreetResponseHolderDto() { StreetResponse = result };
                return Ok(resp);
            }
            catch (Exception ex)
            {

            }
            return null;
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
                var resp = new StreetResponseHolderDto() { StreetResponse = result };
                return Ok(resp);
            }
            catch (Exception ex)
            {

            }
            return null;
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
                var resp = new StreetResponseHolderDto() { StreetResponse = result };
                return Ok(resp);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
