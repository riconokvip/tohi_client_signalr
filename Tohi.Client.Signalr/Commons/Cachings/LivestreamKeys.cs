namespace Tohi.Client.Signalr.Commons.Cachings
{
    public static class LivestreamKeys
    {
        /// <summary>
        /// Khóa lưu trữ thông tin livestream
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string Information(string group) => $"{group}_information";

        /// <summary>
        /// Khóa lưu trữ số lượt xem livestream
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string Viewer(string group) => $"{group}_viewer";
    }
}
