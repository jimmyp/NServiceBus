using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus.Unicast.Subscriptions.Raven.Indexes;
using Raven.Abstractions.Exceptions;
using Raven.Client;

namespace NServiceBus.Unicast.Subscriptions.Raven
{
    public class RavenSubscriptionStorage : ISubscriptionStorage
    {
        public IDocumentStore Store { get; set; }

        public string Endpoint { get; set; }
        
        public void Init()
        {
            new SubscriptionsByMessageType().Execute(Store);
        }

        public void Subscribe(string client, IEnumerable<string> messageTypes)
        {
            Subscribe(Address.Parse(client), messageTypes);
        }

        public void Subscribe(Address client, IEnumerable<string> messageTypes)
        {
            var subscriptions = messageTypes.Select(m => new Subscription
            {
                Id = Subscription.FormatId(Endpoint, m, client),
                MessageType = m,
                Client = client
            }).ToList();

            try
            {
                using (var session = Store.OpenSession())
                {
                    session.Advanced.UseOptimisticConcurrency = true;
                    subscriptions.ForEach(session.Store);
                    session.SaveChanges();
                }
            }
            catch (ConcurrencyException ex)
            {

            }
        }

        public void Unsubscribe(string client, IEnumerable<string> messageTypes)
        {
            Unsubscribe(Address.Parse(client), messageTypes);
        }

        public void Unsubscribe(Address client, IEnumerable<string> messageTypes)
        {
            var ids = messageTypes
                .Select(m => Subscription.FormatId(Endpoint, m, client))
                .ToList();

            using (var session = Store.OpenSession())
            {
                ids.ForEach(id => session.Advanced.DatabaseCommands.Delete(id, null));

                session.SaveChanges();
            }
        }
        
        private IEnumerable<Subscription> GetSubscribersForMessageType(string messageType, IDocumentSession session)
        {
            var clients = session.Query<Subscription, SubscriptionsByMessageType>()
                .Customize(c => c.WaitForNonStaleResults())
                .Where(s => s.MessageType == messageType);

            return clients;
        }

        private IEnumerable<Subscription> GetAllSubscribersToMessageTypes(IEnumerable<string> messageTypes, IDocumentSession session)
        {
            return messageTypes.SelectMany(m => GetSubscribersForMessageType(m, session));
        }

        public IEnumerable<string> GetSubscribersForMessage(IEnumerable<string> messageTypes)
        {
            using (var session = Store.OpenSession())
            {
                return messageTypes.SelectMany(m => GetSubscribersForMessageType(m, session))
                    .Select(s => s.Client.ToString())
                    .ToList()
                    .Distinct();
            }
        }
        
        public IEnumerable<Address> GetSubscriberAddressesForMessage(IEnumerable<string> messageTypes)
        {
            using (var session = Store.OpenSession())
            {
                return GetAllSubscribersToMessageTypes(messageTypes, session)
                   .Select(s => s.Client)
                   .ToList()
                   .Distinct();
            }
        }
    }
}
