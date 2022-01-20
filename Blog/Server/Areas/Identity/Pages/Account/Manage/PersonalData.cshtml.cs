// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Blog.Server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blog.Server.Areas.Identity.Pages.Account.Manage
{
	public class PersonalDataModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly ILogger<PersonalDataModel> _logger;

		public PersonalDataModel(
			UserManager<User> userManager,
			ILogger<PersonalDataModel> logger)
		{
			_userManager = userManager;
			_logger = logger;
		}

		public async Task<IActionResult> OnGet()
		{
			_ = _logger;
			var user = await _userManager.GetUserAsync(User);
			if (user is null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			return Page();
		}
	}
}
