namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class WXYL : PTBase
    {
        public override bool BetsMain(ConfigurationStatus.SCPlan plan, ref string pHint)
        {
            bool flag = false;
            try
            {
                string betsLine = this.GetBetsLine(plan.Type);
                string pReferer = this.GetBetsLine(plan.Type);
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
                            string prize = this.GetPrize(plan.Type, plan.Play);
                            int num2 = Convert.ToInt32(Math.Pow(10.0, (double) (5 - plan.Unit)));
                            string format = "token={0}&planId={1}&bet%5B0%5D.betNum={2}&bet%5B0%5D.playId={3}&bet%5B0%5D.betMultiple={4}&bet%5B0%5D.moneyMethod={5}&bet%5B0%5D.betMethod={6}";
                            format = string.Format(format, new object[] { base.Token, this.GetBetsExpect(plan.CurrentExpect, ""), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), this.GetPlayMethodID(plan.Type, plan.Play), Convert.ToInt32(plan.AutoTimes(str4, true)), num2, "0" });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, pReferer, base.BetsTime3, "UTF-8", true);
                            flag = this.CheckReturn(pResponsetext, true);
                            pHint = this.GetReturn(pResponsetext);
                            Thread.Sleep(0x7d0);
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
            pHint.Contains("重新登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"result\":0,\"msg\":\"ok\"") || (pResponseText == "投注成功"));

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pData = $"token={base.Token}";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string str5 = CommFunc.GetIndexString(pResponsetext, "\"balance\":", ",", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str5) / 10000.0;
                base.Rebate = CommFunc.GetIndexString(pResponsetext, "\"userRebate\":", ",", 0);
                base.Prize = (1700.0 + (Convert.ToDouble(base.Rebate) * 2.0)).ToString();
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/acct/login/doauth.json");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            switch (iD)
            {
                case "XJSSC":
                case "SD11X5":
                case "GD11X5":
                case "JX11X5":
                    str2 = str2.Replace("-0", "-");
                    break;
            }
            return str2;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/ticket/bet/bet.json");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "3";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.HGLTC)
            {
                return "21";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "24";
            }
            if (pType == ConfigurationStatus.LotteryType.QQ15F)
            {
                return "33";
            }
            if (pType == ConfigurationStatus.LotteryType.QQ30M)
            {
                return "32";
            }
            if (pType == ConfigurationStatus.LotteryType.DJSSC)
            {
                return "27";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "26";
            }
            if (pType == ConfigurationStatus.LotteryType.JNDSSC)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.XJPSSC)
            {
                return "25";
            }
            if (pType == ConfigurationStatus.LotteryType.WX15F)
            {
                return "28";
            }
            if (pType == ConfigurationStatus.LotteryType.WXFFC)
            {
                return "10";
            }
            if (pType == ConfigurationStatus.LotteryType.WX3FC)
            {
                return "13";
            }
            if (pType == ConfigurationStatus.LotteryType.WX5FC)
            {
                return "12";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.WX11X5)
            {
                return "14";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "18";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/ticket/ticketmod/openhistory.json");

        public override string GetIndexLine() => 
            (this.GetLine() + "/index.html");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login.html");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            "";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            int num;
            string str3;
            int num2;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        string pStr = pNumberList[num];
                        str3 = CommFunc.Join(pStr, "+", -1);
                        list.Add(str3);
                    }
                    str = CommFunc.Join(list, "%2C").Replace("*", "-");
                }
                else if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num2 = AppInfo.FiveDic[ch.ToString()];
                    list = new List<string>();
                    for (num = 0; num < 5; num++)
                    {
                        str3 = "*";
                        if (num == num2)
                        {
                            str3 = CommFunc.Join(pNumberList, "+");
                        }
                        list.Add(str3);
                    }
                    str = CommFunc.Join(list, "%2C").Replace("*", "-");
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "%2C");
                }
                if (CommFunc.CheckPlayIsRXDS(playName))
                {
                    str = this.GetRXWZString(pRXWZ) + str;
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                return HttpUtility.UrlEncode(str);
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                str = CommFunc.Join(pNumberList, ",");
            }
            else if (CommFunc.CheckPlayIsFS(playName))
            {
                str = CommFunc.Join(pNumberList, ",").Replace("*", "-");
            }
            else
            {
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
                    str3 = "*";
                    if (num == num2)
                    {
                        str3 = CommFunc.Join(pNumberList, " ");
                    }
                    list.Add(str3);
                }
                str = CommFunc.Join(list, ",").Replace("*", "-");
                if (playName == "猜冠军猜冠军")
                {
                    str = str.Replace(" ", ",");
                }
            }
            return HttpUtility.UrlEncode(str);
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    str = "030102";
                }
                else if (playName == "前三直选复式")
                {
                    str = "030101";
                }
                else if (playName == "前三组三复式")
                {
                    str = "030201";
                }
                else if (playName == "前三组六复式")
                {
                    str = "030202";
                }
                else if (playName == "后三直选单式")
                {
                    str = "050102";
                }
                else if (playName == "后三直选复式")
                {
                    str = "050101";
                }
                else if (playName == "后三组三复式")
                {
                    str = "050201";
                }
                else if (playName == "后三组六复式")
                {
                    str = "050202";
                }
                else if (playName == "中三直选单式")
                {
                    str = "040102";
                }
                else if (playName == "中三直选复式")
                {
                    str = "040101";
                }
                else if (playName == "中三组三复式")
                {
                    str = "040201";
                }
                else if (playName == "中三组六复式")
                {
                    str = "040202";
                }
                else if (playName == "前二直选单式")
                {
                    str = "060102";
                }
                else if (playName == "前二直选复式")
                {
                    str = "060101";
                }
                else if (playName == "后二直选单式")
                {
                    str = "060106";
                }
                else if (playName == "后二直选复式")
                {
                    str = "060105";
                }
                else if (playName == "后四直选单式")
                {
                    str = "020102";
                }
                else if (playName == "后四直选复式")
                {
                    str = "020101";
                }
                else if (playName == "五星直选单式")
                {
                    str = "010102";
                }
                else if (playName == "五星直选复式")
                {
                    str = "010101";
                }
                else if (playName == "任三直选单式")
                {
                    str = "100102";
                }
                else if (playName == "任三直选复式")
                {
                    str = "100101";
                }
                else if (playName == "任三组三复式")
                {
                    str = "100201";
                }
                else if (playName == "任三组六复式")
                {
                    str = "100203";
                }
                else if (playName == "任二直选单式")
                {
                    str = "090102";
                }
                else if (playName == "任二直选复式")
                {
                    str = "090101";
                }
                else if (playName == "任四直选单式")
                {
                    str = "110102";
                }
                else if (playName == "任四直选复式")
                {
                    str = "110101";
                }
                else if (playName.Contains("定位胆"))
                {
                    str = "070101";
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    str = "010102";
                }
                else if (playName == "前二直选单式")
                {
                    str = "020102";
                }
                else if (playName == "任选复式一中一")
                {
                    str = "050101";
                }
                else if (playName == "任选复式二中二")
                {
                    str = "050102";
                }
                else if (playName == "任选复式三中三")
                {
                    str = "050103";
                }
                else if (playName == "任选复式四中四")
                {
                    str = "050104";
                }
                else if (playName == "任选复式五中五")
                {
                    str = "050105";
                }
                else if (playName == "任选单式一中一")
                {
                    str = "060101";
                }
                else if (playName == "任选单式二中二")
                {
                    str = "060102";
                }
                else if (playName == "任选单式三中三")
                {
                    str = "060103";
                }
                else if (playName == "任选单式四中四")
                {
                    str = "060104";
                }
                else if (playName == "任选单式五中五")
                {
                    str = "060105";
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    str = "030102";
                }
                else if (playName == "猜前三复式")
                {
                    str = "030101";
                }
                else if (playName == "猜前二单式")
                {
                    str = "020102";
                }
                else if (playName == "猜前二复式")
                {
                    str = "020101";
                }
                else if (playName == "猜前四单式")
                {
                    str = "040102";
                }
                else if (playName == "猜前四复式")
                {
                    str = "040101";
                }
                else if (playName == "猜前五单式")
                {
                    str = "050102";
                }
                else if (playName == "猜前五复式")
                {
                    str = "050101";
                }
                else if (playName == "猜冠军猜冠军")
                {
                    str = "010101";
                }
                else if (playName.Contains("定位胆"))
                {
                    str = "060101";
                }
            }
            return (this.GetBetsLotteryID(pType) + str);
        }

        public override string GetPTHint(string pResponseText)
        {
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            return CommFunc.GetIndexString(pResponseText, "\"msg\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/acct/login/dologout.json");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ) => 
            (CommFunc.Join(pRXWZ, "%2C") + "|");

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.Token == "")
                {
                    string cookieInternal = HttpHelper.GetCookieInternal(this.GetUrlLine());
                    base.Token = CommFunc.GetIndexString(cookieInternal, "store.body.appstorage.token=", ";", 0);
                }
                string pUrl = this.GetLine() + "/ticket/ticketmod/ticketinfo.json";
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pData = $"token={base.Token}&ticketId={this.GetBetsLotteryID(pType)}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    string pExpect = CommFunc.GetIndexString(pResponsetext, "\"planId\":\"", "\"", 0);
                    switch (CommFunc.GetLotteryID(pType))
                    {
                        case "XJSSC":
                        case "SD11X5":
                        case "GD11X5":
                        case "JX11X5":
                            pExpect = pExpect.Replace("-", "-0");
                            break;
                    }
                    base.Expect = CommFunc.ConvertExpect(pExpect, pType);
                }
            }
            catch
            {
            }
        }

        public bool InputWeb(string pID, string pW, ref string pHint)
        {
            bool flag = false;
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLine() + "/acct/login/dologin.json";
            string pResponsetext = "";
            string jScript = DateTime.Now.ToOADate().ToString();
            jScript = VerifyCodeAPI.GetJScript($"encryptSha('{jScript}')", base.PTID);
            string str6 = HttpUtility.UrlEncode(pW);
            string pData = $"flag=login&username={pID}&loginpass={str6}&validcode=&validc={""}&Submit=json";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("\"sError\":0");
            if (!flag)
            {
                pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "\"sMsg\":\"", "\"", 0).Replace(",", ""));
                if (pHint.Contains("验证码"))
                {
                    pHint = "";
                    return this.InputWeb(pID, pW, ref pHint);
                }
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext.Contains("无限娱乐");
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            string pData = $"token={base.Token}";
            HttpHelper.GetResponse(ref pResponsetext, quitPTLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
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

