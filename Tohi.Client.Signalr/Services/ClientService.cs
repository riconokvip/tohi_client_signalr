namespace Tohi.Client.Signalr.Services
{
    public interface IClientService
    {
        /// <summary>
        /// Lấy dữ liệu người dùng và lưu vào cache
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        Task<UserModels> GetClientUser(string userId);


        /// <summary>
        /// Lấy dữ liệu livestream và lưu vào cache
        /// </summary>
        /// <param name="group">Phòng livestream</param>
        /// <returns></returns>
        Task<StreamEntities> GetClientStream(string group);


        /// <summary>
        /// Cập nhật số lượt xem cho phòng livestream
        /// </summary>
        /// <param name="group"></param>
        /// <param name="viewer"></param>
        /// <returns></returns>
        Task UpdateViewerForStreamByGroup(string group, int viewer = 0);


        /// <summary>
        /// Cập nhật số lượt xem cho phòng livestream
        /// </summary>
        /// <param name="group"></param>
        /// <param name="viewer"></param>
        /// <returns></returns>
        Task UpdateViewerForStream(StreamEntities stream, int viewer = 0);
    }

    public class ClientService : IClientService
    {
        private readonly IUserService _userService;
        private readonly IStreamService _streamService;
        private readonly IDistributedCacheExtensionService _cache;
        private readonly IMapper _mapper;

        public ClientService(IUserService userService, IStreamService streamService, IDistributedCacheExtensionService cache)
        {
            _userService = userService;
            _streamService = streamService;
            _cache = cache;
        }

        public async Task<UserModels> GetClientUser(string userId)
        {
            var userInformationKey = UserKeys.Information(userId);
            var user = _cache.TryGetValue<UserModels>(userInformationKey, out var _user);
            if (user)
                return _user;
            else
                return await _userService.GetUser(userId);
        }

        public async Task<StreamEntities> GetClientStream(string group)
        {
            var streamInformationKey = LivestreamKeys.Information(group);
            var stream = _cache.TryGetValue<StreamEntities>(streamInformationKey, out var _stream);
            if (stream) 
                return _stream;
            else
                return await _streamService.GetStream(group);
        }

        public async Task UpdateViewerForStreamByGroup(string group, int viewer = 0)
        {
            var stream = await _streamService.GetStream(group);
            await _streamService.UpdateViewerStream(stream, viewer);
        }

        public async Task UpdateViewerForStream(StreamEntities stream, int viewer = 0)
        {
            await _streamService.UpdateViewerStream(stream, viewer);
        }
    }
}
