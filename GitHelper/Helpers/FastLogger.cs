using System.Collections.Concurrent;
using System.Text;

namespace GitHelper.Helpers;

/// <summary>
/// High-performance logging system with batched UI updates and async file operations
/// </summary>
public class FastLogger : IDisposable
{
    private readonly RichTextBox _logTextBox;
    private readonly ConcurrentQueue<LogEntry> _logQueue = new();
    private readonly StringBuilder _logFileBuffer = new();
    private readonly System.Threading.Timer _uiUpdateTimer;
    private readonly System.Threading.Timer _fileFlushTimer;
    private readonly SemaphoreSlim _fileSemaphore = new(1, 1);
    
    private string? _currentLogFile;
    private bool _disposed;
    private volatile bool _saveRequired;

    // Configuration (can be customized via settings)
    private readonly int _uiUpdateInterval;
    private readonly int _fileFlushInterval;
    private readonly int _maxBatchSize;
    private const int MAX_BUFFER_SIZE = 1024 * 1024; // 1MB buffer

    public FastLogger(RichTextBox logTextBox, int? uiUpdateInterval = null, int? fileFlushInterval = null, int? maxBatchSize = null)
    {
        _logTextBox = logTextBox ?? throw new ArgumentNullException(nameof(logTextBox));
        
        // Load configuration from settings or use defaults
        _uiUpdateInterval = uiUpdateInterval ?? GitHelper.Properties.Settings.Default.LogUIUpdateInterval;
        _fileFlushInterval = fileFlushInterval ?? GitHelper.Properties.Settings.Default.LogFileFlushInterval;
        _maxBatchSize = maxBatchSize ?? GitHelper.Properties.Settings.Default.LogBatchSize;
        
        // Create timers for batched updates
        _uiUpdateTimer = new System.Threading.Timer(ProcessUIUpdates, null, _uiUpdateInterval, _uiUpdateInterval);
        _fileFlushTimer = new System.Threading.Timer(FlushToFile, null, _fileFlushInterval, _fileFlushInterval);
        
        InitializeLogFile();
    }

    public void LogAsync(string message, LogsType type)
    {
        if (_disposed) return;

        var entry = new LogEntry
        {
            Message = message,
            Type = type,
            Timestamp = DateTime.Now,
            ThreadId = Environment.CurrentManagedThreadId
        };

        _logQueue.Enqueue(entry);
    }

    private void ProcessUIUpdates(object? state)
    {
        if (_disposed || _logTextBox.IsDisposed) return;

        var entries = new List<LogEntry>();
        var processedCount = 0;

        // Dequeue up to _maxBatchSize entries
        while (_logQueue.TryDequeue(out var entry) && processedCount < _maxBatchSize)
        {
            entries.Add(entry);
            processedCount++;
        }

        if (entries.Count == 0) return;

        try
        {
            // Update UI on UI thread
            _logTextBox.Invoke(new Action(() =>
            {
                try
                {
                    // Suspend layout for better performance
                    _logTextBox.SuspendLayout();
                    
                    var sb = new StringBuilder();
                    foreach (var entry in entries)
                    {
                        AppendToUI(entry);
                        AppendToBuffer(entry, sb);
                    }
                    
                    // Add to file buffer
                    if (sb.Length > 0)
                    {
                        lock (_logFileBuffer)
                        {
                            _logFileBuffer.Append(sb);
                            _saveRequired = true;
                            
                            // If buffer is getting too large, force flush
                            if (_logFileBuffer.Length > MAX_BUFFER_SIZE)
                            {
                                _ = Task.Run(async () => await FlushToFileAsync());
                            }
                        }
                    }
                }
                finally
                {
                    _logTextBox.ResumeLayout();
                    _logTextBox.ScrollToCaret();
                }
            }));
        }
        catch (ObjectDisposedException)
        {
            // Form is being disposed, ignore
        }
        catch (Exception ex)
        {
            // Log to debug output if UI logging fails
            Debug.WriteLine($"FastLogger UI update failed: {ex.Message}");
        }
    }

