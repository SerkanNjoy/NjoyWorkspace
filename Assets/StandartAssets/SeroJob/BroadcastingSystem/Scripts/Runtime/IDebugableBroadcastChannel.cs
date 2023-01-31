namespace SeroJob.BroadcastingSystem
{
    public interface IDebugableBroadcastChannel
    {
        public string ChannelDebugName { get; set; }

        public BroadcastChannelDebugContent GetBroadcastContent();
    }
}