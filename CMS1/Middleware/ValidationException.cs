namespace UserManagement.Middleware
{
    public class ValidationException : ApplicationException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("One or more validation errors occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(Dictionary<string, string[]> errors)
            : this()
        {
            Errors = errors;
        }

        public ValidationException(string message)
            : base(message)
        {
            Errors = new Dictionary<string, string[]>
            {
                { "General", new[] { message } }
            };
        }
    }
}
