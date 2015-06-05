using System;

namespace OmnivoreApi
{
    namespace Utils
    {
    }

    namespace Exceptions
    {
    }

    public class OmnivoreApiResult<TSuccess, TError>
    {
        public TSuccess success { get; set; }
        public TError error { get; set; }
        public bool ok => error == null;
        public TimeSpan? duration { get; set; }
    }


    namespace Resources
    {
    }

}
