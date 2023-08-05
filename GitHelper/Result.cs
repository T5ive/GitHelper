namespace GitHelper;

public class Result
{
    public MergeResult MergeResult { get; set; }

    public Repository Repository { get; set; }

    public Result(MergeResult mergeResult, Repository repository)
    {
        MergeResult = mergeResult;
        Repository = repository;
    }
}