using System;

namespace Flux.Payloads
{
    public interface IPayload
    {
        string Action { get; }
        Type Type { get; }
        object Data { get; }
    }
}
