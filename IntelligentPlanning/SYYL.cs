namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class SYYL : PTBase
    {
        private string BetsExpectIssue = "";
        private string BetsExpectSubissue = "";
        private string BetsExpectYear = "";
        private string HttpString1 = "";
        private string HttpString2 = "";
        private string HttpString3 = "";

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
                            string prize = base.Prize;
                            List<string> list2 = CommFunc.SplitString(this.GetPlayMethodID(plan.Type, plan.Play), "-", -1);
                            string format = "betData=%7B%22data%22%3A%5B%7B%22gamegroup%22%3A%22{0}%22%2C%22betmode%22%3A%22{1}%22%2C%22location%22%3A%22{2}%22%2C%22content%22%3A%22{3}%22%2C%22multiple%22%3A%22{4}%22%2C%22credit%22%3A%22{5}%22%7D%5D%7D&txtCBID={6}&txtPrizelevel={7}&txtYear={8}&txtIssue={9}&txtSubissue={10}";
                            format = string.Format(format, new object[] { list2[0], list2[1], this.GetRXWZString(plan.RXWZ), this.GetNumberList1(pTNumberList, plan.Play, null), Convert.ToInt32(plan.AutoTimes(str4, true)), plan.Money, this.GetPTLotteryName(plan.Type), prize, this.BetsExpectYear, this.BetsExpectIssue, this.BetsExpectSubissue });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, base.BetsTime3, "UTF-8", true);
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
            pHint.Contains("sessiontimeout");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"showMsg\":\"下注成功\"") || (pResponseText == "投注成功"));

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

        public string ConvertLine1(string pLine) => 
            pLine.Replace(this.HttpString2, this.HttpString3);

        public string ConvertLine2(string pLine) => 
            pLine.Replace(this.HttpString3, this.HttpString2);

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"quota\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/CCGLCBUSRAPI/API/getUsrQuota.jsp?txtTime={DateTime.Now.ToOADate()}";

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if ((pType == ConfigurationStatus.LotteryType.CQSSC) || (pType == ConfigurationStatus.LotteryType.TJSSC))
            {
                if (pIsBets)
                {
                    return str.Replace("-0", "-");
                }
                if (str.Length == 11)
                {
                    str = str.Insert(9, "0");
                }
            }
            return str;
        }

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
            return this.GetAppExpect(AppInfo.Current.Lottery.Type, str2, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/CCGLCBUSRAPI/OrderAPI/addBet.jsp?txtTime={DateTime.Now.ToOADate()}";

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/CCGLCBUSRAPI/API/getHistoryResult.jsp?txtCBID={this.GetPTLotteryName(pType)}&txtPageSize=10&txtTime={DateTime.Now.ToOADate()}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/CCLHUSR/VioletWood/desktop/index.page");

        public override string GetLine()
        {
            if (base.LineIndex == -1)
            {
                return "";
            }
            string pLine = base.LineList[base.LineIndex];
            if (base.PTLoginStatus)
            {
                pLine = this.ConvertLine1(pLine);
            }
            return pLine;
        }

        public override string GetLoginLine() => 
            (this.GetLine() + "/CCLHUSR/VioletWood/desktop/login.page");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            (this.GetLine() + "/CCGLCBUSR/D01/");

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
            List<string> list;
            int num2;
            string str2;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    return CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                    return CommFunc.Join(list, ",").Replace("*", "");
                }
                if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        return CommFunc.Join(pNumberList);
                    }
                    return CommFunc.Join(pNumberList, ",");
                }
                return CommFunc.Join(pNumberList, ",");
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    str = CommFunc.Join(pNumberList, ",");
                }
                else if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, " ");
                }
                return HttpUtility.UrlEncode(str);
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
                return CommFunc.Join(pNumberList, ",").Replace("*", "");
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
            return CommFunc.Join(list, ",").Replace("*", "");
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "F3D-SM";
                }
                if (playName == "前三直选复式")
                {
                    return "F3D-DM";
                }
                if (playName == "前三组三复式")
                {
                    return "F3G3-DM";
                }
                if (playName == "前三组三单式")
                {
                    return "F3G3-SM";
                }
                if (playName == "前三组六复式")
                {
                    return "F3G6-DM";
                }
                if (playName == "前三组六单式")
                {
                    return "F3G6-SM";
                }
                if (playName == "后三直选单式")
                {
                    return "L3D-SM";
                }
                if (playName == "后三直选复式")
                {
                    return "L3D-DM";
                }
                if (playName == "后三组三复式")
                {
                    return "L3G3-DM";
                }
                if (playName == "后三组三单式")
                {
                    return "L3G3-SM";
                }
                if (playName == "后三组六复式")
                {
                    return "L3G6-DM";
                }
                if (playName == "后三组六单式")
                {
                    return "L3G6-SM";
                }
                if (playName == "中三直选单式")
                {
                    return "M3D-SM";
                }
                if (playName == "中三直选复式")
                {
                    return "M3D-DM";
                }
                if (playName == "中三组三复式")
                {
                    return "M3G3-DM";
                }
                if (playName == "中三组三单式")
                {
                    return "M3G3-SM";
                }
                if (playName == "中三组六复式")
                {
                    return "M3G6-DM";
                }
                if (playName == "中三组六单式")
                {
                    return "M3G6-SM";
                }
                if (playName == "前二直选单式")
                {
                    return "F2D-SM";
                }
                if (playName == "前二直选复式")
                {
                    return "F2D-DM";
                }
                if (playName == "后二直选单式")
                {
                    return "L2D-SM";
                }
                if (playName == "后二直选复式")
                {
                    return "L2D-DM";
                }
                if (playName == "前四直选单式")
                {
                    return "F4D-SM";
                }
                if (playName == "前四直选复式")
                {
                    return "F4D-DM";
                }
                if (playName == "后四直选单式")
                {
                    return "L4D-SM";
                }
                if (playName == "后四直选复式")
                {
                    return "L4D-DM";
                }
                if (playName == "五星直选单式")
                {
                    return "5SD-SM";
                }
                if (playName == "五星直选复式")
                {
                    return "5SD-DM";
                }
                if (playName == "任三直选单式")
                {
                    return "ANY3D-SM";
                }
                if (playName == "任三直选复式")
                {
                    return "ANY3D-DM";
                }
                if (playName == "任三组三复式")
                {
                    return "ANY3G3-DM";
                }
                if (playName == "任三组三单式")
                {
                    return "ANY3G3-SM";
                }
                if (playName == "任三组六复式")
                {
                    return "ANY3G6-DM";
                }
                if (playName == "任三组六单式")
                {
                    return "ANY3G6-SM";
                }
                if (playName == "任二直选单式")
                {
                    return "ANY2D-SM";
                }
                if (playName == "任二直选复式")
                {
                    return "ANY2D-DM";
                }
                if (playName == "任四直选单式")
                {
                    return "ANY4D-SM";
                }
                if (playName == "任四直选复式")
                {
                    return "ANY4D-DM";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "FIXED-DM";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "F3D-SM";
                }
                if (playName == "前二直选单式")
                {
                    return "F2D-SM";
                }
                if (playName == "任选复式一中一")
                {
                    return "ANY1OF1-DM";
                }
                if (playName == "任选复式二中二")
                {
                    return "ANY2OF2-DM";
                }
                if (playName == "任选复式三中三")
                {
                    return "ANY3OF3-DM";
                }
                if (playName == "任选复式四中四")
                {
                    return "ANY4OF4-DM";
                }
                if (playName == "任选复式五中五")
                {
                    return "ANY5OF5-DM";
                }
                if (playName == "任选单式一中一")
                {
                    return "ANY1OF1-SM";
                }
                if (playName == "任选单式二中二")
                {
                    return "ANY2OF2-SM";
                }
                if (playName == "任选单式三中三")
                {
                    return "ANY3OF3-SM";
                }
                if (playName == "任选单式四中四")
                {
                    return "ANY4OF4-SM";
                }
                if (playName == "任选单式五中五")
                {
                    str = "ANY5OF5-SM";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "F3D-SM";
                }
                if (playName == "猜前三复式")
                {
                    return "F3D-DM";
                }
                if (playName == "猜前二单式")
                {
                    return "F2D-SM";
                }
                if (playName == "猜前二复式")
                {
                    return "F2D-DM";
                }
                if (playName == "猜前四单式")
                {
                    return "F4D-SM";
                }
                if (playName == "猜前四复式")
                {
                    return "F4D-DM";
                }
                if (playName == "猜前五单式")
                {
                    return "F5D-SM";
                }
                if (playName == "猜前五复式")
                {
                    return "F5D-DM";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "F1D-DM";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "FIXED-DM";
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
            return CommFunc.GetIndexString(pResponseText, "\"showMsg\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "CBCQSSC";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "CBTJSSC";
            }
            if (pType == ConfigurationStatus.LotteryType.SYYLFFC)
            {
                return "CBSY1MSSC";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "CBGD11X5";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "CBSD11X5";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "CBJX11X5";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "CBBJPK10";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/CCLHUSR/VioletWood/desktop/logout.page");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            string str = "";
            if ((pRXWZ == null) || (pRXWZ.Count <= 0))
            {
                return str;
            }
            List<int> pList = new List<int>();
            for (int i = 0; i < pRXWZ.Count; i++)
            {
                pList.Add(5 - pRXWZ[i]);
            }
            return CommFunc.Join(pList, ",");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string str;
                string lotteryLine;
                string str3;
                if (base.Prize == "")
                {
                    str = this.ConvertLine2($"{this.GetLine()}/CCLHUSR/VioletWood/desktop/userSetting.block?_={DateTime.Now.ToOADate()}");
                    lotteryLine = this.ConvertLine2(this.GetIndexLine());
                    str3 = "";
                    HttpHelper.GetResponse(ref str3, str, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                    if (str3 != "")
                    {
                        base.Prize = CommFunc.GetIndexString(str3, "&nbsp;(", ")", str3.IndexOf("彩票返点"));
                    }
                }
                str = $"{this.GetLine()}/CCGLCBUSRAPI/API/getTraceIssue.jsp?txtCBID={this.GetPTLotteryName(pType)}&txtTime={DateTime.Now.ToOADate()}";
                lotteryLine = this.GetLotteryLine(pType, false);
                str3 = "";
                HttpHelper.GetResponse(ref str3, str, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                if (str3 != "")
                {
                    base.Expect = CommFunc.GetIndexString(str3, "\"officialissue\":\"", "\"", 0);
                    base.Expect = this.GetAppExpect(pType, base.Expect, false);
                    base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
                    this.BetsExpectYear = CommFunc.GetIndexString(str3, "\"year\":\"", "\"", 0);
                    this.BetsExpectIssue = CommFunc.GetIndexString(str3, "\"issue\":\"", "\"", 0);
                    this.BetsExpectSubissue = CommFunc.GetIndexString(str3, "\"subissue\":\"", "\"", 0);
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
                string str2 = $"/CCLHUSR/validateCodeServlet.jpg?t={DateTime.Now.ToOADate().ToString()}";
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

        public bool InputIDWeb(string pID, ref string pHint)
        {
            bool flag = false;
            string webVerifyCode = this.GetWebVerifyCode(AutoBetsWindow.VerifyCodeFile);
            if (webVerifyCode != "")
            {
                string loginLine = this.GetLoginLine();
                string pUrl = this.GetLine() + "/CCLHUSR/VioletWood/desktop/checkAccount.page";
                string pResponsetext = "";
                string pData = $"txtAccount={pID}&txtValidateCode={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("showMessage(\"密碼不可空白\")");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "showMessage(\"", "\"", pResponsetext.IndexOf("ready(function()"));
                    if (pHint.Contains("验证码"))
                    {
                        pHint = "";
                        return this.InputIDWeb(pID, ref pHint);
                    }
                }
            }
            return flag;
        }

        public bool InputPWWeb(string pID, string pW, ref string pHint)
        {
            string pReferer = this.GetLine() + "/CCLHUSR/VioletWood/desktop/hello.page";
            string pUrl = this.GetLine() + "/CCLHUSR/VioletWood/desktop/checkPassword.page";
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string pData = $"txtPassword={str4}";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, pReferer, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("title:'交易协议',yes:'确定',no:'取消'");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "showMessage(\"", "\"", pResponsetext.IndexOf("ready(function()"));
                return flag;
            }
            HttpHelper.ConvertCookie(this.GetLine() + "/CCLHUSR", this.GetLine().Replace(this.HttpString1, ""));
            return flag;
        }

        public bool LoginMainIndexWeb1()
        {
            string pReferer = this.GetLine() + "/CCLHUSR/VioletWood/desktop/agreement.page";
            string pUrl = this.GetLine() + "/CCLHUSR/VioletWood/desktop/setAgreement.page";
            string pResponsetext = "";
            string pData = "txtAgreementFlag=Y";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext.Contains("彩票大厅");
        }

        public bool LoginMainIndexWeb2()
        {
            string indexLine = this.GetIndexLine();
            string pUrl = $"{this.GetLine()}/CCLHUSR/VioletWood/desktop/loginProduct.page?txtProductId=LCB&t={DateTime.Now.ToOADate()}";
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("input name=loginKey type=hidden value");
            if (flag)
            {
                flag = this.LoginMainIndexWeb3(pResponsetext);
            }
            return flag;
        }

        public bool LoginMainIndexWeb3(string pResponseText)
        {
            string str = this.ConvertLine1(this.GetLine());
            List<string> list = new List<string>();
            pResponseText = CommFunc.GetIndexString(pResponseText, "<input name", "</form>", 0);
            List<string> list2 = CommFunc.SplitString(pResponseText, "><", -1);
            for (int i = 0; i < list2.Count; i++)
            {
                string pStr = list2[i];
                string item = HttpUtility.UrlEncode(CommFunc.GetIndexString(pStr, "value=\"", "\"", 0));
                list.Add(item);
            }
            string pReferer = $"{this.GetLine()}/CCLHUSR/VioletWood/desktop/loginProduct.page?txtProductId=LCB&t={DateTime.Now.ToOADate()}";
            string pUrl = str + "/CCGLCBUSRAPI/checkLogin.jsp";
            string pResponsetext = "";
            string pData = $"vendorid={list[0]}&uid={list[1]}&lang={list[2]}&timeZone={list[3]}&loginKey={list[4]}&passAgreementPage={list[5]}&gameKey={list[6]}&openInInnerFrame={list[7]}&template={list[8]}&device={list[9]}&lobbyUrl={list[10]}";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, pReferer, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("plugin/jquery/jquery.min.js");
            if (flag)
            {
                HttpHelper.ConvertCookie(str.Replace(this.HttpString1, this.HttpString1 + "www."), str.Replace(this.HttpString1, ""));
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext.Contains("数亿娱乐");
        }

        public override void QuitPT()
        {
            string pUrl = this.ConvertLine2(this.GetQuitPTLine());
            string pReferer = this.ConvertLine2(this.GetIndexLine());
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
        }

        public override bool WebLoginMain(string pID, string pW, ref string pHint)
        {
            this.BetsExpectYear = this.BetsExpectIssue = this.BetsExpectSubissue = "";
            this.HttpString1 = this.GetLine();
            this.HttpString1 = this.HttpString1.Split(new char[] { '/' })[0] + "//";
            this.HttpString2 = this.HttpString1;
            string[] strArray = this.GetLine().Split(new char[] { '.' });
            if (strArray.Length == 3)
            {
                string str = strArray[0].Replace(this.HttpString1, "") + ".";
                this.HttpString2 = this.HttpString2 + str;
            }
            this.HttpString3 = this.HttpString1 + "lcb.";
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
            Thread.Sleep(0x3e8);
            if (!this.LoginMainIndexWeb1())
            {
                return false;
            }
            if (!this.LoginMainIndexWeb2())
            {
                return false;
            }
            return true;
        }
    }
}

