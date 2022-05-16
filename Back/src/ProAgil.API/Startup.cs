using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProAgil.Application;
using ProAgil.Application.Contratos;
using ProAgil.Persistence.Contextos;
using ProAgil.Persistence.Contratos;
using System;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using ProAgil.Persistence;
using System.Text.Json.Serialization;
using ProAgil.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using ProAgil.API.Helpers;

namespace ProAgil.API
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

            services.AddDbContext<ProAgilContext>(
                context => context.UseSqlite(Configuration.GetConnectionString("Default"))
            );

            services.AddIdentityCore<User>(x=>
            {
                x.Password.RequireDigit=false;
                x.Password.RequireNonAlphanumeric=false;
                x.Password.RequireLowercase=false;
                x.Password.RequiredLength =4;
                x.Password.RequireUppercase=false;
            }
            ).AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<User>>()
            .AddRoleValidator<RoleValidator<Role>>()
            .AddEntityFrameworkStores<ProAgilContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x => {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddControllers()
                    .AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add( new JsonStringEnumConverter()))
                    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddScoped<IEventoService, EventoService>();
            services.AddScoped<ILoteService, LoteService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPalestranteService, PalestranteService>();
            services.AddScoped<IRedeSocialService, RedeSocialService>();
            services.AddScoped<IUtil, Util>();
            
            services.AddScoped<IGeralPersist, GeralPersist>();
            services.AddScoped<IEventoPersist, EventoPersist>();
            services.AddScoped<ILotePersist, LotePersist>();
            services.AddScoped<IUserPersist, UserPersist>();
            services.AddScoped<IPalestrantePersist, PalestrantePersist>();
            services.AddScoped<IRedeSocialPersist, RedeSocialPersist>();


            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProAgil.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header usando Bearer.
                                    Entre com o 'Bearer [espaço] então coloque o seu Token.
                                    Exemplo: Bearer '123456asdf'",
                    Name = "Authorization", 
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Baerer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement() 
                {
                      {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference{
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header   
                            },
                            new List<string>()
                      }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProAgil.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
            
            );

            app.UseStaticFiles(new StaticFileOptions() {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                RequestPath = new PathString("/Resources")
            });
            
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
