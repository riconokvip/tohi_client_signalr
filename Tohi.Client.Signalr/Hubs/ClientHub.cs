using Microsoft.AspNetCore.SignalR;

namespace Tohi.Client.Signalr.Hubs
{
    public class ClientHub : Hub
    {
        private static long connections = 0;
        private readonly IServiceScopeFactory _scope;
        private readonly ILogger<ClientHub> _logger;
        private readonly IMapper _mapper;

        public ClientHub(IServiceScopeFactory scope, ILogger<ClientHub> logger, IMapper mapper)
        {
            _scope = scope;
            _logger = logger;
            _mapper = mapper;
        }


        /// <summary>
        /// Kết nối với socket
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            try
            {
                await base.OnConnectedAsync();
                connections++;
                var connectionId = Context.ConnectionId;
                var userId = Context.User.Identity.Name;
                
                // Kiểm tra dữ liệu người dùng hợp lệ và lưu vào cache
                if (userId != null)
                {
                    using (var scope = _scope.CreateScope())
                    {
                        var client = scope.ServiceProvider.GetRequiredService<IClientService>();
                        await client.GetClientUser(userId);
                    }
                }
            }
            catch (BaseException ex)
            {
                _logger.LogError($"[OnConnectedAsync]: handled exception, {ex.message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"[OnConnectedAsync]: unhandled exception, {ex.Message}");
            }
        }

        /// <summary>
        /// Ngắt kết nối với socket
        /// </summary>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                await base.OnDisconnectedAsync(exception);
                connections--;
                var connectionId = Context.ConnectionId;
                var userId = Context.User.Identity.Name;
            }
            catch (BaseException ex)
            {
                _logger.LogError($"[OnConnectedAsync]: handled exception, {ex.message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"[OnConnectedAsync]: unhandled exception, {ex.Message}");
            }
        }

        /// <summary>
        /// Tham gia phòng livestream
        /// </summary>
        /// <param name="group">Phòng livestream</param>
        /// <returns></returns>
        public async Task JoinGroup(string group)
        {
            try
            {
                var connectionId = Context.ConnectionId;
                var userId = Context.User.Identity.Name;

                // Xử lý tham gia phòng livestream
                using (var scope = _scope.CreateScope())
                {
                    var client = scope.ServiceProvider.GetRequiredService<IClientService>();
                    var cache = scope.ServiceProvider.GetRequiredService<IDistributedCacheExtensionService>();

                    // Lấy dữ liệu người dùng và stream
                    var user = await client.GetClientUser(userId);
                    var stream = await client.GetClientStream(group);

                    // Cập nhật phòng tham gia cho client
                    var clientLivestreamKey = ClientKeys.Livestream(connectionId);
                    var clientLivestream = cache.TryGetValue<string>(clientLivestreamKey, out var _clientLivestream);
                    if (clientLivestream)
                    {
                        if (_clientLivestream != group)
                        {
                            // Xóa client khỏi phòng
                            await Groups.RemoveFromGroupAsync(connectionId, _clientLivestream);
                            // Cập nhật lượt xem của phòng cũ
                            await client.DownViewerForStreamByGroup(_clientLivestream, userId);
                        }
                    }
                    await cache.SetAsync(clientLivestreamKey, group);
                    await Groups.AddToGroupAsync(connectionId, group);

                    // Thông báo người dùng mới tham gia phòng và tăng lượt xem của phòng mới
                    var isNotify = await client.IncreaseViewerForStreamByGroup(stream, group, userId);
                    if (isNotify && connections > 0)
                        await Clients.Group(group).SendAsync(EventEnums.RecvRoomNotify.ToString(), _mapper.Map<UserResponseModels>(user));
                }
            }
            catch (BaseException ex)
            {
                _logger.LogError($"[JoinGroup]: handled exception, {ex.message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"[JoinGroup]: unhandled exception, {ex.Message}");
            }
        }
    }
}
