using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.SystemModule.Dtos;
using Gromi.Infra.Entity.CraftHub.AuthModule.Dtos;
using Gromi.Infra.Utils.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gromi.Application.Common.AuthModule
{
    /// <summary>
    /// Jwt相关服务接口
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// 创建Token
        /// </summary>
        /// <returns></returns>
        Task<BaseResult<JwtAuthorizationDto>> CreateToken(UserInfoDto userInfo);
    }

    /// <summary>
    /// Jwt相关服务实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped)]
    public class JwtService : IJwtService
    {
        #region 初始化

        private readonly string JwtIssuer = string.Empty;
        private readonly string JwtAudience = string.Empty;
        private readonly string JwtSecurityKey = string.Empty;
        private readonly string JwtExpireMinutes = string.Empty;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            JwtIssuer = configuration["Jwt:Issuer"] ?? string.Empty;
            JwtAudience = configuration["Jwt:Audience"] ?? string.Empty;
            JwtSecurityKey = configuration["Jwt:SecurityKey"] ?? string.Empty;
            JwtExpireMinutes = configuration["Jwt:ExpireMinutes"] ?? "20";
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion 初始化

        public async Task<BaseResult<JwtAuthorizationDto>> CreateToken(UserInfoDto userInfo)
        {
            try
            {
                BaseResult<JwtAuthorizationDto> res = new BaseResult<JwtAuthorizationDto>();

                #region Token 创建

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecurityKey));

                DateTime authTime = DateTime.UtcNow;
                DateTime expireAt = authTime.AddMinutes(Convert.ToDouble(JwtExpireMinutes));

                // 将用户信息添加到 Claim 中
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

                // 完善用户信息
                IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.Name),
                new Claim(ClaimTypes.Role, ""),
                new Claim(ClaimTypes.Expiration, expireAt.ToString())
            };

                identity.AddClaims(claims);

                // 签发一个加密后的用户信息凭证，用来标识用户的身份
                _httpContextAccessor.HttpContext?.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims), // 创建声明信息
                    Issuer = JwtIssuer, // Jwt Token 的签发者
                    Audience = JwtAudience, // Jwt Token 接收者
                    Expires = expireAt,
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256), // 创建token
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                var jwtRes = new JwtAuthorizationDto
                {
                    UserId = 1,
                    Token = tokenHandler.WriteToken(token),
                    AuthTime = TimeHelper.GetTimestamp(authTime),
                    ExpireTime = TimeHelper.GetTimestamp(expireAt),
                    Success = true
                };

                #endregion Token 创建

                res.Data = jwtRes;
                res.Code = ResponseCodeEnum.Success;

                return res;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"Token创建失败:{ex.Message}");
                return await Task.FromResult(new BaseResult<JwtAuthorizationDto>(ResponseCodeEnum.InternalError, ex.Message));
            }
        }
    }
}