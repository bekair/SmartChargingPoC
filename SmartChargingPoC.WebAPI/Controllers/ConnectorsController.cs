using SmartChargingPoC.Business.Dtos.Requests.Connectors;
using SmartChargingPoC.Business.Dtos.Responses.Connectors;
using SmartChargingPoC.Business.Services.Interfaces;
using SmartChargingPoC.Core.Constants;
using Microsoft.AspNetCore.Mvc;

namespace SmartChargingPoC.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConnectorsController : ControllerBase
    {
        private readonly IConnectorService _connectorService;

        public ConnectorsController(IConnectorService connectorService)
        {
            _connectorService = connectorService;
        }
        
        [HttpOptions]
        public IActionResult GetConnectorsOptions()
        {
            Response.Headers.Add("Allow", $"{ApiConstants.HttpMethod.Get}, {ApiConstants.HttpMethod.Options}, {ApiConstants.HttpMethod.Post}, {ApiConstants.HttpMethod.Patch}, {ApiConstants.HttpMethod.Delete}");
            return Ok();
        }
        
        [HttpGet]
        public IActionResult GetAllConnectors()
        {
            var allConnectors = _connectorService.GetAllConnectors();

            return Ok(allConnectors);
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetConnector(int id)
        {
            var selectedConnector = _connectorService.GetConnector(id);

            return Ok(selectedConnector);
        }

        [HttpPost]
        public ActionResult<ConnectorResponseDto> CreateConnector(CreateConnectorRequestDto createConnectorRequestDto)
        {
            var createdConnector = _connectorService.CreateConnector(createConnectorRequestDto);

            return CreatedAtAction(
                nameof(GetConnector),
                new { id = createdConnector.Id },
                createdConnector
            );
        }

        [HttpPatch]
        [Route("{id:int}")]
        public IActionResult UpdateConnector(int id, [FromBody] UpdateConnectorRequestDto updateGroupRequestDto)
        {
            _connectorService.UpdateConnector(id, updateGroupRequestDto);

            return NoContent();
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteConnector(int id)
        {
            _connectorService.DeleteConnector(id);

            return NoContent();
        }
    }
}
