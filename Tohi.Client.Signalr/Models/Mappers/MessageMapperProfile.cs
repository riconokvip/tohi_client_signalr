namespace Tohi.Client.Signalr.Models.Mappers
{
    public class MessageMapperProfile : Profile
    {
        public MessageMapperProfile()
        {
            CreateMap<MessageEntities, MessageResponseModels>().ReverseMap();
        }
    }
}
