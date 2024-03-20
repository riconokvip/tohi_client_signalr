using System.ComponentModel.DataAnnotations.Schema;

namespace Tohi.Client.Signalr.Models.DbModels
{
    [Table("Streams")]
    public class StreamEntities : BaseEntity<string>
    {
        /// <summary>
        /// Tiêu đề phòng livestream
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Nội dung phòng livestream
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Id của người dùng
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Trạng thái phát livestream
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Số lượt xem cao nhất
        /// </summary>
        public int MaxViewers { get; set; }

        /// <summary>
        /// Số lượt xem hiện tại
        /// </summary>
        public int CurrentViewers { get; set; }

        /// <summary>
        /// Trạng thái chặn livestream
        /// </summary>
        public bool IsViolated { get; set; }

        /// <summary>
        /// Lý do chặn livestream
        /// </summary>
        public string Reason { get; set; }
    }
}
