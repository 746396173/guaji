namespace IntelligentPlanning
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class SIJI : PTBase
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
                            List<string> pList = new List<string>();
                            string pTLotteryName = this.GetPTLotteryName(plan.Type);
                            if (plan.Type == ConfigurationStatus.LotteryType.NYFFC)
                            {
                                pTLotteryName = "ZIZHUYFC";
                            }
                            else if (plan.Type == ConfigurationStatus.LotteryType.NY3FC)
                            {
                                pTLotteryName = "ZIZHUSFC";
                            }
                            else if (plan.Type == ConfigurationStatus.LotteryType.NY5FC)
                            {
                                pTLotteryName = "ZIZHUWFC";
                            }
                            pList.Add(pTLotteryName);
                            pList.Add(this.GetPlayMethodID(plan.Type, plan.Play));
                            pList.Add("2" + plan.UnitZWString);
                            pList.Add(num.ToString());
                            pList.Add(plan.AutoMoney(str4, true).ToString());
                            pList.Add(base.Prize);
                            pList.Add("undefined");
                            pList.Add(Convert.ToInt32(plan.AutoTimes(str4, true)).ToString());
                            pList.Add(this.GetBetsExpect(plan.CurrentExpect, ""));
                            pList.Add(DateAndTime.Now.ToOADate().ToString());
                            pList.Add(this.GetNumberList1(pTNumberList, plan.Play, null));
                            string rXWZString = "";
                            if (CommFunc.CheckPlayIsRXDS(plan.Play))
                            {
                                rXWZString = this.GetRXWZString(plan.RXWZ);
                            }
                            pList.Add(rXWZString + "^");
                            string str8 = CommFunc.Join(pList, "|");
                            string format = "selArr={0}&istask=no&perstop=0&moneys=0&lists=&gamekey={1}";
                            format = string.Format(format, str8, pTLotteryName);
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
            pHint.Contains("会员登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"code\":\"200\"") || (pResponseText == "投注成功"));

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
                string str5 = pResponsetext;
                AppInfo.Account.BankBalance = Convert.ToDouble(str5);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/client/index.php?a=lotto&m=getmoney");

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                if (pIsBets)
                {
                    return str.Insert(8, "0");
                }
                return str.Remove(8, 1);
            }
            if (((((((pType == ConfigurationStatus.LotteryType.GD11X5) || (pType == ConfigurationStatus.LotteryType.JX11X5)) || ((pType == ConfigurationStatus.LotteryType.SH11X5) || (pType == ConfigurationStatus.LotteryType.SD11X5))) || (((pType == ConfigurationStatus.LotteryType.AH11X5) || (pType == ConfigurationStatus.LotteryType.BJ11X5)) || ((pType == ConfigurationStatus.LotteryType.HB11X5) || (pType == ConfigurationStatus.LotteryType.LN11X5)))) || ((((pType == ConfigurationStatus.LotteryType.HLJ11X5) || (pType == ConfigurationStatus.LotteryType.JL11X5)) || ((pType == ConfigurationStatus.LotteryType.GS11X5) || (pType == ConfigurationStatus.LotteryType.QH11X5))) || (((pType == ConfigurationStatus.LotteryType.HN11X5) || (pType == ConfigurationStatus.LotteryType.JS11X5)) || ((pType == ConfigurationStatus.LotteryType.HUB11X5) || (pType == ConfigurationStatus.LotteryType.ZJ11X5))))) || ((((pType == ConfigurationStatus.LotteryType.YN11X5) || (pType == ConfigurationStatus.LotteryType.FJ11X5)) || ((pType == ConfigurationStatus.LotteryType.SXR11X5) || (pType == ConfigurationStatus.LotteryType.SXL11X5))) || (((pType == ConfigurationStatus.LotteryType.GZ11X5) || (pType == ConfigurationStatus.LotteryType.TJ11X5)) || (pType == ConfigurationStatus.LotteryType.GX11X5)))) || (pType == ConfigurationStatus.LotteryType.NMG11X5))
            {
                if (pIsBets)
                {
                    str = str.Replace("-0", "");
                }
                return str;
            }
            if (pIsBets)
            {
                str = str.Replace("-", "");
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
            (this.GetLine() + "/client/index.php?a=lotto&m=gameBuy");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "-1";

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/client/index.php?a=caipiao&m=zoushi&active=getdate&game={this.GetPTLotteryName(pType)}&limit=30";

        public override string GetIndexLine() => 
            (this.GetLine() + "/client/index.php");

        public override string GetLoginLine() => 
            (this.GetLine() + "/client/index.php?a=login");

        public override string GetLoginLineID() => 
            (this.GetLine() + "/client/index.php?a=login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/client/?m=index&a=lotto&play={this.GetPTLotteryName(pType)}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            int num;
            string str2;
            int num2;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    list = new List<string>();
                    for (num = 0; num < pNumberList.Count; num++)
                    {
                        str2 = pNumberList[num];
                        list.Add(str2);
                    }
                    return CommFunc.Join(list, ",").Replace("*", "");
                }
                if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num2 = AppInfo.FiveDic[ch.ToString()];
                    list = new List<string>();
                    for (num = 0; num < 5; num++)
                    {
                        str2 = "*";
                        if (num == num2)
                        {
                            str2 = CommFunc.Join(pNumberList);
                        }
                        list.Add(str2);
                    }
                    return CommFunc.Join(list, ",").Replace("*", "");
                }
                return CommFunc.Join(pNumberList, ",");
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, ",");
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
                str = CommFunc.Join(pNumberList, ",");
            }
            else if (CommFunc.CheckPlayIsFS(playName))
            {
                list = new List<string>();
                for (num = 0; num < pNumberList.Count; num++)
                {
                    string pStr = pNumberList[num];
                    str2 = CommFunc.Join(CommFunc.ConvertPK10CodeToBets(CommFunc.SplitString(pStr, " ", -1), -1), " ");
                    list.Add(str2);
                }
                str = CommFunc.Join(list, ",").Replace("*", "");
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
                pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
                for (num = 0; num < num3; num++)
                {
                    str2 = "*";
                    if (num == num2)
                    {
                        str2 = CommFunc.Join(pNumberList, " ");
                    }
                    list.Add(str2);
                }
                str = CommFunc.Join(list, ",").Replace("*", "");
                if (playName == "猜冠军猜冠军")
                {
                    str = str.Replace(" ", ",");
                }
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
                    return "前三|直选单式|3X1_ds";
                }
                if (playName == "前三直选复式")
                {
                    return "前三|直选复式|3X1_fs";
                }
                if (playName == "前三组三复式")
                {
                    return "前三|组三|3X1_z3";
                }
                if (playName == "前三组三单式")
                {
                    return "前三|组三单式|3X1_z3ds";
                }
                if (playName == "前三组六复式")
                {
                    return "前三|组六|3X1_z6";
                }
                if (playName == "前三组六单式")
                {
                    return "前三|组六单式|3X1_z6ds";
                }
                if (playName == "后三直选单式")
                {
                    return "后三|单式|3X2_ds";
                }
                if (playName == "后三直选复式")
                {
                    return "后三|复式|3X2_fs";
                }
                if (playName == "后三组三复式")
                {
                    return "后三|组三|3X2_z3";
                }
                if (playName == "后三组三单式")
                {
                    return "后三|组三单式|3X2_z3ds";
                }
                if (playName == "后三组六复式")
                {
                    return "后三|组六|3X2_z6";
                }
                if (playName == "后三组六单式")
                {
                    return "后三|组六单式|3X2_z6ds";
                }
                if (playName == "中三直选单式")
                {
                    return "中三|单式|3X3_ds";
                }
                if (playName == "中三直选复式")
                {
                    return "中三|复式|3X3_fs";
                }
                if (playName == "中三组三复式")
                {
                    return "中三|组三|3X3_z3";
                }
                if (playName == "中三组三单式")
                {
                    return "中三|组三单式|3X3_z3ds";
                }
                if (playName == "中三组六复式")
                {
                    return "中三|组六|3X3_z6";
                }
                if (playName == "中三组六单式")
                {
                    return "中三|组六单式|3X3_z6ds";
                }
                if (playName == "前二直选单式")
                {
                    return "二星|直选单式|2X_1_zhxds";
                }
                if (playName == "前二直选复式")
                {
                    return "二星|直选复式|2X_1_zhxfs";
                }
                if (playName == "后二直选单式")
                {
                    return "二星|直选单式|2X_2_zhxds";
                }
                if (playName == "后二直选复式")
                {
                    return "二星|直选复式|2X_2_zhxfs";
                }
                if (playName == "后四直选单式")
                {
                    return "四星|单式|4X_ds";
                }
                if (playName == "后四直选复式")
                {
                    return "四星|复式|4X_fs";
                }
                if (playName == "五星直选单式")
                {
                    return "五星|单式|5X_ds";
                }
                if (playName == "五星直选复式")
                {
                    return "五星|复式|5X_fs";
                }
                if (playName == "任三直选单式")
                {
                    return "任选三|直选单式|RX3_zhxds";
                }
                if (playName == "任三直选复式")
                {
                    return "任选三|直选复式|RX3_zhxfs";
                }
                if (playName == "任三组三复式")
                {
                    return "任选三|组三复式|RX3_z3";
                }
                if (playName == "任三组三单式")
                {
                    return "任选三|组三单式|RX3_z3ds";
                }
                if (playName == "任三组六复式")
                {
                    return "任选三|组六复式|RX3_z6";
                }
                if (playName == "任三组六单式")
                {
                    return "任选三|组六单式|RX3_z6ds";
                }
                if (playName == "任二直选单式")
                {
                    return "任选二|直选单式|RX2_zhxds";
                }
                if (playName == "任二直选复式")
                {
                    return "任选二|直选复式|RX2_zhxfs";
                }
                if (playName == "任四直选单式")
                {
                    return "任选四|直选单式|RX4_ds";
                }
                if (playName == "任四直选复式")
                {
                    return "任选四|直选复式|RX4_zhxfs";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "一星|定位胆|1X_dwd";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "三码|直选单式|3M_zhxds";
                }
                if (playName == "前二直选单式")
                {
                    return "二码|前二直选单式|2M_zhxds";
                }
                if (playName == "任选复式一中一")
                {
                    return "任选复式|任选一中一|RXFS_fs1z1";
                }
                if (playName == "任选复式二中二")
                {
                    return "任选复式|任选二中二|RXFS_fs2z2";
                }
                if (playName == "任选复式三中三")
                {
                    return "任选复式|任选三中三|RXFS_fs3z3";
                }
                if (playName == "任选复式四中四")
                {
                    return "任选复式|任选四中四|RXFS_fs4z4";
                }
                if (playName == "任选复式五中五")
                {
                    return "任选复式|任选五中五|RXFS_fs5z5";
                }
                if (playName == "任选单式一中一")
                {
                    return "任选单式|任选一中一|RXDS_1z1";
                }
                if (playName == "任选单式二中二")
                {
                    return "任选单式|任选二中二|RXDS_2z2";
                }
                if (playName == "任选单式三中三")
                {
                    return "任选单式|任选三中三|RXDS_3z3";
                }
                if (playName == "任选单式四中四")
                {
                    return "任选单式|任选四中四|RXDS_4z4";
                }
                if (playName == "任选单式五中五")
                {
                    str = "任选单式|任选五中五|RXDS_5z5";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "直选单式|前三单式|CMCDS_q3";
                }
                if (playName == "猜前三复式")
                {
                    return "精确排名|精确前三|pkwz_q3";
                }
                if (playName == "猜前二单式")
                {
                    return "直选单式|前二单式|CMCDS_q2";
                }
                if (playName == "猜前二复式")
                {
                    return "精确排名|精确前二|pkwz_q2";
                }
                if (playName == "猜前四单式")
                {
                    return "直选单式|前四单式|CMCDS_q4";
                }
                if (playName == "猜前四复式")
                {
                    return "精确排名|精确前四|pkwz_q4";
                }
                if (playName == "猜前五单式")
                {
                    return "直选单式|前五单式|CMCDS_q5";
                }
                if (playName == "猜前五复式")
                {
                    return "";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "定位胆|定位胆(1~10)|pkdwd_dwd";
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
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"msg\":\"", "\"", 0));
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            if (pType == ConfigurationStatus.LotteryType.JLSSC)
            {
                return "JLSSC";
            }
            if (pType == ConfigurationStatus.LotteryType.NMGSSC)
            {
                return "NMSSC";
            }
            if (pType == ConfigurationStatus.LotteryType.SIJIFFC)
            {
                return "SJYFC";
            }
            if (pType == ConfigurationStatus.LotteryType.SIJI3FC)
            {
                return "SJSFC";
            }
            if (pType == ConfigurationStatus.LotteryType.SIJI5FC)
            {
                return "SJWFC";
            }
            if (pType == ConfigurationStatus.LotteryType.SIJIFLBSSC)
            {
                return "FLB15FC";
            }
            if (pType == ConfigurationStatus.LotteryType.SIJIDJSSC)
            {
                return "DJ15FC";
            }
            if (pType == ConfigurationStatus.LotteryType.SIJIELSSSC)
            {
                return "ELS15FC";
            }
            if (pType == ConfigurationStatus.LotteryType.SIJIHGSSC)
            {
                return "HG15FC";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "QQYFC";
            }
            if (pType == ConfigurationStatus.LotteryType.SIJITXYFC)
            {
                return "TXYFC";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "BJSSC";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "TWWFC";
            }
            if (pType == ConfigurationStatus.LotteryType.HLJSSC)
            {
                return "LJSSC";
            }
            if (pType == ConfigurationStatus.LotteryType.YNSSC)
            {
                return "YNSSC";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "SD11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "GD11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "JX11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.BJ11X5)
            {
                return "BJ11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.HB11X5)
            {
                return "HB11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.LN11X5)
            {
                return "LN11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.HLJ11X5)
            {
                return "HLJ11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.JL11X5)
            {
                return "JN11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.GS11X5)
            {
                return "GS11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.QH11X5)
            {
                return "QH11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.HN11X5)
            {
                return "HN11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.JS11X5)
            {
                return "JS11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.HUB11X5)
            {
                return "HUB11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.ZJ11X5)
            {
                return "ZJ11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.YN11X5)
            {
                return "YN11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.FJ11X5)
            {
                return "FJ11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.SXR11X5)
            {
                return "SX11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.SXL11X5)
            {
                return "SHX11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.GZ11X5)
            {
                return "GZ11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.AH11X5)
            {
                return "AH11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "SH11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.TJ11X5)
            {
                return "TJ11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.GX11X5)
            {
                return "GX11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.NMG11X5)
            {
                return "NM11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.SIJIFF11X5)
            {
                return "SJYF11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.SIJI3F11X5)
            {
                return "SJSF11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.SIJI5F11X5)
            {
                return "SJ11-5";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                return "BJPK10";
            }
            return CommFunc.GetLotteryID(pType);
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/client/index.php?a=login&m=logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ) => 
            CommFunc.Join<int>(pRXWZ);

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                string str;
                if (base.Prize == "")
                {
                    str = this.LoginLotteryWeb(pType, "");
                    if (str == "")
                    {
                        return;
                    }
                    base.Prize = CommFunc.GetIndexString(str, "CurMode:'", "'", 0);
                }
                string pUrl = $"{this.GetLine()}/client/index.php?a=lotto&m=getLotTime&play={this.GetPTLotteryName(pType)}";
                string indexLine = this.GetIndexLine();
                str = "";
                HttpHelper.GetResponse(ref str, pUrl, "GET", string.Empty, indexLine, 0x4e20, "UTF-8", true);
                if (str != "")
                {
                    base.Expect = str.Split(new char[] { '|' })[0];
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
                string str2 = $"/client/code.php?{DateTime.Now.ToOADate()}";
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
                string loginLineID = this.GetLoginLineID();
                string pResponsetext = "";
                string str5 = HttpUtility.UrlEncode(pW);
                string pData = $"user-name={pID}&user-password={str5}&user-code={webVerifyCode}&nextGo=AJAX";
                HttpHelper.GetResponse(ref pResponsetext, loginLineID, "POST", pData, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("\"code\":\"4\"");
                if (!flag)
                {
                    pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "\"msg\":\"", "\"", 0));
                    if (pResponsetext.Contains("\"code\":\"1\""))
                    {
                        pHint = "帐号或者密码不正确";
                    }
                    else if (pHint.Contains("验证码"))
                    {
                        pHint = "";
                        return this.InputWeb(pID, pW, ref pHint);
                    }
                }
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
            string cookieInternal = HttpHelper.GetCookieInternal(this.GetUrlLine());
            AppInfo.PTInfo.WebCookie = cookieInternal;
            HttpHelper.SaveCookies(cookieInternal, "");
            return true;
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

