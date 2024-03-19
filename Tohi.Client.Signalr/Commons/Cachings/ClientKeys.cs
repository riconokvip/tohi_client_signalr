namespace Tohi.Client.Signalr.Commons.Cachings
{
    public static class ClientKeys
    {
        /// <summary>
        /// Khóa lưu trữ phòng livestream hiện tại mà client đang tham gia
        /// </summary>
        /// <param name="connectionId">Id của client</param>
        /// <returns></returns>
        public static string Livestream(string connectionId) => $"{connectionId}_livestream";
    }
}
