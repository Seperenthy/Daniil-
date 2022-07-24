using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AfterTheSilence.Xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            IFordePathService test = DependencyService.Get<IFordePathService>();
            MainPage = new MainPage(test);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
