using Microsoft.EntityFrameworkCore;

namespace Tohi.Client.Signalr.Services.Streams
{
    public interface ICdnLiveService
    {
        /// <summary>
        /// Lấy dữ liệu đường dẫn phát livestream và lưu vào cache
        /// </summary>
        /// <param name="group">Phòng livestream</param>
        /// <returns></returns>
        Task<string> GetCdnlive(string group);
    }

    public class CdnLiveService : ICdnLiveService
    {
        private readonly CdnLiveRepository _repo;
        private readonly IDistributedCacheExtensionService _cache;

        public CdnLiveService(ApplicationDbContext applicationDb, IDistributedCacheExtensionService cache)
        {
            _repo = new CdnLiveRepository(applicationDb);
            _cache = cache;
        }

        public async Task<string> GetCdnlive(string group)
        {
            if (group != null)
            {
                var cdnLive = await _repo.UseQueries().FirstOrDefaultAsync(_ => _.UserId == group);
                if (cdnLive != null)
                {
                    // Cập nhật hls vào cache
                    var userHlsSrcKey = UserKeys.HlsSrc(group);
                    await _cache.SetAsync(userHlsSrcKey, cdnLive.HlsPlayLink, MemoryCaches.ExpiredTimeEntry);
                    return cdnLive.HlsPlayLink;
                }
                else
                    throw new BaseException(ErrorEnums.CdnliveNotFound);
            }
            else
                throw new BaseException(ErrorEnums.LivestreamNotFound);
        }
    }
}
