namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class RDYL : PTBase
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
                            string str7;
                            List<string> pTNumberList = plan.GetPTNumberList(dictionary2[str4]);
                            string pResponsetext = "";
                            int num = plan.FNNumber(str4);
                            string str6 = Guid.NewGuid().ToString();
                            if (this.CheckLotteryIsVR(plan.Type))
                            {
                                str7 = "LotteryGameID={0}&IssueSerialNumber=&Bets%5B0%5D%5BBetTypeCode%5D={1}&Bets%5B0%5D%5BBetTypeName%5D=&Bets%5B0%5D%5BNumber%5D={2}&Bets%5B0%5D%5BPosition%5D={3}&Bets%5B0%5D%5BUnit%5D={4}&Bets%5B0%5D%5BMultiple%5D={5}&Bets%5B0%5D%5BIsCompressed%5D=false&StopIfWin=false&__RequestVerificationToken={6}&SerialNumber={7}&Guid={8}";
                                str7 = string.Format(str7, new object[] { this.GetBetsLotteryID(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList(plan.Type, pTNumberList, plan.Play, null), this.GetRXWZString1(plan.Type, plan.RXWZ), plan.Money, Convert.ToInt32(plan.AutoTimes(str4, true)), base.Token, this.GetBetsExpect(plan.CurrentExpect, ""), str6 });
                                HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str7, lotteryLine, AppInfo.PTInfo.BetsTime2, "UTF-8", true);
                            }
                            else
                            {
                                str7 = "gid={0}&issue={1}&rebate={2}&codes%5B0%5D%5Bpid%5D={3}&codes%5B0%5D%5Bcode%5D={4}&codes%5B0%5D%5Bmultiple%5D={5}&codes%5B0%5D%5Bbet_num%5D={6}&codes%5B0%5D%5Bamount_mode%5D={7}&appendcodestop={8}";
                                str7 = string.Format(str7, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), "0", this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList(plan.Type, pTNumberList, plan.Play, plan.RXWZ), Convert.ToInt32(plan.AutoTimes(str4, true)), num, plan.Money, 1 });
                                HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str7, lotteryLine, base.BetsTime2, "UTF-8", true);
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
            pHint.Contains("登陆");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            ((pResponseText.Contains("\"data\":\"投注成功\"") || pResponseText.Contains("\"ErrorMessage\":null")) || (pResponseText == "投注成功"));

        private bool CheckVerifyCode(string pVerifyCode)
        {
            if (pVerifyCode.Length != 4)
            {
                return false;
            }
            return true;
        }

        public override void CountPrizeDic(string pResponseText)
        {
            base.PlayMethodDic.Clear();
            List<string> list = CommFunc.SplitString(pResponseText, "\"},{\"", -1);
            for (int i = 0; i < list.Count; i++)
            {
                string pStr = list[i];
                string str2 = CommFunc.GetIndexString(pStr, "\"h_g_p_name\":\"", "\"", 0);
                string str3 = CommFunc.GetIndexString(pStr, "\"line\":", ",", 0);
                if (str3 != "")
                {
                    string str4 = CommFunc.GetIndexString(pStr, "\"win\":[", "]", 0);
                    string key = $"{str2}-{str3}-{str4}";
                    string str6 = CommFunc.GetIndexString(pStr, "h_g_p_id\":\"", "\"", 0);
                    if (!base.PlayMethodDic.ContainsKey(key))
                    {
                        base.PlayMethodDic[key] = str6;
                    }
                }
            }
        }

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                if (!AppInfo.PTInfo.PTIsBreak)
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
                            if (this.CheckBreakConnect(str3))
                            {
                                AppInfo.PTInfo.PTIsBreak = true;
                            }
                            else
                            {
                                string str5 = str3;
                                AppInfo.Account.BankBalance = Convert.ToDouble(str5);
                            }
                        }
                    }
                    else
                    {
                        accountsMemLine = this.GetAccountsMemLine(pType);
                        indexLine = this.GetIndexLine();
                        str3 = "";
                        str4 = "";
                        HttpHelper.GetResponse(ref str3, accountsMemLine, "POST", str4, indexLine, 0x2710, "UTF-8", true);
                        if (this.CheckBreakConnect(str3))
                        {
                            AppInfo.PTInfo.PTIsBreak = true;
                        }
                        else
                        {
                            base.BankBalance = CommFunc.GetIndexString(str3, "\"h_u_balance\":\"", "\"", 0);
                            AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
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
            string str = this.GetLine() + "/users/get_user_info";
            if (this.CheckLotteryIsVR(pType))
            {
                str = this.GetVRLine() + "/Home/GetWalletAmount";
            }
            return str;
        }

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string name = AppInfo.Current.Lottery.Name;
            string iD = AppInfo.Current.Lottery.ID;
            string str3 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            if (name.Contains("11选5"))
            {
                return str3.Replace("-0", "-");
            }
            switch (iD)
            {
                case "VRSSC":
                case "VRHXSSC":
                case "VR3FC":
                    str3 = str3.Replace("-", "");
                    break;
            }
            return str3;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType)
        {
            string str = this.GetLine() + "/lottery/add_bet";
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
                return "3";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "29";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCSE15F)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCNY15C)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCHG15F)
            {
                return "26";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCDJ15F)
            {
                return "28";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCFLB15C)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCFLB2FC)
            {
                return "12";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCFLB5FC)
            {
                return "13";
            }
            if (pType == ConfigurationStatus.LotteryType.RDYLFFC)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.RDYL2FC)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.RDYL5FC)
            {
                return "9";
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
                return "17";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "16";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "15";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "14";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "25";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType)
        {
            string lotteryLine = this.GetLine() + "/lottery/get_history_win_code";
            if (this.CheckLotteryIsVR(pType))
            {
                lotteryLine = this.GetLotteryLine(pType, false);
            }
            return lotteryLine;
        }

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLineString(string pResponseText) => 
            pResponseText;

        public override string GetLoginLine() => 
            (this.GetLine() + "/");

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
                    List<string> pList = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        string pStr = pNumberList[num];
                        pList.Add(CommFunc.Join(pStr, ",", -1));
                    }
                    str = CommFunc.Join(pList, "|").Replace("*", "");
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
                            str2 = CommFunc.Join(pNumberList, ",");
                        }
                        list.Add(str2);
                    }
                    str = CommFunc.Join(list, "|").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, ",");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "|");
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
                    return CommFunc.Join(pNumberList, "|").Replace(" ", "");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, "|").Replace(" ", "");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = pNumberList[num].Replace(" ", ",");
                        list.Add(str2);
                    }
                    return CommFunc.Join(list, "|").Replace("*", "");
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
                        str2 = CommFunc.Join(pNumberList, ",");
                    }
                    list.Add(str2);
                }
                str = CommFunc.Join(list, "|").Replace("*", "");
                if (playName == "猜冠军猜冠军")
                {
                    str = str.Replace("|", ",");
                }
            }
            return str;
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
            string playString = this.GetPlayString(playName);
            if (base.PlayMethodDic.ContainsKey(playString))
            {
                str = base.PlayMethodDic[playString];
            }
            return str;
        }

        public override string GetPlayString(string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "单式-3-0,1,2";
                }
                if (playName == "前三直选复式")
                {
                    return "复式-3-0,1,2";
                }
                if (playName == "前三组三复式")
                {
                    return "组三-3-0,1,2";
                }
                if (playName == "前三组六复式")
                {
                    return "组六-3-0,1,2";
                }
                if (playName == "后三直选单式")
                {
                    return "单式-3-2,3,4";
                }
                if (playName == "后三直选复式")
                {
                    return "复式-3-2,3,4";
                }
                if (playName == "后三组三复式")
                {
                    return "组三-3-2,3,4";
                }
                if (playName == "后三组六复式")
                {
                    return "组六-3-2,3,4";
                }
                if (playName == "中三直选单式")
                {
                    return "单式-3-1,2,3";
                }
                if (playName == "中三直选复式")
                {
                    return "复式-3-1,2,3";
                }
                if (playName == "中三组三复式")
                {
                    return "组三-3-1,2,3";
                }
                if (playName == "中三组六复式")
                {
                    return "组六-3-1,2,3";
                }
                if (playName == "前二直选单式")
                {
                    return "前二单式-2-0,1";
                }
                if (playName == "前二直选复式")
                {
                    return "前二复式-2-0,1";
                }
                if (playName == "后二直选单式")
                {
                    return "后二单式-2-3,4";
                }
                if (playName == "后二直选复式")
                {
                    return "后二复式-2-3,4";
                }
                if (playName == "后四直选单式")
                {
                    return "单式-4-1,2,3,4";
                }
                if (playName == "后四直选复式")
                {
                    return "复式-4-1,2,3,4";
                }
                if (playName == "前四直选单式")
                {
                    return "单式-4-0,1,2,3";
                }
                if (playName == "前四直选复式")
                {
                    return "复式-4-0,1,2,3";
                }
                if (playName == "五星直选单式")
                {
                    return "五星单式-5-0,1,2,3,4";
                }
                if (playName == "五星直选复式")
                {
                    return "五星复式-5-0,1,2,3,4";
                }
                if (playName == "任三直选单式")
                {
                    return "单式-3-0,1,2,3,4";
                }
                if (playName == "任三直选复式")
                {
                    return "复式-3-0,1,2,3,4";
                }
                if (playName == "任三组三复式")
                {
                    return "";
                }
                if (playName == "任三组六复式")
                {
                    return "组六-3-0,1,2,3,4";
                }
                if (playName == "任二直选单式")
                {
                    return "单式-2-0,1,2,3,4";
                }
                if (playName == "任二直选复式")
                {
                    return "复式-2-0,1,2,3,4";
                }
                if (playName == "任四直选单式")
                {
                    return "单式-4-0,1,2,3,4";
                }
                if (playName == "任四直选复式")
                {
                    return "单式-4-0,1,2,3,4";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "定位胆-5-0,1,2,3,4";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "前三单式-3-0,1,2";
                }
                if (playName == "前二直选单式")
                {
                    return "前二单式-2-0,1";
                }
                if (playName == "任选复式一中一")
                {
                    return "";
                }
                if (playName == "任选复式二中二")
                {
                    return "";
                }
                if (playName == "任选复式三中三")
                {
                    return "";
                }
                if (playName == "任选复式四中四")
                {
                    return "";
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
                    return "";
                }
                if (playName == "任选单式三中三")
                {
                    return "";
                }
                if (playName == "任选单式四中四")
                {
                    return "";
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
                    return "单式-3-0,1,2";
                }
                if (playName == "猜前三复式")
                {
                    return "复式-3-0,1,2";
                }
                if (playName == "猜前二单式")
                {
                    return "单式-2-0,1";
                }
                if (playName == "猜前二复式")
                {
                    return "复式-2-0,1";
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
                    return "复式-1-0";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "六至十名-5-5,6,7,8,9" : "一至五名-5-0,1,2,3,4";
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
            str = CommFunc.GetIndexString(pResponseText, "\"error\":\"", "\"", 0);
            if (str == "")
            {
                str = CommFunc.GetIndexString(pResponseText, "\"ErrorMessage\":\"", "\"", 0);
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/users/sign_out");

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
            for (num = 0; num < pRXWZ.Count; num++)
            {
                string item = AppInfo.IndexDic[pRXWZ[num]];
                pList.Add(item);
            }
            return $"{CommFunc.Join(pList)}:";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (!this.CheckLotteryIsVR(pType))
                {
                    string str;
                    string lotteryLine;
                    string str3;
                    if (base.PlayMethodDic.Count == 0)
                    {
                        str = $"{this.GetLine()}/config/json/game_play_config_{this.GetBetsLotteryID(pType)}.json";
                        lotteryLine = this.GetLotteryLine(pType, false);
                        str3 = "";
                        HttpHelper.GetResponse(ref str3, str, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                        str3 = CommFunc.GetIndexString(str3, "\"data\":[{", "}]}", 0);
                        if (str3 != "")
                        {
                            this.CountPrizeDic(str3);
                        }
                    }
                    str = this.GetLine() + "/lottery/get_lottery_times";
                    lotteryLine = this.GetLotteryLine(pType, false);
                    str3 = "";
                    string pData = $"id={this.GetBetsLotteryID(pType)}";
                    HttpHelper.GetResponse(ref str3, str, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                    if (str3 != "")
                    {
                        base.Expect = CommFunc.GetIndexString(str3, "\"curr_issue\":\"", "\"", 0);
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
                string str2 = $"/images/set_check_code/?session_name=user_login&type=4&rand={DateTime.Now.ToString()}";
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
                string pUrl = this.GetLine() + "/users/login/";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"username={pID}&password={str5}&verification={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains(pID);
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"error\":\"", "\"", 0);
                    if (pHint.Contains("验证码"))
                    {
                        pHint = "";
                        return this.InputWeb(pID, pW, ref pHint);
                    }
                    return flag;
                }
                base.Rebate = CommFunc.GetIndexString(pResponsetext, "\"h_u_max_rebate\":\"", "\"", 0);
                base.Prize = (1700.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0)).ToString();
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
            return pResponsetext.Contains("仁鼎");
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

