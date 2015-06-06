using System;

namespace OmnivoreApi.Exceptions
{
    public class OmnivoreApiResult<TSuccess, TError>
    {
        public TSuccess success { get; set; }
        public TError error { get; set; }
        public bool ok => error == null;
        public TimeSpan? duration { get; set; }
    }
}
