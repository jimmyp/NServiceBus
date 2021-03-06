using System;
using NServiceBus.Unicast.Subscriptions.Raven.Config;
using NUnit.Framework;

namespace NServiceBus.Unicast.Subscriptions.Raven.Tests.Config
{
    [TestFixture]
    public class When_configuring_the_raven_subscription_storage_with_a_connection_string_that_does_not_exist
    {
        [Test]
        public void It_should_throw_an_exception()
        {
            var cfg = Configure.With(new[] { GetType().Assembly })
                .DefaultBuilder();

            Assert.Throws<ArgumentException>(() => cfg.RavenSubscriptionStorage("ConnectionStringDoesNotExist"));
        }
    }
}