﻿using BlazorEcommerce.Application.Contracts.Identity;
using BlazorEcommerce.Shared.Cart;
using BlazorEcommerce.Shared.Constant;
using BlazorEcommerce.Shared.Response.Concrete;
using BlazorEcommerce.Shared.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public UserController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<IResponse>> ChangePassword([FromBody] UserChangePassword changePassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return new DataResponse<string>("Missing user", HttpStatusCodes.NotFound);
            }
            var response = await _identityService.ChangePassword(userId, changePassword.CurrentPassword, changePassword.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
