namespace GitHelper
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        ///
        internal static PathSetting PathSetting = new();

        [STAThread]
        private static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            PathSetting = PathSetting.Load();
            Application.Run(new FrmMain());
        }
    }
}