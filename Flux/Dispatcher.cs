using Flux.Payloads;
using System;
using System.Collections.Generic;

namespace Flux
{
    public class Dispatcher
    {
        protected Dictionary<DispatchToken, Action<IPayload>> callbackRegistry = new Dictionary<DispatchToken, Action<IPayload>>();
        protected IPayload currentPayload = null;
        protected List<DispatchToken> isPending = new List<DispatchToken>();
        protected List<DispatchToken> isHandled = new List<DispatchToken>();

        public Dispatcher Instance { get; protected set; }

        public bool IsDispatching { get; protected set; }

        public Dispatcher()
        {
            Instance = this;
        }

        public DispatchToken Register(Action<IPayload> callback)
        {
            DispatchToken dispatchToken = new DispatchToken();
            callbackRegistry.Add(dispatchToken, callback);
            return dispatchToken;
        }

        public void Deregister(DispatchToken dispatchToken)
        {
            if (!HasRegistered(dispatchToken))
                throw new DispatcherExceptions.InvalidCallbackException();

            callbackRegistry.Remove(dispatchToken);
        }

        public bool HasRegistered(DispatchToken dispatchToken)
        {
            return dispatchToken != null && callbackRegistry.ContainsKey(dispatchToken);
        }

        public void Dispatch(IPayload payload)
        {
            if (IsDispatching)
                throw new DispatcherExceptions.AlreadyDispatchingException();

            startDispatching(payload);

            try
            {
                foreach (KeyValuePair<DispatchToken, Action<IPayload>> callback in callbackRegistry)
                {
                    if (isPending.Contains(callback.Key))
                        continue;

                    invokeCallback(callback.Key);
                }
            }
            finally
            {
                stopDispatching();
            }
        }

        protected void startDispatching(IPayload payload) {
            isPending = new List<DispatchToken>();
            isHandled = new List<DispatchToken>();
            currentPayload = payload;
            IsDispatching = true;
        }

        protected void stopDispatching()
        {
            currentPayload = null;
            IsDispatching = false;
        }

        public void WaitFor(List<DispatchToken> dispatchTokenList)
        {
            if (!IsDispatching)
                throw new DispatcherExceptions.NotDispatchingException();

            dispatchTokenList.ForEach((DispatchToken dispatchToken) =>
            {
                if (isPending.Contains(dispatchToken) || isHandled.Contains(dispatchToken))
                    throw new DispatcherExceptions.CircularDependecyException();

                if (!callbackRegistry.ContainsKey(dispatchToken))
                    throw new DispatcherExceptions.InvalidCallbackException();

                invokeCallback(dispatchToken);
            });
        }
        public void WaitFor(DispatchToken dispatchToken)
        {
            WaitFor(new List<DispatchToken>() { dispatchToken });
        }

        protected void invokeCallback(DispatchToken dispatchToken)
        {
            isPending.Add(dispatchToken);
            callbackRegistry[dispatchToken](currentPayload);
            isHandled.Add(dispatchToken);
        }
    }
}
