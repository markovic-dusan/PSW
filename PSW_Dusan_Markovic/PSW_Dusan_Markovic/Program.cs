global using PSW_Dusan_Markovic.resources.Data;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.OpenApi.Models;
global using PSW_Dusan_Markovic.resources.service;
global using PSW_Dusan_Markovic.resources.model;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Timers;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<YourDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSwaggerGen();


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    //auth
})
.AddEntityFrameworkStores<YourDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "issuer", // Postavi vrednost issuer-a
        ValidAudience = "issuer", // Postavi vrednost audience-a
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0w0J3Mo1$3cReT")) // Postavi vrednost ključa
    };

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EVERYONE", policy =>
    {
        policy.RequireRole("ADMIN", "AUTHOR", "TOURIST");
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});



//services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TourService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ReportService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1"));
    app.UseCors("AllowAnyOrigin");

}
else
{
    app.UseCors("AllowAnyOrigin");
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();



using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await InitializeRolesAsync(roleManager);
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<YourDbContext>();

        context.Database.Migrate();

        var userManager = services.GetRequiredService<UserManager<User>>();

        if (!context.Users.Any() && !context.Tours.Any())
        {
            fillDatabase(context, userManager);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}

app.MapControllers();




app.Run();

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



void fillDatabase(YourDbContext context, UserManager<User> userManager)
{

    var adminUser = new User("admin1", "Admin23.", "Admin", "Adminovic", "admin@gmail.com", UserType.ADMIN);
    var author1 = new User("drmili", "Mile23..", "Milan", "Milanovic", "mile@gmail.com", UserType.AUTHOR);
    var author2 = new User("grts", "Grts223.", "Ivan", "Ivanovic", "ii@gmail.com", UserType.AUTHOR);
    var tourist1 = new User("turka1", "Tats2x23.", "Ana", "Ivanovic", "aniA@gmail.com", UserType.TOURIST);

    try
    {
        userManager.CreateAsync(tourist1, tourist1.Password).Wait();
        userManager.CreateAsync(author2, author2.Password).Wait();
        userManager.CreateAsync(author1, author1.Password).Wait();
        userManager.CreateAsync(adminUser, adminUser.Password).Wait();
        userManager.AddToRoleAsync(author2, "AUTHOR").Wait();
        userManager.AddToRoleAsync(author1, "AUTHOR").Wait();
        userManager.AddToRoleAsync(adminUser, "ADMIN").Wait();
        userManager.AddToRoleAsync(tourist1, "TOURIST").Wait();

    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error adding user: {ex.Message}");
    }

    var interestSpiritual = new Interest(EnumInterest.SPIRITUAL);
    var interestSightsee = new Interest(EnumInterest.SIGHTSEEING);
    var interestAdventure = new Interest(EnumInterest.ADVENTURE);

    List<Interest> interests1 = new List<Interest>
    {
        interestSpiritual,
        interestSightsee
    };

    List<Interest> interests2 = new List<Interest>
    {
        interestAdventure
    };
    var publishedTour1 = new Tour("Pub tour 1", "Opis pture 1", EnumTourDifficulty.HARD, 20, author1.Id, interests1);
    var publishedTour2 = new Tour("Pub tour 2", "Opis pture 2", EnumTourDifficulty.HARD, 20, author2.Id, interests2);
    var publishedTour3 = new Tour("Pub tour 3", "Opis pture 3", EnumTourDifficulty.EASY, 20, author1.Id);
    publishedTour1.publishTour();
    publishedTour2.publishTour();
    publishedTour3.publishTour();

    var sampleTours = new List<Tour>
    {
        new Tour("Tura1", "Opis ture 1", EnumTourDifficulty.HARD, 20, author1.Id),
        new Tour("Tura2", "Opis ture 2", EnumTourDifficulty.EASY, 30, author2.Id),
        publishedTour1,
        publishedTour2,
        publishedTour3
    };
    context.Interests.Add(interestSpiritual);
    context.Interests.Add(interestSightsee);
    context.Interests.Add(interestAdventure);
    context.UserInterests.Add(new UserInterest(tourist1.Id, interestSpiritual.InterestValue));

    context.Tours.AddRange(sampleTours);
    context.SaveChanges();
    context.TourInterests.Add(new TourInterest(publishedTour1.TourId, interestSpiritual.InterestValue));
    context.TourInterests.Add(new TourInterest(publishedTour1.TourId, interestSightsee.InterestValue));
    context.TourInterests.Add(new TourInterest(publishedTour2.TourId, interestAdventure.InterestValue));
    context.SaveChanges();
}
