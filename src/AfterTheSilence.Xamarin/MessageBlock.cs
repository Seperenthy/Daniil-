using System.Collections.Generic;

namespace AfterTheSilence.Xamarin
{
    public enum MessageTag
    {
        Empty,
        Message,
        Response
    }

    public class MessageBlock
    {
        public List<string> Messages { get; } = new List<string>();
        public List<string> Responses { get; } = new List<string>();
    }
}
