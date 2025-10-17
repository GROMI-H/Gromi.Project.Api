namespace Gromi.Infra.Entity.CraftHub.AuthModule.Dtos
{
    /// <summary>
    /// Jwt授权Dto
    /// </summary>
    public class JwtAuthorizationDto
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// 是否授权成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 授权时间
        /// </summary>
        public long AuthTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long ExpireTime { get; set; }
    }
}