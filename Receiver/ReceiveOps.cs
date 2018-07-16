using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Receiver
{
    public static class ReceiverOps
    {
        public static IDictionary<string, object> CopyHeaders(IBasicProperties originalProperties)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            IDictionary<string, object> headers = originalProperties.Headers;
            if (headers != null)
            {
                foreach (KeyValuePair<string, object> kvp in headers)
                {
                    dict[kvp.Key] = kvp.Value;
                }
            }

            return dict;
        }

        public static int GetRetryCount(IBasicProperties messageProperties, string countHeader)
        {
            IDictionary<string, object> headers = messageProperties.Headers;
            int count = 0;
            if (headers != null)
            {
                if (headers.ContainsKey(countHeader))
                {
                    string countAsString = Convert.ToString(headers[countHeader]);
                    count = Convert.ToInt32(countAsString);
                }
            }

            return count;
        }
    }
}