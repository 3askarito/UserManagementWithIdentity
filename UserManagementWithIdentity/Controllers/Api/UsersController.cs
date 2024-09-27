using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementWithIdentity.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class UsersController(UserManager<ApplicationUser> userManager) : ControllerBase
    {
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception();
            return Ok();
        }
    }
}
