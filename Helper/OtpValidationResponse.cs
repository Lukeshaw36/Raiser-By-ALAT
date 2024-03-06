namespace GROUP2.Helper
{
    public class OTPResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }


    public class Response<T>
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
