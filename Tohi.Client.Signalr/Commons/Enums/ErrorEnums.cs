using System.ComponentModel.DataAnnotations;

namespace Tohi.Client.Signalr.Commons.Enums
{
    public enum ErrorEnums
    {
        /* User Errors */
        [Display(Name = "Người dùng không tồn tại")]
        UserNotFound = -100,
        [Display(Name = "Người dùng đã bị khóa")]
        UserIsBanned = -101,

        [Display(Name = "Không thể lấy dữ liệu người dùng khi có sự thay đổi")]
        UserNotFoundWhileChange = -102,
        [Display(Name = "Không thể lấy dữ liệu số lần tham gia phòng livestream của người dùng")]
        UserNotFoundCountJoin = -103,
        [Display(Name = "Số lần tham gia phòng livestream của người dùng nhỏ hơn 1")]
        UserFailCountJoin = -104,
        [Display(Name = "Không thể trò chuyện hoặc tặng quà do bạn có hành vi tiêu cực")]
        UserIsBannedInLivestream = -105,


        /* Client Errors */
        [Display(Name = "Client đã tham gia phòng livestream")]
        ClientInLivestream = -200,
        [Display(Name = "Client chưa tham gia phòng livestream")]
        ClientNotInLivestream = -201,


        /* Livestream Errors */
        [Display(Name = "Livestream không tồn tại")]
        LivestreamNotFound = -300,
        [Display(Name = "Livestream đã bị chặn")]
        LivestreamIsBanned = -301,

        [Display(Name = "Không thể lấy dữ liệu livestream khi có sự thay đổi")]
        LivestreamNotFoundWhileChange = -302,
        [Display(Name = "Không thể lấy dữ liệu số lượt xem livestream hiện tại")]
        LivestreamNotFoundCountViewer = -303,
        [Display(Name = "Số lượt xem livestream không thể nhỏ hơn 0")]
        LivestreamFailCountViewer = -304,


        /* Cdnlive Errors */
        [Display(Name = "Cdnlive không tồn tại")]
        CdnliveNotFound = -400,
        [Display(Name = "Không thể lấy dữ liệu cdnlive khi có sự thay đổi")]
        CdnLiveNotFoundWhileChange = -401
    }
}
