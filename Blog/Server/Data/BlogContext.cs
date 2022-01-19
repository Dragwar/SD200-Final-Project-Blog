using Blog.Server.Entities;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Blog.Server.Data;
public class BlogContext : ApiAuthorizationDbContext<User>
{
	public const string DEFAULT_SCHEMA = "blg";
	public const string MIGRATIONS_TABLE = "__ef_migrations";

	public BlogContext(
		DbContextOptions options,
		IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.HasDefaultSchema(DEFAULT_SCHEMA);
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
