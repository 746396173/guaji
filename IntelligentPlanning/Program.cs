namespace IntelligentPlanning
{
    using System;
    using System.Windows.Forms;

    internal static class Program
    {
        public static Form MainApp;

        [STAThread]
        private static void Main(string[] pArgs)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainApp = new AutoBetsWindow(pArgs);
            if (CommFunc.CheckAppRunInSamePath() && (AppInfo.App != ConfigurationStatus.AppType.OpenData))
            {
                CommFunc.PublicMessageAll("同目录下程序禁止多开，想多开请复制目录！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                Application.Run(MainApp);
            }
        }
    }
}

