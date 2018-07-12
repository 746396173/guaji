namespace IntelligentPlanning
{
    using MSXML2;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Cache;
    using System.Net.Security;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    public class HttpHelper
    {
        public static string BetsTokenKey = "";
        public static string BetsTokenValue = "";
        public static CookieContainer CookieContainers = new CookieContainer();
        public static string IE7 = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; InfoPath.2; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET4.0C; .NET4.0E)";
        public static string IE8 = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0)";
        public static bool isUserAgentSet = false;
        private static RequestCachePolicy rcp = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
        public static string WebCookie = "";

        public static void ConvertCookie(string pLine1, string pLine2)
        {
            Uri pUrl = new Uri(pLine1);
            string httpHelperCookieString = GetHttpHelperCookieString(pUrl, null);
            GetAllCookies(CookieContainers);
            SaveCookies(httpHelperCookieString, pLine2);
        }

        [DllImport("wininet.dll", CharSet=CharSet.Auto)]
        public static extern bool DeleteUrlCacheEntry(string lpszUrlName);
        public static List<Cookie> GetAllCookies(CookieContainer pCookieContainers)
        {
            List<Cookie> list = new List<Cookie>();
            Hashtable hashtable = (Hashtable) pCookieContainers.GetType().InvokeMember("m_domainTable", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, pCookieContainers, new object[0]);
            foreach (object obj2 in hashtable.Values)
            {
                SortedList list2 = (SortedList) obj2.GetType().InvokeMember("m_list", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, obj2, new object[0]);
                foreach (CookieCollection cookies in list2.Values)
                {
                    foreach (Cookie cookie in cookies)
                    {
                        list.Add(cookie);
                    }
                }
            }
            return list;
        }

        public static string GetCookieInternal(Uri pUrl)
        {
            string str = "";
            uint pchCookieData = 0;
            string url = pUrl.ToString();
            uint flags = 0x2000;
            if (InternetGetCookieEx(url, null, null, ref pchCookieData, flags, IntPtr.Zero))
            {
                pchCookieData++;
                StringBuilder cookieData = new StringBuilder((int) pchCookieData);
                if (InternetGetCookieEx(url, null, cookieData, ref pchCookieData, flags, IntPtr.Zero))
                {
                    str = cookieData.ToString();
                }
            }
            return str;
        }

        public static Dictionary<string, string> GetHttpHelperCookie(Uri pUrl)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            CookieCollection cookies = CookieContainers.GetCookies(pUrl);
            foreach (Cookie cookie in cookies)
            {
                string name = cookie.Name;
                string str2 = cookie.Value;
                dictionary[name] = str2;
            }
            return dictionary;
        }

        public static string GetHttpHelperCookie(Uri pUrl, string pKey) => 
            GetHttpHelperCookie(pUrl)[pKey];

        public static string GetHttpHelperCookieString(Uri pUrl, CookieCollection pCookie = null)
        {
            List<string> pList = new List<string>();
            if (pCookie == null)
            {
                pCookie = CookieContainers.GetCookies(pUrl);
            }
            foreach (Cookie cookie in pCookie)
            {
                pList.Add(cookie.Name + "=" + cookie.Value);
            }
            return CommFunc.Join(pList, "; ");
        }

        public static HttpStatusCode GetResponse(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x2710, string pEncoding = "UTF-8", bool pIsPTHander = true)
        {
            try
            {
                DeleteUrlCacheEntry(pUrl);
                HttpWebRequest pRequest = (HttpWebRequest) WebRequest.Create(pUrl);
                pRequest.KeepAlive = true;
                pRequest.Method = pMethod.ToUpper();
                pRequest.AllowAutoRedirect = true;
                pRequest.CookieContainer = CookieContainers;
                pRequest.Referer = pReferer;
                pRequest.UserAgent = IE8;
                pRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                pRequest.Timeout = pTime;
                pRequest.ReadWriteTimeout = pTime;
                pRequest.CachePolicy = rcp;
                pRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
                if (pIsPTHander)
                {
                    PTRequest(ref pRequest);
                }
                Encoding encoding = Encoding.GetEncoding(pEncoding);
                if ((pMethod.ToUpper() != "GET") && (pData != null))
                {
                    byte[] bytes = encoding.GetBytes(pData);
                    pRequest.ContentLength = bytes.Length;
                    pRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    Stream requestStream = pRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse) pRequest.GetResponse();
                pResponsetext = new StreamReader(response.GetResponseStream(), encoding).ReadToEnd();
                if (pUrl.Contains("TUHAOPLUS.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "FASHAO");
                }
                else if (pUrl.Contains("CSC.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "CSC");
                }
                pRequest.Abort();
                response.Close();
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ExpectationFailed;
            }
        }

        public static HttpStatusCode GetResponse1(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x2710, string pEncoding = "UTF-8", bool pIsPTHander = true)
        {
            try
            {
                DeleteUrlCacheEntry(pUrl);
                HttpWebRequest pRequest = (HttpWebRequest) WebRequest.Create(pUrl);
                pRequest.KeepAlive = true;
                pRequest.Method = pMethod.ToUpper();
                pRequest.AllowAutoRedirect = true;
                pRequest.CookieContainer = CookieContainers;
                pRequest.Referer = pReferer;
                pRequest.UserAgent = IE8;
                pRequest.Accept = "application/json, text/javascript, */*; q=0.01";
                pRequest.Timeout = pTime;
                pRequest.ReadWriteTimeout = pTime;
                pRequest.CachePolicy = rcp;
                pRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
                if (pIsPTHander)
                {
                    PTRequest(ref pRequest);
                }
                Encoding encoding = Encoding.GetEncoding(pEncoding);
                if ((pMethod.ToUpper() == "POST") && (pData != null))
                {
                    byte[] bytes = encoding.GetBytes(pData);
                    pRequest.ContentLength = bytes.Length;
                    pRequest.ContentType = "application/json; charset=UTF-8";
                    Stream requestStream = pRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = null;
                try
                {
                    response = (HttpWebResponse) pRequest.GetResponse();
                }
                catch (WebException exception)
                {
                    response = (HttpWebResponse) exception.Response;
                }
                pResponsetext = new StreamReader(response.GetResponseStream(), encoding).ReadToEnd();
                pRequest.Abort();
                response.Close();
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ExpectationFailed;
            }
        }

        public static HttpStatusCode GetResponse10(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x2710, string pEncoding = "UTF-8", bool pIsPTHander = true)
        {
            try
            {
                DeleteUrlCacheEntry(pUrl);
                HttpWebRequest pRequest = (HttpWebRequest) WebRequest.Create(pUrl);
                pRequest.KeepAlive = true;
                pRequest.Method = pMethod.ToUpper();
                pRequest.AllowAutoRedirect = true;
                pRequest.CookieContainer = CookieContainers;
                pRequest.Referer = pReferer;
                pRequest.UserAgent = IE8;
                pRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                pRequest.Timeout = pTime;
                pRequest.ReadWriteTimeout = pTime;
                pRequest.CachePolicy = rcp;
                pRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
                if (pIsPTHander)
                {
                    PTRequest(ref pRequest);
                }
                Encoding encoding = Encoding.GetEncoding(pEncoding);
                if ((pMethod.ToUpper() != "GET") && (pData != null))
                {
                    byte[] bytes = encoding.GetBytes(pData);
                    pRequest.ContentLength = bytes.Length;
                    pRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    Stream requestStream = pRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse) pRequest.GetResponse();
                pResponsetext = new StreamReader(response.GetResponseStream(), encoding).ReadToEnd();
                if (pUrl.Contains("TUHAOPLUS.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "FASHAO");
                }
                else if (pUrl.Contains("CSC.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "CSC");
                }
                pRequest.Abort();
                response.Close();
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ExpectationFailed;
            }
        }

        public static HttpStatusCode GetResponse2(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x2710, string pEncoding = "UTF-8", bool pIsPTHander = true)
        {
            try
            {
                DeleteUrlCacheEntry(pUrl);
                Uri requestUri = new Uri(pUrl, true);
                HttpWebRequest pRequest = (HttpWebRequest) WebRequest.Create(requestUri);
                pRequest.KeepAlive = true;
                pRequest.Method = pMethod.ToUpper();
                pRequest.AllowAutoRedirect = true;
                pRequest.CookieContainer = CookieContainers;
                pRequest.Referer = pReferer;
                pRequest.UserAgent = IE8;
                pRequest.Accept = "application/json, text/javascript, */*; q=0.01";
                pRequest.Timeout = pTime;
                pRequest.ReadWriteTimeout = pTime;
                pRequest.CachePolicy = rcp;
                pRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
                if (pIsPTHander)
                {
                    PTRequest(ref pRequest);
                }
                Encoding encoding = Encoding.GetEncoding(pEncoding);
                if ((pMethod.ToUpper() == "POST") && (pData != null))
                {
                    byte[] bytes = encoding.GetBytes(pData);
                    pRequest.ContentLength = bytes.Length;
                    pRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    Stream requestStream = pRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse) pRequest.GetResponse();
                pResponsetext = new StreamReader(response.GetResponseStream(), encoding).ReadToEnd();
                pRequest.Abort();
                response.Close();
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ExpectationFailed;
            }
        }

        public static HttpStatusCode GetResponse3(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x2710, string pEncoding = "UTF-8", bool pIsPTHander = true)
        {
            try
            {
                DeleteUrlCacheEntry(pUrl);
                HttpWebRequest pRequest = (HttpWebRequest) WebRequest.Create(pUrl);
                pRequest.KeepAlive = true;
                pRequest.Method = pMethod.ToUpper();
                pRequest.AllowAutoRedirect = true;
                pRequest.CookieContainer = CookieContainers;
                pRequest.Referer = pReferer;
                pRequest.UserAgent = IE7;
                pRequest.Accept = "gzip, deflate, sdch";
                pRequest.AutomaticDecompression = DecompressionMethods.GZip;
                pRequest.Timeout = pTime;
                pRequest.ReadWriteTimeout = pTime;
                pRequest.CachePolicy = rcp;
                pRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
                if (pIsPTHander)
                {
                    PTRequest(ref pRequest);
                }
                Encoding encoding = Encoding.GetEncoding(pEncoding);
                if ((pMethod.ToUpper() == "POST") && (pData != null))
                {
                    byte[] bytes = encoding.GetBytes(pData);
                    pRequest.ContentLength = bytes.Length;
                    pRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    Stream requestStream = pRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse) pRequest.GetResponse();
                pResponsetext = new StreamReader(response.GetResponseStream(), encoding).ReadToEnd();
                if (pUrl.Contains("TUHAOPLUS.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "FASHAO");
                }
                else if (pUrl.Contains("CSC.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "CSC");
                }
                pRequest.Abort();
                response.Close();
                return response.StatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.ExpectationFailed;
            }
        }

        public static HttpStatusCode GetResponse4(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x2710, string pEncoding = "UTF-8", bool pIsPTHander = true)
        {
            try
            {
                DeleteUrlCacheEntry(pUrl);
                HttpWebRequest pRequest = (HttpWebRequest) WebRequest.Create(pUrl);
                pRequest.KeepAlive = true;
                pRequest.Method = pMethod.ToUpper();
                pRequest.AllowAutoRedirect = true;
                pRequest.CookieContainer = CookieContainers;
                pRequest.Referer = pReferer;
                pRequest.UserAgent = IE8;
                pRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                pRequest.Timeout = pTime;
                pRequest.ReadWriteTimeout = pTime;
                pRequest.CachePolicy = rcp;
                if (pIsPTHander)
                {
                    PTRequest(ref pRequest);
                }
                Encoding encoding = Encoding.GetEncoding(pEncoding);
                if ((pMethod.ToUpper() != "GET") && (pData != null))
                {
                    byte[] bytes = encoding.GetBytes(pData);
                    pRequest.ContentLength = bytes.Length;
                    pRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    Stream requestStream = pRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse) pRequest.GetResponse();
                pResponsetext = new StreamReader(response.GetResponseStream(), encoding).ReadToEnd();
                WebCookie = response.Headers["Set-Cookie"];
                if (pUrl.Contains("TUHAOPLUS.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "FASHAO");
                }
                else if (pUrl.Contains("CSC.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "CSC");
                }
                pRequest.Abort();
                response.Close();
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ExpectationFailed;
            }
        }

        public static HttpStatusCode GetResponse5(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x2710, string pEncoding = "UTF-8", bool pIsPTHander = true)
        {
            try
            {
                DeleteUrlCacheEntry(pUrl);
                HttpWebRequest pRequest = (HttpWebRequest) WebRequest.Create(pUrl);
                pRequest.KeepAlive = true;
                pRequest.Method = pMethod.ToUpper();
                pRequest.AllowAutoRedirect = true;
                pRequest.CookieContainer = CookieContainers;
                pRequest.Referer = pReferer;
                pRequest.UserAgent = IE8;
                pRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                pRequest.Timeout = pTime;
                pRequest.ReadWriteTimeout = pTime;
                pRequest.CachePolicy = rcp;
                if (pIsPTHander)
                {
                    PTRequest(ref pRequest);
                }
                Encoding encoding = Encoding.GetEncoding(pEncoding);
                if ((pMethod.ToUpper() != "GET") && (pData != null))
                {
                    byte[] bytes = encoding.GetBytes(pData);
                    pRequest.ContentLength = bytes.Length;
                    pRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    Stream requestStream = pRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse) pRequest.GetResponse();
                pResponsetext = new StreamReader(response.GetResponseStream(), encoding).ReadToEnd();
                if (pUrl.Contains("TUHAOPLUS.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "FASHAO");
                }
                else if (pUrl.Contains("CSC.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "CSC");
                }
                pRequest.Abort();
                response.Close();
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ExpectationFailed;
            }
        }

        public static HttpStatusCode GetResponse6(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x2710, string pEncoding = "UTF-8", bool pIsPTHander = true)
        {
            try
            {
                DeleteUrlCacheEntry(pUrl);
                HttpWebRequest pRequest = (HttpWebRequest) WebRequest.Create(pUrl);
                pRequest.KeepAlive = true;
                pRequest.Method = pMethod.ToUpper();
                pRequest.AllowAutoRedirect = false;
                pRequest.CookieContainer = CookieContainers;
                pRequest.Referer = pReferer;
                pRequest.UserAgent = IE8;
                pRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                pRequest.Timeout = pTime;
                pRequest.ReadWriteTimeout = pTime;
                pRequest.CachePolicy = rcp;
                pRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
                if (pIsPTHander)
                {
                    PTRequest(ref pRequest);
                }
                Encoding encoding = Encoding.GetEncoding(pEncoding);
                if ((pMethod.ToUpper() != "GET") && (pData != null))
                {
                    byte[] bytes = encoding.GetBytes(pData);
                    pRequest.ContentLength = bytes.Length;
                    pRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    Stream requestStream = pRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse) pRequest.GetResponse();
                pResponsetext = new StreamReader(response.GetResponseStream(), encoding).ReadToEnd();
                if (pUrl.Contains("TUHAOPLUS.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "FASHAO");
                }
                else if (pUrl.Contains("CSC.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "CSC");
                }
                pRequest.Abort();
                response.Close();
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ExpectationFailed;
            }
        }

        public static HttpStatusCode GetResponse7(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x2710, string pEncoding = "UTF-8", bool pIsPTHander = true)
        {
            try
            {
                DeleteUrlCacheEntry(pUrl);
                HttpWebRequest pRequest = (HttpWebRequest) WebRequest.Create(pUrl);
                pRequest.KeepAlive = true;
                pRequest.Method = pMethod.ToUpper();
                pRequest.AllowAutoRedirect = true;
                pRequest.CookieContainer = CookieContainers;
                pRequest.Referer = pReferer;
                pRequest.UserAgent = IE8;
                pRequest.Accept = "gzip, deflate, sdch";
                pRequest.AutomaticDecompression = DecompressionMethods.GZip;
                pRequest.Timeout = pTime;
                pRequest.ReadWriteTimeout = pTime;
                pRequest.CachePolicy = rcp;
                pRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
                if (pIsPTHander)
                {
                    PTRequest(ref pRequest);
                }
                Encoding encoding = Encoding.GetEncoding(pEncoding);
                if ((pMethod.ToUpper() != "GET") && (pData != null))
                {
                    byte[] bytes = encoding.GetBytes(pData);
                    pRequest.ContentLength = bytes.Length;
                    pRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    Stream requestStream = pRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse) pRequest.GetResponse();
                pResponsetext = new StreamReader(response.GetResponseStream(), encoding).ReadToEnd();
                if (pUrl.Contains("TUHAOPLUS.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "FASHAO");
                }
                else if (pUrl.Contains("CSC.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "CSC");
                }
                pRequest.Abort();
                response.Close();
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ExpectationFailed;
            }
        }

        public static HttpStatusCode GetResponse8(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x2710, string pEncoding = "UTF-8", bool pIsPTHander = true)
        {
            try
            {
                DeleteUrlCacheEntry(pUrl);
                HttpWebRequest pRequest = (HttpWebRequest) WebRequest.Create(pUrl);
                pRequest.KeepAlive = true;
                pRequest.Method = pMethod.ToUpper();
                pRequest.AllowAutoRedirect = true;
                pRequest.CookieContainer = CookieContainers;
                pRequest.Referer = pReferer;
                pRequest.UserAgent = IE8;
                pRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                pRequest.Timeout = pTime;
                pRequest.ReadWriteTimeout = pTime;
                pRequest.CachePolicy = rcp;
                pRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
                if (pIsPTHander)
                {
                    PTRequest(ref pRequest);
                }
                Encoding encoding = Encoding.GetEncoding(pEncoding);
                if ((pMethod.ToUpper() != "GET") && (pData != null))
                {
                    byte[] bytes = encoding.GetBytes(pData);
                    pRequest.ContentLength = bytes.Length;
                    pRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    Stream requestStream = pRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = null;
                try
                {
                    response = (HttpWebResponse) pRequest.GetResponse();
                }
                catch (WebException exception)
                {
                    response = (HttpWebResponse) exception.Response;
                }
                pResponsetext = new StreamReader(response.GetResponseStream(), encoding).ReadToEnd();
                if (pUrl.Contains("TUHAOPLUS.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "FASHAO");
                }
                else if (pUrl.Contains("CSC.aspx"))
                {
                    pResponsetext = CommFunc.GetDecodeResponse(pResponsetext, "CSC");
                }
                pRequest.Abort();
                response.Close();
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ExpectationFailed;
            }
        }

        public static Stream GetResponseImage(string pUrl, string pReferer = "", string pMethod = "GET", string pData = "", int pTime = 0x1770, string pEncoding = "UTF-8", bool pIsPTHander = true)
        {
            Stream result;
            try
            {
                HttpHelper.DeleteUrlCacheEntry(pUrl);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(pUrl);
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Method = pMethod;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.CookieContainer = HttpHelper.CookieContainers;
                httpWebRequest.Referer = pReferer;
                httpWebRequest.UserAgent = HttpHelper.IE8;
                httpWebRequest.Accept = "*/*";
                httpWebRequest.Timeout = pTime;
                httpWebRequest.CachePolicy = HttpHelper.rcp;
                httpWebRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
                if (pIsPTHander)
                {
                    HttpHelper.PTRequest(ref httpWebRequest);
                }
                Encoding encoding = Encoding.GetEncoding(pEncoding);
                if (pMethod.ToUpper() == "POST" && pData != null)
                {
                    byte[] bytes = encoding.GetBytes(pData);
                    httpWebRequest.ContentLength = (long)bytes.Length;
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    Stream requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback((object se, X509Certificate cert, X509Chain chain, SslPolicyErrors sslerror) => true));
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                result = httpWebResponse.GetResponseStream();
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static string GetWebData(string pText, string path = "")
        {
            if (path == "")
            {
                path = AppInfo.cServerGGUrl;
            }
            string cServerUrl = AppInfo.cServerUrl;
            string pUrl = string.Format($"{path}/{pText}.txt", new object[0]);
            string pResponsetext = "";
            GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, cServerUrl, 0x4e20, "UTF-8", true);
            return pResponsetext;
        }

        public static void GetXMLResponse(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x1770, string pCookie = "")
        {
            try
            {
                ServerXMLHTTP60 serverXMLHTTP = new ServerXMLHTTP60();
                serverXMLHTTP.setTimeouts(10000, 10000, 10000, pTime);
                if (pMethod.ToUpper() == "POST" && pData != null)
                {
                    serverXMLHTTP.open("POST", pUrl, false, null, null);
                    serverXMLHTTP.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    serverXMLHTTP.setRequestHeader("X-Requested-With", "XMLHttpRequest");
                    if (pReferer != "")
                    {
                        serverXMLHTTP.setRequestHeader("Referer", pReferer);
                    }
                    if (pCookie != "")
                    {
                        serverXMLHTTP.setRequestHeader("Cookie", pCookie);
                    }
                    serverXMLHTTP.send(pData);
                }
                else
                {
                    serverXMLHTTP.open("Get", pUrl, false, null, null);
                    if (pReferer != "")
                    {
                        serverXMLHTTP.setRequestHeader("Referer", pReferer);
                    }
                    if (pCookie != "")
                    {
                        serverXMLHTTP.setRequestHeader("Cookie", pCookie);
                    }
                    serverXMLHTTP.send(Missing.Value);
                }
                pResponsetext = serverXMLHTTP.responseText;
            }
            catch
            {
            }
        }

        public static void GetXMLResponse1(ref string pResponsetext, string pUrl, string pMethod, string pData, string pReferer, int pTime = 0x1770, string pCookie = "")
        {
            try
            {
                XMLHTTP60Class xmlhttp = new XMLHTTP60Class();
                if (pMethod.ToUpper() == "POST" && pData != null)
                {
                    xmlhttp.open("POST", pUrl, false, null, null);
                    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    if (pReferer != "")
                    {
                        xmlhttp.setRequestHeader("Referer", pReferer);
                    }
                    if (pCookie != "")
                    {
                        xmlhttp.setRequestHeader("Cookie", pCookie);
                    }
                    xmlhttp.send(pData);
                }
                else
                {
                    xmlhttp.open("Get", pUrl, false, null, null);
                    if (pReferer != "")
                    {
                        xmlhttp.setRequestHeader("Referer", pReferer);
                    }
                    if (pCookie != "")
                    {
                        xmlhttp.setRequestHeader("Cookie", pCookie);
                    }
                    xmlhttp.send(Missing.Value);
                }
                pResponsetext = xmlhttp.responseText;
            }
            catch
            {
            }
        }

        [SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("wininet.dll", EntryPoint="InternetGetCookieExW", CharSet=CharSet.Unicode, SetLastError=true, ExactSpelling=true)]
        internal static extern bool InternetGetCookieEx([In] string Url, [In] string cookieName, [Out] StringBuilder cookieData, [In, Out] ref uint pchCookieData, uint flags, IntPtr reserved);
        public static void PTRequest(ref HttpWebRequest pRequest)
        {
            if (((((AppInfo.PTInfo == AppInfo.WDYLInfo) || (AppInfo.PTInfo == AppInfo.JCXInfo)) || ((AppInfo.PTInfo == AppInfo.WCYLInfo) || (AppInfo.PTInfo == AppInfo.TBYLInfo))) || (AppInfo.PTInfo == AppInfo.HUAYInfo)) || (AppInfo.PTInfo == AppInfo.YXZXInfo))
            {
                if (BetsTokenKey != "")
                {
                    pRequest.Headers[BetsTokenKey] = BetsTokenValue;
                }
            }
            else if (AppInfo.PTInfo == AppInfo.HDYLInfo)
            {
                if (BetsTokenKey != "")
                {
                    pRequest.Headers[BetsTokenKey] = BetsTokenValue;
                }
                pRequest.Headers["Merchant"] = "hongda";
                pRequest.Headers["X-Gateway-Version"] = "1";
            }
            else if (AppInfo.PTInfo == AppInfo.XGLLInfo)
            {
                if (BetsTokenKey != "")
                {
                    pRequest.Headers[BetsTokenKey] = BetsTokenValue;
                }
                pRequest.Headers["Merchant"] = "sgl818";
            }
            if (((((((AppInfo.PTInfo == AppInfo.CAIHInfo) || (AppInfo.PTInfo == AppInfo.CBLInfo)) || ((AppInfo.PTInfo == AppInfo.THENInfo) || (AppInfo.PTInfo == AppInfo.SYYLInfo))) || (((AppInfo.PTInfo == AppInfo.MTYLInfo) || (AppInfo.PTInfo == AppInfo.HUBOInfo)) || ((AppInfo.PTInfo == AppInfo.CTXInfo) || (AppInfo.PTInfo == AppInfo.TYYLInfo)))) || ((((AppInfo.PTInfo == AppInfo.AMBLRInfo) || (AppInfo.PTInfo == AppInfo.CYYLInfo)) || ((AppInfo.PTInfo == AppInfo.JYYLInfo) || (AppInfo.PTInfo == AppInfo.NBAYLInfo))) || (((AppInfo.PTInfo == AppInfo.MXYLInfo) || (AppInfo.PTInfo == AppInfo.WCAIInfo)) || ((AppInfo.PTInfo == AppInfo.YHSGInfo) || (AppInfo.PTInfo == AppInfo.XTYLInfo))))) || ((AppInfo.PTInfo == AppInfo.XWYLInfo) || (AppInfo.PTInfo == AppInfo.B6YLInfo))) || (AppInfo.PTInfo == AppInfo.QFZXInfo))
            {
                pRequest.Accept = "application/json, text/javascript, */*; q=0.01";
            }
            if ((AppInfo.PTInfo == AppInfo.THYLInfo) || (AppInfo.PTInfo == AppInfo.CLYLInfo))
            {
                pRequest.Accept = "application/json, text/plain, */*";
            }
            if ((AppInfo.PTInfo == AppInfo.LFYLInfo) || (AppInfo.PTInfo == AppInfo.LF2Info))
            {
                pRequest.Headers["Upgrade-Insecure-Requests"] = "1";
            }
            if ((AppInfo.PTInfo == AppInfo.LFGJInfo) || (AppInfo.PTInfo == AppInfo.SYYLInfo))
            {
                pRequest.UserAgent = null;
            }
            if (((((((AppInfo.PTInfo == AppInfo.FSYLInfo) || (AppInfo.PTInfo == AppInfo.ZDYLInfo)) || ((AppInfo.PTInfo == AppInfo.NBYLInfo) || (AppInfo.PTInfo == AppInfo.CAIHInfo))) || (((AppInfo.PTInfo == AppInfo.THYLInfo) || (AppInfo.PTInfo == AppInfo.DAZYLInfo)) || ((AppInfo.PTInfo == AppInfo.ZYLInfo) || (AppInfo.PTInfo == AppInfo.CBLInfo)))) || ((((AppInfo.PTInfo == AppInfo.MTYLInfo) || (AppInfo.PTInfo == AppInfo.CTXInfo)) || ((AppInfo.PTInfo == AppInfo.CYYLInfo) || (AppInfo.PTInfo == AppInfo.JYYLInfo))) || (((AppInfo.PTInfo == AppInfo.MXYLInfo) || (AppInfo.PTInfo == AppInfo.WCAIInfo)) || ((AppInfo.PTInfo == AppInfo.YHSGInfo) || (AppInfo.PTInfo == AppInfo.XTYLInfo))))) || (((AppInfo.PTInfo == AppInfo.XWYLInfo) || (AppInfo.PTInfo == AppInfo.WZYLInfo)) || (AppInfo.PTInfo == AppInfo.YZCPInfo))) || (AppInfo.PTInfo == AppInfo.QFZXInfo))
            {
                pRequest.Headers.Remove("x-requested-with");
            }
        }

        public static void SaveCookies(string pCookie, string pHostLine = "")
        {
            if ((pCookie != "") && (pCookie != null))
            {
                if (pHostLine == "")
                {
                    pHostLine = AppInfo.PTInfo.GetHostLine();
                }
                string[] strArray = pCookie.Split(new char[] { ';' });
                foreach (string str in strArray)
                {
                    if (str != "")
                    {
                        string[] strArray2 = str.Split(new char[] { '=' });
                        if (strArray2.Length >= 2)
                        {
                            try
                            {
                                string name = strArray2[0].Trim();
                                string str3 = str.Substring(str.IndexOf('=') + 1).Trim();
                                Cookie cookie = new Cookie(name, str3) {
                                    Domain = pHostLine
                                };
                                CookieContainers.Add(cookie);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        public enum InternetFlags
        {
            INTERNET_COOKIE_HTTPONLY = 0x2000,
            INTERNET_COOKIE_THIRD_PARTY = 0x20000,
            INTERNET_FLAG_RESTRICTED_ZONE = 0x10
        }
    }
}

