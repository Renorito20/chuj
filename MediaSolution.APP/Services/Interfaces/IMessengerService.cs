using CommunityToolkit.Mvvm.Messaging;

namespace MediaSolution.APP.Services.Interfaces;

public interface IMessengerService
{
    IMessenger Messenger { get; }

    void Send<TMessage>(TMessage message)
        where TMessage : class;
}