namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class YDYL : PTBase
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
                            string str6 = CommFunc.CheckPlayIsDS(plan.Play) ? "input" : "digital";
                            int num = plan.FNNumber(str4);
                            string prize = this.GetPrize(plan.Type, plan.Play);
                            string format = "arguments=%7B%22id%22%3A%22{0}%22%2C%22shortName%22%3A%22{1}%22%2C%22total_nums%22%3A{2}%2C%22total_money%22%3A{3}%2C%22items%22%3A%5B%7B%22rulecode%22%3A%22{4}%22%2C%22type%22%3A%22{5}%22%2C%22codes%22%3A%22{6}%22{11}%2C%22nums%22%3A{2}%2C%22times%22%3A{7}%2C%22money%22%3A{3}%2C%22mode%22%3A{8}%2C%22point%22%3A{9}%2C%22curtimes%22%3A{10}%7D%5D%2C%22trace_if%22%3A%22no%22%7D";
                            format = string.Format(format, new object[] { this.GetBetsExpect(plan.CurrentExpect, ""), this.GetPTLotteryName(plan.Type), num, plan.AutoTotalMoney(str4, true), this.GetPlayMethodID(plan.Type, plan.Play), str6, this.GetNumberList1(pTNumberList, plan.Play, null), Convert.ToInt32(plan.AutoTimes(str4, true)), plan.Money, "0", DateTime.Now.ToOADate(), this.GetRXWZString1(plan) });
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
            (pResponseText.Contains("\"msg\":\"投注成功！\"") || (pResponseText == "投注成功"));

        private bool CheckVerifyCode(string pVerifyCode)
        {
            if (pVerifyCode.Length != 4)
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
                string pData = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"data\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/user/getBalance.do");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            if (iD == "WHDJSSC")
            {
                str2 = str2.Replace("-", "");
            }
            return str2;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/ssc/bet.do");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/ssc/getOpenWinCodeHistory.do";

        public override string GetIndexLine() => 
            (this.GetLine() + "/index.html");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login.html");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/view/game/game.html?code={this.GetPTLotteryName(pType)}";

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
                        str2 = CommFunc.Join(pNumberList[num], "&", -1);
                        list.Add(str2);
                    }
                    str = CommFunc.Join(list, "|").Replace("*", "");
                }
                else if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num2 = AppInfo.FiveDic[ch.ToString()];
                    list = new List<string>();
                    for (num = 0; num < 5; num++)
                    {
                        str2 = "*";
                        if (num == num2)
                        {
                            str2 = CommFunc.Join(pNumberList, "&");
                        }
                        list.Add(str2);
                    }
                    str = CommFunc.Join(list, "|").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, "&");
                    }
                    else
                    {
                        str = CommFunc.Join(pNumberList, "&");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "&");
                }
                return HttpUtility.UrlEncode(str);
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                return str;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                str = CommFunc.Join(pNumberList, "&");
            }
            else if (CommFunc.CheckPlayIsFS(playName))
            {
                list = new List<string>();
                for (num = 0; num < pNumberList.Count; num++)
                {
                    str2 = pNumberList[num].Replace(" ", "&");
                    list.Add(str2);
                }
                str = CommFunc.Join(list, "|").Replace("*", "");
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
                if (num2 >= 5)
                {
                    num2 -= 5;
                }
                list = new List<string>();
                int num3 = (playName == "猜冠军猜冠军") ? 1 : 5;
                for (num = 0; num < num3; num++)
                {
                    str2 = "*";
                    if (num == num2)
                    {
                        str2 = CommFunc.Join(pNumberList, "&");
                    }
                    list.Add(str2);
                }
                str = CommFunc.Join(list, "|").Replace("*", "");
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
                if (playName == "前三组三单式")
                {
                    return "sxzuxzsdsq";
                }
                if (playName == "前三组六复式")
                {
                    return "sxzuxzlq";
                }
                if (playName == "前三组六单式")
                {
                    return "sxzuxzldsq";
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
                if (playName == "后三组三单式")
                {
                    return "sxzuxzsdsh";
                }
                if (playName == "后三组六复式")
                {
                    return "sxzuxzlh";
                }
                if (playName == "后三组六单式")
                {
                    return "sxzuxzldsh";
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
                if (playName == "中三组三单式")
                {
                    return "sxzuxzsdsz";
                }
                if (playName == "中三组六复式")
                {
                    return "sxzuxzlz";
                }
                if (playName == "中三组六单式")
                {
                    return "sxzuxzldsz";
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
                if (playName == "任三组三单式")
                {
                    return "rx3z3ds";
                }
                if (playName == "任三组六复式")
                {
                    return "rx3z6";
                }
                if (playName == "任三组六单式")
                {
                    return "rx3z6ds";
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
                    str = "dweid";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
                {
                    return str;
                }
                if (playName == "猜前三单式")
                {
                    return "bjpk10qian3ds";
                }
                if (playName == "猜前三复式")
                {
                    return "bjpk10qian3";
                }
                if (playName == "猜前二单式")
                {
                    return "bjpk10qian2ds";
                }
                if (playName == "猜前二复式")
                {
                    return "bjpk10qian2";
                }
                if (playName == "猜前四单式")
                {
                    return "bjpk10qian4ds";
                }
                if (playName == "猜前四复式")
                {
                    return "bjpk10dwdq";
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
                    return "bjpk10qian1";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "bjpk10dwdh" : "bjpk10dwdq";
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
            return CommFunc.GetIndexString(pResponseText, "\"msg\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = CommFunc.GetLotteryID(pType).ToLower();
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "tw5f";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "bj5f";
            }
            if (pType == ConfigurationStatus.LotteryType.WHDJSSC)
            {
                return "djssc";
            }
            if (pType == ConfigurationStatus.LotteryType.YDXXLSSC)
            {
                return "xxlssc";
            }
            if (pType == ConfigurationStatus.LotteryType.YDHGSSC)
            {
                return "hgssc";
            }
            if (pType == ConfigurationStatus.LotteryType.YDTXFFC)
            {
                return "txssc";
            }
            if (pType == ConfigurationStatus.LotteryType.YDFFC)
            {
                return "ffssc";
            }
            if (pType == ConfigurationStatus.LotteryType.YD2FC)
            {
                return "lfssc";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "bjpk10";
            }
            if (pType == ConfigurationStatus.LotteryType.YDFFPK10)
            {
                str = "ffpk10";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            $"{this.GetLine()}/ajax/ajax.aspx?oper=logout&clienttime={DateTime.Now.ToOADate()}";

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            string str = "";
            if ((pRXWZ == null) || (pRXWZ.Count <= 0))
            {
                return str;
            }
            List<string> pList = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string item = pRXWZ.Contains(i) ? "1" : "0";
                pList.Add(item);
            }
            return CommFunc.Join(pList, ",");
        }

        public string GetRXWZString1(ConfigurationStatus.SCPlan plan)
        {
            string str = "";
            if (CommFunc.CheckPlayIsRXDS(plan.Play))
            {
                str = CommFunc.Join(plan.RXWZ, "%26");
                str = $"%2C%22position%22%3A%22{str}%22";
            }
            return str;
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string str;
                string indexLine;
                string str3;
                string str4;
                if (base.Prize == "")
                {
                    str = $"{this.GetLine()}/user/getUser.do";
                    indexLine = this.GetIndexLine();
                    str3 = "";
                    str4 = "";
                    HttpHelper.GetResponse(ref str3, str, "POST", str4, indexLine, 0x2710, "UTF-8", true);
                    if (str3 != "")
                    {
                        base.Rebate = CommFunc.GetIndexString(str3, "\"pointssc\":", ",", 0);
                        base.Prize = (1800.0 + (Convert.ToDouble(base.Rebate) * 2000.0)).ToString();
                    }
                }
                str = $"{this.GetLine()}/ssc/initSscData.do";
                indexLine = this.GetIndexLine();
                str3 = "";
                str4 = $"shortName={this.GetPTLotteryName(pType)}";
                HttpHelper.GetResponse(ref str3, str, "POST", str4, indexLine, 0x2710, "UTF-8", true);
                if (str3 != "")
                {
                    base.Expect = CommFunc.GetIndexString(str3, "\"issue\":\"", "\"", str3.IndexOf("cursitem"));
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
                string str2 = $"/plus/getcode.aspx?clienttime={DateTime.Now.ToOADate()}";
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
            string loginLine = this.GetLoginLine();
            string pUrl = $"{this.GetLine()}/login/login.do";
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string pData = $"username={pID}&pwd={str4}";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime1, "UTF-8", true);
            flag = pResponsetext.Contains("\"code\":\"0\"");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "\"msg\":\"", "\"", 0);
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, base.BetsTime2, "UTF-8", true);
            return pResponsetext.Contains("万兴国际");
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

