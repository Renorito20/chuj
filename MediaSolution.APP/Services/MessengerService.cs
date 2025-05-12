using CommunityToolkit.Mvvm.Messaging;
using MediaSolution.APP.Services.Interfaces;

namespace MediaSolution.APP.Services;

public class MessengerService(IMessenger messenger) : IMessengerService
{
    public IMessenger Messenger { get; } = messenger;

    public void Send<TMessage>(TMessage message)
        where TMessage : class
    {
        Messenger.Send(message);
    }
}