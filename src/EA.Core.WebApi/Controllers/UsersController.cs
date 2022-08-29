//using EA.Core.Application.Interfaces;
using EA.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EA.NetDevPack.Mediator;
using EA.Core.Application.Queries;
using EA.Core.Application.Commands;
using EA.NetDevPack.Context;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EA.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IMediatorHandler mediator, IContextUser context, ILogger<UsersController> logger)
        {
            _mediator = mediator; _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var name = _context.UserName;
            var c = _context.UserClaims;
            var result = await _mediator.Send(new UserQueryAll());
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / request.Top) + 1;
            var query = new UserPagingQuery("", pageSize, pageIndex, null);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("query")]
        public async Task<IActionResult> Query([FromQuery] UsersRequest request)
        {
            var query = new UserPagingQuery(request.Keyword ?? "", request.PageSize, request.PageIndex, request.ToBaseQuery());
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _mediator.Send(new UserQueryById(id));
            if (user == null)
                return BadRequest(new ValidationResult("User not exists"));
            return Ok(user);
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> Post([FromBody] AddUsersRequest request)
        {
            var c = _context.UserClaims;
            var createdBy = _context.UserName;

            // Check authorization
            //if (c.Roles != 'admin')
            //{
            //     return BadRequest(new ValidationResult("Sorry! You are not allowed to additional"));
            //}

            var userAddCommand = new UserAddCommand(
                Guid.NewGuid(),
                request.LoginID,
                request.LoginName,
                request.FullName,
                request.Email,
                request.Address,
                request.Phone,
                request.Photo,
                request.Status
            );

            userAddCommand.CreatedBy = createdBy;
            var result = await _mediator.SendCommand(userAddCommand);
            return Ok(result);
        }

        [HttpPut("editUser")]
        public async Task<IActionResult> Put(int id, [FromBody] EditUsersRequest request)
        {
            var c = _context.UserClaims;
            var name = _context.UserName;
            var modifiedDate = DateTime.Now;
            var Sub = new Guid(request.Sub);
            var user = await _mediator.Send(new UserQueryById(Sub));

            if (user == null)
                return BadRequest(new ValidationResult("User not exists"));

            // Check authorization
            //if (c.Roles != 'admin' || name != user.LoginID)
            //{
            //     return BadRequest(new ValidationResult("Sorry! You are not allowed to edit this user"));
            //}

            var userEditCommand = new UserEditCommand(
                Sub,
                request.Sub,
                request.LoginID,
                request.LoginName,
                request.FullName,
                request.Email,
                request.Address,
                request.Phone,
                request.Photo,
                request.Status,
                user.CreatedDate,
                user.CreatedBy,
                modifiedDate,
                name
            );

            var result = await _mediator.SendCommand(userEditCommand);

            return Ok(result);
        }

        [HttpDelete("deleteUser")]
        public async Task<IActionResult> Delete(string id)
        {
            var c = _context.UserClaims;
            var name = _context.UserName;
            var Sub = new Guid(id);
            var user = await _mediator.Send(new UserQueryById(Sub));

            if (user == null)
                return BadRequest(new ValidationResult("User not exists"));

            // Check authorization
            //if (c.Roles != 'admin' || name != user.LoginID)
            //{
            //     return BadRequest(new ValidationResult("Sorry! You are not allowed to delete this user"));
            //}

            var userDeleteCommand = new UserDeleteCommand(Sub, user.LoginID, user.FullName);
            var result = await _mediator.SendCommand(userDeleteCommand);

            return Ok(result);
        }
    }
}
