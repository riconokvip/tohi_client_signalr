using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace Tohi.Client.Signalr.SQLDependencies
{
    public class UserDependency : BaseDependency
    {
        SqlTableDependency<UserEntities> _dependency;
        private readonly IServiceScopeFactory _scope;
        private readonly ILogger<UserDependency> _logger;
        private readonly IMapper _mapper;

        public UserDependency(IServiceScopeFactory scope, ILogger<UserDependency> logger, IMapper mapper)
        {
            _scope = scope;
            _logger = logger;
            _mapper = mapper;
        }

        public void InitSqlDependency(string connectionString)
        {
            try
            {
                _dependency = new SqlTableDependency<UserEntities>(connectionString);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void OnUserChanged(object sender, RecordChangedEventArgs<UserEntities> e)
        {
            try
            {
                if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Update)
                {
                    if (e.Entity == null)
                        throw new BaseException(ErrorEnums.UserNotFoundWhileChange);

                    // Cập nhật dữ liệu người dùng vào cache
                    using (var scope = _scope.CreateScope())
                    {
                        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCacheExtensionService>();
                        var userInformationKey = UserKeys.Information(e.Entity.Id);
                        var userInformation = cache.TryGetValue<UserModels>(userInformationKey, out _);
                        if (userInformation)
                            await cache.SetAsync(userInformationKey, _mapper.Map<UserModels>(e.Entity), MemoryCaches.ExpiredTimeEntry);
                    }
                }
            }
            catch (BaseException ex)
            {
                _logger.LogError($"[UserDependency]: handled exception, {ex.message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UserDependency]: unhandled exception, {ex.Message}");
            }
        }

        private void OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            _logger.LogError($"[UserDependency]: failed connect, {e.Error}");
        }
    }
}
