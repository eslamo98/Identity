using Identity.ConstantRoles;
using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using Identity.Data;

namespace Identity.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            // Step 1: Retrieve all users first
            var users = await _userManager.Users
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email
                }).ToListAsync();

            // Step 2: Fetch roles for each user asynchronously
            foreach (var user in users)
            {
                user.Roles = await _userManager.GetRolesAsync(new ApplicationUser { Id = user.Id });
            }

            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.Select(r => new RolesSelectedViewModel { Id = r.Id, Name = r.Name }).ToListAsync();
            var viewModel = new AddUserModelView()
            {
                Roles = roles
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserModelView user)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Notification"] = "User was not created";
                ViewData["ToastIcon"] = "bi bi-x-circle";
                ViewData["ToastBgColor"] = "bg-danger";
                return View();
            }

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!user.Roles.Any(r => r.IsSelected))
                    {
                        ViewData["Notification"] = "Please select at least one role";
                        ViewData["ToastIcon"] = "bi bi-x-circle";
                        ViewData["ToastBgColor"] = "bg-danger";
                        ModelState.AddModelError("Roles", "Please select at least one role");
                        return View(user);
                    }
                    var exist = await _userManager.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
                    if (exist != null)
                    {
                        ViewData["Notification"] = "Email already exists";
                        ViewData["ToastIcon"] = "bi bi-x-circle";
                        ViewData["ToastBgColor"] = "bg-danger";
                        ModelState.AddModelError("Email", "Email already exists");
                        return View(user);
                    }
                    var usernameExists = await _userManager.Users.Where(u => u.UserName == user.UserName).FirstOrDefaultAsync();
                    if (usernameExists != null)

                    {
                        ViewData["Notification"] = "Username already exists";
                        ViewData["ToastIcon"] = "bi bi-x-circle";
                        ViewData["ToastBgColor"] = "bg-danger";
                        ModelState.AddModelError("UserName", "Username already exists");
                        return View(user);
                    }
                    var appUser = new ApplicationUser()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,

                    };
                    var result = await _userManager.CreateAsync(appUser, user.Password);

                    if (!result.Succeeded)
                    {
                        await trans.RollbackAsync();
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(user);
                    }

                    // Add selected roles
                    var selectedRoles = user.Roles.Where(r => r.IsSelected).Select(r => r.Name).ToList();
                    var addResult = await _userManager.AddToRolesAsync(appUser, selectedRoles);

                    await trans.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    await trans.RollbackAsync();
                    return View(user);
                }
            }


        }

        public async Task<IActionResult> Edit(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles
            .Select(r => new RolesSelectedViewModel
            {
                Id = r.Id,
                Name = r.Name,

            })
            .ToListAsync();
            foreach (var item in roles)
            {
                item.IsSelected = userRoles.Contains(item.Name); // Set IsSelected to true if the role is in the user's roles
            }
            var viewModel = new EditUserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                ProfilePic = user.ProfilePicture,
                Id = user.Id,
                Roles = roles
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingUser = await _userManager.FindByIdAsync(model.Id);

                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Check if the username already exists (excluding the current user)
                    var existingUsername = await _userManager.FindByNameAsync(model.UserName);


                    if (existingUsername != null && existingUsername.Id != model.Id)
                    {
                        ViewData["Notification"] = "Username already exists";
                        ViewData["ToastIcon"] = "bi bi-x-circle";
                        ViewData["ToastBgColor"] = "bg-danger";
                        ModelState.AddModelError("UserName", "Username already exists");
                        model.ProfilePic = existingUser.ProfilePicture;

                        return View(model);
                    }

                    // Check if the email already exists (excluding the current user)
                    var existingEmail = await _userManager.FindByEmailAsync(model.Email);

                    if (existingEmail != null && existingEmail.Id != model.Id)
                    {
                        ViewData["Notification"] = "Email already exists";
                        ViewData["ToastIcon"] = "bi bi-x-circle";
                        ViewData["ToastBgColor"] = "bg-danger";
                        ModelState.AddModelError("Email", "Email already exists");
                        model.ProfilePic = existingUser.ProfilePicture;
                        return View(model);
                    }

                    // Update user properties
                    existingUser.FirstName = model.FirstName;
                    existingUser.LastName = model.LastName;
                    existingUser.UserName = model.UserName;
                    existingUser.Email = model.Email;

                    // Handle profile picture update if a new file is uploaded
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        // Check file extension and size (implement your logic here)

                        using (var dataStream = new MemoryStream())
                        {
                            await file.CopyToAsync(dataStream);
                            existingUser.ProfilePicture = dataStream.ToArray();
                        }
                    }

                    // Update the user
                    var updateResult = await _userManager.UpdateAsync(existingUser);
                    if (!updateResult.Succeeded)
                    {
                        throw new Exception("Failed to update user.");
                    }

                    // Remove all roles from the user
                    var currentRoles = await _userManager.GetRolesAsync(existingUser);
                    var removeResult = await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);

                    if (!removeResult.Succeeded)
                    {
                        await trans.RollbackAsync();
                        throw new Exception("Failed to remove existing roles.");
                    }

                    // Add selected roles
                    var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.Name).ToList();
                    var addResult = await _userManager.AddToRolesAsync(existingUser, selectedRoles);

                    if (!addResult.Succeeded)
                    {
                        throw new Exception("Failed to add selected roles.");
                    }

                    // Commit the transaction
                    await trans.CommitAsync();

                    ViewData["Notification"] = "User updated successfully!";
                    ViewData["ToastIcon"] = "bi bi-check-circle"; // Bootstrap icon for success
                    ViewData["ToastBgColor"] = "bg-success"; // Green background for success

                    return RedirectToAction("Index"); // or wherever you want to redirect after success
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();

                    ViewData["ToastIcon"] = "bi bi-x-circle";
                    ViewData["ToastBgColor"] = "bg-danger";
                    ViewData["Notification"] = ex.Message;
                    var existingUser = await _userManager.FindByIdAsync(model.Id);
                    // Reload the roles and return to the view with the model
                    var roles = await _roleManager.Roles.ToListAsync();
                    model.Roles = roles.Select(role => new RolesSelectedViewModel
                    {
                        Id = role.Id,
                        Name = role.Name,
                        IsSelected = _userManager.IsInRoleAsync(existingUser, role.Name).Result
                    }).ToList();

                    return View(model);
                }
            }
        }

        public async Task<IActionResult> ManageRoles(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return NotFound();
            var Roles = await _roleManager.Roles.ToListAsync();

            var userRoles = new UserRolesViewModel()
            {
                UserId = user.Id,
                Username = user.UserName,
                Roles = Roles.Select(role => new RolesSelectedViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };


            return View(userRoles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Notification"] = "User roles could not be updated.";
                ViewData["ToastIcon"] = "bi bi-x-circle";
                ViewData["ToastBgColor"] = "bg-danger";
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            // Begin a transaction

            try
            {
                // Remove all roles from the user
                var currentRoles = await _userManager.GetRolesAsync(user);
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!removeResult.Succeeded)
                {
                    throw new Exception("Failed to remove existing roles.");
                }

                // Add selected roles
                var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.Name).ToList();
                var addResult = await _userManager.AddToRolesAsync(user, selectedRoles);

                if (!addResult.Succeeded)
                {
                    throw new Exception("Failed to add selected roles.");
                }

                //// Commit the transaction
                //await transaction.CommitAsync();
                //throw new Exception("Error");
                ViewData["Notification"] = "Roles updated successfully!";
                ViewData["ToastIcon"] = "bi bi-check-circle"; // Bootstrap icon for success
                ViewData["ToastBgColor"] = "bg-success"; // Green background for success
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //// Rollback the transaction on error
                //await transaction.RollbackAsync();

                ViewData["ToastIcon"] = "bi bi-x-circle";
                ViewData["ToastBgColor"] = "bg-danger";
                ViewData["Notification"] = ex.Message;
                var Roles = await _roleManager.Roles.ToListAsync();

                var userRoles = new UserRolesViewModel()
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    Roles = Roles.Select(role => new RolesSelectedViewModel
                    {
                        Id = role.Id,
                        Name = role.Name,
                        IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                    }).ToList()
                };


                return View(userRoles);
            }

        }


        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            if(userRoles.Contains(Roles.Admin))
            {

                ViewData["ToastIcon"] = "bi bi-x-circle";
                ViewData["ToastBgColor"] = "bg-danger";
                ViewData["Notification"] = $"Error: Can't delete Admin user";
                return RedirectToAction("Index");

            }
            try
                {
                   
                    // Delete the user
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        ViewData["ToastIcon"] = "bi bi-x-circle";
                        ViewData["ToastBgColor"] = "bg-danger";
                        ViewData["Notification"] = "Failed to delete the user.";
                        return RedirectToAction("Index");
                    }

          

                    ViewData["Notification"] = "User was deleted successfully!";
                    ViewData["ToastIcon"] = "bi bi-check-circle"; // Bootstrap icon for success
                    ViewData["ToastBgColor"] = "bg-success"; // Green background for success
                }
                catch (Exception ex)
                {

                    ViewData["ToastIcon"] = "bi bi-x-circle";
                    ViewData["ToastBgColor"] = "bg-danger";
                    ViewData["Notification"] = $"Error occurred: {ex.Message}";
                }

                return RedirectToAction("Index");
            }
        

    }
}