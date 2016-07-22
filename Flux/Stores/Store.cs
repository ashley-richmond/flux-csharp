using Flux.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;

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
        
        public Dispatcher Dispatcher {
            get
            {
                if (_dispatcher == null)
                    throw new DispatcherNotSetException();

                return _dispatcher;
            }
            set
            {
                if (_dispatcher != null)
                    Dispose();

                _dispatcher = value;
                DispatchToken = _dispatcher.Register(ReceiveAction);
            }
        }

        protected Dispatcher _dispatcher;

        protected static IStore _instance;

        private List<Listener> listeners = new List<Listener>();

        public static T Factory<T>(Dispatcher dispatcher) where T : IStore, new()
        {
            T instance = new T();
            instance.Dispatcher = dispatcher;
            return instance;
        }

        public static T SingletonFactory<T>(Dispatcher dispatcher) where T : IStore, new()
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.Dispatcher = dispatcher;
            }

            return (T)_instance;
        }

        protected Store()
        {
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
