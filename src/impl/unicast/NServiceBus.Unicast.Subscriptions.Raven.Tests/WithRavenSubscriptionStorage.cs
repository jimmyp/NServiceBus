using System.IO;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;

namespace NServiceBus.Unicast.Subscriptions.Raven.Tests
{
    public class WithRavenSubscriptionStorage
    {
        protected ISubscriptionStorage storage;
        protected IDocumentStore store;
        string path;
        [TestFixtureSetUp]
        public void SetupContext()
        {
            path = Path.GetRandomFileName();
            store = new EmbeddableDocumentStore { RunInMemory = true };
            store.Initialize();

            storage = new RavenSubscriptionStorage { Store = store, Endpoint = "SubscriptionEndpoint"};
            storage.Init();
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
            store.Dispose();
        }
    }
}