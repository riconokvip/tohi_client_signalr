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
    }
}
