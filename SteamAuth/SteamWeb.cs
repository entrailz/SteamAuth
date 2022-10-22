using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SteamAuth
{
    public class SteamWeb
    {

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);

        /// <summary>
        /// Perform a mobile login request
        /// </summary>
        /// <param name="url">API url</param>
        /// <param name="method">GET or POST</param>
        /// <param name="data">Name-data pairs</param>
        /// <param name="cookies">current cookie container</param>
        /// <returns>response body</returns>
        public static string MobileLoginRequest(string url, string method, NameValueCollection data = null, CookieContainer cookies = null, NameValueCollection headers = null, WebProxy proxy = null)
        {
            return Request(url, method, data, cookies, headers, APIEndpoints.COMMUNITY_BASE + "/mobilelogin?oauth_client_id=DE45CD61&oauth_scope=read_profile%20write_profile%20read_client%20write_client", proxy);
        }

        public static string Request(string url, string method, NameValueCollection data = null, CookieContainer cookies = null)
        {
            string query = (data == null ? string.Empty : string.Join("&", Array.ConvertAll(data.AllKeys, key => String.Format("{0}={1}", WebUtility.UrlEncode(key), WebUtility.UrlEncode(data[key])))));
            if (method == "GET")
            {
                url += (url.Contains("?") ? "&" : "?") + query;
            }

            return Request(url, method, query, cookies, null, null);
        }

        public static string Request(string url, string method, NameValueCollection data = null, CookieContainer cookies = null, WebProxy proxy = null)
        {
            string query = (data == null ? string.Empty : string.Join("&", Array.ConvertAll(data.AllKeys, key => String.Format("{0}={1}", WebUtility.UrlEncode(key), WebUtility.UrlEncode(data[key])))));
            if (method == "GET")
            {
                url += (url.Contains("?") ? "&" : "?") + query;
            }

            return Request(url, method, query, cookies, null, null, proxy);
        }

        public static string Request(string url, string method, string dataString = null, CookieContainer cookies = null, WebProxy proxy = null)
        {
            return Request(url, method, dataString, cookies, null, null, proxy);
        }

        public static string Request(string url, string method, NameValueCollection data = null, CookieContainer cookies = null, NameValueCollection headers = null, string referer = APIEndpoints.COMMUNITY_BASE, WebProxy proxy = null)
        {
            string query = (data == null ? string.Empty : string.Join("&", Array.ConvertAll(data.AllKeys, key => String.Format("{0}={1}", WebUtility.UrlEncode(key), WebUtility.UrlEncode(data[key])))));
            if (method == "GET")
            {
                url += (url.Contains("?") ? "&" : "?") + query;
            }

            return Request(url, method, query, cookies, headers, referer, proxy);
        }

        public static string Request(string url, string method, string dataString = null, CookieContainer cookies = null, NameValueCollection headers = null, string referer = APIEndpoints.COMMUNITY_BASE, WebProxy proxy = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Proxy = proxy;
            request.Method = method;
            request.Accept = "text/javascript, text/html, application/xml, text/xml, */*";
            request.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.1.1; en-us; Google Nexus 4 - 4.1.1 - API 16 - 768x1280 Build/JRO03S) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Referer = referer;

            if (headers != null)
            {
                request.Headers.Add(headers);
            }

            if (cookies != null)
            {
                request.CookieContainer = cookies;
            }

            if (method == "POST")
            {
                try
                {
                    request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    request.ContentLength = dataString.Length;

                    StreamWriter requestStream = new StreamWriter(request.GetRequestStream());
                    requestStream.Write(dataString);
                    requestStream.Close();
                }
                catch (WebException e)
                {
                    HandleFailedWebRequestResponse(e.Response as HttpWebResponse, url, e);
                    return null;
                }
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        HandleFailedWebRequestResponse(response, url);
                        return null;
                    }

                    using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
                    {
                        string responseData = responseStream.ReadToEnd();
                        return responseData;
                    }
                }
            }
            catch (WebException e)
            {
                HandleFailedWebRequestResponse(e.Response as HttpWebResponse, url, e);
                return null;
            }
        }

        public static async Task<string> RequestAsync(string url, string method, NameValueCollection data = null, CookieContainer cookies = null, NameValueCollection headers = null, string referer = APIEndpoints.COMMUNITY_BASE, WebProxy proxy = null)
        {
            string query = (data == null ? string.Empty : string.Join("&", Array.ConvertAll(data.AllKeys, key => String.Format("{0}={1}", WebUtility.UrlEncode(key), WebUtility.UrlEncode(data[key])))));
            if (method == "GET")
            {
                url += (url.Contains("?") ? "&" : "?") + query;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Proxy = proxy;
            request.Method = method;
            request.Accept = "text/javascript, text/html, application/xml, text/xml, */*";
            request.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.1.1; en-us; Google Nexus 4 - 4.1.1 - API 16 - 768x1280 Build/JRO03S) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Referer = referer;

            if (headers != null)
            {
                request.Headers.Add(headers);
            }

            if (cookies != null)
            {
                request.CookieContainer = cookies;
            }

            if (method == "POST")
            {
                try
                {
                    request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    request.ContentLength = query.Length;
                    StreamWriter requestStream = new StreamWriter(request.GetRequestStream());
                    await requestStream.WriteAsync(query);
                    requestStream.Close();
                }
                catch (WebException e)
                {
                    HandleFailedWebRequestResponse(e.Response as HttpWebResponse, url, e);
                    return null;
                }

            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    HandleFailedWebRequestResponse(response, url);
                    return null;
                }

                using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
                {
                    string responseData = await responseStream.ReadToEndAsync();
                    return responseData;
                }
            }
            catch (WebException e)
            {
                HandleFailedWebRequestResponse(e.Response as HttpWebResponse, url, e);
                return null;
            }
        }

        /// <summary>
        /// Raise exceptions relevant to this HttpWebResponse -- EG, to signal that our oauth token has expired.
        /// </summary>
        private static void HandleFailedWebRequestResponse(HttpWebResponse response, string requestURL, WebException e = null)
        {
            if (response == null && e == null) return;
            switch (e.Message.ToString())
            {
                case "Unable to connect to the remote server":
                    throw new SteamGuardAccount.ProxyConnectionException("Proxy error - please check proxy.");
                case "The remote server returned an error: (407) Proxy Authentication Required.":
                    throw new SteamGuardAccount.ProxyConnectionException("Proxy error - Authentication error.");
                case "Cannot handle redirect from HTTP/HTTPS protocols to other dissimilar ones.": //Session is probably expired... lets throw token exception.
                    throw new SteamGuardAccount.WGTokenExpiredException();
                case "The remote server returned an error: (401) Unauthorized.":
                    throw new SteamGuardAccount.WGTokenInvalidException();
                default: //Unhandled, but show the full error to the end user. 
                    throw new SteamGuardAccount.ProxyConnectionException(e.Message);
            }
            //if (response == null) return;

            ////Redirecting -- likely to a steammobile:// URI
            //if (response.StatusCode == HttpStatusCode.Found)
            //{
            //    var location = response.Headers.Get("Location");
            //    if (!string.IsNullOrEmpty(location))
            //    {
            //        //Our OAuth token has expired. This is given both when we must refresh our session, or the entire OAuth Token cannot be refreshed anymore.
            //        //Thus, we should only throw this exception when we're attempting to refresh our session.
            //        if (location == "steammobile://lostauth" && requestURL == APIEndpoints.MOBILEAUTH_GETWGTOKEN)
            //        {
            //            throw new SteamGuardAccount.WGTokenExpiredException();
            //        }
            //    }
            //}
        }
    }
}
