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
        public object Data { get; protected set; }
        public Type Type { get; protected set; }

        public Payload(string action, object data) {
            Action = action;
            Data = data;
            Type = data.GetType();
        }
    }
}
