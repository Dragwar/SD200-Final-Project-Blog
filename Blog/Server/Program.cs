using Blog.Server.Data;
using Blog.Server.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BlogContext>(options =>
{
	options.UseNpgsql(connectionString, npgsqlOptions =>
		{
			npgsqlOptions.MigrationsHistoryTable(BlogContext.MIGRATIONS_TABLE, BlogContext.DEFAULT_SCHEMA);
		})
		.UseSnakeCaseNamingConvention();
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options =>
	{
		options.SignIn.RequireConfirmedAccount = true;
		options.Password.RequireNonAlphanumeric = false;
	})
	.AddRoles<IdentityRole>()
	.AddRoleManager<RoleManager<IdentityRole>>()
	.AddEntityFrameworkStores<BlogContext>();

builder.Services.AddIdentityServer()
	.AddApiAuthorization<User, BlogContext>();

builder.Services.AddAuthentication()
	.AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ISeeder, Seeder>();

await using var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

using var appStoppingTokenSource = CancellationTokenSource.CreateLinkedTokenSource(app.Lifetime.ApplicationStopped, app.Lifetime.ApplicationStopping);
await Seeder.CreateAndSeedAsync(app.Services, appStoppingTokenSource.Token);

await app.RunAsync(appStoppingTokenSource.Token);
