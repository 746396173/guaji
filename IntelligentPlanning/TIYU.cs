namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class TIYU : PTBase
    {
        public override bool BetsMain(ConfigurationStatus.SCPlan plan, ref string pHint)
        {
            bool flag = false;
            try
            {
                string betsLine = this.GetBetsLine(plan.Type);
                string indexLine = this.GetIndexLine();
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
                            string str7 = CommFunc.Random(0x3b9aca00, 0x77359400).ToString() + CommFunc.Random(100, 0x3e7);
                            string str8 = CommFunc.Random(0x3e8, 0x270f).ToString();
                            string format = "(\"random\":\"{0}\",\"gameType\":\"{1}\",\"isTrace\":0,\"traceWinStop\":0,\"traceStopValue\":-1,\"nowstopTime\":\"\",\"showhand\":0,\"balls\":[(\"id\":{2},\"sdz\":0,\"ball\":\"{3}\",\"type\":\"{4}\",\"moneyunit\":{5},\"oneprice\":{6},\"multiple\":{7},\"num\":{8},\"playBonus\":\"{9}\",\"playRebate\":\"{10}\",\"restrictions\":{11})],\"ordersNumber\":\"\\u7B2C{12}\\u671F\",\"orders\":[(\"number\":\"{12}\",\"lotterytime\":\"\",\"issueCode\":\"{12}\",\"multiple\":1)],\"amount\":\"{13}\",\"onlyAmount\":\"{13}\",\"_quickbet\":0)";
                            format = CommFunc.LZMA7zipEncode(string.Format(format, new object[] { str7, this.GetPTLotteryName(plan.Type), str8, this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), this.GetPlayMethodID(plan.Type, plan.Play), plan.Money, "1", Convert.ToInt32(plan.AutoTimes(str4, true)), num, plan.Mode, "0", base.Rebate, this.GetBetsExpect(plan.CurrentExpect, ""), plan.AutoTotalMoney(str4, true) }).Replace("(", "{").Replace(")", "}"));
                            HttpHelper.GetResponse1(ref pResponsetext, betsLine, "POST", format, indexLine, base.BetsTime2, "UTF-8", true);
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
            (pResponseText.Contains("\"msg\":\"恭喜您已投注成功\"") || (pResponseText == "投注成功"));

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
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"data\":", "}", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/wallet/wallet?_={DateTime.Now.ToOADate()}";

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, true, true, false);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/gameBet/lottery/submit?code={this.GetPTLotteryName(pType)}";

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/gameBet/open-data?code={this.GetPTLotteryName(pType)}&_={DateTime.Now.ToOADate()}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/home.html");

        public override string GetLoginLine() => 
            (this.GetLine() + "/home.html");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            (this.GetLine() + "/home.html");

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
                    if (CommFunc.CheckPlayIsRXFS(playName))
                    {
                        str = CommFunc.Join(pNumberList, ",").Replace("*", "-");
                    }
                    else
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
                    str = CommFunc.Join(pNumberList);
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, ",");
                    }
                    else
                    {
                        str = CommFunc.Join(pNumberList, " ");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, " ");
                }
                if (CommFunc.CheckPlayIsRXDS(playName))
                {
                    str = this.GetRXWZString(pRXWZ) + "|" + str;
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
                return CommFunc.Join(pNumberList, "|").Replace(" ", ",").Replace("*", "");
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
                    str2 = CommFunc.Join(pNumberList, ",");
                }
                list.Add(str2);
            }
            return CommFunc.Join(list, "|").Replace("*", "-");
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
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
                    return "housan.zhixuan.danshi";
                }
                if (playName == "中三直选复式")
                {
                    return "housan.zhixuan.fushi";
                }
                if (playName == "中三组三复式")
                {
                    return "housan.zuxuan.zusan";
                }
                if (playName == "中三组三单式")
                {
                    return "housan.zuxuan.zusandanshi";
                }
                if (playName == "中三组六复式")
                {
                    return "housan.zuxuan.zuliu";
                }
                if (playName == "中三组六单式")
                {
                    return "housan.zuxuan.zuliudanshi";
                }
                if (playName == "前二直选单式")
                {
                    return "qianer.zhixuan.zhixuandanshi";
                }
                if (playName == "前二直选复式")
                {
                    return "qianer.zhixuan.zhixuanfushi";
                }
                if (playName == "后二直选单式")
                {
                    return "houer.zhixuan.zhixuandanshi";
                }
                if (playName == "后二直选复式")
                {
                    return "houer.zhixuan.zhixuanfushi";
                }
                if (playName == "前四直选单式")
                {
                    return "mocksixing.qiansi.zhixuan-danshi";
                }
                if (playName == "前四直选复式")
                {
                    return "mocksixing.qiansi.zhixuan-fushi";
                }
                if (playName == "后四直选单式")
                {
                    return "mocksixing.sixing.zhixuan-danshi";
                }
                if (playName == "后四直选复式")
                {
                    return "mocksixing.sixing.zhixuan-fushi";
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
                    return "rensan.wanfa.zhixuandanshi";
                }
                if (playName == "任三直选复式")
                {
                    return "rensan.wanfa.zhixuanfushi";
                }
                if (playName == "任三组三复式")
                {
                    return "rensan.wanfa.zusanfushi";
                }
                if (playName == "任三组三单式")
                {
                    return "rensan.wanfa.zusandanshi";
                }
                if (playName == "任三组六复式")
                {
                    return "rensan.wanfa.zuliufushi";
                }
                if (playName == "任三组六单式")
                {
                    return "rensan.wanfa.zuliudanshi";
                }
                if (playName == "任二直选单式")
                {
                    return "rener.wanfa.zhixuandanshi";
                }
                if (playName == "任二直选复式")
                {
                    return "rener.wanfa.zhixuanfushi";
                }
                if (playName == "任四直选单式")
                {
                    return "rensi.rensi.rensidanshi";
                }
                if (playName == "任四直选复式")
                {
                    return "rensi.rensi.rensifushi";
                }
                if (playName.Contains("定位胆"))
                {
                    return "yixing.dingweidan.fushi";
                }
                if (playName == "龙虎万千")
                {
                    return "longhu.longhuhe.wanqian";
                }
                if (playName == "龙虎万百")
                {
                    return "longhu.longhuhe.wanbai";
                }
                if (playName == "龙虎万十")
                {
                    return "longhu.longhuhe.wanshi";
                }
                if (playName == "龙虎万个")
                {
                    return "longhu.longhuhe.wange";
                }
                if (playName == "龙虎千百")
                {
                    return "longhu.longhuhe.qianbai";
                }
                if (playName == "龙虎千十")
                {
                    return "longhu.longhuhe.qianshi";
                }
                if (playName == "龙虎千个")
                {
                    return "longhu.longhuhe.qiange";
                }
                if (playName == "龙虎百十")
                {
                    return "longhu.longhuhe.baishi";
                }
                if (playName == "龙虎百个")
                {
                    return "longhu.longhuhe.baige";
                }
                if (playName == "龙虎十个")
                {
                    str = "longhu.longhuhe.shige";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "xuansan.qiansanzhixuan.danshi";
                }
                if (playName == "前二直选单式")
                {
                    return "xuaner.qianerzhixuan.danshi";
                }
                if (playName == "任选复式一中一")
                {
                    return "xuanyi.renxuanyizhongyi.fushi";
                }
                if (playName == "任选复式二中二")
                {
                    return "xuaner.renxuanerzhonger.fushi";
                }
                if (playName == "任选复式三中三")
                {
                    return "xuansan.renxuansanzhongsan.fushi";
                }
                if (playName == "任选复式四中四")
                {
                    return "xuansi.renxuansizhongsi.fushi";
                }
                if (playName == "任选复式五中五")
                {
                    return "xuanwu.renxuanwuzhongwu.fushi";
                }
                if (playName == "任选单式一中一")
                {
                    return "xuanyi.renxuanyizhongyi.danshi";
                }
                if (playName == "任选单式二中二")
                {
                    return "xuaner.renxuanerzhonger.danshi";
                }
                if (playName == "任选单式三中三")
                {
                    return "xuansan.renxuansanzhongsan.danshi";
                }
                if (playName == "任选单式四中四")
                {
                    return "xuansi.renxuansizhongsi.danshi";
                }
                if (playName == "任选单式五中五")
                {
                    str = "xuanwu.renxuanwuzhongwu.danshi";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "danshi.danshi.caiqiansandanshi";
                }
                if (playName == "猜前三复式")
                {
                    return "putong.putong.caiqiansanming";
                }
                if (playName == "猜前二单式")
                {
                    return "danshi.danshi.caiqianerdanshi";
                }
                if (playName == "猜前二复式")
                {
                    return "putong.putong.caiguanyajun";
                }
                if (playName == "猜前四单式")
                {
                    return "danshi.danshi.caiqiansidanshi";
                }
                if (playName == "猜前四复式")
                {
                    return "putong.putong.caiqiansiming";
                }
                if (playName == "猜前五单式")
                {
                    return "danshi.danshi.caiqianwudanshi";
                }
                if (playName == "猜前五复式")
                {
                    return "putong.putong.caiqianwuming";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "putong.putong.caiguanjun";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "putong.putong.dingweidan";
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

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "cqssc";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "tjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HLJSSC)
            {
                return "hljssc";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "txffc";
            }
            if (pType == ConfigurationStatus.LotteryType.TIYUML2FC)
            {
                return "mllfc";
            }
            if (pType == ConfigurationStatus.LotteryType.TIYUFLP15C)
            {
                return "flb1d5";
            }
            if (pType == ConfigurationStatus.LotteryType.TIYUNHG15C)
            {
                return "hgklb";
            }
            if (pType == ConfigurationStatus.LotteryType.TIYUXDL15C)
            {
                return "xdl1d5";
            }
            if (pType == ConfigurationStatus.LotteryType.TIYUXJPSSC)
            {
                return "xjpklb";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "sd115";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "ah11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "jx115";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "sh11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.LN11X5)
            {
                return "ln11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "pk10";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/sso/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            string str = "";
            if ((pRXWZ == null) || (pRXWZ.Count <= 0))
            {
                return str;
            }
            List<string> pList = new List<string>();
            foreach (int num in pRXWZ)
            {
                pList.Add(AppInfo.IndexDic[num]);
            }
            return CommFunc.Join(pList);
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            string pUrl = $"{this.GetLine()}/gameBet/lottery/lastNumber?code={this.GetPTLotteryName(pType)}&_={DateTime.Now.ToOADate()}";
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
            if (pResponsetext != "")
            {
                base.Expect = CommFunc.GetIndexString(pResponsetext, "\"number\":\"", "\"", 0);
                base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
            }
        }

        public override string GetWebVerifyCode(string pVerifyCodeFile)
        {
            string pVerifyCode = "";
            try
            {
                string str2 = $"/common/validateImage.action?{DateTime.Now.ToOADate()}";
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
                string pUrl = this.GetLine() + "/uc/loginService";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"username={pID}&password={str5}&validatecode={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("\"code\":\"0\"");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"msg\":\"", "\"", 0);
                    return flag;
                }
                base.Rebate = CommFunc.GetIndexString(pResponsetext, "\"maxusedrebate\":\"", "\"", 0);
                base.Prize = (1800.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0)).ToString();
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext.Contains("天宇娱乐");
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

