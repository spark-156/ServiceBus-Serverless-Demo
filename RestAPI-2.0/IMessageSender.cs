namespace RestAPI_2._0;

public interface IMessageSender
{
    Task Send(string message);
}
