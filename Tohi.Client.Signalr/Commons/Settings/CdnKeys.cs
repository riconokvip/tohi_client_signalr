namespace Tohi.Client.Signalr.Commons.Settings
{
    public class CdnKeys
    {
        public static CdnConfig CdnLiveConfig { get; set; }
        public const int LANG_DEFAULT = 1066;
        public const string CDN_DOMAIN = "https://tohitv.sgp1.cdn.digitaloceanspaces.com/";
        public const int DefaultCryptCost = 10;
        public static DateTime DATETIME_UTC_NOW => DateTime.UtcNow;
    }

    public class CdnConfig
    {
        public const string ConfigName = "CdnLive";
        public string Domain { get; set; }
        public string PlayDomain { get; set; }
        public string AuthKey { get; set; }
        public string ProfileDefault { get; set; }
        public int ProfileDefaultId { get; set; }
        public List<object> ProfileOutputDefault { get; set; }
        public string CallbackEventHandleUrl { get; set; }
    }
}
