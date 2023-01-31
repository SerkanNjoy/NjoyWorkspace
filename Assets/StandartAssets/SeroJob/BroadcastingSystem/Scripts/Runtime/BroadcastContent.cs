using UnityEngine;
using UnityEngine.Events;

namespace SeroJob.BroadcastingSystem
{
    public class BroadcastContent
    {
        private object _content;
        private UnityEvent _defaultContent;

        private BroadcastContentType _contentType;

        private IDebugableBroadcastChannel _channelDebugInfo;

#if UNITY_EDITOR
        private BroadcastChannelDebugContent _channelDebugContent = new BroadcastChannelDebugContent();
#endif
        #region Constraction
        public void Construct(IDebugableBroadcastChannel channelInfo)
        {
            _content = new UnityEvent();
            _defaultContent = new UnityEvent();
            _contentType = BroadcastContentType.ZeroParameter;

            _channelDebugInfo = channelInfo;
        }

        public void Construct<T1>(IDebugableBroadcastChannel channelInfo)
        {
            _content = new UnityEvent<T1>();
            _defaultContent = new UnityEvent();
            _contentType = BroadcastContentType.OneParameter;

            _channelDebugInfo = channelInfo;
        }

        public void Construct<T1, T2>(IDebugableBroadcastChannel channelInfo)
        {
            _content = new UnityEvent<T1, T2>();
            _defaultContent = new UnityEvent();
            _contentType = BroadcastContentType.TwoParameter;

            _channelDebugInfo = channelInfo;
        }

        public void Construct<T1, T2, T3>(IDebugableBroadcastChannel channelInfo)
        {
            _content = new UnityEvent<T1, T2, T3>();
            _defaultContent = new UnityEvent();
            _contentType = BroadcastContentType.ThreeParameter;

            _channelDebugInfo = channelInfo;
        }
        #endregion

        #region ListenerRegistration
        public void RegisterListener(UnityAction listener)
        {
            _defaultContent.AddListener(listener);

#if UNITY_EDITOR
            _channelDebugContent.Add(new BroadcastChannelListener(listener.Target, listener.Method.Name));
#endif
        }

        public void RegisterListener<T1>(UnityAction<T1> listener)
        {
            if (_contentType != BroadcastContentType.OneParameter)
            {
                Debug.LogError("<color=red> BroadcastSystem: </color> Registering listener with different type is not supported | ChannelName => " + _channelDebugInfo.ChannelDebugName);
                return;
            }

            UnityEvent<T1> e = (UnityEvent<T1>)_content;

            e.AddListener(listener);

#if UNITY_EDITOR
            _channelDebugContent.Add(new BroadcastChannelListener(listener.Target, listener.Method.Name));
#endif
        }

        public void RegisterListener<T1, T2>(UnityAction<T1, T2> listener)
        {
            if (_contentType != BroadcastContentType.TwoParameter)
            {
                Debug.LogError("<color=red> BroadcastSystem: </color> Registering listener with different type is not supported | ChannelName => " + _channelDebugInfo.ChannelDebugName);
                return;
            }

            UnityEvent<T1, T2> e = (UnityEvent<T1, T2>)_content;

            e.AddListener(listener);

#if UNITY_EDITOR
            _channelDebugContent.Add(new BroadcastChannelListener(listener.Target, listener.Method.Name));
#endif
        }

        public void RegisterListener<T1, T2, T3>(UnityAction<T1, T2, T3> listener)
        {
            if (_contentType != BroadcastContentType.ThreeParameter)
            {
                Debug.LogError("<color=red> BroadcastSystem: </color> Registering listener with different type is not supported | ChannelName => " + _channelDebugInfo.ChannelDebugName);
                return;
            }

            UnityEvent<T1, T2, T3> e = (UnityEvent<T1, T2, T3>)_content;

            e.AddListener(listener);

#if UNITY_EDITOR
            _channelDebugContent.Add(new BroadcastChannelListener(listener.Target, listener.Method.Name));
#endif
        }
        #endregion

        #region ListenerRemoving
        public void RemoveListener(UnityAction listener)
        {
            _defaultContent.RemoveListener(listener);
#if UNITY_EDITOR
            _channelDebugContent.Remove(listener.Target, listener.Method.Name);
#endif
        }

