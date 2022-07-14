using System;
using System.Net;

namespace SteamAuth
{
    public class Proxy
    {
        public string ProxyHost { get; set; }

        public string ProxyPort { get; set; }

        public string ProxyUsername { get; set; }

        public string ProxyPassword { get; set; }


        public WebProxy BuildProxy()
        {
            if (ProxyHost == null)
                return null;

            var proxyURI = new Uri(string.Format("http://{0}:{1}", ProxyHost, ProxyPort));

            //Set credentials
            ICredentials credentials = new NetworkCredential(ProxyUsername, ProxyPassword);

            //Set proxy
            return new WebProxy(proxyURI, true, null, credentials);
        }
    }
}
