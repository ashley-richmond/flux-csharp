using Flux.Payloads;
using System;

namespace Flux.Stores
{
    public interface IStore: IDisposable
    {
        Dispatcher Dispatcher { get; set; }
        DispatchToken DispatchToken { get; }
        void ReceiveAction(IPayload payload);

        void AddListener(Action callback);
        void AddListener(EventType type, Action callback);
        void RemoveListener(Action callback);
        void RemoveListener(EventType type, Action callback);
    }
}
