namespace CrossCutting.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors)
            : base("Erro de validação.")
        {
            Errors = errors;
        }
    }
}
