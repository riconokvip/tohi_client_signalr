using System.ComponentModel.DataAnnotations;

namespace Tohi.Client.Signalr.Commons.Enums
{
    public enum LivestreamEnums
    {
        [Display(Name = "Đang phát livestream")]
        Online = 0,

        [Display(Name = "Dừng phát livestream")]
        Offline = -1,
    }
}
