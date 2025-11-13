using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.Common.SystemModule
{
    /// <summary>
    /// 系统用户仓储接口
    /// </summary>
    public interface IUserRepository : IRepository<UserInfo>
    {
        /// <summary>
        /// 密码校验
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<long> VerifyPassword(string account, string password);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserInfo> GetUserInfoAsync(long id);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<UserInfo> GetUserInfoAsync(string account);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> ResetPassword(long id, string password);
    }

    /// <summary>
    /// 系统用户仓储实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped)]
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {
        public UserRepository(IMultiFreeSqlManager<DbKey> multiFreeSql) : base(multiFreeSql, DbKey.DbCraftHub)
        {
        }

        public async Task<UserInfo> GetUserInfoAsync(long id)
        {
            var userInfo = await _fsql.Select<UserInfo>()
                .Where(u => u.Id == id)
                .Include(u => u.UsersRoles)
                .FirstAsync();
            return userInfo;
        }

        public async Task<long> VerifyPassword(string account, string password)
        {
            var userInfo = await _fsql.GetRepository<UserInfo>()
                .Where(u => u.Account == account && u.Password == password)
                .FirstAsync();
            return userInfo != null ? userInfo.Id : -1;
        }

        public async Task<bool> ResetPassword(long id, string password)
        {
            var res = await _fsql.Update<UserInfo>().Set(item => item.Password, password).ExecuteAffrowsAsync();
            return res > 0 ? true : false;
        }

        public async Task<UserInfo> GetUserInfoAsync(string account)
        {
            return await _fsql.Select<UserInfo>().Where(u => u.Account == account).FirstAsync();
        }
    }
}