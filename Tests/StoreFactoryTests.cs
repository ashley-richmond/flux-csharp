using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flux.Stores;
using Flux.Payloads;

namespace Flux.Tests
{
    [TestClass]
    public class StoreFactoryTests
    {
        private class TestStoreA : Store
        {
            public override void ReceiveAction(IPayload payload)
            {
                Emit();
            }
        }

        private class TestStoreB : Store
        {
            public override void ReceiveAction(IPayload payload)
            {
                Emit();
            }
        }

        [TestMethod]
        public void StoreFactory_OnCreateUniqueStore_ReturnsAUniqueStore()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStoreA storeA = StoreFactory<TestStoreA>.GetUniqueStore(dispatcher);
            TestStoreA storeB = StoreFactory<TestStoreA>.GetUniqueStore(dispatcher);

            Assert.AreNotEqual(storeA.DispatchToken.Id, storeB.DispatchToken.Id);
        }

        [TestMethod]
        public void StoreFactory_OnGetGenericStoreOnce_ReturnsANewStore()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStoreA store = StoreFactory<TestStoreA>.GetStore(dispatcher);
            Assert.IsInstanceOfType(store, typeof(TestStoreA));
        }

        [TestMethod]
        public void StoreFactory_OnGetGenericStoreTwice_ReturnsTheSameStore()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStoreA storeA = StoreFactory<TestStoreA>.GetStore(dispatcher);
            TestStoreA storeB = StoreFactory<TestStoreA>.GetStore(dispatcher);

            Assert.AreEqual(storeA.DispatchToken.Id, storeB.DispatchToken.Id);
        }

        [TestMethod]
        public void StoreFactory_OnGetDifferentGenericStore_ReturnsANewStore()
        {
            Dispatcher dispatcher = new Dispatcher();
            TestStoreA storeA = StoreFactory<TestStoreA>.GetStore(dispatcher);
            TestStoreB storeB = StoreFactory<TestStoreB>.GetStore(dispatcher);

            Assert.AreNotEqual(storeA.DispatchToken.Id, storeB.DispatchToken.Id);
            Assert.IsNotInstanceOfType(storeB, storeA.GetType());
        }
    }
}
