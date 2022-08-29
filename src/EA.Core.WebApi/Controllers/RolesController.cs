using EA.Core.Api.ViewModels;
using EA.Core.Application.Commands;
using EA.Core.Application.Queries;
using EA.NetDevPack.Context;
using EA.NetDevPack.Mediator;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EA.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<RolesController> _logger;
        public RolesController(IMediatorHandler mediator, IContextUser context, ILogger<RolesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
            _context = context;
        }

        // GET: api/<RolesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new RoleQueryAll());
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingRequest request)
        {
            var pagesize = request.Top;
            var pageIndex = (request.Skip / request.Top) + 1;
            var query = new RolePagingQuery("", pagesize, pageIndex);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // GET api/<RolesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var role = await _mediator.Send(new RoleQueryById(id));
            if (role == null)
                return BadRequest(new ValidationResult("Role not exists"));

            return Ok(role);
        }

        // POST api/<RolesController>
        [HttpPost("addRole")]
        public async Task<IActionResult> Post([FromBody] AddRoleRequest request)
        {
            var c = _context.UserClaims;
            var name = _context.UserName;

            // Check authorization
            //if (c.Roles != 'admin')
            //{
            //     return BadRequest(new ValidationResult("Sorry! You are not allowed to additional"));
            //}

            var roleAddCommand = new RoleAddCommand(
                Guid.NewGuid(),
                request.FullName,
                request.Role,
                request.Description,
                request.Status
            );

            roleAddCommand.CreatedBy = name;
            roleAddCommand.ModifiedBy = name;
            var result = await _mediator.SendCommand(roleAddCommand);
            return Ok(result);
        }

        // PUT api/<RolesController>/5
        [HttpPut("editRole")]
        public async Task<IActionResult> Put(int id, [FromBody] RolesUsersRequest request)
        {
            var c = _context.UserClaims;
            var name = _context.UserName;
            var modifiedDate = DateTime.Now;
            var idToGuid = new Guid(request.Id);

            var role = await _mediator.Send(new RoleQueryById(idToGuid));
            if (role == null)
                return BadRequest(new ValidationResult("Role not exists"));

            // Check authorization
            //if (c.Roles != 'admin' || role.Role != c.Roles)
            //{
            //     return BadRequest(new ValidationResult("Sorry! You are not allowed to edit this role"));
            //}

            var roleEditCommand = new RoleEditCommand(
                idToGuid,
                request.FullName,
                request.Role,
                request.Description,
                request.Status,
                role.CreatedDate,
                role.CreatedBy,
                modifiedDate,
                name
            );

            var result = await _mediator.SendCommand(roleEditCommand);
            return Ok(result);
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("deleteRole")]
        public async Task<IActionResult> Delete(string id)
        {
            var c = _context.UserClaims;
            var idToGuid = new Guid(id);
            var role = await _mediator.Send(new RoleQueryById(idToGuid));

            if (role == null)
                return BadRequest(new ValidationResult("Role not exists"));

            // Check authorization
            //if (c.Roles != 'admin' || role.Role != c.Roles)
            //{
            //     return BadRequest(new ValidationResult("Sorry! You are not allowed to delete this role"));
            //}

            var roleDeleteCommand = new RoleDeleteCommand(idToGuid, role.FullName, role.Role);
            var result = await _mediator.SendCommand(roleDeleteCommand);
            return Ok(result);
        }
    }
}
