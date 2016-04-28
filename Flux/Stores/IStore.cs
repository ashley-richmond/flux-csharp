using Flux.Payloads;

namespace Flux
{
    public interface IStore
    {
        Dispatcher Dispatcher { get; }
        DispatchToken DispatchToken { get; }
        void ReceiveAction(IPayload payload);
    }
}
