namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class JXYL : PTBase
    {
        public override bool BetsMain(ConfigurationStatus.SCPlan plan, ref string pHint)
        {
            bool flag = false;
            try
            {
                string betsLine = this.GetBetsLine(plan.Type);
                string indexCode = this.GetIndexCode(plan.Type);
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
                            string format = "command=lottery_logon_request_transmit_v2&param=%7B%22command_id%22%3A521%2C%22lottery_id%22%3A%22{0}%22%2C%22issue%22%3A%22{1}%22%2C%22count%22%3A{2}%2C%22bet_info%22%3A%5B%7B%22method_id%22%3A%22{3}%22%2C%22number%22%3A%22{4}%22%2C%22rebate_count%22%3A{5}%2C%22multiple%22%3A%22{6}%22%2C%22mode%22%3A{7}%2C%22bet_money%22%3A%22{8}%22%2C%22calc_type%22%3A%22{9}%22%7D%5D%7D";
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), 1, this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), base.Rebate, Convert.ToInt32(plan.AutoTimes(str4, true)), plan.Unit - 1, plan.AutoTotalMoney(str4, true), 1 });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, indexCode, base.BetsTime2, "UTF-8", true);
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
            (pHint.Contains("重新登录") || pHint.Contains(@"\u8bf7\u5148\u767b\u9646"));

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
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pData = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string str5 = CommFunc.GetIndexString(pResponsetext, "\"money\": \"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str5);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/controller/user/get/get_user_balance/{base.UserID}";

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, true, true, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/controller/lottery/{base.UserID}";

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
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "30001";
            }
            if (pType == ConfigurationStatus.LotteryType.JXYLFFC)
            {
                return "29";
            }
            if (pType == ConfigurationStatus.LotteryType.JXYL2FC)
            {
                return "10031";
            }
            if (pType == ConfigurationStatus.LotteryType.JXYL3FC)
            {
                return "10041";
            }
            if (pType == ConfigurationStatus.LotteryType.JXYL5FC)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.JNDSSC)
            {
                return "48";
            }
            if (pType == ConfigurationStatus.LotteryType.JXYLHG5FC)
            {
                return "10061";
            }
            if (pType == ConfigurationStatus.LotteryType.JXYLELSSSC)
            {
                return "10021";
            }
            if (pType == ConfigurationStatus.LotteryType.JXYLXXLSSC)
            {
                return "10011";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "9";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "10";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "20";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "22";
            }
            if (pType == ConfigurationStatus.LotteryType.JXYLFF11X5)
            {
                return "60";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "27";
            }
            if (pType == ConfigurationStatus.LotteryType.XYFTPK10)
            {
                str = "39";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/controller/lottery/{base.UserID}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLineString(string pResponseText)
        {
            string str = pResponseText;
            if (str.Contains("http://www.newfiretop9.com"))
            {
                str = "线路1";
            }
            return str;
        }

        public override string GetLoginLine() => 
            (this.GetLine() + "/login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/controller/lottery/{base.UserID}";

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
                    str = CommFunc.Join(pNumberList, "%2C").Replace("*", "");
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
                    str = CommFunc.Join(list, "%2C").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList);
                    }
                    else
                    {
                        str = CommFunc.Join(pNumberList, "%24");
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, "%24");
                }
                if (CommFunc.CheckPlayIsRXDS(playName))
                {
                    str = this.GetRXWZString(pRXWZ) + str;
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, "%24").Replace(" ", "%2C");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, "%2C");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                str = CommFunc.Join(pNumberList, "$").Replace(" ", ",");
            }
            else if (CommFunc.CheckPlayIsFS(playName))
            {
                list = new List<string>();
                for (num2 = 0; num2 < pNumberList.Count; num2++)
                {
                    string pStr = pNumberList[num2];
                    str2 = CommFunc.Join(CommFunc.ConvertPK10CodeToBets(CommFunc.SplitString(pStr, " ", -1), -1), ",");
                    list.Add(str2);
                }
                str = CommFunc.Join(list, "-").Replace("*", "");
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
                list = new List<string>();
                int num3 = (playName == "猜冠军猜冠军") ? 1 : 10;
                pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
                for (num2 = 0; num2 < num3; num2++)
                {
                    str2 = "*";
                    if (num2 == num)
                    {
                        str2 = CommFunc.Join(pNumberList, ",");
                    }
                    list.Add(str2);
                }
                str = CommFunc.Join(list, "-").Replace("*", "");
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
                if (playName == "任三直选单式")
                {
                    return "130001";
                }
                if (playName == "任三直选复式")
                {
                    return "130002";
                }
                if (playName == "任三组三复式")
                {
                    return "130042";
                }
                if (playName == "任三组三单式")
                {
                    return "130041";
                }
                if (playName == "任三组六复式")
                {
                    return "130044";
                }
                if (playName == "任三组六单式")
                {
                    return "130043";
                }
                if (playName == "任二直选单式")
                {
                    return "120001";
                }
                if (playName == "任二直选复式")
                {
                    return "120002";
                }
                if (playName == "任四直选单式")
                {
                    return "140001";
                }
                if (playName == "任四直选复式")
                {
                    return "140002";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "9";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "45";
                }
                if (playName == "前二直选单式")
                {
                    return "320202";
                }
                if (playName == "任选复式一中一")
                {
                    return "310005";
                }
                if (playName == "任选复式二中二")
                {
                    return "42";
                }
                if (playName == "任选复式三中三")
                {
                    return "48";
                }
                if (playName == "任选复式四中四")
                {
                    return "50";
                }
                if (playName == "任选复式五中五")
                {
                    return "52";
                }
                if (playName == "任选单式一中一")
                {
                    return "310006";
                }
                if (playName == "任选单式二中二")
                {
                    return "320006";
                }
                if (playName == "任选单式三中三")
                {
                    return "330006";
                }
                if (playName == "任选单式四中四")
                {
                    return "340006";
                }
                if (playName == "任选单式五中五")
                {
                    str = "350006";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "400033";
                }
                if (playName == "猜前三复式")
                {
                    return "400013";
                }
                if (playName == "猜前二单式")
                {
                    return "400032";
                }
                if (playName == "猜前二复式")
                {
                    return "400012";
                }
                if (playName == "猜前四单式")
                {
                    return "400034";
                }
                if (playName == "猜前四复式")
                {
                    return "400014";
                }
                if (playName == "猜前五单式")
                {
                    return "400035";
                }
                if (playName == "猜前五复式")
                {
                    return "400015";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "400001";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "400041";
                }
            }
            return str;
        }

        public override string GetPTHint(string pResponseText)
        {
            string str = "";
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            str = CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"remark\": \"", "\"", 0));
            if (str == "")
            {
                str = CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"message\": \"", "\"", 0).Replace("!", ""));
            }
            return str;
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ) => 
            $"{CommFunc.Join(pRXWZ, ",")}@";

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.Prize == "")
                {
                    string pUrl = $"{this.GetLine()}/controller/lottery/{base.UserID}";
                    string indexLine = this.GetIndexLine();
                    string pResponsetext = "";
                    string pData = $"command=lottery_logon_request_transmit_v2&param=%7B%22user_id%22%3A%22{base.UserID}%22%2C%22lottery_id%22%3A%22{this.GetBetsLotteryID(pType)}%22%2C%22command_id%22%3A520%7D";
                    HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        string str5 = CommFunc.GetIndexString(pResponsetext, "\"rebate\": \"", "\"", 0);
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

        public string GetWebVerifyCode(string pVerifyCodeFile, string pID)
        {
            string pVerifyCode = "";
            try
            {
                string str2 = $"/valid_code?t={DateTime.Now.ToOADate()}&user_name={pID}";
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
            string webVerifyCode = this.GetWebVerifyCode(AutoBetsWindow.VerifyCodeFile, pID);
            if (webVerifyCode != "")
            {
                string loginLine = this.GetLoginLine();
                string pUrl = this.GetLoginLine();
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"user_name={pID}&password={str5}&valid_code={webVerifyCode}&command=login";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("\"success\": 1");
                if (!flag)
                {
                    pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "\"message\": \"", "\"", 0));
                    if ((pHint == "异常登陆") || pHint.Contains("验证码"))
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
            return pResponsetext.Contains("锦绣娱乐");
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

