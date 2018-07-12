namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class JHC : PTBase
    {
        public override bool BetsMain(ConfigurationStatus.SCPlan plan, ref string pHint)
        {
            bool flag = false;
            try
            {
                string betsLine = this.GetBetsLine(plan.Type);
                string lotteryLine = this.GetLotteryLine(plan.Type, true);
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
                            string str7;
                            List<string> pTNumberList = plan.GetPTNumberList(dictionary2[str4]);
                            string pResponsetext = "";
                            int num = plan.FNNumber(str4);
                            string str6 = Guid.NewGuid().ToString();
                            if (this.CheckLotteryIsVR(plan.Type))
                            {
                                str7 = "LotteryGameID={0}&IssueSerialNumber=&Bets%5B0%5D%5BBetTypeCode%5D={1}&Bets%5B0%5D%5BBetTypeName%5D=&Bets%5B0%5D%5BNumber%5D={2}&Bets%5B0%5D%5BPosition%5D={3}&Bets%5B0%5D%5BUnit%5D={4}&Bets%5B0%5D%5BMultiple%5D={5}&Bets%5B0%5D%5BIsCompressed%5D=false&StopIfWin=false&__RequestVerificationToken={6}&SerialNumber={7}&Guid={8}";
                                str7 = string.Format(str7, new object[] { this.GetBetsLotteryID(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), this.GetRXWZString(plan.RXWZ), plan.Money, Convert.ToInt32(plan.AutoTimes(str4, true)), base.Token, this.GetBetsExpect(plan.CurrentExpect, ""), str6 });
                                HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str7, lotteryLine, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                            }
                            else
                            {
                                string format = "\"BetTypeCode\":{0},\"BetTypeName\":\"\",\"Number\":\"{1}\",\"Position\":\"{4}\",\"Unit\":{2},\"Multiple\":{3},\"ReturnRate\":0,\"IsCompressed\":false,\"CompressedNumber\":\"\"";
                                format = string.Format(format, new object[] { this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), plan.Money, Convert.ToInt32(plan.AutoTimes(str4, true)), this.GetRXWZString(plan.RXWZ) });
                                format = "{" + format + "}";
                                str7 = "\"LotteryGameID\":{0},\"SerialNumber\":\"{1}\",\"Bets\":[{2}],\"Schedules\":[],\"StopIfWin\":false,\"Guid\":\"{3}\"";
                                str7 = string.Format(str7, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), format, str6 });
                                str7 = "{" + str7 + "}";
                                HttpHelper.GetResponse1(ref pResponsetext, betsLine, "POST", str7, lotteryLine, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                            }
                            flag = this.CheckReturn(pResponsetext, true);
                            pHint = this.GetReturn(pResponsetext);
                            if (!(flag || !this.CheckBreakConnect(pResponsetext)))
                            {
                                AppInfo.PTInfo.PTIsBreak = true;
                            }
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
            (pResponseText.Contains("\"ErrorMessageCode\":0") || (pResponseText == "投注成功"));

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
                string str5 = pResponsetext;
                AppInfo.Account.BankBalance = Convert.ToDouble(str5);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/Home/GetWalletAmount");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            if (iD == "XJ11X5")
            {
                return str2.Replace("-0", "");
            }
            return str2.Replace("-", "");
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType)
        {
            string str = $"{this.GetLine()}/Bet/Confirm";
            if (this.CheckLotteryIsVR(pType))
            {
                str = $"{this.GetVRLine()}/Bet/Confirm";
            }
            return str;
        }

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "32";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "60";
            }
            if (pType == ConfigurationStatus.LotteryType.JHCDBFFC)
            {
                return "69";
            }
            if (pType == ConfigurationStatus.LotteryType.JHCJDSSC)
            {
                return "68";
            }
            if (pType == ConfigurationStatus.LotteryType.JHCFFC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.JHC2FC)
            {
                return "40";
            }
            if (pType == ConfigurationStatus.LotteryType.JHC5FC)
            {
                return "66";
            }
            if (pType == ConfigurationStatus.LotteryType.JHCJZFFC)
            {
                return "82";
            }
            if (pType == ConfigurationStatus.LotteryType.VRSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.VRHXSSC)
            {
                return "12";
            }
            if (pType == ConfigurationStatus.LotteryType.VR3FC)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "13";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "10";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "12";
            }
            if (pType == ConfigurationStatus.LotteryType.JS11X5)
            {
                return "51";
            }
            if (pType == ConfigurationStatus.LotteryType.LN11X5)
            {
                return "53";
            }
            if (pType == ConfigurationStatus.LotteryType.HEB11X5)
            {
                return "54";
            }
            if (pType == ConfigurationStatus.LotteryType.HLJ11X5)
            {
                return "65";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "19";
            }
            if (pType == ConfigurationStatus.LotteryType.VRKT)
            {
                str = "13";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType)
        {
            string lotteryLine = this.GetLine() + "/Bet/GameInfo";
            if (this.CheckLotteryIsVR(pType))
            {
                lotteryLine = this.GetLotteryLine(pType, false);
            }
            return lotteryLine;
        }

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/Account/Login");

        public override string GetLoginLinePW() => 
            (this.GetLine() + "/Account/LoginValidate");

        public bool GetLoginYZM()
        {
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLine() + "/Account/Captcha";
            string pResponsetext = "";
            string pData = "CaptchaError=False";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("/DefaultCaptcha/Generate?t=");
            if (flag)
            {
                base.VerifyCodeToken = CommFunc.GetIndexString(pResponsetext, "/DefaultCaptcha/Generate?t=", "\"", 0);
            }
            return flag;
        }

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false)
        {
            string str = $"{this.GetLine()}/Bet/Index/{this.GetBetsLotteryID(pType)}";
            if (this.CheckLotteryIsVR(pType))
            {
                str = $"{this.GetVRLine()}/Bet/Index/{this.GetBetsLotteryID(pType)}";
            }
            return str;
        }

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
                        str2 = pNumberList[num];
                        list.Add(str2);
                    }
                    return CommFunc.Join(list, ",").Replace("*", "");
                }
                if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num2 = AppInfo.FiveDic[ch.ToString()];
                    list = new List<string>();
                    for (num = 0; num < 5; num++)
                    {
                        str2 = "*";
                        if (num == num2)
                        {
                            str2 = CommFunc.Join(pNumberList);
                        }
                        list.Add(str2);
                    }
                    return CommFunc.Join(list, ",").Replace("*", "");
                }
                if (!CommFunc.CheckPlayIsLH(playName) && CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        return CommFunc.Join(pNumberList);
                    }
                    return CommFunc.Join(pNumberList, ",");
                }
                return CommFunc.Join(pNumberList, ",");
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, ",");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, " ");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                return CommFunc.Join(pNumberList, ",");
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                return CommFunc.Join(pNumberList, ",").Replace("*", "");
            }
            if (CommFunc.CheckPlayIsHZ(playName))
            {
                return CommFunc.Join(pNumberList, ",");
            }
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
                str2 = "*";
                if (num == num2)
                {
                    str2 = CommFunc.Join(pNumberList, " ");
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
                    return "6";
                }
                if (playName == "前三直选复式")
                {
                    return "5";
                }
                if (playName == "前三组三复式")
                {
                    return "54";
                }
                if (playName == "前三组三单式")
                {
                    return "55";
                }
                if (playName == "前三组六复式")
                {
                    return "56";
                }
                if (playName == "前三组六单式")
                {
                    return "57";
                }
                if (playName == "后三直选单式")
                {
                    return "10";
                }
                if (playName == "后三直选复式")
                {
                    return "9";
                }
                if (playName == "后三组三复式")
                {
                    return "62";
                }
                if (playName == "后三组三单式")
                {
                    return "63";
                }
                if (playName == "后三组六复式")
                {
                    return "64";
                }
                if (playName == "后三组六单式")
                {
                    return "65";
                }
                if (playName == "中三直选单式")
                {
                    return "8";
                }
                if (playName == "中三直选复式")
                {
                    return "7";
                }
                if (playName == "中三组三复式")
                {
                    return "58";
                }
                if (playName == "中三组三单式")
                {
                    return "59";
                }
                if (playName == "中三组六复式")
                {
                    return "60";
                }
                if (playName == "中三组六单式")
                {
                    return "61";
                }
                if (playName == "前二直选单式")
                {
                    return "12";
                }
                if (playName == "前二直选复式")
                {
                    return "11";
                }
                if (playName == "后二直选单式")
                {
                    return "14";
                }
                if (playName == "后二直选复式")
                {
                    return "13";
                }
                if (playName == "后四直选单式")
                {
                    return "4";
                }
                if (playName == "后四直选复式")
                {
                    return "3";
                }
                if (playName == "五星直选单式")
                {
                    return "2";
                }
                if (playName == "五星直选复式")
                {
                    return "1";
                }
                if (playName == "任三直选单式")
                {
                    return "18";
                }
                if (playName == "任三直选复式")
                {
                    return "17";
                }
                if (playName == "任三组三单式")
                {
                    return "125";
                }
                if (playName == "任三组三复式")
                {
                    return "124";
                }
                if (playName == "任三组六单式")
                {
                    return "127";
                }
                if (playName == "任三组六复式")
                {
                    return "126";
                }
                if (playName == "任二直选单式")
                {
                    return "20";
                }
                if (playName == "任二直选复式")
                {
                    return "19";
                }
                if (playName == "任四直选单式")
                {
                    return "16";
                }
                if (playName == "任四直选复式")
                {
                    return "15";
                }
                if (playName.Contains("定位胆"))
                {
                    return "21";
                }
                if (playName.Contains("龙虎"))
                {
                    if (this.CheckLotteryIsVR(pType))
                    {
                        if (playName == "龙虎万千")
                        {
                            return "184";
                        }
                        if (playName == "龙虎万百")
                        {
                            return "185";
                        }
                        if (playName == "龙虎万十")
                        {
                            return "186";
                        }
                        if (playName == "龙虎万个")
                        {
                            return "187";
                        }
                        if (playName == "龙虎千百")
                        {
                            return "188";
                        }
                        if (playName == "龙虎千十")
                        {
                            return "189";
                        }
                        if (playName == "龙虎千个")
                        {
                            return "190";
                        }
                        if (playName == "龙虎百十")
                        {
                            return "191";
                        }
                        if (playName == "龙虎百个")
                        {
                            return "192";
                        }
                        if (playName == "龙虎十个")
                        {
                            str = "193";
                        }
                        return str;
                    }
                    if (playName == "龙虎万千")
                    {
                        return "149";
                    }
                    if (playName == "龙虎万百")
                    {
                        return "150";
                    }
                    if (playName == "龙虎万十")
                    {
                        return "151";
                    }
                    if (playName == "龙虎万个")
                    {
                        return "152";
                    }
                    if (playName == "龙虎千百")
                    {
                        return "153";
                    }
                    if (playName == "龙虎千十")
                    {
                        return "154";
                    }
                    if (playName == "龙虎千个")
                    {
                        return "155";
                    }
                    if (playName == "龙虎百十")
                    {
                        return "156";
                    }
                    if (playName == "龙虎百个")
                    {
                        return "157";
                    }
                    if (playName == "龙虎十个")
                    {
                        str = "158";
                    }
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "1002";
                }
                if (playName == "前二直选单式")
                {
                    return "1008";
                }
                if (playName == "任选复式一中一")
                {
                    return "1037";
                }
                if (playName == "任选复式二中二")
                {
                    return "1039";
                }
                if (playName == "任选复式三中三")
                {
                    return "1041";
                }
                if (playName == "任选复式四中四")
                {
                    return "1043";
                }
                if (playName == "任选复式五中五")
                {
                    return "1045";
                }
                if (playName == "任选单式一中一")
                {
                    return "1038";
                }
                if (playName == "任选单式二中二")
                {
                    return "1040";
                }
                if (playName == "任选单式三中三")
                {
                    return "1042";
                }
                if (playName == "任选单式四中四")
                {
                    return "1044";
                }
                if (playName == "任选单式五中五")
                {
                    str = "1046";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "3002";
                }
                if (playName == "猜前三复式")
                {
                    return "3001";
                }
                if (playName == "猜前三和值")
                {
                    return "3028";
                }
                if (playName == "猜前二单式")
                {
                    return "3004";
                }
                if (playName == "猜前二复式")
                {
                    return "3003";
                }
                if (playName == "猜前二和值")
                {
                    return "3023";
                }
                if (playName == "猜前四单式")
                {
                    return "3010";
                }
                if (playName == "猜前四复式")
                {
                    return "3009";
                }
                if (playName == "猜前五单式")
                {
                    return "3012";
                }
                if (playName == "猜前五复式")
                {
                    return "3011";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "3005";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "3007";
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
            return CommFunc.GetIndexString(pResponseText, "\"ErrorMessage\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            CommFunc.GetLotteryID(pType);

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/Account/LogOff");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            string str = "";
            if (pRXWZ == null)
            {
                return str;
            }
            List<int> pList = new List<int>();
            for (int i = 0; i < pRXWZ.Count; i++)
            {
                pList.Add(pRXWZ[i] + 1);
            }
            return CommFunc.Join(pList, ",");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (this.CheckLotteryIsVR(pType) && (base.Prize == ""))
                {
                    string pUrl = $"{this.GetLine()}/Bet/GameInfo";
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    string pData = $"lotteryGameId={1}";
                    HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        string str5 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                        base.Prize = (1700.0 + ((Convert.ToDouble(str5) * 2.0) * 10.0)).ToString();
                    }
                }
            }
            catch
            {
            }
        }

        public bool GetVRData(ConfigurationStatus.LotteryType pType, ref string pVRData)
        {
            string indexLine = this.GetIndexLine();
            string pUrl = $"{this.GetLine()}/Home/VR?id={this.GetBetsLotteryID(pType)}";
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, base.BetsTime1, "UTF-8", true);
            bool flag = pResponsetext.Contains("data=");
            if (flag)
            {
                pVRData = CommFunc.GetIndexString(pResponsetext, "data=", "\"", 0);
            }
            return flag;
        }

        public override string GetVRLine() => 
            "http://vrbetapi.com";

        public override string GetWebVerifyCode(string pVerifyCodeFile)
        {
            string pVerifyCode = "";
            try
            {
                bool flag;
                string loginLine = this.GetLoginLine();
                string pUrl = this.GetLine() + "/DefaultCaptcha/Refresh";
                string pResponsetext = "";
                string pData = $"t={base.VerifyCodeToken}";
                int num = 0;
                goto Label_007E;
            Label_003D:
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                num++;
                if ((pResponsetext != "") || (num >= 2))
                {
                    goto Label_0083;
                }
            Label_007E:
                flag = true;
                goto Label_003D;
            Label_0083:
                if (pResponsetext == "")
                {
                    return pVerifyCode;
                }
                base.VerifyCodeToken = CommFunc.GetIndexString(pResponsetext, "\"value\", \"", "\"", 0);
                string str6 = $"/DefaultCaptcha/Generate?t={base.VerifyCodeToken}";
                string str7 = this.GetLine() + str6;
                File.Delete(pVerifyCodeFile);
                Bitmap bitmap = new Bitmap(HttpHelper.GetResponseImage(str7, loginLine, "GET", "", 0x1770, "UTF-8", true));
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
                string pUrl = this.GetLine() + "/Account/LoginVerify";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"__RequestVerificationToken={base.Token}&LoginID={pID}&Password={str5}&CaptchaDeText={base.VerifyCodeToken}&CaptchaInputText={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("投注记录") || pResponsetext.Contains("用户协议");
                if (!flag)
                {
                    pHint = pResponsetext;
                    if (pHint.Contains("验证码错误"))
                    {
                        pHint = "";
                        return this.InputWeb(pID, pW, ref pHint);
                    }
                    pHint = CommFunc.GetIndexString(pResponsetext, "LoginID\" data-valmsg-replace=\"true\">", "<", 0);
                }
            }
            return flag;
        }

        public bool LoginVRWeb(ConfigurationStatus.LotteryType pType, string pVRData)
        {
            string pReferer = "";
            string pUrl = $"{this.GetVRLine()}/Account/LoginValidate?version=1.0&id=JHC&data={pVRData}";
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, base.BetsTime1, "UTF-8", true);
            bool flag = pResponsetext.Contains("竞速娱乐 VR");
            if (flag)
            {
                base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, base.BetsTime1, "UTF-8", true);
            bool flag = pResponsetext.Contains("金皇朝");
            if (flag)
            {
                base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
            }
            return flag;
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            string pData = $"__RequestVerificationToken={base.Token}";
            HttpHelper.GetResponse(ref pResponsetext, quitPTLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
        }

        public override bool VRWebLoginMain(ConfigurationStatus.LotteryType pType)
        {
            if (this.CheckLotteryIsVR(pType) && !base.IsLoginVR)
            {
                string pVRData = "";
                if (!this.GetVRData(pType, ref pVRData))
                {
                    return false;
                }
                HttpHelper.SaveCookies(HttpHelper.GetHttpHelperCookieString(this.GetUrlLine(), null), this.GetVRHostLine());
                if (!this.LoginVRWeb(pType, pVRData))
                {
                    return false;
                }
                base.IsLoginVR = true;
            }
            return true;
        }

        public override bool WebLoginMain(string pID, string pW, ref string pHint)
        {
            if (!this.LoginWeb())
            {
                return false;
            }
            if (!this.GetLoginYZM())
            {
                return false;
            }
            if (!this.InputWeb(pID, pW, ref pHint))
            {
                return false;
            }
            base.IsLoginVR = false;
            return true;
        }
    }
}

