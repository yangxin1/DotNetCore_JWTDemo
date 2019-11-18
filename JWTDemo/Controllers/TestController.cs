using JWTDemo.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JWTDemo.Controllers
{
    /// <summary>
    /// JWT验证控制器
    /// </summary>
    [ApiController]
    public class TestController : BaseAPIController
    {
        /// <summary>
        /// 配置
        /// </summary>
        public readonly JWTSetting _JWTsetting;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Setting"></param>
        public TestController(IOptions<JWTSetting> Setting)
        {
            _JWTsetting = Setting.Value;
        }

        /// <summary>
        /// 测试接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/test/Authorize")]
        [Authorize]
        public IActionResult Index()
        {
            return new Result(StateCode.Ok,"有权限访问");
        }
        /// <summary>
        /// 无权限验证
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/test/NoAuthorize")]
        public IActionResult NoAuthorize()
        {
            return new Result(StateCode.Ok, "无权限验证访问");
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/test/user")]
        [Authorize]
        public IActionResult GetUserName()
        {
            return Success(UserName);
        }
        /// <summary>
        /// 返回实体类
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/test/setting")]
        [Authorize]
        public IActionResult Setting()
        {
            return Success(_JWTsetting);
        }
    }
}