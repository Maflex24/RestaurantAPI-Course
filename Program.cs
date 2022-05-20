using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using RestaurantAPI.Authentications;
using RestaurantAPI.Authorization;
using RestaurantAPI.Controllers;
using RestaurantAPI.Entitites;
using RestaurantAPI.Middleware;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using RestaurantAPI.Validators;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

// configure service
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings); //get(bind) object properties to authenticationSettings

builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MinimumManagerAccess", builder => builder.RequireClaim("RoleId", "2", "3"));
    options.AddPolicy("MinimumAdminAccess", builder => builder.RequireClaim("RoleId", "3"));
    options.AddPolicy("Minimum18YearsOld", builder => builder.AddRequirements(new MinimumAgeRequirement(18)));
    options.AddPolicy("Minimum2RestaurantsIsCreatedByThisUser", builder => builder.AddRequirements(new MinimumRestaurantRequirement(2)));
});

builder.Services.AddScoped<IAuthorizationHandler, MinimumRestaurantRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
builder.Services.AddControllers().AddFluentValidation();

builder.Services.AddDbContext<RestaurantDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantDbConnection")));

builder.Services.AddScoped<RestaurantSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor(); // this is why we can use UserContextService, because we can inject Ihttpcontextaccesor to class
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<TimeRequestMiddleware>();

builder.Services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// CORS Policy - witohut this frontend browser application can't access to data from server, it's blocked by browser
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policyBuilder =>
            policyBuilder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(builder.Configuration["AllowedOrigins"]) // address of app, which we can give access

    );
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantAPI", Version = "v1" });
});


var app = builder.Build();

//configure

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();

app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("FrontEndClient");
seeder.Seed();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAPI"));
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<TimeRequestMiddleware>();

app.UseAuthentication();
app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthorization(); // need to be betweeen UserRouting and UseEndpoints!
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();