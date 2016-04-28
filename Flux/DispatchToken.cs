using Flux;

namespace Flux
{
    public sealed class DispatchToken
    {
        public int Id { get; private set; }

        public DispatchToken(int id)
        {
            Id = id;
        }
    }
}
