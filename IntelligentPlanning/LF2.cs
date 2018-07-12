namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class LF2 : PTBase
    {
        public string EncrypCode = "";
        public string Md5Check = "";
        public string VC = "";

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
                            string format = "%22%3A%5B%7B%22type%22%3A%22{8}%22%2C%22methodid%22%3A{0}%2C%22mode%22%3A{1}%2C%22times%22%3A{2}%2C%22point%22%3A%22{3}%22%2C%22codes%22%3A%22{4}%22%2C%22desc%22%3A%22{5}+{6}%22%2C%22position%22%3A%22{7}%22%7D%5D%7D";
                            format = string.Format(format, new object[] { this.GetPlayMethodID(plan.Type, plan.Play), plan.Unit, Convert.ToInt32(plan.AutoTimes(str4, true)), "0", this.GetNumberList1(pTNumberList, plan.Play, null), this.GetPlayString(plan.Play), this.GetNumberList2(pTNumberList, plan.Play), this.GetRXWZString(plan.RXWZ), str6 });
                            string str8 = "jsonstr=%7B%22lotteryId%22%3A%22{0}%22%2C%22issuo%22%3A%22{1}%22%2C%22memberid%22%3A%22{4}%22%2C%22lt_trace_if%22%3Afalse%2C%22lt_trace_stop%22%3Afalse%2C%22lt_trace_issues%22%3A%5B%5D%2C%22lt_project{2}&__RequestVerificationToken={3}";
                            str8 = string.Format(str8, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), format, "undefined", base.PTUserID });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str8, lotteryLine, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
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
            pHint.Contains("立即登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"stats\":\"success\"") || (pResponseText == "投注成功"));

        private bool CheckVerifyCode(string pVerifyCode)
        {
            if (pVerifyCode.Length != 6)
            {
                return false;
            }
            return true;
        }

        public override void CountPrizeDic(string pResponseText)
        {
            base.PrizeDic.Clear();
            List<string> list = CommFunc.SplitString(CommFunc.GetIndexString(pResponseText, "var pri_user_data = [{", "var pri_cur_issue =", 0), "}]},{", -1);
            for (int i = 0; i < list.Count; i++)
            {
                string pStr = list[i];
                string str3 = CommFunc.GetIndexString(pStr, "methodid: ", ",", 0);
                string str4 = CommFunc.GetIndexString(pStr, "\"prize\": ", "}", pStr.IndexOf("\"point\": \"0\""));
                base.PrizeDic[str3] = str4;
            }
        }

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pData = $"__RequestVerificationToken={base.Token}";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string str5 = CommFunc.GetIndexString(pResponsetext, "\"sscmoney\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str5);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/member/fastdata");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            switch (iD)
            {
                case "XJSSC":
                case "SD11X5":
                case "GD11X5":
                case "JX11X5":
                    str2 = str2.Replace("-0", "-");
                    break;
            }
            return str2;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/lottery/bet");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.LFDJSSC)
            {
                return "46";
            }
            if (pType == ConfigurationStatus.LotteryType.LFHGSSC)
            {
                return "40";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "49";
            }
            if (pType == ConfigurationStatus.LotteryType.QQFFC)
            {
                return "48";
            }
            if (pType == ConfigurationStatus.LotteryType.HGSSC)
            {
                return "47";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "41";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "45";
            }
            if (pType == ConfigurationStatus.LotteryType.MD2FC)
            {
                return "60";
            }
            if (pType == ConfigurationStatus.LotteryType.JNDSSC)
            {
                return "42";
            }
            if (pType == ConfigurationStatus.LotteryType.LF2FFC)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.LF22FC)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.LF25FC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.LF11X5)
            {
                return "14";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "12";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "10";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "28";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            this.GetLotteryLine(pType, false);

        public override string GetIndexLine() => 
            (this.GetLine() + "/home/home");

        public override string GetLoginLine() => 
            (this.GetLine() + "/public/login");

        public override string GetLoginLineID() => 
            (this.GetLine() + "/public/login");

        public override string GetLoginLinePW() => 
            (this.GetLine() + "/public/logincheck");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false)
        {
            string str = "ssc";
            if (((((pType == ConfigurationStatus.LotteryType.LFDJSSC) || (pType == ConfigurationStatus.LotteryType.TWSSC)) || ((pType == ConfigurationStatus.LotteryType.BJSSC) || (pType == ConfigurationStatus.LotteryType.JNDSSC))) || ((pType == ConfigurationStatus.LotteryType.LFHGSSC) || (pType == ConfigurationStatus.LotteryType.HGSSC))) || (pType == ConfigurationStatus.LotteryType.MD2FC))
            {
                str = "hgc";
            }
            else if ((((pType == ConfigurationStatus.LotteryType.LF11X5) || (pType == ConfigurationStatus.LotteryType.GD11X5)) || (pType == ConfigurationStatus.LotteryType.SD11X5)) || (pType == ConfigurationStatus.LotteryType.JX11X5))
            {
                str = "syxw";
            }
            else if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "pks";
            }
            return $"{this.GetLine()}/lottery/{str}/{this.GetBetsLotteryID(pType)}";
        }

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            int num;
            string str3;
            int num2;
            string pSource = "";
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
                    pSource = CommFunc.Join(list, "|").Replace("*", "");
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
                    pSource = CommFunc.Join(list, "|").Replace("*", "");
                }
                else
                {
                    pSource = CommFunc.Join(pNumberList, "&");
                }
                if (CommFunc.CheckPlayIsDS(playName) && ((playName.Contains("后四") || playName.Contains("任四")) || playName.Contains("五星")))
                {
                    return CommFunc.LZMAEncode(pSource);
                }
                return HttpUtility.UrlEncode(pSource);
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    pSource = CommFunc.Join(pNumberList, "&");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    pSource = CommFunc.Join(pNumberList, "&");
                }
                return HttpUtility.UrlEncode(pSource);
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return pSource;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                pSource = CommFunc.Join(pNumberList, "&");
            }
            else if (CommFunc.CheckPlayIsFS(playName))
            {
                list = new List<string>();
                for (num = 0; num < pNumberList.Count; num++)
                {
                    str3 = pNumberList[num].Replace(" ", "&");
                    list.Add(str3);
                }
                pSource = CommFunc.Join(list, "|").Replace("*", "");
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
                int num3 = (playName == "猜冠军猜冠军") ? 1 : 10;
                for (num = 0; num < num3; num++)
                {
                    str3 = "*";
                    if (num == num2)
                    {
                        str3 = CommFunc.Join(pNumberList, "&");
                    }
                    list.Add(str3);
                }
                pSource = CommFunc.Join(list, "|").Replace("*", "");
            }
            return HttpUtility.UrlEncode(pSource);
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "27";
                }
                if (playName == "前三直选复式")
                {
                    return "27";
                }
                if (playName == "前三组三复式")
                {
                    return "33";
                }
                if (playName == "前三组三单式")
                {
                    return "33";
                }
                if (playName == "前三组六复式")
                {
                    return "34";
                }
                if (playName == "前三组六单式")
                {
                    return "34";
                }
                if (playName == "后三直选单式")
                {
                    return "11";
                }
                if (playName == "后三直选复式")
                {
                    return "11";
                }
                if (playName == "后三组三复式")
                {
                    return "17";
                }
                if (playName == "后三组三单式")
                {
                    return "17";
                }
                if (playName == "后三组六复式")
                {
                    return "18";
                }
                if (playName == "后三组六单式")
                {
                    return "18";
                }
                if (playName == "中三直选单式")
                {
                    return "58811";
                }
                if (playName == "中三直选复式")
                {
                    return "58811";
                }
                if (playName == "中三组三复式")
                {
                    return "58817";
                }
                if (playName == "中三组三单式")
                {
                    return "58817";
                }
                if (playName == "中三组六复式")
                {
                    return "58818";
                }
                if (playName == "中三组六单式")
                {
                    return "58818";
                }
                if (playName == "前二直选单式")
                {
                    return "43";
                }
                if (playName == "前二直选复式")
                {
                    return "43";
                }
                if (playName == "后二直选单式")
                {
                    return "47";
                }
                if (playName == "后二直选复式")
                {
                    return "47";
                }
                if (playName == "后四直选单式")
                {
                    return "2";
                }
                if (playName == "后四直选复式")
                {
                    return "2";
                }
                if (playName == "五星直选单式")
                {
                    return "865";
                }
                if (playName == "五星直选复式")
                {
                    return "865";
                }
                if (playName == "任三直选单式")
                {
                    return "676";
                }
                if (playName == "任三直选复式")
                {
                    return "676";
                }
                if (playName == "任三组三单式")
                {
                    return "686";
                }
                if (playName == "任三组三复式")
                {
                    return "686";
                }
                if (playName == "任三组六单式")
                {
                    return "696";
                }
                if (playName == "任三组六复式")
                {
                    return "696";
                }
                if (playName == "任二直选单式")
                {
                    return "654";
                }
                if (playName == "任二直选复式")
                {
                    return "654";
                }
                if (playName == "任四直选单式")
                {
                    return "725";
                }
                if (playName == "任四直选复式")
                {
                    return "725";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "59";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "90";
                }
                if (playName == "前二直选单式")
                {
                    return "94";
                }
                if (playName == "任选复式一中一")
                {
                    return "108";
                }
                if (playName == "任选复式二中二")
                {
                    return "110";
                }
                if (playName == "任选复式三中三")
                {
                    return "112";
                }
                if (playName == "任选复式四中四")
                {
                    return "114";
                }
                if (playName == "任选复式五中五")
                {
                    return "116";
                }
                if (playName == "任选单式一中一")
                {
                    return "108";
                }
                if (playName == "任选单式二中二")
                {
                    return "110";
                }
                if (playName == "任选单式三中三")
                {
                    return "112";
                }
                if (playName == "任选单式四中四")
                {
                    return "114";
                }
                if (playName == "任选单式五中五")
                {
                    str = "116";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "1016";
                }
                if (playName == "猜前三复式")
                {
                    return "1016";
                }
                if (playName == "猜前二单式")
                {
                    return "1015";
                }
                if (playName == "猜前二复式")
                {
                    return "1015";
                }
                if (playName == "猜前四单式")
                {
                    return "1017";
                }
                if (playName == "猜前四复式")
                {
                    return "1017";
                }
                if (playName == "猜前五单式")
                {
                    return "1018";
                }
                if (playName == "猜前五复式")
                {
                    return "1018";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "1005";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "1030";
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
                    return HttpUtility.UrlEncode("[前三直选_单式]");
                }
                if (playName == "前三直选复式")
                {
                    return HttpUtility.UrlEncode("[前三直选_复式]");
                }
                if (playName == "前三组三复式")
                {
                    return HttpUtility.UrlEncode("[前三组选_组三复式]");
                }
                if (playName == "前三组三单式")
                {
                    return HttpUtility.UrlEncode("[前三组选_组三单式]");
                }
                if (playName == "前三组六复式")
                {
                    return HttpUtility.UrlEncode("[前三组选_组六复式]");
                }
                if (playName == "前三组六单式")
                {
                    return HttpUtility.UrlEncode("[前三组选_组六单式]");
                }
                if (playName == "后三直选单式")
                {
                    return HttpUtility.UrlEncode("[后三直选_单式]");
                }
                if (playName == "后三直选复式")
                {
                    return HttpUtility.UrlEncode("[后三直选_复式]");
                }
                if (playName == "后三组三复式")
                {
                    return HttpUtility.UrlEncode("[后三组选_组三复式]");
                }
                if (playName == "后三组三单式")
                {
                    return HttpUtility.UrlEncode("[后三组选_组三单式]");
                }
                if (playName == "后三组六复式")
                {
                    return HttpUtility.UrlEncode("[后三组选_组六复式]");
                }
                if (playName == "后三组六单式")
                {
                    return HttpUtility.UrlEncode("[后三组选_组六单式]");
                }
                if (playName == "中三直选单式")
                {
                    return HttpUtility.UrlEncode("[中三直选_单式]");
                }
                if (playName == "中三直选复式")
                {
                    return HttpUtility.UrlEncode("[中三直选_复式]");
                }
                if (playName == "中三组三复式")
                {
                    return HttpUtility.UrlEncode("[中三组选_组三复式]");
                }
                if (playName == "中三组三单式")
                {
                    return HttpUtility.UrlEncode("[中三组选_组三单式]");
                }
                if (playName == "中三组六复式")
                {
                    return HttpUtility.UrlEncode("[中三组选_组六复式]");
                }
                if (playName == "中三组六单式")
                {
                    return HttpUtility.UrlEncode("[中三组选_组六单式]");
                }
                if (playName == "前二直选单式")
                {
                    return HttpUtility.UrlEncode("[前二直选_单式]");
                }
                if (playName == "前二直选复式")
                {
                    return HttpUtility.UrlEncode("[前二直选_复式]");
                }
                if (playName == "后二直选单式")
                {
                    return HttpUtility.UrlEncode("[后二直选_单式]");
                }
                if (playName == "后二直选复式")
                {
                    return HttpUtility.UrlEncode("[后二直选_复式]");
                }
                if (playName == "后四直选单式")
                {
                    return HttpUtility.UrlEncode("[四星直选_单式]");
                }
                if (playName == "后四直选复式")
                {
                    return HttpUtility.UrlEncode("[四星直选_复式]");
                }
                if (playName == "五星直选单式")
                {
                    return HttpUtility.UrlEncode("[五星直选_单式]");
                }
                if (playName == "五星直选复式")
                {
                    return HttpUtility.UrlEncode("[五星直选_复式]");
                }
                if (playName == "任三直选单式")
                {
                    return HttpUtility.UrlEncode("[任三直选_直选单式]");
                }
                if (playName == "任三直选复式")
                {
                    return HttpUtility.UrlEncode("[任三直选_直选复式]");
                }
                if (playName == "任三组三单式")
                {
                    return HttpUtility.UrlEncode("[任三组选_组三单式]");
                }
                if (playName == "任三组三复式")
                {
                    return HttpUtility.UrlEncode("[任三组选_组三复式]");
                }
                if (playName == "任三组六单式")
                {
                    return HttpUtility.UrlEncode("[任三组选_组六单式]");
                }
                if (playName == "任三组六复式")
                {
                    return HttpUtility.UrlEncode("[任三组选_组六复式]");
                }
                if (playName == "任二直选单式")
                {
                    return HttpUtility.UrlEncode("[任二直选_直选单式]");
                }
                if (playName == "任二直选复式")
                {
                    return HttpUtility.UrlEncode("[任选二_直选复式]");
                }
                if (playName == "任四直选单式")
                {
                    return HttpUtility.UrlEncode("[任四直选_直选单式]");
                }
                if (playName == "任四直选复式")
                {
                    return HttpUtility.UrlEncode("[任选四_直选复式]");
                }
                if (playName.Contains("定位胆"))
                {
                    str = HttpUtility.UrlEncode("[定位胆_定位胆]");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return HttpUtility.UrlEncode("[三码_前三直选单式]");
                }
                if (playName == "前二直选单式")
                {
                    return HttpUtility.UrlEncode("[二码_前二直选单式]");
                }
                if (playName == "任选复式一中一")
                {
                    return HttpUtility.UrlEncode("[任选复式_任选一中一]");
                }
                if (playName == "任选复式二中二")
                {
                    return HttpUtility.UrlEncode("[任选复式_任选二中二]");
                }
                if (playName == "任选复式三中三")
                {
                    return HttpUtility.UrlEncode("[任选复式_任选三中三]");
                }
                if (playName == "任选复式四中四")
                {
                    return HttpUtility.UrlEncode("[任选复式_任选四中四]");
                }
                if (playName == "任选复式五中五")
                {
                    return HttpUtility.UrlEncode("[任选复式_任选五中五]");
                }
                if (playName == "任选单式一中一")
                {
                    return HttpUtility.UrlEncode("[任选单式_任选一中一]");
                }
                if (playName == "任选单式二中二")
                {
                    return HttpUtility.UrlEncode("[任选单式_任选二中二]");
                }
                if (playName == "任选单式三中三")
                {
                    return HttpUtility.UrlEncode("[任选单式_任选三中三]");
                }
                if (playName == "任选单式四中四")
                {
                    return HttpUtility.UrlEncode("[任选单式_任选四中四]");
                }
                if (playName == "任选单式五中五")
                {
                    str = HttpUtility.UrlEncode("[任选单式_任选五中五]");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return HttpUtility.UrlEncode("[精确前三_单式]");
                }
                if (playName == "猜前三复式")
                {
                    return HttpUtility.UrlEncode("[精确前三_精确前三]");
                }
                if (playName == "猜前二单式")
                {
                    return HttpUtility.UrlEncode("[精确前二_单式]");
                }
                if (playName == "猜前二复式")
                {
                    return HttpUtility.UrlEncode("[精确前二_精确前二]");
                }
                if (playName == "猜前四单式")
                {
                    return HttpUtility.UrlEncode("[精确前四_单式]");
                }
                if (playName == "猜前四复式")
                {
                    return HttpUtility.UrlEncode("[精确前四_精确前四]");
                }
                if (playName == "猜前五单式")
                {
                    return HttpUtility.UrlEncode("[精确前五_单式]");
                }
                if (playName == "猜前五复式")
                {
                    return HttpUtility.UrlEncode("[精确前五_精确前五]");
                }
                if (playName == "猜冠军猜冠军")
                {
                    return HttpUtility.UrlEncode("[精确前一_猜冠军]");
                }
                if (playName.Contains("定位胆"))
                {
                    str = HttpUtility.UrlEncode("[定位胆_定位胆]");
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
            string str = "";
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            str = CommFunc.GetIndexString(pResponseText, "\"errmsg\":\"", "\"", 0);
            if (str == "")
            {
                str = CommFunc.GetIndexString(pResponseText, "\"msg\":\"", "\"", 0);
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/public/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            string str = "";
            if (pRXWZ != null)
            {
                str = CommFunc.Join(pRXWZ, "%26");
            }
            return str;
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.PrizeDic.Count <= 0)
                {
                    string pStr = this.LoginLotteryWeb(pType, "");
                    if (pStr != "")
                    {
                        base.Token = CommFunc.GetIndexString(pStr, "value=\"", "\"", pStr.IndexOf("__RequestVerificationToken"));
                        this.CountPrizeDic(pStr);
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
                string str2 = $"/public/getValidCodeByJosn?time={DateTime.Now.ToOADate()}";
                string pUrl = this.GetLine() + str2;
                if (File.Exists(pVerifyCodeFile))
                {
                    File.Delete(pVerifyCodeFile);
                }
                string pData = $"__RequestVerificationToken={base.Token}";
                string loginLine = this.GetLoginLine();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                if (pResponsetext == "")
                {
                    return pVerifyCode;
                }
                string s = CommFunc.GetIndexString(pResponsetext, "base64,", "\"", 0);
                this.EncrypCode = CommFunc.GetIndexString(pResponsetext, "\"EncrypCode\":\"", "\"", 0);
                byte[] buffer = Convert.FromBase64String(s);
                MemoryStream stream = new MemoryStream(buffer, true);
                stream.Write(buffer, 0, buffer.Length);
                Bitmap bitmap = new Bitmap(stream);
                bitmap.Save(pVerifyCodeFile);
                bitmap.Dispose();
                while (!File.Exists(pVerifyCodeFile))
                {
                    Thread.Sleep(500);
                }
                pVerifyCode = VerifyCodeAPI.VerifyCodeMain(base.PTID, pVerifyCodeFile);
                this.VC = CommFunc.GetIndexString(pResponsetext, "\"vc\":\"", "\"", 0);
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

        public bool InputIDWeb(string pID, ref string pHint)
        {
            bool flag = false;
            string webVerifyCode = this.GetWebVerifyCode(AutoBetsWindow.VerifyCodeFile);
            if (webVerifyCode != "")
            {
                string loginLine = this.GetLoginLine();
                string loginLineID = this.GetLoginLineID();
                string pResponsetext = "";
                string pData = $"IsGaLogin=False&UserName={pID}&VC=&ReCaptcha.EncrypCode={HttpUtility.UrlEncode(this.EncrypCode)}&ReCaptcha.VerifyCode={webVerifyCode}&__RequestVerificationToken={HttpUtility.UrlEncode(base.Token)}";
                HttpHelper.GetResponse(ref pResponsetext, loginLineID, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("问候语");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "alert('", "'", 0);
                    if (pHint.Contains("验证码"))
                    {
                        pHint = "";
                        return this.InputIDWeb(pID, ref pHint);
                    }
                    return flag;
                }
                this.VC = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("name=\"VC\""));
                base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("name=\"__RequestVerificationToken\""));
                this.Md5Check = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("name=\"Md5Check\""));
            }
            return flag;
        }

        public bool InputPWWeb(string pID, string pW, ref string pHint)
        {
            string loginLine = this.GetLoginLine();
            string loginLinePW = this.GetLoginLinePW();
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(CommFunc.WebMD51(CommFunc.WebMD51(pW + this.VC)));
            string pData = $"LogGreeting=&UserName={pID}&Password={str4}&Brower=Chrome&VC={HttpUtility.UrlEncode(this.VC)}&IsNeedCheck=0&Md5Check={HttpUtility.UrlEncode(this.Md5Check)}&__RequestVerificationToken={HttpUtility.UrlEncode(base.Token)}";
            HttpHelper.GetResponse(ref pResponsetext, loginLinePW, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("游戏列表");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "alert('", "'", 0);
                return flag;
            }
            base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
            base.PTUserID = CommFunc.GetIndexString(pResponsetext, "userId%3d", "%26", 0);
            return flag;
        }

        public override string LoginLotteryWeb(ConfigurationStatus.LotteryType pType, string pInfo = "")
        {
            string lotteryLine = this.GetLotteryLine(pType, true);
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
            return pResponsetext;
        }

        public bool LoginWeb()
        {
            bool flag;
            if (base.IsLoadWebLogin)
            {
                flag = false;
                string cookieInternal = HttpHelper.GetCookieInternal(this.GetUrlLine());
                if ((cookieInternal == "") || (AppInfo.PTInfo == null))
                {
                    return flag;
                }
                AppInfo.PTInfo.WebCookie = cookieInternal;
                HttpHelper.SaveCookies(cookieInternal, "");
                return true;
            }
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, loginLine, base.BetsTime2, "UTF-8", true);
            flag = pResponsetext.Contains("请输入用户名");
            if (flag)
            {
                base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
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
            if (!this.InputIDWeb(pID, ref pHint))
            {
                return false;
            }
            if (!this.InputPWWeb(pID, pW, ref pHint))
            {
                return false;
            }
            return true;
        }
    }
}

