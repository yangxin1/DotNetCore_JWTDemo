using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.Model
{
    /// <summary>
    /// 返回结果类
    /// </summary>
    public class Result : JsonResult
    {
        #region 字段
        /// <summary>
        /// 识别码
        /// </summary>
        public static StateCode _code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public static string _message { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public static dynamic _data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public Result(StateCode code, string msg = null, dynamic data = null) : base(null)
        {
            _code = code;
            _message = msg;
            _data = data;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 返回消息方法
        /// </summary>
        /// <param name="code">识别码</param>
        /// <param name="data">数据</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static IActionResult ReturnData(StateCode code,dynamic data = null,string msg = null)
        {
            _code = code;
            _message = msg;
            CheckMessage(); // 静态方法
            _data = data;
            return new JsonResult( new { code = _code, msg = _message, data = _data });
        }

        /// <summary>
        /// 返回消息格式整理
        /// </summary>
        private static void CheckMessage()
        {
            if (_code == StateCode.Ok)
            {
                if (_message == null || string.IsNullOrWhiteSpace(_message))
                {
                    _message = "操作成功（封装）";
                }
            }
            else
            {
                if (_message == null || string.IsNullOrWhiteSpace(_message))
                {
                    _message = "操作失败（封装）";
                }
            }
        }

        /// <summary>
        /// 必须要重写该方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            CheckMessage();
            Value = new
            {
                code = _code,
                message = _message,
                data = _data
            };
            return base.ExecuteResultAsync(context);
        }
        #endregion 
    }

    /// <summary>
    /// 返回消息枚举
    /// </summary>
    public enum StateCode
    {
        Ok = 200,
        Fail = 403
    }
}
