using AutoMapper;
using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.Entity.Common.AuthModule.Dtos;
using Gromi.Infra.Entity.Common.AuthModule.Params;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.AutoMapper.Common.LoginModule
{
    [AutoInject(ServiceLifetime.Scoped)]
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<RegisterParam, UserInfo>();
            CreateMap<UserInfo, LoginResponse>();
        }
    }
}