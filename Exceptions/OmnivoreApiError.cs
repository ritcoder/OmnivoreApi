using System;

namespace OmnivoreApi.Exceptions
{
    public class OmnivoreApiError
    {
        public int code { get; set; }
        public string error { get; set; }
        public DateTime date { get; set; }
        public string raw { get; set; }

        public OmnivoreApiException ToException()
        {
            return new OmnivoreApiException(this);
        }

    }
}