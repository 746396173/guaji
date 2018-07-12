namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web;

    public class HCZX : PTBase
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
                            double num2 = Convert.ToDouble(base.Rebate) * 20.0;
                            string rXWZString = this.GetRXWZString(plan.RXWZ, plan.Play);
                            string str7 = CommFunc.Random(0x2710, 0x1869f).ToString();
                            string format = "(\"amount\":{0},\"gameType\":\"{1}\",\"isTrace\":0,\"traceWinStop\":0,\"balls\":[(\"selectPosition\":\"{2}\",\"selectMedia\":\"{3}\",\"id\":\"{4}\",\"ball\":\"{5}\",\"type\":\"{6}\",\"moneyunit\":\"{7}\",\"multiple\":\"{8}\",\"num\":\"{9}\",\"graduationCount\":\"{10}\")],\"orders\":[(\"number\":\"{11}\",\"issueCode\":\"{11}\",\"multiple\":1)])";
                            format = string.Format(format, new object[] { plan.AutoTotalMoney(str4, true), this.GetBetsLotteryID(plan.Type), rXWZString, rXWZString, str7, this.GetNumberList1(pTNumberList, plan.Play, null), this.GetPlayMethodID(plan.Type, plan.Play), plan.Money / 2.0, Convert.ToInt32(plan.AutoTimes(str4, true)), num, num2, this.GetBetsExpect(plan.CurrentExpect, "") }).Replace("(", "{").Replace(")", "}");
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
            (pResponseText.Contains("\"msg\":\"投注成功~~~\"") || (pResponseText == "投注成功"));

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pData = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string str5 = CommFunc.GetIndexString(pResponsetext, "\"UserMoney\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str5);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/home/getuserMoney");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, true, true, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/GameBetSubmit/BettingCommand");

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
            if (pType == ConfigurationStatus.LotteryType.HCZX5FC)
            {
                return "10006";
            }
            if (pType == ConfigurationStatus.LotteryType.HCZXJNDSSC)
            {
                return "10020";
            }
            if (pType == ConfigurationStatus.LotteryType.HCZXDJSSC)
            {
                return "10016";
            }
            if (pType == ConfigurationStatus.LotteryType.HCZXFFC)
            {
                return "10019";
            }
            if (pType == ConfigurationStatus.LotteryType.HCZXMG45M)
            {
                return "10007";
            }
            if (pType == ConfigurationStatus.LotteryType.HCZXNTXFFC)
            {
                return "10005";
            }
            if (pType == ConfigurationStatus.LotteryType.XXLSSC)
            {
                return "10018";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "10014";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "20002";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "20001";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "60002";
            }
            if (pType == ConfigurationStatus.LotteryType.HCZXFFPK10)
            {
                return "60004";
            }
            if (pType == ConfigurationStatus.LotteryType.HCZX3FPK10)
            {
                str = "60003";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/GameBet/getHistoryOpen?lotteryCode={this.GetBetsLotteryID(pType)}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/home/index");

        public override string GetLoginLine() => 
            (this.GetLine() + "/Account/Login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/GameBet/GameBetPage?lotteryCode={this.GetBetsLotteryID(pType)}";

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
                    return CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                        str = CommFunc.Join(pNumberList);
                    }
                    return str;
                }
                return CommFunc.Join(pNumberList, ",");
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, ",").Replace(" ", "");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList);
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                return CommFunc.Join(pNumberList, ",").Replace(" ", "");
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                return CommFunc.Join(pNumberList, ",").Replace(" ", "").Replace("*", "");
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
            return CommFunc.Join(list, ",").Replace("*", "-");
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "三星.直选单式.前三直选";
                }
                if (playName == "前三直选复式")
                {
                    return "三星.直选复式.前三选位";
                }
                if (playName == "前三组三复式")
                {
                    return "三星.组三.前三组三";
                }
                if (playName == "前三组六复式")
                {
                    return "三星.组六.前三组六";
                }
                if (playName == "后三直选单式")
                {
                    return "三星.直选单式.后三直选";
                }
                if (playName == "后三直选复式")
                {
                    return "三星.直选复式.后三选位";
                }
                if (playName == "后三组三复式")
                {
                    return "三星.组三.后三组三";
                }
                if (playName == "后三组六复式")
                {
                    return "三星.组六.后三组六";
                }
                if (playName == "中三直选单式")
                {
                    return "三星.直选单式.中三直选";
                }
                if (playName == "中三直选复式")
                {
                    return "三星.直选复式.中三选位";
                }
                if (playName == "中三组三复式")
                {
                    return "三星.组三.中三组三";
                }
                if (playName == "中三组六复式")
                {
                    return "三星.组六.中三组六";
                }
                if (playName == "前二直选单式")
                {
                    return "二星.直选单式.前二直选";
                }
                if (playName == "前二直选复式")
                {
                    return "二星.直选复式.前二选位";
                }
                if (playName == "后二直选单式")
                {
                    return "二星.直选单式.后二直选";
                }
                if (playName == "后二直选复式")
                {
                    return "二星.直选复式.后二选位";
                }
                if (playName == "前四直选单式")
                {
                    return "四星.直选单式.前四选位";
                }
                if (playName == "前四直选复式")
                {
                    return "四星.直选复式.前四直选";
                }
                if (playName == "后四直选单式")
                {
                    return "四星.直选单式.后四选位";
                }
                if (playName == "后四直选复式")
                {
                    return "四星.直选复式.后四直选";
                }
                if (playName == "五星直选单式")
                {
                    return "五星.直选单式.常规";
                }
                if (playName == "五星直选复式")
                {
                    return "五星.直选复式.常规";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "定位胆.五星定位胆.五星定位胆";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "任三直选.直选单式.前三选位";
                }
                if (playName == "前二直选单式")
                {
                    return "任二直选.直选单式.前二选位";
                }
                if (playName == "任选复式一中一")
                {
                    return "";
                }
                if (playName == "任选复式二中二")
                {
                    return "任选二.直选复式.常规";
                }
                if (playName == "任选复式三中三")
                {
                    return "任选三.直选复式.常规";
                }
                if (playName == "任选复式四中四")
                {
                    return "任选四.直选复式.常规";
                }
                if (playName == "任选复式五中五")
                {
                    return "任选五.直选复式.常规";
                }
                if (playName == "任选单式一中一")
                {
                    return "";
                }
                if (playName == "任选单式二中二")
                {
                    return "任选二.直选单式.常规";
                }
                if (playName == "任选单式三中三")
                {
                    return "任选三.直选单式.常规";
                }
                if (playName == "任选单式四中四")
                {
                    return "任选四.直选单式.常规";
                }
                if (playName == "任选单式五中五")
                {
                    str = "任选五.直选单式.常规";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "猜前三名.单式投注.常规";
                }
                if (playName == "猜前三复式")
                {
                    return "猜前三名.复式投注.常规";
                }
                if (playName == "猜前二单式")
                {
                    return "猜前两名.单式投注.常规";
                }
                if (playName == "猜前二复式")
                {
                    return "猜前两名.复式投注.常规";
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
                    return "猜冠军.复式投注.常规";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "定位胆.后五名.常规" : "定位胆.前五名.常规";
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
            return CommFunc.GetIndexString(pResponseText, "\"msg\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/Home/LoginOut");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString(List<int> pRXWZ, string playName)
        {
            int num;
            string str2;
            string str = "11111";
            ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(playName);
            List<string> pList = new List<string>();
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsRXDS(playName))
                {
                    for (num = 0; num < 5; num++)
                    {
                        str2 = pRXWZ.Contains(num) ? "1" : "0";
                        pList.Add(str2);
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
                            str = "01111";
                            break;
                    }
                    return str;
                }
                if (playName.Contains("定位胆"))
                {
                    return "11111";
                }
                for (num = 0; num < 5; num++)
                {
                    str2 = playInfo.IndexList.Contains(num + 1) ? "1" : "0";
                    pList.Add(str2);
                }
                return CommFunc.Join(pList);
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    for (num = 0; num < 5; num++)
                    {
                        str2 = playInfo.IndexList.Contains(num + 1) ? "1" : "0";
                        pList.Add(str2);
                    }
                    return CommFunc.Join(pList);
                }
                return "11111";
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName) || CommFunc.CheckPlayIsFS(playName))
            {
                for (num = 0; num < 10; num++)
                {
                    str2 = playInfo.IndexList.Contains(num + 1) ? "1" : "0";
                    pList.Add(str2);
                }
                return CommFunc.Join(pList);
            }
            if (playName == "猜冠军猜冠军")
            {
                return "1000000000";
            }
            return ((CommFunc.GetPlayNum(playName) > 5) ? "0000011111" : "1111100000");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string pUrl = $"{this.GetLine()}/GameBet/dynamicConfig?lotteryCode={this.GetBetsLotteryID(pType)}";
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pData = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"number\":\"", "\"", 0);
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
            string pUrl = this.GetLoginLine();
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string pData = $"username={pID}&password={str4}&RmPsw=&vcode=&__RequestVerificationToken=&notRem=false";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains(pID) && pResponsetext.Contains("\"Result\":0,\"Error\":null");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "<Error>", "</Error>", 0);
                return flag;
            }
            base.Rebate = CommFunc.GetIndexString(pResponsetext, "\"Bonus\":", ",", 0);
            base.Prize = (1800.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0)).ToString();
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x4e20, "UTF-8", true);
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

