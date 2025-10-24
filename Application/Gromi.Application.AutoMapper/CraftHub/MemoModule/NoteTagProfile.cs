using AutoMapper;
using Gromi.Infra.Entity.Common.Attributes;
using Gromi.Infra.Entity.CraftHub.MemoModule.Dtos;
using Gromi.Infra.Repository.DbEntity.CraftHub.MemoModule;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.AutoMapper.CraftHub.MemoModule
{
    [AutoInject(ServiceLifetime.Scoped, "CraftHub")]
    public class NoteTagProfile : Profile
    {
        public NoteTagProfile()
        {
            CreateMap<NoteTagDto, NoteTag>();
            CreateMap<NoteTag, NoteTagDto>();
        }
    }
}