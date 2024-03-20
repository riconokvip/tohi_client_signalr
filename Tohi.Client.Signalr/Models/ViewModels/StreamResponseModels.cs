using MessagePack;

namespace Tohi.Client.Signalr.Models.ViewModels
{
    [MessagePackObject]
    public class StreamResponseModels
    {
        /// <summary>
        /// Chủ sở hữu livestream
        /// </summary>
        [Key("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Tiêu đề phòng livestream
        /// </summary>
        [Key("title")]
        public string Title { get; set; }

        /// <summary>
        /// Nội dung phòng livestream
        /// </summary>
        [Key("description")]
        public string Description { get; set; }

        /// <summary>
        /// Link phát livestream
        /// </summary>
        [Key("hlsSrc")]
        public string HlsSrc { get; set; }

        /// <summary>
        /// Trạng thái livestream
        /// </summary>
        [Key("typeStream")]
        public int TypeStream { get; set; }

        /// <summary>
        /// Thông báo trạng thái livestream
        /// </summary>
        [Key("message")]
        public string Message => TypeStream == 0 ? "Bắt đầu stream" : "Kết thúc stream";

        /// <summary>
        /// Số lượt xem livestream
        /// </summary>
        [Key("viewer")]
        public int Viewer { get; set; }

        /// <summary>
        /// Trạng thái chặn livestream
        /// </summary>
        [Key("isViolated")]
        public bool IsViolated { get; set; }

        /// <summary>
        /// Lý do chặn livestream
        /// </summary>
        [Key("reason")]
        public string Reason { get; set; }
    }
}
