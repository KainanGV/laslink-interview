namespace LastLink.Anticipation.Application.Exceptions
{
    public sealed class AppValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public AppValidationException(string message, IDictionary<string, string[]>? errors = null)
            : base(message)
        {
            Errors = errors ?? new Dictionary<string, string[]>();
        }
    }
}
