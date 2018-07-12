namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class WJSJ : PTBase
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
                            List<string> pList = new List<string> {
                                "10" + this.GetBetsLotteryID(plan.Type).PadLeft(2, '0') + this.GetPlayMethodID(plan.Type, plan.Play),
                                this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ),
                                num.ToString()
                            };
                            string str = CommFunc.Join(pList, "+");
                            string format = "Model=Game&Action=AddBuy&Id={0}&Issue={1}&BuyCode={2}&BuyChase=&IsChaseCode=0&Multiple={3}&Rebate={4}&TheTotalAmount={5}";
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), HttpUtility.UrlEncode(str), plan.AutoMoney(str4, true), base.Rebate, plan.AutoTotalMoney(str4, true) });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, base.BetsTime2, "UTF-8", true);
                            flag = this.CheckReturn(pResponsetext, true);
                            pHint = this.GetReturn(pResponsetext);
                            if (!(flag || !this.CheckBreakConnect(pResponsetext)))
                            {
                                AppInfo.PTInfo.PTIsBreak = true;
                            }
                            Thread.Sleep(0xbb8);
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
            (pResponseText.Contains("投注成功") || (pResponseText == "投注成功"));

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
                string pData = "Model=User&Action=GetUserInfo&r=yes";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"U_Money\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
                string str5 = CommFunc.GetIndexString(pResponsetext, "\"U_RebateA\":\"", "\"", 0);
                base.Prize = (1700.0 + ((Convert.ToDouble(str5) * 2.0) * 10.0)).ToString();
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string name = AppInfo.Current.Lottery.Name;
            string iD = AppInfo.Current.Lottery.ID;
            string str3 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            if (name.Contains("11选5"))
            {
                str3 = str3.Replace("-0", "-");
            }
            return str3;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "3";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.HGSSC)
            {
                return "14";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.WHDJSSC)
            {
                return "15";
            }
            if (pType == ConfigurationStatus.LotteryType.JH15C)
            {
                return "20";
            }
            if (pType == ConfigurationStatus.LotteryType.KLFFC)
            {
                return "17";
            }
            if (pType == ConfigurationStatus.LotteryType.KL2FC)
            {
                return "16";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "13";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/");

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            (this.GetLine() + "/");

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            int num;
            int num2;
            List<string> list2;
            string str3;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                List<string> list;
                string str2;
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = pNumberList[num];
                        list.Add(CommFunc.Join(str2, ",", -1));
                    }
                    str = CommFunc.Join(list, "|").Replace("*", "");
                }
                else if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num2 = AppInfo.FiveDic[ch.ToString()];
                    list2 = new List<string>();
                    for (num = 0; num < 5; num++)
                    {
                        str3 = "*";
                        if (num == num2)
                        {
                            str3 = CommFunc.Join(pNumberList, ",");
                        }
                        list2.Add(str3);
                    }
                    str = CommFunc.Join(list2, "|").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, ",");
                    }
                }
                else
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = pNumberList[num];
                        list.Add(CommFunc.Join(str2, "|", -1));
                    }
                    str = CommFunc.Join(list, "$");
                }
                if (CommFunc.CheckPlayIsRXDS(playName))
                {
                    str = this.GetRXWZString(pRXWZ) + str;
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, "|");
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
                return CommFunc.Join(pNumberList, "$").Replace(" ", "|");
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                list2 = new List<string>();
                for (num = 0; num < pNumberList.Count; num++)
                {
                    str3 = pNumberList[num].Replace(" ", ",");
                    list2.Add(str3);
                }
                return CommFunc.Join(list2, "|").Replace("*", "");
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
            if (num2 >= 5)
            {
                num2 -= 5;
            }
            list2 = new List<string>();
            int num3 = (playName == "猜冠军猜冠军") ? 1 : 5;
            for (num = 0; num < num3; num++)
            {
                str3 = "*";
                if (num == num2)
                {
                    str3 = CommFunc.Join(pNumberList, ",");
                }
                list2.Add(str3);
            }
            return CommFunc.Join(list2, "|").Replace("*", "");
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "010";
                }
                if (playName == "前三直选复式")
                {
                    return "009";
                }
                if (playName == "前三组三复式")
                {
                    return "018";
                }
                if (playName == "前三组六复式")
                {
                    return "017";
                }
                if (playName == "后三直选单式")
                {
                    return "014";
                }
                if (playName == "后三直选复式")
                {
                    return "013";
                }
                if (playName == "后三组三复式")
                {
                    return "022";
                }
                if (playName == "后三组六复式")
                {
                    return "021";
                }
                if (playName == "中三直选单式")
                {
                    return "012";
                }
                if (playName == "中三直选复式")
                {
                    return "011";
                }
                if (playName == "中三组三复式")
                {
                    return "020";
                }
                if (playName == "中三组六复式")
                {
                    return "019";
                }
                if (playName == "前二直选单式")
                {
                    return "037";
                }
                if (playName == "前二直选复式")
                {
                    return "036";
                }
                if (playName == "后二直选单式")
                {
                    return "039";
                }
                if (playName == "后二直选复式")
                {
                    return "038";
                }
                if (playName == "后四直选单式")
                {
                    return "006";
                }
                if (playName == "后四直选复式")
                {
                    return "005";
                }
                if (playName == "前四直选单式")
                {
                    return "004";
                }
                if (playName == "前四直选复式")
                {
                    return "003";
                }
                if (playName == "五星直选单式")
                {
                    return "002";
                }
                if (playName == "五星直选复式")
                {
                    return "001";
                }
                if (playName == "任三直选单式")
                {
                    return "016";
                }
                if (playName == "任三组三复式")
                {
                    return "024";
                }
                if (playName == "任三组六复式")
                {
                    return "023";
                }
                if (playName == "任二直选单式")
                {
                    return "041";
                }
                if (playName == "任四直选单式")
                {
                    return "008";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "050";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "";
                }
                if (playName == "前二直选单式")
                {
                    return "";
                }
                if (playName == "任选复式一中一")
                {
                    return "001";
                }
                if (playName == "任选复式二中二")
                {
                    return "002";
                }
                if (playName == "任选复式三中三")
                {
                    return "003";
                }
                if (playName == "任选复式四中四")
                {
                    return "004";
                }
                if (playName == "任选复式五中五")
                {
                    return "005";
                }
                if (playName == "任选单式一中一")
                {
                    return "";
                }
                if (playName == "任选单式二中二")
                {
                    return "";
                }
                if (playName == "任选单式三中三")
                {
                    return "";
                }
                if (playName == "任选单式四中四")
                {
                    return "";
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
                    return "005";
                }
                if (playName == "猜前三复式")
                {
                    return "004";
                }
                if (playName == "猜前二单式")
                {
                    return "003";
                }
                if (playName == "猜前二复式")
                {
                    return "002";
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
                    return "001";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "007" : "006";
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
            return CommFunc.GetIndexString(pResponseText, "\"msg\":\"", "\"", 0);
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            List<string> pList = new List<string>();
            for (int i = 0; i < pRXWZ.Count; i++)
            {
                string item = AppInfo.IndexDic[pRXWZ[i]];
                pList.Add(item);
            }
            return $"{CommFunc.Join(pList)}:";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pReferer = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                string pData = $"Model=Game&Action=GetTheIssueAndTime&Id={this.GetBetsLotteryID(pType)}";
                HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "POST", pData, pReferer, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"Then_Issue\":\"", "\"", 0);
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
                string str2 = $"/?Model=Images&Action=SetCheckCode&a=userlogin&rand={DateTime.Now.ToString()}";
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
                string pData = $"Action=CheckUserLogin&username={pID}&password={str5}&verification={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains(pID);
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"msg\":\"", "\"", 0);
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
            return (pResponsetext != "");
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            string pData = "Model=User&Action=SafetyExit";
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

