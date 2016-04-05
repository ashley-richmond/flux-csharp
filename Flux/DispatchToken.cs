using Flux.Interfaces;

namespace Flux
{
    class DispatchToken : IDispatchToken
    {
        public int Id { get; protected set; }

        public DispatchToken(int id)
        {
            Id = id;
        }
    }
}
