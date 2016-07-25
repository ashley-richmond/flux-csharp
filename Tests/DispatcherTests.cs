using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flux;
using Flux.Stores;
using Flux.Payloads;

namespace Flux.Tests
{
    [TestClass]
    public class DispatcherTests
    {
        [TestMethod]
        public void Dispatcher_OnRegister_GivesValidToken()
        {
            Dispatcher dispatcher = new Dispatcher();
            DispatchToken dispatchToken = dispatcher.Register(p => { });
            
            Assert.IsTrue(dispatcher.HasRegistered(dispatchToken));
        }

        [TestMethod]
        public void Dispatch_CanTakeTwoArgs()
        {
            Dispatcher dispatcher = new Dispatcher();
            dispatcher.Dispatch("Whatup", true);
        }

        [TestMethod]
        public void Dispatcher_OnDispatch_DeliversPayload()
        {
            bool received = false;

            Dispatcher dispatcher = new Dispatcher();
            dispatcher.Register(p =>
            {
                received = true;
            });

            Payload payload = new Payload("", 3);

            dispatcher.Dispatch(payload);

            Assert.IsTrue(received);
        }

        [TestMethod]
        [ExpectedException(typeof(Flux.DispatcherExceptions.AlreadyDispatchingException))]
        public void Dispatcher_OnDispatch_DisablesDispatch()
        {
            Dispatcher dispatcher = new Dispatcher();
            DispatchToken dispatchToken = dispatcher.Register(p =>
            {
                dispatcher.Dispatch(new Payload("", 349));
            });

            dispatcher.Dispatch(new Payload("", 5));
        }

        [TestMethod]
        public void Dispatcher_OnDispatch_AcceptsWaitForRequests()
        {
            bool hasWaited = false;

            Dispatcher dispatcher = new Dispatcher();

            DispatchToken second = null;
            DispatchToken first = dispatcher.Register(p => {
                dispatcher.WaitFor(second);
                Assert.IsTrue(hasWaited);
            });
            second = dispatcher.Register(p => { hasWaited = true; });

            dispatcher.Dispatch(new Payload("", 4));
        }

        [TestMethod]
        [ExpectedException(typeof(Flux.DispatcherExceptions.CircularDependecyException))]
        public void Dispatcher_OnDispatch_RejectsCircularWaitForRequests()
        {
            Dispatcher dispatcher = new Dispatcher();

            DispatchToken second = null;
            DispatchToken first = dispatcher.Register(p => {
                dispatcher.WaitFor(second);
            });
            second = dispatcher.Register(p => {
                dispatcher.WaitFor(first);
            });

            dispatcher.Dispatch(new Payload("", 4));
        }

        [TestMethod]
        public void Dispatcher_OnFinishDispatch_EnablesDispatch()
        {
            int dispatchCounter = 0;

            Dispatcher dispatcher = new Dispatcher();

            DispatchToken dispatchToken = dispatcher.Register(p => { dispatchCounter++; });

            dispatcher.Dispatch(new Payload("", 0));
            dispatcher.Dispatch(new Payload("", 1));

            Assert.AreEqual(dispatchCounter, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(Flux.DispatcherExceptions.NotDispatchingException))]
        public void Dispatcher_WhileNotDispatching_RejectsWaitForRequests()
        {
            Dispatcher dispatcher = new Dispatcher();
            DispatchToken dispatchToken = dispatcher.Register(p => { });

            dispatcher.WaitFor(dispatchToken);
        }

        [TestMethod]
        [ExpectedException(typeof(Flux.DispatcherExceptions.InvalidCallbackException))]
        public void Dispatcher_OnDeregister_RejectsInvalidDispatcherTokens()
        {
            (new Dispatcher()).Deregister(null);
        }

        [TestMethod]
        public void Dispatcher_OnDeregister_RemovesCallback()
        {
            Dispatcher dispatcher = new Dispatcher();
            DispatchToken dispatchToken = dispatcher.Register(p => { });

            dispatcher.Deregister(dispatchToken);

            Assert.IsFalse(dispatcher.HasRegistered(dispatchToken));
        }
    }
}