        public void RemoveListener<T1>(UnityAction<T1> listener)
        {
            if (_contentType != BroadcastContentType.OneParameter)
            {
                Debug.LogError("<color=red> BroadcastSystem: </color> Removing listener with different type is not supported | ChannelName => " + _channelDebugInfo.ChannelDebugName);
                return;
            }

            UnityEvent<T1> e = (UnityEvent<T1>)_content;

            e.RemoveListener(listener);
#if UNITY_EDITOR
            _channelDebugContent.Remove(listener.Target, listener.Method.Name);
#endif
        }

        public void RemoveListener<T1, T2>(UnityAction<T1, T2> listener)
        {
            if (_contentType != BroadcastContentType.OneParameter)
            {
                Debug.LogError("<color=red> BroadcastSystem: </color> Removing listener with different type is not supported | ChannelName => " + _channelDebugInfo.ChannelDebugName);
                return;
            }

            UnityEvent<T1, T2> e = (UnityEvent<T1, T2>)_content;

            e.RemoveListener(listener);
#if UNITY_EDITOR
            _channelDebugContent.Remove(listener.Target, listener.Method.Name);
#endif
        }

        public void RemoveListener<T1, T2, T3>(UnityAction<T1, T2, T3> listener)
        {
            if (_contentType != BroadcastContentType.OneParameter)
            {
                Debug.LogError("<color=red> BroadcastSystem: </color> Removing listener with different type is not supported | ChannelName => " + _channelDebugInfo.ChannelDebugName);
                return;
            }

            UnityEvent<T1, T2, T3> e = (UnityEvent<T1, T2, T3>)_content;

            e.RemoveListener(listener);
#if UNITY_EDITOR
            _channelDebugContent.Remove(listener.Target, listener.Method.Name);
#endif
        }
        #endregion

        #region Broadcasting
        public void Broadcast()
        {
            if (_contentType != BroadcastContentType.ZeroParameter)
            {
                Debug.LogError("<color=red> BroadcastSystem: </color> Broadcasting content with different type is not supported | ChannelName => " + _channelDebugInfo.ChannelDebugName);
                return;
            }

            _defaultContent?.Invoke();
        }

        public void Broadcast<T1>(T1 value)
        {
            if (_contentType != BroadcastContentType.OneParameter)
            {
                Debug.LogError("<color=red> BroadcastSystem: </color> Broadcasting content with different type is not supported | ChannelName => " + _channelDebugInfo.ChannelDebugName);
                return;
            }

            UnityEvent<T1> e = (UnityEvent<T1>)_content;

            e?.Invoke(value);
            _defaultContent?.Invoke();
        }

        public void Broadcast<T1, T2>(T1 value1, T2 value2)
        {
            if (_contentType != BroadcastContentType.TwoParameter)
            {
                Debug.LogError("<color=red> BroadcastSystem: </color> Broadcasting content with different type is not supported | ChannelName => " + _channelDebugInfo.ChannelDebugName);
                return;
            }

            UnityEvent<T1, T2> e = (UnityEvent<T1, T2>)_content;

            e?.Invoke(value1, value2);
            _defaultContent?.Invoke();
        }

        public void Broadcast<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            if (_contentType != BroadcastContentType.ThreeParameter)
            {
                Debug.LogError("<color=red> BroadcastSystem: </color> Broadcasting content with different type is not supported | ChannelName => " + _channelDebugInfo.ChannelDebugName);
                return;
            }

            UnityEvent<T1, T2, T3> e = (UnityEvent<T1, T2, T3>)_content;

            e?.Invoke(value1, value2, value3);
            _defaultContent?.Invoke();
        }
        #endregion

        #region ContentResetting
        public void ResetContent()
        {
            if (_content == null) return;

            UnityEventBase e = (UnityEventBase)_content;

            e.RemoveAllListeners();
            _defaultContent.RemoveAllListeners();
#if UNITY_EDITOR
            _channelDebugContent.Reset();
#endif
        }
        #endregion

#if UNITY_EDITOR
        #region Debugging
        public BroadcastChannelDebugContent GetDebugContent()
        {
            return _channelDebugContent;
        }
        #endregion
#endif
    }

    public enum BroadcastContentType
    {
        ZeroParameter,
        OneParameter,
        TwoParameter,
        ThreeParameter
    }
}