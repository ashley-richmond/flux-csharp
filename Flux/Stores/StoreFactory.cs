#region copyright
// Copyright 2016 Ashley Richmond
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

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
