using System.ComponentModel.DataAnnotations.Schema;

namespace Tohi.Client.Signalr.Models.DbModels
{
    [Table("Messages")]
    public class MessageEntities : BaseEntity<string>
    {
        /// <summary>
        /// Id livestream
        /// </summary>
        public string StreamId { get; set; }

        /// <summary>
        /// Nội dung chat
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Loại tin nhắn
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Id phòng chat
        /// </summary>
        public string RoomId { get; set; }
    }
}
