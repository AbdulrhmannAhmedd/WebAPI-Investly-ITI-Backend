
using Investly.DAL.Entities;
using Investly.DAL.Repos;
using Investly.DAL.Repos.IRepos;
using Investly.DAL.Seeding;
using Investly.PL.Attributes;
using Investly.PL.BL;
using Investly.PL.General.Services;
using Investly.PL.General.Services.IServices;
using Investly.PL.Hubs;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Investly.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials()
                                      );
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var config = builder.Configuration.GetSection("Jwt");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Issuer"],
                    ValidAudience = config["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/notificationHub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

           
            #region General services registeration
            builder.Services.AddScoped<IJWTService, JWTService>();
            builder.Services.AddScoped<IHelper, Helper>();
            builder.Services.AddScoped(typeof(IQueryService<>), typeof(QueryService<>));
            builder.Services.AddScoped(typeof(IRepo<>), typeof(Repo<>));
            builder .Services.AddHttpClient<IAiService, AiService>();
            #endregion

            #region Unit of work  registeration
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IBusinessRepo, BusinessRepo>();


            #endregion

            #region Business services registeration
            builder.Services.AddScoped<IInvestorService, InvestorService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBusinessService, BusinessService>();
            builder.Services.AddScoped<IGovernementService,GovernmentService>();
            builder.Services.AddScoped<IFounderService, FounderService>();

            builder.Services.AddScoped<IGovernementService,GovernmentService>();
            builder.Services.AddScoped<INotficationService,NotificationService>();
            builder.Services.AddScoped<IInvestorContactRequestService, InvestorContactRequestService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IStandardService, StandardService>();
            #endregion

            #region Hubs
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IUserIdProvider, NameIdentifierUserIdProvider>();

            #endregion
            var app = builder.Build();


            #region DataSeeding
            using (var scope = app.Services.CreateScope())
            {
                var services= scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDbContext>();
             
                    // Ensure the database is created and apply migrations
                   // dbContext.Database.Migrate();
                    var seeder = new DataSeeding(dbContext);
                    // Seed the database with initial data
                   //seeder.SuperAdminSeed();
                  // seeder.GovernmentCitiesSeed();
                
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseCors("AllowAllOrigins");
            app.MapHub<NotificationHub>("/notificationHub");

            app.Run();
        }
    }
}
