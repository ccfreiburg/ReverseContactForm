using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ContRev.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }
        private IHostingEnvironment HostingEnvironment { get; }
        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        private void AddSwaggerAuthentication(IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ContRev", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {  
                        new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
        });
        }

        private void AddJWTAuthentication(IServiceCollection services, string secret) {
            
                // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //     .AddJwtBearer(options => {
                //     options.TokenValidationParameters = new TokenValidationParameters{
                //         ValidateIssuer = false,
                //         ValidateAudience = false,
                //         ValidateLifetime = true,
                //         ValidateIssuerSigningKey = true,
                //         ValidIssuer = "Jwt:Issuer",
                //         ValidAudience = "Jwt:Issuer",
    	        //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings")["Secret"]))
                //     };
                // });

            services.AddAuthentication(x =>  
            {  
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
            })  
            .AddJwtBearer(x =>  
            {  
                x.RequireHttpsMetadata = false;  
                x.SaveToken = true;  
                x.TokenValidationParameters = new TokenValidationParameters  
                {  
                    ValidateIssuerSigningKey = false,  
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),  
                    ValidateIssuer = false,  
                    ValidateAudience = false,  
                    ValidateLifetime = false,  
                    ValidIssuer = "Issuer",  
                    ValidAudience = "Audience",  
                    ClockSkew = TimeSpan.Zero,  
                };  
            }); 
        }

        public void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<AppSettings>();
                var a = new AppSettings(Configuration);
                AddJWTAuthentication(services, a.Secret);
                services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                services.AddEndpointsApiExplorer();


                services.AddSpaStaticFiles(config =>
                {
                    config.RootPath = "dist";
                });

                services.AddDbContext<ContRevDb>(opt => opt.UseSqlite("Data Source=data/Database.db"));


                if (HostingEnvironment.IsDevelopment()){
                    services.AddScoped<IEmailService, EmailServiceMock>();
                } else {
                    services.AddScoped<IEmailService, EmailService>();
                }


                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IOneTimeLinkService, OneTimeLinkService>();
                services.AddScoped<ITemplateService, TemplateService>();
    
                AddSwaggerAuthentication(services);
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var context = services.GetRequiredService<ContRevDb>();
                    context.Database.EnsureCreated();

                }

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    // app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthentication();  
                app.UseAuthorization();  

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                app.UseSpaStaticFiles();

                app.UseSpa(builder =>
                {
                    if (env.IsDevelopment())
                    {
                        builder.UseProxyToSpaDevelopmentServer("http://localhost:5173");
                    }
                });

            }
        }
    }