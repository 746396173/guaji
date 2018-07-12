namespace IntelligentPlanning
{
    using IntelligentPlanning.CustomControls;
    using Microsoft.VisualBasic;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;

    public class ConfigurationStatus
    {
        public LotteryConfig Lottery;
        public Dictionary<string, LotteryConfig> LotteryDic = new Dictionary<string, LotteryConfig>();

        public ConfigurationStatus(List<string> pLotteryIDList = null)
        {
            if (pLotteryIDList == null)
            {
                pLotteryIDList = CommFunc.GetNodeAllLotteryList();
            }
            foreach (string str in pLotteryIDList)
            {
                LotteryConfig config = this.LoadLotteryConfig(str);
                this.LotteryDic[str] = config;
            }
        }

        public bool CheckTime()
        {
            bool flag = false;
            DateTime now = DateTime.Now;
            XmlElement xmlLotteryNode = this.Lottery.XmlLotteryNode as XmlElement;
            DateTime time2 = DateTime.ParseExact(xmlLotteryNode.GetAttribute("OpenTime"), "HH:mm:ss", null);
            DateTime time3 = DateTime.ParseExact(xmlLotteryNode.GetAttribute("EndTime"), "HH:mm:ss", null);
            if ((now > time2) && (now < time3))
            {
                flag = true;
            }
            return flag;
        }

        public string CheckTimeInOpenTime(ref int pIndex)
        {
            TimeSpan span;
            string str = "";
            DateTime now = DateTime.Now;
            int num = 1;
            foreach (IntervalTime time2 in this.Lottery.TimeList)
            {
                DateTime time3 = DateTime.ParseExact(time2.beginOpenTime, "HH:mm:ss", null);
                DateTime time4 = DateTime.ParseExact(time2.endOpenTime, "HH:mm:ss", null);
                if ((now > time3) && (now < time4))
                {
                    span = (TimeSpan) (time4 - now);
                    str = string.Concat(new object[] { span.Hours, ":", span.Minutes, ":", span.Seconds });
                    break;
                }
                num++;
            }
            if (str == "")
            {
                DateTime time5 = DateTime.ParseExact(this.Lottery.TimeList[0].beginOpenTime, "HH:mm:ss", null);
                if (time5 < now)
                {
                    time5 = time5.AddDays(1.0);
                }
                span = (TimeSpan) (time5 - now);
                num = 0;
                str = string.Concat(new object[] { span.Hours, ":", span.Minutes, ":", span.Seconds });
            }
            pIndex = num;
            return str;
        }

        public LotteryConfig LoadLotteryConfig(string pID)
        {
            int num;
            int num2;
            double openInterval;
            IntervalTime time2;
            LotteryConfig config = new LotteryConfig {
                XmlLotteryNode = CommFunc.GetNodeLotteryList(pID, "ID"),
                XmlTrendNode = CommFunc.GetNodeLotteryData("号码分布", "Name")
            };
            XmlElement xmlLotteryNode = config.XmlLotteryNode as XmlElement;
            config.ID = pID;
            config.Name = xmlLotteryNode.GetAttribute("Name");
            if (AppInfo.App == AppType.KXYLGJ)
            {
                if (config.ID == "BJSSC")
                {
                    config.Name = "北京时时彩";
                }
            }
            else if ((((AppInfo.App == AppType.XHSD) || (AppInfo.App == AppType.SSHCGJ)) || (AppInfo.App == AppType.DYCT)) || (AppInfo.App == AppType.LDGJ))
            {
                if (config.ID == "TXFFC")
                {
                    config.Name = "老腾讯分分彩";
                }
            }
            else if ((AppInfo.App == AppType.WYGJ) && (config.ID == "TXFFC"))
            {
                config.Name = "腾讯奇趣彩";
            }
            config.Group = CommFunc.GetLotteryGroup(config.Name, config.ID);
            if (config.Group == LotteryGroup.GPSSC)
            {
                config.GroupString = "SSC";
                config.XmlPlayNode = CommFunc.GetNodeLotteryData("时时彩", "Name");
                config.Min = 0;
                config.Max = 9;
            }
            else if (config.Group == LotteryGroup.GP11X5)
            {
                config.GroupString = "11X5";
                config.XmlPlayNode = CommFunc.GetNodeLotteryData("11选5", "Name");
                config.Min = 1;
                config.Max = 11;
            }
            else if (config.Group == LotteryGroup.GPPK10)
            {
                config.GroupString = "PK10";
                config.XmlPlayNode = CommFunc.GetNodeLotteryData("PK10", "Name");
                config.Min = 1;
                config.Max = 10;
            }
            else if (config.Group == LotteryGroup.GP3D)
            {
                config.GroupString = "3D";
                config.XmlPlayNode = CommFunc.GetNodeLotteryData("3D", "Name");
                config.Min = 0;
                config.Max = 9;
            }
            config.OpenInterval = Convert.ToDouble(xmlLotteryNode.GetAttribute("OpenInterval"));
            config.Expect = Convert.ToInt32(xmlLotteryNode.GetAttribute("Expect"));
            config.Code = Convert.ToInt32(xmlLotteryNode.GetAttribute("Code"));
            config.Type = (LotteryType) Convert.ToInt32(xmlLotteryNode.GetAttribute("TypeIndex"));
            config.RefreshExpect = Convert.ToInt32(xmlLotteryNode.GetAttribute("RefreshExpect"));
            config.SaveExpect = Convert.ToInt32(xmlLotteryNode.GetAttribute("SaveExpect"));
            DateTime time = DateTime.ParseExact(xmlLotteryNode.GetAttribute("OpenTime"), "HH:mm:ss", null);
            config.TimeList = new List<IntervalTime>();
            config.TimeDic = new Dictionary<string, string>();
            if (config.ID == "CQSSC")
            {
                num = 0x18;
                for (num2 = 0; num2 < config.Expect; num2++)
                {
                    openInterval = config.OpenInterval;
                    if ((config.ID == "CQSSC") && (num2 > 0x47))
                    {
                        openInterval = config.OpenInterval / 2.0;
                    }
                    string introduced11 = time.ToString("HH:mm:ss");
                    time2 = new IntervalTime(introduced11, time.AddMinutes(openInterval).AddSeconds(-1.0).ToString("HH:mm:ss"));
                    config.TimeList.Add(time2);
                    time = time.AddMinutes(openInterval);
                    config.TimeDic[num.ToString().PadLeft(3, '0')] = time2.beginOpenTime;
                    num = (num % 120) + 1;
                }
                return config;
            }
            num = 1;
            int lotteryExpectLen = CommFunc.GetLotteryExpectLen(config.Name, config.ID);
            for (num2 = 0; num2 < config.Expect; num2++)
            {
                openInterval = config.OpenInterval;
                string introduced12 = time.ToString("HH:mm:ss");
                time2 = new IntervalTime(introduced12, time.AddMinutes(openInterval).AddSeconds(-1.0).ToString("HH:mm:ss"));
                config.TimeList.Add(time2);
                time = time.AddMinutes(openInterval);
                if (config.Name.Contains("经纬") && (((time.Hour == 5) && (time.Minute == 1)) && (time.Second == 0)))
                {
                    time = time.AddMinutes(120.0);
                }
                config.TimeDic[num.ToString().PadLeft(lotteryExpectLen, '0')] = time2.beginOpenTime;
                num++;
            }
            return config;
        }

        public static void SetRangeList(List<int> pCodeList, ref List<int> pRangeList)
        {
            if ((pRangeList.Count == 0) && (pCodeList.Count != 0))
            {
                int num = 1;
                int count = pCodeList.Count;
                for (int i = num; i <= count; i++)
                {
                    pRangeList.Add(i);
                }
            }
        }

        public delegate bool AnalysisVerifyCodeDelegate(string pName, ref int pCount);

        public class AppConfiguration
        {
            public string AppTag = "";
            public const string AppTagString = "标记";
            public string AppText = "";
            public const string AppTextString = "AppText";
            public string BackColor = "";
            public const string BackColorString = "BackColor";
            public string BeaBackColor = "";
            public const string BeaBackColorString = "BeaBackColor";
            public string BeaForeColor = "";
            public const string BeaForeColorString = "BeaForeColor";
            public string BeaHotColor = "";
            public const string BeaHotColorString = "BeaHotColor";
            public bool Beautify = false;
            public const string BeautifyString = "Beautify";
            public string BetsBeginHint = "";
            public const string BetsBeginHintString = "BetsBeginHint";
            public string BetsEndHint = "";
            public const string BetsEndHintString = "BetsEndHint";
            public string BetsHint = "";
            public const string BetsHintString = "BetsHint";
            public bool BTPTUser = false;
            public const string BTPTUserString = "BTPTUser";
            public string CompanyKey = "";
            public const string CompanyString = "代理ID";
            public string CompanyValue = "";
            public string DefaultFore = "";
            public const string DefaultForeString = "defaultFore";
            public Dictionary<string, string> DownloadLinkDic = new Dictionary<string, string>();
            public const string DownloadLinkString = "DownloadLink";
            public bool FixAppText = false;
            public const string FixAppTextString = "FixAppText";
            public string FNEdit = "";
            public Dictionary<string, string> FNEditDic = new Dictionary<string, string>();
            public const string FNEditString = "FNEdit";
            public List<string> FNEncrypIDList = new List<string>();
            public string FNEncrypt = "";
            public const string FNEncryptString = "FNEncrypt";
            public int FNUsed = 0;
            public const string FNUsedString = "FNUsed";
            public string ForeColor = "";
            public const string ForeColorString = "ForeColor";
            public int FreeDay = 0;
            public const string FreeDayString = "FreeDay";
            public string FreeHint = "";
            public const string FreeHintString = "FreeHint";
            public int FreeHour = 0;
            public const string FreeHourString = "FreeHour";
            public List<string> GJBTEncrypIDList = new List<string>();
            public string GJBTEncrypt = "";
            public const string GJBTEncryptString = "GJBTEncrypt";
            public bool HideLSTJ = false;
            public const string HideLSTJString = "HideLSTJ";
            public bool HidePhone = false;
            public const string HidePhoneString = "HidePhone";
            public string HotColor = "";
            public const string HotColorString = "HotColor";
            public string ImageLink = "";
            public Dictionary<string, string> ImageLinkDic = new Dictionary<string, string>();
            public const string ImageLinkString = "ImageLink";
            public List<Image> ImageList = new List<Image>();
            public bool IsHideSetUp = false;
            public const string IsHideSetUpString = "IsHideSetUp";
            public bool IsNotice = false;
            public const string IsNoticeString = "IsNotice";
            public bool IsPTLogin = false;
            public const string IsPTLoginString = "IsPTLogin";
            public bool IsWJApp = false;
            public const string IsWJAppString = "IsWJApp";
            public List<string> LoginPTList = null;
            public const string LoginPTListString = "LoginPTList";
            public bool LoginQQ = true;
            public const string LoginQQString = "LoginQQ";
            public const string LoginWebHintString = "LoginWebHint";
            public string LoginWebKey = "";
            public string LoginWebValue = "";
            public string MaxPrize = "";
            public const string MaxPrizeString = "MaxPrize";
            public int MaxSharePlanCount = -1;
            public const string MaxSharePlanCountString = "MaxSharePlanCount";
            public List<ConfigurationStatus.MoreAppData> MoreAppList = new List<ConfigurationStatus.MoreAppData>();
            public const string MoreAppString = "MoreApp";
            public bool OnlyLogin = false;
            public string OnlyLoginHint = "";
            public const string OnlyLoginHintString = "OnlyLoginHint";
            public string OnlyLoginImage = "";
            public const string OnlyLoginImageString = "OnlyLoginImage";
            public const string OnlyLoginString = "仅登录";
            public string OpenDataUrl = "";
            public const string OpenDataUrlString = "OpenDataUrl";
            public string QQ = "";
            public string QQGroup = "";
            public const string QQGroupString = "QQGroup";
            public string QQHint = "";
            public const string QQHintString = "QQHint";
            public string QQName = "";
            public const string QQNameString = "QQName";
            public const string QQString = "QQ";
            public string RegisterHint = "";
            public const string RegisterHintString = "RegisterHint";
            public string RemoteUrl = "";
            public const string RemoteUrlString = "RemoteUrl";
            public string ScrollText = "";
            public const string ScrollTextString = "ScrollText";
            public bool StopLogin = false;
            public string StopLoginHint = "";
            public const string StopLoginHintString = "StopLoginHint";
            public const string StopLoginString = "StopLogin";
            public string SwitchHint = "";
            public const string SwitchHintString = "SwitchHint";
            public string SwitchUrl = "";
            public const string SwitchUrlString = "SwitchUrl";
            public bool ViewAppName = true;
            public const string ViewAppNameString = "ViewAppName";
            public bool ViewQQPhone = false;
            public const string ViewQQPhoneString = "ViewQQPhone";
            public string WebHint = "";
            public const string WebHintString = "WebHint";
            public string WebUrl = "";
            public const string WebUrlString = "WebUrl";
            public string ZBJHint = "";
            public const string ZBJHintString = "ZBJHint";
            public string ZBJUrl = "";
            public const string ZBJUrlString = "ZBJUrl";

            public string GetLoginPTListViewString(List<string> pLoginPTList)
            {
                if (pLoginPTList == null)
                {
                    return "";
                }
                return CommFunc.Join(pLoginPTList, "，");
            }

            public void LoadAppConfiguration(Dictionary<string, string> pDic)
            {
                foreach (string str in pDic.Keys)
                {
                    string str2 = "";
                    string str3 = "";
                    switch (str)
                    {
                        case "标记":
                        {
                            this.AppTag = pDic[str];
                            continue;
                        }
                        case "代理ID":
                        {
                            if (!pDic[str].Contains("-"))
                            {
                                break;
                            }
                            this.CompanyKey = pDic[str].Split(new char[] { '-' })[0];
                            this.CompanyValue = pDic[str].Split(new char[] { '-' })[1];
                            continue;
                        }
                        case "QQ":
                        {
                            this.QQ = pDic[str];
                            continue;
                        }
                        case "QQHint":
                        {
                            this.QQHint = pDic[str];
                            continue;
                        }
                        case "QQGroup":
                        {
                            this.QQGroup = pDic[str];
                            continue;
                        }
                        case "QQName":
                        {
                            this.QQName = pDic[str];
                            continue;
                        }
                        case "LoginQQ":
                        {
                            this.LoginQQ = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "AppText":
                        {
                            this.AppText = pDic[str];
                            continue;
                        }
                        case "WebUrl":
                        {
                            this.WebUrl = pDic[str];
                            continue;
                        }
                        case "ZBJUrl":
                        {
                            this.ZBJUrl = pDic[str];
                            continue;
                        }
                        case "ZBJHint":
                        {
                            this.ZBJHint = pDic[str];
                            continue;
                        }
                        case "ImageLink":
                        {
                            this.ImageLink = pDic[str];
                            continue;
                        }
                        case "FreeDay":
                        {
                            this.FreeDay = Convert.ToInt32(pDic[str]);
                            continue;
                        }
                        case "FreeHint":
                        {
                            this.FreeHint = pDic[str];
                            continue;
                        }
                        case "FreeHour":
                        {
                            this.FreeHour = Convert.ToInt32(pDic[str]);
                            continue;
                        }
                        case "BTPTUser":
                        {
                            this.BTPTUser = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "LoginPTList":
                        {
                            this.LoginPTList = CommFunc.SplitString(pDic[str], "|", -1);
                            continue;
                        }
                        case "FNUsed":
                        {
                            this.FNUsed = Convert.ToInt32(pDic[str]);
                            continue;
                        }
                        case "IsNotice":
                        {
                            this.IsNotice = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "FNEdit":
                        {
                            this.FNEdit = pDic[str];
                            this.FNEditDic = CommFunc.ConvertConfiguration(CommFunc.SplitString(pDic[str], ";", -1), '=');
                            continue;
                        }
                        case "IsPTLogin":
                        {
                            this.IsPTLogin = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "ViewQQPhone":
                        {
                            this.ViewQQPhone = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "HidePhone":
                        {
                            this.HidePhone = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "ViewAppName":
                        {
                            this.ViewAppName = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "HideLSTJ":
                        {
                            this.HideLSTJ = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "SwitchHint":
                        {
                            this.SwitchHint = pDic[str];
                            continue;
                        }
                        case "SwitchUrl":
                        {
                            this.SwitchUrl = pDic[str];
                            continue;
                        }
                        case "OpenDataUrl":
                        {
                            this.OpenDataUrl = pDic[str];
                            continue;
                        }
                        case "FNEncrypt":
                        {
                            this.FNEncrypt = pDic[str].Split(new char[] { '-' })[0];
                            this.FNEncrypIDList = CommFunc.SplitString(pDic[str].Split(new char[] { '-' })[1], ";", -1);
                            continue;
                        }
                        case "GJBTEncrypt":
                        {
                            this.GJBTEncrypt = pDic[str].Split(new char[] { '-' })[0];
                            this.GJBTEncrypIDList = CommFunc.SplitString(pDic[str].Split(new char[] { '-' })[1], ";", -1);
                            continue;
                        }
                        case "IsHideSetUp":
                        {
                            this.IsHideSetUp = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "IsWJApp":
                        {
                            this.IsWJApp = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "BackColor":
                        {
                            this.BackColor = pDic[str];
                            continue;
                        }
                        case "ForeColor":
                        {
                            this.ForeColor = pDic[str];
                            continue;
                        }
                        case "defaultFore":
                        {
                            this.DefaultFore = pDic[str];
                            continue;
                        }
                        case "BeaBackColor":
                        {
                            this.BeaBackColor = pDic[str];
                            continue;
                        }
                        case "BeaForeColor":
                        {
                            this.BeaForeColor = pDic[str];
                            continue;
                        }
                        case "BeaHotColor":
                        {
                            this.BeaHotColor = pDic[str];
                            continue;
                        }
                        case "Beautify":
                        {
                            this.Beautify = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "HotColor":
                        {
                            this.HotColor = pDic[str];
                            continue;
                        }
                        case "StopLogin":
                        {
                            this.StopLogin = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "FixAppText":
                        {
                            this.FixAppText = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "MaxPrize":
                        {
                            this.MaxPrize = pDic[str];
                            continue;
                        }
                        case "WebHint":
                        {
                            this.WebHint = pDic[str];
                            continue;
                        }
                        case "RemoteUrl":
                        {
                            this.RemoteUrl = pDic[str];
                            continue;
                        }
                        case "StopLoginHint":
                        {
                            this.StopLoginHint = pDic[str];
                            continue;
                        }
                        case "MoreApp":
                        {
                            List<string> list = CommFunc.SplitString(pDic[str], ";", -1);
                            string str4 = CommFunc.getDllPath();
                            foreach (string str5 in list)
                            {
                                ConfigurationStatus.MoreAppData item = new ConfigurationStatus.MoreAppData();
                                string[] strArray = Strings.Split(str5, "-", -1, CompareMethod.Binary);
                                item.Name = strArray[0];
                                string path = str4 + @"\" + item.Name;
                                if (Directory.Exists(path))
                                {
                                    item.File = path + @"\" + strArray[1] + ".exe";
                                    item.Remark = strArray[2];
                                    this.MoreAppList.Add(item);
                                }
                            }
                            continue;
                        }
                        case "DownloadLink":
                        {
                            this.DownloadLinkDic = CommFunc.ConvertConfiguration(pDic[str], '-');
                            continue;
                        }
                        case "仅登录":
                        {
                            this.OnlyLogin = Convert.ToBoolean(pDic[str]);
                            continue;
                        }
                        case "OnlyLoginImage":
                        {
                            this.OnlyLoginImage = pDic[str];
                            continue;
                        }
                        case "OnlyLoginHint":
                        {
                            this.OnlyLoginHint = pDic[str];
                            continue;
                        }
                        case "BetsHint":
                        {
                            this.BetsHint = pDic[str];
                            continue;
                        }
                        case "RegisterHint":
                        {
                            this.RegisterHint = pDic[str];
                            continue;
                        }
                        case "LoginWebHint":
                        {
                            this.LoginWebKey = pDic[str].Split(new char[] { '-' })[0];
                            this.LoginWebValue = pDic[str].Substring(pDic[str].IndexOf('-') + 1);
                            continue;
                        }
                        case "ScrollText":
                        {
                            this.ScrollText = pDic[str];
                            continue;
                        }
                        case "BetsBeginHint":
                        {
                            this.BetsBeginHint = pDic[str];
                            continue;
                        }
                        case "BetsEndHint":
                        {
                            this.BetsEndHint = pDic[str];
                            continue;
                        }
                        case "MaxSharePlanCount":
                        {
                            this.MaxSharePlanCount = Convert.ToInt32(pDic[str]);
                            continue;
                        }
                        default:
                            goto Label_0AC6;
                    }
                    this.CompanyKey = pDic[str];
                    continue;
                Label_0AC6:
                    if (str.Contains("ImageLink"))
                    {
                        str2 = pDic[str].Split(new char[] { '-' })[0];
                        str3 = pDic[str].Substring(pDic[str].IndexOf('-') + 1);
                        this.ImageLinkDic[str2] = str3;
                    }
                }
            }

            public void SaveConfiguration(string pFile)
            {
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["代理ID"] = this.CompanyID,
                    ["标记"] = this.AppTag,
                    ["仅登录"] = this.OnlyLogin.ToString()
                };
                CommFunc.WriteTextFile(pFile, pDic);
            }

            public string CompanyID
            {
                get
                {
                    if (this.CompanyValue == "")
                    {
                        return this.CompanyKey;
                    }
                    return (this.CompanyKey + "-" + this.CompanyValue);
                }
            }

            public string DLConfiguration
            {
                get
                {
                    List<string> pList = new List<string> {
                        "QQ=" + this.QQ,
                        "QQGroup=" + this.QQGroup,
                        "FNEdit=" + this.FNEdit,
                        "ImageLink=" + this.ImageLink,
                        "WebUrl=" + this.WebUrl,
                        "LoginPTList=" + CommFunc.Join(this.LoginPTList, "|")
                    };
                    return CommFunc.Join(pList, "&");
                }
                set
                {
                    Dictionary<string, string> pDic = CommFunc.ConvertConfiguration(CommFunc.SplitString(value, "&", -1), '=');
                    this.LoadAppConfiguration(pDic);
                }
            }

            public bool IsFNEncrypt =>
                (this.FNEncrypt != "");

            public bool IsFNEncryptID =>
                this.FNEncrypIDList.Contains(AppInfo.Account.ID);

            public bool IsGJBTEncrypt =>
                (this.GJBTEncrypt != "");

            public bool IsGJBTEncryptID =>
                this.GJBTEncrypIDList.Contains(AppInfo.Account.ID);

            public bool IsHideMNBets =>
                false;

            public bool IsSendUserID =>
                this.FNEncrypIDList.Contains(AppInfo.Account.SendUserID);

            public string LoginPTListViewString
            {
                get => 
                    this.GetLoginPTListViewString(this.LoginPTList);
                set
                {
                    string pStr = value.Replace(",", "，");
                    this.LoginPTList = CommFunc.SplitString(pStr, "，", -1);
                }
            }
        }

        public enum AppType
        {
            DEBUG,
            AutoBets,
            Manage,
            OpenData,
            CSCGJ,
            ZZJT,
            LHLM,
            NRLM,
            HRCP,
            MDLM,
            JDGJ,
            SIJIGJ,
            JCLM3,
            YDGJ,
            BNGJ,
            BMGJ,
            YFENG,
            DPCGJ,
            CXG3,
            QQTGJ,
            WBJGJ,
            THDGJ,
            XH3GJ,
            NBGJ,
            YSENGJ,
            WMGJ,
            YRYLGJ,
            TAGJ,
            BKCGJ,
            MINCGJ,
            HZGJ,
            ZXGJ,
            UT8GJ,
            DQGJ,
            XCGJ,
            SKYGJ,
            FCGJ,
            FEICGJ,
            LDGJ,
            OEGJ,
            ZYIN,
            MZCGJ,
            LMHGJ,
            HANYGJ,
            XHGJ,
            YBAOGJ,
            CAIHGJ,
            LYSGJ,
            THGJ,
            HSGJ,
            DAZGJ,
            SLTHGJ,
            RDYLGJ,
            K3GJ,
            JFYLGJ,
            WSGJ,
            FLCGJ,
            CAITTGJ,
            JHCGJ,
            KSGJ,
            WDCD,
            QJCGJ,
            HNYLGJ,
            BWTGJ,
            WHCGJ,
            ZYLGJ,
            CBLGJ,
            YL28,
            CLYLGJ,
            THEN,
            LDYLGJ,
            HKCGJ,
            SYYLGJ,
            MTYLGJ,
            HENRGJ,
            LGZXGJ,
            WHEN,
            JHC2GJ,
            HUIZGJ,
            HDYLGJ,
            ALGJGJ,
            HDGJ,
            KYYLGJ,
            GJYLGJ,
            CCGJ,
            CTXGJ,
            KXYLGJ,
            JCXGJ,
            ZLJGJ,
            LSWJSGJ,
            HCYLGJ,
            SSHCGJ,
            DYCT,
            XHSD,
            TRGJ,
            HCZXGJ,
            BHGJ,
            XDBGJ,
            DJGJ,
            DYGJ,
            WDGJ,
            HONDGJ,
            QFGJ,
            TYGJ,
            AMGJ,
            JXGJ,
            XINCGJ,
            YHGJ,
            CYYLGJ,
            BLGJ,
            YBGJ,
            JYGJ,
            WCGJ,
            WYGJ,
            XHHCGJ,
            NBAGJ,
            WEGJ,
            MCGJ,
            MXGJ,
            WCAIGJ,
            QQGJ,
            QIQUGJ,
            YHSGGJ,
            YINHGJ,
            XGLLGJ,
            HENDGJ,
            DEJIGJ,
            JLGJ,
            XTYLGJ,
            XWYLGJ,
            B6YLGJ,
            TBYLGJ,
            WZGJ,
            YZCPGJ,
            TIYUGJ,
            YCYLGJ,
            ZBYLGJ,
            FNYXGJ,
            HUAYGJ,
            YXZXGJ,
            WTYLGJ,
            TCYLGJ,
            QFZXGJ,
            ZBEIGJ,
            JXINGJ,
            XQYLGJ
        }

        public class AutoBets
        {
            public string BeginTime;
            public int BetsNumber;
            public Dictionary<string, ConfigurationStatus.BetsScheme> BetsSchemeDic;
            public int BetsTime;
            public bool DQStopBets;
            public string EndTime;
            public ConfigurationStatus.StopBetsType EndType;
            public string ErrorState;
            public string Expect;
            public Dictionary<string, Color> ExpectBackColorDic;
            public Dictionary<string, double> FNGainSaveDic;
            public bool IsBetsTime;
            public bool IsDQ;
            public bool IsOutLoop;
            public bool IsSDBetsYes;
            public bool IsSelectMN;
            public bool IsSleepTime;
            public bool IsStopAddFN;
            public string KSStopMoney;
            public string LotteryID;
            public string LotteryName;
            public int MaxLimit;
            public string MNBetsMoney1;
            public string MNBetsMoney2;
            public string Name;
            public Dictionary<string, List<string>> NumberDic;
            public List<ConfigurationStatus.SCPlan> PlanList;
            public bool PlanRun;
            public List<ConfigurationStatus.SCPlan> PlanViewList;
            public string Play;
            public string PTState;
            public bool SBStopBets;
            public ConfigurationStatus.ShareBets ShareBetsInfo;
            public bool StartBets;
            public ConfigurationStatus.SCTimesCount Times;
            public string TZFNName;
            public int UnitIndex;
            public ConfigurationStatus.BetsYKZS YKZS;
            public string YLStopMoney;
            public string ZSBetsMoney1;
            public string ZSBetsMoney2;

            public AutoBets()
            {
                this.Name = "";
                this.PlanRun = false;
                this.PlanList = new List<ConfigurationStatus.SCPlan>();
                this.PlanViewList = new List<ConfigurationStatus.SCPlan>();
                this.BetsSchemeDic = new Dictionary<string, ConfigurationStatus.BetsScheme>();
                this.ShareBetsInfo = null;
                this.StartBets = false;
                this.PTState = "";
                this.ErrorState = "";
                this.BetsNumber = 0;
                this.IsOutLoop = false;
                this.SBStopBets = true;
                this.DQStopBets = true;
                this.IsDQ = false;
                this.TZFNName = "";
                this.IsSDBetsYes = false;
                this.ExpectBackColorDic = new Dictionary<string, Color>();
                this.BetsTime = -1;
                this.IsSleepTime = false;
                this.BeginTime = "";
                this.EndTime = "";
                this.IsStopAddFN = false;
                this.EndType = ConfigurationStatus.StopBetsType.Bets;
                this.IsBetsTime = false;
                this.MNBetsMoney1 = "";
                this.MNBetsMoney2 = "";
                this.ZSBetsMoney1 = "";
                this.ZSBetsMoney2 = "";
                this.IsSelectMN = false;
                this.MaxLimit = 0;
                this.YLStopMoney = "";
                this.KSStopMoney = "";
                this.YKZS = ConfigurationStatus.BetsYKZS.AppGain;
                this.LotteryName = "";
                this.LotteryID = "";
                this.Expect = "";
                this.Play = "";
                this.UnitIndex = 0;
                this.NumberDic = new Dictionary<string, List<string>>();
                this.Times = null;
                this.FNGainSaveDic = new Dictionary<string, double>();
            }

            public AutoBets(string pName)
            {
                this.Name = "";
                this.PlanRun = false;
                this.PlanList = new List<ConfigurationStatus.SCPlan>();
                this.PlanViewList = new List<ConfigurationStatus.SCPlan>();
                this.BetsSchemeDic = new Dictionary<string, ConfigurationStatus.BetsScheme>();
                this.ShareBetsInfo = null;
                this.StartBets = false;
                this.PTState = "";
                this.ErrorState = "";
                this.BetsNumber = 0;
                this.IsOutLoop = false;
                this.SBStopBets = true;
                this.DQStopBets = true;
                this.IsDQ = false;
                this.TZFNName = "";
                this.IsSDBetsYes = false;
                this.ExpectBackColorDic = new Dictionary<string, Color>();
                this.BetsTime = -1;
                this.IsSleepTime = false;
                this.BeginTime = "";
                this.EndTime = "";
                this.IsStopAddFN = false;
                this.EndType = ConfigurationStatus.StopBetsType.Bets;
                this.IsBetsTime = false;
                this.MNBetsMoney1 = "";
                this.MNBetsMoney2 = "";
                this.ZSBetsMoney1 = "";
                this.ZSBetsMoney2 = "";
                this.IsSelectMN = false;
                this.MaxLimit = 0;
                this.YLStopMoney = "";
                this.KSStopMoney = "";
                this.YKZS = ConfigurationStatus.BetsYKZS.AppGain;
                this.LotteryName = "";
                this.LotteryID = "";
                this.Expect = "";
                this.Play = "";
                this.UnitIndex = 0;
                this.NumberDic = new Dictionary<string, List<string>>();
                this.Times = null;
                this.FNGainSaveDic = new Dictionary<string, double>();
                this.Name = pName;
            }

            public double AllGain(bool pIsMN, bool isWait = false)
            {
                double num = 0.0;
                foreach (ConfigurationStatus.SCPlan plan in this.PlanViewList)
                {
                    if (!plan.IsNull && (!isWait || plan.CheckPlanIsWait()))
                    {
                        if (pIsMN)
                        {
                            if (plan.IsMNState("", true))
                            {
                                num += plan.Gain;
                            }
                        }
                        else if (!plan.IsMNState("", true))
                        {
                            num += plan.Gain;
                        }
                    }
                }
                return num;
            }

            public double AllMoney(bool pIsMN, bool isWait = false)
            {
                double num = 0.0;
                foreach (ConfigurationStatus.SCPlan plan in this.PlanViewList)
                {
                    if (!plan.IsNull && (!isWait || plan.CheckPlanIsWait()))
                    {
                        if (pIsMN)
                        {
                            if (plan.IsMNState("", true))
                            {
                                num += plan.FNAutoTotalMoney;
                            }
                        }
                        else if (!plan.IsMNState("", true))
                        {
                            num += plan.FNAutoTotalMoney;
                        }
                    }
                }
                return num;
            }

            public static ConfigurationStatus.BetsCode ConvertBetsCode(Dictionary<string, List<string>> pDic)
            {
                List<string> pIndexList = new List<string>();
                List<string> pCodeList = new List<string>();
                foreach (string str in pDic.Keys)
                {
                    if (!pIndexList.Contains(str))
                    {
                        pIndexList.Add(str);
                    }
                    List<string> list3 = pDic[str];
                    foreach (string str2 in list3)
                    {
                        if (!pCodeList.Contains(str2))
                        {
                            pCodeList.Add(str2);
                        }
                    }
                }
                return new ConfigurationStatus.BetsCode(pCodeList, pIndexList);
            }

            public void CountAutoTimes(ConfigurationStatus.BetsScheme pScheme, ConfigurationStatus.SCPlan plan)
            {
                try
                {
                    string fNName = plan.FNName;
                    if (this.BetsSchemeDic.ContainsKey(fNName))
                    {
                        this.BetsSchemeDic[fNName].CountAutoTimes(this, pScheme, plan);
                    }
                }
                catch
                {
                }
            }

            public void DefaultOption(bool pAll)
            {
                this.PTState = "";
                this.ErrorState = "";
                this.IsOutLoop = false;
                this.IsSDBetsYes = false;
                this.IsBetsTime = false;
                this.BetsNumber = 0;
                this.YLStopMoney = "";
                this.KSStopMoney = "";
                this.BeginTime = "";
                this.EndTime = "";
                this.EndType = ConfigurationStatus.StopBetsType.Bets;
                this.IsStopAddFN = false;
                this.MNBetsMoney1 = "";
                this.ZSBetsMoney1 = "";
                this.MNBetsMoney2 = "";
                this.ZSBetsMoney2 = "";
                foreach (string str in this.BetsSchemeDic.Keys)
                {
                    this.BetsSchemeDic[str].DefaultOption(pAll);
                }
                if (pAll)
                {
                    this.StartBets = false;
                    this.PlanRun = false;
                    this.BetsTime = -1;
                    this.IsSleepTime = false;
                    this.IsDQ = false;
                    this.LotteryName = "";
                    this.LotteryID = "";
                    this.Expect = "";
                    this.Play = "";
                    this.UnitIndex = 0;
                    this.TZFNName = "";
                    this.DefaultTimes(true);
                    this.BetsSchemeDic.Clear();
                    this.ShareBetsInfo = null;
                }
            }

            public void DefaultTimes(bool pAll)
            {
                foreach (string str in this.BetsSchemeDic.Keys)
                {
                    this.BetsSchemeDic[str].DefaultTimes(pAll);
                }
            }

            public int ExpectCount()
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (ConfigurationStatus.SCPlan plan in this.PlanViewList)
                {
                    dictionary[plan.CurrentExpect] = "";
                }
                return dictionary.Count;
            }

            public double Gain(bool pIsMN)
            {
                if (this.PlanViewList.Count == 0)
                {
                    return 0.0;
                }
                ConfigurationStatus.SCPlan getLastPlan = this.GetLastPlan;
                if (getLastPlan.IsNull)
                {
                    return 0.0;
                }
                double totalFNGain = 0.0;
                if (pIsMN)
                {
                    totalFNGain = getLastPlan.GetTotalFNGain(true);
                }
                else
                {
                    totalFNGain = getLastPlan.GetTotalFNGain(false);
                }
                return totalFNGain;
            }

            public List<string> GetCurrentState() => 
                new List<string> { 
                    ("Time=" + DateTime.Now.ToString()),
                    ("ErrorState=" + this.ErrorState),
                    ("PTState=" + this.PTState),
                    ("IsOutLoop=" + this.IsOutLoop.ToString()),
                    ("IsBetsYes=" + this.IsBetsYes.ToString()),
                    ("PlanRun=" + this.PlanRun.ToString()),
                    ("BetsTime=" + this.BetsTime.ToString()),
                    ("IsSleepTime=" + this.IsSleepTime.ToString()),
                    ("YLStopBets=" + this.YLStopMoney),
                    ("KSStopBets=" + this.KSStopMoney),
                    ("LotteryID=" + this.LotteryID),
                    ("Expect=" + this.Expect),
                    ("Play=" + this.Play),
                    ("UnitIndex=" + this.UnitIndex.ToString())
                };

            public double Money(bool pIsMN)
            {
                if (this.PlanViewList.Count == 0)
                {
                    return 0.0;
                }
                ConfigurationStatus.SCPlan getLastPlan = this.GetLastPlan;
                if (getLastPlan.IsNull)
                {
                    return 0.0;
                }
                double totalFNMoney = 0.0;
                if (pIsMN)
                {
                    totalFNMoney = getLastPlan.GetTotalFNMoney(true);
                }
                else
                {
                    totalFNMoney = getLastPlan.GetTotalFNMoney(false);
                }
                return (totalFNMoney + this.AllMoney(pIsMN, true));
            }

            public int BetsCount
            {
                get
                {
                    int num = 0;
                    foreach (ConfigurationStatus.SCPlan plan in this.PlanList)
                    {
                        if (!plan.CheckPlanIsWait())
                        {
                            num++;
                        }
                    }
                    return num;
                }
            }

            public string GetKey =>
                (this.LotteryID + this.Expect);

            public ConfigurationStatus.SCPlan GetLastPlan
            {
                get
                {
                    if (this.PlanList.Count == 0)
                    {
                        return null;
                    }
                    return this.PlanList[this.PlanList.Count - 1];
                }
            }

            public bool IsBetsYes
            {
                get
                {
                    if (this.BetsSchemeDic.Count == 0)
                    {
                        return false;
                    }
                    bool flag = true;
                    foreach (string str in this.BetsSchemeDic.Keys)
                    {
                        ConfigurationStatus.BetsScheme scheme = this.BetsSchemeDic[str];
                        if (!scheme.ExpectList.Contains(this.Expect))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if ((this.ShareBetsInfo.BetsTypeInfo == ConfigurationStatus.BetsType.FollowBets) && !((this.ShareBetsInfo.FollowErrorIndexList.Count <= 0) && this.ShareBetsInfo.FollowYes))
                    {
                        flag = false;
                    }
                    return flag;
                }
            }

            public int MaxLGCount
            {
                get
                {
                    int num = 0;
                    int num2 = 0;
                    foreach (ConfigurationStatus.SCPlan plan in this.PlanList)
                    {
                        if (!plan.CheckPlanIsWait())
                        {
                            if (plan.CheckPlanStringIsWIn())
                            {
                                if (num > num2)
                                {
                                    num2 = num;
                                }
                                num = 0;
                            }
                            else
                            {
                                num++;
                            }
                        }
                    }
                    if (num > num2)
                    {
                        num2 = num;
                    }
                    num = 0;
                    return num2;
                }
            }

            public int MaxLZCount
            {
                get
                {
                    int num = 0;
                    int num2 = 0;
                    foreach (ConfigurationStatus.SCPlan plan in this.PlanList)
                    {
                        if (!plan.CheckPlanIsWait())
                        {
                            if (plan.CheckPlanStringIsWIn())
                            {
                                num++;
                            }
                            else
                            {
                                if (num > num2)
                                {
                                    num2 = num;
                                }
                                num = 0;
                            }
                        }
                    }
                    if (num > num2)
                    {
                        num2 = num;
                    }
                    num = 0;
                    return num2;
                }
            }

            public ConfigurationStatus.SCPlan Plan
            {
                get
                {
                    ConfigurationStatus.SCPlan plan = null;
                    if (this.PlanList.Count > 0)
                    {
                        ConfigurationStatus.SCPlan plan2 = this.PlanList[this.PlanList.Count - 1];
                        if (plan2.CheckPlanIsWait())
                        {
                            plan = plan2;
                        }
                    }
                    return plan;
                }
            }

            public int YesCount
            {
                get
                {
                    int num = 0;
                    foreach (ConfigurationStatus.SCPlan plan in this.PlanList)
                    {
                        if (plan.CheckPlanStringIsWIn())
                        {
                            num++;
                        }
                    }
                    return num;
                }
            }

            public string ZQL =>
                CommFunc.CountRatio(this.YesCount, this.BetsCount, "%", 2);
        }

        public delegate void BankRefreshDelegate();

        public class BetsCode
        {
            public List<string> CodeList = new List<string>();
            public List<string> IndexList = new List<string>();
            public bool IsWin = false;

            public BetsCode(List<string> pCodeList, List<string> pIndexList = null)
            {
                this.CodeList = CommFunc.CopyList(pCodeList);
                if (pIndexList != null)
                {
                    this.IndexList = CommFunc.CopyList(pIndexList);
                }
            }
        }

        public class BetsFNTotal
        {
            public Dictionary<string, int> LGDic = new Dictionary<string, int>();
            public Dictionary<string, int> LGMaxDic = new Dictionary<string, int>();
            public Dictionary<string, int> LZDic = new Dictionary<string, int>();
            public Dictionary<string, int> LZMaxDic = new Dictionary<string, int>();
        }

        public delegate bool BetsMainDelegate(ConfigurationStatus.AutoBets pBets);

        public delegate void BetsRefreshDelegate(bool RefreshAll);

        public class BetsScheme
        {
            public DateTime DSJEndBetsTime = DateTime.MinValue;
            public List<string> ExpectList = new List<string>();
            public Dictionary<string, int> FNBTIndexDic = new Dictionary<string, int>();
            public Dictionary<string, ConfigurationStatus.BetsCode> FNNumberDic = new Dictionary<string, ConfigurationStatus.BetsCode>();
            public bool IsBetsYes = false;
            public bool IsMNBets = false;
            public bool IsStopAddFN = false;
            public bool IsStopJK = false;
            public ConfigurationStatus.LSData LSDataInfo = null;
            public Dictionary<string, List<string>> PTNumberDic = new Dictionary<string, List<string>>();
            public ConfigurationStatus.Scheme SchemeInfo = null;
            public bool StartBets = false;
            public ConfigurationStatus.SCTimesCount Times = null;
            public Dictionary<string, int> ZuKeyDic = new Dictionary<string, int>();
            public bool ZuKeyIsChange = true;
            public List<string> ZuKeySaveList = new List<string>();

            public BetsScheme()
            {
                this.DefaultTimes(true);
            }

            public static List<string> ConvertBetsCode(ConfigurationStatus.SCPlan plan, ConfigurationStatus.BetsCode pBetsCode, string pZuKey)
            {
                List<string> list = new List<string>();
                foreach (string str in pBetsCode.CodeList)
                {
                    List<string> pList = new List<string> {
                        str,
                        plan.Mode.ToString(),
                        pZuKey
                    };
                    string item = CommFunc.Join(pList, "|");
                    list.Add(item);
                }
                return list;
            }

            public Dictionary<string, List<string>> ConvertFNBetsCode(ConfigurationStatus.AutoBets pBets, ConfigurationStatus.SCPlan plan, ConfigurationStatus.BetsScheme pScheme, Dictionary<string, ConfigurationStatus.BetsCode> pFNNumberDic)
            {
                plan.AutoTimesString = pScheme.GetBTIndexString;
                Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
                foreach (string str in pFNNumberDic.Keys)
                {
                    ConfigurationStatus.BetsCode pBetsCode = pFNNumberDic[str];
                    dictionary[str] = ConvertBetsCode(plan, pBetsCode, str);
                }
                return dictionary;
            }

            public void CountAutoTimes(ConfigurationStatus.AutoBets pBets, ConfigurationStatus.BetsScheme pScheme, ConfigurationStatus.SCPlan plan)
            {
                if (((pScheme.Times != null) && (pScheme.SchemeInfo != null)) && (pScheme.FNNumberDic.Count != 0))
                {
                    List<string> dicKeyList = CommFunc.GetDicKeyList<ConfigurationStatus.BetsCode>(pScheme.FNNumberDic);
                    foreach (string str in dicKeyList)
                    {
                        if (plan.CheckPlanStringIsNoOpen() && AppInfo.IsDQCT)
                        {
                            this.CountBTIndexMain(str, true);
                            continue;
                        }
                        if (plan.CheckPlanStringIsTXFFCCH())
                        {
                            continue;
                        }
                        bool isWin = this.FNNumberDic[str].IsWin;
                        if (pScheme.Times.BTType == ConfigurationStatus.SCTimesType.Plan)
                        {
                            double num = plan.AutoTimes(str, false);
                            ConfigurationStatus.FBType fBType = plan.GetFBType(str);
                            if (isWin)
                            {
                                switch (fBType)
                                {
                                    case ConfigurationStatus.FBType.BZXCFB:
                                    case ConfigurationStatus.FBType.BZFB:
                                        if (num == -1.0)
                                        {
                                            this.CountBTIndexMain(str, false);
                                        }
                                        else
                                        {
                                            this.CountBTIndexMain(str, true);
                                        }
                                        goto Label_031F;
                                }
                                if (fBType == ConfigurationStatus.FBType.ZFB)
                                {
                                    this.CountBTIndexMain(str, false);
                                }
                            }
                            else
                            {
                                switch (fBType)
                                {
                                    case ConfigurationStatus.FBType.BZXCFB:
                                    case ConfigurationStatus.FBType.BZFB:
                                        if (!(num == -1.0))
                                        {
                                            this.CountBTIndexMain(str, false);
                                        }
                                        goto Label_031F;
                                }
                                if (fBType == ConfigurationStatus.FBType.ZFB)
                                {
                                    this.CountBTIndexMain(str, true);
                                }
                            }
                        }
                        else if (pScheme.Times.BTType == ConfigurationStatus.SCTimesType.FN)
                        {
                            if (!AppInfo.BTFNDic.ContainsKey(plan.BTFNName))
                            {
                                return;
                            }
                            int num2 = plan.BTIndex(str);
                            ConfigurationStatus.TimesScheme scheme = AppInfo.BTFNDic[plan.BTFNName].TimesSchemeList[num2];
                            if (isWin)
                            {
                                pScheme.FNBTIndexDic.Remove(str);
                                pScheme.FNNumberDic.Remove(str);
                                pScheme.IsStopJK = !scheme.YesJK;
                                num2 = scheme.YesAfter - 1;
                                if ((scheme.GetYesOtherFN != "") && (pBets.TZFNName != scheme.GetYesOtherFN))
                                {
                                    pBets.TZFNName = scheme.GetYesOtherFN;
                                    pBets.BetsSchemeDic.Clear();
                                }
                            }
                            else
                            {
                                pScheme.IsStopJK = !scheme.NoJK;
                                num2 = scheme.NoAfter - 1;
                                if ((scheme.GetNoOtherFN != "") && (pBets.TZFNName != scheme.GetNoOtherFN))
                                {
                                    pBets.TZFNName = scheme.GetNoOtherFN;
                                    pBets.BetsSchemeDic.Clear();
                                }
                            }
                            pScheme.FNBTIndexDic[str] = num2;
                        }
                    Label_031F:
                        this.SchemeInfo.FNBaseInfo.CountZuKeyList(str, pScheme, isWin);
                    }
                    if (pScheme.SchemeInfo.FNBaseInfo.BetsJKMode == ConfigurationStatus.JKMode.SCKJ)
                    {
                        pScheme.IsStopJK = true;
                    }
                    if (this.IsQQG)
                    {
                        this.QQGFNBTIndexDic(plan);
                    }
                }
            }

            private void CountBTIndexMain(string pZuKey, bool pReset)
            {
                if (pReset)
                {
                    this.FNBTIndexDic.Remove(pZuKey);
                    this.FNNumberDic.Remove(pZuKey);
                }
                else
                {
                    Dictionary<string, int> dictionary;
                    string str;
                    (dictionary = this.FNBTIndexDic)[str = pZuKey] = dictionary[str] + 1;
                }
            }

            public void DefaultOption(bool pAll)
            {
                this.PTNumberDic.Clear();
                if (pAll)
                {
                    this.IsMNBets = false;
                    this.IsStopJK = false;
                    this.IsStopAddFN = false;
                    this.DSJEndBetsTime = DateTime.MinValue;
                    this.ExpectList.Clear();
                    this.FNNumberDic.Clear();
                    this.FNBTIndexDic.Clear();
                    this.LSDataInfo = null;
                    this.ZuKeyDic.Clear();
                }
            }

            public void DefaultTimes(bool pAll)
            {
                if (pAll)
                {
                    this.Times = null;
                }
            }

            public List<string> GetFNKeyList()
            {
                List<string> list = new List<string>();
                foreach (string str in this.FNNumberDic.Keys)
                {
                    string perZuKey = ConfigurationStatus.FNBase.GetPerZuKey(str);
                    list.Add(perZuKey);
                }
                return list;
            }

            public void QQGFNBTIndexDic(ConfigurationStatus.SCPlan plan)
            {
                if (plan.TimesType != ConfigurationStatus.SCTimesType.FN)
                {
                    List<string> dicKeyList = CommFunc.GetDicKeyList<ConfigurationStatus.BetsCode>(this.FNNumberDic);
                    foreach (string str in dicKeyList)
                    {
                        List<double> times = plan.GetTimes(str);
                        if (this.FNBTIndexDic[str] >= times.Count)
                        {
                            this.FNBTIndexDic.Remove(str);
                            this.FNNumberDic.Remove(str);
                        }
                    }
                }
            }

            public string BTFNName
            {
                get
                {
                    string bTFNName = "";
                    if (this.SchemeInfo != null)
                    {
                        bTFNName = this.SchemeInfo.FNBaseInfo.BTFNName;
                    }
                    return bTFNName;
                }
            }

            public List<string> BTPlanList
            {
                get
                {
                    List<string> bTPlanList = new List<string>();
                    if (this.SchemeInfo != null)
                    {
                        bTPlanList = this.SchemeInfo.FNBaseInfo.BTPlanList;
                    }
                    return bTPlanList;
                }
            }

            public ConfigurationStatus.FBType FBInfo
            {
                get
                {
                    ConfigurationStatus.FBType zFB = ConfigurationStatus.FBType.ZFB;
                    if (this.SchemeInfo != null)
                    {
                        zFB = this.SchemeInfo.FNBaseInfo.FBInfo;
                    }
                    return zFB;
                }
            }

            public string GetBTIndexString
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str in this.FNBTIndexDic.Keys)
                    {
                        pList.Add(str + "|" + this.FNBTIndexDic[str]);
                    }
                    return CommFunc.Join(pList, ";");
                }
            }

            public List<string> GetPTNumberList
            {
                get
                {
                    List<string> list = new List<string>();
                    foreach (string str in this.PTNumberDic.Keys)
                    {
                        List<string> list2 = this.PTNumberDic[str];
                        foreach (string str2 in list2)
                        {
                            list.Add(str2);
                        }
                    }
                    return list;
                }
            }

            public bool IsQQG
            {
                get
                {
                    bool flag = false;
                    if (this.SchemeInfo != null)
                    {
                        flag = this.SchemeInfo.FNBaseInfo.Mode == ConfigurationStatus.SchemeMode.QQG;
                    }
                    return flag;
                }
            }

            public string Play =>
                (this.PlayType + this.PlayName);

            public ConfigurationStatus.PlayBase PlayInfo =>
                CommFunc.GetPlayInfo(this.PlayType, this.PlayName);

            public string PlayName
            {
                get
                {
                    string playName = "";
                    if (this.SchemeInfo != null)
                    {
                        playName = this.SchemeInfo.FNBaseInfo.PlayName;
                    }
                    return playName;
                }
            }

            public string PlayType
            {
                get
                {
                    string playType = "";
                    if (this.SchemeInfo != null)
                    {
                        playType = this.SchemeInfo.FNBaseInfo.PlayType;
                    }
                    return playType;
                }
            }

            public List<int> RXWZ
            {
                get
                {
                    List<int> rXWZList = null;
                    if (this.SchemeInfo != null)
                    {
                        rXWZList = this.SchemeInfo.FNBaseInfo.RXWZList;
                    }
                    return rXWZList;
                }
            }

            public string RXZJ
            {
                get
                {
                    string rXZJ = "";
                    if (this.SchemeInfo != null)
                    {
                        rXZJ = this.SchemeInfo.FNBaseInfo.RXZJ;
                    }
                    return rXZJ;
                }
            }
        }

        public enum BetsType
        {
            FNBets,
            SendBets,
            FollowBets
        }

        public enum BetsYKZS
        {
            AppGain
        }

        public delegate void BTImportDelegate(string pFNName, List<string> pBTPlanList);

        public class CDMoneyLimit
        {
            public int Max;
            public int Min;

            public CDMoneyLimit(int pMin, int pMax)
            {
                this.Min = pMin;
                this.Max = pMax;
            }
        }

        public delegate bool CheckPTLineDelegate();

        public delegate void CloseAppDelegate();

        public enum CombinaType
        {
            ZX,
            PX,
            Z3,
            Z6
        }

        public class DMLH
        {
            public int Number = 0;
            public List<string> NumberList = new List<string>();
        }

        public class ExpectCount
        {
            public int Count = 0;
            public string Data = "";
            public string Expect = "";

            public ExpectCount(string pExpect, int pCount, string pData)
            {
                this.Expect = pExpect;
                this.Count = pCount;
                this.Data = pData;
            }
        }

        public enum FBType
        {
            BZFB,
            ZFB,
            BZXCFB
        }

        public class FNBase
        {
            public string AppName;
            public const string AppNameString = "软件名称";
            public ConfigurationStatus.JKMode BetsJKMode;
            public const string BetsJKModeString = "投注监控模式";
            public bool BetsJKSelect;
            public const string BetsJKString = "投注监控";
            public string BetsJKValue;
            public bool BetsTime;
            public const string BetsTimeString = "投注时间";
            public ConfigurationStatus.TimeType BetsTimeType;
            public const string BetsTimeTypeString = "投注时间类型";
            public string BTFNName;
            public const string BTFNNameString = "倍投方案";
            public List<string> BTPlanList;
            public const string BTPlanListString = "倍投计划";
            public ConfigurationStatus.SCTimesType BTType;
            public const string BTTypeString = "倍投类型";
            public const string DJSEndTimeString = "倒计时停止时间";
            public string DJSEndTimeValue;
            public ConfigurationStatus.StopBetsType DJSEndType;
            public const string DJSEndTypeString = "倒计时停止类型";
            public ConfigurationStatus.FBType FBInfo;
            public const string FBInfoString = "翻倍方式";
            public bool FWBeginTimeSelect;
            public const string FWBeginTimeString = "范围开始时间";
            public string FWBeginTimeValue;
            public bool FWEndTimeSelect;
            public const string FWEndTimeString = "范围停止时间";
            public string FWEndTimeValue;
            public ConfigurationStatus.StopBetsType FWEndType;
            public const string FWEndTypeString = "范围停止类型";
            public bool IsBetsZJ;
            public const string IsBetsZJString = "正集";
            public int KSHTID;
            public string KSHTMoney;
            public bool KSHTSelect;
            public const string KSHTString = "亏损跳转";
            public string KSStopMoney;
            public bool KSStopSelect;
            public const string KSStopString = "亏损停止";
            public const string MNBets1String = "模拟投注1";
            public const string MNBets2String = "模拟投注2";
            public string MNBetsMoney1;
            public string MNBetsMoney2;
            public bool MNBetsSelect1;
            public bool MNBetsSelect2;
            public ConfigurationStatus.SchemeMode Mode;
            public int ModeExpect;
            public const string ModeExpectString = "换号期数";
            public const string ModeString = "换号规则";
            public string PlayName;
            public const string PlayNameString = "玩法名称";
            public string PlayType;
            public const string PlayTypeString = "玩法类型";
            public List<int> RXWZList;
            public const string RXWZListString = "任选位置";
            public string RXZJ;
            public const string RXZJString = "任选中奖";
            public int TotalNo;
            public int TotalYes;
            public ConfigurationStatus.SCUnitType Unit;
            public const string UnitString = "金额模式";
            public bool VisibleMore;
            public const string VisibleMoreString = "显示更多";
            public int YLHTID;
            public string YLHTMoney;
            public bool YLHTSelect;
            public const string YLHTString = "盈利跳转";
            public string YLStopMoney;
            public bool YLStopSelect;
            public const string YLStopString = "盈利停止";
            public const string ZSBets1String = "真实投注1";
            public const string ZSBets2String = "真实投注2";
            public string ZSBetsMoney1;
            public string ZSBetsMoney2;
            public bool ZSBetsSelect1;
            public bool ZSBetsSelect2;

            public FNBase()
            {
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
                {
                    this.PlayType = "定位胆";
                    this.PlayName = "万位";
                }
                else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                {
                    this.PlayType = "任选";
                    this.PlayName = "复式一中一";
                }
                else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                {
                    this.PlayType = "猜冠军";
                    this.PlayName = "猜冠军";
                }
                this.AppName = "YXZXGJ";
                this.Unit = ConfigurationStatus.SCUnitType.Fen;
                this.Mode = ConfigurationStatus.SchemeMode.QQHH;
                this.BetsJKMode = ConfigurationStatus.JKMode.YZJK;
                this.RXZJ = "1-10";
                this.RXWZList = new List<int>();
                this.ModeExpect = 3;
                this.TotalYes = 0;
                this.TotalNo = 0;
                this.BetsJKSelect = false;
                this.BetsJKValue = "";
                this.FBInfo = ConfigurationStatus.FBType.BZFB;
                this.IsBetsZJ = true;
                this.BTType = ConfigurationStatus.SCTimesType.Plan;
                List<string> list = new List<string> { 
                    "2",
                    "4",
                    "8",
                    "17",
                    "36",
                    "76",
                    "160",
                    "338",
                    "714",
                    "1507"
                };
                this.BTPlanList = list;
                this.BTFNName = "";
                this.VisibleMore = false;
                this.YLHTSelect = false;
                this.KSHTSelect = false;
                this.YLHTMoney = "50000";
                this.KSHTMoney = "50000";
                this.YLHTID = 1;
                this.KSHTID = 1;
                this.ZSBetsSelect1 = false;
                this.ZSBetsSelect2 = false;
                this.MNBetsSelect1 = false;
                this.MNBetsSelect2 = false;
                this.ZSBetsMoney1 = "50000";
                this.ZSBetsMoney2 = "50000";
                this.MNBetsMoney1 = "50000";
                this.MNBetsMoney2 = "50000";
                this.YLStopSelect = false;
                this.KSStopSelect = false;
                this.YLStopMoney = "50000";
                this.KSStopMoney = "50000";
                this.BetsTime = false;
                this.BetsTimeType = ConfigurationStatus.TimeType.FW;
                this.FWBeginTimeSelect = false;
                this.FWBeginTimeValue = "09:01";
                this.FWEndTimeSelect = false;
                this.FWEndTimeValue = "21:32";
                this.FWEndType = ConfigurationStatus.StopBetsType.Bets;
                this.DJSEndTimeValue = "02:00";
                this.DJSEndType = ConfigurationStatus.StopBetsType.Bets;
            }

            public static void AddFNBTIndex(Dictionary<string, ConfigurationStatus.BetsCode> pFNNumberDic, Dictionary<string, int> pFNBTIndexDic)
            {
                foreach (string str in pFNNumberDic.Keys)
                {
                    if (!pFNBTIndexDic.ContainsKey(str))
                    {
                        pFNBTIndexDic[str] = 0;
                    }
                }
            }

            public virtual bool CheckCount(ConfigurationStatus.BetsScheme pScheme, ref string pHint) => 
                true;

            public bool CheckIsNeedJK(ConfigurationStatus.BetsScheme pScheme, string pZuKey, string pBetsJKExpect)
            {
                if (pBetsJKExpect == "")
                {
                    return false;
                }
                if (pScheme.IsStopJK)
                {
                    return false;
                }
                if ((pScheme.SchemeInfo.FNBaseInfo.BTType != ConfigurationStatus.SCTimesType.FN) && (pScheme.FNBTIndexDic.ContainsKey(pZuKey) && (pScheme.FNBTIndexDic[pZuKey] > 0)))
                {
                    return false;
                }
                return true;
            }

            public bool CheckModeIsHH(bool pIsWin)
            {
                if (this.Mode == ConfigurationStatus.SchemeMode.QQHH)
                {
                    return true;
                }
                if (this.Mode == ConfigurationStatus.SchemeMode.CBHH)
                {
                    return false;
                }
                bool flag = false;
                if (pIsWin)
                {
                    if (this.Mode == ConfigurationStatus.SchemeMode.ZHHH)
                    {
                        return true;
                    }
                    if ((this.Mode == ConfigurationStatus.SchemeMode.LZNHH) || (this.Mode == ConfigurationStatus.SchemeMode.JZNHH))
                    {
                        this.TotalYes++;
                        if (this.TotalYes >= this.ModeExpect)
                        {
                            flag = true;
                            this.TotalYes = 0;
                        }
                        return flag;
                    }
                    if (this.Mode == ConfigurationStatus.SchemeMode.LGNHH)
                    {
                        this.TotalNo = 0;
                    }
                    return flag;
                }
                if (this.Mode == ConfigurationStatus.SchemeMode.GHHH)
                {
                    return true;
                }
                if ((this.Mode == ConfigurationStatus.SchemeMode.LGNHH) || (this.Mode == ConfigurationStatus.SchemeMode.JGNHH))
                {
                    this.TotalNo++;
                    if (this.TotalNo >= this.ModeExpect)
                    {
                        flag = true;
                        this.TotalNo = 0;
                    }
                    return flag;
                }
                if (this.Mode == ConfigurationStatus.SchemeMode.LZNHH)
                {
                    this.TotalYes = 0;
                }
                return flag;
            }

            public virtual Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex) => 
                null;

            public virtual void CountZuKeyIndexMain(string pZuKey, ConfigurationStatus.BetsScheme pScheme, bool pReset)
            {
            }

            public void CountZuKeyList(string pZuKey, ConfigurationStatus.BetsScheme pScheme, bool pIsWin)
            {
                if (!pScheme.ZuKeyDic.ContainsKey(pZuKey))
                {
                    pScheme.ZuKeyDic[pZuKey] = 0;
                }
                if (this.CheckModeIsHH(pIsWin))
                {
                    this.CountZuKeyIndexMain(pZuKey, pScheme, false);
                }
                else
                {
                    this.CountZuKeyIndexMain(pZuKey, pScheme, true);
                }
            }

            public static string GetExpectZuKey(string pZuKey)
            {
                List<string> pList = CommFunc.SplitString(pZuKey, "-", -1);
                pList.RemoveAt(0);
                return CommFunc.Join(pList, "-");
            }

            public static string GetPerZuKey(string pZuKey) => 
                pZuKey.Split(new char[] { '-' })[0];

            public virtual string InfoToString()
            {
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["软件名称"] = this.AppName,
                    ["玩法类型"] = this.PlayType,
                    ["玩法名称"] = this.PlayName,
                    ["金额模式"] = Convert.ToInt32(this.Unit).ToString(),
                    ["投注监控"] = this.BetsJKSelect.ToString() + "-" + this.BetsJKValue,
                    ["任选中奖"] = this.RXZJ,
                    ["任选位置"] = (this.RXWZList == null) ? "" : CommFunc.Join(this.RXWZList, ","),
                    ["换号规则"] = Convert.ToInt32(this.Mode).ToString(),
                    ["换号期数"] = this.ModeExpect.ToString(),
                    ["翻倍方式"] = Convert.ToInt32(this.FBInfo).ToString(),
                    ["正集"] = this.IsBetsZJ.ToString(),
                    ["倍投类型"] = Convert.ToInt32(this.BTType).ToString(),
                    ["倍投计划"] = CommFunc.Join(this.BTPlanList, ","),
                    ["倍投方案"] = this.BTFNName,
                    ["显示更多"] = this.VisibleMore.ToString(),
                    ["真实投注1"] = this.ZSBetsSelect1.ToString() + "-" + this.ZSBetsMoney1,
                    ["真实投注2"] = this.ZSBetsSelect2.ToString() + "-" + this.ZSBetsMoney2,
                    ["模拟投注1"] = this.MNBetsSelect1.ToString() + "-" + this.MNBetsMoney1,
                    ["模拟投注2"] = this.MNBetsSelect2.ToString() + "-" + this.MNBetsMoney2,
                    ["盈利跳转"] = string.Concat(new object[] { 
                        this.YLHTSelect.ToString(),
                        "-",
                        this.YLHTMoney,
                        "-",
                        this.YLHTID
                    }),
                    ["亏损跳转"] = string.Concat(new object[] { 
                        this.KSHTSelect.ToString(),
                        "-",
                        this.KSHTMoney,
                        "-",
                        this.KSHTID
                    }),
                    ["盈利停止"] = this.YLStopSelect.ToString() + "-" + this.YLStopMoney,
                    ["亏损停止"] = this.KSStopSelect.ToString() + "-" + this.KSStopMoney,
                    ["投注时间"] = this.BetsTime.ToString(),
                    ["投注时间类型"] = Convert.ToInt32(this.BetsTimeType).ToString(),
                    ["范围开始时间"] = this.FWBeginTimeSelect.ToString() + "-" + this.FWBeginTimeValue,
                    ["范围停止时间"] = this.FWEndTimeSelect.ToString() + "-" + this.FWEndTimeValue,
                    ["范围停止类型"] = Convert.ToInt32(this.FWEndType).ToString(),
                    ["倒计时停止时间"] = this.DJSEndTimeValue,
                    ["倒计时停止类型"] = Convert.ToInt32(this.DJSEndType).ToString()
                };
                return CommFunc.Join(pDic, "\r\n");
            }

            public virtual void StringToInfo(string pValue)
            {
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    switch (str)
                    {
                        case "软件名称":
                            this.AppName = configuration[str];
                            break;

                        case "玩法类型":
                            this.PlayType = configuration[str];
                            break;

                        case "玩法名称":
                            this.PlayName = configuration[str];
                            break;

                        case "金额模式":
                            this.Unit = (ConfigurationStatus.SCUnitType) Convert.ToInt32(configuration[str]);
                            break;

                        case "投注监控":
                            this.BetsJKSelect = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                            this.BetsJKValue = configuration[str].Split(new char[] { '-' })[1];
                            break;

                        case "投注监控模式":
                            this.BetsJKMode = (ConfigurationStatus.JKMode) Convert.ToInt32(configuration[str]);
                            break;

                        case "任选中奖":
                            this.RXZJ = configuration[str];
                            break;

                        case "任选位置":
                            this.RXWZList = CommFunc.SplitInt(configuration[str], ",");
                            break;

                        case "换号规则":
                            this.Mode = (ConfigurationStatus.SchemeMode) Convert.ToInt32(configuration[str]);
                            break;

                        case "换号期数":
                            this.ModeExpect = Convert.ToInt32(configuration[str]);
                            break;

                        case "翻倍方式":
                            this.FBInfo = (ConfigurationStatus.FBType) Convert.ToInt32(configuration[str]);
                            break;

                        case "正集":
                            this.IsBetsZJ = Convert.ToBoolean(configuration[str]);
                            break;

                        case "倍投类型":
                            this.BTType = (ConfigurationStatus.SCTimesType) Convert.ToInt32(configuration[str]);
                            break;

                        case "倍投计划":
                            this.BTPlanList = CommFunc.SplitString(configuration[str], ",", -1);
                            break;

                        case "倍投方案":
                            this.BTFNName = configuration[str];
                            break;

                        case "显示更多":
                            this.VisibleMore = Convert.ToBoolean(configuration[str]);
                            break;

                        case "真实投注1":
                            this.ZSBetsSelect1 = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                            this.ZSBetsMoney1 = configuration[str].Split(new char[] { '-' })[1];
                            break;

                        case "真实投注2":
                            this.ZSBetsSelect2 = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                            this.ZSBetsMoney2 = configuration[str].Split(new char[] { '-' })[1];
                            break;

                        case "模拟投注1":
                            this.MNBetsSelect1 = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                            this.MNBetsMoney1 = configuration[str].Split(new char[] { '-' })[1];
                            break;

                        case "模拟投注2":
                            this.MNBetsSelect2 = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                            this.MNBetsMoney2 = configuration[str].Split(new char[] { '-' })[1];
                            break;

                        case "盈利跳转":
                            this.YLHTSelect = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                            this.YLHTMoney = configuration[str].Split(new char[] { '-' })[1];
                            this.YLHTID = Convert.ToInt32(configuration[str].Split(new char[] { '-' })[2]);
                            break;

                        case "亏损跳转":
                            this.KSHTSelect = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                            this.KSHTMoney = configuration[str].Split(new char[] { '-' })[1];
                            this.KSHTID = Convert.ToInt32(configuration[str].Split(new char[] { '-' })[2]);
                            break;

                        case "盈利停止":
                            this.YLStopSelect = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                            this.YLStopMoney = configuration[str].Split(new char[] { '-' })[1];
                            break;

                        case "亏损停止":
                            this.KSStopSelect = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                            this.KSStopMoney = configuration[str].Split(new char[] { '-' })[1];
                            break;

                        case "投注时间":
                            this.BetsTime = Convert.ToBoolean(configuration[str]);
                            break;

                        case "投注时间类型":
                            this.BetsTimeType = (ConfigurationStatus.TimeType) Convert.ToInt32(configuration[str]);
                            break;

                        case "范围开始时间":
                            this.FWBeginTimeSelect = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                            this.FWBeginTimeValue = configuration[str].Split(new char[] { '-' })[1];
                            break;

                        case "范围停止时间":
                            this.FWEndTimeSelect = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                            this.FWEndTimeValue = configuration[str].Split(new char[] { '-' })[1];
                            break;

                        case "范围停止类型":
                            this.FWEndType = (ConfigurationStatus.StopBetsType) Convert.ToInt32(configuration[str]);
                            break;

                        case "倒计时停止时间":
                            this.DJSEndTimeValue = configuration[str];
                            break;

                        case "倒计时停止类型":
                            this.DJSEndType = (ConfigurationStatus.StopBetsType) Convert.ToInt32(configuration[str]);
                            break;
                    }
                }
            }

            public string BetsJKExpect
            {
                get
                {
                    string betsJKValue = "";
                    if (this.BetsJKSelect)
                    {
                        betsJKValue = this.BetsJKValue;
                    }
                    return betsJKValue;
                }
            }

            public string GetDJSEndTime
            {
                get
                {
                    string str = "";
                    if (!this.BetsTime)
                    {
                        return str;
                    }
                    if (this.BetsTimeType != ConfigurationStatus.TimeType.DJS)
                    {
                        return str;
                    }
                    return this.DJSEndTimeValue;
                }
            }

            public ConfigurationStatus.StopBetsType GetEndType
            {
                get
                {
                    if (this.BetsTimeType == ConfigurationStatus.TimeType.FW)
                    {
                        return this.FWEndType;
                    }
                    return this.DJSEndType;
                }
            }

            public string GetFWBeginTime
            {
                get
                {
                    string str = "";
                    if (!this.BetsTime)
                    {
                        return str;
                    }
                    if (this.BetsTimeType != ConfigurationStatus.TimeType.FW)
                    {
                        return str;
                    }
                    return (this.FWBeginTimeSelect ? this.FWBeginTimeValue : "");
                }
            }

            public string GetFWEndTime
            {
                get
                {
                    string str = "";
                    if (!this.BetsTime)
                    {
                        return str;
                    }
                    if (this.BetsTimeType != ConfigurationStatus.TimeType.FW)
                    {
                        return str;
                    }
                    return (this.FWEndTimeSelect ? this.FWEndTimeValue : "");
                }
            }

            public string GetKSHTMoney =>
                (this.KSHTSelect ? this.KSHTMoney : "");

            public string GetKSStopMoney =>
                (this.KSStopSelect ? this.KSStopMoney : "");

            public string GetMNBetsMoney1 =>
                (this.MNBetsSelect1 ? this.MNBetsMoney1 : "");

            public string GetMNBetsMoney2 =>
                (this.MNBetsSelect2 ? this.MNBetsMoney2 : "");

            public string GetYLHTMoney =>
                (this.YLHTSelect ? this.YLHTMoney : "");

            public string GetYLStopMoney =>
                (this.YLStopSelect ? this.YLStopMoney : "");

            public string GetZSBetsMoney1 =>
                (this.ZSBetsSelect1 ? this.ZSBetsMoney1 : "");

            public string GetZSBetsMoney2 =>
                (this.ZSBetsSelect2 ? this.ZSBetsMoney2 : "");

            public string Play =>
                (this.PlayType + this.PlayName);

            public ConfigurationStatus.PlayBase PlayInfo =>
                CommFunc.GetPlayInfo(this.PlayType, this.PlayName);

            public List<int> RXZJList =>
                CommFunc.ConvertIntList(this.RXZJ);

            public string ViewPlay =>
                $"{this.PlayName}【{this.PlayType}】";
        }

        public class FNDMLH : ConfigurationStatus.FNBase
        {
            public bool DMLHSingle;
            public const string DMLHSingleString = "定码轮换单组";
            public string DMLHValue;
            public List<ConfigurationStatus.DMLH> DMLHValueList;
            public const string DMLHValueString = "定码轮换内容";

            public FNDMLH()
            {
                this.DMLHValueList = new List<ConfigurationStatus.DMLH>();
            }

            public FNDMLH(string pValue)
            {
                this.DMLHValueList = new List<ConfigurationStatus.DMLH>();
                if (pValue == "")
                {
                    this.DMLHValue = "1 2 3";
                }
                else
                {
                    this.StringToInfo(pValue);
                }
            }

            public override bool CheckCount(ConfigurationStatus.BetsScheme pScheme, ref string pHint)
            {
                this.DMLHValueList.Clear();
                if (base.PlayInfo == null)
                {
                    return false;
                }
                List<List<string>> list = CommFunc.CheckFilterNumber(this.DMLHValue, base.PlayInfo.CodeCount, base.Play, ref pHint);
                if (pHint != "")
                {
                    return false;
                }
                foreach (List<string> list2 in list)
                {
                    ConfigurationStatus.DMLH item = new ConfigurationStatus.DMLH {
                        NumberList = list2,
                        Number = list2.Count
                    };
                    this.DMLHValueList.Add(item);
                }
                return true;
            }

            public override Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                int num2;
                ConfigurationStatus.DMLH dmlh;
                List<string> list2;
                string key = "0";
                Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
                if (!pScheme.ZuKeyDic.ContainsKey(key))
                {
                    pScheme.ZuKeyDic[key] = 0;
                }
                string betsJKExpect = base.BetsJKExpect;
                string str3 = "";
                bool flag = base.CheckIsNeedJK(pScheme, key, betsJKExpect);
                if (flag)
                {
                    int num = 0;
                    while (true)
                    {
                        Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
                        num2 = pIndex + num;
                        List<string> codeList = pDataList[num2].CodeList;
                        dmlh = this.DMLHValueList[0];
                        list2 = CommFunc.CopyList(dmlh.NumberList);
                        List<List<int>> indexList = CommFunc.GetCodeListByPlay(base.PlayType, base.PlayName, base.RXWZList, list2);
                        bool flag2 = CommFunc.VerificationCode(base.Play, indexList, codeList, list2) > 0;
                        str3 = str3 + (flag2 ? "1" : "0");
                        if (str3.Length >= betsJKExpect.Length)
                        {
                            break;
                        }
                        num++;
                    }
                }
                if (!((str3 != betsJKExpect) && flag))
                {
                    num2 = pScheme.ZuKeyDic[key] % this.DMLHValueList.Count;
                    dmlh = this.DMLHValueList[num2];
                    list2 = CommFunc.CopyList(dmlh.NumberList);
                    dictionary[key] = new ConfigurationStatus.BetsCode(list2, null);
                }
                if (pScheme.IsQQG)
                {
                    Dictionary<string, int> dictionary4;
                    string str4;
                    (dictionary4 = pScheme.ZuKeyDic)[str4 = key] = dictionary4[str4] + 1;
                }
                return dictionary;
            }

            public override void CountZuKeyIndexMain(string pZuKey, ConfigurationStatus.BetsScheme pScheme, bool pReset)
            {
                if (!pReset)
                {
                    Dictionary<string, int> dictionary;
                    string str;
                    (dictionary = pScheme.ZuKeyDic)[str = pZuKey] = dictionary[str] + 1;
                }
            }

            public override string InfoToString()
            {
                string str = base.InfoToString();
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["定码轮换内容"] = this.DMLHValue,
                    ["定码轮换单组"] = this.DMLHSingle.ToString()
                };
                string str2 = CommFunc.Join(pDic, "\r\n");
                return (str + "\r\n" + str2);
            }

            public override void StringToInfo(string pValue)
            {
                base.StringToInfo(pValue);
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    switch (str)
                    {
                        case "定码轮换内容":
                            this.DMLHValue = configuration[str];
                            break;

                        case "定码轮换单组":
                            this.DMLHSingle = Convert.ToBoolean(configuration[str]);
                            break;
                    }
                }
            }
        }

        public class FNGDQM : ConfigurationStatus.FNBase
        {
            public List<ConfigurationStatus.KMTM> GDQMInfoList;
            public const string GDQMValueString = "固定取码内容";

            public FNGDQM()
            {
                this.GDQMInfoList = new List<ConfigurationStatus.KMTM>();
            }

            public FNGDQM(string pValue)
            {
                this.GDQMInfoList = new List<ConfigurationStatus.KMTM>();
                if (pValue == "")
                {
                    this.GDQMValue = "0-4|0-2|1 2 3";
                }
                else
                {
                    this.StringToInfo(pValue);
                }
            }

            public override bool CheckCount(ConfigurationStatus.BetsScheme pScheme, ref string pHint)
            {
                foreach (ConfigurationStatus.KMTM kmtm in this.GDQMInfoList)
                {
                    kmtm.KMTMValueInfo = new ConfigurationStatus.DMLH();
                    List<List<string>> list = CommFunc.CheckFilterNumber(kmtm.KMTMValue, base.PlayInfo.CodeCount, base.Play, ref pHint);
                    if (pHint != "")
                    {
                        return false;
                    }
                    foreach (List<string> list2 in list)
                    {
                        kmtm.KMTMValueInfo.NumberList = list2;
                        kmtm.KMTMValueInfo.Number = list2.Count;
                    }
                }
                return true;
            }

            public override Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> betsCode;
                string pZuKey = "0";
                bool zuKeyIsChange = pScheme.ZuKeyIsChange;
                Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
                string betsJKExpect = base.BetsJKExpect;
                string str3 = "";
                bool flag2 = base.CheckIsNeedJK(pScheme, pZuKey, betsJKExpect);
                if (flag2)
                {
                    int num = 0;
                    while (true)
                    {
                        int num2 = pIndex + num;
                        List<string> codeList = pDataList[num2].CodeList;
                        betsCode = this.GetBetsCode(pDataList, num2 + 1);
                        if (betsCode.Count == 0)
                        {
                            num++;
                        }
                        else
                        {
                            List<List<int>> indexList = CommFunc.GetCodeListByPlay(base.PlayType, base.PlayName, base.RXWZList, betsCode);
                            bool flag3 = CommFunc.VerificationCode(base.Play, indexList, codeList, betsCode) > 0;
                            str3 = str3 + (flag3 ? "1" : "0");
                            if (str3.Length >= betsJKExpect.Length)
                            {
                                break;
                            }
                            num++;
                        }
                    }
                }
                if ((str3 == betsJKExpect) || !flag2)
                {
                    if ((zuKeyIsChange || pScheme.IsQQG) || (pScheme.ZuKeySaveList.Count == 0))
                    {
                        betsCode = this.GetBetsCode(pDataList, pIndex);
                        pScheme.ZuKeySaveList = betsCode;
                    }
                    dictionary[pZuKey] = new ConfigurationStatus.BetsCode(pScheme.ZuKeySaveList, null);
                }
                return dictionary;
            }

            public override void CountZuKeyIndexMain(string pZuKey, ConfigurationStatus.BetsScheme pScheme, bool pReset)
            {
                pScheme.ZuKeyIsChange = !pReset;
            }

            private List<string> GetBetsCode(List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> list = new List<string>();
                int num = pIndex;
                ConfigurationStatus.OpenData data = pDataList[pIndex];
                foreach (ConfigurationStatus.KMTM kmtm in this.GDQMInfoList)
                {
                    for (int i = 0; i < kmtm.KMTMTypeList.Count; i++)
                    {
                        int item = Convert.ToInt32(data.CodeList[kmtm.KMTMTypeList[i]]);
                        if (kmtm.KMTMCodeList.Contains(item))
                        {
                            return CommFunc.CopyList(kmtm.KMTMValueInfo.NumberList);
                        }
                    }
                }
                return list;
            }

            public override string InfoToString()
            {
                string str = base.InfoToString();
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["固定取码内容"] = this.GDQMValue
                };
                string str2 = CommFunc.Join(pDic, "\r\n");
                return (str + "\r\n" + str2);
            }

            public override void StringToInfo(string pValue)
            {
                base.StringToInfo(pValue);
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    string str2 = str;
                    if ((str2 != null) && (str2 == "固定取码内容"))
                    {
                        this.GDQMValue = configuration[str];
                    }
                }
            }

            public string GDQMValue
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (ConfigurationStatus.KMTM kmtm in this.GDQMInfoList)
                    {
                        List<string> list2 = new List<string> {
                            CommFunc.LotteryJoinInt(kmtm.KMTMTypeList),
                            kmtm.KMTMCode,
                            kmtm.KMTMValue.ToString()
                        };
                        string item = CommFunc.Join(list2, "|");
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, ";");
                }
                set
                {
                    List<string> list = CommFunc.SplitString(value, ";", -1);
                    this.GDQMInfoList.Clear();
                    foreach (string str in list)
                    {
                        List<string> list2 = CommFunc.SplitString(str, "|", -1);
                        ConfigurationStatus.KMTM item = new ConfigurationStatus.KMTM {
                            KMTMTypeList = CommFunc.ConvertIntList(list2[0]),
                            KMTMCode = list2[1],
                            KMTMValue = list2[2]
                        };
                        this.GDQMInfoList.Add(item);
                    }
                }
            }
        }

        public class FNGJDMLH : ConfigurationStatus.FNBase
        {
            public List<ConfigurationStatus.GJDMLH> DMLHValueList;
            public const string DMLHValueString = "高级定码轮换内容";

            public FNGJDMLH()
            {
                this.DMLHValueList = new List<ConfigurationStatus.GJDMLH>();
            }

            public FNGJDMLH(string pValue)
            {
                this.DMLHValueList = new List<ConfigurationStatus.GJDMLH>();
                if (pValue == "")
                {
                    if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
                    {
                        string random = this.GetRandom();
                        this.DMLHValue = $"1|{random}|1|1";
                    }
                    else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                    {
                        this.DMLHValue = "1|01 02 03|1|1";
                    }
                    else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                    {
                        this.DMLHValue = "1|01 02 03|1|1";
                    }
                }
                else
                {
                    this.StringToInfo(pValue);
                }
            }

            public override bool CheckCount(ConfigurationStatus.BetsScheme pScheme, ref string pHint)
            {
                foreach (ConfigurationStatus.GJDMLH gjdmlh in this.DMLHValueList)
                {
                    gjdmlh.NumberList = CommFunc.FilterNumber(gjdmlh.Value, base.PlayInfo.CodeCount, base.Play, ref pHint);
                    if (pHint != "")
                    {
                        return false;
                    }
                }
                return true;
            }

            public override Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                int num2;
                ConfigurationStatus.GJDMLH gjdmlh;
                List<string> list2;
                string key = "0";
                Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
                if (!pScheme.ZuKeyDic.ContainsKey(key))
                {
                    pScheme.ZuKeyDic[key] = 0;
                }
                base.Mode = ConfigurationStatus.SchemeMode.ZHHH;
                string betsJKExpect = base.BetsJKExpect;
                string str3 = "";
                bool flag = base.CheckIsNeedJK(pScheme, key, betsJKExpect);
                if (flag)
                {
                    int num = 0;
                    while (true)
                    {
                        Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
                        num2 = pIndex + num;
                        List<string> codeList = pDataList[num2].CodeList;
                        gjdmlh = this.DMLHValueList[0];
                        list2 = CommFunc.CopyList(gjdmlh.NumberList);
                        List<List<int>> indexList = CommFunc.GetCodeListByPlay(base.PlayType, base.PlayName, base.RXWZList, list2);
                        bool flag2 = CommFunc.VerificationCode(base.Play, indexList, codeList, list2) > 0;
                        str3 = str3 + (flag2 ? "1" : "0");
                        if (str3.Length >= betsJKExpect.Length)
                        {
                            break;
                        }
                        num++;
                    }
                }
                if (!((str3 != betsJKExpect) && flag))
                {
                    num2 = pScheme.ZuKeyDic[key];
                    gjdmlh = this.DMLHValueList[num2];
                    list2 = CommFunc.CopyList(gjdmlh.NumberList);
                    dictionary[key] = new ConfigurationStatus.BetsCode(list2, null);
                }
                return dictionary;
            }

            public override void CountZuKeyIndexMain(string pZuKey, ConfigurationStatus.BetsScheme pScheme, bool pReset)
            {
                int num = pScheme.ZuKeyDic[pZuKey];
                ConfigurationStatus.GJDMLH gjdmlh = this.DMLHValueList[num];
                if (!pReset)
                {
                    num = gjdmlh.YesAfter - 1;
                }
                else
                {
                    num = gjdmlh.NoAfter - 1;
                }
                pScheme.ZuKeyDic[pZuKey] = num;
            }

            public string GetRandom()
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (string str in AppInfo.Account.SchemeDic.Keys)
                {
                    ConfigurationStatus.Scheme scheme = AppInfo.Account.SchemeDic[str];
                    if ((scheme.FNCHType == ConfigurationStatus.SchemeCHType.GJDMLH) && (scheme.FNBaseInfo.Play == base.Play))
                    {
                        List<ConfigurationStatus.GJDMLH> dMLHValueList = ((ConfigurationStatus.FNGJDMLH) scheme.FNBaseInfo).DMLHValueList;
                        foreach (ConfigurationStatus.GJDMLH gjdmlh in dMLHValueList)
                        {
                            string str2 = gjdmlh.Value;
                            if (CommFunc.SplitBetsCode(str2, base.Play).Count == 5)
                            {
                                dictionary[str2] = "";
                            }
                        }
                    }
                }
                List<string> list3 = CommFunc.GetCombinaList(ConfigurationStatus.CombinaType.PX, 5, -1, -1);
                int pMax = list3.Count - 1;
                int num2 = CommFunc.Random(0, pMax);
                string pValue = list3[num2];
                string key = CommFunc.ConvertBetsCode(pValue, base.Play, "");
                while (dictionary.ContainsKey(key))
                {
                    num2++;
                    if (num2 >= pMax)
                    {
                        num2 = 0;
                    }
                    pValue = list3[num2];
                    key = CommFunc.ConvertBetsCode(pValue, base.Play, "");
                }
                return key;
            }

            public override string InfoToString()
            {
                string str = base.InfoToString();
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["高级定码轮换内容"] = this.DMLHValue.ToString()
                };
                string str2 = CommFunc.Join(pDic, "\r\n");
                return (str + "\r\n" + str2);
            }

            public override void StringToInfo(string pValue)
            {
                base.StringToInfo(pValue);
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    string str2 = str;
                    if ((str2 != null) && (str2 == "高级定码轮换内容"))
                    {
                        this.DMLHValue = configuration[str];
                    }
                }
            }

            public string DMLHValue
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (ConfigurationStatus.GJDMLH gjdmlh in this.DMLHValueList)
                    {
                        List<string> list2 = new List<string> {
                            gjdmlh.ID.ToString(),
                            gjdmlh.Value,
                            gjdmlh.YesAfter.ToString(),
                            gjdmlh.NoAfter.ToString()
                        };
                        string item = CommFunc.Join(list2, "|");
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, ";");
                }
                set
                {
                    List<string> list = CommFunc.SplitString(value, ";", -1);
                    this.DMLHValueList.Clear();
                    foreach (string str in list)
                    {
                        List<string> list2 = CommFunc.SplitString(str, "|", -1);
                        ConfigurationStatus.GJDMLH item = new ConfigurationStatus.GJDMLH {
                            ID = Convert.ToInt32(list2[0]),
                            Value = list2[1],
                            YesAfter = Convert.ToInt32(list2[2]),
                            NoAfter = Convert.ToInt32(list2[3])
                        };
                        this.DMLHValueList.Add(item);
                    }
                }
            }
        }

        public class FNGJKMTM : ConfigurationStatus.FNBase
        {
            public List<string> BetsValue1List;
            public const string BetsValue1ListString = "高级开某投某正投号码";
            public List<string> BetsValue2List;
            public const string BetsValue2ListString = "高级开某投某反投号码";
            public List<string> KCIndexList;
            public const string KCIndexListString = "高级开某投某选择";
            public List<string> KCValueList;
            public const string KCValueString = "高级开某投某开出号码";
            public int KMTMIndex;
            public ConfigurationStatus.KMTMMode KMTMModeInfo;
            public const string KMTMModeInfoString = "高级开某投某换号规则";

            public FNGJKMTM()
            {
                this.KCIndexList = new List<string>();
                this.KCValueList = new List<string>();
                this.BetsValue1List = new List<string>();
                this.BetsValue2List = new List<string>();
            }

            public FNGJKMTM(string pValue)
            {
                if (pValue == "")
                {
                    this.KCIndexList = CommFunc.RepeatList("True", 10);
                    this.KCValueList = CommFunc.ConvertStringList("0-9");
                    List<string> list = new List<string> { 
                        "0,1,2,3,4",
                        "1,2,3,4,5",
                        "2,3,4,5,6",
                        "3,4,5,6,7",
                        "4,5,6,7,8",
                        "5,6,7,8,9",
                        "6,7,8,9,0",
                        "7,8,9,0,1",
                        "8,9,0,1,2",
                        "9,0,1,2,3"
                    };
                    this.BetsValue1List = list;
                    List<string> list2 = new List<string> { 
                        "0,9,8,7,6",
                        "1,0,9,8,7",
                        "2,1,0,9,8",
                        "3,2,1,0,9",
                        "4,3,2,1,0",
                        "5,4,3,2,1",
                        "6,5,4,3,2",
                        "7,6,5,4,3",
                        "8,7,6,5,4",
                        "9,8,7,6,5"
                    };
                    this.BetsValue2List = list2;
                    this.KMTMIndex = 0;
                    this.KMTMModeInfo = ConfigurationStatus.KMTMMode.ZT;
                    base.PlayType = "定位胆";
                    base.PlayName = "万位";
                    List<string> list3 = new List<string> { 
                        "6",
                        "12",
                        "25",
                        "52",
                        "108",
                        "224",
                        "465",
                        "965",
                        "2003",
                        "4156"
                    };
                    base.BTPlanList = list3;
                }
                else
                {
                    this.StringToInfo(pValue);
                }
            }

            public override Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> list2;
                string key = "0";
                Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
                if (!pScheme.FNBTIndexDic.ContainsKey(key))
                {
                    pScheme.FNBTIndexDic[key] = 0;
                }
                string betsJKExpect = base.BetsJKExpect;
                string str3 = "";
                bool flag = base.CheckIsNeedJK(pScheme, key, betsJKExpect);
                if (flag)
                {
                    int num = 0;
                    while (true)
                    {
                        int num2 = pIndex + num;
                        List<string> codeList = pDataList[num2].CodeList;
                        list2 = this.GetBetsCode(pDataList, num2 + 1, base.PlayInfo.IndexList[0] - 1, false);
                        List<List<int>> indexList = CommFunc.GetCodeListByPlay(base.PlayType, base.PlayName, base.RXWZList, list2);
                        bool flag2 = CommFunc.VerificationCode(base.Play, indexList, codeList, list2) > 0;
                        str3 = str3 + (flag2 ? "1" : "0");
                        if (str3.Length >= betsJKExpect.Length)
                        {
                            break;
                        }
                        num++;
                    }
                }
                if (!((str3 != betsJKExpect) && flag))
                {
                    int num4 = pScheme.FNBTIndexDic[key];
                    bool pIsChange = num4 >= (pScheme.BTPlanList.Count / 2);
                    list2 = this.GetBetsCode(pDataList, pIndex, base.PlayInfo.IndexList[0] - 1, pIsChange);
                    dictionary[key] = new ConfigurationStatus.BetsCode(list2, null);
                }
                return dictionary;
            }

            private List<string> GetBetsCode(List<ConfigurationStatus.OpenData> pDataList, int pIndex, int pQHIndex, bool pIsChange)
            {
                List<string> list = new List<string>();
                int num = pIndex;
                ConfigurationStatus.OpenData data = pDataList[pIndex];
                string item = data.CodeList[pQHIndex];
                List<string> list2 = new List<string>();
                if (this.KMTMModeInfo == ConfigurationStatus.KMTMMode.ZT)
                {
                    list2 = this.BetsValue1List;
                }
                else if (this.KMTMModeInfo == ConfigurationStatus.KMTMMode.FT)
                {
                    list2 = this.BetsValue2List;
                }
                else if (this.KMTMModeInfo == ConfigurationStatus.KMTMMode.ZFT)
                {
                    list2 = pIsChange ? this.BetsValue2List : this.BetsValue1List;
                }
                else if (this.KMTMModeInfo == ConfigurationStatus.KMTMMode.FZT)
                {
                    list2 = pIsChange ? this.BetsValue1List : this.BetsValue2List;
                }
                for (int i = 0; i < this.KCIndexList.Count; i++)
                {
                    if (Convert.ToBoolean(this.KCIndexList[i]) && CommFunc.SplitString(this.KCValueList[i], ",", -1).Contains(item))
                    {
                        List<string> list4 = CommFunc.SplitString(list2[i], ",", -1);
                        for (int j = 0; j < list4.Count; j++)
                        {
                            string str2 = list4[j];
                            if (!list.Contains(str2))
                            {
                                list.Add(str2);
                            }
                        }
                    }
                }
                return list;
            }

            public override string InfoToString()
            {
                string str = base.InfoToString();
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["高级开某投某选择"] = CommFunc.Join(this.KCIndexList, ","),
                    ["高级开某投某开出号码"] = CommFunc.Join(this.KCValueList, "|"),
                    ["高级开某投某正投号码"] = CommFunc.Join(this.BetsValue1List, "|"),
                    ["高级开某投某反投号码"] = CommFunc.Join(this.BetsValue2List, "|"),
                    ["高级开某投某换号规则"] = Convert.ToInt32(this.KMTMModeInfo).ToString()
                };
                string str2 = CommFunc.Join(pDic, "\r\n");
                return (str + "\r\n" + str2);
            }

            public override void StringToInfo(string pValue)
            {
                base.StringToInfo(pValue);
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    string str2 = str;
                    if (str2 != null)
                    {
                        if (str2 != "高级开某投某选择")
                        {
                            if (str2 == "高级开某投某开出号码")
                            {
                                goto Label_0095;
                            }
                            if (str2 == "高级开某投某正投号码")
                            {
                                goto Label_00AF;
                            }
                            if (str2 == "高级开某投某反投号码")
                            {
                                goto Label_00C9;
                            }
                            if (str2 == "高级开某投某换号规则")
                            {
                                goto Label_00E3;
                            }
                        }
                        else
                        {
                            this.KCIndexList = CommFunc.SplitString(configuration[str], ",", -1);
                        }
                    }
                    continue;
                Label_0095:
                    this.KCValueList = CommFunc.SplitString(configuration[str], "|", -1);
                    continue;
                Label_00AF:
                    this.BetsValue1List = CommFunc.SplitString(configuration[str], "|", -1);
                    continue;
                Label_00C9:
                    this.BetsValue2List = CommFunc.SplitString(configuration[str], "|", -1);
                    continue;
                Label_00E3:
                    this.KMTMModeInfo = (ConfigurationStatus.KMTMMode) Convert.ToInt32(configuration[str]);
                }
            }
        }

        public class FNKMSM : ConfigurationStatus.FNBase
        {
            public List<string> KMSMTypeList;
            public const string KMSMTypeString = "开某杀某类型";

            public FNKMSM()
            {
                this.KMSMTypeList = new List<string>();
            }

            public FNKMSM(string pValue)
            {
                this.KMSMTypeList = new List<string>();
                if (pValue == "")
                {
                    this.KMSMTypeList = CommFunc.GetDicKeyList<int>(AppInfo.FiveDic);
                }
                else
                {
                    this.StringToInfo(pValue);
                }
            }

            public override Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> betsCode;
                string key = "0";
                bool zuKeyIsChange = pScheme.ZuKeyIsChange;
                Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
                if (!pScheme.FNBTIndexDic.ContainsKey(key))
                {
                    pScheme.FNBTIndexDic[key] = 0;
                }
                string betsJKExpect = base.BetsJKExpect;
                string str3 = "";
                bool flag2 = base.CheckIsNeedJK(pScheme, key, betsJKExpect);
                if (flag2)
                {
                    int num = 0;
                    while (true)
                    {
                        int num2 = pIndex + num;
                        List<string> codeList = pDataList[num2].CodeList;
                        betsCode = this.GetBetsCode(pDataList, num2 + 1);
                        List<List<int>> indexList = CommFunc.GetCodeListByPlay(base.PlayType, base.PlayName, base.RXWZList, betsCode);
                        bool flag3 = CommFunc.VerificationCode(base.Play, indexList, codeList, betsCode) > 0;
                        str3 = str3 + (flag3 ? "1" : "0");
                        if (str3.Length >= betsJKExpect.Length)
                        {
                            break;
                        }
                        num++;
                    }
                }
                if (!((str3 != betsJKExpect) && flag2))
                {
                    int num4 = pScheme.FNBTIndexDic[key];
                    betsCode = this.GetBetsCode(pDataList, pIndex);
                    dictionary[key] = new ConfigurationStatus.BetsCode(betsCode, null);
                }
                return dictionary;
            }

            public override void CountZuKeyIndexMain(string pZuKey, ConfigurationStatus.BetsScheme pScheme, bool pReset)
            {
                pScheme.ZuKeyIsChange = !pReset;
            }

            private List<string> GetBetsCode(List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> list = new List<string>();
                int num = pIndex;
                ConfigurationStatus.OpenData data = pDataList[pIndex];
                for (int i = 0; i < data.CodeList.Count; i++)
                {
                    List<string> pList = CommFunc.ConvertStringList("0-9");
                    string item = data.CodeList[i];
                    if (this.KMSMConvertList.Contains(i))
                    {
                        pList.Remove(item);
                    }
                    string str2 = CommFunc.Join(pList);
                    list.Add(str2);
                }
                return list;
            }

            public override string InfoToString()
            {
                string str = base.InfoToString();
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["开某杀某类型"] = CommFunc.Join(this.KMSMTypeList, ",")
                };
                string str2 = CommFunc.Join(pDic, "\r\n");
                return (str + "\r\n" + str2);
            }

            public override void StringToInfo(string pValue)
            {
                base.StringToInfo(pValue);
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    string str2 = str;
                    if ((str2 != null) && (str2 == "开某杀某类型"))
                    {
                        this.KMSMTypeList = CommFunc.SplitString(configuration[str], ",", -1);
                    }
                }
            }

            public List<int> KMSMConvertList
            {
                get
                {
                    List<int> list = new List<int>();
                    foreach (string str in this.KMSMTypeList)
                    {
                        int item = AppInfo.FiveDic[str];
                        list.Add(item);
                    }
                    return list;
                }
            }
        }

        public class FNKMTM : ConfigurationStatus.FNBase
        {
            public const string KMTMCodeString = "开某投某号码";
            public ConfigurationStatus.KMTM KMTMInfo;
            public const string KMTMTypeListString = "开某投某类型";
            public const string KMTMValueString = "开某投某内容";

            public FNKMTM()
            {
                this.KMTMInfo = new ConfigurationStatus.KMTM();
            }

            public FNKMTM(string pValue)
            {
                this.KMTMInfo = new ConfigurationStatus.KMTM();
                if (pValue == "")
                {
                    this.KMTMInfo.KMTMTypeList = CommFunc.ConvertIntList("0-4");
                    this.KMTMInfo.KMTMCode = "0-2";
                    this.KMTMInfo.KMTMValue = "1 2 3";
                }
                else
                {
                    this.StringToInfo(pValue);
                }
            }

            public override bool CheckCount(ConfigurationStatus.BetsScheme pScheme, ref string pHint)
            {
                this.KMTMInfo.KMTMValueInfo = new ConfigurationStatus.DMLH();
                List<List<string>> list = CommFunc.CheckFilterNumber(this.KMTMInfo.KMTMValue, base.PlayInfo.CodeCount, base.Play, ref pHint);
                if (pHint != "")
                {
                    return false;
                }
                foreach (List<string> list2 in list)
                {
                    this.KMTMInfo.KMTMValueInfo.NumberList = list2;
                    this.KMTMInfo.KMTMValueInfo.Number = list2.Count;
                }
                return true;
            }

            public override Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> betsCode;
                string pZuKey = "0";
                bool zuKeyIsChange = pScheme.ZuKeyIsChange;
                Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
                string betsJKExpect = base.BetsJKExpect;
                string str3 = "";
                bool flag2 = base.CheckIsNeedJK(pScheme, pZuKey, betsJKExpect);
                if (flag2)
                {
                    int num = 0;
                    while (true)
                    {
                        int num2 = pIndex + num;
                        List<string> codeList = pDataList[num2].CodeList;
                        betsCode = this.GetBetsCode(pDataList, num2 + 1);
                        List<List<int>> indexList = CommFunc.GetCodeListByPlay(base.PlayType, base.PlayName, base.RXWZList, betsCode);
                        bool flag3 = CommFunc.VerificationCode(base.Play, indexList, codeList, betsCode) > 0;
                        str3 = str3 + (flag3 ? "1" : "0");
                        if (str3.Length >= betsJKExpect.Length)
                        {
                            break;
                        }
                        num++;
                    }
                }
                if ((str3 == betsJKExpect) || !flag2)
                {
                    if ((zuKeyIsChange || pScheme.IsQQG) || (pScheme.ZuKeySaveList.Count == 0))
                    {
                        betsCode = this.GetBetsCode(pDataList, pIndex);
                        pScheme.ZuKeySaveList = betsCode;
                    }
                    dictionary[pZuKey] = new ConfigurationStatus.BetsCode(pScheme.ZuKeySaveList, null);
                }
                return dictionary;
            }

            public override void CountZuKeyIndexMain(string pZuKey, ConfigurationStatus.BetsScheme pScheme, bool pReset)
            {
                pScheme.ZuKeyIsChange = !pReset;
            }

            private List<string> GetBetsCode(List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> list = new List<string>();
                bool flag = false;
                int num = pIndex;
                ConfigurationStatus.OpenData data = pDataList[pIndex];
                for (int i = 0; i < this.KMTMInfo.KMTMTypeList.Count; i++)
                {
                    int item = Convert.ToInt32(data.CodeList[this.KMTMInfo.KMTMTypeList[i]]);
                    if (this.KMTMInfo.KMTMCodeList.Contains(item))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    list = CommFunc.CopyList(this.KMTMInfo.KMTMValueInfo.NumberList);
                }
                return list;
            }

            public override string InfoToString()
            {
                string str = base.InfoToString();
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["开某投某类型"] = CommFunc.Join(this.KMTMInfo.KMTMTypeList, ","),
                    ["开某投某号码"] = this.KMTMInfo.KMTMCode,
                    ["开某投某内容"] = this.KMTMInfo.KMTMValue
                };
                string str2 = CommFunc.Join(pDic, "\r\n");
                return (str + "\r\n" + str2);
            }

            public override void StringToInfo(string pValue)
            {
                base.StringToInfo(pValue);
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    string str2 = str;
                    if (str2 != null)
                    {
                        if (str2 != "开某投某类型")
                        {
                            if (str2 == "开某投某号码")
                            {
                                goto Label_0079;
                            }
                            if (str2 == "开某投某内容")
                            {
                                goto Label_008D;
                            }
                        }
                        else
                        {
                            this.KMTMInfo.KMTMTypeList = CommFunc.SplitInt(configuration[str], ",");
                        }
                    }
                    continue;
                Label_0079:
                    this.KMTMInfo.KMTMCode = configuration[str];
                    continue;
                Label_008D:
                    this.KMTMInfo.KMTMValue = configuration[str];
                }
            }
        }

        public class FNLHKMTM : ConfigurationStatus.FNBase
        {
            public List<string> BetsValue1List;
            public const string BetsValue1ListString = "龙虎开某投某正投号码";
            public List<string> BetsValue2List;
            public const string BetsValue2ListString = "龙虎开某投某反投号码";
            public List<string> KCIndexList;
            public const string KCIndexListString = "龙虎开某投某选择";
            public List<string> KCValueList;
            public const string KCValueString = "龙虎开某投某开出号码";
            public int KMTMIndex;
            public ConfigurationStatus.KMTMMode KMTMModeInfo;
            public const string KMTMModeInfoString = "龙虎开某投某换号规则";

            public FNLHKMTM()
            {
                this.KCIndexList = new List<string>();
                this.KCValueList = new List<string>();
                this.BetsValue1List = new List<string>();
                this.BetsValue2List = new List<string>();
            }

            public FNLHKMTM(string pValue)
            {
                if (pValue == "")
                {
                    this.KCIndexList = CommFunc.RepeatList("True", 3);
                    List<string> list = new List<string> { 
                        "龙",
                        "虎",
                        "和"
                    };
                    this.KCValueList = list;
                    List<string> list2 = new List<string> { 
                        "龙",
                        "虎",
                        "和"
                    };
                    this.BetsValue1List = list2;
                    List<string> list3 = new List<string> { 
                        "龙",
                        "虎",
                        "和"
                    };
                    this.BetsValue2List = list3;
                    this.KMTMIndex = 0;
                    this.KMTMModeInfo = ConfigurationStatus.KMTMMode.ZT;
                    base.PlayType = "龙虎";
                    base.PlayName = "万千";
                    List<string> list4 = new List<string> { 
                        "6",
                        "12",
                        "25",
                        "52",
                        "108",
                        "224",
                        "465",
                        "965",
                        "2003",
                        "4156"
                    };
                    base.BTPlanList = list4;
                }
                else
                {
                    this.StringToInfo(pValue);
                }
            }

            public override Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> list2;
                string key = "0";
                Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
                if (!pScheme.FNBTIndexDic.ContainsKey(key))
                {
                    pScheme.FNBTIndexDic[key] = 0;
                }
                string betsJKExpect = base.BetsJKExpect;
                string str3 = "";
                bool flag = base.CheckIsNeedJK(pScheme, key, betsJKExpect);
                if (flag)
                {
                    int num = 0;
                    while (true)
                    {
                        int num2 = pIndex + num;
                        List<string> codeList = pDataList[num2].CodeList;
                        list2 = this.GetBetsCode(pDataList, num2 + 1, base.PlayInfo.IndexList, false);
                        List<List<int>> indexList = CommFunc.GetCodeListByPlay(base.PlayType, base.PlayName, base.RXWZList, list2);
                        bool flag2 = CommFunc.VerificationCode(base.Play, indexList, codeList, list2) > 0;
                        str3 = str3 + (flag2 ? "1" : "0");
                        if (str3.Length >= betsJKExpect.Length)
                        {
                            break;
                        }
                        num++;
                    }
                }
                if (!((str3 != betsJKExpect) && flag))
                {
                    int num4 = pScheme.FNBTIndexDic[key];
                    bool pIsChange = num4 >= (pScheme.BTPlanList.Count / 2);
                    list2 = this.GetBetsCode(pDataList, pIndex, base.PlayInfo.IndexList, pIsChange);
                    dictionary[key] = new ConfigurationStatus.BetsCode(list2, null);
                }
                return dictionary;
            }

            private List<string> GetBetsCode(List<ConfigurationStatus.OpenData> pDataList, int pIndex, List<int> pQHIndexList, bool pIsChange)
            {
                List<string> list = new List<string>();
                int num = pIndex;
                ConfigurationStatus.OpenData data = pDataList[pIndex];
                string str = data.CodeList[pQHIndexList[0] - 1];
                string str2 = data.CodeList[pQHIndexList[1] - 1];
                List<string> list2 = new List<string>();
                if (this.KMTMModeInfo == ConfigurationStatus.KMTMMode.ZT)
                {
                    list2 = this.BetsValue1List;
                }
                else if (this.KMTMModeInfo == ConfigurationStatus.KMTMMode.FT)
                {
                    list2 = this.BetsValue2List;
                }
                else if (this.KMTMModeInfo == ConfigurationStatus.KMTMMode.ZFT)
                {
                    list2 = pIsChange ? this.BetsValue2List : this.BetsValue1List;
                }
                else if (this.KMTMModeInfo == ConfigurationStatus.KMTMMode.FZT)
                {
                    list2 = pIsChange ? this.BetsValue1List : this.BetsValue2List;
                }
                for (int i = 0; i < this.KCIndexList.Count; i++)
                {
                    if (Convert.ToBoolean(this.KCIndexList[i]))
                    {
                        string item = CommFunc.CountLH(str, str2);
                        if (CommFunc.SplitString(this.KCValueList[i], ",", -1).Contains(item))
                        {
                            List<string> list4 = CommFunc.SplitString(list2[i], ",", -1);
                            for (int j = 0; j < list4.Count; j++)
                            {
                                string str4 = list4[j];
                                if (!list.Contains(str4))
                                {
                                    list.Add(str4);
                                }
                            }
                        }
                    }
                }
                return list;
            }

            public override string InfoToString()
            {
                string str = base.InfoToString();
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["龙虎开某投某选择"] = CommFunc.Join(this.KCIndexList, ","),
                    ["龙虎开某投某开出号码"] = CommFunc.Join(this.KCValueList, "|"),
                    ["龙虎开某投某正投号码"] = CommFunc.Join(this.BetsValue1List, "|"),
                    ["龙虎开某投某反投号码"] = CommFunc.Join(this.BetsValue2List, "|"),
                    ["龙虎开某投某换号规则"] = Convert.ToInt32(this.KMTMModeInfo).ToString()
                };
                string str2 = CommFunc.Join(pDic, "\r\n");
                return (str + "\r\n" + str2);
            }

            public override void StringToInfo(string pValue)
            {
                base.StringToInfo(pValue);
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    string str2 = str;
                    if (str2 != null)
                    {
                        if (str2 != "龙虎开某投某选择")
                        {
                            if (str2 == "龙虎开某投某开出号码")
                            {
                                goto Label_0095;
                            }
                            if (str2 == "龙虎开某投某正投号码")
                            {
                                goto Label_00AF;
                            }
                            if (str2 == "龙虎开某投某反投号码")
                            {
                                goto Label_00C9;
                            }
                            if (str2 == "龙虎开某投某换号规则")
                            {
                                goto Label_00E3;
                            }
                        }
                        else
                        {
                            this.KCIndexList = CommFunc.SplitString(configuration[str], ",", -1);
                        }
                    }
                    continue;
                Label_0095:
                    this.KCValueList = CommFunc.SplitString(configuration[str], "|", -1);
                    continue;
                Label_00AF:
                    this.BetsValue1List = CommFunc.SplitString(configuration[str], "|", -1);
                    continue;
                Label_00C9:
                    this.BetsValue2List = CommFunc.SplitString(configuration[str], "|", -1);
                    continue;
                Label_00E3:
                    this.KMTMModeInfo = (ConfigurationStatus.KMTMMode) Convert.ToInt32(configuration[str]);
                }
            }
        }

        public class FNLRWCH : ConfigurationStatus.FNBase
        {
            public int LRWExpect;
            public const string LRWExpectString = "冷热温统计期数";
            public string LRWRC;
            public const string LRWRCString = "冷热温容错个数";
            public List<int> LRWTypeList;
            public const string LRWTypeListString = "冷热温出号类型";

            public FNLRWCH()
            {
                this.LRWExpect = 0;
                this.LRWTypeList = new List<int>();
                this.LRWRC = "";
            }

            public FNLRWCH(string pValue)
            {
                this.LRWExpect = 0;
                this.LRWTypeList = new List<int>();
                this.LRWRC = "";
                if (pValue == "")
                {
                    this.LRWExpect = 10;
                    List<int> list = new List<int> { 0 };
                    this.LRWTypeList = list;
                    this.LRWRC = "1-2";
                }
                else
                {
                    this.StringToInfo(pValue);
                }
            }

            public bool CheckLRWCode(string pCode, List<string> pValueList)
            {
                bool flag = false;
                int item = 0;
                for (int i = 0; i < pCode.Length; i++)
                {
                    string str = pCode[i].ToString();
                    if (pValueList.Contains(str))
                    {
                        item++;
                    }
                }
                if (this.LRWRCList.Contains(item))
                {
                    flag = true;
                }
                return flag;
            }

            public override Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> betsCode;
                string pZuKey = "0";
                bool zuKeyIsChange = pScheme.ZuKeyIsChange;
                Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
                string betsJKExpect = base.BetsJKExpect;
                string str3 = "";
                bool flag2 = base.CheckIsNeedJK(pScheme, pZuKey, betsJKExpect);
                if (flag2)
                {
                    int num = 0;
                    while (true)
                    {
                        int num2 = pIndex + num;
                        List<string> codeList = pDataList[num2].CodeList;
                        betsCode = this.GetBetsCode(pDataList, num2 + 1);
                        List<List<int>> indexList = CommFunc.GetCodeListByPlay(base.PlayType, base.PlayName, base.RXWZList, betsCode);
                        bool flag3 = CommFunc.VerificationCode(base.Play, indexList, codeList, betsCode) > 0;
                        str3 = str3 + (flag3 ? "1" : "0");
                        if (str3.Length >= betsJKExpect.Length)
                        {
                            break;
                        }
                        num++;
                    }
                }
                if ((str3 == betsJKExpect) || !flag2)
                {
                    if (zuKeyIsChange || pScheme.IsQQG)
                    {
                        betsCode = this.GetBetsCode(pDataList, pIndex);
                        pScheme.ZuKeySaveList = betsCode;
                    }
                    dictionary[pZuKey] = new ConfigurationStatus.BetsCode(pScheme.ZuKeySaveList, null);
                }
                return dictionary;
            }

            public override void CountZuKeyIndexMain(string pZuKey, ConfigurationStatus.BetsScheme pScheme, bool pReset)
            {
                pScheme.ZuKeyIsChange = !pReset;
            }

            private List<string> GetBetsCode(List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> list5;
                List<string> list6;
                int num2;
                string str;
                List<string> list = new List<string>();
                List<string> list2 = new List<string>();
                List<int> lRWTypeList = this.LRWTypeList;
                if (CommFunc.CheckPlayIsFS(base.Play))
                {
                    list2 = CommFunc.GetCombinaList(ConfigurationStatus.CombinaType.ZX, 1, -1, -1);
                    foreach (int num in this.LRWPlayIndexList)
                    {
                        List<int> playIndexList = new List<int> {
                            num
                        };
                        list5 = this.RefreshSort(playIndexList, pDataList, pIndex);
                        list6 = new List<string>();
                        num2 = 0;
                        while (num2 < lRWTypeList.Count)
                        {
                            str = list5[lRWTypeList[num2]];
                            list6.Add(str);
                            num2++;
                        }
                        List<string> pList = new List<string>();
                        foreach (string str2 in list2)
                        {
                            if (this.CheckLRWCode(str2, list6))
                            {
                                pList.Add(str2);
                            }
                        }
                        string item = CommFunc.Join(pList);
                        list.Add(item);
                    }
                    return list;
                }
                list5 = this.RefreshSort(this.LRWPlayIndexList, pDataList, pIndex);
                list6 = new List<string>();
                for (num2 = 0; num2 < lRWTypeList.Count; num2++)
                {
                    str = list5[lRWTypeList[num2]];
                    list6.Add(str);
                }
                list2 = CommFunc.GetCombinaList(CommFunc.GetCombinaType(base.Play), base.PlayInfo.CodeCount, -1, -1);
                foreach (string str2 in list2)
                {
                    if (this.CheckLRWCode(str2, list6))
                    {
                        list.Add(str2);
                    }
                }
                return list;
            }

            public List<ConfigurationStatus.SortInt> GetLRWSortList(int pExpcet, List<int> playIndexList, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
                for (int i = 0; i < pExpcet; i++)
                {
                    int num = pIndex + i;
                    if (num < pDataList.Count)
                    {
                        List<string> codeList = pDataList[num].CodeList;
                        for (int j = 0; j < playIndexList.Count; j++)
                        {
                            string text = codeList[playIndexList[j]];
                            if (!dictionary.ContainsKey(text))
                            {
                                dictionary[text] = 1;
                            }
                            else
                            {
                                Dictionary<string, int> dictionary3;
                                string key;
                                (dictionary3 = dictionary)[key = text] = dictionary3[key] + 1;
                            }
                        }
                    }
                }
                List<ConfigurationStatus.SortInt> list = new List<ConfigurationStatus.SortInt>();
                for (int i = 0; i < 10; i++)
                {
                    string text2 = i.ToString();
                    int pValue = dictionary.ContainsKey(text2) ? dictionary[text2] : 0;
                    list.Add(new ConfigurationStatus.SortInt(text2, pValue));
                }
                list.Sort(delegate (ConfigurationStatus.SortInt pSort1, ConfigurationStatus.SortInt pSort2)
                {
                    int value = pSort1.Value;
                    int value2 = pSort2.Value;
                    int result;
                    if (value == value2)
                    {
                        string key2 = pSort1.Key;
                        string key3 = pSort2.Key;
                        result = string.Compare(key2, key3);
                    }
                    else
                    {
                        result = value2 - value;
                    }
                    return result;
                });
                return list;
            }

            public override string InfoToString()
            {
                string str = base.InfoToString();
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["冷热温统计期数"] = this.LRWExpect.ToString(),
                    ["冷热温出号类型"] = CommFunc.Join(this.LRWTypeList, ","),
                    ["冷热温容错个数"] = this.LRWRC.ToString()
                };
                string str2 = CommFunc.Join(pDic, "\r\n");
                return (str + "\r\n" + str2);
            }

            public List<string> RefreshSort(List<int> playIndexList, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> list = new List<string>();
                if (pDataList.Count != 0)
                {
                    List<ConfigurationStatus.SortInt> list2 = this.GetLRWSortList(this.LRWExpect, playIndexList, pDataList, pIndex);
                    for (int i = 0; i < list2.Count; i++)
                    {
                        string key = list2[i].Key;
                        list.Add(key);
                    }
                }
                return list;
            }

            public override void StringToInfo(string pValue)
            {
                base.StringToInfo(pValue);
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    string str2 = str;
                    if (str2 != null)
                    {
                        if (str2 != "冷热温统计期数")
                        {
                            if (str2 == "冷热温出号类型")
                            {
                                goto Label_006F;
                            }
                            if (str2 == "冷热温容错个数")
                            {
                                goto Label_0088;
                            }
                        }
                        else
                        {
                            this.LRWExpect = Convert.ToInt32(configuration[str]);
                        }
                    }
                    continue;
                Label_006F:
                    this.LRWTypeList = CommFunc.SplitInt(configuration[str], ",");
                    continue;
                Label_0088:
                    this.LRWRC = configuration[str];
                }
            }

            public List<int> LRWPlayIndexList
            {
                get
                {
                    List<int> list = new List<int>();
                    List<int> indexList = base.PlayInfo.IndexList;
                    if (CommFunc.CheckPlayIsRXDS(base.Play))
                    {
                        indexList = base.PlayInfo.ConvertRXWZList(base.RXWZList);
                    }
                    foreach (int num in indexList)
                    {
                        list.Add(num - 1);
                    }
                    return list;
                }
            }

            public List<int> LRWRCList =>
                CommFunc.ConvertIntList(this.LRWRC);
        }

        public class FNSJCH : ConfigurationStatus.FNBase
        {
            public int SJCHCount;
            public const string SJCHCountString = "随机出号个数";
            public string SJCHTemplate;
            public const string SJCHTemplateString = "随机出号模板";

            public FNSJCH()
            {
            }

            public FNSJCH(string pValue)
            {
                if (pValue == "")
                {
                    this.SJCHTemplate = "模板1";
                    this.SJCHCount = 3;
                }
                else
                {
                    this.StringToInfo(pValue);
                }
            }

            public override Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                string str = "0";
                bool zuKeyIsChange = pScheme.ZuKeyIsChange;
                Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
                if (zuKeyIsChange || pScheme.IsQQG)
                {
                    List<string> betsCode = this.GetBetsCode();
                    pScheme.ZuKeySaveList = betsCode;
                }
                dictionary[str] = new ConfigurationStatus.BetsCode(pScheme.ZuKeySaveList, null);
                return dictionary;
            }

            public override void CountZuKeyIndexMain(string pZuKey, ConfigurationStatus.BetsScheme pScheme, bool pReset)
            {
                pScheme.ZuKeyIsChange = !pReset;
            }

            private List<string> GetBetsCode()
            {
                List<string> list = new List<string>();
                int pCycle = 1;
                return CommFunc.SplitBetsCode(CommFunc.GetRandomByPlay(base.PlayType, base.PlayName, this.SJCHTemplate, "", pCycle, this.SJCHCount)[0], base.Play);
            }

            public override string InfoToString()
            {
                string str = base.InfoToString();
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["随机出号模板"] = this.SJCHTemplate,
                    ["随机出号个数"] = this.SJCHCount.ToString()
                };
                string str2 = CommFunc.Join(pDic, "\r\n");
                return (str + "\r\n" + str2);
            }

            public override void StringToInfo(string pValue)
            {
                base.StringToInfo(pValue);
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    switch (str)
                    {
                        case "随机出号模板":
                            this.SJCHTemplate = configuration[str];
                            break;

                        case "随机出号个数":
                            this.SJCHCount = Convert.ToInt32(configuration[str]);
                            break;
                    }
                }
            }
        }

        public class FNWJJH : ConfigurationStatus.FNBase
        {
            public IntPtr HWND;
            public string HWNDPlay;
            public const string HWNDPlayString = "外接计划玩法";
            public string HWNDSeparate;
            public const string HWNDSeparateString = "外接计划分割符";
            public const string HWNDString = "外接计划句柄";
            public string HWNDValueLeft;
            public const string HWNDValueLeftString = "外接计划内容左";
            public string HWNDValueRight;
            public const string HWNDValueRightString = "外接计划内容右";

            public FNWJJH()
            {
            }

            public FNWJJH(string pValue)
            {
                if (pValue == "")
                {
                    this.HWND = IntPtr.Zero;
                    this.HWNDSeparate = "-";
                    this.HWNDPlay = "";
                    this.HWNDValueLeft = "【";
                    this.HWNDValueRight = "】";
                }
                else
                {
                    this.StringToInfo(pValue);
                }
            }

            public override Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                string str = "0";
                bool zuKeyIsChange = pScheme.ZuKeyIsChange;
                Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
                if ((zuKeyIsChange || pScheme.IsQQG) || (pScheme.ZuKeySaveList.Count == 0))
                {
                    List<string> betsCode = this.GetBetsCode();
                    pScheme.ZuKeySaveList = betsCode;
                }
                dictionary[str] = new ConfigurationStatus.BetsCode(pScheme.ZuKeySaveList, null);
                return dictionary;
            }

            public override void CountZuKeyIndexMain(string pZuKey, ConfigurationStatus.BetsScheme pScheme, bool pReset)
            {
                pScheme.ZuKeyIsChange = !pReset;
            }

            public List<string> GetBetsCode()
            {
                List<string> list = new List<string>();
                if (this.HWND == IntPtr.Zero)
                {
                    return list;
                }
                ConfigurationStatus.WJJHInfo pInfo = new ConfigurationStatus.WJJHInfo {
                    HWNDString = this.HWND.ToString(),
                    CSSeparate = this.HWNDSeparate,
                    CSPlay = this.HWNDPlay,
                    CSValueLeft = this.HWNDValueLeft,
                    CSValueRight = this.HWNDValueRight,
                    PlayType = base.PlayType,
                    PlayName = base.PlayName
                };
                FNWJJHLine.GetPlanValue(pInfo);
                return CommFunc.SplitBetsCode(pInfo.PlanValue, pInfo.Play);
            }

            public override string InfoToString()
            {
                string str = base.InfoToString();
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["外接计划句柄"] = this.HWND.ToString(),
                    ["外接计划分割符"] = this.HWNDSeparate,
                    ["外接计划玩法"] = this.HWNDPlay,
                    ["外接计划内容左"] = this.HWNDValueLeft,
                    ["外接计划内容右"] = this.HWNDValueRight
                };
                string str2 = CommFunc.Join(pDic, "\r\n");
                return (str + "\r\n" + str2);
            }

            public override void StringToInfo(string pValue)
            {
                base.StringToInfo(pValue);
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    string str2 = str;
                    if (str2 != null)
                    {
                        if (str2 != "外接计划句柄")
                        {
                            if (str2 == "外接计划分割符")
                            {
                                goto Label_008F;
                            }
                            if (str2 == "外接计划玩法")
                            {
                                goto Label_009E;
                            }
                            if (str2 == "外接计划内容左")
                            {
                                goto Label_00AD;
                            }
                            if (str2 == "外接计划内容右")
                            {
                                goto Label_00BC;
                            }
                        }
                        else
                        {
                            this.HWND = CommFunc.ConvertIntPtr(configuration[str]);
                        }
                    }
                    continue;
                Label_008F:
                    this.HWNDSeparate = configuration[str];
                    continue;
                Label_009E:
                    this.HWNDPlay = configuration[str];
                    continue;
                Label_00AD:
                    this.HWNDValueLeft = configuration[str];
                    continue;
                Label_00BC:
                    this.HWNDValueRight = configuration[str];
                }
            }
        }

        public class FNYLCH : ConfigurationStatus.FNBase
        {
            public string YLCode;
            public string YLCount;
            public const string YLCountString = "遗漏出号个数";
            public int YLExpect;
            public const string YLExpectString = "遗漏出号期数";
            public string YLRC;
            public const string YLRCString = "遗漏出号容错个数";
            public ConfigurationStatus.YLCHType YLTypeInfo;
            public const string YLTypeListString = "遗漏出号类型";

            public FNYLCH()
            {
                this.YLExpect = 0;
                this.YLRC = "";
                this.YLCount = "";
                this.YLCode = "";
            }

            public FNYLCH(string pValue)
            {
                this.YLExpect = 0;
                this.YLRC = "";
                this.YLCount = "";
                this.YLCode = "";
                if (pValue == "")
                {
                    this.YLExpect = 10;
                    this.YLTypeInfo = ConfigurationStatus.YLCHType.DaYu;
                    this.YLRC = "1-2";
                    this.YLCount = "1-10";
                }
                else
                {
                    this.StringToInfo(pValue);
                }
            }

            public override Dictionary<string, ConfigurationStatus.BetsCode> CountNumber(ConfigurationStatus.BetsScheme pScheme, List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> betsCode;
                string pZuKey = "0";
                bool zuKeyIsChange = pScheme.ZuKeyIsChange;
                Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
                string betsJKExpect = base.BetsJKExpect;
                string str3 = "";
                bool flag2 = base.CheckIsNeedJK(pScheme, pZuKey, betsJKExpect);
                if (flag2)
                {
                    int num = 0;
                    while (true)
                    {
                        int num2 = pIndex + num;
                        List<string> codeList = pDataList[num2].CodeList;
                        betsCode = this.GetBetsCode(pDataList, num2 + 1);
                        List<List<int>> indexList = CommFunc.GetCodeListByPlay(base.PlayType, base.PlayName, base.RXWZList, betsCode);
                        bool flag3 = CommFunc.VerificationCode(base.Play, indexList, codeList, betsCode) > 0;
                        str3 = str3 + (flag3 ? "1" : "0");
                        if (str3.Length >= betsJKExpect.Length)
                        {
                            break;
                        }
                        num++;
                    }
                }
                if ((str3 == betsJKExpect) || !flag2)
                {
                    if ((zuKeyIsChange || pScheme.IsQQG) || (pScheme.ZuKeySaveList.Count == 0))
                    {
                        betsCode = this.GetBetsCode(pDataList, pIndex);
                        pScheme.ZuKeySaveList = betsCode;
                    }
                    dictionary[pZuKey] = new ConfigurationStatus.BetsCode(pScheme.ZuKeySaveList, null);
                }
                return dictionary;
            }

            public override void CountZuKeyIndexMain(string pZuKey, ConfigurationStatus.BetsScheme pScheme, bool pReset)
            {
                pScheme.ZuKeyIsChange = !pReset;
            }

            private List<string> GetBetsCode(List<ConfigurationStatus.OpenData> pDataList, int pIndex)
            {
                List<string> list = new List<string>();
                return FNYLCHLine.GetPlanValue(this, pDataList, pIndex);
            }

            public override string InfoToString()
            {
                string str = base.InfoToString();
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["遗漏出号期数"] = this.YLExpect.ToString(),
                    ["遗漏出号类型"] = Convert.ToInt32(this.YLTypeInfo).ToString(),
                    ["遗漏出号容错个数"] = this.YLRC,
                    ["遗漏出号个数"] = this.YLCount
                };
                string str2 = CommFunc.Join(pDic, "\r\n");
                return (str + "\r\n" + str2);
            }

            public override void StringToInfo(string pValue)
            {
                base.StringToInfo(pValue);
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, "\r\n");
                foreach (string str in configuration.Keys)
                {
                    string str2 = str;
                    if (str2 != null)
                    {
                        if (str2 != "遗漏出号期数")
                        {
                            if (str2 == "遗漏出号类型")
                            {
                                goto Label_007F;
                            }
                            if (str2 == "遗漏出号容错个数")
                            {
                                goto Label_0093;
                            }
                            if (str2 == "遗漏出号个数")
                            {
                                goto Label_00A2;
                            }
                        }
                        else
                        {
                            this.YLExpect = Convert.ToInt32(configuration[str]);
                        }
                    }
                    continue;
                Label_007F:
                    this.YLTypeInfo = (ConfigurationStatus.YLCHType) Convert.ToInt32(configuration[str]);
                    continue;
                Label_0093:
                    this.YLRC = configuration[str];
                    continue;
                Label_00A2:
                    this.YLCount = configuration[str];
                }
            }

            public List<int> YLCountList =>
                CommFunc.ConvertIntList(this.YLCount);

            public List<int> YLRCList =>
                CommFunc.ConvertIntList(this.YLRC);
        }

        public delegate HtmlDocument GetHtmlDocumentDelegate();

        public delegate string GetLoginUrlDelegate();

        public class GJBTScheme
        {
            public bool IsGJBTEncrypt = false;
            public bool IsInputPW = false;
            public List<ConfigurationStatus.TimesScheme> TimesSchemeList = new List<ConfigurationStatus.TimesScheme>();

            public GJBTScheme(List<ConfigurationStatus.TimesScheme> pTimesSchemeList = null, bool pIsGJBTEncrypt = false, bool pIsInputPW = false)
            {
                if (pTimesSchemeList != null)
                {
                    this.TimesSchemeList = pTimesSchemeList;
                }
                this.IsGJBTEncrypt = pIsGJBTEncrypt;
                this.IsInputPW = pIsInputPW;
            }

            public static ConfigurationStatus.GJBTScheme GetBTFNByFileValue(string pGJBTValue)
            {
                bool pIsGJBTEncrypt = false;
                if (AppInfo.Account.Configuration.IsGJBTEncrypt)
                {
                    string str = CommFunc.Decode(pGJBTValue, AppInfo.Account.Configuration.GJBTEncrypt);
                    if (str != "")
                    {
                        pGJBTValue = str;
                        pIsGJBTEncrypt = true;
                    }
                }
                List<string> list = CommFunc.SplitString(pGJBTValue, "\r\n", -1);
                List<ConfigurationStatus.TimesScheme> pTimesSchemeList = new List<ConfigurationStatus.TimesScheme>();
                foreach (string str2 in list)
                {
                    if (str2 != "")
                    {
                        ConfigurationStatus.TimesScheme item = new ConfigurationStatus.TimesScheme(str2);
                        if (item.ID == -1)
                        {
                            return null;
                        }
                        pTimesSchemeList.Add(item);
                    }
                }
                ConfigurationStatus.GJBTScheme scheme2 = new ConfigurationStatus.GJBTScheme(pTimesSchemeList, pIsGJBTEncrypt, false);
                string appName = scheme2.AppName;
                if (((appName != null) && (appName != "")) && (appName != "YXZXGJ"))
                {
                    return null;
                }
                return scheme2;
            }

            public string GetFileValue()
            {
                List<ConfigurationStatus.TimesScheme> timesSchemeList = this.TimesSchemeList;
                List<string> pList = new List<string>();
                foreach (ConfigurationStatus.TimesScheme scheme in timesSchemeList)
                {
                    string item = scheme.InfoToString();
                    pList.Add(item);
                }
                string pSource = CommFunc.Join(pList, "\r\n");
                if (AppInfo.Account.Configuration.IsGJBTEncryptID || this.IsGJBTEncrypt)
                {
                    pSource = CommFunc.Encode(pSource, AppInfo.Account.Configuration.GJBTEncrypt);
                }
                return pSource;
            }

            public string AppName
            {
                get
                {
                    foreach (ConfigurationStatus.TimesScheme scheme in this.TimesSchemeList)
                    {
                        return scheme.AppName;
                    }
                    return "";
                }
            }

            public bool IsViewGJBTEncrypt =>
                ((this.IsGJBTEncrypt && !AppInfo.Account.Configuration.IsGJBTEncryptID) && !this.IsInputPW);
        }

        public class GJDMLH
        {
            public int ID;
            public int NoAfter;
            public List<string> NumberList;
            public string Value;
            public int YesAfter;

            public GJDMLH()
            {
                this.ID = -1;
                this.Value = "";
                this.NumberList = new List<string>();
                this.YesAfter = 1;
                this.NoAfter = 1;
            }

            public GJDMLH(ConfigurationStatus.GJDMLH pInfo)
            {
                this.ID = -1;
                this.Value = "";
                this.NumberList = new List<string>();
                this.YesAfter = 1;
                this.NoAfter = 1;
                this.ID = pInfo.ID;
                this.Value = pInfo.Value;
                this.YesAfter = pInfo.YesAfter;
                this.NoAfter = pInfo.NoAfter;
            }

            public GJDMLH(int pID)
            {
                this.ID = -1;
                this.Value = "";
                this.NumberList = new List<string>();
                this.YesAfter = 1;
                this.NoAfter = 1;
                this.ID = pID;
            }
        }

        public class HandlerCode
        {
            public bool IsSH = false;
            public bool SHZ = false;
            public bool SHZW = false;
            public bool SKD = false;
            public bool SLH = false;
            public int SLHExpect = 10;
            public bool STH = false;
            public int STHExpect = 10;
            public Dictionary<string, string> THCodeDic = new Dictionary<string, string>();

            public void InfoToString(List<string> pRowList)
            {
                pRowList.Add(this.IsSH.ToString());
                pRowList.Add(this.SHZ.ToString());
                pRowList.Add(this.SHZW.ToString());
                pRowList.Add(this.SKD.ToString());
                pRowList.Add(this.SLH.ToString());
                pRowList.Add(this.SLHExpect.ToString());
                pRowList.Add(this.STH.ToString());
                pRowList.Add(this.STHExpect.ToString());
            }

            public void StringToInfo(List<string> pRowList, int pIndex)
            {
                this.IsSH = Convert.ToBoolean(pRowList[pIndex]);
                pIndex++;
                this.SHZ = Convert.ToBoolean(pRowList[pIndex]);
                pIndex++;
                this.SHZW = Convert.ToBoolean(pRowList[pIndex]);
                pIndex++;
                this.SKD = Convert.ToBoolean(pRowList[pIndex]);
                pIndex++;
                this.SLH = Convert.ToBoolean(pRowList[pIndex]);
                pIndex++;
                this.SLHExpect = Convert.ToInt32(pRowList[pIndex]);
                pIndex++;
                this.STH = Convert.ToBoolean(pRowList[pIndex]);
                pIndex++;
                this.STHExpect = Convert.ToInt32(pRowList[pIndex]);
                pIndex++;
            }
        }

        public class IntervalTime
        {
            public string beginOpenTime;
            public bool DownLoadOk = false;
            public string endOpenTime;

            public IntervalTime(string pTime1, string pTime2)
            {
                this.beginOpenTime = pTime1;
                this.endOpenTime = pTime2;
            }
        }

        public enum JKMode
        {
            YZJK,
            SCKJ
        }

        public class KMTM
        {
            public string KMTMCode;
            public List<int> KMTMTypeList;
            public string KMTMValue;
            public ConfigurationStatus.DMLH KMTMValueInfo;

            public KMTM()
            {
                this.KMTMTypeList = new List<int>();
                this.KMTMCode = "";
                this.KMTMValueInfo = new ConfigurationStatus.DMLH();
            }

            public KMTM(ConfigurationStatus.KMTM pInfo)
            {
                this.KMTMTypeList = new List<int>();
                this.KMTMCode = "";
                this.KMTMValueInfo = new ConfigurationStatus.DMLH();
                this.KMTMTypeList = CommFunc.CopyList<int>(pInfo.KMTMTypeList);
                this.KMTMCode = pInfo.KMTMCode;
                this.KMTMValue = pInfo.KMTMValue;
            }

            public List<int> KMTMCodeList =>
                CommFunc.ConvertIntList(this.KMTMCode);

            public string KMTMViewType
            {
                get
                {
                    List<string> pList = new List<string>();
                    for (int i = 0; i < this.KMTMTypeList.Count; i++)
                    {
                        int num2 = this.KMTMTypeList[i];
                        string item = AppInfo.IndexDic[num2];
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, ",");
                }
            }
        }

        public enum KMTMMode
        {
            ZT,
            FT,
            ZFT,
            FZT
        }

        public delegate void LoadConfigurationDelegate();

        public delegate void LoadConfigurationLaterDelegate();

        public delegate void LoadDelegate();

        public delegate void LoginLotteryDelegate(int pIndex);

        public delegate void LoginMainDelegate(bool Switch, string pName = "");

        public class LotteryConfig
        {
            public int Code;
            public List<int> CodeList;
            public int Expect;
            public int GetInterval;
            public ConfigurationStatus.LotteryGroup Group;
            public string GroupString;
            public string ID;
            public bool IsLoadServerData;
            public int Max;
            public int Min;
            public string Name;
            public string NextExpect;
            public double OpenInterval;
            public int RefreshExpect;
            public int SaveExpect;
            public Dictionary<string, string> TimeDic;
            public List<ConfigurationStatus.IntervalTime> TimeList;
            public ConfigurationStatus.LotteryType Type;
            public XmlNode XmlLotteryNode;
            public XmlNode XmlPlanNode;
            public XmlNode XmlPlayNode;
            public XmlNode XmlShrinkNode;
            public XmlNode XmlTrendNode;
        }

        public enum LotteryGroup
        {
            GPSSC,
            GP11X5,
            GPPK10,
            GP3D
        }

        public enum LotteryType
        {
            CQSSC,
            XJSSC,
            TJSSC,
            GD11X5,
            SD11X5,
            JX11X5,
            AH11X5,
            SH11X5,
            BDFFC,
            BD2FC,
            BD11X5,
            MRFFC,
            MR2FC,
            HRFFC,
            HR2FC,
            JYFFC,
            JY3FC,
            JWFFC,
            JW3FC,
            BM1FC,
            BM2FC,
            BM5FC,
            XBFFC,
            XB3FC,
            AUFFC,
            AU3FC,
            NYFFC,
            NY3FC,
            OEFFC,
            LTFFC,
            LT2FC,
            DFFFC,
            DF3FC,
            YCFFC,
            YC2FC,
            WJH5FC,
            JXFFC,
            QYFFC,
            MZCFFC,
            MZC3FC,
            HGSSC,
            DTFFC,
            DT2FC,
            M5FFC,
            M53FC,
            DAFFC,
            DA3FC,
            KLFFC,
            KL2FC,
            UCFFC,
            UC5FC,
            GFFFC,
            GF3FC,
            LFFFC,
            LF2FC,
            LF5FC,
            BBFFC,
            BB3FC,
            JNDSSC,
            TWSSC,
            BJSSC,
            MR11X5,
            LEFFFC,
            LEF2FC,
            LEF5FC,
            LF11X5,
            BNFFC,
            BN5FC,
            PK10,
            WXFFC,
            WX3FC,
            LXFFC,
            LX3FC,
            LX5FC,
            KCFFC,
            KC2FC,
            KC5FC,
            JYINFFC,
            JYIN3FC,
            MRPK10,
            XB5FC,
            GDFFC,
            OE3FC,
            K5FFC,
            K55FC,
            FSFFC,
            FS5FC,
            BHZYFFC,
            BHZY5FC,
            A6FFC,
            A65FC,
            YIFAFFC,
            YIFA5FC,
            XJPSSC,
            ZDFFC,
            ZD5FC,
            DJSSC,
            DPSSC,
            SKYFFC,
            SKY2FC,
            XDLSSC,
            FLBSSC,
            HHFFC,
            HH3FC,
            DPCFFC,
            DPC5FC,
            LFHGSSC,
            LFDJSSC,
            ELSSSC,
            LUDIFFC,
            LUDI3FC,
            JSK3,
            XXLSSC,
            LF2FFC,
            LF22FC,
            LF25FC,
            JH15C,
            JH2FC,
            JH5FC,
            CTTFFC,
            CTT5FC,
            TXFFC,
            QQFFC,
            DYJFFC,
            DYJ2FC,
            NQQFFC,
            BDSSC,
            GGSSC,
            HGLTC,
            MD2FC,
            VRSSC,
            VRPK10,
            SESSC,
            YBAOFFC,
            YBAO2FC,
            JDSSC,
            YRFFC,
            YR2FC,
            YR5FC,
            YRTXFFC,
            UCHL2FC,
            UCTWSSC,
            WHDJSSC,
            WHXJPSSC,
            ZDDJSSC,
            HG1FC,
            DB15C,
            YRHG15C,
            VRHXSSC,
            VR3FC,
            JBEITG30,
            XJP120M,
            XJP15F,
            XDL90M,
            WX11X5,
            WX15F,
            WX5FC,
            QQ30M,
            QQ15F,
            NY5FC,
            MDHGSSC,
            MDHG90M,
            JHTXFFC,
            JHQQFFC,
            TRFFC,
            TR2FC,
            HLCFFC,
            HLC2FC,
            HLC5FC,
            JN15F,
            UCHGSSC,
            HCSSC,
            VRKT,
            M511X5,
            M55FC,
            HGDBFFC,
            HGDB5FC,
            BJ2HGSSC,
            HLCFLB15C,
            HLCFLB2FC,
            HLCFLB5FC,
            TR11X5,
            JZD15FC,
            LD2FC,
            GGFFC,
            HLCHG15F,
            HLCDJ15F,
            QQTFFC,
            QQT2FC,
            QQT5FC,
            QQTTG15C,
            QQTNY15C,
            QQTTXFFC,
            QQTTX2FC,
            DPCDJSSC,
            WS30M,
            WSBLS60M,
            WSHS15F,
            WSFS15F,
            WSXDL15F,
            BMEIFFC,
            BMEI5FC,
            QQTHGPK10,
            WBJFFC,
            WBJ2FC,
            WBJ5FC,
            WBJDX15C,
            WBJNY15C,
            WBJHG2FC,
            WBJMSK35C,
            WBJBJSSC,
            WBJOM11X5,
            WBJOMPK10,
            WBJNNPK10,
            BMEISE15F,
            BLSFFC,
            YTXJPSSC,
            XYFTPK10,
            ZDHS15F,
            ZDRBSSC,
            HLJSSC,
            YNSSC,
            JL11X5,
            TJ11X5,
            BJ11X5,
            FJ11X5,
            GS11X5,
            GX11X5,
            GZ11X5,
            HEB11X5,
            HLJ11X5,
            HUB11X5,
            JS11X5,
            LN11X5,
            NMG11X5,
            SXL11X5,
            SXR11X5,
            XJ11X5,
            YN11X5,
            ZJ11X5,
            GXK3,
            HUBK3,
            JLK3,
            JXK3,
            AHK3,
            BJK3,
            FJK3,
            GSK3,
            GZK3,
            HEBK3,
            NMGK3,
            SHK3,
            BMTXFFC,
            XDL11X5,
            JZD11X5,
            XDLPK10,
            JZDPK10,
            XHDFFFC,
            XHDF5FC,
            XHDFDJSSC,
            XHDFHGSSC,
            XHDFTXFFC,
            WBJTG30,
            WBJXXLSSC,
            WBJDJSSC,
            WBJHGSSC,
            WBJXDLSSC,
            WBJXJPSSC,
            WBJJNDSSC,
            QQTXXLSSC,
            QQTJNDSSC,
            QQTXDLSSC,
            QQTDJSSC,
            QQTHGSSC,
            NBTGSSC,
            NBRBSSC,
            NBFFC,
            NB5FC,
            YSENFFC,
            YSEN2FC,
            YSEN5FC,
            YSENXGBFC,
            YSENDM45C,
            YSENDL2FC,
            YSENDJ35C,
            YSENXJPSSC,
            YSENJNDSSC,
            YSENHGSSC,
            YSENLT15C,
            YSENXDLSSC,
            YSENJZ15C,
            YSENDJSSC,
            YSENXGSSC,
            YSENJZ11X5,
            YSENTBPK10,
            YSENXGPK10,
            YSENBJSSC,
            MR45C,
            HYGGFFC,
            HYTTFFC,
            HYSkyFFC,
            HYYJFFC,
            HYHLWFFC,
            HYYTFFC,
            HYHGSSC,
            HYDJSSC,
            HYFLBSSC,
            HYXDLSSC,
            HYXJPSSC,
            HYJNDFFC,
            HYHNFFC,
            HYFFC,
            HY2FC,
            HY11X5,
            HYPK10,
            BMHGSSC,
            BMDJSSC,
            BMFLBSSC,
            BMTWFFC,
            BMQQFFC,
            WMFFC,
            WM2FC,
            WMTWSSC,
            WMXXLSSC,
            WMHGSSC,
            WMPK10,
            TAHGSSC,
            TAFLBSSC,
            TAFFC,
            TA3FC,
            TA5FC,
            TA11X5FFC,
            TA11X53FC,
            YDXXLSSC,
            YDHGSSC,
            YDTXFFC,
            YDFFPK10,
            BKCFFC,
            BKC2FC,
            BKC5FC,
            BKC11X5FFC,
            YDFFC,
            YD2FC,
            MINCDJSSC,
            MINCSE15F,
            MINCNY15C,
            MINCFFC,
            MINC2FC,
            MINC5FC,
            JLSSC,
            NMGSSC,
            SIJIFLBSSC,
            SIJIHGSSC,
            SIJIDJSSC,
            SIJIELSSSC,
            SIJIFFC,
            SIJI3FC,
            SIJI5FC,
            HB11X5,
            QH11X5,
            HN11X5,
            SIJIFF11X5,
            SIJI3F11X5,
            SIJI5F11X5,
            HZQQFFC,
            HZFFC,
            HZ3FC,
            HZ5FC,
            HZXXLSSC,
            HZJNDSSC,
            HZHG2FC,
            HZXJP2FC,
            HZHGSSC,
            HZDJSSC,
            HZXDLSSC,
            HZTG15F,
            HZML15F,
            HZFF11X5,
            HZTG11X5,
            ZXGBFFC,
            ZXBX3FC,
            ZXBX5FC,
            ZXXXLSSC,
            ZXJNDSSC,
            ZXHG2FC,
            ZXXJP2FC,
            ZXHGSSC,
            ZXDJSSC,
            ZXXDLSSC,
            ZXTG15F,
            ZXML15F,
            ZXFF11X5,
            UT8HGSSC,
            UT8DJSSC,
            UT8FFC,
            UT83FC,
            LD11X5,
            LDPK10,
            XCQQFFC,
            XCHGSSC,
            XCFLBSSC,
            XCYNFFC,
            XCYN3FC,
            XCYN5FC,
            XC11X5FFC,
            XC11X53FC,
            FCOZ3FC,
            FCSLFK5FC,
            FEICFFC,
            FEIC3FC,
            FEICSLFK5FC,
            FEICXJP2FC,
            FEICHGSSC,
            FEICDJSSC,
            FEIC30M,
            FEIC45M,
            FEICFF11X5,
            FEIC2F11X5,
            FEIC3F11X5,
            FEIC5F11X5,
            FEICFFPK10,
            FEIC2FPK10,
            FEIC3FPK10,
            FEIC5FPK10,
            SIJITXYFC,
            UCRDFFC,
            UCRD2FC,
            UC3F11X5,
            LMHHGSSC,
            LMHDJSSC,
            LMHXY45M,
            XGSM,
            HANYTXFFC,
            HANYHGSSC,
            HANYDJSSC,
            HANYXJP30M,
            HANYFLP30M,
            HANYFLPFFC,
            HANYFLP2FC,
            HANYJPZ30M,
            HANYJPZFFC,
            HANYJPZ5FC,
            HANYMD30M,
            HANYMDFFC,
            HANYMD3FC,
            BHZYHGSSC,
            BHZYDJSSC,
            BHZYTXFFC,
            YBAO45C,
            XCWX15C,
            XCBL15C,
            JHJPZ15C,
            CAIHFLP15C,
            CAIHFLP2FC,
            CAIHFLP5FC,
            CAIHDLD30M,
            CAIHXWYFFC,
            CAIHXDLSSC,
            CAIHHGSSC,
            CAIHDJSSC,
            CAIHXJPSSC,
            CAIHLD2FC,
            CAIHPK10,
            LYSSLFK5FC,
            THFFC,
            THMD2FC,
            THDJSSC,
            THJDSSC,
            THTGSSC,
            TH5FC,
            THHGSSC,
            THPK10,
            THOZPK10,
            XCTXFFC,
            TATXFFC,
            TABL15C,
            TAWX15C,
            TAWBFFC,
            HSFFC,
            HS5FC,
            HSSE15F,
            HSQQFFC,
            HSDJSSC,
            DAZRBSSC,
            DAZFFC,
            DAZ5FC,
            DAZTG15C,
            DAZDJSSC,
            DAZJDSSC,
            DAZHGSSC,
            DAZ11X5,
            BNTXFFC,
            CAIHSE15F,
            CAIHNY15C,
            SLTHHGSSC,
            SLTHNHGSSC,
            SLTHDJSSC,
            SLTHFFC,
            SLTH2FC,
            HLCSE15F,
            RDYLFFC,
            RDYL2FC,
            RDYL5FC,
            HLCNY15C,
            K3XXLSSC,
            K3DJFFC,
            K3HG2FC,
            K3MG5FC,
            K3HGSSC,
            K3DJSSC,
            K311X5,
            JFYLBHD15C,
            JFYLFFC,
            JFYL2FC,
            JFYL2F11X5,
            WSNDJSSC,
            WSELSSSC,
            WSQQFFC,
            WSFLP15C,
            WS11X5,
            WS120MPK10,
            WS180MPK10,
            FLCFFC,
            FLC2FC,
            FLC5FC,
            K3TXFFC,
            ZDTGSSC,
            MYOZBWC,
            K3NTXFFC,
            JHCFFC,
            JHC2FC,
            JHC5FC,
            JHCDBFFC,
            JHCJDSSC,
            WDYLFFC,
            WDYL2FC,
            WDYL5FC,
            WDYL11X5,
            QJCFFC,
            QJC2FC,
            QJC45C,
            QJC11X5,
            QJCPK10,
            DPCTXFFC,
            HNYL5FC,
            HNYLFFC,
            HNYLFLP15C,
            HNYLXJPSSC,
            HNYL11X5,
            BWTFFC,
            BWT5FC,
            BWTDJSSC,
            BWTHGSSC,
            BWTTXFFC,
            BWTOZBWC,
            BWT11X5,
            WHCFFC,
            WHC3FC,
            JFYLGX2FC,
            ZYLTGSSC,
            ZYLHS15F,
            ZYLDJSSC,
            ZYLRBSSC,
            ZYLFFC,
            ZYL5FC,
            CBLDLD30M,
            CBLXWYFFC,
            CBLXDLSSC,
            CBLHGSSC,
            CBLDJSSC,
            CBLXJPSSC,
            CBLLD2FC,
            CBLSE15F,
            CBLNY15C,
            CBLPK10,
            YL2028HGSSC,
            YL2028DJSSC,
            YL2028ML20M,
            YL2028DZ30M,
            YL2028WXFFC,
            YL2028FLP2FC,
            YL2028MG45M,
            YL2028PK10,
            YL2028FFPK10,
            CLYLAM15C,
            CLYLTB15C,
            CLYLDJSSC,
            CLYLXDL15F,
            CLYLHGSSC,
            CLYLFFC,
            CLYL2FC,
            CLYL3FC,
            CLYL5FC,
            CLYLFF11X5,
            CLYL2F11X5,
            CLYL3F11X5,
            CLYL5F11X5,
            THENXJP30M,
            THENMGFFC,
            THENHGSSC,
            THENDJSSC,
            THENXJPSSC,
            THENXDLSSC,
            THENELSSSC,
            THENYD15C,
            THENJND11X5,
            THENNY11X5,
            THEN120MPK10,
            THEN180MPK10,
            MINCTXFFC,
            HUBOFFC,
            HUBO2FC,
            HUBO5FC,
            HUBOHGSSC,
            HUBODJSSC,
            HUBOML20M,
            HUBODZ30M,
            HUBOWXFFC,
            HUBOFLP2FC,
            HUBOMG45M,
            LDYLBL15C,
            LDYLWX15C,
            LDYLWBFFC,
            LDYLHGSSC,
            LDYLFLBSSC,
            LDYLFFC,
            LDYL3FC,
            LDYL5FC,
            LDYL11X5FFC,
            LDYL11X53FC,
            QJCHL15C,
            QJCAM15C,
            QJCXNFFC,
            QJCPDFFC,
            HKCFFC,
            HKC2FC,
            HKC5FC,
            HKC11X5FFC,
            SYYLFFC,
            MTDLD30M,
            MTXWYFFC,
            MTXDLSSC,
            MTHGSSC,
            MTDJSSC,
            MTXJPSSC,
            MTLD2FC,
            MTSE15F,
            MTNY15C,
            MTPK10,
            DPCOZBWC,
            HUBOTXFFC,
            DQOZ3FC,
            DQSLFK5FC,
            JHCJZFFC,
            HENRDJ2FC,
            HENRXJPSSC,
            HENROZ15C,
            HENRXG15C,
            HENRFFC,
            HENR2FC,
            HENR3FC,
            HENRTXFFC,
            LGZXFFC,
            LGZX3FC,
            LGZXFF11X5,
            LGZX3F11X5,
            LGZXPK10,
            WHEN30M,
            WHENBLS60M,
            WHENHS15F,
            WHENFS15F,
            WHENXDL15F,
            WHENNDJSSC,
            WHENELSSSC,
            WHENQQFFC,
            WHENFLP15C,
            WHENHGSSC,
            WHENDJSSC,
            WHENXJPSSC,
            WHEN11X5,
            JHC2FFC,
            JHC215C,
            JHC23FC,
            JHC2DPFFC,
            JHC2JZFFC,
            JHC2JD15C,
            JHC2PK10,
            HUIZFFC,
            HUIZ5FC,
            HUIZHGSSC,
            HUIZJN15C,
            HUIZXJPSSC,
            HUIZFF11X5,
            HUIZFFPK10,
            HDYLFFC,
            HDYL2FC,
            HDYL5FC,
            HDYLASKFFC,
            HDYLFF11X5,
            HDYL2F11X5,
            HDYL5F11X5,
            HDYLFFPK10,
            HDYLFFFT,
            ALGJFFC,
            ALGJ5FC,
            ALGJRB15C,
            ALGJSE15C,
            ALGJTXFFC,
            ALGJNTXFFC,
            ALGJOZBWC,
            ALGJFF11X5,
            KYHGSSC,
            KYBL15C,
            KYWX15C,
            KYFLBSSC,
            KYWBFFC,
            KYYNFFC,
            KYYN3FC,
            KYYN5FC,
            KY11X5FFC,
            KY11X53FC,
            GJFFC,
            GJ5FC,
            GJTX60M,
            GJOZBWC,
            GJFF11X5,
            CCTWSSCGB,
            CCHGSSCGB,
            CCDJSSCGB,
            CCTJ3FC,
            CCTJ5FC,
            CCTG60M,
            CCXG15C,
            CCFLP15C,
            CCRD2FC,
            CCWXFFC,
            CCTW11X5,
            CCAM11X5,
            CCXG11X5,
            CCFFPK10,
            CTXDLD30M,
            CTXXWYFFC,
            CTXXDLSSC,
            CTXHGSSC,
            CTXDJSSC,
            CTXXJPSSC,
            CTXLD2FC,
            CTXSE15F,
            CTXNY15C,
            CTXPK10,
            KXTXFFC,
            KXHGSSC,
            KXBL15C,
            KXWX15C,
            KXFLBSSC,
            KXWBFFC,
            KXYNFFC,
            KXYN3FC,
            KXYN5FC,
            KX11X5FFC,
            KX11X53FC,
            JCXQQFFC,
            JCXXXLSSC,
            JCXMG45M,
            JCXMJFFC,
            JCXNYFFC,
            JCXFLPFFC,
            JCXMG15C,
            JCXHN15C,
            JCXNY15C,
            JCXXDLSSC,
            JCXNHGSSC,
            JCXNDJSSC,
            JCXJN11X5,
            JCX60MPK10,
            ZLJMGFFC,
            ZLJSE15C,
            ZLJDJSSC,
            ZLJFLP2FC,
            ZLJELS5FC,
            ZLJFF11X5,
            ZLJFFPK10,
            LSWJSFFC,
            LSWJS5FC,
            LSWJSTXFFC,
            LSWJSHGSSC,
            LSWJSDJSSC,
            LSWJSMG15C,
            LSWJSMG45M,
            LSWJSOZFFC,
            LSWJSOZ35C,
            LSWJSFF11X5,
            LSWJSFFPK10,
            LSWJSOTXFFC,
            HCYLOZ3FC,
            HCYLSLFK5FC,
            SSHCFFC,
            SSHC3FC,
            SSHCFF11X5,
            SSHC3F11X5,
            SSHCPK10,
            WHEN120MPK10,
            WHEN180MPK10,
            XHSDFFC,
            XHSD5FC,
            XHSDTXFFC,
            XHSDFF11X5,
            TR45C,
            TRPK10,
            HYTXFFC,
            HCZX5FC,
            HCZXJNDSSC,
            HCZXDJSSC,
            HCZXFFC,
            HCZXMG45M,
            HCZXNTXFFC,
            HCZXFFPK10,
            HCZX3FPK10,
            BHGJFFC,
            BHGJ2FC,
            BHGJ5FC,
            BHGJXXLSSC,
            BHGJHGSSC,
            BHGJHN15C,
            BHGJHNFFC,
            BHGJPK10,
            QJCTXFFC,
            XDBFFC,
            XDBHGSSC,
            XDBDJSSC,
            XDB2FC,
            XDB3FC,
            XDB5FC,
            XDBFF11X5,
            XDB2F11X5,
            XDB3F11X5,
            XDB5F11X5,
            XDBFFPK10,
            XDB2FPK10,
            XDB3FPK10,
            XDB5FPK10,
            JFYLGG15C,
            DYFFC,
            DYHGSSC,
            DYDJSSC,
            DY2FC,
            DY3FC,
            DY5FC,
            DYFF11X5,
            DY2F11X5,
            DY3F11X5,
            DY5F11X5,
            DYFFPK10,
            DY2FPK10,
            DY3FPK10,
            DY5FPK10,
            HONDSE15F,
            HONDNY15C,
            HONDHG15F,
            HONDDJ15F,
            HONDFFC,
            HOND2FC,
            HOND5FC,
            HSAZXYC,
            QFYLFFC,
            QFYL3FC,
            QFYL5FC,
            QFYLDJSSC,
            QFYLSE15F,
            QFYLNY15C,
            QFYL11X5,
            TYYLHS15F,
            TYYL30M,
            TYYLMG60M,
            TYYLDJ15F,
            TYYLXJPSSC,
            TYYLXDL15F,
            TYYLELS15F,
            TYYLYD15F,
            TYYL30M11X5,
            TYYL90M11X5,
            TYYL120MPK10,
            TYYL180MPK10,
            AMBLRTXFFC,
            AMBLRAMSSC,
            AMBLRTWSSC,
            AMBLRQQFFC,
            AMBLRHN2FC,
            AMBLRHN5FC,
            AMBLRBX15F,
            AMBLRBXKLC,
            AMBLRAMPK10,
            AMBLRTWPK10,
            AMBLRAM11X5,
            AMBLRTW11X5,
            SLTHTEQ15C,
            JXYLFFC,
            JXYL2FC,
            JXYL3FC,
            JXYL5FC,
            JXYLFF11X5,
            SSHCTXFFC,
            VRSXFFC,
            VRMXSC,
            VRYYPK10,
            VRZXC11X5,
            YHYLTXFFC,
            YHYLQQFFC,
            YHYLWXFFC,
            YHYLFLPFFC,
            YHYLJNDFFC,
            YHYLBLSFFC,
            YHYLXHG15F,
            YHYLXDL15F,
            YHYLFLP2FC,
            YHYLFLP45M,
            YHYLXHG45M,
            YHYLBD45M,
            CYYLDLD30M,
            CYYLXWYFFC,
            CYYLXDLSSC,
            CYYLHGSSC,
            CYYLDJSSC,
            CYYLXJPSSC,
            CYYLLD2FC,
            CYYLSE15F,
            CYYLNY15C,
            BLGJFFC,
            BLGJ5FC,
            BLGJTXFFC,
            BLGJFLP45M,
            BLGJYN15C,
            BLGJXDL45M,
            BLGJXDL90M,
            BLGJNTXFFC,
            BLGJFF11X5,
            YBFFC,
            YB3FC,
            YBBLSFFC,
            YBTXFFC,
            YBHG15F,
            YBXDL15F,
            YBNHG15F,
            YBDJSSC,
            JYYLXDLSSC,
            JYYLXWYFFC,
            JYYLXJPSSC,
            JYYLLD2FC,
            JYYLHGSSC,
            JYYLDJSSC,
            WCFLPFFC,
            WCJPZFFC,
            WCDBFFC,
            WCMDJB15C,
            WCMGDZ2FC,
            WCMG45M,
            WCMLXY3FC,
            WCTWFFC,
            WCXBYFFC,
            WCXDL15C,
            WCYGFFC,
            WYFFC,
            WY2FC,
            WY3FC,
            WYFF11X5,
            WY2F11X5,
            WY3F11X5,
            WYFFPK10,
            WY2FPK10,
            WY3FPK10,
            XHHCOZ3FC,
            XHHCSLFK5FC,
            JXYLELSSSC,
            JXYLXXLSSC,
            NBAJN11X5,
            NBAFF11X5,
            NBA3F11X5,
            NBA5F11X5,
            NBAJZDPK10,
            NBA60MPK10,
            NBA180MPK10,
            NBABJSSC,
            NBATWSSC,
            NBANY5FC,
            NBANY3FC,
            NBANY2FC,
            NBAQQFFC,
            NBAHGSSC,
            NBADJSSC,
            NBAXDLSSC,
            NBANDJSSC,
            NBAFLP15C,
            JYYLTX30M,
            JYYLQQFFC,
            WHENYG60M,
            WEFFC,
            WE2FC,
            WE45C,
            WETXFFC,
            WEAM15C,
            WE11X5,
            WEPK10,
            MCQQFFC,
            MXYLXDLSSC,
            MXYLXWYFFC,
            MXYLXJPSSC,
            MXYLLD2FC,
            MXYLHGSSC,
            MXYLDJSSC,
            MXYLTX30M,
            MXYLQQFFC,
            NBAXXLSSC,
            WCAIXDLSSC,
            WCAIXWYFFC,
            WCAIXJPSSC,
            WCAILD2FC,
            WCAIHGSSC,
            WCAIDJSSC,
            WCAIMBE30M,
            WCAITWSSC,
            VRTXFFC,
            SKY2F11X5,
            QQYLFFC,
            QQYL5FC,
            QQYLFLP45M,
            QQYLRB45M,
            QQYLMG45M,
            QQYLJND4FC,
            QQYLHG5FC,
            QQYLQQXDL45M,
            QQYLQQXDL90M,
            QQYLFF11X5,
            YHSGXDLSSC,
            YHSGXWYFFC,
            YHSGXJPSSC,
            YHSGLD2FC,
            YHSGHGSSC,
            YHSGDJSSC,
            YINHTXFFC,
            YINHQQFFC,
            YINH2FC,
            YINH5FC,
            YINHHGSSC,
            YINHDJSSC,
            YRALFFC,
            XGLLWYN30M,
            XGLLDJSSC,
            XGLLLSJ2FC,
            XGLLFLP15C,
            XGLLWNS15C,
            XGLLLFFC,
            XGLLL2FC,
            XGLLLSY11X5,
            XGLLLDPK10,
            XGLLSSPK10,
            HENDJSPK10,
            HENDOMPK10,
            HENDFFPK10,
            HENDTXPK10,
            HENDJS11X5,
            HENDFFC,
            HENDXDLSSC,
            HEND2FC,
            HEND5FC,
            HENDTG30,
            HENDDJSSC,
            HENDHGSSC,
            HENDDX15C,
            HENDNY15C,
            HENDXXLSSC,
            HENDHG1FC,
            HENDHG2FC,
            HENDXJPSSC,
            DEJIFFC,
            DEJI5FC,
            DEJITXFFC,
            DEJIFLP45M,
            DEJIRB45M,
            DEJIMG45M,
            DEJIJND4FC,
            DEJIHG5FC,
            DEJIQQXDL45M,
            DEJIQQXDL90M,
            DEJIFF11X5,
            JLGJFFC,
            JLGJ5FC,
            JLGJTXFFC,
            JLGJFF11X5,
            XTYLXDLSSC,
            XTYLHGSSC,
            XTYLDLD30M,
            XTYLXWYFFC,
            XTYLDJSSC,
            XTYLXJPSSC,
            XTYLLD2FC,
            XTYLPK10,
            JXYLHG5FC,
            XWYLXDLSSC,
            XWYLXWYFFC,
            XWYLXJPSSC,
            XWYLFL30M,
            XWYLTX30M,
            XWYLQQFFC,
            XWYLLD2FC,
            XWYLHGSSC,
            XWYLDJSSC,
            XWYLSE15F,
            XWYLNY15C,
            B6YLFFC,
            B6YL3FC,
            B6YL5FC,
            B6YL3F11X5,
            TBYLQQFFC,
            TBYLMG15C,
            TBYLSE15C,
            TBYLNY15C,
            TBYLXDLSSC,
            TBYLNHGSSC,
            TBYLNDJSSC,
            TBYFLP15C,
            TBYLJND35C,
            TBYLJND5FC,
            TBYLJS11X5,
            WZYLJDSSC,
            WZYLTXFFC,
            WZYLHGSSC,
            WZYLHG1FC,
            WZYLDJSSC,
            WZYLXJPSSC,
            WZYLXGSSC,
            WZYLLSWJS15C,
            WZYLMG35C,
            WZYLBL1FC,
            WZYLMG15C,
            WZYLTG11X5,
            WZYLBL11X5,
            WZYLJSPK10,
            YZCPYN5FC,
            YZCPTG15C,
            YZCPJSFFC,
            YZCPRBSSC,
            YZCPHGSSC,
            YZCPXJPSSC,
            YZCPJDSSC,
            YZCPMG35C,
            YZCPLSWJS15C,
            YZCPBL1FC,
            YZCPMG15C,
            YZCPHG1FC,
            YZCPTG30M,
            YZCPTG11X5,
            YZCPXG11X5,
            YZCPJSPK10,
            TIYUML2FC,
            TIYUFLP15C,
            TIYUNHG15C,
            TIYUXDL15C,
            TIYUXJPSSC,
            YCYLFFC,
            YCYL5FC,
            YCYLTXFFC,
            YCYLFF11X5,
            ZBYLGGFFC,
            ZBYLTX2FC,
            ZBYLFFC,
            ZBYL2FC,
            ZBYL3FC,
            ZBYL5FC,
            ZBYLFF11X5,
            ZBYL2F11X5,
            ZBYL3F11X5,
            ZBYL5F11X5,
            FNYXFFC,
            FNYXHY11X5,
            HR45C,
            HUAYFFC,
            HUAY2FC,
            HUAY5FC,
            HUAY11X5,
            YXZXQQFFC,
            YXZXMG15C,
            YXZXSE15C,
            YXZXNY15C,
            YXZXXDLSSC,
            YXZXNHGSSC,
            YXZXNDJSSC,
            YXZXFLP15C,
            YXZXJND35C,
            YXZXJND5FC,
            YXZXTW11X5,
            YXZXTWPK10,
            WTYLTXFFC,
            WTYLXXL15C,
            WTYLHG15C,
            WTYLNY2FC,
            WTYLJNDSSC,
            WTYLXDL35C,
            WTYLAZ5FC,
            TCYLFFC,
            TCYL2FC,
            TCYLFF11X5,
            TCYL2F11X5,
            TCYLFFPK10,
            TCYL2FPK10,
            QFZXXDLSSC,
            QFZXXWYFFC,
            QFZXXJPSSC,
            QFZXFL30M,
            QFZXTX30M,
            QFZXQQFFC,
            QFZXLD2FC,
            QFZXHGSSC,
            QFZXDJSSC,
            QFZXSE15F,
            QFZXNY15C,
            QFZXPK10,
            ZBEI3FC,
            ZBEISLFK5FC,
            ZBEIXJP2FC,
            ZBEIFFC,
            ZBEIHGSSC,
            ZBEIDJSSC,
            ZBEI30M,
            ZBEI45M,
            ZBEIQQFFC,
            ZBEIWXFFC,
            ZBEIGGFFC,
            ZBEIXJP15C,
            ZBEIXDL15C,
            ZBEIJNDSSC,
            ZBEIFF11X5,
            ZBEI2F11X5,
            ZBEI3F11X5,
            ZBEI5F11X5,
            ZBEIFFPK10,
            ZBEI2FPK10,
            ZBEI3FPK10,
            ZBEI5FPK10,
            JXINFFC,
            JXIN2FC,
            JXIN5FC,
            JXINFLP15C,
            JXINHGSSC,
            JXINMSK15C,
            JXINJLP15C,
            JXINDB15C,
            JXINDJSSC,
            JXINXJPSSC,
            ZBYZCP15C,
            XQYLFFC,
            XQYL3FC,
            XQYL5FC,
            XQYLHGSSC,
            XQYLDJSSC,
            XQYLSE15F,
            XQYLNY15C,
            XQYLQQFFC,
            XQYLBDFFC,
            XQYLWBFFC,
            XQYL11X5,
            XQYLJS11X5,
            XQYLJSPK10,
            YXZXXXLSSC,
            YXZXHN45M,
            YXZXMG45M
        }

        public class LSData
        {
            public bool BJExpectSelect;
            public int BJExpectValue;
            public DateTime Date;
            public string FNName;
            public bool IsBJ;
            public string LotteryID;
            public string LotteryName;
            public int LSBJTypeIndex;
            public bool RefreshControl;
            public ConfigurationStatus.Scheme SchemeInfo;
            public List<ConfigurationStatus.LSDataView> ViewList = new List<ConfigurationStatus.LSDataView>();
        }

        public class LSDataView
        {
            public bool IsBJ = false;
            public string LGExpect = "";
            public string LZExpect = "";
            public string PerExpect = "";
            public string PerLZExpect = "";
            public string TodayExpect = "";
            public string Value = "";
            public string WeekExpect = "";
            public string YesterdayExpect = "";
            public string ZuKey = "";

            public static int GetNum(string pStr) => 
                ((pStr != "") ? Convert.ToInt32(pStr) : 0);
        }

        public enum ManageType
        {
            User,
            DL,
            PT
        }

        public class MoreAppData
        {
            public string File;
            public string Name;
            public string Remark;

            public string ViewName
            {
                get
                {
                    string expression = this.Name.Replace("软件", "");
                    if (expression.Contains("】"))
                    {
                        expression = Strings.Split(expression, "】", -1, CompareMethod.Binary)[1];
                    }
                    return expression;
                }
            }
        }

        public class OpenData
        {
            public string Code;
            public List<string> CodeList;
            public string Expect;
            public DateTime Time;
        }

        public class PlanScheme
        {
            public string Name;
            public string Path;

            public PlanScheme()
            {
                this.Path = "";
                this.Name = "";
            }

            public PlanScheme(string pPlan, string pName)
            {
                this.Path = "";
                this.Name = "";
                this.Path = pPlan;
                this.Name = pName;
            }

            public string File =>
                (this.Path + @"\" + this.Name + ".txt");
        }

        public class PlayBase
        {
            public int CodeCount;
            public List<int> IndexList;
            public string Number;
            public string PlayName;
            public string PlayType;

            public List<List<int>> ConvertRXDSWZList(List<int> pRXWZ)
            {
                List<List<int>> list = null;
                if (pRXWZ != null)
                {
                    if (!CommFunc.CheckPlayIsRXDS(this.Play))
                    {
                        return list;
                    }
                    list = new List<List<int>>();
                    List<string> list2 = CommFunc.ConvertIntToStrList(pRXWZ);
                    if (list2.Count < this.IndexList.Count)
                    {
                        return null;
                    }
                    list2 = CommFunc.GetCombinations(this.ConvertRXWZList(list2), this.IndexList.Count, "");
                    foreach (string str in list2)
                    {
                        List<int> item = CommFunc.ConvertSameListInt(str);
                        list.Add(item);
                    }
                }
                return list;
            }

            public List<List<string>> ConvertRXFSCodeList(List<string> pCodeList)
            {
                List<List<string>> list = null;
                if (CommFunc.CheckPlayIsFS(this.Play))
                {
                    list = new List<List<string>>();
                    if (pCodeList.Count < this.IndexList.Count)
                    {
                        return null;
                    }
                    List<string> list2 = CommFunc.GetCombinations(pCodeList, this.IndexList.Count, "-");
                    foreach (string str in list2)
                    {
                        List<string> item = CommFunc.SplitString(str, "-", -1);
                        list.Add(item);
                    }
                }
                return list;
            }

            public List<List<int>> ConvertRXFSWZList(List<string> pCodeList)
            {
                List<List<int>> list = null;
                if (CommFunc.CheckPlayIsRXFS(this.Play))
                {
                    list = new List<List<int>>();
                    List<string> itemList = new List<string>();
                    for (int i = 0; i < pCodeList.Count; i++)
                    {
                        if (pCodeList[i] != "*")
                        {
                            itemList.Add((i + 1).ToString());
                        }
                    }
                    if (itemList.Count < this.IndexList.Count)
                    {
                        return null;
                    }
                    itemList = CommFunc.GetCombinations(itemList, this.IndexList.Count, "");
                    foreach (string str in itemList)
                    {
                        List<int> item = CommFunc.ConvertSameListInt(str);
                        list.Add(item);
                    }
                }
                return list;
            }

            public List<int> ConvertRXWZList(List<int> pRXWZ)
            {
                List<int> list = new List<int>();
                for (int i = 0; i < pRXWZ.Count; i++)
                {
                    list.Add(pRXWZ[i] + 1);
                }
                return list;
            }

            public List<string> ConvertRXWZList(List<string> pRXWZ)
            {
                List<string> list = new List<string>();
                for (int i = 0; i < pRXWZ.Count; i++)
                {
                    list.Add((Convert.ToInt32(pRXWZ[i]) + 1).ToString());
                }
                return list;
            }

            public List<string> GetRXFSCodeList(List<string> pCodeList)
            {
                List<string> list = new List<string>();
                foreach (string str in pCodeList)
                {
                    if (str != "*")
                    {
                        list.Add(str);
                    }
                }
                return list;
            }

            public string IndexNumString
            {
                get
                {
                    List<int> pList = new List<int>();
                    foreach (int num in this.IndexList)
                    {
                        pList.Add(num - 1);
                    }
                    return CommFunc.Join(pList, ",");
                }
            }

            public string IndexString
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (int num in this.IndexList)
                    {
                        pList.Add(AppInfo.IndexDic[num - 1]);
                    }
                    return CommFunc.Join(pList, ",");
                }
            }

            public string Play =>
                (this.PlayType + this.PlayName);

            public string PlayChar =>
                CommFunc.GetPlayChar(this.Play);
        }

        public delegate void PTIndexMainDelegate(string pName = "");

        public class PTLine
        {
            public string ID = "";
            public List<string> LineList = new List<string>();
            public string Name = "";
            public string Tag = "";

            public PTLine(string pValue)
            {
                string[] strArray = pValue.Split(new char[] { ' ' });
                this.ID = strArray[0];
                this.Name = strArray[1];
            }
        }

        public class PTOpenData
        {
            public List<string> LoginLineList = new List<string>();
            public List<string> LotteryIDList = new List<string>();
            public string Name;
        }

        public class PTPrizeGroup
        {
            public string GroupID;
            public string GroupName;
            public string PlayCode;
            public string PlayEnName;
            public string PlayID;
            public string PlayName;
        }

        public delegate void RefreshListDelegate(List<string> pDataList, ConfigurationStatus.LotteryType pLottery, int pDataCount);

        public delegate void RefreshLoginHintDelegate();

        public delegate void RefreshLSDataDelegate(object pInfo);

        public delegate void RefreshLSDataLaterDelegate();

        public delegate void RefreshTJDataDelegate(object pInfo);

        public delegate void RefreshTJDataLaterDelegate();

        public delegate void RefreshUserMainDelegate(bool RefreshData);

        public delegate void RemoveLoginLockDelegate(string pError, string pName);

        public class SCAccountData
        {
            public string ActiveTime;
            public bool AllowClear;
            public bool AllowCZ;
            public bool AllowDelete;
            public bool AllowDK;
            public string AppName;
            public string AppViewName;
            public double BankBalance;
            public ConfigurationStatus.AppConfiguration Configuration = new ConfigurationStatus.AppConfiguration();
            public string ConfigurationString;
            public int CZDSDay = 0;
            public string GGImageString;
            public string ID;
            public string IPVerifyCode;
            public bool IsCZDS = false;
            public bool IsCZYS = false;
            public bool IsStop;
            public string LastIP;
            public string LastTime;
            public bool LoginStatus;
            public string MachineCode;
            public int MaxLimit;
            public int MinLimit;
            public string OnLineTime;
            public string Phone;
            public string PTID;
            public string PTLoginAudit;
            public string PTLoginHint;
            public string PTName;
            public string PTPW;
            public string PTUser;
            public string PW;
            public string QQ;
            public string RandomNum;
            public string Remark;
            public Dictionary<string, ConfigurationStatus.Scheme> SchemeDic = new Dictionary<string, ConfigurationStatus.Scheme>();
            public string State;
            public string StateString = "";
            public ConfigurationStatus.SCAccountType Type;

            public SCAccountData()
            {
                this.LoadDefultData(true);
            }

            public void AnalysisState(string pState)
            {
                string pStr = pState;
                if (pStr != "")
                {
                    List<string> list = CommFunc.SplitString(pStr, "|", -1);
                    string str2 = list[0];
                    if (str2 != this.ID)
                    {
                        pStr = "状态名称不正确";
                    }
                    else
                    {
                        string str3 = list[2];
                        string str4 = list[1];
                        if (str3 == "0")
                        {
                            pStr = $"已付款【付款时间：{str4}】";
                            this.IsCZYS = true;
                        }
                        else
                        {
                            pStr = $"待收款【{str3}天】【充值时间：{str4}】";
                            this.IsCZDS = true;
                            this.CZDSDay = (str3 == "-1") ? 0 : Convert.ToInt32(str3);
                        }
                    }
                }
                this.StateString = pStr;
            }

            public void LoadDefultData(bool pAll)
            {
                this.PTID = "";
                this.PTPW = "";
                this.PTName = "";
                this.PTLoginHint = "";
                this.BankBalance = 0.0;
                this.LoginStatus = false;
                if (pAll)
                {
                    this.ID = "";
                    this.PW = "";
                    this.MachineCode = "";
                    this.ActiveTime = "";
                    this.LastTime = "";
                    this.LastIP = "";
                    this.QQ = "";
                    this.Phone = "";
                    this.Type = ConfigurationStatus.SCAccountType.Unknown;
                    this.AppName = "YXZXGJ";
                    this.PTLoginAudit = "审核中...";
                }
            }

            public string ActiveTimeString
            {
                get
                {
                    if (this.ActiveTime != "9999-12-31")
                    {
                        return this.ActiveTime;
                    }
                    return "永久版";
                }
            }

            public string AppCharName =>
                (this.AppName + "-");

            public string AppPerName =>
                this.AppName.Split(new char[] { '-' })[0];

            public Image GGImage
            {
                get
                {
                    Image image = null;
                    if ((this.GGImageString != null) && (this.GGImageString != ""))
                    {
                        byte[] buffer = Convert.FromBase64String(this.GGImageString);
                        if (buffer.Length > 0)
                        {
                            MemoryStream stream = new MemoryStream(buffer, true);
                            stream.Write(buffer, 0, buffer.Length);
                            image = new Bitmap(stream);
                        }
                    }
                    return image;
                }
            }

            public bool IsAdmin =>
                !this.AppName.Contains("-");

            public bool IsDL =>
                this.AppName.Contains("-");

            public bool IsOnLineTime
            {
                get
                {
                    bool flag = false;
                    if ((this.OnLineTime != null) && (this.OnLineTime != ""))
                    {
                        DateTime time = DateTime.Parse(this.OnLineTime);
                        TimeSpan span = (TimeSpan) (DateTime.Now - time);
                        if (span.TotalSeconds <= 180.0)
                        {
                            flag = true;
                        }
                    }
                    return flag;
                }
            }

            public string PTLoginHintString
            {
                get
                {
                    string pTLoginHint = this.PTLoginHint;
                    if (pTLoginHint == "")
                    {
                        if (this.PTLoginAudit == "审核中...")
                        {
                            return $"会员【{this.PTID}】未授权，请联系客服授权使用，以免被踢下线！";
                        }
                        if (this.PTLoginAudit == "审核通过")
                        {
                            return $"会员【{this.PTID}】已授权，祝君中奖！";
                        }
                        if (this.PTLoginAudit == "审核未过")
                        {
                            pTLoginHint = $"会员【{this.PTID}】审核未通过，将强制下线！";
                        }
                    }
                    return pTLoginHint;
                }
            }

            public Dictionary<string, string> PTUserList
            {
                get
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    if ((this.PTUser != null) && (this.PTUser != ""))
                    {
                        List<string> list = CommFunc.SplitString(this.PTUser, ";", -1);
                        foreach (string str in list)
                        {
                            string[] strArray = str.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                string str2 = strArray[0];
                                if (str2 == "BHZY1")
                                {
                                    str2 = "BHZY";
                                }
                                dictionary[str2] = strArray[1];
                            }
                        }
                    }
                    return dictionary;
                }
            }

            public string SendUserID
            {
                get
                {
                    if (AppInfo.IsViewLogin)
                    {
                        return this.ID;
                    }
                    return this.PTID;
                }
            }

            public string TypeString
            {
                get
                {
                    string str = "";
                    if (this.Type == ConfigurationStatus.SCAccountType.FULL)
                    {
                        str = "完整版";
                    }
                    return str;
                }
                set
                {
                    string str = value;
                    if ((str != null) && (str == "完整版"))
                    {
                        this.Type = ConfigurationStatus.SCAccountType.FULL;
                    }
                }
            }
        }

        public enum SCAccountType
        {
            FULL,
            Unknown
        }

        public class Scheme
        {
            public string CHType;
            public ConfigurationStatus.FNBase FNBaseInfo = null;
            public ConfigurationStatus.SchemeCHType FNCHType;
            public ConfigurationStatus.FNDMLH FNDMLHInfo = null;
            public ConfigurationStatus.FNGDQM FNGDQMInfo = null;
            public ConfigurationStatus.FNGJDMLH FNGJDMLHInfo = null;
            public ConfigurationStatus.FNGJKMTM FNGJKMTMInfo = null;
            public ConfigurationStatus.FNKMTM FNKMTMInfo = null;
            public ConfigurationStatus.FNLHKMTM FNLHKMTMInfo = null;
            public ConfigurationStatus.FNLRWCH FNLRWCHInfo = null;
            public ConfigurationStatus.FNSJCH FNSJCHInfo = null;
            public ConfigurationStatus.FNWJJH FNWJJHInfo = null;
            public ConfigurationStatus.FNYLCH FNYLCHInfo = null;
            public bool IsFNEncrypt = false;
            public bool IsInputPW = false;
            public string Name;
            public bool Selected;

            public Scheme(bool pSelected, string pName, string pCHType, string pValue = "", bool pIsFNEncrypt = false, bool pIsInputPW = false)
            {
                this.Selected = pSelected;
                this.Name = pName;
                this.CHType = pCHType;
                this.IsFNEncrypt = pIsFNEncrypt;
                this.IsInputPW = pIsInputPW;
                if (pCHType == "定码轮换")
                {
                    this.FNDMLHInfo = new ConfigurationStatus.FNDMLH(pValue);
                    this.FNBaseInfo = this.FNDMLHInfo;
                    this.FNCHType = ConfigurationStatus.SchemeCHType.DMLH;
                }
                else if (pCHType == "高级定码轮换")
                {
                    this.FNGJDMLHInfo = new ConfigurationStatus.FNGJDMLH(pValue);
                    this.FNBaseInfo = this.FNGJDMLHInfo;
                    this.FNCHType = ConfigurationStatus.SchemeCHType.GJDMLH;
                }
                else if (pCHType == "高级开某投某")
                {
                    this.FNGJKMTMInfo = new ConfigurationStatus.FNGJKMTM(pValue);
                    this.FNBaseInfo = this.FNGJKMTMInfo;
                    this.FNCHType = ConfigurationStatus.SchemeCHType.GJKMTM;
                }
                else if (pCHType == "冷热温出号")
                {
                    this.FNLRWCHInfo = new ConfigurationStatus.FNLRWCH(pValue);
                    this.FNBaseInfo = this.FNLRWCHInfo;
                    this.FNCHType = ConfigurationStatus.SchemeCHType.LRWCH;
                }
                else if (pCHType == "外接计划")
                {
                    this.FNWJJHInfo = new ConfigurationStatus.FNWJJH(pValue);
                    this.FNBaseInfo = this.FNWJJHInfo;
                    this.FNCHType = ConfigurationStatus.SchemeCHType.WJJH;
                }
                else if (pCHType == "随机出号")
                {
                    this.FNSJCHInfo = new ConfigurationStatus.FNSJCH(pValue);
                    this.FNBaseInfo = this.FNSJCHInfo;
                    this.FNCHType = ConfigurationStatus.SchemeCHType.SJCH;
                }
                else if (pCHType == "遗漏出号")
                {
                    this.FNYLCHInfo = new ConfigurationStatus.FNYLCH(pValue);
                    this.FNBaseInfo = this.FNYLCHInfo;
                    this.FNCHType = ConfigurationStatus.SchemeCHType.YLCH;
                }
                else if (pCHType == "开某投某")
                {
                    this.FNKMTMInfo = new ConfigurationStatus.FNKMTM(pValue);
                    this.FNBaseInfo = this.FNKMTMInfo;
                    this.FNCHType = ConfigurationStatus.SchemeCHType.KMTM;
                }
                else if (pCHType == "固定取码")
                {
                    this.FNGDQMInfo = new ConfigurationStatus.FNGDQM(pValue);
                    this.FNBaseInfo = this.FNGDQMInfo;
                    this.FNCHType = ConfigurationStatus.SchemeCHType.GDQM;
                }
                else if (pCHType == "龙虎开某投某")
                {
                    this.FNLHKMTMInfo = new ConfigurationStatus.FNLHKMTM(pValue);
                    this.FNBaseInfo = this.FNLHKMTMInfo;
                    this.FNCHType = ConfigurationStatus.SchemeCHType.LHKMTM;
                }
            }

            public string GetFileValue()
            {
                List<string> pList = new List<string> {
                    this.Selected.ToString(),
                    this.CHType
                };
                string item = this.Value;
                pList.Add(item);
                string pSource = CommFunc.Join(pList, "\r\n");
                if (AppInfo.Account.Configuration.IsFNEncryptID || this.IsFNEncrypt)
                {
                    pSource = CommFunc.Encode(pSource, AppInfo.Account.Configuration.FNEncrypt);
                }
                return pSource;
            }

            public ConfigurationStatus.Scheme GetNewScheme(string pName)
            {
                ConfigurationStatus.Scheme scheme = new ConfigurationStatus.Scheme(false, pName, this.CHType, this.Value, this.IsFNEncrypt, this.IsInputPW);
                if (scheme.FNCHType == ConfigurationStatus.SchemeCHType.GJDMLH)
                {
                    ConfigurationStatus.FNGJDMLH fNBaseInfo = (ConfigurationStatus.FNGJDMLH) scheme.FNBaseInfo;
                    if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
                    {
                        string random = fNBaseInfo.GetRandom();
                        fNBaseInfo.DMLHValue = $"1|{random}|1|1";
                    }
                }
                return scheme;
            }

            public static ConfigurationStatus.Scheme GetSchemeByFileValue(string pFNValue, string pName)
            {
                bool pIsFNEncrypt = false;
                if (AppInfo.Account.Configuration.IsFNEncrypt)
                {
                    string str = CommFunc.Decode(pFNValue, AppInfo.Account.Configuration.FNEncrypt);
                    if (str != "")
                    {
                        pFNValue = str;
                        pIsFNEncrypt = true;
                    }
                }
                List<string> pList = CommFunc.SplitString(pFNValue, "\r\n", -1);
                if (pList.Count <= 3)
                {
                    return null;
                }
                bool pSelected = Convert.ToBoolean(pList[0]);
                string pCHType = pList[1];
                string pValue = CommFunc.Join(pList, "\r\n", 2);
                ConfigurationStatus.Scheme scheme = new ConfigurationStatus.Scheme(pSelected, pName, pCHType, pValue, pIsFNEncrypt, false);
                string appName = scheme.FNBaseInfo.AppName;
                if (((appName != null) && (appName != "")) && (appName != "YXZXGJ"))
                {
                    return null;
                }
                return scheme;
            }

            public string GetKey =>
                (this.CHType + "-" + this.Value);

            public bool IsViewFNEncrypt =>
                ((this.IsFNEncrypt && !AppInfo.Account.Configuration.IsFNEncryptID) && !this.IsInputPW);

            public string Value
            {
                get => 
                    this.FNBaseInfo.InfoToString();
                set
                {
                    this.FNBaseInfo.StringToInfo(value);
                }
            }

            public string ViewName
            {
                get
                {
                    string name = this.Name;
                    if (this.IsViewFNEncrypt)
                    {
                        name = name + "【密】";
                    }
                    return name;
                }
            }
        }

        public enum SchemeCHType
        {
            DMLH,
            GJDMLH,
            LRWCH,
            WJJH,
            SJCH,
            GJKMTM,
            YLCH,
            KMTM,
            GDQM,
            LHKMTM
        }

        public enum SchemeMode
        {
            QQHH,
            GHHH,
            ZHHH,
            LGNHH,
            LZNHH,
            JGNHH,
            JZNHH,
            CBHH,
            QQG
        }

        public class SCPlan
        {
            public Dictionary<string, int> AutoTimesDic = new Dictionary<string, int>();
            public double AutoTotalMode = 0.0;
            public double AutoWinCount = 0.0;
            public string BeginExpect;
            public string BTFNName;
            public string Code;
            public int CurrentCycle;
            public string CurrentExpectTime;
            public int Cycle;
            public string EndExpect;
            public Dictionary<string, ConfigurationStatus.FBType> FBDic = new Dictionary<string, ConfigurationStatus.FBType>();
            public Dictionary<string, double> FNAutoGainDic = new Dictionary<string, double>();
            public Dictionary<string, string> FNAutoStateDic = new Dictionary<string, string>();
            public string FNCHType;
            public Dictionary<string, double> FNGainDic = new Dictionary<string, double>();
            public int FNLG = 0;
            public int FNLGMax = 0;
            public int FNLZ = 0;
            public int FNLZMax = 0;
            public Dictionary<string, double> FNMNGainDic = new Dictionary<string, double>();
            public Dictionary<string, double> FNMNMoneyDic = new Dictionary<string, double>();
            public Dictionary<string, double> FNMoneyDic = new Dictionary<string, double>();
            public string FNName;
            public double Gain;
            public double GJTimes;
            public int Index;
            public bool IsMNBets = false;
            public bool IsNoOpen = false;
            public bool IsNull = false;
            public bool IsTXFFCCH = false;
            public bool IsValue1 = true;
            public bool IsWait = false;
            public bool IsWin = false;
            public string LotteryID;
            public string LotteryName;
            public int MaxExpect;
            public int MaxLimit;
            public double Mode;
            public double Money;
            public string NoOpenString = "未开";
            public string NoString = "挂";
            public int Number;
            public List<string> NumberList = new List<string>();
            public double Output;
            public double PHL = 0.0;
            public string PlanSchemeName;
            public string PlayName;
            public string PlayType;
            public double Rebate;
            public List<int> RXWZ;
            public string RXZJ;
            public ConfigurationStatus.BetsScheme SchemeInfo = null;
            public string State;
            public Dictionary<string, List<double>> TimesDic = new Dictionary<string, List<double>>();
            public ConfigurationStatus.SCTimesType TimesType;
            public Dictionary<string, int> TJBZCountDic = new Dictionary<string, int>();
            public double TotalMoney;
            public string TXFFCCHString = "重号";
            public int Unit;
            public ConfigurationStatus.SCUnitType UnitType;
            public DateTime UploadTime;
            public string WaitString = "待开";
            public string YesString = "中";
            public string ZuKey;
            public Dictionary<string, List<string>> ZuKeyDic;

            public double AutoMoney(string pZuKey = "", bool pIsConvert = true) => 
                (this.Money * this.AutoTimes(pZuKey, pIsConvert));

            public double AutoTimes(string pZuKey = "", bool pIsConvert = true)
            {
                double num = -1.0;
                if (pZuKey == "")
                {
                    if (this.ZuKeyDic != null)
                    {
                        pZuKey = CommFunc.GetDicKeyList<List<string>>(this.ZuKeyDic)[0];
                    }
                    else if ((this.FNNumberDic != null) && (this.FNNumberDic.Count > 0))
                    {
                        pZuKey = CommFunc.GetDicKeyList<Dictionary<string, List<string>>>(this.FNNumberDic)[0];
                    }
                }
                string dicKeyByZuKey = this.GetDicKeyByZuKey(pZuKey);
                List<double> list = this.TimesDic[dicKeyByZuKey];
                if (list.Count == 0)
                {
                    return 0.0;
                }
                int num2 = this.BTIndex(pZuKey) % list.Count;
                num = list[num2];
                if ((num == -1.0) && pIsConvert)
                {
                    num = 0.0;
                }
                return num;
            }

            public double AutoTotalMoney(string pZuKey = "", bool pIsConvert = true)
            {
                int num = this.FNNumber(pZuKey);
                return ((this.Money * num) * this.AutoTimes(pZuKey, pIsConvert));
            }

            public int BTIndex(string pZuKey = "") => 
                (((pZuKey != null) && this.AutoTimesDic.ContainsKey(pZuKey)) ? this.AutoTimesDic[pZuKey] : 0);

            public bool CheckPlanIsWait() => 
                this.State.Contains(this.WaitString);

            public bool CheckPlanStringIsNoOpen() => 
                this.State.Contains(this.NoOpenString);

            public bool CheckPlanStringIsTXFFCCH() => 
                this.State.Contains(this.TXFFCCHString);

            public bool CheckPlanStringIsWIn() => 
                this.State.Contains(this.YesString);

            public bool FNAutoIsError(string pZuKey) => 
                (this.FNAutoStateDic[pZuKey] == this.NoString);

            public int FNNumber(string pZuKey)
            {
                int number = this.Number;
                try
                {
                    string perZuKey = ConfigurationStatus.FNBase.GetPerZuKey(pZuKey);
                    if (this.FNNumberDic.ContainsKey(perZuKey))
                    {
                        List<string> pNumberList = this.FNNumberDic[perZuKey][pZuKey];
                        number = CommFunc.GetBetsCodeCount(this.GetPTNumberList(pNumberList), this.Play, this.RXWZ);
                    }
                }
                catch
                {
                }
                return number;
            }

            private string GetDicKeyByZuKey(string pZuKey) => 
                "0";

            public ConfigurationStatus.FBType GetFBType(string pZuKey)
            {
                string dicKeyByZuKey = this.GetDicKeyByZuKey(pZuKey);
                return this.FBDic[dicKeyByZuKey];
            }

            public List<string> GetPTNumberList(List<string> pNumberList = null)
            {
                List<string> list = new List<string>();
                if (pNumberList == null)
                {
                    pNumberList = this.NumberList;
                }
                foreach (string str in pNumberList)
                {
                    string item = CommFunc.SplitString(str, "|", -1)[0];
                    list.Add(item);
                }
                return list;
            }

            public List<double> GetTimes(string pZuKey)
            {
                List<double> list = new List<double>();
                string dicKeyByZuKey = this.GetDicKeyByZuKey(pZuKey);
                return this.TimesDic[dicKeyByZuKey];
            }

            public double GetTotalFNGain(bool pIsMN)
            {
                Dictionary<string, double> dictionary = pIsMN ? this.FNMNGainDic : this.FNGainDic;
                double num = 0.0;
                foreach (string str in dictionary.Keys)
                {
                    num += dictionary[str];
                }
                return num;
            }

            public double GetTotalFNMoney(bool pIsMN)
            {
                Dictionary<string, double> dictionary = pIsMN ? this.FNMNMoneyDic : this.FNMoneyDic;
                double num = 0.0;
                foreach (string str in dictionary.Keys)
                {
                    num += dictionary[str];
                }
                return num;
            }

            public bool IsMNState(string pZuKey = "", bool pIsConvert = true)
            {
                if (AppInfo.Account.Configuration.IsHideMNBets)
                {
                    return false;
                }
                if (this.CheckPlanStringIsTXFFCCH())
                {
                    return false;
                }
                return (this.IsMNBets || (this.AutoMoney(pZuKey, pIsConvert) == 0.0));
            }

            public string NumberString(List<string> pNumberList = null)
            {
                if (pNumberList == null)
                {
                    pNumberList = this.NumberList;
                }
                List<string> list = new List<string>();
                foreach (string str in pNumberList)
                {
                    string item = CommFunc.SplitString(str, "|", -1)[0];
                    list.Add(item);
                }
                return CommFunc.CombinaBetsCode(list, this.Play);
            }

            public void ReplaceNumberListMode()
            {
                for (int i = 0; i < this.NumberList.Count; i++)
                {
                    string pStr = this.NumberList[i];
                    List<string> pList = CommFunc.SplitString(pStr, "|", -1);
                    pList[1] = this.Mode.ToString();
                    this.NumberList[i] = CommFunc.Join(pList, "|");
                }
            }

            public string AlreadLottery
            {
                get
                {
                    if (this.IsNoOpen)
                    {
                        return this.NoOpenString;
                    }
                    if (this.IsTXFFCCH)
                    {
                        return this.TXFFCCHString;
                    }
                    if (this.IsWin)
                    {
                        return this.YesString;
                    }
                    return this.NoString;
                }
            }

            public string AutoTimesString
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str in this.AutoTimesDic.Keys)
                    {
                        pList.Add(str + "|" + this.AutoTimesDic[str]);
                    }
                    return CommFunc.Join(pList, ";");
                }
                set
                {
                    List<string> list = CommFunc.SplitString(value, ";", -1);
                    this.AutoTimesDic = new Dictionary<string, int>();
                    foreach (string str in list)
                    {
                        this.AutoTimesDic[str.Split(new char[] { '|' })[0]] = Convert.ToInt32(str.Split(new char[] { '|' })[1]);
                    }
                }
            }

            public string BetsKey
            {
                get
                {
                    List<string> pList = new List<string> {
                        this.LotteryID,
                        this.CurrentExpect,
                        this.FNName,
                        this.FNCHType,
                        this.NumberString(null)
                    };
                    return CommFunc.Join(pList, ";");
                }
            }

            public string BetsState
            {
                get
                {
                    string str = "";
                    if (this.IsMNState("", true))
                    {
                        str = "模拟";
                    }
                    return str;
                }
            }

            public string CurrentExpect =>
                this.BeginExpect;

            public string CurrentExpectPart1
            {
                get
                {
                    string str = this.CurrentExpect.Substring(0, 6);
                    DateTime time = new DateTime(Convert.ToInt32("20" + str.Substring(0, 2)), Convert.ToInt32(str.Substring(2, 2)), Convert.ToInt32(str.Substring(4, 2)));
                    return time.ToString("yyyy-MM-dd");
                }
            }

            public string CurrentExpectPart2
            {
                get
                {
                    int lotteryExpectLen = CommFunc.GetLotteryExpectLen(this.LotteryName, this.LotteryID);
                    return Convert.ToInt32(Strings.Right(this.CurrentExpect, lotteryExpectLen)).ToString();
                }
            }

            public string FBListString
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.FBDic.Keys)
                    {
                        string item = str2 + "-" + Convert.ToInt32(this.FBDic[str2]);
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "|");
                }
                set
                {
                    List<string> list = CommFunc.SplitString(value, "|", -1);
                    this.FBDic.Clear();
                    foreach (string str in list)
                    {
                        string str2 = str.Split(new char[] { '-' })[0];
                        ConfigurationStatus.FBType type = (ConfigurationStatus.FBType) Convert.ToInt32(str.Split(new char[] { '-' })[1]);
                        this.FBDic[str2] = type;
                    }
                }
            }

            public string FNAutoGain
            {
                get
                {
                    if (this.FNAutoGainDic.Count == 0)
                    {
                        return "0";
                    }
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.FNAutoGainDic.Keys)
                    {
                        string item = CommFunc.TwoDouble(this.FNAutoGainDic[str2], true);
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "\r\n");
                }
            }

            public string FNAutoMoney
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.ZuKeyDic.Keys)
                    {
                        string item = CommFunc.TwoDouble(this.AutoTotalMoney(str2, true), true);
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "\r\n");
                }
            }

            public string FNAutoNumbe
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.ZuKeyDic.Keys)
                    {
                        string item = this.FNNumber(str2).ToString();
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "\r\n");
                }
            }

            public string FNAutoState =>
                CommFunc.Join(CommFunc.GetDicValueList<string>(this.FNAutoStateDic), "\r\n");

            public string FNAutoTimes
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.ZuKeyDic.Keys)
                    {
                        string item = CommFunc.TwoDouble(this.AutoTimes(str2, true), false);
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "\r\n");
                }
            }

            public double FNAutoTotalMoney
            {
                get
                {
                    double num = 0.0;
                    foreach (string str in this.ZuKeyDic.Keys)
                    {
                        double num2 = this.AutoTotalMoney(str, true);
                        num += num2;
                    }
                    return num;
                }
            }

            public string FNBTIndex
            {
                get
                {
                    List<int> pList = new List<int>();
                    foreach (string str2 in this.ZuKeyDic.Keys)
                    {
                        int item = this.BTIndex(str2) + 1;
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "\r\n");
                }
            }

            public string FNGainString
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.FNGainDic.Keys)
                    {
                        string item = str2 + "-" + this.FNGainDic[str2];
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "|");
                }
                set
                {
                    List<string> list = CommFunc.SplitString(value, "|", -1);
                    this.FNGainDic.Clear();
                    foreach (string str in list)
                    {
                        string str2 = str.Split(new char[] { '-' })[0];
                        double num = Convert.ToDouble(str.Substring(str.IndexOf('-') + 1));
                        this.FNGainDic[str2] = num;
                    }
                }
            }

            public string FNLGString =>
                $"{this.FNLG}/{this.FNLGMax}";

            public string FNLZString =>
                $"{this.FNLZ}/{this.FNLZMax}";

            public string FNMNGainString
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.FNMNGainDic.Keys)
                    {
                        string item = str2 + "-" + this.FNMNGainDic[str2];
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "|");
                }
                set
                {
                    List<string> list = CommFunc.SplitString(value, "|", -1);
                    this.FNMNGainDic.Clear();
                    foreach (string str in list)
                    {
                        string str2 = str.Split(new char[] { '-' })[0];
                        double num = Convert.ToDouble(str.Substring(str.IndexOf('-') + 1));
                        this.FNMNGainDic[str2] = num;
                    }
                }
            }

            public string FNMNMoneyString
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.FNMNMoneyDic.Keys)
                    {
                        string item = str2 + "-" + this.FNMNMoneyDic[str2];
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "|");
                }
                set
                {
                    List<string> list = CommFunc.SplitString(value, "|", -1);
                    this.FNMNMoneyDic.Clear();
                    foreach (string str in list)
                    {
                        string str2 = str.Split(new char[] { '-' })[0];
                        double num = Convert.ToDouble(str.Substring(str.IndexOf('-') + 1));
                        this.FNMNMoneyDic[str2] = num;
                    }
                }
            }

            public string FNMoneyString
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.FNMoneyDic.Keys)
                    {
                        string item = str2 + "-" + this.FNMoneyDic[str2];
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "|");
                }
                set
                {
                    List<string> list = CommFunc.SplitString(value, "|", -1);
                    this.FNMoneyDic.Clear();
                    foreach (string str in list)
                    {
                        string str2 = str.Split(new char[] { '-' })[0];
                        double num = Convert.ToDouble(str.Substring(str.IndexOf('-') + 1));
                        this.FNMoneyDic[str2] = num;
                    }
                }
            }

            public Dictionary<string, Dictionary<string, List<string>>> FNNumberDic
            {
                get
                {
                    Dictionary<string, Dictionary<string, List<string>>> dictionary = new Dictionary<string, Dictionary<string, List<string>>>();
                    Dictionary<string, Dictionary<string, List<string>>> dictionary2 = new Dictionary<string, Dictionary<string, List<string>>>();
                    List<string> list = new List<string>();
                    foreach (string str in this.NumberList)
                    {
                        Dictionary<string, List<string>> dictionary3;
                        string pZuKey = str.Split(new char[] { '|' })[2];
                        string perZuKey = ConfigurationStatus.FNBase.GetPerZuKey(pZuKey);
                        if (!dictionary2.ContainsKey(perZuKey))
                        {
                            dictionary3 = new Dictionary<string, List<string>>();
                            List<string> list2 = new List<string> {
                                str
                            };
                            dictionary3[pZuKey] = list2;
                            dictionary2[perZuKey] = dictionary3;
                            list.Add(perZuKey);
                        }
                        else
                        {
                            dictionary3 = dictionary2[perZuKey];
                            if (!dictionary3.ContainsKey(pZuKey))
                            {
                                List<string> list3 = new List<string> {
                                    str
                                };
                                dictionary3[pZuKey] = list3;
                            }
                            else
                            {
                                dictionary3[pZuKey].Add(str);
                            }
                        }
                    }
                    list.Sort(delegate (string ZuKey1, string ZuKey2) {
                        int num = Convert.ToInt32(ZuKey1);
                        int num2 = Convert.ToInt32(ZuKey2);
                        return num - num2;
                    });
                    foreach (string str3 in list)
                    {
                        dictionary[str3] = dictionary2[str3];
                    }
                    return dictionary;
                }
            }

            public string FNNumberString
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.ZuKeyDic.Keys)
                    {
                        string item = this.NumberString(this.ZuKeyDic[str2]);
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "\r\n");
                }
            }

            public double GetFNGain =>
                (this.FNGainDic.ContainsKey(this.FNName) ? this.FNGainDic[this.FNName] : 0.0);

            public string LoadPlanListData
            {
                get
                {
                    List<string> pList = new List<string> {
                        this.UploadTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        this.LotteryName,
                        this.LotteryID,
                        this.CurrentExpect,
                        this.FNName,
                        this.FNCHType,
                        this.PlayType,
                        this.PlayName,
                        this.RXWZListString,
                        this.RXZJ,
                        this.Number.ToString(),
                        this.TimesListString,
                        this.AutoTimesString.ToString(),
                        this.FBListString,
                        this.FNGainString,
                        this.FNMNGainString,
                        this.FNMoneyString,
                        this.FNMNMoneyString,
                        this.Money.ToString(),
                        this.AutoTotalMode.ToString(),
                        this.AutoWinCount.ToString(),
                        this.Gain.ToString(),
                        this.IsMNBets.ToString(),
                        this.State,
                        this.Code,
                        CommFunc.Join(this.NumberList, ";"),
                        this.IsTXFFCCH.ToString()
                    };
                    return CommFunc.Join(pList, "&");
                }
                set
                {
                    List<string> list = CommFunc.SplitString(value, "&", -1);
                    if (list.Count == 0x1b)
                    {
                        this.UploadTime = Convert.ToDateTime(list[0]);
                        this.LotteryName = list[1];
                        this.LotteryID = list[2];
                        this.BeginExpect = this.EndExpect = list[3];
                        this.FNName = list[4];
                        this.FNCHType = list[5];
                        this.PlayType = list[6];
                        this.PlayName = list[7];
                        this.RXWZListString = list[8];
                        this.RXZJ = list[9];
                        this.Number = Convert.ToInt32(list[10]);
                        this.TimesListString = list[11];
                        this.AutoTimesString = list[12];
                        this.FBListString = list[13];
                        this.FNGainString = list[14];
                        this.FNMNGainString = list[15];
                        this.FNMoneyString = list[0x10];
                        this.FNMNMoneyString = list[0x11];
                        this.Money = Convert.ToDouble(list[0x12]);
                        this.AutoTotalMode = Convert.ToDouble(list[0x13]);
                        this.AutoWinCount = Convert.ToDouble(list[20]);
                        this.Gain = Convert.ToDouble(list[0x15]);
                        this.IsMNBets = Convert.ToBoolean(list[0x16]);
                        this.State = list[0x17];
                        this.Code = list[0x18];
                        this.Cycle = 1;
                        this.CurrentCycle = 1;
                        this.NumberList = CommFunc.SplitString(list[0x19], ";", -1);
                        this.IsTXFFCCH = Convert.ToBoolean(list[0x1a]);
                    }
                }
            }

            public string Play =>
                (this.PlayType + this.PlayName);

            public ConfigurationStatus.PlayBase PlayInfo =>
                CommFunc.GetPlayInfo(this.PlayType, this.PlayName);

            public List<List<int>> RXWZList =>
                this.PlayInfo.ConvertRXDSWZList(this.RXWZ);

            public string RXWZListString
            {
                get
                {
                    string str = "";
                    if (this.RXWZ == null)
                    {
                        return str;
                    }
                    return CommFunc.Join(this.RXWZ, "|");
                }
                set
                {
                    if (value == "")
                    {
                        this.RXWZ = null;
                    }
                    this.RXWZ = CommFunc.SplitInt(value, "|");
                }
            }

            public List<int> RXZJList =>
                CommFunc.ConvertIntList(this.RXZJ);

            public string SharePlanListData
            {
                get
                {
                    List<string> pList = new List<string> {
                        this.UploadTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        this.LotteryName,
                        this.LotteryID,
                        this.CurrentExpect,
                        this.FNName,
                        this.FNCHType,
                        this.PlayType,
                        this.PlayName,
                        this.RXWZListString,
                        this.RXZJ,
                        this.Number.ToString(),
                        this.TimesListString,
                        this.AutoTimesString.ToString(),
                        this.FBListString,
                        this.FNGainString,
                        this.FNMNGainString,
                        this.FNMoneyString,
                        this.FNMNMoneyString,
                        this.Unit.ToString(),
                        Convert.ToInt32(this.UnitType).ToString(),
                        this.Money.ToString(),
                        this.AutoTotalMode.ToString(),
                        this.AutoWinCount.ToString(),
                        this.Gain.ToString(),
                        this.IsMNBets.ToString(),
                        this.State,
                        this.Code,
                        CommFunc.LZMAEncode(CommFunc.Join(this.NumberList, ";")),
                        this.IsTXFFCCH.ToString()
                    };
                    return CommFunc.Join(pList, ",");
                }
                set
                {
                    List<string> list = CommFunc.SplitString(value, ",", -1);
                    if (list.Count == 0x1d)
                    {
                        this.UploadTime = Convert.ToDateTime(list[0]);
                        this.LotteryName = list[1];
                        this.LotteryID = list[2];
                        this.BeginExpect = this.EndExpect = list[3];
                        this.FNName = list[4];
                        this.FNCHType = list[5];
                        this.PlayType = list[6];
                        this.PlayName = list[7];
                        this.RXWZListString = list[8];
                        this.RXZJ = list[9];
                        this.Number = Convert.ToInt32(list[10]);
                        this.TimesListString = list[11];
                        this.AutoTimesString = list[12];
                        this.FBListString = list[13];
                        this.FNGainString = list[14];
                        this.FNMNGainString = list[15];
                        this.FNMoneyString = list[0x10];
                        this.FNMNMoneyString = list[0x11];
                        this.Unit = Convert.ToInt32(list[0x12]);
                        this.UnitType = (ConfigurationStatus.SCUnitType) Convert.ToInt32(list[0x13]);
                        this.Money = Convert.ToDouble(list[20]);
                        this.AutoTotalMode = Convert.ToDouble(list[0x15]);
                        this.AutoWinCount = Convert.ToDouble(list[0x16]);
                        this.Gain = Convert.ToDouble(list[0x17]);
                        this.IsMNBets = Convert.ToBoolean(list[0x18]);
                        this.State = list[0x19];
                        this.Code = list[0x1a];
                        this.Cycle = 1;
                        this.CurrentCycle = 1;
                        this.NumberList = CommFunc.SplitString(CommFunc.LZMADecode(list[0x1b]), ";", -1);
                        this.IsTXFFCCH = Convert.ToBoolean(list[0x1c]);
                        this.FNGainString = "";
                        this.FNMNGainString = "";
                        this.FNMoneyString = "";
                        this.FNMNMoneyString = "";
                        this.IsMNBets = false;
                    }
                }
            }

            public string TimesListString
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.TimesDic.Keys)
                    {
                        string item = str2 + "-" + CommFunc.Join(this.TimesDic[str2], ";");
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList, "|");
                }
                set
                {
                    List<string> list = CommFunc.SplitString(value, "|", -1);
                    this.TimesDic.Clear();
                    foreach (string str in list)
                    {
                        string str2 = str.Split(new char[] { '-' })[0];
                        List<double> list2 = CommFunc.SplitDouble(str.Split(new char[] { '-' })[1], ";");
                        this.TimesDic[str2] = list2;
                    }
                }
            }

            public string TJBZCountString =>
                CommFunc.Join(CommFunc.GetDicValueList<int>(this.TJBZCountDic), "\r\n");

            public ConfigurationStatus.LotteryType Type =>
                AppInfo.Current.LotteryDic[this.LotteryID].Type;

            public string UnitString
            {
                get
                {
                    string str = "";
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Yuan)
                    {
                        return "yuan";
                    }
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Jiao)
                    {
                        return "jiao";
                    }
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Fen)
                    {
                        return "fen";
                    }
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Li)
                    {
                        str = "li";
                    }
                    return str;
                }
            }

            public string UnitZWJXString
            {
                get
                {
                    string str = "";
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Yuan)
                    {
                        return "y";
                    }
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Jiao)
                    {
                        return "j";
                    }
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Fen)
                    {
                        return "f";
                    }
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Li)
                    {
                        str = "l";
                    }
                    return str;
                }
            }

            public string UnitZWString
            {
                get
                {
                    string str = "";
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Yuan)
                    {
                        return "元";
                    }
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Jiao)
                    {
                        return "角";
                    }
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Fen)
                    {
                        return "分";
                    }
                    if (this.UnitType == ConfigurationStatus.SCUnitType.Li)
                    {
                        str = "厘";
                    }
                    return str;
                }
            }

            public string ViewPlay =>
                $"{this.PlayName}【{this.PlayType}】";

            public string WaitLottery =>
                this.WaitString;
        }

        public class SCTimesCount
        {
            public ConfigurationStatus.SCTimesType BTType;
            public int Cycle;
            public Dictionary<string, ConfigurationStatus.FBType> FBList = new Dictionary<string, ConfigurationStatus.FBType>();
            public string FNName;
            public List<ConfigurationStatus.TimesScheme> FNTimesList = new List<ConfigurationStatus.TimesScheme>();
            public double Mode;
            public double Money;
            public int Number;
            public Dictionary<string, List<double>> TimesList = new Dictionary<string, List<double>>();
            public ConfigurationStatus.SCUnitType Unit;
            public int UnitIndex = 0;

            public Dictionary<string, List<double>> GetBetsTimesList
            {
                get
                {
                    Dictionary<string, List<double>> dictionary = new Dictionary<string, List<double>>();
                    foreach (string str in this.TimesList.Keys)
                    {
                        List<double> pList = this.TimesList[str];
                        dictionary[str] = CommFunc.CopyList<double>(pList);
                    }
                    return dictionary;
                }
            }

            public List<double> GetFNTimesList
            {
                get
                {
                    List<double> list = new List<double>();
                    foreach (ConfigurationStatus.TimesScheme scheme in this.FNTimesList)
                    {
                        list.Add((double) scheme.Times);
                    }
                    return list;
                }
            }
        }

        public enum SCTimesType
        {
            Plan,
            FN
        }

        public enum SCUnitType
        {
            Yuan,
            Jiao,
            Fen,
            Li
        }

        public class ShareBets
        {
            public ConfigurationStatus.BetsType BetsTypeInfo;
            public string FollowErrorHint = "";
            public List<int> FollowErrorIndexList = new List<int>();
            public Dictionary<string, int> FollowPlanCountDic = new Dictionary<string, int>();
            public List<ConfigurationStatus.SCPlan> FollowPlanList = new List<ConfigurationStatus.SCPlan>();
            public bool FollowYes = false;
            public Dictionary<string, int> SendPlanCountDic = new Dictionary<string, int>();
            public List<ConfigurationStatus.SCPlan> SendPlanList = new List<ConfigurationStatus.SCPlan>();
            public string ShareCode;

            public static string GetDecodeShareCode(string pSource)
            {
                if (pSource == "")
                {
                    return "";
                }
                return CommFunc.Decode(pSource, AppInfo.Account.Configuration.FNEncrypt);
            }

            public static string GetEncodeShareCode(string pID = "")
            {
                if (pID == "")
                {
                    pID = AppInfo.Account.SendUserID;
                }
                string pSource = pID;
                return CommFunc.Encode(pSource, AppInfo.Account.Configuration.FNEncrypt);
            }

            public string ShareUser =>
                GetDecodeShareCode(this.ShareCode);
        }

        public class ShowTap
        {
            public int Index;
            public string Name;
            public bool Selected;

            public ShowTap(string pValue)
            {
                string[] strArray = Strings.Split(pValue, "-", -1, CompareMethod.Binary);
                if (strArray.Length == 3)
                {
                    this.Name = strArray[1];
                    this.Selected = Convert.ToBoolean(strArray[0]);
                    this.Index = Convert.ToInt32(strArray[2]);
                }
            }

            public ShowTap(string pName, bool pSelected, int pIndex)
            {
                this.Name = pName;
                this.Selected = pSelected;
                this.Index = pIndex;
            }

            public string GetValue() => 
                string.Concat(new object[] { this.Selected, "-", this.Name, "-", this.Index });
        }

        public class ShrinkData
        {
            public ConfigurationStatus.ShrinkIntExclude AndEndExclude = null;
            public ConfigurationStatus.ShrinkIntExclude AndExclude = null;
            public ConfigurationStatus.ShrinkDM DM = null;
            public ConfigurationStatus.ShrinkDWExclude DWExclude = null;
            public ConfigurationStatus.ShrinkStringExclude DXExclude = null;
            public ConfigurationStatus.ShrinkStringExclude DZXExclude = null;
            public ConfigurationStatus.ShrinkStringExclude JOExclude = null;
            public ConfigurationStatus.ShrinkStringExclude L012Exclude = null;
            public ConfigurationStatus.ShrinkIntExclude OutExclude = null;
            public ConfigurationStatus.ShrinkStringExclude ZHExclude = null;
        }

        public class ShrinkDM
        {
            public List<int> CodeList = null;

            public ShrinkDM(List<int> pCodeList)
            {
                this.CodeList = pCodeList;
            }
        }

        public class ShrinkDWExclude
        {
            public List<int> RangeList1;
            public List<int> RangeList2;
            public List<int> RangeList3;

            public ShrinkDWExclude(List<int> pRangeList1, List<int> pRangeList2)
            {
                this.RangeList1 = null;
                this.RangeList2 = null;
                this.RangeList3 = null;
                this.RangeList1 = pRangeList1;
                this.RangeList2 = pRangeList2;
            }

            public ShrinkDWExclude(List<int> pRangeList1, List<int> pRangeList2, List<int> pRangeList3)
            {
                this.RangeList1 = null;
                this.RangeList2 = null;
                this.RangeList3 = null;
                this.RangeList1 = pRangeList1;
                this.RangeList2 = pRangeList2;
                this.RangeList3 = pRangeList3;
            }
        }

        public class ShrinkIntExclude
        {
            public List<int> RangeList = null;

            public ShrinkIntExclude(List<int> pRangeList)
            {
                this.RangeList = pRangeList;
            }
        }

        public class ShrinkStringExclude
        {
            public List<string> RangeList = null;

            public ShrinkStringExclude(List<string> pRangeList)
            {
                this.RangeList = pRangeList;
            }
        }

        public class SortInt
        {
            public string Key;
            public int Value;

            public SortInt(string pKey, int pValue)
            {
                this.Key = pKey;
                this.Value = pValue;
            }
        }

        public enum StopBetsType
        {
            Bets,
            AddFN
        }

        public class TimesCount
        {
            public int Cycle;
            public List<int> FreeList = new List<int>();
            public double GainFix;
            public double GainRatio;
            public double MaxPrize;
            public double Mode;
            public double Money;
            public int Number;
            public string PlayName;
            public string PlayType;
            public double Start;
            public int StartTimes;
            public double SumBegin;
            public double SumStep;
            public int TBCycle;
            public double TotalOutput = 0.0;
            public ConfigurationStatus.TimesType Type;
            public string TypeString;

            public double GetTBGain
            {
                get
                {
                    if (this.Type == ConfigurationStatus.TimesType.GainRatio)
                    {
                        return this.GainRatio;
                    }
                    if (this.Type == ConfigurationStatus.TimesType.GainFix)
                    {
                        return this.GainFix;
                    }
                    if (this.Type == ConfigurationStatus.TimesType.Sum)
                    {
                        return (this.SumBegin + this.SumStep);
                    }
                    return -1.0;
                }
            }

            public string Play =>
                (this.PlayType + this.PlayName);
        }

        public class TimesData
        {
            public string Expect;
            public double Gain;
            public double GainRatio;
            public double Input;
            public double Output;
            public int TBCycle;
            public double Times;
            public double TotalOutput = 0.0;
        }

        public class TimesScheme
        {
            public string AppName;
            public const string AppNameString = "软件名称";
            public int ID;
            public const string IDString = "ID";
            public int NoAfter;
            public const string NoAfterString = "挂后ID";
            public bool NoJK;
            public const string NoJKString = "挂后监控";
            public bool NoOtherFNSelect;
            public const string NoOtherFNString = "挂后跳转";
            public string NoOtherFNValue;
            public int Times;
            public const string TimesString = "倍数";
            public int YesAfter;
            public const string YesAfterString = "中后ID";
            public bool YesJK;
            public const string YesJKString = "中后监控";
            public bool YesOtherFNSelect;
            public const string YesOtherFNString = "中后跳转";
            public string YesOtherFNValue;

            public TimesScheme()
            {
                this.ID = -1;
                this.Times = 1;
                this.YesAfter = 1;
                this.NoAfter = 1;
                this.YesJK = false;
                this.YesOtherFNSelect = false;
                this.YesOtherFNValue = "";
                this.NoJK = false;
                this.NoOtherFNSelect = false;
                this.NoOtherFNValue = "";
                this.AppName = "YXZXGJ";
            }

            public TimesScheme(int pID)
            {
                this.ID = -1;
                this.Times = 1;
                this.YesAfter = 1;
                this.NoAfter = 1;
                this.YesJK = false;
                this.YesOtherFNSelect = false;
                this.YesOtherFNValue = "";
                this.NoJK = false;
                this.NoOtherFNSelect = false;
                this.NoOtherFNValue = "";
                this.ID = pID;
                this.AppName = "YXZXGJ";
            }

            public TimesScheme(string pValue)
            {
                this.ID = -1;
                this.Times = 1;
                this.YesAfter = 1;
                this.NoAfter = 1;
                this.YesJK = false;
                this.YesOtherFNSelect = false;
                this.YesOtherFNValue = "";
                this.NoJK = false;
                this.NoOtherFNSelect = false;
                this.NoOtherFNValue = "";
                this.StringToInfo(pValue);
            }

            public string InfoToString()
            {
                Dictionary<string, string> pDic = new Dictionary<string, string> {
                    ["软件名称"] = this.AppName,
                    ["ID"] = this.ID.ToString(),
                    ["倍数"] = this.Times.ToString(),
                    ["中后ID"] = this.YesAfter.ToString(),
                    ["挂后ID"] = this.NoAfter.ToString(),
                    ["中后监控"] = this.YesJK.ToString(),
                    ["中后跳转"] = this.YesOtherFNSelect.ToString() + "-" + this.YesOtherFNValue,
                    ["挂后监控"] = this.NoJK.ToString(),
                    ["挂后跳转"] = this.NoOtherFNSelect.ToString() + "-" + this.NoOtherFNValue
                };
                return CommFunc.Join(pDic, ";");
            }

            public void StringToInfo(string pValue)
            {
                Dictionary<string, string> configuration = CommFunc.GetConfiguration(pValue, ";");
                foreach (string str in configuration.Keys)
                {
                    switch (str)
                    {
                        case "软件名称":
                            this.AppName = configuration[str];
                            break;

                        case "ID":
                            this.ID = Convert.ToInt32(configuration[str]);
                            break;

                        case "倍数":
                            this.Times = Convert.ToInt32(configuration[str]);
                            break;

                        case "中后ID":
                            this.YesAfter = Convert.ToInt32(configuration[str]);
                            break;

                        case "挂后ID":
                            this.NoAfter = Convert.ToInt32(configuration[str]);
                            break;

                        case "中后监控":
                            this.YesJK = Convert.ToBoolean(configuration[str]);
                            break;

                        case "中后跳转":
                            if (configuration[str].Split(new char[] { '-' }).Length == 2)
                            {
                                this.YesOtherFNSelect = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                                this.YesOtherFNValue = configuration[str].Split(new char[] { '-' })[1];
                            }
                            break;

                        case "挂后监控":
                            this.NoJK = Convert.ToBoolean(configuration[str]);
                            break;

                        case "挂后跳转":
                            if (configuration[str].Split(new char[] { '-' }).Length == 2)
                            {
                                this.NoOtherFNSelect = Convert.ToBoolean(configuration[str].Split(new char[] { '-' })[0]);
                                this.NoOtherFNValue = configuration[str].Split(new char[] { '-' })[1];
                            }
                            break;
                    }
                }
            }

            public string GetNoOtherFN
            {
                get
                {
                    string noOtherFNValue = "";
                    if (this.NoOtherFNSelect)
                    {
                        noOtherFNValue = this.NoOtherFNValue;
                    }
                    return noOtherFNValue;
                }
            }

            public string GetYesOtherFN
            {
                get
                {
                    string yesOtherFNValue = "";
                    if (this.YesOtherFNSelect)
                    {
                        yesOtherFNValue = this.YesOtherFNValue;
                    }
                    return yesOtherFNValue;
                }
            }
        }

        public enum TimesType
        {
            GainRatio,
            GainFix,
            Sum,
            Free
        }

        public enum TimeType
        {
            FW,
            DJS
        }

        public class TJData
        {
            public DateTime EndDate;
            public string EndTime;
            public string FNName;
            public bool IsReset;
            public string LotteryID;
            public string LotteryName;
            public ConfigurationStatus.LSData LSDataInfo;
            public double Prize;
            public DateTime StartDate;
            public string StartTime;
            public bool TimeSelect;
        }

        public class TJDataView1
        {
            public int BetsCount;
            public string Date;
            public double Gain;
            public double MaxKS;
            public int MaxLC;
            public double MaxYL;
            public int NoCount;
            public double XZMoney;
            public int YesCount;
            public int ZRCount;
            public Dictionary<string, string> ZRDic = new Dictionary<string, string>();

            public TJDataView1(string pDate)
            {
                this.Date = pDate;
            }

            public string ZRValue
            {
                get
                {
                    List<string> pList = new List<string>();
                    foreach (string str2 in this.ZRDic.Keys)
                    {
                        string item = $"{str2}【{this.ZRDic[str2].Split(new char[] { '|' })[1]}】";
                        pList.Add(item);
                    }
                    return CommFunc.Join(pList);
                }
            }
        }

        public class TJDataView2
        {
            public List<string> CodeList;
            public string Date;
            public string Expect;
            public double Gain1;
            public double Gain2;
            public double Gain3;
            public string Time;
            public Dictionary<string, ConfigurationStatus.SCPlan> ValueDic = new Dictionary<string, ConfigurationStatus.SCPlan>();
        }

        public delegate void WebDataDelegate();

        public class WJJHExpect
        {
            public string Expect1 = "";
            public string Expect2 = "";
            public string Value = "";
        }

        public class WJJHInfo
        {
            public string CSPlay = "";
            public string CSSeparate = "";
            public string CSValueLeft = "";
            public string CSValueRight = "";
            public string Error = "";
            public string HWNDString = "";
            public string PlanValue = "";
            public string PlayName = "";
            public string PlayType = "";

            public string CSPlayName
            {
                get
                {
                    string cSPlay = this.CSPlay;
                    if (cSPlay.Contains("."))
                    {
                        string pStr = cSPlay.Split(new char[] { '.' })[0];
                        if (CommFunc.CheckIsNumber(pStr))
                        {
                            cSPlay = cSPlay.Split(new char[] { '.' })[1];
                        }
                    }
                    return cSPlay;
                }
            }

            public string Play =>
                (this.PlayType + this.PlayName);

            public ConfigurationStatus.PlayBase PlayInfo =>
                CommFunc.GetPlayInfo(this.PlayType, this.PlayName);
        }

        public enum YLCHType
        {
            DaYu,
            XiaoYu,
            DaYuDeng,
            XiaoYuDeng,
            DengYu
        }
    }
}

