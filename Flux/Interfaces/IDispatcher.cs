using System;
using System.Collections.Generic;

namespace Flux.Interfaces
{
    public interface IDispatcher
    {
        bool IsDispatching { get; }

        IDispatchToken Register(Action<IPayload<Object>> callback);
        void Deregister(IDispatchToken dispatchToken);
        void WaitFor(IDispatchToken dispatchToken);
        void WaitFor(List<IDispatchToken> dispatchTokenList);
        void Dispatch(IPayload<Object> payload);
    }
}
