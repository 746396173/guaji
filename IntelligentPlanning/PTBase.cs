namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class PTBase
    {
        public string AgentKey = "";
        public string AgentValue = "";
        public bool AnalysisVerifyCode = false;
        public string BankBalance = "";
        public int BetsTime1 = 0x4e20;
        public int BetsTime2 = 0x9c40;
        public int BetsTime3 = 0xea60;
        public int BetsTime4 = 0x1d4c0;
        public string Expect = "";
        public string ExpectID = "";
        public bool IsGetVerifyCode = false;
        public bool IsLoadBets = true;
        public bool IsLoginRun = false;
        public bool IsLoginVR = false;
        public bool IsLoginZB = false;
        public int LineIndex = -1;
        public List<string> LineList = new List<string>();
        public bool LoadGetSite = false;
        public int LoginError = 0;
        public bool LoginMain = false;
        public int LoginMaxError = 8;
        public string LoginUrl = "";
        public string NextExpect = "";
        public string NextExpectID = "";
        public string PlayerKey = "";
        public string PlayerValue = "";
        public Dictionary<string, string> PlayMethodDic = new Dictionary<string, string>();
        public Dictionary<string, ConfigurationStatus.PTPrizeGroup> PlayMethodGroupDic = new Dictionary<string, ConfigurationStatus.PTPrizeGroup>();
        public string Prize = "";
        public Dictionary<string, string> PrizeDic = new Dictionary<string, string>();
        public int PTDXCount = 0;
        public string PTID = "";
        public bool PTIsBreak = false;
        public bool PTLoginStatus = false;
        public string PTName = "";
        public string PTTag = "";
        public string PTTagID = "";
        public string PTTime = "";
        public string PTUserID = "";
        public string Random = "";
        public string Rebate = "0";
        public bool RefreshAccountsMem = false;
        public List<string> SkipWebLoginUrlList = new List<string>();
        public string SkipWebLoginUrlTag = ".Skip";
        public string Token = "";
        public string UserID = "";
        public string VerifyCodeToken = "";
        public int VersionNumber = -1;
        public string WebCookie = "";

        public void AgainLoginMain(ConfigurationStatus.LotteryType pType, string pResponseText, int pDXCount = 3, bool pSwitch = false)
        {
            DebugLog.SaveDebug(pResponseText, "平台掉线");
            this.PTDXCount++;
            if (((this.PTDXCount >= pDXCount) || this.PTIsBreak) && this.PTLoginStatus)
            {
                DebugLog.SaveDebug(pResponseText, (this.PTIsBreak ? "投注因素" : "") + "重新登录");
                this.QuitPT();
                this.IsLoginRun = false;
                this.PTLoginStatus = false;
                CommFunc.PlaySound("掉线提示");
                Program.MainApp.Invoke(AppInfo.LoginMain, new object[] { pSwitch, this.PTName });
                int num = 0;
                while (true)
                {
                    Thread.Sleep(0xbb8);
                    if (this.PTLoginStatus)
                    {
                        break;
                    }
                    num++;
                    if (num >= 10)
                    {
                        this.PTLoginStatus = true;
                        break;
                    }
                }
                this.PTIsBreak = false;
                this.PTDXCount = 0;
            }
        }

        public virtual bool BetsMain(ConfigurationStatus.SCPlan plan, ref string pHint) => 
            false;

        public virtual bool BetsMain(List<ConfigurationStatus.SCPlan> planList, ref string pHint) => 
            false;

        public virtual bool CheckBreakConnect(string pHint) => 
            false;

        public virtual bool CheckLotteryIsVR(ConfigurationStatus.LotteryType pType) => 
            ((((((pType == ConfigurationStatus.LotteryType.VRSSC) || (pType == ConfigurationStatus.LotteryType.VRHXSSC)) || ((pType == ConfigurationStatus.LotteryType.VR3FC) || (pType == ConfigurationStatus.LotteryType.VRPK10))) || (((pType == ConfigurationStatus.LotteryType.VRSXFFC) || (pType == ConfigurationStatus.LotteryType.VRTXFFC)) || ((pType == ConfigurationStatus.LotteryType.VRMXSC) || (pType == ConfigurationStatus.LotteryType.VRYYPK10)))) || (pType == ConfigurationStatus.LotteryType.VRZXC11X5)) || (pType == ConfigurationStatus.LotteryType.VRKT));

        public virtual bool CheckLotteryIsZB(ConfigurationStatus.LotteryType pType) => 
            (pType == ConfigurationStatus.LotteryType.ZBYZCP15C);

        public virtual bool CheckReturn(string pResponseText, bool pIsChange) => 
            false;

        public virtual string ConvertLotteryExpectChar(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false) => 
            "";

        public virtual void CountPrizeDic(string pResponseText)
        {
        }

        public virtual string DecryptString(string pSource) => 
            pSource;

        public virtual void DefaultOption()
        {
            this.PrizeDic.Clear();
            this.PlayMethodDic.Clear();
            this.PlayMethodGroupDic.Clear();
            this.Prize = "";
            this.Rebate = "0";
            this.UserID = "";
            this.PTTagID = "";
            this.LoginUrl = "";
            this.LineIndex = -1;
            this.PTLoginStatus = false;
            this.IsLoginRun = false;
            this.IsLoadBets = true;
            this.Token = "";
            this.VerifyCodeToken = "";
            this.LoadGetSite = false;
            this.IsLoginVR = false;
            this.IsLoginZB = false;
            this.AgentKey = "";
            this.AgentValue = "";
            this.PlayerKey = "";
            this.PlayerValue = "";
        }

        public virtual void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
        }

        public virtual string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            "";

        public virtual string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false) => 
            "";

        public virtual string GetBetsExpect(string pExpect, string pLotteryID = "") => 
            "";

        public virtual string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            "";

        public virtual string GetBetsLotteryCurmid(ConfigurationStatus.LotteryType pType) => 
            "";

        public virtual string GetBetsLotteryID(ConfigurationStatus.LotteryType pType) => 
            "";

        public virtual string GetBZLine(string pLine)
        {
            string str = pLine;
            str = str.Replace("http://", "");
            str = str.Substring(0, str.IndexOf("/"));
            return ("http://" + str);
        }

        public virtual string GetHostLine()
        {
            string line = this.GetLine();
            if (line.Contains("http://"))
            {
                return line.Replace("http://", "");
            }
            if (line.Contains("https://"))
            {
                line = line.Replace("https://", "");
            }
            return line;
        }

        public virtual string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            "";

        public virtual string GetIndexLine() => 
            "";

        public virtual string GetIPLoginLine() => 
            "";

        public virtual string GetLHPlayMethodID(string playName) => 
            "";

        public virtual string GetLine()
        {
            if (this.LineIndex == -1)
            {
                return "";
            }
            return this.LineList[this.LineIndex];
        }

        public virtual string GetLineString(string pResponseText) => 
            pResponseText;

        public virtual string GetLoginLine() => 
            "";

        public virtual string GetLoginLineID() => 
            "";

        public virtual string GetLoginLinePW() => 
            "";

        public virtual string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            "";

        public virtual string GetMaxPrize(string prize, double pMaxPrize = 1950.0)
        {
            string str = prize;
            if (Convert.ToDouble(prize) > pMaxPrize)
            {
                str = pMaxPrize.ToString();
            }
            return str;
        }

        public virtual string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null) => 
            "";

        public virtual string GetNumberList2(List<string> pNumberList, string playName) => 
            "";

        public virtual string GetOpenTime() => 
            "";

        public virtual string GetPageLine() => 
            "";

        public virtual string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName) => 
            "";

        public virtual string GetPlayString(string playName) => 
            "";

        public virtual string GetPrize(ConfigurationStatus.LotteryType pType, string playName)
        {
            int num = 0;
            while (true)
            {
                if ((num >= 3) || (this.Prize != ""))
                {
                    if (this.Prize == "")
                    {
                        return "";
                    }
                    double pNum = Convert.ToDouble(this.Prize);
                    if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
                    {
                        if (playName.Contains("组三"))
                        {
                            pNum /= this.Z3Key;
                        }
                        else if (playName.Contains("组六"))
                        {
                            pNum /= this.Z6Key;
                        }
                        else if (playName.Contains("龙虎"))
                        {
                            pNum /= 450.0;
                        }
                        else
                        {
                            pNum /= 1000.0;
                            if (playName.Contains("定位胆"))
                            {
                                pNum *= 10.0;
                            }
                            else if (playName.Contains("二"))
                            {
                                pNum *= 100.0;
                            }
                            else if (playName.Contains("三"))
                            {
                                pNum *= 1000.0;
                            }
                            else if (playName.Contains("四"))
                            {
                                pNum *= 10000.0;
                            }
                            else if (playName.Contains("五"))
                            {
                                pNum *= 100000.0;
                            }
                        }
                    }
                    else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                    {
                        if (playName.Contains("一"))
                        {
                            pNum /= 454.524;
                        }
                        else if (playName.Contains("二"))
                        {
                            if (playName.Contains("任"))
                            {
                                pNum /= 181.818;
                            }
                            else
                            {
                                pNum /= 9.0909;
                            }
                        }
                        else if (playName.Contains("三"))
                        {
                            if (playName.Contains("任"))
                            {
                                pNum /= 60.6;
                            }
                            else
                            {
                                pNum /= 1.0101;
                            }
                        }
                        else if (playName.Contains("四"))
                        {
                            pNum /= 15.15;
                        }
                        else if (playName.Contains("五"))
                        {
                            pNum /= 2.165;
                        }
                    }
                    else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                    {
                        if (playName.Contains("定位胆") || playName.Contains("猜冠军猜冠军"))
                        {
                            pNum /= 100.0;
                        }
                        else if (playName.Contains("和值"))
                        {
                            if (playName.Contains("二"))
                            {
                                pNum /= 222.2222;
                            }
                            else if (playName.Contains("三"))
                            {
                                pNum /= 83.3333;
                            }
                        }
                        else if (playName.Contains("二"))
                        {
                            pNum /= 11.1111;
                        }
                        else if (playName.Contains("三"))
                        {
                            pNum /= 1.3889;
                        }
                        else if (playName.Contains("四"))
                        {
                            pNum /= 0.1984;
                        }
                        else if (playName.Contains("五"))
                        {
                            pNum /= 0.033;
                        }
                    }
                    return CommFunc.TwoDouble(pNum, true);
                }
                this.GetSite(pType, "");
                num++;
            }
        }

        public virtual string GetPTHint(string pResponseText) => 
            "";

        public virtual string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public virtual string GetPTLotteryName1(ConfigurationStatus.LotteryType pType) => 
            "";

        public virtual string GetQuitPTLine() => 
            "";

        public virtual string GetRegisterLine() => 
            "";

        public virtual string GetReturn(string pResponseText) => 
            "";

        public virtual string GetRXWZString(List<int> pRXWZ) => 
            "";

        public virtual void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
        }

        public static List<string> GetSkipUrlList(List<string> pLineList, PTBase pInfo)
        {
            List<string> list = new List<string>();
            foreach (string str in pLineList)
            {
                string item = str;
                if (item.Contains(pInfo.SkipWebLoginUrlTag))
                {
                    item = item.Replace(pInfo.SkipWebLoginUrlTag, "");
                    if (!pInfo.SkipWebLoginUrlList.Contains(item))
                    {
                        pInfo.SkipWebLoginUrlList.Add(item);
                    }
                }
                list.Add(item);
            }
            return list;
        }

        public virtual string GetTrendCode() => 
            "";

        public virtual Uri GetUrlLine() => 
            new Uri(this.GetLine());

        public virtual string GetVRHostLine() => 
            this.GetVRLine().Replace("http://", "");

        public virtual string GetVRLine() => 
            "";

        public virtual string GetWebVerifyCode(string pVerifyCodeFile) => 
            "";

        public virtual string GetZBHostLine() => 
            this.GetZBLine().Replace("http://", "");

        public virtual string GetZBLine() => 
            "";

        public bool IsBetsMoney1(ConfigurationStatus.SCUnitType pUnit)
        {
            if (this == AppInfo.WJSJInfo)
            {
                return true;
            }
            if (this == AppInfo.LFGJInfo)
            {
                return true;
            }
            if (this == AppInfo.UT8Info)
            {
                return true;
            }
            if (this == AppInfo.THDYLInfo)
            {
                return true;
            }
            if (this == AppInfo.HANYInfo)
            {
                return true;
            }
            if (this == AppInfo.YHYLInfo)
            {
                return true;
            }
            if (this == AppInfo.YBYLInfo)
            {
                return true;
            }
            if (this == AppInfo.JYGJInfo)
            {
                return true;
            }
            if (this == AppInfo.YYZXInfo)
            {
                return true;
            }
            if (this == AppInfo.M5CPInfo)
            {
                return true;
            }
            if (this == AppInfo.FSYLInfo)
            {
                return true;
            }
            if (this == AppInfo.ZDYLInfo)
            {
                return true;
            }
            if (this == AppInfo.ZYLInfo)
            {
                return true;
            }
            if (this == AppInfo.NBYLInfo)
            {
                return true;
            }
            if (this == AppInfo.WZYLInfo)
            {
                if (this.CheckLotteryIsZB(AppInfo.Current.Lottery.Type))
                {
                    return false;
                }
                return true;
            }
            if (this == AppInfo.YZCPInfo)
            {
                if (this.CheckLotteryIsZB(AppInfo.Current.Lottery.Type))
                {
                    return false;
                }
                return true;
            }
            if (this == AppInfo.DAZYLInfo)
            {
                return true;
            }
            if (this == AppInfo.K3YLInfo)
            {
                return true;
            }
            if (this == AppInfo.HYYLInfo)
            {
                return true;
            }
            if (this == AppInfo.HLCInfo)
            {
                return true;
            }
            if (this == AppInfo.HENRInfo)
            {
                return true;
            }
            if (this == AppInfo.OEYLInfo)
            {
                if (this.IsLoginVR && (pUnit == ConfigurationStatus.SCUnitType.Fen))
                {
                    return false;
                }
                return true;
            }
            if (this == AppInfo.HBSInfo)
            {
                return true;
            }
            if (this == AppInfo.DACPInfo)
            {
                return true;
            }
            if (this == AppInfo.WSYLInfo)
            {
                return true;
            }
            if (this == AppInfo.WCYLInfo)
            {
                return true;
            }
            if (this == AppInfo.WHENInfo)
            {
                return true;
            }
            if (this == AppInfo.TYYLInfo)
            {
                return true;
            }
            if (this == AppInfo.THENInfo)
            {
                return true;
            }
            if (this == AppInfo.CAIHInfo)
            {
                return true;
            }
            if (this == AppInfo.CBLInfo)
            {
                return true;
            }
            if (this == AppInfo.MTYLInfo)
            {
                return true;
            }
            if (this == AppInfo.XTYLInfo)
            {
                return true;
            }
            if (this == AppInfo.CYYLInfo)
            {
                return true;
            }
            if (this == AppInfo.CTXInfo)
            {
                return true;
            }
            if (this == AppInfo.JYYLInfo)
            {
                return true;
            }
            if (this == AppInfo.MXYLInfo)
            {
                return true;
            }
            if (this == AppInfo.YHSGInfo)
            {
                return true;
            }
            if (this == AppInfo.WCAIInfo)
            {
                return true;
            }
            if (this == AppInfo.XWYLInfo)
            {
                return true;
            }
            if (this == AppInfo.QFZXInfo)
            {
                return true;
            }
            if (this == AppInfo.DQYLInfo)
            {
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                {
                    return false;
                }
                return true;
            }
            if (this == AppInfo.FEICInfo)
            {
                return true;
            }
            if (this == AppInfo.ZBEIInfo)
            {
                return true;
            }
            if (this == AppInfo.LYSInfo)
            {
                return true;
            }
            if (this == AppInfo.SLTHInfo)
            {
                return true;
            }
            if (this == AppInfo.JHCInfo)
            {
                return true;
            }
            if (this == AppInfo.JHC2Info)
            {
                if (this.CheckLotteryIsVR(AppInfo.Current.Lottery.Type))
                {
                    return false;
                }
                return true;
            }
            if (this == AppInfo.WHCInfo)
            {
                return true;
            }
            if (this == AppInfo.RDYLInfo)
            {
                return true;
            }
            if (this == AppInfo.FLCInfo)
            {
                return true;
            }
            if (this == AppInfo.WDYLInfo)
            {
                return true;
            }
            if (this == AppInfo.HUAYInfo)
            {
                return true;
            }
            if (this == AppInfo.JCXInfo)
            {
                return true;
            }
            if (this == AppInfo.TBYLInfo)
            {
                return true;
            }
            if (this == AppInfo.YXZXInfo)
            {
                return true;
            }
            if (this == AppInfo.LUDIInfo)
            {
                if (pUnit == ConfigurationStatus.SCUnitType.Fen)
                {
                    return false;
                }
                return true;
            }
            return ((this == AppInfo.SSHCInfo) || ((this == AppInfo.WYYLInfo) || ((this == AppInfo.XDBInfo) || ((this == AppInfo.DYYLInfo) || ((this == AppInfo.QFYLInfo) || ((this == AppInfo.XQYLInfo) || ((this == AppInfo.NBAYLInfo) || ((this == AppInfo.HONDInfo) || ((this == AppInfo.YINHInfo) || ((this == AppInfo.HENDInfo) || ((this == AppInfo.XGLLInfo) || ((this == AppInfo.JXYLInfo) || ((this == AppInfo.JLGJInfo) || ((this == AppInfo.YCYLInfo) || ((this == AppInfo.TIYUInfo) || ((this == AppInfo.WTYLInfo) || ((this == AppInfo.JXINInfo) || (this == AppInfo.HDYLInfo))))))))))))))))));
        }

        public virtual string LoginLotteryWeb(ConfigurationStatus.LotteryType pType, string pInfo = "") => 
            "";

        public virtual void QuitPT()
        {
        }

        public virtual void SetLine(string pLine)
        {
            for (int i = 0; i < this.LineList.Count; i++)
            {
                if (this.LineList[i] == pLine)
                {
                    this.LineIndex = i;
                    break;
                }
            }
        }

        public virtual string SwitchNextLine()
        {
            this.LineIndex++;
            this.LineIndex = this.LineIndex % this.LineList.Count;
            return this.LineList[this.LineIndex];
        }

        public virtual bool VRWebLoginMain(ConfigurationStatus.LotteryType pType) => 
            false;

        public virtual bool WebLoginMain(string pID, string pW, ref string pHint) => 
            false;

        public virtual bool ZBWebLoginMain(ConfigurationStatus.LotteryType pType) => 
            false;

        public bool IsCombinaBets =>
            false;

        public bool IsHTLoginMain
        {
            get
            {
                if ((this == AppInfo.WXYLInfo) || (this == AppInfo.HUBOInfo))
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsLoadWebLogin =>
            (((this == AppInfo.FSYLInfo) || (this == AppInfo.LUDIInfo)) || (((this == AppInfo.LFYLInfo) || (this == AppInfo.LF2Info)) && !this.SkipWebLoginUrlList.Contains(this.LoginUrl)));

        public bool IsSkipLH
        {
            get
            {
                if (((((((this == AppInfo.LSWJSInfo) || (this == AppInfo.JYYLInfo)) || ((this == AppInfo.B6YLInfo) || (this == AppInfo.JHCInfo))) || (((this == AppInfo.JHC2Info) || (this == AppInfo.XTYLInfo)) || ((this == AppInfo.FLCInfo) || (this == AppInfo.TBYLInfo)))) || ((((this == AppInfo.WZYLInfo) || (this == AppInfo.YZCPInfo)) || ((this == AppInfo.TIYUInfo) || (this == AppInfo.YCYLInfo))) || (((this == AppInfo.ZBYLInfo) || (this == AppInfo.YXZXInfo)) || ((this == AppInfo.WTYLInfo) || (this == AppInfo.SKYYLInfo))))) || (((((this == AppInfo.JFYLInfo) || (this == AppInfo.TCYLInfo)) || ((this == AppInfo.QFZXInfo) || (this == AppInfo.JXINInfo))) || (((this == AppInfo.XQYLInfo) || (this == AppInfo.ZXYLInfo)) || ((this == AppInfo.HENDInfo) || (this == AppInfo.QQT2Info)))) || (this == AppInfo.WBJInfo))) || (this == AppInfo.HZYLInfo))
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsSkipRX =>
            ((((((this == AppInfo.YSENInfo) || (this == AppInfo.YYZXInfo)) || ((this == AppInfo.DAZYLInfo) || (this == AppInfo.FSYLInfo))) || (((this == AppInfo.A6YLInfo) || (this == AppInfo.HCZXInfo)) || ((this == AppInfo.DQYLInfo) || (this == AppInfo.ZLJInfo)))) || (this == AppInfo.AMBLRInfo)) || (this == AppInfo.YINHInfo));

        public double Z3Key =>
            3.0;

        public double Z6Key =>
            6.0;

        public string ZBString =>
            $"AgentKey={this.AgentKey}&AgentValue={this.AgentValue}&PlayerKey={this.PlayerKey}&PlayerValue={this.PlayerValue}";
    }
}

