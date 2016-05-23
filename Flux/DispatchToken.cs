using Flux;
using System;

namespace Flux
{
    public sealed class DispatchToken
    {
        public Guid Id { get; private set; }

        public DispatchToken()
        {
            Id = Guid.NewGuid();
        }
    }
}
