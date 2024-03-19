using System.ComponentModel.DataAnnotations;

namespace Tohi.Client.Signalr.Commons.Enums
{
    public enum EventEnums
    {
        /* User Events */
        [Display(Name = "Thông báo người dùng tham gia phòng livestream")]
        RecvRoomNotify,

        [Display(Name = "Thông báo ai đã theo dõi người dùng")]
        RecvFollowerNotify,

        [Display(Name = "Thông báo người dùng đã bị chặn")]
        RecvUserBannedNotify,

        [Display(Name = "Thông báo người dùng đã bị chặn chat trong phòng livestream")]
        RecvUserBannedLivestreamNotify,


        /* Livestream Events */
        [Display(Name = "Thông báo livestream thay đổi")]
        RecvStreamChangeNotify,

        [Display(Name = "Thông báo tín hiệu bắt đầu hoặc kết thúc livestream")]
        RecvStreamNotify,

        [Display(Name = "Thông báo số lượng người xem livestream thay đổi")]
        RecvCurrentViewerNotify,

        [Display(Name = "Thông báo tin nhắn đến phòng livestream")]
        RecvChatMessage,

        [Display(Name = "Thông báo tặng quà đến phòng livestream")]
        RecvGiftMessage,

        [Display(Name = "Thông báo tin nhắn đến phòng livestream fake")]
        RecvFakeChatMessage,

        [Display(Name = "Thông báo tặng quà đến phòng livestream fake")]
        RecvFakeGiftMessage,

        [Display(Name = "Thông báo livestream đã bị ngừng do có nội dung nhạy cảm")]
        RecvLivestreamBannedNotify,


        /* Exception Events */
        [Display(Name = "Thông báo xảy ra ngoại lệ")]
        RecvExceptionNotify
    }
}
