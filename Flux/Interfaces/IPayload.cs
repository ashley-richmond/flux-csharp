using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Interfaces
{
    public interface IPayload<T>
    {
        string Type { get; }
        T Data { get; }
    }
}
