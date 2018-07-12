namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class B6YL : PTBase
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
                            string str6 = CommFunc.CheckPlayIsDS(plan.Play) ? "input" : "digital";
                            string format = "%7B'type'%3A'{9}'%2C'methodid'%3A{0}%2C'codes'%3A'{1}'%2C'nums'%3A{2}%2C'omodel'%3A{3}%2C'times'%3A{4}%2C'money'%3A{5}%2C'mode'%3A{6}%2C'desc'%3A'{7}+{8}'%2C'poschoose'%3A'{10}'%7D";
                            int num = plan.FNNumber(str4);
                            format = string.Format(format, new object[] { this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), num, "2", Convert.ToInt32(plan.AutoTimes(str4, true)), plan.AutoTotalMoney(str4, true), plan.Unit, this.GetPlayString(plan.Play), this.GetNumberList2(pTNumberList, plan.Play), str6, this.GetRXWZString(plan.RXWZ) });
                            string str8 = "lotteryid={0}&curmid={1}&poschoose={7}&flag=save&play_source=&lt_project%5B%5D={2}&lt_issue_start={3}&lt_total_nums={4}&lt_total_money={5}&randomNum={6}";
                            string prize = this.GetPrize(plan.Type, plan.Play);
                            str8 = string.Format(str8, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsLotteryCurmid(plan.Type), format, this.GetBetsExpect(plan.CurrentExpect, ""), num, plan.AutoTotalMoney(str4, true), DateTime.Now.ToOADate(), this.GetRXWZString(plan.RXWZ) });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str8, pReferer, 0x2710, "UTF-8", true);
                            flag = this.CheckReturn(pResponsetext, true);
                            pHint = this.GetReturn(pResponsetext);
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
            pHint.Contains("重新登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            ((pResponseText == "success") || (pResponseText == "投注成功"));

        private bool CheckVerifyCode(string pVerifyCode)
        {
            if (pVerifyCode.Length != 4)
            {
                return false;
            }
            return true;
        }

        public override void CountPrizeDic(string pResponseText)
        {
            base.PrizeDic.Clear();
            base.PlayMethodDic.Clear();
            string str = "";
            string str2 = "";
            List<string> list = CommFunc.SplitString(CommFunc.GetIndexString(pResponseText, "data_label: [", "cur_issue", 0), "methodid", -1);
            for (int i = 0; i < list.Count; i++)
            {
                string pStr = list[i];
                string str5 = CommFunc.GetIndexString(pStr, " : ", ",", 0);
                string str6 = CommFunc.GetIndexString(pStr, "{levs:'", "'", 0);
                if ((str5 != "") && (str6 != ""))
                {
                    string str7 = $"{str}-{str2}-{CommFunc.GetIndexString(pStr, "name:'", "'", 0)}";
                    base.PlayMethodDic[str7] = str5;
                    if (!CommFunc.CheckIsNumber(str6))
                    {
                        str6 = CommFunc.GetIndexString(pStr, "prize:{1:'", "'", 0);
                        if (!CommFunc.CheckIsNumber(str6))
                        {
                            goto Label_0119;
                        }
                    }
                    base.PrizeDic[str5] = str6;
                }
            Label_0119:
                if (pStr.Contains("title:\""))
                {
                    str = CommFunc.GetIndexString(pStr, "title:\"", "\"", 0);
                }
                if (pStr.Contains("gtitle:'"))
                {
                    str2 = CommFunc.GetIndexString(pStr, "gtitle:'", "'", 0);
                }
            }
        }

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pData = "flag=balance";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string str5 = CommFunc.GetIndexString(pResponsetext, "\"availablebalance\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str5);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/?controller=default&action=ajax");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            switch (iD)
            {
                case "XJSSC":
                    return str2.Replace("-0", "-");

                case "SD11X5":
                case "GD11X5":
                case "SH11X5":
                case "XJ11X5":
                    str2 = str2.Replace("-0", "-");
                    break;
            }
            return str2;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/?controller=game&action=play");

        public override string GetBetsLotteryCurmid(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "50";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "220";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "2265";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "37000";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YLFFC)
            {
                return "19000";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YL3FC)
            {
                return "33333";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YL5FC)
            {
                return "2325";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "174";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "302";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "256";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YL3F11X5)
            {
                return "26000";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "22000";
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
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "13";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "37";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YLFFC)
            {
                return "19";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YL3FC)
            {
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YL5FC)
            {
                return "14";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YL3F11X5)
            {
                return "26";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "22";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            this.GetLotteryLine(pType, true);

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false)
        {
            string str = $"{this.GetLine()}/lottery_{this.GetPTLotteryName(pType)}.php";
            if (pAll)
            {
                str = str + "?show=bet";
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
                        str3 = CommFunc.Join(pStr, "&", -1);
                        list.Add(str3);
                    }
                    str = CommFunc.Join(list, "|").Replace("*", "");
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
                            str3 = CommFunc.Join(pNumberList, "&");
                        }
                        list.Add(str3);
                    }
                    str = CommFunc.Join(list, "|").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsLH(playName))
                {
                    str = CommFunc.Join(pNumberList, "&");
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "&");
                }
                return HttpUtility.UrlEncode(str);
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, "&");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, "&");
                }
                return HttpUtility.UrlEncode(str);
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                str = CommFunc.Join(pNumberList, "&");
            }
            else if (CommFunc.CheckPlayIsFS(playName))
            {
                list = new List<string>();
                for (num = 0; num < pNumberList.Count; num++)
                {
                    str3 = pNumberList[num].Replace(" ", "&");
                    list.Add(str3);
                }
                str = CommFunc.Join(list, "|").Replace("*", "");
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
                if (num2 >= 5)
                {
                    num2 -= 5;
                }
                list = new List<string>();
                int num3 = (playName == "猜冠军猜冠军") ? 1 : 5;
                for (num = 0; num < num3; num++)
                {
                    str3 = "*";
                    if (num == num2)
                    {
                        str3 = CommFunc.Join(pNumberList, "&");
                    }
                    list.Add(str3);
                }
                str = CommFunc.Join(list, "|").Replace("*", "");
            }
            return HttpUtility.UrlEncode(str);
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            string playString = this.GetPlayString(playName);
            if (base.PlayMethodDic.ContainsKey(playString))
            {
                str = base.PlayMethodDic[playString];
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
                    return "前三码-前三直选-单式";
                }
                if (playName == "前三直选复式")
                {
                    return "前三码-前三直选-复式";
                }
                if (playName == "前三组三复式")
                {
                    return "前三码-前三组选-组三";
                }
                if (playName == "前三组六复式")
                {
                    return "前三码-前三组选-组六";
                }
                if (playName == "后三直选单式")
                {
                    return "后三码-后三直选-单式";
                }
                if (playName == "后三直选复式")
                {
                    return "后三码-后三直选-复式";
                }
                if (playName == "后三组三复式")
                {
                    return "后三码-后三组选-组三";
                }
                if (playName == "后三组六复式")
                {
                    return "后三码-后三组选-组六";
                }
                if (playName == "中三直选单式")
                {
                    return "中三码-中三直选-单式";
                }
                if (playName == "中三直选复式")
                {
                    return "中三码-中三直选-复式";
                }
                if (playName == "中三组三复式")
                {
                    return "中三码-中三组选-组三";
                }
                if (playName == "中三组六复式")
                {
                    return "中三码-中三组选-组六";
                }
                if (playName == "前二直选单式")
                {
                    return "二码-二星直选-前二直选(单式)";
                }
                if (playName == "前二直选复式")
                {
                    return "二码-二星直选-前二直选(复式)";
                }
                if (playName == "后二直选单式")
                {
                    return "二码-二星直选-后二直选(单式)";
                }
                if (playName == "后二直选复式")
                {
                    return "二码-二星直选-后二直选(复式)";
                }
                if (playName == "后四直选单式")
                {
                    return "四星-四星直选-单式";
                }
                if (playName == "后四直选复式")
                {
                    return "四星-四星直选-复式";
                }
                if (playName == "五星直选单式")
                {
                    return "五星-五星直选-单式";
                }
                if (playName == "五星直选复式")
                {
                    return "五星-五星直选-复式";
                }
                if (playName.Contains("定位胆"))
                {
                    return "定位胆-定位胆-定位胆";
                }
                if (playName == "龙虎万千")
                {
                    return "龙虎-龙虎和-万千";
                }
                if (playName == "龙虎万百")
                {
                    return "龙虎-龙虎和-万百";
                }
                if (playName == "龙虎万十")
                {
                    return "龙虎-龙虎和-万十";
                }
                if (playName == "龙虎万个")
                {
                    return "龙虎-龙虎和-万个";
                }
                if (playName == "龙虎千百")
                {
                    return "龙虎-龙虎和-千百";
                }
                if (playName == "龙虎千十")
                {
                    return "龙虎-龙虎和-千十";
                }
                if (playName == "龙虎千个")
                {
                    return "龙虎-龙虎和-千个";
                }
                if (playName == "龙虎百十")
                {
                    return "龙虎-龙虎和-百十";
                }
                if (playName == "龙虎百个")
                {
                    return "龙虎-龙虎和-百个";
                }
                if (playName == "龙虎十个")
                {
                    return "龙虎-龙虎和-十个";
                }
                if (playName == "任三直选单式")
                {
                    return "任选-任三直选-直选单式";
                }
                if (playName == "任三直选复式")
                {
                    return "任选-任三直选-直选复式";
                }
                if (playName == "任三组三复式")
                {
                    return "任选-任三组选-组三";
                }
                if (playName == "任三组六复式")
                {
                    return "任选-任三组选-组六";
                }
                if (playName == "任二直选单式")
                {
                    return "任选-任二直选-直选单式";
                }
                if (playName == "任二直选复式")
                {
                    return "任选-任二直选-直选复式";
                }
                if (playName == "任四直选单式")
                {
                    return "任选-任四直选-直选单式";
                }
                if (playName == "任四直选复式")
                {
                    str = "任选-任四直选-直选复式";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "三码-三码-前三直选单式";
                }
                if (playName == "前二直选单式")
                {
                    return "二码-二码-前二直选单式";
                }
                if (playName == "任选复式一中一")
                {
                    return "任选复式-任选复式-一中一";
                }
                if (playName == "任选复式二中二")
                {
                    return "任选复式-任选复式-二中二";
                }
                if (playName == "任选复式三中三")
                {
                    return "任选复式-任选复式-三中三";
                }
                if (playName == "任选复式四中四")
                {
                    return "任选复式-任选复式-四中四";
                }
                if (playName == "任选复式五中五")
                {
                    return "任选复式-任选复式-五中五";
                }
                if (playName == "任选单式一中一")
                {
                    return "任选单式-任选单式-一中一";
                }
                if (playName == "任选单式二中二")
                {
                    return "任选单式-任选单式-二中二";
                }
                if (playName == "任选单式三中三")
                {
                    return "任选单式-任选单式-三中三";
                }
                if (playName == "任选单式四中四")
                {
                    return "任选单式-任选单式-四中四";
                }
                if (playName == "任选单式五中五")
                {
                    str = "任选单式-任选单式-五中五";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "猜前三名-猜前三名-单式";
                }
                if (playName == "猜前三复式")
                {
                    return "猜前三名-猜前三名-复式";
                }
                if (playName == "猜前二单式")
                {
                    return "猜冠亚军-猜冠亚军-单式";
                }
                if (playName == "猜前二复式")
                {
                    return "猜冠亚军-猜冠亚军-复式";
                }
                if (playName == "猜前四单式")
                {
                    return "";
                }
                if (playName == "猜前四复式")
                {
                    return "";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "猜冠军-猜冠军-复式";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "定位胆-定位胆-第6～10名[复式]" : "定位胆-定位胆-第1～5名[复式]";
                }
            }
            return str;
        }

        public override string GetPrize(ConfigurationStatus.LotteryType pType, string playName)
        {
            int num = 0;
            while (true)
            {
                if ((num >= 3) || (base.PrizeDic.Count > 0))
                {
                    string playMethodID = this.GetPlayMethodID(pType, playName);
                    if (!base.PrizeDic.ContainsKey(playMethodID))
                    {
                        return "";
                    }
                    return base.PrizeDic[playMethodID];
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
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"data\":\"", "\"", 0));
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "cqssc";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "xjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "tjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "qqffc2";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YLFFC)
            {
                return "ffc";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YL3FC)
            {
                return "am3fc";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YL5FC)
            {
                return "xgssc";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "sd11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "gd11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "jx11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.B6YL3F11X5)
            {
                return "am11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "bjpk10";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/?controller=default&action=logout");

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
            return CommFunc.Join(pList, "%2C");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            string pStr = this.LoginLotteryWeb(pType, "");
            if (pStr != "")
            {
                base.Expect = CommFunc.GetIndexString(pStr, "issue:'", "'", 0);
                base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
                if (base.PrizeDic.Count == 0)
                {
                    this.CountPrizeDic(pStr);
                }
            }
        }

        public override string GetWebVerifyCode(string pVerifyCodeFile)
        {
            string pVerifyCode = "";
            try
            {
                string str2 = $"/index.php?captcha&rd={DateTime.Now.ToOADate()}";
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
                string pUrl = this.GetLine() + "/?controller=default&action=login";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"flag=login&username={pID}&loginpass={str5}&validcode={webVerifyCode}&Submit=json";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
                flag = pResponsetext.Contains("\"sError\":0,\"sMsg\":\"\"");
                if (!flag)
                {
                    pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "\"sMsg\":\"", "\"", 0).Replace(":", ""));
                }
            }
            return flag;
        }

        public override string LoginLotteryWeb(ConfigurationStatus.LotteryType pType, string pInfo = "")
        {
            string lotteryLine = this.GetLotteryLine(pType, true);
            string pReferer = this.GetLotteryLine(pType, false);
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, base.BetsTime2, "UTF-8", true);
            return (pResponsetext.Contains("用户登陆") || pResponsetext.Contains("B6"));
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

