﻿namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class TAYL : PTBase
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
                            string str6 = CommFunc.CheckPlayIsDS(plan.Play) ? "8" : "0";
                            int num = plan.FNNumber(str4);
                            string prize = base.Prize;
                            string format = "betMessage=%7B'UserBetInfo'%3A%7B'IssueNumber'%3A'{0}'%2C'Bet'%3A%5B%7B'BetMode'%3A'{1}'%2C'BetCount'%3A{2}%2C'BetMultiple'%3A'{3}'%2C'BetContent'%3A'{4}'%2C'IssueNumber'%3A'{0}'%2C'BetMoney'%3A{5}%2C'PlayCode'%3A'{6}'%2C'BetRebate'%3A{7}%7D%5D%2C'LotteryCode'%3A{8}%7D%7D";
                            format = string.Format(format, new object[] { this.GetBetsExpect(plan.CurrentExpect, ""), str6, num, Convert.ToInt32(plan.AutoTimes(str4, true)), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), plan.AutoTotalMoney(str4, true), this.GetPlayMethodID(plan.Type, plan.Play), prize, this.GetBetsLotteryID(plan.Type) });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, pReferer, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
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
            (pResponseText.Contains("\"state\":true") || (pResponseText == "投注成功"));

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
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pData = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"lotteryMoney\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/financeCenter/getUserAllMoney");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, true, true, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/lottery/bet_lottery");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "12";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "14";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "71";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "57";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "73";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "86";
            }
            if (pType == ConfigurationStatus.LotteryType.TAHGSSC)
            {
                return "56";
            }
            if (pType == ConfigurationStatus.LotteryType.TAFLBSSC)
            {
                return "66";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "57";
            }
            if (pType == ConfigurationStatus.LotteryType.TABL15C)
            {
                return "44";
            }
            if (pType == ConfigurationStatus.LotteryType.TAWX15C)
            {
                return "43";
            }
            if (pType == ConfigurationStatus.LotteryType.TAWBFFC)
            {
                return "49";
            }
            if (pType == ConfigurationStatus.LotteryType.TAFFC)
            {
                return "51";
            }
            if (pType == ConfigurationStatus.LotteryType.TA3FC)
            {
                return "53";
            }
            if (pType == ConfigurationStatus.LotteryType.TA5FC)
            {
                return "55";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "16";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "77";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "78";
            }
            if (pType == ConfigurationStatus.LotteryType.JS11X5)
            {
                return "85";
            }
            if (pType == ConfigurationStatus.LotteryType.TA11X5FFC)
            {
                return "61";
            }
            if (pType == ConfigurationStatus.LotteryType.TA11X53FC)
            {
                return "63";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "10";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/lottery/getHisNumber");

        public override string GetIndexLine() => 
            (this.GetLine() + "/index");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/lottery/getLotteryInfo?v={DateTime.Now.ToOADate()}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            int num;
            string str2;
            int num2;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = CommFunc.Join(pNumberList[num], ",", -1);
                        list.Add(str2);
                    }
                    str = CommFunc.Join(list, "|").Replace("*", "");
                }
                else if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
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
                    str = CommFunc.Join(list, "|");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, ",");
                    }
                    else
                    {
                        list = new List<string>();
                        for (num = 0; num < pNumberList.Count; num++)
                        {
                            str2 = CommFunc.Join(pNumberList[num], ",", -1);
                            list.Add(str2);
                        }
                        str = CommFunc.Join(list, " ");
                    }
                }
                else if (playName.Contains("五星直选单式"))
                {
                    str = CommFunc.Join(pNumberList, " ");
                }
                else if (CommFunc.CheckPlayIsRXDS(playName))
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = CommFunc.Join(pNumberList[num], ",", -1);
                        list.Add(str2);
                    }
                    str = CommFunc.Join(list, " ");
                }
                else
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = CommFunc.Join(pNumberList[num], "|", -1);
                        list.Add(str2);
                    }
                    str = CommFunc.Join(list, " ");
                }
                if (CommFunc.CheckPlayIsRXDS(playName))
                {
                    str = str + this.GetRXWZString(pRXWZ);
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    if (playName.Contains("任"))
                    {
                        list = new List<string>();
                        for (num = 0; num < pNumberList.Count; num++)
                        {
                            str2 = pNumberList[num].Replace(" ", ",");
                            list.Add(str2);
                        }
                        str = CommFunc.Join(list, " ");
                    }
                    else
                    {
                        list = new List<string>();
                        for (num = 0; num < pNumberList.Count; num++)
                        {
                            str2 = pNumberList[num].Replace(" ", "|");
                            list.Add(str2);
                        }
                        str = CommFunc.Join(list, " ");
                    }
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = pNumberList[num].Replace(" ", "|");
                        list.Add(str2);
                    }
                    str = CommFunc.Join(list, " ");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = pNumberList[num].Replace(" ", ",");
                        list.Add(str2);
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
                    if (num2 >= 5)
                    {
                        num2 -= 5;
                    }
                    list = new List<string>();
                    int num3 = (playName == "猜冠军猜冠军") ? 1 : 5;
                    for (num = 0; num < num3; num++)
                    {
                        str2 = "*";
                        if (num == num2)
                        {
                            str2 = CommFunc.Join(pNumberList, ",");
                        }
                        list.Add(str2);
                    }
                    str = CommFunc.Join(list, "|");
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
                    str = "12";
                }
                else if (playName == "前三直选复式")
                {
                    str = "12";
                }
                else if (playName == "前三组三复式")
                {
                    str = "14";
                }
                else if (playName == "前三组六复式")
                {
                    str = "15";
                }
                else if (playName == "后三直选单式")
                {
                    str = "03";
                }
                else if (playName == "后三直选复式")
                {
                    str = "03";
                }
                else if (playName == "后三组三复式")
                {
                    str = "05";
                }
                else if (playName == "后三组六复式")
                {
                    str = "06";
                }
                else if (playName == "中三直选单式")
                {
                    str = "25";
                }
                else if (playName == "中三直选复式")
                {
                    str = "25";
                }
                else if (playName == "中三组三复式")
                {
                    str = "27";
                }
                else if (playName == "中三组六复式")
                {
                    str = "28";
                }
                else if (playName == "前二直选单式")
                {
                    str = "18";
                }
                else if (playName == "前二直选复式")
                {
                    str = "18";
                }
                else if (playName == "后二直选单式")
                {
                    str = "07";
                }
                else if (playName == "后二直选复式")
                {
                    str = "07";
                }
                else if (playName == "后四直选单式")
                {
                    str = "32";
                }
                else if (playName == "后四直选复式")
                {
                    str = "32";
                }
                else if (playName == "五星直选单式")
                {
                    str = "01";
                }
                else if (playName == "五星直选复式")
                {
                    str = "01";
                }
                else if (playName == "任三直选单式")
                {
                    str = "38";
                }
                else if (playName == "任三直选复式")
                {
                    str = "38";
                }
                else if (playName == "任三组三复式")
                {
                    str = "39";
                }
                else if (playName == "任三组三单式")
                {
                    str = "39";
                }
                else if (playName == "任三组六复式")
                {
                    str = "40";
                }
                else if (playName == "任三组六单式")
                {
                    str = "40";
                }
                else if (playName == "任二直选单式")
                {
                    str = "37";
                }
                else if (playName == "任二直选复式")
                {
                    str = "37";
                }
                else if (playName == "任四直选单式")
                {
                    str = "65";
                }
                else if (playName == "任四直选复式")
                {
                    str = "65";
                }
                else if (playName.Contains("定位胆"))
                {
                    str = "10";
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    str = "11";
                }
                else if (playName == "前二直选单式")
                {
                    str = "09";
                }
                else if (playName == "任选复式一中一")
                {
                    str = "01";
                }
                else if (playName == "任选复式二中二")
                {
                    str = "02";
                }
                else if (playName == "任选复式三中三")
                {
                    str = "03";
                }
                else if (playName == "任选复式四中四")
                {
                    str = "04";
                }
                else if (playName == "任选复式五中五")
                {
                    str = "05";
                }
                else if (playName == "任选单式一中一")
                {
                    str = "01";
                }
                else if (playName == "任选单式二中二")
                {
                    str = "02";
                }
                else if (playName == "任选单式三中三")
                {
                    str = "03";
                }
                else if (playName == "任选单式四中四")
                {
                    str = "04";
                }
                else if (playName == "任选单式五中五")
                {
                    str = "05";
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    str = "13";
                }
                else if (playName == "猜前三复式")
                {
                    str = "13";
                }
                else if (playName == "猜前二单式")
                {
                    str = "12";
                }
                else if (playName == "猜前二复式")
                {
                    str = "12";
                }
                else if (playName == "猜前四单式")
                {
                    str = "";
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
                    str = "11";
                }
                else if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "15" : "14";
                }
            }
            if (str != "")
            {
                str = this.GetBetsLotteryID(pType) + str;
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
            str = CommFunc.GetIndexString(pResponseText, "\"mark\":\"", "\"", 0);
            if (str == "")
            {
                str = CommFunc.GetIndexString(pResponseText, "\"Mark\":\"", "\"", 0);
            }
            return str;
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = CommFunc.GetLotteryID(pType).ToLower();
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "tw5fc";
            }
            if (pType == ConfigurationStatus.LotteryType.JH15C)
            {
                return "flb90m";
            }
            if (pType == ConfigurationStatus.LotteryType.WHDJSSC)
            {
                str = "dj90m";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/exit");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            string str = CommFunc.Join<int>(pRXWZ);
            return $"${str}";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string str;
                string indexLine;
                string str3;
                if (base.Prize == "")
                {
                    str = $"{this.GetLine()}/accountInfo/first_personal_page?index=1&&secondIndex=1";
                    indexLine = this.GetIndexLine();
                    str3 = "";
                    HttpHelper.GetResponse(ref str3, str, "GET", string.Empty, indexLine, base.BetsTime2, "UTF-8", true);
                    if (str3 != "")
                    {
                        base.Prize = CommFunc.GetIndexString(str3, "<i class=\"tye\">", "<", str3.IndexOf("账号返点"));
                    }
                }
                str = $"{this.GetLine()}/lottery/getIssue?v={DateTime.Now.ToOADate()}";
                indexLine = this.GetIndexLine();
                str3 = "";
                string pData = $"lotteryId={this.GetBetsLotteryID(pType)}";
                HttpHelper.GetResponse(ref str3, str, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                if (str3 != "")
                {
                    base.Expect = CommFunc.GetIndexString(str3, "\"IssueNumber\":\"", "\"", 0);
                    base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
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
                string str2 = $"/checkimage.jpg?longs={DateTime.Now.ToOADate()}";
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
                string pUrl = this.GetLoginLine();
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pID);
                string str6 = HttpUtility.UrlEncode(pW);
                string pData = $"message=%7B'UserLoginName'%3A'{str5}'%2C'UserPassWord'%3A'{str6}'%2C'gCode'%3A''%2C'VerifyKey'%3A''%7D&SecCode={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
                flag = pResponsetext == "1";
                if (!flag)
                {
                    pHint = pResponsetext;
                    if (pHint == "2")
                    {
                        pHint = "";
                        return this.InputWeb(pID, pW, ref pHint);
                    }
                    if (pHint == "3")
                    {
                        pHint = "帐号或者密码错误";
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
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, base.BetsTime2, "UTF-8", true);
            return pResponsetext.Contains("TA娱乐");
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            string pData = "";
            HttpHelper.GetResponse(ref pResponsetext, quitPTLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
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

