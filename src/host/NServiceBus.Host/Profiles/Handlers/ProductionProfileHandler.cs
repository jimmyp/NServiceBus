﻿using NServiceBus.Hosting.Profiles;

namespace NServiceBus.Host.Profiles.Handlers
{
    internal class ProductionProfileHandler : IHandleProfile<Production>, IWantTheEndpointConfig
    {
        void IHandleProfile.ProfileActivated()
        {
            Configure.Instance
                .NHibernateSagaPersister();

            if (Config is AsA_Publisher)
                Configure.Instance.DBSubcriptionStorage();
        }

        public IConfigureThisEndpoint Config { get; set; }
    }
}