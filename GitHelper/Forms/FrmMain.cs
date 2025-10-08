namespace GitHelper.Forms;

public partial class FrmMain : Form
{
    #region Variable

    private static readonly List<string> ReturnedPaths = [];
    private FastLogger? _fastLogger;
    private static bool _up2date;

    #endregion Variable

    #region Init/Load/Close

    public FrmMain()
    {
        InitializeComponent();
        InitializeLogging();
    }

    private void InitializeLogging()
    {
        _fastLogger = new FastLogger(txtLogs);
    }

    private async void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (_fastLogger != null)
        {
            await _fastLogger.SaveLogsAsync(forceFlush: true);
            _fastLogger.Dispose();
        }
    }

    #endregion Init/Load/Close

    #region Events

    private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var settingsForm = new FrmSettings();
        var result = settingsForm.ShowDialog(this);
        if (result == DialogResult.OK)
        {
            _fastLogger?.LogAsync("Settings updated.", LogsType.System);
        }
    }

    private void btnRun_Click(object sender, EventArgs e)
    {
        SetControlsEnabled(false);
        var result = MessageBox.Show(@"Press Yes for show UpToDate", Text, MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);

        if (result == DialogResult.Yes)
        {
            _up2date = true;
        }

        backgroundWorker1.RunWorkerAsync();
    }

    private void txtLogs_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.LinkText))
        {
            var url = e.LinkText;
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "https://" + url;
            }

            try
            {
                Process.Start(url);
            }
            catch
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }
    }

    #endregion Events

    #region Git Worker

    private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
        var sw = new Stopwatch();
        sw.Start();
        ProcessRepositories();
        sw.Stop();

        _fastLogger?.LogAsync("================= Done =================", LogsType.System);
        _fastLogger?.LogAsync(ToTime(sw.Elapsed), LogsType.System);
        _fastLogger?.LogAsync("========================================", LogsType.System);

        backgroundWorker1.CancelAsync();
        SetControlsEnabled(true);
    }

    private void ProcessRepositories()
    {
        var listPath = GetRepositoryPaths();
        var allPaths = listPath.SelectMany(x => x).ToList();
        var min = 0;
        var max = allPaths.Count;
        var headerShown = new HashSet<string>();

        var logger = _fastLogger; // Capture instance for use in parallel context
        Parallel.ForEach(allPaths, new ParallelOptions { MaxDegreeOfParallelism = Properties.Settings.Default.MaxParallel }, path =>
        {
            var headerPath = GetRepositoryHeader(path);
            var showHeader = false;
            lock (headerShown)
            {
                if (!string.IsNullOrEmpty(headerPath) && headerShown.Add(headerPath))
                {
                    showHeader = true;
                }
            }

            try
            {
                int current;
                lock (this)
                {
                    min++;
                    current = min;
                }

                Invoke(new MethodInvoker(delegate
                {
                    Text = @$"In process {current}/{max}";
                }));

                var result = PullRepository(path);
                if (result == null)
                {
                    Debug.WriteLine("Can't Pull");
                    return;
                }

                var remote = result.Repository.Network.Remotes.FirstOrDefault();
                var remoteUrl = remote?.Url.Replace(".git", "");
                if (result.MergeResult.Status == MergeStatus.FastForward)
                {
                    if (showHeader || (_up2date && !string.IsNullOrWhiteSpace(headerPath) && !headerShown.Contains(headerPath)))
                    {
                        logger?.LogAsync($"================= {headerPath} =================", LogsType.Directory);
                    }
                    logger?.LogAsync(path, LogsType.Path);
                    if (remoteUrl != null)
                        logger?.LogAsync(remoteUrl, LogsType.Url);
                    logger?.LogAsync(result.MergeResult.Status.ToString(), LogsType.Status);
                    logger?.LogAsync(result.MergeResult.Commit.ToString(), LogsType.Commit);
                    logger?.LogAsync("", LogsType.Empty);
                }
                else if (result.MergeResult.Status == MergeStatus.UpToDate && !_up2date)
                {
                    Debug.WriteLine(path);
                    Debug.WriteLine(result.MergeResult.Status);
                }
                else
                {
                    if (showHeader || (_up2date && !string.IsNullOrWhiteSpace(headerPath) && !headerShown.Contains(headerPath)))
                    {
                        logger?.LogAsync($"================= {headerPath} =================", LogsType.Directory);
                    }
                    logger?.LogAsync(path, LogsType.Path);
                    if (remoteUrl != null)
                        logger?.LogAsync(remoteUrl, LogsType.Url);
                    logger?.LogAsync(result.MergeResult.Status.ToString(), LogsType.Status);
                    if (result.MergeResult.Commit != null)
                        logger?.LogAsync(result.MergeResult.Commit.ToString(), LogsType.Commit);
                    logger?.LogAsync("", LogsType.Empty);
                }
            }
            catch (Exception ex)
            {
                logger?.LogAsync($"Error at {path}", LogsType.Error);
                logger?.LogAsync(ex.Message, LogsType.Error);
                logger?.LogAsync("", LogsType.Error);
            }
        });

        // Logging is now handled automatically by FastLogger
    }

    private static Result? PullRepository(string path)
    {
        try
        {
            var repo = new Repository(path);
            foreach (var submodule in repo.Submodules)
            {
                var subRepoPath = Path.Combine(repo.Info.WorkingDirectory, submodule.Path);

                using var subRepo = new Repository(subRepoPath);
                var allBranch = subRepo.Branches.ToList();
                if (allBranch is { Count: > 0 })
                {
                    var remoteBranch = subRepo.Branches.FirstOrDefault(u => u.IsCurrentRepositoryHead);
                    if (remoteBranch != null)
                        subRepo.Reset(ResetMode.Hard, remoteBranch.Tip);
                }
            }
            var signature = repo.Config.BuildSignature(DateTimeOffset.Now);
            var pullResult = Commands.Pull(repo, signature, new PullOptions());
            return new Result(pullResult, repo);
        }
        catch
        {
            return null;
        }
    }

    #endregion Git Worker

    #region Directory Helper

    private static List<List<string>> GetRepositoryPaths()
    {
        return [.. from t in Program.PathSetting.PathInfo
            where Directory.Exists(t.Path)
            select GetSubDirectories(t.Path, t.Depth, true)];
    }

    private static List<string> GetSubDirectories(string root, int depth, bool except)
    {
        var folders = new List<string>();
        foreach (var directory in Directory.EnumerateDirectories(root))
        {
            if (except)
            {
                if (IsIgnoredDirectory(directory))
                {
                    continue;
                }
            }

            if (IsGitRepository(directory))
                folders.Add(directory);

            if (depth > 0)
            {
                var result = GetSubDirectories(directory, depth - 1, except);
                folders.AddRange(result);
            }
        }

        return folders;
    }

    private static bool IsIgnoredDirectory(string directory)
    {
        foreach (var ignore in Program.PathSetting.IgnoreList)
        {
            switch (ignore.IgnoreType)
            {
                case IgnoreType.Contains:
                    if (directory.Contains(ignore.Name))
                        return true;
                    break;

                case IgnoreType.Equals:
                    var directoryName = Path.GetFileName(Path.GetDirectoryName(directory));
                    if (directoryName != null && directoryName.Equals(ignore.Name))
                        return true;
                    break;

                case IgnoreType.EndsWith:
                    if (directory.EndsWith(ignore.Name))
                        return true;
                    break;
            }
        }

        return false;
    }

    private static bool IsGitRepository(string directory)
    {
        return Directory.Exists(directory + "/.git");
    }

    private static string? GetRepositoryHeader(string? directory)
    {
        if (string.IsNullOrWhiteSpace(directory)) return null;
        foreach (var path in Program.PathSetting.PathInfo)
        {
            if (directory.Contains(path.Path) && !ReturnedPaths.Contains(path.Path))
            {
                ReturnedPaths.Add(path.Path);
                return path.Path;
            }
        }
        return null;
    }

    #endregion Directory Helper

    #region Utils

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

    private void SetControlsEnabled(bool enable)
    {
        Invoke(new MethodInvoker(delegate
        {
            btnRun.Enabled = enable;
            settingsToolStripMenuItem.Enabled = enable;
        }));
    }

    #endregion Utils

    #region Logs
    // LogsType enum moved to GitHelper.Models.LogsType
    #endregion Logs
}