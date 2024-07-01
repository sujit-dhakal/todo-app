namespace TodoApp.Services
{
    public interface IRabbitMQService
    {
        void SendMessage(string message);
    }
}
