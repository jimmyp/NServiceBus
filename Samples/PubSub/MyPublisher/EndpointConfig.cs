using MyMessages;
using NServiceBus;
using NServiceBus.Grid.MessageHandlers;
using NServiceBus.Sagas.Impl;

namespace MyPublisher
{
    class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher,
    IAmResponsibleForMessages<IEvent>, IAmResponsibleForMessages<EventMessage>{} IWantCustomInitialization
{
    public void Init()
    {
        NServiceBus.Configure.With()
            .CastleWindsorBuilder() // just to show we can mix and match containers
            .XmlSerializer()
            .UnicastBus()
            .DoNotAutoSubscribe(); //managed by the class Subscriber2Endpoint
    }
}
}
