using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AfterTheSilence.Xamarin
{
    public partial class MainPage : ContentPage
    {
        private readonly List<MessageBlock> messageBlocks = new List<MessageBlock>();
        private readonly IFordePathService _fordePathService;

        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>()
        {
            new Message(MessageType.Owner, "Овнер", "owner", DateTime.Now),
            new Message(MessageType.Bot, "Bot", "bot", DateTime.Now)
        };


        [Reactive]
        public string Message { get; set; }

        #region SendMessageCommand

        public ReactiveCommand<Unit, Unit> SendMessageCommand =>
            ReactiveCommand.Create(OnSendMessageExecute);

        private void OnSendMessageExecute()
        {
            if (string.IsNullOrWhiteSpace(Message))
                return;

            var message = Message;
            Message = string.Empty;
            var newMessage = new Message( MessageType.Owner, message, "owner", DateTime.UtcNow);
            Messages.Add(newMessage);
        }

        #endregion


        public MainPage(IFordePathService folderPath)
        {
            _fordePathService = folderPath;

            InitializeComponent();

            InitializeScript();

            BindingContext = this;
        }

        private async void InitializeScript()
        {
            while (await Permissions.CheckStatusAsync<Permissions.StorageRead>() == PermissionStatus.Denied)
            {
                await Permissions.RequestAsync<Permissions.StorageRead>();
                await Task.Delay(100);
            }
            while (await Permissions.CheckStatusAsync<Permissions.StorageWrite>() == PermissionStatus.Denied)
            {
                await Permissions.RequestAsync<Permissions.StorageWrite>();
                await Task.Delay(100);
            }

            string[] lines = ReadAllLines();
            MessageBlock currentMessageBlock = new MessageBlock();
            bool isFirst = true, isAnother = false;
            MessageTag lastTag = MessageTag.Empty;

            foreach (var line in lines)
            {
                if (line.Length > 0 && line[0] is '_')
                {
                    if (!isFirst)
                    {
                        messageBlocks.Add(currentMessageBlock);
                        currentMessageBlock = new MessageBlock();
                    }
                    else
                    {
                        isFirst = false;
                    }
                    continue;
                }

                string tag = new string(line.Take(2).ToArray());
                string text = string.Empty;

                isAnother = true;

                switch (tag)
                {
                    case "m>":
                        text = line.Substring(2, line.Length - 2);
                        lastTag = MessageTag.Message;
                        break;
                    case "r>":
                        text = line.Substring(2, line.Length - 2);
                        lastTag = MessageTag.Response;
                        break;
                    default:
                        isAnother = false;
                        break;
                }

                if (isAnother && lastTag == MessageTag.Message)
                {
                    currentMessageBlock.Messages.Add(text);
                    continue;
                }

                if (!isAnother && lastTag == MessageTag.Message)
                {
                    int lastIndex = currentMessageBlock.Messages.Count - 1;
                    string lastText = currentMessageBlock.Messages[lastIndex];
                    currentMessageBlock.Messages[lastIndex] = lastText + Environment.NewLine + line;
                    continue;
                }

                if (isAnother && lastTag == MessageTag.Response)
                {
                    currentMessageBlock.Responses.Add(text);
                    continue;
                }

                if (!isAnother && lastTag == MessageTag.Response)
                {
                    int lastIndex = currentMessageBlock.Responses.Count - 1;
                    string lastText = currentMessageBlock.Responses[lastIndex];
                    currentMessageBlock.Responses[lastIndex] = lastText + Environment.NewLine + line;
                    continue;
                }
            }
        }

        private string[] ReadAllLines()
        {
            var backingFile = Path.Combine(_fordePathService.GetMainFordePath(), "script.txt");

            if (backingFile == null || !File.Exists(backingFile))
                File.Create(backingFile).Close();

            return File.ReadAllLines(backingFile);
        }

        /*
            m>Чего мне нужно больше?
            r>Золота
            r>Построить зиккурат
            ___________
            m>Привет!
            m>ку
            r>Извини, я занят, скребу крышку гроба.
            Отпишу позже.
            ___________
         */
    }
}
