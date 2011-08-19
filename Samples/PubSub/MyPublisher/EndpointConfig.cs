using MyMessages;
using NServiceBus;
using NServiceBus.Grid.MessageHandlers;
using NServiceBus.Sagas.Impl;
using NServiceBus.Unicast.Subscriptions.Raven.Config;

namespace MyPublisher
{
    class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization,
    IAmResponsibleForMessages<IEvent>, IAmResponsibleForMessages<EventMessage>{


        public void Init()
        {
            NServiceBus.Configure.With()
                .EmbeddedRavenSubscriptionStorage()
                .XmlSerializer()
                .UnicastBus()
                .DoNotAutoSubscribe(); //managed by the class Subscriber2Endpoint
        }
    }
}
