using System.ComponentModel.DataAnnotations.Schema;

namespace Tohi.Client.Signalr.Models.DbModels
{
    [Table("CdnLives")]
    public class CdnLiveEntities : BaseEntity<string>
    {
        /// <summary>
        /// Id của người dùng
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Link phát livestream
        /// </summary>
        public string HlsPlayLink { get; set; }
    }
}
