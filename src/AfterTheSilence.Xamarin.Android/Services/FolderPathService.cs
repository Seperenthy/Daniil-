using AfterTheSilence.Xamarin.Droid.Services;
using Xamarin.Forms;

[assembly:Dependency(typeof(FolderPathService))]
namespace AfterTheSilence.Xamarin.Droid.Services
{
    public class FolderPathService : IFordePathService
    {
        [System.Obsolete]
        public string GetMainFordePath()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;
        }
    }
}