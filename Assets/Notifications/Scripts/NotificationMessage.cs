namespace NjoyKidz.Notifications
{
    public struct NotificationMessage
    {
        public readonly int Id;
        public readonly string Title;
        public readonly string Text;

        public NotificationMessage(int id, string title, string text)
        {
            Id = id;
            Title = title;
            Text = text;
        }
    }
}