using Flux.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
