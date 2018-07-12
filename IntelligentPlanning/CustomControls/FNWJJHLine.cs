namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.Properties;
    using mshtml;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class FNWJJHLine : UserControl
    {
        private bool _RunEvent = false;
        private BetsTimeLine BT_Time;
        public List<Button> ButtonList = null;
        private ComboBox Cbb_BTFN;
        private ComboBox Cbb_CSFXPlay;
        private ComboBox Cbb_FBType;
        private ComboBox Cbb_Mode;
        private ComboBox Cbb_PlayName;
        private ComboBox Cbb_PlayType;
        private ComboBox Cbb_Unit;
        private CheckBox Ckb_BTCount;
        private CheckBox Ckb_BTFN;
        private CheckBox Ckb_CSFXPlay;
        private CheckBox Ckb_CSFXSeparate;
        private CheckBox Ckb_KSHT;
        private CheckBox Ckb_KSStopBets;
        private CheckBox Ckb_MN1;
        private CheckBox Ckb_MN2;
        private CheckBox Ckb_MN3;
        private CheckBox Ckb_MN4;
        private CheckBox Ckb_More;
        private CheckBox Ckb_RefreshPlan;
        private CheckBox Ckb_WZ1;
        private CheckBox Ckb_WZ2;
        private CheckBox Ckb_WZ3;
        private CheckBox Ckb_WZ4;
        private CheckBox Ckb_WZ5;
        private CheckBox Ckb_YLHT;
        private CheckBox Ckb_YLStopBets;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private Image CrossImage;
        private IntPtr CurrentHWND;
        private ErrorProvider Err_Hint;
        private bool IsDrag = false;
        private bool IsMore = false;
        private bool IsUsedHWND = false;
        public List<Label> LabelList = null;
        private Label Lbl_CodeType;
        private Label Lbl_CSPlayName;
        private Label Lbl_CSSeparate;
        private Label Lbl_CSValueLeft;
        private Label Lbl_CSValueRight;
        private Label Lbl_FBType;
        private Label Lbl_HWNDHint;
        private Label Lbl_HWNDKey;
        private Label Lbl_KSHT;
        private Label Lbl_MNBets;
        private Label Lbl_Mode;
        private Label Lbl_ModeExpect;
        private Label Lbl_MoreHint;
        private Label Lbl_Name;
        private Label Lbl_NumberCount;
        private Label Lbl_PlayName;
        private Label Lbl_PlayType;
        private Label Lbl_RXZJ;
        private Label Lbl_StopBets;
        private Label Lbl_Unit;
        private Label Lbl_ViewPlan;
        private Label Lbl_YKHT;
        private Label Lbl_YLHT;
        private List<Control> MoreControl = null;
        private NumericUpDown Nm_KSHT;
        private NumericUpDown Nm_ModeExpect;
        private NumericUpDown Nm_YLHT;
        private Image NullImage;
        private PictureBox Pic_HWND;
        private Panel Pnl_CodeType;
        private Panel Pnl_MainInfo;
        private Panel Pnl_More;
        private Panel Pnl_PlanCS;
        private Panel Pnl_RX;
        private Panel Pnl_Times;
        private Panel Pnl_ViewPlan;
        private RadioButton Rdb_BTFN;
        private RadioButton Rdb_BTPlan;
        private RadioButton Rdb_CodeType1;
        private RadioButton Rdb_CodeType2;
        private string RegScenarionConfigPath = "";
        private List<CheckBox> RXWZList = null;
        private List<IntPtr> SkipHandleList = new List<IntPtr>();
        private List<Control> SpecialControlList = null;
        private TabControl Tab_Main;
        private TabPage Tap_PlanCS;
        private TabPage Tap_ViewPlan;
        private TextBox Txt_BTPlan;
        private TextBox Txt_CSPlayName;
        private TextBox Txt_CSSeparate;
        private TextBox Txt_CSValueLeft;
        private TextBox Txt_CSValueRight;
        private TextBox Txt_HWNDValue;
        private TextBox Txt_KSHT;
        private TextBox Txt_KSStopBets;
        private TextBox Txt_MN1;
        private TextBox Txt_MN2;
        private TextBox Txt_MN3;
        private TextBox Txt_MN4;
        private TextBox Txt_RXZJ;
        private RichTextBox Txt_ViewPlan;
        private TextBox Txt_YLHT;
        private TextBox Txt_YLStopBets;

        public FNWJJHLine()
        {
            this.InitializeComponent();
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Pnl_Times
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Pnl_PlanCS,
                    this.Pnl_ViewPlan
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Ckb_BTCount,
                    this.Ckb_BTFN,
                    this.Ckb_More,
                    this.Lbl_PlayType,
                    this.Lbl_PlayName,
                    this.Lbl_Unit,
                    this.Lbl_RXZJ,
                    this.Ckb_WZ1,
                    this.Ckb_WZ2,
                    this.Ckb_WZ3,
                    this.Ckb_WZ4,
                    this.Ckb_WZ5,
                    this.Rdb_BTPlan,
                    this.Rdb_BTFN,
                    this.Lbl_FBType,
                    this.Lbl_CodeType,
                    this.Rdb_CodeType1,
                    this.Rdb_CodeType2,
                    this.Lbl_YKHT,
                    this.Ckb_YLHT,
                    this.Lbl_YLHT,
                    this.Ckb_KSHT,
                    this.Lbl_KSHT,
                    this.Lbl_StopBets,
                    this.Ckb_YLStopBets,
                    this.Ckb_KSStopBets,
                    this.Ckb_MN1,
                    this.Ckb_MN2,
                    this.Ckb_MN3,
                    this.Ckb_MN4,
                    this.Lbl_MNBets,
                    this.Lbl_Mode,
                    this.Lbl_ModeExpect,
                    this.Lbl_HWNDKey,
                    this.Lbl_HWNDHint,
                    this.Lbl_CSSeparate,
                    this.Lbl_CSPlayName,
                    this.Lbl_CSValueLeft,
                    this.Lbl_CSValueRight,
                    this.Lbl_ViewPlan,
                    this.Ckb_RefreshPlan,
                    this.Ckb_CSFXSeparate,
                    this.Ckb_CSFXPlay
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
                List<TabControl> pTabControlList = new List<TabControl> {
                    this.Tab_Main
                };
                CommFunc.BeautifyTabControl(pTabControlList);
                List<ComboBox> pComboBoxList = new List<ComboBox> {
                    this.Cbb_PlayType,
                    this.Cbb_PlayName,
                    this.Cbb_Unit,
                    this.Cbb_BTFN,
                    this.Cbb_FBType,
                    this.Cbb_Mode,
                    this.Cbb_CSFXPlay
                };
                CommFunc.BeautifyComboBox(pComboBoxList);
            }
        }

        private void Cbb_CSFXPlay_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Txt_CSPlayName.Text = this.Cbb_CSFXPlay.Text;
        }

        private void Cbb_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.Cbb_Mode.Text;
            this.Nm_ModeExpect.Visible = this.Lbl_ModeExpect.Visible = text.Contains("N");
        }

        private void Cbb_PlayName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshCSControl(false);
            this.RefreshPlanMain(false);
            this.Ckb_WZ1.Visible = this.Ckb_WZ2.Visible = this.Ckb_WZ3.Visible = this.Ckb_WZ4.Visible = this.Ckb_WZ5.Visible = CommFunc.CheckPlayIsRXDS(this.Play);
            if (CommFunc.CheckPlayIsRXDS(this.Play))
            {
                for (int i = 4; i >= 0; i--)
                {
                    CheckBox box = this.RXWZList[i];
                    box.Checked = (4 - i) < this.PlayInfo.IndexList.Count;
                }
            }
        }

        private void Cbb_PlayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.Cbb_PlayType.Text;
            List<string> playNameList = CommFunc.GetPlayNameList(text);
            CommFunc.SetComboBoxList(this.Cbb_PlayName, playNameList);
            this.Cbb_PlayName.SelectedIndex = 0;
            this.Lbl_RXZJ.Visible = this.Txt_RXZJ.Visible = CommFunc.CheckPlayIsRX(text);
        }

        private void Ckb_BTCount_Click(object sender, EventArgs e)
        {
            string text = this.Txt_BTPlan.Text;
            ((AutoBetsWindow) Program.MainApp).QHBTCount(text);
        }

        private void Ckb_BTFN_Click(object sender, EventArgs e)
        {
            string text = this.Cbb_BTFN.Text;
            ((AutoBetsWindow) Program.MainApp).QHBTFN(text);
        }

        private void Ckb_CSFXPlay_Click(object sender, EventArgs e)
        {
            string hWDNValue = this.GetHWDNValue(true);
            if (hWDNValue != "")
            {
                string text = this.Txt_CSSeparate.Text;
                if (text == "")
                {
                    CommFunc.PublicMessageAll("分析玩法时【玩法分割符】不能为空！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_CSSeparate.Focus();
                }
                else
                {
                    List<string> pList = this.FXPlayMain(hWDNValue, text);
                    CommFunc.SetComboBoxList(this.Cbb_CSFXPlay, pList);
                }
            }
        }

        private void Ckb_CSFXSeparate_Click(object sender, EventArgs e)
        {
            string hWDNValue = this.GetHWDNValue(true);
            if (hWDNValue != "")
            {
                string str2 = this.FXSeparateMain(hWDNValue);
                this.Txt_CSSeparate.Text = str2;
                if (str2 == "")
                {
                    CommFunc.PublicMessageAll("程序分析到的分隔符为空，可能是单个玩法的计划！", true, MessageBoxIcon.Asterisk, "");
                }
            }
        }

        private void Ckb_KSHT_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_KSHT.Enabled = this.Lbl_KSHT.Enabled = this.Nm_KSHT.Enabled = this.Ckb_KSHT.Checked;
        }

        private void Ckb_KSStopBets_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_KSStopBets.Enabled = this.Ckb_KSStopBets.Checked;
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

        private void Ckb_More_Click(object sender, EventArgs e)
        {
            this.IsMore = !this.IsMore;
            this.RefreshMoreControl();
        }

        private void Ckb_RefreshPlan_Click(object sender, EventArgs e)
        {
            this.RefreshPlanMain(true);
        }

        private void Ckb_WZ_CheckedChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshPlanMain(false);
            }
        }

        private void Ckb_YLHT_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_YLHT.Enabled = this.Lbl_YLHT.Enabled = this.Nm_YLHT.Enabled = this.Ckb_YLHT.Checked;
        }

        private void Ckb_YLStopBets_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_YLStopBets.Enabled = this.Ckb_YLStopBets.Checked;
        }

        public static string CountHwndValue(string pHWND)
        {
            string controlText = "";
            IntPtr hwnd = CommFunc.ConvertIntPtr(pHWND);
            if (hwnd != IntPtr.Zero)
            {
                controlText = GetControlText(hwnd);
                if (controlText == "")
                {
                    controlText = GetHtmlDocument(hwnd);
                }
            }
            return controlText;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void DrawRevFrame(IntPtr hWnd)
        {
            if (!(hWnd == IntPtr.Zero))
            {
                IntPtr windowDC = CommFunc.GetWindowDC(hWnd);
                Rectangle lpRect = new Rectangle();
                CommFunc.GetWindowRect(hWnd, ref lpRect);
                CommFunc.OffsetRect(ref lpRect, -lpRect.Left, -lpRect.Top);
                CommFunc.PatBlt(windowDC, lpRect.Left, lpRect.Top, lpRect.Right - lpRect.Left, 3, 0x550009);
                CommFunc.PatBlt(windowDC, lpRect.Left, lpRect.Bottom - 3, 3, -((lpRect.Bottom - lpRect.Top) - 6), 0x550009);
                CommFunc.PatBlt(windowDC, lpRect.Right - 3, lpRect.Top + 3, 3, (lpRect.Bottom - lpRect.Top) - 6, 0x550009);
                CommFunc.PatBlt(windowDC, lpRect.Right, lpRect.Bottom - 3, -(lpRect.Right - lpRect.Left), 3, 0x550009);
            }
        }

        private void FNWJJHLine_Load(object sender, EventArgs e)
        {
            List<Label> list2 = new List<Label> {
                this.Lbl_Name,
                this.Lbl_NumberCount
            };
            this.LabelList = list2;
            CommFunc.SetLabelFormat(this.LabelList);
            List<Control> list3 = new List<Control> {
                this.Pnl_More
            };
            this.MoreControl = list3;
            this.Ckb_More.ForeColor = AppInfo.appForeColor;
            this.Lbl_MoreHint.ForeColor = AppInfo.redForeColor;
            this.SkipHandleList.Add(this.Pic_HWND.Handle);
            this.CrossImage = Resources.Drag;
            this.NullImage = Resources.Drag1;
            List<CheckBox> list4 = new List<CheckBox> {
                this.Ckb_WZ1,
                this.Ckb_WZ2,
                this.Ckb_WZ3,
                this.Ckb_WZ4,
                this.Ckb_WZ5
            };
            this.RXWZList = list4;
            CommFunc.SetControlHint(this.Err_Hint, this.Txt_CSSeparate, "针对多玩法的参数，如御彩轩、宝宝计划等，如果单玩法的计划则不需要设置");
            CommFunc.SetControlHint(this.Err_Hint, this.Txt_CSPlayName, "针对多玩法的参数，如御彩轩、宝宝计划等，如果单玩法的计划则不需要设置");
            CommFunc.SetControlHint(this.Err_Hint, this.Txt_CSValueLeft, "计划内容左边的符号，如\"088-089 皇家后三【02345789】\"则设置为【");
            CommFunc.SetControlHint(this.Err_Hint, this.Txt_CSValueRight, "计划内容右边的符号，如\"088-089 皇家后三【02345789】\"则设置为】");
            List<CheckBox> pCheckBoxList = new List<CheckBox> {
                this.Ckb_BTCount,
                this.Ckb_BTFN,
                this.Ckb_More,
                this.Ckb_RefreshPlan,
                this.Ckb_CSFXSeparate,
                this.Ckb_CSFXPlay
            };
            CommFunc.SetCheckBoxFormatFlat(pCheckBoxList);
            this.BeautifyInterface();
            CommFunc.AppHandler(this.Cbb_Unit);
            this.Ckb_MN1.Visible = this.Txt_MN1.Visible = this.Ckb_MN2.Visible = this.Txt_MN2.Visible = this.Ckb_MN3.Visible = this.Txt_MN3.Visible = this.Ckb_MN4.Visible = this.Txt_MN4.Visible = this.Lbl_MNBets.Visible = !AppInfo.Account.Configuration.IsHideMNBets;
            this._RunEvent = true;
        }

        private List<string> FXPlayMain(string pValue, string pSeparate)
        {
            List<string> list = new List<string>();
            string separate = GetSeparate(pValue);
            if (!separate.Contains(pSeparate))
            {
                CommFunc.PublicMessageAll("【玩法分割符】输入错误，无法分析到对应的玩法列表！", true, MessageBoxIcon.Asterisk, "");
                return list;
            }
            return CommFunc.GetDicKeyList<int>(GetPlayList(pValue, separate));
        }

        private string FXSeparateMain(string pValue)
        {
            string separate = GetSeparate(pValue);
            if (separate != "")
            {
                separate = separate[0].ToString();
            }
            return separate;
        }

        public static string GetControlText(IntPtr hwnd)
        {
            int capacity = 0xf4240;
            StringBuilder lParam = new StringBuilder(capacity);
            CommFunc.SendMessage(hwnd, 13, capacity, lParam);
            return lParam.ToString();
        }

        public ConfigurationStatus.FNWJJH GetControlValue(ref string pError)
        {
            ConfigurationStatus.FNBase pInfo = new ConfigurationStatus.FNWJJH();
            this.BT_Time.GetControlValue(ref pInfo);
            ConfigurationStatus.FNWJJH fnwjjh = (ConfigurationStatus.FNWJJH) pInfo;
            fnwjjh.PlayType = this.Cbb_PlayType.Text;
            fnwjjh.PlayName = this.Cbb_PlayName.Text;
            fnwjjh.Unit = this.GetUnit();
            fnwjjh.RXZJ = this.Txt_RXZJ.Text;
            fnwjjh.RXWZList = this.GetRXWZList();
            if (CommFunc.CheckPlayIsRXDS(this.Play) && (fnwjjh.RXWZList.Count < this.PlayInfo.IndexList.Count))
            {
                pError = $"【{this.Play}】至少要选择{this.PlayInfo.IndexList.Count}个位置！";
            }
            fnwjjh.Mode = this.GetMode();
            fnwjjh.ModeExpect = Convert.ToInt32(this.Nm_ModeExpect.Value);
            fnwjjh.FBInfo = this.GetFBType();
            fnwjjh.IsBetsZJ = this.Rdb_CodeType1.Checked;
            fnwjjh.BTType = this.GetTimesType();
            fnwjjh.BTPlanList = CommFunc.SplitString(this.Txt_BTPlan.Text.Replace("，", ","), ",", -1);
            if (this.Rdb_BTPlan.Checked && (fnwjjh.BTPlanList.Count == 0))
            {
                pError = "至少要输入一个直线倍投！";
                this.Txt_BTPlan.Focus();
                return null;
            }
            foreach (string str in fnwjjh.BTPlanList)
            {
                if (!CommFunc.CheckBetsTimes(str, ref pError))
                {
                    this.Txt_BTPlan.SelectAll();
                    this.Txt_BTPlan.Focus();
                    return null;
                }
            }
            fnwjjh.BTFNName = this.Cbb_BTFN.Text;
            fnwjjh.VisibleMore = this.IsMore;
            fnwjjh.HWND = CommFunc.ConvertIntPtr(this.Txt_HWNDValue.Text);
            fnwjjh.HWNDSeparate = this.Txt_CSSeparate.Text;
            fnwjjh.HWNDPlay = this.Txt_CSPlayName.Text;
            fnwjjh.HWNDValueLeft = this.Txt_CSValueLeft.Text;
            fnwjjh.HWNDValueRight = this.Txt_CSValueRight.Text;
            fnwjjh.ZSBetsSelect1 = this.Ckb_MN1.Checked;
            fnwjjh.ZSBetsSelect2 = this.Ckb_MN3.Checked;
            fnwjjh.MNBetsSelect1 = this.Ckb_MN2.Checked;
            fnwjjh.MNBetsSelect2 = this.Ckb_MN4.Checked;
            fnwjjh.ZSBetsMoney1 = this.Txt_MN1.Text;
            fnwjjh.ZSBetsMoney2 = this.Txt_MN3.Text;
            fnwjjh.MNBetsMoney1 = this.Txt_MN2.Text;
            fnwjjh.MNBetsMoney2 = this.Txt_MN4.Text;
            pInfo.YLHTSelect = this.Ckb_YLHT.Checked;
            pInfo.KSHTSelect = this.Ckb_KSHT.Checked;
            pInfo.YLHTMoney = this.Txt_YLHT.Text;
            pInfo.KSHTMoney = this.Txt_KSHT.Text;
            pInfo.YLHTID = Convert.ToInt32(this.Nm_YLHT.Value);
            pInfo.KSHTID = Convert.ToInt32(this.Nm_KSHT.Value);
            fnwjjh.YLStopSelect = this.Ckb_YLStopBets.Checked;
            fnwjjh.KSStopSelect = this.Ckb_KSStopBets.Checked;
            fnwjjh.YLStopMoney = this.Txt_YLStopBets.Text;
            fnwjjh.KSStopMoney = this.Txt_KSStopBets.Text;
            return fnwjjh;
        }

        private ConfigurationStatus.FBType GetFBType() => 
            ((ConfigurationStatus.FBType) this.Cbb_FBType.SelectedIndex);

        public static string GetHtmlDocument(IntPtr hwnd)
        {
            string str = "";
            object ppvObject = new object();
            Guid riid = new Guid();
            int wMsg = CommFunc.RegisterWindowMessage("WM_Html_GETOBJECT");
            int lParam = 0;
            int lResult = CommFunc.SendMessage(hwnd, wMsg, 0, ref lParam);
            StringBuilder builder = new StringBuilder();
            int num4 = CommFunc.ObjectFromLresult(lResult, ref riid, 0, ref ppvObject);
            mshtml.IHTMLDocument2 document = (mshtml.IHTMLDocument2) ppvObject;
            if (document != null)
            {
                str = document.body.innerHTML.Replace("<", " ").Replace(">", " ");
            }
            return str;
        }

        private string GetHWDNValue(bool pHint = true)
        {
            string str = CountHwndValue(this.Txt_HWNDValue.Text);
            if ((str == "") && pHint)
            {
                CommFunc.PublicMessageAll("当前计划句柄获取到的计划内容为空！", true, MessageBoxIcon.Asterisk, "");
            }
            return str;
        }

        private ConfigurationStatus.SchemeMode GetMode() => 
            ((ConfigurationStatus.SchemeMode) this.Cbb_Mode.SelectedIndex);

        public static void GetPlanValue(ConfigurationStatus.WJJHInfo pInfo)
        {
            try
            {
                pInfo.PlanValue = "";
                string pValue = CountHwndValue(pInfo.HWNDString);
                if (pValue != "")
                {
                    if (((pInfo.CSSeparate == "") && (pInfo.CSPlay != "")) || ((pInfo.CSSeparate != "") && (pInfo.CSPlay == "")))
                    {
                        pInfo.Error = "【玩法分割符】和【玩法名称】必须同时输入一个值！";
                    }
                    if (((pInfo.CSValueLeft == "") && (pInfo.CSValueRight != "")) || ((pInfo.CSValueLeft != "") && (pInfo.CSValueRight == "")))
                    {
                        pInfo.Error = "【计划内容左】和【计划内容右】必须同时输入一个值！";
                    }
                    if (pInfo.Error == "")
                    {
                        List<string> list2;
                        int num2;
                        string str8;
                        string pStr = "";
                        if ((pInfo.CSSeparate != "") && (pInfo.CSPlay != ""))
                        {
                            string separate = GetSeparate(pValue);
                            if (!separate.Contains(pInfo.CSSeparate))
                            {
                                pInfo.Error = "【玩法分割符】输入错误！";
                                return;
                            }
                            Dictionary<string, int> playList = GetPlayList(pValue, separate);
                            List<string> list = CommFunc.SplitString(pValue, separate, -1);
                            if (playList.ContainsKey(pInfo.CSPlay))
                            {
                                int num = playList[pInfo.CSPlay];
                                pStr = list[num];
                            }
                            else
                            {
                                foreach (string str4 in list)
                                {
                                    if (str4.Contains(pInfo.CSPlay))
                                    {
                                        pStr = str4;
                                        break;
                                    }
                                }
                            }
                            if (pStr == "")
                            {
                                pInfo.Error = "【玩法名称】输入错误！";
                                return;
                            }
                        }
                        else
                        {
                            pStr = pValue;
                        }
                        string str5 = "";
                        string play = pInfo.PlayInfo.Play;
                        string pSkip = "";
                        if (play.Contains("定位胆"))
                        {
                            pSkip = "大小单双质合";
                        }
                        if (play.Contains("龙虎"))
                        {
                            pSkip = "龙虎和";
                        }
                        if (((pInfo.CSValueLeft != "") && (pInfo.CSValueRight != "")) && !CommFunc.CheckPlayIsDS(pInfo.Play))
                        {
                            list2 = CommFunc.SplitString(pStr, "\r\n", -1);
                            List<ConfigurationStatus.WJJHExpect> list3 = new List<ConfigurationStatus.WJJHExpect>();
                            for (num2 = 0; num2 < list2.Count; num2++)
                            {
                                str8 = list2[num2];
                                if ((str8 == "") && (list3.Count > 1))
                                {
                                    break;
                                }
                                if (str8.Contains(pInfo.CSValueLeft) && str8.Contains(pInfo.CSValueRight))
                                {
                                    ConfigurationStatus.WJJHExpect item = new ConfigurationStatus.WJJHExpect();
                                    int startIndex = str8.LastIndexOf(pInfo.CSValueLeft) + pInfo.CSValueLeft.Length;
                                    int index = str8.IndexOf(pInfo.CSValueRight, startIndex);
                                    item.Value = str8.Substring(startIndex, index - startIndex);
                                    List<string> list4 = CommFunc.SplitStringSkipNull(CommFunc.ConvertNumberString(str8.Substring(0, str8.IndexOf(pInfo.CSValueLeft)), " ", ""), " ");
                                    if (list4.Count != 0)
                                    {
                                        if (list4[0].Length >= 2)
                                        {
                                            item.Expect1 = list4[0];
                                            if ((list4.Count > 1) && (list4[1].Length == list4[0].Length))
                                            {
                                                item.Expect2 = list4[1];
                                            }
                                        }
                                        list3.Add(item);
                                    }
                                }
                            }
                            if (list3.Count == 0)
                            {
                                return;
                            }
                            ConfigurationStatus.WJJHExpect expect2 = list3[list3.Count - 1];
                            str5 = CommFunc.ConvertBetsCode(expect2.Value, pInfo.Play, pSkip);
                        }
                        if (str5 == "")
                        {
                            List<string> pList = new List<string>();
                            list2 = CommFunc.SplitStringSkipNull(pStr, "\r\n");
                            if (list2.Count == 0)
                            {
                                return;
                            }
                            string str10 = (pInfo.CSPlayName != "") ? pInfo.CSPlayName : "期";
                            if (!list2[0].Contains(str10))
                            {
                                for (num2 = 0; num2 < list2.Count; num2++)
                                {
                                    str8 = list2[num2];
                                    if (str8.Contains(str10))
                                    {
                                        break;
                                    }
                                    pList.Add(str8);
                                }
                            }
                            else
                            {
                                num2 = 0;
                                while (num2 < list2.Count)
                                {
                                    str8 = list2[num2];
                                    if (!str8.Contains(str10))
                                    {
                                        break;
                                    }
                                    num2++;
                                }
                                for (int j = num2; j < list2.Count; j++)
                                {
                                    str8 = list2[j];
                                    if (str8.Contains("期") || str8.Contains("时间"))
                                    {
                                        break;
                                    }
                                    pList.Add(str8);
                                }
                            }
                            List<string> list6 = new List<string>();
                            for (int i = 0; i < pList.Count; i++)
                            {
                                str8 = pList[i];
                                string str11 = CommFunc.GetIndexString(str8, "【", "】", 0);
                                if (str11 != "")
                                {
                                    str8 = str11;
                                }
                                list6.Add(str8);
                            }
                            pList = list6;
                            if (pList.Count == 0)
                            {
                                return;
                            }
                            if (!CommFunc.CheckPlayIsDS(pInfo.Play))
                            {
                                pList = new List<string> {
                                    pList[0]
                                };
                            }
                            string pChar = " ";
                            if ((AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5) || (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10))
                            {
                                pChar = ",";
                            }
                            str5 = CommFunc.Join(pList, pChar);
                            if (!CommFunc.CheckPlayIsDS(pInfo.Play))
                            {
                                str5 = CommFunc.ConvertBetsCode(str5, pInfo.Play, "");
                            }
                            string pErrorHint = "";
                            str5 = CommFunc.CombinaBetsCode(CommFunc.FilterNumber(str5, pInfo.PlayInfo.CodeCount, pInfo.Play, ref pErrorHint), pInfo.Play);
                        }
                        if (play.Contains("定位胆"))
                        {
                            List<string> list8 = new List<string>();
                            for (num2 = AppInfo.Current.Lottery.Min; num2 <= AppInfo.Current.Lottery.Max; num2++)
                            {
                                string pCode = num2.ToString();
                                int pView = 4;
                                if ((AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5) || (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10))
                                {
                                    pCode = CommFunc.Convert11X5Code(pCode);
                                    pView = 5;
                                }
                                if (str5.Contains("大") || str5.Contains("小"))
                                {
                                    if (CommFunc.CountDX(num2.ToString(), pView) == str5)
                                    {
                                        list8.Add(pCode);
                                    }
                                }
                                else if (str5.Contains("单") || str5.Contains("双"))
                                {
                                    if (CommFunc.CountDS(num2.ToString()) == str5)
                                    {
                                        list8.Add(pCode);
                                    }
                                }
                                else if ((str5.Contains("质") || str5.Contains("合")) && (CommFunc.CountZH(num2.ToString()) == str5))
                                {
                                    list8.Add(pCode);
                                }
                            }
                            if (list8.Count > 0)
                            {
                                str5 = CommFunc.Join(list8, pInfo.PlayInfo.PlayChar);
                            }
                        }
                        pInfo.PlanValue = CommFunc.ReplaceText(str5, pInfo.PlayInfo, true);
                    }
                }
            }
            catch
            {
            }
        }

        private static Dictionary<string, int> GetPlayList(string pValue, string pSeparate)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            List<string> list = CommFunc.SplitString(pValue, pSeparate, -1);
            for (int i = 0; i < list.Count; i++)
            {
                string pStr = list[i];
                if (!pStr.Contains("开奖"))
                {
                    List<string> list2 = CommFunc.SplitString(pStr, "\r\n", -1);
                    string str2 = "";
                    foreach (string str3 in list2)
                    {
                        if ((str3.Contains("期") || str3.Contains("【")) || str3.Contains("】"))
                        {
                            List<string> pSikpList = new List<string> { 
                                "期",
                                "码",
                                "注"
                            };
                            List<string> list4 = CommFunc.SplitStringSkipNull(CommFunc.ConvertZWString(str3, " ", pSikpList), " ");
                            if (list4.Count == 3)
                            {
                                str2 = list4[0];
                            }
                            else if (list4.Count > 3)
                            {
                                if (list4[0].Contains("龙虎"))
                                {
                                    str2 = list4[0];
                                }
                                else
                                {
                                    str2 = list4[1];
                                }
                            }
                            else if (list4.Count == 2)
                            {
                                str2 = list4[1];
                            }
                            else if (list4.Count == 1)
                            {
                                str2 = list4[0];
                            }
                            if (str2 != "")
                            {
                                break;
                            }
                        }
                    }
                    if (str2 != "")
                    {
                        str2 = $"{i + 1}.{str2}";
                        dictionary[str2] = i;
                    }
                }
            }
            return dictionary;
        }

        private List<int> GetRXWZList()
        {
            if (!CommFunc.CheckPlayIsRXDS(this.Play))
            {
                return null;
            }
            List<int> list = new List<int>();
            for (int i = 0; i < this.RXWZList.Count; i++)
            {
                if (this.RXWZList[i].Checked)
                {
                    list.Add(AppInfo.FiveDic[this.RXWZList[i].Text]);
                }
            }
            return list;
        }

        private static string GetSeparate(string pValue)
        {
            List<string> list = CommFunc.SplitString(pValue, "\r\n", -1);
            foreach (string str2 in list)
            {
                if (str2 != "")
                {
                    Dictionary<char, int> dictionary = new Dictionary<char, int>();
                    foreach (char ch in str2)
                    {
                        if (!dictionary.ContainsKey(ch))
                        {
                            dictionary[ch] = 1;
                        }
                        else
                        {
                            Dictionary<char, int> dictionary2;
                            char ch2;
                            (dictionary2 = dictionary)[ch2 = ch] = dictionary2[ch2] + 1;
                        }
                    }
                    if (dictionary.Count == 1)
                    {
                        return str2;
                    }
                }
            }
            return "";
        }

        private ConfigurationStatus.SCTimesType GetTimesType()
        {
            if (this.Rdb_BTPlan.Checked)
            {
                return ConfigurationStatus.SCTimesType.Plan;
            }
            return ConfigurationStatus.SCTimesType.FN;
        }

        private ConfigurationStatus.SCUnitType GetUnit() => 
            ((ConfigurationStatus.SCUnitType) this.Cbb_Unit.SelectedIndex);

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FNWJJHLine));
            this.Lbl_Name = new Label();
            this.Txt_BTPlan = new TextBox();
            this.Lbl_FBType = new Label();
            this.Lbl_Mode = new Label();
            this.Cbb_Mode = new ComboBox();
            this.Cbb_FBType = new ComboBox();
            this.Lbl_MNBets = new Label();
            this.Txt_MN4 = new TextBox();
            this.Ckb_MN4 = new CheckBox();
            this.Txt_MN3 = new TextBox();
            this.Ckb_MN3 = new CheckBox();
            this.Txt_MN2 = new TextBox();
            this.Ckb_MN2 = new CheckBox();
            this.Txt_MN1 = new TextBox();
            this.Ckb_MN1 = new CheckBox();
            this.Txt_YLStopBets = new TextBox();
            this.Lbl_StopBets = new Label();
            this.Ckb_YLStopBets = new CheckBox();
            this.Ckb_KSStopBets = new CheckBox();
            this.Txt_KSStopBets = new TextBox();
            this.Rdb_CodeType2 = new RadioButton();
            this.Rdb_CodeType1 = new RadioButton();
            this.Lbl_CodeType = new Label();
            this.Rdb_BTPlan = new RadioButton();
            this.Pnl_CodeType = new Panel();
            this.Pnl_Times = new Panel();
            this.Ckb_BTCount = new CheckBox();
            this.Ckb_BTFN = new CheckBox();
            this.Cbb_BTFN = new ComboBox();
            this.Rdb_BTFN = new RadioButton();
            this.Cbb_PlayName = new ComboBox();
            this.Cbb_PlayType = new ComboBox();
            this.Lbl_PlayType = new Label();
            this.Lbl_PlayName = new Label();
            this.Cbb_Unit = new ComboBox();
            this.Lbl_Unit = new Label();
            this.Pnl_More = new Panel();
            this.Nm_KSHT = new NumericUpDown();
            this.Lbl_KSHT = new Label();
            this.Txt_KSHT = new TextBox();
            this.Ckb_KSHT = new CheckBox();
            this.Nm_YLHT = new NumericUpDown();
            this.Lbl_YLHT = new Label();
            this.Lbl_YKHT = new Label();
            this.Txt_YLHT = new TextBox();
            this.Ckb_YLHT = new CheckBox();
            this.BT_Time = new BetsTimeLine();
            this.Lbl_ModeExpect = new Label();
            this.Nm_ModeExpect = new NumericUpDown();
            this.Lbl_MoreHint = new Label();
            this.Pnl_MainInfo = new Panel();
            this.Tab_Main = new TabControl();
            this.Tap_PlanCS = new TabPage();
            this.Pnl_PlanCS = new Panel();
            this.Cbb_CSFXPlay = new ComboBox();
            this.Txt_CSValueRight = new TextBox();
            this.Lbl_CSValueRight = new Label();
            this.Txt_CSValueLeft = new TextBox();
            this.Lbl_CSValueLeft = new Label();
            this.Txt_CSPlayName = new TextBox();
            this.Lbl_CSPlayName = new Label();
            this.Txt_CSSeparate = new TextBox();
            this.Lbl_CSSeparate = new Label();
            this.Tap_ViewPlan = new TabPage();
            this.Pnl_ViewPlan = new Panel();
            this.Txt_ViewPlan = new RichTextBox();
            this.Lbl_NumberCount = new Label();
            this.Lbl_ViewPlan = new Label();
            this.Pic_HWND = new PictureBox();
            this.Lbl_HWNDHint = new Label();
            this.Txt_HWNDValue = new TextBox();
            this.Lbl_HWNDKey = new Label();
            this.Err_Hint = new ErrorProvider(this.components);
            this.Pnl_RX = new Panel();
            this.Ckb_WZ5 = new CheckBox();
            this.Ckb_WZ4 = new CheckBox();
            this.Ckb_WZ3 = new CheckBox();
            this.Ckb_WZ2 = new CheckBox();
            this.Lbl_RXZJ = new Label();
            this.Txt_RXZJ = new TextBox();
            this.Ckb_WZ1 = new CheckBox();
            this.Ckb_More = new CheckBox();
            this.Ckb_RefreshPlan = new CheckBox();
            this.Ckb_CSFXSeparate = new CheckBox();
            this.Ckb_CSFXPlay = new CheckBox();
            this.Pnl_CodeType.SuspendLayout();
            this.Pnl_Times.SuspendLayout();
            this.Pnl_More.SuspendLayout();
            this.Nm_KSHT.BeginInit();
            this.Nm_YLHT.BeginInit();
            this.Nm_ModeExpect.BeginInit();
            this.Pnl_MainInfo.SuspendLayout();
            this.Tab_Main.SuspendLayout();
            this.Tap_PlanCS.SuspendLayout();
            this.Pnl_PlanCS.SuspendLayout();
            this.Tap_ViewPlan.SuspendLayout();
            this.Pnl_ViewPlan.SuspendLayout();
            ((ISupportInitialize) this.Pic_HWND).BeginInit();
            ((ISupportInitialize) this.Err_Hint).BeginInit();
            this.Pnl_RX.SuspendLayout();
            base.SuspendLayout();
            this.Lbl_Name.AutoSize = true;
            this.Lbl_Name.Location = new Point(3, 7);
            this.Lbl_Name.Name = "Lbl_Name";
            this.Lbl_Name.Size = new Size(0x38, 0x11);
            this.Lbl_Name.TabIndex = 0;
            this.Lbl_Name.Text = "外接计划";
            this.Txt_BTPlan.Location = new Point(0x5d, 5);
            this.Txt_BTPlan.Multiline = true;
            this.Txt_BTPlan.Name = "Txt_BTPlan";
            this.Txt_BTPlan.ScrollBars = ScrollBars.Vertical;
            this.Txt_BTPlan.Size = new Size(0xe9, 0x4b);
            this.Txt_BTPlan.TabIndex = 0x1a;
            this.Txt_BTPlan.Text = "1,2,3";
            this.Lbl_FBType.AutoSize = true;
            this.Lbl_FBType.Location = new Point(3, 0x2b);
            this.Lbl_FBType.Name = "Lbl_FBType";
            this.Lbl_FBType.Size = new Size(0x44, 0x11);
            this.Lbl_FBType.TabIndex = 30;
            this.Lbl_FBType.Text = "翻倍方式：";
            this.Lbl_Mode.AutoSize = true;
            this.Lbl_Mode.Location = new Point(3, 8);
            this.Lbl_Mode.Name = "Lbl_Mode";
            this.Lbl_Mode.Size = new Size(0x44, 0x11);
            this.Lbl_Mode.TabIndex = 0xcc;
            this.Lbl_Mode.Text = "换号规则：";
            this.Cbb_Mode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_Mode.FormattingEnabled = true;
            this.Cbb_Mode.Items.AddRange(new object[] { "每期换号", "挂后换号", "中后换号", "连挂N期换号", "连中N期换号", "累计挂N期换号", "累计中N期换号", "从不换号", "期期滚" });
            this.Cbb_Mode.Location = new Point(0x4d, 5);
            this.Cbb_Mode.Name = "Cbb_Mode";
            this.Cbb_Mode.Size = new Size(110, 0x19);
            this.Cbb_Mode.TabIndex = 0xf6;
            this.Cbb_Mode.SelectedIndexChanged += new EventHandler(this.Cbb_Mode_SelectedIndexChanged);
            this.Cbb_FBType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_FBType.FormattingEnabled = true;
            this.Cbb_FBType.Items.AddRange(new object[] { "挂翻倍", "中翻倍" });
            this.Cbb_FBType.Location = new Point(0x4d, 40);
            this.Cbb_FBType.Name = "Cbb_FBType";
            this.Cbb_FBType.Size = new Size(110, 0x19);
            this.Cbb_FBType.TabIndex = 0xf7;
            this.Lbl_MNBets.AutoSize = true;
            this.Lbl_MNBets.Location = new Point(0xa1, 0xc3);
            this.Lbl_MNBets.Name = "Lbl_MNBets";
            this.Lbl_MNBets.Size = new Size(0x38, 0x11);
            this.Lbl_MNBets.TabIndex = 0x101;
            this.Lbl_MNBets.Text = "循环切换";
            this.Txt_MN4.Enabled = false;
            this.Txt_MN4.Location = new Point(0x139, 0xcb);
            this.Txt_MN4.Name = "Txt_MN4";
            this.Txt_MN4.Size = new Size(60, 0x17);
            this.Txt_MN4.TabIndex = 0x100;
            this.Txt_MN4.Text = "50000";
            this.Ckb_MN4.AutoSize = true;
            this.Ckb_MN4.Location = new Point(0xde, 0xcd);
            this.Ckb_MN4.Name = "Ckb_MN4";
            this.Ckb_MN4.Size = new Size(0x57, 0x15);
            this.Ckb_MN4.TabIndex = 0xff;
            this.Ckb_MN4.Text = "真实投注输";
            this.Ckb_MN4.UseVisualStyleBackColor = true;
            this.Ckb_MN4.CheckedChanged += new EventHandler(this.Ckb_MN4_CheckedChanged);
            this.Txt_MN3.Enabled = false;
            this.Txt_MN3.Location = new Point(0x61, 0xcb);
            this.Txt_MN3.Name = "Txt_MN3";
            this.Txt_MN3.Size = new Size(60, 0x17);
            this.Txt_MN3.TabIndex = 0xfe;
            this.Txt_MN3.Text = "50000";
            this.Ckb_MN3.AutoSize = true;
            this.Ckb_MN3.Location = new Point(6, 0xcd);
            this.Ckb_MN3.Name = "Ckb_MN3";
            this.Ckb_MN3.Size = new Size(0x57, 0x15);
            this.Ckb_MN3.TabIndex = 0xfd;
            this.Ckb_MN3.Text = "模拟投注赢";
            this.Ckb_MN3.UseVisualStyleBackColor = true;
            this.Ckb_MN3.CheckedChanged += new EventHandler(this.Ckb_MN3_CheckedChanged);
            this.Txt_MN2.Enabled = false;
            this.Txt_MN2.Location = new Point(0x139, 0xb2);
            this.Txt_MN2.Name = "Txt_MN2";
            this.Txt_MN2.Size = new Size(60, 0x17);
            this.Txt_MN2.TabIndex = 0xfc;
            this.Txt_MN2.Text = "50000";
            this.Ckb_MN2.AutoSize = true;
            this.Ckb_MN2.Location = new Point(0xde, 180);
            this.Ckb_MN2.Name = "Ckb_MN2";
            this.Ckb_MN2.Size = new Size(0x57, 0x15);
            this.Ckb_MN2.TabIndex = 0xfb;
            this.Ckb_MN2.Text = "真实投注赢";
            this.Ckb_MN2.UseVisualStyleBackColor = true;
            this.Ckb_MN2.CheckedChanged += new EventHandler(this.Ckb_MN2_CheckedChanged);
            this.Txt_MN1.Enabled = false;
            this.Txt_MN1.Location = new Point(0x61, 0xb2);
            this.Txt_MN1.Name = "Txt_MN1";
            this.Txt_MN1.Size = new Size(60, 0x17);
            this.Txt_MN1.TabIndex = 250;
            this.Txt_MN1.Text = "50000";
            this.Ckb_MN1.AutoSize = true;
            this.Ckb_MN1.Location = new Point(6, 180);
            this.Ckb_MN1.Name = "Ckb_MN1";
            this.Ckb_MN1.Size = new Size(0x57, 0x15);
            this.Ckb_MN1.TabIndex = 0xf9;
            this.Ckb_MN1.Text = "模拟投注输";
            this.Ckb_MN1.UseVisualStyleBackColor = true;
            this.Ckb_MN1.CheckedChanged += new EventHandler(this.Ckb_MN1_CheckedChanged);
            this.Txt_YLStopBets.Enabled = false;
            this.Txt_YLStopBets.Location = new Point(0x9b, 0x92);
            this.Txt_YLStopBets.Name = "Txt_YLStopBets";
            this.Txt_YLStopBets.Size = new Size(60, 0x17);
            this.Txt_YLStopBets.TabIndex = 0x11f;
            this.Txt_YLStopBets.Text = "50000";
            this.Lbl_StopBets.AutoSize = true;
            this.Lbl_StopBets.Location = new Point(3, 0x94);
            this.Lbl_StopBets.Name = "Lbl_StopBets";
            this.Lbl_StopBets.Size = new Size(0x44, 0x11);
            this.Lbl_StopBets.TabIndex = 0x11c;
            this.Lbl_StopBets.Text = "止损盈亏：";
            this.Ckb_YLStopBets.AutoSize = true;
            this.Ckb_YLStopBets.Location = new Point(0x4b, 0x93);
            this.Ckb_YLStopBets.Name = "Ckb_YLStopBets";
            this.Ckb_YLStopBets.Size = new Size(0x4b, 0x15);
            this.Ckb_YLStopBets.TabIndex = 0x125;
            this.Ckb_YLStopBets.Text = "盈利大于";
            this.Ckb_YLStopBets.UseVisualStyleBackColor = true;
            this.Ckb_YLStopBets.CheckedChanged += new EventHandler(this.Ckb_YLStopBets_CheckedChanged);
            this.Ckb_KSStopBets.AutoSize = true;
            this.Ckb_KSStopBets.Location = new Point(0xe2, 0x93);
            this.Ckb_KSStopBets.Name = "Ckb_KSStopBets";
            this.Ckb_KSStopBets.Size = new Size(0x4b, 0x15);
            this.Ckb_KSStopBets.TabIndex = 0x127;
            this.Ckb_KSStopBets.Text = "亏损大于";
            this.Ckb_KSStopBets.UseVisualStyleBackColor = true;
            this.Ckb_KSStopBets.CheckedChanged += new EventHandler(this.Ckb_KSStopBets_CheckedChanged);
            this.Txt_KSStopBets.Enabled = false;
            this.Txt_KSStopBets.Location = new Point(0x132, 0x92);
            this.Txt_KSStopBets.Name = "Txt_KSStopBets";
            this.Txt_KSStopBets.Size = new Size(60, 0x17);
            this.Txt_KSStopBets.TabIndex = 0x126;
            this.Txt_KSStopBets.Text = "50000";
            this.Rdb_CodeType2.AutoSize = true;
            this.Rdb_CodeType2.Location = new Point(0x3f, 2);
            this.Rdb_CodeType2.Name = "Rdb_CodeType2";
            this.Rdb_CodeType2.Size = new Size(50, 0x15);
            this.Rdb_CodeType2.TabIndex = 0x129;
            this.Rdb_CodeType2.Text = "反集";
            this.Rdb_CodeType2.UseVisualStyleBackColor = true;
            this.Rdb_CodeType1.AutoSize = true;
            this.Rdb_CodeType1.Checked = true;
            this.Rdb_CodeType1.Location = new Point(3, 2);
            this.Rdb_CodeType1.Name = "Rdb_CodeType1";
            this.Rdb_CodeType1.Size = new Size(50, 0x15);
            this.Rdb_CodeType1.TabIndex = 0x128;
            this.Rdb_CodeType1.TabStop = true;
            this.Rdb_CodeType1.Text = "正集";
            this.Rdb_CodeType1.UseVisualStyleBackColor = true;
            this.Lbl_CodeType.AutoSize = true;
            this.Lbl_CodeType.Location = new Point(3, 0x4e);
            this.Lbl_CodeType.Name = "Lbl_CodeType";
            this.Lbl_CodeType.Size = new Size(0x44, 0x11);
            this.Lbl_CodeType.TabIndex = 0x12a;
            this.Lbl_CodeType.Text = "投注号码：";
            this.Rdb_BTPlan.AutoSize = true;
            this.Rdb_BTPlan.Checked = true;
            this.Rdb_BTPlan.Location = new Point(3, 6);
            this.Rdb_BTPlan.Name = "Rdb_BTPlan";
            this.Rdb_BTPlan.Size = new Size(0x56, 0x15);
            this.Rdb_BTPlan.TabIndex = 0x12b;
            this.Rdb_BTPlan.TabStop = true;
            this.Rdb_BTPlan.Text = "直线倍投：";
            this.Rdb_BTPlan.UseVisualStyleBackColor = true;
            this.Rdb_BTPlan.CheckedChanged += new EventHandler(this.Rdb_BTPlan_CheckedChanged);
            this.Pnl_CodeType.Controls.Add(this.Rdb_CodeType1);
            this.Pnl_CodeType.Controls.Add(this.Rdb_CodeType2);
            this.Pnl_CodeType.Location = new Point(0x48, 0x4a);
            this.Pnl_CodeType.Name = "Pnl_CodeType";
            this.Pnl_CodeType.Size = new Size(0x7b, 0x1b);
            this.Pnl_CodeType.TabIndex = 300;
            this.Pnl_Times.Controls.Add(this.Ckb_BTCount);
            this.Pnl_Times.Controls.Add(this.Ckb_BTFN);
            this.Pnl_Times.Controls.Add(this.Cbb_BTFN);
            this.Pnl_Times.Controls.Add(this.Rdb_BTFN);
            this.Pnl_Times.Controls.Add(this.Rdb_BTPlan);
            this.Pnl_Times.Controls.Add(this.Txt_BTPlan);
            this.Pnl_Times.Location = new Point(0, 0x126);
            this.Pnl_Times.Name = "Pnl_Times";
            this.Pnl_Times.Size = new Size(0x20d, 0x55);
            this.Pnl_Times.TabIndex = 0x12d;
            this.Ckb_BTCount.Appearance = Appearance.Button;
            this.Ckb_BTCount.AutoCheck = false;
            this.Ckb_BTCount.FlatAppearance.BorderSize = 0;
            this.Ckb_BTCount.FlatStyle = FlatStyle.Flat;
            this.Ckb_BTCount.Image = Resources.MathCount;
            this.Ckb_BTCount.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_BTCount.Location = new Point(3, 0x21);
            this.Ckb_BTCount.Name = "Ckb_BTCount";
            this.Ckb_BTCount.Size = new Size(80, 0x19);
            this.Ckb_BTCount.TabIndex = 0x159;
            this.Ckb_BTCount.Text = "计算倍投";
            this.Ckb_BTCount.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_BTCount.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_BTCount.UseVisualStyleBackColor = true;
            this.Ckb_BTCount.Click += new EventHandler(this.Ckb_BTCount_Click);
            this.Ckb_BTFN.Appearance = Appearance.Button;
            this.Ckb_BTFN.AutoCheck = false;
            this.Ckb_BTFN.FlatAppearance.BorderSize = 0;
            this.Ckb_BTFN.FlatStyle = FlatStyle.Flat;
            this.Ckb_BTFN.Image = Resources.PlanEdit;
            this.Ckb_BTFN.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_BTFN.Location = new Point(0x14c, 0x21);
            this.Ckb_BTFN.Name = "Ckb_BTFN";
            this.Ckb_BTFN.Size = new Size(80, 0x19);
            this.Ckb_BTFN.TabIndex = 0x158;
            this.Ckb_BTFN.Text = "方案设置";
            this.Ckb_BTFN.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_BTFN.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_BTFN.UseVisualStyleBackColor = true;
            this.Ckb_BTFN.Click += new EventHandler(this.Ckb_BTFN_Click);
            this.Cbb_BTFN.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_BTFN.Enabled = false;
            this.Cbb_BTFN.FormattingEnabled = true;
            this.Cbb_BTFN.Location = new Point(0x1a7, 5);
            this.Cbb_BTFN.Name = "Cbb_BTFN";
            this.Cbb_BTFN.Size = new Size(0x5f, 0x19);
            this.Cbb_BTFN.TabIndex = 0x12d;
            this.Rdb_BTFN.AutoSize = true;
            this.Rdb_BTFN.Location = new Point(0x14c, 6);
            this.Rdb_BTFN.Name = "Rdb_BTFN";
            this.Rdb_BTFN.Size = new Size(0x56, 0x15);
            this.Rdb_BTFN.TabIndex = 300;
            this.Rdb_BTFN.Text = "高级倍投：";
            this.Rdb_BTFN.UseVisualStyleBackColor = true;
            this.Rdb_BTFN.CheckedChanged += new EventHandler(this.Rdb_BTPlan_CheckedChanged);
            this.Cbb_PlayName.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_PlayName.FormattingEnabled = true;
            this.Cbb_PlayName.Location = new Point(0x196, 0x3e);
            this.Cbb_PlayName.Name = "Cbb_PlayName";
            this.Cbb_PlayName.Size = new Size(110, 0x19);
            this.Cbb_PlayName.TabIndex = 0x131;
            this.Cbb_PlayName.SelectedIndexChanged += new EventHandler(this.Cbb_PlayName_SelectedIndexChanged);
            this.Cbb_PlayType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_PlayType.FormattingEnabled = true;
            this.Cbb_PlayType.Location = new Point(0x196, 0x1b);
            this.Cbb_PlayType.Name = "Cbb_PlayType";
            this.Cbb_PlayType.Size = new Size(110, 0x19);
            this.Cbb_PlayType.TabIndex = 0x130;
            this.Cbb_PlayType.SelectedIndexChanged += new EventHandler(this.Cbb_PlayType_SelectedIndexChanged);
            this.Lbl_PlayType.AutoSize = true;
            this.Lbl_PlayType.Location = new Point(0x14c, 30);
            this.Lbl_PlayType.Name = "Lbl_PlayType";
            this.Lbl_PlayType.Size = new Size(0x44, 0x11);
            this.Lbl_PlayType.TabIndex = 0x12f;
            this.Lbl_PlayType.Text = "玩法类型：";
            this.Lbl_PlayName.AutoSize = true;
            this.Lbl_PlayName.Location = new Point(0x14c, 0x41);
            this.Lbl_PlayName.Name = "Lbl_PlayName";
            this.Lbl_PlayName.Size = new Size(0x44, 0x11);
            this.Lbl_PlayName.TabIndex = 0x12e;
            this.Lbl_PlayName.Text = "玩法名称：";
            this.Cbb_Unit.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_Unit.FormattingEnabled = true;
            this.Cbb_Unit.Items.AddRange(new object[] { "圆", "角", "分", "厘" });
            this.Cbb_Unit.Location = new Point(0x196, 0x61);
            this.Cbb_Unit.Name = "Cbb_Unit";
            this.Cbb_Unit.Size = new Size(110, 0x19);
            this.Cbb_Unit.TabIndex = 0x133;
            this.Lbl_Unit.AutoSize = true;
            this.Lbl_Unit.Location = new Point(0x14c, 100);
            this.Lbl_Unit.Name = "Lbl_Unit";
            this.Lbl_Unit.Size = new Size(0x44, 0x11);
            this.Lbl_Unit.TabIndex = 0x132;
            this.Lbl_Unit.Text = "金额模式：";
            this.Pnl_More.Controls.Add(this.Nm_KSHT);
            this.Pnl_More.Controls.Add(this.Lbl_KSHT);
            this.Pnl_More.Controls.Add(this.Txt_KSHT);
            this.Pnl_More.Controls.Add(this.Ckb_KSHT);
            this.Pnl_More.Controls.Add(this.Nm_YLHT);
            this.Pnl_More.Controls.Add(this.Lbl_YLHT);
            this.Pnl_More.Controls.Add(this.Lbl_YKHT);
            this.Pnl_More.Controls.Add(this.Txt_YLHT);
            this.Pnl_More.Controls.Add(this.Ckb_YLHT);
            this.Pnl_More.Controls.Add(this.BT_Time);
            this.Pnl_More.Controls.Add(this.Lbl_ModeExpect);
            this.Pnl_More.Controls.Add(this.Nm_ModeExpect);
            this.Pnl_More.Controls.Add(this.Ckb_MN1);
            this.Pnl_More.Controls.Add(this.Txt_MN1);
            this.Pnl_More.Controls.Add(this.Ckb_MN2);
            this.Pnl_More.Controls.Add(this.Txt_MN2);
            this.Pnl_More.Controls.Add(this.Ckb_MN3);
            this.Pnl_More.Controls.Add(this.Txt_MN3);
            this.Pnl_More.Controls.Add(this.Ckb_MN4);
            this.Pnl_More.Controls.Add(this.Txt_MN4);
            this.Pnl_More.Controls.Add(this.Lbl_MNBets);
            this.Pnl_More.Controls.Add(this.Ckb_KSStopBets);
            this.Pnl_More.Controls.Add(this.Lbl_StopBets);
            this.Pnl_More.Controls.Add(this.Pnl_CodeType);
            this.Pnl_More.Controls.Add(this.Txt_KSStopBets);
            this.Pnl_More.Controls.Add(this.Lbl_CodeType);
            this.Pnl_More.Controls.Add(this.Txt_YLStopBets);
            this.Pnl_More.Controls.Add(this.Ckb_YLStopBets);
            this.Pnl_More.Controls.Add(this.Lbl_Mode);
            this.Pnl_More.Controls.Add(this.Cbb_Mode);
            this.Pnl_More.Controls.Add(this.Lbl_FBType);
            this.Pnl_More.Controls.Add(this.Cbb_FBType);
            this.Pnl_More.Location = new Point(0, 0x1a1);
            this.Pnl_More.Name = "Pnl_More";
            this.Pnl_More.Size = new Size(0x20d, 0x109);
            this.Pnl_More.TabIndex = 0x135;
            this.Pnl_More.Visible = false;
            this.Nm_KSHT.Enabled = false;
            this.Nm_KSHT.Location = new Point(0x1c9, 0x71);
            this.Nm_KSHT.Name = "Nm_KSHT";
            this.Nm_KSHT.Size = new Size(50, 0x17);
            this.Nm_KSHT.TabIndex = 0x150;
            int[] bits = new int[4];
            bits[0] = 1;
            this.Nm_KSHT.Value = new decimal(bits);
            this.Lbl_KSHT.AutoSize = true;
            this.Lbl_KSHT.Enabled = false;
            this.Lbl_KSHT.Location = new Point(0x199, 0x73);
            this.Lbl_KSHT.Name = "Lbl_KSHT";
            this.Lbl_KSHT.Size = new Size(0x2c, 0x11);
            this.Lbl_KSHT.TabIndex = 0x14f;
            this.Lbl_KSHT.Text = "跳局数";
            this.Txt_KSHT.Enabled = false;
            this.Txt_KSHT.Location = new Point(0x163, 0x70);
            this.Txt_KSHT.Name = "Txt_KSHT";
            this.Txt_KSHT.Size = new Size(50, 0x17);
            this.Txt_KSHT.TabIndex = 0x14d;
            this.Txt_KSHT.Text = "50000";
            this.Ckb_KSHT.AutoSize = true;
            this.Ckb_KSHT.Location = new Point(300, 0x71);
            this.Ckb_KSHT.Name = "Ckb_KSHT";
            this.Ckb_KSHT.Size = new Size(0x33, 0x15);
            this.Ckb_KSHT.TabIndex = 0x14e;
            this.Ckb_KSHT.Text = "亏损";
            this.Ckb_KSHT.UseVisualStyleBackColor = true;
            this.Ckb_KSHT.CheckedChanged += new EventHandler(this.Ckb_KSHT_CheckedChanged);
            this.Nm_YLHT.Enabled = false;
            this.Nm_YLHT.Location = new Point(0xe8, 0x70);
            this.Nm_YLHT.Name = "Nm_YLHT";
            this.Nm_YLHT.Size = new Size(50, 0x17);
            this.Nm_YLHT.TabIndex = 0x14c;
            bits = new int[4];
            bits[0] = 1;
            this.Nm_YLHT.Value = new decimal(bits);
            this.Lbl_YLHT.AutoSize = true;
            this.Lbl_YLHT.Enabled = false;
            this.Lbl_YLHT.Location = new Point(0xb8, 0x72);
            this.Lbl_YLHT.Name = "Lbl_YLHT";
            this.Lbl_YLHT.Size = new Size(0x2c, 0x11);
            this.Lbl_YLHT.TabIndex = 0x14b;
            this.Lbl_YLHT.Text = "跳局数";
            this.Lbl_YKHT.AutoSize = true;
            this.Lbl_YKHT.Location = new Point(3, 0x71);
            this.Lbl_YKHT.Name = "Lbl_YKHT";
            this.Lbl_YKHT.Size = new Size(0x44, 0x11);
            this.Lbl_YKHT.TabIndex = 0x148;
            this.Lbl_YKHT.Text = "盈亏跳转：";
            this.Txt_YLHT.Enabled = false;
            this.Txt_YLHT.Location = new Point(130, 0x6f);
            this.Txt_YLHT.Name = "Txt_YLHT";
            this.Txt_YLHT.Size = new Size(50, 0x17);
            this.Txt_YLHT.TabIndex = 0x149;
            this.Txt_YLHT.Text = "50000";
            this.Ckb_YLHT.AutoSize = true;
            this.Ckb_YLHT.Location = new Point(0x4b, 0x70);
            this.Ckb_YLHT.Name = "Ckb_YLHT";
            this.Ckb_YLHT.Size = new Size(0x33, 0x15);
            this.Ckb_YLHT.TabIndex = 330;
            this.Ckb_YLHT.Text = "盈利";
            this.Ckb_YLHT.UseVisualStyleBackColor = true;
            this.Ckb_YLHT.CheckedChanged += new EventHandler(this.Ckb_YLHT_CheckedChanged);
            this.BT_Time.Font = new Font("微软雅黑", 9f);
            this.BT_Time.Location = new Point(1, 0xe4);
            this.BT_Time.Margin = new Padding(3, 4, 3, 4);
            this.BT_Time.Name = "BT_Time";
            this.BT_Time.Size = new Size(520, 0x23);
            this.BT_Time.TabIndex = 0x13d;
            this.Lbl_ModeExpect.AutoSize = true;
            this.Lbl_ModeExpect.Location = new Point(0x101, 9);
            this.Lbl_ModeExpect.Name = "Lbl_ModeExpect";
            this.Lbl_ModeExpect.Size = new Size(20, 0x11);
            this.Lbl_ModeExpect.TabIndex = 0x13b;
            this.Lbl_ModeExpect.Text = "期";
            this.Nm_ModeExpect.Location = new Point(0xc1, 6);
            this.Nm_ModeExpect.Name = "Nm_ModeExpect";
            this.Nm_ModeExpect.Size = new Size(60, 0x17);
            this.Nm_ModeExpect.TabIndex = 0x13a;
            bits = new int[4];
            bits[0] = 2;
            this.Nm_ModeExpect.Value = new decimal(bits);
            this.Lbl_MoreHint.AutoSize = true;
            this.Lbl_MoreHint.Font = new Font("微软雅黑", 9f, FontStyle.Bold);
            this.Lbl_MoreHint.Location = new Point(0x58, 0x185);
            this.Lbl_MoreHint.Name = "Lbl_MoreHint";
            this.Lbl_MoreHint.Size = new Size(140, 0x11);
            this.Lbl_MoreHint.TabIndex = 0x13a;
            this.Lbl_MoreHint.Text = "【设置更多参数请点击】\r\n";
            this.Pnl_MainInfo.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_MainInfo.Controls.Add(this.Tab_Main);
            this.Pnl_MainInfo.Controls.Add(this.Pic_HWND);
            this.Pnl_MainInfo.Controls.Add(this.Lbl_HWNDHint);
            this.Pnl_MainInfo.Controls.Add(this.Txt_HWNDValue);
            this.Pnl_MainInfo.Controls.Add(this.Lbl_HWNDKey);
            this.Pnl_MainInfo.Location = new Point(6, 30);
            this.Pnl_MainInfo.Name = "Pnl_MainInfo";
            this.Pnl_MainInfo.Size = new Size(320, 0x102);
            this.Pnl_MainInfo.TabIndex = 0x13b;
            this.Tab_Main.Controls.Add(this.Tap_PlanCS);
            this.Tab_Main.Controls.Add(this.Tap_ViewPlan);
            this.Tab_Main.Dock = DockStyle.Bottom;
            this.Tab_Main.Location = new Point(0, 0x49);
            this.Tab_Main.Name = "Tab_Main";
            this.Tab_Main.SelectedIndex = 0;
            this.Tab_Main.Size = new Size(0x13e, 0xb7);
            this.Tab_Main.TabIndex = 0xb0;
            this.Tap_PlanCS.BackColor = SystemColors.Control;
            this.Tap_PlanCS.Controls.Add(this.Pnl_PlanCS);
            this.Tap_PlanCS.Location = new Point(4, 0x1a);
            this.Tap_PlanCS.Name = "Tap_PlanCS";
            this.Tap_PlanCS.Padding = new Padding(3);
            this.Tap_PlanCS.Size = new Size(310, 0x99);
            this.Tap_PlanCS.TabIndex = 0;
            this.Tap_PlanCS.Text = "参数设置";
            this.Pnl_PlanCS.Controls.Add(this.Ckb_CSFXPlay);
            this.Pnl_PlanCS.Controls.Add(this.Ckb_CSFXSeparate);
            this.Pnl_PlanCS.Controls.Add(this.Cbb_CSFXPlay);
            this.Pnl_PlanCS.Controls.Add(this.Txt_CSValueRight);
            this.Pnl_PlanCS.Controls.Add(this.Lbl_CSValueRight);
            this.Pnl_PlanCS.Controls.Add(this.Txt_CSValueLeft);
            this.Pnl_PlanCS.Controls.Add(this.Lbl_CSValueLeft);
            this.Pnl_PlanCS.Controls.Add(this.Txt_CSPlayName);
            this.Pnl_PlanCS.Controls.Add(this.Lbl_CSPlayName);
            this.Pnl_PlanCS.Controls.Add(this.Txt_CSSeparate);
            this.Pnl_PlanCS.Controls.Add(this.Lbl_CSSeparate);
            this.Pnl_PlanCS.Dock = DockStyle.Fill;
            this.Pnl_PlanCS.Location = new Point(3, 3);
            this.Pnl_PlanCS.Name = "Pnl_PlanCS";
            this.Pnl_PlanCS.Size = new Size(0x130, 0x93);
            this.Pnl_PlanCS.TabIndex = 0;
            this.Cbb_CSFXPlay.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_CSFXPlay.FormattingEnabled = true;
            this.Cbb_CSFXPlay.Items.AddRange(new object[] { "每期换号", "挂后换号", "中后换号", "连挂N期换号", "连中N期换号", "累计挂N期换号", "累计中N期换号", "从不换号", "期期滚" });
            this.Cbb_CSFXPlay.Location = new Point(0xd0, 0x4d);
            this.Cbb_CSFXPlay.Name = "Cbb_CSFXPlay";
            this.Cbb_CSFXPlay.Size = new Size(90, 0x19);
            this.Cbb_CSFXPlay.TabIndex = 0x131;
            this.Cbb_CSFXPlay.Visible = false;
            this.Cbb_CSFXPlay.SelectedIndexChanged += new EventHandler(this.Cbb_CSFXPlay_SelectedIndexChanged);
            this.Txt_CSValueRight.Location = new Point(0x59, 0x71);
            this.Txt_CSValueRight.Name = "Txt_CSValueRight";
            this.Txt_CSValueRight.Size = new Size(100, 0x17);
            this.Txt_CSValueRight.TabIndex = 7;
            this.Lbl_CSValueRight.AutoSize = true;
            this.Lbl_CSValueRight.Location = new Point(5, 0x73);
            this.Lbl_CSValueRight.Name = "Lbl_CSValueRight";
            this.Lbl_CSValueRight.Size = new Size(80, 0x11);
            this.Lbl_CSValueRight.TabIndex = 6;
            this.Lbl_CSValueRight.Text = "计划内容右：";
            this.Txt_CSValueLeft.Location = new Point(0x59, 0x4e);
            this.Txt_CSValueLeft.Name = "Txt_CSValueLeft";
            this.Txt_CSValueLeft.Size = new Size(100, 0x17);
            this.Txt_CSValueLeft.TabIndex = 5;
            this.Lbl_CSValueLeft.AutoSize = true;
            this.Lbl_CSValueLeft.Location = new Point(5, 80);
            this.Lbl_CSValueLeft.Name = "Lbl_CSValueLeft";
            this.Lbl_CSValueLeft.Size = new Size(80, 0x11);
            this.Lbl_CSValueLeft.TabIndex = 4;
            this.Lbl_CSValueLeft.Text = "计划内容左：";
            this.Txt_CSPlayName.Location = new Point(0x59, 0x2b);
            this.Txt_CSPlayName.Name = "Txt_CSPlayName";
            this.Txt_CSPlayName.Size = new Size(100, 0x17);
            this.Txt_CSPlayName.TabIndex = 3;
            this.Txt_CSPlayName.Enter += new EventHandler(this.Txt_CSPlayName_Enter);
            this.Lbl_CSPlayName.AutoSize = true;
            this.Lbl_CSPlayName.Location = new Point(5, 0x2d);
            this.Lbl_CSPlayName.Name = "Lbl_CSPlayName";
            this.Lbl_CSPlayName.Size = new Size(0x44, 0x11);
            this.Lbl_CSPlayName.TabIndex = 2;
            this.Lbl_CSPlayName.Text = "玩法名称：";
            this.Txt_CSSeparate.Location = new Point(0x59, 8);
            this.Txt_CSSeparate.Name = "Txt_CSSeparate";
            this.Txt_CSSeparate.Size = new Size(100, 0x17);
            this.Txt_CSSeparate.TabIndex = 1;
            this.Txt_CSSeparate.Enter += new EventHandler(this.Txt_CSSeparate_Enter);
            this.Lbl_CSSeparate.AutoSize = true;
            this.Lbl_CSSeparate.Location = new Point(5, 10);
            this.Lbl_CSSeparate.Name = "Lbl_CSSeparate";
            this.Lbl_CSSeparate.Size = new Size(80, 0x11);
            this.Lbl_CSSeparate.TabIndex = 0;
            this.Lbl_CSSeparate.Text = "玩法分割符：";
            this.Tap_ViewPlan.BackColor = SystemColors.Control;
            this.Tap_ViewPlan.Controls.Add(this.Pnl_ViewPlan);
            this.Tap_ViewPlan.Location = new Point(4, 0x1a);
            this.Tap_ViewPlan.Name = "Tap_ViewPlan";
            this.Tap_ViewPlan.Padding = new Padding(3);
            this.Tap_ViewPlan.Size = new Size(310, 0x99);
            this.Tap_ViewPlan.TabIndex = 1;
            this.Tap_ViewPlan.Text = "计划预览";
            this.Pnl_ViewPlan.Controls.Add(this.Ckb_RefreshPlan);
            this.Pnl_ViewPlan.Controls.Add(this.Txt_ViewPlan);
            this.Pnl_ViewPlan.Controls.Add(this.Lbl_NumberCount);
            this.Pnl_ViewPlan.Controls.Add(this.Lbl_ViewPlan);
            this.Pnl_ViewPlan.Dock = DockStyle.Fill;
            this.Pnl_ViewPlan.Location = new Point(3, 3);
            this.Pnl_ViewPlan.Name = "Pnl_ViewPlan";
            this.Pnl_ViewPlan.Size = new Size(0x130, 0x93);
            this.Pnl_ViewPlan.TabIndex = 0;
            this.Txt_ViewPlan.Dock = DockStyle.Bottom;
            this.Txt_ViewPlan.Location = new Point(0, 0x23);
            this.Txt_ViewPlan.Name = "Txt_ViewPlan";
            this.Txt_ViewPlan.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.Txt_ViewPlan.Size = new Size(0x130, 0x70);
            this.Txt_ViewPlan.TabIndex = 0x13c;
            this.Txt_ViewPlan.Text = "";
            this.Lbl_NumberCount.AutoSize = true;
            this.Lbl_NumberCount.Location = new Point(0x4b, 8);
            this.Lbl_NumberCount.Name = "Lbl_NumberCount";
            this.Lbl_NumberCount.Size = new Size(0x16, 0x11);
            this.Lbl_NumberCount.TabIndex = 0xaf;
            this.Lbl_NumberCount.Text = "00";
            this.Lbl_ViewPlan.AutoSize = true;
            this.Lbl_ViewPlan.Location = new Point(3, 8);
            this.Lbl_ViewPlan.Name = "Lbl_ViewPlan";
            this.Lbl_ViewPlan.Size = new Size(0x44, 0x11);
            this.Lbl_ViewPlan.TabIndex = 0xad;
            this.Lbl_ViewPlan.Text = "预览计划：";
            this.Pic_HWND.Image = Resources.Drag;
            this.Pic_HWND.Location = new Point(9, 0x21);
            this.Pic_HWND.Name = "Pic_HWND";
            this.Pic_HWND.Size = new Size(0x20, 0x20);
            this.Pic_HWND.TabIndex = 0xa9;
            this.Pic_HWND.TabStop = false;
            this.Pic_HWND.MouseDown += new MouseEventHandler(this.Pic_Hwnd_MouseDown);
            this.Pic_HWND.MouseMove += new MouseEventHandler(this.Pic_Hwnd_MouseMove);
            this.Pic_HWND.MouseUp += new MouseEventHandler(this.Pic_Hwnd_MouseUp);
            this.Lbl_HWNDHint.AutoSize = true;
            this.Lbl_HWNDHint.Location = new Point(0x34, 0x2a);
            this.Lbl_HWNDHint.Name = "Lbl_HWNDHint";
            this.Lbl_HWNDHint.Size = new Size(0xb0, 0x11);
            this.Lbl_HWNDHint.TabIndex = 0xa8;
            this.Lbl_HWNDHint.Text = "拖动图标到计划的区域获取句柄";
            this.Txt_HWNDValue.Location = new Point(80, 5);
            this.Txt_HWNDValue.Name = "Txt_HWNDValue";
            this.Txt_HWNDValue.Size = new Size(0xe5, 0x17);
            this.Txt_HWNDValue.TabIndex = 0xa7;
            this.Lbl_HWNDKey.AutoSize = true;
            this.Lbl_HWNDKey.Location = new Point(5, 8);
            this.Lbl_HWNDKey.Name = "Lbl_HWNDKey";
            this.Lbl_HWNDKey.Size = new Size(0x44, 0x11);
            this.Lbl_HWNDKey.TabIndex = 0xa6;
            this.Lbl_HWNDKey.Text = "计划句柄：";
            this.Err_Hint.ContainerControl = this;
            this.Err_Hint.Icon = (Icon) manager.GetObject("Err_Hint.Icon");
            this.Pnl_RX.Controls.Add(this.Ckb_WZ5);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ4);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ3);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ2);
            this.Pnl_RX.Controls.Add(this.Lbl_RXZJ);
            this.Pnl_RX.Controls.Add(this.Txt_RXZJ);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ1);
            this.Pnl_RX.Location = new Point(0x149, 130);
            this.Pnl_RX.Name = "Pnl_RX";
            this.Pnl_RX.Size = new Size(0xc3, 0x41);
            this.Pnl_RX.TabIndex = 0x14e;
            this.Ckb_WZ5.AutoSize = true;
            this.Ckb_WZ5.Location = new Point(0x99, 0x26);
            this.Ckb_WZ5.Name = "Ckb_WZ5";
            this.Ckb_WZ5.Size = new Size(0x27, 0x15);
            this.Ckb_WZ5.TabIndex = 0x14d;
            this.Ckb_WZ5.Text = "个";
            this.Ckb_WZ5.UseVisualStyleBackColor = true;
            this.Ckb_WZ5.Visible = false;
            this.Ckb_WZ5.CheckedChanged += new EventHandler(this.Ckb_WZ_CheckedChanged);
            this.Ckb_WZ4.AutoSize = true;
            this.Ckb_WZ4.Location = new Point(0x74, 0x26);
            this.Ckb_WZ4.Name = "Ckb_WZ4";
            this.Ckb_WZ4.Size = new Size(0x27, 0x15);
            this.Ckb_WZ4.TabIndex = 0x14c;
            this.Ckb_WZ4.Text = "十";
            this.Ckb_WZ4.UseVisualStyleBackColor = true;
            this.Ckb_WZ4.Visible = false;
            this.Ckb_WZ4.CheckedChanged += new EventHandler(this.Ckb_WZ_CheckedChanged);
            this.Ckb_WZ3.AutoSize = true;
            this.Ckb_WZ3.Location = new Point(0x4f, 0x26);
            this.Ckb_WZ3.Name = "Ckb_WZ3";
            this.Ckb_WZ3.Size = new Size(0x27, 0x15);
            this.Ckb_WZ3.TabIndex = 0x14b;
            this.Ckb_WZ3.Text = "百";
            this.Ckb_WZ3.UseVisualStyleBackColor = true;
            this.Ckb_WZ3.Visible = false;
            this.Ckb_WZ3.CheckedChanged += new EventHandler(this.Ckb_WZ_CheckedChanged);
            this.Ckb_WZ2.AutoSize = true;
            this.Ckb_WZ2.Location = new Point(0x2a, 0x26);
            this.Ckb_WZ2.Name = "Ckb_WZ2";
            this.Ckb_WZ2.Size = new Size(0x27, 0x15);
            this.Ckb_WZ2.TabIndex = 330;
            this.Ckb_WZ2.Text = "千";
            this.Ckb_WZ2.UseVisualStyleBackColor = true;
            this.Ckb_WZ2.Visible = false;
            this.Ckb_WZ2.CheckedChanged += new EventHandler(this.Ckb_WZ_CheckedChanged);
            this.Lbl_RXZJ.AutoSize = true;
            this.Lbl_RXZJ.Location = new Point(3, 5);
            this.Lbl_RXZJ.Name = "Lbl_RXZJ";
            this.Lbl_RXZJ.Size = new Size(0x41, 0x11);
            this.Lbl_RXZJ.TabIndex = 0x143;
            this.Lbl_RXZJ.Text = "中几=中：";
            this.Lbl_RXZJ.Visible = false;
            this.Txt_RXZJ.Location = new Point(0x4e, 2);
            this.Txt_RXZJ.Name = "Txt_RXZJ";
            this.Txt_RXZJ.Size = new Size(110, 0x17);
            this.Txt_RXZJ.TabIndex = 0x144;
            this.Txt_RXZJ.Text = "1-10";
            this.Txt_RXZJ.Visible = false;
            this.Ckb_WZ1.AutoSize = true;
            this.Ckb_WZ1.Location = new Point(5, 0x26);
            this.Ckb_WZ1.Name = "Ckb_WZ1";
            this.Ckb_WZ1.Size = new Size(0x27, 0x15);
            this.Ckb_WZ1.TabIndex = 0x149;
            this.Ckb_WZ1.Text = "万";
            this.Ckb_WZ1.UseVisualStyleBackColor = true;
            this.Ckb_WZ1.Visible = false;
            this.Ckb_WZ1.CheckedChanged += new EventHandler(this.Ckb_WZ_CheckedChanged);
            this.Ckb_More.Appearance = Appearance.Button;
            this.Ckb_More.AutoCheck = false;
            this.Ckb_More.FlatAppearance.BorderSize = 0;
            this.Ckb_More.FlatStyle = FlatStyle.Flat;
            this.Ckb_More.Font = new Font("微软雅黑", 9f, FontStyle.Bold);
            this.Ckb_More.Image = Resources.WindowDown;
            this.Ckb_More.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_More.Location = new Point(6, 0x17e);
            this.Ckb_More.Name = "Ckb_More";
            this.Ckb_More.Size = new Size(80, 0x19);
            this.Ckb_More.TabIndex = 0x158;
            this.Ckb_More.Text = "更多设置";
            this.Ckb_More.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_More.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_More.UseVisualStyleBackColor = true;
            this.Ckb_More.Click += new EventHandler(this.Ckb_More_Click);
            this.Ckb_RefreshPlan.Appearance = Appearance.Button;
            this.Ckb_RefreshPlan.AutoCheck = false;
            this.Ckb_RefreshPlan.FlatAppearance.BorderSize = 0;
            this.Ckb_RefreshPlan.FlatStyle = FlatStyle.Flat;
            this.Ckb_RefreshPlan.Image = Resources.Refresh;
            this.Ckb_RefreshPlan.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_RefreshPlan.Location = new Point(0xf1, 5);
            this.Ckb_RefreshPlan.Name = "Ckb_RefreshPlan";
            this.Ckb_RefreshPlan.Size = new Size(60, 0x19);
            this.Ckb_RefreshPlan.TabIndex = 0x13d;
            this.Ckb_RefreshPlan.Text = "刷新";
            this.Ckb_RefreshPlan.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_RefreshPlan.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_RefreshPlan.UseVisualStyleBackColor = true;
            this.Ckb_RefreshPlan.Click += new EventHandler(this.Ckb_RefreshPlan_Click);
            this.Ckb_CSFXSeparate.Appearance = Appearance.Button;
            this.Ckb_CSFXSeparate.AutoCheck = false;
            this.Ckb_CSFXSeparate.FlatAppearance.BorderSize = 0;
            this.Ckb_CSFXSeparate.FlatStyle = FlatStyle.Flat;
            this.Ckb_CSFXSeparate.Image = Resources.Search;
            this.Ckb_CSFXSeparate.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_CSFXSeparate.Location = new Point(0xce, 7);
            this.Ckb_CSFXSeparate.Name = "Ckb_CSFXSeparate";
            this.Ckb_CSFXSeparate.Size = new Size(60, 0x19);
            this.Ckb_CSFXSeparate.TabIndex = 0x133;
            this.Ckb_CSFXSeparate.Text = "分析";
            this.Ckb_CSFXSeparate.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_CSFXSeparate.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_CSFXSeparate.UseVisualStyleBackColor = true;
            this.Ckb_CSFXSeparate.Click += new EventHandler(this.Ckb_CSFXSeparate_Click);
            this.Ckb_CSFXPlay.Appearance = Appearance.Button;
            this.Ckb_CSFXPlay.AutoCheck = false;
            this.Ckb_CSFXPlay.FlatAppearance.BorderSize = 0;
            this.Ckb_CSFXPlay.FlatStyle = FlatStyle.Flat;
            this.Ckb_CSFXPlay.Image = Resources.Search;
            this.Ckb_CSFXPlay.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_CSFXPlay.Location = new Point(0xce, 0x2a);
            this.Ckb_CSFXPlay.Name = "Ckb_CSFXPlay";
            this.Ckb_CSFXPlay.Size = new Size(60, 0x19);
            this.Ckb_CSFXPlay.TabIndex = 0x134;
            this.Ckb_CSFXPlay.Text = "分析";
            this.Ckb_CSFXPlay.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_CSFXPlay.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_CSFXPlay.UseVisualStyleBackColor = true;
            this.Ckb_CSFXPlay.Click += new EventHandler(this.Ckb_CSFXPlay_Click);
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            base.Controls.Add(this.Ckb_More);
            base.Controls.Add(this.Pnl_RX);
            base.Controls.Add(this.Pnl_MainInfo);
            base.Controls.Add(this.Lbl_MoreHint);
            base.Controls.Add(this.Pnl_More);
            base.Controls.Add(this.Cbb_Unit);
            base.Controls.Add(this.Lbl_Unit);
            base.Controls.Add(this.Cbb_PlayName);
            base.Controls.Add(this.Cbb_PlayType);
            base.Controls.Add(this.Lbl_PlayType);
            base.Controls.Add(this.Lbl_PlayName);
            base.Controls.Add(this.Pnl_Times);
            base.Controls.Add(this.Lbl_Name);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "FNWJJHLine";
            base.Size = new Size(0x20d, 0x2ad);
            base.Load += new EventHandler(this.FNWJJHLine_Load);
            this.Pnl_CodeType.ResumeLayout(false);
            this.Pnl_CodeType.PerformLayout();
            this.Pnl_Times.ResumeLayout(false);
            this.Pnl_Times.PerformLayout();
            this.Pnl_More.ResumeLayout(false);
            this.Pnl_More.PerformLayout();
            this.Nm_KSHT.EndInit();
            this.Nm_YLHT.EndInit();
            this.Nm_ModeExpect.EndInit();
            this.Pnl_MainInfo.ResumeLayout(false);
            this.Pnl_MainInfo.PerformLayout();
            this.Tab_Main.ResumeLayout(false);
            this.Tap_PlanCS.ResumeLayout(false);
            this.Pnl_PlanCS.ResumeLayout(false);
            this.Pnl_PlanCS.PerformLayout();
            this.Tap_ViewPlan.ResumeLayout(false);
            this.Pnl_ViewPlan.ResumeLayout(false);
            this.Pnl_ViewPlan.PerformLayout();
            ((ISupportInitialize) this.Pic_HWND).EndInit();
            ((ISupportInitialize) this.Err_Hint).EndInit();
            this.Pnl_RX.ResumeLayout(false);
            this.Pnl_RX.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void Pic_Hwnd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.IsDrag = true;
                Bitmap bitmap = new Bitmap(Resources.Eye);
                this.Cursor = new Cursor(bitmap.GetHicon());
                this.Pic_HWND.Image = this.NullImage;
            }
        }

        private void Pic_Hwnd_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsDrag)
            {
                IntPtr item = CommFunc.WindowFromPoint(Control.MousePosition);
                if (this.SkipHandleList.Contains(item))
                {
                    item = IntPtr.Zero;
                }
                if (item != this.CurrentHWND)
                {
                    this.DrawRevFrame(this.CurrentHWND);
                    this.DrawRevFrame(item);
                    this.CurrentHWND = item;
                    this.IsUsedHWND = true;
                }
            }
        }

        private void Pic_Hwnd_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.IsDrag)
            {
                this.IsDrag = false;
                this.Cursor = Cursors.Default;
                if (this.CurrentHWND != IntPtr.Zero)
                {
                    this.DrawRevFrame(this.CurrentHWND);
                    this.CurrentHWND = IntPtr.Zero;
                    CommFunc.SendMessage(base.Handle, 0x202, 0, 0);
                }
                this.Pic_HWND.Image = this.CrossImage;
            }
        }

        private void Rdb_BTPlan_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_BTPlan.Enabled = this.Ckb_BTCount.Enabled = this.Rdb_BTPlan.Checked;
            this.Cbb_BTFN.Enabled = this.Ckb_BTFN.Enabled = this.Rdb_BTFN.Checked;
        }

        private void RefreshCSControl(bool pAll)
        {
            if (pAll)
            {
                this.Txt_CSPlayName.Text = "";
                this.Ckb_CSFXSeparate.Visible = false;
                this.Ckb_CSFXPlay.Visible = this.Cbb_CSFXPlay.Visible = false;
                this.Cbb_CSFXPlay.Items.Clear();
            }
        }

        private void RefreshMoreControl()
        {
            this.Ckb_More.Image = this.IsMore ? Resources.WindowUp : Resources.WindowDown;
            this.Lbl_MoreHint.Visible = !this.IsMore;
            foreach (Control control in this.MoreControl)
            {
                control.Visible = this.IsMore;
            }
        }

        private void RefreshPlanMain(bool pHint = false)
        {
            if (this._RunEvent)
            {
                ConfigurationStatus.WJJHInfo pInfo = new ConfigurationStatus.WJJHInfo {
                    HWNDString = this.Txt_HWNDValue.Text,
                    CSSeparate = this.Txt_CSSeparate.Text,
                    CSPlay = this.Txt_CSPlayName.Text,
                    CSValueLeft = this.Txt_CSValueLeft.Text,
                    CSValueRight = this.Txt_CSValueRight.Text,
                    PlayType = this.Cbb_PlayType.Text,
                    PlayName = this.Cbb_PlayName.Text
                };
                GetPlanValue(pInfo);
                this.Txt_ViewPlan.Text = pInfo.PlanValue;
                List<string> pCodeList = CommFunc.SplitBetsCode(pInfo.PlanValue, pInfo.Play);
                this.Lbl_NumberCount.Text = $"{CommFunc.GetBetsCodeCount(pCodeList, pInfo.Play, this.GetRXWZList())} 注";
                if (pHint && (pInfo.Error != ""))
                {
                    CommFunc.PublicMessageAll(pInfo.Error, true, MessageBoxIcon.Asterisk, "");
                }
            }
        }

        public void SaveControlInfoByReg()
        {
            CommFunc.SaveFormUseingInfo(this.ControlList, this.RegScenarionConfigPath);
            CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegScenarionConfigPath);
        }

        public void SetBTPlanValue(string pValue)
        {
            this.Txt_BTPlan.Text = pValue;
            this.Txt_BTPlan.SelectAll();
            this.Txt_BTPlan.Focus();
        }

        public void SetControlInfoByReg(string pRegScenarionConfigPath)
        {
            this.RegScenarionConfigPath = pRegScenarionConfigPath + @"\" + base.Name;
            this.ControlList = new List<Control>();
            this.SpecialControlList = new List<Control>();
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegScenarionConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegScenarionConfigPath);
            if (this.Cbb_Mode.SelectedIndex == -1)
            {
                this.Cbb_Mode.SelectedIndex = 0;
            }
            if (this.Cbb_FBType.SelectedIndex == -1)
            {
                this.Cbb_FBType.SelectedIndex = 0;
            }
        }

        public void SetControlValue(ConfigurationStatus.FNBase pInfo)
        {
            this._RunEvent = false;
            List<string> dicKeyList = CommFunc.GetDicKeyList<List<ConfigurationStatus.PlayBase>>(AppInfo.PlayDic);
            CommFunc.SetComboBoxList(this.Cbb_PlayType, dicKeyList);
            CommFunc.SetComboBoxList(this.Cbb_BTFN, AppInfo.BTFNList);
            ConfigurationStatus.FNWJJH fnwjjh = (ConfigurationStatus.FNWJJH) pInfo;
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_PlayType, pInfo.PlayType);
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_PlayName, pInfo.PlayName);
            this.Cbb_Unit.SelectedIndex = (int) pInfo.Unit;
            this.Txt_RXZJ.Text = fnwjjh.RXZJ;
            this.SetRXWZList(fnwjjh.RXWZList);
            this.Cbb_Mode.SelectedIndex = (int) pInfo.Mode;
            this.Nm_ModeExpect.Value = pInfo.ModeExpect;
            this.Cbb_FBType.SelectedIndex = (int) pInfo.FBInfo;
            this.Rdb_CodeType1.Checked = pInfo.IsBetsZJ;
            this.Rdb_CodeType2.Checked = !pInfo.IsBetsZJ;
            this.Rdb_BTPlan.Checked = pInfo.BTType == ConfigurationStatus.SCTimesType.Plan;
            this.Rdb_BTFN.Checked = pInfo.BTType == ConfigurationStatus.SCTimesType.FN;
            this.Txt_BTPlan.Text = CommFunc.Join(pInfo.BTPlanList, ",");
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_BTFN, pInfo.BTFNName);
            this.IsMore = pInfo.VisibleMore;
            this.RefreshCSControl(true);
            this.Txt_HWNDValue.Text = fnwjjh.HWND.ToString();
            this.Txt_CSSeparate.Text = fnwjjh.HWNDSeparate;
            this.Txt_CSPlayName.Text = fnwjjh.HWNDPlay;
            this.Txt_CSValueLeft.Text = fnwjjh.HWNDValueLeft;
            this.Txt_CSValueRight.Text = fnwjjh.HWNDValueRight;
            this.Ckb_MN1.Checked = pInfo.ZSBetsSelect1;
            this.Ckb_MN3.Checked = pInfo.ZSBetsSelect2;
            this.Ckb_MN2.Checked = pInfo.MNBetsSelect1;
            this.Ckb_MN4.Checked = pInfo.MNBetsSelect2;
            this.Txt_MN1.Text = pInfo.ZSBetsMoney1;
            this.Txt_MN3.Text = pInfo.ZSBetsMoney2;
            this.Txt_MN2.Text = pInfo.MNBetsMoney1;
            this.Txt_MN4.Text = pInfo.MNBetsMoney2;
            this.Ckb_YLHT.Checked = pInfo.YLHTSelect;
            this.Ckb_KSHT.Checked = pInfo.KSHTSelect;
            this.Txt_YLHT.Text = pInfo.YLHTMoney;
            this.Txt_KSHT.Text = pInfo.KSHTMoney;
            this.Nm_YLHT.Value = pInfo.YLHTID;
            this.Nm_KSHT.Value = pInfo.KSHTID;
            this.Ckb_YLStopBets.Checked = pInfo.YLStopSelect;
            this.Ckb_KSStopBets.Checked = pInfo.KSStopSelect;
            this.Txt_YLStopBets.Text = pInfo.YLStopMoney;
            this.Txt_KSStopBets.Text = pInfo.KSStopMoney;
            this.BT_Time.SetControlValue(pInfo);
            this.RefreshMoreControl();
            this._RunEvent = true;
            this.RefreshPlanMain(false);
            this.Tab_Main.SelectedIndex = (this.Txt_ViewPlan.Text == "") ? 0 : 1;
        }

        private void SetRXWZList(List<int> pValueList)
        {
            if (CommFunc.CheckPlayIsRXDS(this.Play))
            {
                List<int> list = new List<int>();
                for (int i = 0; i < this.RXWZList.Count; i++)
                {
                    this.RXWZList[i].Checked = pValueList.Contains(AppInfo.FiveDic[this.RXWZList[i].Text]);
                }
            }
        }

        private void Txt_CSPlayName_Enter(object sender, EventArgs e)
        {
            this.Ckb_CSFXPlay.Visible = this.Cbb_CSFXPlay.Visible = true;
        }

        private void Txt_CSSeparate_Enter(object sender, EventArgs e)
        {
            this.Ckb_CSFXSeparate.Visible = true;
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg != 0x10) && this.IsUsedHWND)
            {
                Point lpPoint = new Point();
                CommFunc.GetCursorPos(ref lpPoint);
                this.Txt_HWNDValue.Text = CommFunc.WindowFromPoint(lpPoint).ToString();
                this.IsUsedHWND = false;
            }
            base.WndProc(ref m);
        }

        public string Hint
        {
            get => 
                this.Lbl_Name.Text;
            set
            {
                this.Lbl_Name.Text = value;
            }
        }

        public string Play
        {
            get
            {
                string text = this.Cbb_PlayType.Text;
                string str2 = this.Cbb_PlayName.Text;
                return (text + str2);
            }
        }

        public ConfigurationStatus.PlayBase PlayInfo
        {
            get
            {
                string text = this.Cbb_PlayType.Text;
                string playName = this.Cbb_PlayName.Text;
                return CommFunc.GetPlayInfo(text, playName);
            }
        }
    }
}

