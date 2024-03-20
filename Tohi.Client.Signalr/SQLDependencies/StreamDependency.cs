using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using SQLActionEnums = TableDependency.SqlClient.Base.Enums.ChangeType;
using Tohi.Client.Signalr.Hubs;

namespace Tohi.Client.Signalr.SQLDependencies
{
    public class StreamDependency : BaseDependency
    {
        SqlTableDependency<StreamEntities> _dependency;
        private readonly ClientHub _hub;
        private readonly IServiceScopeFactory _scope;
        private readonly ILogger<StreamDependency> _logger;
        private readonly IMapper _mapper;

        public StreamDependency(ClientHub hub, IServiceScopeFactory scope, ILogger<StreamDependency> logger, IMapper mapper)
        {
            _hub = hub;
            _scope = scope;
            _logger = logger;
            _mapper = mapper;
        }

        public void InitSqlDependency(string connectionString)
        {
            try
            {
                _dependency = new SqlTableDependency<StreamEntities>(connectionString);
                _dependency.OnChanged += OnStreamChanged;
                _dependency.OnError += OnError;
                _dependency.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void OnStreamChanged(object sender, RecordChangedEventArgs<StreamEntities> e)
        {
            try
            {
                if (e.ChangeType == SQLActionEnums.Insert)
                {
                    if (e.Entity == null)
                        throw new BaseException(ErrorEnums.LivestreamNotFoundWhileChange);
                    await _hub.SendStreamChangeNotify(e.Entity);
                    using (var scope = _scope.CreateScope())
                    {
                        var stream = scope.ServiceProvider.GetRequiredService<IStreamService>();
                        await stream.UpdateViewerStream(e.Entity);
                    }
                }
                if (e.ChangeType == SQLActionEnums.Update)
                {
                    if (e.Entity == null)
                        throw new BaseException(ErrorEnums.LivestreamNotFoundWhileChange);
                    await _hub.SendStreamChangeNotify(e.Entity);
                }
            }
            catch (BaseException ex)
            {
                _logger.LogError($"[StreamDependency]: handled exception, {ex.message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[StreamDependency]: unhandled exception, {ex.Message}");
            }
        }

        private void OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            _logger.LogError($"[StreamDependency]: failed connect: {e.Error}");
        }
    }
}
