namespace OmnivoreApi.Utils
{
    public class SelfReference<T> : Reference<T>
    {
        public string etag { get; set; }
    }
}