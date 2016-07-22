using Flux.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux.Payloads
{
    public class Payload : IPayload
    {
        public string Action { get; protected set; }
        protected object _data;

        public Payload(string action, object data) {
            Action = action;
            _data = data;
        }

        public T Data<T>()
        {
            return (T)_data;
        }
    }
}
