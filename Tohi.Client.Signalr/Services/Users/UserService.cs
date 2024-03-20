using Microsoft.EntityFrameworkCore;

namespace Tohi.Client.Signalr.Services.Users
{
    public interface IUserService
    {
        /// <summary>
        /// Lấy dữ liệu người dùng và lưu vào cache
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        Task<UserModels> GetUser(string userId);
    }

    public class UserService : IUserService
    {
        private readonly UserRepository _repo;
        private readonly IDistributedCacheExtensionService _cache;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext applicationDb, IDistributedCacheExtensionService cache, IMapper mapper)
        {
            _repo = new UserRepository(applicationDb);
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<UserModels> GetUser(string userId)
        {
            if (userId != null)
            {
                var user = await _repo.UseQueries().FirstOrDefaultAsync(_ => _.Id == userId);
                if (user == null)
                    throw new BaseException(ErrorEnums.UserNotFound);
                if (user.IsDeleted)
                    throw new BaseException(ErrorEnums.UserIsBanned);

                // Cập nhật dữ liệu người dùng vào cache
                var userCacheModel = _mapper.Map<UserModels>(user);
                var userInformationKey = UserKeys.Information(userId);
                await _cache.SetAsync(userInformationKey, userCacheModel);
                return userCacheModel;
            }
            else
                throw new BaseException(ErrorEnums.UserNotFound);
        }
    }
}
