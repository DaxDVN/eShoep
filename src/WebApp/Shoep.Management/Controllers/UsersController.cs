using Microsoft.AspNetCore.Mvc;
using Shoep.Management.Interfaces;
using Shoep.Management.Models.Auth;

namespace Shoep.Management.Controllers;

public class UsersController(IUserService userService) : Controller
{
    // GET: UserController
    public async Task<ActionResult> Index(int? pageNumber = 1, int pageSize = 4)
    {
        var result = await userService.GetUsers(pageNumber ?? 1, pageSize);

        var model = new UserListViewModel
        {
            Users = result.Users,
            CurrentPage = pageNumber ?? 1,
            TotalPages = result.TotalPages,
            PageSize = pageSize
        };

        return View(model);
    }


    public async Task<ActionResult> Details(string id)
    {
        if (string.IsNullOrEmpty(id)) return BadRequest();

        var user = await userService.GetUserById(id);
        if (user == null) return NotFound();

        return View(user);
    }

    // GET: UsersController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: UsersController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(User user)
    {
        user.UserName = user.Email;
        if (!ModelState.IsValid) return View(user);

        var result = await userService.AddUser(user);
        if (result) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, "Failed to create user.");
        return View(user);
    }

    // GET: UsersController/Edit/5
    public async Task<ActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id)) return BadRequest();

        var user = await userService.GetUserById(id);
        if (user == null) return NotFound();

        return View(user);
    }

    // POST: UsersController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(string id, User user)
    {
        if (id != user.Id) return BadRequest();
        if (!ModelState.IsValid) return View(user);

        var result = await userService.UpdateUser(user);
        if (result) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, "Failed to update user.");
        return View(user);
    }

    // GET: UsersController/Delete/5
    public async Task<ActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id)) return BadRequest();

        var user = await userService.GetUserById(id);
        if (user == null) return NotFound();

        return View(user);
    }

    // POST: UsersController/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(string id)
    {
        var result = await userService.DeleteUser(id);
        if (result) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, "Failed to delete user.");
        return View();
    }
}