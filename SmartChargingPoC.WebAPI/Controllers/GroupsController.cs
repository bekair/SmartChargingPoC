using SmartChargingPoC.Business.Dtos.Requests.Groups;
using SmartChargingPoC.Business.Dtos.Responses.Groups;
using SmartChargingPoC.Business.Services.Interfaces;
using SmartChargingPoC.Core.Constants;
using Microsoft.AspNetCore.Mvc;

namespace SmartChargingPoC.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        
        [HttpOptions]
        public IActionResult GetGroupsOptions()
        {
            Response.Headers.Add("Allow", $"{ApiConstants.HttpMethod.Get}, {ApiConstants.HttpMethod.Options}, {ApiConstants.HttpMethod.Post}, {ApiConstants.HttpMethod.Put}, {ApiConstants.HttpMethod.Delete}");
            return Ok();
        }
        
        [HttpGet]
        public IActionResult GetAllGroups()
        {
            var allGroups = _groupService.GetAllGroups();

            return Ok(allGroups);
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetGroup(int id)
        {
            var selectedGroup = _groupService.GetGroup(id);

            return Ok(selectedGroup);
        }

        [HttpPost]
        public ActionResult<CreateGroupResponseDto> CreateGroup(CreateGroupRequestDto createGroupRequestDto)
        {
            var createdGroup = _groupService.CreateGroup(createGroupRequestDto);

            return CreatedAtAction(
                nameof(GetGroup),
                new { id = createdGroup.Id },
                createdGroup
            );
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateGroup(int id, [FromBody] UpdateGroupRequestDto updateGroupRequestDto)
        {
            _groupService.UpdateGroup(id, updateGroupRequestDto);

            return NoContent();
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteGroup(int id)
        {
            _groupService.DeleteGroup(id);

            return NoContent();
        }
    }
}