using UnityEngine;
using UnityEngine.UI;

namespace SeroJob.BroadcastingSystem
{
    [RequireComponent(typeof(BroadcastingButtonAnimator))]
    public class BroadcastingButton : MonoBehaviour
    {
        [Header("Actual Button To Display")]
        [SerializeField] private GameObject _buttonObject;

        [Header("Broadcast Channel To Invoke On Click")]
        [SerializeField] private BroadcastChannel channelToBroadcast;

        [Tooltip("Channels Being Listened To Activate The Button")]
        [SerializeField] private BroadcastChannel[] activateChannels;

        [Tooltip("Channels Being Listened To Deactivate The Button")]
        [SerializeField] private BroadcastChannel[] deactivateChannels;

        [Header("BUTTON SETTINGS")]
        [SerializeField] private bool deactivateButtonOnAwake = true;
        [SerializeField] private BroadcastingButtonAnimator.ButtonAnimType animType;
        [SerializeField] private float animSpeed = 1f;

        private BroadcastingButtonAnimator _buttonAnimator;
        private Button _button;

        private bool _isActive;

        private void Awake()
        {
            _buttonObject.SetActive(!deactivateButtonOnAwake);
            _isActive = !deactivateButtonOnAwake;
            _buttonAnimator = GetComponent<BroadcastingButtonAnimator>();
            _button = _buttonObject.GetComponent<Button>();
            _button.enabled = _isActive;
        }

        private void OnEnable()
        {
            foreach (var channel in deactivateChannels)
            {
                channel.RegisterListener(TryDeactivateButton);
            }

            foreach (var channel in activateChannels)
            {
                channel.RegisterListener(TryActivateButton);
            }
        }

        private void OnDisable()
        {
            foreach (var channel in deactivateChannels)
            {
                channel.RemoveListener(TryDeactivateButton);
            }

            foreach (var channel in activateChannels)
            {
                channel.RemoveListener(TryActivateButton);
            }
        }

        public void OnClick()
        {
            if (!_isActive) return;

            channelToBroadcast.Broadcast();
        }

        private void TryDeactivateButton()
        {
            if (!_isActive) return;

            _button.enabled = false;

            switch (animType)
            {
                case BroadcastingButtonAnimator.ButtonAnimType.None:

                    _buttonObject.SetActive(false);
                    _isActive = false;
                    break;

                case BroadcastingButtonAnimator.ButtonAnimType.Scale:

                    _buttonAnimator.StartAnimatingButtonScale(animSpeed, 1.0f, 0.0f, () =>
                    {
                        _buttonObject.SetActive(false);
                        _isActive = false;
                    });
                    break;
            }
        }

        private void TryActivateButton()
        {
            if (_isActive) return;

            switch (animType)
            {
                case BroadcastingButtonAnimator.ButtonAnimType.None:

                    _buttonObject.SetActive(true);
                    _isActive = true;
                    _button.enabled = true;
                    break;

                case BroadcastingButtonAnimator.ButtonAnimType.Scale:

                    _buttonObject.SetActive(true);
                    _buttonAnimator.StartAnimatingButtonScale(animSpeed, 0.0f, 1.0f, () =>
                    {
                        _isActive = true;
                        _button.enabled = true;
                    });
                    break;
            }
        }
    }
}