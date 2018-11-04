

namespace App.Util
{
    public class Global
    {
        public static string ssid;
        public static AppManager AppManager { get; private set; }
        public static void Initialize()
        {
            AppManager = new AppManager();
        }
    }
}
