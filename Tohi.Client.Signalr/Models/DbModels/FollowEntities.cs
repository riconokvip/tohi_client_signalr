using System.ComponentModel.DataAnnotations.Schema;

namespace Tohi.Client.Signalr.Models.DbModels
{
    [Table("Follows")]
    public class FollowEntities : BaseEntity<string>
    {
        /// <summary>
        /// Id người dùng
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Trạng thái follow: true là follow, false là không follow
        /// </summary>
        public bool IsFollowed { get; set; }
    }
}
