using SmartChargingPoC.Business.Dtos.Requests.ChargeStations;
using SmartChargingPoC.Business.Dtos.Responses.ChargeStations;
using SmartChargingPoC.Business.Services.Interfaces;
using SmartChargingPoC.Core.Constants;
using Microsoft.AspNetCore.Mvc;

namespace SmartChargingPoC.WebAPI.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class ChargeStationsController : ControllerBase
    {
        private readonly IChargeStationService _chargeStationService;

        public ChargeStationsController(IChargeStationService chargeStationService)
        {
            _chargeStationService = chargeStationService;
        }
        
        [HttpOptions]
        public IActionResult GetChargeStationsOptions()
        {
            Response.Headers.Add("Allow", $"{ApiConstants.HttpMethod.Get}, {ApiConstants.HttpMethod.Options}, {ApiConstants.HttpMethod.Post}, {ApiConstants.HttpMethod.Patch}, {ApiConstants.HttpMethod.Delete}");
            return Ok();
        }
        
        [HttpGet]
        public IActionResult GetAllChargeStations()
        {
            var allChargeStations = _chargeStationService.GetAllChargeStations();

            return Ok(allChargeStations);
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetChargeStation(int id)
        {
            var selectedGroup = _chargeStationService.GetChargeStation(id);

            return Ok(selectedGroup);
        }

        [HttpPost]
        public ActionResult<ChargeStationResponseDto> CreateChargeStation(CreateChargeStationRequestDto createChargeStationRequestDto)
        {
            var createdChargeStation = _chargeStationService.CreateChargeStation(createChargeStationRequestDto);

            return CreatedAtAction(
                nameof(GetChargeStation),
                new { id = createdChargeStation.Id },
                createdChargeStation
            );
        }
        
        [HttpPatch]
        [Route("{id:int}")]
        public IActionResult UpdateChargeStation(int id, [FromBody] UpdateChargeStationRequestDto updateGroupRequestDto)
        {
            _chargeStationService.UpdateChargeStation(id, updateGroupRequestDto);

            return NoContent();
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteChargeStation(int id)
        {
            _chargeStationService.DeleteChargeStation(id);

            return NoContent();
        }
    }
}