namespace SeroJob.BroadcastingSystem
{
    public struct BroadcastChannelListener
    {
        public object MethodTarget { get; private set; }
        public string MethodName { get; private set; }

        public BroadcastChannelListener(object target, string method)
        {
            MethodTarget = target;
            MethodName = method;
        }
    }
}