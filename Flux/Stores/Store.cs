using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flux;
using Flux.Payloads;

namespace Flux.Stores
{
    public abstract class Store : IStore
    {
        private class Listener
        {
            public EventType Type { get; private set; }
            public Action Action { get; private set; }

            public Listener(EventType type, Action action)
            {
                Type = type;
                Action = action;
            }
        }

        public DispatchToken DispatchToken { get; protected set; }
        public Dispatcher Dispatcher { get; protected set; }

        private List<Listener> listeners = new List<Listener>();

        public Store(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            DispatchToken = Dispatcher.Instance.Register(ReceiveAction);
        }

        ~Store()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispatcher.Deregister(DispatchToken);
        }

        public abstract void ReceiveAction(IPayload payload);

        protected void Emit(EventType type)
        {
            listeners
                .Where(listener => listener.Type == type)
                .ToList()
                .ForEach(listener => listener.Action());
        }

        public void AddListener(EventType type, Action callback)
        {
            listeners.Add(new Listener(type, callback));
        }

        public void RemoveListener(EventType type, Action callback)
        {
            listeners
                .Where(listener => listener.Type == type && listener.Action == callback)
                .ToList()
                .ForEach(listener => listeners.Remove(listener));
        }
    }
}
