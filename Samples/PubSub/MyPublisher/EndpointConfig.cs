﻿using MyMessages;
using NServiceBus;
using NServiceBus.Unicast.Subscriptions.Raven.Config;

namespace MyPublisher
{
    class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher,
    IAmResponsibleForMessages<IEvent>, IAmResponsibleForMessages<EventMessage>{


        
    }

    public class Busconfig: IWantCustomInitialization
    {
        public void Init()
        {
            NServiceBus.Configure.With()
                .RavenSubscriptionStorage("Server")
                .XmlSerializer()
                .UnicastBus();
        }
    }
}
