namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web;

    public class HDYL : PTBase
    {
        public int DXCount = 0;

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
                            List<string> list2 = CommFunc.SplitString(this.GetPlayMethodID(plan.Type, plan.Play), "-", -1);
                            string format = "(\"gameId\":{0},\"currentNumero\":\"{1}\",\"winningStop\":false,\"abandoning\":false,\"betCartAmountSum\":\"{2}\",\"chase\":false,\"device\":\"WEB\",\"orderType\":{3},\"quickChase\":false,\"prizeModeId\":{4},\"bettingSlipString\":\"{5}~{6}~{7}~{8}~{9}~{10}~\")";
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), plan.AutoTotalMoney(str4, true), "1", "1", this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), list2[0], plan.Unit, prize, Convert.ToInt32(plan.AutoTimes(str4, true)), list2[1] }).Replace("(", "{").Replace(")", "}");
                            HttpHelper.GetResponse1(ref pResponsetext, betsLine, "POST", format, lotteryLine, base.BetsTime2, "UTF-8", true);
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
            (pResponseText.Contains("\"orderNumber\":\"") || (pResponseText == "投注成功"));

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                if (!AppInfo.PTInfo.PTIsBreak)
                {
                    string accountsMemLine = this.GetAccountsMemLine(pType);
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    HttpHelper.GetResponse1(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                    if (this.CheckBreakConnect(pResponsetext) || (pResponsetext == ""))
                    {
                        this.DXCount++;
                        if (this.DXCount >= 3)
                        {
                            this.DXCount = 0;
                            AppInfo.PTInfo.PTIsBreak = true;
                            return;
                        }
                    }
                    string str4 = CommFunc.GetIndexString(pResponsetext, "\"availBalance\":", ",", pResponsetext.IndexOf("\"accountName\":\"LOTT\""));
                    AppInfo.Account.BankBalance = Convert.ToDouble(str4);
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/wps/wallets/balance?_={DateTime.Now.ToOADate()}";

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            switch (iD)
            {
                case "SD11X5":
                case "GD11X5":
                case "JX11X5":
                    return str2.Replace("-0", "-");

                case "TXFFC":
                case "XYFTPK10":
                    str2 = str2.Replace("-", "");
                    break;
            }
            return str2;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/lotto/lgw/orders/betting";

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "9";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "10";
            }
            if (pType == ConfigurationStatus.LotteryType.HLJSSC)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "307";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "17";
            }
            if (pType == ConfigurationStatus.LotteryType.JNDSSC)
            {
                return "24";
            }
            if (pType == ConfigurationStatus.LotteryType.HDYLFFC)
            {
                return "1003";
            }
            if (pType == ConfigurationStatus.LotteryType.HDYL2FC)
            {
                return "1005";
            }
            if (pType == ConfigurationStatus.LotteryType.HDYL5FC)
            {
                return "1006";
            }
            if (pType == ConfigurationStatus.LotteryType.HDYLASKFFC)
            {
                return "1015";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "13";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.HDYLFF11X5)
            {
                return "1007";
            }
            if (pType == ConfigurationStatus.LotteryType.HDYL2F11X5)
            {
                return "1008";
            }
            if (pType == ConfigurationStatus.LotteryType.HDYL5F11X5)
            {
                return "1009";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "20";
            }
            if (pType == ConfigurationStatus.LotteryType.XYFTPK10)
            {
                return "22";
            }
            if (pType == ConfigurationStatus.LotteryType.HDYLFFPK10)
            {
                return "1011";
            }
            if (pType == ConfigurationStatus.LotteryType.HDYLFFFT)
            {
                str = "1010";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/lotto/lgw/draw/{this.GetBetsLotteryID(pType)}?page=0&size=10";

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/lotto/t1/games/{this.GetBetsLotteryID(pType)}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            string str = "";
            ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(playName);
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                List<string> list5;
                int num3;
                string str4;
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    return CommFunc.Join(pNumberList, "_").Replace("*", "#");
                }
                if (playName.Contains("定位胆"))
                {
                    return CommFunc.Join(pNumberList).Replace("*", "");
                }
                if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList);
                    }
                    if (CommFunc.CheckPlayIsRX(playName))
                    {
                        str = this.GetRXWZString(pRXWZ) + str;
                    }
                    return str;
                }
                if (CommFunc.CheckPlayIsRX(playName))
                {
                    int count = playInfo.IndexList.Count;
                    List<string> list2 = CommFunc.GetCombinations(CommFunc.ConvertIntToStrList(pRXWZ), count, "");
                    List<string> pList = new List<string>();
                    for (int i = 0; i < list2.Count; i++)
                    {
                        List<int> list4 = CommFunc.ConvertSameListInt(list2[i]);
                        list5 = new List<string>();
                        num3 = 0;
                        while (num3 < pNumberList.Count)
                        {
                            string str2 = pNumberList[num3];
                            List<string> list6 = new List<string>();
                            int num4 = 0;
                            for (int j = 0; j < 5; j++)
                            {
                                string str3 = "#";
                                if (list4.Contains(j))
                                {
                                    str3 = str2[num4++].ToString();
                                }
                                list6.Add(str3);
                            }
                            str4 = CommFunc.Join(list6, "_");
                            list5.Add(str4);
                            num3++;
                        }
                        string item = CommFunc.Join(list5, ",");
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, ",");
                }
                list5 = new List<string>();
                for (num3 = 0; num3 < pNumberList.Count; num3++)
                {
                    str4 = pNumberList[num3];
                    str4 = CommFunc.Join(str4, "_", -1);
                    list5.Add(str4);
                }
                return CommFunc.Join(list5, ",");
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, ",").Replace(" ", "_");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, "-");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                return CommFunc.Join(pNumberList, ",").Replace(" ", "_");
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                return CommFunc.Join(pNumberList, "_").Replace(" ", "-").Replace("*", "");
            }
            return CommFunc.Join(pNumberList, "-").Replace("*", "");
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            int playNum;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "2~87-2";
                }
                if (playName == "前三直选复式")
                {
                    return "1~40-2";
                }
                if (playName == "前三组三复式")
                {
                    return "1~42-";
                }
                if (playName == "前三组六复式")
                {
                    return "1~43-";
                }
                if (playName == "后三直选单式")
                {
                    return "2~91-";
                }
                if (playName == "后三直选复式")
                {
                    return "1~21-";
                }
                if (playName == "后三组三复式")
                {
                    return "1~23-";
                }
                if (playName == "后三组六复式")
                {
                    return "1~24-";
                }
                if (playName == "中三直选单式")
                {
                    return "2~89-1";
                }
                if (playName == "中三直选复式")
                {
                    return "1~50-1";
                }
                if (playName == "中三组三复式")
                {
                    return "1~52-";
                }
                if (playName == "中三组六复式")
                {
                    return "1~53-";
                }
                if (playName == "前二直选单式")
                {
                    return "2~93-3";
                }
                if (playName == "前二直选复式")
                {
                    return "1~34-3";
                }
                if (playName == "后二直选单式")
                {
                    return "2~95-";
                }
                if (playName == "后二直选复式")
                {
                    return "1~15-";
                }
                if (playName == "前四直选单式")
                {
                    return "2~83-1";
                }
                if (playName == "前四直选复式")
                {
                    return "1~48-1";
                }
                if (playName == "后四直选单式")
                {
                    return "2~85-";
                }
                if (playName == "后四直选复式")
                {
                    return "1~29-";
                }
                if (playName == "五星直选单式")
                {
                    return "2~80-";
                }
                if (playName == "五星直选复式")
                {
                    return "1~31-";
                }
                if (playName == "任三直选单式")
                {
                    return "2~98-";
                }
                if (playName == "任三直选复式")
                {
                    return "1~60-";
                }
                if (playName == "任三组三复式")
                {
                    return "1~179-";
                }
                if (playName == "任三组六复式")
                {
                    return "1~180-";
                }
                if (playName == "任二直选单式")
                {
                    return "2~97-";
                }
                if (playName == "任二直选复式")
                {
                    return "1~59-";
                }
                if (playName == "任四直选单式")
                {
                    return "2~99-";
                }
                if (playName == "任四直选复式")
                {
                    return "1~61-";
                }
                if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    playNum = 4 - AppInfo.FiveDic[ch.ToString()];
                    str = $"1~14-{playNum}";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "2~107-";
                }
                if (playName == "前二直选单式")
                {
                    return "2~109-";
                }
                if (playName == "任选复式一中一")
                {
                    return "1~908-";
                }
                if (playName == "任选复式二中二")
                {
                    return "1~909-";
                }
                if (playName == "任选复式三中三")
                {
                    return "1~910-";
                }
                if (playName == "任选复式四中四")
                {
                    return "1~911-";
                }
                if (playName == "任选复式五中五")
                {
                    return "";
                }
                if (playName == "任选单式一中一")
                {
                    return "";
                }
                if (playName == "任选单式二中二")
                {
                    return "2~100-";
                }
                if (playName == "任选单式三中三")
                {
                    return "2~101-";
                }
                if (playName == "任选单式四中四")
                {
                    return "2~102-";
                }
                if (playName == "任选单式五中五")
                {
                    str = "";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "2~1733-";
                }
                if (playName == "猜前三复式")
                {
                    return "1~1459-";
                }
                if (playName == "猜前二单式")
                {
                    return "2~1732-";
                }
                if (playName == "猜前二复式")
                {
                    return "1~1458-";
                }
                if (playName == "猜前四单式")
                {
                    return "2~1734-";
                }
                if (playName == "猜前四复式")
                {
                    return "1~1460-";
                }
                if (playName == "猜前五单式")
                {
                    return "2~1735-";
                }
                if (playName == "猜前五复式")
                {
                    return "1~1461-";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "1~1457-";
                }
                if (playName.Contains("定位胆"))
                {
                    playNum = CommFunc.GetPlayNum(playName);
                    string str2 = (playNum > 5) ? "1463" : "1462";
                    int num2 = playNum - 1;
                    str = $"1~{str2}-{num2}";
                }
            }
            return str;
        }

        public override string GetPTHint(string pResponseText)
        {
            string str = "";
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            str = CommFunc.GetIndexString(pResponseText, "\"message\":\"", "\"", 0);
            if (str.Contains("balance not enough"))
            {
                str = "余额不足";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            string str = "";
            List<int> pList = new List<int>();
            for (int i = 0; i < pRXWZ.Count; i++)
            {
                int item = pRXWZ[i] + 1;
                pList.Add(item);
            }
            str = CommFunc.Join<int>(pList);
            return $"{str}@";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string pUrl = $"{this.GetLine()}/lotto/lgw/numeros/near?gameId={this.GetBetsLotteryID(pType)}";
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"numero\":\"", "\"", pResponsetext.IndexOf("\"currentNumero\""));
                    base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
                    if (base.Prize == "")
                    {
                        pUrl = this.GetLine() + "/lotto/lgw/customers/series";
                        indexLine = this.GetIndexLine();
                        pResponsetext = "";
                        HttpHelper.GetResponse1(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                        if (pResponsetext != "")
                        {
                            string str4 = "SSC";
                            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                            {
                                str4 = "11X5";
                            }
                            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                            {
                                str4 = "PK10";
                            }
                            str4 = $"gameGroupCode:{str4}";
                            base.Prize = CommFunc.GetIndexString(pResponsetext, "\"maxSeries\":", ",", pResponsetext.IndexOf(str4));
                        }
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
            string pUrl = this.GetLine() + "/wps/session/login/unsecure";
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string pData = $"(username: {pID},password: {str4},ipAddress: )".Replace("(", "{").Replace(")", "}");
            HttpHelper.GetResponse1(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("\"success\":true");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "<div class=\"error \">", "</div>", 0).Replace("\r\n", "").Trim();
                return flag;
            }
            HttpHelper.BetsTokenKey = "Authorization";
            HttpHelper.BetsTokenValue = CommFunc.GetIndexString(pResponsetext, "\"token\":\"", "\"", 0);
            return flag;
        }

        public override void QuitPT()
        {
        }

        public override bool WebLoginMain(string pID, string pW, ref string pHint)
        {
            this.DXCount = 0;
            HttpHelper.BetsTokenKey = HttpHelper.BetsTokenValue = "";
            if (!this.InputWeb(pID, pW, ref pHint))
            {
                return false;
            }
            return true;
        }
    }
}

