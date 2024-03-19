namespace Tohi.Client.Signalr.Commons.Cachings
{
    public static class UserKeys
    {
        /// <summary>
        /// Khóa lưu trữ thông tin cá nhân người dùng
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        public static string Information(string userId) => $"{userId}_information";

        /// <summary>
        /// Khóa lưu trữ số lượng người theo dõi của người dùng
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        public static string Follower(string userId) => $"{userId}_follower";

        /// <summary>
        /// Khóa lưu trữ số lần tham gia phòng livestream của người dùng
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        public static string CountJoinGroup(string userId) => $"{userId}_join";

        /// <summary>
        /// Khóa lưu trữ đường dẫn livestream của người dùng
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        public static string HlsSrc(string userId) => $"{userId}_hlsSrc";
    }
}
