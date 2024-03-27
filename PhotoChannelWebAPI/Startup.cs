using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Business.AutoMapperConfig;
using Business.Concrete;
using Core.Extensions;
using Core.Utilities.PhotoUpload;
using Core.Utilities.PhotoUpload.Cloudinary;
using Core.Utilities.Security.Encyption;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Dal.EntityFramework.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using PhotoChannelWebAPI.Cache;
using PhotoChannelWebAPI.Extensions;
using PhotoChannelWebAPI.Helpers;
using PhotoChannelWebAPI.Helpers.Auth.Cookie;
using PhotoChannelWebAPI.Helpers.Auth.Session;
using PhotoChannelWebAPI.Middleware.Exception;
using PhotoChannelWebAPI.Middleware.MemoryCache;
using TokenOptions = Core.Utilities.Security.Jwt.TokenOptions;

namespace PhotoChannelWebAPI
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("http://localhost:3000", "http://localhost:3001","http://photo-channel-spa:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader().AllowCredentials()
                );
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                };
                options.Events = new JwtEvents();
            });
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthHelper, AuthSessionHelper>();
            services.AddAuthorization();
            services.AddCustomMemoryCache();
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PhotoChannel API",
                    Description = "Users can create channels or subscribe to an existing channel, is a simple web api project where you can share photos on these channels as well as like or comment on photos.",
                    Contact = new OpenApiContact
                    {
                        Name = "Ali Y�ld�z�z",
                        Email = "aliyildizoz909@gmail.com",
                        Url = new Uri("http://localhost:3000")
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                options.AddSecurityDefinition("RefreshToken", new OpenApiSecurityScheme
                {
                    Description = @"Refresh token is required for request a new access token when jwt expired.",
                    Name = "refreshToken",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "RefreshToken"
                            },
                            Scheme = "oauth2",
                            Name = "refreshToken",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddHealthChecks();

            services.AddDbContext<PhotoChannelContext>(
                        options =>
                        {
                            var connectionString = Configuration.GetConnectionString("DefaultConnection");
                            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
                            if (!isDevelopment)
                            {
                                var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
                                connectionString = string.Format(connectionString, password);
                            }
                            options.UseSqlServer(connectionString);
                        });

            #region ServicesDP

            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IUserDal, EfUserDal>();

            services.AddScoped<IHomeService, HomeManager>();
            services.AddScoped<IHomeDal, EfHomeDal>();

            services.AddScoped<ISearchService, SearchManager>();
            services.AddScoped<ISearchDal, EfSearchDal>();

            services.AddScoped<ICommentService, CommentManager>();
            services.AddScoped<ICommentDal, EfCommentDal>();


            services.AddScoped<ILikeService, LikeManager>();
            services.AddScoped<ILikeDal, EfLikeDal>();

            services.AddScoped<ISubscriberService, SubscriberManager>();
            services.AddScoped<ISubscriberDal, EfSubscriberDal>();


            services.AddScoped<IChannelCategoryService, ChannelCategoryManager>();
            services.AddScoped<IChannelCategoryDal, EfChannelCategoryDal>();

            services.AddScoped<ICountService, CountManager>();
            services.AddScoped<ICountDal, EfCountDal>();

            services.AddScoped<IChannelService, ChannelManager>();
            services.AddScoped<IChannelDal, EfChannelDal>();

            services.AddScoped<IPhotoService, PhotoManager>();
            services.AddScoped<IPhotoDal, EfPhotoDal>();

            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICategoryDal, EfCategoryDal>();


            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<ITokenHelper, JwtHelper>();
            services.AddScoped<IPhotoUpload, CloudinaryHelper>();

            #endregion



        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhotoChannelWebApi V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseExceptionMiddleware();
            app.UseCors("AllowOrigin");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCacheMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

        }
    }
}
