namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class JXIN : PTBase
    {
        public override bool BetsMain(ConfigurationStatus.SCPlan plan, ref string pHint)
        {
            bool flag = false;
            try
            {
                string betsLine = this.GetBetsLine(plan.Type);
                string pReferer = $"{this.GetLine()}/game/index?id={this.GetBetsLotteryID(plan.Type)}";
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
                            string prize = base.Prize;
                            string format = "lottery={0}&no={1}&flevel_modes={2}&total_price={3}&total_zhushu={4}&flevel_fd=&data[0][multiple]={5}&data[0][num]={6}&data[0][pattern]={7}&data[0][play]={8}&data[0][price]={3}&data[0][rxw]={11}&data[0][zhushu]={4}&data[0][peilv]=&main_wf=&wf=&token={9}&user_id={10}&sign=";
                            format = string.Format(format, new object[] { this.GetPTLotteryName(plan.Type), this.GetBetsExpect(plan.CurrentExpect, ""), prize, plan.AutoTotalMoney(str4, true), num, Convert.ToInt32(plan.AutoTimes(str4, true)), this.GetNumberList1(pTNumberList, plan.Play, null), plan.Money, this.GetPlayMethodID(plan.Type, plan.Play), base.Token, base.VerifyCodeToken, this.GetRXWZString(plan.RXWZ) });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, pReferer, base.BetsTime3, "UTF-8", true);
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
            pHint.Contains("\"code\":203");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            ((pResponseText == "{\"code\":1}") || (pResponseText == "投注成功"));

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
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                string pData = $"token={base.Token}&user_id={base.VerifyCodeToken}&sign=";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"balance\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
                if (base.Prize == "")
                {
                    base.Rebate = CommFunc.GetIndexString(pResponsetext, "\"flevel_fd\":\"", "\"", 0);
                    double num = 1800.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0);
                    base.Prize = num.ToString();
                    if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                    {
                        base.Prize = (Convert.ToDouble(base.Prize) - 10.0).ToString();
                    }
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/api/Account/GetUserInfo");

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if (((((pType == ConfigurationStatus.LotteryType.GD11X5) || (pType == ConfigurationStatus.LotteryType.SD11X5)) || ((pType == ConfigurationStatus.LotteryType.SH11X5) || (pType == ConfigurationStatus.LotteryType.JX11X5))) || (pType == ConfigurationStatus.LotteryType.JS11X5)) || (pType == ConfigurationStatus.LotteryType.LN11X5))
            {
                if (pIsBets)
                {
                    str = str.Remove(8, 1);
                }
                return str;
            }
            if ((pType == ConfigurationStatus.LotteryType.HLJSSC) && pIsBets)
            {
                str = "0" + str;
            }
            return str;
        }

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, true, false, false);
            return this.GetAppExpect(AppInfo.Current.Lottery.Type, str2, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/api/Play/BetIng");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/api/Common/GetRecentLotteryInfo");

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/#/login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            (this.GetLine() + "/");

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
                    str = CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                    str = CommFunc.Join(list, ",").Replace("*", "");
                }
                else if (CommFunc.CheckPlayIsLH(playName))
                {
                    str = CommFunc.Join(pNumberList);
                }
                else if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        str = CommFunc.Join(pNumberList);
                    }
                }
                else
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, " ");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                    if (num >= 5)
                    {
                        num -= 5;
                    }
                    list = new List<string>();
                    int num3 = (playName == "猜冠军猜冠军") ? 1 : 5;
                    for (num2 = 0; num2 < num3; num2++)
                    {
                        str2 = "*";
                        if (num2 == num)
                        {
                            str2 = CommFunc.Join(pNumberList, " ");
                        }
                        list.Add(str2);
                    }
                    str = CommFunc.Join(list, ",").Replace("*", "");
                }
            }
            return HttpUtility.UrlEncode(str);
        }

        public override string GetOpenTime() => 
            (this.GetLine() + "/api/Play/Bet");

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "3xq3ds";
                }
                if (playName == "前三直选复式")
                {
                    return "3xq3fs";
                }
                if (playName == "前三组三复式")
                {
                    return "3xq3z3";
                }
                if (playName == "前三组六复式")
                {
                    return "3xq3z6";
                }
                if (playName == "后三直选单式")
                {
                    return "3xh3ds";
                }
                if (playName == "后三直选复式")
                {
                    return "3xh3fs";
                }
                if (playName == "后三组三复式")
                {
                    return "3xh3z3";
                }
                if (playName == "后三组六复式")
                {
                    return "3xh3z6";
                }
                if (playName == "中三直选单式")
                {
                    return "3xz3ds";
                }
                if (playName == "中三直选复式")
                {
                    return "3xz3fs";
                }
                if (playName == "中三组三复式")
                {
                    return "3xz3z3";
                }
                if (playName == "中三组六复式")
                {
                    return "3xz3z6";
                }
                if (playName == "前二直选单式")
                {
                    return "2xq2ds";
                }
                if (playName == "前二直选复式")
                {
                    return "2xq2fs";
                }
                if (playName == "后二直选单式")
                {
                    return "2xh2ds";
                }
                if (playName == "后二直选复式")
                {
                    return "2xh2fs";
                }
                if (playName == "前四直选单式")
                {
                    return "4xq4ds";
                }
                if (playName == "前四直选复式")
                {
                    return "4xq4fs";
                }
                if (playName == "后四直选单式")
                {
                    return "4xh4ds";
                }
                if (playName == "后四直选复式")
                {
                    return "4xh4fs";
                }
                if (playName == "五星直选单式")
                {
                    return "5xds";
                }
                if (playName == "五星直选复式")
                {
                    return "5xfs";
                }
                if (playName == "任三直选单式")
                {
                    return "r3ds";
                }
                if (playName == "任三直选复式")
                {
                    return "r3fs";
                }
                if (playName == "任三组三复式")
                {
                    return "r3zx3";
                }
                if (playName == "任三组六复式")
                {
                    return "r3zx6";
                }
                if (playName == "任二直选单式")
                {
                    return "r2ds";
                }
                if (playName == "任二直选复式")
                {
                    return "r2fs";
                }
                if (playName == "任四直选单式")
                {
                    return "r4ds";
                }
                if (playName == "任四直选复式")
                {
                    return "r4fs";
                }
                if (playName.Contains("定位胆"))
                {
                    return "5xdwd";
                }
                if (playName == "龙虎万千")
                {
                    return "lhwq";
                }
                if (playName == "龙虎万百")
                {
                    return "lhwb";
                }
                if (playName == "龙虎万十")
                {
                    return "lhws";
                }
                if (playName == "龙虎万个")
                {
                    return "lhwg";
                }
                if (playName == "龙虎千百")
                {
                    return "lhqb";
                }
                if (playName == "龙虎千十")
                {
                    return "lhqs";
                }
                if (playName == "龙虎千个")
                {
                    return "lhqg";
                }
                if (playName == "龙虎百十")
                {
                    return "lhbs";
                }
                if (playName == "龙虎百个")
                {
                    return "lhbg";
                }
                if (playName == "龙虎十个")
                {
                    str = "lhsg";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "q3ds";
                }
                if (playName == "前二直选单式")
                {
                    return "q2ds";
                }
                if (playName == "任选复式一中一")
                {
                    return "";
                }
                if (playName == "任选复式二中二")
                {
                    return "fsrx2z2";
                }
                if (playName == "任选复式三中三")
                {
                    return "fsrx3z3";
                }
                if (playName == "任选复式四中四")
                {
                    return "fsrx4z4";
                }
                if (playName == "任选复式五中五")
                {
                    return "fsrx5z5";
                }
                if (playName == "任选单式一中一")
                {
                    return "";
                }
                if (playName == "任选单式二中二")
                {
                    return "dsrx2z2";
                }
                if (playName == "任选单式三中三")
                {
                    return "dsrx3z3";
                }
                if (playName == "任选单式四中四")
                {
                    return "dsrx4z4";
                }
                if (playName == "任选单式五中五")
                {
                    str = "dsrx5z5";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "cq3mds";
                }
                if (playName == "猜前三复式")
                {
                    return "cq3mfs";
                }
                if (playName == "猜前二单式")
                {
                    return "cgyjds";
                }
                if (playName == "猜前二复式")
                {
                    return "cgyjfs";
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
                    return "cgjfs";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "dwd610" : "dwd15";
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
            return CommFunc.GetIndexString(pResponseText, "\"code\":", "}", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                str = "cqssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                str = "xjssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.HLJSSC)
            {
                str = "hljssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                str = "txffc";
            }
            else if (pType == ConfigurationStatus.LotteryType.JXIN2FC)
            {
                str = "wh2fc";
            }
            else if (pType == ConfigurationStatus.LotteryType.JXINFFC)
            {
                str = "wh1fc";
            }
            else if (pType == ConfigurationStatus.LotteryType.JXIN5FC)
            {
                str = "wh5fc";
            }
            else if (pType == ConfigurationStatus.LotteryType.JXINFLP15C)
            {
                str = "flbssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.JXINMSK15C)
            {
                str = "mskssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.JXINJLP15C)
            {
                str = "jlpssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.JXINDB15C)
            {
                str = "dbssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.JXINDJSSC)
            {
                str = "djssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.JXINXJPSSC)
            {
                str = "xjpssc";
            }
            else if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                str = "gd11x5";
            }
            else if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                str = "sd11x5";
            }
            else if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                str = "sh11x5";
            }
            else if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                str = "jx11x5";
            }
            else if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "bjpk10";
            }
            return str.ToUpper();
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/api/User/Logout");

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
            for (int i = 0; i < 5; i++)
            {
                string item = pRXWZ.Contains(i) ? "1" : "0";
                pList.Add(item);
            }
            return CommFunc.Join(pList);
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string openTime = this.GetOpenTime();
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pData = $"lottery={this.GetPTLotteryName(pType)}&token={base.Token}&user_id={base.VerifyCodeToken}&sign=";
                HttpHelper.GetResponse(ref pResponsetext, openTime, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"no\":\"", "\"", 0);
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
                if (base.VerifyCodeToken == "")
                {
                    return pVerifyCode;
                }
                string str2 = $"/api/user/code?user_id={base.VerifyCodeToken}?v={DateTime.Now.ToOADate()}";
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
            string loginLine = this.GetLoginLine();
            string str3 = HttpUtility.UrlEncode(pW);
            string pUrl = this.GetLine() + "/api/User/Login";
            string pResponsetext = "";
            string pData = $"user_name={pID}&password={str3}&code={webVerifyCode}&sign=";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
            flag = pResponsetext.Contains("\"code\":1");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "\"code\":", ",", 0);
                if (pHint == "")
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"code\":", "}", 0);
                }
                if (pHint == "6")
                {
                    pHint = "输入的账号密码不正确！";
                }
                else if (pHint == "2")
                {
                    pHint = "输入的账号不存在！";
                }
                else if (pHint == "8")
                {
                    pHint = "";
                    base.VerifyCodeToken = CommFunc.GetIndexString(pResponsetext, "user_id=", "\"", 0);
                    return this.InputWeb(pID, pW, ref pHint);
                }
                return flag;
            }
            base.Token = CommFunc.GetIndexString(pResponsetext, "\"token\":\"", "\"", 0);
            base.VerifyCodeToken = CommFunc.GetIndexString(pResponsetext, "\"user_id\":\"", "\"", 0);
            return flag;
        }

        public bool LoginWeb()
        {
            base.VerifyCodeToken = "";
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, base.BetsTime2, "UTF-8", true);
            return pResponsetext.Contains("聚鑫娱乐");
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            string pData = $"token={base.Token}&user_id={base.VerifyCodeToken}&sign=";
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

