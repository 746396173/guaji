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

    public class WTYL : PTBase
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
                            string format = "\"lotteryId\":{0},\"codes\":\"{1}\",\"num\":{2},\"ruleId\":{3},\"ruleCode\":\"{4}\",\"multiple\":{5},\"model\":\"1{6}\",\"code\":{7}";
                            string prize = base.Prize;
                            string[] strArray = Strings.Split(this.GetPlayMethodID(plan.Type, plan.Play), "-", -1, CompareMethod.Binary);
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), num, strArray[0], strArray[1], Convert.ToInt32(plan.AutoTimes(str4, true)), plan.UnitString, prize, 1 });
                            format = "blist=" + HttpUtility.UrlEncode("[{" + format + "}]");
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, pReferer, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
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
            (pResponseText.Contains("\"error\":0") || (pResponseText == "投注成功"));

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
                if (!AppInfo.PTInfo.PTIsBreak)
                {
                    string accountsMemLine = this.GetAccountsMemLine(pType);
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    string pData = "";
                    HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                    if (this.CheckBreakConnect(pResponsetext))
                    {
                        AppInfo.PTInfo.PTIsBreak = true;
                    }
                    else
                    {
                        pResponsetext = pResponsetext.Substring(pResponsetext.IndexOf("username"));
                        base.Prize = CommFunc.GetIndexString(pResponsetext, "\"code\":", ",", 0);
                        base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"lotteryMoney\":", ",", 0);
                        AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
                    }
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/GetGlobal");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/UserBetsGeneral");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "101";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "122";
            }
            if (pType == ConfigurationStatus.LotteryType.WTYLTXFFC)
            {
                return "118";
            }
            if (pType == ConfigurationStatus.LotteryType.WTYLXXL15C)
            {
                return "119";
            }
            if (pType == ConfigurationStatus.LotteryType.WTYLHG15C)
            {
                return "125";
            }
            if (pType == ConfigurationStatus.LotteryType.WTYLNY2FC)
            {
                return "120";
            }
            if (pType == ConfigurationStatus.LotteryType.WTYLJNDSSC)
            {
                return "126";
            }
            if (pType == ConfigurationStatus.LotteryType.WTYLXDL35C)
            {
                return "124";
            }
            if (pType == ConfigurationStatus.LotteryType.WTYLAZ5FC)
            {
                return "121";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "601";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/LotteryOpenCode");

        public override string GetIndexLine() => 
            (this.GetLine() + "/game-center");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login");

        public bool GetLoginToken()
        {
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLine() + "/DisposableToken";
            string pResponsetext = "";
            string pData = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("\"message\":\"请求操作成功！\"");
            base.Token = CommFunc.GetIndexString(pResponsetext, "\"token\":\"", "\"", 0);
            return flag;
        }

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/lottery-{this.GetPTLotteryName(pType)}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            int num;
            int num2;
            string str2;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(playName);
                    list = new List<string>();
                    num = 0;
                    for (num2 = 0; num2 < 5; num2++)
                    {
                        str2 = "*";
                        if (playInfo.IndexList.Contains(num2 + 1))
                        {
                            str2 = pNumberList[num++];
                        }
                        list.Add(str2);
                    }
                    str = CommFunc.Join(list, ",").Replace("*", "-");
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
                    str = CommFunc.Join(list, ",").Replace("*", "-");
                }
                else if (CommFunc.CheckPlayIsLH(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, ",");
                    }
                }
                else if (CommFunc.CheckPlayIsRX(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else
                {
                    str = CommFunc.Join(pNumberList, " ");
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
                    return CommFunc.Join(pNumberList, ";");
                }
                return CommFunc.Join(pNumberList, ",");
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, ";");
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
                if (num >= 5)
                {
                    num -= 5;
                }
                list = new List<string>();
                int num3 = (playName == "猜冠军猜冠军") ? 1 : 5;
                for (num2 = 0; num2 < num3; num2++)
                {
                    str2 = "*";
                    if (num2 == num)
                    {
                        str2 = CommFunc.Join(pNumberList, " ");
                    }
                    list.Add(str2);
                }
                str = CommFunc.Join(list, ",").Replace("*", "-");
                if (playName == "猜冠军猜冠军")
                {
                    str = str.Replace(" ", ",");
                }
            }
            return str;
        }

        public override string GetOpenTime() => 
            (this.GetLine() + "/LotteryOpenTime");

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "37-sxzhixdsq";
                }
                if (playName == "前三直选复式")
                {
                    return "36-sxzhixfsq";
                }
                if (playName == "前三组三复式")
                {
                    return "39-sxzuxzsq";
                }
                if (playName == "前三组六复式")
                {
                    return "40-sxzuxzlq";
                }
                if (playName == "后三直选单式")
                {
                    return "25-sxzhixdsh";
                }
                if (playName == "后三直选复式")
                {
                    return "24-sxzhixfsh";
                }
                if (playName == "后三组三复式")
                {
                    return "27-sxzuxzsh";
                }
                if (playName == "后三组六复式")
                {
                    return "28-sxzuxzlh";
                }
                if (playName == "中三直选单式")
                {
                    return "31-sxzhixdsz";
                }
                if (playName == "中三直选复式")
                {
                    return "30-sxzhixfsz";
                }
                if (playName == "中三组三复式")
                {
                    return "33-sxzuxzsz";
                }
                if (playName == "中三组六复式")
                {
                    return "34-sxzuxzlz";
                }
                if (playName == "前二直选单式")
                {
                    return "49-exzhixdsq";
                }
                if (playName == "前二直选复式")
                {
                    return "48-exzhixfsq";
                }
                if (playName == "后二直选单式")
                {
                    return "43-exzhixdsh";
                }
                if (playName == "后二直选复式")
                {
                    return "42-exzhixfsh";
                }
                if (playName == "前四直选单式")
                {
                    return "18-sixzhixdsq";
                }
                if (playName == "前四直选复式")
                {
                    return "17-sixzhixfsq";
                }
                if (playName == "后四直选单式")
                {
                    return "11-sixzhixdsh";
                }
                if (playName == "后四直选复式")
                {
                    return "10-sixzhixfsh";
                }
                if (playName == "五星直选单式")
                {
                    return "2-wxzhixds";
                }
                if (playName == "五星直选复式")
                {
                    return "1-wxzhixfs";
                }
                if (playName == "任三直选单式")
                {
                    return "65-rx3ds";
                }
                if (playName == "任三直选复式")
                {
                    return "62-rx3fs";
                }
                if (playName == "任三组三复式")
                {
                    return "67-rx3z3";
                }
                if (playName == "任三组六复式")
                {
                    return "68-rx3z6";
                }
                if (playName == "任二直选单式")
                {
                    return "64-rx2ds";
                }
                if (playName == "任二直选复式")
                {
                    return "61-rx2fs";
                }
                if (playName == "任四直选单式")
                {
                    return "66-rx4ds";
                }
                if (playName == "任四直选复式")
                {
                    return "63-rx4fs";
                }
                if (playName.Contains("定位胆"))
                {
                    return "54-dw";
                }
                if (playName == "龙虎万千")
                {
                    return "168-longhuhewq";
                }
                if (playName == "龙虎万百")
                {
                    return "169-longhuhewb";
                }
                if (playName == "龙虎万十")
                {
                    return "170-longhuhews";
                }
                if (playName == "龙虎万个")
                {
                    return "171-longhuhewg";
                }
                if (playName == "龙虎千百")
                {
                    return "172-longhuheqb";
                }
                if (playName == "龙虎千十")
                {
                    return "173-longhuheqs";
                }
                if (playName == "龙虎千个")
                {
                    return "174-longhuheqg";
                }
                if (playName == "龙虎百十")
                {
                    return "175-longhuhebs";
                }
                if (playName == "龙虎百个")
                {
                    return "176-longhuhebg";
                }
                if (playName == "龙虎十个")
                {
                    str = "177-longhuhesg";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "sanmzhixdsq";
                }
                if (playName == "前二直选单式")
                {
                    return "ermzhixdsq";
                }
                if (playName == "任选复式一中一")
                {
                    return "rx1fs";
                }
                if (playName == "任选复式二中二")
                {
                    return "rx2fs";
                }
                if (playName == "任选复式三中三")
                {
                    return "rx3fs";
                }
                if (playName == "任选复式四中四")
                {
                    return "rx4fs";
                }
                if (playName == "任选复式五中五")
                {
                    return "rx5fs";
                }
                if (playName == "任选单式一中一")
                {
                    return "rx1ds";
                }
                if (playName == "任选单式二中二")
                {
                    return "rx2ds";
                }
                if (playName == "任选单式三中三")
                {
                    return "rx3ds";
                }
                if (playName == "任选单式四中四")
                {
                    return "rx4ds";
                }
                if (playName == "任选单式五中五")
                {
                    str = "rx5ds";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "149-qiansanzxds";
                }
                if (playName == "猜前三复式")
                {
                    return "148-qiansanzxfs";
                }
                if (playName == "猜前二单式")
                {
                    return "147-qianerzxds";
                }
                if (playName == "猜前二复式")
                {
                    return "146-qianerzxfs";
                }
                if (playName == "猜前四单式")
                {
                    return "153-qiansizxds";
                }
                if (playName == "猜前四复式")
                {
                    return "152-qiansizxfs";
                }
                if (playName == "猜前五单式")
                {
                    return "155-qianwuzxds";
                }
                if (playName == "猜前五复式")
                {
                    return "154-qianwuzxfs";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "145-qianyi";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "157-hwdingweidan" : "156-qwdingweidan";
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
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                str = "cqssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                str = "txffc";
            }
            else if (pType == ConfigurationStatus.LotteryType.WTYLTXFFC)
            {
                str = "fgffc";
            }
            else if (pType == ConfigurationStatus.LotteryType.WTYLXXL15C)
            {
                str = "xxl1d5fc";
            }
            else if (pType == ConfigurationStatus.LotteryType.WTYLHG15C)
            {
                str = "hg1d5fc";
            }
            else if (pType == ConfigurationStatus.LotteryType.WTYLNY2FC)
            {
                str = "ny2fc";
            }
            else if (pType == ConfigurationStatus.LotteryType.WTYLJNDSSC)
            {
                str = "jnd3d5fc";
            }
            else if (pType == ConfigurationStatus.LotteryType.WTYLXDL35C)
            {
                str = "xdl3d5fc";
            }
            else if (pType == ConfigurationStatus.LotteryType.WTYLAZ5FC)
            {
                str = "az5fc";
            }
            else if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "bjpk10";
            }
            return str.ToLower();
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            List<string> pList = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string item = pRXWZ.Contains(i) ? "√" : "-";
                pList.Add(item);
            }
            return $"[{CommFunc.Join(pList, ",")}]";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string openTime = this.GetOpenTime();
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                string pData = $"lotteryId={this.GetBetsLotteryID(pType)}";
                HttpHelper.GetResponse(ref pResponsetext, openTime, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"expect\":\"", "\"", 0);
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
                string str2 = $"/LoginCode?t={DateTime.Now.ToOADate()}";
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
                if (!this.GetLoginToken())
                {
                    return flag;
                }
                string loginLine = this.GetLoginLine();
                string pUrl = this.GetLine() + "/AppLogin";
                string pResponsetext = "";
                string str5 = CommFunc.WebMD53(CommFunc.WebMD53(CommFunc.WebMD53(CommFunc.WebMD53(pW))) + base.Token);
                string pData = $"username={pID}&password={str5}&checkcode={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("\"message\":\"请求操作成功！\"");
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

