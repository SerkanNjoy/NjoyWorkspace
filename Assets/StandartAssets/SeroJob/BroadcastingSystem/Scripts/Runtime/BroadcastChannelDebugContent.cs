using System.Collections.Generic;

namespace SeroJob.BroadcastingSystem
{
    [System.Serializable]
    public class BroadcastChannelDebugContent
    {
        public List<BroadcastChannelListener> Content;

        public BroadcastChannelDebugContent()
        {
            Content = new List<BroadcastChannelListener>();
        }

        public void Add(BroadcastChannelListener listenerContent)
        {
            Content.Add(listenerContent);
        }

        public void Remove(object target, string method)
        {
            foreach(var content in Content)
            {
                if(content.MethodTarget == target && content.MethodName == method)
                {
                    Content.Remove(content);
                    return;
                }
            }
        }

        public void Reset()
        {
            Content = new List<BroadcastChannelListener>();
        }
    }
}