namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web;

    public class UCYL : PTBase
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
                            string str6 = CommFunc.CheckPlayIsDS(plan.Play) ? "input" : "digital";
                            string format = "%7B'type'%3A'{9}'%2C'methodid'%3A{0}%2C'codes'%3A'{1}'%2C'nums'%3A{2}%2C'guaji'%3A1%2C'omodel'%3A0%2C'times'%3A{3}%2C'money'%3A{4}%2C'mode'%3A{5}%2C'desc'%3A'{6}+{7}'%2C'poschoose'%3A'{8}'%7D";
                            int num = plan.FNNumber(str4);
                            string prize = this.GetPrize(plan.Type, plan.Play);
                            format = string.Format(format, new object[] { this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), num, Convert.ToInt32(plan.AutoTimes(str4, true)), plan.AutoTotalMoney(str4, true), plan.Unit, this.GetPlayString(plan.Play), this.GetNumberList2(pTNumberList, plan.Play), this.GetRXWZString(plan.RXWZ), str6 });
                            string str9 = "lotteryid={0}&curmid={1}&poschoose=&flag=save&play_source=&pmodelradio=0&pmodel=0&lt_project_modes={2}&lt_price_h=&lt_project%5B%5D={3}&lt_issue_start={4}&lt_total_nums={5}&lt_total_money={6}";
                            str9 = string.Format(str9, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsLotteryCurmid(plan.Type), plan.Unit, format, this.GetBetsExpect(plan.CurrentExpect, ""), num, plan.AutoTotalMoney(str4, true) });
                            int pTime = (plan.Type == ConfigurationStatus.LotteryType.UCFFC) ? 0x4e20 : 0xea60;
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str9, lotteryLine, pTime, "UTF-8", true);
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
            ((pResponseText == "success") || (pResponseText == "投注成功"));

        public override void CountPrizeDic(string pResponseText)
        {
            base.PrizeDic.Clear();
            base.PlayMethodDic.Clear();
            string pStr = CommFunc.GetIndexString(pResponseText, "data_label:", "cur_issue", 0);
            string str2 = CommFunc.GetIndexString(pStr, "title:\"", "\"", 0);
            List<string> list = CommFunc.SplitString(pStr, "show_str :", -1);
            for (int i = 1; i < list.Count; i++)
            {
                string str3 = list[i];
                string str4 = CommFunc.GetIndexString(str3, "methodid : ", ",", 0);
                string str5 = CommFunc.GetIndexString(str3, "{levs:'", "'", 0);
                string str6 = str2 + CommFunc.GetIndexString(str3, "name:'", "'", 0);
                base.PlayMethodDic[str6] = str4;
                string str7 = CommFunc.GetIndexString(str3, "title:\"", "\"", 0);
                if (str7 != "")
                {
                    str2 = str7;
                }
                if (!CommFunc.CheckIsNumber(str5))
                {
                    str5 = CommFunc.GetIndexString(str3, "\"key\":0,\"value\":\"", "\"", 0);
                    if (str5 == "")
                    {
                        str5 = CommFunc.GetIndexString(str3, "prize:{1:'", "'", 0);
                    }
                    if (!CommFunc.CheckIsNumber(str5))
                    {
                        continue;
                    }
                }
                base.PrizeDic[str4] = str5;
            }
        }

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pData = "flag=getmoney";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string str5 = pResponsetext;
                AppInfo.Account.BankBalance = Convert.ToDouble(str5);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/?controller=default&action=menu");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            switch (iD)
            {
                case "XJSSC":
                case "UCFFC":
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
                return "119";
            }
            if (pType == ConfigurationStatus.LotteryType.UCHGSSC)
            {
                return "3156";
            }
            if (pType == ConfigurationStatus.LotteryType.JNDSSC)
            {
                return "3449";
            }
            if (pType == ConfigurationStatus.LotteryType.UCFFC)
            {
                return "2505";
            }
            if (pType == ConfigurationStatus.LotteryType.UC5FC)
            {
                return "2329";
            }
            if (pType == ConfigurationStatus.LotteryType.UCTWSSC)
            {
                return "3733";
            }
            if (pType == ConfigurationStatus.LotteryType.UCHL2FC)
            {
                return "4244";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "3607";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "4856";
            }
            if (pType == ConfigurationStatus.LotteryType.UCRDFFC)
            {
                return "4740";
            }
            if (pType == ConfigurationStatus.LotteryType.UCRD2FC)
            {
                return "4515";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "174";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "302";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "4200";
            }
            if (pType == ConfigurationStatus.LotteryType.XJ11X5)
            {
                return "4156";
            }
            if (pType == ConfigurationStatus.LotteryType.UC3F11X5)
            {
                return "4358";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "2318";
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
                return "3";
            }
            if (pType == ConfigurationStatus.LotteryType.UCHGSSC)
            {
                return "20";
            }
            if (pType == ConfigurationStatus.LotteryType.JNDSSC)
            {
                return "22";
            }
            if (pType == ConfigurationStatus.LotteryType.UCFFC)
            {
                return "16";
            }
            if (pType == ConfigurationStatus.LotteryType.UC5FC)
            {
                return "15";
            }
            if (pType == ConfigurationStatus.LotteryType.UCTWSSC)
            {
                return "24";
            }
            if (pType == ConfigurationStatus.LotteryType.UCHL2FC)
            {
                return "28";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "23";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "32";
            }
            if (pType == ConfigurationStatus.LotteryType.UCRDFFC)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.UCRD2FC)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "26";
            }
            if (pType == ConfigurationStatus.LotteryType.XJ11X5)
            {
                return "27";
            }
            if (pType == ConfigurationStatus.LotteryType.UC3F11X5)
            {
                return "29";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "14";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            this.GetLotteryLine(pType, true);

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/bet/{this.GetPTLotteryName(pType)}";

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
                        str3 = CommFunc.Join(pStr, "%26", -1);
                        list.Add(str3);
                    }
                    return CommFunc.Join(list, "%7C").Replace("*", "");
                }
                if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num2 = AppInfo.FiveDic[ch.ToString()];
                    list = new List<string>();
                    for (num = 0; num < 5; num++)
                    {
                        str3 = "*";
                        if (num == num2)
                        {
                            str3 = CommFunc.Join(pNumberList, "%26");
                        }
                        list.Add(str3);
                    }
                    return CommFunc.Join(list, "%7C").Replace("*", "");
                }
                return CommFunc.Join(pNumberList, "%26");
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
                if ((playName == "前三直选单式") || (playName == "前三直选复式"))
                {
                    return "前三码单式";
                }
                if (playName == "前三组三复式")
                {
                    return "前三码组三";
                }
                if (playName == "前三组六复式")
                {
                    return "前三码组六";
                }
                if ((playName == "后三直选单式") || (playName == "后三直选复式"))
                {
                    return "后三码单式";
                }
                if (playName == "后三组三复式")
                {
                    return "后三码组三";
                }
                if (playName == "后三组六复式")
                {
                    return "后三码组六";
                }
                if ((playName == "中三直选单式") || (playName == "中三直选复式"))
                {
                    return "中三码单式";
                }
                if (playName == "中三组三复式")
                {
                    return "中三码组三";
                }
                if (playName == "中三组六复式")
                {
                    return "中三码组六";
                }
                if ((playName == "前二直选单式") || (playName == "前二直选复式"))
                {
                    return "二码前二直选(单式)";
                }
                if ((playName == "后二直选单式") || (playName == "后二直选复式"))
                {
                    return "二码后二直选(单式)";
                }
                if ((playName == "后四直选单式") || (playName == "后四直选复式"))
                {
                    return "四星单式";
                }
                if ((playName == "五星直选单式") || (playName == "五星直选复式"))
                {
                    return "五星单式";
                }
                if (playName.Contains("定位胆"))
                {
                    return "定位胆定位胆";
                }
                if (playName == "任三直选单式")
                {
                    return "任选三单式";
                }
                if (playName == "任三直选复式")
                {
                    return "任选三复式";
                }
                if (playName == "任三组三复式")
                {
                    return "任选三组三";
                }
                if (playName == "任三组六复式")
                {
                    return "任选三组六";
                }
                if (playName == "任二直选单式")
                {
                    return "任选二直选单式";
                }
                if (playName == "任二直选复式")
                {
                    return "任选二直选复式";
                }
                if (playName == "任四直选单式")
                {
                    return "任选四单式";
                }
                if (playName == "任四直选复式")
                {
                    str = "任选四复式";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "三码前三直选单式";
                }
                if (playName == "前二直选单式")
                {
                    return "二码前二直选单式";
                }
                if (playName == "任选复式一中一")
                {
                    return "任选复式一中一";
                }
                if (playName == "任选复式二中二")
                {
                    return "任选复式二中二";
                }
                if (playName == "任选复式三中三")
                {
                    return "任选复式三中三";
                }
                if (playName == "任选复式四中四")
                {
                    return "任选复式四中四";
                }
                if (playName == "任选复式五中五")
                {
                    return "任选复式五中五";
                }
                if (playName == "任选单式一中一")
                {
                    return "任选单式一中一";
                }
                if (playName == "任选单式二中二")
                {
                    return "任选单式二中二";
                }
                if (playName == "任选单式三中三")
                {
                    return "任选单式三中三";
                }
                if (playName == "任选单式四中四")
                {
                    return "任选单式四中四";
                }
                if (playName == "任选单式五中五")
                {
                    str = "任选单式五中五";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "猜前三单式";
                }
                if (playName == "猜前三复式")
                {
                    return "猜前三复式";
                }
                if (playName == "猜前二单式")
                {
                    return "猜前二单式";
                }
                if (playName == "猜前二复式")
                {
                    return "猜前二复式";
                }
                if (playName == "猜前四单式")
                {
                    return "猜前四单式";
                }
                if (playName == "猜前四复式")
                {
                    return "猜前四复式";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "猜冠军猜冠军";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "" : "定位胆一到五位";
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
            if (pType == ConfigurationStatus.LotteryType.UCFFC)
            {
                return "ffc";
            }
            if (pType == ConfigurationStatus.LotteryType.UC5FC)
            {
                return "wfc";
            }
            if (pType == ConfigurationStatus.LotteryType.UCTWSSC)
            {
                return "twbg";
            }
            if (pType == ConfigurationStatus.LotteryType.UCHL2FC)
            {
                return "hllt";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "bjwfc";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "bjpk10";
            }
            if (pType == ConfigurationStatus.LotteryType.UCHGSSC)
            {
                return "hgssc";
            }
            if (pType == ConfigurationStatus.LotteryType.UCRDFFC)
            {
                return "rdffc";
            }
            if (pType == ConfigurationStatus.LotteryType.UCRD2FC)
            {
                return "rd2fc";
            }
            if (pType == ConfigurationStatus.LotteryType.UC3F11X5)
            {
                return "3f11x5";
            }
            return CommFunc.GetLotteryID(pType).ToLower();
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
            try
            {
                if (base.PrizeDic.Count <= 0)
                {
                    string pResponseText = this.LoginLotteryWeb(pType, "");
                    if (pResponseText != "")
                    {
                        this.CountPrizeDic(pResponseText);
                    }
                }
            }
            catch
            {
            }
        }

        public bool InputWeb(string pID, string pW, ref string pHint)
        {
            bool flag = false;
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLine() + "/?controller=default&action=login";
            string pResponsetext = "";
            string jScriptByPT = VerifyCodeAPI.GetJScriptByPT(pW, base.PTID);
            string pData = $"flag=login&username={pID}&loginpass={jScriptByPT}&validcode=&Submit=json&forgot=0";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
            flag = pResponsetext.Contains("controller=default&action=main");
            if (!flag)
            {
                pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "\"sMsg\":\"", "\"", 0).Replace(":", ""));
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
            return pResponsetext.Contains("众赢娱乐");
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
            if (!this.InputWeb(pID, pW, ref pHint))
            {
                return false;
            }
            return true;
        }
    }
}

