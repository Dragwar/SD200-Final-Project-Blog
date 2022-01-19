using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Server.Entities.Configurations;

public class KeyConfiguration : IEntityTypeConfiguration<Duende.IdentityServer.EntityFramework.Entities.Key>
{
	public void Configure(EntityTypeBuilder<Duende.IdentityServer.EntityFramework.Entities.Key> builder)
	{
		builder.ToTableSnakeCase();
	}
}
