namespace IntelligentPlanning
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Text.RegularExpressions;

    public class SFHttpHelper
    {
        private IPEndPoint _IPEndPoint = null;
        private Encoding encoding = Encoding.Default;
        private Encoding postencoding = Encoding.Default;
        private HttpWebRequest request = null;
        private HttpWebResponse response = null;

        private IPEndPoint BindIPEndPointCallback(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount) => 
            this._IPEndPoint;

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) => 
            true;

        private byte[] GetByte()
        {
            byte[] buffer = null;
            MemoryStream memoryStream = new MemoryStream();
            if ((this.response.ContentEncoding != null) && this.response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
            {
                memoryStream = this.GetMemoryStream(new GZipStream(this.response.GetResponseStream(), CompressionMode.Decompress));
            }
            else
            {
                memoryStream = this.GetMemoryStream(this.response.GetResponseStream());
            }
            buffer = memoryStream.ToArray();
            memoryStream.Close();
            return buffer;
        }

        private void GetData(HttpItem item, HttpResult result)
        {
            result.StatusCode = this.response.StatusCode;
            result.StatusDescription = this.response.StatusDescription;
            result.ResponseUri = this.response.ResponseUri.ToString();
            result.Header = this.response.Headers;
            if (this.response.Cookies != null)
            {
                result.CookieCollection = this.response.Cookies;
            }
            if (this.response.Headers["set-cookie"] != null)
            {
                result.Cookie = this.response.Headers["set-cookie"];
            }
            byte[] @byte = this.GetByte();
            if ((@byte != null) && (@byte.Length > 0))
            {
                this.SetEncoding(item, result, @byte);
                result.Html = this.encoding.GetString(@byte);
            }
            else
            {
                result.Html = string.Empty;
            }
        }

        public HttpResult GetHtml(HttpItem item)
        {
            Exception exception;
            HttpWebResponse response2;
            HttpResult result = new HttpResult();
            try
            {
                this.SetRequest(item);
            }
            catch (Exception exception1)
            {
                exception = exception1;
                result.Cookie = string.Empty;
                result.Header = null;
                result.Html = exception.Message;
                result.StatusDescription = "配置参数时出错：" + exception.Message;
                return result;
            }
            try
            {
                using (response2 = this.response = (HttpWebResponse) this.request.GetResponse())
                {
                    this.GetData(item, result);
                }
            }
            catch (WebException exception2)
            {
                if (exception2.Response != null)
                {
                    using (response2 = this.response = (HttpWebResponse) exception2.Response)
                    {
                        this.GetData(item, result);
                    }
                }
                else
                {
                    result.Html = exception2.Message;
                }
            }
            catch (Exception exception4)
            {
                exception = exception4;
                result.Html = exception.Message;
            }
            if (item.IsToLower)
            {
                result.Html = result.Html.ToLower();
            }
            return result;
        }

        private MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream stream = new MemoryStream();
            int count = 0x100;
            byte[] buffer = new byte[count];
            for (int i = streamResponse.Read(buffer, 0, count); i > 0; i = streamResponse.Read(buffer, 0, count))
            {
                stream.Write(buffer, 0, i);
            }
            return stream;
        }

        private void SetCer(HttpItem item)
        {
            if (!string.IsNullOrEmpty(item.CerPath))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
                this.request = (HttpWebRequest) WebRequest.Create(item.URL);
                this.SetCerList(item);
                this.request.ClientCertificates.Add(new X509Certificate(item.CerPath));
            }
            else
            {
                this.request = (HttpWebRequest) WebRequest.Create(item.URL);
                this.SetCerList(item);
            }
        }

        private void SetCerList(HttpItem item)
        {
            if ((item.ClentCertificates != null) && (item.ClentCertificates.Count > 0))
            {
                foreach (X509Certificate certificate in item.ClentCertificates)
                {
                    this.request.ClientCertificates.Add(certificate);
                }
            }
        }

        private void SetCookie(HttpItem item)
        {
            if (!string.IsNullOrEmpty(item.Cookie))
            {
                this.request.Headers[HttpRequestHeader.Cookie] = item.Cookie;
            }
            if (item.ResultCookieType == ResultCookieType.CookieCollection)
            {
                this.request.CookieContainer = new CookieContainer();
                if ((item.CookieCollection != null) && (item.CookieCollection.Count > 0))
                {
                    this.request.CookieContainer.Add(item.CookieCollection);
                }
            }
        }

        private void SetEncoding(HttpItem item, HttpResult result, byte[] ResponseByte)
        {
            if (item.ResultType == ResultType.Byte)
            {
                result.ResultByte = ResponseByte;
            }
            if (this.encoding == null)
            {
                System.Text.RegularExpressions.Match match = Regex.Match(Encoding.Default.GetString(ResponseByte), "<meta[^<]*charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                string str = string.Empty;
                if ((match != null) && (match.Groups.Count > 0))
                {
                    str = match.Groups[1].Value.ToLower().Trim();
                }
                if (str.Length > 2)
                {
                    try
                    {
                        this.encoding = Encoding.GetEncoding(str.Replace("\"", string.Empty).Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk").Trim());
                    }
                    catch
                    {
                        if (string.IsNullOrEmpty(this.response.CharacterSet))
                        {
                            this.encoding = Encoding.UTF8;
                        }
                        else
                        {
                            this.encoding = Encoding.GetEncoding(this.response.CharacterSet);
                        }
                    }
                }
                else if (string.IsNullOrEmpty(this.response.CharacterSet))
                {
                    this.encoding = Encoding.UTF8;
                }
                else
                {
                    this.encoding = Encoding.GetEncoding(this.response.CharacterSet);
                }
            }
        }

        private void SetPostData(HttpItem item)
        {
            if (!this.request.Method.Trim().ToLower().Contains("get"))
            {
                if (item.PostEncoding != null)
                {
                    this.postencoding = item.PostEncoding;
                }
                byte[] postdataByte = null;
                if (((item.PostDataType == PostDataType.Byte) && (item.PostdataByte != null)) && (item.PostdataByte.Length > 0))
                {
                    postdataByte = item.PostdataByte;
                }
                else if (!((item.PostDataType != PostDataType.FilePath) || string.IsNullOrEmpty(item.Postdata)))
                {
                    StreamReader reader = new StreamReader(item.Postdata, this.postencoding);
                    postdataByte = this.postencoding.GetBytes(reader.ReadToEnd());
                    reader.Close();
                }
                else if (!string.IsNullOrEmpty(item.Postdata))
                {
                    postdataByte = this.postencoding.GetBytes(item.Postdata);
                }
                if (postdataByte != null)
                {
                    this.request.ContentLength = postdataByte.Length;
                    this.request.GetRequestStream().Write(postdataByte, 0, postdataByte.Length);
                }
                else
                {
                    this.request.ContentLength = 0L;
                }
            }
        }

        private void SetProxy(HttpItem item)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(item.ProxyIp))
            {
                flag = item.ProxyIp.ToLower().Contains("ieproxy");
            }
            if (!string.IsNullOrEmpty(item.ProxyIp) && !flag)
            {
                WebProxy proxy;
                if (item.ProxyIp.Contains(":"))
                {
                    string[] strArray = item.ProxyIp.Split(new char[] { ':' });
                    proxy = new WebProxy(strArray[0].Trim(), Convert.ToInt32(strArray[1].Trim())) {
                        Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd)
                    };
                    this.request.Proxy = proxy;
                }
                else
                {
                    proxy = new WebProxy(item.ProxyIp, false) {
                        Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd)
                    };
                    this.request.Proxy = proxy;
                }
            }
            else if (!flag)
            {
                this.request.Proxy = item.WebProxy;
            }
        }

        private void SetRequest(HttpItem item)
        {
            this.SetCer(item);
            if (item.IPEndPoint != null)
            {
                this._IPEndPoint = item.IPEndPoint;
                this.request.ServicePoint.BindIPEndPointDelegate = new BindIPEndPoint(this.BindIPEndPointCallback);
            }
            if ((item.Header != null) && (item.Header.Count > 0))
            {
                foreach (string str in item.Header.AllKeys)
                {
                    this.request.Headers.Add(str, item.Header[str]);
                }
            }
            this.SetProxy(item);
            if (item.ProtocolVersion != null)
            {
                this.request.ProtocolVersion = item.ProtocolVersion;
            }
            this.request.ServicePoint.Expect100Continue = item.Expect100Continue;
            this.request.Method = item.Method;
            this.request.Timeout = item.Timeout;
            this.request.KeepAlive = item.KeepAlive;
            this.request.ReadWriteTimeout = item.ReadWriteTimeout;
            if (item.IfModifiedSince.HasValue)
            {
                this.request.IfModifiedSince = Convert.ToDateTime(item.IfModifiedSince);
            }
            this.request.Accept = item.Accept;
            this.request.ContentType = item.ContentType;
            this.request.UserAgent = item.UserAgent;
            this.encoding = item.Encoding;
            this.request.Credentials = item.ICredentials;
            this.SetCookie(item);
            this.request.Referer = item.Referer;
            this.request.AllowAutoRedirect = item.Allowautoredirect;
            if (item.MaximumAutomaticRedirections > 0)
            {
                this.request.MaximumAutomaticRedirections = item.MaximumAutomaticRedirections;
            }
            this.SetPostData(item);
            if (item.Connectionlimit > 0)
            {
                this.request.ServicePoint.ConnectionLimit = item.Connectionlimit;
            }
        }
    }
}

