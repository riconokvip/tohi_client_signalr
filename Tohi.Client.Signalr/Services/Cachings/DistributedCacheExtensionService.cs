using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Tohi.Client.Signalr.Services.Cachings
{
    public interface IDistributedCacheExtensionService
    {
        /// <summary>
        /// Lưu trữ dữ liệu vào cache
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="key">Khóa</param>
        /// <param name="value">Giá trị</param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value);

        /// <summary>
        /// Lưu trữ dữ liệu vào cache với thời gian hết hạn
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="key">Khóa</param>
        /// <param name="value">Giá trị</param>
        /// <param name="options">Thời gian hết hạn</param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options);

        /// <summary>
        /// Kiểm tra sự tồn tại dữ liệu của một khóa trong cache
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="key">Khóa</param>
        /// <param name="value">Giá trị</param>
        /// <returns></returns>
        bool TryGetValue<T>(string key, out T? value);

        /// <summary>
        /// Xóa dữ liệu và khóa khỏi cache
        /// </summary>
        /// <param name="key">Khóa</param>
        /// <returns></returns>
        Task RemoveAsync(string key);
    }
    public class DistributedCacheExtensionService : IDistributedCacheExtensionService
    {
        private readonly IDistributedCache _cache;
        public DistributedCacheExtensionService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public Task SetAsync<T>(string key, T value)
        {
            return SetAsync(key, value, new DistributedCacheEntryOptions());
        }

        public Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, GetJsonSerializerOptions()));
            return _cache.SetAsync(key, bytes, options);
        }

        public Task RemoveAsync(string key)
        {
            return _cache.RemoveAsync(key);
        }

        public bool TryGetValue<T>(string key, out T? value)
        {
            var val = _cache.Get(key);
            value = default;
            if (val == null) return false;
            value = JsonSerializer.Deserialize<T>(val, GetJsonSerializerOptions());
            return true;
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
    }
}
