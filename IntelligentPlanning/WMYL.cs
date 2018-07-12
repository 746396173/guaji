namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class WMYL : PTBase
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
                            string format = "action=AddBetting&data=%7B%22BettingData%22%3A%5B%7B%22lottery_code%22%3A%22{0}%22%2C%22play_detail_code%22%3A%22{1}%22%2C%22betting_issuseNo%22%3A%22{2}%22%2C%22betting_number%22%3A%22{3}%22%2C%22betting_count%22%3A{4}%2C%22graduation_count%22%3A{5}%2C%22betting_money%22%3A{6}%2C%22betting_point%22%3A%22{7}-0.0%25%22%2C%22betting_model%22%3A{8}%2C%22md_pattern%22%3A{9}%7D%5D%7D";
                            string str7 = (Convert.ToDouble(this.GetPrize(plan.Type, plan.Play)) / Math.Pow(10.0, (double) (plan.Unit - 1))).ToString();
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetBetsExpect(plan.CurrentExpect, ""), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), num, Convert.ToInt32(plan.AutoTimes(str4, true)), plan.AutoTotalMoney(str4, true), str7, plan.Money / 2.0, "1" });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
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
            (pResponseText.Contains("\"Code\":1") || (pResponseText == "投注成功"));

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
                string pData = "action=get_user_money";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"Data\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/tools/ssc_ajax.ashx");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, true, true, false);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/tools/ssc_ajax.ashx");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1000";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "1001";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "1003";
            }
            if (pType == ConfigurationStatus.LotteryType.JNDSSC)
            {
                return "1013";
            }
            if (pType == ConfigurationStatus.LotteryType.WMFFC)
            {
                return "1008";
            }
            if (pType == ConfigurationStatus.LotteryType.WM2FC)
            {
                return "1009";
            }
            if (pType == ConfigurationStatus.LotteryType.WMTWSSC)
            {
                return "1006";
            }
            if (pType == ConfigurationStatus.LotteryType.WMXXLSSC)
            {
                return "1007";
            }
            if (pType == ConfigurationStatus.LotteryType.WMHGSSC)
            {
                return "1015";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "1012";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "1102";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "1100";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "1103";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "1101";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "1303";
            }
            if (pType == ConfigurationStatus.LotteryType.WMPK10)
            {
                str = "1307";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/tools/ssc_ajax.ashx");

        public override string GetIndexLine() => 
            (this.GetLine() + "/index");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/bet/{this.GetBetsLotteryID(pType)}";

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
                    string str2;
                    if (CommFunc.CheckPlayIsRXFS(playName))
                    {
                        list = new List<string>();
                        for (num = 0; num < pNumberList.Count; num++)
                        {
                            str2 = pNumberList[num];
                            str3 = CommFunc.Join(str2, "+", -1);
                            list.Add(str3);
                        }
                        str = CommFunc.Join(list, "%2C").Replace("*", "");
                    }
                    else
                    {
                        list = new List<string>();
                        for (num = 0; num < pNumberList.Count; num++)
                        {
                            str2 = pNumberList[num];
                            str3 = CommFunc.Join(str2, "+", -1);
                            list.Add(str3);
                        }
                        str = CommFunc.Join(list, "%2C").Replace("*", "");
                    }
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
                            str3 = CommFunc.Join(pNumberList, "+");
                        }
                        list.Add(str3);
                    }
                    str = CommFunc.Join(list, "%2C").Replace("*", "-");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, "+");
                    }
                    else
                    {
                        str = CommFunc.Join(pNumberList, "%2C");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "%2C");
                }
                if (CommFunc.CheckPlayIsRXDS(playName))
                {
                    str = str + "%26" + this.GetRXWZString(pRXWZ);
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, " ");
                }
                return HttpUtility.UrlEncode(str);
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                str = CommFunc.Join(pNumberList, ",");
            }
            else if (CommFunc.CheckPlayIsFS(playName))
            {
                str = CommFunc.Join(pNumberList, ",").Replace("*", "-");
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
                        str3 = CommFunc.Join(pNumberList, " ");
                    }
                    list.Add(str3);
                }
                str = CommFunc.Join(list, ",").Replace("*", "-");
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
                    str = "F12";
                }
                else if (playName == "前三直选复式")
                {
                    str = "F11";
                }
                else if (playName == "前三组三复式")
                {
                    str = "F22";
                }
                else if (playName == "前三组三单式")
                {
                    str = "F26";
                }
                else if (playName == "前三组六复式")
                {
                    str = "F23";
                }
                else if (playName == "前三组六单式")
                {
                    str = "F27";
                }
                else if (playName == "后三直选单式")
                {
                    str = "D12";
                }
                else if (playName == "后三直选复式")
                {
                    str = "D11";
                }
                else if (playName == "后三组三复式")
                {
                    str = "D22";
                }
                else if (playName == "后三组三单式")
                {
                    str = "D26";
                }
                else if (playName == "后三组六复式")
                {
                    str = "D23";
                }
                else if (playName == "后三组六单式")
                {
                    str = "D27";
                }
                else if (playName == "中三直选单式")
                {
                    str = "E12";
                }
                else if (playName == "中三直选复式")
                {
                    str = "E11";
                }
                else if (playName == "中三组三复式")
                {
                    str = "E22";
                }
                else if (playName == "中三组三单式")
                {
                    str = "E26";
                }
                else if (playName == "中三组六复式")
                {
                    str = "E23";
                }
                else if (playName == "中三组六单式")
                {
                    str = "E27";
                }
                else if (playName == "前二直选单式")
                {
                    str = "C12";
                }
                else if (playName == "前二直选复式")
                {
                    str = "C11";
                }
                else if (playName == "后二直选单式")
                {
                    str = "B12";
                }
                else if (playName == "后二直选复式")
                {
                    str = "B11";
                }
                else if (playName == "后四直选单式")
                {
                    str = "G12";
                }
                else if (playName == "后四直选复式")
                {
                    str = "G11";
                }
                else if (playName == "五星直选单式")
                {
                    str = "H12";
                }
                else if (playName == "五星直选复式")
                {
                    str = "H11";
                }
                else if (playName == "任三直选单式")
                {
                    str = "K12";
                }
                else if (playName == "任三直选复式")
                {
                    str = "K11";
                }
                else if (playName == "任三组三复式")
                {
                    str = "K21";
                }
                else if (playName == "任三组三单式")
                {
                    str = "K22";
                }
                else if (playName == "任三组六复式")
                {
                    str = "K23";
                }
                else if (playName == "任三组六单式")
                {
                    str = "K24";
                }
                else if (playName == "任二直选单式")
                {
                    str = "J12";
                }
                else if (playName == "任二直选复式")
                {
                    str = "J11";
                }
                else if (playName == "任四直选单式")
                {
                    str = "L12";
                }
                else if (playName == "任四直选复式")
                {
                    str = "L11";
                }
                else if (playName.Contains("定位胆"))
                {
                    str = "A11";
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    str = "C12";
                }
                else if (playName == "前二直选单式")
                {
                    str = "B12";
                }
                else if (playName == "任选复式一中一")
                {
                    str = "A31";
                }
                else if (playName == "任选复式二中二")
                {
                    str = "B31";
                }
                else if (playName == "任选复式三中三")
                {
                    str = "C31";
                }
                else if (playName == "任选复式四中四")
                {
                    str = "D11";
                }
                else if (playName == "任选复式五中五")
                {
                    str = "E11";
                }
                else if (playName == "任选单式一中一")
                {
                    str = "A32";
                }
                else if (playName == "任选单式二中二")
                {
                    str = "B32";
                }
                else if (playName == "任选单式三中三")
                {
                    str = "C32";
                }
                else if (playName == "任选单式四中四")
                {
                    str = "D12";
                }
                else if (playName == "任选单式五中五")
                {
                    str = "E12";
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    str = "";
                }
                else if (playName == "猜前三复式")
                {
                    str = "C11";
                }
                else if (playName == "猜前二单式")
                {
                    str = "";
                }
                else if (playName == "猜前二复式")
                {
                    str = "B11";
                }
                else if (playName == "猜前四单式")
                {
                    str = "";
                }
                else if (playName == "猜前四复式")
                {
                    str = "D11";
                }
                else if (playName == "猜前五单式")
                {
                    str = "";
                }
                else if (playName == "猜前五复式")
                {
                    str = "E11";
                }
                else if (playName == "猜冠军猜冠军")
                {
                    str = "A11";
                }
                else if (playName.Contains("定位胆"))
                {
                    str = "I11";
                }
            }
            return (this.GetBetsLotteryID(pType) + str);
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
                            pNum = 0.33401578947368421 * Convert.ToInt32(base.Prize);
                        }
                        else if (playName.Contains("组六"))
                        {
                            pNum = 0.16666842105263158 * Convert.ToInt32(base.Prize);
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
                    else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                    {
                        if (playName.Contains("定位胆") || playName.Contains("猜冠军猜冠军"))
                        {
                            pNum /= 100.0;
                        }
                        else if (playName.Contains("二"))
                        {
                            pNum = 0.09 * Convert.ToInt32(base.Prize);
                        }
                        else if (playName.Contains("三"))
                        {
                            pNum = 0.72 * Convert.ToInt32(base.Prize);
                        }
                        else if (playName.Contains("四"))
                        {
                            pNum = 5.04 * Convert.ToInt32(base.Prize);
                        }
                        else if (playName.Contains("五"))
                        {
                            pNum = 5.0 * Convert.ToInt32(base.Prize);
                        }
                    }
                    return CommFunc.TwoDouble(pNum, true);
                }
                this.GetSite(pType, "");
                num++;
            }
        }

        public override string GetPTHint(string pResponseText)
        {
            string str = "";
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            str = CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"StrCode\":\"", "\"", 0));
            if (str == "")
            {
                str = CommFunc.GetIndexString(pResponseText, "\"StrCode\":\"", "\"", 0);
            }
            return str;
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/tools/ssc_ajax.ashx");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ) => 
            CommFunc.Join(pRXWZ, "%2C");

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.Prize == "")
                {
                    string lotteryLine = this.GetLotteryLine(pType, false);
                    string pReferer = this.GetLotteryLine(pType, false);
                    string pResponsetext = "";
                    HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        base.Rebate = CommFunc.GetIndexString(pResponsetext, "rebatePoint = ", ";", 0);
                        base.Prize = (1900.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0)).ToString();
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
                string str2 = $"/imagecode?t={DateTime.Now.ToOADate()}";
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
                string pUrl = this.GetLine() + "/login?action=login";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"action=login&userName={pID}&passWord={str5}&verCode={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
                flag = pResponsetext.Contains("\"Code\":1");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"StrCode\":\"", "\"", 0);
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
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, base.BetsTime2, "UTF-8", true);
            return pResponsetext.Contains("万美娱乐");
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            string pData = "action=LogOut";
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

