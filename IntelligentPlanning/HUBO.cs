namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class HUBO : PTBase
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
                            string prize = this.GetPrize(plan.Type, plan.Play);
                            List<string> pList = new List<string> {
                                this.GetPlayMethodID(plan.Type, plan.Play),
                                this.GetNumberList1(pTNumberList, plan.Play, null),
                                num.ToString(),
                                "1",
                                plan.AutoTotalMoney(str4, true).ToString()
                            };
                            pList.Add((plan.Unit - 1).ToString());
                            pList.Add($"{prize}|{"0"}|{"2"}");
                            pList.Add(Convert.ToInt32(plan.AutoTimes(str4, true)).ToString());
                            string str7 = CommFunc.Join(pList, ":");
                            string rXWZString = "";
                            if (CommFunc.CheckPlayIsRXDS(plan.Play))
                            {
                                rXWZString = this.GetRXWZString(plan.RXWZ);
                            }
                            else if (CommFunc.CheckPlayIsRXFS(plan.Play))
                            {
                                List<string> list3 = new List<string>();
                                int num2 = 0;
                                foreach (string str9 in pTNumberList)
                                {
                                    if (str9 != "*")
                                    {
                                        string item = AppInfo.IndexDic[num2] + "位";
                                        list3.Add(item);
                                    }
                                    num2++;
                                }
                                rXWZString = CommFunc.Join(list3, "、");
                            }
                            string format = "data={0}:{7}&lottery_number_id={1}&is_add=0&lottery_id={2}&amount={3}&com=n*{4}*{5}*{6}";
                            format = string.Format(format, new object[] { str7, base.ExpectID, this.GetBetsLotteryID(plan.Type), plan.AutoTotalMoney(str4, true), DateTime.Now.ToOADate(), base.ExpectID, base.Token, rXWZString });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, 0x2710, "UTF-8", true);
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
            pHint.Contains("登陆模块");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains(@"\u8d2d\u4e70\u6210\u529f\uff01") || (pResponseText == "投注成功"));

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
                if (base.WebCookie != "")
                {
                    string accountsMemLine = this.GetAccountsMemLine(pType);
                    string lotteryLine = this.GetLotteryLine(pType, false);
                    string pResponsetext = "";
                    string pData = "";
                    HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                    base.BankBalance = pResponsetext.Split(new char[] { '_' })[0];
                    AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/getMoney");

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            return CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/Small");

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
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "34";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOTXFFC)
            {
                return "32";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOWXFFC)
            {
                return "42";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOFFC)
            {
                return "19";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBO2FC)
            {
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBO5FC)
            {
                return "17";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOHGSSC)
            {
                return "22";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBODJSSC)
            {
                return "37";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOFLP2FC)
            {
                return "43";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBODZ30M)
            {
                return "40";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOML20M)
            {
                return "39";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOMG45M)
            {
                str = "45";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/DisplayBuy/timeDown";

        public bool GetIndexData()
        {
            bool flag = false;
            string pResponsetext = "";
            try
            {
                string indexLine = this.GetIndexLine();
                string pReferer = this.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
                base.Token = CommFunc.GetIndexString(pResponsetext, "var s = \"", "\"", 0);
                flag = base.Token != "";
            }
            catch
            {
            }
            return flag;
        }

        public override string GetIndexLine() => 
            (this.GetLine() + "/Protocol");

        public override string GetLoginLine() => 
            (this.GetLine() + "/login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false)
        {
            string str = "Ssc";
            return $"{this.GetLine()}/GameLottery/{str}/{this.GetPTLotteryName(pType)}";
        }

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            if (CommFunc.CheckPlayIsFS(playName))
            {
                return CommFunc.Join(pNumberList, ",").Replace("*", "");
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
                return CommFunc.Join(pList, ",").Replace("*", "");
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

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (playName == "前三直选单式")
            {
                return "前三直选单式";
            }
            if (playName == "前三直选复式")
            {
                return "前三直选复式";
            }
            if (playName == "前三组三复式")
            {
                return "前三组三";
            }
            if (playName == "前三组六复式")
            {
                return "前三组六";
            }
            if (playName == "后三直选单式")
            {
                return "后三直选单式";
            }
            if (playName == "后三直选复式")
            {
                return "后三直选复式";
            }
            if (playName == "后三组三复式")
            {
                return "后三组三";
            }
            if (playName == "后三组六复式")
            {
                return "后三组六";
            }
            if (playName == "中三直选单式")
            {
                return "中三直选单式";
            }
            if (playName == "中三直选复式")
            {
                return "中三直选复式";
            }
            if (playName == "中三组三复式")
            {
                return "中三组三";
            }
            if (playName == "中三组六复式")
            {
                return "中三组六";
            }
            if (playName == "前二直选单式")
            {
                return "前二直选单式";
            }
            if (playName == "前二直选复式")
            {
                return "前二直选复式";
            }
            if (playName == "后二直选单式")
            {
                return "后二直选单式";
            }
            if (playName == "后二直选复式")
            {
                return "后二直选复式";
            }
            if (playName == "前四直选单式")
            {
                return "前四直选单式";
            }
            if (playName == "前四直选复式")
            {
                return "前四直选复式";
            }
            if (playName == "后四直选单式")
            {
                return "后四直选单式";
            }
            if (playName == "后四直选复式")
            {
                return "后四直选复式";
            }
            if (playName == "五星直选单式")
            {
                return "五星直选单式";
            }
            if (playName == "五星直选复式")
            {
                return "五星直选复式";
            }
            if (playName == "任三直选单式")
            {
                return "任三直选单式";
            }
            if (playName == "任三直选复式")
            {
                return "任三直选复式";
            }
            if (playName == "任三组三复式")
            {
                return "组三复式";
            }
            if (playName == "任三组三单式")
            {
                return "组三单式";
            }
            if (playName == "任三组六复式")
            {
                return "组六复式";
            }
            if (playName == "任三组六单式")
            {
                return "组六单式";
            }
            if (playName == "任二直选单式")
            {
                return "任二直选单式";
            }
            if (playName == "任二直选复式")
            {
                return "任二直选复式";
            }
            if (playName == "任四直选单式")
            {
                return "任四直选单式";
            }
            if (playName == "任四直选复式")
            {
                return "任四直选复式";
            }
            if (playName.Contains("定位胆"))
            {
                str = "定位胆";
            }
            return str;
        }

        public override string GetPTHint(string pResponseText)
        {
            if (this.CheckReturn(pResponseText, false))
            {
                return "投注成功";
            }
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"info\":\"", "\"", 0).Replace("!", ""));
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "Cqssc";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "Xjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "Tjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "Bjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "Twssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOTXFFC)
            {
                return "Txssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOWXFFC)
            {
                return "Wxssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOFFC)
            {
                return "Yfssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBO2FC)
            {
                return "Efssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBO5FC)
            {
                return "Wfssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOHGSSC)
            {
                return "Hgssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBODJSSC)
            {
                return "Djssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOFLP2FC)
            {
                return "Flbssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOML20M)
            {
                return "Mlssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBODZ30M)
            {
                return "Dzssc";
            }
            if (pType == ConfigurationStatus.LotteryType.HUBOMG45M)
            {
                str = "Mgssc";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            List<string> pList = new List<string>();
            foreach (int num in pRXWZ)
            {
                string item = AppInfo.IndexDic[num] + "位";
                pList.Add(item);
            }
            return CommFunc.Join(pList, "、");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
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
                string str2 = $"/verify/{DateTime.Now.ToOADate()}";
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
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLine() + "/userlogin";
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string pData = $"username={pID}&password={str4}";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("\"status\":true");
            if (!flag)
            {
                pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "\"msg\":\"", "\"", 0).Replace("!", ""));
            }
            return flag;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext.Contains("千旺家");
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string pReferer = this.GetLine() + "/index";
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, quitPTLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            base.WebCookie = "";
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

