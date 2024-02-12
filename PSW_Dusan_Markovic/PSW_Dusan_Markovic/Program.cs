global using PSW_Dusan_Markovic.resources.Data;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.OpenApi.Models;
global using PSW_Dusan_Markovic.resources.service;
using PSW_Dusan_Markovic.resources.model;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<YourDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
})
.AddEntityFrameworkStores<YourDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TourService>();
builder.Services.AddScoped<LoginService>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await InitializeRolesAsync(roleManager);
}

static async Task InitializeRolesAsync(RoleManager<IdentityRole> roleManager)
{
    var roles = new[] { "TOURIST", "AUTHOR", "ADMIN" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();