using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AfterTheSilence.Xamarin
{
    public class MessageItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OwnerMessage { get; set; }
        public DataTemplate BotMessage { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is null)
                throw new System.NullReferenceException("Element is null. [SelectTemplate]");

            return SelectTemplateCore(item);
        }

        private DataTemplate SelectTemplateCore(object item)
        {
            if (item is Message)
                return ((Message)item).MessageType == MessageType.Owner ? OwnerMessage : BotMessage;

            throw new System.NotImplementedException($"Uknown DataTemplate parameter. \nType: {nameof(MessageItemDataTemplateSelector)}\nParametr: {item}");
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatView : Grid
    {
        public ChatView()
        {
            InitializeComponent();
        }
    }
}