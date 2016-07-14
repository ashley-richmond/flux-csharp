using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Flux.Tests
{
    [TestClass]
    public class DispatchTokenTests
    {
        [TestMethod]
        public void DispatchToken_OnCreate_GeneratesAUniqueId ()
        {
            DispatchToken tokenA = new DispatchToken();
            DispatchToken tokenB = new DispatchToken();

            Assert.AreNotEqual(tokenA.Id, tokenB.Id);
        }
    }
}
