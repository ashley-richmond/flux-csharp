﻿using System;
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
            public TestStore(Dispatcher dispatcher) : base(dispatcher)
            {
            }

            public override void ReceiveAction(IPayload payload)
            {
                Emit(EventType.Change);
            }
        }

        [TestMethod]
        public void Store_OnInstantiation_RegistersCallback ()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStore store = new TestStore(dispatcher);

            Assert.IsTrue(dispatcher.HasRegistered(store.DispatchToken));
        }

        [TestMethod]
        public void Store_OnDisposal_DeregistersCallback ()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStore store = new TestStore(dispatcher);
            DispatchToken token = store.DispatchToken;
            store.Dispose();

            Assert.IsFalse(dispatcher.HasRegistered(token));
        }

        [TestMethod]
        public void Store_OnRecievePayload_EmitsEvent ()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStore store = new TestStore(dispatcher);

            bool listenerHit = false;

            Action testListener = () =>
            {
                listenerHit = true;
            };

            store.AddListener(EventType.Change, testListener);

            dispatcher.Dispatch(new Payload("wat", 4));

            Assert.IsTrue(listenerHit);
        }

        [TestMethod]
        public void Store_OnRemoveListener_RemovesListener ()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStore store = new TestStore(dispatcher);

            int listenerHit = 0;

            Action testListener = () =>
            {
                listenerHit++;
            };

            store.AddListener(EventType.Change, testListener);
            dispatcher.Dispatch(new Payload("wat", 4));
            store.RemoveListener(EventType.Change, testListener);
            dispatcher.Dispatch(new Payload("wat", 4));

            Assert.AreEqual(1, listenerHit);
        }
    }
}