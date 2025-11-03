using AutoMapper;
using Gromi.Infra.DataAccess.DbEntity.CraftHub.MemoModule;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.CraftHub.MemoModule.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.AutoMapper.CraftHub.MemoModule
{
    [AutoInject(ServiceLifetime.Scoped, "CraftHub")]
    public class NoteRecordProfile : Profile
    {
        public NoteRecordProfile()
        {
            CreateMap<NoteRecordDto, NoteRecord>();
            CreateMap<NoteRecord, NoteRecordDto>();
        }
    }
}