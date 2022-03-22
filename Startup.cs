using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Controllers;
using RestaurantAPI.Entitites;
using RestaurantAPI.Middleware;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using RestaurantAPI.Validators;

namespace RestaurantAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // tu będziemy wstrzykiwać zależności do wbudowanego kontenera dependency injection
        // tu będziemy też konfigurować różne serwisy, np. związane z autentykacja uzytkowników
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddControllers();
            services.AddControllers().AddFluentValidation();
            services.AddDbContext<RestaurantDbContext>();
            services.AddScoped<RestaurantSeeder>();
            services.AddAutoMapper(this.GetType().Assembly);

            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<TimeRequestMiddleware>();

            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestaurantSeeder seeder)
        {
            seeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAPI"));
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<TimeRequestMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


// Wersja z komentarzami: 
//public class Startup
//{
//    public Startup(IConfiguration configuration)
//    {
//        Configuration = configuration;
//    }

//    public IConfiguration Configuration { get; }

//    // This method gets called by the runtime. Use this method to add services to the container.
//    // tu będziemy wstrzykiwać zależności do wbudowanego kontenera dependency injection
//    // tu będziemy też konfigurować różne serwisy, np. związane z autentykacja uzytkowników
//    public void ConfigureServices(IServiceCollection services)
//    {
//        services.AddControllers();
//        services.AddDbContext<RestaurantDbContext>();
//        services.AddScoped<RestaurantSeeder>();
//        services.AddAutoMapper(this.GetType().Assembly);
//        services.AddSwaggerGen(c =>
//        {
//            c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantAPI", Version = "v1" });
//        });
//    }

//    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//    // będzie konfigurować wszystkie niezbędne metody przepływu, przez które musi przejść zapytanie do naszego API przed zwróceniem odpowiedzi
//    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestaurantSeeder seeder) // każda metoda przzepływu w metodzie configure, wywoływana na application builderze jest nazwana middleware. Middleware to kawałek kodu, który ma dostęp do dwóch rzeczy: kontekst zapytania, czyli informacje o czasowniku http, nagłówkach i adresie. Drugą rzeczą jest dostęp do następnego z kolei middleware
//    {
//        seeder.Seed();

//        if (env.IsDevelopment())
//        {
//            app.UseDeveloperExceptionPage();
//            app.UseSwagger();
//            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAPI v1"));
//        }

//        app.UseHttpsRedirection(); // jeśli klient wyśle zapytanie na adres bez protokołu https, jego zapytanie zostanie automatycznie przekierowane na adres z protokołem https

//        app.UseRouting();

//        //app.UseAuthorization();

//        app.UseEndpoints(endpoints =>
//        {
//            endpoints.MapControllers();
//        });
//    }
//}