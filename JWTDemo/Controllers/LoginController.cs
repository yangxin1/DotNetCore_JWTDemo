using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTDemo.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace JWTDemo.Controllers
{
    /// <summary>
    /// JWT验证控制器
    /// </summary>
    [ApiController]
    public class LoginController : BaseAPIController
    {
        #region 构造函数
        /// <summary>
        /// 配置
        /// </summary>
        public readonly JWTSetting _JWTsetting;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Setting"></param>
        public LoginController(IOptions<JWTSetting> Setting)
        {
            _JWTsetting = Setting.Value;
        }
        #endregion

        #region 控制器
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/login")]
        public IActionResult Login(string name,string password)
        {
            Dictionary<string, object> para = new Dictionary<string, object>
            {
                { "name", name },
                { "password", password },
                { "logintime", DateTime.Now }
            };
            //读取配置文件，设置过期时间
            string token = Token.CreateTokenByHandler(para, _JWTsetting.JWTExpries); // 加密
            return Success(token);
        }

        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        [HttpGet("/api/check/")]
        public IActionResult Check(string jwt)
        {
            // 传递消息
            bool msg = Token.Validate(jwt, out string message,SelfCheck); // 验证
            if (msg)
            {
                return Success();
            }
            else
            {
                return Fail("验证失败");
            }
        }        
        #endregion

        #region 自定义验证
        //添加自定义验证
        readonly Func<Dictionary<string, object>, bool> SelfCheck = (dict) =>
        {
            return true;
        };
        #endregion        
    }
}
