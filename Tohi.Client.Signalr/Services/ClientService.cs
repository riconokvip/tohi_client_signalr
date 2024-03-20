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
    }

    public class ClientService : IClientService
    {
        private readonly IUserService _userService;

        public ClientService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserModels> GetClientUser(string userId)
        {
            return await _userService.GetUser(userId);
        }
    }
}
