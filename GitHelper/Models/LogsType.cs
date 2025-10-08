namespace GitHelper.Models;

/// <summary>
/// Log types for categorizing different kinds of log messages
/// </summary>
public enum LogsType
{
    Status,
    Commit,
    Path,
    Url,
    Directory,
    System,
    Empty,
    Error
}