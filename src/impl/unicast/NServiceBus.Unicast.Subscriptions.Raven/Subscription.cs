namespace NServiceBus.Unicast.Subscriptions.Raven
{
    public class Subscription
    {
        public string Id { get; set; }

        public string MessageType { get; set; }

        public Address Client { get; set; }

        public static string FormatId(string endpoint, string messageType, Address client)
        {
            return string.Format("Subscriptions/{0}/{1}/{2}", endpoint, messageType, client);
        }
    }
}