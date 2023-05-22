using Api.Middleware;
using Api.Services;
using Application;
using Application.Contracts;
using Infrastructure;
using Persistence;
using Identity;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Identity.Models;
using Serilog;
using Identity.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Add services of project layers
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddPersistenceServices(configuration);
builder.Services.AddIdentityServices(configuration);

//Inject
builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();

builder.Services.AddControllers();

//Add cros
builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
//    {
//        Scheme = "bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Description = "Bearer Authentication with JWT Token",
//        Type = SecuritySchemeType.Http
//    });
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Id = "bearer",
//                    Type = ReferenceType.SecurityScheme
//                }
//            },
//            new List<string>()
//        }
//    });
//});
builder.Services.AddSwaggerGen(c =>
{
    // ...
    // basic auth scheme (username + password)
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic"
    });
    // dont add global security requirement
    // c.AddSecurityRequirement(/*...*/);
    c.OperationFilter<SecureEndpointAuthRequirementFilter>();
});


var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

//Using For Seed Asp.Net Tables Data.
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await UserCreator.SeedAsync(userManager);

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    await CreateRoles.SeedAsync(roleManager);

    var identityContext = scope.ServiceProvider.GetRequiredService<BasketCommerceIdentityDbContext>();
    await CreateUserRoles.SeedAsync(identityContext);
    await CreateRoleClaims.SeedAsync(identityContext);

    Log.Information("Application Starting");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseCustomExceptionHandler();

app.UseCors(x => x.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()); // add server for angular


app.UseAuthorization();

app.MapControllers();

app.Run();

