using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JWTDemo.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTDemo.Controllers
{
    /// <summary>
    /// 基类控制器
    /// </summary>
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        #region 字段
        /// <summary>
        /// 获取当前用户名
        /// </summary>
        protected string UserName
        {
            get
            {
                Claim claim = User.Claims.FirstOrDefault(x => x.Type == "name"); // 保存在Claim中的用户名
                if (claim == null) return "";
                return claim.Value;
            }
        }         
        #endregion 

        #region 通用方法
        /// <summary>
        /// 返回成功（使用protect）
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected virtual IActionResult Success(dynamic Data =null,string msg="")
        {
            return Result.ReturnData(StateCode.Ok, Data, msg);
        }

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected virtual IActionResult Fail(string msg)
        {
            return Result.ReturnData(StateCode.Fail,null, msg);
        }
        #endregion
    }
}