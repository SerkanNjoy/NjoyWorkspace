using System;
using UnityEngine;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

namespace NjoyKidz.Notifications
{
    public class NotificationControllerAndroid : BaseNotificationController
    {
#if UNITY_ANDROID
        private AndroidNotificationChannel _channel;
#endif

        public NotificationControllerAndroid(string channelName)
        {
            CreateChannel(channelName);

            ResetNotifications();
        }

        public override void ResetNotifications()
        {
#if UNITY_ANDROID
            GetLasNotificationData();
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
            AndroidNotificationCenter.CancelAllScheduledNotifications();
#endif
        }

        private void CreateChannel(string channelName)
        {
#if UNITY_ANDROID
            _channel = new AndroidNotificationChannel
            {
                Id = "id_notif",
                Name = channelName,
                Importance = Importance.High,
                Description = "Generic notifications",
                EnableVibration = true,
                EnableLights = true,
                LockScreenVisibility = LockScreenVisibility.Public,
                CanShowBadge = true
            };

            AndroidNotificationCenter.RegisterNotificationChannel(_channel);
            Debug.Log("<color=cyan>Notifications: </color>Android Notification Channel Created");
#endif
        }

        private void GetLasNotificationData()
        {
            /*var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

            if (notificationIntentData == null) return;

            int id = notificationIntentData.Id;
            string channel = notificationIntentData.Channel;
            var notification = notificationIntentData.Notification;*/

            //EventController.Instance.SendNotificationClickEvent(id);
        }

        private int CheckAndChangeNotificationTime(int delayTime)
        {
            int newDelayTime = delayTime;
            DateTime fireTime = DateTime.Now.AddSeconds(delayTime);
            if (fireTime.Hour >= 0 && fireTime.Hour < 8)
            {
                newDelayTime += 28800 - 3600 * fireTime.Hour;
            }

            return newDelayTime;
        }

        public override void CreateNotification(NotificationMessage message, int delayTime)
        {
#if UNITY_ANDROID
            var notification = new AndroidNotification
            {
                Title = message.Title,
                Text = message.Text,
                SmallIcon = "icon_0",
                FireTime = DateTime.Now.AddSeconds(CheckAndChangeNotificationTime(delayTime)),
                Style = NotificationStyle.BigTextStyle,
                GroupAlertBehaviour = GroupAlertBehaviours.GroupAlertAll
            };

            Debug.Log("<color=cyan>Notifications: </color>Android Notification Created");
            AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "id_notif", message.Id);
#endif
        }
    }
}