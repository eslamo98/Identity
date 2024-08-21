using Identity.ConstantRoles;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Controllers
{
    [Authorize(Roles = Roles.Admin)]

    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _rolesManager;

        public RolesController(RoleManager<IdentityRole> rolesManager)
        {
            _rolesManager = rolesManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _rolesManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ShowModal"] = "show";
                return View("Index", await _rolesManager.Roles.ToListAsync());
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                ViewData["ShowModal"] = "show";
                ModelState.AddModelError("Name", "Role name cannot be empty.");
                return View("Index", await _rolesManager.Roles.ToListAsync());
            }

            if (await _rolesManager.RoleExistsAsync(model.Name))
            {
                ViewData["ShowModal"] = "show";
                ModelState.AddModelError("Name", "This Role already exists.");
                return View("Index", await _rolesManager.Roles.ToListAsync());
            }

            try
            {
                var result = await _rolesManager.CreateAsync(new IdentityRole { Name = model.Name.Trim(), Id = Guid.NewGuid().ToString() });
                if (!result.Succeeded)
                {
                    ViewData["ShowModal"] = "show";
                    ModelState.AddModelError("Name", "The Role wasn't created!");
                }
                else
                {
                    ViewData["Notification"] = "Role created successfully!";
                    ViewData["ToastIcon"] = "bi bi-check-circle"; // Bootstrap icon for success
                    ViewData["ToastBgColor"] = "bg-success"; // Green background for success
                }
                return View("Index", await _rolesManager.Roles.ToListAsync());
            }
            catch (Exception ex)
            {
                ViewData["ShowModal"] = "show";
                ModelState.AddModelError("Error", ex.Message);
                return View("Index", await _rolesManager.Roles.ToListAsync());
            }
        }


        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View("Index", await _rolesManager.Roles.ToListAsync());
            }
            var role = await _rolesManager.FindByIdAsync(id);
            if(role == null || role.Name == Roles.Admin)
            {
                return View("Index", await _rolesManager.Roles.ToListAsync());
            }

           var result =  await _rolesManager.DeleteAsync(role);
            if(result.Succeeded)
            {
                ViewData["ToastIcon"] = "bi bi-check-circle"; // Bootstrap icon for success
                ViewData["ToastBgColor"] = "bg-success"; // Green background for success
                ViewData["Notification"] = "Role was deleted successfully!";
            } else
            {
                ViewData["ToastIcon"] = "bi bi-x-circle";
                ViewData["ToastBgColor"] = "bg-danger";
                ViewData["Notification"] = "Role was not deleted";
            }
            return View(nameof(Index), await _rolesManager.Roles.ToListAsync());
        }
    }
}
