namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class HENR : PTBase
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
                            string str6 = CommFunc.CheckPlayIsDS(plan.Play) ? "1" : "0";
                            string format = "c=1&id={0}&i={1}&b0=i%3D{2}%26t%3D{3}%26m%3D{4}%26len%3D{5}%26ds%3D{6}%26r%3D{7}%26b%3D{8}%26bt%3D{9}";
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), this.GetPlayMethodID(plan.Type, plan.Play), Convert.ToInt32(plan.AutoTimes(str4, true)), plan.Unit + 4, "0", str6, "2", this.GetNumberList1(pTNumberList, plan.Play, null), this.GetRXWZString1(pTNumberList, plan.RXWZ, plan.Play) });
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
            pHint.Contains("重新登录");

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

        public override void CountPrizeDic(string pResponseText)
        {
            base.PlayMethodDic.Clear();
            List<string> list = CommFunc.SplitString(CommFunc.GetIndexString(pResponseText, "nav:[{", "}]}],", 0), "]}]},{", -1);
            for (int i = 0; i < list.Count; i++)
            {
                string pStr = list[i];
                string str3 = CommFunc.GetIndexString(pStr, "name:\"", "\"", 0);
                List<string> list2 = CommFunc.SplitString(pStr.Substring(pStr.IndexOf("menu:[{")), "}]},{", -1);
                for (int j = 0; j < list2.Count; j++)
                {
                    string str4 = list2[j];
                    string str5 = CommFunc.GetIndexString(str4, "name:\"", "\"", 0);
                    int index = str4.IndexOf("sub:[{");
                    if (index == -1)
                    {
                        index = 0;
                    }
                    List<string> list3 = CommFunc.SplitString(str4.Substring(index), "},{", -1);
                    for (int k = 0; k < list3.Count; k++)
                    {
                        string str6 = list3[k];
                        string str7 = CommFunc.GetIndexString(str6, "name:\"", "\"", 0);
                        string str8 = CommFunc.GetIndexString(str6, "id:", ",", 0);
                        string str9 = $"{str3}-{str5}-{str7}";
                        base.PlayMethodDic[str9] = str8;
                    }
                }
            }
        }

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"Amount\":", ",", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/User-Amount.fcgi");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            if (!CommFunc.CheckIsSkipLottery(iD))
            {
                str2 = str2.Substring(2);
            }
            if ((((iD == "GD11X5") || (iD == "SD11X5")) || ((iD == "SH11X5") || (iD == "JX11X5"))) || (iD == "AH11X5"))
            {
                str2 = str2.Replace("-0", "");
            }
            return str2;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType)
        {
            string str = "SSC";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                str = "LL";
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                str = "PK10";
            }
            return $"{this.GetLine()}/handler?name={str}";
        }

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "273";
            }
            if (pType == ConfigurationStatus.LotteryType.HENRTXFFC)
            {
                return "1206";
            }
            if (pType == ConfigurationStatus.LotteryType.HENRDJ2FC)
            {
                return "3057";
            }
            if (pType == ConfigurationStatus.LotteryType.HENRXJPSSC)
            {
                return "3227";
            }
            if (pType == ConfigurationStatus.LotteryType.HENROZ15C)
            {
                return "3397";
            }
            if (pType == ConfigurationStatus.LotteryType.HENRXG15C)
            {
                return "3567";
            }
            if (pType == ConfigurationStatus.LotteryType.HENRFFC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.HENR2FC)
            {
                return "139";
            }
            if (pType == ConfigurationStatus.LotteryType.HENR3FC)
            {
                return "3737";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "274";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "375";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "2339";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "276";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "2295";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "1184";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/Bet-No.fcgi?size=11&id={this.GetBetsLotteryID(pType)}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/Panel.do");

        public override string GetLoginLine() => 
            (this.GetLine() + "/");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/Home.fcgi?pssc_{this.GetBetsLotteryID(pType)}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            int num;
            int num2;
            List<string> list2;
            string str3;
            string str = "";
            ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(playName);
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    List<string> pList = new List<string>();
                    num = 0;
                    for (num2 = 0; num2 < 5; num2++)
                    {
                        if (playInfo.IndexList.Contains(num2 + 1) || CommFunc.CheckPlayIsRXFS(playName))
                        {
                            string pStr = pNumberList[num++];
                            pList.Add(CommFunc.Join(pStr, "-", -1));
                        }
                        else
                        {
                            pList.Add("*");
                        }
                    }
                    str = CommFunc.Join(pList, "_").Replace("*", "");
                }
                else if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num = AppInfo.FiveDic[ch.ToString()];
                    list2 = new List<string>();
                    for (num2 = 0; num2 < 5; num2++)
                    {
                        str3 = "*";
                        if (num2 == num)
                        {
                            str3 = CommFunc.Join(pNumberList, "-");
                        }
                        list2.Add(str3);
                    }
                    str = CommFunc.Join(list2, "_").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = "____" + CommFunc.Join(pNumberList, "-");
                    }
                    else
                    {
                        str = CommFunc.Join(pNumberList, "%7C");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "%7C");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, "%7C").Replace(" ", "");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = "____" + CommFunc.Join(pNumberList, "-");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, "%7C").Replace(" ", "");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
                    str = CommFunc.Join(pNumberList, "_").Replace(" ", "-");
                    int totalWidth = 5 - playInfo.IndexList.Count;
                    str = (str + "_".PadLeft(totalWidth, '_')).Replace("*", "");
                }
                else
                {
                    pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
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
                    list2 = new List<string>();
                    int num4 = (playName == "猜冠军猜冠军") ? 1 : 10;
                    for (num2 = 0; num2 < num4; num2++)
                    {
                        str3 = "*";
                        if (num2 == num)
                        {
                            str3 = CommFunc.Join(pNumberList, "-");
                        }
                        list2.Add(str3);
                    }
                    str = CommFunc.Join(list2, "_").Replace("*", "");
                    if (playName == "猜冠军猜冠军")
                    {
                        str = "____" + str;
                    }
                }
            }
            return HttpUtility.UrlEncode(str);
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
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
                    return "三星-直选-单式";
                }
                if (playName == "前三直选复式")
                {
                    return "三星-直选-复式";
                }
                if (playName == "前三组三复式")
                {
                    return "三星-组三-复式";
                }
                if (playName == "前三组三单式")
                {
                    return "三星-组三-单式";
                }
                if (playName == "前三组六复式")
                {
                    return "三星-组六-复式";
                }
                if (playName == "前三组六单式")
                {
                    return "三星-组六-单式";
                }
                if (playName == "后三直选单式")
                {
                    return "三星-直选-单式";
                }
                if (playName == "后三直选复式")
                {
                    return "三星-直选-复式";
                }
                if (playName == "后三组三复式")
                {
                    return "三星-组三-复式";
                }
                if (playName == "后三组三单式")
                {
                    return "三星-组三-单式";
                }
                if (playName == "后三组六复式")
                {
                    return "三星-组六-复式";
                }
                if (playName == "后三组六单式")
                {
                    return "三星-组六-单式";
                }
                if (playName == "中三直选单式")
                {
                    return "三星-直选-单式";
                }
                if (playName == "中三直选复式")
                {
                    return "三星-直选-复式";
                }
                if (playName == "中三组三复式")
                {
                    return "三星-组三-复式";
                }
                if (playName == "中三组三单式")
                {
                    return "三星-组三-单式";
                }
                if (playName == "中三组六复式")
                {
                    return "三星-组六-复式";
                }
                if (playName == "中三组六单式")
                {
                    return "三星-组六-单式";
                }
                if (playName == "前二直选单式")
                {
                    return "二星-直选-单式";
                }
                if (playName == "前二直选复式")
                {
                    return "二星-直选-复式";
                }
                if (playName == "后二直选单式")
                {
                    return "二星-直选-单式";
                }
                if (playName == "后二直选复式")
                {
                    return "二星-直选-复式";
                }
                if (playName == "前四直选单式")
                {
                    return "四星-直选-单式";
                }
                if (playName == "前四直选复式")
                {
                    return "四星-直选-复式";
                }
                if (playName == "后四直选单式")
                {
                    return "四星-直选-单式";
                }
                if (playName == "后四直选复式")
                {
                    return "四星-直选-复式";
                }
                if (playName == "五星直选单式")
                {
                    return "五星-直选-单式";
                }
                if (playName == "五星直选复式")
                {
                    return "五星-直选-复式";
                }
                if (playName == "任三直选单式")
                {
                    return "三星-直选-单式";
                }
                if (playName == "任三直选复式")
                {
                    return "三星-直选-复式";
                }
                if (playName == "任三组三复式")
                {
                    return "三星-组三-复式";
                }
                if (playName == "任三组三单式")
                {
                    return "三星-组三-单式";
                }
                if (playName == "任三组六复式")
                {
                    return "三星-组六-复式";
                }
                if (playName == "任三组六单式")
                {
                    return "三星-组六-单式";
                }
                if (playName == "任二直选单式")
                {
                    return "二星-直选-单式";
                }
                if (playName == "任二直选复式")
                {
                    return "二星-直选-复式";
                }
                if (playName == "任四直选单式")
                {
                    return "四星-直选-单式";
                }
                if (playName == "任四直选复式")
                {
                    return "四星-直选-复式";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "定位胆-定位胆-定位胆";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "三星-直选-单式";
                }
                if (playName == "前二直选单式")
                {
                    return "二星-直选-单式";
                }
                if (playName == "任选复式一中一")
                {
                    return "任选-复式-一中一";
                }
                if (playName == "任选复式二中二")
                {
                    return "任选-复式-二中二";
                }
                if (playName == "任选复式三中三")
                {
                    return "任选-复式-三中三";
                }
                if (playName == "任选复式四中四")
                {
                    return "任选-复式-四中四";
                }
                if (playName == "任选复式五中五")
                {
                    return "任选-复式-五中五";
                }
                if (playName == "任选单式一中一")
                {
                    return "任选-单式-一中一";
                }
                if (playName == "任选单式二中二")
                {
                    return "任选-单式-二中二";
                }
                if (playName == "任选单式三中三")
                {
                    return "任选-单式-三中三";
                }
                if (playName == "任选单式四中四")
                {
                    return "任选-单式-四中四";
                }
                if (playName == "任选单式五中五")
                {
                    str = "任选-单式-五中五";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "猜前三名-直选-单式";
                }
                if (playName == "猜前三复式")
                {
                    return "猜前三名-直选-复式";
                }
                if (playName == "猜前二单式")
                {
                    return "猜冠亚军-直选-单式";
                }
                if (playName == "猜前二复式")
                {
                    return "猜冠亚军-直选-复式";
                }
                if (playName == "猜前四单式")
                {
                    return "猜前四名-直选-单式";
                }
                if (playName == "猜前四复式")
                {
                    return "猜前四名-直选-复式";
                }
                if (playName == "猜前五单式")
                {
                    return "猜前五名-直选-单式";
                }
                if (playName == "猜前五复式")
                {
                    return "猜前五名-直选-复式";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "猜冠军-直选-复式";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "定位胆-定位胆-定位胆";
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
            return CommFunc.GetIndexString(pResponseText, "\"data\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/Main-Logout.do");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString1(List<string> pNumberList, List<int> pRXWZ, string playName)
        {
            string str = "";
            ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(playName);
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPSSC)
            {
                return str;
            }
            if (playInfo.IndexList.Count == 5)
            {
                return str;
            }
            if (playName.Contains("定位胆"))
            {
                return str;
            }
            List<int> list = new List<int>();
            if ((pRXWZ != null) && (pRXWZ.Count > 0))
            {
                list = CommFunc.CopyList<int>(pRXWZ);
            }
            else if (CommFunc.CheckPlayIsRXFS(playName))
            {
                for (int j = 0; j < 5; j++)
                {
                    if (pNumberList[j] != "*")
                    {
                        list.Add(j);
                    }
                }
            }
            else
            {
                foreach (int num2 in playInfo.IndexList)
                {
                    list.Add(num2 - 1);
                }
            }
            List<string> pList = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                string item = AppInfo.Index1Dic[list[i]];
                pList.Add(item);
            }
            return CommFunc.Join(pList, "_");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string pUrl = $"{this.GetLine()}/Bet-Issue.fcgi?id={this.GetBetsLotteryID(pType)}&_={DateTime.Now.ToOADate()}";
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"issue\":\"", "\"", 0).Replace("-", "");
                    if (!CommFunc.CheckIsKLCLottery(CommFunc.GetLotteryID(pType)))
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
                        base.Rebate = CommFunc.GetIndexString(pResponsetext, "\"ssc\":", ",", 0);
                        base.Prize = (1700.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0)).ToString();
                    }
                }
                if (base.PlayMethodDic.Count == 0)
                {
                    string str4 = "http://dl.hengrui2017.net:7000";
                    pUrl = $"{str4}/front/hr/script/min/{this.GetBetsLotteryID(pType)}.min.gz.js";
                    lotteryLine = this.GetLotteryLine(pType, false);
                    pResponsetext = "";
                    HttpHelper.GetResponse7(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
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
                string str2 = $"/captcha?tm={DateTime.Now.ToOADate()}";
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
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLine() + "/Main-Index.fcgi";
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string pData = $"Mobile_Login=O&UserName={pID}&PassWord={str4}";
            HttpHelper.GetResponse4(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("\"state\": true");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "\"data\":\"", "\"", 0);
                if (pHint.Contains("code"))
                {
                    pHint = "";
                    return this.InputWeb(pID, pW, ref pHint);
                }
                if (pHint.Contains("UserName"))
                {
                    pHint = "用户名或者密码错误";
                }
                return flag;
            }
            HttpHelper.SaveCookies(HttpHelper.WebCookie, this.GetHostLine());
            return flag;
        }

        public override string LoginLotteryWeb(ConfigurationStatus.LotteryType pType, string pInfo = "")
        {
            string lotteryLine = this.GetLotteryLine(pType, true);
            string pReferer = this.GetLotteryLine(pType, false);
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x4e20, "UTF-8", true);
            return pResponsetext.Contains("恒瑞");
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

