using UnityEngine;
using UnityEngine.Events;

namespace SeroJob.BroadcastingSystem
{
    public abstract class BroadcastChannel : ScriptableObject, IDebugableBroadcastChannel
    {
        public BroadcastContent content;

        public virtual string ChannelDebugName { get; set; }

        protected virtual void OnEnable()
        {
            content = new BroadcastContent();
        }

        #region ListenerRegistration
        public void RegisterListener(UnityAction listener)
        {
            content.RegisterListener(listener);
        }

        public void RegisterListener<T1>(UnityAction<T1> listener)
        {
            content.RegisterListener(listener);
        }

        public void RegisterListener<T1, T2>(UnityAction<T1, T2> listener)
        {
            content.RegisterListener(listener);
        }

        public void RegisterListener<T1, T2, T3>(UnityAction<T1, T2, T3> listener)
        {
            content.RegisterListener(listener);
        }
        #endregion

        #region ListenerRemoving
        public void RemoveListener(UnityAction listener)
        {
            content.RemoveListener(listener);
        }

        public void RemoveListener<T1>(UnityAction<T1> listener)
        {
            content.RemoveListener(listener);
        }

        public void RemoveListener<T1, T2>(UnityAction<T1, T2> listener)
        {
            content.RemoveListener(listener);
        }

        public void RemoveListener<T1, T2, T3>(UnityAction<T1, T2, T3> listener)
        {
            content.RemoveListener(listener);
        }
        #endregion

        #region Broadcasting
        public void Broadcast()
        {
            content.Broadcast();
        }

        public void Broadcast<T1>(T1 value)
        {
            content.Broadcast(value);
        }

        public void Broadcast<T1, T2>(T1 value1, T2 value2)
        {
            content.Broadcast(value1, value2);
        }

        public void Broadcast<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            content.Broadcast(value1, value2, value3);
        }
        #endregion

        public virtual BroadcastChannelDebugContent GetBroadcastContent()
        {
#if UNITY_EDITOR
            return content.GetDebugContent();
#else

            return null;
#endif
        }
    }
}