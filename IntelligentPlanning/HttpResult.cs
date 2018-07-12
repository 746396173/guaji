namespace IntelligentPlanning
{
    using System;
    using System.Net;
    using System.Runtime.CompilerServices;

    public class HttpResult
    {
        private string _Cookie;
        private System.Net.CookieCollection _CookieCollection;
        private WebHeaderCollection _Header;
        private string _html = string.Empty;
        private byte[] _ResultByte;
        private HttpStatusCode _StatusCode;
        private string _StatusDescription;

        public string Cookie
        {
            get => 
                this._Cookie;
            set
            {
                this._Cookie = value;
            }
        }

        public System.Net.CookieCollection CookieCollection
        {
            get => 
                this._CookieCollection;
            set
            {
                this._CookieCollection = value;
            }
        }

        public WebHeaderCollection Header
        {
            get => 
                this._Header;
            set
            {
                this._Header = value;
            }
        }

        public string Html
        {
            get => 
                this._html;
            set
            {
                this._html = value;
            }
        }

        public string RedirectUrl
        {
            get
            {
                try
                {
                    if ((this.Header != null) && (this.Header.Count > 0))
                    {
                        string relativeUri = this.Header["location"].ToString().Trim();
                        string str2 = relativeUri.ToLower();
                        if ((str2 != "") && !(str2.StartsWith("http://") || str2.StartsWith("https://")))
                        {
                            relativeUri = new Uri(new Uri(this.ResponseUri), relativeUri).AbsoluteUri;
                        }
                        return relativeUri;
                    }
                }
                catch
                {
                }
                return string.Empty;
            }
        }

        public string ResponseUri { get; set; }

        public byte[] ResultByte
        {
            get => 
                this._ResultByte;
            set
            {
                this._ResultByte = value;
            }
        }

        public HttpStatusCode StatusCode
        {
            get => 
                this._StatusCode;
            set
            {
                this._StatusCode = value;
            }
        }

        public string StatusDescription
        {
            get => 
                this._StatusDescription;
            set
            {
                this._StatusDescription = value;
            }
        }
    }
}

