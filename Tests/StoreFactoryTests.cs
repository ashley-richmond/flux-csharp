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
