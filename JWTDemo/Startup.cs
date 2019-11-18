using System;
using System.Collections.Generic;
using System.Text;
using JWTDemo.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace JWTDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        /// <summary>
        /// 服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // swagger

            #region swagger配置
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Sparkle",
                    Version = "这是显示在名称上面的版本号 ",
                    Description = "这是描述语句",
                    Contact = new Contact { Name = "Sparkle：", Url = "/views/test.html", Email = "a4200322@live.com" }
                });
                // 添加JWT
                var security = new Dictionary<string, IEnumerable<string>> { { "JWTDemo", new string[] { } }, }; // 校验方案
                c.AddSecurityRequirement(security);
                c.AddSecurityDefinition("JWTDemo", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization", //jwt默认的参数名称
                    In = "header",  //jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey" 
                });
            });
            //认证
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    // 是否开启签名认证
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Token.securityKey)),
                    // 发行人验证，这里要和token类中Claim类型的发行人保持一致
                    ValidateIssuer = true,
                    ValidIssuer = "Sparkle",//发行人
                    // 接收人验证
                    ValidateAudience = true,
                    ValidAudience = "User",//订阅人
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
            #endregion
            
            #region 添加跨域
            services.AddCors(options =>
            {
                options.AddPolicy("Cors", builder => builder.AllowAnyOrigin() //添加跨域访问规则
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials());
            });
            #endregion

            #region 读取配置
            services.Configure<JWTSetting>(Configuration.GetSection("JWTSetting"));
            #endregion
        }
        
        /// <summary>
        /// 中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "这是显示在右上角的文字");
            });
            // 验证
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
