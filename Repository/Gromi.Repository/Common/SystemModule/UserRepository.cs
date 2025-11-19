using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.SystemModule.Params;
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
        Task<UserInfo> GetUserInfoAsync(QueryUserParam param);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> ResetPassword(long id, string password);

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> BatchDeleteUserAsync(List<long> ids);
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

        public async Task<UserInfo> GetUserInfoAsync(QueryUserParam param)
        {
            var query = _fsql.Select<UserInfo>()
                .Include(u => u.UsersRoles);
            if (param.Id != null)
            {
                query.Where(u => u.Id == param.Id.Value);
            }
            if (!string.IsNullOrEmpty(param.Account))
            {
                query.Where(u => u.Account == param.Account);
            }
            if (!string.IsNullOrEmpty(param.UserName))
            {
                query.Where(u => u.UserName.Contains(param.UserName));
            }
            var userInfo = await query.FirstAsync();
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

        public async Task<bool> BatchDeleteUserAsync(List<long> ids)
        {
            using (var uow = _fsql.CreateUnitOfWork())
            {
                try
                {
                    var repo = uow.GetRepository<UserInfo>();

                    // 级联删除
                    // 删除用户下的标签、标签对应的日记和用户角色关系
                    var groups = await repo.Select
                        .IncludeMany(a => a.Tags, then => then.IncludeMany(tag => tag.Notes))
                        .IncludeMany(b => b.UsersRoles)
                        .Where(c => ids.Contains(c.Id))
                        .ToListAsync();
                    var delRes = await repo.DeleteAsync(groups);

                    uow.Commit();
                    return delRes > 0;
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}