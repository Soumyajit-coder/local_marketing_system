using localMarketingSystem.Enum;

namespace localMarketingSystem.Helpers
{
    public class APIResponseHelper<T>
    {
        public T? result { get; set; }
        public APIResponseStatus apiResponseStatus { get; set; }
        public string Message { get; set; }
    }
}
