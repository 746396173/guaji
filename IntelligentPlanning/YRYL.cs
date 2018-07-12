namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class YRYL : PTBase
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
                            string prize = base.Prize;
                            string str7 = ((Convert.ToDouble(this.GetPrize(plan.Type, plan.Play)) / Convert.ToDouble(Math.Pow(10.0, (double) (plan.Unit - 1)))) * num).ToString();
                            string[] strArray = this.GetPlayString(plan.Play).Split(new char[] { '-' });
                            string format = "Bet=%5B%7B%22Player%22%3A%22{0}%22%2C%22Group%22%3A%22{1}%22%2C%22Name%22%3A%22{2}%22%2C%22ID%22%3A%22{3}%22%2C%22Ball%22%3A%22{4}%22%2C%22Bet%22%3A{5}%2C%22Money%22%3A{6}%2C%22Times%22%3A{7}%2C%22Mode%22%3A%22{8}%22%2C%22Reward%22%3A{9}%2C%22Rebate%22%3A{10}%2C%22BetReturn%22%3A%22{11}%25%22%7D%5D&Chase=";
                            format = string.Format(format, new object[] { this.GetPTLotteryName(plan.Type), HttpUtility.UrlEncode(strArray[0]), HttpUtility.UrlEncode(strArray[1]), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), num2, plan.AutoTotalMoney(str4, true), num, plan.UnitZWString, str7, prize, "0.00" });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, AppInfo.PTInfo.BetsTime2, "UTF-8", true);
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
            (pResponseText.Contains("\"success\" : 1") || (pResponseText == "投注成功"));

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
                if (!AppInfo.PTInfo.PTIsBreak)
                {
                    string accountsMemLine = this.GetAccountsMemLine(pType);
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    string pData = "";
                    HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                    if (this.CheckBreakConnect(pResponsetext))
                    {
                        AppInfo.PTInfo.PTIsBreak = true;
                    }
                    else
                    {
                        string str5 = CommFunc.GetIndexString(pResponsetext, "\"msg\" : \"", "\"", 0);
                        AppInfo.Account.BankBalance = Convert.ToDouble(str5);
                    }
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/Handler/User.ashx?ac=money");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false).Replace("-", "");
            if (iD == "YRHG15C")
            {
                str2 = str2.Substring(2);
            }
            return str2;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/Handler/Bet.ashx?ac=SaveLotteryList");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/Handler/Reward/History.axd");

        public override string GetIndexLine() => 
            (this.GetLine() + "/User/Main.aspx");

        public override string GetLoginLine() => 
            (this.GetLine() + "/Login.aspx");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/User/Game/Player.aspx?Game={this.GetPTLotteryName(pType)}";

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
                    str = CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                    str = this.GetRXWZString(pRXWZ) + str;
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",").Replace(" ", "");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList);
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",").Replace(" ", "");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",").Replace(" ", "").Replace("*", "");
                }
                else
                {
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
                    str = CommFunc.Join(list, ",").Replace("*", "");
                }
            }
            return HttpUtility.UrlEncode(str);
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    str = "33";
                }
                else if (playName == "前三直选复式")
                {
                    str = "32";
                }
                else if (playName == "前三组三复式")
                {
                    str = "35";
                }
                else if (playName == "前三组六复式")
                {
                    str = "36";
                }
                else if (playName == "后三直选单式")
                {
                    str = "19";
                }
                else if (playName == "后三直选复式")
                {
                    str = "18";
                }
                else if (playName == "后三组三复式")
                {
                    str = "21";
                }
                else if (playName == "后三组六复式")
                {
                    str = "22";
                }
                else if (playName == "中三直选单式")
                {
                    str = "26";
                }
                else if (playName == "中三直选复式")
                {
                    str = "25";
                }
                else if (playName == "中三组三复式")
                {
                    str = "28";
                }
                else if (playName == "中三组六复式")
                {
                    str = "29";
                }
                else if (playName == "前二直选单式")
                {
                    str = "42";
                }
                else if (playName == "前二直选复式")
                {
                    str = "41";
                }
                else if (playName == "后二直选单式")
                {
                    str = "40";
                }
                else if (playName == "后二直选复式")
                {
                    str = "39";
                }
                else if (playName == "后四直选单式")
                {
                    str = "12";
                }
                else if (playName == "后四直选复式")
                {
                    str = "11";
                }
                else if (playName == "五星直选单式")
                {
                    str = "03";
                }
                else if (playName == "五星直选复式")
                {
                    str = "01";
                }
                else if (playName == "任三直选单式")
                {
                    str = "69";
                }
                else if (playName == "任三直选复式")
                {
                    str = "68";
                }
                else if (playName == "任三组三复式")
                {
                    str = "71";
                }
                else if (playName == "任三组六复式")
                {
                    str = "80";
                }
                else if (playName == "任二直选单式")
                {
                    str = "63";
                }
                else if (playName == "任二直选复式")
                {
                    str = "62";
                }
                else if (playName == "任四直选单式")
                {
                    str = "75";
                }
                else if (playName == "任四直选复式")
                {
                    str = "74";
                }
                else if (playName.Contains("定位胆"))
                {
                    str = "51";
                }
                return (this.GetPTLotteryName1(pType) + str);
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    str = "06";
                }
                else if (playName == "前二直选单式")
                {
                    str = "03";
                }
                else if (playName == "任选复式一中一")
                {
                    str = "11";
                }
                else if (playName == "任选复式二中二")
                {
                    str = "12";
                }
                else if (playName == "任选复式三中三")
                {
                    str = "13";
                }
                else if (playName == "任选复式四中四")
                {
                    str = "14";
                }
                else if (playName == "任选复式五中五")
                {
                    str = "15";
                }
                else if (playName == "任选单式一中一")
                {
                    str = "";
                }
                else if (playName == "任选单式二中二")
                {
                    str = "19";
                }
                else if (playName == "任选单式三中三")
                {
                    str = "20";
                }
                else if (playName == "任选单式四中四")
                {
                    str = "21";
                }
                else if (playName == "任选单式五中五")
                {
                    str = "22";
                }
                return (this.GetPTLotteryName1(pType) + str);
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (playName == "猜前三单式")
            {
                str = "28";
            }
            else if (playName == "猜前三复式")
            {
                str = "03";
            }
            else if (playName == "猜前二单式")
            {
                str = "27";
            }
            else if (playName == "猜前二复式")
            {
                str = "02";
            }
            else if (playName == "猜前四单式")
            {
                str = "29";
            }
            else if (playName == "猜前四复式")
            {
                str = "";
            }
            else if (playName == "猜前五单式")
            {
                str = "";
            }
            else if (playName == "猜前五复式")
            {
                str = "";
            }
            else if (playName == "猜冠军猜冠军")
            {
                str = "01";
            }
            else if (playName.Contains("定位胆"))
            {
                str = (CommFunc.GetPlayNum(playName) > 5) ? "26" : "06";
            }
            return (this.GetPTLotteryName1(pType) + str);
        }

        public override string GetPlayString(string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "前三-单式";
                }
                if (playName == "前三直选复式")
                {
                    return "前三-复式";
                }
                if (playName == "前三组三复式")
                {
                    return "前三-组三";
                }
                if (playName == "前三组六复式")
                {
                    return "前三-组六";
                }
                if (playName == "后三直选单式")
                {
                    return "后三-单式";
                }
                if (playName == "后三直选复式")
                {
                    return "后三-复式";
                }
                if (playName == "后三组三复式")
                {
                    return "后三-组三";
                }
                if (playName == "后三组六复式")
                {
                    return "后三-组六";
                }
                if (playName == "中三直选单式")
                {
                    return "中三-单式";
                }
                if (playName == "中三直选复式")
                {
                    return "中三-复式";
                }
                if (playName == "中三组三复式")
                {
                    return "中三-组三";
                }
                if (playName == "中三组六复式")
                {
                    return "中三-组六";
                }
                if (playName == "前二直选单式")
                {
                    return "二星-前二(单式)";
                }
                if (playName == "前二直选复式")
                {
                    return "二星-前二(复式)";
                }
                if (playName == "后二直选单式")
                {
                    return "二星-后二(单式)";
                }
                if (playName == "后二直选复式")
                {
                    return "二星-后二(复式)";
                }
                if (playName == "后四直选单式")
                {
                    return "四星-单式";
                }
                if (playName == "后四直选复式")
                {
                    return "四星-复式";
                }
                if (playName == "五星直选单式")
                {
                    return "五星-单式";
                }
                if (playName == "五星直选复式")
                {
                    return "五星-复式";
                }
                if (playName == "任三直选单式")
                {
                    return "任选-直选单式";
                }
                if (playName == "任三直选复式")
                {
                    return "任选-直选复式";
                }
                if (playName == "任三组三复式")
                {
                    return "任选-组三复式";
                }
                if (playName == "任三组六复式")
                {
                    return "任选-组六复式";
                }
                if (playName == "任二直选单式")
                {
                    return "任选-直选单式";
                }
                if (playName == "任二直选复式")
                {
                    return "任选-直选复式";
                }
                if (playName == "任四直选单式")
                {
                    return "任选-直选单式";
                }
                if (playName == "任四直选复式")
                {
                    return "任选-直选复式";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "定位胆-定位胆";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "前三-前三单选";
                }
                if (playName == "前二直选单式")
                {
                    return "前二-前二单选";
                }
                if (playName == "任选复式一中一")
                {
                    return "任选-任选一中一";
                }
                if (playName == "任选复式二中二")
                {
                    return "任选-任选二中二";
                }
                if (playName == "任选复式三中三")
                {
                    return "任选-任选三中三";
                }
                if (playName == "任选复式四中四")
                {
                    return "任选-任选四中四";
                }
                if (playName == "任选复式五中五")
                {
                    return "任选-任选五中五";
                }
                if (playName == "任选单式一中一")
                {
                    return "";
                }
                if (playName == "任选单式二中二")
                {
                    return "任选-任选二中二";
                }
                if (playName == "任选单式三中三")
                {
                    return "任选-任选三中三";
                }
                if (playName == "任选单式四中四")
                {
                    return "任选-任选四中四";
                }
                if (playName == "任选单式五中五")
                {
                    str = "任选-任选五中五";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "单式-前三";
                }
                if (playName == "猜前三复式")
                {
                    return "排名竞猜-前三";
                }
                if (playName == "猜前二单式")
                {
                    return "单式-前二";
                }
                if (playName == "猜前二复式")
                {
                    return "排名竞猜-冠亚军";
                }
                if (playName == "猜前四单式")
                {
                    return "单式-前四";
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
                    return "排名竞猜-猜冠军";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "定位胆-后五" : "定位胆-前五";
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
            return CommFunc.GetIndexString(pResponseText, "\"msg\" : \"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "ChungKing";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "Sinkiang";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "tjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.YRTXFFC)
            {
                return "txffc";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "bjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.YRHG15C)
            {
                return "krssc";
            }
            if (pType == ConfigurationStatus.LotteryType.YRFFC)
            {
                return "Minute";
            }
            if (pType == ConfigurationStatus.LotteryType.YR2FC)
            {
                return "Minute2";
            }
            if (pType == ConfigurationStatus.LotteryType.YR5FC)
            {
                return "Minute5";
            }
            if (pType == ConfigurationStatus.LotteryType.YRALFFC)
            {
                return "aliffc";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "SD11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "GD11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "JX11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "SH11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "PK10";
            }
            return str;
        }

        public override string GetPTLotteryName1(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "9";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "27";
            }
            if (pType == ConfigurationStatus.LotteryType.YRTXFFC)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "29";
            }
            if (pType == ConfigurationStatus.LotteryType.YRHG15C)
            {
                return "32";
            }
            if (pType == ConfigurationStatus.LotteryType.YRFFC)
            {
                return "3";
            }
            if (pType == ConfigurationStatus.LotteryType.YR2FC)
            {
                return "15";
            }
            if (pType == ConfigurationStatus.LotteryType.YR5FC)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.YRALFFC)
            {
                return "35";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "16";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "17";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "33";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "8";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/User/Main.aspx?ac=logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            List<string> pList = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string item = pRXWZ.Contains(i) ? "1" : "0";
                pList.Add(item);
            }
            return (CommFunc.Join(pList) + "&");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string pUrl = this.GetLine() + "/Handler/Reward/Current.axd";
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                string pData = $"Game={this.GetPTLotteryName(pType)}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"Index\":\"", "\"", 0).Trim();
                    if (pType == ConfigurationStatus.LotteryType.YRHG15C)
                    {
                        base.Expect = "20" + base.Expect;
                    }
                    base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
                }
                if (base.Prize == "")
                {
                    pUrl = this.GetLotteryLine(pType, false);
                    lotteryLine = this.GetLotteryLine(pType, false);
                    pResponsetext = "";
                    HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        base.Prize = CommFunc.GetIndexString(pResponsetext, "data-max=\"", "\"", 0);
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
                string str2 = $"/ValidateCode.axd?Name=login&r={DateTime.Now.ToOADate()}";
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
                string pUrl = this.GetLine() + "/Login.aspx?ac=login";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"UserName={pID}&Password={str5}&login={webVerifyCode}&Url=";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
                flag = pResponsetext.Contains("\"success\" : 1");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"msg\" : \"", "\"", 0);
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
            return pResponsetext.Contains("亿人娱乐");
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

