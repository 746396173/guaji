namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class FNYX : PTBase
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
                            int num = plan.FNNumber(str4);
                            int num2 = Convert.ToInt32(plan.AutoTimes(str4, true));
                            string format = "lotteryid={0}&trace=0&tracewinstop=0&traceexceptstop=0&issuenos%5B{1}%5D=1&projects%5B0%5D%5Bmethodid%5D={2}&projects%5B0%5D%5Bcodes%5D={3}&projects%5B0%5D%5Bnum%5D={4}&projects%5B0%5D%5Btimes%5D={5}&projects%5B0%5D%5Bmode%5D={6}&projects%5B0%5D%5Bpoint%5D={10}&{9}&totalnum={7}&totalCost={8}";
                            format = string.Format(format, new object[] { this.GetPTLotteryName(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), num, num2, plan.Unit - 1, num * num2, plan.AutoTotalMoney(str4, true), this.GetRXWZString(plan.RXWZ, 0, plan.Play), base.Rebate });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, base.BetsTime3, "UTF-8", true);
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
            pHint.Contains("登陆");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"errno\":10000") || (pResponseText == "投注成功"));

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
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"balance\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/user/balance");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, true, true, true);
            if ((!CommFunc.CheckIsSkipLottery(pLotteryID) && (iD != "TXFFC")) && (iD != "FNYXHY11X5"))
            {
                str2 = str2.Substring(2);
            }
            if (iD == "TXFFC")
            {
                return str2.Insert(8, "-");
            }
            if (iD == "FNYXHY11X5")
            {
                str2 = str2.Insert(8, "-");
            }
            return str2;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/game/play");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/openList?kind={this.GetPTLotteryName(pType)}&count={10}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/game/{this.GetPTLotteryName(pType)}";

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
                        str3 = CommFunc.Join(pStr, "&", -1);
                        list.Add(str3);
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
                        str3 = "*";
                        if (num == num2)
                        {
                            str3 = CommFunc.Join(pNumberList, "&");
                        }
                        list.Add(str3);
                    }
                    str = CommFunc.Join(list, "|").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, "&");
                    }
                    else
                    {
                        str = CommFunc.Join(pNumberList, ",");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, ",");
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
                    str = CommFunc.Join(pNumberList, "&");
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
                    str = CommFunc.Join(pNumberList, "|").Replace(" ", "&").Replace("*", "");
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
                        str3 = "*";
                        if (num == num2)
                        {
                            str3 = CommFunc.Join(pNumberList, "&");
                        }
                        list.Add(str3);
                    }
                    str = CommFunc.Join(list, "|").Replace("*", "");
                    if (playName == "猜冠军猜冠军")
                    {
                        str = str.Replace(" ", ",");
                    }
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
                    return "QZX3_S";
                }
                if (playName == "前三直选复式")
                {
                    return "QZX3";
                }
                if (playName == "前三组三复式")
                {
                    return "QZUS";
                }
                if (playName == "前三组三单式")
                {
                    return "QZUS_S";
                }
                if (playName == "前三组六复式")
                {
                    return "QZUL";
                }
                if (playName == "前三组六单式")
                {
                    return "QZUL_S";
                }
                if (playName == "后三直选单式")
                {
                    return "HZX3_S";
                }
                if (playName == "后三直选复式")
                {
                    return "HZX3";
                }
                if (playName == "后三组三复式")
                {
                    return "HZUS";
                }
                if (playName == "后三组三单式")
                {
                    return "HZUS_S";
                }
                if (playName == "后三组六复式")
                {
                    return "HZUL";
                }
                if (playName == "后三组六单式")
                {
                    return "HZUL_S";
                }
                if (playName == "中三直选单式")
                {
                    return "ZZX3_S";
                }
                if (playName == "中三直选复式")
                {
                    return "ZZX3";
                }
                if (playName == "中三组三复式")
                {
                    return "ZZUS";
                }
                if (playName == "中三组三单式")
                {
                    return "ZZUS_S";
                }
                if (playName == "中三组六复式")
                {
                    return "ZZUL";
                }
                if (playName == "中三组六单式")
                {
                    return "ZZUL_S";
                }
                if (playName == "前二直选单式")
                {
                    return "QZX2_S";
                }
                if (playName == "前二直选复式")
                {
                    return "QZX2";
                }
                if (playName == "后二直选单式")
                {
                    return "HZX2_S";
                }
                if (playName == "后二直选复式")
                {
                    return "HZX2";
                }
                if (playName == "后四直选单式")
                {
                    return "ZX4_S";
                }
                if (playName == "后四直选复式")
                {
                    return "ZX4";
                }
                if (playName == "五星直选单式")
                {
                    return "ZX5_S";
                }
                if (playName == "五星直选复式")
                {
                    return "ZX5";
                }
                if (playName == "任三直选单式")
                {
                    return "RZX3_S";
                }
                if (playName == "任三直选复式")
                {
                    return "RZX3";
                }
                if (playName == "任三组三复式")
                {
                    return "RZUS";
                }
                if (playName == "任三组三单式")
                {
                    return "RZUS_S";
                }
                if (playName == "任三组六复式")
                {
                    return "RZUL";
                }
                if (playName == "任三组六单式")
                {
                    return "RZUL_S";
                }
                if (playName == "任二直选单式")
                {
                    return "RZX2_S";
                }
                if (playName == "任二直选复式")
                {
                    return "RZX2";
                }
                if (playName == "任四直选单式")
                {
                    return "RZX4_S";
                }
                if (playName == "任四直选复式")
                {
                    return "RZX4";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "DWD";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "LTZX3_S";
                }
                if (playName == "前二直选单式")
                {
                    return "LTZX2_S";
                }
                if (playName == "任选复式一中一")
                {
                    return "LTRX1";
                }
                if (playName == "任选复式二中二")
                {
                    return "LTRX2";
                }
                if (playName == "任选复式三中三")
                {
                    return "LTRX3";
                }
                if (playName == "任选复式四中四")
                {
                    return "LTRX4";
                }
                if (playName == "任选复式五中五")
                {
                    return "LTRX5";
                }
                if (playName == "任选单式一中一")
                {
                    return "LTRX1_S";
                }
                if (playName == "任选单式二中二")
                {
                    return "LTRX2_S";
                }
                if (playName == "任选单式三中三")
                {
                    return "LTRX3_S";
                }
                if (playName == "任选单式四中四")
                {
                    return "LTRX4_S";
                }
                if (playName == "任选单式五中五")
                {
                    str = "LTRX5_S";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "QZX3_S";
                }
                if (playName == "猜前三复式")
                {
                    return "QZX3";
                }
                if (playName == "猜前二单式")
                {
                    return "QZX2_S";
                }
                if (playName == "猜前二复式")
                {
                    return "QZX2";
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
                    return "QZX1";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "" : "DWD";
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
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"error\":\"", "\"", 0));
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                str = "cqssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                str = "xjssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                str = "txffc";
            }
            else if (pType == ConfigurationStatus.LotteryType.FNYXFFC)
            {
                str = "dfffc";
            }
            else if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                str = "sd115";
            }
            else if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                str = "gd115";
            }
            else if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                str = "jx115";
            }
            else if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                str = "sh115";
            }
            else if (pType == ConfigurationStatus.LotteryType.FNYXHY11X5)
            {
                str = "df115";
            }
            else if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "pk10";
            }
            return str.ToLower();
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString(List<int> pRXWZ, int pIndex, string playName)
        {
            string str = "";
            if (!CommFunc.CheckPlayIsRXDS(playName))
            {
                return str;
            }
            List<string> pList = new List<string>();
            string[] strArray = new string[] { "w", "q", "b", "s", "g" };
            for (int i = 0; i < 5; i++)
            {
                if (pRXWZ.Contains(i))
                {
                    string item = $"projects%5B{pIndex}%5D%5Bposition%5D%5B%5D={strArray[i]}";
                    pList.Add(item);
                }
            }
            return CommFunc.Join(pList, "&");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.Prize == "")
                {
                    string lotteryLine = this.GetLotteryLine(pType, false);
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                    string str4 = CommFunc.GetIndexString(pResponsetext, "maxpoint:", ",", 0);
                    string str5 = CommFunc.GetIndexString(pResponsetext, "point:", ",", 0);
                    double num = Convert.ToDouble(str4) - Convert.ToDouble(str5);
                    base.Rebate = num.ToString();
                    base.Prize = (1800.0 + ((Convert.ToDouble(str5) * 2.0) * 10.0)).ToString();
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
                string str2 = $"/captcha/default?{DateTime.Now.ToOADate()}";
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
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"_token={base.Token}&username={pID}&password={str5}&validateCode={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("\"errno\":20100");
                if (!flag)
                {
                    pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "\"error\":\"", "\"", 0).Replace(",", "").Replace("!", ""));
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
            bool flag = pResponsetext.Contains("飞牛游戏");
            base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("_token"));
            return flag;
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

