namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class HANY : PTBase
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
                            double num2 = 1.0 / Convert.ToDouble(Math.Pow(10.0, (double) (plan.Unit - 1)));
                            string format = "";
                            if (CommFunc.CheckPlayIsDS(plan.Play))
                            {
                                format = "balls%22%3A%7B{1}%7D%2C%22bigCodes%22%3A%22{0}%22%7D%7D%2C%22";
                            }
                            else
                            {
                                format = "balls%22%3A%7B{0}%7D%7D%7D%2C%22";
                            }
                            string str7 = "";
                            if (CommFunc.CheckPlayIsRXDS(plan.Play))
                            {
                                str7 = HttpUtility.UrlEncode(this.GetRXWZString(plan.RXWZ));
                            }
                            format = string.Format(format, this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), str7);
                            string str8 = "orderlist=%7B%22items%22%3A%7B%22{0}%22%3A%7B%22rulecode%22%3A%22{1}%22%2C%22total%22%3A%22{2}%22%2C%22itemcount%22%3A{3}%2C%22times%22%3A%22{4}%22%2C%22yjf%22%3A{5}%2C%22repoint%22%3A%22{6}%22%2C%22mode%22%3A{7}%2C%22{8}total%22%3A%22{9}%22%2C%22itemcount%22%3A{10}%2C%22ordercount%22%3A{11}%2C%22sumCount%22%3A{12}%2C%22lottery%22%3A%22{13}%22%2C%22currExpect%22%3A%22{14}%22%7D";
                            string str9 = string.Concat(new object[] { CommFunc.Random(0x3b9aca00, 0x77359400).ToString(), CommFunc.Random(100, 0x3e7), "-", CommFunc.MixRandom(8) });
                            str8 = string.Format(str8, new object[] { str9, this.GetPlayMethodID(plan.Type, plan.Play), plan.AutoTotalMoney(str4, true), num, Convert.ToInt32(plan.AutoTimes(str4, true)), num2, base.Rebate, base.Prize, format, plan.AutoTotalMoney(str4, true), num, 1, 1, this.GetPTLotteryName(plan.Type), this.GetBetsExpect(plan.CurrentExpect, "") });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str8, lotteryLine, base.BetsTime3, "UTF-8", true);
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
            pHint.Contains("未登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("操作成功") || (pResponseText == "投注成功"));

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
                string str5 = CommFunc.GetIndexString(pResponsetext, "\"balance\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str5);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/ct-data/front/dsFlush");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false).Replace("-", "");
            switch (iD)
            {
                case "SD11X5":
                case "GD11X5":
                case "JX11X5":
                case "SH11X5":
                case "AH11X5":
                    str2 = str2.Remove(8, 1);
                    break;
            }
            return str2;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/ct-data/userBets/buy");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/ct-data/openCodeList");

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/ct-data/acegi/j_acegi_security_check");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/view/game/game.html?code={this.GetPTLotteryName(pType)}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            List<string> list2;
            int num;
            string str2;
            string str3;
            string str4;
            List<string> list4;
            string str5;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                List<string> list3;
                int num2;
                ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(playName);
                list = new List<string> { 
                    "w",
                    "q",
                    "b",
                    "s",
                    "g"
                };
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    list2 = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = pNumberList[num];
                        list3 = new List<string>();
                        foreach (char ch in str2)
                        {
                            str3 = $"{ch.ToString()}";
                            list3.Add(str3);
                        }
                        str4 = CommFunc.Join(list3, ",");
                        list2.Add(str4);
                    }
                    list4 = new List<string>();
                    List<int> indexList = playInfo.IndexList;
                    if (CommFunc.CheckPlayIsRX(playName))
                    {
                        indexList = CommFunc.ConvertIntList("1-5");
                    }
                    for (num = 0; num < list2.Count; num++)
                    {
                        num2 = indexList[num] - 1;
                        str5 = list[num2];
                        str4 = list2[num];
                        if (!str4.Contains("*"))
                        {
                            string item = $"{str5}:[{str4}]";
                            list4.Add(item);
                        }
                    }
                    str = CommFunc.Join(list4, ",");
                }
                else if (playName.Contains("定位胆"))
                {
                    list3 = new List<string>();
                    foreach (string str7 in pNumberList)
                    {
                        str3 = $"{str7.ToString()}";
                        list3.Add(str3);
                    }
                    str4 = CommFunc.Join(list3, ",");
                    num2 = playInfo.IndexList[0] - 1;
                    str5 = list[num2];
                    str = $"{str5}:[{str4}]";
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        list3 = new List<string>();
                        foreach (string str7 in pNumberList)
                        {
                            str2 = $"{str7}";
                            list3.Add(str2);
                        }
                        str = $"zu:[{CommFunc.Join(list3, ",")}]";
                        if (CommFunc.CheckPlayIsRXDS(playName))
                        {
                            str = this.GetRXWZString(pRXWZ) + "," + str;
                        }
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
                    str = CommFunc.Join(pNumberList, ",");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                list = new List<string> { 
                    "f",
                    "s",
                    "t",
                    "fo",
                    "fi",
                    "si",
                    "se",
                    "ei",
                    "ni",
                    "te"
                };
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    list2 = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = pNumberList[num];
                        str5 = list[num];
                        List<string> pList = new List<string>();
                        List<string> list8 = CommFunc.SplitString(str2, " ", -1);
                        foreach (string str8 in list8)
                        {
                            string str9 = $"{str8}";
                            pList.Add(str9);
                        }
                        str3 = CommFunc.Join(pList, ",");
                        str3 = $"{str5}:[{str3}]";
                        list2.Add(str3);
                    }
                    str = CommFunc.Join(list2, ",").Replace("*", "");
                }
                else
                {
                    int num3 = -1;
                    if (playName.Contains("冠军"))
                    {
                        num3 = 0;
                    }
                    else if (playName.Contains("亚军"))
                    {
                        num3 = 1;
                    }
                    else
                    {
                        num3 = CommFunc.GetPlayNum(playName) - 1;
                    }
                    list2 = new List<string>();
                    int num4 = (playName == "猜冠军猜冠军") ? 1 : 10;
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = pNumberList[num];
                        str3 = $"{str2}";
                        list2.Add(str3);
                    }
                    list4 = new List<string>();
                    for (num = 0; num < num4; num++)
                    {
                        str4 = "*";
                        if (num == num3)
                        {
                            str5 = list[num];
                            str4 = CommFunc.Join(list2, ",");
                            str4 = $"{str5}:[{str4}]";
                            list4.Add(str4);
                        }
                    }
                    str = CommFunc.Join(list4, ",").Replace("*", "");
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
                    return "sxzhixdsq";
                }
                if (playName == "前三直选复式")
                {
                    return "sxzhixfsq";
                }
                if (playName == "前三组三复式")
                {
                    return "sxzuxzsq";
                }
                if (playName == "前三组六复式")
                {
                    return "sxzuxzlq";
                }
                if (playName == "后三直选单式")
                {
                    return "sxzhixdsh";
                }
                if (playName == "后三直选复式")
                {
                    return "sxzhixfsh";
                }
                if (playName == "后三组三复式")
                {
                    return "sxzuxzsh";
                }
                if (playName == "后三组六复式")
                {
                    return "sxzuxzlh";
                }
                if (playName == "中三直选单式")
                {
                    return "sxzhixdsz";
                }
                if (playName == "中三直选复式")
                {
                    return "sxzhixfsz";
                }
                if (playName == "中三组三复式")
                {
                    return "sxzuxzsz";
                }
                if (playName == "中三组六复式")
                {
                    return "sxzuxzlz";
                }
                if (playName == "前二直选单式")
                {
                    return "exzhixdsq";
                }
                if (playName == "前二直选复式")
                {
                    return "exzhixfsq";
                }
                if (playName == "后二直选单式")
                {
                    return "exzhixdsh";
                }
                if (playName == "后二直选复式")
                {
                    return "exzhixfsh";
                }
                if (playName == "前四直选单式")
                {
                    return "sixzhixdsq";
                }
                if (playName == "前四直选复式")
                {
                    return "sixzhixfsq";
                }
                if (playName == "后四直选单式")
                {
                    return "sixzhixdsh";
                }
                if (playName == "后四直选复式")
                {
                    return "sixzhixfsh";
                }
                if (playName == "五星直选单式")
                {
                    return "wxzhixds";
                }
                if (playName == "五星直选复式")
                {
                    return "wxzhixfs";
                }
                if (playName == "任三直选单式")
                {
                    return "rx3ds";
                }
                if (playName == "任三直选复式")
                {
                    return "rx3fs";
                }
                if (playName == "任三组三复式")
                {
                    return "rx3z3";
                }
                if (playName == "任三组六复式")
                {
                    return "rx3z6";
                }
                if (playName == "任二直选单式")
                {
                    return "rx2ds";
                }
                if (playName == "任二直选复式")
                {
                    return "rx2fs";
                }
                if (playName == "任四直选单式")
                {
                    return "rx4ds";
                }
                if (playName == "任四直选复式")
                {
                    return "rx4fs";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "dweid";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "11x5qsds";
                }
                if (playName == "前二直选单式")
                {
                    return "11x5qeds";
                }
                if (playName == "任选单式一中一")
                {
                    return "11x5rxds1z1";
                }
                if (playName == "任选单式二中二")
                {
                    return "11x5rxds2z2";
                }
                if (playName == "任选单式三中三")
                {
                    return "11x5rxds3z3";
                }
                if (playName == "任选单式四中四")
                {
                    return "11x5rxds4z4";
                }
                if (playName == "任选单式五中五")
                {
                    str = "11x5rxds5z5";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "bjpk10qian3ds";
                }
                if (playName == "猜前三复式")
                {
                    return "bjpk10qian3";
                }
                if (playName == "猜前二单式")
                {
                    return "bjpk10qian2ds";
                }
                if (playName == "猜前二复式")
                {
                    return "bjpk10qian2";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "bjpk10qian1";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "bjpk10dwd";
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
            return CommFunc.GetIndexString(pResponseText, "\"message\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "txffc";
            }
            if (pType == ConfigurationStatus.LotteryType.XDLSSC)
            {
                return "xdlxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.LD2FC)
            {
                return "ldxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.JZD15FC)
            {
                return "jzdxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.BLSFFC)
            {
                return "blsffc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYHGSSC)
            {
                return "xhgxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYDJSSC)
            {
                return "xdjxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYTXFFC)
            {
                return "xtxffc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYXJP30M)
            {
                return "xjp30mxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYFLP30M)
            {
                return "flb30mxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYFLPFFC)
            {
                return "flbffxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYFLP2FC)
            {
                return "flblfxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYJPZ30M)
            {
                return "jpz30mxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYJPZFFC)
            {
                return "jpzffxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYJPZ5FC)
            {
                return "jpzwfxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYMD30M)
            {
                return "md30mxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYMDFFC)
            {
                return "mdffxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HANYMD3FC)
            {
                return "mdsfxyssc";
            }
            if (pType == ConfigurationStatus.LotteryType.LD11X5)
            {
                return "ld11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.XDL11X5)
            {
                return "xdl11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.JZD11X5)
            {
                return "jzd11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "bjpk10";
            }
            if (pType == ConfigurationStatus.LotteryType.LDPK10)
            {
                return "ldpk10";
            }
            if (pType == ConfigurationStatus.LotteryType.XDLPK10)
            {
                return "xdlpk10";
            }
            if (pType == ConfigurationStatus.LotteryType.JZDPK10)
            {
                return "jzdpk10";
            }
            return CommFunc.GetLotteryID(pType).ToLower();
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/ct-data/acegi/j_acegi_logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            List<string> pList = new List<string>();
            foreach (int num in pRXWZ)
            {
                string item = $"{num}";
                pList.Add(item);
            }
            return $"places:[{CommFunc.Join(pList, ",")}]";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string pUrl = this.GetLine() + "/ct-data/loadOpenTime";
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                string pData = $"shortName={this.GetPTLotteryName(pType)}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"currFullExpect\":\"", "\"", 0);
                    base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
                }
                if (base.Prize == "")
                {
                    pUrl = this.GetLine() + "/ct-data/lottery";
                    lotteryLine = this.GetLotteryLine(pType, false);
                    pResponsetext = "";
                    string str5 = "wxzhixfs";
                    if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                    {
                        str5 = "bjpk10qian1";
                    }
                    else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                    {
                        str5 = "11x5qsfs";
                    }
                    pData = $"shortName={this.GetPTLotteryName(pType)}&rulecode={str5}";
                    HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        base.Prize = CommFunc.GetIndexString(pResponsetext, "\"maxMode\":", ",", 0);
                        double num = 0.128;
                        if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                        {
                            num = 0.125;
                            if (AppInfo.Current.Lottery.Type == ConfigurationStatus.LotteryType.PK10)
                            {
                                num = 0.128;
                            }
                        }
                        else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                        {
                            num = 0.128;
                        }
                        double num2 = Convert.ToDouble(CommFunc.GetIndexString(pResponsetext, "\"repoint\":", ",", 0)) - num;
                        base.Rebate = (num2 > 0.0) ? ((num2 * 100.0)).ToString("0.0") : "0";
                        double num3 = Convert.ToDouble(CommFunc.GetIndexString(pResponsetext, "\"maxModeLi\":", ",", 0));
                        if (Convert.ToDouble(base.Prize) > num3)
                        {
                            base.Rebate = "0";
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
                string str2 = $"/ct-data/acegi/captcha?{DateTime.Now.ToOADate()}";
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
                string pData = $"j_username={pID}&j_password={str5}&validateCode={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("操作成功") && pResponsetext.Contains(pID);
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"message\":\"", "\"", 0);
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
            string line = this.GetLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, line, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("航洋国际");
            return true;
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
            if (!this.InputWeb(pID, pW, ref pHint))
            {
                return false;
            }
            return true;
        }
    }
}

