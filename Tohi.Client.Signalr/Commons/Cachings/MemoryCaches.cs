using Microsoft.Extensions.Caching.Distributed;

namespace Tohi.Client.Signalr.Commons.Cachings
{
    public static class MemoryCaches
    {
        public static DistributedCacheEntryOptions ExpiredTimeEntry = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };

        public static int ExpiredTime = 10;
    }
}
