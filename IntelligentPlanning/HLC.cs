namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class HLC : PTBase
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
                            string format = "gid={0}&issue={1}&rebate={2}&codes%5B0%5D%5Bpid%5D={3}&codes%5B0%5D%5Bcode%5D={4}&codes%5B0%5D%5Bmultiple%5D={5}&codes%5B0%5D%5Bbet_num%5D={6}&codes%5B0%5D%5Bamount_mode%5D={7}&appendcodestop={8}";
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), "0", this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), Convert.ToInt32(plan.AutoTimes(str4, true)), num, plan.Money, 0 });
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
            pHint.Contains("登陆");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"data\":\"投注成功\"") || (pResponseText == "投注成功"));

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
            base.PlayMethodDic.Clear();
            List<string> list = CommFunc.SplitString(pResponseText, "\"},{\"", -1);
            for (int i = 0; i < list.Count; i++)
            {
                string pStr = list[i];
                string str2 = CommFunc.GetIndexString(pStr, "\"h_g_p_name\":\"", "\"", 0);
                string str3 = CommFunc.GetIndexString(pStr, "\"line\":", ",", 0);
                if (str3 != "")
                {
                    string str4 = CommFunc.GetIndexString(pStr, "\"win\":[", "]", 0);
                    string key = $"{str2}-{str3}-{str4}";
                    string str6 = CommFunc.GetIndexString(pStr, "h_g_p_id\":\"", "\"", 0);
                    if (!base.PlayMethodDic.ContainsKey(key))
                    {
                        base.PlayMethodDic[key] = str6;
                    }
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
                string pData = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"h_u_balance\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/users/get_user_info");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string name = AppInfo.Current.Lottery.Name;
            string iD = AppInfo.Current.Lottery.ID;
            string str3 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            if (name.Contains("11选5"))
            {
                str3 = str3.Replace("-0", "-");
            }
            return str3;
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/lottery/add_bet");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "3";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCSE15F)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCNY15C)
            {
                return "31";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCHG15F)
            {
                return "26";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCDJ15F)
            {
                return "28";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "5";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCFLB15C)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCFLB2FC)
            {
                return "12";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCFLB5FC)
            {
                return "13";
            }
            if (pType == ConfigurationStatus.LotteryType.HLCFFC)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.HLC2FC)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.HLC5FC)
            {
                return "9";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "25";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/lottery/get_history_win_code");

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLineString(string pResponseText)
        {
            string str = pResponseText;
            if (str.Contains("http://hlc.client.ph"))
            {
                str = "线路1";
            }
            return str;
        }

        public override string GetLoginLine() => 
            (this.GetLine() + "/");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            (this.GetLine() + "/");

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            int num;
            int num2;
            List<string> list2;
            string str3;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    List<string> pList = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        string pStr = pNumberList[num];
                        pList.Add(CommFunc.Join(pStr, ",", -1));
                    }
                    str = CommFunc.Join(pList, "|").Replace("*", "");
                }
                else if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num2 = AppInfo.FiveDic[ch.ToString()];
                    list2 = new List<string>();
                    for (num = 0; num < 5; num++)
                    {
                        str3 = "*";
                        if (num == num2)
                        {
                            str3 = CommFunc.Join(pNumberList, ",");
                        }
                        list2.Add(str3);
                    }
                    str = CommFunc.Join(list2, "|").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList, ",");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "|");
                }
                if (CommFunc.CheckPlayIsRXDS(playName))
                {
                    str = this.GetRXWZString(pRXWZ) + str;
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
                {
                    return str;
                }
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, "|").Replace(" ", "");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    list2 = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str3 = pNumberList[num].Replace(" ", ",");
                        list2.Add(str3);
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
                if (num2 >= 5)
                {
                    num2 -= 5;
                }
                list2 = new List<string>();
                int num3 = (playName == "猜冠军猜冠军") ? 1 : 5;
                for (num = 0; num < num3; num++)
                {
                    str3 = "*";
                    if (num == num2)
                    {
                        str3 = CommFunc.Join(pNumberList, ",");
                    }
                    list2.Add(str3);
                }
                str = CommFunc.Join(list2, "|").Replace("*", "");
                if (playName == "猜冠军猜冠军")
                {
                    str = str.Replace("|", ",");
                }
            }
            return str;
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
                    return "单式-3-0,1,2";
                }
                if (playName == "前三直选复式")
                {
                    return "复式-3-0,1,2";
                }
                if (playName == "前三组三复式")
                {
                    return "组三-3-0,1,2";
                }
                if (playName == "前三组六复式")
                {
                    return "组六-3-0,1,2";
                }
                if (playName == "后三直选单式")
                {
                    return "单式-3-2,3,4";
                }
                if (playName == "后三直选复式")
                {
                    return "复式-3-2,3,4";
                }
                if (playName == "后三组三复式")
                {
                    return "组三-3-2,3,4";
                }
                if (playName == "后三组六复式")
                {
                    return "组六-3-2,3,4";
                }
                if (playName == "中三直选单式")
                {
                    return "单式-3-1,2,3";
                }
                if (playName == "中三直选复式")
                {
                    return "复式-3-1,2,3";
                }
                if (playName == "中三组三复式")
                {
                    return "组三-3-1,2,3";
                }
                if (playName == "中三组六复式")
                {
                    return "组六-3-1,2,3";
                }
                if (playName == "前二直选单式")
                {
                    return "前二单式-2-0,1";
                }
                if (playName == "前二直选复式")
                {
                    return "前二复式-2-0,1";
                }
                if (playName == "后二直选单式")
                {
                    return "后二单式-2-3,4";
                }
                if (playName == "后二直选复式")
                {
                    return "后二复式-2-3,4";
                }
                if (playName == "后四直选单式")
                {
                    return "单式-4-1,2,3,4";
                }
                if (playName == "后四直选复式")
                {
                    return "复式-4-1,2,3,4";
                }
                if (playName == "前四直选单式")
                {
                    return "单式-4-0,1,2,3";
                }
                if (playName == "前四直选复式")
                {
                    return "复式-4-0,1,2,3";
                }
                if (playName == "五星直选单式")
                {
                    return "五星单式-5-0,1,2,3,4";
                }
                if (playName == "五星直选复式")
                {
                    return "五星复式-5-0,1,2,3,4";
                }
                if (playName == "任三直选单式")
                {
                    return "单式-3-0,1,2,3,4";
                }
                if (playName == "任三直选复式")
                {
                    return "复式-3-0,1,2,3,4";
                }
                if (playName == "任三组三复式")
                {
                    return "";
                }
                if (playName == "任三组六复式")
                {
                    return "组六-3-0,1,2,3,4";
                }
                if (playName == "任二直选单式")
                {
                    return "单式-2-0,1,2,3,4";
                }
                if (playName == "任二直选复式")
                {
                    return "复式-2-0,1,2,3,4";
                }
                if (playName == "任四直选单式")
                {
                    return "单式-4-0,1,2,3,4";
                }
                if (playName == "任四直选复式")
                {
                    return "单式-4-0,1,2,3,4";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "定位胆-5-0,1,2,3,4";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
                {
                    return str;
                }
                if (playName == "猜前三单式")
                {
                    return "单式-3-0,1,2";
                }
                if (playName == "猜前三复式")
                {
                    return "复式-3-0,1,2";
                }
                if (playName == "猜前二单式")
                {
                    return "单式-2-0,1";
                }
                if (playName == "猜前二复式")
                {
                    return "复式-2-0,1";
                }
                if (playName == "猜前四单式")
                {
                    return "";
                }
                if (playName == "猜前四复式")
                {
                    return "";
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
                    return "复式-1-0";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "六至十名-5-5,6,7,8,9" : "一至五名-5-0,1,2,3,4";
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
            return CommFunc.GetIndexString(pResponseText, "\"error\":\"", "\"", 0);
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/users/sign_out");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            List<string> pList = new List<string>();
            for (int i = 0; i < pRXWZ.Count; i++)
            {
                string item = AppInfo.IndexDic[pRXWZ[i]];
                pList.Add(item);
            }
            return $"{CommFunc.Join(pList)}:";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string str;
                string lotteryLine;
                string str3;
                if (base.PlayMethodDic.Count == 0)
                {
                    str = $"{this.GetLine()}/static/model/public/json/game_play_config_{this.GetBetsLotteryID(pType)}.json";
                    lotteryLine = this.GetLotteryLine(pType, false);
                    str3 = "";
                    HttpHelper.GetResponse(ref str3, str, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                    str3 = CommFunc.GetIndexString(str3, "\"data\":[{", "}]}", 0);
                    if (str3 != "")
                    {
                        this.CountPrizeDic(str3);
                    }
                }
                str = this.GetLine() + "/lottery/get_lottery_times";
                lotteryLine = this.GetLotteryLine(pType, false);
                str3 = "";
                string pData = $"id={this.GetBetsLotteryID(pType)}";
                HttpHelper.GetResponse(ref str3, str, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                if (str3 != "")
                {
                    base.Expect = CommFunc.GetIndexString(str3, "\"curr_issue\":\"", "\"", 0);
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
                string str2 = $"/images/set_check_code/?session_name=user_login&type=4&rand={DateTime.Now.ToString()}";
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
                string pUrl = this.GetLine() + "/users/login/";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"username={pID}&password={str5}&verification={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains(pID);
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"error\":\"", "\"", 0);
                    if (pHint.Contains("验证码"))
                    {
                        pHint = "";
                        return this.InputWeb(pID, pW, ref pHint);
                    }
                    return flag;
                }
                base.Rebate = CommFunc.GetIndexString(pResponsetext, "\"h_u_max_rebate\":\"", "\"", 0);
                base.Prize = (1700.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0)).ToString();
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext.Contains("欢乐城");
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

