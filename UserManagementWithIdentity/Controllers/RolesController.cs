using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementWithIdentity.ViewModels;

namespace UserManagementWithIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController(RoleManager<IdentityRole> roleManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await roleManager.Roles.ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await roleManager.Roles.ToListAsync());
            }
            var roleIsExist = await roleManager.RoleExistsAsync(model.Name);
            if (roleIsExist)
            {
                ModelState.AddModelError("Name", "Role Is Exist!!!");
                return View("Index", await roleManager.Roles.ToListAsync());
            }
            await roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
            return RedirectToAction(nameof(Index));
        }
    }
}
