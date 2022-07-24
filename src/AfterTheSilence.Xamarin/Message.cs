using System;

namespace AfterTheSilence.Xamarin
{
    public enum MessageType
    {
        Bot,
        Owner
    }

    public class Message
    {
        public MessageType MessageType { get; }

        public string Text { get; }

        public string UserName { get; }

        public DateTime? DateTime { get; }

        public Message(MessageType messageType, string text, string userName, DateTime? dateTime)
        {
            MessageType = messageType;
            Text = text ?? string.Empty;
            UserName = userName ?? string.Empty;
            DateTime = dateTime ?? new DateTime();
        }
    }
}
