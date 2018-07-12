namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web;

    public class FSYL : PTBase
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
                            string format = "gameId={0}&isTrace=0&traceWinStop=1&traceStopValue=1&orders%5B{1}%5D=1&amount={2}&ballstr=%5B%7B%22jsId%22%3A{3}%2C%22wayId%22%3A{4}%2C%22ball%22%3A%22{5}%22%2C%22viewBalls%22%3A%22%22%2C%22num%22%3A{6}%2C%22type%22%3A%22{7}%22%2C%22onePrice%22%3A%22{8}%22%2C%22moneyunit%22%3A%22{9}%22%2C%22multiple%22%3A{10}%2C%22prizeGroup%22%3A%22{11}%22%7D%5D&orderstr=%7B%22{1}%22%3A{12}%7D";
                            int num = plan.FNNumber(str4);
                            int num2 = 2;
                            string prize = base.Prize;
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), plan.AutoTotalMoney(str4, true), "1", this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), num, this.GetPlayString(plan.Play), num2, plan.Money / ((double) num2), Convert.ToInt32(plan.AutoTimes(str4, true)), prize, "1" });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, base.BetsTime3, "UTF-8", true);
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
            pHint.Contains("重新登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"isSuccess\" : 1") || (pResponseText == "投注成功"));

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"available\" : \"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/win/mkg/api/users/user-monetary-info?_={DateTime.Now.ToOADate()}";

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/win/mkg/api/bets/bet");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "29";
            }
            if (pType == ConfigurationStatus.LotteryType.HGSSC)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.DPSSC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.FSFFC)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.FS5FC)
            {
                str = "4";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/win/mkg/api/load-issue/{this.GetBetsLotteryID(pType)}?_={DateTime.Now.ToOADate()}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/win/home");

        public override string GetLoginLine() => 
            (this.GetLine() + "/win/auth/signin.html");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/win/mkg/bet/{this.GetBetsLotteryID(pType)}.html";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            string str = "";
            if (CommFunc.CheckPlayIsFS(playName))
            {
                str = CommFunc.Join(pNumberList, "|").Replace("*", "");
            }
            else if (playName.Contains("定位胆"))
            {
                char ch = playName[3];
                int num = AppInfo.FiveDic[ch.ToString()];
                List<string> pList = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    string item = "*";
                    if (i == num)
                    {
                        item = CommFunc.Join(pNumberList);
                    }
                    pList.Add(item);
                }
                str = CommFunc.Join(pList, "|").Replace("*", "");
            }
            else if (CommFunc.CheckPlayIsZuX(playName))
            {
                if (playName.Contains("复式"))
                {
                    str = CommFunc.Join(pNumberList);
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "|");
                }
            }
            else
            {
                str = CommFunc.Join(pNumberList, "|");
            }
            return HttpUtility.UrlEncode(str);
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (playName == "前三直选单式")
            {
                return "1";
            }
            if (playName == "前三直选复式")
            {
                return "65";
            }
            if (playName == "前三组三复式")
            {
                return "16";
            }
            if (playName == "前三组六复式")
            {
                return "17";
            }
            if (playName == "后三直选单式")
            {
                return "8";
            }
            if (playName == "后三直选复式")
            {
                return "69";
            }
            if (playName == "后三组三复式")
            {
                return "49";
            }
            if (playName == "后三组六复式")
            {
                return "50";
            }
            if (playName == "中三直选单式")
            {
                return "142";
            }
            if (playName == "中三直选复式")
            {
                return "150";
            }
            if (playName == "中三组三复式")
            {
                return "145";
            }
            if (playName == "中三组六复式")
            {
                return "146";
            }
            if (playName == "前二直选单式")
            {
                return "4";
            }
            if (playName == "前二直选复式")
            {
                return "66";
            }
            if (playName == "后二直选单式")
            {
                return "11";
            }
            if (playName == "后二直选复式")
            {
                return "70";
            }
            if (playName == "后四直选单式")
            {
                return "6";
            }
            if (playName == "后四直选复式")
            {
                return "67";
            }
            if (playName == "五星直选单式")
            {
                return "7";
            }
            if (playName == "五星直选复式")
            {
                return "68";
            }
            if (playName.Contains("定位胆"))
            {
                str = "78";
            }
            return str;
        }

        public override string GetPlayString(string playName)
        {
            string str = "";
            if (playName == "前三直选单式")
            {
                return "qiansan.zhixuan.danshi";
            }
            if (playName == "前三直选复式")
            {
                return "qiansan.zhixuan.fushi";
            }
            if (playName == "前三组三复式")
            {
                return "qiansan.zuxuan.zusan";
            }
            if (playName == "前三组三单式")
            {
                return "qiansan.zuxuan.zusandanshi";
            }
            if (playName == "前三组六复式")
            {
                return "qiansan.zuxuan.zuliu";
            }
            if (playName == "前三组六单式")
            {
                return "qiansan.zuxuan.zuliudanshi";
            }
            if (playName == "后三直选单式")
            {
                return "housan.zhixuan.danshi";
            }
            if (playName == "后三直选复式")
            {
                return "housan.zhixuan.fushi";
            }
            if (playName == "后三组三复式")
            {
                return "housan.zuxuan.zusan";
            }
            if (playName == "后三组三单式")
            {
                return "housan.zuxuan.zusandanshi";
            }
            if (playName == "后三组六复式")
            {
                return "housan.zuxuan.zuliu";
            }
            if (playName == "后三组六单式")
            {
                return "housan.zuxuan.zuliudanshi";
            }
            if (playName == "中三直选单式")
            {
                return "zhongsan.zhixuan.danshi";
            }
            if (playName == "中三直选复式")
            {
                return "zhongsan.zhixuan.fushi";
            }
            if (playName == "中三组三复式")
            {
                return "zhongsan.zuxuan.zusan";
            }
            if (playName == "中三组三单式")
            {
                return "zhongsan.zuxuan.zusandanshi";
            }
            if (playName == "中三组六复式")
            {
                return "zhongsan.zuxuan.zuliu";
            }
            if (playName == "中三组六单式")
            {
                return "zhongsan.zuxuan.zuliudanshi";
            }
            if (playName == "前二直选单式")
            {
                return "erxing.zhixuan.qianerdanshi";
            }
            if (playName == "前二直选复式")
            {
                return "erxing.zhixuan.qianerfushi";
            }
            if (playName == "后二直选单式")
            {
                return "erxing.zhixuan.houerdanshi";
            }
            if (playName == "后二直选复式")
            {
                return "erxing.zhixuan.houerfushi";
            }
            if (playName == "后四直选单式")
            {
                return "sixing.zhixuan.danshi";
            }
            if (playName == "后四直选复式")
            {
                return "sixing.zhixuan.fushi";
            }
            if (playName == "五星直选单式")
            {
                return "wuxing.zhixuan.danshi";
            }
            if (playName == "五星直选复式")
            {
                return "wuxing.zhixuan.fushi";
            }
            if (playName.Contains("定位胆"))
            {
                str = "yixing.dingweidan.fushi";
            }
            return str;
        }

        public override string GetPTHint(string pResponseText)
        {
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"tplData\" : \"", "\"", 0));
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/sso/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.Prize == "")
                {
                    string pStr = this.LoginLotteryWeb(pType, "");
                    if (pStr != "")
                    {
                        base.Prize = CommFunc.GetIndexString(pStr, "\"prize_group\":\"", "\"", 0);
                    }
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
            string str2 = CommFunc.WebMD51(pW);
            string pUrl = $"{this.GetLine()}/sso/login?way=pwd&from=portal&cn={pID}&appId=5&password={str2}&capchaCode=&_={DateTime.Now.ToOADate()}";
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("\"msg\":\"登录成功\"");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "\"msg\":\"", "\"", 0);
            }
            return flag;
        }

        public override string LoginLotteryWeb(ConfigurationStatus.LotteryType pType, string pInfo = "")
        {
            string lotteryLine = this.GetLotteryLine(pType, false);
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
            return pResponsetext;
        }

        public bool LoginWeb()
        {
            string cookieInternal = HttpHelper.GetCookieInternal(this.GetUrlLine());
            AppInfo.PTInfo.WebCookie = cookieInternal;
            HttpHelper.SaveCookies(cookieInternal, "");
            return true;
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            string pData = "";
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

