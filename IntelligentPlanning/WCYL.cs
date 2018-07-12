namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class WCYL : PTBase
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
                            string prize = base.Prize;
                            string format = "[(\"lotteryID\":\"{0}\",\"gameID\":\"{1}\",\"betNum\":\"{2}\",\"betCount\":\"{3}\",\"betAmount\":\"{4}\",\"mode\":\"{5}\",\"multiCount\":\"{6}\",\"unit\":\"{7}\",\"issueNO\":\"{8}\",\"bonusAmount\":\"{9}\",\"subID\":\"{10}\")]";
                            format = string.Format(format, new object[] { this.GetPTLotteryName(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), num, plan.AutoTotalMoney(str4, true), prize, Convert.ToInt32(plan.AutoTimes(str4, true)), plan.Money, this.GetBetsExpect(plan.CurrentExpect, ""), plan.Mode, this.GetRXWZString1(plan.RXWZ, plan.Play) }).Replace("(", "{").Replace(")", "}");
                            HttpHelper.GetResponse1(ref pResponsetext, betsLine, "POST", format, pReferer, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
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
            (pResponseText.Contains("\"success\":true") || (pResponseText == "投注成功"));

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
                string loginLine = this.GetLoginLine();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, loginLine, 0x2710, "UTF-8", true);
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"balanceAmount\":", ",", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
                if (base.Prize == "")
                {
                    base.Rebate = CommFunc.GetIndexString(pResponsetext, "\"rebate\":", ",", 0);
                    base.Prize = (1800.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0)).ToString();
                    if (((AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5) || (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)) && (Convert.ToDouble(base.Prize) > 1950.0))
                    {
                        base.Prize = "1950";
                    }
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/web/member/info");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, true, true, false);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/web/order");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/web/issue/info/{this.GetPTLotteryName(pType)}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            (this.GetLine() + "/lottery.html");

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            int num;
            string str2;
            int num2;
            string pSource = "";
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
                    pSource = CommFunc.Join(list, "|").Replace("*", "");
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
                    pSource = CommFunc.Join(list, "|").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        pSource = CommFunc.Join(pNumberList, ",");
                    }
                }
                else
                {
                    pSource = CommFunc.Join(pNumberList, ",");
                }
                if (CommFunc.CheckPlayIsRXDS(playName))
                {
                    pSource = this.GetRXWZString(pRXWZ) + pSource;
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    pSource = CommFunc.Join(pNumberList, ",").Replace(" ", "");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    pSource = CommFunc.Join(pNumberList, ",");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    pSource = CommFunc.Join(pNumberList, ";");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    pSource = CommFunc.Join(pNumberList, "|").Replace(" ", ",").Replace("*", "");
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
                    pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
                    for (num = 0; num < num3; num++)
                    {
                        str2 = "*";
                        if (num == num2)
                        {
                            str2 = CommFunc.Join(pNumberList, ",");
                        }
                        list.Add(str2);
                    }
                    pSource = CommFunc.Join(list, "|").Replace("*", "");
                }
            }
            return CommFunc.LZMAEncode(pSource);
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "ssc3x11100ixds";
                }
                if (playName == "前三直选复式")
                {
                    return "ssc3x11100ixfs";
                }
                if (playName == "前三组三复式")
                {
                    return "ssc3x11100u3fs";
                }
                if (playName == "前三组六复式")
                {
                    return "ssc3x11100u6fs";
                }
                if (playName == "后三直选单式")
                {
                    return "ssc3x00111ixds";
                }
                if (playName == "后三直选复式")
                {
                    return "ssc3x00111ixfs";
                }
                if (playName == "后三组三复式")
                {
                    return "ssc3x00111u3fs";
                }
                if (playName == "后三组六复式")
                {
                    return "ssc3x00111u6fs";
                }
                if (playName == "中三直选单式")
                {
                    return "ssc3x01110ixds";
                }
                if (playName == "中三直选复式")
                {
                    return "ssc3x01110ixfs";
                }
                if (playName == "中三组三复式")
                {
                    return "ssc3x01110u3fs";
                }
                if (playName == "中三组六复式")
                {
                    return "ssc3x01110u6fs";
                }
                if (playName == "前二直选单式")
                {
                    return "ssc2x11000ixds";
                }
                if (playName == "前二直选复式")
                {
                    return "ssc2x11000ixfs";
                }
                if (playName == "后二直选单式")
                {
                    return "ssc2x00011ixds";
                }
                if (playName == "后二直选复式")
                {
                    return "ssc2x00011ixfs";
                }
                if (playName == "前四直选单式")
                {
                    return "ssc4x11110ixds";
                }
                if (playName == "前四直选复式")
                {
                    return "ssc4x11110ixfs";
                }
                if (playName == "后四直选单式")
                {
                    return "ssc4x01111ixds";
                }
                if (playName == "后四直选复式")
                {
                    return "ssc4x01111ixfs";
                }
                if (playName == "五星直选单式")
                {
                    return "ssc5x11111ixds";
                }
                if (playName == "五星直选复式")
                {
                    return "ssc5x11111ixfs";
                }
                if (playName == "任三直选单式")
                {
                    return "ssc3rixds";
                }
                if (playName == "任三直选复式")
                {
                    return "ssc3rixfs";
                }
                if (playName == "任三组三复式")
                {
                    return "ssc3ru3fs";
                }
                if (playName == "任三组六复式")
                {
                    return "ssc3ru6fs";
                }
                if (playName == "任二直选单式")
                {
                    return "ssc2rixds";
                }
                if (playName == "任二直选复式")
                {
                    return "ssc2rixfs";
                }
                if (playName == "任四直选单式")
                {
                    return "ssc4rixds";
                }
                if (playName == "任四直选复式")
                {
                    return "ssc4rixfs";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "ssc1xdwd";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "syxwq3mixds";
                }
                if (playName == "前二直选单式")
                {
                    return "syxwq2mixds";
                }
                if (playName == "任选复式一中一")
                {
                    return "syxwrxfs1z1";
                }
                if (playName == "任选复式二中二")
                {
                    return "syxwrxfs2z2";
                }
                if (playName == "任选复式三中三")
                {
                    return "syxwrxfs3z3";
                }
                if (playName == "任选复式四中四")
                {
                    return "syxwrxfs4z4";
                }
                if (playName == "任选复式五中五")
                {
                    return "syxwrxfs5z5";
                }
                if (playName == "任选单式一中一")
                {
                    return "syxwrxds1z1";
                }
                if (playName == "任选单式二中二")
                {
                    return "syxwrxds2z2";
                }
                if (playName == "任选单式三中三")
                {
                    return "syxwrxds3z3";
                }
                if (playName == "任选单式四中四")
                {
                    return "syxwrxds4z4";
                }
                if (playName == "任选单式五中五")
                {
                    str = "";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "";
                }
                if (playName == "猜前三复式")
                {
                    return "pk10q3ixfs";
                }
                if (playName == "猜前二单式")
                {
                    return "";
                }
                if (playName == "猜前二复式")
                {
                    return "pk10q2ixfs";
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
                    return "pk10q1ixfs";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "pk10dwd";
                }
            }
            return str;
        }

        public override string GetPrize(ConfigurationStatus.LotteryType pType, string playName)
        {
            int num = 0;
            while (true)
            {
                if ((num >= 3) || (base.Prize != ""))
                {
                    if (base.Prize == "")
                    {
                        return "";
                    }
                    double pNum = Convert.ToDouble(base.Prize);
                    if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
                    {
                        if (playName.Contains("组三"))
                        {
                            pNum /= base.Z3Key;
                        }
                        else if (playName.Contains("组六"))
                        {
                            pNum /= base.Z6Key;
                        }
                        else
                        {
                            pNum /= 1000.0;
                            if (playName.Contains("定位胆"))
                            {
                                pNum *= 10.0;
                            }
                            else if (playName.Contains("二"))
                            {
                                pNum *= 100.0;
                            }
                            else if (playName.Contains("三"))
                            {
                                pNum *= 1000.0;
                            }
                            else if (playName.Contains("四"))
                            {
                                pNum *= 10000.0;
                            }
                            else if (playName.Contains("五"))
                            {
                                pNum *= 100000.0;
                            }
                        }
                    }
                    else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                    {
                        if (playName.Contains("一"))
                        {
                            pNum = (1.98 * pNum) / 1800.0;
                        }
                        else if (playName.Contains("二"))
                        {
                            if (playName.Contains("任"))
                            {
                                pNum = (4.907647 * pNum) / 1800.0;
                            }
                            else
                            {
                                pNum = (97.252941 * pNum) / 1800.0;
                            }
                        }
                        else if (playName.Contains("三"))
                        {
                            if (playName.Contains("任"))
                            {
                                pNum = (14.722941 * pNum) / 1800.0;
                            }
                            else
                            {
                                pNum = (884.117647 * pNum) / 1800.0;
                            }
                        }
                        else if (playName.Contains("四"))
                        {
                            pNum = (58.764706 * pNum) / 1800.0;
                        }
                        else if (playName.Contains("五"))
                        {
                            pNum = (408.303529 * pNum) / 1800.0;
                        }
                        pNum *= 2.0;
                    }
                    else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                    {
                        if (playName.Contains("定位胆") || playName.Contains("猜冠军猜冠军"))
                        {
                            pNum = (9.0 * pNum) / 1800.0;
                        }
                        else if (playName.Contains("二"))
                        {
                            pNum = (81.0 * pNum) / 1800.0;
                        }
                        else if (playName.Contains("三"))
                        {
                            pNum = (648.0 * pNum) / 1800.0;
                        }
                        pNum *= 2.0;
                    }
                    return CommFunc.TwoDouble(pNum, true);
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
            return CommFunc.GetIndexString(pResponseText, "\"errorMessage\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "cqssc";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "tencentffc";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "bj300sgpoffic";
            }
            if (pType == ConfigurationStatus.LotteryType.WCFLPFFC)
            {
                return "flb60sgpoffic";
            }
            if (pType == ConfigurationStatus.LotteryType.WCJPZFFC)
            {
                return "jpz60sgpoffic";
            }
            if (pType == ConfigurationStatus.LotteryType.CAIHFLP15C)
            {
                return "ph90sgpoffic";
            }
            if (pType == ConfigurationStatus.LotteryType.WCDBFFC)
            {
                return "daban60sgpssc";
            }
            if (pType == ConfigurationStatus.LotteryType.WCMDJB15C)
            {
                return "mdjb90sgpssc";
            }
            if (pType == ConfigurationStatus.LotteryType.WCMGDZ2FC)
            {
                return "mgdzdlt120sgpssc";
            }
            if (pType == ConfigurationStatus.LotteryType.WCMG45M)
            {
                return "mgjq45sgpssc";
            }
            if (pType == ConfigurationStatus.LotteryType.WCMLXY3FC)
            {
                return "mlbckl180sgpssc";
            }
            if (pType == ConfigurationStatus.LotteryType.WCTWFFC)
            {
                return "twgxxlw60sgpssc";
            }
            if (pType == ConfigurationStatus.LotteryType.WCXBYFFC)
            {
                return "xbysf60sgpssc";
            }
            if (pType == ConfigurationStatus.LotteryType.WCXDL15C)
            {
                return "xdl90sgpssc";
            }
            if (pType == ConfigurationStatus.LotteryType.WCYGFFC)
            {
                return "ygmc60sgpssc";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "gd11X5";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "jx11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "sd11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "ah11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "sh11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "bjpk10";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/web/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString1(List<int> pRXWZ, string playName)
        {
            string str = "null";
            if (!CommFunc.CheckPlayIsRXDS(playName))
            {
                return str;
            }
            List<string> pList = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string item = pRXWZ.Contains(i) ? "1" : "0";
                pList.Add(item);
            }
            return CommFunc.Join(pList);
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
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
                string str2 = $"/web/captcha.png?{DateTime.Now.ToOADate()}";
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
                string pUrl = this.GetLine() + "/web/login";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"(username:{pID},password:{str5},captcha:{webVerifyCode})".Replace("(", "{").Replace(")", "}");
                HttpHelper.GetResponse1(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
                flag = pResponsetext.Contains("\"success\":true");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"errorMessage\":\"", "\"", 0);
                    if (pHint.Contains("验证码"))
                    {
                        pHint = "";
                        return this.InputWeb(pID, pW, ref pHint);
                    }
                    return flag;
                }
                HttpHelper.BetsTokenKey = "authorization";
                HttpHelper.BetsTokenValue = CommFunc.GetIndexString(pResponsetext, "\"items\":\"", "\"", 0);
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, base.BetsTime2, "UTF-8", true);
            return pResponsetext.Contains("万彩");
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

