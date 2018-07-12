namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class WDYL : PTBase
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
                            int num = plan.FNNumber(str4);
                            string pResponsetext = "";
                            string str6 = Guid.NewGuid().ToString();
                            if (this.CheckLotteryIsVR(plan.Type))
                            {
                                str7 = "LotteryGameID={0}&IssueSerialNumber=&Bets%5B0%5D%5BBetTypeCode%5D={1}&Bets%5B0%5D%5BBetTypeName%5D=&Bets%5B0%5D%5BNumber%5D={2}&Bets%5B0%5D%5BPosition%5D={3}&Bets%5B0%5D%5BUnit%5D={4}&Bets%5B0%5D%5BMultiple%5D={5}&Bets%5B0%5D%5BIsCompressed%5D=false&StopIfWin=false&__RequestVerificationToken={6}&SerialNumber={7}&Guid={8}";
                                str7 = string.Format(str7, new object[] { this.GetBetsLotteryID(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList(plan.Type, pTNumberList, plan.Play, null), this.GetRXWZString1(plan.Type, plan.RXWZ), plan.Money, Convert.ToInt32(plan.AutoTimes(str4, true)), base.Token, this.GetBetsExpect(plan.CurrentExpect, ""), str6 });
                                HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str7, lotteryLine, AppInfo.PTInfo.BetsTime2, "UTF-8", true);
                            }
                            else
                            {
                                str7 = "(\"success\":true,\"fail\":false,\"code\":\"1\",\"message\":null,\"data\":(\"LotteryId\":{0},\"Issuo\":\"{1}\",\"WinIsStop\":true,\"name\":\"{2}\",\"ChaseNum\":0,\"BetNumList\":[(\"Name\":\"{3}\",\"PlayId\":{4},\"Multiple\":{5},\"Pattern\":{6},\"Position\":\"{10}\",\"Quantity\":\"{7}\",\"Amount\":{8},\"Content\":\"{9}\")],\"ChaseNumList\":[],\"BetPwd\":null),\"extend\":null)";
                                str7 = string.Format(str7, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), base.UserID, this.GetPlayString(plan.Play), this.GetPlayMethodID(plan.Type, plan.Play), Convert.ToInt32(plan.AutoTimes(str4, true)), plan.Money, num, plan.AutoTotalMoney(str4, true), this.GetNumberList(plan.Type, pTNumberList, plan.Play, null), this.GetRXWZString1(plan.Type, plan.RXWZ) }).Replace("(", "{").Replace(")", "}");
                                HttpHelper.GetResponse1(ref pResponsetext, betsLine, "POST", str7, lotteryLine, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
                            }
                            flag = this.CheckReturn(pResponsetext, true);
                            if (flag)
                            {
                                string str8 = CommFunc.GetIndexString(pResponsetext, "\"data\":", ",", 0);
                                AppInfo.Account.BankBalance = Convert.ToDouble(str8);
                            }
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
            pHint.Contains("立即登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            ((pResponseText.Contains("\"message\":\"投注成功！\"") || pResponseText.Contains("\"ErrorMessage\":null")) || (pResponseText == "投注成功"));

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                if (!this.CheckLotteryIsVR(pType))
                {
                    string accountsMemLine = this.GetAccountsMemLine(pType);
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    string pData = string.Format("(\"data\":\"{0}\")", this.UserID).Replace("(", "{").Replace(")", "}");
                    HttpHelper.GetResponse1(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                    string str5 = CommFunc.GetIndexString(pResponsetext, "\"data\":", ",", 0);
                    AppInfo.Account.BankBalance = Convert.ToDouble(str5);
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType)
        {
            string str = this.GetLine() + "/api/openapi/getBalance";
            if (this.CheckLotteryIsVR(pType))
            {
                str = this.GetVRLine() + "/Home/GetWalletAmount";
            }
            return str;
        }

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
                    return str2.Replace("-0", "-");

                case "VRSSC":
                case "VRHXSSC":
                case "VR3FC":
                    str2 = str2.Replace("-", "");
                    break;
            }
            return str2;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType)
        {
            string str = this.GetLine() + "/api/openapi/startBet";
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
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "16";
            }
            if (pType == ConfigurationStatus.LotteryType.LFHGSSC)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.LFDJSSC)
            {
                return "12";
            }
            if (pType == ConfigurationStatus.LotteryType.MD2FC)
            {
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.YNSSC)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "3";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.WDYL5FC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.WDYL2FC)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.WDYLFFC)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "10";
            }
            if (pType == ConfigurationStatus.LotteryType.QQFFC)
            {
                return "15";
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
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "103";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "101";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "102";
            }
            if (pType == ConfigurationStatus.LotteryType.WDYL11X5)
            {
                return "104";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "301";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType)
        {
            string lotteryLine = this.GetLine() + "/api/openapi/getLotNum";
            if (this.CheckLotteryIsVR(pType))
            {
                lotteryLine = this.GetLotteryLine(pType, false);
            }
            return lotteryLine;
        }

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

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
                    return CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                        return CommFunc.Join(pNumberList, ",");
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
                    str = CommFunc.Join(pNumberList, ",");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
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
                str = CommFunc.Join(list, ",").Replace("*", "");
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
                    return "50";
                }
                if (playName == "前三直选复式")
                {
                    return "49";
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
                    return "22";
                }
                if (playName == "后三直选复式")
                {
                    return "21";
                }
                if (playName == "后三组三复式")
                {
                    return "26";
                }
                if (playName == "后三组三单式")
                {
                    return "27";
                }
                if (playName == "后三组六复式")
                {
                    return "28";
                }
                if (playName == "后三组六单式")
                {
                    return "29";
                }
                if (playName == "中三直选单式")
                {
                    return "36";
                }
                if (playName == "中三直选复式")
                {
                    return "35";
                }
                if (playName == "中三组三复式")
                {
                    return "40";
                }
                if (playName == "中三组三单式")
                {
                    return "41";
                }
                if (playName == "中三组六复式")
                {
                    return "42";
                }
                if (playName == "中三组六单式")
                {
                    return "43";
                }
                if (playName == "前二直选单式")
                {
                    return "72";
                }
                if (playName == "前二直选复式")
                {
                    return "71";
                }
                if (playName == "后二直选单式")
                {
                    return "64";
                }
                if (playName == "后二直选复式")
                {
                    return "63";
                }
                if (playName == "后四直选单式")
                {
                    return "15";
                }
                if (playName == "后四直选复式")
                {
                    return "14";
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
                    return "99";
                }
                if (playName == "任三直选复式")
                {
                    return "98";
                }
                if (playName == "任三组三单式")
                {
                    return "102";
                }
                if (playName == "任三组三复式")
                {
                    return "101";
                }
                if (playName == "任三组六单式")
                {
                    return "104";
                }
                if (playName == "任三组六复式")
                {
                    return "103";
                }
                if (playName == "任二直选单式")
                {
                    return "93";
                }
                if (playName == "任二直选复式")
                {
                    return "92";
                }
                if (playName == "任四直选单式")
                {
                    return "108";
                }
                if (playName == "任四直选复式")
                {
                    return "107";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "79";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "202";
                }
                if (playName == "前二直选单式")
                {
                    return "207";
                }
                if (playName == "任选复式一中一")
                {
                    return "215";
                }
                if (playName == "任选复式二中二")
                {
                    return "216";
                }
                if (playName == "任选复式三中三")
                {
                    return "217";
                }
                if (playName == "任选复式四中四")
                {
                    return "218";
                }
                if (playName == "任选复式五中五")
                {
                    return "219";
                }
                if (playName == "任选单式一中一")
                {
                    return "223";
                }
                if (playName == "任选单式二中二")
                {
                    return "224";
                }
                if (playName == "任选单式三中三")
                {
                    return "225";
                }
                if (playName == "任选单式四中四")
                {
                    return "226";
                }
                if (playName == "任选单式五中五")
                {
                    str = "227";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "606";
                }
                if (playName == "猜前三复式")
                {
                    return "605";
                }
                if (playName == "猜前二单式")
                {
                    return "604";
                }
                if (playName == "猜前二复式")
                {
                    return "603";
                }
                if (playName == "猜前四单式")
                {
                    return "608";
                }
                if (playName == "猜前四复式")
                {
                    return "607";
                }
                if (playName == "猜前五单式")
                {
                    return "610";
                }
                if (playName == "猜前五复式")
                {
                    return "609";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "602";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "601";
                }
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
                    return "[前三直选_单式]";
                }
                if (playName == "前三直选复式")
                {
                    return "[前三直选_复式]";
                }
                if (playName == "前三组三复式")
                {
                    return "[前三组选_组三复式]";
                }
                if (playName == "前三组三单式")
                {
                    return "[前三组选_组三单式]";
                }
                if (playName == "前三组六复式")
                {
                    return "[前三组选_组六复式]";
                }
                if (playName == "前三组六单式")
                {
                    return "[前三组选_组六单式]";
                }
                if (playName == "后三直选单式")
                {
                    return "[后三直选_单式]";
                }
                if (playName == "后三直选复式")
                {
                    return "[后三直选_复式]";
                }
                if (playName == "后三组三复式")
                {
                    return "[后三组选_组三复式]";
                }
                if (playName == "后三组三单式")
                {
                    return "[后三组选_组三单式]";
                }
                if (playName == "后三组六复式")
                {
                    return "[后三组选_组六复式]";
                }
                if (playName == "后三组六单式")
                {
                    return "[后三组选_组六单式]";
                }
                if (playName == "中三直选单式")
                {
                    return "[中三直选_单式]";
                }
                if (playName == "中三直选复式")
                {
                    return "[中三直选_复式]";
                }
                if (playName == "中三组三复式")
                {
                    return "[中三组选_组三复式]";
                }
                if (playName == "中三组三单式")
                {
                    return "[中三组选_组三单式]";
                }
                if (playName == "中三组六复式")
                {
                    return "[中三组选_组六复式]";
                }
                if (playName == "中三组六单式")
                {
                    return "[中三组选_组六单式]";
                }
                if (playName == "前二直选单式")
                {
                    return "[前二直选_单式]";
                }
                if (playName == "前二直选复式")
                {
                    return "[前二直选_复式]";
                }
                if (playName == "后二直选单式")
                {
                    return "[后二直选_单式]";
                }
                if (playName == "后二直选复式")
                {
                    return "[后二直选_复式]";
                }
                if (playName == "后四直选单式")
                {
                    return "[四星直选_单式]";
                }
                if (playName == "后四直选复式")
                {
                    return "[四星直选_复式]";
                }
                if (playName == "五星直选单式")
                {
                    return "[五星直选_单式]";
                }
                if (playName == "五星直选复式")
                {
                    return "[五星直选_复式]";
                }
                if (playName == "任三直选单式")
                {
                    return "[任三直选_直选单式]";
                }
                if (playName == "任三直选复式")
                {
                    return "[任三直选_直选复式]";
                }
                if (playName == "任三组三单式")
                {
                    return "[任三组选_组三单式]";
                }
                if (playName == "任三组三复式")
                {
                    return "[任三组选_组三复式]";
                }
                if (playName == "任三组六单式")
                {
                    return "[任三组选_组六单式]";
                }
                if (playName == "任三组六复式")
                {
                    return "[任三组选_组六复式]";
                }
                if (playName == "任二直选单式")
                {
                    return "[任二直选_直选单式]";
                }
                if (playName == "任二直选复式")
                {
                    return "[任选二_直选复式]";
                }
                if (playName == "任四直选单式")
                {
                    return "[任四直选_直选单式]";
                }
                if (playName == "任四直选复式")
                {
                    return "[任选四_直选复式]";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "[定位胆_定位胆1]";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "[三码_前三直选单式]";
                }
                if (playName == "前二直选单式")
                {
                    return "[二码_前二直选单式]";
                }
                if (playName == "任选复式一中一")
                {
                    return "[任选复式_任选一中一]";
                }
                if (playName == "任选复式二中二")
                {
                    return "[任选复式_任选二中二]";
                }
                if (playName == "任选复式三中三")
                {
                    return "[任选复式_任选三中三]";
                }
                if (playName == "任选复式四中四")
                {
                    return "[任选复式_任选四中四]";
                }
                if (playName == "任选复式五中五")
                {
                    return "[任选复式_任选五中五]";
                }
                if (playName == "任选单式一中一")
                {
                    return "[任选单式_任选一中一]";
                }
                if (playName == "任选单式二中二")
                {
                    return "[任选单式_任选二中二]";
                }
                if (playName == "任选单式三中三")
                {
                    return "[任选单式_任选三中三]";
                }
                if (playName == "任选单式四中四")
                {
                    return "[任选单式_任选四中四]";
                }
                if (playName == "任选单式五中五")
                {
                    str = "[任选单式_任选五中五]";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "[精确前三_单式]";
                }
                if (playName == "猜前三复式")
                {
                    return "[精确前三_精确前三]";
                }
                if (playName == "猜前二单式")
                {
                    return "[精确前二_单式]";
                }
                if (playName == "猜前二复式")
                {
                    return "[精确前二_精确前二]";
                }
                if (playName == "猜前四单式")
                {
                    return "[精确前四_单式]";
                }
                if (playName == "猜前四复式")
                {
                    return "[精确前四_精确前四]";
                }
                if (playName == "猜前五单式")
                {
                    return "[精确前五_单式]";
                }
                if (playName == "猜前五复式")
                {
                    return "[精确前五_精确前五]";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "[精确前一_猜冠军]";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "[定位胆_定位胆]";
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
            if (pRXWZ == null)
            {
                return str;
            }
            List<string> pList = new List<string>();
            for (num = 0; num < pRXWZ.Count; num++)
            {
                string item = AppInfo.IndexDic[pRXWZ[num]];
                pList.Add(item);
            }
            return CommFunc.Join(pList, ",");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.Prize == "")
                {
                    string pUrl = this.GetLine() + "/api/openapi/getBonus";
                    string lotteryLine = this.GetLotteryLine(pType, false);
                    string pResponsetext = "";
                    string pData = $"(data:{"1-50"})".Replace("(", "{").Replace(")", "}");
                    HttpHelper.GetResponse1(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                    if (pResponsetext.Contains("奖金或赔率"))
                    {
                        base.Prize = CommFunc.GetIndexString(pResponsetext, "\"data\":", ",", 0);
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
            string pUrl = $"{this.GetLine()}/api/vr/vrbet";
            string pResponsetext = "";
            string pSource = "5d000008003100000000000000003d8888865474ae395314711ece3cc6b332621a8f3fc95146e2206e2ecfc2ccb6bd89f2816db97ddb44c13544d9e876d0fad3d7bfffda8f70003ffd8861928c49c1e6de7e25180f05f0";
            string str6 = CommFunc.LZMAEncode(CommFunc.LZMADecode(pSource));
            string pData = $"(data:{str6})".Replace("(", "{").Replace(")", "}");
            HttpHelper.GetResponse1(ref pResponsetext, pUrl, "POST", pData, indexLine, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("data=");
            if (flag)
            {
                pVRData = CommFunc.GetIndexString(pResponsetext, "data=", "\"", 0);
            }
            return flag;
        }

        public override string GetVRLine() => 
            "http://wda.vrbetapi.com";

        public override string LoginLotteryWeb(ConfigurationStatus.LotteryType pType, string pInfo = "")
        {
            string lotteryLine = this.GetLotteryLine(pType, true);
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
            return pResponsetext;
        }

        public bool LoginVRWeb(ConfigurationStatus.LotteryType pType, string pVRData)
        {
            string pReferer = "";
            string pUrl = $"{this.GetVRLine()}/Account/LoginValidate?version=1.0&id=WDA&data={pVRData}";
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("竞速娱乐 VR") || (pResponsetext == "");
            if (flag)
            {
                base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
            }
            return flag;
        }

        public bool LoginWeb(string pID, string pW, ref string pHint)
        {
            string line = this.GetLine();
            string pUrl = this.GetLine() + "/api/openapi/getBalance";
            string pResponsetext = "";
            string pData = $"(data:{base.UserID})".Replace("(", "{").Replace(")", "}");
            HttpHelper.BetsTokenKey = "token";
            HttpHelper.BetsTokenValue = pW;
            HttpHelper.GetResponse1(ref pResponsetext, pUrl, "POST", pData, line, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("\"success\":true");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "\"message\":\"", "\"", 0);
                if ((pHint.Contains("不正确的数据") || pHint.Contains("值不能为 null")) || pHint.Contains("Unexpected character encountered while parsing value"))
                {
                    pHint = "挂机令牌不正确！";
                }
            }
            return flag;
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
            if (!this.LoginWeb(pID, pW, ref pHint))
            {
                return false;
            }
            return true;
        }
    }
}

