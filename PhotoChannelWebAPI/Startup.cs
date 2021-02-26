using System;
using System.Collections.Generic;
using System.Linq;
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
                    builder => builder.WithOrigins("http://localhost:3000", "http://localhost:3001")
                        .AllowAnyMethod()
                        .AllowAnyHeader().AllowCredentials()
                );
            });
            services.AddAutoMapper(typeof(Startup));
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

            services.AddControllers();

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

            services.AddAuthorization();

            services.AddCustomMemoryCache();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
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
            });

        }
    }
}
