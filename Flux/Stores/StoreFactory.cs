using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux.Stores
{
    public static class StoreFactory<T> where T : IStore, new()
    {
        private static T instance;

        public static DispatchToken DispatchToken { get { return instance.DispatchToken; } }

        /// <summary>
        /// Create or retrieve an instance of the given store type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        public static T GetStore(Dispatcher dispatcher)
        {
            if (instance == null)
                instance = CreateInstance(dispatcher);

            return instance;
        }

        /// <summary>
        /// Create a new instance of the given store type, regardless of what's stored.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        public static T GetUniqueStore(Dispatcher dispatcher)
        {
            return CreateInstance(dispatcher);
        }

        /// <summary>
        /// Create a new instance of the given store type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        private static T CreateInstance(Dispatcher dispatcher)
        {
            IStore store = new T();
            store.Dispatcher = dispatcher;
            return (T)store;
        }
    }
}
