using Blog.Server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Blog.Server.Utilities;
public interface ISeeder
{
	Task SeedAsync(CancellationToken cancellationToken = default);
}

public class Seeder : ISeeder
{
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public Seeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public static async Task CreateAndSeedAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
	{
		await using var scope = serviceProvider.CreateAsyncScope();
		var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
		await seeder.SeedAsync(cancellationToken);
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		var isAnyRolesSeeded = await _roleManager.Roles.AnyAsync(cancellationToken);
		var isAnyUsersSeeded = await _userManager.Users.AnyAsync(cancellationToken);

		if (isAnyRolesSeeded && isAnyUsersSeeded)
		{
			return;
		}

		if (!isAnyRolesSeeded)
		{
			await SeedRolesAsync(cancellationToken);
		}

		if (!isAnyUsersSeeded)
		{
			await SeedUsersAsync(cancellationToken);
		}
	}

	private async Task SeedRolesAsync(CancellationToken cancellationToken = default)
	{
		await CreateRoleIfNotExists(RoleConst.AdminRoleName, cancellationToken);
		await CreateRoleIfNotExists(RoleConst.ModeratorRoleName, cancellationToken);
	}

	private async Task CreateRoleIfNotExists(string roleName, CancellationToken cancellationToken = default)
	{
		_ = cancellationToken;
		if (string.IsNullOrWhiteSpace(roleName))
		{
			throw new ArgumentException($"'{nameof(roleName)}' cannot be null or whitespace.", nameof(roleName));
		}

		if (await _roleManager.RoleExistsAsync(roleName) is true)
		{
			return;
		}

		var createRoleResult = await _roleManager.CreateAsync(new(roleName));
		if (createRoleResult is { Succeeded: false })
		{
			throw new InvalidOperationException($"failed to seed roles: {JsonSerializer.Serialize(createRoleResult.Errors)}");
		}
	}

	private async Task CreateUserIfNotExists(string userName, string password, string? roleName = null, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(userName))
		{
			throw new ArgumentException($"'{nameof(userName)}' cannot be null or whitespace.", nameof(userName));
		}

		var normalizedUserName = _userManager.NormalizeName(userName);
		if (await _userManager.Users.AnyAsync(x => x.NormalizedUserName == normalizedUserName, cancellationToken) is true)
		{
			return;
		}

		var user = new User()
		{
			UserName = userName,
			NormalizedUserName = normalizedUserName,
			Email = userName,
			NormalizedEmail = userName,
			EmailConfirmed = true,
		};
		var createUserResult = await _userManager.CreateAsync(user, password);
		if (createUserResult is { Succeeded: false })
		{
			throw new InvalidOperationException($"failed to seed users: {JsonSerializer.Serialize(createUserResult.Errors)}");
		}

		if (string.IsNullOrWhiteSpace(roleName))
		{
			return;
		}

		var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);
		if (addToRoleResult is { Succeeded: false })
		{
			throw new InvalidOperationException($"failed to add user to role: {JsonSerializer.Serialize(addToRoleResult.Errors)}");
		}
	}

	private async Task SeedUsersAsync(CancellationToken cancellationToken = default)
	{
		await CreateUserIfNotExists(UserConst.AdminUserName, UserConst.AdminPassword, RoleConst.AdminRoleName, cancellationToken);
		await CreateUserIfNotExists(UserConst.ModeratorUserName, UserConst.ModeratorPassword, RoleConst.ModeratorRoleName, cancellationToken);
	}
}
