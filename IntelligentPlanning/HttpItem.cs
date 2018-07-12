namespace IntelligentPlanning
{
    using System;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    public class HttpItem
    {
        private string _Accept = "text/html, application/xhtml+xml, */*";
        private string _CerPath = string.Empty;
        private X509CertificateCollection _ClentCertificates;
        private string _ContentType = "text/html";
        private string _Cookie = string.Empty;
        private System.Text.Encoding _Encoding = null;
        private bool _expect100continue = false;
        private System.Net.ICredentials _ICredentials = CredentialCache.DefaultCredentials;
        private DateTime? _IfModifiedSince = null;
        private System.Net.IPEndPoint _IPEndPoint = null;
        private bool _KeepAlive = true;
        private int _MaximumAutomaticRedirections;
        private string _Method = "GET";
        private string _Postdata = string.Empty;
        private byte[] _PostdataByte = null;
        private IntelligentPlanning.PostDataType _PostDataType = IntelligentPlanning.PostDataType.String;
        private System.Text.Encoding _PostEncoding;
        private Version _ProtocolVersion;
        private int _ReadWriteTimeout = 0x7530;
        private string _Referer = string.Empty;
        private IntelligentPlanning.ResultCookieType _ResultCookieType = IntelligentPlanning.ResultCookieType.String;
        private int _Timeout = 0x186a0;
        private string _URL = string.Empty;
        private string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
        private System.Net.WebProxy _WebProxy;
        private bool allowautoredirect = false;
        private int connectionlimit = 0x400;
        private System.Net.CookieCollection cookiecollection = null;
        private WebHeaderCollection header = new WebHeaderCollection();
        private bool isToLower = false;
        private string proxyip = string.Empty;
        private string proxypwd = string.Empty;
        private string proxyusername = string.Empty;
        private IntelligentPlanning.ResultType resulttype = IntelligentPlanning.ResultType.String;

        public string Accept
        {
            get => 
                this._Accept;
            set
            {
                this._Accept = value;
            }
        }

        public bool Allowautoredirect
        {
            get => 
                this.allowautoredirect;
            set
            {
                this.allowautoredirect = value;
            }
        }

        public string CerPath
        {
            get => 
                this._CerPath;
            set
            {
                this._CerPath = value;
            }
        }

        public X509CertificateCollection ClentCertificates
        {
            get => 
                this._ClentCertificates;
            set
            {
                this._ClentCertificates = value;
            }
        }

        public int Connectionlimit
        {
            get => 
                this.connectionlimit;
            set
            {
                this.connectionlimit = value;
            }
        }

        public string ContentType
        {
            get => 
                this._ContentType;
            set
            {
                this._ContentType = value;
            }
        }

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
                this.cookiecollection;
            set
            {
                this.cookiecollection = value;
            }
        }

        public System.Text.Encoding Encoding
        {
            get => 
                this._Encoding;
            set
            {
                this._Encoding = value;
            }
        }

        public bool Expect100Continue
        {
            get => 
                this._expect100continue;
            set
            {
                this._expect100continue = value;
            }
        }

        public WebHeaderCollection Header
        {
            get => 
                this.header;
            set
            {
                this.header = value;
            }
        }

        public System.Net.ICredentials ICredentials
        {
            get => 
                this._ICredentials;
            set
            {
                this._ICredentials = value;
            }
        }

        public DateTime? IfModifiedSince
        {
            get => 
                this._IfModifiedSince;
            set
            {
                this._IfModifiedSince = value;
            }
        }

        public System.Net.IPEndPoint IPEndPoint
        {
            get => 
                this._IPEndPoint;
            set
            {
                this._IPEndPoint = value;
            }
        }

        public bool IsToLower
        {
            get => 
                this.isToLower;
            set
            {
                this.isToLower = value;
            }
        }

        public bool KeepAlive
        {
            get => 
                this._KeepAlive;
            set
            {
                this._KeepAlive = value;
            }
        }

        public int MaximumAutomaticRedirections
        {
            get => 
                this._MaximumAutomaticRedirections;
            set
            {
                this._MaximumAutomaticRedirections = value;
            }
        }

        public string Method
        {
            get => 
                this._Method;
            set
            {
                this._Method = value;
            }
        }

        public string Postdata
        {
            get => 
                this._Postdata;
            set
            {
                this._Postdata = value;
            }
        }

        public byte[] PostdataByte
        {
            get => 
                this._PostdataByte;
            set
            {
                this._PostdataByte = value;
            }
        }

        public IntelligentPlanning.PostDataType PostDataType
        {
            get => 
                this._PostDataType;
            set
            {
                this._PostDataType = value;
            }
        }

        public System.Text.Encoding PostEncoding
        {
            get => 
                this._PostEncoding;
            set
            {
                this._PostEncoding = value;
            }
        }

        public Version ProtocolVersion
        {
            get => 
                this._ProtocolVersion;
            set
            {
                this._ProtocolVersion = value;
            }
        }

        public string ProxyIp
        {
            get => 
                this.proxyip;
            set
            {
                this.proxyip = value;
            }
        }

        public string ProxyPwd
        {
            get => 
                this.proxypwd;
            set
            {
                this.proxypwd = value;
            }
        }

        public string ProxyUserName
        {
            get => 
                this.proxyusername;
            set
            {
                this.proxyusername = value;
            }
        }

        public int ReadWriteTimeout
        {
            get => 
                this._ReadWriteTimeout;
            set
            {
                this._ReadWriteTimeout = value;
            }
        }

        public string Referer
        {
            get => 
                this._Referer;
            set
            {
                this._Referer = value;
            }
        }

        public IntelligentPlanning.ResultCookieType ResultCookieType
        {
            get => 
                this._ResultCookieType;
            set
            {
                this._ResultCookieType = value;
            }
        }

        public IntelligentPlanning.ResultType ResultType
        {
            get => 
                this.resulttype;
            set
            {
                this.resulttype = value;
            }
        }

        public int Timeout
        {
            get => 
                this._Timeout;
            set
            {
                this._Timeout = value;
            }
        }

        public string URL
        {
            get => 
                this._URL;
            set
            {
                this._URL = value;
            }
        }

        public string UserAgent
        {
            get => 
                this._UserAgent;
            set
            {
                this._UserAgent = value;
            }
        }

        public System.Net.WebProxy WebProxy
        {
            get => 
                this._WebProxy;
            set
            {
                this._WebProxy = value;
            }
        }
    }
}

