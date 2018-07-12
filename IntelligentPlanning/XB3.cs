namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web;

    public class XB3 : PTBase
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
                            double num2 = Convert.ToDouble(base.Rebate) * 10.0;
                            string format = "(\"LotteryCode\":\"{0}\",\"BettingCount\":{1},\"GraduationCount\":{2},\"IssueNo\":\"{3}\",\"Multiple\":{4},\"LotteryPlayDetailCode\":\"{5}\",\"SelectMedian\":\"{6}\",\"IsChaseNo\":false,\"BettingNumber\":\"{7}\",\"BettingInfoIssueList\":[(\"IssueNo\":\"{3}\",\"Multiple\":{4},\"PerPrice\":{8})],\"RuleName\":\"{9}\",\"BettingMoney\":\"{10}\",\"BuyIssueCount\":{11})";
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), num, num2, this.GetBetsExpect(plan.CurrentExpect, ""), Convert.ToInt32(plan.AutoTimes(str4, true)), this.GetPlayMethodID(plan.Type, plan.Play), this.GetRXWZString(plan.RXWZ, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), plan.Money, this.GetPlayString(plan.Play), plan.AutoTotalMoney(str4, true), "1" }).Replace("(", "{").Replace(")", "}");
                            HttpHelper.GetResponse1(ref pResponsetext, betsLine, "POST", format, pReferer, base.BetsTime4, "UTF-8", true);
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
            pHint.Contains("重新登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"投注成功\"") || (pResponseText == "投注成功"));

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse1(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = pResponsetext;
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/api/User/GetUserMoney");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string name = AppInfo.Current.Lottery.Name;
            string iD = AppInfo.Current.Lottery.ID;
            string str3 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false).Replace("-", "");
            if (((((iD == "XBFFC") || (iD == "GDFFC")) || name.Contains("11选5")) || (iD == "QQFFC")) && (str3.Substring(8, 1) == "0"))
            {
                str3 = str3.Remove(8, 1);
            }
            return str3;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/api/Lottery/betting");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "10001";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "10003";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "10004";
            }
            if (pType == ConfigurationStatus.LotteryType.HGSSC)
            {
                return "10011";
            }
            if (pType == ConfigurationStatus.LotteryType.QQFFC)
            {
                return "10018";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "10012";
            }
            if (pType == ConfigurationStatus.LotteryType.XJPSSC)
            {
                return "10015";
            }
            if (pType == ConfigurationStatus.LotteryType.XBFFC)
            {
                return "10007";
            }
            if (pType == ConfigurationStatus.LotteryType.XB3FC)
            {
                return "10006";
            }
            if (pType == ConfigurationStatus.LotteryType.XB5FC)
            {
                return "10005";
            }
            if (pType == ConfigurationStatus.LotteryType.BDSSC)
            {
                return "10016";
            }
            if (pType == ConfigurationStatus.LotteryType.GGSSC)
            {
                return "10017";
            }
            if (pType == ConfigurationStatus.LotteryType.GDFFC)
            {
                return "10002";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "20002";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "20004";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "20001";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "20009";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "20008";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "80001";
            }
            if (pType == ConfigurationStatus.LotteryType.XYFTPK10)
            {
                str = "80002";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/api/Lottery/GetHistory?lotteryCode={this.GetBetsLotteryID(pType)}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/#/lottery/{this.GetBetsLotteryID(pType)}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            int num;
            List<string> list;
            int num2;
            string str2;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    return CommFunc.Join(pNumberList, ",").Replace("*", "-");
                }
                if (playName.Contains("定位胆"))
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
                    return CommFunc.Join(list, ",").Replace("*", "-");
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
                    str = CommFunc.Join(pNumberList, ",");
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
                list = new List<string>();
                for (num2 = 0; num2 < pNumberList.Count; num2++)
                {
                    str2 = pNumberList[num2].Replace(" ", "");
                    list.Add(str2);
                }
                return CommFunc.Join(list, ",").Replace("*", "");
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
                    str2 = CommFunc.Join(pNumberList);
                }
                list.Add(str2);
            }
            return CommFunc.Join(list, ",").Replace("*", "");
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "10041";
                }
                if (playName == "前三直选复式")
                {
                    return "10040";
                }
                if (playName == "前三组三复式")
                {
                    return "10044";
                }
                if (playName == "前三组三单式")
                {
                    return "10146";
                }
                if (playName == "前三组六复式")
                {
                    return "10045";
                }
                if (playName == "前三组六单式")
                {
                    return "10150";
                }
                if (playName == "后三直选单式")
                {
                    return "10034";
                }
                if (playName == "后三直选复式")
                {
                    return "100333";
                }
                if (playName == "后三组三复式")
                {
                    return "10037";
                }
                if (playName == "后三组三单式")
                {
                    return "10145";
                }
                if (playName == "后三组六复式")
                {
                    return "10038";
                }
                if (playName == "后三组六单式")
                {
                    return "10149";
                }
                if (playName == "中三直选单式")
                {
                    return "10060";
                }
                if (playName == "中三直选复式")
                {
                    return "10059";
                }
                if (playName == "中三组三复式")
                {
                    return "10063";
                }
                if (playName == "中三组三单式")
                {
                    return "10144";
                }
                if (playName == "中三组六复式")
                {
                    return "10064";
                }
                if (playName == "中三组六单式")
                {
                    return "10148";
                }
                if (playName == "前二直选单式")
                {
                    return "10054";
                }
                if (playName == "前二直选复式")
                {
                    return "10053";
                }
                if (playName == "后二直选单式")
                {
                    return "10048";
                }
                if (playName == "后二直选复式")
                {
                    return "10047";
                }
                if (playName == "后四直选单式")
                {
                    return "10027";
                }
                if (playName == "后四直选复式")
                {
                    return "10026";
                }
                if (playName == "五星直选单式")
                {
                    return "10018";
                }
                if (playName == "五星直选复式")
                {
                    return "10017";
                }
                if (playName == "任三直选单式")
                {
                    return "10081";
                }
                if (playName == "任三直选复式")
                {
                    return "10080";
                }
                if (playName == "任三组三复式")
                {
                    return "10083";
                }
                if (playName == "任三组三单式")
                {
                    return "10147";
                }
                if (playName == "任三组六复式")
                {
                    return "10084";
                }
                if (playName == "任三组六单式")
                {
                    return "10151";
                }
                if (playName == "任二直选单式")
                {
                    return "10075";
                }
                if (playName == "任二直选复式")
                {
                    return "10074";
                }
                if (playName == "任四直选单式")
                {
                    return "10088";
                }
                if (playName == "任四直选复式")
                {
                    return "10087";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "10015";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "20017";
                }
                if (playName == "前二直选单式")
                {
                    return "20018";
                }
                if (playName == "任选复式一中一")
                {
                    return "20034";
                }
                if (playName == "任选复式二中二")
                {
                    return "20001";
                }
                if (playName == "任选复式三中三")
                {
                    return "20003";
                }
                if (playName == "任选复式四中四")
                {
                    return "20005";
                }
                if (playName == "任选复式五中五")
                {
                    return "20007";
                }
                if (playName == "任选单式一中一")
                {
                    return "20033";
                }
                if (playName == "任选单式二中二")
                {
                    return "20002";
                }
                if (playName == "任选单式三中三")
                {
                    return "20004";
                }
                if (playName == "任选单式四中四")
                {
                    return "20006";
                }
                if (playName == "任选单式五中五")
                {
                    str = "20008";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "80005";
                }
                if (playName == "猜前三复式")
                {
                    return "80004";
                }
                if (playName == "猜前二单式")
                {
                    return "80003";
                }
                if (playName == "猜前二复式")
                {
                    return "80002";
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
                    return "80001";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "80007" : "80006";
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
                    return "前三_直选单式";
                }
                if (playName == "前三直选复式")
                {
                    return "前三_直选复式";
                }
                if (playName == "前三组三复式")
                {
                    return "前三_组三复式";
                }
                if (playName == "前三组三单式")
                {
                    return "前三_组三单式";
                }
                if (playName == "前三组六复式")
                {
                    return "前三_组六复式";
                }
                if (playName == "前三组六单式")
                {
                    return "前三_组六单式";
                }
                if (playName == "后三直选单式")
                {
                    return "后三_直选单式";
                }
                if (playName == "后三直选复式")
                {
                    return "后三_直选复式";
                }
                if (playName == "后三组三复式")
                {
                    return "后三_组三复式";
                }
                if (playName == "后三组三单式")
                {
                    return "后三_组三单式";
                }
                if (playName == "后三组六复式")
                {
                    return "后三_组六复式";
                }
                if (playName == "后三组六单式")
                {
                    return "后三_组六单式";
                }
                if (playName == "中三直选单式")
                {
                    return "中三_直选单式";
                }
                if (playName == "中三直选复式")
                {
                    return "中三_直选复式";
                }
                if (playName == "中三组三复式")
                {
                    return "中三_组三复式";
                }
                if (playName == "中三组三单式")
                {
                    return "中三_组三单式";
                }
                if (playName == "中三组六复式")
                {
                    return "中三_组六复式";
                }
                if (playName == "中三组六单式")
                {
                    return "中三_组六单式";
                }
                if (playName == "前二直选单式")
                {
                    return "前二_直选单式";
                }
                if (playName == "前二直选复式")
                {
                    return "前二_直选复式";
                }
                if (playName == "后二直选单式")
                {
                    return "后二_直选单式";
                }
                if (playName == "后二直选复式")
                {
                    return "后二_直选复式";
                }
                if (playName == "后四直选单式")
                {
                    return "四星_直选单式";
                }
                if (playName == "后四直选复式")
                {
                    return "四星_直选复式";
                }
                if (playName == "五星直选单式")
                {
                    return "五星_直选单式";
                }
                if (playName == "五星直选复式")
                {
                    return "五星_直选复式";
                }
                if (playName == "任三直选单式")
                {
                    return "任选三_直选单式";
                }
                if (playName == "任三直选复式")
                {
                    return "任选三_直选复式";
                }
                if (playName == "任三组三复式")
                {
                    return "任选三_组三复式";
                }
                if (playName == "任三组三单式")
                {
                    return "任选三_组三单式";
                }
                if (playName == "任三组六复式")
                {
                    return "任选三_组六复式";
                }
                if (playName == "任三组六单式")
                {
                    return "任选三_组六单式";
                }
                if (playName == "任二直选单式")
                {
                    return "任选二_直选单式";
                }
                if (playName == "任二直选复式")
                {
                    return "任选二_直选复式";
                }
                if (playName == "任四直选单式")
                {
                    return "任选四_直选单式";
                }
                if (playName == "任四直选复式")
                {
                    return "任选四_直选复式";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "定位胆_五星定位胆";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "三码_前三直选单式";
                }
                if (playName == "前二直选单式")
                {
                    return "二码_前二直选单式";
                }
                if (playName == "任选复式一中一")
                {
                    return "任选复式_任选一中一";
                }
                if (playName == "任选复式二中二")
                {
                    return "任选复式_任选二中二";
                }
                if (playName == "任选复式三中三")
                {
                    return "任选复式_任选三中三";
                }
                if (playName == "任选复式四中四")
                {
                    return "任选复式_任选四中四";
                }
                if (playName == "任选复式五中五")
                {
                    return "任选复式_任选五中五";
                }
                if (playName == "任选单式一中一")
                {
                    return "任选单式_任选一中一";
                }
                if (playName == "任选单式二中二")
                {
                    return "任选单式_任选二中二";
                }
                if (playName == "任选单式三中三")
                {
                    return "任选单式_任选三中三";
                }
                if (playName == "任选单式四中四")
                {
                    return "任选单式_任选四中四";
                }
                if (playName == "任选单式五中五")
                {
                    str = "任选单式_任选五中五";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "猜前三名_单式";
                }
                if (playName == "猜前三复式")
                {
                    return "猜前三名_复式";
                }
                if (playName == "猜前二单式")
                {
                    return "猜冠亚军_单式";
                }
                if (playName == "猜前二复式")
                {
                    return "猜冠亚军_复式";
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
                    return "猜冠军_复式";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "定位胆_第6-10名(复式)" : "定位胆_第1-5名(复式)";
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
            return CommFunc.GetIndexString(pResponseText, "\"Error\": \"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/login/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString(List<int> pRXWZ, string playName)
        {
            ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(playName);
            if (CommFunc.CheckPlayIsRXDS(playName))
            {
                List<string> pList = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    string item = pRXWZ.Contains(i) ? "1" : "0";
                    pList.Add(item);
                }
                return CommFunc.Join(pList);
            }
            if (CommFunc.CheckPlayIsRXFS(playName))
            {
                switch (playInfo.IndexList.Count)
                {
                    case 2:
                        return "00011";

                    case 3:
                        return "00111";

                    case 4:
                        return "01111";
                }
            }
            return "11111";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string pUrl = $"{this.GetLine()}/api/Lottery/GetIssue?lotteryCode={this.GetBetsLotteryID(pType)}";
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse1(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                if ((pResponsetext != "") && (pType != ConfigurationStatus.LotteryType.XJPSSC))
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"CurrentIssueNo\": \"", "\"", 0);
                    base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
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
            string pUrl = this.GetLine() + "/api/log/login";
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string pData = $"Name={pID}&Pwd={str4}&ValidateCode=";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains(pID);
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "<Error>", "</Error>", 0);
                return flag;
            }
            base.Rebate = CommFunc.GetIndexString(pResponsetext, "<Bonus>", "</Bonus>", 0);
            base.Prize = (1800.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0)).ToString();
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x4e20, "UTF-8", true);
            return pResponsetext.Contains("新宝");
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

