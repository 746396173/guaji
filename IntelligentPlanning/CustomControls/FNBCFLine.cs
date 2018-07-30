namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FNBCFCHLine : UserControl
    {
        private bool _RunEvent = false;
        private BetsTimeLine BT_Time;
        public List<Button> ButtonList = null;
        private ComboBox Cbb_BetsJKMode;
        private ComboBox Cbb_BTFN;
        private ComboBox Cbb_FBType;
        private ComboBox Cbb_Mode;
        private ComboBox Cbb_PlayName;
        private ComboBox Cbb_PlayType;
        private ComboBox Cbb_Unit;
        private CheckBox Ckb_BetsJK;
        private Label Ckb_BetsJKMode;
        private CheckBox Ckb_BTCount;
        private CheckBox Ckb_BTFN;
        private CheckBox Ckb_EditJK;
        private CheckBox Ckb_KSHT;
        private CheckBox Ckb_KSStopBets;
        private CheckBox Ckb_LRWCHBX;
        private CheckBox Ckb_LRWCHLen;
        private CheckBox Ckb_LRWCHQX;
        private CheckBox Ckb_LRWCHRe;
        private CheckBox Ckb_LRWCHWen;
        private CheckBox Ckb_MN1;
        private CheckBox Ckb_MN2;
        private CheckBox Ckb_MN3;
        private CheckBox Ckb_MN4;
        private CheckBox Ckb_More;
        private CheckBox Ckb_RefreshSort;
        private CheckBox Ckb_Type1;
        private CheckBox Ckb_Type10;
        private CheckBox Ckb_Type2;
        private CheckBox Ckb_Type3;
        private CheckBox Ckb_Type4;
        private CheckBox Ckb_Type5;
        private CheckBox Ckb_Type6;
        private CheckBox Ckb_Type7;
        private CheckBox Ckb_Type8;
        private CheckBox Ckb_Type9;
        private CheckBox Ckb_WZ1;
        private CheckBox Ckb_WZ2;
        private CheckBox Ckb_WZ3;
        private CheckBox Ckb_WZ4;
        private CheckBox Ckb_WZ5;
        private CheckBox Ckb_YLHT;
        private CheckBox Ckb_YLStopBets;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private ErrorProvider Err_Hint;
        private bool IsMore = false;
        public List<Label> LabelList = null;
        private Label Lbl_CodeType;
        private Label Lbl_FBType;
        private Label Lbl_KSHT;
        private Label Lbl_LRWDMSort;
        private Label Lbl_LRWExpect;
        private Label Lbl_LRWRC;
        private Label Lbl_LRWType;
        private Label Lbl_MNBets;
        private Label Lbl_Mode;
        private Label Lbl_ModeExpect;
        private Label Lbl_MoreHint;
        private Label Lbl_Name;
        private Label Lbl_PlayName;
        private Label Lbl_PlayType;
        private Label Lbl_RXZJ;
        private Label Lbl_StopBets;
        private Label Lbl_Unit;
        private Label Lbl_YKHT;
        private Label Lbl_YLHT;
        private List<Control> MoreControl = null;
        private NumericUpDown Nm_KSHT;
        private NumericUpDown Nm_LRWExpect;
        private NumericUpDown Nm_ModeExpect;
        private NumericUpDown Nm_YLHT;
        private Panel Pnl_CodeType;
        private Panel Pnl_MainInfo;
        private Panel Pnl_More;
        private Panel Pnl_RX;
        private Panel Pnl_Times;
        private RadioButton Rdb_BTFN;
        private RadioButton Rdb_BTPlan;
        private RadioButton Rdb_CodeType1;
        private RadioButton Rdb_CodeType2;
        private string RegScenarionConfigPath = "";
        private List<CheckBox> RXWZList = null;
        private List<Control> SpecialControlList = null;
        private TextBox Txt_BetsJK;
        private TextBox Txt_BTPlan;
        private TextBox Txt_KSHT;
        private TextBox Txt_KSStopBets;
        private TextBox Txt_LRWDMSort;
        private TextBox Txt_LRWRC;
        private TextBox Txt_MN1;
        private TextBox Txt_MN2;
        private TextBox Txt_MN3;
        private TextBox Txt_MN4;
        private TextBox Txt_RXZJ;
        private TextBox Txt_YLHT;
        private TextBox Txt_YLStopBets;
        private List<CheckBox> TypeList = null;

        public FNLRWCHLine()
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
                List<Control> list2 = new List<Control>();
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Ckb_EditJK,
                    this.Ckb_BTCount,
                    this.Ckb_BTFN,
                    this.Ckb_More,
                    this.Lbl_PlayType,
                    this.Lbl_PlayName,
                    this.Lbl_Unit,
                    this.Ckb_BetsJK,
                    this.Ckb_BetsJKMode,
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
                    this.Ckb_RefreshSort,
                    this.Ckb_LRWCHRe,
                    this.Ckb_LRWCHLen,
                    this.Ckb_LRWCHWen,
                    this.Ckb_LRWCHQX,
                    this.Ckb_LRWCHBX,
                    this.Lbl_LRWDMSort,
                    this.Lbl_LRWExpect,
                    this.Lbl_LRWType,
                    this.Lbl_LRWRC,
                    this.Ckb_Type1,
                    this.Ckb_Type2,
                    this.Ckb_Type3,
                    this.Ckb_Type4,
                    this.Ckb_Type5,
                    this.Ckb_Type6,
                    this.Ckb_Type7,
                    this.Ckb_Type8,
                    this.Ckb_Type9,
                    this.Ckb_Type10
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
                List<ComboBox> pComboBoxList = new List<ComboBox> {
                    this.Cbb_PlayType,
                    this.Cbb_PlayName,
                    this.Cbb_Unit,
                    this.Cbb_BetsJKMode,
                    this.Cbb_BTFN,
                    this.Cbb_FBType,
                    this.Cbb_Mode
                };
                CommFunc.BeautifyComboBox(pComboBoxList);
            }
        }

        private void Cbb_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.Cbb_Mode.Text;
            this.Nm_ModeExpect.Visible = this.Lbl_ModeExpect.Visible = text.Contains("N");
        }

        private void Cbb_PlayName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshSortMain();
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
            for (int i = playNameList.Count - 1; i >= 0; i--)
            {
                if (CommFunc.CheckPlayIsRXFS(text + playNameList[i]))
                {
                    playNameList.RemoveAt(i);
                }
            }
            CommFunc.SetComboBoxList(this.Cbb_PlayName, playNameList);
            this.Cbb_PlayName.SelectedIndex = 0;
            this.Lbl_RXZJ.Visible = this.Txt_RXZJ.Visible = CommFunc.CheckPlayIsRX(text);
        }

        private void Ckb_BetsJK_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_BetsJK.Enabled = this.Ckb_BetsJK.Checked;
            this.Ckb_BetsJKMode.Enabled = this.Cbb_BetsJKMode.Enabled = this.Ckb_BetsJK.Checked;
        }

        private void Ckb_BTCount_Click(object sender, EventArgs e)
        {
            string text = this.Txt_BTPlan.Text;
            ((AutoBetsWindow)Program.MainApp).QHBTCount(text);
        }

        private void Ckb_BTFN_Click(object sender, EventArgs e)
        {
            string text = this.Cbb_BTFN.Text;
            ((AutoBetsWindow)Program.MainApp).QHBTFN(text);
        }

        private void Ckb_EditJK_Click(object sender, EventArgs e)
        {
            FrmInputJK tjk = new FrmInputJK();
            if (tjk.ShowDialog() == DialogResult.OK)
            {
                this.Txt_BetsJK.Text = FrmInputJK.OutValue;
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

        private void Ckb_LRWCHBX_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.TypeList.Count; i++)
            {
                this.TypeList[i].Checked = false;
            }
        }

        private void Ckb_LRWCHLen_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int> {
                7,
                8,
                9
            };
            for (int i = 0; i < this.TypeList.Count; i++)
            {
                if (list.Contains(i))
                {
                    this.TypeList[i].Checked = true;
                }
            }
        }

        private void Ckb_LRWCHQX_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.TypeList.Count; i++)
            {
                this.TypeList[i].Checked = true;
            }
        }

        private void Ckb_LRWCHRe_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int> {
                0,
                1,
                2
            };
            for (int i = 0; i < this.TypeList.Count; i++)
            {
                if (list.Contains(i))
                {
                    this.TypeList[i].Checked = true;
                }
            }
        }

        private void Ckb_LRWCHWen_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int> {
                3,
                4,
                5,
                6
            };
            for (int i = 0; i < this.TypeList.Count; i++)
            {
                if (list.Contains(i))
                {
                    this.TypeList[i].Checked = true;
                }
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

        private void Ckb_More_Click(object sender, EventArgs e)
        {
            this.IsMore = !this.IsMore;
            this.RefreshMoreControl();
        }

        private void Ckb_RefreshSort_Click(object sender, EventArgs e)
        {
            this.RefreshSortMain();
        }

        private void Ckb_WZ_CheckedChanged(object sender, EventArgs e)
        {
            this.RefreshSortMain();
        }

        private void Ckb_YLHT_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_YLHT.Enabled = this.Lbl_YLHT.Enabled = this.Nm_YLHT.Enabled = this.Ckb_YLHT.Checked;
        }

        private void Ckb_YLStopBets_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_YLStopBets.Enabled = this.Ckb_YLStopBets.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FNLRWCHLine_Load(object sender, EventArgs e)
        {
            List<CheckBox> list2 = new List<CheckBox> {
                this.Ckb_Type1,
                this.Ckb_Type2,
                this.Ckb_Type3,
                this.Ckb_Type4,
                this.Ckb_Type5,
                this.Ckb_Type6,
                this.Ckb_Type7,
                this.Ckb_Type8,
                this.Ckb_Type9,
                this.Ckb_Type10
            };
            this.TypeList = list2;
            List<Label> list3 = new List<Label> {
                this.Lbl_Name
            };
            this.LabelList = list3;
            CommFunc.SetLabelFormat(this.LabelList);
            List<Control> list4 = new List<Control> {
                this.Pnl_More
            };
            this.MoreControl = list4;
            this.Ckb_More.ForeColor = AppInfo.appForeColor;
            this.Lbl_MoreHint.ForeColor = AppInfo.redForeColor;
            List<CheckBox> list5 = new List<CheckBox> {
                this.Ckb_WZ1,
                this.Ckb_WZ2,
                this.Ckb_WZ3,
                this.Ckb_WZ4,
                this.Ckb_WZ5
            };
            this.RXWZList = list5;
            CommFunc.SetControlHint(this.Err_Hint, this.Ckb_BetsJK, "当方案历史中挂情况满足设定条件时开始投注（0表示挂，1表示中），例如设置000就表示连挂3期后开始投注。");
            List<CheckBox> pCheckBoxList = new List<CheckBox> {
                this.Ckb_EditJK,
                this.Ckb_BTCount,
                this.Ckb_BTFN,
                this.Ckb_More,
                this.Ckb_RefreshSort,
                this.Ckb_LRWCHRe,
                this.Ckb_LRWCHLen,
                this.Ckb_LRWCHWen,
                this.Ckb_LRWCHQX,
                this.Ckb_LRWCHBX
            };
            CommFunc.SetCheckBoxFormatFlat(pCheckBoxList);
            this.BeautifyInterface();
            CommFunc.AppHandler(this.Cbb_Unit);
            this.Ckb_MN1.Visible = this.Txt_MN1.Visible = this.Ckb_MN2.Visible = this.Txt_MN2.Visible = this.Ckb_MN3.Visible = this.Txt_MN3.Visible = this.Ckb_MN4.Visible = this.Txt_MN4.Visible = this.Lbl_MNBets.Visible = !AppInfo.Account.Configuration.IsHideMNBets;
            this._RunEvent = true;
        }

        public ConfigurationStatus.FNLRWCH GetControlValue(ref string pError)
        {
            ConfigurationStatus.FNBase pInfo = new ConfigurationStatus.FNLRWCH();
            this.BT_Time.GetControlValue(ref pInfo);
            ConfigurationStatus.FNLRWCH fnlrwch = (ConfigurationStatus.FNLRWCH)pInfo;
            fnlrwch.PlayType = this.Cbb_PlayType.Text;
            fnlrwch.PlayName = this.Cbb_PlayName.Text;
            fnlrwch.Unit = this.GetUnit();
            fnlrwch.BetsJKSelect = this.Ckb_BetsJK.Checked;
            fnlrwch.BetsJKValue = this.Txt_BetsJK.Text;
            if (fnlrwch.BetsJKSelect && (fnlrwch.BetsJKValue == ""))
            {
                pError = "至少要输入一个监控形态！";
                this.Txt_BetsJK.Focus();
                return null;
            }
            if (!CommFunc.CheckStringIsRange(fnlrwch.BetsJKValue, "01"))
            {
                pError = "监控形态只能输入0或1！";
                this.Txt_BetsJK.Focus();
                return null;
            }
            fnlrwch.BetsJKMode = this.GetJKMode();
            fnlrwch.RXZJ = this.Txt_RXZJ.Text;
            fnlrwch.RXWZList = this.GetRXWZList();
            if (CommFunc.CheckPlayIsRXDS(this.Play) && (fnlrwch.RXWZList.Count < this.PlayInfo.IndexList.Count))
            {
                pError = $"【{this.Play}】至少要选择{this.PlayInfo.IndexList.Count}个位置！";
            }
            fnlrwch.Mode = this.GetMode();
            if (fnlrwch.BetsJKSelect && (fnlrwch.Mode != ConfigurationStatus.SchemeMode.QQHH))
            {
                pError = "启动监控形态后只能支持【每期换号】规则！";
                return null;
            }
            fnlrwch.ModeExpect = Convert.ToInt32(this.Nm_ModeExpect.Value);
            fnlrwch.FBInfo = this.GetFBType();
            fnlrwch.IsBetsZJ = this.Rdb_CodeType1.Checked;
            fnlrwch.BTType = this.GetTimesType();
            fnlrwch.BTPlanList = CommFunc.SplitString(this.Txt_BTPlan.Text.Replace("，", ","), ",", -1);
            if (this.Rdb_BTPlan.Checked && (fnlrwch.BTPlanList.Count == 0))
            {
                pError = "至少要输入一个直线倍投！";
                this.Txt_BTPlan.Focus();
                return null;
            }
            foreach (string str in fnlrwch.BTPlanList)
            {
                if (!CommFunc.CheckBetsTimes(str, ref pError))
                {
                    this.Txt_BTPlan.SelectAll();
                    this.Txt_BTPlan.Focus();
                    return null;
                }
            }
            fnlrwch.BTFNName = this.Cbb_BTFN.Text;
            fnlrwch.VisibleMore = this.IsMore;
            fnlrwch.LRWExpect = Convert.ToInt32(this.Nm_LRWExpect.Value);
            fnlrwch.LRWTypeList = this.GetLRWTypeList();
            if (fnlrwch.LRWTypeList.Count == 0)
            {
                pError = "至少要选择一个出号类型！";
                return null;
            }
            fnlrwch.LRWRC = this.Txt_LRWRC.Text;
            if (fnlrwch.LRWRC == "")
            {
                pError = "至少要输入一个出号个数！";
                this.Txt_LRWRC.Focus();
                return null;
            }
            fnlrwch.ZSBetsSelect1 = this.Ckb_MN1.Checked;
            fnlrwch.ZSBetsSelect2 = this.Ckb_MN3.Checked;
            fnlrwch.MNBetsSelect1 = this.Ckb_MN2.Checked;
            fnlrwch.MNBetsSelect2 = this.Ckb_MN4.Checked;
            fnlrwch.ZSBetsMoney1 = this.Txt_MN1.Text;
            fnlrwch.ZSBetsMoney2 = this.Txt_MN3.Text;
            fnlrwch.MNBetsMoney1 = this.Txt_MN2.Text;
            fnlrwch.MNBetsMoney2 = this.Txt_MN4.Text;
            pInfo.YLHTSelect = this.Ckb_YLHT.Checked;
            pInfo.KSHTSelect = this.Ckb_KSHT.Checked;
            pInfo.YLHTMoney = this.Txt_YLHT.Text;
            pInfo.KSHTMoney = this.Txt_KSHT.Text;
            pInfo.YLHTID = Convert.ToInt32(this.Nm_YLHT.Value);
            pInfo.KSHTID = Convert.ToInt32(this.Nm_KSHT.Value);
            fnlrwch.YLStopSelect = this.Ckb_YLStopBets.Checked;
            fnlrwch.KSStopSelect = this.Ckb_KSStopBets.Checked;
            fnlrwch.YLStopMoney = this.Txt_YLStopBets.Text;
            fnlrwch.KSStopMoney = this.Txt_KSStopBets.Text;
            return fnlrwch;
        }

        private ConfigurationStatus.FBType GetFBType() =>
            ((ConfigurationStatus.FBType)this.Cbb_FBType.SelectedIndex);

        private ConfigurationStatus.JKMode GetJKMode() =>
            ((ConfigurationStatus.JKMode)this.Cbb_BetsJKMode.SelectedIndex);

        private List<int> GetLRWTypeList()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.TypeList.Count; i++)
            {
                if (this.TypeList[i].Checked)
                {
                    list.Add(i);
                }
            }
            return list;
        }

        private ConfigurationStatus.SchemeMode GetMode() =>
            ((ConfigurationStatus.SchemeMode)this.Cbb_Mode.SelectedIndex);

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

        private ConfigurationStatus.SCTimesType GetTimesType()
        {
            if (this.Rdb_BTPlan.Checked)
            {
                return ConfigurationStatus.SCTimesType.Plan;
            }
            return ConfigurationStatus.SCTimesType.FN;
        }

        private ConfigurationStatus.SCUnitType GetUnit() =>
            ((ConfigurationStatus.SCUnitType)this.Cbb_Unit.SelectedIndex);

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FNLRWCHLine));
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
            this.Ckb_Type9 = new CheckBox();
            this.Ckb_Type8 = new CheckBox();
            this.Ckb_Type7 = new CheckBox();
            this.Ckb_Type6 = new CheckBox();
            this.Ckb_Type10 = new CheckBox();
            this.Ckb_Type5 = new CheckBox();
            this.Ckb_Type4 = new CheckBox();
            this.Ckb_Type3 = new CheckBox();
            this.Ckb_Type2 = new CheckBox();
            this.Ckb_Type1 = new CheckBox();
            this.Lbl_LRWDMSort = new Label();
            this.Txt_LRWRC = new TextBox();
            this.Txt_LRWDMSort = new TextBox();
            this.Lbl_LRWRC = new Label();
            this.Lbl_LRWExpect = new Label();
            this.Nm_LRWExpect = new NumericUpDown();
            this.Lbl_LRWType = new Label();
            this.Txt_BetsJK = new TextBox();
            this.Ckb_BetsJK = new CheckBox();
            this.Err_Hint = new ErrorProvider(this.components);
            this.Cbb_BetsJKMode = new ComboBox();
            this.Ckb_BetsJKMode = new Label();
            this.Pnl_RX = new Panel();
            this.Ckb_WZ5 = new CheckBox();
            this.Ckb_WZ4 = new CheckBox();
            this.Ckb_WZ3 = new CheckBox();
            this.Ckb_WZ2 = new CheckBox();
            this.Lbl_RXZJ = new Label();
            this.Txt_RXZJ = new TextBox();
            this.Ckb_WZ1 = new CheckBox();
            this.Ckb_EditJK = new CheckBox();
            this.Ckb_More = new CheckBox();
            this.Ckb_RefreshSort = new CheckBox();
            this.Ckb_LRWCHQX = new CheckBox();
            this.Ckb_LRWCHBX = new CheckBox();
            this.Ckb_LRWCHRe = new CheckBox();
            this.Ckb_LRWCHLen = new CheckBox();
            this.Ckb_LRWCHWen = new CheckBox();
            this.Pnl_CodeType.SuspendLayout();
            this.Pnl_Times.SuspendLayout();
            this.Pnl_More.SuspendLayout();
            this.Nm_KSHT.BeginInit();
            this.Nm_YLHT.BeginInit();
            this.Nm_ModeExpect.BeginInit();
            this.Pnl_MainInfo.SuspendLayout();
            this.Nm_LRWExpect.BeginInit();
            ((ISupportInitialize)this.Err_Hint).BeginInit();
            this.Pnl_RX.SuspendLayout();
            base.SuspendLayout();
            this.Lbl_Name.AutoSize = true;
            this.Lbl_Name.Location = new Point(3, 7);
            this.Lbl_Name.Name = "Lbl_Name";
            this.Lbl_Name.Size = new Size(0x44, 0x11);
            this.Lbl_Name.TabIndex = 0;
            this.Lbl_Name.Text = "不重复出号";
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
            this.Pnl_MainInfo.Controls.Add(this.Ckb_LRWCHWen);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_LRWCHLen);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_LRWCHRe);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_LRWCHQX);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_LRWCHBX);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_RefreshSort);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_Type9);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_Type8);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_Type7);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_Type6);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_Type10);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_Type5);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_Type4);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_Type3);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_Type2);
            this.Pnl_MainInfo.Controls.Add(this.Ckb_Type1);
            this.Pnl_MainInfo.Controls.Add(this.Lbl_LRWDMSort);
            this.Pnl_MainInfo.Controls.Add(this.Txt_LRWRC);
            this.Pnl_MainInfo.Controls.Add(this.Txt_LRWDMSort);
            this.Pnl_MainInfo.Controls.Add(this.Lbl_LRWRC);
            this.Pnl_MainInfo.Controls.Add(this.Lbl_LRWExpect);
            this.Pnl_MainInfo.Controls.Add(this.Nm_LRWExpect);
            this.Pnl_MainInfo.Controls.Add(this.Lbl_LRWType);
            this.Pnl_MainInfo.Location = new Point(6, 30);
            this.Pnl_MainInfo.Name = "Pnl_MainInfo";
            this.Pnl_MainInfo.Size = new Size(320, 0x102);
            this.Pnl_MainInfo.TabIndex = 0x13b;
            this.Ckb_Type9.AutoSize = true;
            this.Ckb_Type9.Location = new Point(200, 0x61);
            this.Ckb_Type9.Name = "Ckb_Type9";
            this.Ckb_Type9.Size = new Size(0x22, 0x15);
            this.Ckb_Type9.TabIndex = 0x117;
            this.Ckb_Type9.Text = "9";
            this.Ckb_Type9.UseVisualStyleBackColor = true;
            this.Ckb_Type8.AutoSize = true;
            this.Ckb_Type8.Location = new Point(160, 0x61);
            this.Ckb_Type8.Name = "Ckb_Type8";
            this.Ckb_Type8.Size = new Size(0x22, 0x15);
            this.Ckb_Type8.TabIndex = 0x116;
            this.Ckb_Type8.Text = "8";
            this.Ckb_Type8.UseVisualStyleBackColor = true;
            this.Ckb_Type7.AutoSize = true;
            this.Ckb_Type7.Location = new Point(120, 0x61);
            this.Ckb_Type7.Name = "Ckb_Type7";
            this.Ckb_Type7.Size = new Size(0x22, 0x15);
            this.Ckb_Type7.TabIndex = 0x115;
            this.Ckb_Type7.Text = "7";
            this.Ckb_Type7.UseVisualStyleBackColor = true;
            this.Ckb_Type6.AutoSize = true;
            this.Ckb_Type6.Location = new Point(80, 0x61);
            this.Ckb_Type6.Name = "Ckb_Type6";
            this.Ckb_Type6.Size = new Size(0x22, 0x15);
            this.Ckb_Type6.TabIndex = 0x114;
            this.Ckb_Type6.Text = "6";
            this.Ckb_Type6.UseVisualStyleBackColor = true;
            this.Ckb_Type10.AutoSize = true;
            this.Ckb_Type10.Location = new Point(240, 0x61);
            this.Ckb_Type10.Name = "Ckb_Type10";
            this.Ckb_Type10.Size = new Size(0x29, 0x15);
            this.Ckb_Type10.TabIndex = 0x113;
            this.Ckb_Type10.Text = "10";
            this.Ckb_Type10.UseVisualStyleBackColor = true;
            this.Ckb_Type5.AutoSize = true;
            this.Ckb_Type5.Location = new Point(240, 70);
            this.Ckb_Type5.Name = "Ckb_Type5";
            this.Ckb_Type5.Size = new Size(0x22, 0x15);
            this.Ckb_Type5.TabIndex = 0x112;
            this.Ckb_Type5.Text = "5";
            this.Ckb_Type5.UseVisualStyleBackColor = true;
            this.Ckb_Type4.AutoSize = true;
            this.Ckb_Type4.Location = new Point(200, 70);
            this.Ckb_Type4.Name = "Ckb_Type4";
            this.Ckb_Type4.Size = new Size(0x22, 0x15);
            this.Ckb_Type4.TabIndex = 0x111;
            this.Ckb_Type4.Text = "4";
            this.Ckb_Type4.UseVisualStyleBackColor = true;
            this.Ckb_Type3.AutoSize = true;
            this.Ckb_Type3.Checked = true;
            this.Ckb_Type3.CheckState = CheckState.Checked;
            this.Ckb_Type3.Location = new Point(160, 70);
            this.Ckb_Type3.Name = "Ckb_Type3";
            this.Ckb_Type3.Size = new Size(0x22, 0x15);
            this.Ckb_Type3.TabIndex = 0x110;
            this.Ckb_Type3.Text = "3";
            this.Ckb_Type3.UseVisualStyleBackColor = true;
            this.Ckb_Type2.AutoSize = true;
            this.Ckb_Type2.Checked = true;
            this.Ckb_Type2.CheckState = CheckState.Checked;
            this.Ckb_Type2.Location = new Point(120, 70);
            this.Ckb_Type2.Name = "Ckb_Type2";
            this.Ckb_Type2.Size = new Size(0x22, 0x15);
            this.Ckb_Type2.TabIndex = 0x10f;
            this.Ckb_Type2.Text = "2";
            this.Ckb_Type2.UseVisualStyleBackColor = true;
            this.Ckb_Type1.AutoSize = true;
            this.Ckb_Type1.Checked = true;
            this.Ckb_Type1.CheckState = CheckState.Checked;
            this.Ckb_Type1.Location = new Point(80, 70);
            this.Ckb_Type1.Name = "Ckb_Type1";
            this.Ckb_Type1.Size = new Size(0x22, 0x15);
            this.Ckb_Type1.TabIndex = 270;
            this.Ckb_Type1.Text = "1";
            this.Ckb_Type1.UseVisualStyleBackColor = true;
            this.Lbl_LRWDMSort.AutoSize = true;
            this.Lbl_LRWDMSort.Location = new Point(5, 8);
            this.Lbl_LRWDMSort.Name = "Lbl_LRWDMSort";
            this.Lbl_LRWDMSort.Size = new Size(0x44, 0x11);
            this.Lbl_LRWDMSort.TabIndex = 0x9e;
            this.Lbl_LRWDMSort.Text = "胆码排序：";
            this.Txt_LRWRC.Location = new Point(80, 0x9a);
            this.Txt_LRWRC.Name = "Txt_LRWRC";
            this.Txt_LRWRC.Size = new Size(0xe5, 0x17);
            this.Txt_LRWRC.TabIndex = 0xa4;
            this.Txt_LRWRC.Text = "1-2";
            this.Txt_LRWDMSort.Location = new Point(80, 5);
            this.Txt_LRWDMSort.Name = "Txt_LRWDMSort";
            this.Txt_LRWDMSort.ReadOnly = true;
            this.Txt_LRWDMSort.Size = new Size(0xa6, 0x17);
            this.Txt_LRWDMSort.TabIndex = 0x9f;
            this.Lbl_LRWRC.AutoSize = true;
            this.Lbl_LRWRC.Location = new Point(5, 0x9d);
            this.Lbl_LRWRC.Name = "Lbl_LRWRC";
            this.Lbl_LRWRC.Size = new Size(0x44, 0x11);
            this.Lbl_LRWRC.TabIndex = 0xa3;
            this.Lbl_LRWRC.Text = "容错个数：";
            this.Lbl_LRWExpect.AutoSize = true;
            this.Lbl_LRWExpect.Location = new Point(5, 0x2b);
            this.Lbl_LRWExpect.Name = "Lbl_LRWExpect";
            this.Lbl_LRWExpect.Size = new Size(0x44, 0x11);
            this.Lbl_LRWExpect.TabIndex = 160;
            this.Lbl_LRWExpect.Text = "统计期数：";
            this.Nm_LRWExpect.Location = new Point(80, 40);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.Nm_LRWExpect.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.Nm_LRWExpect.Minimum = new decimal(bits);
            this.Nm_LRWExpect.Name = "Nm_LRWExpect";
            this.Nm_LRWExpect.Size = new Size(0xe5, 0x17);
            this.Nm_LRWExpect.TabIndex = 0xa1;
            bits = new int[4];
            bits[0] = 10;
            this.Nm_LRWExpect.Value = new decimal(bits);
            this.Nm_LRWExpect.ValueChanged += new EventHandler(this.Nm_LRWExpect_ValueChanged);
            this.Lbl_LRWType.AutoSize = true;
            this.Lbl_LRWType.Location = new Point(5, 0x54);
            this.Lbl_LRWType.Name = "Lbl_LRWType";
            this.Lbl_LRWType.Size = new Size(0x44, 0x11);
            this.Lbl_LRWType.TabIndex = 0xa2;
            this.Lbl_LRWType.Text = "出号类型：";
            this.Txt_BetsJK.Enabled = false;
            this.Txt_BetsJK.Location = new Point(0x14e, 0x9e);
            this.Txt_BetsJK.Name = "Txt_BetsJK";
            this.Txt_BetsJK.Size = new Size(0xb6, 0x17);
            this.Txt_BetsJK.TabIndex = 0x13e;
            this.Ckb_BetsJK.AutoSize = true;
            this.Ckb_BetsJK.Location = new Point(0x14e, 0x84);
            this.Ckb_BetsJK.Name = "Ckb_BetsJK";
            this.Ckb_BetsJK.Size = new Size(0x4b, 0x15);
            this.Ckb_BetsJK.TabIndex = 0x13d;
            this.Ckb_BetsJK.Text = "投注监控";
            this.Ckb_BetsJK.UseVisualStyleBackColor = true;
            this.Ckb_BetsJK.CheckedChanged += new EventHandler(this.Ckb_BetsJK_CheckedChanged);
            this.Err_Hint.ContainerControl = this;
            this.Err_Hint.Icon = (Icon)manager.GetObject("Err_Hint.Icon");
            this.Cbb_BetsJKMode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_BetsJKMode.Enabled = false;
            this.Cbb_BetsJKMode.FormattingEnabled = true;
            this.Cbb_BetsJKMode.Items.AddRange(new object[] { "一直监控", "仅开始监控" });
            this.Cbb_BetsJKMode.Location = new Point(0x196, 0xbd);
            this.Cbb_BetsJKMode.Name = "Cbb_BetsJKMode";
            this.Cbb_BetsJKMode.Size = new Size(110, 0x19);
            this.Cbb_BetsJKMode.TabIndex = 320;
            this.Ckb_BetsJKMode.AutoSize = true;
            this.Ckb_BetsJKMode.Enabled = false;
            this.Ckb_BetsJKMode.Location = new Point(0x14c, 0xc0);
            this.Ckb_BetsJKMode.Name = "Ckb_BetsJKMode";
            this.Ckb_BetsJKMode.Size = new Size(0x44, 0x11);
            this.Ckb_BetsJKMode.TabIndex = 0x13f;
            this.Ckb_BetsJKMode.Text = "监控模式：";
            this.Pnl_RX.Controls.Add(this.Ckb_WZ5);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ4);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ3);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ2);
            this.Pnl_RX.Controls.Add(this.Lbl_RXZJ);
            this.Pnl_RX.Controls.Add(this.Txt_RXZJ);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ1);
            this.Pnl_RX.Location = new Point(0x149, 0xe0);
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
            this.Ckb_EditJK.Appearance = Appearance.Button;
            this.Ckb_EditJK.AutoCheck = false;
            this.Ckb_EditJK.FlatAppearance.BorderSize = 0;
            this.Ckb_EditJK.FlatStyle = FlatStyle.Flat;
            this.Ckb_EditJK.Image = Resources.EditHot;
            this.Ckb_EditJK.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_EditJK.Location = new Point(0x1b4, 0x81);
            this.Ckb_EditJK.Name = "Ckb_EditJK";
            this.Ckb_EditJK.Size = new Size(80, 0x19);
            this.Ckb_EditJK.TabIndex = 0x157;
            this.Ckb_EditJK.Text = "快速输入";
            this.Ckb_EditJK.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_EditJK.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_EditJK.UseVisualStyleBackColor = true;
            this.Ckb_EditJK.Click += new EventHandler(this.Ckb_EditJK_Click);
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
            this.Ckb_RefreshSort.Appearance = Appearance.Button;
            this.Ckb_RefreshSort.AutoCheck = false;
            this.Ckb_RefreshSort.FlatAppearance.BorderSize = 0;
            this.Ckb_RefreshSort.FlatStyle = FlatStyle.Flat;
            this.Ckb_RefreshSort.Image = Resources.Refresh;
            this.Ckb_RefreshSort.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_RefreshSort.Location = new Point(250, 4);
            this.Ckb_RefreshSort.Name = "Ckb_RefreshSort";
            this.Ckb_RefreshSort.Size = new Size(60, 0x19);
            this.Ckb_RefreshSort.TabIndex = 0x11d;
            this.Ckb_RefreshSort.Text = "刷新";
            this.Ckb_RefreshSort.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_RefreshSort.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_RefreshSort.UseVisualStyleBackColor = true;
            this.Ckb_RefreshSort.Click += new EventHandler(this.Ckb_RefreshSort_Click);
            this.Ckb_LRWCHQX.Appearance = Appearance.Button;
            this.Ckb_LRWCHQX.AutoCheck = false;
            this.Ckb_LRWCHQX.FlatAppearance.BorderSize = 0;
            this.Ckb_LRWCHQX.FlatStyle = FlatStyle.Flat;
            this.Ckb_LRWCHQX.Location = new Point(0xd7, 0x79);
            this.Ckb_LRWCHQX.Name = "Ckb_LRWCHQX";
            this.Ckb_LRWCHQX.Size = new Size(40, 0x19);
            this.Ckb_LRWCHQX.TabIndex = 0x11e;
            this.Ckb_LRWCHQX.Text = "全选";
            this.Ckb_LRWCHQX.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_LRWCHQX.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_LRWCHQX.UseVisualStyleBackColor = true;
            this.Ckb_LRWCHQX.Click += new EventHandler(this.Ckb_LRWCHQX_Click);
            this.Ckb_LRWCHBX.Appearance = Appearance.Button;
            this.Ckb_LRWCHBX.AutoCheck = false;
            this.Ckb_LRWCHBX.FlatAppearance.BorderSize = 0;
            this.Ckb_LRWCHBX.FlatStyle = FlatStyle.Flat;
            this.Ckb_LRWCHBX.Location = new Point(0x105, 0x79);
            this.Ckb_LRWCHBX.Name = "Ckb_LRWCHBX";
            this.Ckb_LRWCHBX.Size = new Size(40, 0x19);
            this.Ckb_LRWCHBX.TabIndex = 0x11f;
            this.Ckb_LRWCHBX.Text = "不选";
            this.Ckb_LRWCHBX.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_LRWCHBX.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_LRWCHBX.UseVisualStyleBackColor = true;
            this.Ckb_LRWCHBX.Click += new EventHandler(this.Ckb_LRWCHBX_Click);
            this.Ckb_LRWCHRe.Appearance = Appearance.Button;
            this.Ckb_LRWCHRe.AutoCheck = false;
            this.Ckb_LRWCHRe.FlatAppearance.BorderSize = 0;
            this.Ckb_LRWCHRe.FlatStyle = FlatStyle.Flat;
            this.Ckb_LRWCHRe.Location = new Point(0x4f, 0x79);
            this.Ckb_LRWCHRe.Name = "Ckb_LRWCHRe";
            this.Ckb_LRWCHRe.Size = new Size(40, 0x19);
            this.Ckb_LRWCHRe.TabIndex = 0x120;
            this.Ckb_LRWCHRe.Text = "热号";
            this.Ckb_LRWCHRe.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_LRWCHRe.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_LRWCHRe.UseVisualStyleBackColor = true;
            this.Ckb_LRWCHRe.Click += new EventHandler(this.Ckb_LRWCHRe_Click);
            this.Ckb_LRWCHLen.Appearance = Appearance.Button;
            this.Ckb_LRWCHLen.AutoCheck = false;
            this.Ckb_LRWCHLen.FlatAppearance.BorderSize = 0;
            this.Ckb_LRWCHLen.FlatStyle = FlatStyle.Flat;
            this.Ckb_LRWCHLen.Location = new Point(0x7d, 0x79);
            this.Ckb_LRWCHLen.Name = "Ckb_LRWCHLen";
            this.Ckb_LRWCHLen.Size = new Size(40, 0x19);
            this.Ckb_LRWCHLen.TabIndex = 0x121;
            this.Ckb_LRWCHLen.Text = "冷号";
            this.Ckb_LRWCHLen.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_LRWCHLen.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_LRWCHLen.UseVisualStyleBackColor = true;
            this.Ckb_LRWCHLen.Click += new EventHandler(this.Ckb_LRWCHLen_Click);
            this.Ckb_LRWCHWen.Appearance = Appearance.Button;
            this.Ckb_LRWCHWen.AutoCheck = false;
            this.Ckb_LRWCHWen.FlatAppearance.BorderSize = 0;
            this.Ckb_LRWCHWen.FlatStyle = FlatStyle.Flat;
            this.Ckb_LRWCHWen.Location = new Point(0xab, 0x79);
            this.Ckb_LRWCHWen.Name = "Ckb_LRWCHWen";
            this.Ckb_LRWCHWen.Size = new Size(40, 0x19);
            this.Ckb_LRWCHWen.TabIndex = 290;
            this.Ckb_LRWCHWen.Text = "温号";
            this.Ckb_LRWCHWen.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_LRWCHWen.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_LRWCHWen.UseVisualStyleBackColor = true;
            this.Ckb_LRWCHWen.Click += new EventHandler(this.Ckb_LRWCHWen_Click);
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            base.Controls.Add(this.Ckb_More);
            base.Controls.Add(this.Ckb_EditJK);
            base.Controls.Add(this.Pnl_RX);
            base.Controls.Add(this.Cbb_BetsJKMode);
            base.Controls.Add(this.Ckb_BetsJKMode);
            base.Controls.Add(this.Txt_BetsJK);
            base.Controls.Add(this.Ckb_BetsJK);
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
            base.Name = "FNLRWCHLine";
            base.Size = new Size(0x20d, 0x2ad);
            base.Load += new EventHandler(this.FNLRWCHLine_Load);
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
            this.Nm_LRWExpect.EndInit();
            ((ISupportInitialize)this.Err_Hint).EndInit();
            this.Pnl_RX.ResumeLayout(false);
            this.Pnl_RX.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void Nm_LRWExpect_ValueChanged(object sender, EventArgs e)
        {
            this.RefreshSortMain();
        }

        private void Rdb_BTPlan_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_BTPlan.Enabled = this.Ckb_BTCount.Enabled = this.Rdb_BTPlan.Checked;
            this.Cbb_BTFN.Enabled = this.Ckb_BTFN.Enabled = this.Rdb_BTFN.Checked;
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

        private void RefreshSortMain()
        {
            if (this._RunEvent)
            {
                string pError = "";
                this.Txt_LRWDMSort.Text = "";
                ConfigurationStatus.FNLRWCH controlValue = this.GetControlValue(ref pError);
                if (controlValue != null)
                {
                    List<ConfigurationStatus.OpenData> dataList = AppInfo.DataList;
                    List<string> pList = controlValue.RefreshSort(controlValue.LRWPlayIndexList, dataList, 0);
                    this.Txt_LRWDMSort.Text = CommFunc.Join(pList);
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
            List<string> dicKeyList = CommFunc.GetDicKeyList<List<ConfigurationStatus.PlayBase>>(AppInfo.PlayDic);
            dicKeyList.Remove("龙虎");
            CommFunc.SetComboBoxList(this.Cbb_PlayType, dicKeyList);
            CommFunc.SetComboBoxList(this.Cbb_BTFN, AppInfo.BTFNList);
            ConfigurationStatus.FNLRWCH fnlrwch = (ConfigurationStatus.FNLRWCH)pInfo;
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_PlayType, pInfo.PlayType);
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_PlayName, pInfo.PlayName);
            this.Cbb_Unit.SelectedIndex = (int)pInfo.Unit;
            this.Ckb_BetsJK.Checked = pInfo.BetsJKSelect;
            this.Txt_BetsJK.Text = pInfo.BetsJKValue;
            this.Cbb_BetsJKMode.SelectedIndex = (int)pInfo.BetsJKMode;
            this.Txt_RXZJ.Text = fnlrwch.RXZJ;
            this.SetRXWZList(fnlrwch.RXWZList);
            this.Cbb_Mode.SelectedIndex = (int)pInfo.Mode;
            this.Nm_ModeExpect.Value = pInfo.ModeExpect;
            this.Cbb_FBType.SelectedIndex = (int)pInfo.FBInfo;
            this.Rdb_CodeType1.Checked = pInfo.IsBetsZJ;
            this.Rdb_CodeType2.Checked = !pInfo.IsBetsZJ;
            this.Rdb_BTPlan.Checked = pInfo.BTType == ConfigurationStatus.SCTimesType.Plan;
            this.Rdb_BTFN.Checked = pInfo.BTType == ConfigurationStatus.SCTimesType.FN;
            this.Txt_BTPlan.Text = CommFunc.Join(pInfo.BTPlanList, ",");
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_BTFN, pInfo.BTFNName);
            this.IsMore = pInfo.VisibleMore;
            this.Nm_LRWExpect.Value = fnlrwch.LRWExpect;
            this.SetLRWTypeList(fnlrwch.LRWTypeList);
            this.Txt_LRWRC.Text = fnlrwch.LRWRC;
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
            this.RefreshSortMain();
        }

        private void SetLRWTypeList(List<int> pValueList)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.TypeList.Count; i++)
            {
                this.TypeList[i].Checked = pValueList.Contains(i);
            }
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

