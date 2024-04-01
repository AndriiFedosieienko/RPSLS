namespace RPSLS.Applications.Exceptions
{
    /// <summary>
    /// Thrown when the user is not properly authenticated or authorized to continue.
    /// </summary>
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string message) : base(message) { }
    }
}
