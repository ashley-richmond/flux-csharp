using System;

namespace Flux.Payloads
{
    public interface IPayload
    {
        string Action { get; }
        T Data<T>();
    }
}
