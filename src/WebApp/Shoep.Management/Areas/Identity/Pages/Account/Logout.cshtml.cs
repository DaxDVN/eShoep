// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shoep.Management.Areas.Identity.Pages.Account;

public class LogoutModel : PageModel
{
    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await HttpContext.SignOutAsync("Cookies");
        if (returnUrl != null) return LocalRedirect(returnUrl);

        return RedirectToPage();
    }
}