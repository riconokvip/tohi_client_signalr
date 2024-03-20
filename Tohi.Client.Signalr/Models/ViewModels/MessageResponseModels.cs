using MessagePack;

namespace Tohi.Client.Signalr.Models.ViewModels
{
    [MessagePackObject]
    public class MessageResponseModels
    {
        /// <summary>
        /// Id phòng livestream
        /// </summary>
        [Key("streamId")]
        public string StreamId { get; set; }

        /// <summary>
        /// Nội dung chat
        /// </summary>
        [Key("message")]
        public string Message { get; set; }

        /// <summary>
        /// Loại tin nhắn
        /// </summary>
        [Key("type")]
        public int Type { get; set; }

        /// <summary>
        /// Phòng livestream
        /// </summary>
        [Key("roomId")]
        public string RoomId { get; set; }
    }
}
