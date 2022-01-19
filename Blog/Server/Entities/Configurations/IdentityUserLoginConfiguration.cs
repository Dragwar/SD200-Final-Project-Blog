using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Server.Entities.Configurations;

public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
	public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
	{
		builder.ToTableSnakeCase("UserLogins");
	}
}
