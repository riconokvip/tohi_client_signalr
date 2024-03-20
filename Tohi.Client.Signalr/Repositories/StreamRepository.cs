using Microsoft.EntityFrameworkCore;

namespace Tohi.Client.Signalr.Repositories
{
    public class StreamRepository(DbContext context) : BaseRepository<StreamEntities>(context)
    {
    }
}
