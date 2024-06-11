using System.Runtime.Serialization;

namespace TodoApp.Repositories
{
    [Serializable]
    internal class UserEmailAlreadyExsits : Exception
    {
        public UserEmailAlreadyExsits()
        {
        }

        public UserEmailAlreadyExsits(string? message) : base(message)
        {
        }

        public UserEmailAlreadyExsits(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserEmailAlreadyExsits(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}