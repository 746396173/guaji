namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;

    internal class LianZhong
    {
        private static string Between(string str, string leftstr, string rightstr)
        {
            int startIndex = str.IndexOf(leftstr) + leftstr.Length;
            return str.Substring(startIndex, str.IndexOf(rightstr, startIndex) - startIndex);
        }

        public static string GetUserInfo(string strVcodeUser, string strVcodePass)
        {
            string postData = Resources.getUserCode.Replace("[username]", strVcodeUser).Replace("[password]", ToBase64(strVcodePass));
            string str = HttpRequest("http://dama3.hyslt.com/api.php?mod=yzm&act=point", postData, "multipart/form-data; boundary=---------------------------7dd3901221176");
            if (str.LastIndexOf("no point") != -1)
            {
                return "亲爱的联众用户,您当前的余额为0!";
            }
            if (str.LastIndexOf("name or pw error") != -1)
            {
                return "亲爱的联众用户,请输入的账号或者密码错误";
            }
            return ("亲爱的联众用户,您当前的余额为" + Between(str, "true,\"data\":", "}"));
        }

        private static string HttpRequest(string url, string postData, string ContentType)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = ContentType;
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; MALN)";
            request.KeepAlive = true;
            request.Timeout = 0x3a98;
            byte[] bytes = Encoding.UTF8.GetBytes(postData.ToString());
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            response.Close();
            request.Abort();
            return str;
        }

        public static string RecByte_A(byte[] vcode, int len, string strVcodeUser, string strVcodePass, string strSoftkey) => 
            recCode(vcode, len, 0, 0, 0, strVcodeUser, strVcodePass, strSoftkey);

        public static string RecByte_A_2(byte[] vcode, int len, int codeType, int codeMinLen, int codeMaxLen, string strVcodeUser, string strVcodePass, string strSoftkey) => 
            recCode(vcode, len, codeType, codeMinLen, codeMaxLen, strVcodeUser, strVcodePass, strSoftkey);

        public static string recCode(byte[] vcode, int len, int codeType, int codeMinLen, int codeMaxLen, string strVcodeUser, string strVcodePass, string strSoftkey)
        {
            string postData = Resources.getUser.Replace("[username]", strVcodeUser).Replace("[password]", ToBase64(strVcodePass));
            string str = HttpRequest("http://dama3.hyslt.com/api.php?mod=yzm&act=server", postData, "multipart/form-data; boundary=---------------------------7dd3901221176");
            if (str.LastIndexOf("no point") != -1)
            {
                return "亲爱的联众用户,您当前的余额为0!";
            }
            if (str.LastIndexOf("name or pw error") != -1)
            {
                return "亲爱的联众用户,请输入的账号或者密码错误";
            }
            string str3 = Between(str, "true,\"data\":\"", "\"");
            string str4 = Resources.getUpload1.Replace("[username]", strVcodeUser).Replace("[password]", ToBase64(strVcodePass));
            byte[] bytes = Encoding.Default.GetBytes(str4 + "\r\n\r\n");
            string str5 = Resources.getUpload2.Replace("[strSoftkey]", strSoftkey).Replace("[yzmtype]", codeType.ToString()).Replace("[yzm_maxlen]", codeMaxLen.ToString()).Replace("[yzm_minlen]", codeMinLen.ToString());
            byte[] buffer2 = Encoding.Default.GetBytes("\r\n" + str5);
            byte[] array = new byte[(bytes.Length + vcode.Length) + buffer2.Length];
            bytes.CopyTo(array, 0);
            vcode.CopyTo(array, bytes.Length);
            buffer2.CopyTo(array, (int) (bytes.Length + vcode.Length));
            string newValue = Between(UploadImg("http://" + str3 + "/api.php?mod=yzm&act=add", array, "multipart/form-data; boundary=---------------------------7dd165a60630"), "true,\"data\":", "}");
            postData = Resources.getResultParameter.Replace("[打码Id]", newValue);
            Thread.Sleep(0x7d0);
            int num = 0;
            while (true)
            {
                str = HttpRequest("http://" + str3 + "/api.php?mod=yzm&act=result", postData, "multipart/form-data; boundary=---------------------------7dd3901221176");
                if ((str.LastIndexOf("wait dama") != -1) || (str.LastIndexOf("等待识别") != -1))
                {
                    num++;
                    if (num == 15)
                    {
                        return "Error:TimeOut!";
                    }
                    Thread.Sleep(0x3e8);
                }
                else
                {
                    if (str.LastIndexOf("") != -1)
                    {
                        string[] strArray = str3.Split(new char[] { '.' });
                        string str7 = Between(str, "true,\"data\":", "}").Replace("\"", "");
                        return (str7 + "|!|" + newValue + "&" + strArray[0]);
                    }
                    return "Error:TimeOut!";
                }
            }
        }

        public static string RecYZM_A(string strYZMPath, string strVcodeUser, string strVcodePass, string strSoftkey)
        {
            MemoryStream stream = new MemoryStream();
            Bitmap bitmap = new Bitmap(strYZMPath);
            bitmap.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = new byte[stream.Length];
            stream.Position = 0L;
            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
            bitmap.Dispose();
            return RecByte_A(buffer, buffer.Length, strVcodeUser, strVcodePass, strSoftkey);
        }

        public static string RecYZM_A_2(string strYZMPath, int codeType, int codeMinLen, int codeMaxLen, string strVcodeUser, string strVcodePass, string strSoftkey)
        {
            MemoryStream stream = new MemoryStream();
            Bitmap bitmap = new Bitmap(strYZMPath);
            bitmap.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = new byte[stream.Length];
            stream.Position = 0L;
            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
            bitmap.Dispose();
            return recCode(buffer, buffer.Length, codeType, codeMinLen, codeMaxLen, strVcodeUser, strVcodePass, strSoftkey);
        }

        public static string Reglz(string strVcodeUser, string strVcodePass)
        {
            string url = "http://eee.hyslt.com/api.php?mod=yzm&act=register";
            string postData = Resources.Reglz.Replace("[username]", strVcodeUser).Replace("[password]", strVcodePass);
            string str = HttpRequest(url, postData, "multipart/form-data; boundary=---------------------------7dd3901221176");
            if (Between(str, "result\":", ",\"").Equals("true"))
            {
                return "注册成功";
            }
            return Between(str, "data\":\"", "\"");
        }

        public static void ReportError(string strVcodeUser, string vcodeId)
        {
            string[] strArray = vcodeId.Split(new char[] { '&' });
            string postData = Resources.getErrorCode.Replace("[验证码id]", strArray[0]);
            byte[] bytes = Encoding.UTF8.GetBytes(postData.ToString());
            HttpRequest("http://" + strArray[1] + ".hyslt.com/api.php?mod=dmuser&act=yzm_error", postData, "multipart/form-data; boundary=---------------------------7dd3901221176");
        }

        private static string ToBase64(string value) => 
            Convert.ToBase64String(Encoding.Default.GetBytes(value));

        private static string UploadImg(string url, byte[] data, string ContentType)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = ContentType;
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; MALN)";
            request.KeepAlive = true;
            request.Timeout = 0x3a98;
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            response.Close();
            request.Abort();
            return str;
        }
    }
}

