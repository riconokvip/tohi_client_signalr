namespace Tohi.Client.Signalr.Models.CacheModels
{
    public class UserModels
    {
        public string Id { get; set; }

        /// <summary>
        /// Avatar của người dùng
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Họ và tên của người dùng
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Tên hiển thị của người dùng
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Số kim cương hiện tại
        /// </summary>
        public int Diamond { get; set; }
    }
}
