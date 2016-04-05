using Flux.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux
{
    public class Dispatcher : IDispatcher
    {
        protected Dictionary<IDispatchToken, Action<IPayload<Object>>> callbackRegistry = new Dictionary<IDispatchToken, Action<IPayload<Object>>>();
        protected int lastId = 0;
        protected IPayload<Object> currentPayload = null;
        protected List<IDispatchToken> isPending = new List<IDispatchToken>();
        protected List<IDispatchToken> isHandled = new List<IDispatchToken>();

        public bool IsDispatching { get; protected set; }

        public IDispatchToken Register(Action<IPayload<Object>> callback)
        {
            IDispatchToken dispatchToken = new DispatchToken(lastId++);
            callbackRegistry.Add(dispatchToken, callback);
            return dispatchToken;
        }

        public void Deregister(IDispatchToken dispatchToken)
        {
            if (!callbackRegistry.ContainsKey(dispatchToken))
                throw new Exceptions.Dispatcher.InvalidCallbackException();

            callbackRegistry.Remove(dispatchToken);
        }

        public void Dispatch(IPayload<Object> payload)
        {
            if (IsDispatching)
                throw new Exceptions.Dispatcher.AlreadyDispatchingException();

            startDispatching(payload);

            try
            {
                foreach (KeyValuePair<IDispatchToken, Action<IPayload<Object>>> callback in callbackRegistry)
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

        protected void startDispatching(IPayload<Object> payload) {
            isPending = new List<IDispatchToken>();
            isHandled = new List<IDispatchToken>();
            currentPayload = payload;
            IsDispatching = true;
        }

        protected void stopDispatching()
        {
            currentPayload = null;
            IsDispatching = false;
        }

        public void WaitFor(List<IDispatchToken> dispatchTokenList)
        {
            if (!IsDispatching)
                throw new Exceptions.Dispatcher.NotDispatchingException();

            dispatchTokenList.ForEach((IDispatchToken dispatchToken) =>
            {
                if (isPending.Contains(dispatchToken)) {
                    if (isHandled.Contains(dispatchToken))
                        throw new Exceptions.Dispatcher.CircularDependecyException();

                    return;
                }

                if (!callbackRegistry.ContainsKey(dispatchToken))
                    throw new Exceptions.Dispatcher.InvalidCallbackException();

                invokeCallback(dispatchToken);
            });
        }
        public void WaitFor(IDispatchToken dispatchToken)
        {
            WaitFor(new List<IDispatchToken>() { dispatchToken });
        }

        protected void invokeCallback(IDispatchToken dispatchToken)
        {
            isPending.Add(dispatchToken);
            callbackRegistry[dispatchToken](currentPayload);
            isHandled.Add(dispatchToken);
        }
    }
}
