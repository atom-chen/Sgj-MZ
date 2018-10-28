

namespace App.Util
{
    public class Global
    {
        public static AppManager AppManager { get; private set; }
        public static void Initialize()
        {
            AppManager = new AppManager();
        }
    }
}
