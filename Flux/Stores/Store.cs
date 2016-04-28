using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flux;
using Flux.Payloads;

namespace Flux.Stores
{
    public abstract class Store : Flux.IStore
    {
        public DispatchToken DispatchToken { get; protected set; }
        public Dispatcher Dispatcher { get; protected set; }

        public Store(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            DispatchToken = Dispatcher.Instance.Register(ReceiveAction);
        }

        ~Store()
        {
            Dispatcher.Deregister(DispatchToken);
        }

        public void ReceiveAction(IPayload payload)
        {
            throw new NotImplementedException();
        }
    }
}
