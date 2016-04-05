namespace Flux.Interfaces
{
    public interface IPayload<T>
    {
        string Type { get; }
        T Data { get; }
    }
}
