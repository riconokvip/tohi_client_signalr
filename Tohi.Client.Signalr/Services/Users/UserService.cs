using Microsoft.EntityFrameworkCore;

namespace Tohi.Client.Signalr.Services.Users
{
    public interface IUserService
    {
        /// <summary>
        /// Lấy dữ liệu người dùng
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        Task<UserModels> GetUser(string userId);
    }

    public class UserService : IUserService
    {
        private readonly UserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext applicationDb, IMapper mapper)
        {
            _repo = new UserRepository(applicationDb);
            _mapper = mapper;
        }

        public async Task<UserModels> GetUser(string userId)
        {
            if (userId != null)
            {
                var user = await _repo.UseQueries().FirstOrDefaultAsync(_ => _.Id == userId);
                if (user == null)
                    throw new BaseException(ErrorEnums.UserNotFound);
                else if (user.IsDeleted)
                    throw new BaseException(ErrorEnums.UserIsBanned);
                else
                    return _mapper.Map<UserModels>(user);
            }
            else
                throw new BaseException(ErrorEnums.UserNotFound);
        }
    }
}
