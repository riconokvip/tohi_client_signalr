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
        Task<StreamModels> GetStream(string group);

        /// <summary>
        /// Cập nhật lượt xem livestream vào database và cache
        /// </summary>
        /// <param name="entity">Đối tượng stream</param>
        /// <param name="viewer">Số lượt xem thay đổi</param>
        /// <returns></returns>
        Task UpdateViewerStream(StreamEntities entity, int viewer = 0);
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

        public async Task<StreamModels> GetStream(string group)
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
                    var streamCacheModel = _mapper.Map<StreamModels>(stream);
                    await _cache.SetAsync(streamInfomation, streamCacheModel);
                    return streamCacheModel;
                }
                else
                    throw new BaseException(ErrorEnums.LivestreamNotFound);
            }
            else
                throw new BaseException(ErrorEnums.LivestreamNotFound);
        }

        public async Task UpdateViewerStream(StreamEntities entity, int viewer = 0)
        {
            if (entity != null)
            {
                if (entity.CurrentViewers + viewer < 0)
                    throw new BaseException(ErrorEnums.LivestreamFailCountViewer);

                // Cập nhật lượt xem vào database
                if (entity.Status == (int)LivestreamEnums.Online)
                {
                    if (entity.MaxViewers < entity.CurrentViewers + viewer)
                        entity.MaxViewers = entity.CurrentViewers + viewer;
                    entity.CurrentViewers = entity.CurrentViewers + viewer;
                    await _repo.Update(entity);
                }

                // Cập nhật lượt xem hiện tai vào cache
                var streamViewerKey = LivestreamKeys.Viewer(entity.UserId);
                await _cache.SetAsync(streamViewerKey, entity.CurrentViewers + viewer);
            }
            else
                throw new BaseException(ErrorEnums.LivestreamNotFound);
        }
    }
}
