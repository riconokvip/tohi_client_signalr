using System.ComponentModel.DataAnnotations.Schema;

namespace Tohi.Client.Signalr.Models.DbModels
{
    [Table("AspNetUsers")]
    public class UserEntities : BaseEntity<string>
    {
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
