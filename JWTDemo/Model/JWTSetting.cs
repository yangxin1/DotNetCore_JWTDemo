using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.Model
{
    /// <summary>
    /// JWT配置类
    /// </summary>
    public class JWTSetting
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public int JWTExpries { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }
    }
}
