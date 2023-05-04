using System.Drawing;

namespace GitHelper;

public partial class FrmMain : Form
{
    private static bool _up2date;
    public FrmMain()
    {
        InitializeComponent();
    }

    private void btnRun_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show("Press Yes for show UpToDate", Text, MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);

        if (result == DialogResult.Yes)
        {
            _up2date = true;
        }


        backgroundWorker1.RunWorkerAsync();
    }

    private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
        var sw = new Stopwatch();
        sw.Start();
        PullThemAll();
        sw.Stop();

        WriteOutput("================= Done =================", LogsType.System);
        WriteOutput(ToTime(sw.Elapsed), LogsType.System);
        WriteOutput("========================================", LogsType.System);

        backgroundWorker1.CancelAsync();
    }

    private void PullThemAll()
    {
        var listPath = GetPath();
        var min = 0;
        var max = 0;
        max += listPath.Sum(t => t.Count);

        var header = false;
        foreach (var paths in listPath)
        {
            if (_up2date)
            {
                WriteOutput("================= " + paths[0] + " =================", LogsType.Directory);
                header = true;
            }
            foreach (var path in paths)
            {
                try
                {
                    min++;
                    Invoke(new MethodInvoker(delegate
                    {
                        Text = $"In process {min}/{max}";
                    }));

                    var result = GitPull(path);
                    if (result == null)
                    {
                        Debug.WriteLine("It's null");
                        continue;
                    }

                    if (result.Status == MergeStatus.FastForward)
                    {
                        if (!header)
                        {
                            WriteOutput("================= " + paths[0] + " =================", LogsType.Directory);
                            header = true;
                        }
                        WriteOutput(path, LogsType.Path);
                        WriteOutput(result.Status.ToString(), LogsType.Status);
                        WriteOutput(result.Commit.ToString(), LogsType.Commit);
                        WriteOutput("", LogsType.Empty);
                    }
                    else if (result.Status == MergeStatus.UpToDate && !_up2date)
                    {
                        Debug.WriteLine(path);
                        Debug.WriteLine(result.Status);
                    }
                    else
                    {
                        if (!header)
                        {
                            WriteOutput("================= " + paths[0] + " =================", LogsType.Directory);
                            header = true;
                        }
                        WriteOutput(path, LogsType.Path);
                        WriteOutput(result.Status.ToString(), LogsType.Status);
                        WriteOutput(result.Commit.ToString(), LogsType.Commit);
                        WriteOutput("", LogsType.Empty);
                    }
                }
                catch (Exception ex)
                {
                    WriteOutput($"Error at {path} - In Process {min}", LogsType.Error);
                    WriteOutput(ex.Message, LogsType.Error);
                    WriteOutput("", LogsType.Error);
                }
            }

            header = false;
        }

        SaveLogs();
    }

    private static MergeResult? GitPull(string path)
    {
        try
        {
            using var repo = new Repository(path);
            foreach (var submodule in repo.Submodules)
            {
                var subRepoPath = Path.Combine(repo.Info.WorkingDirectory, submodule.Path);

                using var subRepo = new Repository(subRepoPath);
                var remoteBranch = subRepo.Branches["origin/master"];
                subRepo.Reset(ResetMode.Hard, remoteBranch.Tip);
            }
            var signature = repo.Config.BuildSignature(DateTimeOffset.Now);
            var pullResult = Commands.Pull(repo, signature, new PullOptions());
            return pullResult;
        }
        catch
        {
            return null;
        }
    }

    private static List<List<string>> GetPath()
    {
        return (from t in Program.PathSetting.PathInfo where Directory.Exists(t.Path) select GetDirectory(t.Path, t.Depth, true)).ToList();
    }

    private static List<string> GetDirectory(string root, int depth, bool except)
    {
        var folders = new List<string> { root };
        foreach (var directory in Directory.EnumerateDirectories(root))
        {
            if (except)
            {
                if (IsExcept(directory))
                {
                    continue;
                }
            }
            folders.Add(directory);
            if (depth > 0)
            {
                var result = GetDirectory(directory, depth - 1, except);
                folders.AddRange(result);
            }
        }

        return folders;
    }

    private static bool IsExcept(string directory)
    {
        return directory.Contains("#Archived") || directory.Contains("#Remove") || directory.Contains("#My Project") || directory.Contains("#Old") || directory.EndsWith("Logs") || directory.Equals("dnSpy");
    }


    //https://stackoverflow.com/a/4423615
    private static string ToTime(TimeSpan span)
    {
        var day = span.Duration().Days > 0
            ? $"{span.Days:0} day{(span.Days == 1 ? string.Empty : "s")}, "
            : string.Empty;

        var hours = span.Duration().Hours > 0
            ? $"{span.Hours:0} hour{(span.Hours == 1 ? string.Empty : "s")}, "
            : string.Empty;

        var min = span.Duration().Minutes > 0
            ? $"{span.Minutes:0} minute{(span.Minutes == 1 ? string.Empty : "s")}, "
            : string.Empty;

        var sec = span.Duration().Seconds > 0
            ? $"{span.Seconds:0} second{(span.Seconds == 1 ? string.Empty : "s")}"
            : string.Empty;

        var ms = span.Duration().Milliseconds > 0
            ? $"{span.Milliseconds:0} ms{(span.Milliseconds == 1 ? string.Empty : "ms")}"
            : string.Empty;

        var formatted = day + hours + min + sec;

        if (formatted.EndsWith(", ")) formatted = formatted[..^2];

        if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

        return formatted;
    }

    #region Logs

    private static void AppendText(RichTextBox box, string text, string strColor, Color color)
    {
        box.SelectionStart = box.TextLength;
        box.SelectionLength = 0;
        box.SelectionColor = color;
        box.AppendText(strColor);
        box.SelectionColor = box.ForeColor;
        box.AppendText(text);
        box.ScrollToCaret();
    }

    private void TextToLogs(string str, string strColor, Color color)
    {
        Invoke(new MethodInvoker(delegate
        {
            AppendText(txtLogs, str, strColor, color);
        }));
    }

    private void TextToLogs(string strColor, Color color)
    {
        Invoke(new MethodInvoker(delegate
        {
            AppendText(txtLogs, "", strColor, color);
        }));
    }

    public void WriteOutput(string str, LogsType type, string errorNum = null)
    {
        switch (type)
        {
            case LogsType.Path:
                TextToLogs(str + Environment.NewLine, "[Path] ", Color.DodgerBlue);
                break;

            case LogsType.Directory:
                TextToLogs(str + Environment.NewLine, Color.Orange);
                break;

            case LogsType.Status:
                TextToLogs(str + Environment.NewLine, "[Status] ", Color.DarkSalmon);
                break;

            case LogsType.Commit:
                TextToLogs(str + Environment.NewLine, "[Commit] ", Color.DarkSalmon);
                break;

            case LogsType.System:
                TextToLogs(str + Environment.NewLine, Color.Aqua);
                break;
            case LogsType.Error:

                TextToLogs(str + Environment.NewLine, "[Error] ", Color.Red);
                break;

            case LogsType.Empty:
                TextToLogs(str + Environment.NewLine, "", Color.Gold);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void SaveLogs()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var date = DateTime.Now.ToString("yyyy-MM-dd");
        var file = Path.Combine(path, date + ".txt");
        for (var i = 0; ; i++)
        {
            if (File.Exists(file))
            {
                file = Path.Combine(path, date + "_" + i + ".txt");
                continue;
            }
            break;
        }
        Invoke(new MethodInvoker(delegate
        {
            File.WriteAllText(file, txtLogs.Text);
        }));

        WriteOutput($"Log saved successfully. \"{file}\"", LogsType.System);
    }



    public enum LogsType
    {
        Status,
        Commit,
        Path,
        Directory,
        System,
        Empty,
        Error

    }

    #endregion


}