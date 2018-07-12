namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class BMYX : PTBase
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
                            int num2 = 2;
                            string format = "gameId={0}&isTrace=0&traceWinStop=1&traceStopValue=1&balls%5B0%5D%5BjsId%5D={1}&balls%5B0%5D%5BwayId%5D={2}&balls%5B0%5D%5Bball%5D={3}&balls%5B0%5D%5BviewBalls%5D=&balls%5B0%5D%5Bnum%5D={4}&balls%5B0%5D%5Btype%5D={5}&balls%5B0%5D%5BonePrice%5D={6}&balls%5B0%5D%5Bprize_group%5D={7}&balls%5B0%5D%5Bmoneyunit%5D={8}&balls%5B0%5D%5Bmultiple%5D={9}&orders%5B{10}%5D={11}&amount={12}&_token={13}&is_encoded=0&client_key={14}";
                            string prize = this.GetPrize(plan.Type, "前三直选单式");
                            prize = this.GetMaxPrize(prize, 1950.0);
                            string str8 = HttpUtility.UrlEncode("zjhtyb2016011217011000xte&ewr~yko!d");
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), "1", this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), num, this.GetPlayString(plan.Play), num2, prize, plan.Money / ((double) num2), Convert.ToInt32(plan.AutoTimes(str4, true)), this.GetBetsExpect(plan.CurrentExpect, ""), "1", plan.AutoTotalMoney(str4, true), base.Token, str8 });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, 0x2710, "UTF-8", true);
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
            (pResponseText.Contains("success") || (pResponseText == "投注成功"));

        public override void CountPrizeDic(string pResponseText)
        {
            base.PrizeDic.Clear();
            List<string> list = CommFunc.SplitString(CommFunc.GetIndexString(pResponseText, "gameMethods", "uploadPath", 0), "},{", -1);
            foreach (string str2 in list)
            {
                string str3 = CommFunc.GetIndexString(str2, "\"series_way_id\":", ",", 0);
                string str4 = CommFunc.GetIndexString(str2, "\"prize\":\"", "\"", 0);
                base.PrizeDic[str3] = str4;
            }
        }

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"data\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/users/user-account-info");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "") => 
            pExpect.Replace("-", "");

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/bets/bet/{this.GetBetsLotteryID(pType)}";

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
            if (pType == ConfigurationStatus.LotteryType.BM1FC)
            {
                return "23";
            }
            if (pType == ConfigurationStatus.LotteryType.BM2FC)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.BM5FC)
            {
                str = "24";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/bets/bet/{0}?time={1}");

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/auth/signin");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/bets/bet/{this.GetBetsLotteryID(pType)}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            if (CommFunc.CheckPlayIsFS(playName))
            {
                return CommFunc.Join(pNumberList, "|").Replace("*", "");
            }
            if (playName.Contains("定位胆"))
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
                return CommFunc.Join(pList, "|").Replace("*", "");
            }
            if (CommFunc.CheckPlayIsZuX(playName))
            {
                if (playName.Contains("复式"))
                {
                    return CommFunc.Join(pNumberList);
                }
                return CommFunc.Join(pNumberList, "|");
            }
            return CommFunc.Join(pNumberList, "|");
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
            if (playName == "前三组三单式")
            {
                return "2";
            }
            if (playName == "前三组六复式")
            {
                return "17";
            }
            if (playName == "前三组六单式")
            {
                return "3";
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
            if (playName == "后三组三单式")
            {
                return "9";
            }
            if (playName == "后三组六复式")
            {
                return "50";
            }
            if (playName == "后三组六单式")
            {
                return "10";
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

        public override string GetPrize(ConfigurationStatus.LotteryType pType, string playName)
        {
            int num = 0;
            while (true)
            {
                if ((num >= 3) || (base.PrizeDic.Count > 0))
                {
                    string playMethodID = this.GetPlayMethodID(pType, playName);
                    if (!base.PrizeDic.ContainsKey(playMethodID))
                    {
                        return "";
                    }
                    return base.PrizeDic[playMethodID];
                }
                this.GetSite(pType, "");
                num++;
            }
        }

        public override string GetPTHint(string pResponseText)
        {
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"Msg\":\"", "\"", 0));
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "ssccq";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "sscxj";
            }
            if (pType == ConfigurationStatus.LotteryType.BDFFC)
            {
                return "ssccs";
            }
            if (pType == ConfigurationStatus.LotteryType.BD2FC)
            {
                str = "sscbf";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/auth/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string pResponseText = this.LoginLotteryWeb(pType, "");
                this.CountPrizeDic(pResponseText);
                string str2 = CommFunc.GetIndexString(pResponseText, "\"_token\":\"", "\"", 0);
                if (pResponseText != "")
                {
                    if (str2 != "")
                    {
                        base.Token = str2;
                    }
                    base.Expect = "20" + CommFunc.GetIndexString(pResponseText, "\"currentNumber\":\"", "\"", 0);
                    base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
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
            string pUrl = this.GetLoginLine();
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string str5 = HttpUtility.UrlEncode(CommFunc.WebMD51(CommFunc.WebMD51(CommFunc.WebMD51(pID + pW))));
            string pData = $"_token={base.Token}&_random={base.Random}&username={pID}&={str4}&password={str5}";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("管理中心");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "\"login-error \">", "<", 0).Replace("\r\n", "").Trim();
                if (pHint == "")
                {
                    pHint = "";
                    CommFunc.RunCmd("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 2");
                    Thread.Sleep(0x1388);
                    return false;
                }
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
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("博猫游戏");
            if (flag)
            {
                base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("_token"));
                base.Random = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("_random"));
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

