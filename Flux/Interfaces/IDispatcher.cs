using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
