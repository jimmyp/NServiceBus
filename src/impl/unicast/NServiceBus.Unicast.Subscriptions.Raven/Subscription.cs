using System.Security.Cryptography;
using System.Text;

namespace NServiceBus.Unicast.Subscriptions.Raven
{
    public class Subscription
    {
        public string Id { get; set; }

        public string MessageType { get; set; }

        public Address Client { get; set; }

        public static string FormatId(string endpoint, string messageType, Address client)
        {
            var hunanReadableId = string.Format("Subscriptions/{0}/{1}/{2}", endpoint, messageType, client);

            // First we need to convert the string into bytes, which
            // means using a text encoder.
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

            // Create a buffer large enough to hold the string
            byte[] unicodeText = new byte[hunanReadableId.Length * 2];
            enc.GetBytes(hunanReadableId.ToCharArray(), 0, hunanReadableId.Length, unicodeText, 0, true);

            // Now that we have a byte array we can ask the CSP to hash it
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            // Build the final string by converting each byte
            // into hex and appending it to a StringBuilder
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            // And return it
            return sb.ToString();
        }
    }
}