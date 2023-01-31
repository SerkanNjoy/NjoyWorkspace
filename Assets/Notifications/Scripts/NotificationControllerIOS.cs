using System;
using UnityEngine;

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

namespace NjoyKidz.Notifications
{
    public class NotificationControllerIOS : BaseNotificationController
    {
        public NotificationControllerIOS()
        {
            GetLastRespondedNotifications();
            ResetNotifications();
        }

        public override void ResetNotifications()
        {
#if UNITY_IOS
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();
#endif
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

        private void GetLastRespondedNotifications()
        {
            /*var n = iOSNotificationCenter.GetLastRespondedNotification();
            if (n == null) return;

            var msg = "Last Received Notification : " + n.Identifier + "\n";
            msg += "\n - Notification received: ";
            msg += "\n - .Title: " + n.Title;
            msg += "\n - .Badge: " + n.Badge;
            msg += "\n - .Body: " + n.Body;
            msg += "\n - .CategoryIdentifier: " + n.CategoryIdentifier;
            msg += "\n - .Subtitle: " + n.Subtitle;
            msg += "\n - .Data: " + n.Data;*/
            //EventController.Instance.SendNotificationClickEvent(int.Parse(n.Identifier));
        }

        public override void CreateNotification(NotificationMessage message, int seconds)
        {
#if UNITY_IOS
            iOSNotificationTrigger timeTrigger;

            timeTrigger = new iOSNotificationTimeIntervalTrigger
            {
                TimeInterval = TimeSpan.FromSeconds(CheckAndChangeNotificationTime(seconds)),
                Repeats = false
            };

            var notification = new iOSNotification
            {
                Identifier = message.Id.ToString(),
                Title = message.Title,
                Body = message.Text,
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger
            };

            notification.ShowInForeground = true;
            notification.ForegroundPresentationOption = PresentationOption.Sound | PresentationOption.Alert;

            iOSNotificationCenter.ScheduleNotification(notification);
            Debug.Log("<color=cyan>Notifications: </color>Ios Notification Created");
#endif
        }
    }
}