namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class XHHC : PTBase
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
                            int num = Convert.ToInt32(plan.AutoTimes(str4, true));
                            int num2 = plan.FNNumber(str4);
                            double num3 = plan.AutoTotalMoney(str4, true);
                            string str6 = CommFunc.CheckPlayIsDS(plan.Play) ? "true" : "false";
                            string play = plan.Play;
                            List<string> list2 = new List<string>();
                            Dictionary<string, List<string>> dictionary3 = new Dictionary<string, List<string>>();
                            int count = CommFunc.GetPlayInfo(play).IndexList.Count;
                            if (CommFunc.CheckPlayIsRX(play))
                            {
                                int num5;
                                if (CommFunc.CheckPlayIsRXDS(play))
                                {
                                    List<string> itemList = new List<string>();
                                    num5 = 0;
                                    while (num5 < plan.RXWZ.Count)
                                    {
                                        string item = plan.RXWZ[num5].ToString();
                                        itemList.Add(item);
                                        num5++;
                                    }
                                    list2 = CommFunc.GetCombinations(itemList, count, "");
                                    num2 = CommFunc.GetBetsCodeCount(pTNumberList, play, null);
                                    num3 = (num2 * num) * plan.Money;
                                }
                                else if (CommFunc.CheckPlayIsRXFS(play))
                                {
                                    List<string> list4 = new List<string>();
                                    num5 = 0;
                                    while (num5 < pTNumberList.Count)
                                    {
                                        string str9 = $"{num5}-{pTNumberList[num5]}";
                                        list4.Add(str9);
                                        num5++;
                                    }
                                    List<string> list5 = CommFunc.GetCombinations(list4, count, "|");
                                    for (num5 = 0; num5 < list5.Count; num5++)
                                    {
                                        string pStr = list5[num5];
                                        if (!pStr.Contains("*"))
                                        {
                                            List<string> list6 = new List<string>();
                                            List<string> list7 = new List<string>();
                                            List<string> list8 = CommFunc.SplitString(pStr, "|", -1);
                                            for (int j = 0; j < list8.Count; j++)
                                            {
                                                string str11 = list8[j];
                                                list6.Add(str11.Split(new char[] { '-' })[0]);
                                                list7.Add(str11.Split(new char[] { '-' })[1]);
                                            }
                                            string str12 = CommFunc.Join(list6);
                                            dictionary3[str12] = list7;
                                            list2.Add(str12);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                list2.Add("0");
                            }
                            List<string> pList = new List<string>();
                            for (int i = 0; i < list2.Count; i++)
                            {
                                string str13 = list2[i];
                                if (CommFunc.CheckPlayIsRXFS(play))
                                {
                                    string playName = "";
                                    switch (count)
                                    {
                                        case 2:
                                            playName = "前二直选复式";
                                            break;

                                        case 3:
                                            playName = "前三直选复式";
                                            break;

                                        case 4:
                                            playName = "前四直选复式";
                                            break;
                                    }
                                    List<string> pCodeList = dictionary3[str13];
                                    num2 = CommFunc.GetBetsCodeCount(pCodeList, playName, null);
                                    pTNumberList = CommFunc.CopyList(pCodeList);
                                    num3 = (num2 * num) * plan.Money;
                                }
                                List<int> pIndexList = CommFunc.ConvertSameListInt(str13);
                                string str15 = "%7B%22i%22%3A{0}%2C%22c%22%3A%22{1}%22%2C%22n%22%3A{2}%2C%22t%22%3A%22{3}%22%2C%22k%22%3A{4}%2C%22m%22%3A{5}%2C%22a%22%3A{6}%7D";
                                str15 = string.Format(str15, new object[] { this.GetPlayMethodID1(plan.Type, play, pIndexList), this.GetNumberList1(pTNumberList, plan.Play, null), num2, num, "0", plan.Unit, num3 });
                                pList.Add(str15);
                            }
                            string str16 = CommFunc.Join(pList, "%2C");
                            string format = "gameId={0}&periodId={1}&isSingle={2}&canAdvance=false&orderList=%5B{3}%5D";
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), base.ExpectID, str6, str16 });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, AppInfo.PTInfo.BetsTime2, "UTF-8", true);
                            flag = this.CheckReturn(pResponsetext, true);
                            pHint = this.GetReturn(pResponsetext);
                            Thread.Sleep(0xbb8);
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
            (pHint.Contains("登录") || pHint.Contains("/Home/AgainLogin?type=500"));

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"info\":\"投注成功！\"") || (pResponseText == "投注成功"));

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
            base.PlayMethodGroupDic.Clear();
            base.PrizeDic.Clear();
            pResponseText = CommFunc.GetIndexString(pResponseText, "var navListJson = '", "]}]}]}]';", 0);
            List<string> list = CommFunc.SplitString(pResponseText, "]}]}]},{", -1);
            for (int i = 0; i < list.Count; i++)
            {
                string pStr = list[i];
                string str2 = CommFunc.GetIndexString(pStr, "\"N\":\"", "\"", 0);
                List<string> list2 = CommFunc.SplitString(pStr.Substring(pStr.IndexOf("{\"N\":\"")), "}]}]},{", -1);
                for (int j = 0; j < list2.Count; j++)
                {
                    string str3 = list2[j];
                    string str4 = CommFunc.GetIndexString(str3, "\"N\":\"", "\"", 0);
                    List<string> list3 = CommFunc.SplitString(str3.Substring(str3.IndexOf("\"C\"")), "]},{", -1);
                    for (int k = 0; k < list3.Count; k++)
                    {
                        string str5 = list3[k];
                        string str6 = CommFunc.GetIndexString(str5, "\"N\":\"", "\"", 0);
                        List<string> list4 = CommFunc.SplitString(str5.Substring(str5.IndexOf("\"TP\"")), "},{", -1);
                        for (int m = 0; m < list4.Count; m++)
                        {
                            string str7 = list4[m];
                            string str8 = CommFunc.GetIndexString(str7, "\"N\":\"", "\"", 0);
                            ConfigurationStatus.PTPrizeGroup group = new ConfigurationStatus.PTPrizeGroup();
                            string str9 = CommFunc.GetIndexString(str7, "\"I\":", ",", 0);
                            group.PlayID = str9;
                            group.PlayName = $"{str2}-{str4}-{str6}-{str8}";
                            base.PlayMethodGroupDic[group.PlayName] = group;
                            base.PrizeDic[str9] = Convert.ToDouble(CommFunc.GetIndexString(str7, "\"MxO\":\"", "\"", 0)).ToString();
                        }
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
                    string accountsMemLine = this.GetAccountsMemLine(pType);
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                    if (this.CheckBreakConnect(pResponsetext))
                    {
                        AppInfo.PTInfo.PTIsBreak = true;
                    }
                    else
                    {
                        string str4 = CommFunc.GetIndexString(pResponsetext, "\"CreditBalance\":\"", "\"", 0);
                        AppInfo.Account.BankBalance = Convert.ToDouble(str4);
                    }
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/AccountInfo/GetAccount?_={DateTime.Now.ToOADate()}";

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/OfficialAddOrders/AddOrders");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "26";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "40";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "42";
            }
            if (pType == ConfigurationStatus.LotteryType.LFHGSSC)
            {
                return "51";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "57";
            }
            if (pType == ConfigurationStatus.LotteryType.JNDSSC)
            {
                return "64";
            }
            if (pType == ConfigurationStatus.LotteryType.XHHCOZ3FC)
            {
                return "67";
            }
            if (pType == ConfigurationStatus.LotteryType.XHHCSLFK5FC)
            {
                return "72";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "81";
            }
            if (pType == ConfigurationStatus.LotteryType.QQFFC)
            {
                return "82";
            }
            if (pType == ConfigurationStatus.LotteryType.LFDJSSC)
            {
                return "109";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "32";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "46";
            }
            if (pType == ConfigurationStatus.LotteryType.HLJ11X5)
            {
                return "50";
            }
            if (pType == ConfigurationStatus.LotteryType.JS11X5)
            {
                return "74";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "76";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "29";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/Result/GetLotteryResultList?gameID={this.GetBetsLotteryID(pType)}&pagesize=6&pageIndex=1&_={DateTime.Now.ToOADate()}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/HomePage");

        public override string GetLoginLine() => 
            (this.GetLine() + "/");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/OffcialOtherGame/Index/{this.GetBetsLotteryID(pType)}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    List<string> pList = new List<string>();
                    for (int i = 0; i < pNumberList.Count; i++)
                    {
                        string item = CommFunc.Join(pNumberList[i], ",", -1);
                        pList.Add(item);
                    }
                    str = CommFunc.Join(pList, "|").Replace("*", "");
                }
                else if (playName.Contains("定位胆"))
                {
                    str = CommFunc.Join(pNumberList, "|").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, "|");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "_");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, "_").Replace(" ", "");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, "|");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, "_").Replace(" ", "");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, "|").Replace(" ", ",").Replace("*", "");
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "|").Replace("*", "");
                    if (playName == "猜冠军猜冠军")
                    {
                        str = str.Replace("|", ",");
                    }
                }
            }
            return HttpUtility.UrlEncode(str);
        }

        public ConfigurationStatus.PTPrizeGroup GetPlayMethodGroup(ConfigurationStatus.LotteryType pType, string playName, List<int> pIndexList)
        {
            ConfigurationStatus.PTPrizeGroup group = null;
            string playString = this.GetPlayString(playName);
            string rXWZString = this.GetRXWZString(pIndexList);
            if (CommFunc.CheckPlayIsRX(playName))
            {
                playString = string.Format(playString, rXWZString);
            }
            if (base.PlayMethodGroupDic.ContainsKey(playString))
            {
                group = base.PlayMethodGroupDic[playString];
            }
            return group;
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            List<int> pIndexList = new List<int>();
            if (CommFunc.CheckPlayIsRX(playName))
            {
                int count = CommFunc.GetPlayInfo(playName).IndexList.Count;
                for (int i = 0; i < count; i++)
                {
                    pIndexList.Add(i);
                }
            }
            ConfigurationStatus.PTPrizeGroup group = this.GetPlayMethodGroup(pType, playName, pIndexList);
            return ((group == null) ? "" : group.PlayID);
        }

        public string GetPlayMethodID1(ConfigurationStatus.LotteryType pType, string playName, List<int> pIndexList)
        {
            ConfigurationStatus.PTPrizeGroup group = this.GetPlayMethodGroup(pType, playName, pIndexList);
            return ((group == null) ? "" : group.PlayID);
        }

        public override string GetPlayString(string playName)
        {
            string str2;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "前三码-前三直选-前三直选单式-单式";
                }
                if (playName == "前三直选复式")
                {
                    return "前三码-前三直选-前三直选复式-复式";
                }
                if (playName == "前三组三复式")
                {
                    return "前三码-前三组选-前三组三-组三";
                }
                if (playName == "前三组六复式")
                {
                    return "前三码-前三组选-前三组六-组六";
                }
                if (playName == "后三直选单式")
                {
                    return "后三码-后三直选-后三直选单式-单式";
                }
                if (playName == "后三直选复式")
                {
                    return "后三码-后三直选-后三直选复式-复式";
                }
                if (playName == "后三组三复式")
                {
                    return "后三码-后三组选-后三组三-组三";
                }
                if (playName == "后三组六复式")
                {
                    return "后三码-后三组选-后三组六-组六";
                }
                if (playName == "中三直选单式")
                {
                    return "中三码-中三直选-中三直选单式-单式";
                }
                if (playName == "中三直选复式")
                {
                    return "中三码-中三直选-中三直选复式-复式";
                }
                if (playName == "中三组三复式")
                {
                    return "中三码-中三组选-中三组三-组三";
                }
                if (playName == "中三组六复式")
                {
                    return "中三码-中三组选-中三组六-组六";
                }
                if (playName == "前二直选单式")
                {
                    return "二码-二星直选-前二直选(单式)-前二直选(单式)";
                }
                if (playName == "前二直选复式")
                {
                    return "二码-二星直选-前二直选(复式)-前二直选(复式)";
                }
                if (playName == "后二直选单式")
                {
                    return "二码-二星直选-后二直选(单式)-后二直选(单式)";
                }
                if (playName == "后二直选复式")
                {
                    return "二码-二星直选-后二直选(复式)-后二直选(复式)";
                }
                if (playName == "前四直选单式")
                {
                    return "前四-前四直选-前四单式-单式";
                }
                if (playName == "前四直选复式")
                {
                    return "前四-前四直选-前四复式-复式";
                }
                if (playName == "后四直选单式")
                {
                    return "后四-后四直选-后四单式-单式";
                }
                if (playName == "后四直选复式")
                {
                    return "后四-后四直选-后四复式-复式";
                }
                if (playName == "五星直选单式")
                {
                    return "五星-五星直选-五星单式-单式";
                }
                if (playName == "五星直选复式")
                {
                    return "五星-五星直选-五星复式-复式";
                }
                if (playName == "任三直选单式")
                {
                    return "任选三-任三直选-任三直选单式-{0}直选单式";
                }
                if (playName == "任三直选复式")
                {
                    return "任选三-任三直选-任三直选复式-{0}直选复式";
                }
                if (playName == "任三组三复式")
                {
                    return "任选三-任三组选-任三组三-{0}组三";
                }
                if (playName == "任三组六复式")
                {
                    return "任选三-任三组选-任三组六-{0}组六";
                }
                if (playName == "任二直选单式")
                {
                    return "任选二-任二直选-任二直选单式-{0}直选单式";
                }
                if (playName == "任二直选复式")
                {
                    return "任选二-任二直选-任二直选复式-{0}直选复式";
                }
                if (playName == "任四直选单式")
                {
                    return "任选四-任四直选-任四直选单式-{0}直选单式";
                }
                if (playName == "任四直选复式")
                {
                    return "任选四-任四直选-任四直选复式-{0}直选复式";
                }
                if (!playName.Contains("定位胆"))
                {
                    return str;
                }
                if (CommFunc.CheckPlayIsDWD(playName))
                {
                    return "";
                }
                str2 = playName.Substring(3, 2);
                return ("定位胆-定位胆-定位胆-定位胆" + str2);
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "三码-前三-前三直选单式-前三直选单式";
                }
                if (playName == "前二直选单式")
                {
                    return "二码-前二-前二直选单式-前二直选单式";
                }
                if (playName == "任选复式一中一")
                {
                    return "任选复式-任选复式-一中一-一中一";
                }
                if (playName == "任选复式二中二")
                {
                    return "任选复式-任选复式-二中二-二中二";
                }
                if (playName == "任选复式三中三")
                {
                    return "任选复式-任选复式-三中三-三中三";
                }
                if (playName == "任选复式四中四")
                {
                    return "任选复式-任选复式-四中四-四中四";
                }
                if (playName == "任选复式五中五")
                {
                    return "任选复式-任选复式-五中五-五中五";
                }
                if (playName == "任选单式一中一")
                {
                    return "任选单式-任选单式-一中一-一中一";
                }
                if (playName == "任选单式二中二")
                {
                    return "任选单式-任选单式-二中二-二中二";
                }
                if (playName == "任选单式三中三")
                {
                    return "任选单式-任选单式-三中三-三中三";
                }
                if (playName == "任选单式四中四")
                {
                    return "任选单式-任选单式-四中四-四中四";
                }
                if (playName == "任选单式五中五")
                {
                    str = "任选单式-任选单式-五中五-五中五";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (playName == "猜前三单式")
            {
                return "猜前三-猜前三-猜前三单式-猜中三";
            }
            if (playName == "猜前三复式")
            {
                return "猜前三-猜前三-猜前三复式-猜中三";
            }
            if (playName == "猜前二单式")
            {
                return "猜前二-猜前二-猜前二单式-猜中二";
            }
            if (playName == "猜前二复式")
            {
                return "猜前二-猜前二-猜前二复式-猜中二";
            }
            if (playName == "猜前四单式")
            {
                return "猜前四-猜前四-猜前四单式-猜中四";
            }
            if (playName == "猜前四复式")
            {
                return "猜前四-猜前四-猜前四复式-猜中四";
            }
            if (playName == "猜前五单式")
            {
                return "猜前五-猜前五-猜前五单式-猜中五";
            }
            if (playName == "猜前五复式")
            {
                return "猜前五-猜前五-猜前五复式-猜中五";
            }
            if (playName == "猜冠军猜冠军")
            {
                return "猜冠军-猜冠军-猜冠军-猜冠军";
            }
            if (!playName.Contains("定位胆"))
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDWD(playName))
            {
                return "";
            }
            str2 = "";
            if (playName.Contains("冠军"))
            {
                str2 = "一";
            }
            else if (playName.Contains("亚军"))
            {
                str2 = "二";
            }
            else
            {
                str2 = playName.Substring(4, 1);
            }
            return ("定位胆-定位胆-定位胆-第" + str2);
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
            return CommFunc.GetIndexString(pResponseText, "\"info\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/Home/UserLogout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            List<string> pList = new List<string>();
            for (int i = 0; i < pRXWZ.Count; i++)
            {
                string item = AppInfo.IndexDic[pRXWZ[i]];
                pList.Add(item);
            }
            return CommFunc.Join(pList);
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string pUrl = $"{this.GetLine()}/Shared/GetNewPeriod?gameid={this.GetBetsLotteryID(pType)}&_={DateTime.Now.ToOADate()}";
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"fnumberofperiod\":\"", "\"", 0);
                    base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
                    base.ExpectID = CommFunc.GetIndexString(pResponsetext, "\"fid\":\"", "\"", 0);
                }
                if (base.PlayMethodGroupDic.Count == 0)
                {
                    pUrl = this.GetLotteryLine(pType, false);
                    lotteryLine = this.GetLotteryLine(pType, false);
                    pResponsetext = "";
                    HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        this.CountPrizeDic(pResponsetext);
                    }
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
                string str2 = $"/Home/ValidateCode?{DateTime.Now.ToOADate()}";
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
                string pUrl = this.GetLine() + "/Home/login";
                string pResponsetext = "";
                string str5 = CommFunc.WebMD51(CommFunc.WebMD51(pW) + webVerifyCode);
                string pData = $"username={pID}&password={str5}&validateCode={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("\"info\":\"登录成功\"");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"info\":\"", "\"", 0);
                    if (pHint.Contains("验证码"))
                    {
                        pHint = "";
                        return this.InputWeb(pID, pW, ref pHint);
                    }
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
            return (pResponsetext != "");
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
            if (!this.InputWeb(pID, pW, ref pHint))
            {
                return false;
            }
            return true;
        }
    }
}

