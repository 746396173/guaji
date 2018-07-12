namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class THYL : PTBase
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
                            int num = Convert.ToInt32(plan.AutoTimes(str4, true));
                            int num2 = plan.FNNumber(str4);
                            string str6 = (Convert.ToDouble(this.GetPrize(plan.Type, plan.Play)) / Math.Pow(10.0, (double) (plan.Unit - 1))).ToString();
                            string format = "(\"list\":[(\"lotteryCode\":\"{0}\",\"lotteryBetCode\":\"{1}\",\"typeName\":\"{2}\",\"betContent\":\"{3}\",\"moneyType\":\"{4}\",\"betModel\":\"{5}\",\"betCount\":{6},\"betMultiple\":\"{7}\",\"betRebates\":{8},\"betMoney\":\"{9}\",\"betDigit\":\"{10}\",\"desc\":\"玩法:{2} 号码:{3} 模式:{4}, 奖金:{14},返点:{8}% 包含:{6}注,{7}倍, 共计{9}元\")],\"betPeriod\":\"{11}\",\"regSource\":{12},\"token\":\"{13}\")";
                            format = string.Format(format, new object[] { this.GetPTLotteryName(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), this.GetPlayString(plan.Play), this.GetNumberList1(pTNumberList, plan.Play, null), plan.UnitZWString, plan.Unit, num2, num, "0", plan.AutoTotalMoney(str4, true), this.GetRXWZString1(plan.RXWZ, plan.Play), this.GetBetsExpect(plan.CurrentExpect, ""), "1", base.Token, str6 }).Replace("(", "{").Replace(")", "}");
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, AppInfo.PTInfo.BetsTime2, "UTF-8", true);
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
            (pResponseText.Contains("\"msg\":\"成功\"") || (pResponseText == "投注成功"));

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
                    string pData = $"(token:{base.Token})".Replace("(", "{").Replace(")", "}");
                    HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                    if (this.CheckBreakConnect(pResponsetext))
                    {
                        AppInfo.PTInfo.PTIsBreak = true;
                    }
                    else
                    {
                        string str5 = CommFunc.GetIndexString(pResponsetext, "\"data\":", "}", 0);
                        AppInfo.Account.BankBalance = Convert.ToDouble(str5);
                    }
                }
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/v1/member/getAccountBalance");

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if ((((pType != ConfigurationStatus.LotteryType.AH11X5) && (pType != ConfigurationStatus.LotteryType.LN11X5)) && ((pType != ConfigurationStatus.LotteryType.JS11X5) && (pType != ConfigurationStatus.LotteryType.GD11X5))) && (pType != ConfigurationStatus.LotteryType.HLJ11X5))
            {
                return str;
            }
            if (pIsBets)
            {
                return str.Substring(2).Remove(6, 1);
            }
            return ("20" + str);
        }

        public override string GetBetsExpect(string pExpect, string pLotteryID = "")
        {
            string iD = AppInfo.Current.Lottery.ID;
            string str2 = CommFunc.ConvertBetsExpect(pExpect, iD, false, false, false).Replace("-", "");
            return this.GetAppExpect(AppInfo.Current.Lottery.Type, str2, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/v1/bet/bet");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/v1/lottery/lotteryResult");

        public override string GetIndexLine() => 
            (this.GetLine() + "/index.html#/");

        public override string GetLineString(string pResponseText)
        {
            string str = pResponseText;
            if (str.Contains("http://spi.ap38.in"))
            {
                str = "线路1";
            }
            return str;
        }

        public override string GetLoginLine() => 
            (this.GetLine() + "/login.html");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            (this.GetLine() + "/lottery.html");

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
                    return CommFunc.Join(list).Replace("*", ",");
                }
                if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        return CommFunc.Join(pNumberList, ",");
                    }
                    return CommFunc.Join(pNumberList, ",");
                }
                return CommFunc.Join(pNumberList, ",");
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, ",").Replace(" ", "");
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
                return CommFunc.Join(pNumberList, ",").Replace(" ", "");
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                return CommFunc.Join(pNumberList, ",").Replace(" ", "").Replace("*", "");
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
                    str2 = CommFunc.Join(pNumberList);
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
                    return "q3_zhx_ds";
                }
                if (playName == "前三直选复式")
                {
                    return "q3_zhx_fs";
                }
                if (playName == "前三组三复式")
                {
                    return "q3_zx_fs3";
                }
                if (playName == "前三组三单式")
                {
                    return "q3_zx_ds3";
                }
                if (playName == "前三组六复式")
                {
                    return "q3_zx_fs6";
                }
                if (playName == "前三组六单式")
                {
                    return "q3_zx_ds6";
                }
                if (playName == "后三直选单式")
                {
                    return "h3_zhx_ds";
                }
                if (playName == "后三直选复式")
                {
                    return "h3_zhx_fs";
                }
                if (playName == "后三组三复式")
                {
                    return "h3_zx_fs3";
                }
                if (playName == "后三组三单式")
                {
                    return "h3_zx_ds3";
                }
                if (playName == "后三组六复式")
                {
                    return "h3_zx_fs6";
                }
                if (playName == "后三组六单式")
                {
                    return "h3_zx_ds6";
                }
                if (playName == "中三直选单式")
                {
                    return "z3_zhx_ds";
                }
                if (playName == "中三直选复式")
                {
                    return "z3_zhx_fs";
                }
                if (playName == "中三组三复式")
                {
                    return "z3_zx_fs3";
                }
                if (playName == "中三组三单式")
                {
                    return "z3_zx_ds3";
                }
                if (playName == "中三组六复式")
                {
                    return "z3_zx_fs6";
                }
                if (playName == "中三组六单式")
                {
                    return "z3_zx_ds6";
                }
                if (playName == "前二直选单式")
                {
                    return "q2_zhx_ds";
                }
                if (playName == "前二直选复式")
                {
                    return "q2_zhx_fs";
                }
                if (playName == "后二直选单式")
                {
                    return "h2_zhx_ds";
                }
                if (playName == "后二直选复式")
                {
                    return "h2_zhx_fs";
                }
                if (playName == "后四直选单式")
                {
                    return "h4_zhx_ds";
                }
                if (playName == "后四直选复式")
                {
                    return "h4_zhx_fs";
                }
                if (playName == "五星直选单式")
                {
                    return "5x_zhx_ds";
                }
                if (playName == "五星直选复式")
                {
                    return "5x_zhx_fs";
                }
                if (playName == "任三直选单式")
                {
                    return "r3_zhx_ds";
                }
                if (playName == "任三直选复式")
                {
                    return "r3_zhx_fs";
                }
                if (playName == "任三组三复式")
                {
                    return "r3_zx_fs3";
                }
                if (playName == "任三组三单式")
                {
                    return "r3_zx_ds3";
                }
                if (playName == "任三组六复式")
                {
                    return "r3_zx_fs6";
                }
                if (playName == "任三组六单式")
                {
                    return "r3_zx_ds6";
                }
                if (playName == "任二直选单式")
                {
                    return "r2_zhx_ds";
                }
                if (playName == "任二直选复式")
                {
                    return "r2_zhx_fs";
                }
                if (playName == "任四直选单式")
                {
                    return "r4_zhx_ds";
                }
                if (playName == "任四直选复式")
                {
                    return "r4_zhx_fs";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "dwd_dwd_dwd";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "q3_zhx_ds";
                }
                if (playName == "前二直选单式")
                {
                    return "q2_zhx_ds";
                }
                if (playName == "任选复式一中一")
                {
                    return "rx_fs_1z1";
                }
                if (playName == "任选复式二中二")
                {
                    return "rx_fs_2z2";
                }
                if (playName == "任选复式三中三")
                {
                    return "rx_fs_3z3";
                }
                if (playName == "任选复式四中四")
                {
                    return "rx_fs_4z4";
                }
                if (playName == "任选复式五中五")
                {
                    return "rx_fs_5z5";
                }
                if (playName == "任选单式一中一")
                {
                    return "rx_ds_1z1";
                }
                if (playName == "任选单式二中二")
                {
                    return "rx_ds_2z2";
                }
                if (playName == "任选单式三中三")
                {
                    return "rx_ds_3z3";
                }
                if (playName == "任选单式四中四")
                {
                    return "rx_ds_4z4";
                }
                if (playName == "任选单式五中五")
                {
                    str = "rx_ds_5z5";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "cq3_cq3_ds";
                }
                if (playName == "猜前三复式")
                {
                    return "cq3_cq3_fs";
                }
                if (playName == "猜前二单式")
                {
                    return "cq2_cq2_ds";
                }
                if (playName == "猜前二复式")
                {
                    return "cq2_cq2_fs";
                }
                if (playName == "猜前四单式")
                {
                    return "cq4_cq4_ds";
                }
                if (playName == "猜前四复式")
                {
                    return "cq4_cq4_fs";
                }
                if (playName == "猜前五单式")
                {
                    return "cq5_cq5_ds";
                }
                if (playName == "猜前五复式")
                {
                    return "cq5_cq5_fs";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "cq1_cq1_cq1";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "dwd_dwd_dwd";
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
                    return "前三直选单式";
                }
                if (playName == "前三直选复式")
                {
                    return "前三直选复式";
                }
                if (playName == "前三组三复式")
                {
                    return "前三组三复式";
                }
                if (playName == "前三组三单式")
                {
                    return "前三组三单式";
                }
                if (playName == "前三组六复式")
                {
                    return "前三组六复式";
                }
                if (playName == "前三组六单式")
                {
                    return "前三组六单式";
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
                    return "后三组三复式";
                }
                if (playName == "后三组三单式")
                {
                    return "后三组三单式";
                }
                if (playName == "后三组六复式")
                {
                    return "后三组六复式";
                }
                if (playName == "后三组六单式")
                {
                    return "后三组六单式";
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
                    return "中三组三复式";
                }
                if (playName == "中三组三单式")
                {
                    return "中三组三单式";
                }
                if (playName == "中三组六复式")
                {
                    return "中三组六复式";
                }
                if (playName == "中三组六单式")
                {
                    return "中三组六单式";
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
                if (playName == "后四直选单式")
                {
                    return "四星直选单式";
                }
                if (playName == "后四直选复式")
                {
                    return "四星直选复式";
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
                    return "任三组三复式";
                }
                if (playName == "任三组三单式")
                {
                    return "任三组三单式";
                }
                if (playName == "任三组六复式")
                {
                    return "任三组六复式";
                }
                if (playName == "任三组六单式")
                {
                    return "任三组六单式";
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
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "前三直选单式";
                }
                if (playName == "前二直选单式")
                {
                    return "前二直选单式";
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
                if (playName == "猜前五单式")
                {
                    return "猜前五单式";
                }
                if (playName == "猜前五复式")
                {
                    return "猜前五复式";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "猜冠军";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "定位胆";
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
            return CommFunc.GetIndexString(pResponseText, "\"msg\":\"", "\"", 0);
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "ssc_cq";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "ssc_xj";
            }
            if (pType == ConfigurationStatus.LotteryType.THFFC)
            {
                return "ssc_fast";
            }
            if (pType == ConfigurationStatus.LotteryType.THMD2FC)
            {
                return "ssc_md";
            }
            if (pType == ConfigurationStatus.LotteryType.THDJSSC)
            {
                return "ssc_dj15";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "ssc_tx";
            }
            if (pType == ConfigurationStatus.LotteryType.THJDSSC)
            {
                return "ssc_jd15";
            }
            if (pType == ConfigurationStatus.LotteryType.THTGSSC)
            {
                return "ssc_tg15";
            }
            if (pType == ConfigurationStatus.LotteryType.TH5FC)
            {
                return "ssc_az5";
            }
            if (pType == ConfigurationStatus.LotteryType.THHGSSC)
            {
                return "ssc_hs15";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "x5_gd";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "x5_jx";
            }
            if (pType == ConfigurationStatus.LotteryType.HLJ11X5)
            {
                return "x5_hlj";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "x5_ah";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "x5_sh";
            }
            if (pType == ConfigurationStatus.LotteryType.LN11X5)
            {
                return "x5_ln";
            }
            if (pType == ConfigurationStatus.LotteryType.JS11X5)
            {
                return "x5_js";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "pk10_bj";
            }
            if (pType == ConfigurationStatus.LotteryType.THPK10)
            {
                return "pk10_fast";
            }
            if (pType == ConfigurationStatus.LotteryType.THOZPK10)
            {
                str = "pk10_az10";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/v1/member/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public string GetRXWZString1(List<int> pRXWZ, string playName)
        {
            string str = "0,0,0,0,0";
            if (!CommFunc.CheckPlayIsRX(playName))
            {
                return str;
            }
            if (CommFunc.CheckPlayIsRXFS(playName))
            {
                return str;
            }
            List<string> pList = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string item = pRXWZ.Contains(i) ? "1" : "0";
                pList.Add(item);
            }
            return CommFunc.Join(pList, ",");
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string pUrl = $"{this.GetLine()}/v1/lottery/lotteryCurrentPeriod";
                string lotteryLine = this.GetLotteryLine(pType, false);
                string pResponsetext = "";
                string pData = string.Format("(\"lotteryCode\":\"{0}\",\"token\":\"{1}\")", this.GetPTLotteryName(pType), this.Token).Replace("(", "{").Replace(")", "}");
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                if (pResponsetext != "")
                {
                    base.Expect = CommFunc.GetIndexString(pResponsetext, "\"drawPeriod\":\"", "\"", 0);
                    base.Expect = this.GetAppExpect(pType, base.Expect, false);
                    base.Expect = CommFunc.ConvertExpect(base.Expect, pType);
                    base.ExpectID = CommFunc.GetIndexString(pResponsetext, "\"lotteryDrawId\":", ",", 0);
                }
                if (base.Prize == "")
                {
                    pUrl = $"{this.GetLine()}/v1/member/memberMsg";
                    lotteryLine = this.GetLotteryLine(pType, false);
                    pResponsetext = "";
                    pData = string.Format("(\"token\":\"{0}\")", this.Token).Replace("(", "{").Replace(")", "}");
                    HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        base.Rebate = CommFunc.GetIndexString(pResponsetext, "\"rebatesPercent\":", ",", 0);
                        base.Prize = (1800.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 1000.0)).ToString();
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
                string str2 = $"/v1/member/verifyCode?_={DateTime.Now.ToOADate()}&token={base.Token}";
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
                string pUrl = this.GetLine() + "/v1/member/loginNew";
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = string.Format("(\"memberName\":\"{0}\",\"memberPass\":\"{1}\",\"verifyCode\":\"{2}\",\"host\":\"tianhao66.com\",\"token\":\"{3}\")", new object[]
        {
            pID,
            str5,
            webVerifyCode,
            this.Token
        }).Replace("(", "{").Replace(")", "}");
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("\"msg\":\"成功\"");
                if (!flag)
                {
                    pHint = CommFunc.GetIndexString(pResponsetext, "\"msg\":\"", "\"", 0);
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
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLine() + "/v1/getToken";
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, loginLine, 0x2710, "UTF-8", true);
            base.Token = CommFunc.GetIndexString(pResponsetext, "\"token\":\"", "\"", 0);
            return (base.Token != "");
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            string pData = $"(token:{base.Token})".Replace("(", "{").Replace(")", "}");
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

