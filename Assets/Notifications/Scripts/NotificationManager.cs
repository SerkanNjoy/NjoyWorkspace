using System;
using UnityEngine;

namespace NjoyKidz.Notifications
{
    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager Instance { get; private set; }

        [SerializeField] private string notificationChannelName;

        private BaseNotificationController notificationController;
        private NotificationStack _notificationStack;

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _notificationStack = new NotificationStack();
            Instance = this;
        }

        private void Start()
        {
            InitializeNotificationCenter();
        }

        private void InitializeNotificationCenter()
        {
#if UNITY_ANDROID
            notificationController = new NotificationControllerAndroid(notificationChannelName);
#endif

#if UNITY_IOS
            notificationController = new NotificationControllerIOS();
#endif
        }

        private void CreateStackedNotifications()
        {
            while (_notificationStack.StackSize > 0)
            {
                var item = _notificationStack.GetStackedMessage();
                notificationController.CreateNotification(item.message, item.delay);
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                CreateStackedNotifications();
            }
            else
            {
                ResetNotifications();
            }
        }

        private void ResetNotifications()
        {
            if(notificationController != null) notificationController.ResetNotifications();
        }

        public void CreateNotification(NotificationMessage message, int delayTime)
        {
            _notificationStack.StackMessage(message, delayTime);
        }

        public void CreateNotificationAtTime(NotificationMessage message, DateTime dateTime)
        {
            var now = DateTime.Now;
            int delay = (dateTime - now).Seconds;

            CreateNotification(message, delay);
        }
    }
}