namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class HKC : PTBase
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
                            int num2 = Convert.ToInt32(Math.Pow(10.0, (double) (plan.Unit - 1)));
                            string format = "'wanFaID':'{0}','touZhuHaoMa':'{1}','digit':'{2}','touZhuBeiShu':'{3}','danZhuJinEDanWei':'{4}','yongHuSuoTiaoFanDian':'{5}','zhuShu':'{6}','bouse':'{7}'";
                            format = string.Format(format, new object[] { this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), this.GetRXWZString1(plan, pTNumberList), Convert.ToInt32(plan.AutoTimes(str4, true)), num2, 0, num, base.Rebate });
                            format = "{" + format + "}";
                            string str7 = "'token':'{0}','issueNo':'{1}','gameId':'{2}','tingZhiZhuiHao':'true','zhuiHaoQiHao':[],'touZhuHaoMa':[{3}]";
                            str7 = string.Format(str7, new object[] { base.Token, this.GetBetsExpect(plan.CurrentExpect, ""), this.GetBetsLotteryID(plan.Type), format });
                            str7 = "json={" + str7 + "}";
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str7, indexLine, base.BetsTime3, "UTF-8", true);
                            flag = this.CheckReturn(pResponsetext, true);
                            pHint = this.GetReturn(pResponsetext);
                            if (pResponsetext.Contains("请勿重复提交"))
                            {
                                base.Token = "";
                                while (base.Token == "")
                                {
                                    Thread.Sleep(0x3e8);
                                }
                            }
                            else
                            {
                                string str8 = CommFunc.GetIndexString(pResponsetext, "\"token_tz\":\"", "\"", 0);
                                if (!((str8 == "") || str8.Contains("==")))
                                {
                                    base.Token = str8;
                                }
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
            pHint.Contains("sessiontimeout");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("投注成功") || (pResponseText == "投注成功"));

        private bool CheckVerifyCode(string pVerifyCode)
        {
            if (pVerifyCode.Length != 4)
            {
                return false;
            }
            if (!CommFunc.CheckIsNumber(pVerifyCode))
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
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"lottery\":", "}", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/userInfo/getBalance.mvc");

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if (((pType != ConfigurationStatus.LotteryType.HKC2FC) && (pType != ConfigurationStatus.LotteryType.HKC5FC)) && (pType != ConfigurationStatus.LotteryType.JN15F))
            {
                return str;
            }
            if (pIsBets)
            {
                return str.Insert(9, "0");
            }
            return str.Remove(9, 1);
        }

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            return this.GetAppExpect(AppInfo.Current.Lottery.Type, str2, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/cathectic/cathectic.mvc");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "9";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "10";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "32";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "35";
            }
            if (pType == ConfigurationStatus.LotteryType.JN15F)
            {
                return "41";
            }
            if (pType == ConfigurationStatus.LotteryType.HKCFFC)
            {
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.HKC2FC)
            {
                return "22";
            }
            if (pType == ConfigurationStatus.LotteryType.HKC5FC)
            {
                return "17";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "13";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "14";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "15";
            }
            if (pType == ConfigurationStatus.LotteryType.ZJ11X5)
            {
                return "21";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "16";
            }
            if (pType == ConfigurationStatus.LotteryType.HKC11X5FFC)
            {
                return "25";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "19";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/lottery/list.mvc");

        public override string GetIndexLine() => 
            (this.GetLine() + "/webapps/yfclottery/lottery.html?1");

        public override string GetLoginLine() => 
            (this.GetLine() + "/Login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            (this.GetLine() + "/gameType/initGame.mvc?");

        public string GetNewToken(ConfigurationStatus.LotteryType pType)
        {
            string betsLotteryID = this.GetBetsLotteryID(pType);
            string pReferer = $"{this.GetLine()}/game?{betsLotteryID}";
            string pUrl = $"{this.GetLine()}/betRecord/getNewestBet.mvc";
            string pResponsetext = "";
            string pData = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, pReferer, 0x2710, "UTF-8", true);
            return CommFunc.GetIndexString(pResponsetext, "\"token_cd\":\"", "\"", 0);
        }

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            int num;
            int num2;
            List<string> list2;
            string str4;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    List<string> pList = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        string str2 = CommFunc.Join(pNumberList[num], ",", -1);
                        if (CommFunc.CheckPlayIsRXFS(playName))
                        {
                            if (str2 == "*")
                            {
                                continue;
                            }
                            string str3 = AppInfo.IndexDic[num];
                            str2 = $"{str3}位:{str2}";
                        }
                        pList.Add(str2);
                    }
                    return CommFunc.Join(pList, "|").Replace("*", "");
                }
                if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num2 = AppInfo.FiveDic[ch.ToString()];
                    list2 = new List<string>();
                    for (num = 0; num < 5; num++)
                    {
                        str4 = "*";
                        if (num == num2)
                        {
                            str4 = CommFunc.Join(pNumberList, ",");
                        }
                        list2.Add(str4);
                    }
                    return CommFunc.Join(list2, "|").Replace("*", "");
                }
                if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        return CommFunc.Join(pNumberList, ",");
                    }
                    return CommFunc.Join(pNumberList, ",");
                }
                return CommFunc.Join(pNumberList, ",");
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",").Replace(" ", "");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                return HttpUtility.UrlEncode(str);
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                return CommFunc.Join(pNumberList, ",").Replace(" ", "");
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                list2 = new List<string>();
                for (num = 0; num < pNumberList.Count; num++)
                {
                    str4 = pNumberList[num].Replace(" ", ",");
                    list2.Add(str4);
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
            list2 = new List<string>();
            int num3 = (playName == "猜冠军猜冠军") ? 1 : 10;
            for (num = 0; num < num3; num++)
            {
                str4 = "*";
                if (num == num2)
                {
                    str4 = CommFunc.Join(pNumberList, ",");
                }
                list2.Add(str4);
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
                    return "10";
                }
                if (playName == "前三直选复式")
                {
                    return "9";
                }
                if (playName == "前三组三复式")
                {
                    return "17";
                }
                if (playName == "前三组三单式")
                {
                    return "420";
                }
                if (playName == "前三组六复式")
                {
                    return "16";
                }
                if (playName == "前三组六单式")
                {
                    return "419";
                }
                if (playName == "后三直选单式")
                {
                    return "8";
                }
                if (playName == "后三直选复式")
                {
                    return "7";
                }
                if (playName == "后三组三复式")
                {
                    return "14";
                }
                if (playName == "后三组三单式")
                {
                    return "418";
                }
                if (playName == "后三组六复式")
                {
                    return "13";
                }
                if (playName == "后三组六单式")
                {
                    return "417";
                }
                if (playName == "中三直选单式")
                {
                    return "12";
                }
                if (playName == "中三直选复式")
                {
                    return "11";
                }
                if (playName == "中三组三复式")
                {
                    return "20";
                }
                if (playName == "中三组三单式")
                {
                    return "422";
                }
                if (playName == "中三组六复式")
                {
                    return "19";
                }
                if (playName == "中三组六单式")
                {
                    return "421";
                }
                if (playName == "前二直选单式")
                {
                    return "25";
                }
                if (playName == "前二直选复式")
                {
                    return "24";
                }
                if (playName == "后二直选单式")
                {
                    return "23";
                }
                if (playName == "后二直选复式")
                {
                    return "22";
                }
                if (playName == "后四直选单式")
                {
                    return "707";
                }
                if (playName == "后四直选复式")
                {
                    return "706";
                }
                if (playName == "五星直选单式")
                {
                    return "698";
                }
                if (playName == "五星直选复式")
                {
                    return "697";
                }
                if (playName == "任三直选单式")
                {
                    return "620";
                }
                if (playName == "任三直选复式")
                {
                    return "619";
                }
                if (playName == "任三组三复式")
                {
                    return "622";
                }
                if (playName == "任三组三单式")
                {
                    return "623";
                }
                if (playName == "任三组六复式")
                {
                    return "624";
                }
                if (playName == "任三组六单式")
                {
                    return "625";
                }
                if (playName == "任二直选单式")
                {
                    return "614";
                }
                if (playName == "任二直选复式")
                {
                    return "613";
                }
                if (playName == "任四直选单式")
                {
                    return "629";
                }
                if (playName == "任四直选复式")
                {
                    return "628";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "41";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "171";
                }
                if (playName == "前二直选单式")
                {
                    return "186";
                }
                if (playName == "任选复式一中一")
                {
                    return "385";
                }
                if (playName == "任选复式二中二")
                {
                    return "386";
                }
                if (playName == "任选复式三中三")
                {
                    return "387";
                }
                if (playName == "任选复式四中四")
                {
                    return "388";
                }
                if (playName == "任选复式五中五")
                {
                    return "389";
                }
                if (playName == "任选单式一中一")
                {
                    return "393";
                }
                if (playName == "任选单式二中二")
                {
                    return "394";
                }
                if (playName == "任选单式三中三")
                {
                    return "395";
                }
                if (playName == "任选单式四中四")
                {
                    return "396";
                }
                if (playName == "任选单式五中五")
                {
                    str = "397";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "1839";
                }
                if (playName == "猜前三复式")
                {
                    return "1838";
                }
                if (playName == "猜前二单式")
                {
                    return "1837";
                }
                if (playName == "猜前二复式")
                {
                    return "1836";
                }
                if (playName == "猜前四单式")
                {
                    return "3203";
                }
                if (playName == "猜前四复式")
                {
                    return "3202";
                }
                if (playName == "猜前五单式")
                {
                    return "3205";
                }
                if (playName == "猜前五复式")
                {
                    return "3204";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "1835";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "1840";
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
            return CommFunc.GetIndexString(pResponseText, "\"MESSAGE\":\"", "\"", 0);
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/login/loginOut.mvc");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString1(ConfigurationStatus.SCPlan plan, List<string> pNumberList)
        {
            List<int> rXWZ = plan.RXWZ;
            string play = plan.Play;
            if (CommFunc.CheckPlayIsRXDS(play))
            {
                return CommFunc.Join(rXWZ, ",");
            }
            if (CommFunc.CheckPlayIsRXFS(play))
            {
                List<string> pList = new List<string>();
                for (int i = 0; i < pNumberList.Count; i++)
                {
                    if (pNumberList[i] != "*")
                    {
                        pList.Add(i.ToString());
                    }
                }
                return CommFunc.Join(pList, ",");
            }
            return plan.PlayInfo.IndexNumString;
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string pUrl = this.GetLine() + "/betType/lotterTime.mvc?";
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                string pData = $"gameId={this.GetBetsLotteryID(pType)}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"ISSUENO\":\"", "\"", 0);
                    base.Expect = this.GetAppExpect(pType, base.Expect, false);
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
                string str2 = $"/verifyCode?{DateTime.Now.ToOADate().ToString()}";
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
                string str2 = HttpUtility.UrlEncode(pW);
                string loginLine = this.GetLoginLine();
                string pUrl = $"{this.GetLine()}/login/login.mvc?username={pID}&password={str2}&_BrowserInfo=chrome/45.0.2454.101&validate={webVerifyCode}";
                string pResponsetext = "";
                string pData = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("\"code\":200");
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
            return pResponsetext.Contains("豪客彩");
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
            if (!this.InputWeb(pID, pW, ref pHint))
            {
                return false;
            }
            return true;
        }
    }
}

