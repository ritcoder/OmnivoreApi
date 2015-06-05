using System;

namespace OmnivoreApi.Exceptions
{
    public class OmnivoreApiException : Exception
    {
        public OmnivoreApiException(OmnivoreApiError error)
        {
            details = error;
        }

        public OmnivoreApiError details { get; }
    }
}