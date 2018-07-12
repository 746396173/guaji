namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web;

    public class YL2028 : PTBase
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
                            string format = "\"lotteryId\":{0},\"codes\":\"{1}\",\"num\":{2},\"method\":\"{3}\",\"multiple\":{4},\"model\":\"{5}\",\"code\":{6},\"isAll\":{7}";
                            string prize = base.Prize;
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), num, this.GetPlayMethodID(plan.Type, plan.Play), Convert.ToInt32(plan.AutoTimes(str4, true)), plan.UnitString, prize, 1 });
                            format = "blist=" + HttpUtility.UrlEncode("[{" + format + "}]");
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, pReferer, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
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
            (pResponseText.Contains("\"message\":\"操作已成功\"") || (pResponseText == "投注成功"));

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pData = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"lotteryMoney\":", ",", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
                if (base.Prize == "")
                {
                    base.Rebate = CommFunc.GetIndexString(pResponsetext, "\"locatePoint\":", ",", 0);
                    if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                    {
                        base.Rebate = (Convert.ToDouble(base.Rebate) - 1.0).ToString();
                    }
                    base.Prize = (1700.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0)).ToString();
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/personal/GetAccountInfo");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/invest/UserBetsGeneral");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "101";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "103";
            }
            if (pType == ConfigurationStatus.LotteryType.YL2028HGSSC)
            {
                return "131";
            }
            if (pType == ConfigurationStatus.LotteryType.YL2028DJSSC)
            {
                return "132";
            }
            if (pType == ConfigurationStatus.LotteryType.YL2028ML20M)
            {
                return "133";
            }
            if (pType == ConfigurationStatus.LotteryType.YL2028DZ30M)
            {
                return "134";
            }
            if (pType == ConfigurationStatus.LotteryType.YL2028WXFFC)
            {
                return "135";
            }
            if (pType == ConfigurationStatus.LotteryType.YL2028FLP2FC)
            {
                return "136";
            }
            if (pType == ConfigurationStatus.LotteryType.YL2028MG45M)
            {
                return "137";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "138";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "206";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "201";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "202";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "204";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "205";
            }
            if (pType == ConfigurationStatus.LotteryType.SXR11X5)
            {
                return "209";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "601";
            }
            if (pType == ConfigurationStatus.LotteryType.YL2028PK10)
            {
                return "603";
            }
            if (pType == ConfigurationStatus.LotteryType.YL2028FFPK10)
            {
                str = "604";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/invest/lotteryInfo");

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/Login?type=");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            (this.GetLine() + "/invest-lottery.html");

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
                    str = CommFunc.Join(list, ",").Replace("*", "-");
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
                    str = CommFunc.Join(list, ",").Replace("*", "-");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, ",");
                    }
                }
                else if (CommFunc.CheckPlayIsRX(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else
                {
                    str = CommFunc.Join(pNumberList, " ");
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
                    return CommFunc.Join(pNumberList, ";");
                }
                return CommFunc.Join(pNumberList, ",");
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
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "dwh5" : "dwq5";
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
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                str = "cqssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                str = "xjssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.YL2028HGSSC)
            {
                str = "hgssc90";
            }
            else if (pType == ConfigurationStatus.LotteryType.YL2028DJSSC)
            {
                str = "djssc90";
            }
            else if (pType == ConfigurationStatus.LotteryType.YL2028ML20M)
            {
                str = "mlssc20";
            }
            else if (pType == ConfigurationStatus.LotteryType.YL2028DZ30M)
            {
                str = "dzssc30";
            }
            else if (pType == ConfigurationStatus.LotteryType.YL2028WXFFC)
            {
                str = "wxssc60";
            }
            else if (pType == ConfigurationStatus.LotteryType.YL2028FLP2FC)
            {
                str = "flbssc120";
            }
            else if (pType == ConfigurationStatus.LotteryType.YL2028MG45M)
            {
                str = "qlqssc45";
            }
            else if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                str = "qqffc";
            }
            else if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                str = "sd11x5";
            }
            else if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                str = "gd11x5";
            }
            else if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                str = "jx11x5";
            }
            else if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                str = "ah11x5";
            }
            else if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                str = "sh11x5";
            }
            else if (pType == ConfigurationStatus.LotteryType.SXR11X5)
            {
                str = "sxr11x5";
            }
            else if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "bjpk10";
            }
            else if (pType == ConfigurationStatus.LotteryType.YL2028PK10)
            {
                str = "dgpk10";
            }
            else if (pType == ConfigurationStatus.LotteryType.YL2028FFPK10)
            {
                str = "ftpk10";
            }
            return str.ToLower();
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/s/quit");

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
            }
            catch
            {
            }
        }

        public bool InputWeb(string pID, string pW, ref string pHint)
        {
            bool flag = false;
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLine() + "/s/login";
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string pData = $"username={pID}&password={str4}";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("\"code\":0");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "\"message\":\"", "\"", 0);
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
            return pResponsetext.Contains("登录");
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

