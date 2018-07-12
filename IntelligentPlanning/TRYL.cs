namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class TRYL : PTBase
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
                            string format = "%7B'type'%3A'{11}'%2C'methodid'%3A{0}%2C'codes'%3A'{1}'%2C'nums'%3A{2}%2C'times'%3A{3}%2C'money'%3A{4}%2C'mode'%3A{5}%2C'point'%3A'{6}'%2C'desc'%3A'{7}+{8}'%2C'position'%3A'{9}'%2C'curtimes'%3A'{10}'%7D";
                            int num = plan.FNNumber(str4);
                            format = string.Format(format, new object[] { this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), num, Convert.ToInt32(plan.AutoTimes(str4, true)), plan.AutoTotalMoney(str4, true), plan.Unit, "0", this.GetPlayString(plan.Play), this.GetNumberList2(pTNumberList, plan.Play), this.GetRXWZString(plan.RXWZ), "", str6 });
                            string str8 = "mainForm=mainForm&lotteryid={0}&flag=save&lt_sel_times={1}&lt_sel_modes={2}&lt_sel_dyprize={3}%7C{4}&lt_project%5B%5D={5}&lt_issue_start={6}&lt_total_nums={7}&lt_total_money={8}";
                            string prize = this.GetPrize(plan.Type, plan.Play);
                            str8 = string.Format(str8, new object[] { this.GetBetsLotteryID(plan.Type), Convert.ToInt32(plan.AutoTimes(str4, true)), plan.Unit, prize, base.Rebate, format, this.GetBetsExpect(plan.CurrentExpect, ""), num, plan.AutoTotalMoney(str4, true) });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str8, pReferer, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
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
            pHint.Contains("loginE()");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("{\"msg\":\"\"}") || (pResponseText == "投注成功"));

        public override void CountPrizeDic(string pResponseText)
        {
            base.PrizeDic.Clear();
            List<string> list = CommFunc.SplitString(CommFunc.GetIndexString(pResponseText, "pri_user_data", "pri_issues", 0), "}]}]},", -1);
            foreach (string str2 in list)
            {
                string str3 = CommFunc.GetIndexString(str2, "methodid:", ",", 0);
                string str4 = CommFunc.GetIndexString(str2, "'prize':", "}", str2.IndexOf("'point':0"));
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
                string pData = "flag=balance";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string str5 = CommFunc.GetIndexString(pResponsetext, "\"balance\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str5);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/LotteryService.aspx");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            if (pLotteryID == "")
            {
                pLotteryID = AppInfo.Current.Lottery.ID;
            }
            return CommFunc.ConvertBetsExpect(pExpect, pLotteryID, false, false, false);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/LotteryService.aspx");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "14";
            }
            if (pType == ConfigurationStatus.LotteryType.FLBSSC)
            {
                return "3";
            }
            if (pType == ConfigurationStatus.LotteryType.TRFFC)
            {
                return "15";
            }
            if (pType == ConfigurationStatus.LotteryType.TR2FC)
            {
                return "16";
            }
            if (pType == ConfigurationStatus.LotteryType.TR45C)
            {
                return "45";
            }
            if (pType == ConfigurationStatus.LotteryType.QJCTXFFC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.TR11X5)
            {
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "9";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "85";
            }
            if (pType == ConfigurationStatus.LotteryType.TRPK10)
            {
                str = "83";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/page/WORecord.shtml?id={0}&num={1}");

        public override string GetIndexLine() => 
            (this.GetLine() + "/main.shtml");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login.shtml");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/{this.GetPTLotteryName(pType)}.shtml";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            int num;
            string str3;
            int num2;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        string pStr = pNumberList[num];
                        str3 = CommFunc.Join(pStr, "%26", -1);
                        list.Add(str3);
                    }
                    return CommFunc.Join(list, "%7C").Replace("*", "");
                }
                if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num2 = AppInfo.FiveDic[ch.ToString()];
                    list = new List<string>();
                    for (num = 0; num < 5; num++)
                    {
                        str3 = "*";
                        if (num == num2)
                        {
                            str3 = CommFunc.Join(pNumberList, "%26");
                        }
                        list.Add(str3);
                    }
                    return CommFunc.Join(list, "%7C").Replace("*", "");
                }
                return CommFunc.Join(pNumberList, "%26");
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, "%26");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, "%26");
                }
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
                    str3 = pNumberList[num].Replace(" ", "&");
                    list.Add(str3);
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
                list = new List<string>();
                int num3 = (playName == "猜冠军猜冠军") ? 1 : 10;
                for (num = 0; num < num3; num++)
                {
                    str3 = "*";
                    if (num == num2)
                    {
                        str3 = CommFunc.Join(pNumberList, "&");
                    }
                    list.Add(str3);
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
                switch (CommFunc.GetLotteryID(pType))
                {
                    case "XJSSC":
                        if ((playName == "前三直选单式") || (playName == "前三直选复式"))
                        {
                            return "201";
                        }
                        if ((playName == "后三直选单式") || (playName == "后三直选复式"))
                        {
                            return "185";
                        }
                        if ((playName == "前三组三复式") || (playName == "前三组三单式"))
                        {
                            return "207";
                        }
                        if ((playName == "前三组六复式") || (playName == "前三组六单式"))
                        {
                            return "208";
                        }
                        if ((playName == "后三组三复式") || (playName == "后三组三单式"))
                        {
                            return "191";
                        }
                        if ((playName == "后三组六复式") || (playName == "后三组六单式"))
                        {
                            return "192";
                        }
                        if ((playName == "前二直选单式") || (playName == "前二直选复式"))
                        {
                            return "217";
                        }
                        if ((playName == "后二直选单式") || (playName == "后二直选复式"))
                        {
                            return "221";
                        }
                        if ((playName == "后四直选单式") || (playName == "后四直选复式"))
                        {
                            return "176";
                        }
                        if (playName.Contains("定位胆"))
                        {
                            str = "233";
                        }
                        return str;

                    case "TJSSC":
                        if ((playName == "前三直选单式") || (playName == "前三直选复式"))
                        {
                            return "802";
                        }
                        if ((playName == "后三直选单式") || (playName == "后三直选复式"))
                        {
                            return "786";
                        }
                        if ((playName == "前三组三复式") || (playName == "前三组三单式"))
                        {
                            return "808";
                        }
                        if ((playName == "前三组六复式") || (playName == "前三组六单式"))
                        {
                            return "809";
                        }
                        if ((playName == "后三组三复式") || (playName == "后三组三单式"))
                        {
                            return "792";
                        }
                        if ((playName == "后三组六复式") || (playName == "后三组六单式"))
                        {
                            return "793";
                        }
                        if ((playName == "前二直选单式") || (playName == "前二直选复式"))
                        {
                            return "818";
                        }
                        if ((playName == "后二直选单式") || (playName == "后二直选复式"))
                        {
                            return "822";
                        }
                        if ((playName == "后四直选单式") || (playName == "后四直选复式"))
                        {
                            return "777";
                        }
                        if (playName.Contains("定位胆"))
                        {
                            str = "834";
                        }
                        return str;
                }
                if ((playName == "前三直选单式") || (playName == "前三直选复式"))
                {
                    return "27";
                }
                if ((playName == "后三直选单式") || (playName == "后三直选复式"))
                {
                    return "11";
                }
                if ((playName == "中三直选单式") || (playName == "中三直选复式"))
                {
                    return "360";
                }
                if ((playName == "前三组三复式") || (playName == "前三组三单式"))
                {
                    return "33";
                }
                if ((playName == "前三组六复式") || (playName == "前三组六单式"))
                {
                    return "34";
                }
                if ((playName == "后三组三复式") || (playName == "后三组三单式"))
                {
                    return "17";
                }
                if ((playName == "后三组六复式") || (playName == "后三组六单式"))
                {
                    return "18";
                }
                if ((playName == "中三组三复式") || (playName == "中三组三单式"))
                {
                    return "364";
                }
                if ((playName == "中三组六复式") || (playName == "中三组六单式"))
                {
                    return "365";
                }
                if ((playName == "前二直选单式") || (playName == "前二直选复式"))
                {
                    return "43";
                }
                if ((playName == "后二直选单式") || (playName == "后二直选复式"))
                {
                    return "47";
                }
                if ((playName == "后四直选单式") || (playName == "后四直选复式"))
                {
                    return "2";
                }
                if ((playName == "五星直选单式") || (playName == "五星直选复式"))
                {
                    return "865";
                }
                if ((playName == "任三直选单式") || (playName == "任三直选复式"))
                {
                    return "676";
                }
                if ((playName == "任三组三复式") || (playName == "任三组三单式"))
                {
                    return "686";
                }
                if ((playName == "任三组六复式") || (playName == "任三组六单式"))
                {
                    return "696";
                }
                if ((playName == "任二直选单式") || (playName == "任二直选复式"))
                {
                    return "654";
                }
                if ((playName == "任四直选单式") || (playName == "任四直选复式"))
                {
                    return "725";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "59";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                switch (CommFunc.GetLotteryID(pType))
                {
                    case "GD11X5":
                        if (playName == "前三直选单式")
                        {
                            return "483";
                        }
                        if (playName == "前二直选单式")
                        {
                            return "488";
                        }
                        if ((playName == "任选复式一中一") || (playName == "任选单式一中一"))
                        {
                            return "503";
                        }
                        if ((playName == "任选复式二中二") || (playName == "任选单式二中二"))
                        {
                            return "505";
                        }
                        if ((playName == "任选复式三中三") || (playName == "任选单式三中三"))
                        {
                            return "508";
                        }
                        if ((playName == "任选复式四中四") || (playName == "任选单式四中四"))
                        {
                            return "511";
                        }
                        if ((playName == "任选复式五中五") || (playName == "任选单式五中五"))
                        {
                            str = "514";
                        }
                        return str;

                    case "JX11X5":
                        if (playName == "前三直选单式")
                        {
                            str = "440";
                        }
                        else if (playName == "前二直选单式")
                        {
                            str = "445";
                        }
                        if ((playName == "任选复式一中一") || (playName == "任选单式一中一"))
                        {
                            return "460";
                        }
                        if ((playName == "任选复式二中二") || (playName == "任选单式二中二"))
                        {
                            return "462";
                        }
                        if ((playName == "任选复式三中三") || (playName == "任选单式三中三"))
                        {
                            return "465";
                        }
                        if ((playName == "任选复式四中四") || (playName == "任选单式四中四"))
                        {
                            return "468";
                        }
                        if ((playName == "任选复式五中五") || (playName == "任选单式五中五"))
                        {
                            str = "471";
                        }
                        return str;
                }
                if (playName == "前三直选单式")
                {
                    str = "90";
                }
                else if (playName == "前二直选单式")
                {
                    str = "94";
                }
                if ((playName == "任选复式一中一") || (playName == "任选单式一中一"))
                {
                    return "108";
                }
                if ((playName == "任选复式二中二") || (playName == "任选单式二中二"))
                {
                    return "110";
                }
                if ((playName == "任选复式三中三") || (playName == "任选单式三中三"))
                {
                    return "112";
                }
                if ((playName == "任选复式四中四") || (playName == "任选单式四中四"))
                {
                    return "114";
                }
                if ((playName == "任选复式五中五") || (playName == "任选单式五中五"))
                {
                    str = "116";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if ((playName == "猜前三单式") || (playName == "猜前三复式"))
                {
                    return "1218";
                }
                if ((playName == "猜前二单式") || (playName == "猜前二复式"))
                {
                    return "1217";
                }
                if ((playName == "猜前四单式") || (playName == "猜前四复式"))
                {
                    return "1219";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "1216";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "1215";
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
                    return HttpUtility.UrlEncode("[前三直选_单式]");
                }
                if (playName == "前三直选复式")
                {
                    return HttpUtility.UrlEncode("[前三直选_复式]");
                }
                if (playName == "前三组三复式")
                {
                    return HttpUtility.UrlEncode("[前三组选_组三复式]");
                }
                if (playName == "前三组三单式")
                {
                    return HttpUtility.UrlEncode("[前三组选_组三单式]");
                }
                if (playName == "前三组六复式")
                {
                    return HttpUtility.UrlEncode("[前三组选_组六复式]");
                }
                if (playName == "前三组六单式")
                {
                    return HttpUtility.UrlEncode("[前三组选_组六单式]");
                }
                if (playName == "后三直选单式")
                {
                    return HttpUtility.UrlEncode("[后三直选_单式]");
                }
                if (playName == "后三直选复式")
                {
                    return HttpUtility.UrlEncode("[后三直选_复式]");
                }
                if (playName == "后三组三复式")
                {
                    return HttpUtility.UrlEncode("[后三组选_组三复式]");
                }
                if (playName == "后三组三单式")
                {
                    return HttpUtility.UrlEncode("[后三组选_组三单式]");
                }
                if (playName == "后三组六复式")
                {
                    return HttpUtility.UrlEncode("[后三组选_组六复式]");
                }
                if (playName == "后三组六单式")
                {
                    return HttpUtility.UrlEncode("[后三组选_组六单式]");
                }
                if (playName == "中三直选单式")
                {
                    return HttpUtility.UrlEncode("[中三直选_单式]");
                }
                if (playName == "中三直选复式")
                {
                    return HttpUtility.UrlEncode("[中三直选_复式]");
                }
                if (playName == "中三组三复式")
                {
                    return HttpUtility.UrlEncode("[中三组选_组三复式]");
                }
                if (playName == "中三组三单式")
                {
                    return HttpUtility.UrlEncode("[中三组选_组三单式]");
                }
                if (playName == "中三组六复式")
                {
                    return HttpUtility.UrlEncode("[中三组选_组六复式]");
                }
                if (playName == "中三组六单式")
                {
                    return HttpUtility.UrlEncode("[中三组选_组六单式]");
                }
                if (playName == "前二直选单式")
                {
                    return HttpUtility.UrlEncode("[前二直选_单式]");
                }
                if (playName == "前二直选复式")
                {
                    return HttpUtility.UrlEncode("[前二直选_复式]");
                }
                if (playName == "后二直选单式")
                {
                    return HttpUtility.UrlEncode("[后二直选_单式]");
                }
                if (playName == "后二直选复式")
                {
                    return HttpUtility.UrlEncode("[后二直选_复式]");
                }
                if (playName == "后四直选单式")
                {
                    return HttpUtility.UrlEncode("[四星直选_单式]");
                }
                if (playName == "后四直选复式")
                {
                    return HttpUtility.UrlEncode("[四星直选_复式]");
                }
                if (playName == "五星直选单式")
                {
                    return HttpUtility.UrlEncode("[五星直选_单式]");
                }
                if (playName == "五星直选复式")
                {
                    return HttpUtility.UrlEncode("[五星直选_复式]");
                }
                if (playName == "任三直选单式")
                {
                    return HttpUtility.UrlEncode("[任三直选_直选单式]");
                }
                if (playName == "任三直选复式")
                {
                    return HttpUtility.UrlEncode("[任三直选_直选复式]");
                }
                if (playName == "任三组三复式")
                {
                    return HttpUtility.UrlEncode("[任三组选_组三复式]");
                }
                if (playName == "任三组三单式")
                {
                    return HttpUtility.UrlEncode("[任三组选_组三单式]");
                }
                if (playName == "任三组六复式")
                {
                    return HttpUtility.UrlEncode("[任三组选_组六复式]");
                }
                if (playName == "任三组六单式")
                {
                    return HttpUtility.UrlEncode("[任三组选_组六单式]");
                }
                if (playName == "任二直选单式")
                {
                    return HttpUtility.UrlEncode("[任二直选_直选单式]");
                }
                if (playName == "任二直选复式")
                {
                    return HttpUtility.UrlEncode("[任选二_直选复式]");
                }
                if (playName == "任四直选单式")
                {
                    return HttpUtility.UrlEncode("[任四直选_直选单式]");
                }
                if (playName == "任四直选复式")
                {
                    return HttpUtility.UrlEncode("[任选四_直选复式]");
                }
                if (playName.Contains("定位胆"))
                {
                    str = HttpUtility.UrlEncode("[定位胆_定位胆]");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return HttpUtility.UrlEncode("[三码_前三直选单式]");
                }
                if (playName == "前二直选单式")
                {
                    return HttpUtility.UrlEncode("[二码_前二直选单式]");
                }
                if (playName == "任选复式一中一")
                {
                    return HttpUtility.UrlEncode("[任选复式_任选一中一]");
                }
                if (playName == "任选复式二中二")
                {
                    return HttpUtility.UrlEncode("[任选复式_任选二中二]");
                }
                if (playName == "任选复式三中三")
                {
                    return HttpUtility.UrlEncode("[任选复式_任选三中三]");
                }
                if (playName == "任选复式四中四")
                {
                    return HttpUtility.UrlEncode("[任选复式_任选四中四]");
                }
                if (playName == "任选复式五中五")
                {
                    return HttpUtility.UrlEncode("[任选复式_任选五中五]");
                }
                if (playName == "任选单式一中一")
                {
                    return HttpUtility.UrlEncode("[任选单式_任选一中一]");
                }
                if (playName == "任选单式二中二")
                {
                    return HttpUtility.UrlEncode("[任选单式_任选二中二]");
                }
                if (playName == "任选单式三中三")
                {
                    return HttpUtility.UrlEncode("[任选单式_任选三中三]");
                }
                if (playName == "任选单式四中四")
                {
                    return HttpUtility.UrlEncode("[任选单式_任选四中四]");
                }
                if (playName == "任选单式五中五")
                {
                    str = HttpUtility.UrlEncode("[任选单式_任选五中五]");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return HttpUtility.UrlEncode("直选_直选单式");
                }
                if (playName == "猜前三复式")
                {
                    return HttpUtility.UrlEncode("精确前三_直选复式");
                }
                if (playName == "猜前二单式")
                {
                    return HttpUtility.UrlEncode("直选_直选单式");
                }
                if (playName == "猜前二复式")
                {
                    return HttpUtility.UrlEncode("精确前二_直选复式");
                }
                if (playName == "猜前四单式")
                {
                    return HttpUtility.UrlEncode("直选_直选单式");
                }
                if (playName == "猜前四复式")
                {
                    return HttpUtility.UrlEncode("精确前四_直选复式");
                }
                if (playName == "猜冠军猜冠军")
                {
                    return HttpUtility.UrlEncode("精确前一_直选复式");
                }
                if (playName.Contains("定位胆"))
                {
                    str = HttpUtility.UrlEncode("定位胆_定位胆");
                }
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
            return CommFunc.GetIndexString(pResponseText, "\"msg\":\"", "\"", 0);
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
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "ssctj";
            }
            if (pType == ConfigurationStatus.LotteryType.FLBSSC)
            {
                return "hg1_5";
            }
            if (pType == ConfigurationStatus.LotteryType.TRFFC)
            {
                return "ssccs";
            }
            if (pType == ConfigurationStatus.LotteryType.TR2FC)
            {
                return "sscbf";
            }
            if (pType == ConfigurationStatus.LotteryType.TR45C)
            {
                return "ssccs";
            }
            if (pType == ConfigurationStatus.LotteryType.QJCTXFFC)
            {
                return "ssccs";
            }
            if (pType == ConfigurationStatus.LotteryType.TR11X5)
            {
                return "115cs";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "115gd";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "115sd";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "115jx";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "pk105f";
            }
            if (pType == ConfigurationStatus.LotteryType.TRPK10)
            {
                str = "pk10";
            }
            return str;
        }

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            string str = "";
            if (pRXWZ == null)
            {
                return str;
            }
            List<int> pList = new List<int>();
            for (int i = 0; i < pRXWZ.Count; i++)
            {
                pList.Add(pRXWZ[i]);
            }
            return CommFunc.Join(pList, "%26");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            if (base.PrizeDic.Count == 0)
            {
                string pResponseText = this.LoginLotteryWeb(pType, "");
                if (pResponseText != "")
                {
                    this.CountPrizeDic(pResponseText);
                }
            }
        }

        public bool InputWeb(string pID, string pW, ref string pHint)
        {
            bool flag = false;
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLoginLine();
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string pData = string.Format("AJAXREQUEST=LGForm%3Aj_id3&LGForm=LGForm&txtName={0}&txtPsw={1}&LGForm%3AwaitingInfoPanelOpenedState=&javax.faces.ViewState=j_id1&LGForm%3Aj_id4=LGForm%3Aj_id4&pwd={1}&un={0}", pID, str4);
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("main.shtml");
            if (!flag)
            {
                pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "[CDATA[ '", "'", 0));
                if ((AppInfo.App == ConfigurationStatus.AppType.OpenData) && pHint.Contains("频繁"))
                {
                    Thread.Sleep(0x493e0);
                    return this.InputWeb(pID, pW, ref pHint);
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
            return (pResponsetext != "");
        }

        public override void QuitPT()
        {
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

