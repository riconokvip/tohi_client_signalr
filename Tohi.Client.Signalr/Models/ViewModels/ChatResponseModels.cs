using MessagePack;

namespace Tohi.Client.Signalr.Models.ViewModels
{
    [MessagePackObject]
    public class ChatResponseModels
    {
        /// <summary>
        /// Id tin nhắn
        /// </summary>
        [Key("id")]
        public string Id { get; set; }

        /// <summary>
        /// Thông tin người dùng
        /// </summary>
        [Key("user")]
        public UserResponseModels User { get; set; }

        /// <summary>
        /// Thông tin tin nhắn
        /// </summary>
        [Key("message")]
        public MessageResponseModels Message { get; set; }
    }
}
