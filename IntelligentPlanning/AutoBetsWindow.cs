namespace IntelligentPlanning
{
    using IntelligentPlanning.CustomControls;
    using IntelligentPlanning.ExDataGridView;
    using IntelligentPlanning.Properties;
    using IntelligentPlanning.WorkThread;
    using Microsoft.VisualBasic;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    public class AutoBetsWindow : Form
    {
        private bool _RunEvent = false;
        private string AppName = "";
        private string[] Args = null;
        private Dictionary<string, int> AutoBeginDic = new Dictionary<string, int>();
        private Dictionary<string, int> AutoEndDic = new Dictionary<string, int>();
        private Dictionary<string, ConfigurationStatus.AutoBets> BetsDic = new Dictionary<string, ConfigurationStatus.AutoBets>();
        private List<string> BetsErrorList = new List<string>();
        private BTCount BT_Main;
        private Button Btn_Bets;
        private Button Btn_LSRefresh;
        private Button Btn_TJRefresh;
        private Button Btn_TJTop;
        private Button Btn_ViewTop;
        private List<Button> ButtonList = null;
        private ComboBox Cbb_BetsEndType;
        private ComboBox Cbb_BTFNEdit;
        private ComboBox Cbb_FNCHType;
        private ComboBox Cbb_LoginPT;
        private ComboBox Cbb_Lottery;
        private ComboBox Cbb_LSBJType;
        private ComboBox Cbb_LSFN;
        private ComboBox Cbb_TJFN;
        private ComboBox Cbb_TJPrize;
        private CDCount CD_Main;
        private List<CheckBox> CheckBoxList = null;
        private List<string> CHTypeList = null;
        private CheckBox Ckb_AddBTFN;
        private CheckBox Ckb_AddLine;
        private CheckBox Ckb_AddScheme;
        private CheckBox Ckb_AddTimes;
        private CheckBox Ckb_AppName;
        private CheckBox Ckb_AutomationRun;
        private CheckBox Ckb_AutoSizeTJ;
        private CheckBox Ckb_BetsBeginTime;
        private CheckBox Ckb_BetsEndTime;
        private CheckBox Ckb_BetsSort;
        private CheckBox Ckb_BTFNEdit;
        private CheckBox Ckb_BTFNEditSkip;
        private CheckBox Ckb_CancelScheme;
        private CheckBox Ckb_ClearBetsList;
        private CheckBox Ckb_ClearScheme;
        private CheckBox Ckb_ClearTimes;
        private CheckBox Ckb_CloseMin;
        private CheckBox Ckb_CopyScheme;
        private CheckBox Ckb_Data;
        private CheckBox Ckb_DeleteBTFN;
        private CheckBox Ckb_DeleteExpect;
        private CheckBox Ckb_DeleteLine;
        private CheckBox Ckb_DeleteScheme;
        private CheckBox Ckb_DeleteTimes;
        private CheckBox Ckb_DQStopBets;
        private CheckBox Ckb_EditScheme;
        private CheckBox Ckb_EditTimes;
        private CheckBox Ckb_EditTimesPlan;
        private CheckBox Ckb_ExportScheme;
        private CheckBox Ckb_FNLT;
        private CheckBox Ckb_ImportScheme;
        private CheckBox Ckb_LeftInfo;
        private CheckBox Ckb_Login;
        private CheckBox Ckb_LSAutoRefresh;
        private CheckBox Ckb_LSBJ;
        private CheckBox Ckb_LSStop;
        private CheckBox Ckb_MN1;
        private CheckBox Ckb_MN2;
        private CheckBox Ckb_MN3;
        private CheckBox Ckb_MN4;
        private CheckBox Ckb_MNBets;
        private CheckBox Ckb_OpenHint;
        private CheckBox Ckb_PlanShowHide;
        private CheckBox Ckb_PlaySound;
        private CheckBox Ckb_PWClear;
        private CheckBox Ckb_PWPaste;
        private CheckBox Ckb_RefreshUser;
        private CheckBox Ckb_RrfreshPT;
        private CheckBox Ckb_RrfreshPTLine;
        private CheckBox Ckb_SaveScheme;
        private CheckBox Ckb_SaveTimes;
        private CheckBox Ckb_SBStopBets;
        private CheckBox Ckb_ShareBetsManage;
        private CheckBox Ckb_ShareScheme;
        private CheckBox Ckb_ShareSchemeManage;
        private CheckBox Ckb_ShowHideUser;
        private CheckBox Ckb_TBCount;
        private CheckBox Ckb_TJFindXS;
        private CheckBox Ckb_TJReset;
        private CheckBox Ckb_TJStop;
        private CheckBox Ckb_TJTimeRange;
        private ContextMenuStrip Cms_Menu;
        private List<ComboBox> ComboBoxList = null;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private DateTimePicker Dtp_BetsBeginTime;
        private DateTimePicker Dtp_BetsEndTime;
        private DateTimePicker Dtp_LSDataRange;
        private DateTimePicker Dtp_TJDataRange1;
        private DateTimePicker Dtp_TJDataRange2;
        private DateTimePicker Dtp_TJTimeRange1;
        private DateTimePicker Dtp_TJTimeRange2;
        private ExpandGirdView Egv_BTFNMain;
        private ExpandGirdView Egv_BTFNTimesList;
        private ExpandGirdView Egv_DataList;
        private ExpandGirdView Egv_LSDataList;
        private ExpandGirdView Egv_NoticeList;
        private ExpandGirdView Egv_PlanList;
        private ExpandGirdView Egv_PTLineList;
        private ExpandGirdView Egv_SchemeList;
        private ExpandGirdView Egv_ShowTapList;
        private ExpandGirdView Egv_TJDataList1;
        private ExpandGirdView Egv_TJDataList2;
        private ErrorProvider Err_Hint;
        private FNDMLHLine FN_DMLH;
        private FNGDQMLine FN_GDQM;
        private FNGJDMLHLine FN_GJDMLH;
        private FNGJKMTMLine FN_GJKMTM;
        private FNKMTMLine FN_KMTM;
        private FNLHKMTMLine FN_LHKMTM;
        private FNLRWCHLine FN_LRWCH;
        private FNBCFCHLine FN_BCFCH;
        private FNSJCHLine FN_SJCH;
        private FNWJJHLine FN_WJJH;
        private FNYLCHLine FN_YLCH;
        private HJFGCount HJFG_Main;
        private bool IsEditScheme = false;
        private List<Label> LabelList = null;
        private string LastBetsIndex = "";
        private Label Lbl_AppHint;
        private Label Lbl_AppName;
        private Label Lbl_BankBalanceKey;
        private Label Lbl_BankBalanceValue;
        private Label Lbl_BetsCountKey;
        private Label Lbl_BetsCountValue;
        private Label Lbl_BetsExpectKey;
        private Label Lbl_BetsExpectValue;
        private Label Lbl_BetsGainPlanKey;
        private Label Lbl_BetsGainPlanValue;
        private Label Lbl_BetsHint;
        private Label Lbl_BetsKey;
        private Label Lbl_BetsMoneyPlanKey;
        private Label Lbl_BetsMoneyPlanValue;
        private Label Lbl_BetsTime1;
        private Label Lbl_BetsTime2;
        private Label Lbl_BetsValue;
        private Label Lbl_BTFNEdit;
        private Label Lbl_CurrentCode1;
        private Label Lbl_CurrentCode10;
        private Label Lbl_CurrentCode2;
        private Label Lbl_CurrentCode3;
        private Label Lbl_CurrentCode4;
        private Label Lbl_CurrentCode5;
        private Label Lbl_CurrentCode6;
        private Label Lbl_CurrentCode7;
        private Label Lbl_CurrentCode8;
        private Label Lbl_CurrentCode9;
        private Label Lbl_CurrentExpect;
        private Label Lbl_CurrentExpect1;
        private Label Lbl_FNCHType;
        private Label Lbl_FNEncrypt;
        private Label Lbl_FNName;
        private Label Lbl_GJFNEncrypt;
        private Label Lbl_ID;
        private Label Lbl_IDHint;
        private Label Lbl_IDKey;
        private Label Lbl_IDValue;
        private Label Lbl_KSStopBets;
        private Label Lbl_LGMaxKey;
        private Label Lbl_LGMaxValue;
        private Label Lbl_LoginHint;
        private Label Lbl_LoginPT;
        private Label Lbl_Lottery;
        private Label Lbl_LSBJExpect2;
        private Label Lbl_LSDataRange;
        private Label Lbl_LSFN;
        private Label Lbl_LSLotteryKey;
        private Label Lbl_LSLotteryValue;
        private Label Lbl_LSPlayKey;
        private Label Lbl_LSPlayValue;
        private Label Lbl_LSRefreshHint;
        private Label Lbl_LZMaxKey;
        private Label Lbl_LZMaxValue;
        private Label Lbl_MNBets;
        private Label Lbl_MNBetsGainPlanKey;
        private Label Lbl_MNBetsGainPlanValue;
        private Label Lbl_MNBetsMoneyPlanKey;
        private Label Lbl_MNBetsMoneyPlanValue;
        private Label Lbl_NextExpect;
        private Label Lbl_NextExpect1;
        private Label Lbl_NextTime;
        private Label Lbl_NextTime1;
        private Label Lbl_PW;
        private Label Lbl_ShareBetsHint;
        private Label Lbl_StopBets;
        private Label Lbl_TJData;
        private Label Lbl_TJDataRange;
        private Label Lbl_TJFindXS;
        private Label Lbl_TJFN;
        private Label Lbl_TJLotteryKey;
        private Label Lbl_TJLotteryValue;
        private Label Lbl_TJPlayKey;
        private Label Lbl_TJPlayValue;
        private Label Lbl_TJPrize;
        private Label Lbl_TJRefreshHint;
        private Label Lbl_TJTime;
        private LinkLabel Lbl_Web;
        private Label Lbl_YLStopBets;
        private Label Lbl_ZQLKey;
        private Label Lbl_ZQLValue;
        private List<ConfigurationStatus.LSDataView> LSDataViewList = new List<ConfigurationStatus.LSDataView>();
        private List<TabPage> MainPageList = new List<TabPage>();
        private MainThread mainThread = null;
        private NotifyIcon Nic_Hint;
        private NumericUpDown Nm_BetsTime;
        private NumericUpDown Nm_BTFNEdit;
        private NumericUpDown Nm_DeleteExpect;
        private NumericUpDown Nm_LSBJExpect;
        private NumericUpDown Nm_TJFindXS;
        private Dictionary<string, string> NoticeDic = new Dictionary<string, string>();
        private List<string> NoticeList = new List<string>();
        private PictureBox Pic_Notice;
        private PictureSwitch Piw_Main;
        private PK10CodeLine PK_Code;
        private PK10CodeLineSmall PK_CodeSmall;
        private Panel Pnl_AppName;
        private Panel Pnl_Bets;
        private Panel Pnl_Bets1;
        private Panel Pnl_Bets2;
        private Panel Pnl_BetsInfoExpect;
        private Panel Pnl_BetsInfoMain;
        private Panel Pnl_BetsInfoMN;
        private Panel Pnl_BetsInfoMNRight;
        private Panel Pnl_BetsInfoTop;
        private Panel Pnl_BetsInfoTop1;
        private Panel Pnl_BetsInfoTop2;
        private Panel Pnl_BetsInfoTop2Left;
        private Panel Pnl_BetsInfoTopLeft;
        private Panel Pnl_BetsInfoTopMain;
        private Panel Pnl_BetsInfoTopRight;
        private Panel Pnl_BetsInfoTopRight1;
        private Panel Pnl_BetsLeft;
        private Panel Pnl_BetsMain;
        private Panel Pnl_BetsRight;
        private Panel Pnl_BetsType;
        private Panel Pnl_BTFN;
        private Panel Pnl_BTFNList;
        private Panel Pnl_BTFNMain;
        private Panel Pnl_CDCount;
        private Panel Pnl_CurrentCode1;
        private Panel Pnl_CurrentCode2;
        private Panel Pnl_CurrentCode3;
        private Panel Pnl_CurrentCode4;
        private Panel Pnl_CurrentExpect;
        private Panel Pnl_CurrentExpect1;
        private Panel Pnl_CurrentExpectTop;
        private Panel Pnl_DataBottom;
        private Panel Pnl_DataBottom1;
        private Panel Pnl_DataMain;
        private Panel Pnl_DataTop2;
        private Panel Pnl_FNBottom;
        private Panel Pnl_GG;
        private Panel Pnl_Info;
        private Panel Pnl_InfoRight;
        private Panel Pnl_LSData;
        private Panel Pnl_LSDataLeft;
        private Panel Pnl_LSDataMain;
        private Panel Pnl_LSDataRight;
        private Panel Pnl_LSDataTop;
        private Panel Pnl_LSDataTop1;
        private Panel Pnl_LTUserInfo;
        private Panel Pnl_LTUserInfoTop;
        private Panel Pnl_Main;
        private Panel Pnl_NextExpect;
        private Panel Pnl_NextExpectTop;
        private Panel Pnl_Notice;
        private Panel Pnl_NoticeLeft;
        private Panel Pnl_OpenData;
        private Panel Pnl_PlanListBottom;
        private Panel Pnl_PlanListTop;
        private Panel Pnl_PTRefresh;
        private Panel Pnl_RrfreshPT;
        private Panel Pnl_Scheme;
        private Panel Pnl_SchemeBottom;
        private Panel Pnl_SchemeInfo;
        private Panel Pnl_SchemeLeft;
        private Panel Pnl_SchemeMain;
        private Panel Pnl_SchemeShare;
        private Panel Pnl_SchemeTop1;
        private Panel Pnl_SchemeTop2;
        private Panel Pnl_Scroll;
        private Panel Pnl_Setting;
        private Panel Pnl_TimesBottom;
        private Panel Pnl_TJData;
        private Panel Pnl_TJDataFind;
        private Label Pnl_TJDataHint;
        private Panel Pnl_TJDataMain;
        private Panel Pnl_TJDataTop;
        private Panel Pnl_TJDataTop1;
        private Panel Pnl_TJDataTop2;
        private Panel Pnl_TJRight1;
        private Panel Pnl_TJRight2;
        private Panel Pnl_Top;
        private Panel Pnl_UserLogin1;
        private Panel Pnl_UserLogin2;
        private Dictionary<string, ConfigurationStatus.PTLine> PTLineDic = new Dictionary<string, ConfigurationStatus.PTLine>();
        private string PTName = "";
        private List<RadioButton> RadioButtonList = null;
        private RadioButton Rdb_CGBets;
        private RadioButton Rdb_LSBJExpect;
        private RadioButton Rdb_LSBJType;
        private RadioButton Rdb_ShareBets;
        private string RegConfigPath = "";
        public FrmRunProgress runProgress;
        private ScrollingText Sct_Notice;
        private string SelectPT = "";
        private List<TabPage> SettingPageList = new List<TabPage>();
        private List<ConfigurationStatus.ShowTap> ShowTapList = new List<ConfigurationStatus.ShowTap>();
        private ShrinkEX SK_EX;
        private ShrinkSX SK_SX;
        private List<Control> SpecialControlList = null;
        private List<CheckBox> StandardList = null;
        private StatusStrip Stp_Hint;
        private TabControl Tab_Main;
        private TabPage Tap_BTCount;
        private TabPage Tap_BTFN;
        private TabPage Tap_CDCount;
        private TabPage Tap_HJFG;
        private TabPage Tap_LSData;
        private TabPage Tap_PT;
        private TabPage Tap_Scheme;
        private TabPage Tap_Setting;
        private TabPage Tap_ShrinkEX;
        private TabPage Tap_ShrinkSX;
        private TabPage Tap_TBCount;
        private TabPage Tap_TJData;
        private TabPage Tap_TrendView;
        private TabPage Tap_ZBJ;
        private TabPage Tap_ZDBets;
        private TBCount TB_Main;
        private System.Windows.Forms.Timer Tim_NextExpect;
        private List<ConfigurationStatus.TimesData> TimesList = new List<ConfigurationStatus.TimesData>();
        private Dictionary<string, string> TJViewExpect = new Dictionary<string, string>();
        private List<ConfigurationStatus.TJDataView1> TJViewList1 = new List<ConfigurationStatus.TJDataView1>();
        private List<ConfigurationStatus.TJDataView2> TJViewList2 = new List<ConfigurationStatus.TJDataView2>();
        private Dictionary<string, int> TJZuKeyDic = new Dictionary<string, int>();
        private ToolStripSeparator toolStripSeparator1;
        private ToolTip Tot_FNHint;
        private ToolTip Tot_Hint;
        private ToolStripMenuItem Tsm_Colse;
        private ToolStripMenuItem Tsm_Vis;
        private ToolStripStatusLabel Tsp_HintKey;
        private ToolStripStatusLabel Tsp_HintValue;
        private ToolStripStatusLabel Tsp_LoginKey;
        private ToolStripStatusLabel Tsp_LoginValue;
        private ToolStripStatusLabel Tsp_PeopleKey;
        private ToolStripStatusLabel Tsp_PeopleValue;
        private ToolStripStatusLabel Tsp_QQGroupKey;
        private ToolStripStatusLabel Tsp_QQGroupValue;
        private ToolStripStatusLabel Tsp_QQKey;
        private ToolStripStatusLabel Tsp_QQValue;
        private TrendView TV_Main;
        private TextBox Txt_AppName;
        private TextBox Txt_FNName;
        private TextBox Txt_ID;
        private TextBox Txt_KSStopBets;
        private TextBox Txt_MN1;
        private TextBox Txt_MN2;
        private TextBox Txt_MN3;
        private TextBox Txt_MN4;
        private TextBox Txt_PW;
        private TextBox Txt_TJPrize;
        private TextBox Txt_YLStopBets;
        private Dictionary<string, string> VerifyCodeDic = new Dictionary<string, string>();
        private int VerifyCodeError = 0;
        private WebBrowser Web_Login;
        private List<WebBrowser> WebBrowserList = null;
        private ZBJView Zbj_Main;

        public AutoBetsWindow(string[] pArgs)
        {
            this.InitializeComponent();
            base.Icon = AppInfo.AppIcon16;
            this.Nic_Hint.Icon = AppInfo.AppIcon16;
            this.LoadMainApp();
            CommFunc.ChangeIEVersion();
            this.Args = pArgs;
        }

        private void AddQQGData(ref Dictionary<string, ConfigurationStatus.BetsCode> pFNNumberDic, ConfigurationStatus.BetsScheme pScheme, string pExpect)
        {
            List<string> dicKeyList = CommFunc.GetDicKeyList<ConfigurationStatus.BetsCode>(pScheme.FNNumberDic);
            foreach (string str in dicKeyList)
            {
                pFNNumberDic[str] = pScheme.FNNumberDic[str];
            }
            Dictionary<string, ConfigurationStatus.BetsCode> dictionary = new Dictionary<string, ConfigurationStatus.BetsCode>();
            foreach (string str2 in pFNNumberDic.Keys)
            {
                string str3 = str2;
                if (!str3.Contains("-"))
                {
                    str3 = str3 + "-" + pExpect;
                }
                dictionary[str3] = pFNNumberDic[str2];
            }
            pFNNumberDic = dictionary;
        }

        private bool AnalysisVerifyCode(string pName, ref int pCount)
        {
            bool flag = false;
            try
            {
                List<string> pTextList = new List<string>();
                string pHint = CommFunc.ClickEnter(this.Text, "来自网页的消息", pTextList, true);
                if (AppInfo.LoginCancel)
                {
                    pHint = "用户取消登录";
                    AppInfo.LoginCancel = false;
                }
                if (!AppInfo.PTInfo.IsLoginRun && !AppInfo.PTInfo.AnalysisVerifyCode)
                {
                    return flag;
                }
                if ((pHint == "") && (AppInfo.PTInfo.LoginMain && AppInfo.PTInfo.AnalysisVerifyCode))
                {
                    if (AppInfo.PTInfo.IsHTLoginMain)
                    {
                        if (AppInfo.PTInfo.WebLoginMain(AppInfo.Account.PTID, AppInfo.Account.PTPW, ref pHint))
                        {
                            base.Invoke(AppInfo.PTIndexMain, new object[] { pName });
                            AppInfo.PTInfo.LoginMain = false;
                        }
                        else if (pHint == "")
                        {
                            pHint = "登录超时";
                        }
                    }
                    else
                    {
                        base.Invoke(AppInfo.LoginVerify);
                        AppInfo.PTInfo.LoginMain = false;
                    }
                }
                if (pHint == "")
                {
                    pCount++;
                    if (pCount >= 30)
                    {
                        pHint = "登录超时";
                        pCount = 0;
                    }
                }
                if (AppInfo.PTInfo.PTLoginStatus)
                {
                    AppInfo.PTInfo.AnalysisVerifyCode = false;
                    this.VerifyCodeDic.Clear();
                    this.VerifyCodeError = 0;
                    base.Invoke(AppInfo.LoginPTLottery, new object[] { 1 });
                    string cookieInternal = HttpHelper.GetCookieInternal(AppInfo.PTInfo.GetUrlLine());
                    AppInfo.PTInfo.WebCookie = cookieInternal;
                    HttpHelper.SaveCookies(cookieInternal, "");
                    AppInfo.PTInfo.GetSite(AppInfo.Current.Lottery.Type, "");
                    flag = true;
                }
                if ((pHint == "") || AppInfo.LoginCancel)
                {
                    return flag;
                }
                if (pHint == "登录超时")
                {
                    base.Invoke(AppInfo.RemoveLoginLock, new object[] { pHint, pName });
                    AppInfo.PTInfo.AnalysisVerifyCode = false;
                    AppInfo.PTInfo.LoginMain = false;
                    AppInfo.PTInfo.PTLoginStatus = false;
                    CommFunc.PlaySound("登录失败");
                    base.Invoke(AppInfo.LoginMain, new object[] { true, pName });
                    pCount = 0;
                    return flag;
                }
                if (pHint.Contains("验证码") && (this.VerifyCodeError < 2))
                {
                    base.Invoke(AppInfo.RefreshVerifyCode);
                    this.SaveErrorVerifyCode(VerifyCodeFile);
                    AppInfo.PTInfo.LoginMain = true;
                    this.VerifyCodeError++;
                    return flag;
                }
                if (pHint.Contains("用户名"))
                {
                    pHint = "用户名或者密码错误";
                }
                AppInfo.PTInfo.DefaultOption();
                this.mainThread.downCode.DefaultOption();
                base.Invoke(AppInfo.RemoveLoginLock, new object[] { pHint, this.PTName });
                AppInfo.PTInfo.AnalysisVerifyCode = false;
                AppInfo.PTInfo.LoginMain = false;
                CommFunc.PlaySound("登录失败");
            }
            catch (Exception exception)
            {
                DebugLog.SaveDebug(exception, "");
            }
            return flag;
        }

        public void AppConfig()
        {
            if (((((((((AppInfo.App == ConfigurationStatus.AppType.LDGJ) || (AppInfo.App == ConfigurationStatus.AppType.FCGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.YFENG) || (AppInfo.App == ConfigurationStatus.AppType.DPCGJ))) || (((AppInfo.App == ConfigurationStatus.AppType.BNGJ) || (AppInfo.App == ConfigurationStatus.AppType.BMGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.QQTGJ) || (AppInfo.App == ConfigurationStatus.AppType.NBGJ)))) || ((((AppInfo.App == ConfigurationStatus.AppType.YSENGJ) || (AppInfo.App == ConfigurationStatus.AppType.NRLM)) || ((AppInfo.App == ConfigurationStatus.AppType.YRYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.TAGJ))) || (((AppInfo.App == ConfigurationStatus.AppType.YDGJ) || (AppInfo.App == ConfigurationStatus.AppType.BKCGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.HZGJ) || (AppInfo.App == ConfigurationStatus.AppType.ZXGJ))))) || (((((AppInfo.App == ConfigurationStatus.AppType.UT8GJ) || (AppInfo.App == ConfigurationStatus.AppType.MZCGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.DQGJ) || (AppInfo.App == ConfigurationStatus.AppType.FEICGJ))) || (((AppInfo.App == ConfigurationStatus.AppType.OEGJ) || (AppInfo.App == ConfigurationStatus.AppType.LMHGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.HANYGJ) || (AppInfo.App == ConfigurationStatus.AppType.YBAOGJ)))) || ((((AppInfo.App == ConfigurationStatus.AppType.CAIHGJ) || (AppInfo.App == ConfigurationStatus.AppType.SLTHGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.LYSGJ) || (AppInfo.App == ConfigurationStatus.AppType.HENRGJ))) || (((AppInfo.App == ConfigurationStatus.AppType.HSGJ) || (AppInfo.App == ConfigurationStatus.AppType.DAZGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.RDYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.FLCGJ)))))) || ((((((AppInfo.App == ConfigurationStatus.AppType.CAITTGJ) || (AppInfo.App == ConfigurationStatus.AppType.QJCGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.BWTGJ) || (AppInfo.App == ConfigurationStatus.AppType.WHCGJ))) || (((AppInfo.App == ConfigurationStatus.AppType.ZYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.CBLGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.CLYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.LDYLGJ)))) || ((((AppInfo.App == ConfigurationStatus.AppType.HKCGJ) || (AppInfo.App == ConfigurationStatus.AppType.SYYLGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.MTYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.HUIZGJ))) || (((AppInfo.App == ConfigurationStatus.AppType.HDYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.ALGJGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.KYYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.GJYLGJ))))) || (((((AppInfo.App == ConfigurationStatus.AppType.CTXGJ) || (AppInfo.App == ConfigurationStatus.AppType.KXYLGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.JCXGJ) || (AppInfo.App == ConfigurationStatus.AppType.ZLJGJ))) || (((AppInfo.App == ConfigurationStatus.AppType.HCYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.SSHCGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.HCZXGJ) || (AppInfo.App == ConfigurationStatus.AppType.BHGJ)))) || ((((AppInfo.App == ConfigurationStatus.AppType.XDBGJ) || (AppInfo.App == ConfigurationStatus.AppType.DYGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.HONDGJ) || (AppInfo.App == ConfigurationStatus.AppType.QFGJ))) || (((AppInfo.App == ConfigurationStatus.AppType.XINCGJ) || (AppInfo.App == ConfigurationStatus.AppType.CYYLGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.BLGJ) || (AppInfo.App == ConfigurationStatus.AppType.WCGJ))))))) || (((((AppInfo.App == ConfigurationStatus.AppType.XHHCGJ) || (AppInfo.App == ConfigurationStatus.AppType.MCGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.YHSGGJ) || (AppInfo.App == ConfigurationStatus.AppType.QQGJ))) || (((AppInfo.App == ConfigurationStatus.AppType.XGLLGJ) || (AppInfo.App == ConfigurationStatus.AppType.HENDGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.XTYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.TYGJ)))) || ((((AppInfo.App == ConfigurationStatus.AppType.XWYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.TIYUGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.ZBYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.FNYXGJ))) || (((AppInfo.App == ConfigurationStatus.AppType.QFZXGJ) || (AppInfo.App == ConfigurationStatus.AppType.ZBEIGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.JXINGJ) || (AppInfo.App == ConfigurationStatus.AppType.XQYLGJ)))))) || (AppInfo.App == ConfigurationStatus.AppType.JLGJ))
            {
                AppInfo.IsViewLogin = false;
            }
            if (((((((((AppInfo.App != ConfigurationStatus.AppType.DEBUG) && (AppInfo.App != ConfigurationStatus.AppType.HSGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.THGJ) && (AppInfo.App != ConfigurationStatus.AppType.DAZGJ))) && (((AppInfo.App != ConfigurationStatus.AppType.CAIHGJ) && (AppInfo.App != ConfigurationStatus.AppType.HZGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.NBGJ) && (AppInfo.App != ConfigurationStatus.AppType.HNYLGJ)))) && ((((AppInfo.App != ConfigurationStatus.AppType.SLTHGJ) && (AppInfo.App != ConfigurationStatus.AppType.FEICGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.RDYLGJ) && (AppInfo.App != ConfigurationStatus.AppType.WMGJ))) && (((AppInfo.App != ConfigurationStatus.AppType.K3GJ) && (AppInfo.App != ConfigurationStatus.AppType.SIJIGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.JFYLGJ) && (AppInfo.App != ConfigurationStatus.AppType.WSGJ))))) && (((((AppInfo.App != ConfigurationStatus.AppType.FLCGJ) && (AppInfo.App != ConfigurationStatus.AppType.SKYGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.CAITTGJ) && (AppInfo.App != ConfigurationStatus.AppType.JHCGJ))) && (((AppInfo.App != ConfigurationStatus.AppType.KSGJ) && (AppInfo.App != ConfigurationStatus.AppType.WDCD)) && ((AppInfo.App != ConfigurationStatus.AppType.MZCGJ) && (AppInfo.App != ConfigurationStatus.AppType.LDGJ)))) && ((((AppInfo.App != ConfigurationStatus.AppType.WHCGJ) && (AppInfo.App != ConfigurationStatus.AppType.ZYLGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.CBLGJ) && (AppInfo.App != ConfigurationStatus.AppType.CLYLGJ))) && (((AppInfo.App != ConfigurationStatus.AppType.THEN) && (AppInfo.App != ConfigurationStatus.AppType.LDYLGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.MTYLGJ) && (AppInfo.App != ConfigurationStatus.AppType.HENRGJ)))))) && ((((((AppInfo.App != ConfigurationStatus.AppType.LGZXGJ) && (AppInfo.App != ConfigurationStatus.AppType.WHEN)) && ((AppInfo.App != ConfigurationStatus.AppType.LYSGJ) && (AppInfo.App != ConfigurationStatus.AppType.JHC2GJ))) && (((AppInfo.App != ConfigurationStatus.AppType.HUIZGJ) && (AppInfo.App != ConfigurationStatus.AppType.HDYLGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.ALGJGJ) && (AppInfo.App != ConfigurationStatus.AppType.KYYLGJ)))) && ((((AppInfo.App != ConfigurationStatus.AppType.GJYLGJ) && (AppInfo.App != ConfigurationStatus.AppType.CCGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.CTXGJ) && (AppInfo.App != ConfigurationStatus.AppType.QJCGJ))) && (((AppInfo.App != ConfigurationStatus.AppType.KXYLGJ) && (AppInfo.App != ConfigurationStatus.AppType.JCXGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.ZLJGJ) && (AppInfo.App != ConfigurationStatus.AppType.LSWJSGJ))))) && (((((AppInfo.App != ConfigurationStatus.AppType.HCYLGJ) && (AppInfo.App != ConfigurationStatus.AppType.SSHCGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.DYCT) && (AppInfo.App != ConfigurationStatus.AppType.XHSD))) && (((AppInfo.App != ConfigurationStatus.AppType.HCZXGJ) && (AppInfo.App != ConfigurationStatus.AppType.BHGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.XDBGJ) && (AppInfo.App != ConfigurationStatus.AppType.DJGJ)))) && ((((AppInfo.App != ConfigurationStatus.AppType.DYGJ) && (AppInfo.App != ConfigurationStatus.AppType.WDGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.HONDGJ) && (AppInfo.App != ConfigurationStatus.AppType.QFGJ))) && (((AppInfo.App != ConfigurationStatus.AppType.FCGJ) && (AppInfo.App != ConfigurationStatus.AppType.TYGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.AMGJ) && (AppInfo.App != ConfigurationStatus.AppType.JXGJ))))))) && (((((((AppInfo.App != ConfigurationStatus.AppType.XINCGJ) && (AppInfo.App != ConfigurationStatus.AppType.YHGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.CYYLGJ) && (AppInfo.App != ConfigurationStatus.AppType.BLGJ))) && (((AppInfo.App != ConfigurationStatus.AppType.YBGJ) && (AppInfo.App != ConfigurationStatus.AppType.JYGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.WCGJ) && (AppInfo.App != ConfigurationStatus.AppType.WYGJ)))) && ((((AppInfo.App != ConfigurationStatus.AppType.XHHCGJ) && (AppInfo.App != ConfigurationStatus.AppType.NBAGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.MCGJ) && (AppInfo.App != ConfigurationStatus.AppType.MXGJ))) && (((AppInfo.App != ConfigurationStatus.AppType.WCAIGJ) && (AppInfo.App != ConfigurationStatus.AppType.QQGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.QIQUGJ) && (AppInfo.App != ConfigurationStatus.AppType.YHSGGJ))))) && (((((AppInfo.App != ConfigurationStatus.AppType.XGLLGJ) && (AppInfo.App != ConfigurationStatus.AppType.HENDGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.DEJIGJ) && (AppInfo.App != ConfigurationStatus.AppType.JLGJ))) && (((AppInfo.App != ConfigurationStatus.AppType.XTYLGJ) && (AppInfo.App != ConfigurationStatus.AppType.XWYLGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.B6YLGJ) && (AppInfo.App != ConfigurationStatus.AppType.TBYLGJ)))) && ((((AppInfo.App != ConfigurationStatus.AppType.WZGJ) && (AppInfo.App != ConfigurationStatus.AppType.YZCPGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.TIYUGJ) && (AppInfo.App != ConfigurationStatus.AppType.YCYLGJ))) && (((AppInfo.App != ConfigurationStatus.AppType.ZBYLGJ) && (AppInfo.App != ConfigurationStatus.AppType.FNYXGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.HUAYGJ) && (AppInfo.App != ConfigurationStatus.AppType.YXZXGJ)))))) && ((((AppInfo.App != ConfigurationStatus.AppType.WTYLGJ) && (AppInfo.App != ConfigurationStatus.AppType.TCYLGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.QFZXGJ) && (AppInfo.App != ConfigurationStatus.AppType.ZBEIGJ))) && (AppInfo.App != ConfigurationStatus.AppType.JXINGJ)))) && (AppInfo.App != ConfigurationStatus.AppType.XQYLGJ))
            {
                AppInfo.IsCheckTXFFC = false;
            }
            if ((((((AppInfo.App == ConfigurationStatus.AppType.CSCGJ) || (AppInfo.App == ConfigurationStatus.AppType.ZZJT)) || ((AppInfo.App == ConfigurationStatus.AppType.LHLM) || (AppInfo.App == ConfigurationStatus.AppType.NRLM))) || (((AppInfo.App == ConfigurationStatus.AppType.HRCP) || (AppInfo.App == ConfigurationStatus.AppType.MDLM)) || ((AppInfo.App == ConfigurationStatus.AppType.JDGJ) || (AppInfo.App == ConfigurationStatus.AppType.SIJIGJ)))) || ((((AppInfo.App == ConfigurationStatus.AppType.JCLM3) || (AppInfo.App == ConfigurationStatus.AppType.YDGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.BNGJ) || (AppInfo.App == ConfigurationStatus.AppType.BMGJ))) || (AppInfo.App == ConfigurationStatus.AppType.YFENG))) || (AppInfo.App == ConfigurationStatus.AppType.DPCGJ))
            {
                AppInfo.IsViewPeople = true;
            }
            if (AppInfo.cAppName.Contains("CXG"))
            {
                AppInfo.IsCXG = true;
            }
        }

        private void AutoBetsMain(bool pIsHint = true)
        {
            string betsBeginHint;
            ConfigurationStatus.AutoBets pBets = this.GetBets("");
            if (!pBets.PlanRun)
            {
                betsBeginHint = "";
                if (AppInfo.Account.Configuration.BetsBeginHint != "")
                {
                    betsBeginHint = AppInfo.Account.Configuration.BetsBeginHint;
                }
                if (((!pIsHint || (betsBeginHint == "")) || CommFunc.AgreeMessage(betsBeginHint, false, MessageBoxIcon.Asterisk, "")) && this.CheckUserLogin(true))
                {
                    pBets.DefaultOption(true);
                    if (this.SetBetsInfo(pBets))
                    {
                        this.Btn_Bets.Enabled = false;
                        this.AutoBetsThread(pBets);
                    }
                }
            }
            else
            {
                betsBeginHint = "注意：关闭自动投注隔期再次开启时软件将重置之前的方案！";
                if (AppInfo.Account.Configuration.BetsEndHint != "")
                {
                    betsBeginHint = AppInfo.Account.Configuration.BetsEndHint;
                }
                if (!(pIsHint && !CommFunc.AgreeMessage(betsBeginHint, false, MessageBoxIcon.Asterisk, "")))
                {
                    this.CloseBetsMain(pBets);
                    this.SavePlanListData(true);
                    this.RefreshControl(true);
                }
            }
        }

        private void AutoBetsThread(ConfigurationStatus.AutoBets pBets)
        {
            string name = pBets.Name;
            Thread thread = null;
            if (this.mainThread.BetsThreadDic.ContainsKey(name))
            {
                this.CloseBetsThread(name);
            }
            pBets.StartBets = true;
            ParameterizedThreadStart start = new ParameterizedThreadStart(this.StartBetsMain);
            thread = new Thread(start) {
                IsBackground = true
            };
            this.mainThread.BetsThreadDic[name] = thread;
            thread.Start(pBets);
        }

        private void AutoBetsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.Ckb_CloseMin.Checked)
                {
                    this.StartBackGround();
                    e.Cancel = true;
                }
                else
                {
                    this.CloseFormMain(false, true);
                }
            }
            catch
            {
            }
        }

        private void AutoBetsWindow_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckOpenIsRight())
                {
                    string xHintStr = "缺少程序运行的文件，请检查以下2点：\r\n1.如果你没有解压运行，则右击整个压缩包选择【解压到...】。\r\n2.如果你已经解压成功，请在文件夹目录里面打开【挂机EXE文件】，不要将【挂机EXE文件】复制到桌面或者其他地方。";
                    CommFunc.PublicMessage(xHintStr, MessageBoxIcon.Asterisk, "");
                    this.CloseFormMain(false, false);
                    return;
                }
                if (!CommFunc.LoadConfiguration())
                {
                    this.CloseFormMain(false, false);
                    return;
                }
                if (AppInfo.Account.Configuration.SwitchHint != "")
                {
                    CommFunc.PublicMessageAll(AppInfo.Account.Configuration.SwitchHint, true, MessageBoxIcon.Asterisk, "");
                    this.CloseFormMain(false, false);
                    if (AppInfo.Account.Configuration.SwitchUrl != "")
                    {
                        CommFunc.OpenWeb(AppInfo.Account.Configuration.SwitchUrl);
                    }
                    return;
                }
                if (AppInfo.Account.Configuration.StopLogin)
                {
                    string stopLoginHint = AppInfo.Account.Configuration.StopLoginHint;
                    if (stopLoginHint == "")
                    {
                        stopLoginHint = "系统维护中，请稍后在使用！";
                    }
                    CommFunc.PublicMessageAll(stopLoginHint, true, MessageBoxIcon.Asterisk, "");
                    this.CloseFormMain(false, false);
                    return;
                }
                this.Txt_AppName.Text = AppInfo.Account.Configuration.AppTag;
                this.AppConfig();
                if (AppInfo.App != ConfigurationStatus.AppType.DEBUG)
                {
                    /*if (CommFunc.CheckUpdateAppl())
                    {
                        CommFunc.PublicMessageAll("检测到有新版本，请下载使用！", true, MessageBoxIcon.Asterisk, "");
                        CommFunc.UpdateApp(AppInfo.Account.AppPerName);
                        this.CloseFormMain(false, false);
                        return;
                    }*/
                    if (AppInfo.App == ConfigurationStatus.AppType.CSCGJ)
                    {
                        AppInfo.Account.Configuration.IsWJApp = true;
                    }
                    if (AppInfo.Account.Configuration.IsWJApp)
                    {
                        if (this.Args.Length == 0)
                        {
                            this.CloseFormMain(false, false);
                            return;
                        }
                        this.Args = this.Args[0].Split(new char[] { '|' });
                        this.Tsp_LoginValue.Text = AppInfo.Account.ID = this.Args[0];
                        if (this.Args.Length == 2)
                        {
                            this.SelectPT = this.Args[1];
                        }
                    }
                    else if (!AppInfo.IsViewLogin)
                    {
                        this.Tsp_LoginKey.Visible = this.Tsp_LoginValue.Visible = false;
                        AppInfo.Account.LoginStatus = true;
                    }
                    else if (!this.WebLoginMain())
                    {
                        this.CloseFormMain(false, false);
                        return;
                    }
                    string str3 = AppInfo.Account.Configuration.ViewAppName ? "永信在线挂机软件" : "";
                    this.AppName = str3 + $"V{"1.0.4"}";
                    if ((AppInfo.Account.ID != "") && (AppInfo.Account.ActiveTimeString != ""))
                    {
                        this.AppName = this.AppName + $"     授权至：{AppInfo.Account.ID}  {AppInfo.Account.ActiveTimeString}";
                    }
                    this.Ckb_AppName_Click(null, null);
                }
                this.LoadDataGridView();
                this.LoadMainControl();
                this.SetControlInfoByReg();
                DebugLog.ClearLogList();
                this.LoadLotteryInfo();
                this.LoadConfigurationThread();
                this.SetPTLoginHint();
                this.SetTabIndex();
                this._RunEvent = true;
            }
            catch (Exception exception)
            {
                DebugLog.SaveDebug(exception, "");
            }
            base.Opacity = 1.0;
            AppInfo.IsAppLoaded = true;
            this.Ckb_Login.Focus();
        }

        private void BeautifyInterface()
        {
            List<Label> list = new List<Label> {
                this.Lbl_CurrentCode1,
                this.Lbl_CurrentCode2,
                this.Lbl_CurrentCode3,
                this.Lbl_CurrentCode4,
                this.Lbl_CurrentCode5
            };
            List<Label> list2 = new List<Label> {
                this.Lbl_CurrentCode6,
                this.Lbl_CurrentCode7,
                this.Lbl_CurrentCode8,
                this.Lbl_CurrentCode9,
                this.Lbl_CurrentCode10
            };
            if ((AppInfo.App == ConfigurationStatus.AppType.TCYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.JFYLGJ))
            {
                foreach (Label label in list2)
                {
                    label.Image = AppInfo.CodeImage1;
                }
            }
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Pnl_InfoRight,
                    this.Stp_Hint,
                    this.Pnl_AppName,
                    this.Pnl_DataBottom1,
                    this.Pnl_PTRefresh,
                    this.Pnl_PlanListTop,
                    this.Pnl_PlanListBottom,
                    this.Pnl_SchemeTop1,
                    this.Pnl_SchemeTop2,
                    this.Pnl_LSDataTop,
                    this.Pnl_TJDataTop,
                    this.Pnl_TJDataTop2,
                    this.Btn_TJTop,
                    this.Pnl_FNBottom,
                    this.Pnl_TimesBottom,
                    this.Btn_LSRefresh,
                    this.Btn_TJRefresh
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list4 = new List<Control> {
                    this.Pnl_NextExpect,
                    this.Pnl_CurrentCode1,
                    this.Pnl_LTUserInfoTop,
                    this.Pnl_UserLogin1,
                    this.Pnl_UserLogin2,
                    this.Pnl_OpenData,
                    this.Btn_ViewTop,
                    this.Pnl_BetsInfoTop,
                    this.Lbl_FNEncrypt,
                    this.FN_DMLH,
                    this.FN_GDQM,
                    this.FN_GJDMLH,
                    this.FN_GJKMTM,
                    this.FN_KMTM,
                    this.FN_LRWCH,
                    this.FN_BCFCH,
                    this.FN_SJCH,
                    this.FN_WJJH,
                    this.FN_YLCH,
                    this.FN_LHKMTM
                };
                CommFunc.SetControlBackColor(list4, AppInfo.beaBackColor);
                List<Control> list5 = new List<Control> {
                    this.Lbl_NextTime,
                    this.Lbl_Lottery,
                    this.Lbl_CurrentCode1,
                    this.Lbl_CurrentCode2,
                    this.Lbl_CurrentCode3,
                    this.Lbl_CurrentCode4,
                    this.Lbl_CurrentCode5,
                    this.Ckb_OpenHint,
                    this.Ckb_CloseMin,
                    this.Ckb_LeftInfo,
                    this.Ckb_RrfreshPT,
                    this.Ckb_Data,
                    this.Ckb_PlaySound,
                    this.Ckb_OpenHint,
                    this.Ckb_CloseMin,
                    this.Lbl_AppName,
                    this.Lbl_Lottery,
                    this.Lbl_LoginPT,
                    this.Lbl_ID,
                    this.Lbl_IDHint,
                    this.Lbl_PW,
                    this.Lbl_AppName,
                    this.Lbl_BetsExpectKey,
                    this.Lbl_IDKey,
                    this.Lbl_BankBalanceKey,
                    this.Lbl_StopBets,
                    this.Lbl_YLStopBets,
                    this.Lbl_KSStopBets,
                    this.Ckb_RrfreshPTLine,
                    this.Ckb_AddLine,
                    this.Ckb_DeleteLine,
                    this.Ckb_RefreshUser,
                    this.Ckb_Login,
                    this.Ckb_ClearBetsList,
                    this.Ckb_MNBets,
                    this.Ckb_MN1,
                    this.Ckb_MN3,
                    this.Lbl_MNBets,
                    this.Ckb_MN2,
                    this.Ckb_MN4,
                    this.Ckb_SBStopBets,
                    this.Ckb_DQStopBets,
                    this.Ckb_BetsBeginTime,
                    this.Ckb_BetsEndTime,
                    this.Lbl_BetsTime1,
                    this.Lbl_BetsTime2,
                    this.Lbl_BetsGainPlanKey,
                    this.Lbl_BetsMoneyPlanKey,
                    this.Lbl_MNBetsGainPlanKey,
                    this.Lbl_MNBetsMoneyPlanKey,
                    this.Lbl_LZMaxKey,
                    this.Lbl_LGMaxKey,
                    this.Lbl_ZQLKey,
                    this.Lbl_BetsCountKey,
                    this.Lbl_BetsKey,
                    this.Ckb_DeleteExpect,
                    this.Ckb_FNLT,
                    this.Lbl_FNName,
                    this.Lbl_FNCHType,
                    this.Lbl_LSLotteryKey,
                    this.Lbl_LSFN,
                    this.Ckb_LSAutoRefresh,
                    this.Lbl_LSPlayKey,
                    this.Lbl_LSDataRange,
                    this.Ckb_LSBJ,
                    this.Rdb_LSBJType,
                    this.Rdb_LSBJExpect,
                    this.Lbl_LSBJExpect2,
                    this.Lbl_LSRefreshHint,
                    this.Lbl_TJLotteryKey,
                    this.Lbl_TJFN,
                    this.Lbl_TJPrize,
                    this.Lbl_TJPlayKey,
                    this.Lbl_TJDataRange,
                    this.Lbl_TJData,
                    this.Ckb_TJTimeRange,
                    this.Lbl_TJTime,
                    this.Ckb_TJReset,
                    this.Lbl_TJRefreshHint,
                    this.Lbl_TJFindXS,
                    this.Lbl_BTFNEdit,
                    this.Ckb_BTFNEditSkip,
                    this.Pnl_TJDataHint,
                    this.Btn_LSRefresh,
                    this.Btn_TJRefresh,
                    this.Ckb_AddScheme,
                    this.Ckb_CopyScheme,
                    this.Ckb_DeleteScheme,
                    this.Ckb_EditTimesPlan,
                    this.Ckb_EditScheme,
                    this.Ckb_SaveScheme,
                    this.Ckb_CancelScheme,
                    this.Ckb_LSStop,
                    this.Ckb_TJStop,
                    this.Ckb_AutoSizeTJ,
                    this.Ckb_TJFindXS,
                    this.Ckb_AddBTFN,
                    this.Ckb_DeleteBTFN,
                    this.Ckb_BTFNEdit,
                    this.Ckb_SaveTimes,
                    this.Ckb_TBCount,
                    this.Ckb_AddTimes,
                    this.Ckb_EditTimes,
                    this.Ckb_DeleteTimes,
                    this.Ckb_ClearTimes,
                    this.Ckb_AppName,
                    this.Lbl_FNEncrypt,
                    this.Ckb_PWPaste,
                    this.Ckb_PWClear,
                    this.Ckb_ShowHideUser,
                    this.Ckb_ImportScheme,
                    this.Ckb_ExportScheme,
                    this.Ckb_ClearScheme,
                    this.Ckb_ShareSchemeManage,
                    this.Ckb_ShareScheme,
                    this.Ckb_ShareBetsManage
                };
                CommFunc.SetControlForeColor(list5, AppInfo.whiteColor);
                List<Control> list6 = new List<Control> {
                    this.Lbl_LoginHint
                };
                CommFunc.SetControlForeColor(list6, AppInfo.appForeColor);
                List<TabControl> pTabControlList = new List<TabControl> {
                    this.Tab_Main
                };
                CommFunc.BeautifyTabControl(pTabControlList);
                List<ComboBox> pComboBoxList = new List<ComboBox> {
                    this.Cbb_LoginPT,
                    this.Cbb_Lottery,
                    this.Cbb_BetsEndType,
                    this.Cbb_FNCHType,
                    this.Cbb_LSFN,
                    this.Cbb_LSBJType,
                    this.Cbb_TJFN,
                    this.Cbb_TJPrize,
                    this.Cbb_BTFNEdit
                };
                CommFunc.BeautifyComboBox(pComboBoxList);
                this.Tsp_PeopleKey.ForeColor = this.Tsp_LoginKey.ForeColor = AppInfo.whiteColor;
                this.Lbl_Web.BackColor = AppInfo.appBackColor;
                this.Lbl_Web.LinkColor = AppInfo.appForeColor;
            }
        }

        private bool BetsMain1(ConfigurationStatus.AutoBets pBets)
        {
            pBets.SBStopBets = this.Ckb_SBStopBets.Checked;
            pBets.DQStopBets = this.Ckb_DQStopBets.Checked;
            if (pBets.ShareBetsInfo.BetsTypeInfo != ConfigurationStatus.BetsType.FollowBets)
            {
                if (!this.CountBetsScheme(pBets))
                {
                    return false;
                }
                foreach (string str in pBets.BetsSchemeDic.Keys)
                {
                    ConfigurationStatus.BetsScheme scheme = pBets.BetsSchemeDic[str];
                    ConfigurationStatus.LSData lSDataInfo = this.GetLSDataInfo();
                    lSDataInfo.SchemeInfo = scheme.SchemeInfo;
                    lSDataInfo.RefreshControl = false;
                    scheme.LSDataInfo = lSDataInfo;
                }
            }
            if (pBets.DQStopBets)
            {
                if (!pBets.IsDQ)
                {
                    bool flag = CommFunc.CountNextExpect(this.GetOpenDataSecondExpectString, "") != this.GetOpenDataFirstExpectString;
                    bool flag2 = CommFunc.CountNextExpect(this.GetOpenDataFirstExpectString, "") != pBets.Expect;
                    if (flag || flag2)
                    {
                        pBets.IsDQ = true;
                    }
                }
                if (pBets.IsDQ)
                {
                    pBets.ErrorState = $"{this.GetOpenDataFirstExpectString}期出现断期";
                    return false;
                }
            }
            if (this.Ckb_BetsEndTime.Checked)
            {
                pBets.EndTime = this.Dtp_BetsEndTime.Value.ToShortTimeString();
                pBets.EndType = (ConfigurationStatus.StopBetsType) this.Cbb_BetsEndType.SelectedIndex;
            }
            pBets.IsSelectMN = this.Ckb_MNBets.Checked;
            if (!pBets.IsSelectMN)
            {
                if (this.Ckb_MN1.Checked)
                {
                    pBets.ZSBetsMoney1 = this.Txt_MN1.Text;
                }
                if (this.Ckb_MN2.Checked)
                {
                    pBets.MNBetsMoney1 = this.Txt_MN2.Text;
                }
                if (this.Ckb_MN3.Checked)
                {
                    pBets.ZSBetsMoney2 = this.Txt_MN3.Text;
                }
                if (this.Ckb_MN4.Checked)
                {
                    pBets.MNBetsMoney2 = this.Txt_MN4.Text;
                }
            }
            pBets.YLStopMoney = this.Txt_YLStopBets.Text;
            pBets.KSStopMoney = this.Txt_KSStopBets.Text;
            pBets.YKZS = ConfigurationStatus.BetsYKZS.AppGain;
            bool flag3 = true;
            DateTime now = DateTime.Now;
            if (pBets.EndTime != "")
            {
                DateTime time2 = DateTime.Parse(pBets.EndTime);
                if ((now >= time2) && (pBets.EndType == ConfigurationStatus.StopBetsType.AddFN))
                {
                    pBets.IsStopAddFN = true;
                }
            }
            pBets.IsBetsTime = flag3;
            return flag3;
        }

        private void BetsMain2(ConfigurationStatus.AutoBets pBets)
        {
            ConfigurationStatus.SCPlan current;
            ConfigurationStatus.BetsScheme schemeInfo;
            ConfigurationStatus.SCPlan getLastPlan;
            string prize;
            string expect = pBets.Expect;
            List<ConfigurationStatus.SCPlan> planList = new List<ConfigurationStatus.SCPlan>();
            List<string> list2 = new List<string>();
            if (pBets.ShareBetsInfo.BetsTypeInfo == ConfigurationStatus.BetsType.FollowBets)
            {
                int num;
                planList = pBets.ShareBetsInfo.FollowPlanList;
                if (pBets.ShareBetsInfo.FollowErrorIndexList.Count != 0)
                {
                    pBets.ShareBetsInfo.FollowErrorHint = "";
                    for (num = pBets.ShareBetsInfo.FollowErrorIndexList.Count - 1; num >= 0; num--)
                    {
                        int pIndex = pBets.ShareBetsInfo.FollowErrorIndexList[num];
                        this.GetSharePlanListByIndex(pBets, planList, pIndex);
                    }
                }
                else if (!pBets.ShareBetsInfo.FollowYes)
                {
                    int pCount = -1;
                    pBets.ShareBetsInfo.FollowErrorHint = "";
                    if (SQLData.GetSharePlan(pBets, ref pCount, ref pBets.ShareBetsInfo.FollowErrorHint))
                    {
                        int num4 = planList.Count;
                        for (num = pCount - 1; num >= num4; num--)
                        {
                            this.GetSharePlanListByIndex(pBets, planList, num);
                        }
                    }
                }
                using (List<ConfigurationStatus.SCPlan>.Enumerator enumerator = planList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        schemeInfo = current.SchemeInfo;
                        if (!schemeInfo.ExpectList.Contains(expect))
                        {
                            schemeInfo.IsBetsYes = false;
                            getLastPlan = pBets.GetLastPlan;
                            current.UploadTime = DateTime.Now;
                            current.Gain = 0.0;
                            current.State = current.WaitLottery;
                            current.Rebate = Convert.ToDouble(AppInfo.PTInfo.Rebate);
                            if (getLastPlan != null)
                            {
                                current.FNGainString = getLastPlan.FNGainString;
                                current.FNMNGainString = getLastPlan.FNMNGainString;
                                current.FNMoneyString = getLastPlan.FNMoneyString;
                                current.FNMNMoneyString = getLastPlan.FNMNMoneyString;
                            }
                            if (pBets.IsSelectMN)
                            {
                                current.IsMNBets = true;
                            }
                            prize = AppInfo.PTInfo.GetPrize(current.Type, current.Play);
                            if (prize == "")
                            {
                                pBets.ErrorState = $"没有找到玩法({current.ViewPlay})的赔率";
                            }
                            else
                            {
                                if (AppInfo.PTInfo.GetPlayMethodID(current.Type, current.Play) == "")
                                {
                                    pBets.ErrorState = $"平台不支持玩法({current.ViewPlay})";
                                    continue;
                                }
                                current.Mode = Convert.ToDouble(prize) / Math.Pow(10.0, (double) (current.Unit - 1));
                                if (AppInfo.PTInfo.IsBetsMoney1(current.UnitType))
                                {
                                    current.Mode /= 2.0;
                                }
                                current.ReplaceNumberListMode();
                                pBets.BetsNumber += current.Number;
                                if (!this.PTBetsMain(pBets, current, schemeInfo))
                                {
                                    if (schemeInfo.IsQQG)
                                    {
                                        this.DeleteQQGData(schemeInfo, expect);
                                    }
                                    list2.Add(current.FNName);
                                    continue;
                                }
                                if (!AppInfo.PTInfo.IsCombinaBets)
                                {
                                    if (!schemeInfo.IsBetsYes)
                                    {
                                        pBets.PlanList.Add(current);
                                        string currentExpect = current.CurrentExpect;
                                        if (!pBets.ShareBetsInfo.FollowPlanCountDic.ContainsKey(currentExpect))
                                        {
                                            pBets.ShareBetsInfo.FollowPlanCountDic[currentExpect] = 1;
                                        }
                                        else
                                        {
                                            Dictionary<string, int> dictionary;
                                            string str9;
                                            (dictionary = pBets.ShareBetsInfo.FollowPlanCountDic)[str9 = currentExpect] = dictionary[str9] + 1;
                                        }
                                    }
                                    schemeInfo.IsBetsYes = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (string str4 in pBets.BetsSchemeDic.Keys)
                {
                    schemeInfo = pBets.BetsSchemeDic[str4];
                    if (!schemeInfo.ExpectList.Contains(expect))
                    {
                        schemeInfo.IsBetsYes = false;
                        current = new ConfigurationStatus.SCPlan();
                        getLastPlan = pBets.GetLastPlan;
                        planList.Add(current);
                        current.SchemeInfo = schemeInfo;
                        current.FNName = schemeInfo.SchemeInfo.Name;
                        current.FNCHType = schemeInfo.SchemeInfo.CHType;
                        current.UploadTime = DateTime.Now;
                        current.LotteryName = pBets.LotteryName;
                        current.LotteryID = pBets.LotteryID;
                        current.BeginExpect = current.EndExpect = expect;
                        current.PlayType = schemeInfo.PlayType;
                        current.PlayName = schemeInfo.PlayName;
                        current.RXWZ = schemeInfo.RXWZ;
                        current.RXZJ = schemeInfo.RXZJ;
                        current.Gain = 0.0;
                        current.State = current.WaitLottery;
                        current.Rebate = Convert.ToDouble(AppInfo.PTInfo.Rebate);
                        if (!pBets.IsSelectMN)
                        {
                            double num5;
                            double num6;
                            ConfigurationStatus.FNBase fNBaseInfo = schemeInfo.SchemeInfo.FNBaseInfo;
                            if (schemeInfo.IsMNBets)
                            {
                                current.IsMNBets = true;
                                num5 = pBets.Gain(false);
                                if (num5 < 0.0)
                                {
                                    string str5 = pBets.ZSBetsMoney1;
                                    if (fNBaseInfo.GetZSBetsMoney1 != "")
                                    {
                                        str5 = fNBaseInfo.GetZSBetsMoney1;
                                    }
                                    if (str5 != "")
                                    {
                                        num6 = Convert.ToDouble(str5);
                                        if (Math.Abs(num5) >= num6)
                                        {
                                            current.IsMNBets = false;
                                            schemeInfo.IsMNBets = false;
                                            pBets.DefaultTimes(false);
                                        }
                                    }
                                }
                                else
                                {
                                    string str6 = pBets.ZSBetsMoney2;
                                    if (fNBaseInfo.GetZSBetsMoney2 != "")
                                    {
                                        str6 = fNBaseInfo.GetZSBetsMoney2;
                                    }
                                    if (str6 != "")
                                    {
                                        num6 = Convert.ToDouble(str6);
                                        if (num5 >= num6)
                                        {
                                            current.IsMNBets = false;
                                            schemeInfo.IsMNBets = false;
                                            pBets.DefaultTimes(false);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                num5 = pBets.Gain(false);
                                if (num5 < 0.0)
                                {
                                    string str7 = pBets.MNBetsMoney2;
                                    if (fNBaseInfo.GetMNBetsMoney2 != "")
                                    {
                                        str7 = fNBaseInfo.GetMNBetsMoney2;
                                    }
                                    if (str7 != "")
                                    {
                                        num6 = Convert.ToDouble(str7);
                                        if (Math.Abs(num5) >= num6)
                                        {
                                            current.IsMNBets = true;
                                            schemeInfo.IsMNBets = true;
                                            pBets.DefaultTimes(false);
                                        }
                                    }
                                }
                                else
                                {
                                    string str8 = pBets.MNBetsMoney1;
                                    if (fNBaseInfo.GetMNBetsMoney1 != "")
                                    {
                                        str8 = fNBaseInfo.GetMNBetsMoney1;
                                    }
                                    if (str8 != "")
                                    {
                                        num6 = Convert.ToDouble(str8);
                                        if (num5 >= num6)
                                        {
                                            current.IsMNBets = true;
                                            schemeInfo.IsMNBets = true;
                                            pBets.DefaultTimes(false);
                                        }
                                    }
                                }
                            }
                        }
                        ConfigurationStatus.SCTimesCount times = schemeInfo.Times;
                        current.Unit = times.UnitIndex;
                        current.UnitType = times.Unit;
                        current.Money = this.GetBetMoney(times.Unit);
                        current.Cycle = 1;
                        current.CurrentCycle = 1;
                        current.TimesType = times.BTType;
                        current.TimesDic = times.GetBetsTimesList;
                        current.FBDic = times.FBList;
                        current.BTFNName = times.FNName;
                        if (getLastPlan != null)
                        {
                            current.FNGainString = getLastPlan.FNGainString;
                            current.FNMNGainString = getLastPlan.FNMNGainString;
                            current.FNMoneyString = getLastPlan.FNMoneyString;
                            current.FNMNMoneyString = getLastPlan.FNMNMoneyString;
                        }
                        if (pBets.IsSelectMN)
                        {
                            current.IsMNBets = true;
                        }
                        prize = AppInfo.PTInfo.GetPrize(current.Type, current.Play);
                        if (prize == "")
                        {
                            pBets.ErrorState = $"没有找到玩法({current.ViewPlay})的赔率";
                        }
                        else if (AppInfo.PTInfo.GetPlayMethodID(current.Type, current.Play) == "")
                        {
                            pBets.ErrorState = $"平台不支持玩法({current.ViewPlay})";
                        }
                        else
                        {
                            current.Mode = Convert.ToDouble(prize) / Math.Pow(10.0, (double) (current.Unit - 1));
                            if (AppInfo.PTInfo.IsBetsMoney1(current.UnitType))
                            {
                                current.Mode /= 2.0;
                            }
                            List<string> pList = new List<string>();
                            if (!this.GetPlanNumberList(pBets, current, schemeInfo, ref pList))
                            {
                                list2.Add(current.FNName);
                            }
                            else
                            {
                                current.NumberList = pList;
                                current.Number = CommFunc.GetBetsCodeCount(current.GetPTNumberList(null), current.Play, current.RXWZ);
                                pBets.BetsNumber += current.Number;
                                if (!this.PTBetsMain(pBets, current, schemeInfo))
                                {
                                    if (schemeInfo.IsQQG)
                                    {
                                        this.DeleteQQGData(schemeInfo, expect);
                                    }
                                    list2.Add(current.FNName);
                                }
                                else if (!AppInfo.PTInfo.IsCombinaBets)
                                {
                                    if (!schemeInfo.IsBetsYes)
                                    {
                                        pBets.PlanList.Add(current);
                                        if ((pBets.ShareBetsInfo.BetsTypeInfo == ConfigurationStatus.BetsType.SendBets) && !(current.AutoTotalMoney("0", true) == 0.0))
                                        {
                                            pBets.ShareBetsInfo.SendPlanList.Add(current);
                                        }
                                    }
                                    schemeInfo.IsBetsYes = true;
                                }
                            }
                        }
                    }
                }
            }
            if (AppInfo.PTInfo.IsCombinaBets)
            {
                List<ConfigurationStatus.SCPlan> list4 = new List<ConfigurationStatus.SCPlan>();
                foreach (ConfigurationStatus.SCPlan plan in planList)
                {
                    if (!(plan.SchemeInfo.IsBetsYes || list2.Contains(plan.FNName)))
                    {
                        list4.Add(plan);
                    }
                }
                if ((list4.Count > 0) && AppInfo.PTInfo.BetsMain(list4, ref pBets.PTState))
                {
                    foreach (ConfigurationStatus.SCPlan plan in planList)
                    {
                        plan.SchemeInfo.IsBetsYes = true;
                        pBets.PlanList.Add(plan);
                    }
                }
            }
            foreach (ConfigurationStatus.SCPlan plan in planList)
            {
                schemeInfo = plan.SchemeInfo;
                if (schemeInfo.IsBetsYes && !schemeInfo.ExpectList.Contains(expect))
                {
                    schemeInfo.ExpectList.Add(expect);
                }
            }
        }

        private void BTImport(string pFNName, List<string> pBTPlanList)
        {
            ConfigurationStatus.Scheme pInfo = this.GetScheme(pFNName);
            if (!this.IsEditScheme)
            {
                pInfo.FNBaseInfo.BTPlanList = pBTPlanList;
                this.SaveSchemeData(pFNName, "");
                this.RefreshScheme();
                CommFunc.PublicMessageAll($"成功将金额导入【{pFNName}】", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                CommFunc.SetTabSelectIndex(this.Tab_Main, this.Tap_Scheme.Text);
                this.SetSchemeControlTimes(pInfo, CommFunc.Join(pBTPlanList, ","));
            }
        }

        private void Btn_Bets_Click(object sender, EventArgs e)
        {
            this.AutoBetsMain(true);
        }

        private void Btn_LSRefresh_Click(object sender, EventArgs e)
        {
            if (this.Btn_LSRefresh.Enabled)
            {
                try
                {
                    string selectLSFN = this.GetSelectLSFN();
                    if (selectLSFN != "")
                    {
                        this.RefreshLSPlay();
                        ConfigurationStatus.LSData lSDataInfo = this.GetLSDataInfo();
                        lSDataInfo.SchemeInfo = this.GetScheme(selectLSFN);
                        lSDataInfo.RefreshControl = true;
                        this.Btn_LSRefresh.Enabled = false;
                        this.Lbl_LSRefreshHint.Visible = true;
                        this.Pnl_LSDataRight.Visible = true;
                        this.mainThread.refreshLSDataThread = new Thread(new ParameterizedThreadStart(AppInfo.RefreshLSData.Invoke));
                        this.mainThread.refreshLSDataThread.IsBackground = true;
                        this.mainThread.refreshLSDataThread.Start(lSDataInfo);
                    }
                }
                catch
                {
                }
            }
        }

        private void Btn_RefreshData_Click(object sender, EventArgs e)
        {
            this.FilterOpenData();
        }

        private void Btn_TJRefresh_Click(object sender, EventArgs e)
        {
            if (this.Btn_TJRefresh.Enabled)
            {
                try
                {
                    string text = this.Lbl_TJLotteryValue.Text;
                    string str2 = AppInfo.LotterNameDic[text];
                    string selectTJFN = this.GetSelectTJFN();
                    if (selectTJFN == "")
                    {
                        CommFunc.PublicMessageAll("请选择一个方案进行统计！", true, MessageBoxIcon.Asterisk, "");
                    }
                    else
                    {
                        this.RefreshTJPlay();
                        ConfigurationStatus.TJData parameter = new ConfigurationStatus.TJData {
                            LotteryName = text,
                            LotteryID = str2,
                            FNName = selectTJFN
                        };
                        if (this.Txt_TJPrize.Text == "")
                        {
                            CommFunc.PublicMessageAll("请输入一个奖金！", true, MessageBoxIcon.Asterisk, "");
                            this.Txt_TJPrize.Focus();
                        }
                        else
                        {
                            parameter.Prize = Convert.ToDouble(this.Txt_TJPrize.Text);
                            parameter.IsReset = this.Ckb_TJReset.Checked;
                            parameter.StartDate = this.Dtp_TJDataRange1.Value.Date;
                            parameter.EndDate = this.Dtp_TJDataRange2.Value.Date;
                            parameter.StartTime = this.Dtp_TJTimeRange1.Value.ToShortTimeString();
                            parameter.EndTime = this.Dtp_TJTimeRange2.Value.ToShortTimeString();
                            parameter.TimeSelect = this.Ckb_TJTimeRange.Checked;
                            parameter.LSDataInfo = this.GetLSDataInfo();
                            ConfigurationStatus.LSData lSDataInfo = this.GetLSDataInfo();
                            lSDataInfo.RefreshControl = false;
                            parameter.LSDataInfo = lSDataInfo;
                            this.Btn_TJRefresh.Enabled = false;
                            this.Pnl_TJRight2.Visible = true;
                            this.mainThread.refreshTJDataThread = new Thread(new ParameterizedThreadStart(AppInfo.RefreshTJData.Invoke));
                            this.mainThread.refreshTJDataThread.IsBackground = true;
                            this.mainThread.refreshTJDataThread.Start(parameter);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void Btn_TJTop_Click(object sender, EventArgs e)
        {
            this.Egv_TJDataList1.Visible = !this.Egv_TJDataList1.Visible;
            this.Btn_TJTop.Image = this.Egv_TJDataList1.Visible ? Resources.WindowUp : Resources.WindowDown;
            this.Tot_Hint.SetToolTip(this.Btn_TJTop, this.Egv_TJDataList1.Visible ? "隐藏" : "显示");
        }

        private void Btn_ViewTop_Click(object sender, EventArgs e)
        {
            this.Pnl_Top.Visible = !this.Pnl_Top.Visible;
            this.Btn_ViewTop.Image = this.Pnl_Top.Visible ? Resources.WindowUp : Resources.WindowDown;
            this.Tot_Hint.SetToolTip(this.Btn_ViewTop, this.Egv_TJDataList1.Visible ? "隐藏" : "显示");
        }

        private void Cbb_FNCHType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._RunEvent && this.IsEditScheme)
            {
                ConfigurationStatus.Scheme scheme = this.GetScheme("");
                scheme.CHType = this.Cbb_FNCHType.Text;
                scheme = new ConfigurationStatus.Scheme(false, scheme.Name, scheme.CHType, "", false, false);
                AppInfo.Account.SchemeDic[scheme.Name] = scheme;
                this.SaveSchemeData(scheme.Name, "");
                this.RefreshScheme();
            }
        }

        private void Cbb_LoginPT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                string text = this.Cbb_Lottery.Text;
                this.LoadPTLine(this.PTName);
                CommFunc.SetComboBoxSelectedIndex(this.Cbb_Lottery, text);
                this.SetPTLoginHint();
            }
        }

        private void Cbb_Lottery_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                if (AppInfo.Current.Lottery != null)
                {
                    AppInfo.Current.Lottery.IsLoadServerData = false;
                }
                this.ColsingCheck();
                this.LoadLotteryInfo();
            }
        }

        private void Cbb_LSFN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
            }
        }

        private void Cbb_TJFN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshTJPlay();
            }
        }

        private void Cbb_TJPrize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshTJPrize();
            }
        }

        private void ChangeUserLoginStatus()
        {
            bool flag = this._RunEvent;
            this._RunEvent = false;
            try
            {
                if (AppInfo.PTInfo.PTLoginStatus)
                {
                    this.Pnl_UserLogin2.Visible = false;
                    this.Pnl_UserLogin1.Visible = true;
                    this.Ckb_Login.Text = "退出登录";
                    this.Ckb_RefreshUser.Text = "刷新";
                    this.Ckb_RefreshUser.Image = Resources.Refresh;
                    this.Ckb_RefreshUser.Visible = true;
                    this.Ckb_ShowHideUser.Text = "隐藏";
                    this.Ckb_ShowHideUser.Visible = true;
                    this.Pnl_RrfreshPT.Enabled = false;
                    this.Lbl_IDValue.Text = AppInfo.Account.PTID;
                }
                else
                {
                    AppInfo.Account.LoadDefultData(false);
                    this.Pnl_UserLogin2.Visible = true;
                    this.Pnl_UserLogin1.Visible = false;
                    this.Ckb_Login.Text = "平台登录";
                    this.Ckb_RefreshUser.Text = "取消";
                    this.Ckb_RefreshUser.Image = Resources.CancelRound;
                    this.Ckb_RefreshUser.Visible = false;
                    this.Ckb_ShowHideUser.Visible = false;
                    this.Pnl_RrfreshPT.Enabled = true;
                    this.Lbl_IDValue.Text = this.Lbl_BankBalanceValue.Text = "";
                    this.Lbl_BetsCountValue.Text = "";
                    this.Lbl_BetsGainPlanValue.Text = this.Lbl_MNBetsGainPlanValue.Text = "";
                    this.Lbl_BetsMoneyPlanValue.Text = this.Lbl_MNBetsMoneyPlanValue.Text = "";
                    this.Lbl_LZMaxValue.Text = this.Lbl_LGMaxValue.Text = this.Lbl_ZQLValue.Text = "";
                }
                this.RefreshShareBetsControl();
                this.RefreshShareSchemeControl();
                this.CheckPTUserA();
            }
            catch
            {
            }
            this._RunEvent = flag;
        }

        private bool CheckBetsBeginTimes()
        {
            bool flag = false;
            if (this.Ckb_BetsBeginTime.Checked)
            {
                flag = DateTime.Now >= DateTime.Parse(this.Dtp_BetsBeginTime.Value.ToShortTimeString());
            }
            return flag;
        }

        private bool CheckBetsEndTimes()
        {
            bool flag = false;
            if (this.Ckb_BetsEndTime.Checked && (this.Cbb_BetsEndType.SelectedIndex == 0))
            {
                flag = DateTime.Now >= DateTime.Parse(this.Dtp_BetsEndTime.Value.ToShortTimeString());
            }
            return flag;
        }

        private bool CheckFBYKHT(ConfigurationStatus.BetsScheme pBetsScheme, ref string pHint)
        {
            if (pBetsScheme.SchemeInfo.FNBaseInfo.BTType == ConfigurationStatus.SCTimesType.FN)
            {
                int yLHTID;
                List<ConfigurationStatus.TimesScheme> timesSchemeList = AppInfo.BTFNDic[pBetsScheme.SchemeInfo.FNBaseInfo.BTFNName].TimesSchemeList;
                List<int> list2 = new List<int>();
                foreach (ConfigurationStatus.TimesScheme scheme in timesSchemeList)
                {
                    list2.Add(scheme.ID);
                }
                if (pBetsScheme.SchemeInfo.FNBaseInfo.GetYLHTMoney != "")
                {
                    yLHTID = pBetsScheme.SchemeInfo.FNBaseInfo.YLHTID;
                    if (!list2.Contains(yLHTID))
                    {
                        pHint = $"【{pBetsScheme.SchemeInfo.Name}】盈利跳转中局数【{yLHTID}】不存在！";
                        return false;
                    }
                }
                if (pBetsScheme.SchemeInfo.FNBaseInfo.GetKSHTMoney != "")
                {
                    yLHTID = pBetsScheme.SchemeInfo.FNBaseInfo.KSHTID;
                    if (!list2.Contains(yLHTID))
                    {
                        pHint = $"【{pBetsScheme.SchemeInfo.Name}】亏损跳转中局数【{yLHTID}】不存在！";
                        return false;
                    }
                }
            }
            return true;
        }

        private void CheckFNBetsTime(ref Dictionary<string, ConfigurationStatus.BetsCode> pFNNumberDic1, ConfigurationStatus.AutoBets pBets, ConfigurationStatus.BetsScheme pScheme)
        {
            DateTime dSJEndBetsTime;
            bool flag = true;
            DateTime now = DateTime.Now;
            if (pScheme.SchemeInfo.FNBaseInfo.BetsTimeType == ConfigurationStatus.TimeType.FW)
            {
                if (pScheme.SchemeInfo.FNBaseInfo.GetFWBeginTime != "")
                {
                    dSJEndBetsTime = DateTime.Parse(pScheme.SchemeInfo.FNBaseInfo.GetFWBeginTime);
                    if (now < dSJEndBetsTime)
                    {
                        flag = false;
                    }
                }
                if (pScheme.SchemeInfo.FNBaseInfo.GetFWEndTime != "")
                {
                    dSJEndBetsTime = DateTime.Parse(pScheme.SchemeInfo.FNBaseInfo.GetFWEndTime);
                    if (now > dSJEndBetsTime)
                    {
                        if (pScheme.SchemeInfo.FNBaseInfo.GetEndType == ConfigurationStatus.StopBetsType.AddFN)
                        {
                            pScheme.IsStopAddFN = true;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                }
            }
            else if (pScheme.SchemeInfo.FNBaseInfo.BetsTimeType == ConfigurationStatus.TimeType.DJS)
            {
                dSJEndBetsTime = pScheme.DSJEndBetsTime;
                if ((dSJEndBetsTime != DateTime.MinValue) && (now > dSJEndBetsTime))
                {
                    if (pScheme.SchemeInfo.FNBaseInfo.GetEndType == ConfigurationStatus.StopBetsType.AddFN)
                    {
                        pScheme.IsStopAddFN = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            if (!flag)
            {
                pFNNumberDic1.Clear();
            }
        }

        public bool CheckFNEncrypt(ConfigurationStatus.Scheme pInfo)
        {
            if (pInfo.IsViewFNEncrypt)
            {
                string pText = "方案已经加密，请输入密码";
                string str2 = CommFunc.GetStringFromInputBox(pText, "", "", true);
                if (str2 != AppInfo.Account.Configuration.FNEncrypt)
                {
                    if (str2 != "")
                    {
                        CommFunc.PublicMessageAll("密码不正确！", true, MessageBoxIcon.Asterisk, "");
                    }
                    return false;
                }
                pInfo.IsInputPW = true;
                return true;
            }
            return true;
        }

        private void CheckFNYKStop(ref Dictionary<string, ConfigurationStatus.BetsCode> pFNNumberDic, ConfigurationStatus.AutoBets pBets, ConfigurationStatus.BetsScheme pScheme, double pGain)
        {
            string getYLStopMoney = pScheme.SchemeInfo.FNBaseInfo.GetYLStopMoney;
            string getKSStopMoney = pScheme.SchemeInfo.FNBaseInfo.GetKSStopMoney;
            if (getYLStopMoney != "")
            {
                double num = Convert.ToDouble(getYLStopMoney);
                if ((pGain >= 0.0) && (pGain >= num))
                {
                    pBets.ErrorState = $"目前盈利不在【{pScheme.SchemeInfo.Name}】设定的投注范围内";
                    pFNNumberDic.Clear();
                }
            }
            if (getKSStopMoney != "")
            {
                double num2 = Convert.ToDouble(getKSStopMoney);
                if ((pGain < 0.0) && (Math.Abs(pGain) >= num2))
                {
                    pBets.ErrorState = $"目前亏损不在【{pScheme.SchemeInfo.Name}】设定的投注范围内";
                    pFNNumberDic.Clear();
                }
            }
        }

        public bool CheckGJBTEncrypt(ConfigurationStatus.GJBTScheme pInfo)
        {
            if (pInfo.IsViewGJBTEncrypt)
            {
                string pText = "倍投方案已经加密，请输入密码";
                string str2 = CommFunc.GetStringFromInputBox(pText, "", "", false);
                if (str2 != AppInfo.Account.Configuration.GJBTEncrypt)
                {
                    if (str2 != "")
                    {
                        CommFunc.PublicMessageAll("密码不正确！", true, MessageBoxIcon.Asterisk, "");
                    }
                    return false;
                }
                pInfo.IsInputPW = true;
                return true;
            }
            return true;
        }

        private void CheckLSDataBJ(ConfigurationStatus.LSData pinfo)
        {
            int lSBJTypeIndex = pinfo.LSBJTypeIndex;
            int bJExpectValue = -1;
            if (pinfo.BJExpectSelect)
            {
                bJExpectValue = pinfo.BJExpectValue;
            }
            else if (lSBJTypeIndex >= 4)
            {
                List<int> list = new List<int>();
                foreach (ConfigurationStatus.LSDataView view in this.LSDataViewList)
                {
                    if (lSBJTypeIndex == 4)
                    {
                        list.Add(ConfigurationStatus.LSDataView.GetNum(view.LGExpect));
                    }
                    else if (lSBJTypeIndex == 5)
                    {
                        list.Add(ConfigurationStatus.LSDataView.GetNum(view.LZExpect));
                    }
                }
                list.Sort();
                list.Reverse();
                if (list[0] == list[1])
                {
                    return;
                }
                bJExpectValue = list[0];
            }
            foreach (ConfigurationStatus.LSDataView view in this.LSDataViewList)
            {
                int num;
                int num3 = bJExpectValue;
                if (num3 == -1)
                {
                    switch (lSBJTypeIndex)
                    {
                        case 0:
                            num3 = ConfigurationStatus.LSDataView.GetNum(view.TodayExpect);
                            break;

                        case 1:
                            num3 = ConfigurationStatus.LSDataView.GetNum(view.YesterdayExpect);
                            break;

                        case 2:
                            num3 = ConfigurationStatus.LSDataView.GetNum(view.WeekExpect);
                            break;
                    }
                    num = ConfigurationStatus.LSDataView.GetNum(view.LGExpect);
                    view.IsBJ = (view.LGExpect != "") && (num >= (num3 - 1));
                }
                else if (pinfo.BJExpectSelect)
                {
                    num = ConfigurationStatus.LSDataView.GetNum(view.LGExpect);
                    view.IsBJ = (view.LGExpect != "") && (num >= (num3 - 1));
                }
                else
                {
                    switch (lSBJTypeIndex)
                    {
                        case 4:
                            num = ConfigurationStatus.LSDataView.GetNum(view.LGExpect);
                            view.IsBJ = (view.LGExpect != "") && (num == num3);
                            break;

                        case 5:
                            num = ConfigurationStatus.LSDataView.GetNum(view.LZExpect);
                            view.IsBJ = (view.LZExpect != "") && (num == num3);
                            break;
                    }
                }
            }
        }

        public bool CheckNeedTXFFCIsCH()
        {
            try
            {
                if ((AppInfo.Current.Lottery.Type != ConfigurationStatus.LotteryType.TXFFC) && (AppInfo.Current.Lottery.Type != ConfigurationStatus.LotteryType.LSWJSOTXFFC))
                {
                    return false;
                }
                if (!AppInfo.IsCheckTXFFC)
                {
                    return false;
                }
                return true;
            }
            catch
            {
            }
            return false;
        }

        public bool CheckOpenIsRight()
        {
            if (!File.Exists(CommFunc.getDllPath() + @"\MdiTabControl.dll\"))
            {
                return false;
            }
            if (!File.Exists(CommFunc.getDllPath() + @"\Update\AutoUpdatePlus.exe"))
            {
                return false;
            }
            return true;
        }

        private bool CheckPTLine()
        {
            try
            {
                if (AppInfo.PTInfo.PTLoginStatus)
                {
                    base.Invoke(AppInfo.RefreshUserMain, new object[] { false });
                    AppInfo.BankRefresh();
                    AppInfo.PTInfo.GetSite(AppInfo.Current.Lottery.Type, "");
                }
            }
            catch
            {
            }
            return false;
        }

        private void CheckPTUserA()
        {
            if (!AppInfo.CheckPTLogin)
            {
                AppInfo.CheckPTLogin = true;
                try
                {
                    if (AppInfo.Account.Configuration.IsPTLogin)
                    {
                        if (AppInfo.PTInfo.PTLoginStatus)
                        {
                            string pHint = "";
                            if (!SQLData.PTUserLogin(AppInfo.Account, ref pHint))
                            {
                                if (pHint.Contains("平台用户不存在"))
                                {
                                    AppInfo.Account.PTLoginAudit = "审核中...";
                                    SQLData.AddPTUserRow(AppInfo.Account);
                                }
                            }
                            else if (AppInfo.Account.PTLoginAudit == "审核未过")
                            {
                                CommFunc.PublicMessageAll(AppInfo.Account.PTLoginHintString, true, MessageBoxIcon.Asterisk, "");
                                DebugLog.SaveDebug(AppInfo.Account.PTLoginHintString, "审核未过");
                                base.Invoke(AppInfo.LoginMain, new object[] { false, this.PTName });
                            }
                            SQLData.UpdataPTUserOnLine(AppInfo.Account);
                        }
                        base.Invoke(AppInfo.RefreshLoginHint);
                    }
                }
                catch
                {
                }
                AppInfo.CheckPTLogin = false;
            }
        }

        public bool CheckTXFFCIsCH(string pExpect, List<ConfigurationStatus.OpenData> pDataList = null)
        {
            if (!this.CheckNeedTXFFCIsCH())
            {
                return false;
            }
            try
            {
                if (pDataList == null)
                {
                    pDataList = AppInfo.DataList;
                }
                for (int i = 0; i < (pDataList.Count - 1); i++)
                {
                    ConfigurationStatus.OpenData data = pDataList[i];
                    if (data.Expect == pExpect)
                    {
                        ConfigurationStatus.OpenData data2 = pDataList[i + 1];
                        return (data.Code == data2.Code);
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        private bool CheckUrlIsPTIndex(string pUrl) => 
            (pUrl == AppInfo.PTInfo.GetIndexLine());

        private bool CheckUserID(bool pHint = false)
        {
            bool flag = AppInfo.Account.PTID != "";
            if (!(!pHint || flag))
            {
                CommFunc.PublicMessageAll("请先登录平台后再使用该功能！", true, MessageBoxIcon.Asterisk, "");
            }
            return flag;
        }

        private bool CheckUserLogin(bool pHint = false)
        {
            if (!AppInfo.PTInfo.PTLoginStatus)
            {
                if (pHint)
                {
                    CommFunc.PublicMessageAll("请先登录平台后再使用该功能！", true, MessageBoxIcon.Asterisk, "");
                }
                return false;
            }
            return true;
        }

        private bool CheckVerifyCode(string pVerifyCode)
        {
            if (pVerifyCode.Length != 4)
            {
                return false;
            }
            return true;
        }

        private void Ckb_AddBTFN_Click(object sender, EventArgs e)
        {
            string item = "";
            while (true)
            {
                item = CommFunc.GetStringFromInputBox("请输入方案的名称", this.GetDefultBTFNName(), "", false);
                if (item == "")
                {
                    return;
                }
                if (!AppInfo.BTFNList.Contains(item))
                {
                    AppInfo.BTFNList.Add(item);
                    AppInfo.BTFNDic[item] = new ConfigurationStatus.GJBTScheme(null, false, false);
                    CommFunc.RefreshDataGridView(this.Egv_BTFNMain, AppInfo.BTFNList.Count);
                    this.Egv_BTFNMain.Rows[AppInfo.BTFNList.Count - 1].Selected = true;
                    this.RefreshBTFNControl(true);
                    return;
                }
                CommFunc.PublicMessageAll($"方案名【{item}】已存在，请重新输入？", true, MessageBoxIcon.Asterisk, "");
            }
        }

        private void Ckb_AddLine_Click(object sender, EventArgs e)
        {
            string item = "";
            while (true)
            {
                item = CommFunc.GetStringFromInputBox("请输入添加平台线路", "http://", "", false);
                if (item == "")
                {
                    return;
                }
                if (!AppInfo.PTInfo.LineList.Contains(item))
                {
                    string text = this.Cbb_LoginPT.Text;
                    ConfigurationStatus.PTLine line = this.PTLineDic[text];
                    line.LineList.Insert(0, item);
                    AppInfo.PTInfo.LineList.Insert(0, item);
                    this.RefreshPTLineList();
                    this.SavePTLine();
                    return;
                }
                CommFunc.PublicMessageAll($"线路【{item}】已存在，请重新输入！", true, MessageBoxIcon.Asterisk, "");
            }
        }

        private void Ckb_AddScheme_Click(object sender, EventArgs e)
        {
            string item = "";
            while (true)
            {
                item = CommFunc.GetStringFromInputBox("请输入方案的名称", this.GetDefultSchemeName(), "", false);
                if (item == "")
                {
                    return;
                }
                if (!AppInfo.SchemeList.Contains(item))
                {
                    string text = this.Cbb_FNCHType.Text;
                    ConfigurationStatus.Scheme scheme = new ConfigurationStatus.Scheme(false, item, text, "", false, false);
                    AppInfo.Account.SchemeDic[item] = scheme;
                    AppInfo.SchemeList.Add(item);
                    this.SortSchemeList(AppInfo.SchemeList);
                    this.SaveSchemeData(item, "");
                    this.RefreshAllFNList();
                    CommFunc.SetDataGridViewSelected(this.Egv_SchemeList, item, 1);
                    this.RefreshScheme();
                    return;
                }
                CommFunc.PublicMessageAll($"方案名【{item}】已存在，请重新输入？", true, MessageBoxIcon.Asterisk, "");
            }
        }

        private void Ckb_AddTimes_Click(object sender, EventArgs e)
        {
            string bTFNName = this.GetBTFNName();
            if (bTFNName != "")
            {
                List<ConfigurationStatus.TimesScheme> timesSchemeList = AppInfo.BTFNDic[bTFNName].TimesSchemeList;
                List<int> pIDList = new List<int>();
                foreach (ConfigurationStatus.TimesScheme scheme in timesSchemeList)
                {
                    pIDList.Add(scheme.ID);
                }
                ConfigurationStatus.TimesScheme pInput = new ConfigurationStatus.TimesScheme(this.GetDefultTimesID(pIDList));
                FrmTimesInput input = new FrmTimesInput(pInput, true);
                if (input.ShowDialog() == DialogResult.OK)
                {
                    pInput = FrmTimesInput.OutValue;
                    timesSchemeList.Add(pInput);
                    this.SortTimesList(timesSchemeList);
                    this.RefreshBTFNControl(true);
                    this.Egv_BTFNTimesList.Rows[this.Egv_BTFNTimesList.RowCount - 1].Selected = true;
                }
            }
        }

        private void Ckb_AppName_Click(object sender, EventArgs e)
        {
            string text = this.Txt_AppName.Text;
            AppInfo.Account.Configuration.AppTag = text;
            if (text == "")
            {
                this.Text = this.AppName;
            }
            else
            {
                this.Text = text;
            }
        }

        private void Ckb_AutoSizeTJ_Click(object sender, EventArgs e)
        {
            this.Egv_TJDataList2.AutoResizeColumns();
            this.Egv_TJDataList2.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
        }

        private void Ckb_BetsSort_CheckedChanged(object sender, EventArgs e)
        {
            this.RefreshPlanList1();
        }

        private void Ckb_BTFNEdit_Click(object sender, EventArgs e)
        {
            CommFunc.CreateDirectory(BTFNPath);
            string bTFNName = this.GetBTFNName();
            if (bTFNName != "")
            {
                string text = this.Cbb_BTFNEdit.Text;
                int num = Convert.ToInt32(this.Nm_BTFNEdit.Value);
                if (CommFunc.AgreeMessage($"是否将【{bTFNName}】的所有倍数批量【{text}{num}】？", false, MessageBoxIcon.Asterisk, ""))
                {
                    List<ConfigurationStatus.TimesScheme> timesSchemeList = AppInfo.BTFNDic[bTFNName].TimesSchemeList;
                    foreach (ConfigurationStatus.TimesScheme scheme in timesSchemeList)
                    {
                        int times = scheme.Times;
                        if (!this.Ckb_BTFNEditSkip.Checked || (times != 0))
                        {
                            switch (text)
                            {
                                case "+":
                                    times += num;
                                    break;

                                case "-":
                                    times -= num;
                                    break;

                                case "*":
                                    times *= num;
                                    break;

                                case "/":
                                    times /= num;
                                    break;
                            }
                            if (times < 0)
                            {
                                times = 0;
                            }
                            scheme.Times = times;
                        }
                    }
                    this.RefreshBTFNControl(true);
                }
            }
        }

        private void Ckb_CancelScheme_Click(object sender, EventArgs e)
        {
            this.IsEditScheme = false;
            this.RefreshScheme();
        }

        private void Ckb_ClearBetsList_Click(object sender, EventArgs e)
        {
            string pName = "";
            ConfigurationStatus.AutoBets bets = this.GetBets(pName);
            if (CommFunc.AgreeMessage($"是否清空【{bets.Name}】的投注记录？", true, MessageBoxIcon.Asterisk, ""))
            {
                this.BetsDic.Remove(bets.Name);
                string betsPlanListPath = this.BetsPlanListPath;
                if (Directory.Exists(betsPlanListPath))
                {
                    File.Delete(betsPlanListPath + bets.Name + ".txt");
                }
                this.RefreshControl(true);
            }
        }

        private void Ckb_ClearScheme_Click(object sender, EventArgs e)
        {
            int count = AppInfo.Account.SchemeDic.Count;
            int pBTFNCount = AppInfo.BTFNList.Count;
            if (CommFunc.AgreeMessage(CommFunc.GetShareSchemeHint("是否要清空当前", count, pBTFNCount, true), false, MessageBoxIcon.Asterisk, ""))
            {
                this.ClearSchemeMain();
            }
        }

        private void Ckb_ClearTimes_Click(object sender, EventArgs e)
        {
            string bTFNName = this.GetBTFNName();
            if ((bTFNName != "") && CommFunc.AgreeMessage($"是否要清空方案【{bTFNName}】的所有倍投数据？", true, MessageBoxIcon.Asterisk, ""))
            {
                AppInfo.BTFNDic[bTFNName].TimesSchemeList.Clear();
                this.RefreshBTFNControl(true);
            }
        }

        private void Ckb_CopyScheme_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.Scheme pInfo = this.GetScheme("");
            if ((pInfo != null) && this.CheckFNEncrypt(pInfo))
            {
                string item = "";
                while (true)
                {
                    item = CommFunc.GetStringFromInputBox("请输入方案的名称", this.GetDefultSchemeName(), "", false);
                    if (item == "")
                    {
                        return;
                    }
                    if (!AppInfo.SchemeList.Contains(item))
                    {
                        AppInfo.Account.SchemeDic[item] = pInfo.GetNewScheme(item);
                        AppInfo.SchemeList.Add(item);
                        this.SortSchemeList(AppInfo.SchemeList);
                        this.SaveSchemeData(item, "");
                        this.RefreshAllFNList();
                        CommFunc.SetDataGridViewSelected(this.Egv_SchemeList, item, 1);
                        this.RefreshScheme();
                        return;
                    }
                    CommFunc.PublicMessageAll($"方案名【{item}】已存在，请重新输入？", true, MessageBoxIcon.Asterisk, "");
                }
            }
        }

        private void Ckb_Data_CheckedChanged(object sender, EventArgs e)
        {
            this.Egv_DataList.Visible = this.Ckb_Data.Checked;
        }

        private void Ckb_DeleteBTFN_Click(object sender, EventArgs e)
        {
            string bTFNName = this.GetBTFNName();
            if (bTFNName != "")
            {
                string path = BTFNPath + bTFNName + ".txt";
                if (CommFunc.AgreeMessage($"是否要删除方案【{bTFNName}】？", true, MessageBoxIcon.Asterisk, ""))
                {
                    File.Delete(path);
                    AppInfo.BTFNList.Remove(bTFNName);
                    AppInfo.BTFNDic.Remove(bTFNName);
                    this.RefreshBTFNControl(true);
                }
            }
        }

        private void Ckb_DeleteLine_Click(object sender, EventArgs e)
        {
            string pTLineUrl = this.GetPTLineUrl();
            if ((pTLineUrl != "") && CommFunc.AgreeMessage("是否要删除当前选中服务器？", true, MessageBoxIcon.Asterisk, ""))
            {
                string text = this.Cbb_LoginPT.Text;
                ConfigurationStatus.PTLine line = this.PTLineDic[text];
                line.LineList.Remove(pTLineUrl);
                AppInfo.PTInfo.LineList.Remove(pTLineUrl);
                this.RefreshPTLineList();
                this.SavePTLine();
            }
        }

        private void Ckb_DeleteScheme_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.Scheme scheme = this.GetScheme("");
            if (scheme != null)
            {
                string schemePath = this.SchemePath;
                string name = scheme.Name;
                if (CommFunc.AgreeMessage($"是否要删除【{name}】？", true, MessageBoxIcon.Asterisk, ""))
                {
                    File.Delete(schemePath + name + "-" + scheme.CHType + ".txt");
                    AppInfo.Account.SchemeDic.Remove(name);
                    AppInfo.SchemeList.Remove(name);
                    this.RefreshAllFNList();
                    this.RefreshScheme();
                }
            }
        }

        private void Ckb_DeleteTimes_Click(object sender, EventArgs e)
        {
            string bTFNName = this.GetBTFNName();
            int timesIndex = this.GetTimesIndex();
            if ((bTFNName != "") && (timesIndex != -1))
            {
                AppInfo.BTFNDic[bTFNName].TimesSchemeList.RemoveAt(timesIndex);
                this.RefreshBTFNControl(true);
            }
        }

        private void Ckb_EditScheme_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.Scheme pInfo = this.GetScheme("");
            if ((pInfo != null) && this.CheckFNEncrypt(pInfo))
            {
                this.IsEditScheme = true;
                this.RefreshScheme();
            }
        }

        private void Ckb_EditTimes_Click(object sender, EventArgs e)
        {
            string bTFNName = this.GetBTFNName();
            int timesIndex = this.GetTimesIndex();
            if ((bTFNName != "") && (timesIndex != -1))
            {
                List<ConfigurationStatus.TimesScheme> timesSchemeList = AppInfo.BTFNDic[bTFNName].TimesSchemeList;
                ConfigurationStatus.TimesScheme pInput = timesSchemeList[timesIndex];
                FrmTimesInput input = new FrmTimesInput(pInput, false);
                if (input.ShowDialog() == DialogResult.OK)
                {
                    pInput = FrmTimesInput.OutValue;
                    timesSchemeList[timesIndex] = pInput;
                    this.RefreshBTFNControl(true);
                }
            }
        }

        private void Ckb_EditTimesPlan_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.Scheme pInfo = this.GetScheme("");
            if (((pInfo != null) && this.CheckFNEncrypt(pInfo)) && CommFunc.AgreeMessage($"是否将所有方案的直线倍投修改为【{pInfo.Name}】的直线倍投？", false, MessageBoxIcon.Asterisk, ""))
            {
                List<string> bTPlanList = pInfo.FNBaseInfo.BTPlanList;
                foreach (string str in AppInfo.Account.SchemeDic.Keys)
                {
                    ConfigurationStatus.Scheme scheme2 = AppInfo.Account.SchemeDic[str];
                    scheme2.FNBaseInfo.BTPlanList = CommFunc.CopyList(bTPlanList);
                    this.SaveSchemeData(str, "");
                }
                this.RefreshScheme();
            }
        }

        private void Ckb_ExportScheme_Click(object sender, EventArgs e)
        {
            string pPath = "";
            string pHint = "请选择保存导出方案的文件夹";
            if (CommFunc.GetPathFromDialog(ref pPath, base.Name, pHint))
            {
                pPath = pPath + @"\";
                this.SaveAllSchemeData(pPath);
                if (AppInfo.BTFNDic.Count > 0)
                {
                    string path = pPath + @"\GJBTScheme\";
                    this.SaveAllBTFNData(false, path);
                }
                CommFunc.PublicMessageAll("导出成功！", true, MessageBoxIcon.Asterisk, "");
            }
        }

        private void Ckb_ImportScheme_Click(object sender, EventArgs e)
        {
            if (((AppInfo.Account.SchemeDic.Count + AppInfo.BTFNList.Count) > 0) && CommFunc.AgreeMessage("在导入方案之前是否要清空当前全部方案？\r\n确定：清空当前全部方案，取消：不清空", false, MessageBoxIcon.Asterisk, ""))
            {
                this.ClearSchemeMain();
            }
            string pPath = "";
            string pHint = "请选择导入方案的文件夹";
            if (CommFunc.GetPathFromDialog(ref pPath, base.Name, pHint))
            {
                int pSchemeCount = this.LoadSchemeData(pPath, false);
                this.SaveAllSchemeData("");
                string path = pPath + @"\GJBTScheme\";
                int pBTFNCount = 0;
                if (Directory.Exists(path))
                {
                    pBTFNCount = this.LoadBTFNListData(path);
                    this.SaveAllBTFNData(false, "");
                }
                CommFunc.PublicMessageAll(CommFunc.GetShareSchemeHint("成功导入", pSchemeCount, pBTFNCount, false), true, MessageBoxIcon.Asterisk, "");
            }
        }

        private void Ckb_LeftInfo_CheckedChanged(object sender, EventArgs e)
        {
            this.Pnl_OpenData.Visible = this.Ckb_LeftInfo.Checked;
        }

        private void Ckb_Login_Click(object sender, EventArgs e)
        {
            this.LoginMain();
        }

        private void Ckb_LSBJType_CheckedChanged(object sender, EventArgs e)
        {
            this.Cbb_LSBJType.Enabled = this.Rdb_LSBJType.Enabled = this.Rdb_LSBJExpect.Enabled = this.Lbl_LSBJExpect2.Enabled = this.Ckb_LSBJ.Checked;
            this.Rdb_LSBJType_CheckedChanged(null, null);
        }

        private void Ckb_LSStop_Click(object sender, EventArgs e)
        {
            Thread refreshLSDataThread = this.mainThread.refreshLSDataThread;
            if (refreshLSDataThread.ThreadState != ThreadState.Stopped)
            {
                refreshLSDataThread.Abort();
                base.Invoke(AppInfo.RefreshLSDataLater);
            }
        }

        private void Ckb_MN1_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_MN1.Enabled = this.Ckb_MN1.Checked;
        }

        private void Ckb_MN2_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_MN2.Enabled = this.Ckb_MN2.Checked;
        }

        private void Ckb_MN3_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_MN3.Enabled = this.Ckb_MN3.Checked;
        }

        private void Ckb_MN4_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_MN4.Enabled = this.Ckb_MN4.Checked;
        }

        private void Ckb_MNBets_CheckedChanged(object sender, EventArgs e)
        {
            this.Pnl_BetsInfoMNRight.Enabled = !this.Ckb_MNBets.Checked;
        }

        private void Ckb_PlanShowHide_CheckedChanged(object sender, EventArgs e)
        {
            List<int> list = new List<int> { 
                0,
                1,
                4
            };
            foreach (int num in list)
            {
                this.Egv_PlanList.Columns[num].Visible = !this.Ckb_PlanShowHide.Checked;
            }
        }

        private void Ckb_PlaySound_CheckedChanged(object sender, EventArgs e)
        {
            AppInfo.PlaySound = this.Ckb_PlaySound.Checked;
        }

        private void Ckb_PWClear_Click(object sender, EventArgs e)
        {
            this.Txt_PW.Text = "";
            this.Txt_PW.Focus();
        }

        private void Ckb_PWPaste_Click(object sender, EventArgs e)
        {
            CommFunc.PasteText(this.Txt_PW);
        }

        private void Ckb_Refresh_Click(object sender, EventArgs e)
        {
            this.RefreshLoad(true);
        }

        private void Ckb_RefreshUser_Click(object sender, EventArgs e)
        {
            switch (this.Ckb_RefreshUser.Text)
            {
                case "刷新":
                    this.RefreshUserMain(true);
                    break;

                case "取消":
                    AppInfo.LoginCancel = true;
                    this.Web_Login.Url = null;
                    break;
            }
        }

        private void Ckb_RrfreshPT_CheckedChanged(object sender, EventArgs e)
        {
            this.Pnl_RrfreshPT.Visible = this.Ckb_RrfreshPT.Checked;
        }

        private void Ckb_RrfreshPTLine_Click(object sender, EventArgs e)
        {
            this.RefreshPTMain(true);
        }

        private void Ckb_SaveScheme_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.Scheme pInfo = this.GetScheme("");
            pInfo.CHType = this.Cbb_FNCHType.Text;
            if (this.GetSchemeControlInfo(ref pInfo))
            {
                this.SaveSchemeData(pInfo.Name, "");
                this.IsEditScheme = false;
                this.RefreshScheme();
                this.RefreshSchemeList();
            }
        }

        private void Ckb_SaveTimes_Click(object sender, EventArgs e)
        {
            this.SaveAllBTFNData(true, "");
        }

        private void Ckb_ShareBetsManage_Click(object sender, EventArgs e)
        {
            if (AppInfo.Account.SendUserID != "")
            {
                if (this.Rdb_ShareBets.Text == "共享投注")
                {
                    new FrmSendBets(true).ShowDialog();
                }
                else
                {
                    new FrmFollowBets(this.RegConfigPath).ShowDialog();
                }
            }
        }

        private void Ckb_ShareScheme_Click(object sender, EventArgs e)
        {
            if (AppInfo.Account.SendUserID != "")
            {
                try
                {
                    int count;
                    int num2;
                    int num3;
                    int num4;
                    long num5;
                    bool flag;
                    ConfigurationStatus.Scheme schemeByFileValue;
                    ConfigurationStatus.GJBTScheme bTFNByFileValue;
                    if (this.Ckb_ShareScheme.Text == "上传")
                    {
                        count = AppInfo.Account.SchemeDic.Count;
                        num2 = AppInfo.BTFNList.Count;
                        num3 = 0;
                        num4 = 0;
                        num5 = count + num2;
                        if (CommFunc.AgreeMessage(CommFunc.GetShareSchemeHint("是否要上传当前", count, num2, true), false, MessageBoxIcon.Asterisk, ""))
                        {
                            if (!SQLData.DelShareSchemeRow())
                            {
                                CommFunc.PublicMessageAll("删除之前方案失败，请重新上传！", true, MessageBoxIcon.Asterisk, "");
                                return;
                            }
                            CommFunc.InitialProgress(this, "方案上传中...", (float) num5, 2);
                            flag = false;
                            foreach (string str in AppInfo.Account.SchemeDic.Keys)
                            {
                                if (CommFunc.SetProgress(ref flag))
                                {
                                    break;
                                }
                                schemeByFileValue = AppInfo.Account.SchemeDic[str];
                                if (SQLData.AddShareSchemeRow(AppInfo.Current.Lottery.GroupString, schemeByFileValue.Name, schemeByFileValue.CHType, schemeByFileValue.FNBaseInfo.Play, schemeByFileValue.GetFileValue()))
                                {
                                    num3++;
                                }
                            }
                            foreach (string str in AppInfo.BTFNDic.Keys)
                            {
                                if (CommFunc.SetProgress(ref flag))
                                {
                                    break;
                                }
                                bTFNByFileValue = AppInfo.BTFNDic[str];
                                if (SQLData.AddShareSchemeRow("BTFN", str, "无", "无", bTFNByFileValue.GetFileValue()))
                                {
                                    num4++;
                                }
                            }
                            CommFunc.CloseProgress();
                            if ((count != num3) || (num2 != num4))
                            {
                                CommFunc.PublicMessageAll("方案没有全部上传，请重新上传！", true, MessageBoxIcon.Asterisk, "");
                                return;
                            }
                            if (SQLData.AddShareSchemeStateRow(num3, num4))
                            {
                                CommFunc.PublicMessageAll(CommFunc.GetShareSchemeHint("成功上传", num3, num4, false), true, MessageBoxIcon.Asterisk, "");
                            }
                            else
                            {
                                CommFunc.PublicMessageAll("上传失败，请重新上传！", true, MessageBoxIcon.Asterisk, "");
                            }
                        }
                    }
                    else
                    {
                        string pSource = CommFunc.ReadRegString(this.RegConfigPath, "ShareCode", "");
                        string decodeShareCode = ConfigurationStatus.ShareBets.GetDecodeShareCode(pSource);
                        if (pSource == "")
                        {
                            CommFunc.PublicMessageAll("请输入上级发给你的共享码！", true, MessageBoxIcon.Asterisk, "");
                            this.Ckb_ShareSchemeManage_Click(null, null);
                            return;
                        }
                        if (decodeShareCode == "")
                        {
                            CommFunc.PublicMessageAll("您输入的共享码错误，请重新输入！", true, MessageBoxIcon.Asterisk, "");
                            this.Ckb_ShareSchemeManage_Click(null, null);
                            return;
                        }
                        count = 0;
                        num2 = 0;
                        string pHint = "";
                        if (!SQLData.GetShareSchemeCount(decodeShareCode, pSource, ref pHint, ref count, ref num2))
                        {
                            CommFunc.PublicMessageAll(pHint, true, MessageBoxIcon.Asterisk, "");
                            return;
                        }
                        if (((AppInfo.Account.SchemeDic.Count + AppInfo.BTFNList.Count) > 0) && CommFunc.AgreeMessage("在下载方案之前是否要清空当前全部方案？\r\n确定：清空当前全部方案，取消：不清空", false, MessageBoxIcon.Asterisk, ""))
                        {
                            this.ClearSchemeMain();
                        }
                        num3 = 0;
                        num4 = 0;
                        num5 = count + num2;
                        if (CommFunc.AgreeMessage(CommFunc.GetShareSchemeHint("是否要下载上级共享的", count, num2, true), false, MessageBoxIcon.Asterisk, ""))
                        {
                            int num6;
                            string str5;
                            XmlDocument document;
                            XmlNode node;
                            string innerText;
                            CommFunc.InitialProgress(this, "方案下载中...", (float) num5, 2);
                            flag = false;
                            for (num6 = 0; num6 < count; num6++)
                            {
                                if (CommFunc.SetProgress(ref flag))
                                {
                                    break;
                                }
                                str5 = "";
                                if (SQLData.GetShareSchemeList(AppInfo.Current.Lottery.GroupString, decodeShareCode, num6, ref pHint, ref str5))
                                {
                                    document = new XmlDocument();
                                    document.LoadXml(str5);
                                    node = document.SelectNodes("Scheme/Item")[0];
                                    innerText = node.SelectSingleNode("FNName").InnerText;
                                    schemeByFileValue = ConfigurationStatus.Scheme.GetSchemeByFileValue(node.SelectSingleNode("Value").InnerText, innerText);
                                    if (schemeByFileValue != null)
                                    {
                                        AppInfo.Account.SchemeDic[innerText] = schemeByFileValue;
                                        if (!AppInfo.SchemeList.Contains(innerText))
                                        {
                                            AppInfo.SchemeList.Add(innerText);
                                        }
                                        num3++;
                                    }
                                }
                            }
                            for (num6 = 0; num6 < num2; num6++)
                            {
                                if (CommFunc.SetProgress(ref flag))
                                {
                                    break;
                                }
                                str5 = "";
                                if (SQLData.GetShareSchemeList("BTFN", decodeShareCode, num6, ref pHint, ref str5))
                                {
                                    document = new XmlDocument();
                                    document.LoadXml(str5);
                                    node = document.SelectNodes("Scheme/Item")[0];
                                    innerText = node.SelectSingleNode("FNName").InnerText;
                                    bTFNByFileValue = ConfigurationStatus.GJBTScheme.GetBTFNByFileValue(node.SelectSingleNode("Value").InnerText);
                                    if (bTFNByFileValue != null)
                                    {
                                        AppInfo.BTFNDic[innerText] = bTFNByFileValue;
                                        if (!AppInfo.BTFNList.Contains(innerText))
                                        {
                                            AppInfo.BTFNList.Add(innerText);
                                        }
                                        num4++;
                                    }
                                }
                            }
                            CommFunc.CloseProgress();
                            if ((count != num3) || (num2 != num4))
                            {
                                CommFunc.PublicMessageAll("方案没有全部下载，请重新下载！", true, MessageBoxIcon.Asterisk, "");
                            }
                            else
                            {
                                CommFunc.PublicMessageAll(CommFunc.GetShareSchemeHint("成功下载", num3, num4, false), true, MessageBoxIcon.Asterisk, "");
                            }
                            this.SaveAllSchemeData("");
                            this.SaveAllBTFNData(false, "");
                            this.SortSchemeList(AppInfo.SchemeList);
                            this.RefreshAllFNList();
                            this.RefreshScheme();
                            this.Egv_SchmeeList_CellClick(null, null);
                        }
                    }
                }
                catch
                {
                }
                CommFunc.CloseProgress();
            }
        }

        private void Ckb_ShareSchemeManage_Click(object sender, EventArgs e)
        {
            if (AppInfo.Account.SendUserID != "")
            {
                if (this.Ckb_ShareScheme.Text == "上传")
                {
                    new FrmSendBets(false).ShowDialog();
                }
                else
                {
                    new FrmFollowBets(this.RegConfigPath).ShowDialog();
                }
            }
        }

        private void Ckb_ShowHideUser_Click(object sender, EventArgs e)
        {
            switch (this.Ckb_ShowHideUser.Text)
            {
                case "隐藏":
                    this.Lbl_IDValue.Visible = false;
                    this.Ckb_ShowHideUser.Text = "显示";
                    break;

                case "显示":
                    this.Lbl_IDValue.Visible = true;
                    this.Ckb_ShowHideUser.Text = "隐藏";
                    break;
            }
        }

        private void Ckb_TBCount_Click(object sender, EventArgs e)
        {
            this.SetTapShow(this.Tap_TBCount.Tag.ToString(), true, true);
            CommFunc.SetTabSelectIndex(this.Tab_Main, this.Tap_TBCount.Text);
        }

        private void Ckb_TJFindXS_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.TJDataView1 view = this.GetTJDataView1Row();
            if (view != null)
            {
                this.Egv_TJDataList2.ClearSelection();
                string key = this.Nm_TJFindXS.Value.ToString();
                if (!view.ZRDic.ContainsKey(key))
                {
                    CommFunc.PublicMessageAll("没有找到符合的凶手！", true, MessageBoxIcon.Asterisk, "");
                }
                else
                {
                    string str2 = view.ZRDic[key].Split(new char[] { '|' })[0];
                    List<int> list = CommFunc.SplitInt(view.ZRDic[key].Split(new char[] { '|' })[2], "-");
                    List<int> list2 = new List<int> { 
                        8,
                        9,
                        10,
                        11,
                        12,
                        13,
                        14
                    };
                    for (int i = list[0]; i < list[1]; i++)
                    {
                        foreach (int num2 in list2)
                        {
                            this.Egv_TJDataList2.Rows[i].Cells[num2].Selected = true;
                        }
                    }
                    this.Egv_TJDataList2.FirstDisplayedScrollingRowIndex = list[0];
                    this.Egv_TJDataList2.FirstDisplayedScrollingColumnIndex = list2[0];
                }
            }
        }

        private void Ckb_TJStop_Click(object sender, EventArgs e)
        {
            Thread refreshTJDataThread = this.mainThread.refreshTJDataThread;
            if (refreshTJDataThread.ThreadState != ThreadState.Stopped)
            {
                refreshTJDataThread.Abort();
                base.Invoke(AppInfo.RefreshTJDataLater);
            }
        }

        private void ClearSchemeMain()
        {
            foreach (string str in AppInfo.Account.SchemeDic.Keys)
            {
                ConfigurationStatus.Scheme scheme = this.GetScheme(str);
                File.Delete(this.SchemePath + str + "-" + scheme.CHType + ".txt");
            }
            AppInfo.Account.SchemeDic.Clear();
            AppInfo.SchemeList.Clear();
            this.RefreshAllFNList();
            this.RefreshScheme();
            foreach (string str in AppInfo.BTFNList)
            {
                File.Delete(BTFNPath + str + ".txt");
            }
            AppInfo.BTFNList.Clear();
            AppInfo.BTFNDic.Clear();
            this.RefreshBTFNControl(true);
        }

        private void CloseAllBets()
        {
            foreach (string str in this.BetsDic.Keys)
            {
                ConfigurationStatus.AutoBets pBets = this.BetsDic[str];
                this.CloseBetsMain(pBets);
            }
        }

        private void CloseApp()
        {
            this.CloseFormMain(false, true);
        }

        private void CloseBetsMain(ConfigurationStatus.AutoBets pBets)
        {
            string name = pBets.Name;
            this.CloseBetsThread(name);
            pBets.DefaultOption(true);
        }

        private void CloseBetsThread(string pName)
        {
            if (this.mainThread.BetsThreadDic.ContainsKey(pName))
            {
                Thread thread = this.mainThread.BetsThreadDic[pName];
                while (thread.ThreadState != ThreadState.Stopped)
                {
                    thread.Abort();
                    thread.Join();
                }
            }
        }

        private void CloseFormMain(bool pHint, bool pSave = true)
        {
            try
            {
                if (!pHint || CommFunc.AgreeMessage("是否要退出软件？", false, MessageBoxIcon.Asterisk, ""))
                {
                    if (pSave)
                    {
                        this.SaveControlInfoByReg();
                        this.SavePTInfoByReg();
                        this.SavePlanListData(false);
                        this.ColsingCheck();
                    }
                    this.Dispose(true);
                    Application.ExitThread();
                    Application.Exit();
                    CommFunc.ClearObejct();
                }
            }
            catch
            {
            }
        }

        private void ColsingCheck()
        {
            if (this.mainThread != null)
            {
                this.mainThread.CloseThreadList();
            }
            this.CloseAllBets();
            this.BetsDic.Clear();
        }

        private void CombinaPlanValue(ConfigurationStatus.SCPlan plan, List<ConfigurationStatus.SCPlan> pTempPlanList, ConfigurationStatus.BetsFNTotal pFNTotal = null)
        {
            try
            {
                string fNCHType = plan.FNCHType;
                if (this.CHTypeList.Contains(fNCHType))
                {
                    Dictionary<string, Dictionary<string, List<string>>> fNNumberDic = plan.FNNumberDic;
                    foreach (string str2 in fNNumberDic.Keys)
                    {
                        string fNName;
                        Dictionary<string, List<string>> dictionary2 = fNNumberDic[str2];
                        List<string> list = new List<string>();
                        foreach (string str3 in dictionary2.Keys)
                        {
                            list = CommFunc.CombinaList<string>(list, dictionary2[str3]);
                        }
                        ConfigurationStatus.SCPlan item = new ConfigurationStatus.SCPlan {
                            ZuKey = str2,
                            ZuKeyDic = dictionary2,
                            UploadTime = plan.UploadTime,
                            LotteryName = plan.LotteryName,
                            LotteryID = plan.LotteryID
                        };
                        item.BeginExpect = item.EndExpect = plan.BeginExpect;
                        item.Code = plan.Code;
                        item.FNName = plan.FNName;
                        item.PlayType = plan.PlayType;
                        item.PlayName = plan.PlayName;
                        item.RXWZ = plan.RXWZ;
                        item.RXZJ = plan.RXZJ;
                        item.TimesDic = plan.TimesDic;
                        item.AutoTimesDic = plan.AutoTimesDic;
                        item.Money = plan.Money;
                        item.IsMNBets = plan.IsMNBets;
                        item.FNGainString = plan.FNGainString;
                        item.FNMNGainString = plan.FNMNGainString;
                        item.FNMoneyString = plan.FNMoneyString;
                        item.FNMNMoneyString = plan.FNMNMoneyString;
                        item.NumberList = CommFunc.CopyList(list);
                        item.Number = CommFunc.GetBetsCodeCount(plan.GetPTNumberList(null), plan.Play, plan.RXWZ);
                        if ((plan.CheckPlanIsWait() || plan.CheckPlanStringIsNoOpen()) || plan.CheckPlanStringIsTXFFCCH())
                        {
                            item.State = plan.State;
                        }
                        else
                        {
                            string str6;
                            int pWinCount = 0;
                            double num2 = 0.0;
                            Dictionary<string, double> dictionary3 = new Dictionary<string, double>();
                            foreach (string str4 in dictionary2.Keys)
                            {
                                List<string> pNumberList = dictionary2[str4];
                                List<string> list3 = CommFunc.SplitString(pNumberList[0], "|", -1);
                                if (list3.Count < 4)
                                {
                                    item.State = plan.WaitLottery;
                                    goto Label_0711;
                                }
                                List<string> pTNumberList = plan.GetPTNumberList(pNumberList);
                                int num3 = Convert.ToInt32(list3[3]);
                                if (!item.FNAutoStateDic.ContainsKey(str4))
                                {
                                    item.FNAutoStateDic[str4] = item.NoString;
                                }
                                if (num3 > 0)
                                {
                                    double num4 = (Convert.ToDouble(list3[1]) * plan.AutoTimes(str4, true)) * num3;
                                    pWinCount += num3;
                                    num2 += num4;
                                    if (!dictionary3.ContainsKey(str4))
                                    {
                                        dictionary3[str4] = num4;
                                    }
                                    else
                                    {
                                        Dictionary<string, double> dictionary4;
                                        (dictionary4 = dictionary3)[str6 = str4] = dictionary4[str6] + num4;
                                    }
                                }
                                if (CommFunc.VerificationWinCount(pWinCount, plan.Play, plan.RXZJList))
                                {
                                    item.FNAutoStateDic[str4] = item.YesString;
                                }
                            }
                            item.IsWin = CommFunc.VerificationWinCount(pWinCount, plan.Play, plan.RXZJList);
                            item.AutoWinCount = pWinCount;
                            item.AutoTotalMode = num2;
                            item.Gain = item.AutoTotalMode - item.FNAutoTotalMoney;
                            if (pFNTotal != null)
                            {
                                int num5;
                                int num6;
                                Dictionary<string, int> dictionary5;
                                fNName = item.FNName;
                                if (item.IsWin)
                                {
                                    if (!pFNTotal.LZDic.ContainsKey(fNName))
                                    {
                                        pFNTotal.LZDic[fNName] = 1;
                                    }
                                    else
                                    {
                                        (dictionary5 = pFNTotal.LZDic)[str6 = fNName] = dictionary5[str6] + 1;
                                    }
                                    num5 = pFNTotal.LZDic[fNName];
                                    if (!pFNTotal.LZMaxDic.ContainsKey(fNName))
                                    {
                                        pFNTotal.LZMaxDic[fNName] = num5;
                                    }
                                    else if (pFNTotal.LZMaxDic[fNName] < num5)
                                    {
                                        pFNTotal.LZMaxDic[fNName] = num5;
                                    }
                                    if (pFNTotal.LGDic.ContainsKey(fNName))
                                    {
                                        num6 = pFNTotal.LGDic[fNName];
                                        if (!pFNTotal.LGMaxDic.ContainsKey(fNName))
                                        {
                                            pFNTotal.LGMaxDic[fNName] = num6;
                                        }
                                        else if (pFNTotal.LGMaxDic[fNName] < num6)
                                        {
                                            pFNTotal.LGMaxDic[fNName] = num6;
                                        }
                                        pFNTotal.LGDic.Remove(fNName);
                                    }
                                }
                                else
                                {
                                    if (!pFNTotal.LGDic.ContainsKey(fNName))
                                    {
                                        pFNTotal.LGDic[fNName] = 1;
                                    }
                                    else
                                    {
                                        (dictionary5 = pFNTotal.LGDic)[str6 = fNName] = dictionary5[str6] + 1;
                                    }
                                    num6 = pFNTotal.LGDic[fNName];
                                    if (!pFNTotal.LGMaxDic.ContainsKey(fNName))
                                    {
                                        pFNTotal.LGMaxDic[fNName] = num6;
                                    }
                                    else if (pFNTotal.LGMaxDic[fNName] < num6)
                                    {
                                        pFNTotal.LGMaxDic[fNName] = num6;
                                    }
                                    if (pFNTotal.LZDic.ContainsKey(fNName))
                                    {
                                        num5 = pFNTotal.LZDic[fNName];
                                        if (!pFNTotal.LZMaxDic.ContainsKey(fNName))
                                        {
                                            pFNTotal.LZMaxDic[fNName] = num5;
                                        }
                                        else if (pFNTotal.LZMaxDic[fNName] < num5)
                                        {
                                            pFNTotal.LZMaxDic[fNName] = num5;
                                        }
                                        pFNTotal.LZDic.Remove(fNName);
                                    }
                                }
                            }
                            foreach (string str3 in item.ZuKeyDic.Keys)
                            {
                                if (dictionary3.ContainsKey(str3))
                                {
                                    item.FNAutoGainDic[str3] = dictionary3[str3] - item.AutoTotalMoney(str3, true);
                                }
                                else
                                {
                                    item.FNAutoGainDic[str3] = -1.0 * item.AutoTotalMoney(str3, true);
                                }
                            }
                            item.State = item.AlreadLottery;
                        }
                    Label_0711:
                        if (pFNTotal != null)
                        {
                            fNName = item.FNName;
                            if (pFNTotal.LGDic.ContainsKey(fNName))
                            {
                                item.FNLG = pFNTotal.LGDic[fNName];
                            }
                            if (pFNTotal.LGMaxDic.ContainsKey(fNName))
                            {
                                item.FNLGMax = pFNTotal.LGMaxDic[fNName];
                            }
                            if (pFNTotal.LZDic.ContainsKey(fNName))
                            {
                                item.FNLZ = pFNTotal.LZDic[fNName];
                            }
                            if (pFNTotal.LZMaxDic.ContainsKey(fNName))
                            {
                                item.FNLZMax = pFNTotal.LZMaxDic[fNName];
                            }
                        }
                        pTempPlanList.Add(item);
                    }
                }
                else
                {
                    pTempPlanList.Add(plan);
                }
            }
            catch (Exception exception)
            {
                DebugLog.SaveDebug(exception, "");
            }
        }

        private bool CountBetsScheme(ConfigurationStatus.AutoBets pBets)
        {
            Dictionary<string, ConfigurationStatus.BetsScheme> betsSchemeDic = pBets.BetsSchemeDic;
            bool flag = this.Ckb_FNLT.Checked;
            foreach (string str in AppInfo.Account.SchemeDic.Keys)
            {
                try
                {
                    ConfigurationStatus.Scheme scheme = AppInfo.Account.SchemeDic[str];
                    string name = scheme.Name;
                    if (!scheme.Selected)
                    {
                        betsSchemeDic.Remove(name);
                    }
                    else
                    {
                        ConfigurationStatus.BetsScheme pBetsScheme = betsSchemeDic.ContainsKey(name) ? betsSchemeDic[name] : new ConfigurationStatus.BetsScheme();
                        pBetsScheme.SchemeInfo = scheme;
                        if (!this.CountTimes(pBetsScheme, ref pBets.ErrorState))
                        {
                            return false;
                        }
                        if (!this.CheckFBYKHT(pBetsScheme, ref pBets.ErrorState))
                        {
                            return false;
                        }
                        if (!pBetsScheme.SchemeInfo.FNBaseInfo.CheckCount(pBetsScheme, ref pBets.ErrorState))
                        {
                            return false;
                        }
                        if ((AppInfo.PTInfo == AppInfo.WJSJInfo) || (AppInfo.PTInfo == AppInfo.LFGJInfo))
                        {
                            string play = pBetsScheme.SchemeInfo.FNBaseInfo.Play;
                            if (CommFunc.CheckPlayIsRXDS(play))
                            {
                                int codeCount = pBetsScheme.SchemeInfo.FNBaseInfo.PlayInfo.CodeCount;
                                if (play.Contains("组三") || play.Contains("组六"))
                                {
                                    codeCount = 3;
                                }
                                if (pBetsScheme.SchemeInfo.FNBaseInfo.RXWZList.Count != codeCount)
                                {
                                    pBets.ErrorState = $"【{pBetsScheme.SchemeInfo.Name}】任选的投注位置必须为【{codeCount}】个！";
                                    return false;
                                }
                            }
                        }
                        if ((AppInfo.PTInfo == AppInfo.HSGJInfo) || (AppInfo.PTInfo == AppInfo.HUIZInfo))
                        {
                            List<string> list = new List<string> { 
                                "XXLSSC",
                                "ELSSSC"
                            };
                            if (list.Contains(pBets.LotteryID) && (pBetsScheme.SchemeInfo.FNBaseInfo.Unit == ConfigurationStatus.SCUnitType.Li))
                            {
                                pBets.ErrorState = $"【{pBets.LotteryName}】不支持厘模式！";
                                return false;
                            }
                        }
                        if ((AppInfo.PTInfo == AppInfo.YRYLInfo) && (pBetsScheme.SchemeInfo.FNBaseInfo.Unit == ConfigurationStatus.SCUnitType.Li))
                        {
                            pBets.ErrorState = $"【{pBets.LotteryName}】不支持厘模式！";
                            return false;
                        }
                        if ((((pBets.LotteryID == "TXFFC") || (pBets.LotteryID == "QQFFC")) && (AppInfo.PTInfo == AppInfo.DJYLInfo)) && !(((pBetsScheme.Play.Contains("定位胆") || pBetsScheme.Play.Contains("后二")) || pBetsScheme.Play.Contains("前二")) || pBetsScheme.Play.Contains("任二")))
                        {
                            pBets.ErrorState = $"【{pBets.LotteryName}】只支持定位胆玩法、前中二、任选二！";
                            return false;
                        }
                        if (flag)
                        {
                            if (pBets.TZFNName == "")
                            {
                                pBets.TZFNName = name;
                                betsSchemeDic[name] = pBetsScheme;
                                break;
                            }
                            if (name == pBets.TZFNName)
                            {
                                betsSchemeDic[name] = pBetsScheme;
                                break;
                            }
                        }
                        else
                        {
                            betsSchemeDic[name] = pBetsScheme;
                        }
                    }
                }
                catch (Exception exception)
                {
                    DebugLog.SaveDebug(exception, "");
                }
            }
            if (betsSchemeDic.Count == 0)
            {
                pBets.ErrorState = "当前没有可投注的方案！";
                return false;
            }
            if ((AppInfo.Account.Configuration.FNUsed > 0) && (betsSchemeDic.Count > AppInfo.Account.Configuration.FNUsed))
            {
                pBets.ErrorState = $"当前最多只能勾选【{AppInfo.Account.Configuration.FNUsed}】个方案同时投注！";
                return false;
            }
            if (pBets.ShareBetsInfo.BetsTypeInfo == ConfigurationStatus.BetsType.SendBets)
            {
                int maxSharePlanCount = AppInfo.Account.Configuration.MaxSharePlanCount;
                if ((maxSharePlanCount != -1) && (betsSchemeDic.Count > maxSharePlanCount))
                {
                    pBets.ErrorState = $"共享投注最多支持【{maxSharePlanCount}】个方案同时投注！";
                    return false;
                }
            }
            pBets.BetsSchemeDic = betsSchemeDic;
            return true;
        }

        private void CountPlanViewList(ConfigurationStatus.AutoBets pBets)
        {
            pBets.ExpectBackColorDic.Clear();
            List<string> list = new List<string>();
            List<ConfigurationStatus.SCPlan> pTempPlanList = new List<ConfigurationStatus.SCPlan>();
            Dictionary<string, List<ConfigurationStatus.SCPlan>> dictionary = new Dictionary<string, List<ConfigurationStatus.SCPlan>>();
            foreach (ConfigurationStatus.SCPlan plan in pBets.PlanList)
            {
                if (!list.Contains(plan.CurrentExpect))
                {
                    list.Add(plan.CurrentExpect);
                }
                string currentExpect = plan.CurrentExpect;
                if (!dictionary.ContainsKey(currentExpect))
                {
                    List<ConfigurationStatus.SCPlan> list3 = new List<ConfigurationStatus.SCPlan> {
                        plan
                    };
                    dictionary[currentExpect] = list3;
                }
                else
                {
                    dictionary[currentExpect].Add(plan);
                }
            }
            ConfigurationStatus.BetsFNTotal pFNTotal = new ConfigurationStatus.BetsFNTotal();
            foreach (string str in dictionary.Keys)
            {
                List<ConfigurationStatus.SCPlan> list4 = dictionary[str];
                foreach (ConfigurationStatus.SCPlan plan in list4)
                {
                    this.CombinaPlanValue(plan, pTempPlanList, pFNTotal);
                }
            }
            List<Color> list5 = new List<Color> {
                AppInfo.lightRedColor,
                AppInfo.lightYellowColor,
                AppInfo.lightGreenColor
            };
            int num = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                int num3 = num++ % list5.Count;
                Color color = list5[num3];
                pBets.ExpectBackColorDic[list[i]] = color;
            }
            if (this.Ckb_BetsSort.Checked)
            {
                pTempPlanList.Reverse();
            }
            pBets.PlanViewList = pTempPlanList;
        }

        private bool CountTimes(ConfigurationStatus.BetsScheme pBetsScheme, ref string pHint)
        {
            ConfigurationStatus.SCTimesCount count = new ConfigurationStatus.SCTimesCount {
                Cycle = 1,
                Unit = pBetsScheme.SchemeInfo.FNBaseInfo.Unit
            };
            count.UnitIndex = Convert.ToInt32(count.Unit) + 1;
            count.BTType = pBetsScheme.SchemeInfo.FNBaseInfo.BTType;
            if (count.BTType == ConfigurationStatus.SCTimesType.Plan)
            {
                List<double> list = this.GetBTMoney(pBetsScheme.BTPlanList, pBetsScheme, ref pHint);
                if (pHint != "")
                {
                    return false;
                }
                count.TimesList["0"] = list;
                count.FBList["0"] = pBetsScheme.FBInfo;
            }
            else if (count.BTType == ConfigurationStatus.SCTimesType.FN)
            {
                string bTFNName = pBetsScheme.BTFNName;
                if (!AppInfo.BTFNDic.ContainsKey(bTFNName))
                {
                    pHint = $"倍投【{bTFNName}】不存在，请重新选择！";
                    return false;
                }
                count.FNName = bTFNName;
                List<ConfigurationStatus.TimesScheme> timesSchemeList = AppInfo.BTFNDic[bTFNName].TimesSchemeList;
                this.SortTimesList(timesSchemeList);
                for (int i = 1; i <= timesSchemeList.Count; i++)
                {
                    if (timesSchemeList[i - 1].ID != i)
                    {
                        pHint = $"倍投【{bTFNName}】列表不完整！缺少局数【{i}】";
                        return false;
                    }
                }
                List<int> list3 = new List<int>();
                foreach (ConfigurationStatus.TimesScheme scheme in timesSchemeList)
                {
                    list3.Add(scheme.ID);
                }
                foreach (ConfigurationStatus.TimesScheme scheme in timesSchemeList)
                {
                    if (!(list3.Contains(scheme.YesAfter) && list3.Contains(scheme.NoAfter)))
                    {
                        pHint = $"倍投【{bTFNName}】中第【{scheme.ID}】局设置不正确！";
                        return false;
                    }
                }
                count.FNTimesList = timesSchemeList;
                count.TimesList["0"] = count.GetFNTimesList;
            }
            pBetsScheme.Times = count;
            return true;
        }

        private void DeleteQQGData(ConfigurationStatus.BetsScheme pScheme, string pExpect)
        {
            List<string> dicKeyList = CommFunc.GetDicKeyList<ConfigurationStatus.BetsCode>(pScheme.FNNumberDic);
            foreach (string str in dicKeyList)
            {
                if (ConfigurationStatus.FNBase.GetExpectZuKey(str) == pExpect)
                {
                    pScheme.FNNumberDic.Remove(str);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Egv_BetsList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if (this.Egv_PlanList.RowCount != 0)
                {
                    ConfigurationStatus.AutoBets bets = this.GetBets("");
                    List<ConfigurationStatus.SCPlan> planViewList = bets.PlanViewList;
                    if ((planViewList.Count != 0) && (this.Egv_PlanList.RowCount == planViewList.Count))
                    {
                        DataGridViewCell cell = this.Egv_PlanList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        ConfigurationStatus.SCPlan plan = planViewList[e.RowIndex];
                        string lotteryName = "";
                        if (!plan.IsNull)
                        {
                            cell.Style.BackColor = bets.ExpectBackColorDic.ContainsKey(plan.CurrentExpect) ? bets.ExpectBackColorDic[plan.CurrentExpect] : AppInfo.whiteColor;
                            if (e.ColumnIndex == 0)
                            {
                                lotteryName = plan.UploadTime.ToString("HH:mm:ss");
                            }
                            else if (e.ColumnIndex == 1)
                            {
                                lotteryName = plan.LotteryName;
                            }
                            else if (e.ColumnIndex == 2)
                            {
                                lotteryName = plan.CurrentExpect;
                            }
                            else if (e.ColumnIndex == 3)
                            {
                                lotteryName = plan.FNName;
                            }
                            else if (e.ColumnIndex == 4)
                            {
                                lotteryName = plan.ViewPlay;
                            }
                            else if (e.ColumnIndex == 5)
                            {
                                lotteryName = plan.FNAutoNumbe;
                            }
                            else if (e.ColumnIndex == 6)
                            {
                                lotteryName = plan.FNAutoTimes;
                            }
                            else if (e.ColumnIndex == 7)
                            {
                                lotteryName = plan.FNBTIndex;
                            }
                            else if (e.ColumnIndex == 8)
                            {
                                lotteryName = plan.FNAutoMoney;
                            }
                            else
                            {
                                Color color;
                                if (e.ColumnIndex == 9)
                                {
                                    lotteryName = plan.FNAutoGain;
                                    color = plan.CheckPlanIsWait() ? AppInfo.blackColor : (plan.CheckPlanStringIsWIn() ? AppInfo.redForeColor : AppInfo.blueForeColor);
                                    cell.Style.SelectionForeColor = cell.Style.ForeColor = color;
                                }
                                else if (e.ColumnIndex == 10)
                                {
                                    lotteryName = CommFunc.TwoDouble(plan.GetFNGain, true);
                                    color = (plan.GetFNGain > 0.0) ? AppInfo.redForeColor : AppInfo.blueForeColor;
                                    cell.Style.SelectionForeColor = cell.Style.ForeColor = color;
                                }
                                else if (e.ColumnIndex == 11)
                                {
                                    lotteryName = plan.FNLGString;
                                }
                                else if (e.ColumnIndex == 12)
                                {
                                    lotteryName = plan.FNLZString;
                                }
                                else if (e.ColumnIndex == 13)
                                {
                                    lotteryName = CommFunc.CheckPlayIsDS(plan.ViewPlay) ? "详细请点击" : plan.FNNumberString;
                                }
                                else if (e.ColumnIndex == 14)
                                {
                                    lotteryName = plan.Code;
                                }
                                else if (e.ColumnIndex == 15)
                                {
                                    lotteryName = plan.State;
                                }
                                else if (e.ColumnIndex == 0x10)
                                {
                                    lotteryName = plan.BetsState;
                                }
                            }
                        }
                        e.Value = lotteryName;
                    }
                }
            }
            catch
            {
            }
        }

        private void Egv_BTFNList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.RefreshBTFNTimesList();
        }

        private void Egv_BTFNList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if ((this.Egv_BTFNMain.RowCount != 0) && (AppInfo.BTFNList.Count != 0))
            {
                e.Value = AppInfo.BTFNList[e.RowIndex];
            }
        }

        private void Egv_BTFNTimesList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.Ckb_EditTimes_Click(null, null);
        }

        private void Egv_BTFNTimesList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (this.Egv_BTFNTimesList.RowCount != 0)
            {
                string bTFNName = this.GetBTFNName();
                if (bTFNName != "")
                {
                    List<ConfigurationStatus.TimesScheme> timesSchemeList = AppInfo.BTFNDic[bTFNName].TimesSchemeList;
                    if ((timesSchemeList.Count != 0) && (e.RowIndex < timesSchemeList.Count))
                    {
                        ConfigurationStatus.TimesScheme scheme = timesSchemeList[e.RowIndex];
                        string str2 = "";
                        if (e.ColumnIndex == 0)
                        {
                            str2 = $"第{scheme.ID}局";
                        }
                        else if (e.ColumnIndex == 1)
                        {
                            str2 = scheme.Times.ToString();
                        }
                        else if (e.ColumnIndex == 2)
                        {
                            str2 = $"第{scheme.YesAfter}局";
                            if (scheme.YesJK)
                            {
                                str2 = str2 + "+【重新监控】";
                            }
                            if (scheme.GetYesOtherFN != "")
                            {
                                str2 = str2 + $"+跳转【{scheme.GetYesOtherFN}】";
                            }
                        }
                        else if (e.ColumnIndex == 3)
                        {
                            str2 = $"第{scheme.NoAfter}局";
                            if (scheme.NoJK)
                            {
                                str2 = str2 + "+【重新监控】";
                            }
                            if (scheme.GetNoOtherFN != "")
                            {
                                str2 = str2 + $"+跳转【{scheme.GetNoOtherFN}】";
                            }
                        }
                        e.Value = str2;
                    }
                }
            }
        }

        private void Egv_DataList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if ((this.Egv_DataList.RowCount != 0) && (AppInfo.FilterList.Count != 0))
            {
                if (e.ColumnIndex == 0)
                {
                    e.Value = AppInfo.FilterList[e.RowIndex].Expect;
                }
                else if (e.ColumnIndex == 1)
                {
                    e.Value = AppInfo.FilterList[e.RowIndex].Code;
                }
            }
        }

        private void Egv_LSDataList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if ((this.Egv_LSDataList.RowCount != 0) && (this.LSDataViewList.Count != 0))
                {
                    DataGridViewCell cell = this.Egv_LSDataList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    ConfigurationStatus.LSDataView view = this.LSDataViewList[e.RowIndex];
                    string selectLSFN = "";
                    if (e.ColumnIndex == 0)
                    {
                        selectLSFN = this.GetSelectLSFN();
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        selectLSFN = view.Value;
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        cell.Style.ForeColor = cell.Style.SelectionForeColor = view.IsBJ ? AppInfo.redForeColor : AppInfo.blueForeColor;
                        selectLSFN = view.LZExpect;
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        selectLSFN = view.PerLZExpect;
                    }
                    else if (e.ColumnIndex == 4)
                    {
                        selectLSFN = view.LGExpect;
                    }
                    else if (e.ColumnIndex == 5)
                    {
                        selectLSFN = view.PerExpect;
                    }
                    else if (e.ColumnIndex == 6)
                    {
                        selectLSFN = view.TodayExpect;
                    }
                    else if (e.ColumnIndex == 7)
                    {
                        selectLSFN = view.YesterdayExpect;
                    }
                    else if (e.ColumnIndex == 8)
                    {
                        selectLSFN = view.WeekExpect;
                    }
                    e.Value = selectLSFN;
                    if (e.ColumnIndex != 2)
                    {
                        cell.Style.SelectionForeColor = cell.Style.ForeColor = view.IsBJ ? AppInfo.redForeColor : AppInfo.blackColor;
                    }
                }
            }
            catch
            {
            }
        }

        private void Egv_NoticeList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                string pKey = this.NoticeList[e.RowIndex];
                string pValue = this.NoticeDic[pKey];
                new FrmNotice(pKey, pValue).ShowDialog();
            }
        }

        private void Egv_NoticeList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if ((this.Egv_NoticeList.RowCount != 0) && (this.NoticeList.Count != 0))
                {
                    string str = this.NoticeList[e.RowIndex];
                    DataGridViewCell cell = this.Egv_NoticeList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (e.ColumnIndex == 0)
                    {
                        e.Value = str;
                    }
                }
            }
            catch
            {
            }
        }

        private void Egv_PlanList_ButtonClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.Egv_PlanList.RowCount != 0)
            {
                List<ConfigurationStatus.SCPlan> planViewList = this.GetBets("").PlanViewList;
                if ((planViewList.Count != 0) && (this.Egv_PlanList.RowCount == planViewList.Count))
                {
                    ConfigurationStatus.SCPlan plan = planViewList[e.RowIndex];
                    new FrmViewValue(plan.FNNumberString, plan.PlayInfo).ShowDialog();
                }
            }
        }

        private void Egv_PTLineList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if ((this.Egv_PTLineList.RowCount != 0) && (AppInfo.PTInfo.LineList.Count != 0))
            {
                DataGridViewCell cell = this.Egv_PTLineList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string pResponseText = "";
                if (e.ColumnIndex == 0)
                {
                    pResponseText = AppInfo.PTInfo.LineList[e.RowIndex];
                    pResponseText = AppInfo.PTInfo.GetLineString(pResponseText);
                }
                else if (e.ColumnIndex == 1)
                {
                    if (AppInfo.PTInfo.LoginUrl == AppInfo.PTInfo.LineList[e.RowIndex])
                    {
                        pResponseText = AppInfo.PTInfo.IsLoginRun ? "登录中..." : "使用";
                        cell.Style.SelectionForeColor = cell.Style.ForeColor = AppInfo.PTInfo.IsLoginRun ? AppInfo.redForeColor : AppInfo.greenForeColor;
                    }
                    else
                    {
                        pResponseText = "";
                    }
                }
                e.Value = pResponseText;
            }
        }

        private void Egv_SchemeList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.Ckb_EditScheme_Click(null, null);
        }

        private void Egv_SchemeList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if ((this.Egv_SchemeList.RowCount != 0) && (AppInfo.SchemeList.Count != 0))
                {
                    string str = AppInfo.SchemeList[e.RowIndex];
                    ConfigurationStatus.Scheme scheme = AppInfo.Account.SchemeDic[str];
                    DataGridViewCell cell = this.Egv_SchemeList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (e.ColumnIndex == 0)
                    {
                        e.Value = scheme.Selected;
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        e.Value = scheme.ViewName;
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        e.Value = scheme.CHType;
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        e.Value = scheme.FNBaseInfo.ViewPlay;
                    }
                    cell.ToolTipText = "双击可编辑修改方案";
                }
            }
            catch
            {
            }
        }

        private void Egv_SchemeList_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if (((this.Egv_SchemeList.RowCount != 0) && (AppInfo.SchemeList.Count != 0)) && (e.ColumnIndex == 0))
                {
                    string str = AppInfo.SchemeList[e.RowIndex];
                    ConfigurationStatus.Scheme scheme = AppInfo.Account.SchemeDic[str];
                    bool flag = Convert.ToBoolean(e.Value);
                    scheme.Selected = flag;
                    this.SaveSchemeData(scheme.Name, "");
                }
            }
            catch
            {
            }
        }

        private void Egv_SchmeeList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ConfigurationStatus.Scheme pInfo = this.GetScheme("");
            if (pInfo != null)
            {
                this.RefreshSchemeControl(pInfo);
            }
        }

        private void Egv_ShowTapList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if ((this.Egv_ShowTapList.RowCount != 0) && (this.ShowTapList.Count != 0))
                {
                    ConfigurationStatus.ShowTap tap = this.ShowTapList[e.RowIndex];
                    if (e.ColumnIndex == 0)
                    {
                        e.Value = Convert.ToBoolean(tap.Selected);
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        e.Value = tap.Name;
                    }
                }
            }
            catch
            {
            }
        }

        private void Egv_ShowTapList_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if (((this.Egv_ShowTapList.RowCount != 0) && (this.ShowTapList.Count != 0)) && (e.ColumnIndex == 0))
                {
                    ConfigurationStatus.ShowTap pInfo = this.ShowTapList[e.RowIndex];
                    bool flag = Convert.ToBoolean(e.Value);
                    pInfo.Selected = flag;
                    this.SetTapVis(pInfo, false);
                }
            }
            catch
            {
            }
        }

        private void Egv_TJDataList1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if ((this.Egv_TJDataList1.RowCount != 0) && (this.TJViewList1.Count != 0))
                {
                    DataGridViewCell cell = this.Egv_TJDataList1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    ConfigurationStatus.TJDataView1 view = this.TJViewList1[e.RowIndex];
                    string date = "";
                    if (e.ColumnIndex == 0)
                    {
                        date = view.Date;
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        date = view.BetsCount.ToString();
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        date = view.YesCount.ToString();
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        date = view.NoCount.ToString();
                    }
                    else if (e.ColumnIndex == 4)
                    {
                        date = CommFunc.TwoDouble(view.MaxYL, true);
                    }
                    else if (e.ColumnIndex == 5)
                    {
                        date = CommFunc.TwoDouble(view.MaxKS, true);
                    }
                    else if (e.ColumnIndex == 6)
                    {
                        date = CommFunc.TwoDouble(view.XZMoney, true);
                    }
                    else if (e.ColumnIndex == 7)
                    {
                        date = CommFunc.TwoDouble(view.Gain, true);
                        cell.Style.SelectionForeColor = cell.Style.ForeColor = (Convert.ToDouble(date) > 0.0) ? AppInfo.redForeColor : AppInfo.blueForeColor;
                    }
                    else if (e.ColumnIndex == 8)
                    {
                        date = view.MaxLC.ToString();
                    }
                    else if (e.ColumnIndex == 9)
                    {
                        date = view.ZRCount.ToString();
                    }
                    else if (e.ColumnIndex == 10)
                    {
                        date = view.ZRValue;
                    }
                    e.Value = date;
                }
            }
            catch
            {
            }
        }

        private void Egv_TJDataList2_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if ((this.Egv_TJDataList2.RowCount != 0) && (this.TJViewList2.Count != 0))
                {
                    DataGridViewCell cell = this.Egv_TJDataList2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    ConfigurationStatus.TJDataView2 view = this.TJViewList2[e.RowIndex];
                    string date = "";
                    if (e.ColumnIndex == 0)
                    {
                        date = view.Date;
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        date = view.Time;
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        date = view.Expect;
                    }
                    else if ((e.ColumnIndex >= 3) && (e.ColumnIndex < 8))
                    {
                        date = view.CodeList[e.ColumnIndex - 3];
                    }
                    else if ((e.ColumnIndex >= 15) && (e.ColumnIndex <= 0x11))
                    {
                        string headerText = this.Egv_TJDataList2.Columns[e.ColumnIndex].HeaderText;
                        if (headerText.Contains("当期"))
                        {
                            date = CommFunc.TwoDouble(view.Gain1, true);
                        }
                        else if (headerText.Contains("当日"))
                        {
                            date = CommFunc.TwoDouble(view.Gain2, true);
                        }
                        else if (headerText.Contains("累计"))
                        {
                            date = CommFunc.TwoDouble(view.Gain3, true);
                        }
                        cell.Style.SelectionForeColor = cell.Style.ForeColor = (Convert.ToDouble(date) > 0.0) ? AppInfo.redForeColor : AppInfo.blueForeColor;
                    }
                    else if (view.ValueDic.ContainsKey("0"))
                    {
                        ConfigurationStatus.SCPlan plan = view.ValueDic["0"];
                        if (e.ColumnIndex == 8)
                        {
                            date = plan.FNAutoNumbe;
                        }
                        else if (e.ColumnIndex == 9)
                        {
                            date = plan.FNAutoTimes;
                        }
                        else if (e.ColumnIndex == 10)
                        {
                            date = plan.FNBTIndex;
                        }
                        else if (e.ColumnIndex == 11)
                        {
                            date = plan.FNAutoMoney;
                        }
                        else if (e.ColumnIndex == 12)
                        {
                            date = plan.FNNumberString;
                        }
                        else if (e.ColumnIndex == 13)
                        {
                            date = plan.FNAutoState;
                        }
                        else if (e.ColumnIndex == 14)
                        {
                            date = plan.TJBZCountString;
                            cell.Style.ForeColor = AppInfo.whiteColor;
                            cell.Style.BackColor = (date.Replace("\r\n", "") == "") ? AppInfo.whiteColor : AppInfo.greenBackColor;
                        }
                        if (e.ColumnIndex != 14)
                        {
                            cell.Style.SelectionForeColor = cell.Style.ForeColor = plan.IsWin ? AppInfo.redForeColor : AppInfo.blueForeColor;
                        }
                    }
                    else if (e.ColumnIndex == 14)
                    {
                        cell.Style.BackColor = AppInfo.whiteColor;
                    }
                    e.Value = date;
                }
            }
            catch
            {
            }
        }

        private void FillNextTime()
        {
            if (AppInfo.NextTime.Hour >= 12)
            {
                this.ResetTime();
            }
            string format = "HH   mm   ss";
            this.Lbl_NextTime.Text = (AppInfo.NextTime.Hour == 0) ? AppInfo.NextTime.ToString(format) : AppInfo.NextTime.ToString(format);
            this.Lbl_NextTime1.Text = (AppInfo.NextTime.Hour == 0) ? AppInfo.NextTime.ToString("mm:ss") : AppInfo.NextTime.ToString("HH:mm:ss");
        }

        private void FillPTLineList()
        {
            ConfigurationStatus.PTLine line = this.PTLineDic[this.PTName];
            AppInfo.PTInfo.PTID = line.ID;
            AppInfo.PTInfo.PTName = line.Name;
            AppInfo.PTInfo.LineList.Clear();
            List<string> lineList = line.LineList;
            AppInfo.PTInfo.LineList = PTBase.GetSkipUrlList(lineList, AppInfo.PTInfo);
            this.RefreshPTLineList();
        }

        private List<ConfigurationStatus.OpenData> FilterLSOpenData(DateTime pDate)
        {
            List<ConfigurationStatus.OpenData> list = new List<ConfigurationStatus.OpenData>();
            DateTime date = pDate.AddDays(-31.0).Date;
            DateTime time2 = pDate.Date;
            for (int i = 0; i < AppInfo.DataList.Count; i++)
            {
                ConfigurationStatus.OpenData item = AppInfo.DataList[i];
                DateTime time3 = item.Time.Date;
                if ((time3 >= date) && (time3 <= time2))
                {
                    list.Add(item);
                }
                else if (time3 < date)
                {
                    return list;
                }
            }
            return list;
        }

        public void FilterOpenData()
        {
            AppInfo.FilterList.Clear();
            int num = 120;
            if (num == 0)
            {
                num = AppInfo.DataList.Count;
            }
            if (num > AppInfo.DataList.Count)
            {
                num = AppInfo.DataList.Count;
            }
            for (int i = 0; i < num; i++)
            {
                AppInfo.FilterList.Add(AppInfo.DataList[i]);
            }
            int count = AppInfo.FilterList.Count;
            CommFunc.RefreshDataGridView(this.Egv_DataList, count);
        }

        private List<ConfigurationStatus.OpenData> FilterTJOpenData(ConfigurationStatus.TJData pInfo)
        {
            List<ConfigurationStatus.OpenData> list = new List<ConfigurationStatus.OpenData>();
            DateTime startDate = pInfo.StartDate;
            DateTime endDate = pInfo.EndDate;
            for (int i = 0; i < AppInfo.DataList.Count; i++)
            {
                ConfigurationStatus.OpenData item = AppInfo.DataList[i];
                DateTime date = item.Time.Date;
                if ((date >= startDate) && (date <= endDate))
                {
                    if (pInfo.TimeSelect)
                    {
                        DateTime time4 = DateTime.Parse(item.Time.ToLongTimeString());
                        DateTime time5 = DateTime.Parse(pInfo.StartTime);
                        DateTime time6 = DateTime.Parse(pInfo.EndTime);
                        if ((time4 < time5) || (time4 > time6))
                        {
                            continue;
                        }
                    }
                    list.Add(item);
                }
                else if (date < startDate)
                {
                    return list;
                }
            }
            return list;
        }

        public double GetBetMoney(ConfigurationStatus.SCUnitType pUnitType)
        {
            double num = 0.0;
            if (pUnitType == ConfigurationStatus.SCUnitType.Yuan)
            {
                num = 2.0;
            }
            else if (pUnitType == ConfigurationStatus.SCUnitType.Jiao)
            {
                num = 0.2;
            }
            else if (pUnitType == ConfigurationStatus.SCUnitType.Fen)
            {
                num = 0.02;
            }
            else if (pUnitType == ConfigurationStatus.SCUnitType.Li)
            {
                num = 0.002;
            }
            if (AppInfo.PTInfo.IsBetsMoney1(pUnitType))
            {
                num /= 2.0;
            }
            return num;
        }

        private ConfigurationStatus.AutoBets GetBets(string pName = "")
        {
            ConfigurationStatus.AutoBets bets = null;
            if (pName == "")
            {
                pName = "自动投注";
            }
            if (this.BetsDic.ContainsKey(pName))
            {
                return this.BetsDic[pName];
            }
            bets = new ConfigurationStatus.AutoBets(pName);
            this.BetsDic[pName] = bets;
            return bets;
        }

        private string GetBetsCodeChar() => 
            ";";

        private HtmlDocument GetBetsDocument() => 
            this.Web_Login.Document.Window.Frames[0].Document;

        private string GetBTFNName()
        {
            string str = "";
            if (this.Egv_BTFNMain.SelectedRows.Count != 0)
            {
                str = this.Egv_BTFNMain.SelectedRows[0].Cells[0].Value.ToString();
            }
            return str;
        }

        private List<double> GetBTMoney(List<string> pTimesBTList, ConfigurationStatus.BetsScheme pBetsScheme, ref string pHint)
        {
            List<double> list = new List<double>();
            foreach (string str in pTimesBTList)
            {
                string pError = "";
                if (str != "")
                {
                    if (!CommFunc.CheckBetsTimes(str, ref pError))
                    {
                        pHint = $"【{pBetsScheme.SchemeInfo.Name}】{pError}";
                        return null;
                    }
                    double item = CommFunc.ConvertBetTimes(str);
                    list.Add(item);
                }
            }
            return list;
        }

        public string GetDefultBTFNName()
        {
            string item = "";
            int num = 1;
            while (true)
            {
                item = $"方案{num}";
                if (!AppInfo.BTFNList.Contains(item))
                {
                    return item;
                }
                num++;
            }
        }

        public string GetDefultSchemeName()
        {
            string item = "";
            int num = 1;
            while (true)
            {
                item = $"方案{num}";
                if (!AppInfo.SchemeList.Contains(item))
                {
                    return item;
                }
                num++;
            }
        }

        public int GetDefultTimesID(List<int> pIDList)
        {
            int item = 1;
            while (true)
            {
                if (!pIDList.Contains(item))
                {
                    return item;
                }
                item++;
            }
        }

        private string GetFBExpect(List<ConfigurationStatus.ExpectCount> pExpectList, DateTime pDate, int pDay)
        {
            string str = "";
            DateTime date = pDate.AddDays((double) (pDay * -1)).Date;
            DateTime time2 = pDate.Date;
            if (pDay != 0)
            {
                time2 = time2.AddDays(-1.0);
            }
            int num = -1;
            List<int> list = new List<int>();
            for (int i = pExpectList.Count - 1; i >= 0; i--)
            {
                ConfigurationStatus.ExpectCount count = pExpectList[i];
                DateTime time3 = Convert.ToDateTime(count.Data);
                if ((time3 >= date) && (time3 <= time2))
                {
                    int item = count.Count;
                    if (item > num)
                    {
                        num = item;
                    }
                    list.Add(item);
                }
                else if (time3 < date)
                {
                    break;
                }
            }
            list.Sort();
            list.Reverse();
            if (num != -1)
            {
                str = num.ToString();
            }
            return str;
        }

        private void GetFNLSData(Dictionary<string, ConfigurationStatus.BetsCode> pFNNumberDic, ConfigurationStatus.BetsScheme pScheme)
        {
            ConfigurationStatus.FNBase fNBaseInfo = pScheme.SchemeInfo.FNBaseInfo;
            ConfigurationStatus.LSData lSDataInfo = pScheme.LSDataInfo;
            this.RefreshLSDataMain(lSDataInfo);
        }

        public HtmlDocument GetLoginDocument() => 
            this.Web_Login.Document;

        public string GetLoginUrl() => 
            this.Web_Login.Url.ToString();

        private ConfigurationStatus.LSData GetLSDataInfo()
        {
            ConfigurationStatus.LSData data = new ConfigurationStatus.LSData {
                LotteryName = this.Lbl_LSLotteryValue.Text
            };
            data.LotteryID = AppInfo.LotterNameDic[data.LotteryName];
            data.IsBJ = this.Ckb_LSBJ.Checked;
            data.Date = this.Dtp_LSDataRange.Value.Date;
            data.LSBJTypeIndex = this.Cbb_LSBJType.SelectedIndex;
            data.BJExpectSelect = this.Rdb_LSBJExpect.Checked;
            data.BJExpectValue = Convert.ToInt32(this.Nm_LSBJExpect.Value);
            return data;
        }

        private ConfigurationStatus.LSDataView GetLSDataRow()
        {
            ConfigurationStatus.LSDataView view = null;
            if ((this.Egv_LSDataList.Rows.Count != 0) && (this.Egv_LSDataList.SelectedRows.Count != 0))
            {
                int index = this.Egv_LSDataList.SelectedRows[0].Index;
                view = this.LSDataViewList[index];
            }
            return view;
        }

        private string GetNotice()
        {
            string str = "";
            if ((this.Egv_NoticeList.Rows.Count != 0) && (this.Egv_NoticeList.SelectedRows.Count != 0))
            {
                int index = this.Egv_NoticeList.SelectedRows[0].Index;
                str = this.NoticeList[index];
            }
            return str;
        }

        public ConfigurationStatus.OpenData GetOpenDataByExpect(string pExpect, List<ConfigurationStatus.OpenData> pDataList = null)
        {
            if (pDataList == null)
            {
                pDataList = AppInfo.DataList;
            }
            for (int i = 0; i < pDataList.Count; i++)
            {
                ConfigurationStatus.OpenData data2 = pDataList[i];
                if (data2.Expect == pExpect)
                {
                    return data2;
                }
            }
            return null;
        }

        public int GetOpenDataIndexByExpect(string pExpect, List<ConfigurationStatus.OpenData> pDataList = null)
        {
            if (pDataList == null)
            {
                pDataList = AppInfo.DataList;
            }
            for (int i = 0; i < pDataList.Count; i++)
            {
                if (pDataList[i].Expect == pExpect)
                {
                    return i;
                }
            }
            return -1;
        }

        private bool GetPlanNumberList(ConfigurationStatus.AutoBets pBets, ConfigurationStatus.SCPlan plan, ConfigurationStatus.BetsScheme pScheme, ref List<string> pList)
        {
            pList.Clear();
            List<string> list = new List<string>();
            string currentExpect = plan.CurrentExpect;
            Dictionary<string, ConfigurationStatus.BetsCode> pFNNumberDic = pScheme.SchemeInfo.FNBaseInfo.CountNumber(pScheme, AppInfo.DataList, 0);
            if (!pScheme.SchemeInfo.FNBaseInfo.IsBetsZJ)
            {
                this.ReverseCode(ref pFNNumberDic, pScheme.SchemeInfo.FNBaseInfo);
            }
            if (pScheme.IsQQG)
            {
                this.AddQQGData(ref pFNNumberDic, pScheme, currentExpect);
            }
            this.CheckFNYKStop(ref pFNNumberDic, pBets, pScheme, plan.GetFNGain);
            this.CheckFNBetsTime(ref pFNNumberDic, pBets, pScheme);
            if (pBets.IsStopAddFN || pScheme.IsStopAddFN)
            {
                this.StopAddFN(ref pFNNumberDic, pScheme.FNBTIndexDic);
            }
            ConfigurationStatus.FNBase.AddFNBTIndex(pFNNumberDic, pScheme.FNBTIndexDic);
            pScheme.FNNumberDic = pFNNumberDic;
            pScheme.PTNumberDic = pScheme.ConvertFNBetsCode(pBets, plan, pScheme, pScheme.FNNumberDic);
            pList = pScheme.GetPTNumberList;
            if (pList.Count == 0)
            {
                pBets.ErrorState = $"{pBets.Expect}期【{pScheme.SchemeInfo.Name}】没有符合条件的投注";
                return false;
            }
            foreach (string str2 in pList)
            {
                if (str2 == "")
                {
                    pBets.ErrorState = "投注号码不正确";
                    return false;
                }
            }
            if (((pList.Count > 0) && (pScheme.DSJEndBetsTime == DateTime.MinValue)) && (pScheme.SchemeInfo.FNBaseInfo.GetDJSEndTime != ""))
            {
                DateTime time = DateTime.Parse(pScheme.SchemeInfo.FNBaseInfo.GetDJSEndTime);
                pScheme.DSJEndBetsTime = DateTime.Now.AddHours((double) time.Hour).AddMinutes((double) time.Minute);
            }
            return true;
        }

        private string GetPTLineUrl()
        {
            string str = "";
            if (this.Egv_PTLineList.SelectedRows.Count != 0)
            {
                str = this.Egv_PTLineList.SelectedRows[0].Cells[0].Value.ToString();
            }
            return str;
        }

        private ConfigurationStatus.Scheme GetScheme(string pName = "")
        {
            if (pName == "")
            {
                pName = this.GetSchemeName();
            }
            if (pName == "")
            {
                return null;
            }
            return AppInfo.Account.SchemeDic[pName];
        }

        private bool GetSchemeControlInfo(ref ConfigurationStatus.Scheme pInfo)
        {
            ConfigurationStatus.FNBase controlValue;
            string pError = "";
            if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.DMLH)
            {
                controlValue = this.FN_DMLH.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    pInfo.FNBaseInfo = controlValue;
                }
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GJDMLH)
            {
                controlValue = this.FN_GJDMLH.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    pInfo.FNBaseInfo = controlValue;
                }
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GJKMTM)
            {
                controlValue = this.FN_GJKMTM.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    pInfo.FNBaseInfo = controlValue;
                }
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.LRWCH)
            {
                controlValue = this.FN_LRWCH.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    pInfo.FNBaseInfo = controlValue;
                }
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.BCFCH)
            {
                controlValue = this.FN_BCFCH.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    pInfo.FNBaseInfo = controlValue;
                }
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.WJJH)
            {
                controlValue = this.FN_WJJH.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    pInfo.FNBaseInfo = controlValue;
                }
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.SJCH)
            {
                controlValue = this.FN_SJCH.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    pInfo.FNBaseInfo = controlValue;
                }
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.YLCH)
            {
                controlValue = this.FN_YLCH.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    pInfo.FNBaseInfo = controlValue;
                }
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.KMTM)
            {
                controlValue = this.FN_KMTM.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    pInfo.FNBaseInfo = controlValue;
                }
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GDQM)
            {
                controlValue = this.FN_GDQM.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    pInfo.FNBaseInfo = controlValue;
                }
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.LHKMTM)
            {
                controlValue = this.FN_LHKMTM.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    pInfo.FNBaseInfo = controlValue;
                }
            }
            if (pError != "")
            {
                CommFunc.PublicMessageAll(pError, true, MessageBoxIcon.Asterisk, "");
                return false;
            }
            return true;
        }

        private string GetSchemeName()
        {
            string str = "";
            if ((this.Egv_SchemeList.Rows.Count != 0) && (this.Egv_SchemeList.SelectedRows.Count != 0))
            {
                int index = this.Egv_SchemeList.SelectedRows[0].Index;
                str = AppInfo.SchemeList[index];
            }
            return str;
        }

        public string GetSelectLotteryID()
        {
            string text = this.Cbb_Lottery.Text;
            return AppInfo.LotterNameDic[text];
        }

        public string GetSelectLSFN() => 
            this.Cbb_LSFN.Text;

        public string GetSelectTJFN() => 
            this.Cbb_TJFN.Text;

        private void GetSharePlanListByIndex(ConfigurationStatus.AutoBets pBets, List<ConfigurationStatus.SCPlan> planList, int pIndex)
        {
            ConfigurationStatus.SCPlan plan = null;
            if (SQLData.GetSharePlanList(pBets, pIndex, ref plan, ref pBets.ShareBetsInfo.FollowErrorHint))
            {
                bool flag = true;
                if (plan.LotteryName == null)
                {
                    flag = false;
                }
                if (flag)
                {
                    ConfigurationStatus.BetsScheme scheme = new ConfigurationStatus.BetsScheme();
                    pBets.BetsSchemeDic[plan.FNName] = scheme;
                    plan.SchemeInfo = scheme;
                    planList.Add(plan);
                }
                if (pBets.ShareBetsInfo.FollowErrorIndexList.Contains(pIndex))
                {
                    pBets.ShareBetsInfo.FollowErrorIndexList.Remove(pIndex);
                }
            }
            else
            {
                pBets.ErrorState = pBets.ShareBetsInfo.FollowErrorHint = $"{pBets.Expect}期发现没有成功获取的共享计划！";
                if (!pBets.ShareBetsInfo.FollowErrorIndexList.Contains(pIndex))
                {
                    pBets.ShareBetsInfo.FollowErrorIndexList.Add(pIndex);
                }
            }
        }

        private Dictionary<string, ConfigurationStatus.ShowTap> GetShowTapDic()
        {
            Dictionary<string, ConfigurationStatus.ShowTap> dictionary = new Dictionary<string, ConfigurationStatus.ShowTap>();
            foreach (ConfigurationStatus.ShowTap tap in this.ShowTapList)
            {
                dictionary[tap.Name] = tap;
            }
            return dictionary;
        }

        private int GetTimesIndex()
        {
            int index = -1;
            if (this.Egv_BTFNTimesList.SelectedRows.Count != 0)
            {
                index = this.Egv_BTFNTimesList.SelectedRows[0].Index;
            }
            return index;
        }

        private string GetTimesName()
        {
            string str = "";
            if (this.Egv_BTFNTimesList.SelectedRows.Count != 0)
            {
                str = this.Egv_BTFNTimesList.SelectedRows[0].Cells[0].Value.ToString();
            }
            return str;
        }

        private ConfigurationStatus.TJDataView1 GetTJDataView1Row()
        {
            ConfigurationStatus.TJDataView1 view = null;
            if ((this.Egv_TJDataList1.Rows.Count != 0) && (this.Egv_TJDataList1.SelectedRows.Count != 0))
            {
                int index = this.Egv_TJDataList1.SelectedRows[0].Index;
                view = this.TJViewList1[index];
            }
            return view;
        }

        private string GetWebVerifyCode(string pVerifyCodeFile)
        {
            string pVerifyCode = "";
            try
            {
                string str2 = $"/?captcha&rd={DateTime.Now.ToOADate()}";
                string pUrl = AppInfo.PTInfo.GetLine() + str2;
                File.Delete(pVerifyCodeFile);
                Bitmap bitmap = new Bitmap(HttpHelper.GetResponseImage(pUrl, "", "GET", "", 0x1770, "UTF-8", true));
                bitmap.Save(pVerifyCodeFile);
                bitmap.Dispose();
                while (!File.Exists(pVerifyCodeFile))
                {
                    Thread.Sleep(500);
                }
                pVerifyCode = VerifyCodeAPI.VerifyCodeMain(AppInfo.PTInfo.PTID, pVerifyCodeFile);
                if (!this.CheckVerifyCode(pVerifyCode))
                {
                    base.Invoke(AppInfo.RefreshVerifyCode);
                    return pVerifyCode;
                }
            }
            catch
            {
            }
            return pVerifyCode;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Pnl_Main = new System.Windows.Forms.Panel();
            this.Pnl_Bets = new System.Windows.Forms.Panel();
            this.Lbl_AppHint = new System.Windows.Forms.Label();
            this.Btn_ViewTop = new System.Windows.Forms.Button();
            this.Tab_Main = new System.Windows.Forms.TabControl();
            this.Tap_PT = new System.Windows.Forms.TabPage();
            this.Pnl_Bets2 = new System.Windows.Forms.Panel();
            this.Web_Login = new System.Windows.Forms.WebBrowser();
            this.Tap_ZDBets = new System.Windows.Forms.TabPage();
            this.Pnl_Bets1 = new System.Windows.Forms.Panel();
            this.Pnl_BetsMain = new System.Windows.Forms.Panel();
            this.Pnl_BetsLeft = new System.Windows.Forms.Panel();
            this.Pnl_BetsInfoMain = new System.Windows.Forms.Panel();
            this.Pnl_BetsRight = new System.Windows.Forms.Panel();
            this.Egv_PlanList = new IntelligentPlanning.ExDataGridView.ExpandGirdView(this.components);
            this.Pnl_PlanListBottom = new System.Windows.Forms.Panel();
            this.Nm_DeleteExpect = new System.Windows.Forms.NumericUpDown();
            this.Ckb_DeleteExpect = new System.Windows.Forms.CheckBox();
            this.Pnl_PlanListTop = new System.Windows.Forms.Panel();
            this.Ckb_ClearBetsList = new System.Windows.Forms.CheckBox();
            this.Lbl_BetsHint = new System.Windows.Forms.Label();
            this.Lbl_ZQLValue = new System.Windows.Forms.Label();
            this.Lbl_ZQLKey = new System.Windows.Forms.Label();
            this.Ckb_PlanShowHide = new System.Windows.Forms.CheckBox();
            this.Lbl_LGMaxValue = new System.Windows.Forms.Label();
            this.Ckb_BetsSort = new System.Windows.Forms.CheckBox();
            this.Lbl_LGMaxKey = new System.Windows.Forms.Label();
            this.Lbl_LZMaxValue = new System.Windows.Forms.Label();
            this.Lbl_LZMaxKey = new System.Windows.Forms.Label();
            this.Lbl_BetsCountValue = new System.Windows.Forms.Label();
            this.Lbl_BetsCountKey = new System.Windows.Forms.Label();
            this.Lbl_MNBetsMoneyPlanValue = new System.Windows.Forms.Label();
            this.Lbl_MNBetsMoneyPlanKey = new System.Windows.Forms.Label();
            this.Lbl_BetsMoneyPlanValue = new System.Windows.Forms.Label();
            this.Lbl_BetsMoneyPlanKey = new System.Windows.Forms.Label();
            this.Lbl_MNBetsGainPlanValue = new System.Windows.Forms.Label();
            this.Lbl_MNBetsGainPlanKey = new System.Windows.Forms.Label();
            this.Lbl_BetsValue = new System.Windows.Forms.Label();
            this.Lbl_BetsKey = new System.Windows.Forms.Label();
            this.Lbl_BetsGainPlanValue = new System.Windows.Forms.Label();
            this.Lbl_BetsGainPlanKey = new System.Windows.Forms.Label();
            this.Pnl_BetsInfoTop = new System.Windows.Forms.Panel();
            this.Pnl_BetsInfoTopMain = new System.Windows.Forms.Panel();
            this.Pnl_BetsInfoTopRight = new System.Windows.Forms.Panel();
            this.Pnl_BetsType = new System.Windows.Forms.Panel();
            this.Lbl_ShareBetsHint = new System.Windows.Forms.Label();
            this.Ckb_ShareBetsManage = new System.Windows.Forms.CheckBox();
            this.Rdb_ShareBets = new System.Windows.Forms.RadioButton();
            this.Rdb_CGBets = new System.Windows.Forms.RadioButton();
            this.Pnl_BetsInfoTopRight1 = new System.Windows.Forms.Panel();
            this.Btn_Bets = new System.Windows.Forms.Button();
            this.Pnl_BetsInfoTopLeft = new System.Windows.Forms.Panel();
            this.Pnl_BetsInfoTop2 = new System.Windows.Forms.Panel();
            this.Pnl_BetsInfoTop2Left = new System.Windows.Forms.Panel();
            this.Ckb_BetsBeginTime = new System.Windows.Forms.CheckBox();
            this.Ckb_BetsEndTime = new System.Windows.Forms.CheckBox();
            this.Cbb_BetsEndType = new System.Windows.Forms.ComboBox();
            this.Dtp_BetsBeginTime = new System.Windows.Forms.DateTimePicker();
            this.Dtp_BetsEndTime = new System.Windows.Forms.DateTimePicker();
            this.Lbl_BetsTime2 = new System.Windows.Forms.Label();
            this.Lbl_BetsTime1 = new System.Windows.Forms.Label();
            this.Nm_BetsTime = new System.Windows.Forms.NumericUpDown();
            this.Pnl_BetsInfoTop1 = new System.Windows.Forms.Panel();
            this.Pnl_BetsInfoExpect = new System.Windows.Forms.Panel();
            this.Ckb_DQStopBets = new System.Windows.Forms.CheckBox();
            this.Ckb_SBStopBets = new System.Windows.Forms.CheckBox();
            this.Pnl_BetsInfoMN = new System.Windows.Forms.Panel();
            this.Pnl_BetsInfoMNRight = new System.Windows.Forms.Panel();
            this.Ckb_MN1 = new System.Windows.Forms.CheckBox();
            this.Lbl_MNBets = new System.Windows.Forms.Label();
            this.Txt_MN3 = new System.Windows.Forms.TextBox();
            this.Ckb_MN4 = new System.Windows.Forms.CheckBox();
            this.Txt_MN1 = new System.Windows.Forms.TextBox();
            this.Ckb_MN3 = new System.Windows.Forms.CheckBox();
            this.Ckb_MN2 = new System.Windows.Forms.CheckBox();
            this.Txt_MN2 = new System.Windows.Forms.TextBox();
            this.Txt_MN4 = new System.Windows.Forms.TextBox();
            this.Ckb_MNBets = new System.Windows.Forms.CheckBox();
            this.Tap_Scheme = new System.Windows.Forms.TabPage();
            this.Pnl_Scheme = new System.Windows.Forms.Panel();
            this.Pnl_SchemeMain = new System.Windows.Forms.Panel();
            this.Pnl_SchemeInfo = new System.Windows.Forms.Panel();
            this.Lbl_FNEncrypt = new System.Windows.Forms.Label();
            this.FN_KMTM = new IntelligentPlanning.CustomControls.FNKMTMLine();
            this.FN_YLCH = new IntelligentPlanning.CustomControls.FNYLCHLine();
            this.FN_GJDMLH = new IntelligentPlanning.CustomControls.FNGJDMLHLine();
            this.FN_SJCH = new IntelligentPlanning.CustomControls.FNSJCHLine();
            this.FN_WJJH = new IntelligentPlanning.CustomControls.FNWJJHLine();
            this.FN_LRWCH = new IntelligentPlanning.CustomControls.FNLRWCHLine();
            this.FN_BCFCH = new IntelligentPlanning.CustomControls.FNBCFCHLine();
            this.FN_GJKMTM = new IntelligentPlanning.CustomControls.FNGJKMTMLine();
            this.FN_LHKMTM = new IntelligentPlanning.CustomControls.FNLHKMTMLine();
            this.FN_DMLH = new IntelligentPlanning.CustomControls.FNDMLHLine();
            this.FN_GDQM = new IntelligentPlanning.CustomControls.FNGDQMLine();
            this.Pnl_SchemeTop2 = new System.Windows.Forms.Panel();
            this.Ckb_CancelScheme = new System.Windows.Forms.CheckBox();
            this.Ckb_SaveScheme = new System.Windows.Forms.CheckBox();
            this.Lbl_FNCHType = new System.Windows.Forms.Label();
            this.Cbb_FNCHType = new System.Windows.Forms.ComboBox();
            this.Txt_FNName = new System.Windows.Forms.TextBox();
            this.Lbl_FNName = new System.Windows.Forms.Label();
            this.Pnl_SchemeLeft = new System.Windows.Forms.Panel();
            this.Egv_SchemeList = new IntelligentPlanning.ExDataGridView.ExpandGirdView(this.components);
            this.Pnl_SchemeBottom = new System.Windows.Forms.Panel();
            this.Pnl_SchemeShare = new System.Windows.Forms.Panel();
            this.Ckb_ShareSchemeManage = new System.Windows.Forms.CheckBox();
            this.Ckb_ShareScheme = new System.Windows.Forms.CheckBox();
            this.Ckb_ClearScheme = new System.Windows.Forms.CheckBox();
            this.Ckb_ExportScheme = new System.Windows.Forms.CheckBox();
            this.Ckb_ImportScheme = new System.Windows.Forms.CheckBox();
            this.Pnl_SchemeTop1 = new System.Windows.Forms.Panel();
            this.Ckb_EditTimesPlan = new System.Windows.Forms.CheckBox();
            this.Ckb_EditScheme = new System.Windows.Forms.CheckBox();
            this.Ckb_DeleteScheme = new System.Windows.Forms.CheckBox();
            this.Ckb_CopyScheme = new System.Windows.Forms.CheckBox();
            this.Ckb_AddScheme = new System.Windows.Forms.CheckBox();
            this.Ckb_FNLT = new System.Windows.Forms.CheckBox();
            this.Tap_LSData = new System.Windows.Forms.TabPage();
            this.Pnl_LSData = new System.Windows.Forms.Panel();
            this.Pnl_LSDataMain = new System.Windows.Forms.Panel();
            this.Egv_LSDataList = new IntelligentPlanning.ExDataGridView.ExpandGirdView(this.components);
            this.Pnl_LSDataTop = new System.Windows.Forms.Panel();
            this.Pnl_LSDataRight = new System.Windows.Forms.Panel();
            this.Ckb_LSStop = new System.Windows.Forms.CheckBox();
            this.Lbl_LSRefreshHint = new System.Windows.Forms.Label();
            this.Pnl_LSDataLeft = new System.Windows.Forms.Panel();
            this.Btn_LSRefresh = new System.Windows.Forms.Button();
            this.Pnl_LSDataTop1 = new System.Windows.Forms.Panel();
            this.Lbl_LSPlayKey = new System.Windows.Forms.Label();
            this.Lbl_LSPlayValue = new System.Windows.Forms.Label();
            this.Lbl_LSLotteryKey = new System.Windows.Forms.Label();
            this.Lbl_LSLotteryValue = new System.Windows.Forms.Label();
            this.Lbl_LSDataRange = new System.Windows.Forms.Label();
            this.Dtp_LSDataRange = new System.Windows.Forms.DateTimePicker();
            this.Lbl_LSFN = new System.Windows.Forms.Label();
            this.Ckb_LSAutoRefresh = new System.Windows.Forms.CheckBox();
            this.Cbb_LSFN = new System.Windows.Forms.ComboBox();
            this.Rdb_LSBJExpect = new System.Windows.Forms.RadioButton();
            this.Ckb_LSBJ = new System.Windows.Forms.CheckBox();
            this.Rdb_LSBJType = new System.Windows.Forms.RadioButton();
            this.Cbb_LSBJType = new System.Windows.Forms.ComboBox();
            this.Lbl_LSBJExpect2 = new System.Windows.Forms.Label();
            this.Nm_LSBJExpect = new System.Windows.Forms.NumericUpDown();
            this.Tap_TJData = new System.Windows.Forms.TabPage();
            this.Pnl_TJData = new System.Windows.Forms.Panel();
            this.Pnl_TJDataMain = new System.Windows.Forms.Panel();
            this.Egv_TJDataList2 = new IntelligentPlanning.ExDataGridView.ExpandGirdView(this.components);
            this.Pnl_TJDataTop2 = new System.Windows.Forms.Panel();
            this.Pnl_TJDataHint = new System.Windows.Forms.Label();
            this.Pnl_TJDataFind = new System.Windows.Forms.Panel();
            this.Ckb_AutoSizeTJ = new System.Windows.Forms.CheckBox();
            this.Ckb_TJFindXS = new System.Windows.Forms.CheckBox();
            this.Nm_TJFindXS = new System.Windows.Forms.NumericUpDown();
            this.Btn_TJTop = new System.Windows.Forms.Button();
            this.Lbl_TJFindXS = new System.Windows.Forms.Label();
            this.Egv_TJDataList1 = new IntelligentPlanning.ExDataGridView.ExpandGirdView(this.components);
            this.Pnl_TJDataTop = new System.Windows.Forms.Panel();
            this.Pnl_TJRight2 = new System.Windows.Forms.Panel();
            this.Ckb_TJStop = new System.Windows.Forms.CheckBox();
            this.Lbl_TJRefreshHint = new System.Windows.Forms.Label();
            this.Pnl_TJRight1 = new System.Windows.Forms.Panel();
            this.Btn_TJRefresh = new System.Windows.Forms.Button();
            this.Pnl_TJDataTop1 = new System.Windows.Forms.Panel();
            this.Lbl_TJPlayKey = new System.Windows.Forms.Label();
            this.Lbl_TJLotteryValue = new System.Windows.Forms.Label();
            this.Lbl_TJPlayValue = new System.Windows.Forms.Label();
            this.Cbb_TJPrize = new System.Windows.Forms.ComboBox();
            this.Lbl_TJLotteryKey = new System.Windows.Forms.Label();
            this.Dtp_TJTimeRange2 = new System.Windows.Forms.DateTimePicker();
            this.Lbl_TJDataRange = new System.Windows.Forms.Label();
            this.Lbl_TJTime = new System.Windows.Forms.Label();
            this.Dtp_TJDataRange1 = new System.Windows.Forms.DateTimePicker();
            this.Dtp_TJTimeRange1 = new System.Windows.Forms.DateTimePicker();
            this.Lbl_TJData = new System.Windows.Forms.Label();
            this.Ckb_TJTimeRange = new System.Windows.Forms.CheckBox();
            this.Txt_TJPrize = new System.Windows.Forms.TextBox();
            this.Ckb_TJReset = new System.Windows.Forms.CheckBox();
            this.Lbl_TJPrize = new System.Windows.Forms.Label();
            this.Dtp_TJDataRange2 = new System.Windows.Forms.DateTimePicker();
            this.Lbl_TJFN = new System.Windows.Forms.Label();
            this.Cbb_TJFN = new System.Windows.Forms.ComboBox();
            this.Tap_ZBJ = new System.Windows.Forms.TabPage();
            this.Zbj_Main = new IntelligentPlanning.CustomControls.ZBJView();
            this.Tap_TrendView = new System.Windows.Forms.TabPage();
            this.TV_Main = new IntelligentPlanning.CustomControls.TrendView();
            this.Tap_BTCount = new System.Windows.Forms.TabPage();
            this.BT_Main = new IntelligentPlanning.CustomControls.BTCount();
            this.Tap_BTFN = new System.Windows.Forms.TabPage();
            this.Pnl_BTFN = new System.Windows.Forms.Panel();
            this.Pnl_BTFNMain = new System.Windows.Forms.Panel();
            this.Egv_BTFNTimesList = new IntelligentPlanning.ExDataGridView.ExpandGirdView(this.components);
            this.Pnl_TimesBottom = new System.Windows.Forms.Panel();
            this.Ckb_TBCount = new System.Windows.Forms.CheckBox();
            this.Ckb_ClearTimes = new System.Windows.Forms.CheckBox();
            this.Ckb_EditTimes = new System.Windows.Forms.CheckBox();
            this.Ckb_DeleteTimes = new System.Windows.Forms.CheckBox();
            this.Ckb_AddTimes = new System.Windows.Forms.CheckBox();
            this.Ckb_SaveTimes = new System.Windows.Forms.CheckBox();
            this.Ckb_BTFNEdit = new System.Windows.Forms.CheckBox();
            this.Ckb_BTFNEditSkip = new System.Windows.Forms.CheckBox();
            this.Nm_BTFNEdit = new System.Windows.Forms.NumericUpDown();
            this.Cbb_BTFNEdit = new System.Windows.Forms.ComboBox();
            this.Lbl_BTFNEdit = new System.Windows.Forms.Label();
            this.Pnl_BTFNList = new System.Windows.Forms.Panel();
            this.Egv_BTFNMain = new IntelligentPlanning.ExDataGridView.ExpandGirdView(this.components);
            this.Pnl_FNBottom = new System.Windows.Forms.Panel();
            this.Ckb_DeleteBTFN = new System.Windows.Forms.CheckBox();
            this.Ckb_AddBTFN = new System.Windows.Forms.CheckBox();
            this.Lbl_GJFNEncrypt = new System.Windows.Forms.Label();
            this.Tap_TBCount = new System.Windows.Forms.TabPage();
            this.TB_Main = new IntelligentPlanning.CustomControls.TBCount();
            this.Tap_HJFG = new System.Windows.Forms.TabPage();
            this.HJFG_Main = new IntelligentPlanning.CustomControls.HJFGCount();
            this.Tap_ShrinkEX = new System.Windows.Forms.TabPage();
            this.SK_EX = new IntelligentPlanning.CustomControls.ShrinkEX();
            this.Tap_ShrinkSX = new System.Windows.Forms.TabPage();
            this.SK_SX = new IntelligentPlanning.CustomControls.ShrinkSX();
            this.Tap_Setting = new System.Windows.Forms.TabPage();
            this.Pnl_Setting = new System.Windows.Forms.Panel();
            this.Egv_ShowTapList = new IntelligentPlanning.ExDataGridView.ExpandGirdView(this.components);
            this.Tap_CDCount = new System.Windows.Forms.TabPage();
            this.Pnl_CDCount = new System.Windows.Forms.Panel();
            this.CD_Main = new IntelligentPlanning.CustomControls.CDCount();
            this.Pnl_OpenData = new System.Windows.Forms.Panel();
            this.Pnl_DataMain = new System.Windows.Forms.Panel();
            this.Pnl_DataBottom = new System.Windows.Forms.Panel();
            this.Egv_DataList = new IntelligentPlanning.ExDataGridView.ExpandGirdView(this.components);
            this.Pnl_RrfreshPT = new System.Windows.Forms.Panel();
            this.Egv_PTLineList = new IntelligentPlanning.ExDataGridView.ExpandGirdView(this.components);
            this.Pnl_PTRefresh = new System.Windows.Forms.Panel();
            this.Ckb_DeleteLine = new System.Windows.Forms.CheckBox();
            this.Ckb_RrfreshPTLine = new System.Windows.Forms.CheckBox();
            this.Ckb_AddLine = new System.Windows.Forms.CheckBox();
            this.Pnl_DataTop2 = new System.Windows.Forms.Panel();
            this.Pnl_DataBottom1 = new System.Windows.Forms.Panel();
            this.Ckb_ShowHideUser = new System.Windows.Forms.CheckBox();
            this.Ckb_Login = new System.Windows.Forms.CheckBox();
            this.Ckb_RefreshUser = new System.Windows.Forms.CheckBox();
            this.Pnl_UserLogin2 = new System.Windows.Forms.Panel();
            this.Ckb_PWPaste = new System.Windows.Forms.CheckBox();
            this.Ckb_PWClear = new System.Windows.Forms.CheckBox();
            this.Lbl_IDHint = new System.Windows.Forms.Label();
            this.Cbb_Lottery = new System.Windows.Forms.ComboBox();
            this.Lbl_Lottery = new System.Windows.Forms.Label();
            this.Txt_ID = new System.Windows.Forms.TextBox();
            this.Txt_PW = new System.Windows.Forms.TextBox();
            this.Lbl_LoginPT = new System.Windows.Forms.Label();
            this.Cbb_LoginPT = new System.Windows.Forms.ComboBox();
            this.Lbl_LoginHint = new System.Windows.Forms.Label();
            this.Lbl_PW = new System.Windows.Forms.Label();
            this.Lbl_ID = new System.Windows.Forms.Label();
            this.Pnl_UserLogin1 = new System.Windows.Forms.Panel();
            this.Txt_KSStopBets = new System.Windows.Forms.TextBox();
            this.Txt_YLStopBets = new System.Windows.Forms.TextBox();
            this.Lbl_KSStopBets = new System.Windows.Forms.Label();
            this.Lbl_YLStopBets = new System.Windows.Forms.Label();
            this.Lbl_StopBets = new System.Windows.Forms.Label();
            this.Lbl_BankBalanceValue = new System.Windows.Forms.Label();
            this.Lbl_BetsExpectValue = new System.Windows.Forms.Label();
            this.Lbl_BankBalanceKey = new System.Windows.Forms.Label();
            this.Lbl_BetsExpectKey = new System.Windows.Forms.Label();
            this.Lbl_IDValue = new System.Windows.Forms.Label();
            this.Lbl_IDKey = new System.Windows.Forms.Label();
            this.Ckb_PlaySound = new System.Windows.Forms.CheckBox();
            this.Tim_NextExpect = new System.Windows.Forms.Timer(this.components);
            this.Pnl_Top = new System.Windows.Forms.Panel();
            this.Pnl_GG = new System.Windows.Forms.Panel();
            this.Piw_Main = new IntelligentPlanning.CustomControls.PictureSwitch();
            this.Pnl_CurrentExpect1 = new System.Windows.Forms.Panel();
            this.Pnl_CurrentCode3 = new System.Windows.Forms.Panel();
            this.Lbl_CurrentCode7 = new System.Windows.Forms.Label();
            this.Lbl_CurrentCode6 = new System.Windows.Forms.Label();
            this.Lbl_CurrentCode10 = new System.Windows.Forms.Label();
            this.Lbl_CurrentCode9 = new System.Windows.Forms.Label();
            this.Lbl_CurrentCode8 = new System.Windows.Forms.Label();
            this.Pnl_CurrentCode4 = new System.Windows.Forms.Panel();
            this.PK_CodeSmall = new IntelligentPlanning.CustomControls.PK10CodeLineSmall();
            this.Lbl_NextExpect1 = new System.Windows.Forms.Label();
            this.Lbl_NextTime1 = new System.Windows.Forms.Label();
            this.Lbl_CurrentExpect1 = new System.Windows.Forms.Label();
            this.Pnl_Notice = new System.Windows.Forms.Panel();
            this.Egv_NoticeList = new IntelligentPlanning.ExDataGridView.ExpandGirdView(this.components);
            this.Pnl_LTUserInfo = new System.Windows.Forms.Panel();
            this.Pnl_AppName = new System.Windows.Forms.Panel();
            this.Ckb_AppName = new System.Windows.Forms.CheckBox();
            this.Txt_AppName = new System.Windows.Forms.TextBox();
            this.Lbl_AppName = new System.Windows.Forms.Label();
            this.Pnl_LTUserInfoTop = new System.Windows.Forms.Panel();
            this.Ckb_CloseMin = new System.Windows.Forms.CheckBox();
            this.Ckb_LeftInfo = new System.Windows.Forms.CheckBox();
            this.Ckb_OpenHint = new System.Windows.Forms.CheckBox();
            this.Ckb_RrfreshPT = new System.Windows.Forms.CheckBox();
            this.Ckb_Data = new System.Windows.Forms.CheckBox();
            this.Pnl_CurrentExpect = new System.Windows.Forms.Panel();
            this.Pnl_CurrentCode1 = new System.Windows.Forms.Panel();
            this.Lbl_CurrentCode1 = new System.Windows.Forms.Label();
            this.Lbl_CurrentCode5 = new System.Windows.Forms.Label();
            this.Lbl_CurrentCode3 = new System.Windows.Forms.Label();
            this.Lbl_CurrentCode2 = new System.Windows.Forms.Label();
            this.Lbl_CurrentCode4 = new System.Windows.Forms.Label();
            this.Pnl_CurrentCode2 = new System.Windows.Forms.Panel();
            this.PK_Code = new IntelligentPlanning.CustomControls.PK10CodeLine();
            this.Pnl_CurrentExpectTop = new System.Windows.Forms.Panel();
            this.Lbl_CurrentExpect = new System.Windows.Forms.Label();
            this.Ckb_AutomationRun = new System.Windows.Forms.CheckBox();
            this.Pnl_NextExpect = new System.Windows.Forms.Panel();
            this.Pnl_NextExpectTop = new System.Windows.Forms.Panel();
            this.Lbl_NextExpect = new System.Windows.Forms.Label();
            this.Lbl_NextTime = new System.Windows.Forms.Label();
            this.Tot_Hint = new System.Windows.Forms.ToolTip(this.components);
            this.Nic_Hint = new System.Windows.Forms.NotifyIcon(this.components);
            this.Cms_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Tsm_Vis = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Tsm_Colse = new System.Windows.Forms.ToolStripMenuItem();
            this.Stp_Hint = new System.Windows.Forms.StatusStrip();
            this.Tsp_PeopleKey = new System.Windows.Forms.ToolStripStatusLabel();
            this.Tsp_PeopleValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.Tsp_LoginKey = new System.Windows.Forms.ToolStripStatusLabel();
            this.Tsp_LoginValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.Tsp_QQKey = new System.Windows.Forms.ToolStripStatusLabel();
            this.Tsp_QQValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.Tsp_QQGroupKey = new System.Windows.Forms.ToolStripStatusLabel();
            this.Tsp_QQGroupValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.Tsp_HintKey = new System.Windows.Forms.ToolStripStatusLabel();
            this.Tsp_HintValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.Lbl_Web = new System.Windows.Forms.LinkLabel();
            this.Err_Hint = new System.Windows.Forms.ErrorProvider(this.components);
            this.Pnl_Info = new System.Windows.Forms.Panel();
            this.Pnl_InfoRight = new System.Windows.Forms.Panel();
            this.Pnl_Scroll = new System.Windows.Forms.Panel();
            this.Sct_Notice = new IntelligentPlanning.CustomControls.ScrollingText();
            this.Pnl_NoticeLeft = new System.Windows.Forms.Panel();
            this.Pic_Notice = new System.Windows.Forms.PictureBox();
            this.Tot_FNHint = new System.Windows.Forms.ToolTip(this.components);
            this.Pnl_Main.SuspendLayout();
            this.Pnl_Bets.SuspendLayout();
            this.Tab_Main.SuspendLayout();
            this.Tap_PT.SuspendLayout();
            this.Pnl_Bets2.SuspendLayout();
            this.Tap_ZDBets.SuspendLayout();
            this.Pnl_Bets1.SuspendLayout();
            this.Pnl_BetsMain.SuspendLayout();
            this.Pnl_BetsLeft.SuspendLayout();
            this.Pnl_BetsInfoMain.SuspendLayout();
            this.Pnl_BetsRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_PlanList)).BeginInit();
            this.Pnl_PlanListBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nm_DeleteExpect)).BeginInit();
            this.Pnl_PlanListTop.SuspendLayout();
            this.Pnl_BetsInfoTop.SuspendLayout();
            this.Pnl_BetsInfoTopMain.SuspendLayout();
            this.Pnl_BetsInfoTopRight.SuspendLayout();
            this.Pnl_BetsType.SuspendLayout();
            this.Pnl_BetsInfoTopRight1.SuspendLayout();
            this.Pnl_BetsInfoTopLeft.SuspendLayout();
            this.Pnl_BetsInfoTop2.SuspendLayout();
            this.Pnl_BetsInfoTop2Left.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nm_BetsTime)).BeginInit();
            this.Pnl_BetsInfoTop1.SuspendLayout();
            this.Pnl_BetsInfoExpect.SuspendLayout();
            this.Pnl_BetsInfoMN.SuspendLayout();
            this.Pnl_BetsInfoMNRight.SuspendLayout();
            this.Tap_Scheme.SuspendLayout();
            this.Pnl_Scheme.SuspendLayout();
            this.Pnl_SchemeMain.SuspendLayout();
            this.Pnl_SchemeInfo.SuspendLayout();
            this.Pnl_SchemeTop2.SuspendLayout();
            this.Pnl_SchemeLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_SchemeList)).BeginInit();
            this.Pnl_SchemeBottom.SuspendLayout();
            this.Pnl_SchemeShare.SuspendLayout();
            this.Pnl_SchemeTop1.SuspendLayout();
            this.Tap_LSData.SuspendLayout();
            this.Pnl_LSData.SuspendLayout();
            this.Pnl_LSDataMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_LSDataList)).BeginInit();
            this.Pnl_LSDataTop.SuspendLayout();
            this.Pnl_LSDataRight.SuspendLayout();
            this.Pnl_LSDataLeft.SuspendLayout();
            this.Pnl_LSDataTop1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nm_LSBJExpect)).BeginInit();
            this.Tap_TJData.SuspendLayout();
            this.Pnl_TJData.SuspendLayout();
            this.Pnl_TJDataMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_TJDataList2)).BeginInit();
            this.Pnl_TJDataTop2.SuspendLayout();
            this.Pnl_TJDataFind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nm_TJFindXS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_TJDataList1)).BeginInit();
            this.Pnl_TJDataTop.SuspendLayout();
            this.Pnl_TJRight2.SuspendLayout();
            this.Pnl_TJRight1.SuspendLayout();
            this.Pnl_TJDataTop1.SuspendLayout();
            this.Tap_ZBJ.SuspendLayout();
            this.Tap_TrendView.SuspendLayout();
            this.Tap_BTCount.SuspendLayout();
            this.Tap_BTFN.SuspendLayout();
            this.Pnl_BTFN.SuspendLayout();
            this.Pnl_BTFNMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_BTFNTimesList)).BeginInit();
            this.Pnl_TimesBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nm_BTFNEdit)).BeginInit();
            this.Pnl_BTFNList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_BTFNMain)).BeginInit();
            this.Pnl_FNBottom.SuspendLayout();
            this.Tap_TBCount.SuspendLayout();
            this.Tap_HJFG.SuspendLayout();
            this.Tap_ShrinkEX.SuspendLayout();
            this.Tap_ShrinkSX.SuspendLayout();
            this.Tap_Setting.SuspendLayout();
            this.Pnl_Setting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_ShowTapList)).BeginInit();
            this.Tap_CDCount.SuspendLayout();
            this.Pnl_CDCount.SuspendLayout();
            this.Pnl_OpenData.SuspendLayout();
            this.Pnl_DataMain.SuspendLayout();
            this.Pnl_DataBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_DataList)).BeginInit();
            this.Pnl_RrfreshPT.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_PTLineList)).BeginInit();
            this.Pnl_PTRefresh.SuspendLayout();
            this.Pnl_DataTop2.SuspendLayout();
            this.Pnl_DataBottom1.SuspendLayout();
            this.Pnl_UserLogin2.SuspendLayout();
            this.Pnl_UserLogin1.SuspendLayout();
            this.Pnl_Top.SuspendLayout();
            this.Pnl_GG.SuspendLayout();
            this.Pnl_CurrentExpect1.SuspendLayout();
            this.Pnl_CurrentCode3.SuspendLayout();
            this.Pnl_CurrentCode4.SuspendLayout();
            this.Pnl_Notice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_NoticeList)).BeginInit();
            this.Pnl_LTUserInfo.SuspendLayout();
            this.Pnl_AppName.SuspendLayout();
            this.Pnl_LTUserInfoTop.SuspendLayout();
            this.Pnl_CurrentExpect.SuspendLayout();
            this.Pnl_CurrentCode1.SuspendLayout();
            this.Pnl_CurrentCode2.SuspendLayout();
            this.Pnl_CurrentExpectTop.SuspendLayout();
            this.Pnl_NextExpect.SuspendLayout();
            this.Pnl_NextExpectTop.SuspendLayout();
            this.Cms_Menu.SuspendLayout();
            this.Stp_Hint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Err_Hint)).BeginInit();
            this.Pnl_Info.SuspendLayout();
            this.Pnl_Scroll.SuspendLayout();
            this.Pnl_NoticeLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Notice)).BeginInit();
            this.SuspendLayout();
            // 
            // Pnl_Main
            // 
            this.Pnl_Main.Controls.Add(this.Pnl_Bets);
            this.Pnl_Main.Controls.Add(this.Pnl_OpenData);
            this.Pnl_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_Main.Location = new System.Drawing.Point(0, 145);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new System.Drawing.Size(1234, 576);
            this.Pnl_Main.TabIndex = 0;
            // 
            // Pnl_Bets
            // 
            this.Pnl_Bets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_Bets.Controls.Add(this.Lbl_AppHint);
            this.Pnl_Bets.Controls.Add(this.Btn_ViewTop);
            this.Pnl_Bets.Controls.Add(this.Tab_Main);
            this.Pnl_Bets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_Bets.Location = new System.Drawing.Point(226, 0);
            this.Pnl_Bets.Name = "Pnl_Bets";
            this.Pnl_Bets.Size = new System.Drawing.Size(1008, 576);
            this.Pnl_Bets.TabIndex = 71;
            // 
            // Lbl_AppHint
            // 
            this.Lbl_AppHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_AppHint.AutoSize = true;
            this.Lbl_AppHint.Location = new System.Drawing.Point(-1000, -1000);
            this.Lbl_AppHint.Name = "Lbl_AppHint";
            this.Lbl_AppHint.Size = new System.Drawing.Size(140, 17);
            this.Lbl_AppHint.TabIndex = 190;
            this.Lbl_AppHint.Text = "【彩无界】软件公司出品";
            this.Lbl_AppHint.Visible = false;
            // 
            // Btn_ViewTop
            // 
            this.Btn_ViewTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_ViewTop.BackColor = System.Drawing.Color.Transparent;
            this.Btn_ViewTop.FlatAppearance.BorderSize = 0;
            this.Btn_ViewTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_ViewTop.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.Btn_ViewTop.Location = new System.Drawing.Point(968, 1);
            this.Btn_ViewTop.Name = "Btn_ViewTop";
            this.Btn_ViewTop.Size = new System.Drawing.Size(30, 30);
            this.Btn_ViewTop.TabIndex = 157;
            this.Tot_Hint.SetToolTip(this.Btn_ViewTop, "隐藏");
            this.Btn_ViewTop.UseVisualStyleBackColor = false;
            this.Btn_ViewTop.Click += new System.EventHandler(this.Btn_ViewTop_Click);
            // 
            // Tab_Main
            // 
            this.Tab_Main.Controls.Add(this.Tap_PT);
            this.Tab_Main.Controls.Add(this.Tap_ZDBets);
            this.Tab_Main.Controls.Add(this.Tap_Scheme);
            this.Tab_Main.Controls.Add(this.Tap_LSData);
            this.Tab_Main.Controls.Add(this.Tap_TJData);
            this.Tab_Main.Controls.Add(this.Tap_ZBJ);
            this.Tab_Main.Controls.Add(this.Tap_TrendView);
            this.Tab_Main.Controls.Add(this.Tap_BTCount);
            this.Tab_Main.Controls.Add(this.Tap_BTFN);
            this.Tab_Main.Controls.Add(this.Tap_TBCount);
            this.Tab_Main.Controls.Add(this.Tap_HJFG);
            this.Tab_Main.Controls.Add(this.Tap_ShrinkEX);
            this.Tab_Main.Controls.Add(this.Tap_ShrinkSX);
            this.Tab_Main.Controls.Add(this.Tap_Setting);
            this.Tab_Main.Controls.Add(this.Tap_CDCount);
            this.Tab_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tab_Main.ItemSize = new System.Drawing.Size(65, 30);
            this.Tab_Main.Location = new System.Drawing.Point(0, 0);
            this.Tab_Main.Name = "Tab_Main";
            this.Tab_Main.SelectedIndex = 0;
            this.Tab_Main.Size = new System.Drawing.Size(1006, 574);
            this.Tab_Main.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.Tab_Main.TabIndex = 11;
            this.Tab_Main.SelectedIndexChanged += new System.EventHandler(this.Tab_Main_SelectedIndexChanged);
            // 
            // Tap_PT
            // 
            this.Tap_PT.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_PT.Controls.Add(this.Pnl_Bets2);
            this.Tap_PT.Location = new System.Drawing.Point(4, 34);
            this.Tap_PT.Name = "Tap_PT";
            this.Tap_PT.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_PT.Size = new System.Drawing.Size(998, 536);
            this.Tap_PT.TabIndex = 0;
            this.Tap_PT.Text = "官网网址";
            // 
            // Pnl_Bets2
            // 
            this.Pnl_Bets2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_Bets2.Controls.Add(this.Web_Login);
            this.Pnl_Bets2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_Bets2.Location = new System.Drawing.Point(3, 3);
            this.Pnl_Bets2.Name = "Pnl_Bets2";
            this.Pnl_Bets2.Size = new System.Drawing.Size(992, 530);
            this.Pnl_Bets2.TabIndex = 76;
            // 
            // Web_Login
            // 
            this.Web_Login.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Web_Login.Location = new System.Drawing.Point(0, 0);
            this.Web_Login.MinimumSize = new System.Drawing.Size(20, 20);
            this.Web_Login.Name = "Web_Login";
            this.Web_Login.Size = new System.Drawing.Size(990, 528);
            this.Web_Login.TabIndex = 3;
            this.Web_Login.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.Web_Login_DocumentCompleted);
            this.Web_Login.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.Web_Login_Navigating);
            this.Web_Login.NewWindow += new System.ComponentModel.CancelEventHandler(this.Web_Login_NewWindow);
            // 
            // Tap_ZDBets
            // 
            this.Tap_ZDBets.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_ZDBets.Controls.Add(this.Pnl_Bets1);
            this.Tap_ZDBets.Location = new System.Drawing.Point(4, 34);
            this.Tap_ZDBets.Name = "Tap_ZDBets";
            this.Tap_ZDBets.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_ZDBets.Size = new System.Drawing.Size(998, 536);
            this.Tap_ZDBets.TabIndex = 1;
            this.Tap_ZDBets.Text = "自动投注";
            // 
            // Pnl_Bets1
            // 
            this.Pnl_Bets1.Controls.Add(this.Pnl_BetsMain);
            this.Pnl_Bets1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_Bets1.Location = new System.Drawing.Point(3, 3);
            this.Pnl_Bets1.Name = "Pnl_Bets1";
            this.Pnl_Bets1.Size = new System.Drawing.Size(992, 530);
            this.Pnl_Bets1.TabIndex = 77;
            // 
            // Pnl_BetsMain
            // 
            this.Pnl_BetsMain.Controls.Add(this.Pnl_BetsLeft);
            this.Pnl_BetsMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_BetsMain.Location = new System.Drawing.Point(0, 0);
            this.Pnl_BetsMain.Name = "Pnl_BetsMain";
            this.Pnl_BetsMain.Size = new System.Drawing.Size(992, 530);
            this.Pnl_BetsMain.TabIndex = 3;
            // 
            // Pnl_BetsLeft
            // 
            this.Pnl_BetsLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_BetsLeft.Controls.Add(this.Pnl_BetsInfoMain);
            this.Pnl_BetsLeft.Controls.Add(this.Pnl_BetsInfoTop);
            this.Pnl_BetsLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_BetsLeft.Location = new System.Drawing.Point(0, 0);
            this.Pnl_BetsLeft.Name = "Pnl_BetsLeft";
            this.Pnl_BetsLeft.Size = new System.Drawing.Size(992, 530);
            this.Pnl_BetsLeft.TabIndex = 72;
            // 
            // Pnl_BetsInfoMain
            // 
            this.Pnl_BetsInfoMain.Controls.Add(this.Pnl_BetsRight);
            this.Pnl_BetsInfoMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_BetsInfoMain.Location = new System.Drawing.Point(0, 105);
            this.Pnl_BetsInfoMain.Name = "Pnl_BetsInfoMain";
            this.Pnl_BetsInfoMain.Size = new System.Drawing.Size(990, 423);
            this.Pnl_BetsInfoMain.TabIndex = 71;
            // 
            // Pnl_BetsRight
            // 
            this.Pnl_BetsRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_BetsRight.Controls.Add(this.Egv_PlanList);
            this.Pnl_BetsRight.Controls.Add(this.Pnl_PlanListBottom);
            this.Pnl_BetsRight.Controls.Add(this.Pnl_PlanListTop);
            this.Pnl_BetsRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_BetsRight.Location = new System.Drawing.Point(0, 0);
            this.Pnl_BetsRight.Name = "Pnl_BetsRight";
            this.Pnl_BetsRight.Size = new System.Drawing.Size(990, 423);
            this.Pnl_BetsRight.TabIndex = 73;
            // 
            // Egv_PlanList
            // 
            this.Egv_PlanList.AllowUserToAddRows = false;
            this.Egv_PlanList.AllowUserToDeleteRows = false;
            this.Egv_PlanList.AllowUserToResizeRows = false;
            this.Egv_PlanList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.Egv_PlanList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Egv_PlanList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_PlanList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Egv_PlanList.ColumnHeadersHeight = 30;
            this.Egv_PlanList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_PlanList.DefaultCellStyle = dataGridViewCellStyle2;
            this.Egv_PlanList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Egv_PlanList.DragLineColor = System.Drawing.Color.Silver;
            this.Egv_PlanList.ExternalVirtualMode = true;
            this.Egv_PlanList.GridColor = System.Drawing.Color.Silver;
            this.Egv_PlanList.HeadersCheckDefult = System.Windows.Forms.CheckState.Checked;
            this.Egv_PlanList.Location = new System.Drawing.Point(0, 60);
            this.Egv_PlanList.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Egv_PlanList.MergeColumnHeaderForeColor = System.Drawing.Color.Black;
            this.Egv_PlanList.MultiSelect = false;
            this.Egv_PlanList.Name = "Egv_PlanList";
            this.Egv_PlanList.RowHeadersVisible = false;
            this.Egv_PlanList.RowNum = 17;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.Egv_PlanList.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.Egv_PlanList.RowTemplate.Height = 23;
            this.Egv_PlanList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Egv_PlanList.Size = new System.Drawing.Size(988, 326);
            this.Egv_PlanList.TabIndex = 69;
            this.Egv_PlanList.VirtualMode = true;
            // 
            // Pnl_PlanListBottom
            // 
            this.Pnl_PlanListBottom.Controls.Add(this.Nm_DeleteExpect);
            this.Pnl_PlanListBottom.Controls.Add(this.Ckb_DeleteExpect);
            this.Pnl_PlanListBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Pnl_PlanListBottom.Location = new System.Drawing.Point(0, 386);
            this.Pnl_PlanListBottom.Name = "Pnl_PlanListBottom";
            this.Pnl_PlanListBottom.Size = new System.Drawing.Size(988, 35);
            this.Pnl_PlanListBottom.TabIndex = 70;
            // 
            // Nm_DeleteExpect
            // 
            this.Nm_DeleteExpect.Location = new System.Drawing.Point(165, 7);
            this.Nm_DeleteExpect.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Nm_DeleteExpect.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Nm_DeleteExpect.Name = "Nm_DeleteExpect";
            this.Nm_DeleteExpect.Size = new System.Drawing.Size(60, 23);
            this.Nm_DeleteExpect.TabIndex = 211;
            this.Nm_DeleteExpect.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Ckb_DeleteExpect
            // 
            this.Ckb_DeleteExpect.AutoSize = true;
            this.Ckb_DeleteExpect.Location = new System.Drawing.Point(5, 8);
            this.Ckb_DeleteExpect.Name = "Ckb_DeleteExpect";
            this.Ckb_DeleteExpect.Size = new System.Drawing.Size(157, 21);
            this.Ckb_DeleteExpect.TabIndex = 209;
            this.Ckb_DeleteExpect.Text = "自动删除N期前的计划：\r\n";
            this.Ckb_DeleteExpect.UseVisualStyleBackColor = true;
            // 
            // Pnl_PlanListTop
            // 
            this.Pnl_PlanListTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_PlanListTop.Controls.Add(this.Ckb_ClearBetsList);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_BetsHint);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_ZQLValue);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_ZQLKey);
            this.Pnl_PlanListTop.Controls.Add(this.Ckb_PlanShowHide);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_LGMaxValue);
            this.Pnl_PlanListTop.Controls.Add(this.Ckb_BetsSort);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_LGMaxKey);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_LZMaxValue);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_LZMaxKey);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_BetsCountValue);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_BetsCountKey);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_MNBetsMoneyPlanValue);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_MNBetsMoneyPlanKey);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_BetsMoneyPlanValue);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_BetsMoneyPlanKey);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_MNBetsGainPlanValue);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_MNBetsGainPlanKey);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_BetsValue);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_BetsKey);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_BetsGainPlanValue);
            this.Pnl_PlanListTop.Controls.Add(this.Lbl_BetsGainPlanKey);
            this.Pnl_PlanListTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_PlanListTop.Location = new System.Drawing.Point(0, 0);
            this.Pnl_PlanListTop.Name = "Pnl_PlanListTop";
            this.Pnl_PlanListTop.Size = new System.Drawing.Size(988, 60);
            this.Pnl_PlanListTop.TabIndex = 2;
            // 
            // Ckb_ClearBetsList
            // 
            this.Ckb_ClearBetsList.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_ClearBetsList.AutoCheck = false;
            this.Ckb_ClearBetsList.FlatAppearance.BorderSize = 0;
            this.Ckb_ClearBetsList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_ClearBetsList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_ClearBetsList.Location = new System.Drawing.Point(557, 29);
            this.Ckb_ClearBetsList.Name = "Ckb_ClearBetsList";
            this.Ckb_ClearBetsList.Size = new System.Drawing.Size(80, 25);
            this.Ckb_ClearBetsList.TabIndex = 214;
            this.Ckb_ClearBetsList.Text = "清空记录";
            this.Ckb_ClearBetsList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_ClearBetsList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Tot_Hint.SetToolTip(this.Ckb_ClearBetsList, "清空投注的记录");
            this.Ckb_ClearBetsList.UseVisualStyleBackColor = true;
            this.Ckb_ClearBetsList.Click += new System.EventHandler(this.Ckb_ClearBetsList_Click);
            // 
            // Lbl_BetsHint
            // 
            this.Lbl_BetsHint.AutoSize = true;
            this.Lbl_BetsHint.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_BetsHint.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_BetsHint.Location = new System.Drawing.Point(713, 34);
            this.Lbl_BetsHint.Name = "Lbl_BetsHint";
            this.Lbl_BetsHint.Size = new System.Drawing.Size(0, 17);
            this.Lbl_BetsHint.TabIndex = 209;
            // 
            // Lbl_ZQLValue
            // 
            this.Lbl_ZQLValue.AutoSize = true;
            this.Lbl_ZQLValue.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_ZQLValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_ZQLValue.Location = new System.Drawing.Point(908, 8);
            this.Lbl_ZQLValue.Name = "Lbl_ZQLValue";
            this.Lbl_ZQLValue.Size = new System.Drawing.Size(22, 17);
            this.Lbl_ZQLValue.TabIndex = 206;
            this.Lbl_ZQLValue.Text = "00";
            // 
            // Lbl_ZQLKey
            // 
            this.Lbl_ZQLKey.AutoSize = true;
            this.Lbl_ZQLKey.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_ZQLKey.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_ZQLKey.Location = new System.Drawing.Point(855, 8);
            this.Lbl_ZQLKey.Name = "Lbl_ZQLKey";
            this.Lbl_ZQLKey.Size = new System.Drawing.Size(56, 17);
            this.Lbl_ZQLKey.TabIndex = 205;
            this.Lbl_ZQLKey.Text = "准确率：";
            // 
            // Ckb_PlanShowHide
            // 
            this.Ckb_PlanShowHide.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_PlanShowHide.FlatAppearance.BorderSize = 0;
            this.Ckb_PlanShowHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_PlanShowHide.Location = new System.Drawing.Point(675, 29);
            this.Ckb_PlanShowHide.Name = "Ckb_PlanShowHide";
            this.Ckb_PlanShowHide.Size = new System.Drawing.Size(30, 25);
            this.Ckb_PlanShowHide.TabIndex = 208;
            this.Tot_Hint.SetToolTip(this.Ckb_PlanShowHide, "隐藏投注记录中不重要的列");
            this.Ckb_PlanShowHide.UseVisualStyleBackColor = true;
            this.Ckb_PlanShowHide.CheckedChanged += new System.EventHandler(this.Ckb_PlanShowHide_CheckedChanged);
            // 
            // Lbl_LGMaxValue
            // 
            this.Lbl_LGMaxValue.AutoSize = true;
            this.Lbl_LGMaxValue.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_LGMaxValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_LGMaxValue.Location = new System.Drawing.Point(785, 8);
            this.Lbl_LGMaxValue.Name = "Lbl_LGMaxValue";
            this.Lbl_LGMaxValue.Size = new System.Drawing.Size(22, 17);
            this.Lbl_LGMaxValue.TabIndex = 204;
            this.Lbl_LGMaxValue.Text = "00";
            // 
            // Ckb_BetsSort
            // 
            this.Ckb_BetsSort.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_BetsSort.FlatAppearance.BorderSize = 0;
            this.Ckb_BetsSort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_BetsSort.Location = new System.Drawing.Point(642, 29);
            this.Ckb_BetsSort.Name = "Ckb_BetsSort";
            this.Ckb_BetsSort.Size = new System.Drawing.Size(30, 25);
            this.Ckb_BetsSort.TabIndex = 207;
            this.Tot_Hint.SetToolTip(this.Ckb_BetsSort, "将最新投注记录显示在第一行");
            this.Ckb_BetsSort.UseVisualStyleBackColor = true;
            this.Ckb_BetsSort.CheckedChanged += new System.EventHandler(this.Ckb_BetsSort_CheckedChanged);
            // 
            // Lbl_LGMaxKey
            // 
            this.Lbl_LGMaxKey.AutoSize = true;
            this.Lbl_LGMaxKey.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_LGMaxKey.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_LGMaxKey.Location = new System.Drawing.Point(720, 8);
            this.Lbl_LGMaxKey.Name = "Lbl_LGMaxKey";
            this.Lbl_LGMaxKey.Size = new System.Drawing.Size(68, 17);
            this.Lbl_LGMaxKey.TabIndex = 203;
            this.Lbl_LGMaxKey.Text = "最大连挂：";
            // 
            // Lbl_LZMaxValue
            // 
            this.Lbl_LZMaxValue.AutoSize = true;
            this.Lbl_LZMaxValue.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_LZMaxValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_LZMaxValue.Location = new System.Drawing.Point(650, 8);
            this.Lbl_LZMaxValue.Name = "Lbl_LZMaxValue";
            this.Lbl_LZMaxValue.Size = new System.Drawing.Size(22, 17);
            this.Lbl_LZMaxValue.TabIndex = 202;
            this.Lbl_LZMaxValue.Text = "00";
            // 
            // Lbl_LZMaxKey
            // 
            this.Lbl_LZMaxKey.AutoSize = true;
            this.Lbl_LZMaxKey.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_LZMaxKey.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_LZMaxKey.Location = new System.Drawing.Point(585, 8);
            this.Lbl_LZMaxKey.Name = "Lbl_LZMaxKey";
            this.Lbl_LZMaxKey.Size = new System.Drawing.Size(68, 17);
            this.Lbl_LZMaxKey.TabIndex = 201;
            this.Lbl_LZMaxKey.Text = "最大连中：";
            // 
            // Lbl_BetsCountValue
            // 
            this.Lbl_BetsCountValue.AutoSize = true;
            this.Lbl_BetsCountValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_BetsCountValue.Location = new System.Drawing.Point(70, 33);
            this.Lbl_BetsCountValue.Name = "Lbl_BetsCountValue";
            this.Lbl_BetsCountValue.Size = new System.Drawing.Size(22, 17);
            this.Lbl_BetsCountValue.TabIndex = 192;
            this.Lbl_BetsCountValue.Text = "00";
            // 
            // Lbl_BetsCountKey
            // 
            this.Lbl_BetsCountKey.AutoSize = true;
            this.Lbl_BetsCountKey.Location = new System.Drawing.Point(5, 33);
            this.Lbl_BetsCountKey.Name = "Lbl_BetsCountKey";
            this.Lbl_BetsCountKey.Size = new System.Drawing.Size(68, 17);
            this.Lbl_BetsCountKey.TabIndex = 191;
            this.Lbl_BetsCountKey.Text = "投注记录：";
            // 
            // Lbl_MNBetsMoneyPlanValue
            // 
            this.Lbl_MNBetsMoneyPlanValue.AutoSize = true;
            this.Lbl_MNBetsMoneyPlanValue.Location = new System.Drawing.Point(505, 8);
            this.Lbl_MNBetsMoneyPlanValue.Name = "Lbl_MNBetsMoneyPlanValue";
            this.Lbl_MNBetsMoneyPlanValue.Size = new System.Drawing.Size(22, 17);
            this.Lbl_MNBetsMoneyPlanValue.TabIndex = 190;
            this.Lbl_MNBetsMoneyPlanValue.Text = "00";
            // 
            // Lbl_MNBetsMoneyPlanKey
            // 
            this.Lbl_MNBetsMoneyPlanKey.AutoSize = true;
            this.Lbl_MNBetsMoneyPlanKey.Location = new System.Drawing.Point(440, 8);
            this.Lbl_MNBetsMoneyPlanKey.Name = "Lbl_MNBetsMoneyPlanKey";
            this.Lbl_MNBetsMoneyPlanKey.Size = new System.Drawing.Size(68, 17);
            this.Lbl_MNBetsMoneyPlanKey.TabIndex = 189;
            this.Lbl_MNBetsMoneyPlanKey.Text = "模拟下注：";
            // 
            // Lbl_BetsMoneyPlanValue
            // 
            this.Lbl_BetsMoneyPlanValue.AutoSize = true;
            this.Lbl_BetsMoneyPlanValue.Location = new System.Drawing.Point(215, 8);
            this.Lbl_BetsMoneyPlanValue.Name = "Lbl_BetsMoneyPlanValue";
            this.Lbl_BetsMoneyPlanValue.Size = new System.Drawing.Size(22, 17);
            this.Lbl_BetsMoneyPlanValue.TabIndex = 188;
            this.Lbl_BetsMoneyPlanValue.Text = "00";
            // 
            // Lbl_BetsMoneyPlanKey
            // 
            this.Lbl_BetsMoneyPlanKey.AutoSize = true;
            this.Lbl_BetsMoneyPlanKey.Location = new System.Drawing.Point(150, 8);
            this.Lbl_BetsMoneyPlanKey.Name = "Lbl_BetsMoneyPlanKey";
            this.Lbl_BetsMoneyPlanKey.Size = new System.Drawing.Size(68, 17);
            this.Lbl_BetsMoneyPlanKey.TabIndex = 187;
            this.Lbl_BetsMoneyPlanKey.Text = "真实下注：";
            // 
            // Lbl_MNBetsGainPlanValue
            // 
            this.Lbl_MNBetsGainPlanValue.AutoSize = true;
            this.Lbl_MNBetsGainPlanValue.Location = new System.Drawing.Point(360, 8);
            this.Lbl_MNBetsGainPlanValue.Name = "Lbl_MNBetsGainPlanValue";
            this.Lbl_MNBetsGainPlanValue.Size = new System.Drawing.Size(22, 17);
            this.Lbl_MNBetsGainPlanValue.TabIndex = 186;
            this.Lbl_MNBetsGainPlanValue.Text = "00";
            // 
            // Lbl_MNBetsGainPlanKey
            // 
            this.Lbl_MNBetsGainPlanKey.AutoSize = true;
            this.Lbl_MNBetsGainPlanKey.Location = new System.Drawing.Point(295, 8);
            this.Lbl_MNBetsGainPlanKey.Name = "Lbl_MNBetsGainPlanKey";
            this.Lbl_MNBetsGainPlanKey.Size = new System.Drawing.Size(68, 17);
            this.Lbl_MNBetsGainPlanKey.TabIndex = 185;
            this.Lbl_MNBetsGainPlanKey.Text = "模拟盈亏：";
            // 
            // Lbl_BetsValue
            // 
            this.Lbl_BetsValue.AutoSize = true;
            this.Lbl_BetsValue.Location = new System.Drawing.Point(215, 33);
            this.Lbl_BetsValue.Name = "Lbl_BetsValue";
            this.Lbl_BetsValue.Size = new System.Drawing.Size(20, 17);
            this.Lbl_BetsValue.TabIndex = 184;
            this.Lbl_BetsValue.Text = "无";
            // 
            // Lbl_BetsKey
            // 
            this.Lbl_BetsKey.AutoSize = true;
            this.Lbl_BetsKey.Location = new System.Drawing.Point(150, 33);
            this.Lbl_BetsKey.Name = "Lbl_BetsKey";
            this.Lbl_BetsKey.Size = new System.Drawing.Size(68, 17);
            this.Lbl_BetsKey.TabIndex = 183;
            this.Lbl_BetsKey.Text = "投注状态：";
            // 
            // Lbl_BetsGainPlanValue
            // 
            this.Lbl_BetsGainPlanValue.AutoSize = true;
            this.Lbl_BetsGainPlanValue.Location = new System.Drawing.Point(70, 8);
            this.Lbl_BetsGainPlanValue.Name = "Lbl_BetsGainPlanValue";
            this.Lbl_BetsGainPlanValue.Size = new System.Drawing.Size(22, 17);
            this.Lbl_BetsGainPlanValue.TabIndex = 182;
            this.Lbl_BetsGainPlanValue.Text = "00";
            // 
            // Lbl_BetsGainPlanKey
            // 
            this.Lbl_BetsGainPlanKey.AutoSize = true;
            this.Lbl_BetsGainPlanKey.Location = new System.Drawing.Point(5, 8);
            this.Lbl_BetsGainPlanKey.Name = "Lbl_BetsGainPlanKey";
            this.Lbl_BetsGainPlanKey.Size = new System.Drawing.Size(68, 17);
            this.Lbl_BetsGainPlanKey.TabIndex = 181;
            this.Lbl_BetsGainPlanKey.Text = "真实盈亏：";
            // 
            // Pnl_BetsInfoTop
            // 
            this.Pnl_BetsInfoTop.Controls.Add(this.Pnl_BetsInfoTopMain);
            this.Pnl_BetsInfoTop.Controls.Add(this.Pnl_BetsInfoTopLeft);
            this.Pnl_BetsInfoTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_BetsInfoTop.Location = new System.Drawing.Point(0, 0);
            this.Pnl_BetsInfoTop.Name = "Pnl_BetsInfoTop";
            this.Pnl_BetsInfoTop.Size = new System.Drawing.Size(990, 105);
            this.Pnl_BetsInfoTop.TabIndex = 73;
            // 
            // Pnl_BetsInfoTopMain
            // 
            this.Pnl_BetsInfoTopMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_BetsInfoTopMain.Controls.Add(this.Pnl_BetsInfoTopRight);
            this.Pnl_BetsInfoTopMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_BetsInfoTopMain.Location = new System.Drawing.Point(555, 0);
            this.Pnl_BetsInfoTopMain.Name = "Pnl_BetsInfoTopMain";
            this.Pnl_BetsInfoTopMain.Size = new System.Drawing.Size(435, 105);
            this.Pnl_BetsInfoTopMain.TabIndex = 74;
            // 
            // Pnl_BetsInfoTopRight
            // 
            this.Pnl_BetsInfoTopRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_BetsInfoTopRight.Controls.Add(this.Pnl_BetsType);
            this.Pnl_BetsInfoTopRight.Controls.Add(this.Pnl_BetsInfoTopRight1);
            this.Pnl_BetsInfoTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_BetsInfoTopRight.Location = new System.Drawing.Point(0, 0);
            this.Pnl_BetsInfoTopRight.Name = "Pnl_BetsInfoTopRight";
            this.Pnl_BetsInfoTopRight.Size = new System.Drawing.Size(433, 103);
            this.Pnl_BetsInfoTopRight.TabIndex = 0;
            // 
            // Pnl_BetsType
            // 
            this.Pnl_BetsType.Controls.Add(this.Lbl_ShareBetsHint);
            this.Pnl_BetsType.Controls.Add(this.Ckb_ShareBetsManage);
            this.Pnl_BetsType.Controls.Add(this.Rdb_ShareBets);
            this.Pnl_BetsType.Controls.Add(this.Rdb_CGBets);
            this.Pnl_BetsType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_BetsType.Location = new System.Drawing.Point(118, 0);
            this.Pnl_BetsType.Name = "Pnl_BetsType";
            this.Pnl_BetsType.Size = new System.Drawing.Size(313, 101);
            this.Pnl_BetsType.TabIndex = 178;
            this.Pnl_BetsType.Visible = false;
            // 
            // Lbl_ShareBetsHint
            // 
            this.Lbl_ShareBetsHint.AutoSize = true;
            this.Lbl_ShareBetsHint.Location = new System.Drawing.Point(5, 70);
            this.Lbl_ShareBetsHint.Name = "Lbl_ShareBetsHint";
            this.Lbl_ShareBetsHint.Size = new System.Drawing.Size(20, 17);
            this.Lbl_ShareBetsHint.TabIndex = 342;
            this.Lbl_ShareBetsHint.Text = "无";
            // 
            // Ckb_ShareBetsManage
            // 
            this.Ckb_ShareBetsManage.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_ShareBetsManage.AutoCheck = false;
            this.Ckb_ShareBetsManage.Enabled = false;
            this.Ckb_ShareBetsManage.FlatAppearance.BorderSize = 0;
            this.Ckb_ShareBetsManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_ShareBetsManage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_ShareBetsManage.Location = new System.Drawing.Point(100, 37);
            this.Ckb_ShareBetsManage.Name = "Ckb_ShareBetsManage";
            this.Ckb_ShareBetsManage.Size = new System.Drawing.Size(80, 25);
            this.Ckb_ShareBetsManage.TabIndex = 341;
            this.Ckb_ShareBetsManage.Text = "共享管理";
            this.Ckb_ShareBetsManage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_ShareBetsManage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Ckb_ShareBetsManage.UseVisualStyleBackColor = true;
            this.Ckb_ShareBetsManage.Click += new System.EventHandler(this.Ckb_ShareBetsManage_Click);
            // 
            // Rdb_ShareBets
            // 
            this.Rdb_ShareBets.AutoSize = true;
            this.Rdb_ShareBets.Location = new System.Drawing.Point(6, 40);
            this.Rdb_ShareBets.Name = "Rdb_ShareBets";
            this.Rdb_ShareBets.Size = new System.Drawing.Size(74, 21);
            this.Rdb_ShareBets.TabIndex = 301;
            this.Rdb_ShareBets.Text = "共享投注";
            this.Rdb_ShareBets.UseVisualStyleBackColor = true;
            this.Rdb_ShareBets.CheckedChanged += new System.EventHandler(this.Rdb_ShareBets_CheckedChanged);
            // 
            // Rdb_CGBets
            // 
            this.Rdb_CGBets.AutoSize = true;
            this.Rdb_CGBets.Checked = true;
            this.Rdb_CGBets.Location = new System.Drawing.Point(6, 10);
            this.Rdb_CGBets.Name = "Rdb_CGBets";
            this.Rdb_CGBets.Size = new System.Drawing.Size(74, 21);
            this.Rdb_CGBets.TabIndex = 300;
            this.Rdb_CGBets.TabStop = true;
            this.Rdb_CGBets.Text = "方案投注";
            this.Rdb_CGBets.UseVisualStyleBackColor = true;
            // 
            // Pnl_BetsInfoTopRight1
            // 
            this.Pnl_BetsInfoTopRight1.Controls.Add(this.Btn_Bets);
            this.Pnl_BetsInfoTopRight1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_BetsInfoTopRight1.Location = new System.Drawing.Point(0, 0);
            this.Pnl_BetsInfoTopRight1.Name = "Pnl_BetsInfoTopRight1";
            this.Pnl_BetsInfoTopRight1.Size = new System.Drawing.Size(118, 101);
            this.Pnl_BetsInfoTopRight1.TabIndex = 177;
            // 
            // Btn_Bets
            // 
            this.Btn_Bets.Dock = System.Windows.Forms.DockStyle.Left;
            this.Btn_Bets.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.Btn_Bets.Location = new System.Drawing.Point(0, 0);
            this.Btn_Bets.Name = "Btn_Bets";
            this.Btn_Bets.Size = new System.Drawing.Size(117, 101);
            this.Btn_Bets.TabIndex = 175;
            this.Btn_Bets.Text = "开启\r\n自动投注";
            this.Btn_Bets.UseVisualStyleBackColor = true;
            this.Btn_Bets.Click += new System.EventHandler(this.Btn_Bets_Click);
            // 
            // Pnl_BetsInfoTopLeft
            // 
            this.Pnl_BetsInfoTopLeft.Controls.Add(this.Pnl_BetsInfoTop2);
            this.Pnl_BetsInfoTopLeft.Controls.Add(this.Pnl_BetsInfoTop1);
            this.Pnl_BetsInfoTopLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_BetsInfoTopLeft.Location = new System.Drawing.Point(0, 0);
            this.Pnl_BetsInfoTopLeft.Name = "Pnl_BetsInfoTopLeft";
            this.Pnl_BetsInfoTopLeft.Size = new System.Drawing.Size(555, 105);
            this.Pnl_BetsInfoTopLeft.TabIndex = 73;
            // 
            // Pnl_BetsInfoTop2
            // 
            this.Pnl_BetsInfoTop2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_BetsInfoTop2.Controls.Add(this.Pnl_BetsInfoTop2Left);
            this.Pnl_BetsInfoTop2.Controls.Add(this.Lbl_BetsTime2);
            this.Pnl_BetsInfoTop2.Controls.Add(this.Lbl_BetsTime1);
            this.Pnl_BetsInfoTop2.Controls.Add(this.Nm_BetsTime);
            this.Pnl_BetsInfoTop2.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_BetsInfoTop2.Location = new System.Drawing.Point(0, 70);
            this.Pnl_BetsInfoTop2.Name = "Pnl_BetsInfoTop2";
            this.Pnl_BetsInfoTop2.Size = new System.Drawing.Size(555, 35);
            this.Pnl_BetsInfoTop2.TabIndex = 70;
            // 
            // Pnl_BetsInfoTop2Left
            // 
            this.Pnl_BetsInfoTop2Left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_BetsInfoTop2Left.Controls.Add(this.Ckb_BetsBeginTime);
            this.Pnl_BetsInfoTop2Left.Controls.Add(this.Ckb_BetsEndTime);
            this.Pnl_BetsInfoTop2Left.Controls.Add(this.Cbb_BetsEndType);
            this.Pnl_BetsInfoTop2Left.Controls.Add(this.Dtp_BetsBeginTime);
            this.Pnl_BetsInfoTop2Left.Controls.Add(this.Dtp_BetsEndTime);
            this.Pnl_BetsInfoTop2Left.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_BetsInfoTop2Left.Location = new System.Drawing.Point(0, 0);
            this.Pnl_BetsInfoTop2Left.Name = "Pnl_BetsInfoTop2Left";
            this.Pnl_BetsInfoTop2Left.Size = new System.Drawing.Size(380, 33);
            this.Pnl_BetsInfoTop2Left.TabIndex = 26;
            // 
            // Ckb_BetsBeginTime
            // 
            this.Ckb_BetsBeginTime.AutoSize = true;
            this.Ckb_BetsBeginTime.Location = new System.Drawing.Point(5, 7);
            this.Ckb_BetsBeginTime.Name = "Ckb_BetsBeginTime";
            this.Ckb_BetsBeginTime.Size = new System.Drawing.Size(75, 21);
            this.Ckb_BetsBeginTime.TabIndex = 175;
            this.Ckb_BetsBeginTime.Text = "自动开始";
            this.Ckb_BetsBeginTime.UseVisualStyleBackColor = true;
            // 
            // Ckb_BetsEndTime
            // 
            this.Ckb_BetsEndTime.AutoSize = true;
            this.Ckb_BetsEndTime.Location = new System.Drawing.Point(152, 7);
            this.Ckb_BetsEndTime.Name = "Ckb_BetsEndTime";
            this.Ckb_BetsEndTime.Size = new System.Drawing.Size(51, 21);
            this.Ckb_BetsEndTime.TabIndex = 177;
            this.Ckb_BetsEndTime.Text = "自动";
            this.Ckb_BetsEndTime.UseVisualStyleBackColor = true;
            // 
            // Cbb_BetsEndType
            // 
            this.Cbb_BetsEndType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbb_BetsEndType.FormattingEnabled = true;
            this.Cbb_BetsEndType.Items.AddRange(new object[] {
            "停止投注",
            "停止新增投注"});
            this.Cbb_BetsEndType.Location = new System.Drawing.Point(206, 4);
            this.Cbb_BetsEndType.Name = "Cbb_BetsEndType";
            this.Cbb_BetsEndType.Size = new System.Drawing.Size(100, 25);
            this.Cbb_BetsEndType.TabIndex = 184;
            // 
            // Dtp_BetsBeginTime
            // 
            this.Dtp_BetsBeginTime.CustomFormat = "HH:mm";
            this.Dtp_BetsBeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_BetsBeginTime.Location = new System.Drawing.Point(83, 5);
            this.Dtp_BetsBeginTime.Name = "Dtp_BetsBeginTime";
            this.Dtp_BetsBeginTime.ShowUpDown = true;
            this.Dtp_BetsBeginTime.Size = new System.Drawing.Size(60, 23);
            this.Dtp_BetsBeginTime.TabIndex = 176;
            this.Dtp_BetsBeginTime.Value = new System.DateTime(2015, 7, 20, 9, 1, 0, 0);
            // 
            // Dtp_BetsEndTime
            // 
            this.Dtp_BetsEndTime.CustomFormat = "HH:mm";
            this.Dtp_BetsEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_BetsEndTime.Location = new System.Drawing.Point(312, 5);
            this.Dtp_BetsEndTime.Name = "Dtp_BetsEndTime";
            this.Dtp_BetsEndTime.ShowUpDown = true;
            this.Dtp_BetsEndTime.Size = new System.Drawing.Size(60, 23);
            this.Dtp_BetsEndTime.TabIndex = 178;
            this.Dtp_BetsEndTime.Value = new System.DateTime(2015, 7, 20, 22, 32, 0, 0);
            // 
            // Lbl_BetsTime2
            // 
            this.Lbl_BetsTime2.AutoSize = true;
            this.Lbl_BetsTime2.Location = new System.Drawing.Point(515, 8);
            this.Lbl_BetsTime2.Name = "Lbl_BetsTime2";
            this.Lbl_BetsTime2.Size = new System.Drawing.Size(32, 17);
            this.Lbl_BetsTime2.TabIndex = 195;
            this.Lbl_BetsTime2.Text = "秒后";
            // 
            // Lbl_BetsTime1
            // 
            this.Lbl_BetsTime1.AutoSize = true;
            this.Lbl_BetsTime1.Location = new System.Drawing.Point(386, 8);
            this.Lbl_BetsTime1.Name = "Lbl_BetsTime1";
            this.Lbl_BetsTime1.Size = new System.Drawing.Size(68, 17);
            this.Lbl_BetsTime1.TabIndex = 197;
            this.Lbl_BetsTime1.Text = "延迟投注：";
            // 
            // Nm_BetsTime
            // 
            this.Nm_BetsTime.Location = new System.Drawing.Point(456, 5);
            this.Nm_BetsTime.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Nm_BetsTime.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Nm_BetsTime.Name = "Nm_BetsTime";
            this.Nm_BetsTime.Size = new System.Drawing.Size(55, 23);
            this.Nm_BetsTime.TabIndex = 196;
            this.Nm_BetsTime.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Pnl_BetsInfoTop1
            // 
            this.Pnl_BetsInfoTop1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_BetsInfoTop1.Controls.Add(this.Pnl_BetsInfoExpect);
            this.Pnl_BetsInfoTop1.Controls.Add(this.Pnl_BetsInfoMN);
            this.Pnl_BetsInfoTop1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_BetsInfoTop1.Location = new System.Drawing.Point(0, 0);
            this.Pnl_BetsInfoTop1.Name = "Pnl_BetsInfoTop1";
            this.Pnl_BetsInfoTop1.Size = new System.Drawing.Size(555, 70);
            this.Pnl_BetsInfoTop1.TabIndex = 72;
            // 
            // Pnl_BetsInfoExpect
            // 
            this.Pnl_BetsInfoExpect.Controls.Add(this.Ckb_DQStopBets);
            this.Pnl_BetsInfoExpect.Controls.Add(this.Ckb_SBStopBets);
            this.Pnl_BetsInfoExpect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_BetsInfoExpect.Location = new System.Drawing.Point(460, 0);
            this.Pnl_BetsInfoExpect.Name = "Pnl_BetsInfoExpect";
            this.Pnl_BetsInfoExpect.Size = new System.Drawing.Size(93, 68);
            this.Pnl_BetsInfoExpect.TabIndex = 188;
            // 
            // Ckb_DQStopBets
            // 
            this.Ckb_DQStopBets.AutoSize = true;
            this.Ckb_DQStopBets.Checked = true;
            this.Ckb_DQStopBets.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Ckb_DQStopBets.Location = new System.Drawing.Point(6, 40);
            this.Ckb_DQStopBets.Name = "Ckb_DQStopBets";
            this.Ckb_DQStopBets.Size = new System.Drawing.Size(75, 21);
            this.Ckb_DQStopBets.TabIndex = 179;
            this.Ckb_DQStopBets.Text = "断期停投";
            this.Tot_Hint.SetToolTip(this.Ckb_DQStopBets, "当投注遇到断期时【永久停止投注】，直到重新开启投注或取消该选项\r\n");
            this.Ckb_DQStopBets.UseVisualStyleBackColor = true;
            // 
            // Ckb_SBStopBets
            // 
            this.Ckb_SBStopBets.AutoSize = true;
            this.Ckb_SBStopBets.Location = new System.Drawing.Point(6, 10);
            this.Ckb_SBStopBets.Name = "Ckb_SBStopBets";
            this.Ckb_SBStopBets.Size = new System.Drawing.Size(75, 21);
            this.Ckb_SBStopBets.TabIndex = 178;
            this.Ckb_SBStopBets.Text = "失败停投";
            this.Tot_Hint.SetToolTip(this.Ckb_SBStopBets, "方案投注失败时【当期停止投注】，直到下期才继续投注");
            this.Ckb_SBStopBets.UseVisualStyleBackColor = true;
            // 
            // Pnl_BetsInfoMN
            // 
            this.Pnl_BetsInfoMN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_BetsInfoMN.Controls.Add(this.Pnl_BetsInfoMNRight);
            this.Pnl_BetsInfoMN.Controls.Add(this.Ckb_MNBets);
            this.Pnl_BetsInfoMN.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_BetsInfoMN.Location = new System.Drawing.Point(0, 0);
            this.Pnl_BetsInfoMN.Name = "Pnl_BetsInfoMN";
            this.Pnl_BetsInfoMN.Size = new System.Drawing.Size(460, 68);
            this.Pnl_BetsInfoMN.TabIndex = 187;
            // 
            // Pnl_BetsInfoMNRight
            // 
            this.Pnl_BetsInfoMNRight.Controls.Add(this.Ckb_MN1);
            this.Pnl_BetsInfoMNRight.Controls.Add(this.Lbl_MNBets);
            this.Pnl_BetsInfoMNRight.Controls.Add(this.Txt_MN3);
            this.Pnl_BetsInfoMNRight.Controls.Add(this.Ckb_MN4);
            this.Pnl_BetsInfoMNRight.Controls.Add(this.Txt_MN1);
            this.Pnl_BetsInfoMNRight.Controls.Add(this.Ckb_MN3);
            this.Pnl_BetsInfoMNRight.Controls.Add(this.Ckb_MN2);
            this.Pnl_BetsInfoMNRight.Controls.Add(this.Txt_MN2);
            this.Pnl_BetsInfoMNRight.Controls.Add(this.Txt_MN4);
            this.Pnl_BetsInfoMNRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.Pnl_BetsInfoMNRight.Location = new System.Drawing.Point(83, 0);
            this.Pnl_BetsInfoMNRight.Name = "Pnl_BetsInfoMNRight";
            this.Pnl_BetsInfoMNRight.Size = new System.Drawing.Size(375, 66);
            this.Pnl_BetsInfoMNRight.TabIndex = 209;
            // 
            // Ckb_MN1
            // 
            this.Ckb_MN1.AutoSize = true;
            this.Ckb_MN1.Location = new System.Drawing.Point(5, 9);
            this.Ckb_MN1.Name = "Ckb_MN1";
            this.Ckb_MN1.Size = new System.Drawing.Size(87, 21);
            this.Ckb_MN1.TabIndex = 177;
            this.Ckb_MN1.Text = "模拟投注输";
            this.Ckb_MN1.UseVisualStyleBackColor = true;
            this.Ckb_MN1.CheckedChanged += new System.EventHandler(this.Ckb_MN1_CheckedChanged);
            // 
            // Lbl_MNBets
            // 
            this.Lbl_MNBets.AutoSize = true;
            this.Lbl_MNBets.Location = new System.Drawing.Point(160, 28);
            this.Lbl_MNBets.Name = "Lbl_MNBets";
            this.Lbl_MNBets.Size = new System.Drawing.Size(56, 17);
            this.Lbl_MNBets.TabIndex = 194;
            this.Lbl_MNBets.Text = "循环切换";
            // 
            // Txt_MN3
            // 
            this.Txt_MN3.Enabled = false;
            this.Txt_MN3.Location = new System.Drawing.Point(96, 37);
            this.Txt_MN3.Name = "Txt_MN3";
            this.Txt_MN3.Size = new System.Drawing.Size(60, 23);
            this.Txt_MN3.TabIndex = 190;
            this.Txt_MN3.Text = "50000";
            // 
            // Ckb_MN4
            // 
            this.Ckb_MN4.AutoSize = true;
            this.Ckb_MN4.Location = new System.Drawing.Point(221, 39);
            this.Ckb_MN4.Name = "Ckb_MN4";
            this.Ckb_MN4.Size = new System.Drawing.Size(87, 21);
            this.Ckb_MN4.TabIndex = 192;
            this.Ckb_MN4.Text = "真实投注输";
            this.Ckb_MN4.UseVisualStyleBackColor = true;
            this.Ckb_MN4.CheckedChanged += new System.EventHandler(this.Ckb_MN4_CheckedChanged);
            // 
            // Txt_MN1
            // 
            this.Txt_MN1.Enabled = false;
            this.Txt_MN1.Location = new System.Drawing.Point(96, 7);
            this.Txt_MN1.Name = "Txt_MN1";
            this.Txt_MN1.Size = new System.Drawing.Size(60, 23);
            this.Txt_MN1.TabIndex = 178;
            this.Txt_MN1.Text = "50000";
            // 
            // Ckb_MN3
            // 
            this.Ckb_MN3.AutoSize = true;
            this.Ckb_MN3.Location = new System.Drawing.Point(5, 39);
            this.Ckb_MN3.Name = "Ckb_MN3";
            this.Ckb_MN3.Size = new System.Drawing.Size(87, 21);
            this.Ckb_MN3.TabIndex = 189;
            this.Ckb_MN3.Text = "模拟投注赢";
            this.Ckb_MN3.UseVisualStyleBackColor = true;
            this.Ckb_MN3.CheckedChanged += new System.EventHandler(this.Ckb_MN3_CheckedChanged);
            // 
            // Ckb_MN2
            // 
            this.Ckb_MN2.AutoSize = true;
            this.Ckb_MN2.Location = new System.Drawing.Point(220, 9);
            this.Ckb_MN2.Name = "Ckb_MN2";
            this.Ckb_MN2.Size = new System.Drawing.Size(87, 21);
            this.Ckb_MN2.TabIndex = 180;
            this.Ckb_MN2.Text = "真实投注赢";
            this.Ckb_MN2.UseVisualStyleBackColor = true;
            this.Ckb_MN2.CheckedChanged += new System.EventHandler(this.Ckb_MN2_CheckedChanged);
            // 
            // Txt_MN2
            // 
            this.Txt_MN2.Enabled = false;
            this.Txt_MN2.Location = new System.Drawing.Point(311, 7);
            this.Txt_MN2.Name = "Txt_MN2";
            this.Txt_MN2.Size = new System.Drawing.Size(60, 23);
            this.Txt_MN2.TabIndex = 181;
            this.Txt_MN2.Text = "50000";
            // 
            // Txt_MN4
            // 
            this.Txt_MN4.Enabled = false;
            this.Txt_MN4.Location = new System.Drawing.Point(311, 37);
            this.Txt_MN4.Name = "Txt_MN4";
            this.Txt_MN4.Size = new System.Drawing.Size(60, 23);
            this.Txt_MN4.TabIndex = 193;
            this.Txt_MN4.Text = "50000";
            // 
            // Ckb_MNBets
            // 
            this.Ckb_MNBets.AutoSize = true;
            this.Ckb_MNBets.Location = new System.Drawing.Point(5, 24);
            this.Ckb_MNBets.Name = "Ckb_MNBets";
            this.Ckb_MNBets.Size = new System.Drawing.Size(75, 21);
            this.Ckb_MNBets.TabIndex = 186;
            this.Ckb_MNBets.Text = "模拟投注";
            this.Ckb_MNBets.UseVisualStyleBackColor = true;
            this.Ckb_MNBets.CheckedChanged += new System.EventHandler(this.Ckb_MNBets_CheckedChanged);
            // 
            // Tap_Scheme
            // 
            this.Tap_Scheme.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_Scheme.Controls.Add(this.Pnl_Scheme);
            this.Tap_Scheme.Location = new System.Drawing.Point(4, 34);
            this.Tap_Scheme.Name = "Tap_Scheme";
            this.Tap_Scheme.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_Scheme.Size = new System.Drawing.Size(998, 536);
            this.Tap_Scheme.TabIndex = 2;
            this.Tap_Scheme.Text = "方案设定";
            // 
            // Pnl_Scheme
            // 
            this.Pnl_Scheme.Controls.Add(this.Pnl_SchemeMain);
            this.Pnl_Scheme.Controls.Add(this.Pnl_SchemeLeft);
            this.Pnl_Scheme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_Scheme.Location = new System.Drawing.Point(3, 3);
            this.Pnl_Scheme.Name = "Pnl_Scheme";
            this.Pnl_Scheme.Size = new System.Drawing.Size(992, 530);
            this.Pnl_Scheme.TabIndex = 0;
            // 
            // Pnl_SchemeMain
            // 
            this.Pnl_SchemeMain.Controls.Add(this.Pnl_SchemeInfo);
            this.Pnl_SchemeMain.Controls.Add(this.Pnl_SchemeTop2);
            this.Pnl_SchemeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_SchemeMain.Location = new System.Drawing.Point(450, 0);
            this.Pnl_SchemeMain.Name = "Pnl_SchemeMain";
            this.Pnl_SchemeMain.Size = new System.Drawing.Size(542, 530);
            this.Pnl_SchemeMain.TabIndex = 1;
            // 
            // Pnl_SchemeInfo
            // 
            this.Pnl_SchemeInfo.AutoScroll = true;
            this.Pnl_SchemeInfo.Controls.Add(this.Lbl_FNEncrypt);
            this.Pnl_SchemeInfo.Controls.Add(this.FN_KMTM);
            this.Pnl_SchemeInfo.Controls.Add(this.FN_YLCH);
            this.Pnl_SchemeInfo.Controls.Add(this.FN_GJDMLH);
            this.Pnl_SchemeInfo.Controls.Add(this.FN_SJCH);
            this.Pnl_SchemeInfo.Controls.Add(this.FN_WJJH);
            this.Pnl_SchemeInfo.Controls.Add(this.FN_LRWCH);
            this.Pnl_SchemeInfo.Controls.Add(this.FN_BCFCH);
            this.Pnl_SchemeInfo.Controls.Add(this.FN_GJKMTM);
            this.Pnl_SchemeInfo.Controls.Add(this.FN_LHKMTM);
            this.Pnl_SchemeInfo.Controls.Add(this.FN_DMLH);
            this.Pnl_SchemeInfo.Controls.Add(this.FN_GDQM);
            this.Pnl_SchemeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_SchemeInfo.Location = new System.Drawing.Point(0, 35);
            this.Pnl_SchemeInfo.Name = "Pnl_SchemeInfo";
            this.Pnl_SchemeInfo.Size = new System.Drawing.Size(542, 495);
            this.Pnl_SchemeInfo.TabIndex = 77;
            // 
            // Lbl_FNEncrypt
            // 
            this.Lbl_FNEncrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Lbl_FNEncrypt.Font = new System.Drawing.Font("微软雅黑", 30F);
            this.Lbl_FNEncrypt.Location = new System.Drawing.Point(0, 0);
            this.Lbl_FNEncrypt.Name = "Lbl_FNEncrypt";
            this.Lbl_FNEncrypt.Size = new System.Drawing.Size(542, 495);
            this.Lbl_FNEncrypt.TabIndex = 1;
            this.Lbl_FNEncrypt.Text = "已加密";
            this.Lbl_FNEncrypt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FN_KMTM
            // 
            this.FN_KMTM.AutoScroll = true;
            this.FN_KMTM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FN_KMTM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FN_KMTM.Hint = "开某投某";
            this.FN_KMTM.Location = new System.Drawing.Point(0, 0);
            this.FN_KMTM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FN_KMTM.Name = "FN_KMTM";
            this.FN_KMTM.Size = new System.Drawing.Size(542, 495);
            this.FN_KMTM.TabIndex = 8;
            // 
            // FN_YLCH
            // 
            this.FN_YLCH.AutoScroll = true;
            this.FN_YLCH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FN_YLCH.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FN_YLCH.Hint = "遗漏出号";
            this.FN_YLCH.Location = new System.Drawing.Point(0, 0);
            this.FN_YLCH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FN_YLCH.Name = "FN_YLCH";
            this.FN_YLCH.Size = new System.Drawing.Size(542, 495);
            this.FN_YLCH.TabIndex = 7;
            // 
            // FN_GJDMLH
            // 
            this.FN_GJDMLH.AutoScroll = true;
            this.FN_GJDMLH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FN_GJDMLH.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FN_GJDMLH.Hint = "高级定码轮换";
            this.FN_GJDMLH.Location = new System.Drawing.Point(0, 0);
            this.FN_GJDMLH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FN_GJDMLH.Name = "FN_GJDMLH";
            this.FN_GJDMLH.Size = new System.Drawing.Size(542, 495);
            this.FN_GJDMLH.TabIndex = 6;
            // 
            // FN_SJCH
            // 
            this.FN_SJCH.AutoScroll = true;
            this.FN_SJCH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FN_SJCH.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FN_SJCH.Hint = "随机出号";
            this.FN_SJCH.Location = new System.Drawing.Point(0, 0);
            this.FN_SJCH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FN_SJCH.Name = "FN_SJCH";
            this.FN_SJCH.Size = new System.Drawing.Size(542, 495);
            this.FN_SJCH.TabIndex = 4;
            // 
            // FN_WJJH
            // 
            this.FN_WJJH.AutoScroll = true;
            this.FN_WJJH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FN_WJJH.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FN_WJJH.Hint = "外接计划";
            this.FN_WJJH.Location = new System.Drawing.Point(0, 0);
            this.FN_WJJH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FN_WJJH.Name = "FN_WJJH";
            this.FN_WJJH.Size = new System.Drawing.Size(542, 495);
            this.FN_WJJH.TabIndex = 3;
            // 
            // FN_LRWCH
            // 
            this.FN_LRWCH.AutoScroll = true;
            this.FN_LRWCH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FN_LRWCH.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FN_LRWCH.Hint = "冷热温出号";
            this.FN_LRWCH.Location = new System.Drawing.Point(0, 0);
            this.FN_LRWCH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FN_LRWCH.Name = "FN_LRWCH";
            this.FN_LRWCH.Size = new System.Drawing.Size(542, 495);
            this.FN_LRWCH.TabIndex = 2;
            // 
            // FN_BCFCH
            // 
            this.FN_BCFCH.AutoScroll = true;
            this.FN_BCFCH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FN_BCFCH.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FN_BCFCH.Hint = "不重复出号";
            this.FN_BCFCH.Location = new System.Drawing.Point(0, 0);
            this.FN_BCFCH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FN_BCFCH.Name = "FN_BCFCH";
            this.FN_BCFCH.Size = new System.Drawing.Size(542, 495);
            this.FN_BCFCH.TabIndex = 11;
            // 
            // FN_GJKMTM
            // 
            this.FN_GJKMTM.AutoScroll = true;
            this.FN_GJKMTM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FN_GJKMTM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FN_GJKMTM.Hint = "高级开某投某";
            this.FN_GJKMTM.Location = new System.Drawing.Point(0, 0);
            this.FN_GJKMTM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FN_GJKMTM.Name = "FN_GJKMTM";
            this.FN_GJKMTM.Size = new System.Drawing.Size(542, 495);
            this.FN_GJKMTM.TabIndex = 1;
            // 
            // FN_LHKMTM
            // 
            this.FN_LHKMTM.AutoScroll = true;
            this.FN_LHKMTM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FN_LHKMTM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FN_LHKMTM.Hint = "龙虎开某投某";
            this.FN_LHKMTM.Location = new System.Drawing.Point(0, 0);
            this.FN_LHKMTM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FN_LHKMTM.Name = "FN_LHKMTM";
            this.FN_LHKMTM.Size = new System.Drawing.Size(542, 495);
            this.FN_LHKMTM.TabIndex = 77;
            // 
            // FN_DMLH
            // 
            this.FN_DMLH.AutoScroll = true;
            this.FN_DMLH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FN_DMLH.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FN_DMLH.Hint = "定码轮换";
            this.FN_DMLH.Location = new System.Drawing.Point(0, 0);
            this.FN_DMLH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FN_DMLH.Name = "FN_DMLH";
            this.FN_DMLH.Size = new System.Drawing.Size(542, 495);
            this.FN_DMLH.TabIndex = 0;
            // 
            // FN_GDQM
            // 
            this.FN_GDQM.AutoScroll = true;
            this.FN_GDQM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FN_GDQM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FN_GDQM.Hint = "固定取码";
            this.FN_GDQM.Location = new System.Drawing.Point(0, 0);
            this.FN_GDQM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FN_GDQM.Name = "FN_GDQM";
            this.FN_GDQM.Size = new System.Drawing.Size(542, 495);
            this.FN_GDQM.TabIndex = 9;
            // 
            // Pnl_SchemeTop2
            // 
            this.Pnl_SchemeTop2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_SchemeTop2.Controls.Add(this.Ckb_CancelScheme);
            this.Pnl_SchemeTop2.Controls.Add(this.Ckb_SaveScheme);
            this.Pnl_SchemeTop2.Controls.Add(this.Lbl_FNCHType);
            this.Pnl_SchemeTop2.Controls.Add(this.Cbb_FNCHType);
            this.Pnl_SchemeTop2.Controls.Add(this.Txt_FNName);
            this.Pnl_SchemeTop2.Controls.Add(this.Lbl_FNName);
            this.Pnl_SchemeTop2.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_SchemeTop2.Location = new System.Drawing.Point(0, 0);
            this.Pnl_SchemeTop2.Name = "Pnl_SchemeTop2";
            this.Pnl_SchemeTop2.Size = new System.Drawing.Size(542, 35);
            this.Pnl_SchemeTop2.TabIndex = 76;
            // 
            // Ckb_CancelScheme
            // 
            this.Ckb_CancelScheme.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_CancelScheme.AutoCheck = false;
            this.Ckb_CancelScheme.FlatAppearance.BorderSize = 0;
            this.Ckb_CancelScheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_CancelScheme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_CancelScheme.Location = new System.Drawing.Point(444, 4);
            this.Ckb_CancelScheme.Name = "Ckb_CancelScheme";
            this.Ckb_CancelScheme.Size = new System.Drawing.Size(60, 25);
            this.Ckb_CancelScheme.TabIndex = 177;
            this.Ckb_CancelScheme.Text = "放弃";
            this.Ckb_CancelScheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_CancelScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_FNHint.SetToolTip(this.Ckb_CancelScheme, "放弃当前方案的设置");
            this.Ckb_CancelScheme.UseVisualStyleBackColor = true;
            this.Ckb_CancelScheme.Click += new System.EventHandler(this.Ckb_CancelScheme_Click);
            // 
            // Ckb_SaveScheme
            // 
            this.Ckb_SaveScheme.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_SaveScheme.AutoCheck = false;
            this.Ckb_SaveScheme.FlatAppearance.BorderSize = 0;
            this.Ckb_SaveScheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_SaveScheme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_SaveScheme.Location = new System.Drawing.Point(378, 4);
            this.Ckb_SaveScheme.Name = "Ckb_SaveScheme";
            this.Ckb_SaveScheme.Size = new System.Drawing.Size(60, 25);
            this.Ckb_SaveScheme.TabIndex = 176;
            this.Ckb_SaveScheme.Text = "保存";
            this.Ckb_SaveScheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_SaveScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_FNHint.SetToolTip(this.Ckb_SaveScheme, "保存当前方案");
            this.Ckb_SaveScheme.UseVisualStyleBackColor = true;
            this.Ckb_SaveScheme.Click += new System.EventHandler(this.Ckb_SaveScheme_Click);
            // 
            // Lbl_FNCHType
            // 
            this.Lbl_FNCHType.AutoSize = true;
            this.Lbl_FNCHType.Location = new System.Drawing.Point(204, 8);
            this.Lbl_FNCHType.Name = "Lbl_FNCHType";
            this.Lbl_FNCHType.Size = new System.Drawing.Size(44, 17);
            this.Lbl_FNCHType.TabIndex = 175;
            this.Lbl_FNCHType.Text = "类别：";
            // 
            // Cbb_FNCHType
            // 
            this.Cbb_FNCHType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbb_FNCHType.FormattingEnabled = true;
            this.Cbb_FNCHType.Items.AddRange(new object[] {
            "前五后五正反投注"});
            this.Cbb_FNCHType.Location = new System.Drawing.Point(254, 4);
            this.Cbb_FNCHType.Name = "Cbb_FNCHType";
            this.Cbb_FNCHType.Size = new System.Drawing.Size(120, 25);
            this.Cbb_FNCHType.TabIndex = 174;
            this.Cbb_FNCHType.SelectedIndexChanged += new System.EventHandler(this.Cbb_FNCHType_SelectedIndexChanged);
            // 
            // Txt_FNName
            // 
            this.Txt_FNName.Location = new System.Drawing.Point(53, 5);
            this.Txt_FNName.Name = "Txt_FNName";
            this.Txt_FNName.ReadOnly = true;
            this.Txt_FNName.Size = new System.Drawing.Size(145, 23);
            this.Txt_FNName.TabIndex = 173;
            // 
            // Lbl_FNName
            // 
            this.Lbl_FNName.AutoSize = true;
            this.Lbl_FNName.Location = new System.Drawing.Point(3, 8);
            this.Lbl_FNName.Name = "Lbl_FNName";
            this.Lbl_FNName.Size = new System.Drawing.Size(44, 17);
            this.Lbl_FNName.TabIndex = 152;
            this.Lbl_FNName.Text = "名称：";
            // 
            // Pnl_SchemeLeft
            // 
            this.Pnl_SchemeLeft.Controls.Add(this.Egv_SchemeList);
            this.Pnl_SchemeLeft.Controls.Add(this.Pnl_SchemeBottom);
            this.Pnl_SchemeLeft.Controls.Add(this.Pnl_SchemeTop1);
            this.Pnl_SchemeLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_SchemeLeft.Location = new System.Drawing.Point(0, 0);
            this.Pnl_SchemeLeft.Name = "Pnl_SchemeLeft";
            this.Pnl_SchemeLeft.Size = new System.Drawing.Size(450, 530);
            this.Pnl_SchemeLeft.TabIndex = 0;
            // 
            // Egv_SchemeList
            // 
            this.Egv_SchemeList.AllowUserToAddRows = false;
            this.Egv_SchemeList.AllowUserToDeleteRows = false;
            this.Egv_SchemeList.AllowUserToResizeColumns = false;
            this.Egv_SchemeList.AllowUserToResizeRows = false;
            this.Egv_SchemeList.BackgroundColor = System.Drawing.Color.White;
            this.Egv_SchemeList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Egv_SchemeList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_SchemeList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.Egv_SchemeList.ColumnHeadersHeight = 30;
            this.Egv_SchemeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_SchemeList.DefaultCellStyle = dataGridViewCellStyle5;
            this.Egv_SchemeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Egv_SchemeList.DragLineColor = System.Drawing.Color.Silver;
            this.Egv_SchemeList.ExternalVirtualMode = true;
            this.Egv_SchemeList.GridColor = System.Drawing.Color.Silver;
            this.Egv_SchemeList.HeadersCheckDefult = System.Windows.Forms.CheckState.Checked;
            this.Egv_SchemeList.Location = new System.Drawing.Point(0, 35);
            this.Egv_SchemeList.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Egv_SchemeList.MergeColumnHeaderForeColor = System.Drawing.Color.Black;
            this.Egv_SchemeList.MultiSelect = false;
            this.Egv_SchemeList.Name = "Egv_SchemeList";
            this.Egv_SchemeList.RowHeadersVisible = false;
            this.Egv_SchemeList.RowNum = 17;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            this.Egv_SchemeList.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.Egv_SchemeList.RowTemplate.Height = 23;
            this.Egv_SchemeList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Egv_SchemeList.Size = new System.Drawing.Size(450, 460);
            this.Egv_SchemeList.TabIndex = 74;
            this.Egv_SchemeList.VirtualMode = true;
            this.Egv_SchemeList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Egv_SchmeeList_CellClick);
            this.Egv_SchemeList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Egv_SchemeList_CellDoubleClick);
            // 
            // Pnl_SchemeBottom
            // 
            this.Pnl_SchemeBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_SchemeBottom.Controls.Add(this.Pnl_SchemeShare);
            this.Pnl_SchemeBottom.Controls.Add(this.Ckb_ClearScheme);
            this.Pnl_SchemeBottom.Controls.Add(this.Ckb_ExportScheme);
            this.Pnl_SchemeBottom.Controls.Add(this.Ckb_ImportScheme);
            this.Pnl_SchemeBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Pnl_SchemeBottom.Location = new System.Drawing.Point(0, 495);
            this.Pnl_SchemeBottom.Name = "Pnl_SchemeBottom";
            this.Pnl_SchemeBottom.Size = new System.Drawing.Size(450, 35);
            this.Pnl_SchemeBottom.TabIndex = 76;
            this.Pnl_SchemeBottom.Visible = false;
            // 
            // Pnl_SchemeShare
            // 
            this.Pnl_SchemeShare.Controls.Add(this.Ckb_ShareSchemeManage);
            this.Pnl_SchemeShare.Controls.Add(this.Ckb_ShareScheme);
            this.Pnl_SchemeShare.Dock = System.Windows.Forms.DockStyle.Right;
            this.Pnl_SchemeShare.Location = new System.Drawing.Point(288, 0);
            this.Pnl_SchemeShare.Name = "Pnl_SchemeShare";
            this.Pnl_SchemeShare.Size = new System.Drawing.Size(160, 33);
            this.Pnl_SchemeShare.TabIndex = 310;
            this.Pnl_SchemeShare.Visible = false;
            // 
            // Ckb_ShareSchemeManage
            // 
            this.Ckb_ShareSchemeManage.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_ShareSchemeManage.AutoCheck = false;
            this.Ckb_ShareSchemeManage.FlatAppearance.BorderSize = 0;
            this.Ckb_ShareSchemeManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_ShareSchemeManage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_ShareSchemeManage.Location = new System.Drawing.Point(8, 4);
            this.Ckb_ShareSchemeManage.Name = "Ckb_ShareSchemeManage";
            this.Ckb_ShareSchemeManage.Size = new System.Drawing.Size(80, 25);
            this.Ckb_ShareSchemeManage.TabIndex = 214;
            this.Ckb_ShareSchemeManage.Text = "共享管理";
            this.Ckb_ShareSchemeManage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_ShareSchemeManage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Ckb_ShareSchemeManage.UseVisualStyleBackColor = false;
            this.Ckb_ShareSchemeManage.Click += new System.EventHandler(this.Ckb_ShareSchemeManage_Click);
            // 
            // Ckb_ShareScheme
            // 
            this.Ckb_ShareScheme.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_ShareScheme.AutoCheck = false;
            this.Ckb_ShareScheme.FlatAppearance.BorderSize = 0;
            this.Ckb_ShareScheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_ShareScheme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_ShareScheme.Location = new System.Drawing.Point(94, 4);
            this.Ckb_ShareScheme.Name = "Ckb_ShareScheme";
            this.Ckb_ShareScheme.Size = new System.Drawing.Size(60, 25);
            this.Ckb_ShareScheme.TabIndex = 166;
            this.Ckb_ShareScheme.Text = "上传";
            this.Ckb_ShareScheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_ShareScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Ckb_ShareScheme.UseVisualStyleBackColor = true;
            this.Ckb_ShareScheme.Click += new System.EventHandler(this.Ckb_ShareScheme_Click);
            // 
            // Ckb_ClearScheme
            // 
            this.Ckb_ClearScheme.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_ClearScheme.AutoCheck = false;
            this.Ckb_ClearScheme.FlatAppearance.BorderSize = 0;
            this.Ckb_ClearScheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_ClearScheme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_ClearScheme.Location = new System.Drawing.Point(136, 4);
            this.Ckb_ClearScheme.Name = "Ckb_ClearScheme";
            this.Ckb_ClearScheme.Size = new System.Drawing.Size(60, 25);
            this.Ckb_ClearScheme.TabIndex = 309;
            this.Ckb_ClearScheme.Text = "清空";
            this.Ckb_ClearScheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_ClearScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_FNHint.SetToolTip(this.Ckb_ClearScheme, "清空当前全部方案");
            this.Ckb_ClearScheme.UseVisualStyleBackColor = true;
            this.Ckb_ClearScheme.Click += new System.EventHandler(this.Ckb_ClearScheme_Click);
            // 
            // Ckb_ExportScheme
            // 
            this.Ckb_ExportScheme.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_ExportScheme.AutoCheck = false;
            this.Ckb_ExportScheme.FlatAppearance.BorderSize = 0;
            this.Ckb_ExportScheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_ExportScheme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_ExportScheme.Location = new System.Drawing.Point(70, 4);
            this.Ckb_ExportScheme.Name = "Ckb_ExportScheme";
            this.Ckb_ExportScheme.Size = new System.Drawing.Size(60, 25);
            this.Ckb_ExportScheme.TabIndex = 164;
            this.Ckb_ExportScheme.Text = "导出";
            this.Ckb_ExportScheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_ExportScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_FNHint.SetToolTip(this.Ckb_ExportScheme, "将当前方案导出到文件夹");
            this.Ckb_ExportScheme.UseVisualStyleBackColor = true;
            this.Ckb_ExportScheme.Click += new System.EventHandler(this.Ckb_ExportScheme_Click);
            // 
            // Ckb_ImportScheme
            // 
            this.Ckb_ImportScheme.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_ImportScheme.AutoCheck = false;
            this.Ckb_ImportScheme.FlatAppearance.BorderSize = 0;
            this.Ckb_ImportScheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_ImportScheme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_ImportScheme.Location = new System.Drawing.Point(4, 4);
            this.Ckb_ImportScheme.Name = "Ckb_ImportScheme";
            this.Ckb_ImportScheme.Size = new System.Drawing.Size(60, 25);
            this.Ckb_ImportScheme.TabIndex = 163;
            this.Ckb_ImportScheme.Text = "导入";
            this.Ckb_ImportScheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_ImportScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_FNHint.SetToolTip(this.Ckb_ImportScheme, "从文件夹中导入方案");
            this.Ckb_ImportScheme.UseVisualStyleBackColor = true;
            this.Ckb_ImportScheme.Click += new System.EventHandler(this.Ckb_ImportScheme_Click);
            // 
            // Pnl_SchemeTop1
            // 
            this.Pnl_SchemeTop1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_SchemeTop1.Controls.Add(this.Ckb_EditTimesPlan);
            this.Pnl_SchemeTop1.Controls.Add(this.Ckb_EditScheme);
            this.Pnl_SchemeTop1.Controls.Add(this.Ckb_DeleteScheme);
            this.Pnl_SchemeTop1.Controls.Add(this.Ckb_CopyScheme);
            this.Pnl_SchemeTop1.Controls.Add(this.Ckb_AddScheme);
            this.Pnl_SchemeTop1.Controls.Add(this.Ckb_FNLT);
            this.Pnl_SchemeTop1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_SchemeTop1.Location = new System.Drawing.Point(0, 0);
            this.Pnl_SchemeTop1.Name = "Pnl_SchemeTop1";
            this.Pnl_SchemeTop1.Size = new System.Drawing.Size(450, 35);
            this.Pnl_SchemeTop1.TabIndex = 75;
            // 
            // Ckb_EditTimesPlan
            // 
            this.Ckb_EditTimesPlan.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_EditTimesPlan.AutoCheck = false;
            this.Ckb_EditTimesPlan.FlatAppearance.BorderSize = 0;
            this.Ckb_EditTimesPlan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_EditTimesPlan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_EditTimesPlan.Location = new System.Drawing.Point(297, 4);
            this.Ckb_EditTimesPlan.Name = "Ckb_EditTimesPlan";
            this.Ckb_EditTimesPlan.Size = new System.Drawing.Size(80, 25);
            this.Ckb_EditTimesPlan.TabIndex = 214;
            this.Ckb_EditTimesPlan.Text = "修改倍投";
            this.Ckb_EditTimesPlan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_EditTimesPlan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Tot_FNHint.SetToolTip(this.Ckb_EditTimesPlan, "将当前方案的直选倍投批量修改到其他方案中");
            this.Ckb_EditTimesPlan.UseVisualStyleBackColor = true;
            this.Ckb_EditTimesPlan.Click += new System.EventHandler(this.Ckb_EditTimesPlan_Click);
            // 
            // Ckb_EditScheme
            // 
            this.Ckb_EditScheme.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_EditScheme.AutoCheck = false;
            this.Ckb_EditScheme.FlatAppearance.BorderSize = 0;
            this.Ckb_EditScheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_EditScheme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_EditScheme.Location = new System.Drawing.Point(383, 4);
            this.Ckb_EditScheme.Name = "Ckb_EditScheme";
            this.Ckb_EditScheme.Size = new System.Drawing.Size(60, 25);
            this.Ckb_EditScheme.TabIndex = 166;
            this.Ckb_EditScheme.Text = "编辑";
            this.Ckb_EditScheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_EditScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_FNHint.SetToolTip(this.Ckb_EditScheme, "编辑当前选中的一个方案");
            this.Ckb_EditScheme.UseVisualStyleBackColor = true;
            this.Ckb_EditScheme.Click += new System.EventHandler(this.Ckb_EditScheme_Click);
            // 
            // Ckb_DeleteScheme
            // 
            this.Ckb_DeleteScheme.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_DeleteScheme.AutoCheck = false;
            this.Ckb_DeleteScheme.FlatAppearance.BorderSize = 0;
            this.Ckb_DeleteScheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_DeleteScheme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_DeleteScheme.Location = new System.Drawing.Point(136, 4);
            this.Ckb_DeleteScheme.Name = "Ckb_DeleteScheme";
            this.Ckb_DeleteScheme.Size = new System.Drawing.Size(60, 25);
            this.Ckb_DeleteScheme.TabIndex = 165;
            this.Ckb_DeleteScheme.Text = "删除";
            this.Ckb_DeleteScheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_DeleteScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_FNHint.SetToolTip(this.Ckb_DeleteScheme, "删除当前选中的一个方案");
            this.Ckb_DeleteScheme.UseVisualStyleBackColor = true;
            this.Ckb_DeleteScheme.Click += new System.EventHandler(this.Ckb_DeleteScheme_Click);
            // 
            // Ckb_CopyScheme
            // 
            this.Ckb_CopyScheme.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_CopyScheme.AutoCheck = false;
            this.Ckb_CopyScheme.FlatAppearance.BorderSize = 0;
            this.Ckb_CopyScheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_CopyScheme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_CopyScheme.Location = new System.Drawing.Point(70, 4);
            this.Ckb_CopyScheme.Name = "Ckb_CopyScheme";
            this.Ckb_CopyScheme.Size = new System.Drawing.Size(60, 25);
            this.Ckb_CopyScheme.TabIndex = 164;
            this.Ckb_CopyScheme.Text = "复制";
            this.Ckb_CopyScheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_CopyScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_FNHint.SetToolTip(this.Ckb_CopyScheme, "复制当前选中的一个方案");
            this.Ckb_CopyScheme.UseVisualStyleBackColor = true;
            this.Ckb_CopyScheme.Click += new System.EventHandler(this.Ckb_CopyScheme_Click);
            // 
            // Ckb_AddScheme
            // 
            this.Ckb_AddScheme.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_AddScheme.AutoCheck = false;
            this.Ckb_AddScheme.FlatAppearance.BorderSize = 0;
            this.Ckb_AddScheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_AddScheme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_AddScheme.Location = new System.Drawing.Point(4, 4);
            this.Ckb_AddScheme.Name = "Ckb_AddScheme";
            this.Ckb_AddScheme.Size = new System.Drawing.Size(60, 25);
            this.Ckb_AddScheme.TabIndex = 163;
            this.Ckb_AddScheme.Text = "添加";
            this.Ckb_AddScheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_AddScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_FNHint.SetToolTip(this.Ckb_AddScheme, "添加一个方案");
            this.Ckb_AddScheme.UseVisualStyleBackColor = true;
            this.Ckb_AddScheme.Click += new System.EventHandler(this.Ckb_AddScheme_Click);
            // 
            // Ckb_FNLT
            // 
            this.Ckb_FNLT.AutoSize = true;
            this.Ckb_FNLT.Location = new System.Drawing.Point(202, 7);
            this.Ckb_FNLT.Name = "Ckb_FNLT";
            this.Ckb_FNLT.Size = new System.Drawing.Size(75, 21);
            this.Ckb_FNLT.TabIndex = 154;
            this.Ckb_FNLT.Text = "方案轮投";
            this.Tot_FNHint.SetToolTip(this.Ckb_FNLT, "方案轮投的设置请在高级倍投中设定");
            this.Ckb_FNLT.UseVisualStyleBackColor = true;
            // 
            // Tap_LSData
            // 
            this.Tap_LSData.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_LSData.Controls.Add(this.Pnl_LSData);
            this.Tap_LSData.Location = new System.Drawing.Point(4, 34);
            this.Tap_LSData.Name = "Tap_LSData";
            this.Tap_LSData.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_LSData.Size = new System.Drawing.Size(998, 536);
            this.Tap_LSData.TabIndex = 3;
            this.Tap_LSData.Text = "参考数据";
            // 
            // Pnl_LSData
            // 
            this.Pnl_LSData.Controls.Add(this.Pnl_LSDataMain);
            this.Pnl_LSData.Controls.Add(this.Pnl_LSDataTop);
            this.Pnl_LSData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_LSData.Location = new System.Drawing.Point(3, 3);
            this.Pnl_LSData.Name = "Pnl_LSData";
            this.Pnl_LSData.Size = new System.Drawing.Size(992, 530);
            this.Pnl_LSData.TabIndex = 0;
            // 
            // Pnl_LSDataMain
            // 
            this.Pnl_LSDataMain.Controls.Add(this.Egv_LSDataList);
            this.Pnl_LSDataMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_LSDataMain.Location = new System.Drawing.Point(0, 70);
            this.Pnl_LSDataMain.Name = "Pnl_LSDataMain";
            this.Pnl_LSDataMain.Size = new System.Drawing.Size(992, 460);
            this.Pnl_LSDataMain.TabIndex = 72;
            // 
            // Egv_LSDataList
            // 
            this.Egv_LSDataList.AllowUserToAddRows = false;
            this.Egv_LSDataList.AllowUserToDeleteRows = false;
            this.Egv_LSDataList.AllowUserToResizeColumns = false;
            this.Egv_LSDataList.AllowUserToResizeRows = false;
            this.Egv_LSDataList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.Egv_LSDataList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Egv_LSDataList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_LSDataList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.Egv_LSDataList.ColumnHeadersHeight = 30;
            this.Egv_LSDataList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_LSDataList.DefaultCellStyle = dataGridViewCellStyle8;
            this.Egv_LSDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Egv_LSDataList.DragLineColor = System.Drawing.Color.Silver;
            this.Egv_LSDataList.ExternalVirtualMode = true;
            this.Egv_LSDataList.GridColor = System.Drawing.Color.Silver;
            this.Egv_LSDataList.HeadersCheckDefult = System.Windows.Forms.CheckState.Checked;
            this.Egv_LSDataList.Location = new System.Drawing.Point(0, 0);
            this.Egv_LSDataList.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Egv_LSDataList.MergeColumnHeaderForeColor = System.Drawing.Color.Black;
            this.Egv_LSDataList.MultiSelect = false;
            this.Egv_LSDataList.Name = "Egv_LSDataList";
            this.Egv_LSDataList.RowHeadersVisible = false;
            this.Egv_LSDataList.RowNum = 17;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black;
            this.Egv_LSDataList.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.Egv_LSDataList.RowTemplate.Height = 23;
            this.Egv_LSDataList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Egv_LSDataList.Size = new System.Drawing.Size(992, 460);
            this.Egv_LSDataList.TabIndex = 70;
            this.Egv_LSDataList.VirtualMode = true;
            // 
            // Pnl_LSDataTop
            // 
            this.Pnl_LSDataTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_LSDataTop.Controls.Add(this.Pnl_LSDataRight);
            this.Pnl_LSDataTop.Controls.Add(this.Pnl_LSDataLeft);
            this.Pnl_LSDataTop.Controls.Add(this.Pnl_LSDataTop1);
            this.Pnl_LSDataTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_LSDataTop.Location = new System.Drawing.Point(0, 0);
            this.Pnl_LSDataTop.Name = "Pnl_LSDataTop";
            this.Pnl_LSDataTop.Size = new System.Drawing.Size(992, 70);
            this.Pnl_LSDataTop.TabIndex = 71;
            // 
            // Pnl_LSDataRight
            // 
            this.Pnl_LSDataRight.BackColor = System.Drawing.Color.Transparent;
            this.Pnl_LSDataRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_LSDataRight.Controls.Add(this.Ckb_LSStop);
            this.Pnl_LSDataRight.Controls.Add(this.Lbl_LSRefreshHint);
            this.Pnl_LSDataRight.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_LSDataRight.Location = new System.Drawing.Point(687, 0);
            this.Pnl_LSDataRight.Name = "Pnl_LSDataRight";
            this.Pnl_LSDataRight.Size = new System.Drawing.Size(81, 68);
            this.Pnl_LSDataRight.TabIndex = 193;
            this.Pnl_LSDataRight.Visible = false;
            // 
            // Ckb_LSStop
            // 
            this.Ckb_LSStop.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_LSStop.AutoCheck = false;
            this.Ckb_LSStop.FlatAppearance.BorderSize = 0;
            this.Ckb_LSStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_LSStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_LSStop.Location = new System.Drawing.Point(7, 35);
            this.Ckb_LSStop.Name = "Ckb_LSStop";
            this.Ckb_LSStop.Size = new System.Drawing.Size(60, 25);
            this.Ckb_LSStop.TabIndex = 195;
            this.Ckb_LSStop.Text = "停止";
            this.Ckb_LSStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_LSStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Ckb_LSStop.UseVisualStyleBackColor = true;
            this.Ckb_LSStop.Click += new System.EventHandler(this.Ckb_LSStop_Click);
            // 
            // Lbl_LSRefreshHint
            // 
            this.Lbl_LSRefreshHint.AutoSize = true;
            this.Lbl_LSRefreshHint.Location = new System.Drawing.Point(5, 10);
            this.Lbl_LSRefreshHint.Name = "Lbl_LSRefreshHint";
            this.Lbl_LSRefreshHint.Size = new System.Drawing.Size(65, 17);
            this.Lbl_LSRefreshHint.TabIndex = 163;
            this.Lbl_LSRefreshHint.Text = "正在计算...";
            this.Lbl_LSRefreshHint.Visible = false;
            // 
            // Pnl_LSDataLeft
            // 
            this.Pnl_LSDataLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_LSDataLeft.Controls.Add(this.Btn_LSRefresh);
            this.Pnl_LSDataLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_LSDataLeft.Location = new System.Drawing.Point(570, 0);
            this.Pnl_LSDataLeft.Name = "Pnl_LSDataLeft";
            this.Pnl_LSDataLeft.Size = new System.Drawing.Size(117, 68);
            this.Pnl_LSDataLeft.TabIndex = 192;
            // 
            // Btn_LSRefresh
            // 
            this.Btn_LSRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_LSRefresh.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.Btn_LSRefresh.Location = new System.Drawing.Point(0, 0);
            this.Btn_LSRefresh.Name = "Btn_LSRefresh";
            this.Btn_LSRefresh.Size = new System.Drawing.Size(115, 66);
            this.Btn_LSRefresh.TabIndex = 154;
            this.Btn_LSRefresh.Text = "开始计算";
            this.Btn_LSRefresh.UseVisualStyleBackColor = true;
            this.Btn_LSRefresh.Click += new System.EventHandler(this.Btn_LSRefresh_Click);
            // 
            // Pnl_LSDataTop1
            // 
            this.Pnl_LSDataTop1.BackColor = System.Drawing.Color.Transparent;
            this.Pnl_LSDataTop1.Controls.Add(this.Lbl_LSPlayKey);
            this.Pnl_LSDataTop1.Controls.Add(this.Lbl_LSPlayValue);
            this.Pnl_LSDataTop1.Controls.Add(this.Lbl_LSLotteryKey);
            this.Pnl_LSDataTop1.Controls.Add(this.Lbl_LSLotteryValue);
            this.Pnl_LSDataTop1.Controls.Add(this.Lbl_LSDataRange);
            this.Pnl_LSDataTop1.Controls.Add(this.Dtp_LSDataRange);
            this.Pnl_LSDataTop1.Controls.Add(this.Lbl_LSFN);
            this.Pnl_LSDataTop1.Controls.Add(this.Ckb_LSAutoRefresh);
            this.Pnl_LSDataTop1.Controls.Add(this.Cbb_LSFN);
            this.Pnl_LSDataTop1.Controls.Add(this.Rdb_LSBJExpect);
            this.Pnl_LSDataTop1.Controls.Add(this.Ckb_LSBJ);
            this.Pnl_LSDataTop1.Controls.Add(this.Rdb_LSBJType);
            this.Pnl_LSDataTop1.Controls.Add(this.Cbb_LSBJType);
            this.Pnl_LSDataTop1.Controls.Add(this.Lbl_LSBJExpect2);
            this.Pnl_LSDataTop1.Controls.Add(this.Nm_LSBJExpect);
            this.Pnl_LSDataTop1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_LSDataTop1.Location = new System.Drawing.Point(0, 0);
            this.Pnl_LSDataTop1.Name = "Pnl_LSDataTop1";
            this.Pnl_LSDataTop1.Size = new System.Drawing.Size(570, 68);
            this.Pnl_LSDataTop1.TabIndex = 166;
            // 
            // Lbl_LSPlayKey
            // 
            this.Lbl_LSPlayKey.AutoSize = true;
            this.Lbl_LSPlayKey.Location = new System.Drawing.Point(377, 8);
            this.Lbl_LSPlayKey.Name = "Lbl_LSPlayKey";
            this.Lbl_LSPlayKey.Size = new System.Drawing.Size(44, 17);
            this.Lbl_LSPlayKey.TabIndex = 194;
            this.Lbl_LSPlayKey.Text = "玩法：";
            // 
            // Lbl_LSPlayValue
            // 
            this.Lbl_LSPlayValue.AutoSize = true;
            this.Lbl_LSPlayValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.Lbl_LSPlayValue.Location = new System.Drawing.Point(423, 8);
            this.Lbl_LSPlayValue.Name = "Lbl_LSPlayValue";
            this.Lbl_LSPlayValue.Size = new System.Drawing.Size(56, 17);
            this.Lbl_LSPlayValue.TabIndex = 193;
            this.Lbl_LSPlayValue.Text = "玩法名称";
            this.Lbl_LSPlayValue.Visible = false;
            // 
            // Lbl_LSLotteryKey
            // 
            this.Lbl_LSLotteryKey.AutoSize = true;
            this.Lbl_LSLotteryKey.Location = new System.Drawing.Point(5, 8);
            this.Lbl_LSLotteryKey.Name = "Lbl_LSLotteryKey";
            this.Lbl_LSLotteryKey.Size = new System.Drawing.Size(44, 17);
            this.Lbl_LSLotteryKey.TabIndex = 76;
            this.Lbl_LSLotteryKey.Text = "彩种：";
            // 
            // Lbl_LSLotteryValue
            // 
            this.Lbl_LSLotteryValue.AutoSize = true;
            this.Lbl_LSLotteryValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.Lbl_LSLotteryValue.Location = new System.Drawing.Point(51, 8);
            this.Lbl_LSLotteryValue.Name = "Lbl_LSLotteryValue";
            this.Lbl_LSLotteryValue.Size = new System.Drawing.Size(68, 17);
            this.Lbl_LSLotteryValue.TabIndex = 165;
            this.Lbl_LSLotteryValue.Text = "重庆时时彩";
            // 
            // Lbl_LSDataRange
            // 
            this.Lbl_LSDataRange.AutoSize = true;
            this.Lbl_LSDataRange.Location = new System.Drawing.Point(5, 42);
            this.Lbl_LSDataRange.Name = "Lbl_LSDataRange";
            this.Lbl_LSDataRange.Size = new System.Drawing.Size(44, 17);
            this.Lbl_LSDataRange.TabIndex = 0;
            this.Lbl_LSDataRange.Text = "日期：";
            // 
            // Dtp_LSDataRange
            // 
            this.Dtp_LSDataRange.CustomFormat = "yyyy-MM-dd";
            this.Dtp_LSDataRange.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_LSDataRange.Location = new System.Drawing.Point(52, 39);
            this.Dtp_LSDataRange.Name = "Dtp_LSDataRange";
            this.Dtp_LSDataRange.Size = new System.Drawing.Size(110, 23);
            this.Dtp_LSDataRange.TabIndex = 72;
            // 
            // Lbl_LSFN
            // 
            this.Lbl_LSFN.AutoSize = true;
            this.Lbl_LSFN.Location = new System.Drawing.Point(129, 8);
            this.Lbl_LSFN.Name = "Lbl_LSFN";
            this.Lbl_LSFN.Size = new System.Drawing.Size(44, 17);
            this.Lbl_LSFN.TabIndex = 78;
            this.Lbl_LSFN.Text = "方案：";
            // 
            // Ckb_LSAutoRefresh
            // 
            this.Ckb_LSAutoRefresh.AutoSize = true;
            this.Ckb_LSAutoRefresh.Checked = true;
            this.Ckb_LSAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Ckb_LSAutoRefresh.Location = new System.Drawing.Point(296, 7);
            this.Ckb_LSAutoRefresh.Name = "Ckb_LSAutoRefresh";
            this.Ckb_LSAutoRefresh.Size = new System.Drawing.Size(75, 21);
            this.Ckb_LSAutoRefresh.TabIndex = 162;
            this.Ckb_LSAutoRefresh.Text = "自动刷新";
            this.Ckb_LSAutoRefresh.UseVisualStyleBackColor = true;
            // 
            // Cbb_LSFN
            // 
            this.Cbb_LSFN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbb_LSFN.FormattingEnabled = true;
            this.Cbb_LSFN.Location = new System.Drawing.Point(176, 5);
            this.Cbb_LSFN.Name = "Cbb_LSFN";
            this.Cbb_LSFN.Size = new System.Drawing.Size(110, 25);
            this.Cbb_LSFN.TabIndex = 77;
            this.Cbb_LSFN.SelectedIndexChanged += new System.EventHandler(this.Cbb_LSFN_SelectedIndexChanged);
            // 
            // Rdb_LSBJExpect
            // 
            this.Rdb_LSBJExpect.AutoSize = true;
            this.Rdb_LSBJExpect.Location = new System.Drawing.Point(412, 41);
            this.Rdb_LSBJExpect.Name = "Rdb_LSBJExpect";
            this.Rdb_LSBJExpect.Size = new System.Drawing.Size(50, 21);
            this.Rdb_LSBJExpect.TabIndex = 161;
            this.Rdb_LSBJExpect.TabStop = true;
            this.Rdb_LSBJExpect.Text = "轮次";
            this.Rdb_LSBJExpect.UseVisualStyleBackColor = true;
            // 
            // Ckb_LSBJ
            // 
            this.Ckb_LSBJ.AutoSize = true;
            this.Ckb_LSBJ.Checked = true;
            this.Ckb_LSBJ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Ckb_LSBJ.Location = new System.Drawing.Point(171, 41);
            this.Ckb_LSBJ.Name = "Ckb_LSBJ";
            this.Ckb_LSBJ.Size = new System.Drawing.Size(63, 21);
            this.Ckb_LSBJ.TabIndex = 155;
            this.Ckb_LSBJ.Text = "当接近";
            this.Ckb_LSBJ.UseVisualStyleBackColor = true;
            this.Ckb_LSBJ.CheckedChanged += new System.EventHandler(this.Ckb_LSBJType_CheckedChanged);
            // 
            // Rdb_LSBJType
            // 
            this.Rdb_LSBJType.AutoSize = true;
            this.Rdb_LSBJType.Checked = true;
            this.Rdb_LSBJType.Location = new System.Drawing.Point(240, 41);
            this.Rdb_LSBJType.Name = "Rdb_LSBJType";
            this.Rdb_LSBJType.Size = new System.Drawing.Size(50, 21);
            this.Rdb_LSBJType.TabIndex = 160;
            this.Rdb_LSBJType.TabStop = true;
            this.Rdb_LSBJType.Text = "类型";
            this.Rdb_LSBJType.UseVisualStyleBackColor = true;
            this.Rdb_LSBJType.CheckedChanged += new System.EventHandler(this.Rdb_LSBJType_CheckedChanged);
            // 
            // Cbb_LSBJType
            // 
            this.Cbb_LSBJType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbb_LSBJType.FormattingEnabled = true;
            this.Cbb_LSBJType.Items.AddRange(new object[] {
            "今日未出",
            "昨日未出",
            "一周未出",
            "一月未出",
            "十组连错最多",
            "十组连中最多"});
            this.Cbb_LSBJType.Location = new System.Drawing.Point(296, 38);
            this.Cbb_LSBJType.Name = "Cbb_LSBJType";
            this.Cbb_LSBJType.Size = new System.Drawing.Size(110, 25);
            this.Cbb_LSBJType.TabIndex = 156;
            // 
            // Lbl_LSBJExpect2
            // 
            this.Lbl_LSBJExpect2.AutoSize = true;
            this.Lbl_LSBJExpect2.Location = new System.Drawing.Point(519, 42);
            this.Lbl_LSBJExpect2.Name = "Lbl_LSBJExpect2";
            this.Lbl_LSBJExpect2.Size = new System.Drawing.Size(44, 17);
            this.Lbl_LSBJExpect2.TabIndex = 159;
            this.Lbl_LSBJExpect2.Text = "时提醒";
            // 
            // Nm_LSBJExpect
            // 
            this.Nm_LSBJExpect.Enabled = false;
            this.Nm_LSBJExpect.Location = new System.Drawing.Point(466, 39);
            this.Nm_LSBJExpect.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Nm_LSBJExpect.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Nm_LSBJExpect.Name = "Nm_LSBJExpect";
            this.Nm_LSBJExpect.Size = new System.Drawing.Size(50, 23);
            this.Nm_LSBJExpect.TabIndex = 158;
            this.Nm_LSBJExpect.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Tap_TJData
            // 
            this.Tap_TJData.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_TJData.Controls.Add(this.Pnl_TJData);
            this.Tap_TJData.Location = new System.Drawing.Point(4, 34);
            this.Tap_TJData.Name = "Tap_TJData";
            this.Tap_TJData.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_TJData.Size = new System.Drawing.Size(998, 536);
            this.Tap_TJData.TabIndex = 5;
            this.Tap_TJData.Text = "历史统计";
            // 
            // Pnl_TJData
            // 
            this.Pnl_TJData.Controls.Add(this.Pnl_TJDataMain);
            this.Pnl_TJData.Controls.Add(this.Pnl_TJDataTop);
            this.Pnl_TJData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_TJData.Location = new System.Drawing.Point(3, 3);
            this.Pnl_TJData.Name = "Pnl_TJData";
            this.Pnl_TJData.Size = new System.Drawing.Size(992, 530);
            this.Pnl_TJData.TabIndex = 1;
            // 
            // Pnl_TJDataMain
            // 
            this.Pnl_TJDataMain.Controls.Add(this.Egv_TJDataList2);
            this.Pnl_TJDataMain.Controls.Add(this.Pnl_TJDataTop2);
            this.Pnl_TJDataMain.Controls.Add(this.Egv_TJDataList1);
            this.Pnl_TJDataMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_TJDataMain.Location = new System.Drawing.Point(0, 70);
            this.Pnl_TJDataMain.Name = "Pnl_TJDataMain";
            this.Pnl_TJDataMain.Size = new System.Drawing.Size(992, 460);
            this.Pnl_TJDataMain.TabIndex = 72;
            // 
            // Egv_TJDataList2
            // 
            this.Egv_TJDataList2.AllowUserToAddRows = false;
            this.Egv_TJDataList2.AllowUserToDeleteRows = false;
            this.Egv_TJDataList2.AllowUserToResizeColumns = false;
            this.Egv_TJDataList2.AllowUserToResizeRows = false;
            this.Egv_TJDataList2.BackgroundColor = System.Drawing.SystemColors.Window;
            this.Egv_TJDataList2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Egv_TJDataList2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_TJDataList2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.Egv_TJDataList2.ColumnHeadersHeight = 30;
            this.Egv_TJDataList2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_TJDataList2.DefaultCellStyle = dataGridViewCellStyle11;
            this.Egv_TJDataList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Egv_TJDataList2.DragLineColor = System.Drawing.Color.Silver;
            this.Egv_TJDataList2.ExternalVirtualMode = true;
            this.Egv_TJDataList2.GridColor = System.Drawing.Color.Silver;
            this.Egv_TJDataList2.HeadersCheckDefult = System.Windows.Forms.CheckState.Checked;
            this.Egv_TJDataList2.Location = new System.Drawing.Point(0, 243);
            this.Egv_TJDataList2.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Egv_TJDataList2.MergeColumnHeaderForeColor = System.Drawing.Color.Black;
            this.Egv_TJDataList2.MultiSelect = false;
            this.Egv_TJDataList2.Name = "Egv_TJDataList2";
            this.Egv_TJDataList2.RowHeadersVisible = false;
            this.Egv_TJDataList2.RowNum = 17;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Black;
            this.Egv_TJDataList2.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.Egv_TJDataList2.RowTemplate.Height = 23;
            this.Egv_TJDataList2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Egv_TJDataList2.Size = new System.Drawing.Size(992, 217);
            this.Egv_TJDataList2.TabIndex = 73;
            this.Egv_TJDataList2.VirtualMode = true;
            // 
            // Pnl_TJDataTop2
            // 
            this.Pnl_TJDataTop2.Controls.Add(this.Pnl_TJDataHint);
            this.Pnl_TJDataTop2.Controls.Add(this.Pnl_TJDataFind);
            this.Pnl_TJDataTop2.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_TJDataTop2.Location = new System.Drawing.Point(0, 208);
            this.Pnl_TJDataTop2.Name = "Pnl_TJDataTop2";
            this.Pnl_TJDataTop2.Size = new System.Drawing.Size(992, 35);
            this.Pnl_TJDataTop2.TabIndex = 72;
            // 
            // Pnl_TJDataHint
            // 
            this.Pnl_TJDataHint.BackColor = System.Drawing.Color.Transparent;
            this.Pnl_TJDataHint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_TJDataHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_TJDataHint.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.Pnl_TJDataHint.Location = new System.Drawing.Point(0, 0);
            this.Pnl_TJDataHint.Name = "Pnl_TJDataHint";
            this.Pnl_TJDataHint.Size = new System.Drawing.Size(612, 35);
            this.Pnl_TJDataHint.TabIndex = 71;
            this.Pnl_TJDataHint.Text = "投注明细";
            this.Pnl_TJDataHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Pnl_TJDataFind
            // 
            this.Pnl_TJDataFind.BackColor = System.Drawing.Color.Transparent;
            this.Pnl_TJDataFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_TJDataFind.Controls.Add(this.Ckb_AutoSizeTJ);
            this.Pnl_TJDataFind.Controls.Add(this.Ckb_TJFindXS);
            this.Pnl_TJDataFind.Controls.Add(this.Nm_TJFindXS);
            this.Pnl_TJDataFind.Controls.Add(this.Btn_TJTop);
            this.Pnl_TJDataFind.Controls.Add(this.Lbl_TJFindXS);
            this.Pnl_TJDataFind.Dock = System.Windows.Forms.DockStyle.Right;
            this.Pnl_TJDataFind.Location = new System.Drawing.Point(612, 0);
            this.Pnl_TJDataFind.Name = "Pnl_TJDataFind";
            this.Pnl_TJDataFind.Size = new System.Drawing.Size(380, 35);
            this.Pnl_TJDataFind.TabIndex = 72;
            // 
            // Ckb_AutoSizeTJ
            // 
            this.Ckb_AutoSizeTJ.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_AutoSizeTJ.AutoCheck = false;
            this.Ckb_AutoSizeTJ.FlatAppearance.BorderSize = 0;
            this.Ckb_AutoSizeTJ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_AutoSizeTJ.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_AutoSizeTJ.Location = new System.Drawing.Point(3, 4);
            this.Ckb_AutoSizeTJ.Name = "Ckb_AutoSizeTJ";
            this.Ckb_AutoSizeTJ.Size = new System.Drawing.Size(80, 25);
            this.Ckb_AutoSizeTJ.TabIndex = 215;
            this.Ckb_AutoSizeTJ.Text = "表格调整";
            this.Ckb_AutoSizeTJ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_AutoSizeTJ.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Tot_Hint.SetToolTip(this.Ckb_AutoSizeTJ, "调整统计的表格到合适的大小");
            this.Ckb_AutoSizeTJ.UseVisualStyleBackColor = true;
            this.Ckb_AutoSizeTJ.Click += new System.EventHandler(this.Ckb_AutoSizeTJ_Click);
            // 
            // Ckb_TJFindXS
            // 
            this.Ckb_TJFindXS.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_TJFindXS.AutoCheck = false;
            this.Ckb_TJFindXS.FlatAppearance.BorderSize = 0;
            this.Ckb_TJFindXS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_TJFindXS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_TJFindXS.Location = new System.Drawing.Point(248, 4);
            this.Ckb_TJFindXS.Name = "Ckb_TJFindXS";
            this.Ckb_TJFindXS.Size = new System.Drawing.Size(60, 25);
            this.Ckb_TJFindXS.TabIndex = 197;
            this.Ckb_TJFindXS.Text = "查找";
            this.Ckb_TJFindXS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_TJFindXS.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_Hint.SetToolTip(this.Ckb_TJFindXS, "查找指定的凶手位置");
            this.Ckb_TJFindXS.UseVisualStyleBackColor = true;
            this.Ckb_TJFindXS.Click += new System.EventHandler(this.Ckb_TJFindXS_Click);
            // 
            // Nm_TJFindXS
            // 
            this.Nm_TJFindXS.Location = new System.Drawing.Point(162, 5);
            this.Nm_TJFindXS.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Nm_TJFindXS.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Nm_TJFindXS.Name = "Nm_TJFindXS";
            this.Nm_TJFindXS.Size = new System.Drawing.Size(80, 23);
            this.Nm_TJFindXS.TabIndex = 159;
            this.Nm_TJFindXS.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Nm_TJFindXS.ValueChanged += new System.EventHandler(this.Nm_TJFindXS_ValueChanged);
            // 
            // Btn_TJTop
            // 
            this.Btn_TJTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_TJTop.FlatAppearance.BorderSize = 0;
            this.Btn_TJTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_TJTop.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.Btn_TJTop.Location = new System.Drawing.Point(346, 1);
            this.Btn_TJTop.Name = "Btn_TJTop";
            this.Btn_TJTop.Size = new System.Drawing.Size(30, 30);
            this.Btn_TJTop.TabIndex = 178;
            this.Tot_Hint.SetToolTip(this.Btn_TJTop, "隐藏");
            this.Btn_TJTop.UseVisualStyleBackColor = true;
            this.Btn_TJTop.Click += new System.EventHandler(this.Btn_TJTop_Click);
            // 
            // Lbl_TJFindXS
            // 
            this.Lbl_TJFindXS.AutoSize = true;
            this.Lbl_TJFindXS.Location = new System.Drawing.Point(89, 8);
            this.Lbl_TJFindXS.Name = "Lbl_TJFindXS";
            this.Lbl_TJFindXS.Size = new System.Drawing.Size(68, 17);
            this.Lbl_TJFindXS.TabIndex = 175;
            this.Lbl_TJFindXS.Text = "查找凶手：";
            // 
            // Egv_TJDataList1
            // 
            this.Egv_TJDataList1.AllowUserToAddRows = false;
            this.Egv_TJDataList1.AllowUserToDeleteRows = false;
            this.Egv_TJDataList1.AllowUserToResizeColumns = false;
            this.Egv_TJDataList1.AllowUserToResizeRows = false;
            this.Egv_TJDataList1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.Egv_TJDataList1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Egv_TJDataList1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_TJDataList1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.Egv_TJDataList1.ColumnHeadersHeight = 30;
            this.Egv_TJDataList1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_TJDataList1.DefaultCellStyle = dataGridViewCellStyle14;
            this.Egv_TJDataList1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Egv_TJDataList1.DragLineColor = System.Drawing.Color.Silver;
            this.Egv_TJDataList1.ExternalVirtualMode = true;
            this.Egv_TJDataList1.GridColor = System.Drawing.Color.Silver;
            this.Egv_TJDataList1.HeadersCheckDefult = System.Windows.Forms.CheckState.Checked;
            this.Egv_TJDataList1.Location = new System.Drawing.Point(0, 0);
            this.Egv_TJDataList1.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Egv_TJDataList1.MergeColumnHeaderForeColor = System.Drawing.Color.Black;
            this.Egv_TJDataList1.MultiSelect = false;
            this.Egv_TJDataList1.Name = "Egv_TJDataList1";
            this.Egv_TJDataList1.RowHeadersVisible = false;
            this.Egv_TJDataList1.RowNum = 17;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.Black;
            this.Egv_TJDataList1.RowsDefaultCellStyle = dataGridViewCellStyle15;
            this.Egv_TJDataList1.RowTemplate.Height = 23;
            this.Egv_TJDataList1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Egv_TJDataList1.Size = new System.Drawing.Size(992, 208);
            this.Egv_TJDataList1.TabIndex = 70;
            this.Egv_TJDataList1.VirtualMode = true;
            // 
            // Pnl_TJDataTop
            // 
            this.Pnl_TJDataTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_TJDataTop.Controls.Add(this.Pnl_TJRight2);
            this.Pnl_TJDataTop.Controls.Add(this.Pnl_TJRight1);
            this.Pnl_TJDataTop.Controls.Add(this.Pnl_TJDataTop1);
            this.Pnl_TJDataTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_TJDataTop.Location = new System.Drawing.Point(0, 0);
            this.Pnl_TJDataTop.Name = "Pnl_TJDataTop";
            this.Pnl_TJDataTop.Size = new System.Drawing.Size(992, 70);
            this.Pnl_TJDataTop.TabIndex = 71;
            // 
            // Pnl_TJRight2
            // 
            this.Pnl_TJRight2.BackColor = System.Drawing.Color.Transparent;
            this.Pnl_TJRight2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_TJRight2.Controls.Add(this.Ckb_TJStop);
            this.Pnl_TJRight2.Controls.Add(this.Lbl_TJRefreshHint);
            this.Pnl_TJRight2.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_TJRight2.Location = new System.Drawing.Point(787, 0);
            this.Pnl_TJRight2.Name = "Pnl_TJRight2";
            this.Pnl_TJRight2.Size = new System.Drawing.Size(81, 68);
            this.Pnl_TJRight2.TabIndex = 191;
            this.Pnl_TJRight2.Visible = false;
            // 
            // Ckb_TJStop
            // 
            this.Ckb_TJStop.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_TJStop.AutoCheck = false;
            this.Ckb_TJStop.FlatAppearance.BorderSize = 0;
            this.Ckb_TJStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_TJStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_TJStop.Location = new System.Drawing.Point(7, 35);
            this.Ckb_TJStop.Name = "Ckb_TJStop";
            this.Ckb_TJStop.Size = new System.Drawing.Size(60, 25);
            this.Ckb_TJStop.TabIndex = 196;
            this.Ckb_TJStop.Text = "停止";
            this.Ckb_TJStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_TJStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Ckb_TJStop.UseVisualStyleBackColor = true;
            this.Ckb_TJStop.Click += new System.EventHandler(this.Ckb_TJStop_Click);
            // 
            // Lbl_TJRefreshHint
            // 
            this.Lbl_TJRefreshHint.AutoSize = true;
            this.Lbl_TJRefreshHint.Location = new System.Drawing.Point(5, 10);
            this.Lbl_TJRefreshHint.Name = "Lbl_TJRefreshHint";
            this.Lbl_TJRefreshHint.Size = new System.Drawing.Size(65, 17);
            this.Lbl_TJRefreshHint.TabIndex = 165;
            this.Lbl_TJRefreshHint.Text = "正在统计...";
            // 
            // Pnl_TJRight1
            // 
            this.Pnl_TJRight1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_TJRight1.Controls.Add(this.Btn_TJRefresh);
            this.Pnl_TJRight1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_TJRight1.Location = new System.Drawing.Point(670, 0);
            this.Pnl_TJRight1.Name = "Pnl_TJRight1";
            this.Pnl_TJRight1.Size = new System.Drawing.Size(117, 68);
            this.Pnl_TJRight1.TabIndex = 190;
            // 
            // Btn_TJRefresh
            // 
            this.Btn_TJRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_TJRefresh.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.Btn_TJRefresh.Location = new System.Drawing.Point(0, 0);
            this.Btn_TJRefresh.Name = "Btn_TJRefresh";
            this.Btn_TJRefresh.Size = new System.Drawing.Size(115, 66);
            this.Btn_TJRefresh.TabIndex = 175;
            this.Btn_TJRefresh.Text = "开始统计";
            this.Btn_TJRefresh.UseVisualStyleBackColor = true;
            this.Btn_TJRefresh.Click += new System.EventHandler(this.Btn_TJRefresh_Click);
            // 
            // Pnl_TJDataTop1
            // 
            this.Pnl_TJDataTop1.BackColor = System.Drawing.Color.Transparent;
            this.Pnl_TJDataTop1.Controls.Add(this.Lbl_TJPlayKey);
            this.Pnl_TJDataTop1.Controls.Add(this.Lbl_TJLotteryValue);
            this.Pnl_TJDataTop1.Controls.Add(this.Lbl_TJPlayValue);
            this.Pnl_TJDataTop1.Controls.Add(this.Cbb_TJPrize);
            this.Pnl_TJDataTop1.Controls.Add(this.Lbl_TJLotteryKey);
            this.Pnl_TJDataTop1.Controls.Add(this.Dtp_TJTimeRange2);
            this.Pnl_TJDataTop1.Controls.Add(this.Lbl_TJDataRange);
            this.Pnl_TJDataTop1.Controls.Add(this.Lbl_TJTime);
            this.Pnl_TJDataTop1.Controls.Add(this.Dtp_TJDataRange1);
            this.Pnl_TJDataTop1.Controls.Add(this.Dtp_TJTimeRange1);
            this.Pnl_TJDataTop1.Controls.Add(this.Lbl_TJData);
            this.Pnl_TJDataTop1.Controls.Add(this.Ckb_TJTimeRange);
            this.Pnl_TJDataTop1.Controls.Add(this.Txt_TJPrize);
            this.Pnl_TJDataTop1.Controls.Add(this.Ckb_TJReset);
            this.Pnl_TJDataTop1.Controls.Add(this.Lbl_TJPrize);
            this.Pnl_TJDataTop1.Controls.Add(this.Dtp_TJDataRange2);
            this.Pnl_TJDataTop1.Controls.Add(this.Lbl_TJFN);
            this.Pnl_TJDataTop1.Controls.Add(this.Cbb_TJFN);
            this.Pnl_TJDataTop1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_TJDataTop1.Location = new System.Drawing.Point(0, 0);
            this.Pnl_TJDataTop1.Name = "Pnl_TJDataTop1";
            this.Pnl_TJDataTop1.Size = new System.Drawing.Size(670, 68);
            this.Pnl_TJDataTop1.TabIndex = 189;
            // 
            // Lbl_TJPlayKey
            // 
            this.Lbl_TJPlayKey.AutoSize = true;
            this.Lbl_TJPlayKey.Location = new System.Drawing.Point(509, 8);
            this.Lbl_TJPlayKey.Name = "Lbl_TJPlayKey";
            this.Lbl_TJPlayKey.Size = new System.Drawing.Size(44, 17);
            this.Lbl_TJPlayKey.TabIndex = 192;
            this.Lbl_TJPlayKey.Text = "玩法：";
            // 
            // Lbl_TJLotteryValue
            // 
            this.Lbl_TJLotteryValue.AutoSize = true;
            this.Lbl_TJLotteryValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.Lbl_TJLotteryValue.Location = new System.Drawing.Point(51, 8);
            this.Lbl_TJLotteryValue.Name = "Lbl_TJLotteryValue";
            this.Lbl_TJLotteryValue.Size = new System.Drawing.Size(68, 17);
            this.Lbl_TJLotteryValue.TabIndex = 191;
            this.Lbl_TJLotteryValue.Text = "重庆时时彩";
            // 
            // Lbl_TJPlayValue
            // 
            this.Lbl_TJPlayValue.AutoSize = true;
            this.Lbl_TJPlayValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.Lbl_TJPlayValue.Location = new System.Drawing.Point(555, 8);
            this.Lbl_TJPlayValue.Name = "Lbl_TJPlayValue";
            this.Lbl_TJPlayValue.Size = new System.Drawing.Size(56, 17);
            this.Lbl_TJPlayValue.TabIndex = 190;
            this.Lbl_TJPlayValue.Text = "玩法名称";
            this.Lbl_TJPlayValue.Visible = false;
            // 
            // Cbb_TJPrize
            // 
            this.Cbb_TJPrize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbb_TJPrize.FormattingEnabled = true;
            this.Cbb_TJPrize.Items.AddRange(new object[] {
            "读取盘口",
            "手动输入"});
            this.Cbb_TJPrize.Location = new System.Drawing.Point(339, 5);
            this.Cbb_TJPrize.Name = "Cbb_TJPrize";
            this.Cbb_TJPrize.Size = new System.Drawing.Size(80, 25);
            this.Cbb_TJPrize.TabIndex = 189;
            this.Cbb_TJPrize.SelectedIndexChanged += new System.EventHandler(this.Cbb_TJPrize_SelectedIndexChanged);
            // 
            // Lbl_TJLotteryKey
            // 
            this.Lbl_TJLotteryKey.AutoSize = true;
            this.Lbl_TJLotteryKey.Location = new System.Drawing.Point(5, 8);
            this.Lbl_TJLotteryKey.Name = "Lbl_TJLotteryKey";
            this.Lbl_TJLotteryKey.Size = new System.Drawing.Size(44, 17);
            this.Lbl_TJLotteryKey.TabIndex = 171;
            this.Lbl_TJLotteryKey.Text = "彩种：";
            // 
            // Dtp_TJTimeRange2
            // 
            this.Dtp_TJTimeRange2.CustomFormat = "HH:mm";
            this.Dtp_TJTimeRange2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_TJTimeRange2.Location = new System.Drawing.Point(483, 39);
            this.Dtp_TJTimeRange2.Name = "Dtp_TJTimeRange2";
            this.Dtp_TJTimeRange2.ShowUpDown = true;
            this.Dtp_TJTimeRange2.Size = new System.Drawing.Size(60, 23);
            this.Dtp_TJTimeRange2.TabIndex = 188;
            this.Dtp_TJTimeRange2.Value = new System.DateTime(2015, 7, 20, 22, 32, 0, 0);
            // 
            // Lbl_TJDataRange
            // 
            this.Lbl_TJDataRange.AutoSize = true;
            this.Lbl_TJDataRange.Location = new System.Drawing.Point(5, 42);
            this.Lbl_TJDataRange.Name = "Lbl_TJDataRange";
            this.Lbl_TJDataRange.Size = new System.Drawing.Size(44, 17);
            this.Lbl_TJDataRange.TabIndex = 0;
            this.Lbl_TJDataRange.Text = "日期：";
            // 
            // Lbl_TJTime
            // 
            this.Lbl_TJTime.AutoSize = true;
            this.Lbl_TJTime.Location = new System.Drawing.Point(457, 42);
            this.Lbl_TJTime.Name = "Lbl_TJTime";
            this.Lbl_TJTime.Size = new System.Drawing.Size(20, 17);
            this.Lbl_TJTime.TabIndex = 187;
            this.Lbl_TJTime.Text = "至";
            // 
            // Dtp_TJDataRange1
            // 
            this.Dtp_TJDataRange1.CustomFormat = "yyyy-MM-dd";
            this.Dtp_TJDataRange1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_TJDataRange1.Location = new System.Drawing.Point(52, 39);
            this.Dtp_TJDataRange1.Name = "Dtp_TJDataRange1";
            this.Dtp_TJDataRange1.Size = new System.Drawing.Size(110, 23);
            this.Dtp_TJDataRange1.TabIndex = 72;
            // 
            // Dtp_TJTimeRange1
            // 
            this.Dtp_TJTimeRange1.CustomFormat = "HH:mm";
            this.Dtp_TJTimeRange1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_TJTimeRange1.Location = new System.Drawing.Point(392, 39);
            this.Dtp_TJTimeRange1.Name = "Dtp_TJTimeRange1";
            this.Dtp_TJTimeRange1.ShowUpDown = true;
            this.Dtp_TJTimeRange1.Size = new System.Drawing.Size(60, 23);
            this.Dtp_TJTimeRange1.TabIndex = 186;
            this.Dtp_TJTimeRange1.Value = new System.DateTime(2015, 7, 20, 9, 1, 0, 0);
            // 
            // Lbl_TJData
            // 
            this.Lbl_TJData.AutoSize = true;
            this.Lbl_TJData.Location = new System.Drawing.Point(168, 42);
            this.Lbl_TJData.Name = "Lbl_TJData";
            this.Lbl_TJData.Size = new System.Drawing.Size(20, 17);
            this.Lbl_TJData.TabIndex = 169;
            this.Lbl_TJData.Text = "至";
            // 
            // Ckb_TJTimeRange
            // 
            this.Ckb_TJTimeRange.AutoSize = true;
            this.Ckb_TJTimeRange.Location = new System.Drawing.Point(316, 41);
            this.Ckb_TJTimeRange.Name = "Ckb_TJTimeRange";
            this.Ckb_TJTimeRange.Size = new System.Drawing.Size(75, 21);
            this.Ckb_TJTimeRange.TabIndex = 185;
            this.Ckb_TJTimeRange.Text = "选择时间";
            this.Ckb_TJTimeRange.UseVisualStyleBackColor = true;
            // 
            // Txt_TJPrize
            // 
            this.Txt_TJPrize.Location = new System.Drawing.Point(424, 6);
            this.Txt_TJPrize.Name = "Txt_TJPrize";
            this.Txt_TJPrize.ReadOnly = true;
            this.Txt_TJPrize.Size = new System.Drawing.Size(80, 23);
            this.Txt_TJPrize.TabIndex = 174;
            // 
            // Ckb_TJReset
            // 
            this.Ckb_TJReset.AutoSize = true;
            this.Ckb_TJReset.Checked = true;
            this.Ckb_TJReset.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Ckb_TJReset.Location = new System.Drawing.Point(553, 41);
            this.Ckb_TJReset.Name = "Ckb_TJReset";
            this.Ckb_TJReset.Size = new System.Drawing.Size(99, 21);
            this.Ckb_TJReset.TabIndex = 184;
            this.Ckb_TJReset.Text = "每天重置数据";
            this.Ckb_TJReset.UseVisualStyleBackColor = true;
            // 
            // Lbl_TJPrize
            // 
            this.Lbl_TJPrize.AutoSize = true;
            this.Lbl_TJPrize.Location = new System.Drawing.Point(291, 8);
            this.Lbl_TJPrize.Name = "Lbl_TJPrize";
            this.Lbl_TJPrize.Size = new System.Drawing.Size(44, 17);
            this.Lbl_TJPrize.TabIndex = 173;
            this.Lbl_TJPrize.Text = "奖金：";
            // 
            // Dtp_TJDataRange2
            // 
            this.Dtp_TJDataRange2.CustomFormat = "yyyy-MM-dd";
            this.Dtp_TJDataRange2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_TJDataRange2.Location = new System.Drawing.Point(195, 39);
            this.Dtp_TJDataRange2.Name = "Dtp_TJDataRange2";
            this.Dtp_TJDataRange2.Size = new System.Drawing.Size(110, 23);
            this.Dtp_TJDataRange2.TabIndex = 178;
            // 
            // Lbl_TJFN
            // 
            this.Lbl_TJFN.AutoSize = true;
            this.Lbl_TJFN.Location = new System.Drawing.Point(129, 8);
            this.Lbl_TJFN.Name = "Lbl_TJFN";
            this.Lbl_TJFN.Size = new System.Drawing.Size(44, 17);
            this.Lbl_TJFN.TabIndex = 176;
            this.Lbl_TJFN.Text = "方案：";
            // 
            // Cbb_TJFN
            // 
            this.Cbb_TJFN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbb_TJFN.FormattingEnabled = true;
            this.Cbb_TJFN.Location = new System.Drawing.Point(176, 5);
            this.Cbb_TJFN.Name = "Cbb_TJFN";
            this.Cbb_TJFN.Size = new System.Drawing.Size(110, 25);
            this.Cbb_TJFN.TabIndex = 177;
            this.Cbb_TJFN.SelectedIndexChanged += new System.EventHandler(this.Cbb_TJFN_SelectedIndexChanged);
            // 
            // Tap_ZBJ
            // 
            this.Tap_ZBJ.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_ZBJ.Controls.Add(this.Zbj_Main);
            this.Tap_ZBJ.Location = new System.Drawing.Point(4, 34);
            this.Tap_ZBJ.Name = "Tap_ZBJ";
            this.Tap_ZBJ.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_ZBJ.Size = new System.Drawing.Size(998, 536);
            this.Tap_ZBJ.TabIndex = 15;
            this.Tap_ZBJ.Text = "直播间";
            // 
            // Zbj_Main
            // 
            this.Zbj_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Zbj_Main.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Zbj_Main.Location = new System.Drawing.Point(3, 3);
            this.Zbj_Main.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Zbj_Main.Name = "Zbj_Main";
            this.Zbj_Main.Size = new System.Drawing.Size(992, 530);
            this.Zbj_Main.TabIndex = 0;
            // 
            // Tap_TrendView
            // 
            this.Tap_TrendView.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_TrendView.Controls.Add(this.TV_Main);
            this.Tap_TrendView.Location = new System.Drawing.Point(4, 34);
            this.Tap_TrendView.Name = "Tap_TrendView";
            this.Tap_TrendView.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_TrendView.Size = new System.Drawing.Size(998, 536);
            this.Tap_TrendView.TabIndex = 18;
            this.Tap_TrendView.Text = "走势分析";
            // 
            // TV_Main
            // 
            this.TV_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TV_Main.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.TV_Main.Location = new System.Drawing.Point(3, 3);
            this.TV_Main.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TV_Main.Name = "TV_Main";
            this.TV_Main.Size = new System.Drawing.Size(992, 530);
            this.TV_Main.TabIndex = 0;
            // 
            // Tap_BTCount
            // 
            this.Tap_BTCount.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_BTCount.Controls.Add(this.BT_Main);
            this.Tap_BTCount.Location = new System.Drawing.Point(4, 34);
            this.Tap_BTCount.Name = "Tap_BTCount";
            this.Tap_BTCount.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_BTCount.Size = new System.Drawing.Size(998, 536);
            this.Tap_BTCount.TabIndex = 4;
            this.Tap_BTCount.Text = "倍投计算";
            // 
            // BT_Main
            // 
            this.BT_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BT_Main.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.BT_Main.Location = new System.Drawing.Point(3, 3);
            this.BT_Main.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BT_Main.Name = "BT_Main";
            this.BT_Main.Size = new System.Drawing.Size(992, 530);
            this.BT_Main.TabIndex = 0;
            // 
            // Tap_BTFN
            // 
            this.Tap_BTFN.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_BTFN.Controls.Add(this.Pnl_BTFN);
            this.Tap_BTFN.Location = new System.Drawing.Point(4, 34);
            this.Tap_BTFN.Name = "Tap_BTFN";
            this.Tap_BTFN.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_BTFN.Size = new System.Drawing.Size(998, 536);
            this.Tap_BTFN.TabIndex = 6;
            this.Tap_BTFN.Text = "高级倍投";
            // 
            // Pnl_BTFN
            // 
            this.Pnl_BTFN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pnl_BTFN.Controls.Add(this.Pnl_BTFNMain);
            this.Pnl_BTFN.Controls.Add(this.Pnl_BTFNList);
            this.Pnl_BTFN.Controls.Add(this.Lbl_GJFNEncrypt);
            this.Pnl_BTFN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_BTFN.Location = new System.Drawing.Point(3, 3);
            this.Pnl_BTFN.Name = "Pnl_BTFN";
            this.Pnl_BTFN.Size = new System.Drawing.Size(992, 530);
            this.Pnl_BTFN.TabIndex = 15;
            // 
            // Pnl_BTFNMain
            // 
            this.Pnl_BTFNMain.BackColor = System.Drawing.Color.Transparent;
            this.Pnl_BTFNMain.Controls.Add(this.Egv_BTFNTimesList);
            this.Pnl_BTFNMain.Controls.Add(this.Pnl_TimesBottom);
            this.Pnl_BTFNMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_BTFNMain.Location = new System.Drawing.Point(188, 0);
            this.Pnl_BTFNMain.Name = "Pnl_BTFNMain";
            this.Pnl_BTFNMain.Size = new System.Drawing.Size(804, 530);
            this.Pnl_BTFNMain.TabIndex = 13;
            // 
            // Egv_BTFNTimesList
            // 
            this.Egv_BTFNTimesList.AllowUserToAddRows = false;
            this.Egv_BTFNTimesList.AllowUserToDeleteRows = false;
            this.Egv_BTFNTimesList.AllowUserToResizeColumns = false;
            this.Egv_BTFNTimesList.AllowUserToResizeRows = false;
            this.Egv_BTFNTimesList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Egv_BTFNTimesList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Egv_BTFNTimesList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_BTFNTimesList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.Egv_BTFNTimesList.ColumnHeadersHeight = 30;
            this.Egv_BTFNTimesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_BTFNTimesList.DefaultCellStyle = dataGridViewCellStyle17;
            this.Egv_BTFNTimesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Egv_BTFNTimesList.DragLineColor = System.Drawing.Color.Silver;
            this.Egv_BTFNTimesList.ExternalVirtualMode = true;
            this.Egv_BTFNTimesList.GridColor = System.Drawing.Color.Silver;
            this.Egv_BTFNTimesList.HeadersCheckDefult = System.Windows.Forms.CheckState.Checked;
            this.Egv_BTFNTimesList.Location = new System.Drawing.Point(0, 0);
            this.Egv_BTFNTimesList.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Egv_BTFNTimesList.MergeColumnHeaderForeColor = System.Drawing.Color.Black;
            this.Egv_BTFNTimesList.MultiSelect = false;
            this.Egv_BTFNTimesList.Name = "Egv_BTFNTimesList";
            this.Egv_BTFNTimesList.RowHeadersVisible = false;
            this.Egv_BTFNTimesList.RowNum = 17;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black;
            this.Egv_BTFNTimesList.RowsDefaultCellStyle = dataGridViewCellStyle18;
            this.Egv_BTFNTimesList.RowTemplate.Height = 23;
            this.Egv_BTFNTimesList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Egv_BTFNTimesList.Size = new System.Drawing.Size(804, 495);
            this.Egv_BTFNTimesList.TabIndex = 68;
            this.Egv_BTFNTimesList.VirtualMode = true;
            this.Egv_BTFNTimesList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Egv_BTFNTimesList_CellMouseDoubleClick);
            // 
            // Pnl_TimesBottom
            // 
            this.Pnl_TimesBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_TimesBottom.Controls.Add(this.Ckb_TBCount);
            this.Pnl_TimesBottom.Controls.Add(this.Ckb_ClearTimes);
            this.Pnl_TimesBottom.Controls.Add(this.Ckb_EditTimes);
            this.Pnl_TimesBottom.Controls.Add(this.Ckb_DeleteTimes);
            this.Pnl_TimesBottom.Controls.Add(this.Ckb_AddTimes);
            this.Pnl_TimesBottom.Controls.Add(this.Ckb_SaveTimes);
            this.Pnl_TimesBottom.Controls.Add(this.Ckb_BTFNEdit);
            this.Pnl_TimesBottom.Controls.Add(this.Ckb_BTFNEditSkip);
            this.Pnl_TimesBottom.Controls.Add(this.Nm_BTFNEdit);
            this.Pnl_TimesBottom.Controls.Add(this.Cbb_BTFNEdit);
            this.Pnl_TimesBottom.Controls.Add(this.Lbl_BTFNEdit);
            this.Pnl_TimesBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Pnl_TimesBottom.Location = new System.Drawing.Point(0, 495);
            this.Pnl_TimesBottom.Name = "Pnl_TimesBottom";
            this.Pnl_TimesBottom.Size = new System.Drawing.Size(804, 35);
            this.Pnl_TimesBottom.TabIndex = 75;
            // 
            // Ckb_TBCount
            // 
            this.Ckb_TBCount.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_TBCount.AutoCheck = false;
            this.Ckb_TBCount.FlatAppearance.BorderSize = 0;
            this.Ckb_TBCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_TBCount.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_TBCount.Location = new System.Drawing.Point(455, 4);
            this.Ckb_TBCount.Name = "Ckb_TBCount";
            this.Ckb_TBCount.Size = new System.Drawing.Size(80, 25);
            this.Ckb_TBCount.TabIndex = 309;
            this.Ckb_TBCount.Text = "计算推波";
            this.Ckb_TBCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_TBCount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Ckb_TBCount.UseVisualStyleBackColor = true;
            this.Ckb_TBCount.Click += new System.EventHandler(this.Ckb_TBCount_Click);
            // 
            // Ckb_ClearTimes
            // 
            this.Ckb_ClearTimes.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_ClearTimes.AutoCheck = false;
            this.Ckb_ClearTimes.FlatAppearance.BorderSize = 0;
            this.Ckb_ClearTimes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_ClearTimes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_ClearTimes.Location = new System.Drawing.Point(736, 4);
            this.Ckb_ClearTimes.Name = "Ckb_ClearTimes";
            this.Ckb_ClearTimes.Size = new System.Drawing.Size(60, 25);
            this.Ckb_ClearTimes.TabIndex = 308;
            this.Ckb_ClearTimes.Text = "清空";
            this.Ckb_ClearTimes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_ClearTimes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_Hint.SetToolTip(this.Ckb_ClearTimes, "清空当前方案的全部局数");
            this.Ckb_ClearTimes.UseVisualStyleBackColor = true;
            this.Ckb_ClearTimes.Click += new System.EventHandler(this.Ckb_ClearTimes_Click);
            // 
            // Ckb_EditTimes
            // 
            this.Ckb_EditTimes.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_EditTimes.AutoCheck = false;
            this.Ckb_EditTimes.FlatAppearance.BorderSize = 0;
            this.Ckb_EditTimes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_EditTimes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_EditTimes.Location = new System.Drawing.Point(604, 4);
            this.Ckb_EditTimes.Name = "Ckb_EditTimes";
            this.Ckb_EditTimes.Size = new System.Drawing.Size(60, 25);
            this.Ckb_EditTimes.TabIndex = 308;
            this.Ckb_EditTimes.Text = "编辑";
            this.Ckb_EditTimes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_EditTimes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_Hint.SetToolTip(this.Ckb_EditTimes, "编辑当前选中的一个局数");
            this.Ckb_EditTimes.UseVisualStyleBackColor = true;
            this.Ckb_EditTimes.Click += new System.EventHandler(this.Ckb_EditTimes_Click);
            // 
            // Ckb_DeleteTimes
            // 
            this.Ckb_DeleteTimes.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_DeleteTimes.AutoCheck = false;
            this.Ckb_DeleteTimes.FlatAppearance.BorderSize = 0;
            this.Ckb_DeleteTimes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_DeleteTimes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_DeleteTimes.Location = new System.Drawing.Point(670, 4);
            this.Ckb_DeleteTimes.Name = "Ckb_DeleteTimes";
            this.Ckb_DeleteTimes.Size = new System.Drawing.Size(60, 25);
            this.Ckb_DeleteTimes.TabIndex = 307;
            this.Ckb_DeleteTimes.Text = "删除";
            this.Ckb_DeleteTimes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_DeleteTimes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_Hint.SetToolTip(this.Ckb_DeleteTimes, "删除当前选中的一个局数");
            this.Ckb_DeleteTimes.UseVisualStyleBackColor = true;
            this.Ckb_DeleteTimes.Click += new System.EventHandler(this.Ckb_DeleteTimes_Click);
            // 
            // Ckb_AddTimes
            // 
            this.Ckb_AddTimes.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_AddTimes.AutoCheck = false;
            this.Ckb_AddTimes.FlatAppearance.BorderSize = 0;
            this.Ckb_AddTimes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_AddTimes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_AddTimes.Location = new System.Drawing.Point(541, 4);
            this.Ckb_AddTimes.Name = "Ckb_AddTimes";
            this.Ckb_AddTimes.Size = new System.Drawing.Size(60, 25);
            this.Ckb_AddTimes.TabIndex = 306;
            this.Ckb_AddTimes.Text = "添加";
            this.Ckb_AddTimes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_AddTimes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_Hint.SetToolTip(this.Ckb_AddTimes, "添加一个局数");
            this.Ckb_AddTimes.UseVisualStyleBackColor = true;
            this.Ckb_AddTimes.Click += new System.EventHandler(this.Ckb_AddTimes_Click);
            // 
            // Ckb_SaveTimes
            // 
            this.Ckb_SaveTimes.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_SaveTimes.AutoCheck = false;
            this.Ckb_SaveTimes.FlatAppearance.BorderSize = 0;
            this.Ckb_SaveTimes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_SaveTimes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_SaveTimes.Location = new System.Drawing.Point(341, 4);
            this.Ckb_SaveTimes.Name = "Ckb_SaveTimes";
            this.Ckb_SaveTimes.Size = new System.Drawing.Size(60, 25);
            this.Ckb_SaveTimes.TabIndex = 305;
            this.Ckb_SaveTimes.Text = "保存";
            this.Ckb_SaveTimes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_SaveTimes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_Hint.SetToolTip(this.Ckb_SaveTimes, "保存当前高级倍投方案");
            this.Ckb_SaveTimes.UseVisualStyleBackColor = true;
            this.Ckb_SaveTimes.Click += new System.EventHandler(this.Ckb_SaveTimes_Click);
            // 
            // Ckb_BTFNEdit
            // 
            this.Ckb_BTFNEdit.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_BTFNEdit.AutoCheck = false;
            this.Ckb_BTFNEdit.FlatAppearance.BorderSize = 0;
            this.Ckb_BTFNEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_BTFNEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_BTFNEdit.Location = new System.Drawing.Point(189, 4);
            this.Ckb_BTFNEdit.Name = "Ckb_BTFNEdit";
            this.Ckb_BTFNEdit.Size = new System.Drawing.Size(60, 25);
            this.Ckb_BTFNEdit.TabIndex = 304;
            this.Ckb_BTFNEdit.Text = "修改";
            this.Ckb_BTFNEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_BTFNEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_Hint.SetToolTip(this.Ckb_BTFNEdit, "批量修改全部局数的倍数");
            this.Ckb_BTFNEdit.UseVisualStyleBackColor = true;
            this.Ckb_BTFNEdit.Click += new System.EventHandler(this.Ckb_BTFNEdit_Click);
            // 
            // Ckb_BTFNEditSkip
            // 
            this.Ckb_BTFNEditSkip.AutoSize = true;
            this.Ckb_BTFNEditSkip.Location = new System.Drawing.Point(255, 7);
            this.Ckb_BTFNEditSkip.Name = "Ckb_BTFNEditSkip";
            this.Ckb_BTFNEditSkip.Size = new System.Drawing.Size(82, 21);
            this.Ckb_BTFNEditSkip.TabIndex = 199;
            this.Ckb_BTFNEditSkip.Text = "跳过倍数0";
            this.Ckb_BTFNEditSkip.UseVisualStyleBackColor = true;
            // 
            // Nm_BTFNEdit
            // 
            this.Nm_BTFNEdit.Location = new System.Drawing.Point(123, 6);
            this.Nm_BTFNEdit.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Nm_BTFNEdit.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Nm_BTFNEdit.Name = "Nm_BTFNEdit";
            this.Nm_BTFNEdit.Size = new System.Drawing.Size(60, 23);
            this.Nm_BTFNEdit.TabIndex = 197;
            this.Nm_BTFNEdit.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Cbb_BTFNEdit
            // 
            this.Cbb_BTFNEdit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbb_BTFNEdit.FormattingEnabled = true;
            this.Cbb_BTFNEdit.Items.AddRange(new object[] {
            "+",
            "-",
            "*",
            "/"});
            this.Cbb_BTFNEdit.Location = new System.Drawing.Point(67, 4);
            this.Cbb_BTFNEdit.Name = "Cbb_BTFNEdit";
            this.Cbb_BTFNEdit.Size = new System.Drawing.Size(50, 25);
            this.Cbb_BTFNEdit.TabIndex = 185;
            // 
            // Lbl_BTFNEdit
            // 
            this.Lbl_BTFNEdit.AutoSize = true;
            this.Lbl_BTFNEdit.Location = new System.Drawing.Point(5, 8);
            this.Lbl_BTFNEdit.Name = "Lbl_BTFNEdit";
            this.Lbl_BTFNEdit.Size = new System.Drawing.Size(56, 17);
            this.Lbl_BTFNEdit.TabIndex = 154;
            this.Lbl_BTFNEdit.Text = "倍数批量";
            // 
            // Pnl_BTFNList
            // 
            this.Pnl_BTFNList.BackColor = System.Drawing.Color.Transparent;
            this.Pnl_BTFNList.Controls.Add(this.Egv_BTFNMain);
            this.Pnl_BTFNList.Controls.Add(this.Pnl_FNBottom);
            this.Pnl_BTFNList.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_BTFNList.Location = new System.Drawing.Point(0, 0);
            this.Pnl_BTFNList.Name = "Pnl_BTFNList";
            this.Pnl_BTFNList.Size = new System.Drawing.Size(188, 530);
            this.Pnl_BTFNList.TabIndex = 12;
            // 
            // Egv_BTFNMain
            // 
            this.Egv_BTFNMain.AllowUserToAddRows = false;
            this.Egv_BTFNMain.AllowUserToDeleteRows = false;
            this.Egv_BTFNMain.AllowUserToResizeColumns = false;
            this.Egv_BTFNMain.AllowUserToResizeRows = false;
            this.Egv_BTFNMain.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Egv_BTFNMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Egv_BTFNMain.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_BTFNMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.Egv_BTFNMain.ColumnHeadersHeight = 30;
            this.Egv_BTFNMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_BTFNMain.DefaultCellStyle = dataGridViewCellStyle20;
            this.Egv_BTFNMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Egv_BTFNMain.DragLineColor = System.Drawing.Color.Silver;
            this.Egv_BTFNMain.ExternalVirtualMode = true;
            this.Egv_BTFNMain.GridColor = System.Drawing.Color.Silver;
            this.Egv_BTFNMain.HeadersCheckDefult = System.Windows.Forms.CheckState.Checked;
            this.Egv_BTFNMain.Location = new System.Drawing.Point(0, 0);
            this.Egv_BTFNMain.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Egv_BTFNMain.MergeColumnHeaderForeColor = System.Drawing.Color.Black;
            this.Egv_BTFNMain.MultiSelect = false;
            this.Egv_BTFNMain.Name = "Egv_BTFNMain";
            this.Egv_BTFNMain.RowHeadersVisible = false;
            this.Egv_BTFNMain.RowNum = 17;
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.Color.Black;
            this.Egv_BTFNMain.RowsDefaultCellStyle = dataGridViewCellStyle21;
            this.Egv_BTFNMain.RowTemplate.Height = 23;
            this.Egv_BTFNMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Egv_BTFNMain.Size = new System.Drawing.Size(188, 495);
            this.Egv_BTFNMain.TabIndex = 73;
            this.Egv_BTFNMain.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Egv_BTFNList_CellMouseClick);
            // 
            // Pnl_FNBottom
            // 
            this.Pnl_FNBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_FNBottom.Controls.Add(this.Ckb_DeleteBTFN);
            this.Pnl_FNBottom.Controls.Add(this.Ckb_AddBTFN);
            this.Pnl_FNBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Pnl_FNBottom.Location = new System.Drawing.Point(0, 495);
            this.Pnl_FNBottom.Name = "Pnl_FNBottom";
            this.Pnl_FNBottom.Size = new System.Drawing.Size(188, 35);
            this.Pnl_FNBottom.TabIndex = 74;
            // 
            // Ckb_DeleteBTFN
            // 
            this.Ckb_DeleteBTFN.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_DeleteBTFN.AutoCheck = false;
            this.Ckb_DeleteBTFN.FlatAppearance.BorderSize = 0;
            this.Ckb_DeleteBTFN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_DeleteBTFN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_DeleteBTFN.Location = new System.Drawing.Point(70, 3);
            this.Ckb_DeleteBTFN.Name = "Ckb_DeleteBTFN";
            this.Ckb_DeleteBTFN.Size = new System.Drawing.Size(60, 25);
            this.Ckb_DeleteBTFN.TabIndex = 167;
            this.Ckb_DeleteBTFN.Text = "删除";
            this.Ckb_DeleteBTFN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_DeleteBTFN.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_Hint.SetToolTip(this.Ckb_DeleteBTFN, "删除当前选中的一个方案");
            this.Ckb_DeleteBTFN.UseVisualStyleBackColor = true;
            this.Ckb_DeleteBTFN.Click += new System.EventHandler(this.Ckb_DeleteBTFN_Click);
            // 
            // Ckb_AddBTFN
            // 
            this.Ckb_AddBTFN.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_AddBTFN.AutoCheck = false;
            this.Ckb_AddBTFN.FlatAppearance.BorderSize = 0;
            this.Ckb_AddBTFN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_AddBTFN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_AddBTFN.Location = new System.Drawing.Point(4, 3);
            this.Ckb_AddBTFN.Name = "Ckb_AddBTFN";
            this.Ckb_AddBTFN.Size = new System.Drawing.Size(60, 25);
            this.Ckb_AddBTFN.TabIndex = 166;
            this.Ckb_AddBTFN.Text = "添加";
            this.Ckb_AddBTFN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_AddBTFN.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_Hint.SetToolTip(this.Ckb_AddBTFN, "添加一个方案");
            this.Ckb_AddBTFN.UseVisualStyleBackColor = true;
            this.Ckb_AddBTFN.Click += new System.EventHandler(this.Ckb_AddBTFN_Click);
            // 
            // Lbl_GJFNEncrypt
            // 
            this.Lbl_GJFNEncrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Lbl_GJFNEncrypt.Font = new System.Drawing.Font("微软雅黑", 30F);
            this.Lbl_GJFNEncrypt.Location = new System.Drawing.Point(0, 0);
            this.Lbl_GJFNEncrypt.Name = "Lbl_GJFNEncrypt";
            this.Lbl_GJFNEncrypt.Size = new System.Drawing.Size(992, 530);
            this.Lbl_GJFNEncrypt.TabIndex = 76;
            this.Lbl_GJFNEncrypt.Text = "已加密";
            this.Lbl_GJFNEncrypt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Tap_TBCount
            // 
            this.Tap_TBCount.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_TBCount.Controls.Add(this.TB_Main);
            this.Tap_TBCount.Location = new System.Drawing.Point(4, 34);
            this.Tap_TBCount.Name = "Tap_TBCount";
            this.Tap_TBCount.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_TBCount.Size = new System.Drawing.Size(998, 536);
            this.Tap_TBCount.TabIndex = 7;
            this.Tap_TBCount.Text = "推波计算";
            // 
            // TB_Main
            // 
            this.TB_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TB_Main.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.TB_Main.Location = new System.Drawing.Point(3, 3);
            this.TB_Main.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TB_Main.Name = "TB_Main";
            this.TB_Main.Size = new System.Drawing.Size(992, 530);
            this.TB_Main.TabIndex = 0;
            // 
            // Tap_HJFG
            // 
            this.Tap_HJFG.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_HJFG.Controls.Add(this.HJFG_Main);
            this.Tap_HJFG.Location = new System.Drawing.Point(4, 34);
            this.Tap_HJFG.Name = "Tap_HJFG";
            this.Tap_HJFG.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_HJFG.Size = new System.Drawing.Size(998, 536);
            this.Tap_HJFG.TabIndex = 17;
            this.Tap_HJFG.Text = "黄金分割";
            // 
            // HJFG_Main
            // 
            this.HJFG_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HJFG_Main.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.HJFG_Main.Location = new System.Drawing.Point(3, 3);
            this.HJFG_Main.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.HJFG_Main.Name = "HJFG_Main";
            this.HJFG_Main.Size = new System.Drawing.Size(992, 530);
            this.HJFG_Main.TabIndex = 0;
            // 
            // Tap_ShrinkEX
            // 
            this.Tap_ShrinkEX.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_ShrinkEX.Controls.Add(this.SK_EX);
            this.Tap_ShrinkEX.Location = new System.Drawing.Point(4, 34);
            this.Tap_ShrinkEX.Name = "Tap_ShrinkEX";
            this.Tap_ShrinkEX.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_ShrinkEX.Size = new System.Drawing.Size(998, 536);
            this.Tap_ShrinkEX.TabIndex = 19;
            this.Tap_ShrinkEX.Text = "二星缩水";
            // 
            // SK_EX
            // 
            this.SK_EX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SK_EX.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.SK_EX.Location = new System.Drawing.Point(3, 3);
            this.SK_EX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SK_EX.Name = "SK_EX";
            this.SK_EX.Size = new System.Drawing.Size(992, 530);
            this.SK_EX.TabIndex = 0;
            // 
            // Tap_ShrinkSX
            // 
            this.Tap_ShrinkSX.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_ShrinkSX.Controls.Add(this.SK_SX);
            this.Tap_ShrinkSX.Location = new System.Drawing.Point(4, 34);
            this.Tap_ShrinkSX.Name = "Tap_ShrinkSX";
            this.Tap_ShrinkSX.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_ShrinkSX.Size = new System.Drawing.Size(998, 536);
            this.Tap_ShrinkSX.TabIndex = 20;
            this.Tap_ShrinkSX.Text = "三星缩水";
            // 
            // SK_SX
            // 
            this.SK_SX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SK_SX.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.SK_SX.Location = new System.Drawing.Point(3, 3);
            this.SK_SX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SK_SX.Name = "SK_SX";
            this.SK_SX.Size = new System.Drawing.Size(992, 530);
            this.SK_SX.TabIndex = 0;
            // 
            // Tap_Setting
            // 
            this.Tap_Setting.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_Setting.Controls.Add(this.Pnl_Setting);
            this.Tap_Setting.Location = new System.Drawing.Point(4, 34);
            this.Tap_Setting.Name = "Tap_Setting";
            this.Tap_Setting.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_Setting.Size = new System.Drawing.Size(998, 536);
            this.Tap_Setting.TabIndex = 16;
            this.Tap_Setting.Text = "软件设置";
            // 
            // Pnl_Setting
            // 
            this.Pnl_Setting.Controls.Add(this.Egv_ShowTapList);
            this.Pnl_Setting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_Setting.Location = new System.Drawing.Point(3, 3);
            this.Pnl_Setting.Name = "Pnl_Setting";
            this.Pnl_Setting.Size = new System.Drawing.Size(992, 530);
            this.Pnl_Setting.TabIndex = 0;
            // 
            // Egv_ShowTapList
            // 
            this.Egv_ShowTapList.AllowUserToAddRows = false;
            this.Egv_ShowTapList.AllowUserToDeleteRows = false;
            this.Egv_ShowTapList.AllowUserToResizeColumns = false;
            this.Egv_ShowTapList.AllowUserToResizeRows = false;
            this.Egv_ShowTapList.BackgroundColor = System.Drawing.Color.White;
            this.Egv_ShowTapList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Egv_ShowTapList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_ShowTapList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.Egv_ShowTapList.ColumnHeadersHeight = 30;
            this.Egv_ShowTapList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_ShowTapList.DefaultCellStyle = dataGridViewCellStyle23;
            this.Egv_ShowTapList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Egv_ShowTapList.DragLineColor = System.Drawing.Color.Silver;
            this.Egv_ShowTapList.ExternalVirtualMode = true;
            this.Egv_ShowTapList.GridColor = System.Drawing.Color.Silver;
            this.Egv_ShowTapList.HeadersCheckDefult = System.Windows.Forms.CheckState.Checked;
            this.Egv_ShowTapList.Location = new System.Drawing.Point(0, 0);
            this.Egv_ShowTapList.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Egv_ShowTapList.MergeColumnHeaderForeColor = System.Drawing.Color.Black;
            this.Egv_ShowTapList.MultiSelect = false;
            this.Egv_ShowTapList.Name = "Egv_ShowTapList";
            this.Egv_ShowTapList.RowHeadersVisible = false;
            this.Egv_ShowTapList.RowNum = 17;
            dataGridViewCellStyle24.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.Color.Black;
            this.Egv_ShowTapList.RowsDefaultCellStyle = dataGridViewCellStyle24;
            this.Egv_ShowTapList.RowTemplate.Height = 23;
            this.Egv_ShowTapList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Egv_ShowTapList.Size = new System.Drawing.Size(992, 530);
            this.Egv_ShowTapList.TabIndex = 75;
            this.Egv_ShowTapList.VirtualMode = true;
            // 
            // Tap_CDCount
            // 
            this.Tap_CDCount.BackColor = System.Drawing.SystemColors.Control;
            this.Tap_CDCount.Controls.Add(this.Pnl_CDCount);
            this.Tap_CDCount.Location = new System.Drawing.Point(4, 34);
            this.Tap_CDCount.Name = "Tap_CDCount";
            this.Tap_CDCount.Padding = new System.Windows.Forms.Padding(3);
            this.Tap_CDCount.Size = new System.Drawing.Size(998, 536);
            this.Tap_CDCount.TabIndex = 21;
            this.Tap_CDCount.Text = "拆单计算";
            // 
            // Pnl_CDCount
            // 
            this.Pnl_CDCount.Controls.Add(this.CD_Main);
            this.Pnl_CDCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_CDCount.Location = new System.Drawing.Point(3, 3);
            this.Pnl_CDCount.Name = "Pnl_CDCount";
            this.Pnl_CDCount.Size = new System.Drawing.Size(992, 530);
            this.Pnl_CDCount.TabIndex = 0;
            // 
            // CD_Main
            // 
            this.CD_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CD_Main.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.CD_Main.Location = new System.Drawing.Point(0, 0);
            this.CD_Main.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CD_Main.Name = "CD_Main";
            this.CD_Main.Size = new System.Drawing.Size(992, 530);
            this.CD_Main.TabIndex = 0;
            // 
            // Pnl_OpenData
            // 
            this.Pnl_OpenData.Controls.Add(this.Pnl_DataMain);
            this.Pnl_OpenData.Controls.Add(this.Pnl_DataTop2);
            this.Pnl_OpenData.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_OpenData.Location = new System.Drawing.Point(0, 0);
            this.Pnl_OpenData.Name = "Pnl_OpenData";
            this.Pnl_OpenData.Size = new System.Drawing.Size(226, 576);
            this.Pnl_OpenData.TabIndex = 70;
            // 
            // Pnl_DataMain
            // 
            this.Pnl_DataMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_DataMain.Controls.Add(this.Pnl_DataBottom);
            this.Pnl_DataMain.Controls.Add(this.Pnl_RrfreshPT);
            this.Pnl_DataMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_DataMain.Location = new System.Drawing.Point(0, 270);
            this.Pnl_DataMain.Name = "Pnl_DataMain";
            this.Pnl_DataMain.Size = new System.Drawing.Size(226, 306);
            this.Pnl_DataMain.TabIndex = 67;
            // 
            // Pnl_DataBottom
            // 
            this.Pnl_DataBottom.Controls.Add(this.Egv_DataList);
            this.Pnl_DataBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_DataBottom.Location = new System.Drawing.Point(0, 185);
            this.Pnl_DataBottom.Name = "Pnl_DataBottom";
            this.Pnl_DataBottom.Size = new System.Drawing.Size(224, 119);
            this.Pnl_DataBottom.TabIndex = 73;
            // 
            // Egv_DataList
            // 
            this.Egv_DataList.AllowUserToAddRows = false;
            this.Egv_DataList.AllowUserToDeleteRows = false;
            this.Egv_DataList.AllowUserToResizeColumns = false;
            this.Egv_DataList.AllowUserToResizeRows = false;
            this.Egv_DataList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Egv_DataList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Egv_DataList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle25.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_DataList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle25;
            this.Egv_DataList.ColumnHeadersHeight = 30;
            this.Egv_DataList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_DataList.DefaultCellStyle = dataGridViewCellStyle26;
            this.Egv_DataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Egv_DataList.DragLineColor = System.Drawing.Color.Silver;
            this.Egv_DataList.ExternalVirtualMode = true;
            this.Egv_DataList.GridColor = System.Drawing.Color.Silver;
            this.Egv_DataList.HeadersCheckDefult = System.Windows.Forms.CheckState.Checked;
            this.Egv_DataList.Location = new System.Drawing.Point(0, 0);
            this.Egv_DataList.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Egv_DataList.MergeColumnHeaderForeColor = System.Drawing.Color.Black;
            this.Egv_DataList.MultiSelect = false;
            this.Egv_DataList.Name = "Egv_DataList";
            this.Egv_DataList.RowHeadersVisible = false;
            this.Egv_DataList.RowNum = 17;
            dataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.Color.Black;
            this.Egv_DataList.RowsDefaultCellStyle = dataGridViewCellStyle27;
            this.Egv_DataList.RowTemplate.Height = 23;
            this.Egv_DataList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Egv_DataList.Size = new System.Drawing.Size(224, 119);
            this.Egv_DataList.TabIndex = 75;
            this.Egv_DataList.VirtualMode = true;
            this.Egv_DataList.Visible = false;
            // 
            // Pnl_RrfreshPT
            // 
            this.Pnl_RrfreshPT.Controls.Add(this.Egv_PTLineList);
            this.Pnl_RrfreshPT.Controls.Add(this.Pnl_PTRefresh);
            this.Pnl_RrfreshPT.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_RrfreshPT.Location = new System.Drawing.Point(0, 0);
            this.Pnl_RrfreshPT.Name = "Pnl_RrfreshPT";
            this.Pnl_RrfreshPT.Size = new System.Drawing.Size(224, 185);
            this.Pnl_RrfreshPT.TabIndex = 72;
            // 
            // Egv_PTLineList
            // 
            this.Egv_PTLineList.AllowUserToAddRows = false;
            this.Egv_PTLineList.AllowUserToDeleteRows = false;
            this.Egv_PTLineList.AllowUserToResizeColumns = false;
            this.Egv_PTLineList.AllowUserToResizeRows = false;
            this.Egv_PTLineList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Egv_PTLineList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Egv_PTLineList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle28.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle28.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle28.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle28.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle28.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle28.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_PTLineList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle28;
            this.Egv_PTLineList.ColumnHeadersHeight = 30;
            this.Egv_PTLineList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle29.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_PTLineList.DefaultCellStyle = dataGridViewCellStyle29;
            this.Egv_PTLineList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Egv_PTLineList.DragLineColor = System.Drawing.Color.Silver;
            this.Egv_PTLineList.ExternalVirtualMode = true;
            this.Egv_PTLineList.GridColor = System.Drawing.Color.Silver;
            this.Egv_PTLineList.HeadersCheckDefult = System.Windows.Forms.CheckState.Checked;
            this.Egv_PTLineList.Location = new System.Drawing.Point(0, 35);
            this.Egv_PTLineList.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Egv_PTLineList.MergeColumnHeaderForeColor = System.Drawing.Color.Black;
            this.Egv_PTLineList.MultiSelect = false;
            this.Egv_PTLineList.Name = "Egv_PTLineList";
            this.Egv_PTLineList.RowHeadersVisible = false;
            this.Egv_PTLineList.RowNum = 17;
            dataGridViewCellStyle30.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle30.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle30.SelectionForeColor = System.Drawing.Color.Black;
            this.Egv_PTLineList.RowsDefaultCellStyle = dataGridViewCellStyle30;
            this.Egv_PTLineList.RowTemplate.Height = 23;
            this.Egv_PTLineList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Egv_PTLineList.Size = new System.Drawing.Size(224, 150);
            this.Egv_PTLineList.TabIndex = 73;
            // 
            // Pnl_PTRefresh
            // 
            this.Pnl_PTRefresh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_PTRefresh.Controls.Add(this.Ckb_DeleteLine);
            this.Pnl_PTRefresh.Controls.Add(this.Ckb_RrfreshPTLine);
            this.Pnl_PTRefresh.Controls.Add(this.Ckb_AddLine);
            this.Pnl_PTRefresh.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_PTRefresh.Location = new System.Drawing.Point(0, 0);
            this.Pnl_PTRefresh.Name = "Pnl_PTRefresh";
            this.Pnl_PTRefresh.Size = new System.Drawing.Size(224, 35);
            this.Pnl_PTRefresh.TabIndex = 74;
            // 
            // Ckb_DeleteLine
            // 
            this.Ckb_DeleteLine.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_DeleteLine.AutoCheck = false;
            this.Ckb_DeleteLine.FlatAppearance.BorderSize = 0;
            this.Ckb_DeleteLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_DeleteLine.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_DeleteLine.Location = new System.Drawing.Point(156, 4);
            this.Ckb_DeleteLine.Name = "Ckb_DeleteLine";
            this.Ckb_DeleteLine.Size = new System.Drawing.Size(60, 25);
            this.Ckb_DeleteLine.TabIndex = 211;
            this.Ckb_DeleteLine.Text = "删除";
            this.Ckb_DeleteLine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_DeleteLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Tot_Hint.SetToolTip(this.Ckb_DeleteLine, "删除选中的平台线路");
            this.Ckb_DeleteLine.UseVisualStyleBackColor = true;
            this.Ckb_DeleteLine.Click += new System.EventHandler(this.Ckb_DeleteLine_Click);
            // 
            // Ckb_RrfreshPTLine
            // 
            this.Ckb_RrfreshPTLine.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_RrfreshPTLine.AutoCheck = false;
            this.Ckb_RrfreshPTLine.FlatAppearance.BorderSize = 0;
            this.Ckb_RrfreshPTLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_RrfreshPTLine.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_RrfreshPTLine.Location = new System.Drawing.Point(4, 4);
            this.Ckb_RrfreshPTLine.Name = "Ckb_RrfreshPTLine";
            this.Ckb_RrfreshPTLine.Size = new System.Drawing.Size(80, 25);
            this.Ckb_RrfreshPTLine.TabIndex = 212;
            this.Ckb_RrfreshPTLine.Text = "更新线路";
            this.Ckb_RrfreshPTLine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_RrfreshPTLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Tot_Hint.SetToolTip(this.Ckb_RrfreshPTLine, "更新平台的最新线路");
            this.Ckb_RrfreshPTLine.UseVisualStyleBackColor = true;
            this.Ckb_RrfreshPTLine.Click += new System.EventHandler(this.Ckb_RrfreshPTLine_Click);
            // 
            // Ckb_AddLine
            // 
            this.Ckb_AddLine.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_AddLine.AutoCheck = false;
            this.Ckb_AddLine.FlatAppearance.BorderSize = 0;
            this.Ckb_AddLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_AddLine.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_AddLine.Location = new System.Drawing.Point(89, 4);
            this.Ckb_AddLine.Name = "Ckb_AddLine";
            this.Ckb_AddLine.Size = new System.Drawing.Size(60, 25);
            this.Ckb_AddLine.TabIndex = 210;
            this.Ckb_AddLine.Text = "添加";
            this.Ckb_AddLine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_AddLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_Hint.SetToolTip(this.Ckb_AddLine, "添加一条平台的线路");
            this.Ckb_AddLine.UseVisualStyleBackColor = true;
            this.Ckb_AddLine.Click += new System.EventHandler(this.Ckb_AddLine_Click);
            // 
            // Pnl_DataTop2
            // 
            this.Pnl_DataTop2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_DataTop2.Controls.Add(this.Pnl_DataBottom1);
            this.Pnl_DataTop2.Controls.Add(this.Pnl_UserLogin2);
            this.Pnl_DataTop2.Controls.Add(this.Pnl_UserLogin1);
            this.Pnl_DataTop2.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_DataTop2.Location = new System.Drawing.Point(0, 0);
            this.Pnl_DataTop2.Name = "Pnl_DataTop2";
            this.Pnl_DataTop2.Size = new System.Drawing.Size(226, 270);
            this.Pnl_DataTop2.TabIndex = 71;
            // 
            // Pnl_DataBottom1
            // 
            this.Pnl_DataBottom1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_DataBottom1.Controls.Add(this.Ckb_ShowHideUser);
            this.Pnl_DataBottom1.Controls.Add(this.Ckb_Login);
            this.Pnl_DataBottom1.Controls.Add(this.Ckb_RefreshUser);
            this.Pnl_DataBottom1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Pnl_DataBottom1.Location = new System.Drawing.Point(0, 233);
            this.Pnl_DataBottom1.Name = "Pnl_DataBottom1";
            this.Pnl_DataBottom1.Size = new System.Drawing.Size(224, 35);
            this.Pnl_DataBottom1.TabIndex = 13;
            // 
            // Ckb_ShowHideUser
            // 
            this.Ckb_ShowHideUser.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_ShowHideUser.AutoCheck = false;
            this.Ckb_ShowHideUser.FlatAppearance.BorderSize = 0;
            this.Ckb_ShowHideUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_ShowHideUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_ShowHideUser.Location = new System.Drawing.Point(7, 4);
            this.Ckb_ShowHideUser.Name = "Ckb_ShowHideUser";
            this.Ckb_ShowHideUser.Size = new System.Drawing.Size(60, 25);
            this.Ckb_ShowHideUser.TabIndex = 214;
            this.Ckb_ShowHideUser.Text = "隐藏";
            this.Ckb_ShowHideUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_ShowHideUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Tot_Hint.SetToolTip(this.Ckb_ShowHideUser, "隐藏已登录的平台账号");
            this.Ckb_ShowHideUser.UseVisualStyleBackColor = true;
            this.Ckb_ShowHideUser.Visible = false;
            this.Ckb_ShowHideUser.Click += new System.EventHandler(this.Ckb_ShowHideUser_Click);
            // 
            // Ckb_Login
            // 
            this.Ckb_Login.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_Login.AutoCheck = false;
            this.Ckb_Login.Enabled = false;
            this.Ckb_Login.FlatAppearance.BorderSize = 0;
            this.Ckb_Login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_Login.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_Login.Location = new System.Drawing.Point(137, 4);
            this.Ckb_Login.Name = "Ckb_Login";
            this.Ckb_Login.Size = new System.Drawing.Size(80, 25);
            this.Ckb_Login.TabIndex = 213;
            this.Ckb_Login.Text = "平台登录";
            this.Ckb_Login.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_Login.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Ckb_Login.UseVisualStyleBackColor = true;
            this.Ckb_Login.Click += new System.EventHandler(this.Ckb_Login_Click);
            // 
            // Ckb_RefreshUser
            // 
            this.Ckb_RefreshUser.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_RefreshUser.AutoCheck = false;
            this.Ckb_RefreshUser.FlatAppearance.BorderSize = 0;
            this.Ckb_RefreshUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_RefreshUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_RefreshUser.Location = new System.Drawing.Point(72, 4);
            this.Ckb_RefreshUser.Name = "Ckb_RefreshUser";
            this.Ckb_RefreshUser.Size = new System.Drawing.Size(60, 25);
            this.Ckb_RefreshUser.TabIndex = 211;
            this.Ckb_RefreshUser.Text = "刷新";
            this.Ckb_RefreshUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_RefreshUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Ckb_RefreshUser.UseVisualStyleBackColor = true;
            this.Ckb_RefreshUser.Visible = false;
            this.Ckb_RefreshUser.Click += new System.EventHandler(this.Ckb_RefreshUser_Click);
            // 
            // Pnl_UserLogin2
            // 
            this.Pnl_UserLogin2.Controls.Add(this.Ckb_PWPaste);
            this.Pnl_UserLogin2.Controls.Add(this.Ckb_PWClear);
            this.Pnl_UserLogin2.Controls.Add(this.Lbl_IDHint);
            this.Pnl_UserLogin2.Controls.Add(this.Cbb_Lottery);
            this.Pnl_UserLogin2.Controls.Add(this.Lbl_Lottery);
            this.Pnl_UserLogin2.Controls.Add(this.Txt_ID);
            this.Pnl_UserLogin2.Controls.Add(this.Txt_PW);
            this.Pnl_UserLogin2.Controls.Add(this.Lbl_LoginPT);
            this.Pnl_UserLogin2.Controls.Add(this.Cbb_LoginPT);
            this.Pnl_UserLogin2.Controls.Add(this.Lbl_LoginHint);
            this.Pnl_UserLogin2.Controls.Add(this.Lbl_PW);
            this.Pnl_UserLogin2.Controls.Add(this.Lbl_ID);
            this.Pnl_UserLogin2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_UserLogin2.Location = new System.Drawing.Point(0, 0);
            this.Pnl_UserLogin2.Name = "Pnl_UserLogin2";
            this.Pnl_UserLogin2.Size = new System.Drawing.Size(224, 268);
            this.Pnl_UserLogin2.TabIndex = 4;
            // 
            // Ckb_PWPaste
            // 
            this.Ckb_PWPaste.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_PWPaste.AutoCheck = false;
            this.Ckb_PWPaste.FlatAppearance.BorderSize = 0;
            this.Ckb_PWPaste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_PWPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_PWPaste.Location = new System.Drawing.Point(106, 152);
            this.Ckb_PWPaste.Name = "Ckb_PWPaste";
            this.Ckb_PWPaste.Size = new System.Drawing.Size(60, 25);
            this.Ckb_PWPaste.TabIndex = 310;
            this.Ckb_PWPaste.Text = "粘贴";
            this.Ckb_PWPaste.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_PWPaste.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Tot_Hint.SetToolTip(this.Ckb_PWPaste, "粘贴挂机令牌");
            this.Ckb_PWPaste.UseVisualStyleBackColor = true;
            this.Ckb_PWPaste.Visible = false;
            this.Ckb_PWPaste.Click += new System.EventHandler(this.Ckb_PWPaste_Click);
            // 
            // Ckb_PWClear
            // 
            this.Ckb_PWClear.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_PWClear.AutoCheck = false;
            this.Ckb_PWClear.FlatAppearance.BorderSize = 0;
            this.Ckb_PWClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_PWClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_PWClear.Location = new System.Drawing.Point(162, 152);
            this.Ckb_PWClear.Name = "Ckb_PWClear";
            this.Ckb_PWClear.Size = new System.Drawing.Size(60, 25);
            this.Ckb_PWClear.TabIndex = 309;
            this.Ckb_PWClear.Text = "清空";
            this.Ckb_PWClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_PWClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tot_Hint.SetToolTip(this.Ckb_PWClear, "清空当前挂机令牌");
            this.Ckb_PWClear.UseVisualStyleBackColor = true;
            this.Ckb_PWClear.Visible = false;
            this.Ckb_PWClear.Click += new System.EventHandler(this.Ckb_PWClear_Click);
            // 
            // Lbl_IDHint
            // 
            this.Lbl_IDHint.AutoSize = true;
            this.Lbl_IDHint.Location = new System.Drawing.Point(64, 108);
            this.Lbl_IDHint.Name = "Lbl_IDHint";
            this.Lbl_IDHint.Size = new System.Drawing.Size(68, 17);
            this.Lbl_IDHint.TabIndex = 30;
            this.Lbl_IDHint.Text = "（已绑定）";
            this.Lbl_IDHint.Visible = false;
            // 
            // Cbb_Lottery
            // 
            this.Cbb_Lottery.BackColor = System.Drawing.SystemColors.Window;
            this.Cbb_Lottery.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbb_Lottery.FormattingEnabled = true;
            this.Cbb_Lottery.Location = new System.Drawing.Point(8, 78);
            this.Cbb_Lottery.Name = "Cbb_Lottery";
            this.Cbb_Lottery.Size = new System.Drawing.Size(209, 25);
            this.Cbb_Lottery.TabIndex = 2;
            this.Cbb_Lottery.SelectedIndexChanged += new System.EventHandler(this.Cbb_Lottery_SelectedIndexChanged);
            // 
            // Lbl_Lottery
            // 
            this.Lbl_Lottery.AutoSize = true;
            this.Lbl_Lottery.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_Lottery.Location = new System.Drawing.Point(5, 58);
            this.Lbl_Lottery.Name = "Lbl_Lottery";
            this.Lbl_Lottery.Size = new System.Drawing.Size(68, 17);
            this.Lbl_Lottery.TabIndex = 29;
            this.Lbl_Lottery.Text = "投注彩种：";
            // 
            // Txt_ID
            // 
            this.Txt_ID.Location = new System.Drawing.Point(8, 128);
            this.Txt_ID.Name = "Txt_ID";
            this.Txt_ID.Size = new System.Drawing.Size(209, 23);
            this.Txt_ID.TabIndex = 3;
            // 
            // Txt_PW
            // 
            this.Txt_PW.Location = new System.Drawing.Point(8, 178);
            this.Txt_PW.Name = "Txt_PW";
            this.Txt_PW.PasswordChar = '*';
            this.Txt_PW.Size = new System.Drawing.Size(209, 23);
            this.Txt_PW.TabIndex = 4;
            // 
            // Lbl_LoginPT
            // 
            this.Lbl_LoginPT.AutoSize = true;
            this.Lbl_LoginPT.Location = new System.Drawing.Point(5, 8);
            this.Lbl_LoginPT.Name = "Lbl_LoginPT";
            this.Lbl_LoginPT.Size = new System.Drawing.Size(68, 17);
            this.Lbl_LoginPT.TabIndex = 17;
            this.Lbl_LoginPT.Text = "选择平台：";
            // 
            // Cbb_LoginPT
            // 
            this.Cbb_LoginPT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbb_LoginPT.FormattingEnabled = true;
            this.Cbb_LoginPT.Location = new System.Drawing.Point(8, 28);
            this.Cbb_LoginPT.Name = "Cbb_LoginPT";
            this.Cbb_LoginPT.Size = new System.Drawing.Size(209, 25);
            this.Cbb_LoginPT.TabIndex = 1;
            this.Cbb_LoginPT.SelectedIndexChanged += new System.EventHandler(this.Cbb_LoginPT_SelectedIndexChanged);
            // 
            // Lbl_LoginHint
            // 
            this.Lbl_LoginHint.AutoSize = true;
            this.Lbl_LoginHint.Location = new System.Drawing.Point(7, 208);
            this.Lbl_LoginHint.Name = "Lbl_LoginHint";
            this.Lbl_LoginHint.Size = new System.Drawing.Size(0, 17);
            this.Lbl_LoginHint.TabIndex = 27;
            // 
            // Lbl_PW
            // 
            this.Lbl_PW.AutoSize = true;
            this.Lbl_PW.Location = new System.Drawing.Point(5, 158);
            this.Lbl_PW.Name = "Lbl_PW";
            this.Lbl_PW.Size = new System.Drawing.Size(68, 17);
            this.Lbl_PW.TabIndex = 24;
            this.Lbl_PW.Text = "会员密码：";
            // 
            // Lbl_ID
            // 
            this.Lbl_ID.AutoSize = true;
            this.Lbl_ID.Location = new System.Drawing.Point(5, 108);
            this.Lbl_ID.Name = "Lbl_ID";
            this.Lbl_ID.Size = new System.Drawing.Size(68, 17);
            this.Lbl_ID.TabIndex = 23;
            this.Lbl_ID.Text = "会员账号：";
            // 
            // Pnl_UserLogin1
            // 
            this.Pnl_UserLogin1.Controls.Add(this.Txt_KSStopBets);
            this.Pnl_UserLogin1.Controls.Add(this.Txt_YLStopBets);
            this.Pnl_UserLogin1.Controls.Add(this.Lbl_KSStopBets);
            this.Pnl_UserLogin1.Controls.Add(this.Lbl_YLStopBets);
            this.Pnl_UserLogin1.Controls.Add(this.Lbl_StopBets);
            this.Pnl_UserLogin1.Controls.Add(this.Lbl_BankBalanceValue);
            this.Pnl_UserLogin1.Controls.Add(this.Lbl_BetsExpectValue);
            this.Pnl_UserLogin1.Controls.Add(this.Lbl_BankBalanceKey);
            this.Pnl_UserLogin1.Controls.Add(this.Lbl_BetsExpectKey);
            this.Pnl_UserLogin1.Controls.Add(this.Lbl_IDValue);
            this.Pnl_UserLogin1.Controls.Add(this.Lbl_IDKey);
            this.Pnl_UserLogin1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_UserLogin1.Location = new System.Drawing.Point(0, 0);
            this.Pnl_UserLogin1.Name = "Pnl_UserLogin1";
            this.Pnl_UserLogin1.Size = new System.Drawing.Size(224, 268);
            this.Pnl_UserLogin1.TabIndex = 71;
            // 
            // Txt_KSStopBets
            // 
            this.Txt_KSStopBets.Location = new System.Drawing.Point(136, 130);
            this.Txt_KSStopBets.Name = "Txt_KSStopBets";
            this.Txt_KSStopBets.Size = new System.Drawing.Size(80, 23);
            this.Txt_KSStopBets.TabIndex = 84;
            this.Txt_KSStopBets.Text = "50000";
            this.Txt_KSStopBets.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Txt_Input1_KeyPress);
            // 
            // Txt_YLStopBets
            // 
            this.Txt_YLStopBets.Location = new System.Drawing.Point(136, 100);
            this.Txt_YLStopBets.Name = "Txt_YLStopBets";
            this.Txt_YLStopBets.Size = new System.Drawing.Size(80, 23);
            this.Txt_YLStopBets.TabIndex = 83;
            this.Txt_YLStopBets.Text = "50000";
            this.Txt_YLStopBets.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Txt_Input1_KeyPress);
            // 
            // Lbl_KSStopBets
            // 
            this.Lbl_KSStopBets.AutoSize = true;
            this.Lbl_KSStopBets.Location = new System.Drawing.Point(76, 132);
            this.Lbl_KSStopBets.Name = "Lbl_KSStopBets";
            this.Lbl_KSStopBets.Size = new System.Drawing.Size(56, 17);
            this.Lbl_KSStopBets.TabIndex = 81;
            this.Lbl_KSStopBets.Text = "亏损大于";
            // 
            // Lbl_YLStopBets
            // 
            this.Lbl_YLStopBets.AutoSize = true;
            this.Lbl_YLStopBets.Location = new System.Drawing.Point(76, 102);
            this.Lbl_YLStopBets.Name = "Lbl_YLStopBets";
            this.Lbl_YLStopBets.Size = new System.Drawing.Size(56, 17);
            this.Lbl_YLStopBets.TabIndex = 79;
            this.Lbl_YLStopBets.Text = "盈利大于";
            // 
            // Lbl_StopBets
            // 
            this.Lbl_StopBets.AutoSize = true;
            this.Lbl_StopBets.Location = new System.Drawing.Point(5, 117);
            this.Lbl_StopBets.Name = "Lbl_StopBets";
            this.Lbl_StopBets.Size = new System.Drawing.Size(68, 17);
            this.Lbl_StopBets.TabIndex = 78;
            this.Lbl_StopBets.Text = "止损盈亏：";
            // 
            // Lbl_BankBalanceValue
            // 
            this.Lbl_BankBalanceValue.AutoSize = true;
            this.Lbl_BankBalanceValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_BankBalanceValue.Location = new System.Drawing.Point(76, 72);
            this.Lbl_BankBalanceValue.Name = "Lbl_BankBalanceValue";
            this.Lbl_BankBalanceValue.Size = new System.Drawing.Size(22, 17);
            this.Lbl_BankBalanceValue.TabIndex = 75;
            this.Lbl_BankBalanceValue.Text = "00";
            // 
            // Lbl_BetsExpectValue
            // 
            this.Lbl_BetsExpectValue.AutoSize = true;
            this.Lbl_BetsExpectValue.Location = new System.Drawing.Point(76, 12);
            this.Lbl_BetsExpectValue.Name = "Lbl_BetsExpectValue";
            this.Lbl_BetsExpectValue.Size = new System.Drawing.Size(22, 17);
            this.Lbl_BetsExpectValue.TabIndex = 1;
            this.Lbl_BetsExpectValue.Text = "00";
            // 
            // Lbl_BankBalanceKey
            // 
            this.Lbl_BankBalanceKey.AutoSize = true;
            this.Lbl_BankBalanceKey.Location = new System.Drawing.Point(5, 72);
            this.Lbl_BankBalanceKey.Name = "Lbl_BankBalanceKey";
            this.Lbl_BankBalanceKey.Size = new System.Drawing.Size(68, 17);
            this.Lbl_BankBalanceKey.TabIndex = 74;
            this.Lbl_BankBalanceKey.Text = "账号余额：";
            // 
            // Lbl_BetsExpectKey
            // 
            this.Lbl_BetsExpectKey.AutoSize = true;
            this.Lbl_BetsExpectKey.Location = new System.Drawing.Point(5, 12);
            this.Lbl_BetsExpectKey.Name = "Lbl_BetsExpectKey";
            this.Lbl_BetsExpectKey.Size = new System.Drawing.Size(68, 17);
            this.Lbl_BetsExpectKey.TabIndex = 0;
            this.Lbl_BetsExpectKey.Text = "投注期数：";
            // 
            // Lbl_IDValue
            // 
            this.Lbl_IDValue.AutoSize = true;
            this.Lbl_IDValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_IDValue.Location = new System.Drawing.Point(76, 42);
            this.Lbl_IDValue.Name = "Lbl_IDValue";
            this.Lbl_IDValue.Size = new System.Drawing.Size(22, 17);
            this.Lbl_IDValue.TabIndex = 73;
            this.Lbl_IDValue.Text = "00";
            // 
            // Lbl_IDKey
            // 
            this.Lbl_IDKey.AutoSize = true;
            this.Lbl_IDKey.Location = new System.Drawing.Point(5, 42);
            this.Lbl_IDKey.Name = "Lbl_IDKey";
            this.Lbl_IDKey.Size = new System.Drawing.Size(68, 17);
            this.Lbl_IDKey.TabIndex = 72;
            this.Lbl_IDKey.Text = "会员账号：";
            // 
            // Ckb_PlaySound
            // 
            this.Ckb_PlaySound.AutoSize = true;
            this.Ckb_PlaySound.Location = new System.Drawing.Point(5, 39);
            this.Ckb_PlaySound.Name = "Ckb_PlaySound";
            this.Ckb_PlaySound.Size = new System.Drawing.Size(75, 21);
            this.Ckb_PlaySound.TabIndex = 183;
            this.Ckb_PlaySound.Text = "声音提示";
            this.Tot_Hint.SetToolTip(this.Ckb_PlaySound, "是否要开启软件的声音");
            this.Ckb_PlaySound.UseVisualStyleBackColor = true;
            this.Ckb_PlaySound.CheckedChanged += new System.EventHandler(this.Ckb_PlaySound_CheckedChanged);
            // 
            // Tim_NextExpect
            // 
            this.Tim_NextExpect.Interval = 1000;
            // 
            // Pnl_Top
            // 
            this.Pnl_Top.BackColor = System.Drawing.SystemColors.Control;
            this.Pnl_Top.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_Top.Controls.Add(this.Pnl_GG);
            this.Pnl_Top.Controls.Add(this.Pnl_CurrentExpect1);
            this.Pnl_Top.Controls.Add(this.Pnl_Notice);
            this.Pnl_Top.Controls.Add(this.Pnl_LTUserInfo);
            this.Pnl_Top.Controls.Add(this.Pnl_CurrentExpect);
            this.Pnl_Top.Controls.Add(this.Pnl_NextExpect);
            this.Pnl_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_Top.Location = new System.Drawing.Point(0, 0);
            this.Pnl_Top.Name = "Pnl_Top";
            this.Pnl_Top.Size = new System.Drawing.Size(1234, 110);
            this.Pnl_Top.TabIndex = 3;
            // 
            // Pnl_GG
            // 
            this.Pnl_GG.Controls.Add(this.Piw_Main);
            this.Pnl_GG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_GG.Location = new System.Drawing.Point(1279, 0);
            this.Pnl_GG.Name = "Pnl_GG";
            this.Pnl_GG.Size = new System.Drawing.Size(0, 108);
            this.Pnl_GG.TabIndex = 26;
            // 
            // Piw_Main
            // 
            this.Piw_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Piw_Main.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Piw_Main.Location = new System.Drawing.Point(0, 0);
            this.Piw_Main.Name = "Piw_Main";
            this.Piw_Main.Size = new System.Drawing.Size(0, 108);
            this.Piw_Main.TabIndex = 0;
            // 
            // Pnl_CurrentExpect1
            // 
            this.Pnl_CurrentExpect1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_CurrentExpect1.Controls.Add(this.Pnl_CurrentCode3);
            this.Pnl_CurrentExpect1.Controls.Add(this.Pnl_CurrentCode4);
            this.Pnl_CurrentExpect1.Controls.Add(this.Lbl_NextExpect1);
            this.Pnl_CurrentExpect1.Controls.Add(this.Lbl_NextTime1);
            this.Pnl_CurrentExpect1.Controls.Add(this.Lbl_CurrentExpect1);
            this.Pnl_CurrentExpect1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_CurrentExpect1.Location = new System.Drawing.Point(1054, 0);
            this.Pnl_CurrentExpect1.Name = "Pnl_CurrentExpect1";
            this.Pnl_CurrentExpect1.Size = new System.Drawing.Size(225, 108);
            this.Pnl_CurrentExpect1.TabIndex = 71;
            this.Pnl_CurrentExpect1.Visible = false;
            // 
            // Pnl_CurrentCode3
            // 
            this.Pnl_CurrentCode3.Controls.Add(this.Lbl_CurrentCode7);
            this.Pnl_CurrentCode3.Controls.Add(this.Lbl_CurrentCode6);
            this.Pnl_CurrentCode3.Controls.Add(this.Lbl_CurrentCode10);
            this.Pnl_CurrentCode3.Controls.Add(this.Lbl_CurrentCode9);
            this.Pnl_CurrentCode3.Controls.Add(this.Lbl_CurrentCode8);
            this.Pnl_CurrentCode3.Location = new System.Drawing.Point(0, 28);
            this.Pnl_CurrentCode3.Name = "Pnl_CurrentCode3";
            this.Pnl_CurrentCode3.Size = new System.Drawing.Size(223, 50);
            this.Pnl_CurrentCode3.TabIndex = 5;
            // 
            // Lbl_CurrentCode7
            // 
            this.Lbl_CurrentCode7.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.Lbl_CurrentCode7.ForeColor = System.Drawing.Color.White;
            this.Lbl_CurrentCode7.Location = new System.Drawing.Point(49, 5);
            this.Lbl_CurrentCode7.Name = "Lbl_CurrentCode7";
            this.Lbl_CurrentCode7.Size = new System.Drawing.Size(40, 40);
            this.Lbl_CurrentCode7.TabIndex = 2;
            this.Lbl_CurrentCode7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lbl_CurrentCode6
            // 
            this.Lbl_CurrentCode6.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.Lbl_CurrentCode6.ForeColor = System.Drawing.Color.White;
            this.Lbl_CurrentCode6.Location = new System.Drawing.Point(5, 5);
            this.Lbl_CurrentCode6.Name = "Lbl_CurrentCode6";
            this.Lbl_CurrentCode6.Size = new System.Drawing.Size(40, 40);
            this.Lbl_CurrentCode6.TabIndex = 0;
            this.Lbl_CurrentCode6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lbl_CurrentCode10
            // 
            this.Lbl_CurrentCode10.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.Lbl_CurrentCode10.ForeColor = System.Drawing.Color.White;
            this.Lbl_CurrentCode10.Location = new System.Drawing.Point(181, 5);
            this.Lbl_CurrentCode10.Name = "Lbl_CurrentCode10";
            this.Lbl_CurrentCode10.Size = new System.Drawing.Size(40, 40);
            this.Lbl_CurrentCode10.TabIndex = 5;
            this.Lbl_CurrentCode10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lbl_CurrentCode9
            // 
            this.Lbl_CurrentCode9.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.Lbl_CurrentCode9.ForeColor = System.Drawing.Color.White;
            this.Lbl_CurrentCode9.Location = new System.Drawing.Point(137, 5);
            this.Lbl_CurrentCode9.Name = "Lbl_CurrentCode9";
            this.Lbl_CurrentCode9.Size = new System.Drawing.Size(40, 40);
            this.Lbl_CurrentCode9.TabIndex = 4;
            this.Lbl_CurrentCode9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lbl_CurrentCode8
            // 
            this.Lbl_CurrentCode8.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.Lbl_CurrentCode8.ForeColor = System.Drawing.Color.White;
            this.Lbl_CurrentCode8.Location = new System.Drawing.Point(93, 5);
            this.Lbl_CurrentCode8.Name = "Lbl_CurrentCode8";
            this.Lbl_CurrentCode8.Size = new System.Drawing.Size(40, 40);
            this.Lbl_CurrentCode8.TabIndex = 3;
            this.Lbl_CurrentCode8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Pnl_CurrentCode4
            // 
            this.Pnl_CurrentCode4.Controls.Add(this.PK_CodeSmall);
            this.Pnl_CurrentCode4.Location = new System.Drawing.Point(0, 28);
            this.Pnl_CurrentCode4.Name = "Pnl_CurrentCode4";
            this.Pnl_CurrentCode4.Size = new System.Drawing.Size(223, 50);
            this.Pnl_CurrentCode4.TabIndex = 6;
            // 
            // PK_CodeSmall
            // 
            this.PK_CodeSmall.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.PK_CodeSmall.Location = new System.Drawing.Point(1, 0);
            this.PK_CodeSmall.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PK_CodeSmall.Name = "PK_CodeSmall";
            this.PK_CodeSmall.Size = new System.Drawing.Size(223, 50);
            this.PK_CodeSmall.TabIndex = 0;
            // 
            // Lbl_NextExpect1
            // 
            this.Lbl_NextExpect1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_NextExpect1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lbl_NextExpect1.Location = new System.Drawing.Point(5, 84);
            this.Lbl_NextExpect1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lbl_NextExpect1.Name = "Lbl_NextExpect1";
            this.Lbl_NextExpect1.Size = new System.Drawing.Size(155, 17);
            this.Lbl_NextExpect1.TabIndex = 1;
            this.Lbl_NextExpect1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lbl_NextTime1
            // 
            this.Lbl_NextTime1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_NextTime1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.Lbl_NextTime1.Location = new System.Drawing.Point(157, 84);
            this.Lbl_NextTime1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lbl_NextTime1.Name = "Lbl_NextTime1";
            this.Lbl_NextTime1.Size = new System.Drawing.Size(60, 17);
            this.Lbl_NextTime1.TabIndex = 8;
            this.Lbl_NextTime1.Text = "00:00:00";
            this.Lbl_NextTime1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lbl_CurrentExpect1
            // 
            this.Lbl_CurrentExpect1.Location = new System.Drawing.Point(0, 6);
            this.Lbl_CurrentExpect1.Name = "Lbl_CurrentExpect1";
            this.Lbl_CurrentExpect1.Size = new System.Drawing.Size(223, 17);
            this.Lbl_CurrentExpect1.TabIndex = 1;
            this.Lbl_CurrentExpect1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Pnl_Notice
            // 
            this.Pnl_Notice.Controls.Add(this.Egv_NoticeList);
            this.Pnl_Notice.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_Notice.Location = new System.Drawing.Point(830, 0);
            this.Pnl_Notice.Name = "Pnl_Notice";
            this.Pnl_Notice.Size = new System.Drawing.Size(224, 108);
            this.Pnl_Notice.TabIndex = 68;
            this.Pnl_Notice.Visible = false;
            // 
            // Egv_NoticeList
            // 
            this.Egv_NoticeList.AllowUserToAddRows = false;
            this.Egv_NoticeList.AllowUserToDeleteRows = false;
            this.Egv_NoticeList.AllowUserToResizeColumns = false;
            this.Egv_NoticeList.AllowUserToResizeRows = false;
            this.Egv_NoticeList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Egv_NoticeList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Egv_NoticeList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle31.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle31.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle31.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle31.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle31.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle31.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Egv_NoticeList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle31;
            this.Egv_NoticeList.ColumnHeadersHeight = 30;
            this.Egv_NoticeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle32.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle32.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle32.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle32.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle32.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle32.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Egv_NoticeList.DefaultCellStyle = dataGridViewCellStyle32;
            this.Egv_NoticeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Egv_NoticeList.DragLineColor = System.Drawing.Color.Silver;
            this.Egv_NoticeList.ExternalVirtualMode = true;
            this.Egv_NoticeList.GridColor = System.Drawing.Color.Silver;
            this.Egv_NoticeList.HeadersCheckDefult = System.Windows.Forms.CheckState.Checked;
            this.Egv_NoticeList.Location = new System.Drawing.Point(0, 0);
            this.Egv_NoticeList.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.Egv_NoticeList.MergeColumnHeaderForeColor = System.Drawing.Color.Black;
            this.Egv_NoticeList.MultiSelect = false;
            this.Egv_NoticeList.Name = "Egv_NoticeList";
            this.Egv_NoticeList.RowHeadersVisible = false;
            this.Egv_NoticeList.RowNum = 17;
            dataGridViewCellStyle33.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle33.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle33.SelectionForeColor = System.Drawing.Color.Black;
            this.Egv_NoticeList.RowsDefaultCellStyle = dataGridViewCellStyle33;
            this.Egv_NoticeList.RowTemplate.Height = 23;
            this.Egv_NoticeList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Egv_NoticeList.Size = new System.Drawing.Size(224, 108);
            this.Egv_NoticeList.TabIndex = 67;
            this.Egv_NoticeList.VirtualMode = true;
            this.Egv_NoticeList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Egv_NoticeList_CellClick);
            // 
            // Pnl_LTUserInfo
            // 
            this.Pnl_LTUserInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_LTUserInfo.Controls.Add(this.Pnl_AppName);
            this.Pnl_LTUserInfo.Controls.Add(this.Pnl_LTUserInfoTop);
            this.Pnl_LTUserInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_LTUserInfo.Location = new System.Drawing.Point(580, 0);
            this.Pnl_LTUserInfo.Name = "Pnl_LTUserInfo";
            this.Pnl_LTUserInfo.Size = new System.Drawing.Size(250, 108);
            this.Pnl_LTUserInfo.TabIndex = 25;
            // 
            // Pnl_AppName
            // 
            this.Pnl_AppName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_AppName.Controls.Add(this.Ckb_AppName);
            this.Pnl_AppName.Controls.Add(this.Txt_AppName);
            this.Pnl_AppName.Controls.Add(this.Lbl_AppName);
            this.Pnl_AppName.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_AppName.Location = new System.Drawing.Point(0, 72);
            this.Pnl_AppName.Name = "Pnl_AppName";
            this.Pnl_AppName.Size = new System.Drawing.Size(248, 35);
            this.Pnl_AppName.TabIndex = 188;
            // 
            // Ckb_AppName
            // 
            this.Ckb_AppName.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_AppName.AutoCheck = false;
            this.Ckb_AppName.FlatAppearance.BorderSize = 0;
            this.Ckb_AppName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_AppName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ckb_AppName.Location = new System.Drawing.Point(183, 4);
            this.Ckb_AppName.Name = "Ckb_AppName";
            this.Ckb_AppName.Size = new System.Drawing.Size(60, 25);
            this.Ckb_AppName.TabIndex = 309;
            this.Ckb_AppName.Text = "修改";
            this.Ckb_AppName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ckb_AppName.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Ckb_AppName.UseVisualStyleBackColor = true;
            this.Ckb_AppName.Click += new System.EventHandler(this.Ckb_AppName_Click);
            // 
            // Txt_AppName
            // 
            this.Txt_AppName.Location = new System.Drawing.Point(78, 5);
            this.Txt_AppName.Name = "Txt_AppName";
            this.Txt_AppName.Size = new System.Drawing.Size(100, 23);
            this.Txt_AppName.TabIndex = 10;
            // 
            // Lbl_AppName
            // 
            this.Lbl_AppName.AutoSize = true;
            this.Lbl_AppName.Location = new System.Drawing.Point(5, 8);
            this.Lbl_AppName.Name = "Lbl_AppName";
            this.Lbl_AppName.Size = new System.Drawing.Size(68, 17);
            this.Lbl_AppName.TabIndex = 182;
            this.Lbl_AppName.Text = "软件标识：";
            // 
            // Pnl_LTUserInfoTop
            // 
            this.Pnl_LTUserInfoTop.Controls.Add(this.Ckb_CloseMin);
            this.Pnl_LTUserInfoTop.Controls.Add(this.Ckb_LeftInfo);
            this.Pnl_LTUserInfoTop.Controls.Add(this.Ckb_OpenHint);
            this.Pnl_LTUserInfoTop.Controls.Add(this.Ckb_RrfreshPT);
            this.Pnl_LTUserInfoTop.Controls.Add(this.Ckb_PlaySound);
            this.Pnl_LTUserInfoTop.Controls.Add(this.Ckb_Data);
            this.Pnl_LTUserInfoTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_LTUserInfoTop.Location = new System.Drawing.Point(0, 0);
            this.Pnl_LTUserInfoTop.Name = "Pnl_LTUserInfoTop";
            this.Pnl_LTUserInfoTop.Size = new System.Drawing.Size(248, 72);
            this.Pnl_LTUserInfoTop.TabIndex = 68;
            // 
            // Ckb_CloseMin
            // 
            this.Ckb_CloseMin.AutoSize = true;
            this.Ckb_CloseMin.Location = new System.Drawing.Point(167, 39);
            this.Ckb_CloseMin.Name = "Ckb_CloseMin";
            this.Ckb_CloseMin.Size = new System.Drawing.Size(75, 21);
            this.Ckb_CloseMin.TabIndex = 192;
            this.Ckb_CloseMin.Text = "关闭托盘";
            this.Tot_Hint.SetToolTip(this.Ckb_CloseMin, "是否要关闭时最小化到托盘区");
            this.Ckb_CloseMin.UseVisualStyleBackColor = true;
            // 
            // Ckb_LeftInfo
            // 
            this.Ckb_LeftInfo.AutoSize = true;
            this.Ckb_LeftInfo.Checked = true;
            this.Ckb_LeftInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Ckb_LeftInfo.Location = new System.Drawing.Point(5, 8);
            this.Ckb_LeftInfo.Name = "Ckb_LeftInfo";
            this.Ckb_LeftInfo.Size = new System.Drawing.Size(75, 21);
            this.Ckb_LeftInfo.TabIndex = 185;
            this.Ckb_LeftInfo.Text = "平台信息";
            this.Tot_Hint.SetToolTip(this.Ckb_LeftInfo, "是否要显示左边的平台登录信息");
            this.Ckb_LeftInfo.UseVisualStyleBackColor = true;
            this.Ckb_LeftInfo.CheckedChanged += new System.EventHandler(this.Ckb_LeftInfo_CheckedChanged);
            // 
            // Ckb_OpenHint
            // 
            this.Ckb_OpenHint.AutoSize = true;
            this.Ckb_OpenHint.Location = new System.Drawing.Point(86, 39);
            this.Ckb_OpenHint.Name = "Ckb_OpenHint";
            this.Ckb_OpenHint.Size = new System.Drawing.Size(75, 21);
            this.Ckb_OpenHint.TabIndex = 190;
            this.Ckb_OpenHint.Text = "开奖提示";
            this.Tot_Hint.SetToolTip(this.Ckb_OpenHint, "是否要在托盘区提示最新开奖号码");
            this.Ckb_OpenHint.UseVisualStyleBackColor = true;
            // 
            // Ckb_RrfreshPT
            // 
            this.Ckb_RrfreshPT.AutoSize = true;
            this.Ckb_RrfreshPT.Checked = true;
            this.Ckb_RrfreshPT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Ckb_RrfreshPT.Location = new System.Drawing.Point(86, 8);
            this.Ckb_RrfreshPT.Name = "Ckb_RrfreshPT";
            this.Ckb_RrfreshPT.Size = new System.Drawing.Size(75, 21);
            this.Ckb_RrfreshPT.TabIndex = 187;
            this.Ckb_RrfreshPT.Text = "平台地址";
            this.Tot_Hint.SetToolTip(this.Ckb_RrfreshPT, "是否要显示左边的平台地址");
            this.Ckb_RrfreshPT.UseVisualStyleBackColor = true;
            this.Ckb_RrfreshPT.CheckedChanged += new System.EventHandler(this.Ckb_RrfreshPT_CheckedChanged);
            // 
            // Ckb_Data
            // 
            this.Ckb_Data.AutoSize = true;
            this.Ckb_Data.Location = new System.Drawing.Point(167, 8);
            this.Ckb_Data.Name = "Ckb_Data";
            this.Ckb_Data.Size = new System.Drawing.Size(75, 21);
            this.Ckb_Data.TabIndex = 189;
            this.Ckb_Data.Text = "历史号码";
            this.Tot_Hint.SetToolTip(this.Ckb_Data, "是否要显示左边的历史开奖号码");
            this.Ckb_Data.UseVisualStyleBackColor = true;
            this.Ckb_Data.CheckedChanged += new System.EventHandler(this.Ckb_Data_CheckedChanged);
            // 
            // Pnl_CurrentExpect
            // 
            this.Pnl_CurrentExpect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_CurrentExpect.Controls.Add(this.Pnl_CurrentCode1);
            this.Pnl_CurrentExpect.Controls.Add(this.Pnl_CurrentCode2);
            this.Pnl_CurrentExpect.Controls.Add(this.Pnl_CurrentExpectTop);
            this.Pnl_CurrentExpect.Controls.Add(this.Ckb_AutomationRun);
            this.Pnl_CurrentExpect.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_CurrentExpect.Location = new System.Drawing.Point(225, 0);
            this.Pnl_CurrentExpect.Name = "Pnl_CurrentExpect";
            this.Pnl_CurrentExpect.Size = new System.Drawing.Size(355, 108);
            this.Pnl_CurrentExpect.TabIndex = 9;
            // 
            // Pnl_CurrentCode1
            // 
            this.Pnl_CurrentCode1.Controls.Add(this.Lbl_CurrentCode1);
            this.Pnl_CurrentCode1.Controls.Add(this.Lbl_CurrentCode5);
            this.Pnl_CurrentCode1.Controls.Add(this.Lbl_CurrentCode3);
            this.Pnl_CurrentCode1.Controls.Add(this.Lbl_CurrentCode2);
            this.Pnl_CurrentCode1.Controls.Add(this.Lbl_CurrentCode4);
            this.Pnl_CurrentCode1.Location = new System.Drawing.Point(0, 35);
            this.Pnl_CurrentCode1.Name = "Pnl_CurrentCode1";
            this.Pnl_CurrentCode1.Size = new System.Drawing.Size(353, 71);
            this.Pnl_CurrentCode1.TabIndex = 10;
            // 
            // Lbl_CurrentCode1
            // 
            this.Lbl_CurrentCode1.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_CurrentCode1.Font = new System.Drawing.Font("微软雅黑", 23F, System.Drawing.FontStyle.Bold);
            this.Lbl_CurrentCode1.ForeColor = System.Drawing.Color.White;
            this.Lbl_CurrentCode1.Location = new System.Drawing.Point(11, 7);
            this.Lbl_CurrentCode1.Name = "Lbl_CurrentCode1";
            this.Lbl_CurrentCode1.Size = new System.Drawing.Size(55, 55);
            this.Lbl_CurrentCode1.TabIndex = 0;
            this.Lbl_CurrentCode1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lbl_CurrentCode5
            // 
            this.Lbl_CurrentCode5.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_CurrentCode5.Font = new System.Drawing.Font("微软雅黑", 23F, System.Drawing.FontStyle.Bold);
            this.Lbl_CurrentCode5.ForeColor = System.Drawing.Color.White;
            this.Lbl_CurrentCode5.Location = new System.Drawing.Point(287, 7);
            this.Lbl_CurrentCode5.Name = "Lbl_CurrentCode5";
            this.Lbl_CurrentCode5.Size = new System.Drawing.Size(55, 55);
            this.Lbl_CurrentCode5.TabIndex = 4;
            this.Lbl_CurrentCode5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lbl_CurrentCode3
            // 
            this.Lbl_CurrentCode3.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_CurrentCode3.Font = new System.Drawing.Font("微软雅黑", 23F, System.Drawing.FontStyle.Bold);
            this.Lbl_CurrentCode3.ForeColor = System.Drawing.Color.White;
            this.Lbl_CurrentCode3.Location = new System.Drawing.Point(149, 7);
            this.Lbl_CurrentCode3.Name = "Lbl_CurrentCode3";
            this.Lbl_CurrentCode3.Size = new System.Drawing.Size(55, 55);
            this.Lbl_CurrentCode3.TabIndex = 2;
            this.Lbl_CurrentCode3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lbl_CurrentCode2
            // 
            this.Lbl_CurrentCode2.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_CurrentCode2.Font = new System.Drawing.Font("微软雅黑", 23F, System.Drawing.FontStyle.Bold);
            this.Lbl_CurrentCode2.ForeColor = System.Drawing.Color.White;
            this.Lbl_CurrentCode2.Location = new System.Drawing.Point(80, 7);
            this.Lbl_CurrentCode2.Name = "Lbl_CurrentCode2";
            this.Lbl_CurrentCode2.Size = new System.Drawing.Size(55, 55);
            this.Lbl_CurrentCode2.TabIndex = 1;
            this.Lbl_CurrentCode2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lbl_CurrentCode4
            // 
            this.Lbl_CurrentCode4.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_CurrentCode4.Font = new System.Drawing.Font("微软雅黑", 23F, System.Drawing.FontStyle.Bold);
            this.Lbl_CurrentCode4.ForeColor = System.Drawing.Color.White;
            this.Lbl_CurrentCode4.Location = new System.Drawing.Point(218, 7);
            this.Lbl_CurrentCode4.Name = "Lbl_CurrentCode4";
            this.Lbl_CurrentCode4.Size = new System.Drawing.Size(55, 55);
            this.Lbl_CurrentCode4.TabIndex = 3;
            this.Lbl_CurrentCode4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Pnl_CurrentCode2
            // 
            this.Pnl_CurrentCode2.Controls.Add(this.PK_Code);
            this.Pnl_CurrentCode2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_CurrentCode2.Location = new System.Drawing.Point(0, 35);
            this.Pnl_CurrentCode2.Name = "Pnl_CurrentCode2";
            this.Pnl_CurrentCode2.Size = new System.Drawing.Size(353, 71);
            this.Pnl_CurrentCode2.TabIndex = 11;
            // 
            // PK_Code
            // 
            this.PK_Code.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PK_Code.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.PK_Code.Location = new System.Drawing.Point(0, 0);
            this.PK_Code.Name = "PK_Code";
            this.PK_Code.Size = new System.Drawing.Size(353, 71);
            this.PK_Code.TabIndex = 0;
            // 
            // Pnl_CurrentExpectTop
            // 
            this.Pnl_CurrentExpectTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pnl_CurrentExpectTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_CurrentExpectTop.Controls.Add(this.Lbl_CurrentExpect);
            this.Pnl_CurrentExpectTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_CurrentExpectTop.Location = new System.Drawing.Point(0, 0);
            this.Pnl_CurrentExpectTop.Name = "Pnl_CurrentExpectTop";
            this.Pnl_CurrentExpectTop.Size = new System.Drawing.Size(353, 35);
            this.Pnl_CurrentExpectTop.TabIndex = 9;
            // 
            // Lbl_CurrentExpect
            // 
            this.Lbl_CurrentExpect.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_CurrentExpect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Lbl_CurrentExpect.Location = new System.Drawing.Point(0, 0);
            this.Lbl_CurrentExpect.Name = "Lbl_CurrentExpect";
            this.Lbl_CurrentExpect.Size = new System.Drawing.Size(351, 33);
            this.Lbl_CurrentExpect.TabIndex = 2;
            this.Lbl_CurrentExpect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Ckb_AutomationRun
            // 
            this.Ckb_AutomationRun.Appearance = System.Windows.Forms.Appearance.Button;
            this.Ckb_AutomationRun.AutoSize = true;
            this.Ckb_AutomationRun.FlatAppearance.BorderSize = 0;
            this.Ckb_AutomationRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ckb_AutomationRun.Location = new System.Drawing.Point(-1000, 16);
            this.Ckb_AutomationRun.Name = "Ckb_AutomationRun";
            this.Ckb_AutomationRun.Size = new System.Drawing.Size(6, 6);
            this.Ckb_AutomationRun.TabIndex = 6;
            this.Ckb_AutomationRun.UseVisualStyleBackColor = true;
            // 
            // Pnl_NextExpect
            // 
            this.Pnl_NextExpect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_NextExpect.Controls.Add(this.Pnl_NextExpectTop);
            this.Pnl_NextExpect.Controls.Add(this.Lbl_NextTime);
            this.Pnl_NextExpect.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_NextExpect.Location = new System.Drawing.Point(0, 0);
            this.Pnl_NextExpect.Name = "Pnl_NextExpect";
            this.Pnl_NextExpect.Size = new System.Drawing.Size(225, 108);
            this.Pnl_NextExpect.TabIndex = 10;
            // 
            // Pnl_NextExpectTop
            // 
            this.Pnl_NextExpectTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pnl_NextExpectTop.Controls.Add(this.Lbl_NextExpect);
            this.Pnl_NextExpectTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_NextExpectTop.Location = new System.Drawing.Point(0, 0);
            this.Pnl_NextExpectTop.Name = "Pnl_NextExpectTop";
            this.Pnl_NextExpectTop.Size = new System.Drawing.Size(223, 35);
            this.Pnl_NextExpectTop.TabIndex = 9;
            // 
            // Lbl_NextExpect
            // 
            this.Lbl_NextExpect.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_NextExpect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lbl_NextExpect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Lbl_NextExpect.Location = new System.Drawing.Point(0, 0);
            this.Lbl_NextExpect.Name = "Lbl_NextExpect";
            this.Lbl_NextExpect.Size = new System.Drawing.Size(223, 35);
            this.Lbl_NextExpect.TabIndex = 2;
            this.Lbl_NextExpect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lbl_NextTime
            // 
            this.Lbl_NextTime.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_NextTime.Font = new System.Drawing.Font("微软雅黑", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lbl_NextTime.ForeColor = System.Drawing.SystemColors.Window;
            this.Lbl_NextTime.Location = new System.Drawing.Point(6, 41);
            this.Lbl_NextTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lbl_NextTime.Name = "Lbl_NextTime";
            this.Lbl_NextTime.Size = new System.Drawing.Size(210, 57);
            this.Lbl_NextTime.TabIndex = 8;
            this.Lbl_NextTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Nic_Hint
            // 
            this.Nic_Hint.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.Nic_Hint.ContextMenuStrip = this.Cms_Menu;
            this.Nic_Hint.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Nic_Hint_MouseDoubleClick);
            // 
            // Cms_Menu
            // 
            this.Cms_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tsm_Vis,
            this.toolStripSeparator1,
            this.Tsm_Colse});
            this.Cms_Menu.Name = "Cms_Menu";
            this.Cms_Menu.Size = new System.Drawing.Size(137, 54);
            // 
            // Tsm_Vis
            // 
            this.Tsm_Vis.Name = "Tsm_Vis";
            this.Tsm_Vis.Size = new System.Drawing.Size(136, 22);
            this.Tsm_Vis.Text = "显示主窗体";
            this.Tsm_Vis.Click += new System.EventHandler(this.Tsm_Vis_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            this.toolStripSeparator1.Visible = false;
            // 
            // Tsm_Colse
            // 
            this.Tsm_Colse.Name = "Tsm_Colse";
            this.Tsm_Colse.Size = new System.Drawing.Size(136, 22);
            this.Tsm_Colse.Text = "退出软件";
            this.Tsm_Colse.Click += new System.EventHandler(this.Tsm_Colse_Click);
            // 
            // Stp_Hint
            // 
            this.Stp_Hint.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Stp_Hint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tsp_PeopleKey,
            this.Tsp_PeopleValue,
            this.Tsp_LoginKey,
            this.Tsp_LoginValue,
            this.Tsp_QQKey,
            this.Tsp_QQValue,
            this.Tsp_QQGroupKey,
            this.Tsp_QQGroupValue,
            this.Tsp_HintKey,
            this.Tsp_HintValue});
            this.Stp_Hint.Location = new System.Drawing.Point(0, 721);
            this.Stp_Hint.Name = "Stp_Hint";
            this.Stp_Hint.Size = new System.Drawing.Size(1234, 26);
            this.Stp_Hint.TabIndex = 74;
            // 
            // Tsp_PeopleKey
            // 
            this.Tsp_PeopleKey.Name = "Tsp_PeopleKey";
            this.Tsp_PeopleKey.Size = new System.Drawing.Size(68, 21);
            this.Tsp_PeopleKey.Text = "在线人数：";
            // 
            // Tsp_PeopleValue
            // 
            this.Tsp_PeopleValue.Name = "Tsp_PeopleValue";
            this.Tsp_PeopleValue.Size = new System.Drawing.Size(15, 21);
            this.Tsp_PeopleValue.Text = "0";
            // 
            // Tsp_LoginKey
            // 
            this.Tsp_LoginKey.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.Tsp_LoginKey.Name = "Tsp_LoginKey";
            this.Tsp_LoginKey.Size = new System.Drawing.Size(48, 21);
            this.Tsp_LoginKey.Text = "用户：";
            // 
            // Tsp_LoginValue
            // 
            this.Tsp_LoginValue.Name = "Tsp_LoginValue";
            this.Tsp_LoginValue.Size = new System.Drawing.Size(44, 21);
            this.Tsp_LoginValue.Text = "未登录";
            // 
            // Tsp_QQKey
            // 
            this.Tsp_QQKey.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.Tsp_QQKey.Name = "Tsp_QQKey";
            this.Tsp_QQKey.Size = new System.Drawing.Size(68, 21);
            this.Tsp_QQKey.Text = "客服QQ：";
            this.Tsp_QQKey.Visible = false;
            // 
            // Tsp_QQValue
            // 
            this.Tsp_QQValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.Tsp_QQValue.Name = "Tsp_QQValue";
            this.Tsp_QQValue.Size = new System.Drawing.Size(0, 21);
            this.Tsp_QQValue.Visible = false;
            // 
            // Tsp_QQGroupKey
            // 
            this.Tsp_QQGroupKey.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.Tsp_QQGroupKey.Name = "Tsp_QQGroupKey";
            this.Tsp_QQGroupKey.Size = new System.Drawing.Size(84, 21);
            this.Tsp_QQGroupKey.Text = "用户讨论群：";
            this.Tsp_QQGroupKey.Visible = false;
            // 
            // Tsp_QQGroupValue
            // 
            this.Tsp_QQGroupValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.Tsp_QQGroupValue.Name = "Tsp_QQGroupValue";
            this.Tsp_QQGroupValue.Size = new System.Drawing.Size(0, 21);
            this.Tsp_QQGroupValue.Visible = false;
            // 
            // Tsp_HintKey
            // 
            this.Tsp_HintKey.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.Tsp_HintKey.Name = "Tsp_HintKey";
            this.Tsp_HintKey.Size = new System.Drawing.Size(48, 21);
            this.Tsp_HintKey.Text = "提示：";
            this.Tsp_HintKey.Visible = false;
            // 
            // Tsp_HintValue
            // 
            this.Tsp_HintValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.Tsp_HintValue.Name = "Tsp_HintValue";
            this.Tsp_HintValue.Size = new System.Drawing.Size(0, 21);
            this.Tsp_HintValue.Visible = false;
            // 
            // Lbl_Web
            // 
            this.Lbl_Web.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_Web.AutoSize = true;
            this.Lbl_Web.Location = new System.Drawing.Point(1020, 726);
            this.Lbl_Web.Name = "Lbl_Web";
            this.Lbl_Web.Size = new System.Drawing.Size(68, 17);
            this.Lbl_Web.TabIndex = 77;
            this.Lbl_Web.TabStop = true;
            this.Lbl_Web.Text = "官方网址：";
            this.Lbl_Web.Visible = false;
            this.Lbl_Web.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Lbl_Web_LinkClicked);
            // 
            // Err_Hint
            // 
            this.Err_Hint.ContainerControl = this;
            // 
            // Pnl_Info
            // 
            this.Pnl_Info.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_Info.Controls.Add(this.Pnl_InfoRight);
            this.Pnl_Info.Controls.Add(this.Pnl_Scroll);
            this.Pnl_Info.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pnl_Info.Location = new System.Drawing.Point(0, 110);
            this.Pnl_Info.Name = "Pnl_Info";
            this.Pnl_Info.Size = new System.Drawing.Size(1234, 35);
            this.Pnl_Info.TabIndex = 72;
            this.Pnl_Info.Visible = false;
            // 
            // Pnl_InfoRight
            // 
            this.Pnl_InfoRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_InfoRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_InfoRight.Location = new System.Drawing.Point(580, 0);
            this.Pnl_InfoRight.Name = "Pnl_InfoRight";
            this.Pnl_InfoRight.Size = new System.Drawing.Size(652, 33);
            this.Pnl_InfoRight.TabIndex = 194;
            // 
            // Pnl_Scroll
            // 
            this.Pnl_Scroll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_Scroll.Controls.Add(this.Sct_Notice);
            this.Pnl_Scroll.Controls.Add(this.Pnl_NoticeLeft);
            this.Pnl_Scroll.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_Scroll.Location = new System.Drawing.Point(0, 0);
            this.Pnl_Scroll.Name = "Pnl_Scroll";
            this.Pnl_Scroll.Size = new System.Drawing.Size(580, 33);
            this.Pnl_Scroll.TabIndex = 72;
            // 
            // Sct_Notice
            // 
            this.Sct_Notice.BackgroundBrush = null;
            this.Sct_Notice.BorderColor = System.Drawing.Color.Black;
            this.Sct_Notice.Cursor = System.Windows.Forms.Cursors.Default;
            this.Sct_Notice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Sct_Notice.ForegroundBrush = null;
            this.Sct_Notice.Location = new System.Drawing.Point(32, 0);
            this.Sct_Notice.Name = "Sct_Notice";
            this.Sct_Notice.ScrollDirection = IntelligentPlanning.CustomControls.ScrollDirection.RightToLeft;
            this.Sct_Notice.ScrollText = "Text";
            this.Sct_Notice.ShowBorder = false;
            this.Sct_Notice.Size = new System.Drawing.Size(546, 31);
            this.Sct_Notice.StopScrollOnMouseOver = false;
            this.Sct_Notice.TabIndex = 28;
            this.Sct_Notice.Text = "scrollingText1";
            this.Sct_Notice.TextScrollDistance = 2;
            this.Sct_Notice.TextScrollSpeed = 40;
            this.Sct_Notice.VerticleTextPosition = IntelligentPlanning.CustomControls.VerticleTextPosition.Center;
            // 
            // Pnl_NoticeLeft
            // 
            this.Pnl_NoticeLeft.Controls.Add(this.Pic_Notice);
            this.Pnl_NoticeLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_NoticeLeft.Location = new System.Drawing.Point(0, 0);
            this.Pnl_NoticeLeft.Name = "Pnl_NoticeLeft";
            this.Pnl_NoticeLeft.Size = new System.Drawing.Size(32, 31);
            this.Pnl_NoticeLeft.TabIndex = 27;
            // 
            // Pic_Notice
            // 
            this.Pic_Notice.Location = new System.Drawing.Point(3, 5);
            this.Pic_Notice.Name = "Pic_Notice";
            this.Pic_Notice.Size = new System.Drawing.Size(25, 25);
            this.Pic_Notice.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Pic_Notice.TabIndex = 25;
            this.Pic_Notice.TabStop = false;
            // 
            // AutoBetsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1234, 747);
            this.Controls.Add(this.Lbl_Web);
            this.Controls.Add(this.Pnl_Main);
            this.Controls.Add(this.Pnl_Info);
            this.Controls.Add(this.Pnl_Top);
            this.Controls.Add(this.Stp_Hint);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AutoBetsWindow";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoBetsWindow_FormClosing);
            this.Load += new System.EventHandler(this.AutoBetsWindow_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Bets.ResumeLayout(false);
            this.Pnl_Bets.PerformLayout();
            this.Tab_Main.ResumeLayout(false);
            this.Tap_PT.ResumeLayout(false);
            this.Pnl_Bets2.ResumeLayout(false);
            this.Tap_ZDBets.ResumeLayout(false);
            this.Pnl_Bets1.ResumeLayout(false);
            this.Pnl_BetsMain.ResumeLayout(false);
            this.Pnl_BetsLeft.ResumeLayout(false);
            this.Pnl_BetsInfoMain.ResumeLayout(false);
            this.Pnl_BetsRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Egv_PlanList)).EndInit();
            this.Pnl_PlanListBottom.ResumeLayout(false);
            this.Pnl_PlanListBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nm_DeleteExpect)).EndInit();
            this.Pnl_PlanListTop.ResumeLayout(false);
            this.Pnl_PlanListTop.PerformLayout();
            this.Pnl_BetsInfoTop.ResumeLayout(false);
            this.Pnl_BetsInfoTopMain.ResumeLayout(false);
            this.Pnl_BetsInfoTopRight.ResumeLayout(false);
            this.Pnl_BetsType.ResumeLayout(false);
            this.Pnl_BetsType.PerformLayout();
            this.Pnl_BetsInfoTopRight1.ResumeLayout(false);
            this.Pnl_BetsInfoTopLeft.ResumeLayout(false);
            this.Pnl_BetsInfoTop2.ResumeLayout(false);
            this.Pnl_BetsInfoTop2.PerformLayout();
            this.Pnl_BetsInfoTop2Left.ResumeLayout(false);
            this.Pnl_BetsInfoTop2Left.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nm_BetsTime)).EndInit();
            this.Pnl_BetsInfoTop1.ResumeLayout(false);
            this.Pnl_BetsInfoExpect.ResumeLayout(false);
            this.Pnl_BetsInfoExpect.PerformLayout();
            this.Pnl_BetsInfoMN.ResumeLayout(false);
            this.Pnl_BetsInfoMN.PerformLayout();
            this.Pnl_BetsInfoMNRight.ResumeLayout(false);
            this.Pnl_BetsInfoMNRight.PerformLayout();
            this.Tap_Scheme.ResumeLayout(false);
            this.Pnl_Scheme.ResumeLayout(false);
            this.Pnl_SchemeMain.ResumeLayout(false);
            this.Pnl_SchemeInfo.ResumeLayout(false);
            this.Pnl_SchemeTop2.ResumeLayout(false);
            this.Pnl_SchemeTop2.PerformLayout();
            this.Pnl_SchemeLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Egv_SchemeList)).EndInit();
            this.Pnl_SchemeBottom.ResumeLayout(false);
            this.Pnl_SchemeShare.ResumeLayout(false);
            this.Pnl_SchemeTop1.ResumeLayout(false);
            this.Pnl_SchemeTop1.PerformLayout();
            this.Tap_LSData.ResumeLayout(false);
            this.Pnl_LSData.ResumeLayout(false);
            this.Pnl_LSDataMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Egv_LSDataList)).EndInit();
            this.Pnl_LSDataTop.ResumeLayout(false);
            this.Pnl_LSDataRight.ResumeLayout(false);
            this.Pnl_LSDataRight.PerformLayout();
            this.Pnl_LSDataLeft.ResumeLayout(false);
            this.Pnl_LSDataTop1.ResumeLayout(false);
            this.Pnl_LSDataTop1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nm_LSBJExpect)).EndInit();
            this.Tap_TJData.ResumeLayout(false);
            this.Pnl_TJData.ResumeLayout(false);
            this.Pnl_TJDataMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Egv_TJDataList2)).EndInit();
            this.Pnl_TJDataTop2.ResumeLayout(false);
            this.Pnl_TJDataFind.ResumeLayout(false);
            this.Pnl_TJDataFind.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nm_TJFindXS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Egv_TJDataList1)).EndInit();
            this.Pnl_TJDataTop.ResumeLayout(false);
            this.Pnl_TJRight2.ResumeLayout(false);
            this.Pnl_TJRight2.PerformLayout();
            this.Pnl_TJRight1.ResumeLayout(false);
            this.Pnl_TJDataTop1.ResumeLayout(false);
            this.Pnl_TJDataTop1.PerformLayout();
            this.Tap_ZBJ.ResumeLayout(false);
            this.Tap_TrendView.ResumeLayout(false);
            this.Tap_BTCount.ResumeLayout(false);
            this.Tap_BTFN.ResumeLayout(false);
            this.Pnl_BTFN.ResumeLayout(false);
            this.Pnl_BTFNMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Egv_BTFNTimesList)).EndInit();
            this.Pnl_TimesBottom.ResumeLayout(false);
            this.Pnl_TimesBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nm_BTFNEdit)).EndInit();
            this.Pnl_BTFNList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Egv_BTFNMain)).EndInit();
            this.Pnl_FNBottom.ResumeLayout(false);
            this.Tap_TBCount.ResumeLayout(false);
            this.Tap_HJFG.ResumeLayout(false);
            this.Tap_ShrinkEX.ResumeLayout(false);
            this.Tap_ShrinkSX.ResumeLayout(false);
            this.Tap_Setting.ResumeLayout(false);
            this.Pnl_Setting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Egv_ShowTapList)).EndInit();
            this.Tap_CDCount.ResumeLayout(false);
            this.Pnl_CDCount.ResumeLayout(false);
            this.Pnl_OpenData.ResumeLayout(false);
            this.Pnl_DataMain.ResumeLayout(false);
            this.Pnl_DataBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Egv_DataList)).EndInit();
            this.Pnl_RrfreshPT.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Egv_PTLineList)).EndInit();
            this.Pnl_PTRefresh.ResumeLayout(false);
            this.Pnl_DataTop2.ResumeLayout(false);
            this.Pnl_DataBottom1.ResumeLayout(false);
            this.Pnl_UserLogin2.ResumeLayout(false);
            this.Pnl_UserLogin2.PerformLayout();
            this.Pnl_UserLogin1.ResumeLayout(false);
            this.Pnl_UserLogin1.PerformLayout();
            this.Pnl_Top.ResumeLayout(false);
            this.Pnl_GG.ResumeLayout(false);
            this.Pnl_CurrentExpect1.ResumeLayout(false);
            this.Pnl_CurrentCode3.ResumeLayout(false);
            this.Pnl_CurrentCode4.ResumeLayout(false);
            this.Pnl_Notice.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Egv_NoticeList)).EndInit();
            this.Pnl_LTUserInfo.ResumeLayout(false);
            this.Pnl_AppName.ResumeLayout(false);
            this.Pnl_AppName.PerformLayout();
            this.Pnl_LTUserInfoTop.ResumeLayout(false);
            this.Pnl_LTUserInfoTop.PerformLayout();
            this.Pnl_CurrentExpect.ResumeLayout(false);
            this.Pnl_CurrentExpect.PerformLayout();
            this.Pnl_CurrentCode1.ResumeLayout(false);
            this.Pnl_CurrentCode2.ResumeLayout(false);
            this.Pnl_CurrentExpectTop.ResumeLayout(false);
            this.Pnl_NextExpect.ResumeLayout(false);
            this.Pnl_NextExpectTop.ResumeLayout(false);
            this.Cms_Menu.ResumeLayout(false);
            this.Stp_Hint.ResumeLayout(false);
            this.Stp_Hint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Err_Hint)).EndInit();
            this.Pnl_Info.ResumeLayout(false);
            this.Pnl_Scroll.ResumeLayout(false);
            this.Pnl_NoticeLeft.ResumeLayout(false);
            this.Pnl_NoticeLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Notice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Lbl_Web_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CommFunc.OpenWeb(AppInfo.Account.Configuration.WebUrl);
        }

        public void LoadBetsTime()
        {
            foreach (string str in this.BetsDic.Keys)
            {
                ConfigurationStatus.AutoBets bets = this.BetsDic[str];
                if (bets.PlanRun)
                {
                    bets.StartBets = true;
                    bets.IsSleepTime = true;
                    bets.ShareBetsInfo.SendPlanList.Clear();
                    bets.ShareBetsInfo.FollowPlanList.Clear();
                    bets.ShareBetsInfo.FollowErrorIndexList.Clear();
                    bets.ShareBetsInfo.FollowYes = false;
                }
            }
        }

        private void LoadBTFNList()
        {
            List<int> pType = new List<int> { 1 };
            List<string> pText = new List<string> { "方案名称" };
            List<int> pWidth = new List<int> { 10 };
            List<bool> pRead = new List<bool> { true };
            List<bool> pVis = new List<bool> { true };
            this.Egv_BTFNMain.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_BTFNMain.MultiSelect = false;
            this.Egv_BTFNMain.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_BTFNMain, 9);
            this.Egv_BTFNMain.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Egv_BTFNMain.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_BTFNList_CellValueNeeded);
            for (int i = 0; i < this.Egv_BTFNMain.ColumnCount; i++)
            {
                this.Egv_BTFNMain.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.LoadBTFNListData(BTFNPath);
        }

        private int LoadBTFNListData(string path)
        {
            int num = 0;
            if (Directory.Exists(path))
            {
                DirectoryInfo info = new DirectoryInfo(path);
                foreach (FileInfo info2 in info.GetFiles("*.txt"))
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(info2.FullName);
                    ConfigurationStatus.GJBTScheme bTFNByFileValue = ConfigurationStatus.GJBTScheme.GetBTFNByFileValue(CommFunc.ReadTextFileToStr(info2.FullName));
                    if (bTFNByFileValue != null)
                    {
                        AppInfo.BTFNDic[fileNameWithoutExtension] = bTFNByFileValue;
                        if (!AppInfo.BTFNList.Contains(fileNameWithoutExtension))
                        {
                            AppInfo.BTFNList.Add(fileNameWithoutExtension);
                        }
                        num++;
                    }
                }
            }
            return num;
        }

        private void LoadBTFNTimesList()
        {
            List<int> pType = new List<int> { 
                1,
                1,
                1,
                1,
                1
            };
            List<string> pText = new List<string> { 
                "局数",
                "倍数",
                "中后",
                "挂后",
                "取集"
            };
            List<int> pWidth = new List<int> { 
                70,
                40,
                60,
                60,
                60
            };
            List<bool> pRead = new List<bool> { 
                true,
                true,
                true,
                true,
                true
            };
            List<bool> pVis = new List<bool> { 
                true,
                true,
                true,
                true,
                false
            };
            this.Egv_BTFNTimesList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_BTFNTimesList.MultiSelect = false;
            this.Egv_BTFNTimesList.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_BTFNTimesList, 9);
            this.Egv_BTFNTimesList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.Egv_BTFNTimesList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_BTFNTimesList_CellValueNeeded);
            for (int i = 0; i < this.Egv_BTFNTimesList.ColumnCount; i++)
            {
                this.Egv_BTFNTimesList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void LoadCodeList()
        {
            List<int> pType = new List<int> { 
                1,
                1
            };
            List<string> pText = new List<string> { 
                "开奖期号",
                "号码"
            };
            List<int> pWidth = new List<int> { 
                100,
                140
            };
            List<bool> pRead = new List<bool> { 
                true,
                true
            };
            List<bool> pVis = new List<bool> { 
                true,
                true
            };
            this.Egv_DataList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_DataList.VirtualMode = true;
            this.Egv_DataList.MultiSelect = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_DataList, 9);
            this.Egv_DataList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_DataList_CellValueNeeded);
            for (int i = 0; i < this.Egv_DataList.ColumnCount; i++)
            {
                this.Egv_DataList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.Egv_DataList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void LoadConfiguration()
        {
            if (AppInfo.Account.Configuration.IsNotice)
            {
                this.LoadNoticeData();
            }
            else
            {
                string path = CommFunc.getApplicationDataPath();
                string cServerGGUrl = AppInfo.cServerGGUrl;
                foreach (string str3 in AppInfo.Account.Configuration.ImageLinkDic.Keys)
                {
                    CommFunc.GetWebImage(ref AppInfo.Account.Configuration.ImageList, cServerGGUrl, path, str3, true);
                }
                if (AppInfo.Account.GGImage != null)
                {
                    AppInfo.Account.Configuration.ImageList.Add(AppInfo.Account.GGImage);
                    AppInfo.Account.Configuration.ImageLinkDic["ImageLink"] = AppInfo.Account.Configuration.ImageLink;
                }
            }
            AppInfo.Account.RandomNum = SQLData.GetRandomNum();
            base.Invoke(AppInfo.LoadConfigurationLater);
        }

        private void LoadConfigurationLater()
        {
            if (AppInfo.Account.Configuration.IsNotice)
            {
                this.Pnl_Notice.Visible = true;
                this.RefreshNoticeList();
            }
            else if (AppInfo.Account.Configuration.ImageList.Count > 0)
            {
                this.Piw_Main.LoadImageList(AppInfo.Account.Configuration.ImageList, AppInfo.Account.Configuration.ImageLinkDic);
            }
            this.Tsp_PeopleValue.Text = AppInfo.Account.RandomNum;
        }

        private void LoadConfigurationThread()
        {
            this.mainThread.loadConfigurationThread = new Thread(new System.Threading.ThreadStart(AppInfo.LoadConfiguration.Invoke));
            this.mainThread.loadConfigurationThread.IsBackground = true;
            this.mainThread.loadConfigurationThread.Start();
        }

        private void LoadControl()
        {
            this.Lbl_LSLotteryValue.Text = this.Lbl_TJLotteryValue.Text = AppInfo.Current.Lottery.Name;
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                this.Egv_DataList.Columns[0].Width = 70;
            }
            else
            {
                this.Egv_DataList.Columns[0].Width = 100;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                List<string> list = new List<string> { 
                    "定码轮换",
                    "高级定码轮换",
                    "冷热温出号",
                    "外接计划",
                    "高级开某投某",
                    "龙虎开某投某",
                    "随机出号",
                    "不重复出号"
                };
                this.CHTypeList = list;
                List<string> collection = new List<string> { 
                    "固定取码",
                    "遗漏出号",
                    "开某投某"
                };
                this.CHTypeList.AddRange(collection);
                if (AppInfo.PTInfo.IsSkipLH)
                {
                    this.CHTypeList.Remove("龙虎开某投某");
                }
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                List<string> list3 = new List<string> { 
                    "高级定码轮换",
                    "外接计划"
                };
                this.CHTypeList = list3;
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                List<string> list4 = new List<string> { 
                    "高级定码轮换",
                    "外接计划"
                };
                this.CHTypeList = list4;
            }
            CommFunc.SetComboBoxList(this.Cbb_FNCHType, this.CHTypeList);
        }

        private void LoadDataGridView()
        {
            this.LoadShowTapList();
            this.LoadCodeList();
            this.LoadNoticeList();
            this.LoadPlanList1();
            this.LoadPTLineList();
            this.LoadSchemeList();
            this.LoadLSDataList();
            this.LoadTJDataList1();
            this.LoadTJDataList2();
            this.LoadBTFNList();
            this.LoadBTFNTimesList();
            this.RefreshBTFNControl(false);
        }

        public void LoadDataList(List<string> pList)
        {
            List<ConfigurationStatus.OpenData> list = new List<ConfigurationStatus.OpenData>();
            int count = pList.Count;
            AppInfo.PK10ExpectDic.Clear();
            for (int i = 0; i < count; i++)
            {
                if (pList[i] != "")
                {
                    string pData = pList[i];
                    ConfigurationStatus.OpenData item = CommFunc.GetOpenData(pData, AppInfo.Current.Lottery.ID, ref AppInfo.PK10ExpectDic);
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
            }
            AppInfo.DataList = list;
            if (AppInfo.DataList.Count > 0)
            {
                this.FilterOpenData();
                this.RefreshNewData();
            }
            if (!AppInfo.IsPassApp)
            {
                this.Ckb_Login.Enabled = true;
            }
        }

        private void LoadLotteryInfo()
        {
            this.LoadLotteryNameInfo();
            this.LoadPlayNameList();
            this.LoadControl();
            this.LoadSchemeData(this.SchemePath, true);
            this.RefreshAllFNList();
            this.LoadTime();
            this.mainThread = new MainThread(AppInfo.Current.Lottery.ID, AppInfo.PTInfo);
        }

        private void LoadLotteryNameInfo()
        {
            string selectLotteryID = this.GetSelectLotteryID();
            AppInfo.Current.Lottery = AppInfo.Current.LotteryDic[selectLotteryID];
        }

        private void LoadLSDataList()
        {
            List<int> pType = new List<int> { 
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1
            };
            List<string> pText = new List<string> { 
                "方案名称",
                "内容",
                "目前连中",
                "上次连中",
                "目前连错",
                "上次开出",
                "今日未出",
                "昨日未出",
                "一周未出"
            };
            List<int> pWidth = new List<int> { 
                90,
                130,
                0x47,
                0x47,
                0x47,
                0x47,
                0x47,
                0x47,
                0x47,
                0x47
            };
            List<bool> pRead = new List<bool> { 
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true
            };
            List<bool> pVis = new List<bool> { 
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true
            };
            this.Egv_LSDataList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_LSDataList.MultiSelect = false;
            this.Egv_LSDataList.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_LSDataList, 9);
            this.Egv_LSDataList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Egv_LSDataList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_LSDataList_CellValueNeeded);
            List<string> list6 = new List<string> { 
                "方案名称",
                "下期投注内容",
                "目前连中N轮",
                "上次连中N轮",
                "目前连错N轮",
                "上次第N轮开出",
                "今日最长N轮未出",
                "昨日最长N轮未出",
                "一周最长N轮未出",
                "一月最长N轮未出"
            };
            for (int i = 0; i < this.Egv_LSDataList.ColumnCount; i++)
            {
                this.Egv_LSDataList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.Egv_LSDataList.Columns[i].HeaderCell.ToolTipText = list6[i];
            }
        }

        private void LoadMainApp()
        {
            AppInfo.RefreshList = new ConfigurationStatus.RefreshListDelegate(this.RefreshData);
            AppInfo.LoadStart = new ConfigurationStatus.LoadDelegate(this.ThreadStart);
            AppInfo.LoadEnd = new ConfigurationStatus.LoadDelegate(this.ThreadEnd);
            AppInfo.BetsMain = new ConfigurationStatus.BetsMainDelegate(this.BetsMain1);
            AppInfo.BetsRefresh = new ConfigurationStatus.BetsRefreshDelegate(this.RefreshControl);
            AppInfo.BankRefresh = new ConfigurationStatus.BankRefreshDelegate(this.RefreshBank);
            AppInfo.BankControlRefresh = new ConfigurationStatus.BankRefreshDelegate(this.RefreshBankControl);
            AppInfo.LoginMain = new ConfigurationStatus.LoginMainDelegate(this.SwitchNextLine);
            AppInfo.LoginVerify = new ConfigurationStatus.LoadDelegate(this.LoginVerify);
            AppInfo.LoginIPVerify = new ConfigurationStatus.LoadDelegate(this.LoginIPVerify);
            AppInfo.LoginPTLottery = new ConfigurationStatus.LoginLotteryDelegate(this.LoginPTLottery);
            AppInfo.AnalysisVerifyCode = new ConfigurationStatus.AnalysisVerifyCodeDelegate(this.AnalysisVerifyCode);
            AppInfo.RefreshVerifyCode = new ConfigurationStatus.LoadDelegate(this.RefreshVerifyCode);
            AppInfo.RemoveLoginLock = new ConfigurationStatus.RemoveLoginLockDelegate(this.RemoveLoginLock);
            AppInfo.GetHtmlDocument = new ConfigurationStatus.GetHtmlDocumentDelegate(this.GetLoginDocument);
            AppInfo.CheckPTLine = new ConfigurationStatus.CheckPTLineDelegate(this.CheckPTLine);
            AppInfo.WebData = new ConfigurationStatus.WebDataDelegate(this.WebData);
            AppInfo.CloseApp = new ConfigurationStatus.CloseAppDelegate(this.CloseApp);
            AppInfo.RefreshUserMain = new ConfigurationStatus.RefreshUserMainDelegate(this.RefreshUserMain);
            AppInfo.RefreshLSData = new ConfigurationStatus.RefreshLSDataDelegate(this.RefreshLSDataMain);
            AppInfo.RefreshLSDataLater = new ConfigurationStatus.RefreshLSDataLaterDelegate(this.RefreshLSDataLater);
            AppInfo.RefreshTJData = new ConfigurationStatus.RefreshTJDataDelegate(this.RefreshTJDataMain);
            AppInfo.RefreshTJDataLater = new ConfigurationStatus.RefreshTJDataLaterDelegate(this.RefreshTJDataLater);
            AppInfo.GetLoginUrl = new ConfigurationStatus.GetLoginUrlDelegate(this.GetLoginUrl);
            AppInfo.PTIndexMain = new ConfigurationStatus.PTIndexMainDelegate(this.PTIndexMain);
            AppInfo.LoadConfiguration = new ConfigurationStatus.LoadConfigurationDelegate(this.LoadConfiguration);
            AppInfo.LoadConfigurationLater = new ConfigurationStatus.LoadConfigurationLaterDelegate(this.LoadConfigurationLater);
            AppInfo.RefreshLoginHint = new ConfigurationStatus.RefreshLoginHintDelegate(this.RefreshLoginHint);
            AppInfo.BTImport = new ConfigurationStatus.BTImportDelegate(this.BTImport);
        }

        private void LoadMainControl()
        {
            TabPage page;
            List<CheckBox> list4 = new List<CheckBox> {
                this.Ckb_PlanShowHide,
                this.Ckb_BetsSort,
                this.Ckb_RrfreshPTLine,
                this.Ckb_AddLine,
                this.Ckb_DeleteLine,
                this.Ckb_RefreshUser,
                this.Ckb_Login,
                this.Ckb_ClearBetsList,
                this.Ckb_AddScheme,
                this.Ckb_CopyScheme,
                this.Ckb_DeleteScheme,
                this.Ckb_EditTimesPlan,
                this.Ckb_EditScheme,
                this.Ckb_SaveScheme,
                this.Ckb_CancelScheme,
                this.Ckb_LSStop,
                this.Ckb_TJStop,
                this.Ckb_AutoSizeTJ,
                this.Ckb_TJFindXS,
                this.Ckb_AddBTFN,
                this.Ckb_DeleteBTFN,
                this.Ckb_BTFNEdit,
                this.Ckb_SaveTimes,
                this.Ckb_TBCount,
                this.Ckb_AddTimes,
                this.Ckb_EditTimes,
                this.Ckb_DeleteTimes,
                this.Ckb_ClearTimes,
                this.Ckb_AppName,
                this.Ckb_PWPaste,
                this.Ckb_PWClear,
                this.Ckb_ShowHideUser,
                this.Ckb_ImportScheme,
                this.Ckb_ExportScheme,
                this.Ckb_ClearScheme,
                this.Ckb_ShareSchemeManage,
                this.Ckb_ShareScheme,
                this.Ckb_ShareBetsManage
            };
            this.CheckBoxList = list4;
            CommFunc.SetCheckBoxFormatFlat(this.CheckBoxList);
            List<Button> list5 = new List<Button> {
                this.Btn_ViewTop,
                this.Btn_TJTop
            };
            this.ButtonList = list5;
            CommFunc.SetButtonFormatFlat(this.ButtonList);
            this.StandardList = new List<CheckBox>();
            CommFunc.SetCheckBoxFormatStandard(this.StandardList);
            this.RadioButtonList = new List<RadioButton>();
            CommFunc.SetRadioButtonFormat(this.RadioButtonList);
            List<Label> list6 = new List<Label> {
                this.Lbl_BetsExpectValue,
                this.Lbl_IDValue,
                this.Lbl_BankBalanceValue,
                this.Lbl_CurrentExpect,
                this.Lbl_NextExpect,
                this.Lbl_BetsGainPlanValue,
                this.Lbl_MNBetsGainPlanValue,
                this.Lbl_BetsMoneyPlanValue,
                this.Lbl_MNBetsMoneyPlanValue,
                this.Lbl_LSRefreshHint,
                this.Lbl_TJRefreshHint,
                this.Lbl_BetsCountValue,
                this.Lbl_LZMaxValue,
                this.Lbl_LGMaxValue,
                this.Lbl_ZQLValue,
                this.Lbl_TJPlayValue,
                this.Lbl_LSLotteryValue,
                this.Lbl_TJLotteryValue,
                this.Lbl_LSPlayValue,
                this.Lbl_IDHint,
                this.Lbl_CurrentExpect1
            };
            this.LabelList = list6;
            CommFunc.SetLabelFormat(this.LabelList);
            List<WebBrowser> list7 = new List<WebBrowser> {
                this.Web_Login
            };
            this.WebBrowserList = list7;
            CommFunc.SetWebBrowserFormat(this.WebBrowserList);
            List<ComboBox> list8 = new List<ComboBox> {
                this.Cbb_LoginPT,
                this.Cbb_Lottery,
                this.Cbb_LSFN,
                this.Cbb_LSBJType,
                this.Cbb_BetsEndType,
                this.Cbb_TJFN,
                this.Cbb_TJPrize,
                this.Cbb_BTFNEdit
            };
            this.ComboBoxList = list8;
            CommFunc.SetComboBoxFormat(this.ComboBoxList, 8);
            this.Tsp_LoginValue.ForeColor = this.Tsp_PeopleValue.ForeColor = AppInfo.appForeColor;
            this.Tsp_QQValue.ForeColor = AppInfo.redForeColor;
            this.Tsp_QQGroupValue.ForeColor = AppInfo.redForeColor;
            this.Tsp_HintValue.ForeColor = AppInfo.redForeColor;
            this.Lbl_LoginHint.ForeColor = AppInfo.redForeColor;
            this.Pnl_CurrentExpect1.BackColor = AppInfo.appBackColor;
            this.Lbl_CurrentExpect1.ForeColor = this.Lbl_NextExpect1.ForeColor = AppInfo.whiteColor;
            this.Lbl_NextTime1.ForeColor = AppInfo.orangeForeColor;
            this.Lbl_AppHint.ForeColor = AppInfo.redForeColor;
            if (AppInfo.Account.Configuration.QQ != "")
            {
                this.Tsp_QQValue.Text = AppInfo.Account.Configuration.QQ;
                this.Tsp_QQKey.Visible = this.Tsp_QQValue.Visible = true;
                if (AppInfo.Account.Configuration.QQHint != "")
                {
                    this.Tsp_QQKey.Text = AppInfo.Account.Configuration.QQHint + "：";
                }
            }
            if (AppInfo.Account.Configuration.QQGroup != "")
            {
                this.Tsp_QQGroupValue.Text = AppInfo.Account.Configuration.QQGroup;
                this.Tsp_QQGroupKey.Visible = this.Tsp_QQGroupValue.Visible = true;
            }
            if (AppInfo.Account.Configuration.WebUrl != "")
            {
                if (AppInfo.Account.Configuration.WebUrl.Contains("："))
                {
                    this.Lbl_Web.Text = AppInfo.Account.Configuration.WebUrl;
                }
                else
                {
                    this.Lbl_Web.Text = this.Lbl_Web.Text + AppInfo.Account.Configuration.WebUrl;
                }
                this.Lbl_Web.Visible = true;
                int x = (base.Width - this.Lbl_Web.Width) - 0x19;
                this.Lbl_Web.Location = new Point(x, this.Lbl_Web.Location.Y);
            }
            else if (AppInfo.App != ConfigurationStatus.AppType.DEBUG)
            {
                this.Tap_PT.Parent = null;
            }
            if (AppInfo.Account.Configuration.BetsHint != "")
            {
                this.Lbl_BetsHint.ForeColor = AppInfo.redForeColor;
                this.Lbl_BetsHint.Text = AppInfo.Account.Configuration.BetsHint;
            }
            if (AppInfo.Account.Configuration.IsHideSetUp)
            {
                this.Pnl_CurrentExpect.Visible = false;
                this.Pnl_NextExpect.Visible = false;
                this.Pnl_CurrentExpect1.Visible = true;
                if (AppInfo.Account.Configuration.ScrollText == "")
                {
                    this.Pnl_LTUserInfo.Controls.Add(this.Egv_ShowTapList);
                    this.Pnl_LTUserInfo.Controls.SetChildIndex(this.Egv_ShowTapList, 0);
                    this.Pnl_Setting.Controls.Add(this.Pnl_LTUserInfo);
                }
            }
            if ((((AppInfo.App == ConfigurationStatus.AppType.JHC2GJ) || (AppInfo.App == ConfigurationStatus.AppType.HUIZGJ)) || ((AppInfo.App == ConfigurationStatus.AppType.HDYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.HDGJ))) || (AppInfo.App == ConfigurationStatus.AppType.HONDGJ))
            {
                if (AppInfo.Account.Configuration.ZBJUrl == "")
                {
                    this.Tap_ZBJ.Parent = null;
                }
            }
            else
            {
                this.Tap_ZBJ.Parent = null;
            }
            if (AppInfo.App != ConfigurationStatus.AppType.HUAYGJ)
            {
                this.Tap_CDCount.Parent = null;
            }
            int index = this.Tab_Main.TabPages.IndexOf(this.Tap_TJData) + 1;
            if (this.Tap_ZBJ.Parent != null)
            {
                index++;
            }
            if (AppInfo.Account.Configuration.MoreAppList.Count > 0)
            {
                foreach (ConfigurationStatus.MoreAppData data in AppInfo.Account.Configuration.MoreAppList)
                {
                    page = new TabPage {
                        Text = data.ViewName
                    };
                    MoreAppLine line = new MoreAppLine {
                        Dock = DockStyle.Top
                    };
                    line.LoadData(data);
                    page.Controls.Add(line);
                    if (AppInfo.Account.Configuration.Beautify)
                    {
                        List<Control> pControlList = new List<Control> {
                            page
                        };
                        CommFunc.SetControlBackColor(pControlList, AppInfo.beaBackColor);
                    }
                    this.Tab_Main.TabPages.Insert(index, page);
                }
            }
            if (AppInfo.Account.Configuration.DownloadLinkDic.Count > 0)
            {
                foreach (string str in AppInfo.Account.Configuration.DownloadLinkDic.Keys)
                {
                    page = new TabPage {
                        Text = str
                    };
                    WebBrowser browser = new WebBrowser {
                        Dock = DockStyle.Fill
                    };
                    List<WebBrowser> pWebBrowserList = new List<WebBrowser> {
                        browser
                    };
                    CommFunc.SetWebBrowserFormat(pWebBrowserList);
                    page.Controls.Add(browser);
                    this.Tab_Main.TabPages.Insert(index, page);
                    string urlString = AppInfo.Account.Configuration.DownloadLinkDic[str];
                    browser.Navigate(urlString);
                }
            }
            if (AppInfo.Account.Configuration.HideLSTJ)
            {
                this.Tap_LSData.Parent = (Control) (this.Tap_TJData.Parent = null);
            }
            if (AppInfo.Account.Configuration.ScrollText != "")
            {
                this.Pnl_Info.Visible = true;
                this.Pnl_LTUserInfo.Visible = false;
                this.Sct_Notice.ScrollText = AppInfo.Account.Configuration.ScrollText;
                this.Sct_Notice.ForeColor = AppInfo.appForeColor;
                this.Ckb_LeftInfo.Parent = this.Ckb_RrfreshPT.Parent = this.Ckb_Data.Parent = this.Ckb_PlaySound.Parent = this.Ckb_OpenHint.Parent = this.Ckb_CloseMin.Parent = this.Pnl_InfoRight;
                this.Ckb_LeftInfo.Location = new Point(5, 5);
                this.Ckb_RrfreshPT.Location = new Point(0x56, 5);
                this.Ckb_Data.Location = new Point(0xa7, 5);
                this.Ckb_PlaySound.Location = new Point(0xf8, 5);
                this.Ckb_OpenHint.Location = new Point(0x149, 5);
                this.Ckb_CloseMin.Location = new Point(410, 5);
            }
            if (AppInfo.Account.Configuration.FixAppText)
            {
                this.Pnl_AppName.Visible = false;
            }
            if (AppInfo.Account.Configuration.WebHint != "")
            {
                this.Tap_PT.Text = AppInfo.Account.Configuration.WebHint;
            }
            if (AppInfo.Account.Configuration.IsHideMNBets)
            {
                this.Pnl_BetsInfoMN.Visible = false;
                this.Lbl_MNBetsGainPlanKey.Visible = this.Lbl_MNBetsGainPlanValue.Visible = this.Lbl_MNBetsMoneyPlanKey.Visible = this.Lbl_MNBetsMoneyPlanValue.Visible = false;
            }
            this.BeautifyInterface();
            this.LoadWebLoginIndex();
            CommFunc.CreateDirectory(VerifyCodePath);
            if (!AppInfo.IsViewPeople)
            {
                this.Tsp_PeopleKey.Visible = this.Tsp_PeopleValue.Visible = false;
            }
            if (AppInfo.App == ConfigurationStatus.AppType.HENDGJ)
            {
                this.Lbl_LoginPT.Visible = this.Cbb_LoginPT.Visible = false;
            }
            if (AppInfo.IsCXG)
            {
                this.Pnl_SchemeBottom.Visible = true;
            }
            for (int i = 1; i <= this.Tab_Main.TabPages.Count; i++)
            {
                TabPage item = this.Tab_Main.TabPages[i - 1];
                string text = item.Text;
                if ((item == this.Tap_Setting) && (((AppInfo.App == ConfigurationStatus.AppType.SKYGJ) || (AppInfo.App == ConfigurationStatus.AppType.JFYLGJ)) || (AppInfo.App == ConfigurationStatus.AppType.TCYLGJ)))
                {
                    text = "辅助功能";
                }
                item.Tag = text;
                this.MainPageList.Add(item);
            }
            List<TabPage> collection = new List<TabPage> {
                this.Tap_TrendView,
                this.Tap_BTCount,
                this.Tap_BTFN,
                this.Tap_TBCount,
                this.Tap_HJFG,
                this.Tap_ShrinkEX,
                this.Tap_ShrinkSX,
                this.Tap_Setting
            };
            this.SettingPageList.AddRange(collection);
        }

        private void LoadNoticeData()
        {
            this.NoticeList.Clear();
            this.NoticeDic.Clear();
            List<string> list = CommFunc.SplitString(HttpHelper.GetWebData("YXZXGJNotice", ""), "[", -1);
            foreach (string str2 in list)
            {
                if (str2 != "")
                {
                    List<string> list2 = CommFunc.SplitString(str2, "]", -1);
                    string item = list2[0];
                    string str = list2[1];
                    if (Strings.Left(str, 2) == "\r\n")
                    {
                        str = str.Substring(2);
                    }
                    if (Strings.Right(str, 2) == "\r\n")
                    {
                        str = str.Substring(0, str.Length - 2);
                    }
                    this.NoticeList.Add(item);
                    this.NoticeDic[item] = str;
                }
            }
        }

        private void LoadNoticeList()
        {
            List<int> pType = new List<int> { 1 };
            List<string> pText = new List<string> { "系统公告" };
            List<int> pWidth = new List<int> { 100 };
            List<bool> pRead = new List<bool> { true };
            List<bool> pVis = new List<bool> { true };
            this.Egv_NoticeList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_NoticeList.VirtualMode = true;
            this.Egv_NoticeList.MultiSelect = false;
            CommFunc.SetExpandGirdViewFormat(this.Egv_NoticeList, 9);
            this.Egv_NoticeList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_NoticeList_CellValueNeeded);
            for (int i = 0; i < this.Egv_NoticeList.ColumnCount; i++)
            {
                this.Egv_NoticeList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.Egv_NoticeList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void LoadPlanList1()
        {
            List<int> pType = new List<int> { 
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                2,
                1,
                1,
                1
            };
            List<string> pText = new List<string> { 
                "投注时间",
                "投注彩种",
                "期数",
                "方案",
                "玩法",
                "注数",
                "倍数",
                "轮次",
                "金额",
                "盈亏",
                "方案盈亏",
                "连挂",
                "连中",
                "内容",
                "开奖号码",
                "中挂",
                "投注"
            };
            List<int> pWidth = new List<int> { 
                0x4b,
                90,
                110,
                70,
                130,
                0x2d,
                0x2d,
                0x2d,
                80,
                80,
                80,
                70,
                70,
                10,
                0x41,
                0x2d,
                0x2d
            };
            List<bool> pRead = new List<bool> { 
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true
            };
            List<bool> pVis = new List<bool> { 
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true
            };
            this.Egv_PlanList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_PlanList.MultiSelect = true;
            this.Egv_PlanList.VirtualMode = true;
            this.Egv_PlanList.ScrollBars = ScrollBars.Both;
            CommFunc.SetExpandGirdViewFormat(this.Egv_PlanList, 11);
            this.Egv_PlanList.RowsDefaultCellStyle.ForeColor = AppInfo.blackColor;
            this.Egv_PlanList.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Egv_PlanList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.Egv_PlanList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_BetsList_CellValueNeeded);
            for (int i = 0; i < this.Egv_PlanList.ColumnCount; i++)
            {
                this.Egv_PlanList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ((ImageTextColumn) this.Egv_PlanList.Columns[13]).ButtonClick += new DataGridViewCellMouseEventHandler(this.Egv_PlanList_ButtonClick);
        }

        private void LoadPlanListData()
        {
            if (AppInfo.Current.Lottery == null)
            {
                this.LoadLotteryNameInfo();
            }
            string betsPlanListPath = this.BetsPlanListPath;
            if (Directory.Exists(betsPlanListPath))
            {
                DirectoryInfo info = new DirectoryInfo(betsPlanListPath);
                foreach (FileInfo info2 in info.GetFiles("*.txt"))
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(info2.FullName);
                    if (!fileNameWithoutExtension.Contains("投注"))
                    {
                        break;
                    }
                    ConfigurationStatus.AutoBets bets = new ConfigurationStatus.AutoBets(fileNameWithoutExtension);
                    List<string> list = CommFunc.ReadTextFileToList(info2.FullName);
                    foreach (string str3 in list)
                    {
                        try
                        {
                            if (str3 != "")
                            {
                                ConfigurationStatus.SCPlan item = new ConfigurationStatus.SCPlan {
                                    LoadPlanListData = str3
                                };
                                if (item.LotteryName != null)
                                {
                                    bets.PlanList.Add(item);
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    this.BetsDic[fileNameWithoutExtension] = bets;
                }
            }
        }

        private void LoadPlayNameList()
        {
            AppInfo.PlayDic.Clear();
            XmlNode xmlPlayNode = AppInfo.Current.Lottery.XmlPlayNode;
            foreach (XmlNode node2 in xmlPlayNode)
            {
                string playName = CommFunc.GetAttributeString(node2, "Name", "");
                if ((!CommFunc.CheckPlayIsRX(playName) || !AppInfo.PTInfo.IsSkipRX) && (!CommFunc.CheckPlayIsLH(playName) || !AppInfo.PTInfo.IsSkipLH))
                {
                    List<ConfigurationStatus.PlayBase> list = new List<ConfigurationStatus.PlayBase>();
                    foreach (XmlNode node3 in node2.ChildNodes)
                    {
                        ConfigurationStatus.PlayBase item = new ConfigurationStatus.PlayBase {
                            PlayType = playName,
                            PlayName = CommFunc.GetAttributeString(node3, "Name", "")
                        };
                        if ((((item.PlayType != "定位胆") || (item.PlayName != "定位胆")) || (AppInfo.PTInfo != AppInfo.HDYLInfo)) && (((AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10) || ((item.PlayType != "猜前二") && (item.PlayType != "猜前三"))) || ((item.PlayName != "和值") || (AppInfo.PTInfo == AppInfo.OEYLInfo))))
                        {
                            item.CodeCount = CommFunc.GetAttributeInt(node3, "Code", 0);
                            item.Number = CommFunc.GetAttributeString(node3, "Number", "");
                            item.IndexList = CommFunc.SplitInt(CommFunc.GetAttributeString(node3, "Index", ""), ",");
                            list.Add(item);
                        }
                    }
                    AppInfo.PlayDic[playName] = list;
                }
            }
            if (((AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPSSC) && (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GP11X5)) && (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10))
            {
                AppInfo.CombinaDicPK10HZ.Clear();
                AppInfo.CombinaDicPK10HZ["和值2"] = CommFunc.ConvertStringList("3-19");
                AppInfo.CombinaDicPK10HZ["和值3"] = CommFunc.ConvertStringList("6-27");
            }
        }

        private void LoadPTLine(string name = "")
        {
            string pTLineFile = AppInfo.PTLineFile;
            string pLineString = "";
            if (File.Exists(pTLineFile))
            {
                pLineString = CommFunc.ReadTextFileToStr(pTLineFile);
            }
            if (pLineString == "")
            {
                pLineString = HttpHelper.GetWebData(AppInfo.cPTLineFile(""), "");
            }
            List<string> pList = CommFunc.AnalysisPTLine(ref this.PTLineDic, pLineString);
            if (this.PTName != "")
            {
                this.SavePTInfoByReg();
            }
            if (name == "")
            {
                CommFunc.SetComboBoxList(this.Cbb_LoginPT, pList);
                name = CommFunc.ReadRegString(this.RegConfigPath, "PTNameValue", "");
                if (this.SelectPT != "")
                {
                    name = this.SelectPT;
                    this.SelectPT = "";
                }
                if (!((name != "") && pList.Contains(name)))
                {
                    name = this.Cbb_LoginPT.Items[0].ToString();
                }
                CommFunc.SetComboBoxSelectedIndex(this.Cbb_LoginPT, name);
                this.PTName = name;
            }
            else
            {
                this.PTName = this.Cbb_LoginPT.Text;
            }
            this.SetPTInfoByReg();
            this.Cbb_Lottery.Items.Clear();
            List<string> pLotteryIDList = new List<string>();
            if (this.PTName == "361彩票")
            {
                AppInfo.PTInfo = AppInfo.CP361Info;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BDFFC",
                    "BD2FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.JDGJ)
                {
                    pLotteryIDList = new List<string> { 
                        "BD11X5",
                        "SD11X5",
                        "GD11X5",
                        "JX11X5"
                    };
                }
            }
            else if (this.PTName == "名人娱乐")
            {
                AppInfo.PTInfo = AppInfo.MRYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "FLBSSC",
                    "MRFFC",
                    "MR2FC",
                    "MR45C"
                };
                if (((AppInfo.App == ConfigurationStatus.AppType.LHLM) || (AppInfo.App == ConfigurationStatus.AppType.NRLM)) || (AppInfo.App == ConfigurationStatus.AppType.CSCGJ))
                {
                    List<string> collection = new List<string> { 
                        "MR11X5",
                        "SD11X5",
                        "GD11X5",
                        "JX11X5"
                    };
                    pLotteryIDList.AddRange(collection);
                }
                if ((AppInfo.App == ConfigurationStatus.AppType.LHLM) || (AppInfo.App == ConfigurationStatus.AppType.NRLM))
                {
                    List<string> list6 = new List<string> { 
                        "PK10",
                        "MRPK10"
                    };
                    pLotteryIDList.AddRange(list6);
                }
            }
            else if (this.PTName == "华人彩票")
            {
                AppInfo.PTInfo = AppInfo.HRCPInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HRFFC",
                    "HR2FC",
                    "HR45C",
                    "QJCTXFFC",
                    "FLBSSC"
                };
            }
            else if (this.PTName == "博猫平台")
            {
                AppInfo.PTInfo = AppInfo.BMYXInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "BM1FC",
                    "BM2FC",
                    "BM5FC"
                };
            }
            else if (this.PTName == "欧亿娱乐")
            {
                AppInfo.PTInfo = AppInfo.OEYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "LFDJSSC",
                    "BJSSC",
                    "TWSSC",
                    "TXFFC",
                    "QQFFC",
                    "JNDSSC",
                    "MD2FC",
                    "OEFFC",
                    "OE3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.OEGJ)
                {
                    List<string> list10 = new List<string> { 
                        "VRSSC",
                        "VRHXSSC",
                        "VR3FC",
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "PK10",
                        "VRPK10"
                    };
                    pLotteryIDList.AddRange(list10);
                }
            }
            else if (this.PTName == "梦之城")
            {
                AppInfo.PTInfo = AppInfo.MZCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "LFDJSSC",
                    "BJSSC",
                    "TWSSC",
                    "SSHCTXFFC",
                    "JNDSSC",
                    "MD2FC",
                    "MZCFFC",
                    "MZC3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.MZCGJ)
                {
                    List<string> list12 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list12);
                }
            }
            else if (this.PTName == "大唐彩票")
            {
                AppInfo.PTInfo = AppInfo.DTCPInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "FLBSSC",
                    "DTFFC",
                    "DT2FC"
                };
            }
            else if (this.PTName == "游艇会(UT8)")
            {
                AppInfo.PTInfo = AppInfo.UT8Info;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "UT8FFC",
                    "UT83FC",
                    "UT8HGSSC",
                    "UT8DJSSC",
                    "BLSFFC",
                    "XDLSSC",
                    "JZD15FC",
                    "LD2FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.UT8GJ)
                {
                    List<string> list15 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "JX11X5",
                        "AH11X5",
                        "SH11X5",
                        "XDL11X5",
                        "JZD11X5",
                        "LD11X5",
                        "PK10",
                        "XDLPK10",
                        "JZDPK10",
                        "LDPK10"
                    };
                    pLotteryIDList.AddRange(list15);
                }
            }
            else if (this.PTName == "M5彩票")
            {
                AppInfo.PTInfo = AppInfo.M5CPInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TWSSC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC",
                    "M5FFC",
                    "M53FC",
                    "M55FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.CXG3)
                {
                    List<string> list17 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "M511X5",
                        "PK10",
                        "VRPK10",
                        "VRKT"
                    };
                    pLotteryIDList.AddRange(list17);
                }
            }
            else if (this.PTName == "大发彩票")
            {
                AppInfo.PTInfo = AppInfo.DACPInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TWSSC",
                    "VRSSC",
                    "DAFFC",
                    "DA3FC"
                };
            }
            else if (this.PTName == "玩家世界")
            {
                AppInfo.PTInfo = AppInfo.WJSJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "JH15C",
                    "WHDJSSC",
                    "TWSSC",
                    "KLFFC",
                    "KL2FC"
                };
            }
            else if (this.PTName == "众赢娱乐")
            {
                AppInfo.PTInfo = AppInfo.UCYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TJSSC",
                    "XJSSC",
                    "BJSSC",
                    "UCHGSSC",
                    "JNDSSC",
                    "UCTWSSC",
                    "UCHL2FC",
                    "UCFFC",
                    "UC5FC",
                    "TXFFC",
                    "UCRDFFC",
                    "UCRD2FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.ZYIN)
                {
                    List<string> list21 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "SH11X5",
                        "UC3F11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list21);
                }
            }
            else if (this.PTName == "B6娱乐城")
            {
                AppInfo.PTInfo = AppInfo.B6YLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "B6YLFFC",
                    "B6YL3FC",
                    "B6YL5FC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "B6YL3F11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "拉菲娱乐")
            {
                AppInfo.PTInfo = AppInfo.LFYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TXFFC",
                    "QQFFC",
                    "HGSSC",
                    "LFHGSSC",
                    "MD2FC",
                    "LFDJSSC",
                    "TWSSC",
                    "BJSSC",
                    "JNDSSC",
                    "TJSSC",
                    "XJSSC",
                    "LFFFC",
                    "LF2FC",
                    "LF5FC"
                };
                if ((AppInfo.App == ConfigurationStatus.AppType.DEBUG) || (AppInfo.App == ConfigurationStatus.AppType.CSCGJ))
                {
                    List<string> list24 = new List<string> { 
                        "LF11X5",
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list24);
                }
            }
            else if (this.PTName == "万达娱乐")
            {
                AppInfo.PTInfo = AppInfo.WDYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TXFFC",
                    "LFHGSSC",
                    "LFDJSSC",
                    "MD2FC",
                    "TJSSC",
                    "XJSSC",
                    "BJSSC",
                    "WDYL5FC",
                    "WDYL2FC",
                    "WDYLFFC",
                    "TWSSC",
                    "QQFFC"
                };
                if ((AppInfo.App == ConfigurationStatus.AppType.WDCD) || (AppInfo.App == ConfigurationStatus.AppType.WDGJ))
                {
                    List<string> list26 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "JX11X5",
                        "WDYL11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list26);
                }
            }
            else if (this.PTName == "华宇娱乐")
            {
                AppInfo.PTInfo = AppInfo.HUAYInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TXFFC",
                    "LFHGSSC",
                    "LFDJSSC",
                    "MD2FC",
                    "YNSSC",
                    "TJSSC",
                    "XJSSC",
                    "BJSSC",
                    "HUAY5FC",
                    "HUAY2FC",
                    "HUAYFFC",
                    "GD11X5",
                    "SD11X5",
                    "JX11X5",
                    "HUAY11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "乐丰国际")
            {
                AppInfo.PTInfo = AppInfo.LFGJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HGSSC",
                    "WHDJSSC",
                    "TWSSC",
                    "LEFFFC",
                    "LEF2FC",
                    "LEF5FC"
                };
            }
            else if (this.PTName == "无限娱乐")
            {
                AppInfo.PTInfo = AppInfo.WXYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HGLTC",
                    "DJSSC",
                    "BJSSC",
                    "QQ15F",
                    "QQ30M",
                    "TWSSC",
                    "TXFFC",
                    "XJPSSC",
                    "JNDSSC",
                    "WXFFC",
                    "WX15F",
                    "WX3FC",
                    "WX5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.CSCGJ)
                {
                    List<string> list30 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "WX11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list30);
                }
            }
            else if (this.PTName == "利信娱乐")
            {
                AppInfo.PTInfo = AppInfo.LXYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "SESSC",
                    "HGSSC",
                    "BJSSC",
                    "TWSSC",
                    "LXFFC",
                    "LX3FC",
                    "LX5FC"
                };
            }
            else if (this.PTName == "BA娱乐")
            {
                AppInfo.PTInfo = AppInfo.BAYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "HGSSC",
                    "XJPSSC",
                    "DJSSC",
                    "TWSSC",
                    "BJSSC",
                    "NYFFC",
                    "NY3FC",
                    "NY5FC"
                };
            }
            else if (this.PTName == "四季娱乐")
            {
                AppInfo.PTInfo = AppInfo.SIJIInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "JLSSC",
                    "NMGSSC",
                    "HLJSSC",
                    "YNSSC",
                    "SIJIFFC",
                    "SIJI3FC",
                    "SIJI5FC",
                    "TXFFC",
                    "SIJITXYFC",
                    "SIJIFLBSSC",
                    "SIJIELSSSC",
                    "SIJIDJSSC",
                    "SIJIHGSSC",
                    "BJSSC",
                    "TWSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.SIJIGJ)
                {
                    List<string> list34 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "BJ11X5",
                        "HB11X5",
                        "LN11X5",
                        "HLJ11X5",
                        "JL11X5",
                        "GS11X5",
                        "QH11X5",
                        "HN11X5",
                        "JS11X5",
                        "HUB11X5",
                        "ZJ11X5",
                        "YN11X5",
                        "FJ11X5",
                        "SXR11X5",
                        "SXL11X5",
                        "GZ11X5",
                        "AH11X5",
                        "SH11X5",
                        "TJ11X5",
                        "GX11X5",
                        "NMG11X5",
                        "SIJIFF11X5",
                        "SIJI3F11X5",
                        "SIJI5F11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list34);
                }
            }
            else if (this.PTName == "易赢在线")
            {
                AppInfo.PTInfo = AppInfo.YYZXInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HGSSC",
                    "JNDSSC",
                    "TWSSC",
                    "PK10"
                };
            }
            else if (this.PTName == "久赢国际")
            {
                AppInfo.PTInfo = AppInfo.JYGJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HGSSC",
                    "BJSSC",
                    "TWSSC",
                    "XDLSSC",
                    "JYINFFC",
                    "JYIN3FC"
                };
            }
            else if (this.PTName == "红宝石")
            {
                AppInfo.PTInfo = AppInfo.HBSInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HGSSC"
                };
            }
            else if (this.PTName == "新宝3")
            {
                AppInfo.PTInfo = AppInfo.XB3Info;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HGSSC",
                    "QQFFC",
                    "GDFFC",
                    "XJPSSC",
                    "TWSSC",
                    "BDSSC",
                    "GGSSC",
                    "XBFFC",
                    "XB3FC",
                    "XB5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.DEBUG)
                {
                    List<string> list39 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SD11X5",
                        "AH11X5",
                        "SH11X5",
                        "PK10",
                        "XYFTPK10"
                    };
                    pLotteryIDList.AddRange(list39);
                }
            }
            else if (this.PTName == "K5娱乐")
            {
                AppInfo.PTInfo = AppInfo.K5YLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "K5FFC",
                    "K55FC",
                    "BJSSC",
                    "DJSSC",
                    "ELSSSC",
                    "XJPSSC",
                    "JNDSSC",
                    "TWSSC",
                    "XXLSSC"
                };
            }
            else if (this.PTName == "丰尚娱乐")
            {
                AppInfo.PTInfo = AppInfo.FSYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HGSSC",
                    "DPSSC",
                    "FSFFC",
                    "FS5FC"
                };
            }
            else if (this.PTName == "必火直营")
            {
                AppInfo.PTInfo = AppInfo.BHZYInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BHZYFFC",
                    "BHZY5FC",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "ELSSSC",
                    "XXLSSC",
                    "BHZYHGSSC",
                    "BHZYDJSSC",
                    "BHZYTXFFC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.XHGJ)
                {
                    List<string> list43 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SD11X5",
                        "AH11X5",
                        "SH11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list43);
                }
            }
            else if (this.PTName == "A6娱乐")
            {
                AppInfo.PTInfo = AppInfo.A6YLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "A6FFC",
                    "A65FC",
                    "BJSSC",
                    "HGSSC",
                    "XJPSSC",
                    "JNDSSC",
                    "TWSSC",
                    "ELSSSC",
                    "XXLSSC"
                };
            }
            else if (this.PTName == "易发彩票")
            {
                AppInfo.PTInfo = AppInfo.YIFAInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "YIFAFFC",
                    "YIFA5FC",
                    "JNDSSC",
                    "TWSSC",
                    "DJSSC",
                    "ELSSSC",
                    "XXLSSC"
                };
            }
            else if (this.PTName == "金狐娱乐")
            {
                AppInfo.PTInfo = AppInfo.JHYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "JHTXFFC",
                    "JHQQFFC",
                    "MDHG90M",
                    "MDHGSSC",
                    "XDL90M",
                    "XJP15F",
                    "XJP120M",
                    "BJSSC",
                    "JNDSSC",
                    "HCSSC",
                    "WHDJSSC",
                    "JHJPZ15C",
                    "JH2FC",
                    "JH5FC",
                    "PK10"
                };
            }
            else if (this.PTName == "杏彩娱乐")
            {
                AppInfo.PTInfo = AppInfo.XCAIInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "HZFFC",
                    "HZ5FC",
                    "UCRDFFC",
                    "UCRD2FC"
                };
            }
            else if (this.PTName == "SKY娱乐")
            {
                AppInfo.PTInfo = AppInfo.SKYYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TXFFC",
                    "JFYLGG15C",
                    "DB15C",
                    "SKYFFC",
                    "SKY2FC",
                    "JFYLBHD15C",
                    "JFYLGX2FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.SKYGJ)
                {
                    List<string> list49 = new List<string> { 
                        "VRSSC",
                        "VR3FC",
                        "VRSXFFC",
                        "VRZXC11X5",
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "JS11X5",
                        "LN11X5",
                        "HEB11X5",
                        "HLJ11X5",
                        "SKY2F11X5",
                        "PK10",
                        "VRPK10",
                        "VRMXSC",
                        "VRYYPK10",
                        "VRKT"
                    };
                    pLotteryIDList.AddRange(list49);
                }
            }
            else if (this.PTName == "天辰娱乐")
            {
                AppInfo.PTInfo = AppInfo.TCYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "JFYLBHD15C",
                    "JFYLGG15C",
                    "DB15C",
                    "JFYLGX2FC",
                    "TCYLFFC",
                    "TCYL2FC",
                    "VRSSC",
                    "VR3FC",
                    "VRSXFFC",
                    "GD11X5",
                    "JX11X5",
                    "SH11X5",
                    "JS11X5",
                    "HEB11X5",
                    "TCYLFF11X5",
                    "TCYL2F11X5",
                    "VRZXC11X5",
                    "PK10",
                    "TCYLFFPK10",
                    "TCYL2FPK10",
                    "VRMXSC",
                    "VRYYPK10",
                    "VRPK10",
                    "VRKT"
                };
            }
            else if (this.PTName == "鹿鼎娱乐")
            {
                AppInfo.PTInfo = AppInfo.LUDIInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "LFDJSSC",
                    "JNDSSC",
                    "MD2FC",
                    "SSHCTXFFC",
                    "TXFFC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC",
                    "LUDIFFC",
                    "LUDI3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.LDGJ)
                {
                    List<string> list52 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "PK10",
                        "VRPK10"
                    };
                    pLotteryIDList.AddRange(list52);
                }
            }
            else if (this.PTName == "十里桃花")
            {
                AppInfo.PTInfo = AppInfo.SLTHInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "SLTHHGSSC",
                    "SLTHNHGSSC",
                    "SLTHDJSSC",
                    "TXFFC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC",
                    "SLTHFFC",
                    "SLTH2FC",
                    "SLTHTEQ15C"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.SLTHGJ)
                {
                    List<string> list54 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "XJ11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list54);
                }
            }
            else if (this.PTName == "蓝冠在线")
            {
                AppInfo.PTInfo = AppInfo.LGZXInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "YNSSC",
                    "LGZXFFC",
                    "LGZX3FC",
                    "LFDJSSC",
                    "MD2FC",
                    "TXFFC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.LGZXGJ)
                {
                    List<string> list56 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "XJ11X5",
                        "JS11X5",
                        "LN11X5",
                        "HEB11X5",
                        "HLJ11X5",
                        "LGZX3F11X5",
                        "LGZXFF11X5",
                        "PK10",
                        "XYFTPK10",
                        "LGZXPK10",
                        "VRPK10"
                    };
                    pLotteryIDList.AddRange(list56);
                }
            }
            else if (this.PTName == "玖富娱乐")
            {
                AppInfo.PTInfo = AppInfo.JFYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "TXFFC",
                    "DB15C",
                    "JFYLBHD15C",
                    "JFYLFFC",
                    "JFYL2FC",
                    "JFYLGX2FC",
                    "JFYLGG15C",
                    "VRSSC",
                    "VR3FC",
                    "VRSXFFC",
                    "VRZXC11X5"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.JFYLGJ)
                {
                    List<string> list58 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "JS11X5",
                        "LN11X5",
                        "HEB11X5",
                        "HLJ11X5",
                        "JFYL2F11X5",
                        "PK10",
                        "VRMXSC",
                        "VRYYPK10",
                        "VRPK10",
                        "VRKT"
                    };
                    pLotteryIDList.AddRange(list58);
                }
            }
            else if (this.PTName == "金皇朝")
            {
                AppInfo.PTInfo = AppInfo.JHCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "TXFFC",
                    "JHCDBFFC",
                    "JHCJDSSC",
                    "JHCFFC",
                    "JHC2FC",
                    "JHC5FC",
                    "JHCJZFFC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.JHCGJ)
                {
                    List<string> list60 = new List<string> { 
                        "PK10",
                        "VRKT"
                    };
                    pLotteryIDList.AddRange(list60);
                }
            }
            else if (this.PTName == "金皇朝2")
            {
                AppInfo.PTInfo = AppInfo.JHC2Info;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "JHC2FFC",
                    "JHC215C",
                    "JHC23FC",
                    "TXFFC",
                    "JHC2DPFFC",
                    "JHC2JZFFC",
                    "JHC2JD15C",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC",
                    "VRPK10",
                    "VRKT"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.JHC2GJ)
                {
                    List<string> list62 = new List<string> { 
                        "PK10",
                        "JHC2PK10"
                    };
                    pLotteryIDList.AddRange(list62);
                }
            }
            else if (this.PTName == "幻影娱乐")
            {
                AppInfo.PTInfo = AppInfo.WYYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "WYFFC",
                    "WY2FC",
                    "WY3FC",
                    "SSHCTXFFC",
                    "TXFFC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC",
                    "VRSXFFC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "SH11X5",
                    "JS11X5",
                    "LN11X5",
                    "HEB11X5",
                    "WYFF11X5",
                    "WY2F11X5",
                    "WY3F11X5",
                    "VRZXC11X5",
                    "PK10",
                    "WYFFPK10",
                    "WY2FPK10",
                    "WY3FPK10",
                    "VRPK10",
                    "VRKT",
                    "VRMXSC",
                    "VRYYPK10"
                };
            }
            else if (this.PTName == "盛世皇朝")
            {
                AppInfo.PTInfo = AppInfo.SSHCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "YNSSC",
                    "SSHCTXFFC",
                    "LFDJSSC",
                    "MD2FC",
                    "SSHCFFC",
                    "SSHC3FC",
                    "TXFFC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "SH11X5",
                    "XJ11X5",
                    "JS11X5",
                    "LN11X5",
                    "HEB11X5",
                    "HLJ11X5",
                    "SSHCFF11X5",
                    "SSHC3F11X5",
                    "PK10",
                    "SSHCPK10",
                    "VRPK10"
                };
            }
            else if (this.PTName == "万和城")
            {
                AppInfo.PTInfo = AppInfo.WHCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "MD2FC",
                    "DJSSC",
                    "TXFFC",
                    "WHCFFC",
                    "WHC3FC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.WHCGJ)
                {
                    List<string> list66 = new List<string> { 
                        "SH11X5",
                        "SD11X5",
                        "XJ11X5",
                        "JX11X5",
                        "GD11X5",
                        "JS11X5",
                        "LN11X5",
                        "HEB11X5",
                        "HLJ11X5",
                        "PK10",
                        "VRKT"
                    };
                    pLotteryIDList.AddRange(list66);
                }
            }
            else if (this.PTName == "拉菲II")
            {
                AppInfo.PTInfo = AppInfo.LF2Info;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TXFFC",
                    "QQFFC",
                    "HGSSC",
                    "LFHGSSC",
                    "MD2FC",
                    "LFDJSSC",
                    "TWSSC",
                    "BJSSC",
                    "JNDSSC",
                    "TJSSC",
                    "XJSSC",
                    "LF2FFC",
                    "LF22FC",
                    "LF25FC"
                };
            }
            else if (this.PTName == "亿宝娱乐")
            {
                AppInfo.PTInfo = AppInfo.YBAOInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "FLBSSC",
                    "YBAOFFC",
                    "YBAO2FC",
                    "YBAO45C"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.YBAOGJ)
                {
                    List<string> list69 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list69);
                }
            }
            else if (this.PTName == "千金城")
            {
                AppInfo.PTInfo = AppInfo.QJCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "QJCTXFFC",
                    "FLBSSC",
                    "QJCFFC",
                    "QJC2FC",
                    "QJC45C",
                    "QJCHL15C",
                    "QJCAM15C",
                    "QJCXNFFC",
                    "QJCPDFFC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.QJCGJ)
                {
                    List<string> list71 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "JX11X5",
                        "QJC11X5",
                        "PK10",
                        "QJCPK10"
                    };
                    pLotteryIDList.AddRange(list71);
                }
            }
            else if (this.PTName == "WE娱乐")
            {
                AppInfo.PTInfo = AppInfo.WEYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "FLBSSC",
                    "WEFFC",
                    "WE2FC",
                    "WE45C",
                    "WETXFFC",
                    "WEAM15C",
                    "GD11X5",
                    "SD11X5",
                    "JX11X5",
                    "JS11X5",
                    "WE11X5",
                    "PK10",
                    "WEPK10"
                };
            }
            else if (this.PTName == "亿人娱乐")
            {
                AppInfo.PTInfo = AppInfo.YRYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "YRHG15C",
                    "YRTXFFC",
                    "YRFFC",
                    "YR2FC",
                    "YR5FC",
                    "YRALFFC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.YRYLGJ)
                {
                    List<string> list74 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list74);
                }
            }
            else if (this.PTName == "疯彩娱乐")
            {
                AppInfo.PTInfo = AppInfo.FCYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "JNDSSC",
                    "TXFFC",
                    "QQFFC",
                    "LFHGSSC",
                    "LFDJSSC",
                    "FCOZ3FC",
                    "FCSLFK5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.FCGJ)
                {
                    List<string> list76 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "SH11X5",
                        "JX11X5",
                        "HLJ11X5",
                        "PK10",
                        "XYFTPK10"
                    };
                    pLotteryIDList.AddRange(list76);
                }
            }
            else if (this.PTName == "路易斯")
            {
                AppInfo.PTInfo = AppInfo.LYSInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "JNDSSC",
                    "TXFFC",
                    "QQFFC",
                    "LFHGSSC",
                    "LFDJSSC",
                    "LYSSLFK5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.LYSGJ)
                {
                    List<string> list78 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "SH11X5",
                        "JX11X5",
                        "HLJ11X5",
                        "JS11X5",
                        "XJ11X5",
                        "PK10",
                        "XYFTPK10"
                    };
                    pLotteryIDList.AddRange(list78);
                }
            }
            else if (this.PTName == "凯萨娱乐")
            {
                AppInfo.PTInfo = AppInfo.KSYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "LFHGSSC",
                    "BJSSC",
                    "TWSSC",
                    "JNDSSC",
                    "LYSSLFK5FC",
                    "QQFFC",
                    "TXFFC",
                    "LFDJSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.KSGJ)
                {
                    List<string> list80 = new List<string> { 
                        "SD11X5",
                        "SH11X5",
                        "JX11X5",
                        "HLJ11X5",
                        "JS11X5",
                        "XJ11X5",
                        "PK10",
                        "XYFTPK10"
                    };
                    pLotteryIDList.AddRange(list80);
                }
            }
            else if (this.PTName == "鼎尖娱乐")
            {
                AppInfo.PTInfo = AppInfo.DJYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "LFHGSSC",
                    "QQFFC",
                    "LFDJSSC",
                    "GD11X5",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "HLJ11X5",
                    "JS11X5",
                    "XJ11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "星彩娱乐")
            {
                AppInfo.PTInfo = AppInfo.XINCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "LFHGSSC",
                    "BJSSC",
                    "TWSSC",
                    "JNDSSC",
                    "TXFFC",
                    "QQFFC",
                    "LFDJSSC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "HLJ11X5",
                    "JS11X5",
                    "SH11X5",
                    "PK10",
                    "XYFTPK10"
                };
            }
            else if (this.PTName == "新濠环彩")
            {
                AppInfo.PTInfo = AppInfo.XHHCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "LFHGSSC",
                    "BJSSC",
                    "JNDSSC",
                    "XHHCOZ3FC",
                    "XHHCSLFK5FC",
                    "TXFFC",
                    "QQFFC",
                    "LFDJSSC",
                    "SD11X5",
                    "JX11X5",
                    "HLJ11X5",
                    "JS11X5",
                    "SH11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "迷彩娱乐")
            {
                AppInfo.PTInfo = AppInfo.MCYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "LFHGSSC",
                    "BJSSC",
                    "JNDSSC",
                    "TXFFC",
                    "MCQQFFC",
                    "LFDJSSC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "HLJ11X5",
                    "JS11X5",
                    "SH11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "唐人娱乐")
            {
                AppInfo.PTInfo = AppInfo.TRYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "QJCTXFFC",
                    "FLBSSC",
                    "TRFFC",
                    "TR2FC",
                    "TR45C",
                    "TR11X5",
                    "GD11X5",
                    "SD11X5",
                    "JX11X5",
                    "PK10",
                    "TRPK10"
                };
            }
            else if (this.PTName == "欢乐城")
            {
                AppInfo.PTInfo = AppInfo.HLCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HLCHG15F",
                    "TWSSC",
                    "BJSSC",
                    "HLCDJ15F",
                    "HLCFLB15C",
                    "HLCFLB2FC",
                    "HLCFLB5FC",
                    "HLCFFC",
                    "HLC2FC",
                    "HLC5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.DEBUG)
                {
                    List<string> list87 = new List<string> { "PK10" };
                    pLotteryIDList.AddRange(list87);
                }
            }
            else if (this.PTName == "名城娱乐")
            {
                AppInfo.PTInfo = AppInfo.MINCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "MINCTXFFC",
                    "HLCHG15F",
                    "MINCDJSSC",
                    "MINCSE15F",
                    "MINCNY15C",
                    "TWSSC",
                    "BJSSC",
                    "HLCFLB15C",
                    "HLCFLB2FC",
                    "HLCFLB5FC",
                    "MINCFFC",
                    "MINC2FC",
                    "MINC5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.MINCGJ)
                {
                    List<string> list89 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list89);
                }
            }
            else if (this.PTName == "仁鼎娱乐")
            {
                AppInfo.PTInfo = AppInfo.RDYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "HLCSE15F",
                    "HLCNY15C",
                    "HLCHG15F",
                    "HLCDJ15F",
                    "BJSSC",
                    "TWSSC",
                    "HLCFLB15C",
                    "HLCFLB2FC",
                    "HLCFLB5FC",
                    "RDYLFFC",
                    "RDYL2FC",
                    "RDYL5FC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.RDYLGJ)
                {
                    List<string> list91 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list91);
                }
            }
            else if (this.PTName == "宏達娱乐")
            {
                AppInfo.PTInfo = AppInfo.HONDInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "HONDSE15F",
                    "HONDNY15C",
                    "HONDHG15F",
                    "HONDDJ15F",
                    "BJSSC",
                    "TWSSC",
                    "HLCFLB15C",
                    "HLCFLB2FC",
                    "HLCFLB5FC",
                    "HONDFFC",
                    "HOND2FC",
                    "HOND5FC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC",
                    "GD11X5",
                    "SH11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "菲洛城")
            {
                AppInfo.PTInfo = AppInfo.FLCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "HLCSE15F",
                    "HLCNY15C",
                    "HLCHG15F",
                    "HLCDJ15F",
                    "HLCFLB15C",
                    "HLCFLB2FC",
                    "HLCFLB5FC",
                    "FLCFFC",
                    "FLC2FC",
                    "FLC5FC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.FLCGJ)
                {
                    List<string> list94 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list94);
                }
            }
            else if (this.PTName == "彩天堂")
            {
                AppInfo.PTInfo = AppInfo.CTTInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "HLCSE15F",
                    "HLCNY15C",
                    "HLCHG15F",
                    "HLCDJ15F",
                    "HLCFLB15C",
                    "HLCFLB2FC",
                    "HLCFLB5FC",
                    "CTTFFC",
                    "CTT5FC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.CAITTGJ)
                {
                    List<string> list96 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list96);
                }
            }
            else if (this.PTName == "乐美汇")
            {
                AppInfo.PTInfo = AppInfo.LMHInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "LMHHGSSC",
                    "LMHDJSSC",
                    "LMHXY45M",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.LMHGJ)
                {
                    List<string> list98 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "SXR11X5",
                        "JS11X5",
                        "PK10",
                        "XGSM",
                        "VRPK10",
                        "XYFTPK10"
                    };
                    pLotteryIDList.AddRange(list98);
                }
            }
            else if (this.PTName == "博牛国际")
            {
                AppInfo.PTInfo = AppInfo.BNGJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BNFFC",
                    "BN5FC",
                    "BJSSC",
                    "DPCDJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "ELSSSC",
                    "XXLSSC",
                    "BMEISE15F",
                    "BMTXFFC",
                    "BNTXFFC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.BNGJ)
                {
                    List<string> list100 = new List<string> { "PK10" };
                    pLotteryIDList.AddRange(list100);
                }
            }
            else if (this.PTName == "博美娱乐")
            {
                AppInfo.PTInfo = AppInfo.BMEIInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BMEIFFC",
                    "BMEI5FC",
                    "BJSSC",
                    "DPCDJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "BMEISE15F",
                    "ELSSSC",
                    "XXLSSC",
                    "BMTXFFC",
                    "BMHGSSC",
                    "BMDJSSC",
                    "BMFLBSSC",
                    "BMTWFFC",
                    "BMQQFFC",
                    "ALGJNTXFFC",
                    "ALGJOZBWC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.BMGJ)
                {
                    List<string> list102 = new List<string> { "PK10" };
                    pLotteryIDList.AddRange(list102);
                }
            }
            else if (this.PTName == "迪拜城")
            {
                AppInfo.PTInfo = AppInfo.DPCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "DPCFFC",
                    "DPC5FC",
                    "BJSSC",
                    "DPCDJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "ELSSSC",
                    "XXLSSC",
                    "DPCTXFFC",
                    "DPCOZBWC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.DPCGJ)
                {
                    List<string> list104 = new List<string> { "PK10" };
                    pLotteryIDList.AddRange(list104);
                }
            }
            else if (this.PTName == "红馆迪拜")
            {
                AppInfo.PTInfo = AppInfo.HGDBInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HGDBFFC",
                    "HGDB5FC",
                    "BJSSC",
                    "DPCDJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "ELSSSC",
                    "BMEISE15F",
                    "XXLSSC",
                    "BMTXFFC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.YFENG)
                {
                    List<string> list106 = new List<string> { "PK10" };
                    pLotteryIDList.AddRange(list106);
                }
            }
            else if (this.PTName == "美娱国际")
            {
                AppInfo.PTInfo = AppInfo.MYGJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "MYOZBWC"
                };
            }
            else if (this.PTName == "万森娱乐")
            {
                AppInfo.PTInfo = AppInfo.WSYLInfo;
                pLotteryIDList = new List<string> { 
                    "WS30M",
                    "WSBLS60M",
                    "WSHS15F",
                    "WSFS15F",
                    "WSXDL15F",
                    "WSNDJSSC",
                    "WSELSSSC",
                    "WSQQFFC",
                    "TXFFC",
                    "WSFLP15C",
                    "CQSSC",
                    "XJSSC",
                    "BJSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.WSGJ)
                {
                    List<string> list109 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "AH11X5",
                        "SH11X5",
                        "WS11X5",
                        "PK10",
                        "WS120MPK10",
                        "WS180MPK10"
                    };
                    pLotteryIDList.AddRange(list109);
                }
            }
            else if (this.PTName == "万彩娱乐")
            {
                AppInfo.PTInfo = AppInfo.WCYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TXFFC",
                    "BJSSC",
                    "WCFLPFFC",
                    "WCJPZFFC",
                    "CAIHFLP15C",
                    "WCDBFFC",
                    "WCMDJB15C",
                    "WCMGDZ2FC",
                    "WCMG45M",
                    "WCMLXY3FC",
                    "WCTWFFC",
                    "WCXBYFFC",
                    "WCXDL15C",
                    "WCYGFFC",
                    "GD11X5",
                    "JX11X5",
                    "SD11X5",
                    "AH11X5",
                    "SH11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "万恒娱乐")
            {
                AppInfo.PTInfo = AppInfo.WHENInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TJSSC",
                    "XJSSC",
                    "BJSSC",
                    "TWSSC",
                    "TXFFC",
                    "WHEN30M",
                    "WHENBLS60M",
                    "WHENHS15F",
                    "WHENFS15F",
                    "WHENXDL15F",
                    "WHENNDJSSC",
                    "WHENELSSSC",
                    "WHENQQFFC",
                    "WHENFLP15C",
                    "WHENHGSSC",
                    "WHENDJSSC",
                    "WHENXJPSSC",
                    "WHENYG60M"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.WHEN)
                {
                    List<string> list112 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SD11X5",
                        "AH11X5",
                        "SH11X5",
                        "WHEN11X5",
                        "PK10",
                        "WHEN120MPK10",
                        "WHEN180MPK10"
                    };
                    pLotteryIDList.AddRange(list112);
                }
            }
            else if (this.PTName == "天易娱乐")
            {
                AppInfo.PTInfo = AppInfo.TYYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TJSSC",
                    "XJSSC",
                    "BJSSC",
                    "TWSSC",
                    "TXFFC",
                    "TYYLHS15F",
                    "TYYL30M",
                    "TYYLMG60M",
                    "TYYLDJ15F",
                    "TYYLXJPSSC",
                    "TYYLXDL15F",
                    "TYYLELS15F",
                    "TYYLYD15F",
                    "GD11X5",
                    "JX11X5",
                    "SD11X5",
                    "AH11X5",
                    "SH11X5",
                    "TYYL30M11X5",
                    "TYYL90M11X5",
                    "PK10",
                    "TYYL120MPK10",
                    "TYYL180MPK10"
                };
            }
            else if (this.PTName == "天恒娱乐")
            {
                AppInfo.PTInfo = AppInfo.THENInfo;
                pLotteryIDList = new List<string> { 
                    "TXFFC",
                    "CQSSC",
                    "TJSSC",
                    "XJSSC",
                    "BJSSC",
                    "TWSSC",
                    "THENXJP30M",
                    "THENMGFFC",
                    "THENHGSSC",
                    "THENDJSSC",
                    "THENXJPSSC",
                    "THENXDLSSC",
                    "THENELSSSC",
                    "THENYD15C"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.THEN)
                {
                    List<string> list115 = new List<string> { 
                        "THENNY11X5",
                        "THENJND11X5",
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "AH11X5",
                        "SH11X5",
                        "PK10",
                        "THEN120MPK10",
                        "THEN180MPK10"
                    };
                    pLotteryIDList.AddRange(list115);
                }
            }
            else if (this.PTName == "华纳娱乐")
            {
                AppInfo.PTInfo = AppInfo.HNYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "HNYL5FC",
                    "HNYLFFC",
                    "HNYLFLP15C",
                    "HNYLXJPSSC",
                    "TXFFC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.HNYLGJ)
                {
                    List<string> list117 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SD11X5",
                        "AH11X5",
                        "SH11X5",
                        "HNYL11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list117);
                }
            }
            else if (this.PTName == "起凡娱乐")
            {
                AppInfo.PTInfo = AppInfo.QFYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "QFYLFFC",
                    "QFYL3FC",
                    "QFYL5FC",
                    "QFYLDJSSC",
                    "HLCFLB15C",
                    "QFYLSE15F",
                    "TXFFC",
                    "QFYLNY15C",
                    "HLCFLB2FC",
                    "HLCFLB5FC",
                    "GD11X5",
                    "JX11X5",
                    "SD11X5",
                    "AH11X5",
                    "SH11X5",
                    "QFYL11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "喜鹊娱乐")
            {
                AppInfo.PTInfo = AppInfo.XQYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "XQYLFFC",
                    "XQYL3FC",
                    "XQYL5FC",
                    "XQYLHGSSC",
                    "XQYLDJSSC",
                    "HLCFLB15C",
                    "XQYLSE15F",
                    "TXFFC",
                    "XQYLNY15C",
                    "HLCFLB2FC",
                    "HLCFLB5FC",
                    "XQYLQQFFC",
                    "XQYLBDFFC",
                    "XQYLWBFFC",
                    "GD11X5",
                    "JX11X5",
                    "SD11X5",
                    "AH11X5",
                    "SH11X5",
                    "XQYL11X5",
                    "XQYLJS11X5",
                    "PK10",
                    "XQYLJSPK10",
                    "XYFTPK10"
                };
            }
            else if (this.PTName == "全球通2")
            {
                AppInfo.PTInfo = AppInfo.QQT2Info;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "QQTJNDSSC",
                    "TXFFC",
                    "QQTTX2FC",
                    "QQTXDLSSC",
                    "QQTDJSSC",
                    "QQTHGSSC",
                    "QQTTG15C",
                    "QQTXXLSSC",
                    "QQTNY15C",
                    "QQTFFC",
                    "QQT2FC",
                    "QQT5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.QQTGJ)
                {
                    List<string> list121 = new List<string> { 
                        "PK10",
                        "QQTHGPK10"
                    };
                    pLotteryIDList.AddRange(list121);
                }
            }
            else if (this.PTName == "旺百家")
            {
                AppInfo.PTInfo = AppInfo.WBJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "WBJBJSSC",
                    "BJSSC",
                    "WBJHG2FC",
                    "TXFFC",
                    "WBJDX15C",
                    "WBJNY15C",
                    "WBJMSK35C",
                    "WBJFFC",
                    "WBJ2FC",
                    "WBJ5FC",
                    "WBJTG30",
                    "WBJXXLSSC",
                    "WBJDJSSC",
                    "WBJHGSSC",
                    "WBJXDLSSC",
                    "WBJXJPSSC",
                    "WBJJNDSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.WBJGJ)
                {
                    List<string> list123 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "WBJOM11X5",
                        "PK10",
                        "WBJOMPK10",
                        "WBJNNPK10"
                    };
                    pLotteryIDList.AddRange(list123);
                }
            }
            else if (this.PTName == "恒达娱乐")
            {
                AppInfo.PTInfo = AppInfo.HENDInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "HENDFFC",
                    "HENDXDLSSC",
                    "HEND2FC",
                    "HEND5FC",
                    "TXFFC",
                    "HENDTG30",
                    "HENDDJSSC",
                    "HENDNY15C",
                    "HENDHGSSC",
                    "HENDDX15C",
                    "HENDXXLSSC",
                    "HENDHG1FC",
                    "HENDHG2FC",
                    "HENDXJPSSC",
                    "GD11X5",
                    "JX11X5",
                    "SH11X5",
                    "HLJ11X5",
                    "HENDJS11X5",
                    "PK10",
                    "HENDJSPK10",
                    "HENDOMPK10",
                    "HENDFFPK10",
                    "HENDTXPK10"
                };
            }
            else if (this.PTName == "星多宝")
            {
                AppInfo.PTInfo = AppInfo.XDBInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "XDBFFC",
                    "XDBDJSSC",
                    "XDBHGSSC",
                    "XDB2FC",
                    "XDB3FC",
                    "XDB5FC",
                    "GD11X5",
                    "SD11X5",
                    "AH11X5",
                    "JX11X5",
                    "XDBFF11X5",
                    "XDB2F11X5",
                    "XDB3F11X5",
                    "XDB5F11X5",
                    "PK10",
                    "XDBFFPK10",
                    "XDB2FPK10",
                    "XDB3FPK10",
                    "XDB5FPK10"
                };
            }
            else if (this.PTName == "侏罗纪")
            {
                AppInfo.PTInfo = AppInfo.ZLJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "ZLJMGFFC",
                    "ZLJSE15C",
                    "ZLJDJSSC",
                    "ZLJFLP2FC",
                    "ZLJELS5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.ZLJGJ)
                {
                    List<string> list127 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "ZLJFF11X5",
                        "PK10",
                        "ZLJFFPK10"
                    };
                    pLotteryIDList.AddRange(list127);
                }
            }
            else if (this.PTName == "亿昇娱乐")
            {
                AppInfo.PTInfo = AppInfo.YSENInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "YSENXGSSC",
                    "YSENBJSSC",
                    "TXFFC",
                    "YSENFFC",
                    "YSEN2FC",
                    "YSEN5FC",
                    "YSENXGBFC",
                    "YSENDM45C",
                    "YSENDL2FC",
                    "YSENDJ35C",
                    "YSENXJPSSC",
                    "YSENJNDSSC",
                    "YSENHGSSC",
                    "YSENLT15C",
                    "YSENXDLSSC",
                    "YSENJZ15C",
                    "YSENDJSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.YSENGJ)
                {
                    List<string> list129 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "YSENJZ11X5",
                        "PK10",
                        "YSENTBPK10",
                        "YSENXGPK10"
                    };
                    pLotteryIDList.AddRange(list129);
                }
            }
            else if (this.PTName == "翡翠娱乐")
            {
                AppInfo.PTInfo = AppInfo.FEICInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "FEICXJP2FC",
                    "FEIC3FC",
                    "FEICSLFK5FC",
                    "FEICFFC",
                    "TXFFC",
                    "FEICDJSSC",
                    "FEICHGSSC",
                    "FEIC30M",
                    "FEIC45M"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.FEICGJ)
                {
                    List<string> list131 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "FEICFF11X5",
                        "FEIC2F11X5",
                        "FEIC3F11X5",
                        "FEIC5F11X5",
                        "PK10",
                        "FEICFFPK10",
                        "FEIC2FPK10",
                        "FEIC3FPK10",
                        "FEIC5FPK10"
                    };
                    pLotteryIDList.AddRange(list131);
                }
            }
            else if (this.PTName == "中呗娱乐")
            {
                AppInfo.PTInfo = AppInfo.ZBEIInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "ZBEIXJP2FC",
                    "ZBEI3FC",
                    "ZBEISLFK5FC",
                    "TXFFC",
                    "ZBEIFFC",
                    "ZBEIDJSSC",
                    "ZBEIHGSSC",
                    "ZBEI30M",
                    "ZBEI45M",
                    "ZBEIQQFFC",
                    "ZBEIWXFFC",
                    "ZBEIGGFFC",
                    "ZBEIXJP15C",
                    "ZBEIXDL15C",
                    "ZBEIJNDSSC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "AH11X5",
                    "FJ11X5",
                    "ZBEIFF11X5",
                    "ZBEI2F11X5",
                    "ZBEI3F11X5",
                    "ZBEI5F11X5",
                    "PK10",
                    "ZBEIFFPK10",
                    "ZBEI2FPK10",
                    "ZBEI3FPK10",
                    "ZBEI5FPK10"
                };
            }
            else if (this.PTName == "桃花岛娱乐")
            {
                AppInfo.PTInfo = AppInfo.THDYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "XJPSSC",
                    "TWSSC",
                    "BLSFFC",
                    "XDLSSC",
                    "JZD15FC",
                    "LD2FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.THDGJ)
                {
                    List<string> list134 = new List<string> { 
                        "SD11X5",
                        "XDL11X5",
                        "JZD11X5",
                        "PK10",
                        "XDLPK10",
                        "JZDPK10"
                    };
                    pLotteryIDList.AddRange(list134);
                }
            }
            else if (this.PTName == "新火巅峰")
            {
                AppInfo.PTInfo = AppInfo.XHDFInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "XHDFFFC",
                    "XHDF5FC",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "ELSSSC",
                    "XHDFDJSSC",
                    "XHDFHGSSC",
                    "XHDFTXFFC",
                    "XXLSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.XH3GJ)
                {
                    List<string> list136 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "AH11X5",
                        "SH11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list136);
                }
            }
            else if (this.PTName == "新火大时代")
            {
                AppInfo.PTInfo = AppInfo.XHSDInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "XHSDFFC",
                    "XHSD5FC",
                    "XXLSSC",
                    "ELSSSC",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "LSWJSMG15C",
                    "LSWJSMG45M",
                    "TXFFC",
                    "GD11X5",
                    "SD11X5",
                    "JX11X5",
                    "AH11X5",
                    "SH11X5",
                    "XHSDFF11X5",
                    "PK10",
                    "XYFTPK10"
                };
            }
            else if (this.PTName == "博乐国际")
            {
                AppInfo.PTInfo = AppInfo.BLGJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "BLGJFFC",
                    "BLGJ5FC",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "BLGJTXFFC",
                    "ELSSSC",
                    "XXLSSC",
                    "BLGJFLP45M",
                    "BLGJYN15C",
                    "BLGJXDL45M",
                    "BLGJXDL90M",
                    "ALGJOZBWC",
                    "GD11X5",
                    "SD11X5",
                    "JX11X5",
                    "BLGJFF11X5",
                    "PK10",
                    "XYFTPK10"
                };
            }
            else if (this.PTName == "奇趣娱乐")
            {
                AppInfo.PTInfo = AppInfo.QQYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "QQYLFFC",
                    "QQYL5FC",
                    "ALGJOZBWC",
                    "TXFFC",
                    "ELSSSC",
                    "XXLSSC",
                    "QQYLFLP45M",
                    "QQYLRB45M",
                    "QQYLMG45M",
                    "QQYLJND4FC",
                    "QQYLHG5FC",
                    "QQYLQQXDL45M",
                    "QQYLQQXDL90M",
                    "GD11X5",
                    "SD11X5",
                    "JX11X5",
                    "QQYLFF11X5",
                    "PK10",
                    "XYFTPK10"
                };
            }
            else if (this.PTName == "德晋娱乐")
            {
                AppInfo.PTInfo = AppInfo.DEJIInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "DEJIFFC",
                    "DEJI5FC",
                    "ALGJOZBWC",
                    "DEJITXFFC",
                    "ELSSSC",
                    "XXLSSC",
                    "DEJIFLP45M",
                    "DEJIRB45M",
                    "DEJIMG45M",
                    "DEJIJND4FC",
                    "DEJIHG5FC",
                    "DEJIQQXDL45M",
                    "DEJIQQXDL90M",
                    "GD11X5",
                    "SD11X5",
                    "DEJIFF11X5",
                    "PK10",
                    "XYFTPK10"
                };
            }
            else if (this.PTName == "巨龙国际")
            {
                AppInfo.PTInfo = AppInfo.JLGJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TJSSC",
                    "XJSSC",
                    "JLGJFFC",
                    "JLGJ5FC",
                    "JLGJTXFFC",
                    "ALGJOZBWC",
                    "TXFFC",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "ELSSSC",
                    "XXLSSC",
                    "GD11X5",
                    "SD11X5",
                    "JX11X5",
                    "AH11X5",
                    "SH11X5",
                    "JLGJFF11X5",
                    "PK10",
                    "XYFTPK10"
                };
            }
            else if (this.PTName == "亿城娱乐")
            {
                AppInfo.PTInfo = AppInfo.YCYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TJSSC",
                    "XJSSC",
                    "YCYLFFC",
                    "YCYL5FC",
                    "TXFFC",
                    "YCYLTXFFC",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "ELSSSC",
                    "XXLSSC",
                    "ALGJOZBWC",
                    "GD11X5",
                    "SD11X5",
                    "YCYLFF11X5",
                    "PK10",
                    "XYFTPK10"
                };
            }
            else if (this.PTName == "锦绣娱乐")
            {
                AppInfo.PTInfo = AppInfo.JXYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "JXYLFFC",
                    "JXYL2FC",
                    "JXYL3FC",
                    "JXYL5FC",
                    "JNDSSC",
                    "JXYLHG5FC",
                    "JXYLELSSSC",
                    "JXYLXXLSSC",
                    "GD11X5",
                    "SD11X5",
                    "JX11X5",
                    "AH11X5",
                    "SH11X5",
                    "JXYLFF11X5",
                    "PK10",
                    "XYFTPK10"
                };
            }
            else if (this.PTName == "博万通")
            {
                AppInfo.PTInfo = AppInfo.BWTInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BWTFFC",
                    "BWT5FC",
                    "XXLSSC",
                    "ELSSSC",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "BWTHGSSC",
                    "BWTDJSSC",
                    "BWTTXFFC",
                    "BWTOZBWC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.BWTGJ)
                {
                    List<string> list145 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SD11X5",
                        "AH11X5",
                        "SH11X5",
                        "BWT11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list145);
                }
            }
            else if (this.PTName == "汇盛国际")
            {
                AppInfo.PTInfo = AppInfo.HSGJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HSFFC",
                    "HS5FC",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "ELSSSC",
                    "XXLSSC",
                    "HSDJSSC",
                    "TXFFC",
                    "HSQQFFC",
                    "HSSE15F",
                    "HSAZXYC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.HSGJ)
                {
                    List<string> list147 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "AH11X5",
                        "SH11X5",
                        "PK10",
                        "XYFTPK10"
                    };
                    pLotteryIDList.AddRange(list147);
                }
            }
            else if (this.PTName == "鑫旺娱乐")
            {
                AppInfo.PTInfo = AppInfo.XWYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "XWYLXDLSSC",
                    "XWYLXWYFFC",
                    "XWYLXJPSSC",
                    "BJSSC",
                    "XWYLFL30M",
                    "XWYLTX30M",
                    "XWYLQQFFC",
                    "XWYLLD2FC",
                    "XWYLHGSSC",
                    "XWYLDJSSC",
                    "XWYLSE15F",
                    "XWYLNY15C",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "GD11X5",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "青蜂在线")
            {
                AppInfo.PTInfo = AppInfo.QFZXInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "QFZXXDLSSC",
                    "QFZXXWYFFC",
                    "QFZXHGSSC",
                    "QFZXXJPSSC",
                    "QFZXDJSSC",
                    "BJSSC",
                    "TWSSC",
                    "QFZXFL30M",
                    "QFZXTX30M",
                    "QFZXQQFFC",
                    "QFZXLD2FC",
                    "QFZXSE15F",
                    "QFZXNY15C",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "GD11X5",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "LN11X5",
                    "JS11X5",
                    "PK10",
                    "XYFTPK10",
                    "QFZXPK10"
                };
            }
            else if (this.PTName == "澳利国际")
            {
                AppInfo.PTInfo = AppInfo.ALGJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "ALGJFFC",
                    "ALGJ5FC",
                    "ELSSSC",
                    "XXLSSC",
                    "ALGJRB15C",
                    "ALGJSE15C",
                    "ALGJTXFFC",
                    "ALGJNTXFFC",
                    "ALGJOZBWC",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.ALGJGJ)
                {
                    List<string> list151 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SD11X5",
                        "AH11X5",
                        "SH11X5",
                        "ALGJFF11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list151);
                }
            }
            else if (this.PTName == "公爵娱乐")
            {
                AppInfo.PTInfo = AppInfo.GJYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "GJFFC",
                    "GJ5FC",
                    "GJTX60M",
                    "TXFFC",
                    "XXLSSC",
                    "ELSSSC",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "GJOZBWC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.GJYLGJ)
                {
                    List<string> list153 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SD11X5",
                        "AH11X5",
                        "SH11X5",
                        "GJFF11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list153);
                }
            }
            else if (this.PTName == "拉斯维加斯")
            {
                AppInfo.PTInfo = AppInfo.LSWJSInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "LSWJSFFC",
                    "LSWJS5FC",
                    "LSWJSOTXFFC",
                    "LSWJSMG15C",
                    "LSWJSMG45M",
                    "LSWJSOZFFC",
                    "LSWJSOZ35C",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "LSWJSHGSSC",
                    "LSWJSDJSSC",
                    "LSWJSTXFFC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.LSWJSGJ)
                {
                    List<string> list155 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SD11X5",
                        "AH11X5",
                        "SH11X5",
                        "LSWJSFF11X5",
                        "PK10",
                        "LSWJSFFPK10"
                    };
                    pLotteryIDList.AddRange(list155);
                }
            }
            else if (this.PTName == "豪彩娱乐")
            {
                AppInfo.PTInfo = AppInfo.HCYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "LFHGSSC",
                    "BJSSC",
                    "TWSSC",
                    "JNDSSC",
                    "LFDJSSC",
                    "TXFFC",
                    "QQFFC",
                    "HCYLOZ3FC",
                    "HCYLSLFK5FC",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "HLJ11X5",
                    "JS11X5",
                    "XJ11X5",
                    "PK10",
                    "XYFTPK10"
                };
            }
            else if (this.PTName == "汇众娱乐")
            {
                AppInfo.PTInfo = AppInfo.HUIZInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HUIZFFC",
                    "HUIZ5FC",
                    "BJSSC",
                    "JNDSSC",
                    "TWSSC",
                    "HUIZHGSSC",
                    "HUIZJN15C",
                    "HUIZXJPSSC",
                    "ELSSSC",
                    "XXLSSC",
                    "TXFFC",
                    "HSAZXYC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.HUIZGJ)
                {
                    List<string> list158 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SD11X5",
                        "AH11X5",
                        "SH11X5",
                        "HUIZFF11X5",
                        "PK10",
                        "HUIZFFPK10",
                        "XYFTPK10"
                    };
                    pLotteryIDList.AddRange(list158);
                }
            }
            else if (this.PTName == "NB娱乐")
            {
                AppInfo.PTInfo = AppInfo.NBYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "HGSSC",
                    "NBTGSSC",
                    "NBRBSSC",
                    "WHDJSSC",
                    "WHXJPSSC",
                    "JDSSC",
                    "HG1FC",
                    "JBEITG30",
                    "TXFFC",
                    "NBFFC",
                    "NB5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.NBGJ)
                {
                    List<string> list160 = new List<string> { 
                        "GD11X5",
                        "SH11X5",
                        "JX11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list160);
                }
            }
            else if (this.PTName == "网赚娱乐")
            {
                AppInfo.PTInfo = AppInfo.WZYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "WZYLJDSSC",
                    "WZYLTXFFC",
                    "WZYLHGSSC",
                    "WZYLHG1FC",
                    "WZYLDJSSC",
                    "WZYLXJPSSC",
                    "WZYLXGSSC",
                    "WZYLLSWJS15C",
                    "WZYLMG35C",
                    "WZYLBL1FC",
                    "WZYLMG15C",
                    "ZBYZCP15C",
                    "SD11X5",
                    "GD11X5",
                    "SH11X5",
                    "JX11X5",
                    "LN11X5",
                    "WZYLTG11X5",
                    "WZYLBL11X5",
                    "PK10",
                    "WZYLJSPK10"
                };
            }
            else if (this.PTName == "亚洲彩票")
            {
                AppInfo.PTInfo = AppInfo.YZCPInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "YZCPYN5FC",
                    "YZCPTG15C",
                    "YZCPJSFFC",
                    "YZCPRBSSC",
                    "YZCPHGSSC",
                    "YZCPXJPSSC",
                    "YZCPJDSSC",
                    "YZCPMG35C",
                    "YZCPLSWJS15C",
                    "YZCPBL1FC",
                    "YZCPMG15C",
                    "YZCPHG1FC",
                    "YZCPTG30M",
                    "ZBYZCP15C",
                    "SD11X5",
                    "GD11X5",
                    "SH11X5",
                    "JX11X5",
                    "LN11X5",
                    "JS11X5",
                    "YZCPTG11X5",
                    "YZCPXG11X5",
                    "PK10",
                    "YZCPJSPK10"
                };
            }
            else if (this.PTName == "纵达娱乐")
            {
                AppInfo.PTInfo = AppInfo.ZDYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "ZDTGSSC",
                    "ZDDJSSC",
                    "ZDHS15F",
                    "ZDRBSSC",
                    "ZDFFC",
                    "ZD5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.DEBUG)
                {
                    List<string> list164 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "JX11X5",
                        "SH11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list164);
                }
            }
            else if (this.PTName == "Z娱乐")
            {
                AppInfo.PTInfo = AppInfo.ZYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "ZYLTGSSC",
                    "ZYLRBSSC",
                    "ZYLFFC",
                    "ZYL5FC",
                    "ZYLDJSSC",
                    "ZYLHS15F"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.ZYLGJ)
                {
                    List<string> list166 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "SH11X5",
                        "JX11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list166);
                }
            }
            else if (this.PTName == "大众娱乐")
            {
                AppInfo.PTInfo = AppInfo.DAZYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "DAZRBSSC",
                    "DAZFFC",
                    "DAZ5FC",
                    "DAZTG15C",
                    "DAZDJSSC",
                    "DAZJDSSC",
                    "DAZHGSSC",
                    "TXFFC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.DAZGJ)
                {
                    List<string> list168 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "LN11X5",
                        "SH11X5",
                        "JX11X5",
                        "JS11X5",
                        "DAZ11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list168);
                }
            }
            else if (this.PTName == "好友")
            {
                AppInfo.PTInfo = AppInfo.HYYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "HYGGFFC",
                    "HYTTFFC",
                    "HYSkyFFC",
                    "HYYJFFC",
                    "HYHLWFFC",
                    "HYYTFFC",
                    "HYTXFFC",
                    "HYHGSSC",
                    "HYDJSSC",
                    "HYFLBSSC",
                    "HYXDLSSC",
                    "HYXJPSSC",
                    "HYJNDFFC",
                    "HYHNFFC",
                    "HYFFC",
                    "HY2FC"
                };
                if ((AppInfo.App == ConfigurationStatus.AppType.NRLM) || (AppInfo.App == ConfigurationStatus.AppType.LHLM))
                {
                    List<string> list170 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "HY11X5",
                        "PK10",
                        "HYPK10"
                    };
                    pLotteryIDList.AddRange(list170);
                }
            }
            else if (this.PTName == "万美娱乐")
            {
                AppInfo.PTInfo = AppInfo.WMYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "WMFFC",
                    "WM2FC",
                    "JNDSSC",
                    "WMTWSSC",
                    "WMXXLSSC",
                    "WMHGSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.WMGJ)
                {
                    List<string> list172 = new List<string> { 
                        "GD11X5",
                        "SH11X5",
                        "JX11X5",
                        "SH11X5",
                        "PK10",
                        "WMPK10"
                    };
                    pLotteryIDList.AddRange(list172);
                }
            }
            else if (this.PTName == "宝汇国际")
            {
                AppInfo.PTInfo = AppInfo.BHGJInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "BHGJFFC",
                    "BHGJ2FC",
                    "BHGJ5FC",
                    "BHGJXXLSSC",
                    "BHGJHGSSC",
                    "BHGJHN15C",
                    "BHGJHNFFC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "SH11X5",
                    "PK10",
                    "BHGJPK10"
                };
            }
            else if (this.PTName == "TA娱乐")
            {
                AppInfo.PTInfo = AppInfo.TAYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TWSSC",
                    "BJSSC",
                    "TXFFC",
                    "TAWBFFC",
                    "TABL15C",
                    "TAWX15C",
                    "TAHGSSC",
                    "TAFLBSSC",
                    "TAFFC",
                    "TA3FC",
                    "TA5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.TAGJ)
                {
                    List<string> list175 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "JX11X5",
                        "SH11X5",
                        "AH11X5",
                        "JS11X5",
                        "TA11X5FFC",
                        "TA11X53FC",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list175);
                }
            }
            else if (this.PTName == "立鼎娱乐")
            {
                AppInfo.PTInfo = AppInfo.LDYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "LDYLWX15C",
                    "LDYLBL15C",
                    "TXFFC",
                    "LDYLWBFFC",
                    "LDYLFLBSSC",
                    "LDYLHGSSC",
                    "BJSSC",
                    "TWSSC",
                    "LDYLFFC",
                    "LDYL3FC",
                    "LDYL5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.LDYLGJ)
                {
                    List<string> list177 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "AH11X5",
                        "JS11X5",
                        "LDYL11X5FFC",
                        "LDYL11X53FC",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list177);
                }
            }
            else if (this.PTName == "轩彩娱乐")
            {
                AppInfo.PTInfo = AppInfo.XCYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "XCTXFFC",
                    "XCQQFFC",
                    "XCHGSSC",
                    "XCFLBSSC",
                    "BJSSC",
                    "XCWX15C",
                    "XCBL15C",
                    "XCYNFFC",
                    "XCYN3FC",
                    "XCYN5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.XCGJ)
                {
                    List<string> list179 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "JX11X5",
                        "SH11X5",
                        "AH11X5",
                        "JS11X5",
                        "XC11X5FFC",
                        "XC11X53FC",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list179);
                }
            }
            else if (this.PTName == "开元娱乐")
            {
                AppInfo.PTInfo = AppInfo.KYYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "TXFFC",
                    "KYHGSSC",
                    "KYBL15C",
                    "KYWX15C",
                    "KYFLBSSC",
                    "KYWBFFC",
                    "KYYNFFC",
                    "KYYN3FC",
                    "KYYN5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.KYYLGJ)
                {
                    List<string> list181 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "AH11X5",
                        "JS11X5",
                        "KY11X5FFC",
                        "KY11X53FC",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list181);
                }
            }
            else if (this.PTName == "凯鑫娱乐")
            {
                AppInfo.PTInfo = AppInfo.KXYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "TXFFC",
                    "KXHGSSC",
                    "KXBL15C",
                    "KXWX15C",
                    "KXFLBSSC",
                    "KXWBFFC",
                    "KXYNFFC",
                    "KXYN3FC",
                    "KXYN5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.KXYLGJ)
                {
                    List<string> list183 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "AH11X5",
                        "JS11X5",
                        "KX11X5FFC",
                        "KX11X53FC",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list183);
                }
            }
            else if (this.PTName == "华众娱乐")
            {
                AppInfo.PTInfo = AppInfo.HZYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TXFFC",
                    "TWSSC",
                    "HZQQFFC",
                    "HZFFC",
                    "HZ3FC",
                    "HZ5FC",
                    "HZXXLSSC",
                    "HZJNDSSC",
                    "HZHG2FC",
                    "HZXJP2FC",
                    "HZHGSSC",
                    "HZDJSSC",
                    "HZXDLSSC",
                    "HZTG15F",
                    "HZML15F"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.HZGJ)
                {
                    List<string> list185 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "AH11X5",
                        "JS11X5",
                        "HZFF11X5",
                        "HZTG11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list185);
                }
            }
            else if (this.PTName == "在线娱乐")
            {
                AppInfo.PTInfo = AppInfo.ZXYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TXFFC",
                    "TWSSC",
                    "ZXGBFFC",
                    "ZXBX3FC",
                    "ZXBX5FC",
                    "ZXXXLSSC",
                    "ZXJNDSSC",
                    "ZXHG2FC",
                    "ZXXJP2FC",
                    "ZXHGSSC",
                    "ZXDJSSC",
                    "ZXXDLSSC",
                    "ZXTG15F",
                    "ZXML15F"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.ZXGJ)
                {
                    List<string> list187 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "ZXFF11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list187);
                }
            }
            else if (this.PTName == "彩宏娱乐")
            {
                AppInfo.PTInfo = AppInfo.CAIHInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "CAIHDLD30M",
                    "TXFFC",
                    "CAIHXWYFFC",
                    "CAIHXDLSSC",
                    "CAIHHGSSC",
                    "CAIHDJSSC",
                    "CAIHXJPSSC",
                    "CAIHLD2FC",
                    "CAIHSE15F",
                    "CAIHNY15C"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.CAIHGJ)
                {
                    List<string> list189 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "SD11X5",
                        "JS11X5",
                        "SH11X5",
                        "PK10",
                        "CAIHPK10"
                    };
                    pLotteryIDList.AddRange(list189);
                }
            }
            else if (this.PTName == "彩部落")
            {
                AppInfo.PTInfo = AppInfo.CBLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "CBLDJSSC",
                    "CBLXJPSSC",
                    "CBLLD2FC",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "CBLXDLSSC",
                    "CBLHGSSC",
                    "CBLDLD30M",
                    "TXFFC",
                    "CBLXWYFFC",
                    "CBLSE15F",
                    "CBLNY15C"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.CBLGJ)
                {
                    List<string> list191 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "SH11X5",
                        "JX11X5",
                        "JS11X5",
                        "PK10",
                        "CBLPK10"
                    };
                    pLotteryIDList.AddRange(list191);
                }
            }
            else if (this.PTName == "马泰娱乐")
            {
                AppInfo.PTInfo = AppInfo.MTYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "MTSE15F",
                    "MTDLD30M",
                    "TXFFC",
                    "MTXWYFFC",
                    "MTXDLSSC",
                    "MTHGSSC",
                    "MTDJSSC",
                    "MTXJPSSC",
                    "MTLD2FC",
                    "MTNY15C"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.MTYLGJ)
                {
                    List<string> list193 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "SH11X5",
                        "JX11X5",
                        "JS11X5",
                        "PK10",
                        "MTPK10"
                    };
                    pLotteryIDList.AddRange(list193);
                }
            }
            else if (this.PTName == "新泰娱乐")
            {
                AppInfo.PTInfo = AppInfo.XTYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "XTYLXDLSSC",
                    "XTYLHGSSC",
                    "XTYLDLD30M",
                    "TXFFC",
                    "XTYLXWYFFC",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "XTYLDJSSC",
                    "XTYLXJPSSC",
                    "XTYLLD2FC",
                    "GD11X5",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "PK10",
                    "XTYLPK10"
                };
            }
            else if (this.PTName == "彩云娱乐")
            {
                AppInfo.PTInfo = AppInfo.CYYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "CYYLXJPSSC",
                    "CYYLLD2FC",
                    "CYYLDJSSC",
                    "CYYLSE15F",
                    "CYYLXDLSSC",
                    "CYYLXWYFFC",
                    "CYYLDLD30M",
                    "CYYLHGSSC",
                    "TXFFC",
                    "CYYLNY15C",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "GD11X5",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "鲸鱼娱乐")
            {
                AppInfo.PTInfo = AppInfo.JYYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "JYYLTX30M",
                    "JYYLQQFFC",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "JYYLXDLSSC",
                    "JYYLXWYFFC",
                    "JYYLXJPSSC",
                    "JYYLLD2FC",
                    "JYYLHGSSC",
                    "JYYLDJSSC",
                    "VRSSC",
                    "GD11X5",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "JS11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "梦想娱乐")
            {
                AppInfo.PTInfo = AppInfo.MXYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "MXYLHGSSC",
                    "MXYLDJSSC",
                    "MXYLXDLSSC",
                    "MXYLXWYFFC",
                    "MXYLXJPSSC",
                    "MXYLLD2FC",
                    "MXYLTX30M",
                    "MXYLQQFFC",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "VRSSC",
                    "GD11X5",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "JS11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "永恒时光")
            {
                AppInfo.PTInfo = AppInfo.YHSGInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "YHSGXDLSSC",
                    "YHSGHGSSC",
                    "YHSGXWYFFC",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "YHSGDJSSC",
                    "YHSGXJPSSC",
                    "YHSGLD2FC",
                    "GD11X5",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "JS11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "万彩")
            {
                AppInfo.PTInfo = AppInfo.WCAIInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "BJSSC",
                    "WCAIHGSSC",
                    "WCAIDJSSC",
                    "WCAIXDLSSC",
                    "WCAIXWYFFC",
                    "WCAIXJPSSC",
                    "WCAILD2FC",
                    "WCAIMBE30M",
                    "WCAITWSSC",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "GD11X5",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "彩天下")
            {
                AppInfo.PTInfo = AppInfo.CTXInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "CTXXDLSSC",
                    "CTXHGSSC",
                    "CTXDLD30M",
                    "TXFFC",
                    "CTXXWYFFC",
                    "CTXSE15F",
                    "CTXNY15C",
                    "CAIHFLP15C",
                    "CAIHFLP2FC",
                    "CAIHFLP5FC",
                    "BJSSC",
                    "TWSSC",
                    "CTXDJSSC",
                    "CTXXJPSSC",
                    "CTXLD2FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.CTXGJ)
                {
                    List<string> list201 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "SH11X5",
                        "JX11X5",
                        "PK10",
                        "CTXPK10"
                    };
                    pLotteryIDList.AddRange(list201);
                }
            }
            else if (this.PTName == "航洋国际")
            {
                AppInfo.PTInfo = AppInfo.HANYInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "XDLSSC",
                    "LD2FC",
                    "JZD15FC",
                    "HANYDJSSC",
                    "HANYHGSSC",
                    "BLSFFC",
                    "HANYTXFFC",
                    "HANYXJP30M",
                    "HANYFLP30M",
                    "HANYFLPFFC",
                    "HANYFLP2FC",
                    "HANYJPZ30M",
                    "HANYJPZFFC",
                    "HANYJPZ5FC",
                    "HANYMD30M",
                    "HANYMDFFC",
                    "HANYMD3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.HANYGJ)
                {
                    List<string> list203 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "JX11X5",
                        "AH11X5",
                        "SH11X5",
                        "XDL11X5",
                        "JZD11X5",
                        "LD11X5",
                        "PK10",
                        "XDLPK10",
                        "JZDPK10",
                        "LDPK10"
                    };
                    pLotteryIDList.AddRange(list203);
                }
            }
            else if (this.PTName == "亿皇娱乐")
            {
                AppInfo.PTInfo = AppInfo.YHYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TJSSC",
                    "TXFFC",
                    "YHYLTXFFC",
                    "YHYLQQFFC",
                    "YHYLWXFFC",
                    "YHYLFLPFFC",
                    "YHYLJNDFFC",
                    "YHYLBLSFFC",
                    "YHYLXHG15F",
                    "YHYLXDL15F",
                    "YHYLFLP2FC",
                    "YHYLFLP45M",
                    "YHYLXHG45M",
                    "YHYLBD45M",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "SH11X5",
                    "AH11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "亿博娱乐")
            {
                AppInfo.PTInfo = AppInfo.YBYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "YBFFC",
                    "YB3FC",
                    "YBBLSFFC",
                    "TXFFC",
                    "YBTXFFC",
                    "YBHG15F",
                    "YBXDL15F",
                    "XJPSSC",
                    "JZD15FC",
                    "LD2FC",
                    "YBNHG15F",
                    "YBDJSSC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "SH11X5",
                    "AH11X5",
                    "XDL11X5",
                    "JZD11X5",
                    "PK10",
                    "XDLPK10",
                    "JZDPK10",
                    "LDPK10"
                };
            }
            else if (this.PTName == "万兴国际")
            {
                AppInfo.PTInfo = AppInfo.YDYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TJSSC",
                    "XJSSC",
                    "YDFFC",
                    "YD2FC",
                    "YDXXLSSC",
                    "YDTXFFC",
                    "YDHGSSC",
                    "WHDJSSC",
                    "BJSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.YDGJ)
                {
                    List<string> list207 = new List<string> { 
                        "YDFFPK10",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list207);
                }
            }
            else if (this.PTName == "博客彩")
            {
                AppInfo.PTInfo = AppInfo.BKCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "JN15F",
                    "BKCFFC",
                    "BKC2FC",
                    "BKC5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.BKCGJ)
                {
                    List<string> list209 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "ZJ11X5",
                        "BKC11X5FFC",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list209);
                }
            }
            else if (this.PTName == "豪客彩")
            {
                AppInfo.PTInfo = AppInfo.HKCInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "JN15F",
                    "HKCFFC",
                    "HKC2FC",
                    "HKC5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.HKCGJ)
                {
                    List<string> list211 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "ZJ11X5",
                        "AH11X5",
                        "HKC11X5FFC",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list211);
                }
            }
            else if (this.PTName == "地球娱乐")
            {
                AppInfo.PTInfo = AppInfo.DQYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "LFHGSSC",
                    "BJSSC",
                    "TWSSC",
                    "DQOZ3FC",
                    "DQSLFK5FC",
                    "TXFFC",
                    "QQFFC",
                    "LFDJSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.DQGJ)
                {
                    List<string> list213 = new List<string> { 
                        "SD11X5",
                        "SH11X5",
                        "JX11X5",
                        "HLJ11X5",
                        "JS11X5",
                        "XJ11X5",
                        "PK10",
                        "XYFTPK10"
                    };
                    pLotteryIDList.AddRange(list213);
                }
            }
            else if (this.PTName == "天豪娱乐")
            {
                AppInfo.PTInfo = AppInfo.THYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "THFFC",
                    "THMD2FC",
                    "THDJSSC",
                    "TXFFC",
                    "THJDSSC",
                    "THTGSSC",
                    "TH5FC",
                    "THHGSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.THGJ)
                {
                    List<string> list215 = new List<string> { 
                        "GD11X5",
                        "JX11X5",
                        "HLJ11X5",
                        "AH11X5",
                        "SH11X5",
                        "LN11X5",
                        "JS11X5",
                        "PK10",
                        "THPK10",
                        "THOZPK10"
                    };
                    pLotteryIDList.AddRange(list215);
                }
            }
            else if (this.PTName == "K3娱乐")
            {
                AppInfo.PTInfo = AppInfo.K3YLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "K3TXFFC",
                    "K3NTXFFC",
                    "K3XXLSSC",
                    "K3DJFFC",
                    "K3HG2FC",
                    "K3MG5FC",
                    "K3HGSSC",
                    "K3DJSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.K3GJ)
                {
                    List<string> list217 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "LN11X5",
                        "JX11X5",
                        "K311X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list217);
                }
            }
            else if (this.PTName == "恒瑞")
            {
                AppInfo.PTInfo = AppInfo.HENRInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HENRTXFFC",
                    "HENRDJ2FC",
                    "HENRXJPSSC",
                    "HENROZ15C",
                    "HENRXG15C",
                    "HENRFFC",
                    "HENR2FC",
                    "HENR3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.HENRGJ)
                {
                    List<string> list219 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "SH11X5",
                        "JX11X5",
                        "AH11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list219);
                }
            }
            else if (this.PTName == "长隆娱乐")
            {
                AppInfo.PTInfo = AppInfo.CLYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "CLYLAM15C",
                    "CLYLTB15C",
                    "CLYLDJSSC",
                    "CLYLXDL15F",
                    "CLYLHGSSC",
                    "CLYLFFC",
                    "CLYL2FC",
                    "CLYL3FC",
                    "CLYL5FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.CLYLGJ)
                {
                    List<string> list221 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "CLYLFF11X5",
                        "CLYL2F11X5",
                        "CLYL3F11X5",
                        "CLYL5F11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list221);
                }
            }
            else if (this.PTName == "众博娱乐")
            {
                AppInfo.PTInfo = AppInfo.ZBYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "ZBYLGGFFC",
                    "ZBYLTX2FC",
                    "ZBYLFFC",
                    "ZBYL2FC",
                    "ZBYL3FC",
                    "ZBYL5FC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "JS11X5",
                    "ZJ11X5",
                    "SH11X5",
                    "ZBYLFF11X5",
                    "ZBYL2F11X5",
                    "ZBYL3F11X5",
                    "ZBYL5F11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "2028娱乐")
            {
                AppInfo.PTInfo = AppInfo.YL2028Info;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "YL2028HGSSC",
                    "YL2028DJSSC",
                    "YL2028ML20M",
                    "YL2028DZ30M",
                    "YL2028WXFFC",
                    "YL2028FLP2FC",
                    "YL2028MG45M",
                    "TXFFC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.YL28)
                {
                    List<string> list224 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "AH11X5",
                        "SH11X5",
                        "SXR11X5",
                        "PK10",
                        "YL2028PK10",
                        "YL2028FFPK10"
                    };
                    pLotteryIDList.AddRange(list224);
                }
            }
            else if (this.PTName == "威霆娱乐")
            {
                AppInfo.PTInfo = AppInfo.WTYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TXFFC",
                    "WTYLTXFFC",
                    "WTYLXXL15C",
                    "WTYLHG15C",
                    "WTYLNY2FC",
                    "WTYLJNDSSC",
                    "WTYLXDL35C",
                    "WTYLAZ5FC",
                    "PK10"
                };
            }
            else if (this.PTName == "琥珀游戏")
            {
                AppInfo.PTInfo = AppInfo.HUBOInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "BJSSC",
                    "TWSSC",
                    "HUBOTXFFC",
                    "HUBOWXFFC",
                    "HUBOFFC",
                    "HUBO2FC",
                    "HUBO5FC",
                    "HUBOHGSSC",
                    "HUBODJSSC",
                    "HUBOFLP2FC",
                    "HUBODZ30M",
                    "HUBOML20M",
                    "HUBOMG45M"
                };
            }
            else if (this.PTName == "数亿娱乐")
            {
                AppInfo.PTInfo = AppInfo.SYYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TJSSC",
                    "SYYLFFC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.SYYLGJ)
                {
                    List<string> list228 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "JX11X5",
                        "PK10"
                    };
                    pLotteryIDList.AddRange(list228);
                }
            }
            else if (this.PTName == "宏达娱乐")
            {
                AppInfo.PTInfo = AppInfo.HDYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "HLJSSC",
                    "TXFFC",
                    "BJSSC",
                    "TWSSC",
                    "JNDSSC",
                    "HDYLFFC",
                    "HDYL2FC",
                    "HDYL5FC",
                    "HDYLASKFFC"
                };
                if ((AppInfo.App == ConfigurationStatus.AppType.HDYLGJ) || (AppInfo.App == ConfigurationStatus.AppType.HDGJ))
                {
                    List<string> list230 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "HDYLFF11X5",
                        "HDYL2F11X5",
                        "HDYL5F11X5",
                        "PK10",
                        "XYFTPK10",
                        "HDYLFFPK10",
                        "HDYLFFFT"
                    };
                    pLotteryIDList.AddRange(list230);
                }
            }
            else if (this.PTName == "香格里拉")
            {
                AppInfo.PTInfo = AppInfo.XGLLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "XGLLWYN30M",
                    "XGLLDJSSC",
                    "XGLLLSJ2FC",
                    "XGLLFLP15C",
                    "XGLLWNS15C",
                    "XGLLLFFC",
                    "XGLLL2FC",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "JS11X5",
                    "XGLLLSY11X5",
                    "PK10",
                    "XYFTPK10",
                    "XGLLLDPK10",
                    "XGLLSSPK10"
                };
            }
            else if (this.PTName == "长城娱乐")
            {
                AppInfo.PTInfo = AppInfo.CCYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "CCTJ3FC",
                    "CCTJ5FC",
                    "CCTG60M",
                    "CCXG15C",
                    "CCFLP15C",
                    "CCRD2FC",
                    "CCWXFFC",
                    "VRSSC",
                    "VRHXSSC",
                    "VR3FC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.CCGJ)
                {
                    List<string> list233 = new List<string> { 
                        "GD11X5",
                        "SD11X5",
                        "JX11X5",
                        "CCTW11X5",
                        "CCAM11X5",
                        "CCXG11X5",
                        "PK10",
                        "CCFFPK10"
                    };
                    pLotteryIDList.AddRange(list233);
                }
            }
            else if (this.PTName == "金诚信")
            {
                AppInfo.PTInfo = AppInfo.JCXInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "TXFFC",
                    "JCXQQFFC",
                    "JCXXXLSSC",
                    "JCXMG45M",
                    "JCXMJFFC",
                    "JCXNYFFC",
                    "JCXFLPFFC",
                    "JCXMG15C",
                    "JCXHN15C",
                    "JCXNY15C",
                    "JCXXDLSSC",
                    "JCXNHGSSC",
                    "JCXNDJSSC"
                };
                if (AppInfo.App == ConfigurationStatus.AppType.JCXGJ)
                {
                    List<string> list235 = new List<string> { 
                        "SD11X5",
                        "GD11X5",
                        "JX11X5",
                        "SH11X5",
                        "AH11X5",
                        "JCXJN11X5",
                        "PK10",
                        "JCX60MPK10"
                    };
                    pLotteryIDList.AddRange(list235);
                }
            }
            else if (this.PTName == "天博娱乐")
            {
                AppInfo.PTInfo = AppInfo.TBYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "BJSSC",
                    "TWSSC",
                    "TXFFC",
                    "TBYLQQFFC",
                    "TBYLMG15C",
                    "TBYLSE15C",
                    "TBYLNY15C",
                    "TBYLXDLSSC",
                    "TBYLNHGSSC",
                    "TBYLNDJSSC",
                    "TBYFLP15C",
                    "TBYLJND35C",
                    "TBYLJND5FC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "SH11X5",
                    "AH11X5",
                    "TBYLJS11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "永信在线")
            {
                AppInfo.PTInfo = AppInfo.YXZXInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "YXZXQQFFC",
                    "YXZXMG15C",
                    "YXZXSE15C",
                    "YXZXNY15C",
                    "YXZXXDLSSC",
                    "YXZXNHGSSC",
                    "YXZXNDJSSC",
                    "YXZXFLP15C",
                    "YXZXJND35C",
                    "YXZXJND5FC",
                    "YXZXXXLSSC",
                    "YXZXHN45M",
                    "YXZXMG45M",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "SH11X5",
                    "AH11X5",
                    "YXZXTW11X5",
                    "PK10",
                    "YXZXTWPK10"
                };
            }
            else if (this.PTName == "宏川在线")
            {
                AppInfo.PTInfo = AppInfo.HCZXInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "HCZX5FC",
                    "HCZXJNDSSC",
                    "HCZXDJSSC",
                    "HCZXFFC",
                    "HCZXMG45M",
                    "HCZXNTXFFC",
                    "XXLSSC",
                    "TXFFC",
                    "GD11X5",
                    "SD11X5",
                    "PK10",
                    "HCZXFFPK10",
                    "HCZX3FPK10"
                };
            }
            else if (this.PTName == "斗鱼娱乐")
            {
                AppInfo.PTInfo = AppInfo.DYYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "DYFFC",
                    "DYDJSSC",
                    "DYHGSSC",
                    "DY2FC",
                    "DY3FC",
                    "DY5FC",
                    "GD11X5",
                    "SD11X5",
                    "AH11X5",
                    "JX11X5",
                    "FJ11X5",
                    "DYFF11X5",
                    "DY2F11X5",
                    "DY3F11X5",
                    "DY5F11X5",
                    "PK10",
                    "DYFFPK10",
                    "DY2FPK10",
                    "DY3FPK10",
                    "DY5FPK10"
                };
            }
            else if (this.PTName == "澳门巴黎人")
            {
                AppInfo.PTInfo = AppInfo.AMBLRInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "AMBLRTXFFC",
                    "AMBLRAMSSC",
                    "AMBLRTWSSC",
                    "AMBLRQQFFC",
                    "AMBLRHN2FC",
                    "AMBLRHN5FC",
                    "AMBLRBX15F",
                    "AMBLRBXKLC",
                    "PK10",
                    "AMBLRAMPK10",
                    "AMBLRTWPK10"
                };
            }
            else if (this.PTName == "银河")
            {
                AppInfo.PTInfo = AppInfo.YINHInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "YINHTXFFC",
                    "YINHQQFFC",
                    "YINH2FC",
                    "YINH5FC",
                    "YINHDJSSC",
                    "GD11X5",
                    "JX11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "NBA娱乐")
            {
                AppInfo.PTInfo = AppInfo.NBAYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "NBABJSSC",
                    "NBATWSSC",
                    "NBANY5FC",
                    "NBANY2FC",
                    "NBAQQFFC",
                    "TXFFC",
                    "NBAHGSSC",
                    "NBADJSSC",
                    "NBAXDLSSC",
                    "NBANDJSSC",
                    "NBAFLP15C",
                    "NBAXXLSSC",
                    "GD11X5",
                    "SD11X5",
                    "JX11X5",
                    "SH11X5",
                    "AH11X5",
                    "NBAJN11X5",
                    "NBA5F11X5",
                    "NBA3F11X5",
                    "NBAFF11X5",
                    "PK10",
                    "NBA60MPK10",
                    "NBA180MPK10",
                    "NBAJZDPK10"
                };
            }
            else if (this.PTName == "天宇娱乐")
            {
                AppInfo.PTInfo = AppInfo.TIYUInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "TJSSC",
                    "HLJSSC",
                    "TXFFC",
                    "TIYUML2FC",
                    "TIYUFLP15C",
                    "TIYUNHG15C",
                    "TIYUXDL15C",
                    "TIYUXJPSSC",
                    "SD11X5",
                    "AH11X5",
                    "JX11X5",
                    "SH11X5",
                    "LN11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "飞牛游戏")
            {
                AppInfo.PTInfo = AppInfo.FNYXInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TXFFC",
                    "FNYXFFC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "SH11X5",
                    "FNYXHY11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "聚鑫娱乐")
            {
                AppInfo.PTInfo = AppInfo.JXINInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "HLJSSC",
                    "TXFFC",
                    "JXIN2FC",
                    "JXINFFC",
                    "JXIN5FC",
                    "JXINFLP15C",
                    "JXINMSK15C",
                    "JXINJLP15C",
                    "JXINDB15C",
                    "JXINDJSSC",
                    "JXINXJPSSC",
                    "GD11X5",
                    "SD11X5",
                    "SH11X5",
                    "JX11X5",
                    "PK10"
                };
            }
            else if (this.PTName == "弘尚娱乐")
            {
                AppInfo.PTInfo = AppInfo.HSYLInfo;
                pLotteryIDList = new List<string> { 
                    "CQSSC",
                    "XJSSC",
                    "TJSSC",
                    "SD11X5",
                    "GD11X5",
                    "JX11X5",
                    "PK10"
                };
            }
            this.FillPTLineList();
            if (AppInfo.Account.PTUserList.ContainsKey(AppInfo.PTInfo.PTID))
            {
                string str3 = AppInfo.Account.PTUserList[AppInfo.PTInfo.PTID];
                this.Txt_ID.Text = str3;
                this.Txt_ID.ReadOnly = true;
                this.Lbl_IDHint.Visible = true;
            }
            else
            {
                this.Txt_ID.ReadOnly = false;
                this.Lbl_IDHint.Visible = false;
            }
            if (((AppInfo.PTInfo == AppInfo.WDYLInfo) || (AppInfo.PTInfo == AppInfo.CCYLInfo)) || (AppInfo.PTInfo == AppInfo.HUAYInfo))
            {
                this.Lbl_PW.Text = "挂机令牌：";
                string pHint = "先去平台申请挂机令牌，以后就可以直接使用挂机";
                CommFunc.SetControlHint(this.Err_Hint, this.Lbl_PW, pHint);
                this.Ckb_PWPaste.Visible = this.Ckb_PWClear.Visible = true;
            }
            else
            {
                this.Lbl_PW.Text = "会员密码：";
                CommFunc.SetControlHint(this.Err_Hint, this.Lbl_PW, "");
                this.Ckb_PWPaste.Visible = this.Ckb_PWClear.Visible = false;
            }
            AppInfo.Current = new ConfigurationStatus(pLotteryIDList);
            List<string> list248 = CommFunc.ConvertLotteryNameList(pLotteryIDList);
            CommFunc.SetComboBoxList(this.Cbb_Lottery, list248);
        }

        private void LoadPTLineList()
        {
            List<int> pType = new List<int> { 
                1,
                1
            };
            List<string> pText = new List<string> { 
                "服务器",
                "状态"
            };
            List<int> pWidth = new List<int> { 
                10,
                60
            };
            List<bool> pRead = new List<bool> { 
                true,
                true
            };
            List<bool> pVis = new List<bool> { 
                true,
                true
            };
            this.Egv_PTLineList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_PTLineList.MultiSelect = false;
            this.Egv_PTLineList.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_PTLineList, 9);
            this.Egv_PTLineList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Egv_PTLineList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_PTLineList_CellValueNeeded);
            for (int i = 0; i < this.Egv_PTLineList.ColumnCount; i++)
            {
                this.Egv_PTLineList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.Egv_PTLineList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private int LoadSchemeData(string path, bool pIsFirstLoad)
        {
            string str2;
            ConfigurationStatus.Scheme schemeByFileValue;
            int num = 0;
            if (pIsFirstLoad)
            {
                CommFunc.CreateDirectory(path);
                CommFunc.CutPathFile(this.SchemeOldPath, path);
                AppInfo.Account.SchemeDic.Clear();
                AppInfo.SchemeList.Clear();
            }
            DirectoryInfo info = new DirectoryInfo(path);
            foreach (FileInfo info2 in info.GetFiles("*.txt"))
            {
                try
                {
                    str2 = Path.GetFileNameWithoutExtension(info2.FullName).Split(new char[] { '-' })[0];
                    schemeByFileValue = ConfigurationStatus.Scheme.GetSchemeByFileValue(CommFunc.ReadTextFileToStr(info2.FullName), str2);
                    if (schemeByFileValue != null)
                    {
                        AppInfo.Account.SchemeDic[str2] = schemeByFileValue;
                        if (!AppInfo.SchemeList.Contains(str2))
                        {
                            AppInfo.SchemeList.Add(str2);
                        }
                        num++;
                    }
                }
                catch
                {
                }
            }
            if (!AppInfo.IsAppLoaded && (AppInfo.Account.SchemeDic.Count == 0))
            {
                for (int i = 1; i <= this.CHTypeList.Count; i++)
                {
                    str2 = $"方案{i}";
                    string pCHType = this.CHTypeList[i - 1];
                    schemeByFileValue = new ConfigurationStatus.Scheme(false, str2, pCHType, "", false, false);
                    AppInfo.Account.SchemeDic[str2] = schemeByFileValue;
                    AppInfo.SchemeList.Add(str2);
                }
                this.SaveAllSchemeData("");
            }
            this.SortSchemeList(AppInfo.SchemeList);
            this.RefreshAllFNList();
            this.RefreshScheme();
            this.RefreshShareBetsControl();
            this.RefreshShareSchemeControl();
            this.Egv_SchmeeList_CellClick(null, null);
            return num;
        }

        private void LoadSchemeList()
        {
            List<int> pType = new List<int> { 
                0,
                1,
                1,
                1
            };
            List<string> pText = new List<string> { 
                "方案",
                "类别",
                "玩法"
            };
            List<int> pWidth = new List<int> { 
                0x15,
                60,
                120,
                120
            };
            List<bool> pRead = new List<bool> { 
                false,
                true,
                true,
                true
            };
            List<bool> pVis = new List<bool> { 
                true,
                true,
                true,
                true
            };
            this.Egv_SchemeList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_SchemeList.MultiSelect = false;
            this.Egv_SchemeList.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_SchemeList, 9);
            this.Egv_SchemeList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Egv_SchemeList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_SchemeList_CellValueNeeded);
            this.Egv_SchemeList.CellValuePushed += new DataGridViewCellValueEventHandler(this.Egv_SchemeList_CellValuePushed);
            for (int i = 0; i < this.Egv_SchemeList.ColumnCount; i++)
            {
                this.Egv_SchemeList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void LoadShowTapData()
        {
            ConfigurationStatus.ShowTap tap;
            string pStr = CommFunc.ReadRegString(this.RegConfigPath, "ShowTapData", "");
            if (pStr != "")
            {
                List<string> list = CommFunc.SplitString(pStr, ",", -1);
                foreach (string str2 in list)
                {
                    tap = new ConfigurationStatus.ShowTap(str2);
                    if (tap.Name != null)
                    {
                        this.ShowTapList.Add(tap);
                    }
                }
            }
            if (this.ShowTapList.Count != this.SettingPageList.Count)
            {
                this.ShowTapList.Clear();
            }
            if (this.ShowTapList.Count == 0)
            {
                for (int i = 0; i < this.SettingPageList.Count; i++)
                {
                    TabPage page = this.SettingPageList[i];
                    bool pSelected = (page == this.Tap_TrendView) || (page == this.Tap_Setting);
                    tap = new ConfigurationStatus.ShowTap(page.Tag.ToString(), pSelected, i);
                    this.ShowTapList.Add(tap);
                }
            }
            this.RefreshShowTapList();
            this.RefreshTab(true);
        }

        private void LoadShowTapList()
        {
            List<int> pType = new List<int> { 
                0,
                1
            };
            List<string> pText = new List<string> { "辅助功能" };
            List<int> pWidth = new List<int> { 
                0x15,
                60
            };
            List<bool> pRead = new List<bool> { 
                false,
                true
            };
            List<bool> pVis = new List<bool> { 
                true,
                true
            };
            this.Egv_ShowTapList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_ShowTapList.MultiSelect = false;
            this.Egv_ShowTapList.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_ShowTapList, 9);
            this.Egv_ShowTapList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Egv_ShowTapList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_ShowTapList_CellValueNeeded);
            this.Egv_ShowTapList.CellValuePushed += new DataGridViewCellValueEventHandler(this.Egv_ShowTapList_CellValuePushed);
            for (int i = 0; i < this.Egv_ShowTapList.ColumnCount; i++)
            {
                this.Egv_ShowTapList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void LoadTime()
        {
            this.ResetTime();
            this.FillNextTime();
            if (!this.Tim_NextExpect.Enabled)
            {
                this.Tim_NextExpect.Start();
                this.Tim_NextExpect.Tick += new EventHandler(this.Tim_NextExpect_Tick);
            }
        }

        private void LoadTJDataList1()
        {
            List<int> pType = new List<int> { 
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1
            };
            List<string> pText = new List<string> { 
                "日期",
                "当天总投注次数",
                "当天中次数",
                "当天不中次数",
                "当天最多盈利金额",
                "当天最多亏损金额 ",
                "当天总投注额",
                "当天最终输赢金额",
                "当天最多连错次数",
                "当天总炸次数",
                "炸人凶手"
            };
            List<int> pWidth = new List<int> { 
                90,
                10,
                10,
                10,
                10,
                10,
                10,
                10,
                10,
                10,
                10
            };
            List<bool> pRead = new List<bool> { 
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true
            };
            List<bool> pVis = new List<bool> { 
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true
            };
            this.Egv_TJDataList1.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_TJDataList1.MultiSelect = false;
            this.Egv_TJDataList1.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_TJDataList1, 9);
            this.Egv_TJDataList1.Columns[this.Egv_TJDataList1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Egv_TJDataList1.Columns[this.Egv_TJDataList1.Columns.Count - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.Egv_TJDataList1.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_TJDataList1_CellValueNeeded);
            for (int i = 0; i < this.Egv_TJDataList1.ColumnCount; i++)
            {
                this.Egv_TJDataList1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.Egv_TJDataList1.AutoResizeColumns();
        }

        private void LoadTJDataList2()
        {
            int num2;
            int num = 0x12;
            List<int> pType = new List<int>();
            List<string> list2 = new List<string> { 
                "日期",
                "时间",
                "期号",
                "万",
                "千",
                "百",
                "十",
                "个"
            };
            List<string> list3 = new List<string> { 
                "注数",
                "倍数",
                "轮次",
                "金额",
                "内容",
                "中挂",
                "错误"
            };
            List<string> list4 = new List<string> { 
                "当期盈亏",
                "当日盈亏",
                "累计盈亏"
            };
            List<int> list5 = new List<int> { 
                80,
                50,
                90,
                30,
                30,
                30,
                30,
                30
            };
            List<int> list6 = new List<int> { 
                50,
                50,
                50,
                50,
                50,
                50,
                50
            };
            List<int> list7 = new List<int> { 
                70,
                70,
                70
            };
            List<bool> pRead = new List<bool>();
            List<bool> pVis = new List<bool>();
            for (num2 = 0; num2 < num; num2++)
            {
                pType.Add(1);
                pRead.Add(true);
                pVis.Add(true);
            }
            list2 = CommFunc.CombinaList<string>(CommFunc.CombinaList<string>(list2, list3), list4);
            list5 = CommFunc.CombinaList<int>(CommFunc.CombinaList<int>(list5, list6), list7);
            this.Egv_TJDataList2.LoadInitialization(pType, list2, list5, pRead, pVis, null);
            this.Egv_TJDataList2.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.Egv_TJDataList2.MultiSelect = true;
            this.Egv_TJDataList2.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_TJDataList2, 9);
            this.Egv_TJDataList2.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            this.Egv_TJDataList2.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_TJDataList2_CellValueNeeded);
            for (num2 = 0; num2 < this.Egv_TJDataList2.ColumnCount; num2++)
            {
                this.Egv_TJDataList2.Columns[num2].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.Egv_TJDataList2.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void LoadWebLoginIndex()
        {
            if (AppInfo.Account.Configuration.WebUrl != "")
            {
                if (AppInfo.Account.Configuration.WebUrl.Contains("http://") || AppInfo.Account.Configuration.WebUrl.Contains("https://"))
                {
                    this.Web_Login.Navigate(AppInfo.Account.Configuration.WebUrl);
                }
            }
            else
            {
                this.Web_Login.Url = null;
            }
        }

        public void LoginIPVerify()
        {
        }

        private void LoginMain()
        {
            if (!AppInfo.PTInfo.IsLoginRun)
            {
                if (!AppInfo.PTInfo.PTLoginStatus)
                {
                    if (this.Txt_ID.Text == "")
                    {
                        CommFunc.PublicMessageAll("输入账号不能为空！", true, MessageBoxIcon.Asterisk, "");
                        this.Txt_ID.Focus();
                    }
                    else if (this.Txt_PW.Text == "")
                    {
                        CommFunc.PublicMessageAll("输入密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                        this.Txt_PW.Focus();
                    }
                    else
                    {
                        this.RefreshLogin(true, "");
                        AppInfo.Account.PTID = this.Txt_ID.Text;
                        AppInfo.Account.PTPW = this.Txt_PW.Text;
                        AppInfo.Account.PTName = this.Cbb_LoginPT.Text;
                        if (AppInfo.PTInfo.IsLoadBets)
                        {
                            this.LoadPlanListData();
                            this.VerificationBetsPlan();
                            AppInfo.PTInfo.IsLoadBets = false;
                        }
                        AppInfo.PTInfo.UserID = AppInfo.Account.PTID;
                        if (AppInfo.PTInfo.LineIndex == -1)
                        {
                            AppInfo.PTInfo.LineIndex = this.Egv_PTLineList.SelectedRows[0].Index;
                        }
                        AppInfo.PTInfo.LoginUrl = AppInfo.PTInfo.GetLine();
                        this.RefreshPTLineList();
                        if (!(!AppInfo.PTInfo.IsHTLoginMain || AppInfo.PTInfo.IsLoadWebLogin))
                        {
                            AppInfo.PTInfo.AnalysisVerifyCode = true;
                            AppInfo.PTInfo.LoginMain = true;
                        }
                        else
                        {
                            string cookieInternal = HttpHelper.GetCookieInternal(AppInfo.PTInfo.GetUrlLine());
                            AppInfo.PTInfo.WebCookie = cookieInternal;
                            string loginLine = AppInfo.PTInfo.GetLoginLine();
                            this.Web_Login.Navigate(loginLine);
                        }
                    }
                }
                else
                {
                    AppInfo.PTInfo.PTLoginStatus = false;
                    AppInfo.PTInfo.IsLoadBets = true;
                    this.ChangeUserLoginStatus();
                    this.SavePlanListData(true);
                    this.CloseAllBets();
                    this.BetsDic.Clear();
                    this.BetsErrorList.Clear();
                    this.WebLoginOut();
                    AppInfo.PTInfo.DefaultOption();
                    this.mainThread.downCode.DefaultOption();
                    this.RefreshControl(true);
                    this.RefreshPTLineList();
                }
            }
        }

        public void LoginPTLottery(int pIndex)
        {
            HtmlDocument document = this.Web_Login.Document;
        }

        public void LoginVerify()
        {
            try
            {
                HtmlDocument pDocument = this.Web_Login.Document;
                if (AppInfo.Account.PTID != "")
                {
                    if (AppInfo.PTInfo == AppInfo.WXYLInfo)
                    {
                        CommFunc.SetWebHtmlElement(pDocument, "classname", "input-block-level user", "value", AppInfo.Account.PTID, true);
                        CommFunc.SetWebHtmlElement(pDocument, "classname", "input-block-level password", "value", AppInfo.Account.PTPW, true);
                        CommFunc.GetWebHtmlElement(pDocument, "data-loading-text", "登录中", false, true);
                    }
                    else if (AppInfo.PTInfo == AppInfo.HUBOInfo)
                    {
                        CommFunc.SetWebHtmlElement(pDocument, "classname", "login_inp_xin", "value", AppInfo.Account.PTID, true);
                        CommFunc.SetWebHtmlElement(pDocument, "classname", "login_inp_xinpwd", "value", AppInfo.Account.PTPW, true);
                        CommFunc.GetWebHtmlElement(pDocument, "classname", "login_in_ok_xin btn-submit", false, true);
                    }
                }
            }
            catch
            {
            }
        }

        private void Nic_Hint_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                base.ShowInTaskbar = true;
                this.Nic_Hint.Visible = false;
                base.Visible = true;
            }
        }

        private void Nm_TJFindXS_ValueChanged(object sender, EventArgs e)
        {
            this.Ckb_TJFindXS_Click(null, null);
        }

        private void OpenDataHint(ConfigurationStatus.OpenData pData)
        {
            string tipText = $"{AppInfo.Current.Lottery.Name} 第{pData.Expect}期开奖号码为：{pData.Code}";
            this.Nic_Hint.Visible = true;
            this.Nic_Hint.Text = tipText;
            this.Nic_Hint.ShowBalloonTip(0x2710, "永信在线挂机软件提示", tipText, ToolTipIcon.Info);
        }

        private void Pit_Logo1_Click(object sender, EventArgs e)
        {
            CommFunc.OpenWeb(AppInfo.Account.Configuration.ImageLink);
        }

        public bool PTBetsMain(ConfigurationStatus.AutoBets pBets, ConfigurationStatus.SCPlan plan, ConfigurationStatus.BetsScheme pScheme)
        {
            double num2;
            if (AppInfo.PTInfo.PTIsBreak)
            {
                return false;
            }
            if (pBets.ShareBetsInfo.BetsTypeInfo == ConfigurationStatus.BetsType.FNBets)
            {
                for (int i = pBets.PlanList.Count - 1; i >= 0; i--)
                {
                    ConfigurationStatus.SCPlan plan2 = pBets.PlanList[i];
                    if ((plan2.CheckPlanIsWait() && (plan2.NumberList.Count > 0)) && (plan2.BetsKey == plan.BetsKey))
                    {
                        pScheme.IsBetsYes = true;
                        return true;
                    }
                }
            }
            bool isMNBets = plan.IsMNBets;
            if (pBets.YLStopMoney != "")
            {
                num2 = pBets.Gain(isMNBets);
                double num3 = Convert.ToDouble(pBets.YLStopMoney);
                if ((num2 >= 0.0) && (num2 >= num3))
                {
                    pBets.ErrorState = "目前总盈利不在设定的投注范围内";
                    return false;
                }
            }
            if (pBets.KSStopMoney != "")
            {
                num2 = pBets.Gain(isMNBets);
                double num4 = Convert.ToDouble(pBets.KSStopMoney);
                if ((num2 < 0.0) && (Math.Abs(num2) >= num4))
                {
                    pBets.ErrorState = "目前总亏损不在设定的投注范围内";
                    return false;
                }
            }
            string currentExpect = plan.CurrentExpect;
            if ((AppInfo.PTInfo.Expect != "") && (string.Compare(currentExpect, AppInfo.PTInfo.Expect) < 0))
            {
                pBets.ErrorState = $"{currentExpect}期投注时间已过";
                return false;
            }
            if (plan.IsMNBets)
            {
                pBets.PTState = "投注成功";
                return true;
            }
            if (!AppInfo.PTInfo.IsCombinaBets && !AppInfo.PTInfo.BetsMain(plan, ref pBets.PTState))
            {
                return false;
            }
            return true;
        }

        private void PTIndexMain(string pName = "")
        {
            if (!AppInfo.PTInfo.PTLoginStatus)
            {
                AppInfo.PTInfo.PTLoginStatus = true;
                this.RefreshLogin(false, "");
                this.RefreshControl(true);
                this.RefreshPTLineList();
                this.SetLableLoading();
                CommFunc.PlaySound("登录成功");
                AppInfo.PTInfo.AnalysisVerifyCode = true;
                this.ChangeUserLoginStatus();
            }
        }

        public void QHBTCount(string pValue)
        {
            ConfigurationStatus.Scheme scheme = this.GetScheme("");
            if (scheme != null)
            {
                string name = scheme.Name;
                this.SetTapShow(this.Tap_BTCount.Tag.ToString(), true, true);
                CommFunc.SetTabSelectIndex(this.Tab_Main, this.Tap_BTCount.Text);
                this.BT_Main.SetSelectBTFN(name, pValue);
            }
        }

        public void QHBTFN(string pValue)
        {
            this.SetTapShow(this.Tap_BTFN.Tag.ToString(), true, true);
            CommFunc.SetTabSelectIndex(this.Tab_Main, this.Tap_BTFN.Text);
            CommFunc.SetDataGridViewSelected(this.Egv_BTFNMain, pValue, 0);
            this.Egv_BTFNList_CellMouseClick(null, null);
        }

        private void Rdb_Expect_CheckedChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                RadioButton button = sender as RadioButton;
                if (button.Checked)
                {
                    this.Btn_RefreshData_Click(null, null);
                }
            }
        }

        private void Rdb_LSBJType_CheckedChanged(object sender, EventArgs e)
        {
            this.Cbb_LSBJType.Enabled = this.Rdb_LSBJType.Checked && this.Rdb_LSBJType.Enabled;
            this.Nm_LSBJExpect.Enabled = this.Rdb_LSBJExpect.Checked && this.Rdb_LSBJExpect.Enabled;
        }

        private void Rdb_ShareBets_CheckedChanged(object sender, EventArgs e)
        {
            this.Ckb_ShareBetsManage.Enabled = this.Rdb_ShareBets.Checked;
        }

        private void RefreshAllFNList()
        {
            this.RefreshLSFNList();
            this.RefreshTJFNList();
            this.RefreshSchemeList();
            this.BT_Main.RefreshBTList();
        }

        private void RefreshBank()
        {
            AppInfo.PTInfo.GetAccountsMem(AppInfo.Current.Lottery.Type, AppInfo.Account);
            base.Invoke(AppInfo.BankControlRefresh);
        }

        private void RefreshBankControl()
        {
            double bankBalance = AppInfo.Account.BankBalance;
            if (((AppInfo.PTInfo == AppInfo.WDYLInfo) || (AppInfo.PTInfo == AppInfo.HUAYInfo)) && (bankBalance == -1.0))
            {
                this.Lbl_BankBalanceValue.Text = "投注之后显示余额";
            }
            else if (!(bankBalance == -1.0))
            {
                this.Lbl_BankBalanceValue.Text = CommFunc.TwoDouble(AppInfo.Account.BankBalance, true);
            }
        }

        private void RefreshBetsHint(ConfigurationStatus.AutoBets pBets)
        {
            Label label = this.Lbl_BetsValue;
            if ((pBets.ErrorState != "") && (pBets.ErrorState != "无"))
            {
                label.Text = $"{"投注失败-"}{pBets.ErrorState}";
                label.ForeColor = AppInfo.appForeColor;
            }
            else if (pBets.PTState != "")
            {
                string pTHint = AppInfo.PTInfo.GetPTHint(pBets.PTState);
                if (pTHint == "")
                {
                    pTHint = "未知错误";
                }
                if (pTHint != "投注成功")
                {
                    pTHint = "投注失败-" + pTHint;
                }
                label.Text = pTHint;
                label.ForeColor = AppInfo.PTInfo.CheckReturn(pBets.PTState, false) ? AppInfo.greenForeColor : AppInfo.appForeColor;
            }
            else if (pBets.PlanRun && (pBets.BetsNumber == 0))
            {
                label.Text = $"{pBets.Expect}期没有符合条件的投注";
                label.ForeColor = AppInfo.appForeColor;
            }
            else
            {
                label.Text = "无";
                label.ForeColor = AppInfo.Account.Configuration.Beautify ? AppInfo.whiteColor : AppInfo.blackColor;
            }
        }

        private void RefreshBTFNControl(bool pRefreshTimesLis)
        {
            CommFunc.RefreshDataGridView(this.Egv_BTFNMain, AppInfo.BTFNList.Count);
            if (pRefreshTimesLis)
            {
                this.RefreshBTFNTimesList();
            }
        }

        private void RefreshBTFNTimesList()
        {
            string bTFNName = this.GetBTFNName();
            if (bTFNName != "")
            {
                ConfigurationStatus.GJBTScheme scheme = AppInfo.BTFNDic[bTFNName];
                if (scheme.IsViewGJBTEncrypt)
                {
                    this.Lbl_GJFNEncrypt.Visible = true;
                    this.Lbl_GJFNEncrypt.BringToFront();
                }
                else
                {
                    this.Lbl_GJFNEncrypt.Visible = false;
                    this.Lbl_GJFNEncrypt.SendToBack();
                    this.RefreshTimesList();
                    this.Ckb_DeleteBTFN.Enabled = this.Egv_BTFNMain.RowCount > 0;
                    this.Ckb_EditTimes.Enabled = this.Ckb_DeleteTimes.Enabled = this.Ckb_ClearTimes.Enabled = this.Egv_BTFNTimesList.RowCount > 0;
                }
            }
        }

        private void RefreshControl(bool pRefreshAll)
        {
            try
            {
                if (pRefreshAll)
                {
                    ConfigurationStatus.AutoBets pBets = this.GetBets("");
                    this.Btn_Bets.Enabled = true;
                    bool planRun = pBets.PlanRun;
                    this.Btn_Bets.ForeColor = planRun ? AppInfo.whiteColor : AppInfo.whiteColor;
                    this.Btn_Bets.BackColor = planRun ? AppInfo.greenBackColor : AppInfo.appBackColor;
                    this.Btn_Bets.Text = planRun ? "关闭\r\n自动投注" : "开启\r\n自动投注";
                    this.Ckb_ClearBetsList.Enabled = !planRun && (pBets.PlanList.Count > 0);
                    this.Rdb_CGBets.Enabled = this.Rdb_ShareBets.Enabled = !planRun;
                    this.RefreshBetsHint(pBets);
                    this.RefreshShareBetsHint(pBets);
                    this.RefreshPlanList1();
                    this.Lbl_BetsGainPlanValue.Text = CommFunc.TwoDouble(pBets.Gain(false), true);
                    this.Lbl_MNBetsGainPlanValue.Text = CommFunc.TwoDouble(pBets.Gain(true), true);
                    this.Lbl_BetsMoneyPlanValue.Text = CommFunc.TwoDouble(pBets.Money(false), true);
                    this.Lbl_MNBetsMoneyPlanValue.Text = CommFunc.TwoDouble(pBets.Money(true), true);
                    this.Lbl_LZMaxValue.Text = pBets.MaxLZCount.ToString();
                    this.Lbl_LGMaxValue.Text = pBets.MaxLGCount.ToString();
                    this.Lbl_ZQLValue.Text = pBets.ZQL;
                    string str = pBets.PlanList.Count.ToString();
                    if (pBets.PlanList.Count != 0)
                    {
                        str = str + $"/{pBets.ExpectCount()}期";
                    }
                    this.Lbl_BetsCountValue.Text = str;
                }
            }
            catch (Exception exception)
            {
                DebugLog.SaveDebug(exception, "");
            }
        }

        private void RefreshControlThreading(ConfigurationStatus.AutoBets pBets)
        {
            bool flag = this.LastBetsIndex.Contains("自动投注");
            base.Invoke(AppInfo.BetsRefresh, new object[] { flag });
        }

        public void RefreshData(List<string> pList, ConfigurationStatus.LotteryType pLottery, int pDataCount)
        {
            try
            {
                this.LoadDataList(pList);
                if (AppInfo.DataList.Count > 0)
                {
                    if (this.Ckb_OpenHint.Checked)
                    {
                        this.OpenDataHint(this.GetOpenDataFirstExpect);
                    }
                    this.VerificationBetsPlan();
                    this.LoadBetsTime();
                    this.TV_Main.Tgv_TrendList_CellMouseClick(null, null);
                    this.RefreshControl(true);
                }
            }
            catch (Exception exception)
            {
                DebugLog.SaveDebug(exception, "");
            }
        }

        private void RefreshLoad(bool RefreshData)
        {
            if (this.mainThread.loadThread.ThreadState != ThreadState.Stopped)
            {
                CommFunc.PublicMessageAll("线程正在进行中...", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                this.mainThread.loadThread = new Thread(new ParameterizedThreadStart(this.mainThread.LoadThread));
                this.mainThread.loadThread.IsBackground = true;
                this.mainThread.loadThread.Start(RefreshData);
            }
        }

        private void RefreshLogin(bool pState, string pHint = "")
        {
            AppInfo.PTInfo.IsLoginRun = pState;
            this.Ckb_Login.Enabled = !pState;
            this.Lbl_LoginHint.Text = pState ? "正在登录中..." : pHint;
            if (pState)
            {
                this.Pnl_RrfreshPT.Enabled = false;
                this.Ckb_RefreshUser.Text = "取消";
                this.Ckb_RefreshUser.Image = Resources.CancelRound;
                this.Ckb_RefreshUser.Visible = true;
            }
            else if (!AppInfo.PTInfo.PTLoginStatus)
            {
                this.Pnl_RrfreshPT.Enabled = true;
                this.Ckb_RefreshUser.Visible = false;
            }
        }

        private void RefreshLoginHint()
        {
            if (AppInfo.PTInfo.PTLoginStatus)
            {
                this.Tsp_HintKey.Visible = this.Tsp_HintValue.Visible = true;
                this.Tsp_HintValue.Text = AppInfo.Account.PTLoginHintString;
            }
            else
            {
                this.Tsp_HintKey.Visible = this.Tsp_HintValue.Visible = false;
                this.Tsp_HintValue.Text = "";
            }
        }

        private void RefreshLSDataLater()
        {
            this.RefreshLSDataList();
            this.Btn_LSRefresh.Enabled = true;
            this.Lbl_LSRefreshHint.Visible = false;
            this.Pnl_LSDataRight.Visible = false;
        }

        private void RefreshLSDataList()
        {
            int count = this.LSDataViewList.Count;
            CommFunc.RefreshDataGridView(this.Egv_LSDataList, count);
            this.Egv_LSDataList.AutoResizeColumns();
        }

        private void RefreshLSDataMain(object pInfo)
        {
            ConfigurationStatus.LSData pinfo = pInfo as ConfigurationStatus.LSData;
            try
            {
                DateTime date = pinfo.Date;
                ConfigurationStatus.Scheme schemeInfo = pinfo.SchemeInfo;
                if (schemeInfo == null)
                {
                    return;
                }
                List<ConfigurationStatus.OpenData> list = this.FilterLSOpenData(date);
                List<ConfigurationStatus.OpenData> list2 = new List<ConfigurationStatus.OpenData>();
                Dictionary<string, List<ConfigurationStatus.ExpectCount>> dictionary = new Dictionary<string, List<ConfigurationStatus.ExpectCount>>();
                Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
                ConfigurationStatus.BetsScheme pScheme = new ConfigurationStatus.BetsScheme();
                ConfigurationStatus.AutoBets bets = new ConfigurationStatus.AutoBets();
                string name = schemeInfo.Name;
                pScheme.SchemeInfo = schemeInfo;
                if (pScheme.SchemeInfo.FNBaseInfo.CheckCount(pScheme, ref bets.ErrorState))
                {
                    ConfigurationStatus.BetsCode code;
                    ConfigurationStatus.ExpectCount count;
                    int num = list.Count;
                    int num2 = (AppInfo.DataList.Count - schemeInfo.FNBaseInfo.BetsJKValue.Length) - 1;
                    if (num > num2)
                    {
                        num = num2;
                    }
                    Dictionary<string, ConfigurationStatus.BetsCode> dictionary3 = new Dictionary<string, ConfigurationStatus.BetsCode>();
                    string pData = "";
                    for (int i = num - 1; i >= -1; i--)
                    {
                        int openDataIndexByExpect = i;
                        ConfigurationStatus.OpenData data2 = null;
                        if (openDataIndexByExpect != -1)
                        {
                            data2 = list[i];
                            openDataIndexByExpect = this.GetOpenDataIndexByExpect(data2.Expect, null);
                            if ((openDataIndexByExpect + 1) >= AppInfo.DataList.Count)
                            {
                                continue;
                            }
                            string str3 = data2.Time.ToShortDateString();
                            if ((pData != "") && (pData != str3))
                            {
                                dictionary2.Clear();
                                pScheme.FNBTIndexDic.Clear();
                                pScheme.ZuKeyDic.Clear();
                            }
                            pData = str3;
                        }
                        Dictionary<string, ConfigurationStatus.BetsCode> pFNNumberDic = schemeInfo.FNBaseInfo.CountNumber(pScheme, AppInfo.DataList, openDataIndexByExpect + 1);
                        if (!pScheme.SchemeInfo.FNBaseInfo.IsBetsZJ)
                        {
                            this.ReverseCode(ref pFNNumberDic, pScheme.SchemeInfo.FNBaseInfo);
                        }
                        ConfigurationStatus.FNBase.AddFNBTIndex(pFNNumberDic, pScheme.FNBTIndexDic);
                        if (openDataIndexByExpect == -1)
                        {
                            dictionary3 = pFNNumberDic;
                        }
                        else if (pFNNumberDic.Count != 0)
                        {
                            List<string> codeList = data2.CodeList;
                            string expect = data2.Expect;
                            foreach (string str5 in pFNNumberDic.Keys)
                            {
                                Dictionary<string, int> dictionary5;
                                string str6;
                                if (!dictionary2.ContainsKey(str5))
                                {
                                    dictionary2[str5] = 0;
                                }
                                code = pFNNumberDic[str5];
                                ConfigurationStatus.FBType fBInfo = pScheme.FBInfo;
                                List<List<int>> indexList = CommFunc.GetCodeListByPlay(pScheme.PlayType, pScheme.PlayName, pScheme.RXWZ, code.CodeList);
                                int pWinCount = CommFunc.VerificationCode(pScheme.Play, indexList, codeList, code.CodeList);
                                code.IsWin = CommFunc.VerificationWinCount(pWinCount, pScheme.Play, schemeInfo.FNBaseInfo.RXZJList);
                                if (code.IsWin)
                                {
                                    dictionary2[str5] = 0;
                                    switch (fBInfo)
                                    {
                                        case ConfigurationStatus.FBType.BZFB:
                                            pScheme.FNBTIndexDic[str5] = 0;
                                            break;

                                        case ConfigurationStatus.FBType.ZFB:
                                            (dictionary5 = pScheme.FNBTIndexDic)[str6 = str5] = dictionary5[str6] + 1;
                                            break;
                                    }
                                }
                                else
                                {
                                    (dictionary5 = dictionary2)[str6 = str5] = dictionary5[str6] + 1;
                                    if (dictionary.ContainsKey(str5))
                                    {
                                        List<ConfigurationStatus.ExpectCount> list5 = dictionary[str5];
                                        count = list5[list5.Count - 1];
                                        if (count.Count != 0)
                                        {
                                            list5.RemoveAt(list5.Count - 1);
                                        }
                                    }
                                    switch (fBInfo)
                                    {
                                        case ConfigurationStatus.FBType.BZFB:
                                            (dictionary5 = pScheme.FNBTIndexDic)[str6 = str5] = dictionary5[str6] + 1;
                                            break;

                                        case ConfigurationStatus.FBType.ZFB:
                                            pScheme.FNBTIndexDic[str5] = 0;
                                            break;
                                    }
                                }
                                pScheme.SchemeInfo.FNBaseInfo.CountZuKeyList(str5, pScheme, code.IsWin);
                                ConfigurationStatus.ExpectCount count2 = new ConfigurationStatus.ExpectCount(expect, dictionary2[str5], pData);
                                if (!dictionary.ContainsKey(str5))
                                {
                                    List<ConfigurationStatus.ExpectCount> list6 = new List<ConfigurationStatus.ExpectCount> {
                                        count2
                                    };
                                    dictionary[str5] = list6;
                                }
                                else
                                {
                                    dictionary[str5].Add(count2);
                                }
                            }
                        }
                    }
                    List<ConfigurationStatus.LSDataView> list7 = pinfo.RefreshControl ? this.LSDataViewList : pinfo.ViewList;
                    list7.Clear();
                    string key = "0";
                    ConfigurationStatus.LSDataView item = new ConfigurationStatus.LSDataView {
                        ZuKey = key
                    };
                    if (dictionary3.ContainsKey(key))
                    {
                        code = dictionary3[key];
                        item.Value = CommFunc.CombinaBetsCode(code.CodeList, schemeInfo.FNBaseInfo.Play);
                    }
                    if (dictionary.ContainsKey(key))
                    {
                        int num8;
                        List<ConfigurationStatus.ExpectCount> pExpectList = dictionary[key];
                        int num6 = 0;
                        int num7 = 0;
                        for (num8 = pExpectList.Count - 1; num8 >= 0; num8--)
                        {
                            count = pExpectList[num8];
                            int num9 = count.Count;
                            if ((num6 == 0) && (num7 == 0))
                            {
                                if (num9 == 0)
                                {
                                    num6 = 1;
                                    continue;
                                }
                                num7 = num9;
                                break;
                            }
                            if (num6 != 0)
                            {
                                if (num9 != 0)
                                {
                                    break;
                                }
                                num6++;
                            }
                        }
                        if (num6 != 0)
                        {
                            item.LZExpect = num6.ToString();
                        }
                        else if (num7 != 0)
                        {
                            item.LGExpect = num7.ToString();
                        }
                        num8 = pExpectList.Count - 1;
                        while (num8 >= 0)
                        {
                            count = pExpectList[num8];
                            if (count.Count == 0)
                            {
                                item.PerExpect = (pExpectList[num8 - 1].Count + 1).ToString();
                                break;
                            }
                            num8--;
                        }
                        int num10 = 0;
                        for (num8 = pExpectList.Count - 1; num8 >= 0; num8--)
                        {
                            count = pExpectList[num8];
                            if (count.Count != 0)
                            {
                                for (int j = num8 - 1; j >= 0; j--)
                                {
                                    ConfigurationStatus.ExpectCount count3 = pExpectList[j];
                                    if (count3.Count != 0)
                                    {
                                        break;
                                    }
                                    num10++;
                                }
                                break;
                            }
                        }
                        item.PerLZExpect = num10.ToString();
                        item.TodayExpect = this.GetFBExpect(pExpectList, date, 0);
                        item.YesterdayExpect = this.GetFBExpect(pExpectList, date, 1);
                        item.WeekExpect = this.GetFBExpect(pExpectList, date, 8);
                    }
                    list7.Add(item);
                    if (pinfo.RefreshControl && pinfo.IsBJ)
                    {
                        this.CheckLSDataBJ(pinfo);
                    }
                }
            }
            catch (Exception exception)
            {
                DebugLog.SaveDebug(exception, "");
            }
            if (pinfo.RefreshControl)
            {
                base.Invoke(AppInfo.RefreshLSDataLater);
            }
        }

        private void RefreshLSFNList()
        {
            CommFunc.SetComboBoxList(this.Cbb_LSFN, AppInfo.SchemeList);
        }

        private void RefreshLSPlay()
        {
            string selectLSFN = this.GetSelectLSFN();
            ConfigurationStatus.Scheme scheme = this.GetScheme(selectLSFN);
            this.Lbl_LSPlayValue.Text = scheme.FNBaseInfo.ViewPlay;
            this.Lbl_LSPlayValue.Visible = true;
        }

        public void RefreshNewData()
        {
            ConfigurationStatus.OpenData getOpenDataFirstExpect = this.GetOpenDataFirstExpect;
            this.Lbl_CurrentExpect.Text = this.Lbl_CurrentExpect1.Text = $"{AppInfo.Current.Lottery.Name} 第 {getOpenDataFirstExpect.Expect} 期开奖号码";
            List<string> codeList = getOpenDataFirstExpect.CodeList;
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (this.Pnl_CurrentExpect.Visible)
                {
                    this.Pnl_CurrentCode1.Visible = false;
                    this.Pnl_CurrentCode2.Visible = true;
                    this.PK_Code.LoadOpenData(codeList);
                }
                else
                {
                    this.Pnl_CurrentCode3.Visible = false;
                    this.Pnl_CurrentCode4.Visible = true;
                    this.PK_CodeSmall.LoadOpenData(codeList);
                }
            }
            else
            {
                this.Pnl_CurrentCode1.Visible = true;
                this.Pnl_CurrentCode2.Visible = false;
                this.Pnl_CurrentCode3.Visible = true;
                this.Pnl_CurrentCode4.Visible = false;
                this.Lbl_CurrentCode1.Text = this.Lbl_CurrentCode6.Text = codeList[0];
                this.Lbl_CurrentCode2.Text = this.Lbl_CurrentCode7.Text = codeList[1];
                this.Lbl_CurrentCode3.Text = this.Lbl_CurrentCode8.Text = codeList[2];
                this.Lbl_CurrentCode4.Text = this.Lbl_CurrentCode9.Text = codeList[3];
                this.Lbl_CurrentCode5.Text = this.Lbl_CurrentCode10.Text = codeList[4];
            }
            this.RefreshNextExpect(getOpenDataFirstExpect.Expect, true);
            this.ResetTime();
            AppInfo.Current.Lottery.IsLoadServerData = true;
        }

        private void RefreshNextExpect(string pExpect, bool pRefreshNext)
        {
            if (pExpect != "")
            {
                string str = CommFunc.CountNextExpect(pExpect, "");
                AppInfo.Current.Lottery.NextExpect = str;
                if (pRefreshNext)
                {
                    this.Lbl_NextExpect.Text = this.Lbl_NextExpect1.Text = $"距第 {str} 期开奖：";
                    this.Lbl_BetsExpectValue.Text = str + "期";
                }
            }
        }

        private void RefreshNoticeList()
        {
            CommFunc.RefreshDataGridView(this.Egv_NoticeList, this.NoticeList.Count);
        }

        private void RefreshPlanComboBox(ComboBox pComboBox, string path)
        {
            List<string> pathNameList = CommFunc.GetPathNameList(path);
            CommFunc.SetComboBoxList(pComboBox, pathNameList);
        }

        private void RefreshPlanList1()
        {
            ConfigurationStatus.AutoBets pBets = this.GetBets("");
            this.CountPlanViewList(pBets);
            int count = pBets.PlanViewList.Count;
            string getOpenDataFirstExpectString = this.GetOpenDataFirstExpectString;
            if ((this.Egv_PlanList.RowCount != count) || ((getOpenDataFirstExpectString != "") && !this.TJViewExpect.ContainsKey(getOpenDataFirstExpectString)))
            {
                CommFunc.RefreshDataGridView(this.Egv_PlanList, count, this.Ckb_BetsSort.Checked);
                if (getOpenDataFirstExpectString != "")
                {
                    this.TJViewExpect[getOpenDataFirstExpectString] = getOpenDataFirstExpectString;
                }
            }
            else
            {
                CommFunc.RefreshDataGridView(this.Egv_PlanList, count);
            }
        }

        private void RefreshPTLineList()
        {
            CommFunc.RefreshDataGridView(this.Egv_PTLineList, AppInfo.PTInfo.LineList.Count);
        }

        private void RefreshPTMain(bool pHint)
        {
            if (!pHint || CommFunc.AgreeMessage("是否要更新所有平台的线路？", true, MessageBoxIcon.Asterisk, ""))
            {
                bool flag = this._RunEvent;
                this._RunEvent = false;
                string text = this.Cbb_Lottery.Text;
                File.Delete(AppInfo.PTLineFile);
                this.LoadPTLine(this.PTName);
                this.SavePTLine();
                CommFunc.SetComboBoxSelectedIndex(this.Cbb_Lottery, text);
                this.LoadLotteryNameInfo();
                if (pHint)
                {
                    CommFunc.PublicMessageAll("平台线路更新成功！", true, MessageBoxIcon.Asterisk, "");
                }
                this._RunEvent = flag;
            }
        }

        private void RefreshScheme()
        {
            int count = AppInfo.SchemeList.Count;
            this.Ckb_DeleteScheme.Enabled = count > 0;
            this.Ckb_CopyScheme.Enabled = count > 0;
            this.Ckb_EditScheme.Enabled = !this.IsEditScheme && (count > 0);
            this.Ckb_EditTimesPlan.Enabled = count > 0;
            this.Ckb_ExportScheme.Enabled = count > 0;
            this.Ckb_ClearScheme.Enabled = count > 0;
            this.Pnl_SchemeTop1.Enabled = this.Pnl_SchemeBottom.Enabled = this.Egv_SchemeList.Enabled = !this.IsEditScheme;
            this.Pnl_SchemeTop2.Enabled = this.Pnl_SchemeInfo.Enabled = this.IsEditScheme;
            this.Pnl_SchemeMain.Visible = count > 0;
            this.Egv_SchmeeList_CellClick(null, null);
        }

        private void RefreshSchemeControl(ConfigurationStatus.Scheme pInfo)
        {
            try
            {
                this.Txt_FNName.Text = pInfo.Name;
                CommFunc.SetComboBoxSelectedIndex(this.Cbb_FNCHType, pInfo.CHType);
                this.FN_DMLH.Visible = pInfo.FNCHType == ConfigurationStatus.SchemeCHType.DMLH;
                this.FN_GJDMLH.Visible = pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GJDMLH;
                this.FN_GJKMTM.Visible = pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GJKMTM;
                this.FN_LRWCH.Visible = pInfo.FNCHType == ConfigurationStatus.SchemeCHType.LRWCH;
                this.FN_BCFCH.Visible = pInfo.FNCHType == ConfigurationStatus.SchemeCHType.BCFCH;

                this.FN_WJJH.Visible = pInfo.FNCHType == ConfigurationStatus.SchemeCHType.WJJH;
                this.FN_SJCH.Visible = pInfo.FNCHType == ConfigurationStatus.SchemeCHType.SJCH;
                this.FN_YLCH.Visible = pInfo.FNCHType == ConfigurationStatus.SchemeCHType.YLCH;
                this.FN_KMTM.Visible = pInfo.FNCHType == ConfigurationStatus.SchemeCHType.KMTM;
                this.FN_GDQM.Visible = pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GDQM;
                this.FN_LHKMTM.Visible = pInfo.FNCHType == ConfigurationStatus.SchemeCHType.LHKMTM;
                this.SetSchemeControl(pInfo);
                if (pInfo.IsViewFNEncrypt)
                {
                    this.Lbl_FNEncrypt.Visible = true;
                    this.Lbl_FNEncrypt.BringToFront();
                }
                else
                {
                    this.Lbl_FNEncrypt.Visible = false;
                    this.Lbl_FNEncrypt.SendToBack();
                }
            }
            catch
            {
            }
        }

        private void RefreshSchemeList()
        {
            CommFunc.RefreshDataGridView(this.Egv_SchemeList, AppInfo.SchemeList.Count);
            this.Egv_SchemeList.HeaderCheckBoxHide();
        }

        private void RefreshShareBetsControl()
        {
            if (AppInfo.IsCXG)
            {
                if (AppInfo.Account.SendUserID != "")
                {
                    if (AppInfo.Account.Configuration.IsSendUserID)
                    {
                        this.Rdb_ShareBets.Text = "共享投注";
                        this.Tot_Hint.SetToolTip(this.Rdb_ShareBets, "将投注数据共享给指定的会员，建议开启模拟投注");
                        this.Ckb_ShareBetsManage.Text = "共享管理";
                        this.Tot_Hint.SetToolTip(this.Ckb_ShareBetsManage, "添加或删除投注共享的下级");
                        CommFunc.SetControlHint(this.Err_Hint, this.Rdb_ShareBets, "上级投注什么，下级跟单投注，不用再担心方案的泄密");
                    }
                    else
                    {
                        this.Rdb_ShareBets.Text = "跟单投注";
                        this.Tot_Hint.SetToolTip(this.Rdb_ShareBets, "跟随上级投注");
                        this.Ckb_ShareBetsManage.Text = "跟单设置";
                        this.Tot_Hint.SetToolTip(this.Ckb_ShareBetsManage, "设置上级的信息");
                        CommFunc.SetControlHint(this.Err_Hint, this.Rdb_ShareBets, "输入上级发给你的共享码，即可跟着上级投注，不在进行方案的设置");
                    }
                    this.Pnl_BetsType.Visible = true;
                }
                else
                {
                    this.Pnl_BetsType.Visible = false;
                }
            }
        }

        private void RefreshShareBetsHint(ConfigurationStatus.AutoBets pBets)
        {
            Label label = this.Lbl_ShareBetsHint;
            if ((pBets.ShareBetsInfo == null) || (pBets.ShareBetsInfo.BetsTypeInfo == ConfigurationStatus.BetsType.FNBets))
            {
                label.Text = "";
            }
            else if (pBets.PlanRun)
            {
                int num;
                string expect = pBets.Expect;
                if (pBets.ShareBetsInfo.BetsTypeInfo == ConfigurationStatus.BetsType.SendBets)
                {
                    if (!pBets.ShareBetsInfo.SendPlanCountDic.ContainsKey(expect))
                    {
                        if (pBets.ShareBetsInfo.SendPlanList.Count == 0)
                        {
                            label.Text = $"{pBets.Expect}期没有成功共享的投注计划";
                            label.ForeColor = AppInfo.appForeColor;
                        }
                        else
                        {
                            label.Text = $"{pBets.Expect}期正在上传【{pBets.ShareBetsInfo.SendPlanList.Count}】条投注计划";
                            label.ForeColor = AppInfo.appForeColor;
                        }
                    }
                    else
                    {
                        num = pBets.ShareBetsInfo.SendPlanCountDic[expect];
                        label.Text = $"{pBets.Expect}期成功共享【{num}】条投注计划";
                        label.ForeColor = AppInfo.greenForeColor;
                    }
                }
                else if (pBets.ShareBetsInfo.BetsTypeInfo == ConfigurationStatus.BetsType.FollowBets)
                {
                    if (!pBets.ShareBetsInfo.FollowPlanCountDic.ContainsKey(expect))
                    {
                        if (pBets.ShareBetsInfo.FollowPlanList.Count == 0)
                        {
                            label.Text = pBets.ShareBetsInfo.FollowErrorHint;
                            label.ForeColor = AppInfo.appForeColor;
                        }
                        else
                        {
                            label.Text = $"{pBets.Expect}期正在跟随【{pBets.ShareBetsInfo.FollowPlanList.Count}】条计划投注";
                            label.ForeColor = AppInfo.appForeColor;
                        }
                    }
                    else
                    {
                        num = pBets.ShareBetsInfo.FollowPlanCountDic[expect];
                        label.Text = $"{pBets.Expect}期成功跟随【{num}】条投注计划";
                        label.ForeColor = AppInfo.greenForeColor;
                    }
                }
            }
            else
            {
                label.Text = "";
            }
        }

        private void RefreshShareSchemeControl()
        {
            if (AppInfo.IsCXG)
            {
                if (AppInfo.Account.SendUserID != "")
                {
                    if (AppInfo.Account.Configuration.IsSendUserID)
                    {
                        this.Ckb_ShareScheme.Text = "上传";
                        this.Ckb_ShareScheme.Image = Resources.WindowUp;
                        this.Ckb_ShareSchemeManage.Text = "共享管理";
                        this.Tot_Hint.SetToolTip(this.Ckb_ShareSchemeManage, "添加或删除方案共享的下级");
                        this.Tot_Hint.SetToolTip(this.Ckb_ShareScheme, "上传共享的方案，不在发送方案文件给下级");
                    }
                    else
                    {
                        this.Ckb_ShareScheme.Text = "下载";
                        this.Ckb_ShareScheme.Image = Resources.WindowDown;
                        this.Ckb_ShareSchemeManage.Text = "下载设置";
                        this.Tot_Hint.SetToolTip(this.Ckb_ShareSchemeManage, "设置上级的信息");
                        this.Tot_Hint.SetToolTip(this.Ckb_ShareScheme, "下载上级共享的方案");
                    }
                    this.Pnl_SchemeShare.Visible = true;
                }
                else
                {
                    this.Pnl_SchemeShare.Visible = false;
                }
            }
        }

        private void RefreshShowTapList()
        {
            CommFunc.RefreshDataGridView(this.Egv_ShowTapList, this.ShowTapList.Count - 1);
            this.Egv_ShowTapList.HeaderCheckBoxHide();
        }

        private void RefreshTab(bool pIsHide = true)
        {
            int num;
            TabPage page;
            string str;
            if (pIsHide)
            {
                Dictionary<string, ConfigurationStatus.ShowTap> showTapDic = this.GetShowTapDic();
                for (num = this.SettingPageList.Count - 1; num >= 0; num--)
                {
                    page = this.SettingPageList[num];
                    str = page.Tag.ToString();
                    if (!showTapDic[str].Selected)
                    {
                        page.Parent = null;
                    }
                }
            }
            int count = this.Tab_Main.TabPages.Count;
            for (num = 0; num < count; num++)
            {
                page = this.Tab_Main.TabPages[num];
                str = page.Tag.ToString();
                page.Text = ((num + 1)).ToString() + "-" + str;
            }
            if (count >= 13)
            {
                this.Tab_Main.ItemSize = new Size(70, 30);
            }
            else if (count >= 10)
            {
                this.Tab_Main.ItemSize = new Size(80, 30);
            }
            else
            {
                this.Tab_Main.ItemSize = new Size(100, 30);
            }
            if (AppInfo.IsCXG && ((((AppInfo.App != ConfigurationStatus.AppType.THEN) && (AppInfo.App != ConfigurationStatus.AppType.SYYLGJ)) && ((AppInfo.App != ConfigurationStatus.AppType.TYGJ) && (AppInfo.App != ConfigurationStatus.AppType.NBAGJ))) && !AppInfo.cAppName.Contains("TBG")))
            {
                this.Lbl_AppHint.Location = new Point(0x33a, 8);
                this.Lbl_AppHint.Visible = ((count * this.Tab_Main.ItemSize.Width) <= 800) && (count < 10);
                if (AppInfo.App == ConfigurationStatus.AppType.TIYUGJ)
                {
                    this.Lbl_AppHint.Text = "【彩仙阁】技术支持";
                }
            }
        }

        private void RefreshTimesList()
        {
            string bTFNName = this.GetBTFNName();
            if (bTFNName != "")
            {
                List<ConfigurationStatus.TimesScheme> timesSchemeList = AppInfo.BTFNDic[bTFNName].TimesSchemeList;
                CommFunc.RefreshDataGridView(this.Egv_BTFNTimesList, timesSchemeList.Count);
            }
        }

        private void RefreshTJDataLater()
        {
            this.RefreshTJDataList1();
            this.RefreshTJDataList2();
            this.Btn_TJRefresh.Enabled = true;
            this.Pnl_TJRight2.Visible = false;
        }

        private void RefreshTJDataList1()
        {
            int count = this.TJViewList1.Count;
            CommFunc.RefreshDataGridView(this.Egv_TJDataList1, count);
            this.Egv_TJDataList1.AutoResizeColumns();
        }

        private void RefreshTJDataList2()
        {
            int count = this.TJViewList2.Count;
            CommFunc.RefreshDataGridView(this.Egv_TJDataList2, count);
        }

        private void RefreshTJDataMain(object pInfo)
        {
            try
            {
                ConfigurationStatus.TJData data = pInfo as ConfigurationStatus.TJData;
                ConfigurationStatus.Scheme scheme = this.GetScheme(data.FNName);
                if (scheme == null)
                {
                    return;
                }
                List<ConfigurationStatus.OpenData> list = this.FilterTJOpenData(data);
                Dictionary<string, List<ConfigurationStatus.SCPlan>> dictionary = new Dictionary<string, List<ConfigurationStatus.SCPlan>>();
                int num = list.Count;
                int num2 = (AppInfo.DataList.Count - scheme.FNBaseInfo.BetsJKValue.Length) - 1;
                if (num > num2)
                {
                    num = num2;
                }
                ConfigurationStatus.BetsScheme pBetsScheme = new ConfigurationStatus.BetsScheme();
                ConfigurationStatus.AutoBets pBets = new ConfigurationStatus.AutoBets();
                string name = scheme.Name;
                pBetsScheme.SchemeInfo = scheme;
                if (this.CountTimes(pBetsScheme, ref pBets.ErrorState) && pBetsScheme.SchemeInfo.FNBaseInfo.CheckCount(pBetsScheme, ref pBets.ErrorState))
                {
                    int num4;
                    ConfigurationStatus.OpenData openDataByExpect;
                    ConfigurationStatus.SCPlan plan;
                    string date;
                    string str3;
                    ConfigurationStatus.TJDataView1 view2;
                    pBetsScheme.LSDataInfo = data.LSDataInfo;
                    pBetsScheme.LSDataInfo.SchemeInfo = pBetsScheme.SchemeInfo;
                    pBets.BetsSchemeDic[name] = pBetsScheme;
                    double pGain = 0.0;
                    for (num4 = num - 1; num4 >= 0; num4--)
                    {
                        openDataByExpect = list[num4];
                        plan = new ConfigurationStatus.SCPlan {
                            UploadTime = openDataByExpect.Time,
                            FNName = scheme.Name,
                            FNCHType = scheme.CHType,
                            LotteryName = data.LotteryName,
                            LotteryID = data.LotteryID,
                            PlayType = pBetsScheme.PlayType,
                            PlayName = pBetsScheme.PlayName,
                            RXWZ = pBetsScheme.RXWZ,
                            RXZJ = pBetsScheme.RXZJ
                        };
                        plan.BeginExpect = plan.EndExpect = openDataByExpect.Expect;
                        ConfigurationStatus.SCTimesCount times = pBetsScheme.Times;
                        plan.Unit = times.UnitIndex;
                        plan.UnitType = times.Unit;
                        plan.Money = this.GetBetMoney(times.Unit);
                        plan.Cycle = 1;
                        plan.CurrentCycle = 1;
                        plan.TimesType = times.BTType;
                        plan.TimesDic = times.GetBetsTimesList;
                        plan.FBDic = times.FBList;
                        plan.BTFNName = times.FNName;
                        plan.Mode = data.Prize / Math.Pow(10.0, (double) (plan.Unit - 1));
                        if (AppInfo.PTInfo.IsBetsMoney1(plan.UnitType))
                        {
                            plan.Mode /= 2.0;
                        }
                        int openDataIndexByExpect = this.GetOpenDataIndexByExpect(openDataByExpect.Expect, null);
                        if ((openDataIndexByExpect + 1) < AppInfo.DataList.Count)
                        {
                            Dictionary<string, ConfigurationStatus.BetsCode> pFNNumberDic = scheme.FNBaseInfo.CountNumber(pBetsScheme, AppInfo.DataList, openDataIndexByExpect + 1);
                            if (!pBetsScheme.SchemeInfo.FNBaseInfo.IsBetsZJ)
                            {
                                this.ReverseCode(ref pFNNumberDic, pBetsScheme.SchemeInfo.FNBaseInfo);
                            }
                            if (pBetsScheme.IsQQG)
                            {
                                this.AddQQGData(ref pFNNumberDic, pBetsScheme, plan.CurrentExpect);
                            }
                            this.CheckFNYKStop(ref pFNNumberDic, pBets, pBetsScheme, pGain);
                            ConfigurationStatus.FNBase.AddFNBTIndex(pFNNumberDic, pBetsScheme.FNBTIndexDic);
                            pBetsScheme.FNNumberDic = pFNNumberDic;
                            pBetsScheme.PTNumberDic = pBetsScheme.ConvertFNBetsCode(pBets, plan, pBetsScheme, pBetsScheme.FNNumberDic);
                            plan.NumberList = pBetsScheme.GetPTNumberList;
                            plan.Number = CommFunc.GetBetsCodeCount(plan.GetPTNumberList(null), plan.Play, plan.RXWZ);
                            if ((this.VerificationPlan(pBets, pBetsScheme, plan, false) && !plan.CheckPlanStringIsNoOpen()) && (!plan.CheckPlanStringIsTXFFCCH() && (plan.Number != 0)))
                            {
                                pBets.CountAutoTimes(pBetsScheme, plan);
                            }
                            if (data.IsReset && (num4 >= 1))
                            {
                                date = openDataByExpect.Time.ToShortDateString();
                                str3 = list[num4 - 1].Time.ToShortDateString();
                                if (date != str3)
                                {
                                    pBetsScheme.FNBTIndexDic.Clear();
                                    pBetsScheme.ZuKeyDic.Clear();
                                    pGain = 0.0;
                                }
                            }
                            pGain += plan.Gain;
                            pBets.PlanList.Add(plan);
                        }
                    }
                    this.TJViewList1.Clear();
                    this.TJViewList2.Clear();
                    this.TJZuKeyDic.Clear();
                    Dictionary<string, List<string>> dictionary3 = new Dictionary<string, List<string>>();
                    Dictionary<string, List<int>> dictionary4 = new Dictionary<string, List<int>>();
                    double num6 = 0.0;
                    double num7 = 0.0;
                    Dictionary<string, ConfigurationStatus.TJDataView1> dictionary5 = new Dictionary<string, ConfigurationStatus.TJDataView1>();
                    int item = pBets.PlanList.Count;
                    for (num4 = 0; num4 < pBets.PlanList.Count; num4++)
                    {
                        plan = pBets.PlanList[num4];
                        ConfigurationStatus.TJDataView2 view = new ConfigurationStatus.TJDataView2 {
                            Expect = plan.CurrentExpect
                        };
                        openDataByExpect = this.GetOpenDataByExpect(plan.CurrentExpect, null);
                        view.CodeList = openDataByExpect.CodeList;
                        view.Date = openDataByExpect.Time.ToString("yyyy-MM-dd");
                        if (!dictionary5.ContainsKey(view.Date))
                        {
                            dictionary5[view.Date] = new ConfigurationStatus.TJDataView1(view.Date);
                        }
                        view2 = dictionary5[view.Date];
                        view.Time = openDataByExpect.Time.ToString("HH:mm");
                        List<ConfigurationStatus.SCPlan> pTempPlanList = new List<ConfigurationStatus.SCPlan>();
                        this.CombinaPlanValue(plan, pTempPlanList, null);
                        double num9 = 0.0;
                        foreach (ConfigurationStatus.SCPlan plan2 in pTempPlanList)
                        {
                            foreach (string str4 in plan2.ZuKeyDic.Keys)
                            {
                                string key = str4;
                                if (!dictionary3.ContainsKey(key))
                                {
                                    dictionary3[key] = new List<string>();
                                }
                                if (!dictionary4.ContainsKey(key))
                                {
                                    dictionary4[key] = new List<int>();
                                }
                                if (!plan2.CheckPlanStringIsTXFFCCH())
                                {
                                    if (plan2.FNAutoIsError(key))
                                    {
                                        dictionary3[key].Add(view.Time);
                                        dictionary4[key].Add(item);
                                        view2.NoCount++;
                                    }
                                    else
                                    {
                                        dictionary3[key].Clear();
                                        dictionary4[key].Clear();
                                        view2.YesCount++;
                                    }
                                    int count = dictionary3[key].Count;
                                    if (view2.MaxLC < count)
                                    {
                                        view2.MaxLC = count;
                                    }
                                    List<double> list3 = plan.GetTimes(key);
                                    if ((count > 0) && ((count % list3.Count) == 0))
                                    {
                                        view2.ZRCount++;
                                        List<string> list4 = dictionary3[key];
                                        string str6 = list4[list4.Count - list3.Count];
                                        string str7 = list4[list4.Count - 1];
                                        List<int> list5 = dictionary4[key];
                                        int num11 = list5[list5.Count - list3.Count];
                                        int num12 = list5[list5.Count - 1] - 1;
                                        view2.ZRDic[view2.ZRCount.ToString()] = string.Concat(new object[] { key, "|", str6, "-", str7, "|", num12, "-", num11 });
                                    }
                                    plan2.TJBZCountDic[key] = dictionary3[key].Count;
                                }
                            }
                            view2.BetsCount++;
                            view2.XZMoney += plan2.FNAutoTotalMoney;
                            this.TJZuKeyDic[plan2.ZuKey] = 0;
                            view.ValueDic[plan2.ZuKey] = plan2;
                            num9 += plan2.Gain;
                        }
                        view2.Gain += num9;
                        num6 += num9;
                        num7 += num9;
                        if (num7 >= 0.0)
                        {
                            if (view2.MaxYL < num7)
                            {
                                view2.MaxYL = num7;
                            }
                        }
                        else if (view2.MaxKS > num7)
                        {
                            view2.MaxKS = num7;
                        }
                        view.Gain1 = num9;
                        view.Gain2 = num6;
                        view.Gain3 = num7;
                        this.TJViewList2.Add(view);
                        if (data.IsReset && (num4 < (pBets.PlanList.Count - 1)))
                        {
                            date = view.Date;
                            str3 = pBets.PlanList[num4 + 1].UploadTime.ToString("yyyy-MM-dd");
                            if (date != str3)
                            {
                                num6 = 0.0;
                                num7 = 0.0;
                                dictionary3.Clear();
                                dictionary4.Clear();
                            }
                        }
                        item--;
                    }
                    ConfigurationStatus.TJDataView1 view3 = new ConfigurationStatus.TJDataView1("合计");
                    foreach (string str8 in dictionary5.Keys)
                    {
                        view2 = dictionary5[str8];
                        this.TJViewList1.Add(view2);
                        if (view2.BetsCount > view3.BetsCount)
                        {
                            view3.BetsCount = view2.BetsCount;
                        }
                        if (view2.YesCount > view3.YesCount)
                        {
                            view3.YesCount = view2.YesCount;
                        }
                        if (view2.NoCount > view3.NoCount)
                        {
                            view3.NoCount = view2.NoCount;
                        }
                        if (view2.MaxYL > view3.MaxYL)
                        {
                            view3.MaxYL = view2.MaxYL;
                        }
                        if (view2.MaxKS < view3.MaxKS)
                        {
                            view3.MaxKS = view2.MaxKS;
                        }
                        view3.XZMoney += view2.XZMoney;
                        view3.Gain += view2.Gain;
                        if (view2.MaxLC > view3.MaxLC)
                        {
                            view3.MaxLC = view2.MaxLC;
                        }
                        if (view2.ZRCount > view3.ZRCount)
                        {
                            view3.ZRCount = view2.ZRCount;
                        }
                    }
                    this.TJViewList1.Add(view3);
                    this.TJViewList2.Reverse();
                }
            }
            catch (Exception exception)
            {
                DebugLog.SaveDebug(exception, "");
                MessageBox.Show(exception.ToString());
            }
            base.Invoke(AppInfo.RefreshTJDataLater);
        }

        private void RefreshTJFNList()
        {
            CommFunc.SetComboBoxList(this.Cbb_TJFN, AppInfo.SchemeList);
        }

        private void RefreshTJPlay()
        {
            string selectTJFN = this.GetSelectTJFN();
            ConfigurationStatus.Scheme scheme = this.GetScheme(selectTJFN);
            this.Lbl_TJPlayValue.Text = scheme.FNBaseInfo.ViewPlay;
            this.Lbl_TJPlayValue.Visible = true;
            this.RefreshTJPrize();
        }

        private void RefreshTJPrize()
        {
            try
            {
                int selectedIndex = this.Cbb_TJPrize.SelectedIndex;
                this.Txt_TJPrize.ReadOnly = selectedIndex == 0;
                string selectTJFN = this.GetSelectTJFN();
                if (((selectTJFN != "") && (selectedIndex == 0)) && AppInfo.PTInfo.PTLoginStatus)
                {
                    ConfigurationStatus.Scheme scheme = this.GetScheme(selectTJFN);
                    string prize = AppInfo.PTInfo.GetPrize(AppInfo.Current.Lottery.Type, scheme.FNBaseInfo.Play);
                    this.Txt_TJPrize.Text = prize;
                }
            }
            catch
            {
            }
        }

        private void RefreshUserMain(bool pRefreshData)
        {
            this.RefreshTJPrize();
            if (pRefreshData)
            {
                this.RefreshBank();
            }
        }

        private void RefreshVerifyCode()
        {
        }

        private void RemoveLoginLock(string pError, string pName = "")
        {
            this.RefreshLogin(false, $"登录失败-{pError}");
        }

        public void ResetTime()
        {
            int pIndex = 0;
            string str = AppInfo.Current.CheckTimeInOpenTime(ref pIndex);
            int hour = Convert.ToInt32(str.Split(new char[] { ':' })[0]);
            int minute = Convert.ToInt32(str.Split(new char[] { ':' })[1]);
            int second = Convert.ToInt32(str.Split(new char[] { ':' })[2]);
            AppInfo.NextTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, second);
        }

        private void ReverseCode(ref Dictionary<string, ConfigurationStatus.BetsCode> pFNNumberDic, ConfigurationStatus.FNBase pFNBase)
        {
            string playType = pFNBase.PlayType;
            string playName = pFNBase.PlayName;
            ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(playType, playName);
            string play = playInfo.Play;
            int codeCount = playInfo.CodeCount;
            foreach (string str4 in pFNNumberDic.Keys)
            {
                ConfigurationStatus.BetsCode code = pFNNumberDic[str4];
                List<string> codeList = code.CodeList;
                if (!(pFNBase is ConfigurationStatus.FNWJJH) || (codeList.Count != 0))
                {
                    List<string> list3;
                    List<string> list2 = new List<string>();
                    if (CommFunc.CheckPlayIsFS(play))
                    {
                        list3 = CommFunc.GetCombinaList(ConfigurationStatus.CombinaType.ZX, 1, -1, -1);
                        foreach (string str5 in codeList)
                        {
                            string item = "";
                            if (str5 != "*")
                            {
                                foreach (string str7 in list3)
                                {
                                    if (!str5.Contains(str7))
                                    {
                                        item = item + str7;
                                    }
                                }
                                list2.Add(item);
                            }
                        }
                    }
                    else if (CommFunc.CheckPlayIsLH(play))
                    {
                        list3 = CommFunc.ConvertSameListString("龙虎和");
                        foreach (string str7 in list3)
                        {
                            if (!codeList.Contains(str7))
                            {
                                list2.Add(str7);
                            }
                        }
                    }
                    else
                    {
                        list3 = CommFunc.GetCombinaList(CommFunc.GetCombinaType(play), codeCount, -1, -1);
                        foreach (string str7 in list3)
                        {
                            if (!codeList.Contains(str7))
                            {
                                list2.Add(str7);
                            }
                        }
                    }
                    code.CodeList = list2;
                }
            }
        }

        private void runProgress_ButtonClick(object sender, EventArgs e)
        {
            this.mainThread.loadThread.Abort();
            this.mainThread.loadThread.Join();
            this.ThreadEnd();
        }

        private bool SaveAllBTFNData(bool pHint, string path = "")
        {
            if (path == "")
            {
                path = BTFNPath;
            }
            CommFunc.CreateDirectory(path);
            foreach (string str in AppInfo.BTFNDic.Keys)
            {
                string pFile = path + str + ".txt";
                string fileValue = AppInfo.BTFNDic[str].GetFileValue();
                CommFunc.WriteTextFileToStr(pFile, fileValue);
            }
            if (pHint)
            {
                CommFunc.PublicMessageAll("保存成功！", true, MessageBoxIcon.Asterisk, "");
            }
            return true;
        }

        private void SaveAllSchemeData(string path = "")
        {
            if (path == "")
            {
                path = this.SchemePath;
            }
            foreach (string str in AppInfo.Account.SchemeDic.Keys)
            {
                this.SaveSchemeData(str, path);
            }
        }

        public void SaveControlInfoByReg()
        {
            CommFunc.SaveFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
            CommFunc.WriteRegValue(this.RegConfigPath, "PTNameValue", this.Cbb_LoginPT.Text);
            this.BT_Main.SaveControlInfoByReg();
            this.TB_Main.SaveControlInfoByReg();
            this.Zbj_Main.SaveControlInfoByReg();
            this.HJFG_Main.SaveControlInfoByReg();
            this.CD_Main.SaveControlInfoByReg();
            this.TV_Main.SaveControlInfoByReg();
            this.SK_EX.SaveControlInfoByReg();
            this.SK_SX.SaveControlInfoByReg();
            this.SavePTLine();
            this.SaveAllSchemeData("");
            this.SaveShowTapData();
            CommFunc.SaveConfiguration();
        }

        public void SaveErrorVerifyCode(string pFile)
        {
            string verifyCodeErrorPath = VerifyCodeErrorPath;
            string destFileName = verifyCodeErrorPath + $"VerifyCode_{DateTime.Now.ToOADate()}.png";
            CommFunc.CreateDirectory(verifyCodeErrorPath);
            File.Move(pFile, destFileName);
        }

        private void SavePlanListData(bool pIsSave = false)
        {
            string betsPlanListPath = this.BetsPlanListPath;
            CommFunc.CreateDirectory(betsPlanListPath);
            foreach (string str2 in this.BetsDic.Keys)
            {
                ConfigurationStatus.AutoBets bets = this.BetsDic[str2];
                if (pIsSave || bets.PlanRun)
                {
                    string pFile = betsPlanListPath + bets.Name + ".txt";
                    List<string> pList = new List<string>();
                    foreach (ConfigurationStatus.SCPlan plan in bets.PlanList)
                    {
                        string loadPlanListData = plan.LoadPlanListData;
                        pList.Add(loadPlanListData);
                    }
                    CommFunc.WriteTextFile(pFile, pList);
                }
            }
        }

        public void SavePTInfoByReg()
        {
            CommFunc.WriteRegValue(this.RegConfigPath, this.PTName + this.Txt_ID.Name, this.Txt_ID.Text);
            CommFunc.WriteRegValue(this.RegConfigPath, this.PTName + this.Txt_PW.Name, this.Txt_PW.Text);
        }

        private void SavePTLine()
        {
            string pTLineFile = AppInfo.PTLineFile;
            List<string> pList = new List<string>();
            if (this.PTLineDic.Count != 0)
            {
                foreach (string str2 in this.PTLineDic.Keys)
                {
                    ConfigurationStatus.PTLine line = this.PTLineDic[str2];
                    string item = line.ID + " " + line.Name;
                    pList.Add(item);
                    foreach (string str4 in line.LineList)
                    {
                        pList.Add(str4);
                    }
                    pList.Add("");
                }
                if (pList.Count > 0)
                {
                    pList.RemoveAt(pList.Count - 1);
                }
                CommFunc.WriteTextFile(pTLineFile, pList);
            }
        }

        private void SaveSchemeData(string pName, string path = "")
        {
            if (path == "")
            {
                path = this.SchemePath;
            }
            ConfigurationStatus.Scheme scheme = AppInfo.Account.SchemeDic[pName];
            string fileValue = scheme.GetFileValue();
            string pFile = path + pName + "-" + scheme.CHType + ".txt";
            DirectoryInfo info = new DirectoryInfo(path);
            foreach (FileInfo info2 in info.GetFiles(pName + "-*.txt"))
            {
                info2.Delete();
            }
            CommFunc.WriteTextFileToStr(pFile, fileValue);
        }

        private void SaveShowTapData()
        {
            List<string> pList = new List<string>();
            foreach (ConfigurationStatus.ShowTap tap in this.ShowTapList)
            {
                pList.Add(tap.GetValue());
            }
            string regValue = CommFunc.Join(pList, ",");
            CommFunc.WriteRegValue(this.RegConfigPath, "ShowTapData", regValue);
        }

        private bool SetBetsInfo(ConfigurationStatus.AutoBets pBets)
        {
            pBets.LotteryName = AppInfo.Current.Lottery.Name;
            pBets.LotteryID = AppInfo.Current.Lottery.ID;
            pBets.ShareBetsInfo = new ConfigurationStatus.ShareBets();
            if (!(this.Pnl_BetsType.Visible && !this.Rdb_CGBets.Checked))
            {
                pBets.ShareBetsInfo.BetsTypeInfo = ConfigurationStatus.BetsType.FNBets;
            }
            else if (this.Rdb_ShareBets.Text == "共享投注")
            {
                pBets.ShareBetsInfo.BetsTypeInfo = ConfigurationStatus.BetsType.SendBets;
            }
            else
            {
                pBets.ShareBetsInfo.BetsTypeInfo = ConfigurationStatus.BetsType.FollowBets;
                string str = CommFunc.ReadRegString(this.RegConfigPath, "ShareCode", "");
                pBets.ShareBetsInfo.ShareCode = str;
                if (pBets.ShareBetsInfo.ShareCode == "")
                {
                    CommFunc.PublicMessageAll("请输入上级发给你的共享码！", true, MessageBoxIcon.Asterisk, "");
                    this.Ckb_ShareBetsManage_Click(null, null);
                    return false;
                }
                if (pBets.ShareBetsInfo.ShareUser == "")
                {
                    CommFunc.PublicMessageAll("您输入的共享码错误，请重新输入！", true, MessageBoxIcon.Asterisk, "");
                    this.Ckb_ShareBetsManage_Click(null, null);
                    return false;
                }
            }
            return true;
        }

        public void SetControlInfoByReg()
        {
            this.RegConfigPath = @"software\TUHAOPLUS\YXZXGJ\DlgConfig\永信在线挂机软件\" + this.Text.Split(new char[] { 'V' })[0];
            List<Control> list = new List<Control> {
                this,
                this.Txt_KSStopBets,
                this.Txt_YLStopBets,
                this.Dtp_BetsBeginTime,
                this.Dtp_BetsEndTime,
                this.Ckb_MN1,
                this.Ckb_MN2,
                this.Ckb_MN3,
                this.Ckb_MN4,
                this.Txt_MN1,
                this.Txt_MN2,
                this.Txt_MN3,
                this.Txt_MN4,
                this.Ckb_PlaySound,
                this.Ckb_LSBJ,
                this.Rdb_LSBJType,
                this.Rdb_LSBJExpect,
                this.Nm_LSBJExpect,
                this.Ckb_LSAutoRefresh,
                this.Nm_BetsTime,
                this.Txt_TJPrize,
                this.Ckb_TJReset,
                this.Ckb_TJTimeRange,
                this.Dtp_TJTimeRange1,
                this.Dtp_TJTimeRange2,
                this.Nm_TJFindXS,
                this.Ckb_PlanShowHide,
                this.Ckb_BetsSort,
                this.Ckb_RrfreshPT,
                this.Ckb_Data,
                this.Ckb_OpenHint,
                this.Ckb_CloseMin,
                this.Nm_BTFNEdit,
                this.Ckb_BTFNEditSkip,
                this.Ckb_FNLT,
                this.Ckb_DQStopBets,
                this.Ckb_SBStopBets,
                this.Ckb_DeleteExpect,
                this.Nm_DeleteExpect,
                this.Rdb_ShareBets,
                this.Rdb_CGBets
            };
            this.ControlList = list;
            List<Control> list2 = new List<Control> {
                this.Cbb_Lottery,
                this.Cbb_LSBJType,
                this.Cbb_BetsEndType,
                this.Cbb_TJPrize,
                this.Cbb_BTFNEdit
            };
            this.SpecialControlList = list2;
            CommFunc.CheckFIPSEnable();
            this.LoadPTLine("");
            this.LoadShowTapData();
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
            if (this.Cbb_Lottery.SelectedIndex == -1)
            {
                this.Cbb_Lottery.SelectedIndex = 0;
            }
            if ((AppInfo.App == ConfigurationStatus.AppType.YHGJ) || (AppInfo.App == ConfigurationStatus.AppType.YBGJ))
            {
                this.Nm_BetsTime.Value = 2M;
                this.Ckb_DQStopBets.Checked = false;
                if (AppInfo.App == ConfigurationStatus.AppType.YBGJ)
                {
                    CommFunc.SetComboBoxSelectedIndex(this.Cbb_Lottery, AppInfo.Current.LotteryDic["TXFFC"].Name);
                }
            }
            this.Cbb_LSBJType.SelectedIndex = 4;
            AppInfo.PlaySound = this.Ckb_PlaySound.Checked;
            this.Dtp_LSDataRange.Value = DateTime.Now;
            this.Dtp_TJDataRange1.Value = this.Dtp_TJDataRange2.Value = DateTime.Now;
        }

        private void SetLableLoading()
        {
            this.Lbl_BankBalanceValue.Text = "加载中...";
        }

        public void SetPTInfoByReg()
        {
            this.Txt_ID.Text = CommFunc.ReadRegString(this.RegConfigPath, this.PTName + this.Txt_ID.Name, "");
            this.Txt_PW.Text = CommFunc.ReadRegString(this.RegConfigPath, this.PTName + this.Txt_PW.Name, "");
        }

        private void SetPTLoginHint()
        {
            string text = this.Cbb_LoginPT.Text;
        }

        private void SetSchemeControl(ConfigurationStatus.Scheme pInfo)
        {
            if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.DMLH)
            {
                this.FN_DMLH.SetControlValue(pInfo.FNBaseInfo);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GJDMLH)
            {
                this.FN_GJDMLH.SetControlValue(pInfo.FNBaseInfo);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GJKMTM)
            {
                this.FN_GJKMTM.SetControlValue(pInfo.FNBaseInfo);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.LRWCH)
            {
                this.FN_LRWCH.SetControlValue(pInfo.FNBaseInfo);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.BCFCH)
            {
                this.FN_BCFCH.SetControlValue(pInfo.FNBaseInfo);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.WJJH)
            {
                this.FN_WJJH.SetControlValue(pInfo.FNBaseInfo);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.SJCH)
            {
                this.FN_SJCH.SetControlValue(pInfo.FNBaseInfo);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.YLCH)
            {
                this.FN_YLCH.SetControlValue(pInfo.FNBaseInfo);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.KMTM)
            {
                this.FN_KMTM.SetControlValue(pInfo.FNBaseInfo);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GDQM)
            {
                this.FN_GDQM.SetControlValue(pInfo.FNBaseInfo);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.LHKMTM)
            {
                this.FN_LHKMTM.SetControlValue(pInfo.FNBaseInfo);
            }
        }

        private void SetSchemeControlTimes(ConfigurationStatus.Scheme pInfo, string pValue)
        {
            if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.DMLH)
            {
                this.FN_DMLH.SetBTPlanValue(pValue);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GJDMLH)
            {
                this.FN_GJDMLH.SetBTPlanValue(pValue);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GJKMTM)
            {
                this.FN_GJKMTM.SetBTPlanValue(pValue);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.LRWCH)
            {
                this.FN_LRWCH.SetBTPlanValue(pValue);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.BCFCH)
            {
                this.FN_BCFCH.SetBTPlanValue(pValue);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.WJJH)
            {
                this.FN_WJJH.SetBTPlanValue(pValue);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.SJCH)
            {
                this.FN_SJCH.SetBTPlanValue(pValue);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.YLCH)
            {
                this.FN_YLCH.SetBTPlanValue(pValue);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.KMTM)
            {
                this.FN_KMTM.SetBTPlanValue(pValue);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.GDQM)
            {
                this.FN_GDQM.SetBTPlanValue(pValue);
            }
            else if (pInfo.FNCHType == ConfigurationStatus.SchemeCHType.LHKMTM)
            {
                this.FN_LHKMTM.SetBTPlanValue(pValue);
            }
        }

        private void SetTabIndex()
        {
            string text = this.Tap_ZDBets.Text;
            CommFunc.SetTabSelectIndex(this.Tab_Main, text);
            this.Tab_Main_SelectedIndexChanged(null, null);
        }

        private void SetTapShow(string pText, bool pSelected, bool pIsRefresh = true)
        {
            Dictionary<string, ConfigurationStatus.ShowTap> showTapDic = this.GetShowTapDic();
            if (showTapDic.ContainsKey(pText))
            {
                ConfigurationStatus.ShowTap pInfo = showTapDic[pText];
                pInfo.Selected = pSelected;
                this.SetTapVis(pInfo, pIsRefresh);
            }
        }

        private void SetTapVis(ConfigurationStatus.ShowTap pInfo, bool pIsRefresh = true)
        {
            foreach (TabPage page in this.SettingPageList)
            {
                if (page.Tag.ToString() != pInfo.Name)
                {
                    continue;
                }
                if (!pInfo.Selected)
                {
                    page.Parent = null;
                }
                else
                {
                    if (page.Parent == this.Tab_Main)
                    {
                        break;
                    }
                    Dictionary<string, ConfigurationStatus.ShowTap> showTapDic = this.GetShowTapDic();
                    int index = this.Tab_Main.TabPages.Count - 1;
                    for (int i = 0; i < this.Tab_Main.TabPages.Count; i++)
                    {
                        TabPage page2 = this.Tab_Main.TabPages[i];
                        string key = page2.Tag.ToString();
                        if (showTapDic.ContainsKey(key) && (showTapDic[key].Index > pInfo.Index))
                        {
                            index = i;
                            break;
                        }
                    }
                    this.Tab_Main.TabPages.Insert(index, page);
                }
                if (pIsRefresh)
                {
                    this.RefreshShowTapList();
                }
                this.RefreshTab(false);
                break;
            }
        }

        private void SortSchemeList(List<ConfigurationStatus.SCPlan> planList)
        {
            try
            {
                planList.Sort(delegate (ConfigurationStatus.SCPlan info1, ConfigurationStatus.SCPlan info2) {
                    string fNName = info1.FNName;
                    string pStr = info2.FNName;
                    string number = CommFunc.GetNumber(fNName);
                    string str4 = CommFunc.GetNumber(pStr);
                    if (number == "")
                    {
                        return 1;
                    }
                    if (str4 == "")
                    {
                        return -1;
                    }
                    int num = Convert.ToInt32(CommFunc.GetNumber(fNName));
                    int num2 = Convert.ToInt32(CommFunc.GetNumber(pStr));
                    return num - num2;
                });
            }
            catch
            {
            }
        }

        private void SortSchemeList(List<string> pSchemeList)
        {
            try
            {
                pSchemeList.Sort(delegate (string name1, string name2) {
                    string number = CommFunc.GetNumber(name1);
                    string str2 = CommFunc.GetNumber(name2);
                    if (number == "")
                    {
                        return 1;
                    }
                    if (str2 == "")
                    {
                        return -1;
                    }
                    int num = Convert.ToInt32(CommFunc.GetNumber(name1));
                    int num2 = Convert.ToInt32(CommFunc.GetNumber(name2));
                    return num - num2;
                });
            }
            catch
            {
            }
        }

        private void SortTimesList(List<ConfigurationStatus.TimesScheme> pTimesList)
        {
            pTimesList.Sort(delegate (ConfigurationStatus.TimesScheme info1, ConfigurationStatus.TimesScheme info2) {
                int iD = info1.ID;
                int num2 = info2.ID;
                return iD - num2;
            });
        }

        private void StartBackGround()
        {
            this.Nic_Hint.Visible = true;
            string tipText = this.Text.Split(new char[] { ' ' })[0];
            this.Nic_Hint.Text = tipText;
            this.Nic_Hint.ShowBalloonTip(0x2710, "永信在线挂机软件后台", tipText, ToolTipIcon.Info);
            base.Visible = false;
            base.ShowInTaskbar = false;
            base.Visible = false;
        }

        private void StartBetsMain(object oBets)
        {
            try
            {
                bool flag;
                ConfigurationStatus.AutoBets pBets = oBets as ConfigurationStatus.AutoBets;
                goto Label_02F3;
            Label_000E:
                if (pBets.StartBets)
                {
                    string getKey;
                    pBets.Expect = this.NextExpect;
                    pBets.BetsTime = Convert.ToInt32(this.Nm_BetsTime.Value);
                    if (pBets.IsSleepTime && (pBets.BetsTime != -1))
                    {
                        Thread.Sleep((int) (0x3e8 * pBets.BetsTime));
                        pBets.IsSleepTime = false;
                    }
                    pBets.DefaultOption(false);
                    if (Convert.ToBoolean(base.Invoke(AppInfo.BetsMain, new object[] { pBets })))
                    {
                        this.BetsMain2(pBets);
                    }
                    if (pBets.IsOutLoop)
                    {
                        return;
                    }
                    if (!pBets.IsBetsYes)
                    {
                        DebugLog.SaveLogList(pBets);
                        pBets.PlanRun = true;
                        this.RefreshControlThreading(pBets);
                        getKey = pBets.GetKey;
                        if (!this.BetsErrorList.Contains(getKey))
                        {
                            this.BetsErrorList.Add(getKey);
                            if (pBets.IsBetsTime && (pBets.ErrorState != "无"))
                            {
                                CommFunc.PlaySound("投注失败");
                            }
                        }
                        if (pBets.SBStopBets)
                        {
                            pBets.StartBets = false;
                        }
                        Thread.Sleep(0x7d0);
                    }
                    else
                    {
                        pBets.PlanRun = true;
                        this.RefreshControlThreading(pBets);
                        pBets.StartBets = false;
                        this.RefreshBank();
                        getKey = pBets.GetKey;
                        this.BetsErrorList.Remove(getKey);
                        if (pBets.BetsNumber > 0)
                        {
                            CommFunc.PlaySound("投注成功");
                        }
                    }
                }
                if ((pBets.ShareBetsInfo.BetsTypeInfo == ConfigurationStatus.BetsType.SendBets) && (pBets.ShareBetsInfo.SendPlanList.Count > 0))
                {
                    int pSendCount = 0;
                    for (int i = pBets.ShareBetsInfo.SendPlanList.Count - 1; i >= 0; i--)
                    {
                        ConfigurationStatus.SCPlan plan = pBets.ShareBetsInfo.SendPlanList[i];
                        if (SQLData.AddSharePlanRow(plan))
                        {
                            string currentExpect = plan.CurrentExpect;
                            if (!pBets.ShareBetsInfo.SendPlanCountDic.ContainsKey(currentExpect))
                            {
                                pBets.ShareBetsInfo.SendPlanCountDic[currentExpect] = 1;
                            }
                            else
                            {
                                Dictionary<string, int> dictionary;
                                string str3;
                                (dictionary = pBets.ShareBetsInfo.SendPlanCountDic)[str3 = currentExpect] = dictionary[str3] + 1;
                            }
                            pBets.ShareBetsInfo.SendPlanList.RemoveAt(i);
                            pSendCount++;
                        }
                    }
                    if (pSendCount > 0)
                    {
                        SQLData.AddSharePlanStateRow(pBets, pSendCount, pBets.IsBetsYes);
                    }
                    this.RefreshControlThreading(pBets);
                }
                Thread.Sleep(0x7d0);
            Label_02F3:
                flag = true;
                goto Label_000E;
            }
            catch (Exception exception)
            {
                DebugLog.SaveDebug(exception, "");
            }
        }

        private void StopAddFN(ref Dictionary<string, ConfigurationStatus.BetsCode> pFNNumberDic1, Dictionary<string, int> pFNNumberDic2)
        {
            List<string> dicKeyList = CommFunc.GetDicKeyList<ConfigurationStatus.BetsCode>(pFNNumberDic1);
            foreach (string str in dicKeyList)
            {
                if (!(pFNNumberDic2.ContainsKey(str) && (pFNNumberDic2[str] != 0)))
                {
                    pFNNumberDic1.Remove(str);
                }
            }
        }

        public void SwitchNextLine(bool pSwitch, string pName = "")
        {
            if (!AppInfo.PTInfo.IsLoginRun)
            {
                if (pSwitch)
                {
                    AppInfo.PTInfo.SwitchNextLine();
                }
                this.LoginMain();
            }
        }

        private void Tab_Main_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag = this._RunEvent;
            this._RunEvent = false;
            try
            {
                string text = this.Tab_Main.SelectedTab.Text;
                this.LastBetsIndex = text;
                if (this.LastBetsIndex == this.Tap_Scheme.Text)
                {
                    if (!this.IsEditScheme)
                    {
                        this.Egv_SchemeList.HeaderCheckBoxHide();
                        this.RefreshScheme();
                    }
                }
                else if (this.LastBetsIndex == this.Tap_ZDBets.Text)
                {
                    this.RefreshControl(true);
                }
                else if (this.LastBetsIndex == this.Tap_BTFN.Text)
                {
                    this.RefreshBTFNControl(true);
                }
            }
            catch
            {
            }
            this._RunEvent = flag;
        }

        public void ThreadEnd()
        {
            this.runProgress.Close();
            CommFunc.ClearObejct();
            CommFunc.SetForegroundWindow(base.Handle);
        }

        public void ThreadStart()
        {
            if (this.runProgress != null)
            {
                this.runProgress.Close();
            }
            this.runProgress = new FrmRunProgress("加载中...");
            this.runProgress.ButtonClick += new EventHandler<EventArgs>(this.runProgress_ButtonClick);
            this.runProgress.Show(new WindowWrap(base.Handle));
        }

        private void Tim_NextExpect_Tick(object sender, EventArgs e)
        {
            try
            {
                if (((AppInfo.NextTime.Hour == 0) && (AppInfo.NextTime.Minute == 0)) && (AppInfo.NextTime.Second == 0))
                {
                    double openInterval = 0.0;
                    if (AppInfo.Current.Lottery.ID == "CQSSC")
                    {
                        openInterval = ((DateTime.Now.Hour >= 10) && (DateTime.Now.Hour < 0x16)) ? ((double) 10) : ((double) 5);
                    }
                    else
                    {
                        openInterval = AppInfo.Current.Lottery.OpenInterval;
                    }
                    AppInfo.NextTime = AppInfo.NextTime.AddMinutes(openInterval);
                }
                AppInfo.NextTime = AppInfo.NextTime.AddSeconds(-1.0);
                this.FillNextTime();
                if (AppInfo.PTInfo.PTLoginStatus)
                {
                    ConfigurationStatus.AutoBets bets = this.GetBets("");
                    DateTime time2 = DateTime.Parse(AppInfo.Current.Lottery.TimeList[0].beginOpenTime).AddMinutes(-1.0 * AppInfo.Current.Lottery.OpenInterval).AddMinutes(AppInfo.Current.Lottery.OpenInterval);
                    DateTime now = DateTime.Now;
                    string key = DateTime.Now.ToShortDateString();
                    if (((!this.AutoBeginDic.ContainsKey(key) && this.CheckBetsBeginTimes()) && !this.CheckBetsEndTimes()) && !bets.PlanRun)
                    {
                        this.AutoBeginDic[key] = 1;
                        this.AutoBetsMain(true);
                    }
                    if ((!this.AutoEndDic.ContainsKey(key) && this.CheckBetsEndTimes()) && bets.PlanRun)
                    {
                        this.AutoEndDic[key] = 1;
                        this.AutoBetsMain(false);
                    }
                }
            }
            catch
            {
            }
        }

        private void Tsm_Colse_Click(object sender, EventArgs e)
        {
            this.CloseFormMain(true, true);
        }

        private void Tsm_Vis_Click(object sender, EventArgs e)
        {
            base.ShowInTaskbar = true;
            this.Nic_Hint.Visible = false;
            base.Visible = true;
        }

        private void Txt_Input1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar != '\b') && !char.IsDigit(e.KeyChar)) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            else
            {
                TextBox box = sender as TextBox;
                if ((box.Text == "") && (e.KeyChar == '.'))
                {
                    e.Handled = true;
                }
                if (box.Text.Contains(".") && (e.KeyChar == '.'))
                {
                    e.Handled = true;
                }
                if ((box.Text == "0") && (e.KeyChar == '0'))
                {
                    e.Handled = true;
                }
            }
        }

        private void Txt_Input2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((((e.KeyChar != '\b') && !char.IsDigit(e.KeyChar)) && ((e.KeyChar != '-') && (e.KeyChar != ','))) && (e.KeyChar != 0xff0c))
            {
                e.Handled = true;
            }
        }

        private void Txt_Input3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar != '\b') && !char.IsDigit(e.KeyChar)) && (e.KeyChar != ' '))
            {
                e.Handled = true;
            }
        }

        private void VerificationBetsPlan()
        {
            if (this.CheckUserID(false))
            {
                foreach (string str in this.BetsDic.Keys)
                {
                    ConfigurationStatus.BetsScheme scheme;
                    ConfigurationStatus.AutoBets pBets = this.BetsDic[str];
                    int index = 0;
                    while (index < pBets.PlanList.Count)
                    {
                        ConfigurationStatus.SCPlan plan = pBets.PlanList[index];
                        string fNName = plan.FNName;
                        scheme = ((fNName != null) && pBets.BetsSchemeDic.ContainsKey(fNName)) ? pBets.BetsSchemeDic[fNName] : null;
                        if ((plan.CheckPlanIsWait() && this.VerificationPlan(pBets, scheme, plan, false)) && (!plan.CheckPlanStringIsNoOpen() && !plan.CheckPlanStringIsTXFFCCH()))
                        {
                            pBets.CountAutoTimes(scheme, plan);
                        }
                        index++;
                    }
                    foreach (string str2 in pBets.BetsSchemeDic.Keys)
                    {
                        scheme = pBets.BetsSchemeDic[str2];
                        if ((scheme != null) && (scheme.FNBTIndexDic.Count != 0))
                        {
                            double num2 = pBets.Gain(false);
                            List<string> dicKeyList = CommFunc.GetDicKeyList<int>(scheme.FNBTIndexDic);
                            foreach (string str3 in dicKeyList)
                            {
                                if (scheme.Times.BTType == ConfigurationStatus.SCTimesType.FN)
                                {
                                    int num3 = scheme.FNBTIndexDic[str3];
                                    if (scheme.SchemeInfo.FNBaseInfo.GetYLHTMoney != "")
                                    {
                                        double num4 = Convert.ToDouble(scheme.SchemeInfo.FNBaseInfo.GetYLHTMoney) + pBets.FNGainSaveDic[str2];
                                        if ((num2 >= 0.0) && (num2 >= num4))
                                        {
                                            num3 = scheme.SchemeInfo.FNBaseInfo.YLHTID - 1;
                                            pBets.FNGainSaveDic[str2] = num2;
                                        }
                                    }
                                    if (scheme.SchemeInfo.FNBaseInfo.GetKSHTMoney != "")
                                    {
                                        double num5 = Convert.ToDouble(scheme.SchemeInfo.FNBaseInfo.GetKSHTMoney) + pBets.FNGainSaveDic[str2];
                                        if ((num2 < 0.0) && (Math.Abs(num2) >= num5))
                                        {
                                            num3 = scheme.SchemeInfo.FNBaseInfo.KSHTID - 1;
                                            pBets.FNGainSaveDic[str2] = Math.Abs(num2);
                                        }
                                    }
                                    scheme.FNBTIndexDic[str3] = num3;
                                }
                            }
                        }
                    }
                    if (this.Ckb_DeleteExpect.Checked)
                    {
                        int num6 = Convert.ToInt32(this.Nm_DeleteExpect.Value);
                        List<string> list2 = new List<string>();
                        for (index = pBets.PlanList.Count - 1; index >= 0; index--)
                        {
                            string currentExpect = pBets.PlanList[index].CurrentExpect;
                            if (!list2.Contains(currentExpect))
                            {
                                list2.Add(currentExpect);
                            }
                            if (list2.Count > num6)
                            {
                                pBets.PlanList.RemoveAt(index);
                            }
                        }
                    }
                }
            }
        }

        private bool VerificationPlan(ConfigurationStatus.AutoBets pBets, ConfigurationStatus.BetsScheme pScheme, ConfigurationStatus.SCPlan plan, bool pRefreshData)
        {
            ConfigurationStatus.SCPlan getLastPlan = pBets.GetLastPlan;
            for (int i = plan.CurrentCycle; i <= plan.Cycle; i++)
            {
                string currentExpect = plan.CurrentExpect;
                ConfigurationStatus.OpenData openDataByExpect = this.GetOpenDataByExpect(currentExpect, null);
                string str2 = (openDataByExpect == null) ? "" : openDataByExpect.Code;
                if (this.CheckTXFFCIsCH(currentExpect, null))
                {
                    plan.IsTXFFCCH = true;
                    plan.Money = plan.Gain = 0.0;
                    plan.State = plan.AlreadLottery;
                    plan.Code = str2;
                    return true;
                }
                if (str2 != "")
                {
                    plan.IsWin = CommFunc.VerificationCode(pScheme, plan, openDataByExpect.CodeList);
                    plan.State = plan.AlreadLottery;
                    List<ConfigurationStatus.SCPlan> pTempPlanList = new List<ConfigurationStatus.SCPlan>();
                    this.CombinaPlanValue(plan, pTempPlanList, null);
                    double num2 = 0.0;
                    double num3 = 0.0;
                    foreach (ConfigurationStatus.SCPlan plan3 in pTempPlanList)
                    {
                        num2 += plan3.Gain;
                        num3 += plan3.FNAutoTotalMoney;
                    }
                    plan.Gain = num2;
                    plan.TotalMoney = num3;
                    plan.Code = str2;
                    string fNName = plan.FNName;
                    double gain = plan.Gain;
                    double totalMoney = plan.TotalMoney;
                    if (getLastPlan != null)
                    {
                        Dictionary<string, double> dictionary;
                        string str4;
                        if (plan.IsMNState("", true))
                        {
                            if (!getLastPlan.FNMNGainDic.ContainsKey(fNName))
                            {
                                getLastPlan.FNMNGainDic[fNName] = gain;
                            }
                            else
                            {
                                (dictionary = getLastPlan.FNMNGainDic)[str4 = fNName] = dictionary[str4] + gain;
                            }
                            if (!getLastPlan.FNMNMoneyDic.ContainsKey(fNName))
                            {
                                getLastPlan.FNMNMoneyDic[fNName] = totalMoney;
                            }
                            else
                            {
                                (dictionary = getLastPlan.FNMNMoneyDic)[str4 = fNName] = dictionary[str4] + totalMoney;
                            }
                        }
                        else
                        {
                            if (!getLastPlan.FNGainDic.ContainsKey(fNName))
                            {
                                getLastPlan.FNGainDic[fNName] = gain;
                            }
                            else
                            {
                                (dictionary = getLastPlan.FNGainDic)[str4 = fNName] = dictionary[str4] + gain;
                            }
                            if (!getLastPlan.FNMoneyDic.ContainsKey(fNName))
                            {
                                getLastPlan.FNMoneyDic[fNName] = totalMoney;
                            }
                            else
                            {
                                (dictionary = getLastPlan.FNMoneyDic)[str4 = fNName] = dictionary[str4] + totalMoney;
                            }
                        }
                        plan.FNGainString = getLastPlan.FNGainString;
                        plan.FNMNGainString = getLastPlan.FNMNGainString;
                        plan.FNMoneyString = getLastPlan.FNMoneyString;
                        plan.FNMNMoneyString = getLastPlan.FNMNMoneyString;
                    }
                    if (!pBets.FNGainSaveDic.ContainsKey(fNName))
                    {
                        pBets.FNGainSaveDic[fNName] = 0.0;
                    }
                    return true;
                }
                if (this.GetOpenDataFirstExpect == null)
                {
                    return false;
                }
                if (string.Compare(this.GetOpenDataFirstExpect.Expect, currentExpect) > 0)
                {
                    plan.IsNoOpen = true;
                    plan.Gain = 0.0;
                    plan.State = plan.AlreadLottery;
                    return true;
                }
            }
            return false;
        }

        private void Web_Login_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string pUrl = e.Url.ToString();
            HttpHelper.isUserAgentSet = false;
            if (AppInfo.PTInfo.IsLoginRun)
            {
                List<string> list = new List<string> { 
                    "114导航",
                    "408 Request Time-out",
                    "An Error 522",
                    "502 Bad Gateway",
                    "无法显示此页",
                    "已取消网页导航"
                };
                string documentText = this.Web_Login.DocumentText;
                string outerText = this.Web_Login.Document.Body.OuterText;
                foreach (string str4 in list)
                {
                    if (pUrl.Contains(str4) || documentText.Contains(str4))
                    {
                        this.RefreshLogin(false, $"登录失败-{"服务器加载失败！"}");
                        this.SwitchNextLine(true, "");
                        return;
                    }
                }
                if (!(!this.CheckUrlIsPTIndex(pUrl) || AppInfo.PTInfo.IsLoadWebLogin))
                {
                    this.PTIndexMain("");
                }
                else if ((pUrl == AppInfo.PTInfo.GetLoginLine()) && ((AppInfo.Account.PTID != "") && (AppInfo.Account.PTPW != "")))
                {
                    if ((AppInfo.PTInfo == AppInfo.LFYLInfo) || (AppInfo.PTInfo == AppInfo.LF2Info))
                    {
                        AppInfo.PTInfo.Token = CommFunc.GetIndexString(documentText, "value=\"", "\"", documentText.IndexOf("__RequestVerificationToken"));
                        if (AppInfo.PTInfo.Token == "")
                        {
                            return;
                        }
                    }
                    else if (AppInfo.PTInfo == AppInfo.LUDIInfo)
                    {
                        AppInfo.PTInfo.Token = CommFunc.GetIndexString(documentText, "value=\"", "\"", documentText.IndexOf("__RequestVerificationToken"));
                        if (AppInfo.PTInfo.Token == "")
                        {
                            return;
                        }
                    }
                    bool flag = false;
                    string str4 = "未知原因";
                    if ((outerText != null) && outerText.Contains("Fiddler: DNS"))
                    {
                        str4 = "服务器加载失败！";
                    }
                    else
                    {
                        AppInfo.PTInfo.AnalysisVerifyCode = true;
                        AppInfo.PTInfo.LoginMain = true;
                        flag = true;
                    }
                    if (!flag)
                    {
                        this.RefreshLogin(false, $"登录失败-{str4}");
                        this.SwitchNextLine(true, "");
                    }
                }
            }
        }

        private void Web_Login_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (((AppInfo.PTInfo == AppInfo.LFYLInfo) || (AppInfo.PTInfo == AppInfo.LF2Info)) && !HttpHelper.isUserAgentSet)
            {
                e.Cancel = true;
                HttpHelper.isUserAgentSet = true;
                this.Web_Login.Navigate(e.Url, e.TargetFrameName, null, $"User-Agent: {HttpHelper.IE8}");
            }
        }

        private void Web_Login_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            WebBrowser browser = (WebBrowser) sender;
            string attribute = browser.Document.ActiveElement.GetAttribute("href");
            this.Web_Login.Navigate(attribute);
        }

        private void WebData()
        {
            try
            {
                if (!AppInfo.Account.Configuration.IsWJApp)
                {
                    if (AppInfo.Account.ID != "")
                    {
                        SQLData.UpdataUserOnLine(AppInfo.Account);
                        string pExtraString = SQLData.CheckUserState(AppInfo.Account.ID);
                        if (pExtraString != "")
                        {
                            DebugLog.SaveDebug(pExtraString, "账号异常");
                            base.Invoke(AppInfo.CloseApp);
                        }
                    }
                    this.CheckPTUserA();
                }
            }
            catch
            {
            }
        }

        private void WebHtmlClick(HtmlDocument pDocument, string pID)
        {
            pDocument.GetElementById(pID).InvokeMember("click");
        }

        public bool WebLoginMain()
        {
            if (AppInfo.Account.LoginStatus)
            {
                AppInfo.Account.LoginStatus = false;
                this.CloseAllBets();
                this.RefreshControl(true);
            }
            else if (AppInfo.Account.Configuration.IsPTLogin)
            {
                AppInfo.Account.LoginStatus = true;
                this.Tsp_LoginKey.Visible = this.Tsp_LoginValue.Visible = false;
                this.Tsp_HintKey.Visible = this.Tsp_HintValue.Visible = true;
            }
            else
            {
                FrmUserLogin login = new FrmUserLogin(AppInfo.Account);
                if (login.ShowDialog() == DialogResult.OK)
                {
                    AppInfo.Account.LoginStatus = true;
                    this.Tsp_LoginValue.Text = AppInfo.Account.ID;
                }
            }
            return AppInfo.Account.LoginStatus;
        }

        private void WebLoginOut()
        {
            AppInfo.PTInfo.QuitPT();
            this.LoadWebLoginIndex();
        }

        public string BetsPlanListPath =>
            (CommFunc.getDllPath() + @"\BetsPlanList\" + AppInfo.PTInfo.PTID + @"\" + AppInfo.Current.Lottery.ID + @"\" + AppInfo.Account.ID + @"\");

        public static string BTFNPath =>
            (CommFunc.getDllPath() + @"\GJBTScheme\");

        public ConfigurationStatus.OpenData GetOpenDataFirstExpect =>
            ((AppInfo.DataList.Count > 0) ? AppInfo.DataList[0] : null);

        public string GetOpenDataFirstExpectString
        {
            get
            {
                ConfigurationStatus.OpenData getOpenDataFirstExpect = this.GetOpenDataFirstExpect;
                return ((getOpenDataFirstExpect != null) ? getOpenDataFirstExpect.Expect : "");
            }
        }

        public ConfigurationStatus.OpenData GetOpenDataSecondExpect =>
            ((AppInfo.DataList.Count > 1) ? AppInfo.DataList[1] : null);

        public string GetOpenDataSecondExpectString
        {
            get
            {
                ConfigurationStatus.OpenData getOpenDataSecondExpect = this.GetOpenDataSecondExpect;
                return ((getOpenDataSecondExpect != null) ? getOpenDataSecondExpect.Expect : "");
            }
        }

        public string NextExpect
        {
            get
            {
                string text = this.Lbl_NextExpect.Text;
                if (text != "")
                {
                    text = text.Split(new char[] { ' ' })[1];
                }
                return text;
            }
        }

        public string SchemeLSDataPath =>
            (this.SchemePath + @"\LSData\");

        public string SchemeOldPath =>
            (CommFunc.getDllPath() + @"\Scheme\");

        public string SchemePath =>
            (CommFunc.getDllPath() + @"\Scheme\" + AppInfo.Current.Lottery.GroupString + @"\");

        public static string VerifyCodeErrorPath =>
            (VerifyCodePath + @"\ErrorList\");

        public static string VerifyCodeFile =>
            (VerifyCodePath + "VerifyCode.png");

        public static string VerifyCodePath =>
            (CommFunc.getDllPath() + @"\VerifyCode\");
    }
}

