using Microsoft.EntityFrameworkCore;

namespace Tohi.Client.Signalr.Repositories
{
    public class UserRepository(DbContext context) : BaseRepository<UserEntities>(context)
    {
    }
}
