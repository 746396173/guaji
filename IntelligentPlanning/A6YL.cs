namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class A6YL : PTBase
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
                            string prize = this.GetPrize(plan.Type, plan.Play);
                            string format = "content=%7B%22command_id%22%3A{0}%2C%22lottery_id%22%3A%22{1}%22%2C%22issue%22%3A%22{2}%22%2C%22count%22%3A{3}%2C%22bet_info%22%3A%5B%7B%22method_id%22%3A%22{4}%22%2C%22number%22%3A%22{5}%22%2C%22rebate_count%22%3A{6}%2C%22multiple%22%3A%22{7}%22%2C%22mode%22%3A%22{8}%22%2C%22bet_money%22%3A%22{9}%22%2C%22calc_type%22%3A%22{10}%22%7D%5D%7D";
                            format = string.Format(format, new object[] { "521", this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), 1, this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), base.Rebate, Convert.ToInt32(plan.AutoTimes(str4, true)), plan.Unit - 1, plan.AutoTotalMoney(str4, true), 0 });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, pReferer, base.BetsTime2, "UTF-8", true);
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
            (pResponseText.Contains(@"\u8d2d\u4e70\u6210\u529f") || (pResponseText == "投注成功"));

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
                    string pData = "command=get_user_balance";
                    HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                    if (this.CheckBreakConnect(pResponsetext))
                    {
                        AppInfo.PTInfo.PTIsBreak = true;
                    }
                    else
                    {
                        string str5 = CommFunc.GetIndexString(pResponsetext, "\"money\": \"", "\"", 0);
                        AppInfo.Account.BankBalance = Convert.ToDouble(str5);
                    }
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/user/get");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false).Replace("-", "");
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/proxy_v2");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "3";
            }
            if (pType == ConfigurationStatus.LotteryType.A6FFC)
            {
                return "29";
            }
            if (pType == ConfigurationStatus.LotteryType.A65FC)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "46";
            }
            if (pType == ConfigurationStatus.LotteryType.ELSSSC)
            {
                return "91";
            }
            if (pType == ConfigurationStatus.LotteryType.XXLSSC)
            {
                return "96";
            }
            if (pType == ConfigurationStatus.LotteryType.HGSSC)
            {
                return "47";
            }
            if (pType == ConfigurationStatus.LotteryType.ELSSSC)
            {
                return "91";
            }
            if (pType == ConfigurationStatus.LotteryType.JNDSSC)
            {
                return "48";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "49";
            }
            if (pType == ConfigurationStatus.LotteryType.XJPSSC)
            {
                str = "62";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/proxy");

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            string.Format("{0}/?view=lottery&cpid={0}&v={1}", this.GetLine(), this.GetBetsLotteryID(pType), DateTime.Now.ToOADate());

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            if (CommFunc.CheckPlayIsFS(playName))
            {
                return CommFunc.Join(pNumberList, "%2C").Replace("*", "");
            }
            if (playName.Contains("定位胆"))
            {
                char ch = playName[3];
                int num = AppInfo.FiveDic[ch.ToString()];
                List<string> pList = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    string item = "*";
                    if (i == num)
                    {
                        item = CommFunc.Join(pNumberList);
                    }
                    pList.Add(item);
                }
                return CommFunc.Join(pList, "%2C").Replace("*", "");
            }
            if (CommFunc.CheckPlayIsZuX(playName))
            {
                if (playName.Contains("复式"))
                {
                    return CommFunc.Join(pNumberList);
                }
                return CommFunc.Join(pNumberList, "%24");
            }
            return CommFunc.Join(pNumberList, "%24");
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (playName == "前三直选单式")
            {
                return "18";
            }
            if (playName == "前三直选复式")
            {
                return "17";
            }
            if (playName == "前三组三复式")
            {
                return "20";
            }
            if (playName == "前三组三单式")
            {
                return "21";
            }
            if (playName == "前三组六复式")
            {
                return "22";
            }
            if (playName == "前三组六单式")
            {
                return "23";
            }
            if (playName == "后三直选单式")
            {
                return "3";
            }
            if (playName == "后三直选复式")
            {
                return "2";
            }
            if (playName == "后三组三复式")
            {
                return "4";
            }
            if (playName == "后三组三单式")
            {
                return "5";
            }
            if (playName == "后三组六复式")
            {
                return "6";
            }
            if (playName == "后三组六单式")
            {
                return "7";
            }
            if (playName == "中三直选单式")
            {
                return "131401";
            }
            if (playName == "中三直选复式")
            {
                return "131402";
            }
            if (playName == "中三组三复式")
            {
                return "131404";
            }
            if (playName == "中三组三单式")
            {
                return "131403";
            }
            if (playName == "中三组六复式")
            {
                return "131406";
            }
            if (playName == "中三组六单式")
            {
                return "131405";
            }
            if (playName == "前二直选单式")
            {
                return "11";
            }
            if (playName == "前二直选复式")
            {
                return "10";
            }
            if (playName == "后二直选单式")
            {
                return "13";
            }
            if (playName == "后二直选复式")
            {
                return "12";
            }
            if (playName == "后四直选单式")
            {
                return "141501";
            }
            if (playName == "后四直选复式")
            {
                return "141502";
            }
            if (playName == "前四直选单式")
            {
                return "140401";
            }
            if (playName == "前四直选复式")
            {
                return "140402";
            }
            if (playName == "五星直选复式")
            {
                return "150001";
            }
            if (playName == "五星直选单式")
            {
                return "150030";
            }
            if (playName.Contains("定位胆"))
            {
                str = "9";
            }
            return str;
        }

        public override string GetPTHint(string pResponseText)
        {
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"remark\": \"", "\"", 0));
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.Prize == "")
                {
                    string pUrl = this.GetLine() + "/proxy";
                    string lotteryLine = this.GetLotteryLine(pType, false);
                    string pResponsetext = "";
                    string pData = $"content=520|{base.UserID}|{this.GetBetsLotteryID(pType)}|";
                    HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        string str5 = CommFunc.GetIndexString(pResponsetext, "|", "|", 0);
                        double num = 1800.0 + ((Convert.ToDouble(str5) * 2.0) * 10.0);
                        base.Prize = num.ToString();
                        base.Rebate = (Convert.ToDouble(str5) * 10.0).ToString();
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
                string str2 = $"/valid_code?code_type=login&t={DateTime.Now.ToOADate()}";
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
                string pUrl = this.GetLoginLine();
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = string.Format("valid_code={2}&user_name={0}&password={1}", pID, str5, webVerifyCode);
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("\"success\": 1");
                if (!flag)
                {
                    pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "\"message\": \"", "\"", 0));
                    if (pHint == "异常登陆")
                    {
                        pHint = "";
                        return this.InputWeb(pID, pW, ref pHint);
                    }
                    return flag;
                }
                base.UserID = CommFunc.GetIndexString(pResponsetext, "\"user_id\": \"", "\"", 0);
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext.Contains("A6娱乐");
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

