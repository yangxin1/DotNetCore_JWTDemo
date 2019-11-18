using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWTDemo.Model
{
    public class Token
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public static string securityKey = "GQDstclechengroberbojPOXOYg5MbeJ1XT0uFiwDVvVBrk";

        #region 加密
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="payLoad">加密信息</param>
        /// <param name="expiresMinute">过期时间（分钟）</param>
        /// <returns></returns>
        public static string CreateTokenByHandler(Dictionary<string, object> payLoad, int expiresMinute)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>();
            foreach (var key in payLoad.Keys)
            {
                var tempClaim = new Claim(key, payLoad[key]?.ToString());
                claims.Add(tempClaim);
            }
            
            var jwt = new JwtSecurityToken(
                issuer: "Sparkle", // 签发者
                audience: "User", // 使用者
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(expiresMinute)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
        #endregion

        #region 解密
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="encodeJwt"></param>
        /// <param name="validatePayLoad"></param>
        /// <returns></returns>
        public static bool Validate(string encodeJwt, out string msg, Func<Dictionary<string, object>, bool> validatePayLoad=null)
        {
            var success = true;
            Dictionary<string, object> header;
            Dictionary<string, object> payLoad;
            var jwtArr = encodeJwt.Split('.');
            try
            {
                header = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[0]));
                payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));
            }
            catch(Exception error)
            {
                msg = error.Message;
                return false;
            }
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(securityKey));
            //首先验证签名是否正确(重要)
            success = success && string.Equals(jwtArr[2], Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1])))));
            if (!success)
            {
                msg = "签名不正确";
                return success;//签名不正确直接返回
            }
            //其次验证是否在有效期内
            var now = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000; // 获取时间戳
            success = success && (now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString()));

            //再其次 进行自定义的验证
            if (validatePayLoad == null && success)
            {
                msg = "验证通过";
                return success;
            }
            success = success && validatePayLoad(payLoad); // 执行自定义验证
            if (success) msg = "通过"; else msg = "不通过";
            return success;
        }
        #endregion 
    }
}
