using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocationForEmployee;
using HR.LeaveManagement.Application.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        private readonly IMediator mediator;

        public AuthController(IAuthService authenticationService, IMediator mediator)
        {
            this._authenticationService = authenticationService;
            this.mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
        {
            return Ok(await _authenticationService.Login(request));
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            var userRegistrationResponse = await _authenticationService.Register(request);
            await mediator.Send(new CreateLeaveAllocationForEmployeeCommand { UserId = userRegistrationResponse.UserId });
            return Ok(userRegistrationResponse);
        }
    }
}
