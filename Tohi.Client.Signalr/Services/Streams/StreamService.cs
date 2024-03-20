using Microsoft.EntityFrameworkCore;

namespace Tohi.Client.Signalr.Services.Streams
{
    public interface IStreamService
    {
        /// <summary>
        /// Lấy dữ liệu phòng livestream và lưu vào cache
        /// </summary>
        /// <param name="group">Phòng livestream</param>
        /// <returns></returns>
        Task<StreamEntities> GetStream(string group);

        /// <summary>
        /// Giảm lượt xem livestream vào database và cache
        /// </summary>
        /// <param name="entity">Đối tượng stream</param>
        /// <returns></returns>
        Task DownViewerStream(StreamEntities entity);

        /// <summary>
        /// Tăng lượt xem livestream vào database và cache
        /// </summary>
        /// <param name="entity">Đối tượng stream</param>
        /// <returns></returns>
        Task IncreaseViewerStream(StreamEntities entity);
    }

    public class StreamService : IStreamService
    {
        private readonly StreamRepository _repo;
        private readonly IDistributedCacheExtensionService _cache;
        private readonly IMapper _mapper;

        public StreamService(ApplicationDbContext applicationDb, IDistributedCacheExtensionService cache, IMapper mapper)
        {
            _repo = new StreamRepository(applicationDb);
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<StreamEntities> GetStream(string group)
        {
            if (group != null)
            {
                var stream = await _repo.UseQueries().FirstOrDefaultAsync(_ => _.UserId == group);
                if (stream != null)
                {
                    if (stream.IsDeleted)
                        throw new BaseException(ErrorEnums.LivestreamNotFound);
                    if (stream.IsViolated)
                        throw new BaseException(ErrorEnums.LivestreamIsBanned);

                    // Lưu thông tin livestream vào cache
                    var streamInfomation = LivestreamKeys.Information(group);
                    await _cache.SetAsync(streamInfomation, stream, MemoryCaches.ExpiredTimeEntry);
                    return stream;
                }
                else
                    throw new BaseException(ErrorEnums.LivestreamNotFound);
            }
            else
                throw new BaseException(ErrorEnums.LivestreamNotFound);
        }

        public async Task DownViewerStream(StreamEntities entity)
        {
            if (entity != null)
            {
                if (entity.CurrentViewers < 1)
                    throw new BaseException(ErrorEnums.LivestreamFailCountViewer);

                // Giảm lượt xem vào database nếu livestream đang online
                if (entity.Status == (int)LivestreamEnums.Online)
                {
                    entity.CurrentViewers = entity.CurrentViewers - 1;
                    await _repo.Update(entity);
                }

                // Giảm lượt xem hiện tại vào cache, nếu lượt xem bằng 0 thì xóa cache
                var streamViewerKey = LivestreamKeys.Viewer(entity.UserId);
                var streamViewer = _cache.TryGetValue<int>(streamViewerKey, out var _streamViewer);
                if (streamViewer)
                {
                    if (_streamViewer > 1)
                        await _cache.SetAsync(streamViewerKey, _streamViewer - 1);
                    else if (_streamViewer == 1)
                        await _cache.RemoveAsync(streamViewerKey);
                    else
                        throw new BaseException(ErrorEnums.UserFailCountJoin);
                }
                else
                    throw new BaseException(ErrorEnums.UserNotFoundCountJoin);
            }
            else
                throw new BaseException(ErrorEnums.LivestreamNotFound);
        }

        public async Task IncreaseViewerStream(StreamEntities entity)
        {
            if (entity != null)
            {
                // Tăng lượt xem vào database nếu livestream đang online
                if (entity.Status == (int)LivestreamEnums.Online)
                {
                    if (entity.MaxViewers < entity.CurrentViewers + 1)
                        entity.MaxViewers = entity.CurrentViewers + 1;
                    entity.CurrentViewers = entity.CurrentViewers + 1;
                    await _repo.Update(entity);
                }

                // Tăng lượt xem hiện tại vào cache
                var streamViewerKey = LivestreamKeys.Viewer(entity.UserId);
                var streamViewer = _cache.TryGetValue<int>(streamViewerKey, out var _streamViewer);
                if (streamViewer)
                    await _cache.SetAsync(streamViewerKey, _streamViewer + 1);
                else
                    await _cache.SetAsync(streamViewerKey, 1);
            }
            else
                throw new BaseException(ErrorEnums.LivestreamNotFound);
        }
    }
}
