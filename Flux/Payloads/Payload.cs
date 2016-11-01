#region copyright
// Copyright 2016 Ashley Richmond
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

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

        public T Data<T>(T defaultValue)
        {
            if (_data == null)
                return defaultValue;

            return (T)_data;
        }
    }
}
