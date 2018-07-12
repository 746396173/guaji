namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class YHSG : PTBase
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
                            string str7;
                            List<string> pTNumberList = plan.GetPTNumberList(dictionary2[str4]);
                            string pResponsetext = "";
                            int num = plan.FNNumber(str4);
                            string str6 = Guid.NewGuid().ToString();
                            if (this.CheckLotteryIsVR(plan.Type))
                            {
                                str7 = "LotteryGameID={0}&IssueSerialNumber=&Bets%5B0%5D%5BBetTypeCode%5D={1}&Bets%5B0%5D%5BBetTypeName%5D=&Bets%5B0%5D%5BNumber%5D={2}&Bets%5B0%5D%5BPosition%5D={3}&Bets%5B0%5D%5BUnit%5D={4}&Bets%5B0%5D%5BMultiple%5D={5}&Bets%5B0%5D%5BIsCompressed%5D=false&StopIfWin=false&__RequestVerificationToken={6}&SerialNumber={7}&Guid={8}";
                                str7 = string.Format(str7, new object[] { this.GetBetsLotteryID(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList(plan.Type, pTNumberList, plan.Play, null), this.GetRXWZString1(plan.Type, plan.RXWZ), plan.Money, Convert.ToInt32(plan.AutoTimes(str4, true)), base.Token, this.GetBetsExpect(plan.CurrentExpect, ""), str6 });
                                HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str7, pReferer, AppInfo.PTInfo.BetsTime2, "UTF-8", true);
                            }
                            else
                            {
                                str7 = "text=%5B%7B%22lottery%22%3A%22{0}%22%2C%22issue%22%3A%22%22%2C%22method%22%3A%22{1}%22%2C%22content%22%3A%22{2}%22%2C%22model%22%3A%22{3}%22%2C%22multiple%22%3A{4}%2C%22code%22%3A{5}%2C%22issue%22%3A%22{6}%22%2C%22compress%22%3Afalse%7D%5D";
                                string prize = base.Prize;
                                str7 = string.Format(str7, new object[] { this.GetPTLotteryName(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList(plan.Type, pTNumberList, plan.Play, plan.RXWZ), plan.UnitString, Convert.ToInt32(plan.AutoTimes(str4, true)), prize, this.GetBetsExpect(plan.CurrentExpect, "") });
                                HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str7, pReferer, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
                            }
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
            ((pResponseText.Contains("\"message\" : \"请求成功\"") || pResponseText.Contains("\"ErrorMessage\":null")) || (pResponseText == "投注成功"));

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
                string accountsMemLine;
                string indexLine;
                string str3;
                string str4;
                if (this.CheckLotteryIsVR(pType))
                {
                    if (base.IsLoginVR)
                    {
                        accountsMemLine = this.GetAccountsMemLine(pType);
                        indexLine = this.GetIndexLine();
                        str3 = "";
                        str4 = "";
                        HttpHelper.GetResponse(ref str3, accountsMemLine, "POST", str4, indexLine, 0x2710, "UTF-8", true);
                        string str5 = str3;
                        AppInfo.Account.BankBalance = Convert.ToDouble(str5);
                    }
                }
                else
                {
                    accountsMemLine = this.GetAccountsMemLine(pType);
                    indexLine = this.GetLotteryLine(pType, false);
                    str3 = "";
                    str4 = "";
                    HttpHelper.GetResponse(ref str3, accountsMemLine, "POST", str4, indexLine, 0x2710, "UTF-8", true);
                    base.BankBalance = CommFunc.GetIndexString(str3, "\"lotteryBalance\" : ", ",", 0);
                    AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType)
        {
            string str = this.GetLine() + "/yx/u/api/loop";
            if (this.CheckLotteryIsVR(pType))
            {
                str = this.GetVRLine() + "/Home/GetWalletAmount";
            }
            return str;
        }

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if (pType == ConfigurationStatus.LotteryType.YHSGXDLSSC)
            {
                if (pIsBets)
                {
                    return str.Replace("-", "-0");
                }
                return str.Replace("-0", "-");
            }
            if ((((pType == ConfigurationStatus.LotteryType.GD11X5) || (pType == ConfigurationStatus.LotteryType.SD11X5)) || ((pType == ConfigurationStatus.LotteryType.SH11X5) || (pType == ConfigurationStatus.LotteryType.JX11X5))) || (pType == ConfigurationStatus.LotteryType.JS11X5))
            {
                if (pIsBets)
                {
                    str = str.Replace("-0", "-");
                }
                return str;
            }
            if ((((pType == ConfigurationStatus.LotteryType.VRSSC) || (pType == ConfigurationStatus.LotteryType.VRHXSSC)) || (pType == ConfigurationStatus.LotteryType.VR3FC)) && pIsBets)
            {
                str = str.Replace("-", "");
            }
            return str;
        }

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            return this.GetAppExpect(AppInfo.Current.Lottery.Type, str2, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType)
        {
            string str = this.GetLine() + "/yx/u/api/game-lottery/add-order";
            if (this.CheckLotteryIsVR(pType))
            {
                str = $"{this.GetVRLine()}/Bet/Confirm";
            }
            return str;
        }

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "151";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "911";
            }
            if (pType == ConfigurationStatus.LotteryType.YHSGXDLSSC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.YHSGHGSSC)
            {
                return "191";
            }
            if (pType == ConfigurationStatus.LotteryType.YHSGXWYFFC)
            {
                return "119";
            }
            if (pType == ConfigurationStatus.LotteryType.CAIHFLP15C)
            {
                return "200";
            }
            if (pType == ConfigurationStatus.LotteryType.CAIHFLP2FC)
            {
                return "201";
            }
            if (pType == ConfigurationStatus.LotteryType.CAIHFLP5FC)
            {
                return "202";
            }
            if (pType == ConfigurationStatus.LotteryType.YHSGDJSSC)
            {
                return "601";
            }
            if (pType == ConfigurationStatus.LotteryType.YHSGXJPSSC)
            {
                return "51";
            }
            if (pType == ConfigurationStatus.LotteryType.YHSGLD2FC)
            {
                return "203";
            }
            if (pType == ConfigurationStatus.LotteryType.VRSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "24";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "22";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "26";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "23";
            }
            if (pType == ConfigurationStatus.LotteryType.JS11X5)
            {
                return "28";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "43";
            }
            if (pType == ConfigurationStatus.LotteryType.XYFTPK10)
            {
                str = "47";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType)
        {
            string lotteryLine = this.GetLine() + "/yx/u/api/game-lottery/static-open-code";
            if (this.CheckLotteryIsVR(pType))
            {
                lotteryLine = this.GetLotteryLine(pType, false);
            }
            return lotteryLine;
        }

        public override string GetIndexLine() => 
            (this.GetLine() + "/index");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false)
        {
            string str = this.GetLine() + "/static/lt.html";
            if (this.CheckLotteryIsVR(pType))
            {
                str = $"{this.GetVRLine()}/Bet/Index/{this.GetBetsLotteryID(pType)}";
            }
            return str;
        }

        public string GetNumberList(ConfigurationStatus.LotteryType pType, List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            int num;
            string str2;
            int num2;
            int num3;
            char ch;
            string str = "";
            if (this.CheckLotteryIsVR(pType))
            {
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
                {
                    if (CommFunc.CheckPlayIsFS(playName))
                    {
                        list = new List<string>();
                        for (num = 0; num < pNumberList.Count; num++)
                        {
                            str2 = pNumberList[num];
                            list.Add(str2);
                        }
                        return CommFunc.Join(list, ",").Replace("*", "");
                    }
                    if (playName.Contains("定位胆"))
                    {
                        ch = playName[3];
                        num2 = AppInfo.FiveDic[ch.ToString()];
                        list = new List<string>();
                        for (num = 0; num < 5; num++)
                        {
                            str2 = "*";
                            if (num == num2)
                            {
                                str2 = CommFunc.Join(pNumberList);
                            }
                            list.Add(str2);
                        }
                        return CommFunc.Join(list, ",").Replace("*", "");
                    }
                    if (CommFunc.CheckPlayIsZuX(playName))
                    {
                        if (playName.Contains("复式"))
                        {
                            return CommFunc.Join(pNumberList);
                        }
                        return CommFunc.Join(pNumberList, ",");
                    }
                    return CommFunc.Join(pNumberList, ",");
                }
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                {
                    if (CommFunc.CheckPlayIsDS(playName))
                    {
                        return CommFunc.Join(pNumberList, ",");
                    }
                    if (CommFunc.CheckPlayIsFS(playName))
                    {
                        str = CommFunc.Join(pNumberList, " ");
                    }
                    return str;
                }
                if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
                {
                    return str;
                }
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, ",");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    return CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                num3 = (playName == "猜冠军猜冠军") ? 1 : 10;
                for (num = 0; num < num3; num++)
                {
                    str2 = "*";
                    if (num == num2)
                    {
                        str2 = CommFunc.Join(pNumberList, " ");
                    }
                    list.Add(str2);
                }
                return CommFunc.Join(list, ",").Replace("*", "");
            }
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
                        num2 = 0;
                        for (num = 0; num < 5; num++)
                        {
                            str2 = "*";
                            if (playInfo.IndexList.Contains(num + 1))
                            {
                                str2 = pNumberList[num2++];
                            }
                            list.Add(str2);
                        }
                        str = CommFunc.Join(list, "%2C").Replace("*", "-");
                    }
                }
                else if (playName.Contains("定位胆"))
                {
                    ch = playName[3];
                    num2 = AppInfo.FiveDic[ch.ToString()];
                    list = new List<string>();
                    for (num = 0; num < 5; num++)
                    {
                        str2 = "*";
                        if (num == num2)
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
                    str = this.GetRXWZString1(pType, pRXWZ) + str;
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
                num3 = (playName == "猜冠军猜冠军") ? 1 : 5;
                for (num = 0; num < num3; num++)
                {
                    str2 = "*";
                    if (num == num2)
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
            (this.GetLine() + "/yx/u/api/game-lottery/static-open-time");

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (this.CheckLotteryIsVR(pType))
            {
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
                {
                    if (playName == "前三直选单式")
                    {
                        return "6";
                    }
                    if (playName == "前三直选复式")
                    {
                        return "5";
                    }
                    if (playName == "前三组三复式")
                    {
                        return "54";
                    }
                    if (playName == "前三组三单式")
                    {
                        return "55";
                    }
                    if (playName == "前三组六复式")
                    {
                        return "56";
                    }
                    if (playName == "前三组六单式")
                    {
                        return "57";
                    }
                    if (playName == "后三直选单式")
                    {
                        return "10";
                    }
                    if (playName == "后三直选复式")
                    {
                        return "9";
                    }
                    if (playName == "后三组三复式")
                    {
                        return "62";
                    }
                    if (playName == "后三组三单式")
                    {
                        return "63";
                    }
                    if (playName == "后三组六复式")
                    {
                        return "64";
                    }
                    if (playName == "后三组六单式")
                    {
                        return "65";
                    }
                    if (playName == "中三直选单式")
                    {
                        return "8";
                    }
                    if (playName == "中三直选复式")
                    {
                        return "7";
                    }
                    if (playName == "中三组三复式")
                    {
                        return "58";
                    }
                    if (playName == "中三组三单式")
                    {
                        return "59";
                    }
                    if (playName == "中三组六复式")
                    {
                        return "60";
                    }
                    if (playName == "中三组六单式")
                    {
                        return "61";
                    }
                    if (playName == "前二直选单式")
                    {
                        return "12";
                    }
                    if (playName == "前二直选复式")
                    {
                        return "11";
                    }
                    if (playName == "后二直选单式")
                    {
                        return "14";
                    }
                    if (playName == "后二直选复式")
                    {
                        return "13";
                    }
                    if (playName == "后四直选单式")
                    {
                        return "4";
                    }
                    if (playName == "后四直选复式")
                    {
                        return "3";
                    }
                    if (playName == "五星直选单式")
                    {
                        return "2";
                    }
                    if (playName == "五星直选复式")
                    {
                        return "1";
                    }
                    if (playName == "任三直选单式")
                    {
                        return "18";
                    }
                    if (playName == "任三直选复式")
                    {
                        return "17";
                    }
                    if (playName == "任三组三单式")
                    {
                        return "125";
                    }
                    if (playName == "任三组三复式")
                    {
                        return "124";
                    }
                    if (playName == "任三组六单式")
                    {
                        return "127";
                    }
                    if (playName == "任三组六复式")
                    {
                        return "126";
                    }
                    if (playName == "任二直选单式")
                    {
                        return "20";
                    }
                    if (playName == "任二直选复式")
                    {
                        return "19";
                    }
                    if (playName == "任四直选单式")
                    {
                        return "16";
                    }
                    if (playName == "任四直选复式")
                    {
                        return "15";
                    }
                    if (playName.Contains("定位胆"))
                    {
                        str = "21";
                    }
                    return str;
                }
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                {
                    if (playName == "前三直选单式")
                    {
                        return "1002";
                    }
                    if (playName == "前二直选单式")
                    {
                        return "1008";
                    }
                    if (playName == "任选复式一中一")
                    {
                        return "1037";
                    }
                    if (playName == "任选复式二中二")
                    {
                        return "1039";
                    }
                    if (playName == "任选复式三中三")
                    {
                        return "1041";
                    }
                    if (playName == "任选复式四中四")
                    {
                        return "1043";
                    }
                    if (playName == "任选复式五中五")
                    {
                        return "1045";
                    }
                    if (playName == "任选单式一中一")
                    {
                        return "1038";
                    }
                    if (playName == "任选单式二中二")
                    {
                        return "1040";
                    }
                    if (playName == "任选单式三中三")
                    {
                        return "1042";
                    }
                    if (playName == "任选单式四中四")
                    {
                        return "1044";
                    }
                    if (playName == "任选单式五中五")
                    {
                        str = "1046";
                    }
                    return str;
                }
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                {
                    if (playName == "猜前三单式")
                    {
                        return "3002";
                    }
                    if (playName == "猜前三复式")
                    {
                        return "3001";
                    }
                    if (playName == "猜前二单式")
                    {
                        return "3004";
                    }
                    if (playName == "猜前二复式")
                    {
                        return "3003";
                    }
                    if (playName == "猜前四单式")
                    {
                        return "3010";
                    }
                    if (playName == "猜前四复式")
                    {
                        return "3009";
                    }
                    if (playName == "猜前五单式")
                    {
                        return "3012";
                    }
                    if (playName == "猜前五复式")
                    {
                        return "3011";
                    }
                    if (playName == "猜冠军猜冠军")
                    {
                        return "3005";
                    }
                    if (playName.Contains("定位胆"))
                    {
                        str = "3007";
                    }
                }
                return str;
            }
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
                    return "";
                }
                if (playName == "前四直选复式")
                {
                    return "";
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
                    return "zx_q4_ds";
                }
                if (playName == "猜前四复式")
                {
                    return "zx_q4_fs";
                }
                if (playName == "猜前五单式")
                {
                    return "zx_q5_ds";
                }
                if (playName == "猜前五复式")
                {
                    return "zx_q5_fs";
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
            string str = "";
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            str = CommFunc.GetIndexString(pResponseText, "\"message\" : \"", "\"", 0);
            if (str == "")
            {
                str = CommFunc.GetIndexString(pResponseText, "\"ErrorMessage\":\"", "\"", 0);
            }
            return str;
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                str = "CQSSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                str = "XJSSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                str = "TXSSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.YHSGXDLSSC)
            {
                str = "TGSSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.YHSGHGSSC)
            {
                str = "HGSSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.YHSGXWYFFC)
            {
                str = "ZYFFSSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.CAIHFLP15C)
            {
                str = "FLB15SSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.CAIHFLP2FC)
            {
                str = "FLB2SSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.CAIHFLP5FC)
            {
                str = "FLB5SSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.YHSGDJSSC)
            {
                str = "DJSSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.YHSGXJPSSC)
            {
                str = "XJPSSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.YHSGLD2FC)
            {
                str = "LD2SSC";
            }
            else if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                str = "GD11Y";
            }
            else if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                str = "SD11Y";
            }
            else if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                str = "SH11Y";
            }
            else if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                str = "JX11Y";
            }
            else if (pType == ConfigurationStatus.LotteryType.JS11X5)
            {
                str = "JS11Y";
            }
            else if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "BJPK10";
            }
            else if (pType == ConfigurationStatus.LotteryType.XYFTPK10)
            {
                str = "XYFTPK10";
            }
            return str.ToUpper();
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/sso/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString1(ConfigurationStatus.LotteryType pType, List<int> pRXWZ)
        {
            int num;
            string str = "";
            if (this.CheckLotteryIsVR(pType))
            {
                if (pRXWZ == null)
                {
                    return str;
                }
                List<int> list = new List<int>();
                for (num = 0; num < pRXWZ.Count; num++)
                {
                    list.Add(pRXWZ[num] + 1);
                }
                return CommFunc.Join(list, ",");
            }
            List<string> pList = new List<string>();
            for (num = 0; num < 5; num++)
            {
                string item = pRXWZ.Contains(num) ? "√" : "-";
                pList.Add(item);
            }
            return $"[{CommFunc.Join(pList, ",")}]";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string openTime;
                string lotteryLine;
                string str3;
                string str4;
                if (base.Prize == "")
                {
                    openTime = this.GetLine() + "/yx/u/api/game-lottery/init-game-lottery";
                    lotteryLine = this.GetLotteryLine(pType, false);
                    str3 = "";
                    str4 = $"name={this.GetBetsLotteryID(pType)}";
                    HttpHelper.GetResponse(ref str3, openTime, "POST", str4, lotteryLine, 0x2710, "UTF-8", true);
                    if (str3 != "")
                    {
                        base.Prize = CommFunc.GetIndexString(str3, "\"maxBetCode\" : ", ",", 0);
                    }
                }
                if (!this.CheckLotteryIsVR(pType))
                {
                    openTime = this.GetOpenTime();
                    lotteryLine = this.GetLotteryLine(pType, false);
                    str3 = "";
                    str4 = $"name={this.GetPTLotteryName(pType)}";
                    HttpHelper.GetResponse(ref str3, openTime, "POST", str4, lotteryLine, 0x2710, "UTF-8", true);
                    if (str3 != "")
                    {
                        base.Expect = CommFunc.GetIndexString(str3, "\"issue\" : \"", "\"", 0);
                        base.Expect = this.GetAppExpect(pType, base.Expect, false);
                        base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
                    }
                }
            }
            catch
            {
            }
        }

        public bool GetVRData(ConfigurationStatus.LotteryType pType, ref string pVRData)
        {
            string indexLine = this.GetIndexLine();
            string pUrl = $"{this.GetLine()}/yx/u/api/game/vr/go";
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, base.BetsTime1, "UTF-8", true);
            bool flag = pResponsetext.Contains("竞速娱乐 VR") || (pResponsetext == "");
            if (flag)
            {
                base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
            }
            return flag;
        }

        public override string GetVRLine() => 
            "http://dmo.vrbetapi.com";

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
            string loginLine = this.GetLoginLine();
            string str2 = HttpUtility.UrlEncode(CommFunc.WebMD51(pW));
            string pUrl = $"{this.GetLine()}/sso/login?cn={pID}&password={str2}";
            string pResponsetext = "";
            string pData = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
            flag = pResponsetext.Contains("\"msg\":\"登录成功\"");
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
            return pResponsetext.Contains("永恒时光");
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            string pData = "";
            HttpHelper.GetResponse(ref pResponsetext, quitPTLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
        }

        public override bool VRWebLoginMain(ConfigurationStatus.LotteryType pType)
        {
            if (this.CheckLotteryIsVR(pType) && !base.IsLoginVR)
            {
                string pVRData = "";
                if (!this.GetVRData(pType, ref pVRData))
                {
                    return false;
                }
                HttpHelper.SaveCookies(HttpHelper.GetHttpHelperCookieString(this.GetUrlLine(), null), this.GetVRHostLine());
                base.IsLoginVR = true;
            }
            return true;
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

