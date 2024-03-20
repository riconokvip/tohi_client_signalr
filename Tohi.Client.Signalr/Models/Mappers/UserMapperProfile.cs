namespace Tohi.Client.Signalr.Models.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserEntities, UserModels>().ReverseMap();

            CreateMap<UserModels, UserResponseModels>().ReverseMap();
        }
    }
}
