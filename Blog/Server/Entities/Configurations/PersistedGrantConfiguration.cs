using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Server.Entities.Configurations;

public class PersistedGrantConfiguration : IEntityTypeConfiguration<Duende.IdentityServer.EntityFramework.Entities.PersistedGrant>
{
	public void Configure(EntityTypeBuilder<Duende.IdentityServer.EntityFramework.Entities.PersistedGrant> builder)
	{
		builder.ToTableSnakeCase();
	}
}
