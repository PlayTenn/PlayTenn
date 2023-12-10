using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PlayTen.DAL;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PlayTen.BLL.Interfaces;
using PlayTen.BLL.Services;
using PlayTen.BLL.Services.Jwt;
using Microsoft.AspNetCore.Identity.UI.Services;
using PlayTen.BLL.Interfaces.EmailSending;
using PlayTen.BLL.Services.EmailSending;

namespace PlayTen
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddDbContextPool<PlayTenDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<PlayTenDbContext>()
                .AddDefaultTokenProviders();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    });
            });

            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PlayTen API"
                });

                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.\nExample: 'Bearer {your token}'"
                });
                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()
                .Where(x =>
                    x.FullName != null && (x.FullName.Equals("PlayTen.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null") ||
                                           x.FullName.Equals("PlayTen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"))));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;

                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization();
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));
            services.Configure<EmailServiceSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.AddTransient<IJwtService, JwtService>();
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IParticipantManager, ParticipantManager>();
            services.AddScoped<IParticipantStatusManager, ParticipantStatusManager>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<ITennisLevelService, TennisLevelService>();
            services.AddScoped<ITrainingActionManager, TrainingActionManager>();
            services.AddScoped<ITrainingUserManager, TrainingUserManager>();
            services.AddScoped<ITrainingUserService, TrainingUserService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISurfaceService, SurfaceService>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<IEmailSendingService, EmailSendingService>();
            services.AddScoped<IEmailContentService, EmailContentService>();
            services.AddScoped<ITournamentActionManager, TournamentActionManager>();
            services.AddScoped<ITournamentUserManager, TournamentUserManager>();
            services.AddScoped<ITournamentUserService, TournamentUserService>();
            services.AddScoped<IMatchService, MatchService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseDeveloperExceptionPage();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<PlayTenDbContext>();
                context.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();

            app.UseRouting(); 
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            CreateRoles(serviceProvider).Wait();

            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "PlayTen Api");
                o.RoutePrefix = String.Empty;
            });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!(await roleManager.RoleExistsAsync(role)))
                {
                    var idRole = new IdentityRole
                    {
                        Name = role
                    };

                    var res = await roleManager.CreateAsync(idRole);
                }
            }
            var admin = Configuration.GetSection("Admin");
            var profile = new User
            {
                Email = admin["Email"],
                UserName = admin["Email"],
                Name = "Admin",
                Surname = "Admin",
                EmailConfirmed = true,
                TennisLevelId = 3
            };
            if (await userManager.FindByEmailAsync(admin["Email"]) == null)
            {
                var res = await userManager.CreateAsync(profile, admin["Password"]);
                if (res.Succeeded)
                    await userManager.AddToRoleAsync(profile, "Admin");
            }
            else if (!await userManager.IsInRoleAsync(userManager.Users.First(item => item.Email == profile.Email), "Admin"))
            {
                var user = userManager.Users.First(item => item.Email == profile.Email);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