    private void AppendToUI(LogEntry entry)
    {
        if (_logTextBox.IsDisposed) return;

        var (prefix, color) = GetLogFormatting(entry.Type);
        
        // Set color and append prefix
        _logTextBox.SelectionStart = _logTextBox.TextLength;
        _logTextBox.SelectionLength = 0;
        _logTextBox.SelectionColor = color;
        _logTextBox.AppendText(prefix);
        
        // Reset to default color and append message
        _logTextBox.SelectionColor = _logTextBox.ForeColor;
        _logTextBox.AppendText(entry.Message + Environment.NewLine);
    }

    private static void AppendToBuffer(LogEntry entry, StringBuilder sb)
    {
        var (prefix, _) = GetLogFormatting(entry.Type);
        sb.AppendLine($"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}] {prefix}{entry.Message}");
    }

    private static (string prefix, Color color) GetLogFormatting(LogsType type)
    {
        return type switch
        {
            LogsType.Path => ("[Path] ", Color.DodgerBlue),
            LogsType.Url => ("[Url] ", Color.DodgerBlue),
            LogsType.Directory => ("", Color.Orange),
            LogsType.Status => ("[Status] ", Color.DarkSalmon),
            LogsType.Commit => ("[Commit] ", Color.DarkSalmon),
            LogsType.System => ("", Color.Aqua),
            LogsType.Error => ("[Error] ", Color.Red),
            LogsType.Empty => ("", Color.Gold),
            _ => ("", Color.White)
        };
    }

    private void InitializeLogFile()
    {
        try
        {
            var logsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(logsPath))
            {
                Directory.CreateDirectory(logsPath);
            }

            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var baseFileName = Path.Combine(logsPath, date);
            
            _currentLogFile = $"{baseFileName}.txt";
            var counter = 0;
            
            while (File.Exists(_currentLogFile))
            {
                _currentLogFile = $"{baseFileName}_{++counter}.txt";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to initialize log file: {ex.Message}");
        }
    }

    private void FlushToFile(object? state)
    {
        if (!_saveRequired) return;
        _ = Task.Run(async () => await FlushToFileAsync());
    }

    private async Task FlushToFileAsync()
    {
        if (_disposed || string.IsNullOrEmpty(_currentLogFile) || !_saveRequired)
            return;

        await _fileSemaphore.WaitAsync();
        try
        {
            string contentToWrite;
            lock (_logFileBuffer)
            {
                if (_logFileBuffer.Length == 0)
                {
                    _saveRequired = false;
                    return;
                }
                
                contentToWrite = _logFileBuffer.ToString();
                _logFileBuffer.Clear();
                _saveRequired = false;
            }

            await File.AppendAllTextAsync(_currentLogFile, contentToWrite);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to write log file: {ex.Message}");
        }
        finally
        {
            _fileSemaphore.Release();
        }
    }

    public async Task SaveLogsAsync(bool forceFlush = false)
    {
        if (_disposed) return;

        // Process any remaining queue items immediately if forcing flush
        if (forceFlush)
        {
            ProcessUIUpdates(null);
        }

        // Flush any remaining buffer to file
        await FlushToFileAsync();

        if (!string.IsNullOrEmpty(_currentLogFile))
        {
            var message = $"Log saved successfully. \"{_currentLogFile}\"";
            LogAsync(message, LogsType.System);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _uiUpdateTimer?.Dispose();
        _fileFlushTimer?.Dispose();

        // Process any remaining logs
        ProcessUIUpdates(null);
        
        // Final flush to file
        Task.Run(async () => await FlushToFileAsync()).Wait(TimeSpan.FromSeconds(5));

        _fileSemaphore?.Dispose();
    }
}

public class LogEntry
{
    public string Message { get; set; } = string.Empty;
    public LogsType Type { get; set; }
    public DateTime Timestamp { get; set; }
    public int ThreadId { get; set; }
}