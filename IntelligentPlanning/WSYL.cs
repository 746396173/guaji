namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class WSYL : PTBase
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
                            string format = "text=%5B%7B%22lottery%22%3A%22{0}%22%2C%22issue%22%3A%22%22%2C%22method%22%3A%22{1}%22%2C%22content%22%3A%22{2}%22%2C%22model%22%3A%22{3}%22%2C%22multiple%22%3A{4}%2C%22code%22%3A{5}%2C%22issue%22%3A%22{6}%22%2C%22compress%22%3Afalse%7D%5D";
                            string prize = base.Prize;
                            format = string.Format(format, new object[] { this.GetPTLotteryName(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), plan.UnitString, Convert.ToInt32(plan.AutoTimes(str4, true)), prize, this.GetBetsExpect(plan.CurrentExpect, "") });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, pReferer, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
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
            pHint.Contains("登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"message\":\"请求成功\"") || (pResponseText == "投注成功"));

        private bool CheckVerifyCode(string pVerifyCode)
        {
            if (pVerifyCode.Length != 4)
            {
                return false;
            }
            return true;
        }

        public override string ConvertLotteryExpectChar(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if (((((pType != ConfigurationStatus.LotteryType.WSHS15F) && (pType != ConfigurationStatus.LotteryType.WSFS15F)) && ((pType != ConfigurationStatus.LotteryType.WSXDL15F) && (pType != ConfigurationStatus.LotteryType.WSNDJSSC))) && ((pType != ConfigurationStatus.LotteryType.WSELSSSC) && (pType != ConfigurationStatus.LotteryType.WS120MPK10))) && (pType != ConfigurationStatus.LotteryType.WS180MPK10))
            {
                return str;
            }
            if (pIsBets)
            {
                return str.Replace("-", "-0");
            }
            return str.Replace("-0", "-");
        }

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string loginLine = this.GetLoginLine();
                string pResponsetext = "";
                string pData = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                pResponsetext = pResponsetext.Substring(pResponsetext.IndexOf("username"));
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"availableBalance\":", ",", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
                if (base.Prize == "")
                {
                    base.Prize = CommFunc.GetIndexString(pResponsetext, "\"code\":", ",", 0);
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/api/web-ajax/init-page");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            return this.ConvertLotteryExpectChar(AppInfo.Current.Lottery.Type, str2, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/api/game-lottery/add-order");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/api/game-lottery/static-open-code");

        public override string GetIndexLine() => 
            (this.GetLine() + "/index.html");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login.html");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            (this.GetLine() + "/lottery.html");

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            int num;
            int num2;
            string str2;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    if (CommFunc.CheckPlayIsRXFS(playName))
                    {
                        str = CommFunc.Join(pNumberList, "%2C").Replace("*", "-");
                    }
                    else
                    {
                        ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(playName);
                        list = new List<string>();
                        num = 0;
                        for (num2 = 0; num2 < 5; num2++)
                        {
                            str2 = "*";
                            if (playInfo.IndexList.Contains(num2 + 1))
                            {
                                str2 = pNumberList[num++];
                            }
                            list.Add(str2);
                        }
                        str = CommFunc.Join(list, "%2C").Replace("*", "-");
                    }
                }
                else if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num = AppInfo.FiveDic[ch.ToString()];
                    list = new List<string>();
                    for (num2 = 0; num2 < 5; num2++)
                    {
                        str2 = "*";
                        if (num2 == num)
                        {
                            str2 = CommFunc.Join(pNumberList);
                        }
                        list.Add(str2);
                    }
                    str = CommFunc.Join(list, "%2C").Replace("*", "-");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, "%2C");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "+");
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
                    str = CommFunc.Join(pNumberList, ";");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                return HttpUtility.UrlEncode(str);
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, ";");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    return CommFunc.Join(pNumberList, ",").Replace("*", "");
                }
                num = -1;
                if (playName.Contains("冠军"))
                {
                    num = 0;
                }
                else if (playName.Contains("亚军"))
                {
                    num = 1;
                }
                else
                {
                    num = CommFunc.GetPlayNum(playName) - 1;
                }
                if (num >= 5)
                {
                    num -= 5;
                }
                list = new List<string>();
                int num3 = (playName == "猜冠军猜冠军") ? 1 : 5;
                for (num2 = 0; num2 < num3; num2++)
                {
                    str2 = "*";
                    if (num2 == num)
                    {
                        str2 = CommFunc.Join(pNumberList, " ");
                    }
                    list.Add(str2);
                }
                str = CommFunc.Join(list, ",").Replace("*", "-");
                if (playName == "猜冠军猜冠军")
                {
                    str = str.Replace(" ", ",");
                }
            }
            return str;
        }

        public override string GetOpenTime() => 
            (this.GetLine() + "/api/game-lottery/static-open-time");

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "sxzhixdsq";
                }
                if (playName == "前三直选复式")
                {
                    return "sxzhixfsq";
                }
                if (playName == "前三组三复式")
                {
                    return "sxzuxzsq";
                }
                if (playName == "前三组六复式")
                {
                    return "sxzuxzlq";
                }
                if (playName == "后三直选单式")
                {
                    return "sxzhixdsh";
                }
                if (playName == "后三直选复式")
                {
                    return "sxzhixfsh";
                }
                if (playName == "后三组三复式")
                {
                    return "sxzuxzsh";
                }
                if (playName == "后三组六复式")
                {
                    return "sxzuxzlh";
                }
                if (playName == "中三直选单式")
                {
                    return "sxzhixdsz";
                }
                if (playName == "中三直选复式")
                {
                    return "sxzhixfsz";
                }
                if (playName == "中三组三复式")
                {
                    return "sxzuxzsz";
                }
                if (playName == "中三组六复式")
                {
                    return "sxzuxzlz";
                }
                if (playName == "前二直选单式")
                {
                    return "exzhixdsq";
                }
                if (playName == "前二直选复式")
                {
                    return "exzhixfsq";
                }
                if (playName == "后二直选单式")
                {
                    return "exzhixdsh";
                }
                if (playName == "后二直选复式")
                {
                    return "exzhixfsh";
                }
                if (playName == "前四直选单式")
                {
                    return "sixzhixdsq";
                }
                if (playName == "前四直选复式")
                {
                    return "sixzhixfsq";
                }
                if (playName == "后四直选单式")
                {
                    return "sixzhixdsh";
                }
                if (playName == "后四直选复式")
                {
                    return "sixzhixfsh";
                }
                if (playName == "五星直选单式")
                {
                    return "wxzhixds";
                }
                if (playName == "五星直选复式")
                {
                    return "wxzhixfs";
                }
                if (playName == "任三直选单式")
                {
                    return "rx3ds";
                }
                if (playName == "任三直选复式")
                {
                    return "rx3fs";
                }
                if (playName == "任三组三复式")
                {
                    return "rx3z3";
                }
                if (playName == "任三组六复式")
                {
                    return "rx3z6";
                }
                if (playName == "任二直选单式")
                {
                    return "rx2ds";
                }
                if (playName == "任二直选复式")
                {
                    return "rx2fs";
                }
                if (playName == "任四直选单式")
                {
                    return "rx4ds";
                }
                if (playName == "任四直选复式")
                {
                    return "rx4fs";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "dw";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "sanmzhixdsq";
                }
                if (playName == "前二直选单式")
                {
                    return "ermzhixdsq";
                }
                if (playName == "任选复式一中一")
                {
                    return "rx1fs";
                }
                if (playName == "任选复式二中二")
                {
                    return "rx2fs";
                }
                if (playName == "任选复式三中三")
                {
                    return "rx3fs";
                }
                if (playName == "任选复式四中四")
                {
                    return "rx4fs";
                }
                if (playName == "任选复式五中五")
                {
                    return "rx5fs";
                }
                if (playName == "任选单式一中一")
                {
                    return "rx1ds";
                }
                if (playName == "任选单式二中二")
                {
                    return "rx2ds";
                }
                if (playName == "任选单式三中三")
                {
                    return "rx3ds";
                }
                if (playName == "任选单式四中四")
                {
                    return "rx4ds";
                }
                if (playName == "任选单式五中五")
                {
                    str = "rx5ds";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "qiansanzxds";
                }
                if (playName == "猜前三复式")
                {
                    return "qiansanzxfs";
                }
                if (playName == "猜前二单式")
                {
                    return "qianerzxds";
                }
                if (playName == "猜前二复式")
                {
                    return "qianerzxfs";
                }
                if (playName == "猜前四单式")
                {
                    return "";
                }
                if (playName == "猜前四复式")
                {
                    return "";
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
                    return "qianyi";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "dwhou" : "dwqian";
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
            return CommFunc.GetIndexString(pResponseText, "\"message\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = CommFunc.GetLotteryID(pType).ToLower();
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "cqssc";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "xjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "bjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.WS30M)
            {
                return "t1s30";
            }
            if (pType == ConfigurationStatus.LotteryType.WSBLS60M)
            {
                return "t1s60";
            }
            if (pType == ConfigurationStatus.LotteryType.WSHS15F)
            {
                return "t1s90";
            }
            if (pType == ConfigurationStatus.LotteryType.WSFS15F)
            {
                return "t1s90a";
            }
            if (pType == ConfigurationStatus.LotteryType.WSXDL15F)
            {
                return "t1s90b";
            }
            if (pType == ConfigurationStatus.LotteryType.WSNDJSSC)
            {
                return "t1s90c";
            }
            if (pType == ConfigurationStatus.LotteryType.WSELSSSC)
            {
                return "t1s90d";
            }
            if (pType == ConfigurationStatus.LotteryType.WSQQFFC)
            {
                return "t1s60a";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "qqmin";
            }
            if (pType == ConfigurationStatus.LotteryType.WSFLP15C)
            {
                return "phssc";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "gd11X5";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "jx11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "sd11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "ah11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "sh11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.WS11X5)
            {
                return "t2s90";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "bjpk10";
            }
            if (pType == ConfigurationStatus.LotteryType.WS120MPK10)
            {
                return "t6s120";
            }
            if (pType == ConfigurationStatus.LotteryType.WS180MPK10)
            {
                str = "t6s180";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/api/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            List<string> pList = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string item = pRXWZ.Contains(i) ? "√" : "-";
                pList.Add(item);
            }
            return $"[{CommFunc.Join(pList, ",")}]";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string openTime = this.GetOpenTime();
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                string pData = $"name={this.GetPTLotteryName(pType)}";
                HttpHelper.GetResponse(ref pResponsetext, openTime, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"issue\":\"", "\"", 0);
                    base.Expect = this.ConvertLotteryExpectChar(pType, base.Expect, false);
                    base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
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
                string str2 = $"/api/utils/login-security-code?={DateTime.Now.ToOADate()}";
                string pUrl = this.GetLine() + str2;
                File.Delete(pVerifyCodeFile);
                Bitmap bitmap = new Bitmap(HttpHelper.GetResponseImage(pUrl, "", "GET", "", 0x1770, "UTF-8", true));
                bitmap.Save(pVerifyCodeFile);
                bitmap.Dispose();
                while (!File.Exists(pVerifyCodeFile))
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
            string webVerifyCode = this.GetWebVerifyCode(AutoBetsWindow.VerifyCodeFile);
            if (webVerifyCode != "")
            {
                string loginLine = this.GetLoginLine();
                string pUrl = this.GetLine() + "/api/web-login";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"username={pID}&password={str5}&token={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
                flag = pResponsetext.Contains("\"message\":\"请求成功\"");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"message\":\"", "\"", 0);
                }
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, base.BetsTime2, "UTF-8", true);
            return pResponsetext.Contains("用户登录");
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
            if (!this.InputWeb(pID, pW, ref pHint))
            {
                return false;
            }
            return true;
        }
    }
}

