using AutoMapper;
using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.SystemModule.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.AutoMapper.Common.SystemModule
{
    [AutoInject(ServiceLifetime.Scoped)]
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInfo, UserInfoDto>();
            CreateMap<UserInfoDto, UserInfo>();
        }
    }
}