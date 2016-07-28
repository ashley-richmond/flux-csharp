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

        protected static Dictionary<Type, IStore> _instances = new Dictionary<Type, IStore>();

        private List<Listener> listeners = new List<Listener>();

        /// <summary>
        /// Create a new instance of the given store type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        public static T Factory<T>(Dispatcher dispatcher) where T : IStore, new()
        {
            T instance = new T();
            instance.Dispatcher = dispatcher;
            return instance;
        }

        /// <summary>
        /// Create or retrieve an instance of the given store type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        public static T SingletonFactory<T>(Dispatcher dispatcher) where T : IStore, new()
        {
            Type type = typeof(T);

            if (!_instances.ContainsKey(type))
                _instances.Add(type, Factory<T>(dispatcher));

            return (T)_instances[type];
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

        /// <summary>
        /// Handles receiving a payload from the dispatcher.
        /// </summary>
        /// <param name="payload"></param>
        public abstract void ReceiveAction(IPayload payload);

        /// <summary>
        /// Triggers the callbacks listening to the given event type.
        /// </summary>
        /// <param name="type"></param>
        protected void Emit(EventType type)
        {
            listeners
                .Where(listener => listener.Type == type)
                .ToList()
                .ForEach(listener => listener.Action());
        }

        /// <summary>
        /// Triggers the callbacks listening to the change event.
        /// </summary>
        protected void EmitChange()
        {
            Emit(EventType.Change);
        }

        /// <summary>
        /// Adds a listener on a given store event.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        public void AddListener(EventType type, Action callback)
        {
            listeners.Add(new Listener(type, callback));
        }
        /// <summary>
        /// Adds a listener on the store's change event.
        /// </summary>
        /// <param name="callback"></param>
        public void AddListener(Action callback)
        {
            AddListener(EventType.Change, callback);
        }

        /// <summary>
        /// Removes a listener assigned to the given store event.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        public void RemoveListener(EventType type, Action callback)
        {
            listeners
                .Where(listener => listener.Type == type && listener.Action == callback)
                .ToList()
                .ForEach(listener => listeners.Remove(listener));
        }
        /// <summary>
        /// Removes a listener assigned to the store's change event.
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveListener(Action callback)
        {
            RemoveListener(EventType.Change, callback);
        }
    }
}
