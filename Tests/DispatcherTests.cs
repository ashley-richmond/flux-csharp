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
        public void Dispatcher_OnDispatch_DeliversPayload()
        {
            Dispatcher dispatcher = new Dispatcher();
            dispatcher.Register(p =>
            {
                Assert.IsTrue(p.Type == typeof(int));
            });

            Payload payload = new Payload("", 3);

            dispatcher.Dispatch(payload);
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
