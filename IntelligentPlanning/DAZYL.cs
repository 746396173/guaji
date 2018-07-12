namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web;

    public class DAZYL : PTBase
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
                            string format = "gameId={0}&isTrace=0&traceWinStop=1&traceStopValue=1&orders%5B{1}%5D=1&amount={2}&ballstr=%5B%7B%22jsId%22%3A{3}%2C%22wayId%22%3A{4}%2C%22ball%22%3A%22{5}%22%2C%22viewBalls%22%3A%22%22%2C%22num%22%3A{6}%2C%22type%22%3A%22{7}%22%2C%22onePrice%22%3A%22{8}%22%2C%22moneyunit%22%3A%22{9}%22%2C%22multiple%22%3A{10}%2C%22prizeGroup%22%3A%22{11}%22%7D%5D&orderstr=%7B%22{1}%22%3A{12}%7D";
                            int num = plan.FNNumber(str4);
                            int num2 = 2;
                            string prize = base.Prize;
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), plan.AutoTotalMoney(str4, true), "1", this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), num, this.GetPlayString(plan.Play), num2, plan.Money / ((double) num2), Convert.ToInt32(plan.AutoTimes(str4, true)), prize, "1" });
                            HttpHelper.GetResponse2(ref pResponsetext, betsLine, "POST", format, lotteryLine, base.BetsTime3, "UTF-8", true);
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
            (pResponseText.Contains("\"isSuccess\" : 1") || (pResponseText == "投注成功"));

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse2(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"available\" : \"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/hz/mkg/api/users/user-monetary-info?_={DateTime.Now.ToOADate()}";

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if (pType == ConfigurationStatus.LotteryType.DAZTG15C)
            {
                if (pIsBets)
                {
                    return str.Insert(9, "0");
                }
                return str.Remove(9, 1);
            }
            if ((((((pType == ConfigurationStatus.LotteryType.GD11X5) || (pType == ConfigurationStatus.LotteryType.JX11X5)) || ((pType == ConfigurationStatus.LotteryType.SH11X5) || (pType == ConfigurationStatus.LotteryType.LN11X5))) || (pType == ConfigurationStatus.LotteryType.SD11X5)) || (pType == ConfigurationStatus.LotteryType.JS11X5)) && pIsBets)
            {
                str = str.Replace("-0", "-");
            }
            return str;
        }

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            return this.GetAppExpect(AppInfo.Current.Lottery.Type, str2, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/hz/mkg/api/bets/bet");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "29";
            }
            if (pType == ConfigurationStatus.LotteryType.DAZRBSSC)
            {
                return "23";
            }
            if (pType == ConfigurationStatus.LotteryType.DAZFFC)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.DAZ5FC)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.DAZTG15C)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.DAZDJSSC)
            {
                return "36";
            }
            if (pType == ConfigurationStatus.LotteryType.DAZJDSSC)
            {
                return "41";
            }
            if (pType == ConfigurationStatus.LotteryType.DAZHGSSC)
            {
                return "42";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "44";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "9";
            }
            if (pType == ConfigurationStatus.LotteryType.LN11X5)
            {
                return "19";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "20";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.JS11X5)
            {
                return "43";
            }
            if (pType == ConfigurationStatus.LotteryType.DAZ11X5)
            {
                return "43";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "32";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/hz/mkg/api/load-issue/{this.GetBetsLotteryID(pType)}?_={DateTime.Now.ToOADate()}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/hz/home");

        public override string GetLoginLine() => 
            (this.GetLine() + "/hz/auth/signin.html");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/hz/mkg/bet/{this.GetBetsLotteryID(pType)}.htm";

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
                    str = CommFunc.Join(pNumberList, "|").Replace("*", "");
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
                    str = CommFunc.Join(list, "|").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList);
                    }
                    else
                    {
                        str = CommFunc.Join(pNumberList, "|");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "|");
                }
                return HttpUtility.UrlEncode(str);
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, "|");
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
                str = CommFunc.Join(pNumberList, "|");
            }
            else if (CommFunc.CheckPlayIsFS(playName))
            {
                str = CommFunc.Join(pNumberList, "|").Replace("*", "");
            }
            else
            {
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
                str = CommFunc.Join(list, "|").Replace("*", "");
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
                    return "1";
                }
                if (playName == "前三直选复式")
                {
                    return "65";
                }
                if (playName == "前三组三复式")
                {
                    return "16";
                }
                if (playName == "前三组六复式")
                {
                    return "17";
                }
                if (playName == "后三直选单式")
                {
                    return "8";
                }
                if (playName == "后三直选复式")
                {
                    return "69";
                }
                if (playName == "后三组三复式")
                {
                    return "49";
                }
                if (playName == "后三组六复式")
                {
                    return "50";
                }
                if (playName == "中三直选单式")
                {
                    return "142";
                }
                if (playName == "中三直选复式")
                {
                    return "150";
                }
                if (playName == "中三组三复式")
                {
                    return "145";
                }
                if (playName == "中三组六复式")
                {
                    return "146";
                }
                if (playName == "前二直选单式")
                {
                    return "4";
                }
                if (playName == "前二直选复式")
                {
                    return "66";
                }
                if (playName == "后二直选单式")
                {
                    return "11";
                }
                if (playName == "后二直选复式")
                {
                    return "70";
                }
                if (playName == "后四直选单式")
                {
                    return "6";
                }
                if (playName == "后四直选复式")
                {
                    return "67";
                }
                if (playName == "五星直选单式")
                {
                    return "7";
                }
                if (playName == "五星直选复式")
                {
                    return "68";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "78";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "95";
                }
                if (playName == "前二直选单式")
                {
                    return "94";
                }
                if (playName == "任选复式一中一")
                {
                    return "98";
                }
                if (playName == "任选复式二中二")
                {
                    return "99";
                }
                if (playName == "任选复式三中三")
                {
                    return "100";
                }
                if (playName == "任选复式四中四")
                {
                    return "101";
                }
                if (playName == "任选复式五中五")
                {
                    return "102";
                }
                if (playName == "任选单式一中一")
                {
                    return "86";
                }
                if (playName == "任选单式二中二")
                {
                    return "87";
                }
                if (playName == "任选单式三中三")
                {
                    return "88";
                }
                if (playName == "任选单式四中四")
                {
                    return "89";
                }
                if (playName == "任选单式五中五")
                {
                    str = "90";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "523";
                }
                if (playName == "猜前三复式")
                {
                    return "522";
                }
                if (playName == "猜前二单式")
                {
                    return "513";
                }
                if (playName == "猜前二复式")
                {
                    return "512";
                }
                if (playName == "猜前四单式")
                {
                    return "533";
                }
                if (playName == "猜前四复式")
                {
                    return "532";
                }
                if (playName == "猜前五单式")
                {
                    return "543";
                }
                if (playName == "猜前五复式")
                {
                    return "542";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "502";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "553" : "552";
                }
            }
            return str;
        }

        public override string GetPlayString(string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "qiansan.zhixuan.danshi";
                }
                if (playName == "前三直选复式")
                {
                    return "qiansan.zhixuan.fushi";
                }
                if (playName == "前三组三复式")
                {
                    return "qiansan.zuxuan.zusan";
                }
                if (playName == "前三组三单式")
                {
                    return "qiansan.zuxuan.zusandanshi";
                }
                if (playName == "前三组六复式")
                {
                    return "qiansan.zuxuan.zuliu";
                }
                if (playName == "前三组六单式")
                {
                    return "qiansan.zuxuan.zuliudanshi";
                }
                if (playName == "后三直选单式")
                {
                    return "housan.zhixuan.danshi";
                }
                if (playName == "后三直选复式")
                {
                    return "housan.zhixuan.fushi";
                }
                if (playName == "后三组三复式")
                {
                    return "housan.zuxuan.zusan";
                }
                if (playName == "后三组三单式")
                {
                    return "housan.zuxuan.zusandanshi";
                }
                if (playName == "后三组六复式")
                {
                    return "housan.zuxuan.zuliu";
                }
                if (playName == "后三组六单式")
                {
                    return "housan.zuxuan.zuliudanshi";
                }
                if (playName == "中三直选单式")
                {
                    return "zhongsan.zhixuan.danshi";
                }
                if (playName == "中三直选复式")
                {
                    return "zhongsan.zhixuan.fushi";
                }
                if (playName == "中三组三复式")
                {
                    return "zhongsan.zuxuan.zusan";
                }
                if (playName == "中三组三单式")
                {
                    return "zhongsan.zuxuan.zusandanshi";
                }
                if (playName == "中三组六复式")
                {
                    return "zhongsan.zuxuan.zuliu";
                }
                if (playName == "中三组六单式")
                {
                    return "zhongsan.zuxuan.zuliudanshi";
                }
                if (playName == "前二直选单式")
                {
                    return "erxing.zhixuan.qianerdanshi";
                }
                if (playName == "前二直选复式")
                {
                    return "erxing.zhixuan.qianerfushi";
                }
                if (playName == "后二直选单式")
                {
                    return "erxing.zhixuan.houerdanshi";
                }
                if (playName == "后二直选复式")
                {
                    return "erxing.zhixuan.houerfushi";
                }
                if (playName == "后四直选单式")
                {
                    return "sixing.zhixuan.danshi";
                }
                if (playName == "后四直选复式")
                {
                    return "sixing.zhixuan.fushi";
                }
                if (playName == "五星直选单式")
                {
                    return "wuxing.zhixuan.danshi";
                }
                if (playName == "五星直选复式")
                {
                    return "wuxing.zhixuan.fushi";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "yixing.dingweidan.fushi";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "sanma.zhixuan.danshi";
                }
                if (playName == "前二直选单式")
                {
                    return "erma.zhixuan.danshi";
                }
                if (playName == "任选复式一中一")
                {
                    return "renxuanfushi.renxuanfushi.renxuanyi";
                }
                if (playName == "任选复式二中二")
                {
                    return "renxuanfushi.renxuanfushi.renxuaner";
                }
                if (playName == "任选复式三中三")
                {
                    return "renxuanfushi.renxuanfushi.renxuansan";
                }
                if (playName == "任选复式四中四")
                {
                    return "renxuanfushi.renxuanfushi.renxuansi";
                }
                if (playName == "任选复式五中五")
                {
                    return "renxuanfushi.renxuanfushi.renxuanwu";
                }
                if (playName == "任选单式一中一")
                {
                    return "renxuandanshi.renxuandanshi.renxuanyi";
                }
                if (playName == "任选单式二中二")
                {
                    return "renxuandanshi.renxuandanshi.renxuaner";
                }
                if (playName == "任选单式三中三")
                {
                    return "renxuandanshi.renxuandanshi.renxuansan";
                }
                if (playName == "任选单式四中四")
                {
                    return "renxuandanshi.renxuandanshi.renxuansi";
                }
                if (playName == "任选单式五中五")
                {
                    str = "renxuandanshi.renxuandanshi.renxuanwu";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "xuansan.zhixuan.danshi";
                }
                if (playName == "猜前三复式")
                {
                    return "xuansan.zhixuan.fushi";
                }
                if (playName == "猜前二单式")
                {
                    return "xuaner.zhixuan.danshi";
                }
                if (playName == "猜前二复式")
                {
                    return "xuaner.zhixuan.fushi";
                }
                if (playName == "猜前四单式")
                {
                    return "xuansi.zhixuan.danshi";
                }
                if (playName == "猜前四复式")
                {
                    return "xuansi.zhixuan.fushi";
                }
                if (playName == "猜前五单式")
                {
                    return "xuanwu.zhixuan.danshi";
                }
                if (playName == "猜前五复式")
                {
                    return "xuanwu.zhixuan.fushi";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "xuanyi.zhixuan.fushi";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "dingweidan.dingweidan.houwudingweidan" : "dingweidan.dingweidan.dingweidan";
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
            return CommFunc.GetIndexString(pResponseText, "\"tplData\" : \"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/sso/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            if (base.Prize == "")
            {
                string pStr = this.LoginLotteryWeb(pType, "");
                if (pStr != "")
                {
                    base.Prize = CommFunc.GetIndexString(pStr, "\"prize_group\":\"", "\"", 0);
                }
            }
        }

        public bool InputWeb(string pID, string pW, ref string pHint)
        {
            bool flag = false;
            string loginLine = this.GetLoginLine();
            string str2 = CommFunc.WebMD51(pW);
            string pUrl = $"{this.GetLine()}/sso/login?way=pwd&from=portal&cn={pID}&appId=5&password={str2}&capchaCode=&_={DateTime.Now.ToOADate()}";
            string pResponsetext = "";
            HttpHelper.GetResponse2(ref pResponsetext, pUrl, "GET", string.Empty, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("\"msg\":\"登录成功\"");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "\"msg\":\"", "\"", 0);
            }
            return flag;
        }

        public override string LoginLotteryWeb(ConfigurationStatus.LotteryType pType, string pInfo = "")
        {
            string lotteryLine = this.GetLotteryLine(pType, false);
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            HttpHelper.GetResponse2(ref pResponsetext, lotteryLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
            return pResponsetext;
        }

        public bool LoginWeb() => 
            true;

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

