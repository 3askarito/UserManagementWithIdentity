namespace UserManagementWithIdentity.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : Controller
    {
        public async Task <IActionResult> Index()
        {
            var user = await userManager.Users.Select(u => new UserViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                Roles = userManager.GetRolesAsync(u).Result
            }).ToListAsync();
            return View(user);
        }
        public async Task<IActionResult> Add()
        {
            var roles = await roleManager.Roles.Select(r => new RoleViewModel { RoleId = r.Id, RoleName = r.Name}).ToListAsync();
            var viewModel = new AddNewUserViewModel
            {
                Roles = roles
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddNewUserViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            if(!model.Roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Please Select At Least one Role!");
                return View(model);
            }
            if(await userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", "Username Is already Exists!");
                return View(model);
            }
            if (await userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email Is already Exists!");
                return View(model);
            }
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }
                return View(model);
            }
            await userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            var roles = await roleManager.Roles.ToListAsync();
            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if(user == null)
                return NotFound();
            var userRoles = await userManager.GetRolesAsync(user);
            foreach(var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await userManager.RemoveFromRoleAsync(user, role.RoleName);
                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    await userManager.AddToRoleAsync(user, role.RoleName);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if( user == null )
                return NotFound();
            var viewModel = new ProfileFormViewModel
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();
            var userWithSameUsername = await userManager.FindByNameAsync(model.UserName);
            if (userWithSameUsername != null && userWithSameUsername.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "This Username Is Already assigned to anthoer User!");
                return View(model);
            }
            var userWithSameEmail = await userManager.FindByEmailAsync(model.Email);
            if(userWithSameEmail != null && userWithSameEmail.Id != model.Id)
            {
                ModelState.AddModelError("Email", "This Email Is Already assigned to anthoer User!");
                return View(model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            await userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
