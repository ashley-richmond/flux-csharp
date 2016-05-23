using Flux.Payloads;
using System;

namespace Flux.Stores
{
    public interface IStore: IDisposable
    {
        Dispatcher Dispatcher { get; }
        DispatchToken DispatchToken { get; }
        void ReceiveAction(IPayload payload);

        void AddListener(EventType type, Action callback);
        void RemoveListener(EventType type, Action callback);
    }
}
