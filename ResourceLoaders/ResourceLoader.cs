namespace OmnivoreApi.ResourceLoaders
{
    public class ResourceLoader
    {
        public ResourceLoader(Omnivore io)
        {
            this.io = io;
        }

        protected Omnivore io { get; }
    }
}