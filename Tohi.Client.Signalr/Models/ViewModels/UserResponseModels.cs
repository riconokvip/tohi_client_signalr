using MessagePack;

namespace Tohi.Client.Signalr.Models.ViewModels
{
    [MessagePackObject]
    public class UserResponseModels
    {
        [Key("id")]
        public string Id { get; set; }

        /// <summary>
        /// Avatar của người dùng
        /// </summary>
        [Key("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// Tên người dùng
        /// </summary>
        [Key("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Họ và tên của người dùng
        /// </summary>
        [Key("fullName")]
        public string FullName { get; set; }

        /// <summary>
        /// Tên hiển thị của người dùng
        /// </summary>
        [Key("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Số kim cương hiện tại
        /// </summary>
        [Key("diamond")]
        public int Diamond { get; set; }
    }
}
