namespace TodoApp.Model
{
    public class SmtpSettings
    {
            public string SmtpServer { get; set; }
            public int Port { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string FromAddress { get; set; }
        }
}
