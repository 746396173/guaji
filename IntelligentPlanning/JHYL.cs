namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class JHYL : PTBase
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
                            string format = "action=AddBetting&data=%7B%22BettingData%22%3A%5B%7B%22lottery_code%22%3A%22{0}%22%2C%22play_detail_code%22%3A%22{1}%22%2C%22betting_issuseNo%22%3A%22{2}%22%2C%22betting_number%22%3A%22{3}%22%2C%22betting_count%22%3A{4}%2C%22graduation_count%22%3A{5}%2C%22betting_money%22%3A{6}%2C%22betting_point%22%3A%22{7}-0.0%22%2C%22betting_model%22%3A{8}%7D%5D%7D";
                            string str7 = (Convert.ToDouble(this.GetPrize(plan.Type, plan.Play)) / Math.Pow(10.0, (double) (plan.Unit - 1))).ToString();
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetBetsExpect(plan.CurrentExpect, ""), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), num, Convert.ToInt32(plan.AutoTimes(str4, true)), plan.AutoTotalMoney(str4, true), str7, plan.Money / 2.0 });
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
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"Data\":[\"", "\"", 0);
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
            return CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false).Replace("-", "");
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
            if (pType == ConfigurationStatus.LotteryType.MDHGSSC)
            {
                return "1018";
            }
            if (pType == ConfigurationStatus.LotteryType.MDHG90M)
            {
                return "1007";
            }
            if (pType == ConfigurationStatus.LotteryType.HCSSC)
            {
                return "1022";
            }
            if (pType == ConfigurationStatus.LotteryType.JNDSSC)
            {
                return "1015";
            }
            if (pType == ConfigurationStatus.LotteryType.JHTXFFC)
            {
                return "1019";
            }
            if (pType == ConfigurationStatus.LotteryType.JHQQFFC)
            {
                return "1020";
            }
            if (pType == ConfigurationStatus.LotteryType.XJP120M)
            {
                return "1010";
            }
            if (pType == ConfigurationStatus.LotteryType.XJP15F)
            {
                return "1021";
            }
            if (pType == ConfigurationStatus.LotteryType.XDL90M)
            {
                return "1016";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "1017";
            }
            if (pType == ConfigurationStatus.LotteryType.WHDJSSC)
            {
                return "1011";
            }
            if (pType == ConfigurationStatus.LotteryType.JHJPZ15C)
            {
                return "1012";
            }
            if (pType == ConfigurationStatus.LotteryType.JH2FC)
            {
                return "1013";
            }
            if (pType == ConfigurationStatus.LotteryType.JH5FC)
            {
                return "1014";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "1303";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/tools/ssc_ajax.ashx");

        public override string GetIndexLine() => 
            (this.GetLine() + "/index.html");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login.html");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            string.Format("{0}/gameBet_cqssc-{1}.html?lottery={1}", this.GetLine(), this.GetBetsLotteryID(pType));

        public string GetLotteryType(ConfigurationStatus.LotteryType pType)
        {
            string str = "F11";
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "A11";
            }
            return str;
        }

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
                        str3 = CommFunc.Join(pStr, "+", -1);
                        list.Add(str3);
                    }
                    str = CommFunc.Join(list, "%2C").Replace("*", "-");
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
                str = CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                if (str != "")
                {
                    str = this.GetBetsLotteryID(pType) + str;
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    str = "C12";
                }
                else if (playName == "猜前三复式")
                {
                    str = "C11";
                }
                else if (playName == "猜前二单式")
                {
                    str = "B12";
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
                    str = "A11";
                }
                else if (playName.Contains("定位胆"))
                {
                    str = "K11";
                }
                if (str != "")
                {
                    str = this.GetBetsLotteryID(pType) + str;
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
                            pNum /= 11.1111;
                        }
                        else if (playName.Contains("三"))
                        {
                            pNum /= 1.3889;
                        }
                        else if (playName.Contains("四"))
                        {
                            pNum /= 0.1984;
                        }
                        else if (playName.Contains("五"))
                        {
                            pNum /= 0.033;
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
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"StrCode\":\"", "\"", 0).Replace("-", ""));
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
                    string pUrl = this.GetLine() + "/tools/ssc_ajax.ashx";
                    string lotteryLine = this.GetLotteryLine(pType, false);
                    string pResponsetext = "";
                    string pData = string.Format("action=get_lottery_rebate_list&lottery_code={0}&play_code={0}{1}", this.GetBetsLotteryID(pType), this.GetLotteryType(pType));
                    HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        double num = Convert.ToDouble(CommFunc.GetIndexString(pResponsetext, "\"Data\":[\"", "\"", 0).Split(new char[] { '-' })[0]);
                        if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                        {
                            num *= 100.0;
                        }
                        base.Prize = num.ToString();
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
                string str2 = $"/ashx/GetImage.ashx?temp={DateTime.Now.ToOADate()}";
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
            string pUrl = this.GetLine() + "/ashx/login.ashx";
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string pData = $"userName={pID}&passWord={str4}";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
            flag = pResponsetext.Contains("\"Code\":1");
            if (!flag)
            {
                pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "\"StrCode\":\"", "\"", 0));
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, base.BetsTime2, "UTF-8", true);
            return pResponsetext.Contains("金狐");
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

