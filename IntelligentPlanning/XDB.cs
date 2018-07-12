namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web;

    public class XDB : PTBase
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
                            string play = plan.Play;
                            ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(play);
                            int num = plan.FNNumber(str4);
                            int num2 = 2;
                            string prize = base.Prize;
                            string str8 = CommFunc.CheckPlayIsRX(play) ? playInfo.IndexList.Count.ToString() : "";
                            string format = "balls%5B0%5D%5BjsId%5D={0}&balls%5B0%5D%5BwayId%5D={1}&balls%5B0%5D%5Bball%5D={2}&balls%5B0%5D%5BviewBalls%5D=&balls%5B0%5D%5Bnum%5D={3}&balls%5B0%5D%5Btype%5D={4}&balls%5B0%5D%5BonePrice%5D={5}&balls%5B0%5D%5BprizeGroup%5D={6}&balls%5B0%5D%5Bmoneyunit%5D={7}&balls%5B0%5D%5Bmultiple%5D={8}&balls%5B0%5D%5Bextra%5D%5Bposition%5D={9}&balls%5B0%5D%5Bextra%5D%5Bseat%5D={10}";
                            format = string.Format(format, new object[] { "1", this.GetPlayMethodID(plan.Type, plan.Play), HttpUtility.UrlEncode(this.GetNumberList1(pTNumberList, plan.Play, null)), num, this.GetPlayString(plan.Play), num2, prize, plan.Money / ((double) num2), Convert.ToInt32(plan.AutoTimes(str4, true)), this.GetRXWZString1(plan.RXWZ, play, pTNumberList), str8 });
                            string str10 = "gameId={0}&isTrace=0&traceWinStop=1&traceStopValue=1&{1}&orders%5B{2}%5D={3}&amount={4}";
                            str10 = string.Format(str10, new object[] { this.GetBetsLotteryID(plan.Type), format, this.GetBetsExpect(plan.CurrentExpect, ""), "1", plan.AutoTotalMoney(str4, true) });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str10, lotteryLine, base.BetsTime3, "UTF-8", true);
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
            (pResponseText.Contains("\"isSuccess\":1") || (pResponseText == "投注成功"));

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"available\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/users/user-monetary-info");

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                if (pIsBets)
                {
                    str = str.Replace("-", "");
                }
                return str;
            }
            if ((((pType == ConfigurationStatus.LotteryType.K3HGSSC) || (pType == ConfigurationStatus.LotteryType.K3DJSSC)) || (pType == ConfigurationStatus.LotteryType.K3TXFFC)) || (pType == ConfigurationStatus.LotteryType.K3NTXFFC))
            {
                if (pIsBets)
                {
                    return ("20" + str.Replace("-", ""));
                }
                return str.Substring(2);
            }
            if ((pType == ConfigurationStatus.LotteryType.K3HG2FC) || (pType == ConfigurationStatus.LotteryType.K3MG5FC))
            {
                if (pIsBets)
                {
                    return str.Insert(6, "0").Replace("-", "");
                }
                return str.Remove(6, 1);
            }
            if (pIsBets)
            {
                str = str.Replace("-", "");
            }
            return str;
        }

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return this.GetAppExpect(AppInfo.Current.Lottery.Type, pExpect, true);
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
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "47";
            }
            if (pType == ConfigurationStatus.LotteryType.XDBFFC)
            {
                return "13";
            }
            if (pType == ConfigurationStatus.LotteryType.XDBDJSSC)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.XDBHGSSC)
            {
                return "33";
            }
            if (pType == ConfigurationStatus.LotteryType.XDB2FC)
            {
                return "32";
            }
            if (pType == ConfigurationStatus.LotteryType.XDB3FC)
            {
                return "17";
            }
            if (pType == ConfigurationStatus.LotteryType.XDB5FC)
            {
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "9";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "53";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.XDBFF11X5)
            {
                return "14";
            }
            if (pType == ConfigurationStatus.LotteryType.XDB2F11X5)
            {
                return "34";
            }
            if (pType == ConfigurationStatus.LotteryType.XDB3F11X5)
            {
                return "35";
            }
            if (pType == ConfigurationStatus.LotteryType.XDB5F11X5)
            {
                return "36";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "10";
            }
            if (pType == ConfigurationStatus.LotteryType.XDBFFPK10)
            {
                return "41";
            }
            if (pType == ConfigurationStatus.LotteryType.XDB2FPK10)
            {
                return "42";
            }
            if (pType == ConfigurationStatus.LotteryType.XDB3FPK10)
            {
                return "43";
            }
            if (pType == ConfigurationStatus.LotteryType.XDB5FPK10)
            {
                str = "44";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/buy/load-numbers/{this.GetBetsLotteryID(pType)}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/auth/signin");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/buy/bet/{this.GetBetsLotteryID(pType)}";

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
            pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
            if (CommFunc.CheckPlayIsDS(playName))
            {
                return CommFunc.Join(pNumberList, "|");
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                return CommFunc.Join(pNumberList, "|").Replace("*", "");
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
                if (playName == "中三组三单式")
                {
                    return "143";
                }
                if (playName == "中三组六复式")
                {
                    return "146";
                }
                if (playName == "中三组六单式")
                {
                    return "144";
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
                if (playName == "任三直选单式")
                {
                    return "186";
                }
                if (playName == "任三直选复式")
                {
                    return "179";
                }
                if (playName == "任三组三复式")
                {
                    return "181";
                }
                if (playName == "任三组三单式")
                {
                    return "188";
                }
                if (playName == "任三组六复式")
                {
                    return "182";
                }
                if (playName == "任三组六单式")
                {
                    return "189";
                }
                if (playName == "任二直选单式")
                {
                    return "200";
                }
                if (playName == "任二直选复式")
                {
                    return "199";
                }
                if (playName == "任四直选单式")
                {
                    return "187";
                }
                if (playName == "任四直选复式")
                {
                    return "180";
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
                    return "";
                }
                if (playName == "猜前三复式")
                {
                    return "173";
                }
                if (playName == "猜前二单式")
                {
                    return "";
                }
                if (playName == "猜前二复式")
                {
                    return "172";
                }
                if (playName == "猜前四单式")
                {
                    return "";
                }
                if (playName == "猜前四复式")
                {
                    return "394";
                }
                if (playName == "猜前五单式")
                {
                    return "";
                }
                if (playName == "猜前五复式")
                {
                    return "395";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "177";
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
                if (playName == "任三直选单式")
                {
                    return "renxuan.renxuan3.zhixuandanshi";
                }
                if (playName == "任三直选复式")
                {
                    return "renxuan.renxuan3.zhixuanfushi";
                }
                if (playName == "任三组三复式")
                {
                    return "enxuan.renxuan3.zu3fushi";
                }
                if (playName == "任三组三单式")
                {
                    return "188-renxuan.renxuan3.zu3danshi";
                }
                if (playName == "任三组六复式")
                {
                    return "renxuan.renxuan3.zu6fushi";
                }
                if (playName == "任三组六单式")
                {
                    return "renxuan.renxuan3.zu6danshi";
                }
                if (playName == "任二直选单式")
                {
                    return "renxuan.renxuan2.zhixuandanshi";
                }
                if (playName == "任二直选复式")
                {
                    return "renxuan.renxuan2.zhixuanfushi";
                }
                if (playName == "任四直选单式")
                {
                    return "renxuan.renxuan4.danshi";
                }
                if (playName == "任四直选复式")
                {
                    return "renxuan.renxuan4.fushi";
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
                    return "";
                }
                if (playName == "猜前三复式")
                {
                    return "cmc.cmc.jqqiansan";
                }
                if (playName == "猜前二单式")
                {
                    return "";
                }
                if (playName == "猜前二复式")
                {
                    return "cmc.cmc.jqguanya";
                }
                if (playName == "猜前四单式")
                {
                    return "";
                }
                if (playName == "猜前四复式")
                {
                    return "cmc.cmc.jqqiansi";
                }
                if (playName == "猜前五单式")
                {
                    return "";
                }
                if (playName == "猜前五复式")
                {
                    return "cmc.cmc.jqqianwu";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "cmc.cmc.cmc";
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
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"Msg\":\"", "\"", 0).Replace("-", ""));
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/auth/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString1(List<int> pRXWZ, string playName, List<string> pBetsNumberList)
        {
            string str = "";
            if (CommFunc.CheckPlayIsRXDS(playName))
            {
                return CommFunc.Join<int>(pRXWZ);
            }
            if (!CommFunc.CheckPlayIsRXFS(playName))
            {
                return str;
            }
            List<int> pList = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                if (pBetsNumberList[i] != "*")
                {
                    pList.Add(i);
                }
            }
            return CommFunc.Join<int>(pList);
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            if (base.Prize == "")
            {
                string pStr = this.LoginLotteryWeb(pType, "");
                if (pStr != "")
                {
                    base.Prize = CommFunc.GetIndexString(pStr, "\"prize_group\":\"", "\"", pStr.IndexOf("\"rate\":\"0.0010\""));
                }
            }
        }

        public bool InputWeb(string pID, string pW, ref string pHint)
        {
            bool flag = false;
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLoginLine();
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string str5 = HttpUtility.UrlEncode(CommFunc.WebMD51(CommFunc.WebMD51(CommFunc.WebMD51(pID + pW))));
            string pData = $"_token={base.Token}&_random={base.Random}&username={pID}&password={str5}";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("彩票大厅");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "\"pop-content\">", "<", 0);
            }
            return flag;
        }

        public override string LoginLotteryWeb(ConfigurationStatus.LotteryType pType, string pInfo = "")
        {
            string lotteryLine = this.GetLotteryLine(pType, false);
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
            return pResponsetext;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("星多宝");
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

