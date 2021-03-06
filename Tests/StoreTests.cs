﻿#region copyright
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flux.Stores;
using Flux.Payloads;

namespace Flux.Tests
{
    [TestClass]
    public class StoreTests
    {
        private class TestStore : Store
        {
            public bool SetupChecker = false;

            public override void ReceiveAction(IPayload payload)
            {
                Emit();
            }

            protected override void Setup()
            {
                SetupChecker = true;
            }
        }

        [TestMethod]
        public void Store_OnInstantiation_RegistersCallback ()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStore store = StoreFactory<TestStore>.GetUniqueStore(dispatcher);

            Assert.IsTrue(dispatcher.HasRegistered(store.DispatchToken));
        }

        [TestMethod]
        public void Store_OnInstantiation_RunsSetupMethod()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStore store = StoreFactory<TestStore>.GetUniqueStore(dispatcher);

            Assert.IsTrue(store.SetupChecker);
        }

        [TestMethod]
        public void Store_OnDisposal_DeregistersCallback ()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStore store = StoreFactory<TestStore>.GetUniqueStore(dispatcher);
            DispatchToken token = store.DispatchToken;
            store.Dispose();

            Assert.IsFalse(dispatcher.HasRegistered(token));
        }

        [TestMethod]
        public void Store_OnRecievePayload_EmitsEvent ()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStore store = StoreFactory<TestStore>.GetUniqueStore(dispatcher);

            bool listenerHit = false;

            Action testListener = () =>
            {
                listenerHit = true;
            };

            store.AddListener(EventType.Change, testListener);

            store.ReceiveAction(new Payload("test", true));

            Assert.IsTrue(listenerHit);
        }

        [TestMethod]
        public void Store_OnRemoveListener_RemovesListener ()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStore store = StoreFactory<TestStore>.GetUniqueStore(dispatcher);

            int listenerHit = 0;

            Action testListener = () =>
            {
                listenerHit++;
            };

            store.AddListener(EventType.Change, testListener);
            dispatcher.Dispatch("");
            store.RemoveListener(EventType.Change, testListener);
            dispatcher.Dispatch("");

            Assert.AreEqual(1, listenerHit);
        }

        public void Store_OnRegister_SavesDispatchTokenStatic()
        {
            Dispatcher dispatcher = new Dispatcher();
            StoreFactory<TestStore>.GetStore(dispatcher);

            Assert.IsNotNull(StoreFactory<TestStore>.DispatchToken);
        }

        public void Store_OnRegisterUnique_SavesDispatchTokenInstanced()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStore store = StoreFactory<TestStore>.GetUniqueStore(dispatcher);

            Assert.IsNotNull(store.DispatchToken);
        }
    }
}
