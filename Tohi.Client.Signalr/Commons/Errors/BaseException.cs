namespace Tohi.Client.Signalr.Commons.Errors
{
    public class BaseException : Exception
    {
        public int error { get; set; }
        public string message { get; set; }

        public BaseException(ErrorEnums type, string msg = null)
        {
            error = (int)type;
            message = string.IsNullOrEmpty(msg) ? EnumHelpers<ErrorEnums>.GetDisplayValue(type) : msg;
        }
    }
}
