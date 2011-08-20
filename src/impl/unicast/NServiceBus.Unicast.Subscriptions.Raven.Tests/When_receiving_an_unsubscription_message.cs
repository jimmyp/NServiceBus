using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NUnit.Framework;

namespace NServiceBus.Unicast.Subscriptions.Raven.Tests
{
    [TestFixture]
    public class When_receiving_an_unsubscription_message : WithRavenSubscriptionStorage
    {
        [Test]
        public void All_subscription_entries_for_specfied_message_types_should_be_removed()
        {
            var clientEndpoint = Address.Parse("TestEndpoint");

            var messageTypes = new List<string> {"MessageType1", "MessageType2"};

            storage.Subscribe(clientEndpoint, messageTypes);

            storage.Unsubscribe(clientEndpoint, messageTypes);

            using (var session = store.OpenSession())
            {
                var subscriptionCount = session
                    .Query<Subscription>()
                    .Customize(c => c.WaitForNonStaleResults())
                    .ToList()
                    .Count();


                Assert.AreEqual(0, subscriptionCount);
            }
        }
    }
}
