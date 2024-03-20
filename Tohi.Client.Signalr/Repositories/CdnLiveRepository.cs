using Microsoft.EntityFrameworkCore;

namespace Tohi.Client.Signalr.Repositories
{
    public class CdnLiveRepository(DbContext context) : BaseRepository<CdnLiveEntities>(context)
    {
    }
}
