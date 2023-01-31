using System.Collections.Generic;

namespace NjoyKidz.Notifications
{
    public class NotificationStack
    {
        private List<NotificationStackContent> _content;

        public int StackSize { get; private set; }

        public NotificationStack()
        {
            StackSize = 0;
            _content = new List<NotificationStackContent>();
        }

        public void StackMessage(NotificationMessage message, int delay)
        {
            StackSize++;
            _content.Add(new NotificationStackContent(message, delay));
        }

        public (NotificationMessage message, int delay) GetStackedMessage()
        {
            if (_content.Count <= 0) return (default, -1);

            StackSize--;
            var item = _content[0];
            _content.RemoveAt(0);

            return (item.Message, item.Delay);
        }
    }

    public struct NotificationStackContent
    {
        public readonly NotificationMessage Message;
        public readonly int Delay;

        public NotificationStackContent(NotificationMessage message, int delay)
        {
            Message = message;
            Delay = delay;
        }
    }
}