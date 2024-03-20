using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using SQLActionEnums = TableDependency.SqlClient.Base.Enums.ChangeType;

namespace Tohi.Client.Signalr.SQLDependencies
{
    public class CdnLiveDependency : BaseDependency
    {
        SqlTableDependency<CdnLiveEntities> _dependency;
        private readonly IServiceScopeFactory _scope;
        private readonly ILogger<CdnLiveDependency> _logger;
        private readonly IMapper _mapper;

        public CdnLiveDependency(IServiceScopeFactory scope, ILogger<CdnLiveDependency> logger, IMapper mapper)
        {
            _scope = scope;
            _logger = logger;
            _mapper = mapper;
        }

        public void InitSqlDependency(string connectionString)
        {
            try
            {
                _dependency = new SqlTableDependency<CdnLiveEntities>(connectionString);
                _dependency.OnChanged += OnCdnLiveChanged;
                _dependency.OnError += OnError;
                _dependency.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void OnCdnLiveChanged(object sender, RecordChangedEventArgs<CdnLiveEntities> e)
        {
            try
            {
                if (e.ChangeType == SQLActionEnums.Update)
                {
                    if (e.Entity == null)
                        throw new BaseException(ErrorEnums.CdnLiveNotFoundWhileChange);

                    // Cập nhật dữ liệu đường dẫn livestream vào cache
                    using (var scope = _scope.CreateScope())
                    {
                        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCacheExtensionService>();
                        var userHlsSrcKey = UserKeys.HlsSrc(e.Entity.UserId);
                        var userHlsSrc = cache.TryGetValue<string>(userHlsSrcKey, out _);
                        if (userHlsSrc)
                            await cache.SetAsync(userHlsSrcKey, e.Entity.HlsPlayLink, MemoryCaches.ExpiredTimeEntry);
                    }
                }
            }
            catch (BaseException ex)
            {
                _logger.LogError($"[CdnLiveDependency]: handled exception, {ex.message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[CdnLiveDependency]: unhandled exception, {ex.Message}");
            }
        }

        private void OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            _logger.LogError($"[CdnLiveDependency]: failed connect, {e.Error}");
        }
    }
}
