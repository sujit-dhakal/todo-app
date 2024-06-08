using System.Runtime.Serialization;

namespace TodoApp.Repositories
{
    [Serializable]
    internal class UserNameAlreadyExists : Exception
    {
        public UserNameAlreadyExists()
        {
        }

        public UserNameAlreadyExists(string? message) : base(message)
        {
        }

        public UserNameAlreadyExists(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserNameAlreadyExists(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}