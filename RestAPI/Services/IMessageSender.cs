namespace RestAPI.Services;

public interface IMessageSender
{
    Task Send(object message);
}
