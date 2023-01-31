namespace NjoyKidz.Notifications
{
    public abstract class BaseNotificationController
    {
        public abstract void ResetNotifications();

        public abstract void CreateNotification(NotificationMessage message, int seconds);
    }
}