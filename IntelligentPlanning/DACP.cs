namespace IntelligentPlanning
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class DACP : PTBase
    {
        public override bool BetsMain(ConfigurationStatus.SCPlan plan, ref string pHint)
        {
            bool flag = false;
            try
            {
                Dictionary<string, Dictionary<string, List<string>>> fNNumberDic = plan.FNNumberDic;
                foreach (string str in fNNumberDic.Keys)
                {
                    Dictionary<string, List<string>> dictionary2 = fNNumberDic[str];
                    foreach (string str2 in dictionary2.Keys)
                    {
                        if (plan.IsMNState(str2, true))
                        {
                            flag = true;
                            pHint = "投注成功";
                        }
                        else
                        {
                            List<string> pTNumberList = plan.GetPTNumberList(dictionary2[str2]);
                            string pUrl = this.GetLine() + "/DoBets/__Advisory";
                            string indexLine = this.GetIndexLine();
                            string pResponsetext = "";
                            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                            base.Token = CommFunc.GetIndexString(pResponsetext, "\"adv\":\"", "\"", 0);
                            if (base.Token != "")
                            {
                                pUrl = this.GetBetsLine(plan.Type);
                                int num = Convert.ToInt32(Math.Pow(10.0, (double) (plan.Unit - 1)));
                                int num2 = plan.FNNumber(str2);
                                string format = "data=[\"{0}|{1}|{2}|{3}|{4}|{5}|{6}\"]&lottyid={7}&adv={8}";
                                format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), Convert.ToInt32(plan.AutoTimes(str2, true)), base.Rebate, num2, num, this.GetNumberList1(pTNumberList, plan.Play, null), base.ExpectID, base.Token });
                                pResponsetext = "";
                                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", format, indexLine, 0x2710, "UTF-8", true);
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
            pHint.Contains("重新登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"code\":200") || (pResponseText == "投注成功"));

        private bool CheckVerifyCode(string pVerifyCode)
        {
            if (pVerifyCode.Length != 5)
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
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"balance\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/Common/getUserBalance?_{DateTime.Now.ToOADate()}";

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/DoBets/index/{this.GetBetsLotteryID(pType)}";

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
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "17";
            }
            if (pType == ConfigurationStatus.LotteryType.VRSSC)
            {
                return "21";
            }
            if (pType == ConfigurationStatus.LotteryType.DAFFC)
            {
                return "37";
            }
            if (pType == ConfigurationStatus.LotteryType.DA3FC)
            {
                return "35";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "28";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "24";
            }
            if (pType == ConfigurationStatus.LotteryType.VRPK10)
            {
                str = "23";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            this.GetLotteryLine(pType, false);

        public override string GetIndexLine() => 
            (this.GetLine() + "/LobbyOp");

        public override string GetLoginLine() => 
            (this.GetLine() + "/Login/index");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/LottyOp/getResultByGTid/{this.GetBetsLotteryID(pType)}/1?_={DateTime.Now.ToOADate()}";

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
                    return CommFunc.Join(list, ",").Replace("*", "");
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
            list = new List<string>();
            int num3 = (playName == "猜冠军猜冠军") ? 1 : 10;
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
                    return "14|123";
                }
                if (playName == "前三直选复式")
                {
                    return "1|123";
                }
                if (playName == "前三组三复式")
                {
                    return "7|123";
                }
                if (playName == "前三组六复式")
                {
                    return "8|123";
                }
                if (playName == "后三直选单式")
                {
                    return "14|345";
                }
                if (playName == "后三直选复式")
                {
                    return "1|345";
                }
                if (playName == "后三组三复式")
                {
                    return "7|345";
                }
                if (playName == "后三组六复式")
                {
                    return "8|345";
                }
                if (playName == "中三直选单式")
                {
                    return "14|234";
                }
                if (playName == "中三直选复式")
                {
                    return "1|234";
                }
                if (playName == "中三组三复式")
                {
                    return "7|234";
                }
                if (playName == "中三组六复式")
                {
                    return "8|234";
                }
                if (playName == "前二直选单式")
                {
                    return "14|12";
                }
                if (playName == "前二直选复式")
                {
                    return "1|12";
                }
                if (playName == "后二直选单式")
                {
                    return "14|45";
                }
                if (playName == "后二直选复式")
                {
                    return "1|45";
                }
                if (playName == "后四直选单式")
                {
                    return "14|2345";
                }
                if (playName == "后四直选复式")
                {
                    return "1|2345";
                }
                if (playName == "五星直选单式")
                {
                    return "14|12345";
                }
                if (playName == "五星直选复式")
                {
                    return "1|12345";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "1|112345";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "67|3113";
                }
                if (playName == "前二直选单式")
                {
                    return "65|2112";
                }
                if (playName == "任选复式一中一")
                {
                    return "55|0";
                }
                if (playName == "任选复式二中二")
                {
                    return "56|0";
                }
                if (playName == "任选复式三中三")
                {
                    return "57|0";
                }
                if (playName == "任选复式四中四")
                {
                    return "58|0";
                }
                if (playName == "任选复式五中五")
                {
                    return "59|0";
                }
                if (playName == "任选单式一中一")
                {
                    return "55|1";
                }
                if (playName == "任选单式二中二")
                {
                    return "56|1";
                }
                if (playName == "任选单式三中三")
                {
                    return "57|1";
                }
                if (playName == "任选单式四中四")
                {
                    return "58|1";
                }
                if (playName == "任选单式五中五")
                {
                    str = "59|1";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "14|123";
                }
                if (playName == "猜前三复式")
                {
                    return "1|123";
                }
                if (playName == "猜前二单式")
                {
                    return "14|12";
                }
                if (playName == "猜前二复式")
                {
                    return "1|12";
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
                    return "1|1";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "1|10";
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
            return CommFunc.GetIndexString(pResponseText, "\"code\":", ",", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/Login/Logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string pUrl = $"{this.GetLine()}/LobbyOp/getCountdown/{this.GetBetsLotteryID(pType)}?_={DateTime.Now.ToOADate()}";
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.ExpectID = CommFunc.GetIndexString(pResponsetext, "\"lottyid\":", ",", 0);
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"round\":\"", "\"", 0);
                    base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
                    if (base.Prize == "")
                    {
                        string str4 = "1/123";
                        if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                        {
                            str4 = "67/3113";
                        }
                        else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                        {
                            str4 = "1/1";
                        }
                        pUrl = $"{this.GetLine()}/GetRateOp/index/{this.GetBetsLotteryID(pType)}/{str4}";
                        indexLine = this.GetIndexLine();
                        pResponsetext = "";
                        HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                        if (pResponsetext != "")
                        {
                            string[] strArray = Strings.Filter(Strings.Split(pResponsetext, "\",\"", -1, CompareMethod.Binary), @"\/", true, CompareMethod.Binary);
                            string expression = strArray[strArray.Length - 1];
                            double num = Convert.ToDouble(Strings.Split(expression, @"\/ ", -1, CompareMethod.Binary)[0]) * 2.0;
                            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                            {
                                num *= 1.0101;
                            }
                            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                            {
                                num *= 100.0;
                            }
                            base.Prize = num.ToString();
                            base.Rebate = (Convert.ToDouble(Strings.Split(strArray[0], @"\/ ", -1, CompareMethod.Binary)[1]) * 10.0).ToString();
                        }
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
                string str2 = "/Login/authnum";
                string pUrl = this.GetLine() + str2;
                File.Delete(pVerifyCodeFile);
                string loginLine = this.GetLoginLine();
                Bitmap bitmap = new Bitmap(HttpHelper.GetResponseImage(pUrl, loginLine, "GET", "", 0x1770, "UTF-8", true));
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
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"login={pID}&pass={str5}&authnum={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext == "{\"tips\":\"\"}";
                if (!flag)
                {
                    pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "\"tips\":\"", "\"", 0));
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
            return pResponsetext.Contains("会员登录");
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

