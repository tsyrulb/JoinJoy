namespace JoinJoy.Core.Models
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }  // Add Token property here
        public object? Data { get; set; } // Generic data property for any type of response


    }
    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; set; }
    }
}
