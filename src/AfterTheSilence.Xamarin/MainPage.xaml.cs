using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Xamarin.Forms;

namespace AfterTheSilence.Xamarin
{
    public class ChatViewModel : ReactiveObject
    {
        private string _message;
        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>()
        {
            new Message(MessageType.Owner, "Овнер", "owner", DateTime.Now),
            new Message(MessageType.Bot, "Bot", "bot", DateTime.Now)
        };

        public ReactiveCommand<Unit, Unit> SendMessageCommand =>
            ReactiveCommand.Create(OnSendMessageExecute);

        private void OnSendMessageExecute()
        {
            if (string.IsNullOrWhiteSpace(Message))
                return;

            string message = Message;
            Message = String.Empty;
            Messages.Add(new Message(MessageType.Owner, message, "owner", DateTime.UtcNow));

            string response = BotService.GetResponse(message);
            Messages.Add(new Message(MessageType.Bot, response, "bot", DateTime.UtcNow));
        }
    }

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ChatViewModel();
        }
    }
}
