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
        /// Giảm số lượt xem cho phòng livestream
        /// </summary>
        /// <param name="group"></param>
        /// <param name="viewer"></param>
        /// <returns></returns>
        Task DownViewerForStreamByGroup(string group, string userId);


        /// <summary>
        /// Tăng số lượt xem cho phòng livestream
        /// </summary>
        /// <param name="group"></param>
        /// <param name="viewer"></param>
        /// <returns></returns>
        Task IncreaseViewerForStreamByGroup(string group, string userId);


        /// <summary>
        /// Giảm số lượt xem cho phòng livestream
        /// </summary>
        /// <param name="group"></param>
        /// <param name="viewer"></param>
        /// <returns></returns>
        Task DownViewerForStreamByGroup(StreamEntities stream, string group, string userId);


        /// <summary>
        /// Tăng số lượt xem cho phòng livestream
        /// </summary>
        /// <param name="group"></param>
        /// <param name="viewer"></param>
        /// <returns></returns>
        Task<bool> IncreaseViewerForStreamByGroup(StreamEntities stream, string group, string userId);
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

        public async Task DownViewerForStreamByGroup(string group, string userId)
        {
            // Nếu là người dùng
            if (userId != null)
            {
                var userJoinKey = UserKeys.CountJoinGroup(userId, group);
                var userJoin = _cache.TryGetValue<int>(userJoinKey, out var _userJoin);
                if (userJoin)
                {
                    if (_userJoin == 1)
                    {
                        var stream = await _streamService.GetStream(group);
                        await _streamService.DownViewerStream(stream);
                        await _cache.RemoveAsync(userJoinKey);
                    }
                    else if (_userJoin > 1)
                        await _cache.SetAsync(userJoinKey, _userJoin - 1);
                    else
                        throw new BaseException(ErrorEnums.UserFailCountJoin);
                }
                else
                    throw new BaseException(ErrorEnums.UserNotFoundCountJoin);
            }
            // Nếu là người dùng ẩn danh
            else
            {
                var stream = await _streamService.GetStream(group);
                await _streamService.DownViewerStream(stream);
            }
        }

        public async Task IncreaseViewerForStreamByGroup(string group, string userId)
        {
            // Nếu là người dùng
            if (userId != null)
            {
                var userJoinKey = UserKeys.CountJoinGroup(userId, group);
                var userJoin = _cache.TryGetValue<int>(userJoinKey, out var _userJoin);
                if (userJoin)
                {
                    await _cache.SetAsync(userJoinKey, _userJoin + 1);
                }    
                else
                {
                    var stream = await _streamService.GetStream(group);
                    await _streamService.IncreaseViewerStream(stream);
                    await _cache.SetAsync(userJoinKey, 1);
                }   
            }
            // Nếu là người dùng ẩn danh
            else
            {
                var stream = await _streamService.GetStream(group);
                await _streamService.IncreaseViewerStream(stream);
            }
        }

        public async Task DownViewerForStreamByGroup(StreamEntities stream, string group, string userId)
        {
            // Nếu là người dùng
            if (userId != null)
            {
                var userJoinKey = UserKeys.CountJoinGroup(userId, group);
                var userJoin = _cache.TryGetValue<int>(userJoinKey, out var _userJoin);
                if (userJoin)
                {
                    if (_userJoin == 1)
                    {
                        await _streamService.DownViewerStream(stream);
                        await _cache.RemoveAsync(userJoinKey);
                    }
                    else if (_userJoin > 1)
                        await _cache.SetAsync(userJoinKey, _userJoin - 1);
                    else
                        throw new BaseException(ErrorEnums.UserFailCountJoin);
                }
                else
                    throw new BaseException(ErrorEnums.UserNotFoundCountJoin);
            }
            // Nếu là người dùng ẩn danh
            else
            {
                await _streamService.DownViewerStream(stream);
            }
        }

        public async Task<bool> IncreaseViewerForStreamByGroup(StreamEntities stream, string group, string userId)
        {
            // Nếu là người dùng
            if (userId != null)
            {
                var userJoinKey = UserKeys.CountJoinGroup(userId, group);
                var userJoin = _cache.TryGetValue<int>(userJoinKey, out var _userJoin);
                if (userJoin)
                {
                    await _cache.SetAsync(userJoinKey, _userJoin + 1);
                }
                else
                {
                    await _streamService.IncreaseViewerStream(stream);
                    await _cache.SetAsync(userJoinKey, 1);
                    return true;
                }
            }
            // Nếu là người dùng ẩn danh
            else
            {
                await _streamService.IncreaseViewerStream(stream);
            }
            return false;
        }
    }
}
