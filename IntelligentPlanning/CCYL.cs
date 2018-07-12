namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web;

    public class CCYL : PTBase
    {
        public string DataToken = "";

        public override bool BetsMain(ConfigurationStatus.SCPlan plan, ref string pHint)
        {
            bool flag = false;
            try
            {
                string betsLine = this.GetBetsLine(plan.Type);
                string indexLine = this.GetIndexLine();
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
                            string str6 = "0";
                            string str7 = Guid.NewGuid().ToString();
                            if (this.CheckLotteryIsVR(plan.Type))
                            {
                                str8 = "LotteryGameID={0}&IssueSerialNumber=&Bets%5B0%5D%5BBetTypeCode%5D={1}&Bets%5B0%5D%5BBetTypeName%5D=&Bets%5B0%5D%5BNumber%5D={2}&Bets%5B0%5D%5BPosition%5D={3}&Bets%5B0%5D%5BUnit%5D={4}&Bets%5B0%5D%5BMultiple%5D={5}&Bets%5B0%5D%5BIsCompressed%5D=false&StopIfWin=false&__RequestVerificationToken={6}&SerialNumber={7}&Guid={8}";
                                str8 = string.Format(str8, new object[] { this.GetBetsLotteryID(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList(plan.Type, pTNumberList, plan.Play, null), this.GetRXWZString1(plan.Type, plan.RXWZ), plan.Money, Convert.ToInt32(plan.AutoTimes(str4, true)), base.Token, this.GetBetsExpect(plan.CurrentExpect, ""), str7 });
                                HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str8, indexLine, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
                            }
                            else
                            {
                                str8 = "api_token={11}&orders%5B0%5D%5Bbase_bet_amount%5D={0}&orders%5B0%5D%5Bbet_amount%5D={1}&orders%5B0%5D%5Bbet_count%5D={2}&orders%5B0%5D%5Bcategory_id%5D={3}&orders%5B0%5D%5Bchunk%5D={4}&orders%5B0%5D%5Bcompressed%5D={5}&orders%5B0%5D%5Bcontent%5D={6}&orders%5B0%5D%5Bissue%5D={7}&orders%5B0%5D%5Bmultiple%5D={8}&orders%5B0%5D%5Bplay_id%5D={9}&orders%5B0%5D%5Bselected_feedback_percent%5D={10}";
                                string prize = base.Prize;
                                str8 = string.Format(str8, new object[] { plan.Money, plan.AutoTotalMoney(str4, true), num, this.GetBetsLotteryID(plan.Type), str6, "0", this.GetNumberList(plan.Type, pTNumberList, plan.Play, plan.RXWZ), this.GetBetsExpect(plan.CurrentExpect, ""), Convert.ToInt32(plan.AutoTimes(str4, true)), this.GetPlayMethodID(plan.Type, plan.Play), "0", this.DataToken });
                                HttpHelper.GetResponse8(ref pResponsetext, betsLine, "POST", str8, indexLine, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
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
            (pHint.Contains("\"code\":2004") || pHint.Contains("\"code\":2005"));

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            ((pResponseText.Contains("\"message\":\"OK\"") || pResponseText.Contains("\"ErrorMessage\":null")) || (pResponseText == "投注成功"));

        public override void CountPrizeDic(string pResponseText)
        {
            base.PlayMethodDic.Clear();
            List<string> list = CommFunc.SplitString(pResponseText, "},{", -1);
            for (int i = 0; i < list.Count; i++)
            {
                string pStr = list[i];
                string key = CommFunc.UniconToString(CommFunc.GetIndexString(pStr, "\"display_name\":\"", "\"", 0));
                string str3 = CommFunc.GetIndexString(pStr, "\"id\":", ",", 0);
                if (!((key == "") || base.PlayMethodDic.ContainsKey(key)))
                {
                    base.PlayMethodDic[key] = str3;
                }
            }
        }

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                if (!AppInfo.PTInfo.PTIsBreak)
                {
                    string accountsMemLine = this.GetAccountsMemLine(pType);
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    string pData = "";
                    HttpHelper.GetResponse8(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                    if (this.CheckBreakConnect(pResponsetext))
                    {
                        AppInfo.PTInfo.PTIsBreak = true;
                    }
                    else
                    {
                        string str5 = CommFunc.GetIndexString(pResponsetext, "\"balance\":", "}", pResponsetext.IndexOf("WALLET"));
                        AppInfo.Account.BankBalance = Convert.ToDouble(str5);
                    }
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/proxy/game/AG/balance/refresh?api_token={this.DataToken}";

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, true, true, false);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType)
        {
            string str = $"{this.GetLine()}/proxy/lottery/game/bet";
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
                return "15";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "16";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "102";
            }
            if (pType == ConfigurationStatus.LotteryType.CCTWSSCGB)
            {
                return "131";
            }
            if (pType == ConfigurationStatus.LotteryType.CCHGSSCGB)
            {
                return "130";
            }
            if (pType == ConfigurationStatus.LotteryType.CCDJSSCGB)
            {
                return "129";
            }
            if (pType == ConfigurationStatus.LotteryType.CCTJ3FC)
            {
                return "27";
            }
            if (pType == ConfigurationStatus.LotteryType.CCTJ5FC)
            {
                return "28";
            }
            if (pType == ConfigurationStatus.LotteryType.CCTG60M)
            {
                return "26";
            }
            if (pType == ConfigurationStatus.LotteryType.CCXG15C)
            {
                return "72";
            }
            if (pType == ConfigurationStatus.LotteryType.CCFLP15C)
            {
                return "125";
            }
            if (pType == ConfigurationStatus.LotteryType.CCRD2FC)
            {
                return "127";
            }
            if (pType == ConfigurationStatus.LotteryType.CCWXFFC)
            {
                return "123";
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
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "21";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "19";
            }
            if (pType == ConfigurationStatus.LotteryType.CCTW11X5)
            {
                return "29";
            }
            if (pType == ConfigurationStatus.LotteryType.CCAM11X5)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.CCXG11X5)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "32";
            }
            if (pType == ConfigurationStatus.LotteryType.CCFFPK10)
            {
                str = "95";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType)
        {
            string lotteryLine = $"{this.GetLine()}/proxy/lottery/game/category/{this.GetPTLotteryName(pType)}/drew?condition=%7B%22status%22:300%7D&curpage=1&perpage=30&sort=%7B%22publish_time%22:-1%7D&api_token={this.DataToken}";
            if (this.CheckLotteryIsVR(pType))
            {
                lotteryLine = this.GetLotteryLine(pType, false);
            }
            return lotteryLine;
        }

        public override string GetIndexLine() => 
            (this.GetLine() + "/#/site/index");

        public override string GetLoginLine() => 
            (this.GetLine() + "/");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false)
        {
            string str = "";
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
                        str = CommFunc.Join(pNumberList);
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                if (CommFunc.CheckPlayIsRXDS(playName))
                {
                    str = this.GetRXWZString1(pType, pRXWZ) + str;
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
                    str = CommFunc.Join(pNumberList, " ");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                    str = CommFunc.Join(list, ",").Replace("*", "");
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
                    return "前三直选单式";
                }
                if (playName == "前三直选复式")
                {
                    return "前三直选复式";
                }
                if (playName == "前三组三复式")
                {
                    return "前三组3";
                }
                if (playName == "前三组六复式")
                {
                    return "前三组6";
                }
                if (playName == "后三直选单式")
                {
                    return "后三直选单式";
                }
                if (playName == "后三直选复式")
                {
                    return "后三直选复式";
                }
                if (playName == "后三组三复式")
                {
                    return "后三组3";
                }
                if (playName == "后三组六复式")
                {
                    return "后三组6";
                }
                if (playName == "中三直选单式")
                {
                    return "中三直选单式";
                }
                if (playName == "中三直选复式")
                {
                    return "中三直选复式";
                }
                if (playName == "中三组三复式")
                {
                    return "中三组3";
                }
                if (playName == "中三组六复式")
                {
                    return "中三组6";
                }
                if (playName == "前二直选单式")
                {
                    return "前二单式";
                }
                if (playName == "前二直选复式")
                {
                    return "前二复式";
                }
                if (playName == "后二直选单式")
                {
                    return "后二单式";
                }
                if (playName == "后二直选复式")
                {
                    return "后二复式";
                }
                if (playName == "前四直选单式")
                {
                    return "前四直选单式";
                }
                if (playName == "前四直选复式")
                {
                    return "前四直选复式";
                }
                if (playName == "后四直选单式")
                {
                    return "后四直选单式";
                }
                if (playName == "后四直选复式")
                {
                    return "后四直选复式";
                }
                if (playName == "五星直选单式")
                {
                    return "五星单式";
                }
                if (playName == "五星直选复式")
                {
                    return "五星复式";
                }
                if (playName == "任三直选单式")
                {
                    return "任选三直选单式";
                }
                if (playName == "任三直选复式")
                {
                    return "任选三直选复式";
                }
                if (playName == "任三组三复式")
                {
                    return "任选三组3";
                }
                if (playName == "任三组六复式")
                {
                    return "任选三组6";
                }
                if (playName == "任二直选单式")
                {
                    return "任选二直选单式";
                }
                if (playName == "任二直选复式")
                {
                    return "任选二直选复式";
                }
                if (playName == "任四直选单式")
                {
                    return "任选四直选单式";
                }
                if (playName == "任四直选复式")
                {
                    return "任选四直选复式";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "五星定位胆";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "前三直选单式";
                }
                if (playName == "前二直选单式")
                {
                    return "前二直选单式";
                }
                if (playName == "任选复式一中一")
                {
                    return "复式一中一";
                }
                if (playName == "任选复式二中二")
                {
                    return "复式二中二";
                }
                if (playName == "任选复式三中三")
                {
                    return "复式三中三";
                }
                if (playName == "任选复式四中四")
                {
                    return "复式四中四";
                }
                if (playName == "任选复式五中五")
                {
                    return "复式五中五";
                }
                if (playName == "任选单式一中一")
                {
                    return "单式一中一";
                }
                if (playName == "任选单式二中二")
                {
                    return "单式二中二";
                }
                if (playName == "任选单式三中三")
                {
                    return "单式三中三";
                }
                if (playName == "任选单式四中四")
                {
                    return "单式四中四";
                }
                if (playName == "任选单式五中五")
                {
                    str = "单式五中五";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "单式前三";
                }
                if (playName == "猜前三复式")
                {
                    return "猜名次猜前三";
                }
                if (playName == "猜前二单式")
                {
                    return "单式前二";
                }
                if (playName == "猜前二复式")
                {
                    return "猜名次猜前二";
                }
                if (playName == "猜前四单式")
                {
                    return "单式前四";
                }
                if (playName == "猜前四复式")
                {
                    return "猜名次猜前四";
                }
                if (playName == "猜前五单式")
                {
                    return "单式前五";
                }
                if (playName == "猜前五复式")
                {
                    return "猜名次猜前五";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "猜名次猜冠军";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "定位胆后五" : "定位胆前五";
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
                str = CommFunc.GetIndexString(pResponseText, "\"code\":", ",", 0);
            }
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
                return "cqssc";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "xjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "tencent";
            }
            if (pType == ConfigurationStatus.LotteryType.CCTWSSCGB)
            {
                return "gbtwbingo";
            }
            if (pType == ConfigurationStatus.LotteryType.CCHGSSCGB)
            {
                return "gbkrkeno";
            }
            if (pType == ConfigurationStatus.LotteryType.CCDJSSCGB)
            {
                return "gbjpkeno";
            }
            if (pType == ConfigurationStatus.LotteryType.CCTJ3FC)
            {
                return "mp3fc";
            }
            if (pType == ConfigurationStatus.LotteryType.CCTJ5FC)
            {
                return "mp5fc";
            }
            if (pType == ConfigurationStatus.LotteryType.CCTG60M)
            {
                return "mpffc";
            }
            if (pType == ConfigurationStatus.LotteryType.CCXG15C)
            {
                return "mpfbc";
            }
            if (pType == ConfigurationStatus.LotteryType.CCFLP15C)
            {
                return "mpfbc2";
            }
            if (pType == ConfigurationStatus.LotteryType.CCRD2FC)
            {
                return "mp2fc";
            }
            if (pType == ConfigurationStatus.LotteryType.CCWXFFC)
            {
                return "mpffc1";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "gd11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "sd11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "jx11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.CCTW11X5)
            {
                return "mp5f11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.CCAM11X5)
            {
                return "mpff11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.CCXG11X5)
            {
                return "mp3f11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "bjpk10";
            }
            if (pType == ConfigurationStatus.LotteryType.CCFFPK10)
            {
                str = "mpffpk10";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            $"{this.GetLine()}/proxy/logout";

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString1(ConfigurationStatus.LotteryType pType, List<int> pRXWZ)
        {
            string str = "";
            if (this.CheckLotteryIsVR(pType))
            {
                if (pRXWZ == null)
                {
                    return str;
                }
                List<int> pList = new List<int>();
                for (int i = 0; i < pRXWZ.Count; i++)
                {
                    pList.Add(pRXWZ[i] + 1);
                }
                return CommFunc.Join(pList, ",");
            }
            return (CommFunc.Join<int>(pRXWZ) + ":");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (!this.CheckLotteryIsVR(pType))
                {
                    string pUrl = $"{this.GetLine()}/proxy/lottery/game/category/{this.GetPTLotteryName(pType)}/recent-issues/4?api_token={this.DataToken}";
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    HttpHelper.GetResponse8(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        base.Expect = CommFunc.GetIndexString(pResponsetext, "\"data\":{\"", "\"", 0);
                        base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
                    }
                    if (base.PlayMethodDic.Count == 0)
                    {
                        pUrl = $"{this.GetLine()}/proxy/lottery/game/category/{this.GetPTLotteryName(pType)}/plays";
                        indexLine = this.GetIndexLine();
                        pResponsetext = "";
                        HttpHelper.GetResponse8(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                        if (pResponsetext != "")
                        {
                            this.CountPrizeDic(pResponsetext);
                        }
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
            string pUrl = $"{this.GetLine()}/proxy/game/VR/login";
            string pResponsetext = "";
            string pData = $"token={this.DataToken}";
            HttpHelper.GetResponse8(ref pResponsetext, pUrl, "POST", pData, indexLine, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("data=");
            if (flag)
            {
                pVRData = CommFunc.GetIndexString(pResponsetext, "data=", @"\u0026", 0);
            }
            return flag;
        }

        public override string GetVRLine() => 
            "http://xc.vrbetapi.com";

        public bool InputWeb(string pID, string pW, ref string pHint)
        {
            bool flag = false;
            string loginLine = this.GetLoginLine();
            string str2 = HttpUtility.UrlEncode(pW);
            string pUrl = $"{this.GetLine()}/proxy/login?api_token={str2}";
            string pResponsetext = "";
            HttpHelper.GetResponse8(ref pResponsetext, pUrl, "GET", string.Empty, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains($"username:{pID}");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "\"code\":", ",", 0);
                if (pHint == "5000")
                {
                    pHint = "挂机令牌不存在！";
                    return flag;
                }
                if (pHint == "1000")
                {
                    pHint = "会员账号和挂机令牌不匹配！";
                }
                return flag;
            }
            this.DataToken = str2;
            base.Rebate = CommFunc.GetIndexString(pResponsetext, "\"feedback\":\"", "\"", 0);
            base.Prize = (1800.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0)).ToString();
            return flag;
        }

        public bool LoginVRWeb(ConfigurationStatus.LotteryType pType, string pVRData)
        {
            string pReferer = "";
            string pUrl = $"{this.GetVRLine()}/Account/LoginValidate?version=1.0&data={pVRData}&id=XC";
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("竞速娱乐 VR");
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
            HttpHelper.GetResponse8(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return (pResponsetext != "");
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            string pData = $"api_token={this.DataToken}";
            HttpHelper.GetResponse8(ref pResponsetext, quitPTLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
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
            this.DataToken = "";
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

