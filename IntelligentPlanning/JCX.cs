namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class JCX : PTBase
    {
        public string VerifyCodeKey = "";

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
                            string str8;
                            List<string> pTNumberList = plan.GetPTNumberList(dictionary2[str4]);
                            string pResponsetext = "";
                            int num = plan.FNNumber(str4);
                            string prize = base.Prize;
                            int num2 = plan.Unit + 4;
                            string str7 = Guid.NewGuid().ToString();
                            if (this.CheckLotteryIsVR(plan.Type))
                            {
                                str8 = "LotteryGameID={0}&IssueSerialNumber=&Bets%5B0%5D%5BBetTypeCode%5D={1}&Bets%5B0%5D%5BBetTypeName%5D=&Bets%5B0%5D%5BNumber%5D={2}&Bets%5B0%5D%5BPosition%5D={3}&Bets%5B0%5D%5BUnit%5D={4}&Bets%5B0%5D%5BMultiple%5D={5}&Bets%5B0%5D%5BIsCompressed%5D=false&StopIfWin=false&__RequestVerificationToken={6}&SerialNumber={7}&Guid={8}";
                                str8 = string.Format(str8, new object[] { this.GetBetsLotteryID(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList(plan.Type, pTNumberList, plan.Play, null), this.GetRXWZString1(plan.Type, plan.RXWZ), plan.Money, Convert.ToInt32(plan.AutoTimes(str4, true)), base.Token, this.GetBetsExpect(plan.CurrentExpect, ""), str7 });
                                HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str8, pReferer, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
                            }
                            else
                            {
                                str8 = "LotteryType={0}&IsChase=false&IsStopAfterWin=true&Issue={1}&IsAllIn=false&LstOrder%5B0%5D%5BPlayType%5D={2}&LstOrder%5B0%5D%5BBetPlan%5D={7}&LstOrder%5B0%5D%5BBetCodes%5D={3}&LstOrder%5B0%5D%5BIsZip%5D=false&LstOrder%5B0%5D%5BBetBonusGroup%5D={4}&LstOrder%5B0%5D%5BMoneyUnit%5D={5}&LstOrder%5B0%5D%5BBetMultiple%5D={6}";
                                str8 = string.Format(str8, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList(plan.Type, pTNumberList, plan.Play, plan.RXWZ), prize, num2, Convert.ToInt32(plan.AutoTimes(str4, true)), this.GetRXWZString1(plan.Type, plan.RXWZ) });
                                HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str8, pReferer, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
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
            ((pResponseText.Contains("\"Msg\":\"投注成功！\"") || pResponseText.Contains("\"ErrorMessage\":null")) || (pResponseText == "投注成功"));

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
                    indexLine = this.GetIndexLine();
                    str3 = "";
                    str4 = $"type={this.GetBetsLotteryID(pType)}";
                    HttpHelper.GetResponse(ref str3, accountsMemLine, "POST", str4, indexLine, 0x2710, "UTF-8", true);
                    if (str3 != "")
                    {
                        base.Expect = CommFunc.GetIndexString(str3, "\"Issue\":\"", "\"", 0);
                        base.Expect = this.GetAppExpect(pType, base.Expect, false);
                        base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
                        base.BankBalance = CommFunc.GetIndexString(str3, "\"AvaiableAmountStr\":\"", "\"", 0);
                        AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
                        if (base.Prize == "")
                        {
                            base.Prize = CommFunc.GetIndexString(str3, "\"BonusGroup\":", ",", 0);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType)
        {
            string str = this.GetLine() + "/lottery/saleissue";
            if (this.CheckLotteryIsVR(pType))
            {
                str = this.GetVRLine() + "/Home/GetWalletAmount";
            }
            return str;
        }

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string lotteryID = CommFunc.GetLotteryID(pType);
            string str2 = pExpect;
            if (CommFunc.CheckIsSkipLottery(lotteryID))
            {
                return str2;
            }
            if (pIsBets)
            {
                str2 = str2.Substring(2);
                switch (lotteryID)
                {
                    case "GD11X5":
                    case "SD11X5":
                    case "SH11X5":
                    case "JX11X5":
                    case "AH11X5":
                    case "XJSSC":
                        return str2.Replace("-0", "");
                }
                return str2.Replace("-", "");
            }
            return ("20" + str2);
        }

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            return this.GetAppExpect(AppInfo.Current.Lottery.Type, str2, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType)
        {
            string str = this.GetLine() + "/lottery/bet";
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
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "3";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXQQFFC)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXXXLSSC)
            {
                return "15";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXMG45M)
            {
                return "17";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXMJFFC)
            {
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXNYFFC)
            {
                return "19";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXFLPFFC)
            {
                return "20";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXMG15C)
            {
                return "25";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXHN15C)
            {
                return "27";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXNY15C)
            {
                return "28";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXXDLSSC)
            {
                return "29";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXNHGSSC)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXNDJSSC)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.VRSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.VRHXSSC)
            {
                return "12";
            }
            if (pType == ConfigurationStatus.LotteryType.VR3FC)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "101";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "102";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "103";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "104";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "105";
            }
            if (pType == ConfigurationStatus.LotteryType.JCXJN11X5)
            {
                return "120";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "401";
            }
            if (pType == ConfigurationStatus.LotteryType.JCX60MPK10)
            {
                str = "420";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType)
        {
            string lotteryLine = this.GetLine() + "/lottery/lotterycode";
            if (this.CheckLotteryIsVR(pType))
            {
                lotteryLine = this.GetLotteryLine(pType, false);
            }
            return lotteryLine;
        }

        public override string GetIndexLine() => 
            (this.GetLine() + "/web/index.html");

        public override string GetLoginLine() => 
            (this.GetLine() + "/web/index.html");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false)
        {
            string str = this.GetLine() + "/";
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
            ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(playName);
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
                    str = CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                    str = CommFunc.Join(list, ",").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, ",");
                    }
                    else
                    {
                        str = CommFunc.Join(pNumberList, ",");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
                    str = CommFunc.Join(pNumberList, ",");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, " ,").Replace("*", "");
                }
                else
                {
                    pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
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
                    str = CommFunc.Join(list, ",").Replace("*", "");
                    if (playName == "猜冠军猜冠军")
                    {
                        str = str.Replace(" ", ",");
                    }
                }
            }
            return HttpUtility.UrlEncode(str);
        }

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
                    return "56";
                }
                if (playName == "前三直选复式")
                {
                    return "55";
                }
                if (playName == "前三组三复式")
                {
                    return "60";
                }
                if (playName == "前三组三单式")
                {
                    return "61";
                }
                if (playName == "前三组六复式")
                {
                    return "62";
                }
                if (playName == "前三组六单式")
                {
                    return "63";
                }
                if (playName == "后三直选单式")
                {
                    return "106";
                }
                if (playName == "后三直选复式")
                {
                    return "105";
                }
                if (playName == "后三组三复式")
                {
                    return "110";
                }
                if (playName == "后三组三单式")
                {
                    return "111";
                }
                if (playName == "后三组六复式")
                {
                    return "112";
                }
                if (playName == "后三组六单式")
                {
                    return "113";
                }
                if (playName == "中三直选单式")
                {
                    return "82";
                }
                if (playName == "中三直选复式")
                {
                    return "81";
                }
                if (playName == "中三组三复式")
                {
                    return "86";
                }
                if (playName == "中三组三单式")
                {
                    return "87";
                }
                if (playName == "中三组六复式")
                {
                    return "88";
                }
                if (playName == "中三组六单式")
                {
                    return "89";
                }
                if (playName == "前二直选单式")
                {
                    return "132";
                }
                if (playName == "前二直选复式")
                {
                    return "131";
                }
                if (playName == "后二直选单式")
                {
                    return "146";
                }
                if (playName == "后二直选复式")
                {
                    return "145";
                }
                if (playName == "前四直选单式")
                {
                    return "22";
                }
                if (playName == "前四直选复式")
                {
                    return "21";
                }
                if (playName == "后四直选单式")
                {
                    return "39";
                }
                if (playName == "后四直选复式")
                {
                    return "38";
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
                    return "276";
                }
                if (playName == "任三直选复式")
                {
                    return "275";
                }
                if (playName == "任三组三复式")
                {
                    return "280";
                }
                if (playName == "任三组三单式")
                {
                    return "281";
                }
                if (playName == "任三组六复式")
                {
                    return "282";
                }
                if (playName == "任三组六单式")
                {
                    return "283";
                }
                if (playName == "任二直选单式")
                {
                    return "256";
                }
                if (playName == "任二直选复式")
                {
                    return "255";
                }
                if (playName == "任四直选单式")
                {
                    return "296";
                }
                if (playName == "任四直选复式")
                {
                    return "295";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "165";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "11";
                }
                if (playName == "前二直选单式")
                {
                    return "09";
                }
                if (playName == "任选复式一中一")
                {
                    return "550";
                }
                if (playName == "任选复式二中二")
                {
                    return "551";
                }
                if (playName == "任选复式三中三")
                {
                    return "552";
                }
                if (playName == "任选复式四中四")
                {
                    return "553";
                }
                if (playName == "任选复式五中五")
                {
                    return "554";
                }
                if (playName == "任选单式一中一")
                {
                    return "558";
                }
                if (playName == "任选单式二中二")
                {
                    return "559";
                }
                if (playName == "任选单式三中三")
                {
                    return "560";
                }
                if (playName == "任选单式四中四")
                {
                    return "561";
                }
                if (playName == "任选单式五中五")
                {
                    str = "562";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "818";
                }
                if (playName == "猜前三复式")
                {
                    return "817";
                }
                if (playName == "猜前二单式")
                {
                    return "805";
                }
                if (playName == "猜前二复式")
                {
                    return "804";
                }
                if (playName == "猜前四单式")
                {
                    return "826";
                }
                if (playName == "猜前四复式")
                {
                    return "825";
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
                    return "800";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "836";
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
            str = CommFunc.GetIndexString(pResponseText, "\"Msg\":\"", "\"", 0);
            if (str == "")
            {
                str = CommFunc.GetIndexString(pResponseText, "\"ErrorMessage\":\"", "\"", 0);
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString1(ConfigurationStatus.LotteryType pType, List<int> pRXWZ)
        {
            List<int> list;
            string str = "";
            if (this.CheckLotteryIsVR(pType))
            {
                if (pRXWZ == null)
                {
                    return str;
                }
                list = new List<int>();
                for (int i = 0; i < pRXWZ.Count; i++)
                {
                    list.Add(pRXWZ[i] + 1);
                }
                return CommFunc.Join(list, ",");
            }
            if ((pRXWZ == null) || (pRXWZ.Count <= 0))
            {
                return str;
            }
            list = new List<int>();
            foreach (int num2 in pRXWZ)
            {
                int item = num2 + 1;
                list.Add(item);
            }
            return CommFunc.Join(list, ",");
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

        public bool GetVRData(ConfigurationStatus.LotteryType pType, ref string pVRData)
        {
            string indexLine = this.GetIndexLine();
            string pUrl = $"{this.GetLine()}/vr/login";
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, base.BetsTime1, "UTF-8", true);
            bool flag = pResponsetext.Contains("data=");
            if (flag)
            {
                pVRData = CommFunc.GetIndexString(pResponsetext, "data=", "\"", 0);
            }
            return flag;
        }

        public override string GetVRLine() => 
            "http://ctt.vrbetapi.com";

        public override string GetWebVerifyCode(string pVerifyCodeFile)
        {
            string pVerifyCode = "";
            try
            {
                this.VerifyCodeKey = "";
                string str2 = "/user/vcode";
                string pUrl = this.GetLine() + str2;
                File.Delete(pVerifyCodeFile);
                string pReferer = "";
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
                string pImageString = CommFunc.GetIndexString(pResponsetext, "\"VerifyCode\":\"", "\"", 0);
                this.VerifyCodeKey = CommFunc.GetIndexString(pResponsetext, "\"VerifyKey\":\"", "\"", 0);
                Image image = CommFunc.ConvertImageBy64String(pImageString);
                image.Save(pVerifyCodeFile);
                image.Dispose();
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
                string pUrl = this.GetLine() + "/user/login";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string str6 = HttpUtility.UrlEncode(this.VerifyCodeKey);
                string pData = $"loginName={pID}&pwd={str5}&captcha={webVerifyCode}&captchaKey={str6}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
                flag = pResponsetext.Contains("\"Msg\":\"登陆成功！\"");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"Msg\":\"", "\"", 0);
                    if (pHint.Contains("验证码错误"))
                    {
                        pHint = "";
                        return this.WebLoginMain(pID, pW, ref pHint);
                    }
                    return flag;
                }
                HttpHelper.BetsTokenKey = "identity";
                HttpHelper.BetsTokenValue = CommFunc.GetIndexString(pResponsetext, "\"Key\":\"", "\"", 0);
            }
            return flag;
        }

        public bool LoginVRWeb(ConfigurationStatus.LotteryType pType, string pVRData)
        {
            string pReferer = "";
            string pUrl = $"{this.GetVRLine()}/Account/LoginValidate?version=1.0&id=CTT&data={pVRData}";
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("竞速娱乐 VR") || (pResponsetext == "");
            if (flag)
            {
                base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext.Contains("金诚信");
        }

        public override void QuitPT()
        {
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
                if (!this.LoginVRWeb(pType, pVRData))
                {
                    return false;
                }
                base.IsLoginVR = true;
            }
            return true;
        }

        public override bool WebLoginMain(string pID, string pW, ref string pHint)
        {
            HttpHelper.BetsTokenKey = HttpHelper.BetsTokenValue = "";
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

