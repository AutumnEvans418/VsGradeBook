namespace Grader
{
    public class RepositoryResult<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public RepositoryStatus Status { get; set; }
    }
}