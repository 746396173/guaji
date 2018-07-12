namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class NBAYL : PTBase
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
                            string prize = this.GetPrize(plan.Type, plan.Play);
                            string format = "(\"IsChase\":false,\"Issue\":\"{0}\",\"IsAllIn\":false,\"LstOrder\":[(\"PlayType\":\"{1}\",\"Codes\":\"{2}\",\"IsZip\":\"false\",\"Model\":\"{3}\",\"MoneyUnit\":\"{4}\",\"Multiple\":\"{5}\")],\"LotteryType\":{6})";
                            format = string.Format(format, new object[] { this.GetBetsExpect(plan.CurrentExpect, ""), this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), "0.0", plan.Unit + 3, Convert.ToInt32(plan.AutoTimes(str4, true)), this.GetBetsLotteryID(plan.Type) }).Replace("(", "{").Replace(")", "}");
                            HttpHelper.GetResponse1(ref pResponsetext, betsLine, "POST", format, lotteryLine, base.BetsTime2, "UTF-8", true);
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
            (pResponseText.Contains("\"Msg\":\"购买成功！\"") || (pResponseText == "投注成功"));

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
                string str5 = CommFunc.GetIndexString(pResponsetext, "\"Balance\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str5);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/user/check");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, true, true, false);
            if (!CommFunc.CheckIsSkipLottery(iD))
            {
                str2 = str2.Substring(2);
            }
            return str2;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/lottery/PurchaseLottery");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "3";
            }
            if (pType == ConfigurationStatus.LotteryType.NBABJSSC)
            {
                return "27";
            }
            if (pType == ConfigurationStatus.LotteryType.NBATWSSC)
            {
                return "28";
            }
            if (pType == ConfigurationStatus.LotteryType.NBANY5FC)
            {
                return "50";
            }
            if (pType == ConfigurationStatus.LotteryType.NBANY3FC)
            {
                return "21";
            }
            if (pType == ConfigurationStatus.LotteryType.NBANY2FC)
            {
                return "20";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAQQFFC)
            {
                return "19";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAHGSSC)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.NBADJSSC)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAXDLSSC)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.NBANDJSSC)
            {
                return "17";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAFLP15C)
            {
                return "22";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAXXLSSC)
            {
                return "25";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "51";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "52";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "53";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "54";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "55";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAJN11X5)
            {
                return "56";
            }
            if (pType == ConfigurationStatus.LotteryType.NBA5F11X5)
            {
                return "57";
            }
            if (pType == ConfigurationStatus.LotteryType.NBA3F11X5)
            {
                return "58";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAFF11X5)
            {
                return "70";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "121";
            }
            if (pType == ConfigurationStatus.LotteryType.NBA60MPK10)
            {
                return "123";
            }
            if (pType == ConfigurationStatus.LotteryType.NBA180MPK10)
            {
                return "122";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAJZDPK10)
            {
                str = "134";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/lottery/LotteryOpenCode");

        public override string GetIndexLine() => 
            (this.GetLine() + "/portal/index");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login/index");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/lottery/{this.GetPTLotteryName(pType)}";

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
                    str = CommFunc.Join(pNumberList, ",").Replace("*", "");
                }
                else if (playName.Contains("定位胆"))
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
                    str = CommFunc.Join(list, ",").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, ",");
                        if (CommFunc.CheckPlayIsRXDS(playName))
                        {
                            str = str.Replace(",", "");
                        }
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
                    return CommFunc.Join(pNumberList, ",");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, ",");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    return CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                        str2 = CommFunc.Join(pNumberList, " ");
                    }
                    list.Add(str2);
                }
                str = CommFunc.Join(list, ",").Replace("*", "");
                if (playName == "猜冠军猜冠军")
                {
                    str = str.Replace(" ", ",");
                }
            }
            return str;
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "8";
                }
                if (playName == "前三直选复式")
                {
                    return "7";
                }
                if (playName == "前三组三复式")
                {
                    return "9";
                }
                if (playName == "前三组三单式")
                {
                    return "10";
                }
                if (playName == "前三组六复式")
                {
                    return "11";
                }
                if (playName == "前三组六单式")
                {
                    return "5";
                }
                if (playName == "后三直选单式")
                {
                    return "1";
                }
                if (playName == "后三直选复式")
                {
                    return "0";
                }
                if (playName == "后三组三复式")
                {
                    return "2";
                }
                if (playName == "后三组三单式")
                {
                    return "3";
                }
                if (playName == "后三组六复式")
                {
                    return "4";
                }
                if (playName == "后三组六单式")
                {
                    return "5";
                }
                if (playName == "中三直选单式")
                {
                    return "229";
                }
                if (playName == "中三直选复式")
                {
                    return "228";
                }
                if (playName == "中三组三复式")
                {
                    return "230";
                }
                if (playName == "中三组三单式")
                {
                    return "231";
                }
                if (playName == "中三组六复式")
                {
                    return "232";
                }
                if (playName == "中三组六单式")
                {
                    return "233";
                }
                if (playName == "前二直选单式")
                {
                    return "15";
                }
                if (playName == "前二直选复式")
                {
                    return "14";
                }
                if (playName == "后二直选单式")
                {
                    return "19";
                }
                if (playName == "后二直选复式")
                {
                    return "18";
                }
                if (playName == "后四直选单式")
                {
                    return "30";
                }
                if (playName == "后四直选复式")
                {
                    return "29";
                }
                if (playName == "前四直选单式")
                {
                    return "264";
                }
                if (playName == "前四直选复式")
                {
                    return "263";
                }
                if (playName == "五星直选复式")
                {
                    return "31";
                }
                if (playName == "五星直选单式")
                {
                    return "32";
                }
                if (playName == "任三直选单式")
                {
                    return "34";
                }
                if (playName == "任三直选复式")
                {
                    return "33";
                }
                if (playName == "任三组三复式")
                {
                    return "35";
                }
                if (playName == "任三组三单式")
                {
                    return "36";
                }
                if (playName == "任三组六复式")
                {
                    return "37";
                }
                if (playName == "任三组六单式")
                {
                    return "38";
                }
                if (playName == "任二直选单式")
                {
                    return "41";
                }
                if (playName == "任二直选复式")
                {
                    return "40";
                }
                if (playName == "任四直选单式")
                {
                    return "209";
                }
                if (playName == "任四直选复式")
                {
                    return "208";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "22";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "61";
                }
                if (playName == "前二直选单式")
                {
                    return "67";
                }
                if (playName == "任选复式一中一")
                {
                    return "75";
                }
                if (playName == "任选复式二中二")
                {
                    return "76";
                }
                if (playName == "任选复式三中三")
                {
                    return "77";
                }
                if (playName == "任选复式四中四")
                {
                    return "78";
                }
                if (playName == "任选复式五中五")
                {
                    return "79";
                }
                if (playName == "任选单式一中一")
                {
                    return "83";
                }
                if (playName == "任选单式二中二")
                {
                    return "84";
                }
                if (playName == "任选单式三中三")
                {
                    return "85";
                }
                if (playName == "任选单式四中四")
                {
                    return "86";
                }
                if (playName == "任选单式五中五")
                {
                    str = "87";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "253";
                }
                if (playName == "猜前三复式")
                {
                    return "252";
                }
                if (playName == "猜前二单式")
                {
                    return "247";
                }
                if (playName == "猜前二复式")
                {
                    return "246";
                }
                if (playName == "猜前四单式")
                {
                    return "259";
                }
                if (playName == "猜前四复式")
                {
                    return "258";
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
                    return "242";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "262";
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
            return CommFunc.GetIndexString(pResponseText, "\"Msg\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "ssccq";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "sscxj";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "ssctj";
            }
            if (pType == ConfigurationStatus.LotteryType.NBABJSSC)
            {
                return "sscbjf";
            }
            if (pType == ConfigurationStatus.LotteryType.NBATWSSC)
            {
                return "ssctwf";
            }
            if (pType == ConfigurationStatus.LotteryType.NBANY5FC)
            {
                return "sscny5";
            }
            if (pType == ConfigurationStatus.LotteryType.NBANY3FC)
            {
                return "sscny3";
            }
            if (pType == ConfigurationStatus.LotteryType.NBANY2FC)
            {
                return "sscny2";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAQQFFC)
            {
                return "sscny";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "ssctx";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAHGSSC)
            {
                return "sschg";
            }
            if (pType == ConfigurationStatus.LotteryType.NBADJSSC)
            {
                return "sscdj";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAXDLSSC)
            {
                return "sscxdl";
            }
            if (pType == ConfigurationStatus.LotteryType.NBANDJSSC)
            {
                return "sscxdj";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAFLP15C)
            {
                return "sscflb";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAXXLSSC)
            {
                return "sscxxl";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "syxwgd";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "syxwsd";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "syxwjx";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "syxwsh";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "syxwah";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAJN11X5)
            {
                return "syxwjn";
            }
            if (pType == ConfigurationStatus.LotteryType.NBA5F11X5)
            {
                return "syxw5f";
            }
            if (pType == ConfigurationStatus.LotteryType.NBA3F11X5)
            {
                return "syxw3f";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAFF11X5)
            {
                return "syxw1f";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "pk10bj";
            }
            if (pType == ConfigurationStatus.LotteryType.NBA60MPK10)
            {
                return "pk10yg60";
            }
            if (pType == ConfigurationStatus.LotteryType.NBA180MPK10)
            {
                return "pk10yg180";
            }
            if (pType == ConfigurationStatus.LotteryType.NBAJZDPK10)
            {
                str = "pk10jzd";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/login/loginout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            List<int> pList = new List<int>();
            for (int i = 0; i < pRXWZ.Count; i++)
            {
                pList.Add(5 - pRXWZ[i]);
            }
            return $"{CommFunc.Join<int>(pList)}-";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.Prize == "")
                {
                    string pUrl = $"{this.GetLine()}/lottery/getmaxinvestmodel";
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                    base.Prize = pResponsetext;
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
                string str2 = $"/login/GetCheckCode?timestamp={DateTime.Now.ToOADate()}";
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
                string pUrl = this.GetLine() + "/login/login";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"username={pID}&password={str5}&checkCode={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("\"Msg\":\"登陆成功！\"");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"Msg\":\"", "\"", 0);
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
            return pResponsetext.Contains("用户登录");
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

