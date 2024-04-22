namespace GitHelper;

public class Result
{
    public MergeResult MergeResult { get; set; }

    public Repository Repository { get; set; }

    public string? Error { get; set; }

    public Result(MergeResult mergeResult, Repository repository)
    {
        MergeResult = mergeResult;
        Repository = repository;
    }

    public Result(string message)
    {
        Error = message;
    }
}