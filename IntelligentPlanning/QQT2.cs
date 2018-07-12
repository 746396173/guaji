namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class QQT2 : PTBase
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
                            int num2 = 2;
                            string format = "gameId={0}&isTrace=0&traceWinStop=1&traceStopValue=1&balls%5B0%5D%5BjsId%5D={1}&balls%5B0%5D%5BwayId%5D={2}&balls%5B0%5D%5Bball%5D={3}&balls%5B0%5D%5BviewBalls%5D=&balls%5B0%5D%5Bnum%5D={4}&balls%5B0%5D%5Btype%5D={5}&balls%5B0%5D%5BonePrice%5D={6}&balls%5B0%5D%5Bprize_group%5D={7}&balls%5B0%5D%5Bmoneyunit%5D={8}&balls%5B0%5D%5Bmultiple%5D={9}&orders%5B{10}%5D={11}&amount={12}&_token={13}&is_encoded=0&client_key={14}";
                            string prize = base.Prize;
                            prize = this.GetMaxPrize(prize, 1960.0);
                            string str8 = HttpUtility.UrlEncode("zjhtyb2016011217011000xte&ewr~yko!d");
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), "1", this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), num, this.GetPlayString(plan.Play), num2, prize, plan.Money / ((double) num2), Convert.ToInt32(plan.AutoTimes(str4, true)), this.GetBetsExpect(plan.CurrentExpect, ""), "1", plan.AutoTotalMoney(str4, true), base.Token, str8 });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, base.BetsTime2, "UTF-8", true);
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
            pHint.Contains("loginTimeout");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("success") || (pResponseText == "投注成功"));

        private bool CheckVerifyCode(string pVerifyCode)
        {
            if (pVerifyCode.Length != 5)
            {
                return false;
            }
            return true;
        }

        public override void CountPrizeDic(string pResponseText)
        {
        }

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"data\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/users/user-account-info");

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if ((pType == ConfigurationStatus.LotteryType.JX11X5) || (pType == ConfigurationStatus.LotteryType.SH11X5))
            {
                if (pIsBets)
                {
                    str = "20" + str;
                    if (pType == ConfigurationStatus.LotteryType.JX11X5)
                    {
                        str = str.Insert(8, "-");
                    }
                    return str;
                }
                return str.Replace("-", "").Substring(2);
            }
            if (((((pType != ConfigurationStatus.LotteryType.QQT2FC) && (pType != ConfigurationStatus.LotteryType.QQTTX2FC)) && ((pType != ConfigurationStatus.LotteryType.QQTJNDSSC) && (pType != ConfigurationStatus.LotteryType.QQTXDLSSC))) && (pType != ConfigurationStatus.LotteryType.QQTDJSSC)) && (pType != ConfigurationStatus.LotteryType.QQTHGSSC))
            {
                return str;
            }
            if (pIsBets)
            {
                return str.Insert(6, "0");
            }
            return str.Remove(6, 1);
        }

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string str = pExpect.Replace("-", "");
            return this.GetAppExpect(AppInfo.Current.Lottery.Type, str, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/bets/bet/{this.GetBetsLotteryID(pType)}";

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "34";
            }
            if (pType == ConfigurationStatus.LotteryType.QQTTX2FC)
            {
                return "35";
            }
            if (pType == ConfigurationStatus.LotteryType.QQTTG15C)
            {
                return "28";
            }
            if (pType == ConfigurationStatus.LotteryType.QQTNY15C)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.QQTFFC)
            {
                return "24";
            }
            if (pType == ConfigurationStatus.LotteryType.QQT2FC)
            {
                return "25";
            }
            if (pType == ConfigurationStatus.LotteryType.QQT5FC)
            {
                return "26";
            }
            if (pType == ConfigurationStatus.LotteryType.QQTXXLSSC)
            {
                return "38";
            }
            if (pType == ConfigurationStatus.LotteryType.QQTJNDSSC)
            {
                return "39";
            }
            if (pType == ConfigurationStatus.LotteryType.QQTXDLSSC)
            {
                return "40";
            }
            if (pType == ConfigurationStatus.LotteryType.QQTDJSSC)
            {
                return "42";
            }
            if (pType == ConfigurationStatus.LotteryType.QQTHGSSC)
            {
                return "41";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "9";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "27";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.QQTHGPK10)
            {
                str = "36";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/bets/game-info/{this.GetBetsLotteryID(pType)}?_={DateTime.Now.ToOADate()}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLineString(string pResponseText)
        {
            string str = pResponseText;
            if (str.Contains("http://guaji.qqt333.com"))
            {
                str = "线路1";
            }
            return str;
        }

        public override string GetLoginLine() => 
            (this.GetLine() + "/auth/signin");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/bets/bet/{this.GetBetsLotteryID(pType)}";

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
                    return CommFunc.Join(pNumberList, "|").Replace("*", "");
                }
                if (playName.Contains("定位胆"))
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
                    return CommFunc.Join(list, "|").Replace("*", "");
                }
                if (CommFunc.CheckPlayIsLH(playName))
                {
                    List<string> pList = new List<string>();
                    foreach (string str3 in pNumberList)
                    {
                        pList.Add((AppInfo.LHDic[str3] - 1).ToString());
                    }
                    return CommFunc.Join(pList);
                }
                if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        return CommFunc.Join(pNumberList);
                    }
                    return CommFunc.Join(pNumberList, "|");
                }
                return CommFunc.Join(pNumberList, "|");
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
                return CommFunc.Join(pNumberList, "|");
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
                return CommFunc.Join(pNumberList, "|").Replace("*", "");
            }
            pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
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
            return CommFunc.Join(list, "|").Replace("*", "");
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
                if (playName == "前三组三单式")
                {
                    return "2";
                }
                if (playName == "前三组六复式")
                {
                    return "17";
                }
                if (playName == "前三组六单式")
                {
                    return "3";
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
                if (playName == "后三组三单式")
                {
                    return "9";
                }
                if (playName == "后三组六复式")
                {
                    return "50";
                }
                if (playName == "后三组六单式")
                {
                    return "10";
                }
                if (playName == "中三直选单式")
                {
                    return "143";
                }
                if (playName == "中三直选复式")
                {
                    return "142";
                }
                if (playName == "中三组三复式")
                {
                    return "150";
                }
                if (playName == "中三组三单式")
                {
                    return "144";
                }
                if (playName == "中三组六复式")
                {
                    return "152";
                }
                if (playName == "中三组六单式")
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
                    return "78";
                }
                if (playName == "龙虎万千")
                {
                    return "220";
                }
                if (playName == "龙虎万百")
                {
                    return "221";
                }
                if (playName == "龙虎万十")
                {
                    return "222";
                }
                if (playName == "龙虎万个")
                {
                    return "223";
                }
                if (playName == "龙虎千百")
                {
                    return "224";
                }
                if (playName == "龙虎千十")
                {
                    return "225";
                }
                if (playName == "龙虎千个")
                {
                    return "226";
                }
                if (playName == "龙虎百十")
                {
                    return "227";
                }
                if (playName == "龙虎百个")
                {
                    return "228";
                }
                if (playName == "龙虎十个")
                {
                    str = "229";
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
                    return "278";
                }
                if (playName == "猜前三复式")
                {
                    return "260";
                }
                if (playName == "猜前二单式")
                {
                    return "";
                }
                if (playName == "猜前二复式")
                {
                    return "259";
                }
                if (playName == "猜前四单式")
                {
                    return "279";
                }
                if (playName == "猜前四复式")
                {
                    return "261";
                }
                if (playName == "猜前五单式")
                {
                    return "280";
                }
                if (playName == "猜前五复式")
                {
                    return "262";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "258";
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
                    return "caipaiwei.zhixuan.qiansandanshi";
                }
                if (playName == "猜前三复式")
                {
                    return "caipaiwei.zhixuan.qiansan";
                }
                if (playName == "猜前二单式")
                {
                    return "";
                }
                if (playName == "猜前二复式")
                {
                    return "caipaiwei.zhixuan.guanyajun";
                }
                if (playName == "猜前四单式")
                {
                    return "caipaiwei.zhixuan.qiansidanshi";
                }
                if (playName == "猜前四复式")
                {
                    return "caipaiwei.zhixuan.qiansi";
                }
                if (playName == "猜前五单式")
                {
                    return "caipaiwei.zhixuan.qianwudanshi";
                }
                if (playName == "猜前五复式")
                {
                    return "caipaiwei.zhixuan.qianwu";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "caichehao.dingweidan.dingweidan";
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
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"Msg\":\"", "\"", 0));
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/auth/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.Prize == "")
                {
                    string lotteryLine = this.GetLotteryLine(pType, false);
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    HttpHelper.GetResponse5(ref pResponsetext, lotteryLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        base.Prize = CommFunc.GetIndexString(pResponsetext, "\"user_prize_group\":\"", "\"", 0);
                        if (base.Prize == "")
                        {
                            base.Prize = CommFunc.GetIndexString(pResponsetext, "\"user_prize_group\":", ",", 0);
                        }
                        string str4 = CommFunc.GetIndexString(pResponsetext, "\"_token\":\"", "\"", 0);
                        if (str4 != "")
                        {
                            base.Token = str4;
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
                string str2 = $"/captcha?{DateTime.Now.ToOADate()}";
                string pUrl = this.GetLine() + str2;
                File.Delete(pVerifyCodeFile);
                Bitmap bitmap = new Bitmap(HttpHelper.GetResponseImage(pUrl, this.GetLoginLine(), "GET", "", 0x1770, "UTF-8", true));
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
                string jScript = HttpUtility.UrlEncode(CommFunc.WebMD51(CommFunc.WebMD51(CommFunc.WebMD51(pID + pW))));
                jScript = VerifyCodeAPI.GetJScript($"qqtjm('{jScript}')", base.PTID);
                string pData = $"_token={base.Token}&_random={base.Random}&username={pID}&password={jScript}&captcha={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("游戏记录");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "<div class=\"error \">", "</div>", 0).Replace("\r\n", "").Trim();
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
            bool flag = pResponsetext.Contains("全球通");
            if (flag)
            {
                base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("_token"));
                base.Random = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("_random"));
            }
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

