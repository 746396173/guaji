namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class LMH : PTBase
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
                            int num2 = Convert.ToInt32(plan.AutoTimes(str4, true));
                            string str6 = ((Convert.ToDouble(base.Prize) / Convert.ToDouble(Math.Pow(10.0, (double) (plan.Unit - 1)))) * num2).ToString();
                            string format = "[(\"id\":\"{0}\",\"number\":\"{1}\",\"mode\":\"{2}\",\"times\":{3},\"rebate\":{4},\"reward\":{5},\"bet\":{6},\"money\":{7})]";
                            format = string.Format(format, new object[] { this.GetPlayMethodID(plan.Type, plan.Play), this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), plan.UnitZWString, num2, "0", str6, num, plan.AutoTotalMoney(str4, true) }).Replace("(", "{").Replace(")", "}");
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, pReferer, AppInfo.PTInfo.BetsTime3, "UTF-8", true);
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
            pHint.Contains("登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"success\" : 1") || (pResponseText == "投注成功"));

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
            List<string> list = CommFunc.SplitString(pResponseText, "},{", -1);
            for (int i = 0; i < list.Count; i++)
            {
                string pStr = list[i];
                string str2 = CommFunc.GetIndexString(pStr, "\"GroupName\":\"", "\"", 0);
                string str3 = CommFunc.GetIndexString(pStr, "\"LabelName\":\"", "\"", 0);
                string str4 = CommFunc.GetIndexString(pStr, "\"PlayName\":\"", "\"", 0);
                string str5 = $"{str2}-{str3}-{str4}";
                string str6 = CommFunc.GetIndexString(pStr, "\"ID\":\"", "\"", 0);
                base.PlayMethodDic[str5] = str6;
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
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"Money\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
                if (base.Prize == "")
                {
                    base.Prize = CommFunc.GetIndexString(pResponsetext, "\"Rebate\":\"", "\"", 0);
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/handler/user/info/get");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/handler/user/lottery/save");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/handler/game/lottery/resultnumber");

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            (this.GetLine() + "/handler/user/lottery/player");

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            int num;
            int num2;
            List<string> list2;
            string str3;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                List<string> list;
                string str2;
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = pNumberList[num];
                        list.Add(CommFunc.Join(str2, ",", -1));
                    }
                    str = CommFunc.Join(list, "|").Replace("*", "");
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
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = pNumberList[num];
                        list.Add(CommFunc.Join(str2, ",", -1));
                    }
                    str = CommFunc.Join(list, "|");
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
                    return CommFunc.Join(pNumberList, "|").Replace(" ", ",");
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
                return CommFunc.Join(pNumberList, "|").Replace(" ", ",");
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                return CommFunc.Join(pNumberList, "|").Replace(" ", ",").Replace("*", "");
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
            return CommFunc.Join(list2, "|").Replace("*", "");
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
                    return "前三-直选-单式";
                }
                if (playName == "前三直选复式")
                {
                    return "前三-直选-复式";
                }
                if (playName == "前三组三复式")
                {
                    return "前三-组选-组三";
                }
                if (playName == "前三组六复式")
                {
                    return "前三-组选-组六";
                }
                if (playName == "后三直选单式")
                {
                    return "后三-直选-单式";
                }
                if (playName == "后三直选复式")
                {
                    return "后三-直选-复式";
                }
                if (playName == "后三组三复式")
                {
                    return "后三-组选-组三";
                }
                if (playName == "后三组六复式")
                {
                    return "后三-组选-组六";
                }
                if (playName == "中三直选单式")
                {
                    return "中三-直选-单式";
                }
                if (playName == "中三直选复式")
                {
                    return "中三-直选-复式";
                }
                if (playName == "中三组三复式")
                {
                    return "中三-组选-组三";
                }
                if (playName == "中三组六复式")
                {
                    return "中三-组选-组六";
                }
                if (playName == "前二直选单式")
                {
                    return "前二-直选-单式";
                }
                if (playName == "前二直选复式")
                {
                    return "前二-直选-复式";
                }
                if (playName == "后二直选单式")
                {
                    return "后二-直选-单式";
                }
                if (playName == "后二直选复式")
                {
                    return "后二-直选-复式";
                }
                if (playName == "后四直选单式")
                {
                    return "四星-直选-单式";
                }
                if (playName == "后四直选复式")
                {
                    return "四星-直选-复式";
                }
                if (playName == "五星直选单式")
                {
                    return "五星-直选-单式";
                }
                if (playName == "五星直选复式")
                {
                    return "五星-直选-复式";
                }
                if (playName == "任三直选单式")
                {
                    return "任选-任三-单式";
                }
                if (playName == "任三直选复式")
                {
                    return "";
                }
                if (playName == "任三组三复式")
                {
                    return "任选-任三-组三";
                }
                if (playName == "任三组六复式")
                {
                    return "任选-任三-组六";
                }
                if (playName == "任二直选单式")
                {
                    return "任选-任二-单式";
                }
                if (playName == "任二直选复式")
                {
                    return "";
                }
                if (playName == "任四直选单式")
                {
                    return "任选-任四-单式";
                }
                if (playName == "任四直选复式")
                {
                    return "";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "胆码-定位胆-定位胆";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "前三码-直选-单式";
                }
                if (playName == "前二直选单式")
                {
                    return "前二码-直选-单式";
                }
                if (playName == "任选复式一中一")
                {
                    return "任选-复式-一中一";
                }
                if (playName == "任选复式二中二")
                {
                    return "任选-复式-二中二";
                }
                if (playName == "任选复式三中三")
                {
                    return "任选-复式-三中三";
                }
                if (playName == "任选复式四中四")
                {
                    return "任选-复式-四中四";
                }
                if (playName == "任选复式五中五")
                {
                    return "任选-复式-五中五";
                }
                if (playName == "任选单式一中一")
                {
                    return "";
                }
                if (playName == "任选单式二中二")
                {
                    return "任选-单式-二中二";
                }
                if (playName == "任选单式三中三")
                {
                    return "任选-单式-三中三";
                }
                if (playName == "任选单式四中四")
                {
                    return "任选-单式-四中四";
                }
                if (playName == "任选单式五中五")
                {
                    str = "任选-单式-五中五";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "排名竞猜-单式-前三";
                }
                if (playName == "猜前三复式")
                {
                    return "排名竞猜-排名-前三";
                }
                if (playName == "猜前二单式")
                {
                    return "排名竞猜-单式-前二";
                }
                if (playName == "猜前二复式")
                {
                    return "排名竞猜-排名-冠亚军";
                }
                if (playName == "猜前四单式")
                {
                    return "排名竞猜-单式-前四";
                }
                if (playName == "猜前四复式")
                {
                    return "";
                }
                if (playName == "猜前五单式")
                {
                    return "排名竞猜-单式-前五";
                }
                if (playName == "猜前五复式")
                {
                    return "";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "排名竞猜-排名-冠军";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "排名竞猜-定位胆-后五" : "排名竞猜-定位胆-前五";
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
            return CommFunc.GetIndexString(pResponseText, "\"msg\" : \"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "ChungKing";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "Sinkiang";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "Tencent";
            }
            if (pType == ConfigurationStatus.LotteryType.LMHHGSSC)
            {
                return "Minute15";
            }
            if (pType == ConfigurationStatus.LotteryType.LMHDJSSC)
            {
                return "Tokyo15";
            }
            if (pType == ConfigurationStatus.LotteryType.LMHXY45M)
            {
                return "Second45";
            }
            if (pType == ConfigurationStatus.LotteryType.VRSSC)
            {
                return "VRVenus";
            }
            if (pType == ConfigurationStatus.LotteryType.VRHXSSC)
            {
                return "VRMars";
            }
            if (pType == ConfigurationStatus.LotteryType.VR3FC)
            {
                return "VR3";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "SD11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "GD11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.SXR11X5)
            {
                return "SX11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.JS11X5)
            {
                return "JS11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "PK10";
            }
            if (pType == ConfigurationStatus.LotteryType.XGSM)
            {
                return "HKSM";
            }
            if (pType == ConfigurationStatus.LotteryType.VRPK10)
            {
                return "VRBoat";
            }
            if (pType == ConfigurationStatus.LotteryType.XYFTPK10)
            {
                str = "Boat";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/handler/user/account/logout");

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
            return $"{CommFunc.Join(pList)}*";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string str;
                string lotteryLine;
                string str3;
                string str4;
                if (base.PlayMethodDic.Count == 0)
                {
                    str = this.GetLotteryLine(pType, false);
                    lotteryLine = this.GetLotteryLine(pType, false);
                    str3 = "";
                    str4 = $"Game={this.GetPTLotteryName(pType)}";
                    HttpHelper.GetResponse(ref str3, str, "POST", str4, lotteryLine, 0x2710, "UTF-8", true);
                    str3 = CommFunc.GetIndexString(str3, "[{", "}]", 0);
                    if (str3 != "")
                    {
                        this.CountPrizeDic(str3);
                    }
                }
                str = this.GetLine() + "/handler/game/lottery/index";
                lotteryLine = this.GetLotteryLine(pType, false);
                str3 = "";
                str4 = $"Game={this.GetPTLotteryName(pType)}";
                HttpHelper.GetResponse(ref str3, str, "POST", str4, lotteryLine, 0x2710, "UTF-8", true);
                if (str3 != "")
                {
                    base.Expect = CommFunc.GetIndexString(str3, "\"BetIndex\":\"", "\"", 0);
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
                string str2 = $"/handler/ValidateCode.png?name=login&r={DateTime.Now.ToOADate()}";
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
                string pUrl = this.GetLine() + "/handler/user/account/login";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"UserName={pID}&Password={str5}&Code={webVerifyCode}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, base.BetsTime2, "UTF-8", true);
                flag = pResponsetext.Contains("\"msg\" : \"登录成功\"");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"msg\" : \"", "\"", 0);
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
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, base.BetsTime2, "UTF-8", true);
            return pResponsetext.Contains("乐美汇");
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

