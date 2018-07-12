namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.ExDataGridView;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FNGJDMLHLine : UserControl
    {
        private bool _RunEvent = false;
        private BetsTimeLine BT_Time;
        public List<Button> ButtonList = null;
        private ComboBox Cbb_BetsJKMode;
        private ComboBox Cbb_BTFN;
        private ComboBox Cbb_FBType;
        private ComboBox Cbb_PlayName;
        private ComboBox Cbb_PlayType;
        private ComboBox Cbb_Unit;
        private CheckBox Ckb_AddDMLH;
        private CheckBox Ckb_BetsJK;
        private Label Ckb_BetsJKMode;
        private CheckBox Ckb_BTCount;
        private CheckBox Ckb_BTFN;
        private CheckBox Ckb_ClearDMLH;
        private CheckBox Ckb_DeleteDMLH;
        private CheckBox Ckb_EditDMLH;
        private CheckBox Ckb_EditJK;
        private CheckBox Ckb_KSHT;
        private CheckBox Ckb_KSStopBets;
        private CheckBox Ckb_MN1;
        private CheckBox Ckb_MN2;
        private CheckBox Ckb_MN3;
        private CheckBox Ckb_MN4;
        private CheckBox Ckb_More;
        private CheckBox Ckb_WZ1;
        private CheckBox Ckb_WZ2;
        private CheckBox Ckb_WZ3;
        private CheckBox Ckb_WZ4;
        private CheckBox Ckb_WZ5;
        private CheckBox Ckb_YLHT;
        private CheckBox Ckb_YLStopBets;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private List<ConfigurationStatus.GJDMLH> DMLHList = new List<ConfigurationStatus.GJDMLH>();
        private ExpandGirdView Egv_DMLHList;
        private ErrorProvider Err_Hint;
        private bool IsMore = false;
        public List<Label> LabelList = null;
        private Label Lbl_CodeType;
        private Label Lbl_FBType;
        private Label Lbl_KSHT;
        private Label Lbl_MNBets;
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
        private NumericUpDown Nm_YLHT;
        private Panel Pnl_CodeType;
        private Panel Pnl_DMLH2;
        private Panel Pnl_DMLHBottom2;
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
        private TextBox Txt_MN1;
        private TextBox Txt_MN2;
        private TextBox Txt_MN3;
        private TextBox Txt_MN4;
        private TextBox Txt_RXZJ;
        private TextBox Txt_YLHT;
        private TextBox Txt_YLStopBets;

        public FNGJDMLHLine()
        {
            this.InitializeComponent();
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Pnl_DMLHBottom2,
                    this.Pnl_Times
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control>();
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Ckb_EditJK,
                    this.Ckb_AddDMLH,
                    this.Ckb_EditDMLH,
                    this.Ckb_DeleteDMLH,
                    this.Ckb_ClearDMLH,
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
                    this.Lbl_MNBets
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
                    this.Cbb_FBType
                };
                CommFunc.BeautifyComboBox(pComboBoxList);
            }
        }

        private void Cbb_PlayName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshDMLHList();
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
        }

        private void Cbb_PlayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.Cbb_PlayType.Text;
            List<string> playNameList = CommFunc.GetPlayNameList(text);
            CommFunc.SetComboBoxList(this.Cbb_PlayName, playNameList);
            this.Cbb_PlayName.SelectedIndex = 0;
            this.Lbl_RXZJ.Visible = this.Txt_RXZJ.Visible = CommFunc.CheckPlayIsRX(text);
        }

        private void Ckb_AddDMLH_Click(object sender, EventArgs e)
        {
            List<int> pIDList = new List<int>();
            foreach (ConfigurationStatus.GJDMLH gjdmlh in this.DMLHList)
            {
                pIDList.Add(gjdmlh.ID);
            }
            ConfigurationStatus.GJDMLH pInput = new ConfigurationStatus.GJDMLH(this.GetDefultID(pIDList));
            FrmInputCode code = new FrmInputCode(pInput, this.PlayInfo, true, pIDList);
            if (code.ShowDialog() == DialogResult.OK)
            {
                pInput = FrmInputCode.OutValue;
                this.DMLHList.Add(pInput);
                this.SortDMLHList(this.DMLHList);
                this.RefreshDMLHList();
                this.Egv_DMLHList.Rows[this.DMLHList.Count - 1].Selected = true;
            }
        }

        private void Ckb_BetsJK_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_BetsJK.Enabled = this.Ckb_BetsJK.Checked;
            this.Ckb_BetsJKMode.Enabled = this.Cbb_BetsJKMode.Enabled = this.Ckb_BetsJK.Checked;
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

        private void Ckb_ClearDMLH_Click(object sender, EventArgs e)
        {
            this.DMLHList.Clear();
            this.RefreshDMLHList();
        }

        private void Ckb_DeleteDMLH_Click(object sender, EventArgs e)
        {
            int zuCodeIndex = this.GetZuCodeIndex();
            if (zuCodeIndex != -1)
            {
                this.DMLHList.RemoveAt(zuCodeIndex);
                this.RefreshDMLHList();
            }
        }

        private void Ckb_EditDMLH_Click(object sender, EventArgs e)
        {
            int zuCodeIndex = this.GetZuCodeIndex();
            if (zuCodeIndex != -1)
            {
                ConfigurationStatus.GJDMLH pInput = this.DMLHList[zuCodeIndex];
                FrmInputCode code = new FrmInputCode(pInput, this.PlayInfo, false, null);
                if (code.ShowDialog() == DialogResult.OK)
                {
                    pInput = FrmInputCode.OutValue;
                    this.DMLHList[zuCodeIndex] = pInput;
                    this.RefreshDMLHList();
                }
            }
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

        private void Ckb_WZ_CheckedChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshDMLHList();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Egv_DMLHList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.Ckb_EditDMLH_Click(null, null);
        }

        private void Egv_DMLHList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if ((this.Egv_DMLHList.RowCount != 0) && (this.DMLHList.Count != 0))
            {
                ConfigurationStatus.GJDMLH gjdmlh = this.DMLHList[e.RowIndex];
                if (e.ColumnIndex == 0)
                {
                    e.Value = gjdmlh.ID;
                }
                else if (e.ColumnIndex == 1)
                {
                    e.Value = gjdmlh.Value;
                }
                else if (e.ColumnIndex == 2)
                {
                    e.Value = CommFunc.GetBetsCodeCount(gjdmlh.NumberList, this.PlayInfo.Play, this.GetRXWZList());
                }
                else if (e.ColumnIndex == 3)
                {
                    e.Value = gjdmlh.YesAfter.ToString();
                }
                else if (e.ColumnIndex == 4)
                {
                    e.Value = gjdmlh.NoAfter.ToString();
                }
            }
        }

        private void FNGJDMLHLine_Load(object sender, EventArgs e)
        {
            List<Label> list2 = new List<Label> {
                this.Lbl_Name
            };
            this.LabelList = list2;
            CommFunc.SetLabelFormat(this.LabelList);
            List<Control> list3 = new List<Control> {
                this.Pnl_More
            };
            this.MoreControl = list3;
            this.Ckb_More.ForeColor = AppInfo.appForeColor;
            this.Lbl_MoreHint.ForeColor = AppInfo.redForeColor;
            CommFunc.SetControlHint(this.Err_Hint, this.Ckb_BetsJK, "当方案历史中挂情况满足设定条件时开始投注（0表示挂，1表示中），例如设置000就表示连挂3期后开始投注。");
            List<CheckBox> list4 = new List<CheckBox> {
                this.Ckb_WZ1,
                this.Ckb_WZ2,
                this.Ckb_WZ3,
                this.Ckb_WZ4,
                this.Ckb_WZ5
            };
            this.RXWZList = list4;
            this.LoadDMLHList();
            List<CheckBox> pCheckBoxList = new List<CheckBox> {
                this.Ckb_EditJK,
                this.Ckb_AddDMLH,
                this.Ckb_EditDMLH,
                this.Ckb_DeleteDMLH,
                this.Ckb_ClearDMLH,
                this.Ckb_BTCount,
                this.Ckb_BTFN,
                this.Ckb_More
            };
            CommFunc.SetCheckBoxFormatFlat(pCheckBoxList);
            this.BeautifyInterface();
            CommFunc.AppHandler(this.Cbb_Unit);
            this.Ckb_MN1.Visible = this.Txt_MN1.Visible = this.Ckb_MN2.Visible = this.Txt_MN2.Visible = this.Ckb_MN3.Visible = this.Txt_MN3.Visible = this.Ckb_MN4.Visible = this.Txt_MN4.Visible = this.Lbl_MNBets.Visible = !AppInfo.Account.Configuration.IsHideMNBets;
            this._RunEvent = true;
        }

        public ConfigurationStatus.FNGJDMLH GetControlValue(ref string pError)
        {
            ConfigurationStatus.FNBase pInfo = new ConfigurationStatus.FNGJDMLH();
            this.BT_Time.GetControlValue(ref pInfo);
            ConfigurationStatus.FNGJDMLH fngjdmlh = (ConfigurationStatus.FNGJDMLH) pInfo;
            fngjdmlh.PlayType = this.Cbb_PlayType.Text;
            fngjdmlh.PlayName = this.Cbb_PlayName.Text;
            fngjdmlh.Unit = this.GetUnit();
            if (this.DMLHList.Count == 0)
            {
                pError = "至少要有一组投注号码！";
                return null;
            }
            this.SortDMLHList(this.DMLHList);
            for (int i = 1; i <= this.DMLHList.Count; i++)
            {
                if (this.DMLHList[i - 1].ID != i)
                {
                    pError = $"组号列表不完整！缺少局数【{i}】";
                    return null;
                }
            }
            List<int> list = new List<int>();
            foreach (ConfigurationStatus.GJDMLH gjdmlh in this.DMLHList)
            {
                list.Add(gjdmlh.ID);
            }
            foreach (ConfigurationStatus.GJDMLH gjdmlh in this.DMLHList)
            {
                if (!(list.Contains(gjdmlh.YesAfter) && list.Contains(gjdmlh.NoAfter)))
                {
                    pError = $"组号列表中第【{gjdmlh.ID}】局设置不正确！";
                    return null;
                }
            }
            fngjdmlh.DMLHValueList.Clear();
            foreach (ConfigurationStatus.GJDMLH gjdmlh in this.DMLHList)
            {
                ConfigurationStatus.GJDMLH item = new ConfigurationStatus.GJDMLH(gjdmlh);
                fngjdmlh.DMLHValueList.Add(item);
            }
            fngjdmlh.BetsJKSelect = this.Ckb_BetsJK.Checked;
            fngjdmlh.BetsJKValue = this.Txt_BetsJK.Text;
            if (fngjdmlh.BetsJKSelect && (fngjdmlh.BetsJKValue == ""))
            {
                pError = "至少要输入一个监控形态！";
                this.Txt_BetsJK.Focus();
                return null;
            }
            if (!CommFunc.CheckStringIsRange(fngjdmlh.BetsJKValue, "01"))
            {
                pError = "监控形态只能输入0或1！";
                this.Txt_BetsJK.Focus();
                return null;
            }
            if (fngjdmlh.BetsJKSelect && (fngjdmlh.DMLHValueList.Count > 1))
            {
                pError = "启动监控形态后只能支持一组号码！";
                return null;
            }
            fngjdmlh.BetsJKMode = this.GetJKMode();
            fngjdmlh.RXZJ = this.Txt_RXZJ.Text;
            fngjdmlh.RXWZList = this.GetRXWZList();
            if (CommFunc.CheckPlayIsRXDS(this.Play) && (fngjdmlh.RXWZList.Count < this.PlayInfo.IndexList.Count))
            {
                pError = $"【{this.Play}】至少要选择{this.PlayInfo.IndexList.Count}个位置！";
            }
            fngjdmlh.FBInfo = this.GetFBType();
            fngjdmlh.IsBetsZJ = this.Rdb_CodeType1.Checked;
            fngjdmlh.BTType = this.GetTimesType();
            fngjdmlh.BTPlanList = CommFunc.SplitString(this.Txt_BTPlan.Text.Replace("，", ","), ",", -1);
            if (this.Rdb_BTPlan.Checked && (fngjdmlh.BTPlanList.Count == 0))
            {
                pError = "至少要输入一个直线倍投！";
                this.Txt_BTPlan.Focus();
                return null;
            }
            foreach (string str in fngjdmlh.BTPlanList)
            {
                if (!CommFunc.CheckBetsTimes(str, ref pError))
                {
                    this.Txt_BTPlan.SelectAll();
                    this.Txt_BTPlan.Focus();
                    return null;
                }
            }
            fngjdmlh.BTFNName = this.Cbb_BTFN.Text;
            fngjdmlh.VisibleMore = this.IsMore;
            fngjdmlh.ZSBetsSelect1 = this.Ckb_MN1.Checked;
            fngjdmlh.ZSBetsSelect2 = this.Ckb_MN3.Checked;
            fngjdmlh.MNBetsSelect1 = this.Ckb_MN2.Checked;
            fngjdmlh.MNBetsSelect2 = this.Ckb_MN4.Checked;
            fngjdmlh.ZSBetsMoney1 = this.Txt_MN1.Text;
            fngjdmlh.ZSBetsMoney2 = this.Txt_MN3.Text;
            fngjdmlh.MNBetsMoney1 = this.Txt_MN2.Text;
            fngjdmlh.MNBetsMoney2 = this.Txt_MN4.Text;
            pInfo.YLHTSelect = this.Ckb_YLHT.Checked;
            pInfo.KSHTSelect = this.Ckb_KSHT.Checked;
            pInfo.YLHTMoney = this.Txt_YLHT.Text;
            pInfo.KSHTMoney = this.Txt_KSHT.Text;
            pInfo.YLHTID = Convert.ToInt32(this.Nm_YLHT.Value);
            pInfo.KSHTID = Convert.ToInt32(this.Nm_KSHT.Value);
            fngjdmlh.YLStopSelect = this.Ckb_YLStopBets.Checked;
            fngjdmlh.KSStopSelect = this.Ckb_KSStopBets.Checked;
            fngjdmlh.YLStopMoney = this.Txt_YLStopBets.Text;
            fngjdmlh.KSStopMoney = this.Txt_KSStopBets.Text;
            return fngjdmlh;
        }

        public int GetDefultID(List<int> pIDList)
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

        private ConfigurationStatus.FBType GetFBType() => 
            ((ConfigurationStatus.FBType) this.Cbb_FBType.SelectedIndex);

        private ConfigurationStatus.JKMode GetJKMode() => 
            ((ConfigurationStatus.JKMode) this.Cbb_BetsJKMode.SelectedIndex);

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
            ((ConfigurationStatus.SCUnitType) this.Cbb_Unit.SelectedIndex);

        private int GetZuCodeIndex()
        {
            int index = -1;
            if (this.Egv_DMLHList.SelectedRows.Count != 0)
            {
                index = this.Egv_DMLHList.SelectedRows[0].Index;
            }
            return index;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FNGJDMLHLine));
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            this.Lbl_Name = new Label();
            this.Txt_BTPlan = new TextBox();
            this.Lbl_FBType = new Label();
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
            this.Lbl_MoreHint = new Label();
            this.Ckb_BetsJK = new CheckBox();
            this.Txt_BetsJK = new TextBox();
            this.Err_Hint = new ErrorProvider(this.components);
            this.Ckb_BetsJKMode = new Label();
            this.Cbb_BetsJKMode = new ComboBox();
            this.Pnl_DMLH2 = new Panel();
            this.Egv_DMLHList = new ExpandGirdView(this.components);
            this.Pnl_DMLHBottom2 = new Panel();
            this.Ckb_AddDMLH = new CheckBox();
            this.Ckb_EditDMLH = new CheckBox();
            this.Ckb_DeleteDMLH = new CheckBox();
            this.Ckb_ClearDMLH = new CheckBox();
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
            this.Pnl_CodeType.SuspendLayout();
            this.Pnl_Times.SuspendLayout();
            this.Pnl_More.SuspendLayout();
            this.Nm_KSHT.BeginInit();
            this.Nm_YLHT.BeginInit();
            ((ISupportInitialize) this.Err_Hint).BeginInit();
            this.Pnl_DMLH2.SuspendLayout();
            ((ISupportInitialize) this.Egv_DMLHList).BeginInit();
            this.Pnl_DMLHBottom2.SuspendLayout();
            this.Pnl_RX.SuspendLayout();
            base.SuspendLayout();
            this.Lbl_Name.AutoSize = true;
            this.Lbl_Name.Location = new Point(3, 7);
            this.Lbl_Name.Name = "Lbl_Name";
            this.Lbl_Name.Size = new Size(80, 0x11);
            this.Lbl_Name.TabIndex = 0;
            this.Lbl_Name.Text = "高级定码轮换";
            this.Txt_BTPlan.Location = new Point(0x5d, 5);
            this.Txt_BTPlan.Multiline = true;
            this.Txt_BTPlan.Name = "Txt_BTPlan";
            this.Txt_BTPlan.ScrollBars = ScrollBars.Vertical;
            this.Txt_BTPlan.Size = new Size(0xe9, 0x4b);
            this.Txt_BTPlan.TabIndex = 0x1a;
            this.Txt_BTPlan.Text = "1,2,3";
            this.Lbl_FBType.AutoSize = true;
            this.Lbl_FBType.Location = new Point(3, 8);
            this.Lbl_FBType.Name = "Lbl_FBType";
            this.Lbl_FBType.Size = new Size(0x44, 0x11);
            this.Lbl_FBType.TabIndex = 30;
            this.Lbl_FBType.Text = "翻倍方式：";
            this.Cbb_FBType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_FBType.FormattingEnabled = true;
            this.Cbb_FBType.Items.AddRange(new object[] { "挂翻倍", "中翻倍" });
            this.Cbb_FBType.Location = new Point(0x4d, 5);
            this.Cbb_FBType.Name = "Cbb_FBType";
            this.Cbb_FBType.Size = new Size(110, 0x19);
            this.Cbb_FBType.TabIndex = 0xf7;
            this.Lbl_MNBets.AutoSize = true;
            this.Lbl_MNBets.Location = new Point(0xa1, 160);
            this.Lbl_MNBets.Name = "Lbl_MNBets";
            this.Lbl_MNBets.Size = new Size(0x38, 0x11);
            this.Lbl_MNBets.TabIndex = 0x101;
            this.Lbl_MNBets.Text = "循环切换";
            this.Txt_MN4.Enabled = false;
            this.Txt_MN4.Location = new Point(0x139, 0xa8);
            this.Txt_MN4.Name = "Txt_MN4";
            this.Txt_MN4.Size = new Size(60, 0x17);
            this.Txt_MN4.TabIndex = 0x100;
            this.Txt_MN4.Text = "50000";
            this.Ckb_MN4.AutoSize = true;
            this.Ckb_MN4.Location = new Point(0xde, 170);
            this.Ckb_MN4.Name = "Ckb_MN4";
            this.Ckb_MN4.Size = new Size(0x57, 0x15);
            this.Ckb_MN4.TabIndex = 0xff;
            this.Ckb_MN4.Text = "真实投注输";
            this.Ckb_MN4.UseVisualStyleBackColor = true;
            this.Ckb_MN4.CheckedChanged += new EventHandler(this.Ckb_MN4_CheckedChanged);
            this.Txt_MN3.Enabled = false;
            this.Txt_MN3.Location = new Point(0x61, 0xa8);
            this.Txt_MN3.Name = "Txt_MN3";
            this.Txt_MN3.Size = new Size(60, 0x17);
            this.Txt_MN3.TabIndex = 0xfe;
            this.Txt_MN3.Text = "50000";
            this.Ckb_MN3.AutoSize = true;
            this.Ckb_MN3.Location = new Point(6, 170);
            this.Ckb_MN3.Name = "Ckb_MN3";
            this.Ckb_MN3.Size = new Size(0x57, 0x15);
            this.Ckb_MN3.TabIndex = 0xfd;
            this.Ckb_MN3.Text = "模拟投注赢";
            this.Ckb_MN3.UseVisualStyleBackColor = true;
            this.Ckb_MN3.CheckedChanged += new EventHandler(this.Ckb_MN3_CheckedChanged);
            this.Txt_MN2.Enabled = false;
            this.Txt_MN2.Location = new Point(0x139, 0x8f);
            this.Txt_MN2.Name = "Txt_MN2";
            this.Txt_MN2.Size = new Size(60, 0x17);
            this.Txt_MN2.TabIndex = 0xfc;
            this.Txt_MN2.Text = "50000";
            this.Ckb_MN2.AutoSize = true;
            this.Ckb_MN2.Location = new Point(0xde, 0x91);
            this.Ckb_MN2.Name = "Ckb_MN2";
            this.Ckb_MN2.Size = new Size(0x57, 0x15);
            this.Ckb_MN2.TabIndex = 0xfb;
            this.Ckb_MN2.Text = "真实投注赢";
            this.Ckb_MN2.UseVisualStyleBackColor = true;
            this.Ckb_MN2.CheckedChanged += new EventHandler(this.Ckb_MN2_CheckedChanged);
            this.Txt_MN1.Enabled = false;
            this.Txt_MN1.Location = new Point(0x61, 0x8f);
            this.Txt_MN1.Name = "Txt_MN1";
            this.Txt_MN1.Size = new Size(60, 0x17);
            this.Txt_MN1.TabIndex = 250;
            this.Txt_MN1.Text = "50000";
            this.Ckb_MN1.AutoSize = true;
            this.Ckb_MN1.Location = new Point(6, 0x91);
            this.Ckb_MN1.Name = "Ckb_MN1";
            this.Ckb_MN1.Size = new Size(0x57, 0x15);
            this.Ckb_MN1.TabIndex = 0xf9;
            this.Ckb_MN1.Text = "模拟投注输";
            this.Ckb_MN1.UseVisualStyleBackColor = true;
            this.Ckb_MN1.CheckedChanged += new EventHandler(this.Ckb_MN1_CheckedChanged);
            this.Txt_YLStopBets.Enabled = false;
            this.Txt_YLStopBets.Location = new Point(0x9b, 0x6f);
            this.Txt_YLStopBets.Name = "Txt_YLStopBets";
            this.Txt_YLStopBets.Size = new Size(60, 0x17);
            this.Txt_YLStopBets.TabIndex = 0x11f;
            this.Txt_YLStopBets.Text = "50000";
            this.Lbl_StopBets.AutoSize = true;
            this.Lbl_StopBets.Location = new Point(3, 0x71);
            this.Lbl_StopBets.Name = "Lbl_StopBets";
            this.Lbl_StopBets.Size = new Size(0x44, 0x11);
            this.Lbl_StopBets.TabIndex = 0x11c;
            this.Lbl_StopBets.Text = "止损盈亏：";
            this.Ckb_YLStopBets.AutoSize = true;
            this.Ckb_YLStopBets.Location = new Point(0x4b, 0x70);
            this.Ckb_YLStopBets.Name = "Ckb_YLStopBets";
            this.Ckb_YLStopBets.Size = new Size(0x4b, 0x15);
            this.Ckb_YLStopBets.TabIndex = 0x125;
            this.Ckb_YLStopBets.Text = "盈利大于";
            this.Ckb_YLStopBets.UseVisualStyleBackColor = true;
            this.Ckb_YLStopBets.CheckedChanged += new EventHandler(this.Ckb_YLStopBets_CheckedChanged);
            this.Ckb_KSStopBets.AutoSize = true;
            this.Ckb_KSStopBets.Location = new Point(0xe2, 0x70);
            this.Ckb_KSStopBets.Name = "Ckb_KSStopBets";
            this.Ckb_KSStopBets.Size = new Size(0x4b, 0x15);
            this.Ckb_KSStopBets.TabIndex = 0x127;
            this.Ckb_KSStopBets.Text = "亏损大于";
            this.Ckb_KSStopBets.UseVisualStyleBackColor = true;
            this.Ckb_KSStopBets.CheckedChanged += new EventHandler(this.Ckb_KSStopBets_CheckedChanged);
            this.Txt_KSStopBets.Enabled = false;
            this.Txt_KSStopBets.Location = new Point(0x132, 0x6f);
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
            this.Lbl_CodeType.Location = new Point(3, 0x2b);
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
            this.Pnl_CodeType.Location = new Point(0x48, 0x27);
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
            this.Ckb_BTCount.TabIndex = 0x153;
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
            this.Ckb_BTFN.TabIndex = 0x152;
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
            this.Pnl_More.Controls.Add(this.Lbl_FBType);
            this.Pnl_More.Controls.Add(this.Cbb_FBType);
            this.Pnl_More.Location = new Point(0, 0x1a1);
            this.Pnl_More.Name = "Pnl_More";
            this.Pnl_More.Size = new Size(0x20d, 0xeb);
            this.Pnl_More.TabIndex = 0x135;
            this.Pnl_More.Visible = false;
            this.Nm_KSHT.Enabled = false;
            this.Nm_KSHT.Location = new Point(0x1c9, 0x4e);
            this.Nm_KSHT.Name = "Nm_KSHT";
            this.Nm_KSHT.Size = new Size(50, 0x17);
            this.Nm_KSHT.TabIndex = 0x150;
            int[] bits = new int[4];
            bits[0] = 1;
            this.Nm_KSHT.Value = new decimal(bits);
            this.Lbl_KSHT.AutoSize = true;
            this.Lbl_KSHT.Enabled = false;
            this.Lbl_KSHT.Location = new Point(0x199, 80);
            this.Lbl_KSHT.Name = "Lbl_KSHT";
            this.Lbl_KSHT.Size = new Size(0x2c, 0x11);
            this.Lbl_KSHT.TabIndex = 0x14f;
            this.Lbl_KSHT.Text = "跳局数";
            this.Txt_KSHT.Enabled = false;
            this.Txt_KSHT.Location = new Point(0x163, 0x4d);
            this.Txt_KSHT.Name = "Txt_KSHT";
            this.Txt_KSHT.Size = new Size(50, 0x17);
            this.Txt_KSHT.TabIndex = 0x14d;
            this.Txt_KSHT.Text = "50000";
            this.Ckb_KSHT.AutoSize = true;
            this.Ckb_KSHT.Location = new Point(300, 0x4e);
            this.Ckb_KSHT.Name = "Ckb_KSHT";
            this.Ckb_KSHT.Size = new Size(0x33, 0x15);
            this.Ckb_KSHT.TabIndex = 0x14e;
            this.Ckb_KSHT.Text = "亏损";
            this.Ckb_KSHT.UseVisualStyleBackColor = true;
            this.Ckb_KSHT.CheckedChanged += new EventHandler(this.Ckb_KSHT_CheckedChanged);
            this.Nm_YLHT.Enabled = false;
            this.Nm_YLHT.Location = new Point(0xe8, 0x4d);
            this.Nm_YLHT.Name = "Nm_YLHT";
            this.Nm_YLHT.Size = new Size(50, 0x17);
            this.Nm_YLHT.TabIndex = 0x14c;
            bits = new int[4];
            bits[0] = 1;
            this.Nm_YLHT.Value = new decimal(bits);
            this.Lbl_YLHT.AutoSize = true;
            this.Lbl_YLHT.Enabled = false;
            this.Lbl_YLHT.Location = new Point(0xb8, 0x4f);
            this.Lbl_YLHT.Name = "Lbl_YLHT";
            this.Lbl_YLHT.Size = new Size(0x2c, 0x11);
            this.Lbl_YLHT.TabIndex = 0x14b;
            this.Lbl_YLHT.Text = "跳局数";
            this.Lbl_YKHT.AutoSize = true;
            this.Lbl_YKHT.Location = new Point(3, 0x4e);
            this.Lbl_YKHT.Name = "Lbl_YKHT";
            this.Lbl_YKHT.Size = new Size(0x44, 0x11);
            this.Lbl_YKHT.TabIndex = 0x148;
            this.Lbl_YKHT.Text = "盈亏跳转：";
            this.Txt_YLHT.Enabled = false;
            this.Txt_YLHT.Location = new Point(130, 0x4c);
            this.Txt_YLHT.Name = "Txt_YLHT";
            this.Txt_YLHT.Size = new Size(50, 0x17);
            this.Txt_YLHT.TabIndex = 0x149;
            this.Txt_YLHT.Text = "50000";
            this.Ckb_YLHT.AutoSize = true;
            this.Ckb_YLHT.Location = new Point(0x4b, 0x4d);
            this.Ckb_YLHT.Name = "Ckb_YLHT";
            this.Ckb_YLHT.Size = new Size(0x33, 0x15);
            this.Ckb_YLHT.TabIndex = 330;
            this.Ckb_YLHT.Text = "盈利";
            this.Ckb_YLHT.UseVisualStyleBackColor = true;
            this.Ckb_YLHT.CheckedChanged += new EventHandler(this.Ckb_YLHT_CheckedChanged);
            this.BT_Time.Font = new Font("微软雅黑", 9f);
            this.BT_Time.Location = new Point(1, 0xc1);
            this.BT_Time.Margin = new Padding(3, 4, 3, 4);
            this.BT_Time.Name = "BT_Time";
            this.BT_Time.Size = new Size(520, 0x23);
            this.BT_Time.TabIndex = 0x13c;
            this.Lbl_MoreHint.AutoSize = true;
            this.Lbl_MoreHint.Font = new Font("微软雅黑", 9f, FontStyle.Bold);
            this.Lbl_MoreHint.Location = new Point(0x58, 0x185);
            this.Lbl_MoreHint.Name = "Lbl_MoreHint";
            this.Lbl_MoreHint.Size = new Size(140, 0x11);
            this.Lbl_MoreHint.TabIndex = 0x13a;
            this.Lbl_MoreHint.Text = "【设置更多参数请点击】\r\n";
            this.Ckb_BetsJK.AutoSize = true;
            this.Ckb_BetsJK.Location = new Point(0x14e, 0x84);
            this.Ckb_BetsJK.Name = "Ckb_BetsJK";
            this.Ckb_BetsJK.Size = new Size(0x4b, 0x15);
            this.Ckb_BetsJK.TabIndex = 0x13b;
            this.Ckb_BetsJK.Text = "投注监控";
            this.Ckb_BetsJK.UseVisualStyleBackColor = true;
            this.Ckb_BetsJK.CheckedChanged += new EventHandler(this.Ckb_BetsJK_CheckedChanged);
            this.Txt_BetsJK.Enabled = false;
            this.Txt_BetsJK.Location = new Point(0x14e, 0x9e);
            this.Txt_BetsJK.Name = "Txt_BetsJK";
            this.Txt_BetsJK.Size = new Size(0xb6, 0x17);
            this.Txt_BetsJK.TabIndex = 0x13c;
            this.Err_Hint.ContainerControl = this;
            this.Err_Hint.Icon = (Icon) manager.GetObject("Err_Hint.Icon");
            this.Ckb_BetsJKMode.AutoSize = true;
            this.Ckb_BetsJKMode.Enabled = false;
            this.Ckb_BetsJKMode.Location = new Point(0x14c, 0xc0);
            this.Ckb_BetsJKMode.Name = "Ckb_BetsJKMode";
            this.Ckb_BetsJKMode.Size = new Size(0x44, 0x11);
            this.Ckb_BetsJKMode.TabIndex = 0x13d;
            this.Ckb_BetsJKMode.Text = "监控模式：";
            this.Cbb_BetsJKMode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_BetsJKMode.Enabled = false;
            this.Cbb_BetsJKMode.FormattingEnabled = true;
            this.Cbb_BetsJKMode.Items.AddRange(new object[] { "一直监控", "仅开始监控" });
            this.Cbb_BetsJKMode.Location = new Point(0x196, 0xbd);
            this.Cbb_BetsJKMode.Name = "Cbb_BetsJKMode";
            this.Cbb_BetsJKMode.Size = new Size(110, 0x19);
            this.Cbb_BetsJKMode.TabIndex = 0x13e;
            this.Pnl_DMLH2.Controls.Add(this.Egv_DMLHList);
            this.Pnl_DMLH2.Controls.Add(this.Pnl_DMLHBottom2);
            this.Pnl_DMLH2.Location = new Point(6, 0x1a);
            this.Pnl_DMLH2.Name = "Pnl_DMLH2";
            this.Pnl_DMLH2.Size = new Size(320, 0x106);
            this.Pnl_DMLH2.TabIndex = 0x13f;
            this.Egv_DMLHList.AllowUserToAddRows = false;
            this.Egv_DMLHList.AllowUserToDeleteRows = false;
            this.Egv_DMLHList.AllowUserToResizeColumns = false;
            this.Egv_DMLHList.AllowUserToResizeRows = false;
            this.Egv_DMLHList.BackgroundColor = SystemColors.Control;
            this.Egv_DMLHList.BorderStyle = BorderStyle.Fixed3D;
            this.Egv_DMLHList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Window;
            style.Font = new Font("微软雅黑", 9f);
            style.ForeColor = SystemColors.ControlText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.False;
            this.Egv_DMLHList.ColumnHeadersDefaultCellStyle = style;
            this.Egv_DMLHList.ColumnHeadersHeight = 30;
            this.Egv_DMLHList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            style2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style2.BackColor = SystemColors.Control;
            style2.Font = new Font("微软雅黑", 9f);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = Color.SteelBlue;
            style2.SelectionForeColor = Color.White;
            style2.WrapMode = DataGridViewTriState.False;
            this.Egv_DMLHList.DefaultCellStyle = style2;
            this.Egv_DMLHList.Dock = DockStyle.Fill;
            this.Egv_DMLHList.DragLineColor = Color.Silver;
            this.Egv_DMLHList.ExternalVirtualMode = true;
            this.Egv_DMLHList.GridColor = Color.Silver;
            this.Egv_DMLHList.HeadersCheckDefult = CheckState.Checked;
            this.Egv_DMLHList.Location = new Point(0, 0);
            this.Egv_DMLHList.MergeColumnHeaderBackColor = SystemColors.Control;
            this.Egv_DMLHList.MultiSelect = false;
            this.Egv_DMLHList.Name = "Egv_DMLHList";
            this.Egv_DMLHList.RowHeadersVisible = false;
            this.Egv_DMLHList.RowNum = 0x11;
            style3.BackColor = SystemColors.Control;
            style3.SelectionBackColor = Color.FromArgb(0xde, 0xe8, 0xfc);
            style3.SelectionForeColor = Color.Black;
            this.Egv_DMLHList.RowsDefaultCellStyle = style3;
            this.Egv_DMLHList.RowTemplate.Height = 0x17;
            this.Egv_DMLHList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Egv_DMLHList.Size = new Size(320, 0xe3);
            this.Egv_DMLHList.TabIndex = 0x4c;
            this.Egv_DMLHList.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.Egv_DMLHList_CellMouseDoubleClick);
            this.Pnl_DMLHBottom2.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_DMLHBottom2.Controls.Add(this.Ckb_AddDMLH);
            this.Pnl_DMLHBottom2.Controls.Add(this.Ckb_EditDMLH);
            this.Pnl_DMLHBottom2.Controls.Add(this.Ckb_DeleteDMLH);
            this.Pnl_DMLHBottom2.Controls.Add(this.Ckb_ClearDMLH);
            this.Pnl_DMLHBottom2.Dock = DockStyle.Bottom;
            this.Pnl_DMLHBottom2.Location = new Point(0, 0xe3);
            this.Pnl_DMLHBottom2.Name = "Pnl_DMLHBottom2";
            this.Pnl_DMLHBottom2.Size = new Size(320, 0x23);
            this.Pnl_DMLHBottom2.TabIndex = 0x4b;
            this.Ckb_AddDMLH.Appearance = Appearance.Button;
            this.Ckb_AddDMLH.AutoCheck = false;
            this.Ckb_AddDMLH.FlatAppearance.BorderSize = 0;
            this.Ckb_AddDMLH.FlatStyle = FlatStyle.Flat;
            this.Ckb_AddDMLH.Image = Resources.Add;
            this.Ckb_AddDMLH.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_AddDMLH.Location = new Point(4, 5);
            this.Ckb_AddDMLH.Name = "Ckb_AddDMLH";
            this.Ckb_AddDMLH.Size = new Size(60, 0x19);
            this.Ckb_AddDMLH.TabIndex = 0x14f;
            this.Ckb_AddDMLH.Text = "添加";
            this.Ckb_AddDMLH.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_AddDMLH.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_AddDMLH.UseVisualStyleBackColor = true;
            this.Ckb_AddDMLH.Click += new EventHandler(this.Ckb_AddDMLH_Click);
            this.Ckb_EditDMLH.Appearance = Appearance.Button;
            this.Ckb_EditDMLH.AutoCheck = false;
            this.Ckb_EditDMLH.FlatAppearance.BorderSize = 0;
            this.Ckb_EditDMLH.FlatStyle = FlatStyle.Flat;
            this.Ckb_EditDMLH.Image = Resources.EditHot;
            this.Ckb_EditDMLH.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_EditDMLH.Location = new Point(70, 5);
            this.Ckb_EditDMLH.Name = "Ckb_EditDMLH";
            this.Ckb_EditDMLH.Size = new Size(60, 0x19);
            this.Ckb_EditDMLH.TabIndex = 0x150;
            this.Ckb_EditDMLH.Text = "修改";
            this.Ckb_EditDMLH.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_EditDMLH.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_EditDMLH.UseVisualStyleBackColor = true;
            this.Ckb_EditDMLH.Click += new EventHandler(this.Ckb_EditDMLH_Click);
            this.Ckb_DeleteDMLH.Appearance = Appearance.Button;
            this.Ckb_DeleteDMLH.AutoCheck = false;
            this.Ckb_DeleteDMLH.FlatAppearance.BorderSize = 0;
            this.Ckb_DeleteDMLH.FlatStyle = FlatStyle.Flat;
            this.Ckb_DeleteDMLH.Image = Resources.Remove;
            this.Ckb_DeleteDMLH.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_DeleteDMLH.Location = new Point(0x88, 5);
            this.Ckb_DeleteDMLH.Name = "Ckb_DeleteDMLH";
            this.Ckb_DeleteDMLH.Size = new Size(60, 0x19);
            this.Ckb_DeleteDMLH.TabIndex = 0x151;
            this.Ckb_DeleteDMLH.Text = "删除";
            this.Ckb_DeleteDMLH.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_DeleteDMLH.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_DeleteDMLH.UseVisualStyleBackColor = true;
            this.Ckb_DeleteDMLH.Click += new EventHandler(this.Ckb_DeleteDMLH_Click);
            this.Ckb_ClearDMLH.Appearance = Appearance.Button;
            this.Ckb_ClearDMLH.AutoCheck = false;
            this.Ckb_ClearDMLH.FlatAppearance.BorderSize = 0;
            this.Ckb_ClearDMLH.FlatStyle = FlatStyle.Flat;
            this.Ckb_ClearDMLH.Image = Resources.ClearAll;
            this.Ckb_ClearDMLH.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_ClearDMLH.Location = new Point(0xff, 5);
            this.Ckb_ClearDMLH.Name = "Ckb_ClearDMLH";
            this.Ckb_ClearDMLH.Size = new Size(60, 0x19);
            this.Ckb_ClearDMLH.TabIndex = 0x152;
            this.Ckb_ClearDMLH.Text = "清空";
            this.Ckb_ClearDMLH.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_ClearDMLH.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_ClearDMLH.UseVisualStyleBackColor = true;
            this.Ckb_ClearDMLH.Click += new EventHandler(this.Ckb_ClearDMLH_Click);
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
            this.Pnl_RX.TabIndex = 0x14d;
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
            this.Ckb_EditJK.TabIndex = 0x151;
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
            this.Ckb_More.TabIndex = 0x153;
            this.Ckb_More.Text = "更多设置";
            this.Ckb_More.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_More.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_More.UseVisualStyleBackColor = true;
            this.Ckb_More.Click += new EventHandler(this.Ckb_More_Click);
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            base.Controls.Add(this.Ckb_More);
            base.Controls.Add(this.Ckb_EditJK);
            base.Controls.Add(this.Pnl_RX);
            base.Controls.Add(this.Pnl_DMLH2);
            base.Controls.Add(this.Cbb_BetsJKMode);
            base.Controls.Add(this.Ckb_BetsJKMode);
            base.Controls.Add(this.Txt_BetsJK);
            base.Controls.Add(this.Ckb_BetsJK);
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
            base.Name = "FNGJDMLHLine";
            base.Size = new Size(0x20d, 0x28f);
            base.Load += new EventHandler(this.FNGJDMLHLine_Load);
            this.Pnl_CodeType.ResumeLayout(false);
            this.Pnl_CodeType.PerformLayout();
            this.Pnl_Times.ResumeLayout(false);
            this.Pnl_Times.PerformLayout();
            this.Pnl_More.ResumeLayout(false);
            this.Pnl_More.PerformLayout();
            this.Nm_KSHT.EndInit();
            this.Nm_YLHT.EndInit();
            ((ISupportInitialize) this.Err_Hint).EndInit();
            this.Pnl_DMLH2.ResumeLayout(false);
            ((ISupportInitialize) this.Egv_DMLHList).EndInit();
            this.Pnl_DMLHBottom2.ResumeLayout(false);
            this.Pnl_RX.ResumeLayout(false);
            this.Pnl_RX.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadDMLHList()
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
                "号码",
                "注数",
                "中后",
                "挂后"
            };
            List<int> pWidth = new List<int> { 
                40,
                10,
                40,
                40,
                40
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
                true
            };
            this.Egv_DMLHList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_DMLHList.MultiSelect = false;
            this.Egv_DMLHList.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_DMLHList, 9);
            this.Egv_DMLHList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Egv_DMLHList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_DMLHList_CellValueNeeded);
            for (int i = 0; i < this.Egv_DMLHList.ColumnCount; i++)
            {
                this.Egv_DMLHList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void Rdb_BTPlan_CheckedChanged(object sender, EventArgs e)
        {
            this.Txt_BTPlan.Enabled = this.Ckb_BTCount.Enabled = this.Rdb_BTPlan.Checked;
            this.Cbb_BTFN.Enabled = this.Ckb_BTFN.Enabled = this.Rdb_BTFN.Checked;
        }

        private void RefreshDMLHList()
        {
            string pErrorHint = "";
            foreach (ConfigurationStatus.GJDMLH gjdmlh in this.DMLHList)
            {
                gjdmlh.NumberList = CommFunc.FilterNumber(gjdmlh.Value, this.PlayInfo.CodeCount, this.Play, ref pErrorHint);
            }
            CommFunc.RefreshDataGridView(this.Egv_DMLHList, this.DMLHList.Count);
            this.Ckb_EditDMLH.Enabled = this.Ckb_DeleteDMLH.Enabled = this.DMLHList.Count > 0;
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
            if (this.Cbb_FBType.SelectedIndex == -1)
            {
                this.Cbb_FBType.SelectedIndex = 0;
            }
        }

        public void SetControlValue(ConfigurationStatus.FNBase pInfo)
        {
            List<string> dicKeyList = CommFunc.GetDicKeyList<List<ConfigurationStatus.PlayBase>>(AppInfo.PlayDic);
            CommFunc.SetComboBoxList(this.Cbb_PlayType, dicKeyList);
            CommFunc.SetComboBoxList(this.Cbb_BTFN, AppInfo.BTFNList);
            ConfigurationStatus.FNGJDMLH fngjdmlh = (ConfigurationStatus.FNGJDMLH) pInfo;
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_PlayType, pInfo.PlayType);
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_PlayName, pInfo.PlayName);
            this.Cbb_Unit.SelectedIndex = (int) pInfo.Unit;
            this.Ckb_BetsJK.Checked = pInfo.BetsJKSelect;
            this.Txt_BetsJK.Text = pInfo.BetsJKValue;
            this.Cbb_BetsJKMode.SelectedIndex = (int) pInfo.BetsJKMode;
            this.Txt_RXZJ.Text = fngjdmlh.RXZJ;
            this.SetRXWZList(fngjdmlh.RXWZList);
            this.Cbb_FBType.SelectedIndex = (int) pInfo.FBInfo;
            this.Rdb_CodeType1.Checked = pInfo.IsBetsZJ;
            this.Rdb_CodeType2.Checked = !pInfo.IsBetsZJ;
            this.Rdb_BTPlan.Checked = pInfo.BTType == ConfigurationStatus.SCTimesType.Plan;
            this.Rdb_BTFN.Checked = pInfo.BTType == ConfigurationStatus.SCTimesType.FN;
            this.Txt_BTPlan.Text = CommFunc.Join(pInfo.BTPlanList, ",");
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_BTFN, pInfo.BTFNName);
            this.IsMore = pInfo.VisibleMore;
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
            this.DMLHList.Clear();
            foreach (ConfigurationStatus.GJDMLH gjdmlh in fngjdmlh.DMLHValueList)
            {
                ConfigurationStatus.GJDMLH item = new ConfigurationStatus.GJDMLH(gjdmlh);
                this.DMLHList.Add(item);
            }
            this.RefreshDMLHList();
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

        private void SortDMLHList(List<ConfigurationStatus.GJDMLH> pList)
        {
            pList.Sort(delegate (ConfigurationStatus.GJDMLH info1, ConfigurationStatus.GJDMLH info2) {
                int iD = info1.ID;
                int num2 = info2.ID;
                return iD - num2;
            });
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

