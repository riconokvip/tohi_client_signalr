namespace Tohi.Client.Signalr.Models.Mappers
{
    public class StreamMapperProfile : Profile
    {
        public StreamMapperProfile()
        {
            CreateMap<StreamEntities, StreamResponseModels>()
                .ForMember(dest => dest.TypeStream, act => act.MapFrom(src => src.Status))
                .ReverseMap();

            CreateMap<StreamEntities, StreamModels>().ReverseMap();
        }
    }
}
