using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Server.Entities.Configurations;

public class DeviceFlowCodesConfiguration : IEntityTypeConfiguration<Duende.IdentityServer.EntityFramework.Entities.DeviceFlowCodes>
{
	public void Configure(EntityTypeBuilder<Duende.IdentityServer.EntityFramework.Entities.DeviceFlowCodes> builder)
	{
		builder.ToTableSnakeCase();
	}
}
