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
