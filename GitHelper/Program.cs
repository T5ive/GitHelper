namespace GitHelper;

internal static class Program
{
    internal static readonly PathSetting PathSetting = PathSetting.Load();

    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FrmMain());
    }
}