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

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flux.Payloads;

namespace Flux.Tests
{
    [TestClass]
    public class PayloadTests
    {
        [TestMethod]
        public void Payload_OnGetData_ReturnsCorrectValue()
        {
            IPayload payload = new Payload("test", 1);
            Assert.AreEqual(1, payload.Data<int>());
        }

        [TestMethod]
        public void Payload_OnGetMissingData_ReturnsDefaultValue()
        {
            IPayload payload = new Payload("test", null);
            Assert.AreEqual(2, payload.Data<int>(2));
        }

        [TestMethod]
        public void Payload_OnGetDataWithDefault_ReturnsOriginalValue()
        {
            IPayload payload = new Payload("test", 1);
            Assert.AreEqual(1, payload.Data<int>(2));
        }
    }
}
