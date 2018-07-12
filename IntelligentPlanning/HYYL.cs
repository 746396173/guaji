namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class HYYL : PTBase
    {
        public override bool BetsMain(ConfigurationStatus.SCPlan plan, ref string pHint)
        {
            bool flag = false;
            try
            {
                string betsLine = this.GetBetsLine(plan.Type);
                string lotteryLine = this.GetLotteryLine(plan.Type, false);
                Dictionary<string, Dictionary<string, List<string>>> fNNumberDic = plan.FNNumberDic;
                foreach (string str3 in fNNumberDic.Keys)
                {
                    Dictionary<string, List<string>> dictionary2 = fNNumberDic[str3];
                    foreach (string str4 in dictionary2.Keys)
                    {
                        if (plan.IsMNState(str4, true))
                        {
                            flag = true;
                            pHint = "投注成功";
                        }
                        else
                        {
                            List<string> pTNumberList = plan.GetPTNumberList(dictionary2[str4]);
                            string pResponsetext = "";
                            int num = plan.FNNumber(str4);
                            string prize = base.Prize;
                            string format = "betnum={0}&money={1}&gameId={2}&number={3}&isTrace=false&istracestop=1&orders%5B0%5D%5BmethodId%5D={4}&orders%5B0%5D%5Bbetnum%5D={0}&orders%5B0%5D%5Bmultiple%5D={5}&orders%5B0%5D%5Bmode%5D={6}&orders%5B0%5D%5Bmoney%5D={1}&orders%5B0%5D%5Bcontent%5D={7}&orders%5B0%5D%5Brebate%5D={8}&orders%5B0%5D%5BrebateMoney%5D={9}&{10}";
                            format = string.Format(format, new object[] { num, plan.AutoTotalMoney(str4, true), this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), this.GetPlayMethodID(plan.Type, plan.Play), Convert.ToInt32(plan.AutoTimes(str4, true)), plan.Money / 2.0, this.GetNumberList1(pTNumberList, plan.Play, null), prize, "0", this.GetRXWZString1(plan.RXWZ, plan.Play) });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, base.BetsTime2, "UTF-8", true);
                            flag = this.CheckReturn(pResponsetext, true);
                            pHint = this.GetReturn(pResponsetext);
                        }
                    }
                }
            }
            catch
            {
            }
            return flag;
        }

        public override bool CheckBreakConnect(string pHint) => 
            pHint.Contains("loginTimeout");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"isSuccess\":1") || (pResponseText == "投注成功"));

        private bool CheckVerifyCode(string pVerifyCode)
        {
            if (pVerifyCode.Length != 5)
            {
                return false;
            }
            return true;
        }

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"amount\":", ",", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/keepalive?_={DateTime.Now.ToOADate()}";

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false).Replace("-", "");
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/game/bet/bet");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.HYGGFFC)
            {
                return "35";
            }
            if (pType == ConfigurationStatus.LotteryType.HYTTFFC)
            {
                return "37";
            }
            if (pType == ConfigurationStatus.LotteryType.HYSkyFFC)
            {
                return "41";
            }
            if (pType == ConfigurationStatus.LotteryType.HYYJFFC)
            {
                return "34";
            }
            if (pType == ConfigurationStatus.LotteryType.HYHLWFFC)
            {
                return "42";
            }
            if (pType == ConfigurationStatus.LotteryType.HYYTFFC)
            {
                return "38";
            }
            if (pType == ConfigurationStatus.LotteryType.HYHGSSC)
            {
                return "23";
            }
            if (pType == ConfigurationStatus.LotteryType.HYDJSSC)
            {
                return "43";
            }
            if (pType == ConfigurationStatus.LotteryType.HYFLBSSC)
            {
                return "22";
            }
            if (pType == ConfigurationStatus.LotteryType.HYXDLSSC)
            {
                return "44";
            }
            if (pType == ConfigurationStatus.LotteryType.HYXJPSSC)
            {
                return "45";
            }
            if (pType == ConfigurationStatus.LotteryType.HYJNDFFC)
            {
                return "25";
            }
            if (pType == ConfigurationStatus.LotteryType.HYHNFFC)
            {
                return "24";
            }
            if (pType == ConfigurationStatus.LotteryType.HYFFC)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.HY2FC)
            {
                return "48";
            }
            if (pType == ConfigurationStatus.LotteryType.HYTXFFC)
            {
                return "53";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.HY11X5)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "26";
            }
            if (pType == ConfigurationStatus.LotteryType.HYPK10)
            {
                str = "27";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/game/index/get_historys?gameid={this.GetBetsLotteryID(pType)}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLineString(string pResponseText)
        {
            string str = pResponseText;
            if (str.Contains("http://789aobo.com"))
            {
                str = "线路1";
            }
            return str;
        }

        public override string GetLoginLine() => 
            (this.GetLine() + "/login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/game?g={this.GetPTLotteryName(pType)}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            int num;
            string str2;
            int num2;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = CommFunc.Join(pNumberList[num], ",", -1);
                        list.Add(str2);
                    }
                    return CommFunc.Join(list, "|").Replace("*", "");
                }
                if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num2 = AppInfo.FiveDic[ch.ToString()];
                    list = new List<string>();
                    for (num = 0; num < 5; num++)
                    {
                        str2 = "*";
                        if (num == num2)
                        {
                            str2 = CommFunc.Join(pNumberList, ",");
                        }
                        list.Add(str2);
                    }
                    return CommFunc.Join(list, "|").Replace("*", "");
                }
                if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, ",");
                    }
                    return str;
                }
                list = new List<string>();
                for (num = 0; num < pNumberList.Count; num++)
                {
                    str2 = CommFunc.Join(pNumberList[num], ",", -1);
                    list.Add(str2);
                }
                return CommFunc.Join(list, "|");
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, "|").Replace(" ", ",");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                return CommFunc.Join(pNumberList, "|").Replace(" ", ",");
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                return CommFunc.Join(pNumberList, "|").Replace(" ", ",").Replace("*", "");
            }
            num2 = -1;
            if (playName.Contains("冠军"))
            {
                num2 = 0;
            }
            else if (playName.Contains("亚军"))
            {
                num2 = 1;
            }
            else
            {
                num2 = CommFunc.GetPlayNum(playName) - 1;
            }
            list = new List<string>();
            int num3 = (playName == "猜冠军猜冠军") ? 1 : 10;
            for (num = 0; num < num3; num++)
            {
                str2 = "*";
                if (num == num2)
                {
                    str2 = CommFunc.Join(pNumberList, ",");
                }
                list.Add(str2);
            }
            return CommFunc.Join(list, "|").Replace("*", "");
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "70";
                }
                if (playName == "前三直选复式")
                {
                    return "68";
                }
                if (playName == "前三组三复式")
                {
                    return "81";
                }
                if (playName == "前三组六复式")
                {
                    return "83";
                }
                if (playName == "后三直选单式")
                {
                    return "114";
                }
                if (playName == "后三直选复式")
                {
                    return "112";
                }
                if (playName == "后三组三复式")
                {
                    return "125";
                }
                if (playName == "后三组六复式")
                {
                    return "127";
                }
                if (playName == "中三直选单式")
                {
                    return "92";
                }
                if (playName == "中三直选复式")
                {
                    return "90";
                }
                if (playName == "中三组三复式")
                {
                    return "103";
                }
                if (playName == "中三组六复式")
                {
                    return "105";
                }
                if (playName == "前二直选单式")
                {
                    return "136";
                }
                if (playName == "前二直选复式")
                {
                    return "134";
                }
                if (playName == "后二直选单式")
                {
                    return "142";
                }
                if (playName == "后二直选复式")
                {
                    return "140";
                }
                if (playName == "前四直选单式")
                {
                    return "30";
                }
                if (playName == "前四直选复式")
                {
                    return "28";
                }
                if (playName == "后四直选单式")
                {
                    return "50";
                }
                if (playName == "后四直选复式")
                {
                    return "48";
                }
                if (playName == "五星直选单式")
                {
                    return "5";
                }
                if (playName == "五星直选复式")
                {
                    return "3";
                }
                if (playName == "任三直选单式")
                {
                    return "811";
                }
                if (playName == "任三直选复式")
                {
                    return "500";
                }
                if (playName == "任三组三复式")
                {
                    return "502";
                }
                if (playName == "任三组六复式")
                {
                    return "503";
                }
                if (playName == "任二直选单式")
                {
                    return "813";
                }
                if (playName == "任二直选复式")
                {
                    return "506";
                }
                if (playName == "任四直选单式")
                {
                    return "809";
                }
                if (playName == "任四直选复式")
                {
                    return "494";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "146";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "152";
                }
                if (playName == "前二直选单式")
                {
                    return "185";
                }
                if (playName == "任选复式一中一")
                {
                    return "776";
                }
                if (playName == "任选复式二中二")
                {
                    return "778";
                }
                if (playName == "任选复式三中三")
                {
                    return "780";
                }
                if (playName == "任选复式四中四")
                {
                    return "782";
                }
                if (playName == "任选复式五中五")
                {
                    return "784";
                }
                if (playName == "任选单式一中一")
                {
                    return "793";
                }
                if (playName == "任选单式二中二")
                {
                    return "795";
                }
                if (playName == "任选单式三中三")
                {
                    return "797";
                }
                if (playName == "任选单式四中四")
                {
                    return "799";
                }
                if (playName == "任选单式五中五")
                {
                    str = "801";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "351";
                }
                if (playName == "猜前三复式")
                {
                    return "333";
                }
                if (playName == "猜前二单式")
                {
                    return "355";
                }
                if (playName == "猜前二复式")
                {
                    return "341";
                }
                if (playName == "猜前四单式")
                {
                    return "347";
                }
                if (playName == "猜前四复式")
                {
                    return "325";
                }
                if (playName == "猜前五单式")
                {
                    return "";
                }
                if (playName == "猜前五复式")
                {
                    return "";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "321";
                }
            }
            return str;
        }

        public override string GetPTHint(string pResponseText)
        {
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"msg\":\"", "\"", 0));
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "ssc_cq";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "ssc_xj";
            }
            if (pType == ConfigurationStatus.LotteryType.HYGGFFC)
            {
                return "ssc_google";
            }
            if (pType == ConfigurationStatus.LotteryType.HYTTFFC)
            {
                return "ssc_tweets";
            }
            if (pType == ConfigurationStatus.LotteryType.HYSkyFFC)
            {
                return "ssc_skype";
            }
            if (pType == ConfigurationStatus.LotteryType.HYYJFFC)
            {
                return "ssc_em";
            }
            if (pType == ConfigurationStatus.LotteryType.HYHLWFFC)
            {
                return "ssc_traffic";
            }
            if (pType == ConfigurationStatus.LotteryType.HYYTFFC)
            {
                return "ssc_youtube";
            }
            if (pType == ConfigurationStatus.LotteryType.HYHGSSC)
            {
                return "ssc_korea";
            }
            if (pType == ConfigurationStatus.LotteryType.HYDJSSC)
            {
                return "ssc_tokyo";
            }
            if (pType == ConfigurationStatus.LotteryType.HYFLBSSC)
            {
                return "ssc_ph";
            }
            if (pType == ConfigurationStatus.LotteryType.HYXDLSSC)
            {
                return "ssc_newdelhi";
            }
            if (pType == ConfigurationStatus.LotteryType.HYXJPSSC)
            {
                return "ssc_singapore";
            }
            if (pType == ConfigurationStatus.LotteryType.HYJNDFFC)
            {
                return "ssc_canada";
            }
            if (pType == ConfigurationStatus.LotteryType.HYHNFFC)
            {
                return "ssc_hanoi";
            }
            if (pType == ConfigurationStatus.LotteryType.HYFFC)
            {
                return "ssc_ffc";
            }
            if (pType == ConfigurationStatus.LotteryType.HY2FC)
            {
                return "ssc_ffc2";
            }
            if (pType == ConfigurationStatus.LotteryType.HYTXFFC)
            {
                return "ssc_tencent";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "n115_gd";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "n115_jx";
            }
            if (pType == ConfigurationStatus.LotteryType.HY11X5)
            {
                return "n115_ffc";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "pk10_bj";
            }
            if (pType == ConfigurationStatus.LotteryType.HYPK10)
            {
                return "pk10_ffc";
            }
            return CommFunc.GetLotteryID(pType);
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString1(List<int> pRXWZ, string playName)
        {
            string str = "";
            if (!CommFunc.CheckPlayIsRX(playName))
            {
                return str;
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                pRXWZ = CommFunc.ConvertIntList("0-4");
            }
            List<string> pList = new List<string>();
            foreach (int num in pRXWZ)
            {
                string item = $"orders%5B0%5D%5Bpositions%5D%5B%5D={num}";
                pList.Add(item);
            }
            return CommFunc.Join(pList, "&");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.Prize == "")
                {
                    string lotteryLine = this.GetLotteryLine(pType, false);
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        base.Prize = CommFunc.GetIndexString(pResponsetext, "<option value=\"", "\"", 0);
                    }
                }
            }
            catch
            {
            }
        }

        public override string GetWebVerifyCode(string pVerifyCodeFile)
        {
            string pVerifyCode = "";
            try
            {
                string str2 = $"/captcha?{DateTime.Now.ToOADate()}";
                string pUrl = this.GetLine() + str2;
                System.IO.File.Delete(pVerifyCodeFile);
                Bitmap bitmap = new Bitmap(HttpHelper.GetResponseImage(pUrl, this.GetLoginLine(), "GET", "", 0x1770, "UTF-8", true));
                bitmap.Save(pVerifyCodeFile);
                bitmap.Dispose();
                while (!System.IO.File.Exists(pVerifyCodeFile))
                {
                    Thread.Sleep(500);
                }
                pVerifyCode = VerifyCodeAPI.VerifyCodeMain(base.PTID, pVerifyCodeFile);
                if (!this.CheckVerifyCode(pVerifyCode))
                {
                    return this.GetWebVerifyCode(pVerifyCodeFile);
                }
            }
            catch
            {
            }
            return pVerifyCode;
        }

        public bool InputWeb(string pID, string pW, ref string pHint)
        {
            bool flag = false;
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLoginLine();
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string pData = string.Format("forward=&dosubmit=1&hashcode={2}&username={0}&password={1}&code=", pID, str4, base.Token);
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("登录成功");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "span class=\"title-status title-status-fail\">", "<", 0);
                return flag;
            }
            string httpHelperCookieString = HttpHelper.GetHttpHelperCookieString(new Uri(this.GetLoginLine()), null);
            AppInfo.PTInfo.WebCookie = httpHelperCookieString;
            HttpHelper.SaveCookies(httpHelperCookieString, "");
            return flag;
        }

        public bool LoginWeb()
        {
            HttpHelper.CookieContainers = new CookieContainer();
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("登录");
            if (flag)
            {
                base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("hashcode"));
            }
            return flag;
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, quitPTLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
        }

        public override bool WebLoginMain(string pID, string pW, ref string pHint)
        {
            if (!this.LoginWeb())
            {
                return false;
            }
            if (!this.InputWeb(pID, pW, ref pHint))
            {
                return false;
            }
            return true;
        }
    }
}

