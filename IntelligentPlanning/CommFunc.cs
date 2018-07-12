namespace IntelligentPlanning
{
    using IntelligentPlanning.CustomControls;
    using IntelligentPlanning.ExDataGridView;
    using IntelligentPlanning.Properties;
    
    using Microsoft.VisualBasic;
    using Microsoft.VisualBasic.CompilerServices;
    using Microsoft.Win32;
    using SevenZip;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Management;
    using System.Media;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Xml;

    internal class CommFunc
    {
        public static string AddSignInString(string pStr, char pSign)
        {
            string str = "";
            for (int i = 0; i < pStr.Length; i++)
            {
                str = str + pSign.ToString() + pStr[i];
            }
            return str.Substring(1);
        }

        public static string AESDecrypt(string pSource, string pKey)
        {
            string str = "";
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(pKey.Substring(0, 0x10));
                byte[] rgbIV = Encoding.UTF8.GetBytes(pKey.Substring(0, 0x10));
                byte[] buffer = Convert.FromBase64String(pSource);
                RijndaelManaged managed = new RijndaelManaged();
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, managed.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                str = Encoding.UTF8.GetString(stream.ToArray());
            }
            catch
            {
            }
            return str;
        }

        public static string AESEncrypt(string pSource, string pKey)
        {
            string str;
            MemoryStream stream = new MemoryStream();
            RijndaelManaged managed = new RijndaelManaged();
            byte[] bytes = Encoding.UTF8.GetBytes(pSource);
            byte[] destinationArray = new byte[0x20];
            Array.Copy(Encoding.UTF8.GetBytes(pKey.PadRight(destinationArray.Length)), destinationArray, destinationArray.Length);
            managed.Mode = CipherMode.ECB;
            managed.Padding = PaddingMode.PKCS7;
            managed.KeySize = 0x80;
            managed.Key = destinationArray;
            CryptoStream stream2 = new CryptoStream(stream, managed.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                str = Convert.ToBase64String(stream.ToArray());
            }
            finally
            {
                stream2.Close();
                stream.Close();
                managed.Clear();
            }
            return str;
        }

        public static bool AgreeMessage(string xHintStr, bool pIsAutoColse = true, MessageBoxIcon pIcon = MessageBoxIcon.Asterisk, string xTitleId = "")
        {
            if (xTitleId == "")
            {
                xTitleId = "永信在线挂机软件";
            }
            FrmMessageBox box = new FrmMessageBox(xHintStr, xTitleId, true, pIsAutoColse);
            return (box.ShowDialog() == DialogResult.Yes);
        }

        public static string[] AllToPart(string[] Arr)
        {
            List<string> list = new List<string>();
            foreach (string str in Arr)
            {
                char[] chArray = str.ToCharArray();
                for (int i = 0; i < chArray.Length; i++)
                {
                    int num2 = i;
                    for (int j = i + 1; j < chArray.Length; j++)
                    {
                        if (Convert.ToInt32(chArray[num2]) > Convert.ToInt32(chArray[j]))
                        {
                            num2 = j;
                        }
                    }
                    if (num2 != i)
                    {
                        char ch = chArray[num2];
                        chArray[num2] = chArray[i];
                        chArray[i] = ch;
                    }
                }
                string item = "";
                foreach (char ch2 in chArray)
                {
                    item = item + ch2;
                }
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
            string[] strArray = new string[list.Count];
            int index = 0;
            foreach (string str in list)
            {
                strArray[index] = str;
                index++;
            }
            return strArray;
        }

        public static List<string> AnalysisPTLine(ref Dictionary<string, ConfigurationStatus.PTLine> PTLineDic, string pLineString)
        {
            List<string> list = new List<string>();
            List<string> list2 = SplitString(pLineString, "\r\n", -1);
            PTLineDic.Clear();
            string item = "";
            List<string> pList = new List<string>();
            foreach (string str2 in list2)
            {
                if (str2 == "")
                {
                    if (item != "")
                    {
                        PTLineDic[item].LineList = CopyList(pList);
                        pList.Clear();
                        item = "";
                    }
                }
                else if (item == "")
                {
                    ConfigurationStatus.PTLine line = new ConfigurationStatus.PTLine(str2);
                    item = line.Name;
                    if (((AppInfo.AppPTNameList.Count == 0) || AppInfo.AppPTNameList.Contains(item)) && ((AppInfo.Account.Configuration.LoginPTList == null) || AppInfo.Account.Configuration.LoginPTList.Contains(line.Name)))
                    {
                        list.Add(item);
                    }
                    PTLineDic[item] = line;
                }
                else
                {
                    pList.Add(str2);
                }
            }
            return list;
        }

        public static void AppHandler(ComboBox pCbb)
        {
            if ((AppInfo.PTInfo == AppInfo.ZLJInfo) || (AppInfo.PTInfo == AppInfo.YRYLInfo))
            {
                pCbb.Items.Remove("厘");
            }
        }

        public static string Base64Decode(Encoding pEncode, string pSource)
        {
            byte[] bytes = Convert.FromBase64String(pSource);
            return pEncode.GetString(bytes);
        }

        public static string Base64Encode(Encoding pEncode, string pSource) =>
            Convert.ToBase64String(pEncode.GetBytes(pSource));

        public static void BeautifyComboBox(List<ComboBox> pComboBoxList)
        {
            foreach (ComboBox box in pComboBoxList)
            {
                box.FlatStyle = FlatStyle.Flat;
                box.BackColor = AppInfo.beaBackColor;
                box.ForeColor = AppInfo.beaForeColor;
            }
        }

        public static void BeautifyTabControl(List<TabControl> pTabControlList)
        {
            foreach (TabControl control in pTabControlList)
            {
                control.DrawMode = TabDrawMode.OwnerDrawFixed;
                control.DrawItem += new DrawItemEventHandler(CommFunc.TabControl_DrawItem);
            }
        }

        public static void ChangeIEVersion()
        {
            string fileName = Path.GetFileName(getWinFormPath());
            string regPath = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
            string str3 = @"Software\Wow6432Node\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
            WriteRegValue(regPath, fileName, "11001", RegistryValueKind.DWord);
            WriteRegValue(str3, fileName, "11001", RegistryValueKind.DWord);
        }

        public static string ChangeWebString(string pStr)
        {
            string str = pStr;
            return str.Replace("&amp;", "&");
        }

        public static bool CheckAppRunInSamePath()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
            foreach (Process process2 in processesByName)
            {
                if (process2.Id != currentProcess.Id)
                {
                    string directoryName = Path.GetDirectoryName(process2.MainModule.FileName);
                    string str2 = Path.GetDirectoryName(currentProcess.MainModule.FileName);
                    if (directoryName == str2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CheckAutomaticStart(string keyName)
        {
            string str = Detect3264() ? @"software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run" : @"software\Microsoft\Windows\CurrentVersion\Run";
            string[] valueNames = Registry.LocalMachine.OpenSubKey(@"software\Microsoft\Windows\CurrentVersion\Run", true).GetValueNames();
            foreach (string str2 in valueNames)
            {
                if (str2 == keyName)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckBetsTimes(string pTimes, ref string pError)
        {
            if ((pTimes != "") && !(CheckIsNumber(pTimes) && !pTimes.Contains(".")))
            {
                pError = "输入的金额必须是整数！";
                return false;
            }
            return true;
        }

        public static bool CheckBetsTimes(TextBox pTextBox, double pTimes)
        {
            string pError = "";
            bool flag = CheckBetsTimes(pTimes.ToString(), ref pError);
            if (!flag)
            {
                PublicMessageAll(pError, true, MessageBoxIcon.Asterisk, "");
                pTextBox.SelectAll();
                pTextBox.Focus();
            }
            return flag;
        }

        public static bool CheckBetsTimes(ConfigurationStatus.AutoBets pBets, TextBox pTextBox, double pTimes)
        {
            bool flag = CheckBetsTimes(pTextBox, pTimes);
            if (!flag)
            {
                pBets.IsOutLoop = true;
            }
            return flag;
        }

        public static void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = sender as CheckBox;
            if (box.Checked)
            {
                box.ForeColor = AppInfo.defaultForeColor;
            }
            else
            {
                box.ForeColor = AppInfo.beaForeColor;
            }
        }

        private static void CheckBox_MouseLeave(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(sender, e);
        }

        private static void CheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            box.ForeColor = AppInfo.defaultForeColor;
        }

        public static bool CheckCodeIsBaoZi(string pCode)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < 3; i++)
            {
                char ch = pCode[i];
                dictionary[ch.ToString()] = "";
            }
            return (dictionary.Count == 1);
        }

        public static bool CheckCodeIsZu3(string pCode)
        {
            Dictionary<char, string> dictionary = new Dictionary<char, string>();
            for (int i = 0; i < pCode.Length; i++)
            {
                dictionary[pCode[i]] = "";
            }
            return (dictionary.Count == 2);
        }

        public static bool CheckCodeIsZu6(string pCode)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < pCode.Length; i++)
            {
                char ch = pCode[i];
                dictionary[ch.ToString()] = "";
            }
            return (dictionary.Count == 3);
        }

        public static List<List<string>> CheckFilterNumber(string pInput, int pCodeLen, string playName, ref string pErrorHint)
        {
            List<List<string>> list = new List<List<string>>();
            List<string> list2 = SplitString(pInput, "|", -1);
            foreach (string str in list2)
            {
                List<string> item = FilterNumber(str, pCodeLen, playName, ref pErrorHint);
                if (pErrorHint != "")
                {
                    return list;
                }
                list.Add(item);
            }
            return list;
        }

        public static void CheckFIPSEnable()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Lsa\FipsAlgorithmPolicy", true);
                string name = "Enabled";
                if (Convert.ToInt32(key.GetValue(name)) != 0)
                {
                    key.SetValue(name, "0");
                }
            }
            catch
            {
            }
        }

        public static bool CheckIsKLCLottery(string pLotteryID)
        {
            List<string> list = new List<string> {
                "HGSSC",
                "JNDSSC",
                "TWSSC",
                "BJSSC",
                "XJPSSC",
                "LFHGSSC",
                "HGLTC",
                "XJP120M",
                "MD2FC",
                "UCTWSSC",
                "UCHL2FC",
                "WHXJPSSC",
                "XJP15F",
                "XDL90M",
                "MDHGSSC",
                "MDHG90M",
                "UCHGSSC",
                "HCSSC",
                "YTXJPSSC",
                "SIJIELSSSC",
                "YSENDJ35C",
                "YSENBJSSC",
                "YSENBJSSC",
                "WMHGSSC",
                "SIJIHGSSC",
                "FCOZ3FC",
                "FEICHGSSC",
                "LMHHGSSC",
                "CAIHXJPSSC",
                "CCTWSSCGB",
                "THFFC",
                "CTXXJPSSC",
                "THMD2FC",
                "THDJSSC",
                "THJDSSC",
                "THTGSSC",
                "TH5FC",
                "THHGSSC",
                "HNYLXJPSSC",
                "CBLXJPSSC",
                "THENXJPSSC",
                "MTXJPSSC",
                "DQOZ3FC",
                "HENRDJ2FC",
                "HENRXJPSSC",
                "HENROZ15C",
                "HENRXG15C",
                "HENR3FC",
                "WHENHGSSC",
                "WHENXJPSSC",
                "HDYLFFC",
                "HDYL2FC",
                "HDYL5FC",
                "HDYLASKFFC",
                "HCYLOZ3FC",
                "BHGJHGSSC",
                "XDBHGSSC",
                "DYHGSSC",
                "TYYLXJPSSC",
                "AMBLRAMPK10",
                "AMBLRTWPK10",
                "CYYLXJPSSC",
                "JYYLXJPSSC",
                "XHHCOZ3FC",
                "NBAJN11X5",
                "NBAFF11X5",
                "NBA3F11X5",
                "NBA5F11X5",
                "NBAJZDPK10",
                "NBA60MPK10",
                "NBA180MPK10",
                "NBABJSSC",
                "NBATWSSC",
                "NBANY5FC",
                "NBANY3FC",
                "NBANY2FC",
                "NBAQQFFC",
                "NBAHGSSC",
                "NBANDJSSC",
                "MXYLXJPSSC",
                "NBAXXLSSC",
                "WCAIXJPSSC",
                "WCAITWSSC",
                "YHSGXJPSSC",
                "YINHHGSSC",
                "XGLLWYN30M",
                "XGLLDJSSC",
                "XGLLLSJ2FC",
                "XGLLFLP15C",
                "XGLLWNS15C",
                "XGLLLFFC",
                "XGLLL2FC",
                "XGLLLSY11X5",
                "XGLLLDPK10",
                "XGLLSSPK10",
                "HENDJSPK10",
                "HENDOMPK10",
                "HENDFFPK10",
                "XTYLXJPSSC",
                "XWYLXJPSSC",
                "WZYLXJPSSC",
                "WZYLHGSSC",
                "WZYLXGSSC",
                "YZCPHGSSC",
                "YZCPXJPSSC",
                "TIYUXJPSSC",
                "HLJSSC",
                "TIYUNHG15C",
                "QFZXXJPSSC",
                "ZBEIHGSSC",
                "JXINXJPSSC",
                "JXINHGSSC"
            };
            return list.Contains(pLotteryID);
        }

        public static bool CheckIsNumber(string pStr)
        {
            double num;
            return double.TryParse(pStr, out num);
        }

        public static bool CheckIsSkipLottery(string pLotteryID)
        {
            List<string> list = new List<string> {
                "PK10",
                "QQTHGPK10",
                "WBJOMPK10",
                "YSENTBPK10",
                "XGSM",
                "THPK10",
                "THOZPK10",
                "HDYLFF11X5",
                "HDYL2F11X5",
                "HDYL5F11X5",
                "HDYLFFPK10",
                "HDYLFFFT"
            };
            return (list.Contains(pLotteryID) || CheckIsKLCLottery(pLotteryID));
        }

        public static bool CheckIsZW(char pChr) =>
            ((pChr >= '一') && (pChr <= 0x9fbb));

        public static bool CheckLotteryIsBD(string pLotteryID)
        {
            List<string> list = new List<string> {
                "CQSSC",
                "XJSSC",
                "TJSSC",
                "JLSSC",
                "NMGSSC",
                "HLJSSC",
                "YNSSC",
                "GD11X5",
                "SD11X5",
                "JX11X5",
                "AH11X5",
                "SH11X5",
                "JL11X5",
                "TJ11X5",
                "BJ11X5",
                "FJ11X5",
                "GS11X5",
                "GX11X5",
                "GZ11X5",
                "HEB11X5",
                "HLJ11X5",
                "HUB11X5",
                "JS11X5",
                "LN11X5",
                "NMG11X5",
                "SXL11X5",
                "SXR11X5",
                "XJ11X5",
                "YN11X5",
                "ZJ11X5",
                "HB11X5",
                "QH11X5",
                "HN11X5"
            };
            return !list.Contains(pLotteryID);
        }

        public static bool CheckPlayIsDS(string playName) =>
            playName.Contains("单式");

        public static bool CheckPlayIsDWD(string playName) =>
            (playName == "定位胆定位胆");

        public static bool CheckPlayIsFS(string playName)
        {
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (!CheckPlayIsZuXFS(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        return true;
                    }
                    if (CheckPlayIsDWD(playName))
                    {
                        return true;
                    }
                }
                return false;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                return playName.Contains("复式");
            }
            return ((AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10) && (playName.Contains("复式") || CheckPlayIsDWD(playName)));
        }

        public static bool CheckPlayIsHZ(string playName)
        {
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                {
                    return false;
                }
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                {
                    return playName.Contains("和值");
                }
            }
            return false;
        }

        public static bool CheckPlayIsLH(string playName)
        {
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPSSC)
            {
                return false;
            }
            return playName.Contains("龙虎");
        }

        public static bool CheckPlayIsMass(string pStr) =>
            (pStr.Contains("混选") || pStr.Contains("任选"));

        public static bool CheckPlayIsRX(string playName)
        {
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPSSC)
            {
                return false;
            }
            return playName.Contains("任");
        }

        public static bool CheckPlayIsRXDS(string playName)
        {
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPSSC)
            {
                return false;
            }
            return (playName.Contains("任") && !CheckPlayIsFS(playName));
        }

        public static bool CheckPlayIsRXFS(string playName)
        {
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPSSC)
            {
                return false;
            }
            return (playName.Contains("任") && CheckPlayIsFS(playName));
        }

        public static bool CheckPlayIsZu3(string playName) =>
            playName.Contains("组三");

        public static bool CheckPlayIsZu6(string playName) =>
            playName.Contains("组六");

        public static bool CheckPlayIsZuX(string playName) =>
            (CheckPlayIsZu3(playName) || CheckPlayIsZu6(playName));

        public static bool CheckPlayIsZuXFS(string playName) =>
            (CheckPlayIsZuX(playName) && playName.Contains("复式"));

        public static bool CheckRepeat(string pStr)
        {
            Dictionary<char, string> dictionary = new Dictionary<char, string>();
            foreach (char ch in pStr)
            {
                dictionary[ch] = "";
            }
            return (pStr.Length != dictionary.Count);
        }

        public static bool CheckResponseText(string pResponseText) =>
            (pResponseText == "1");

        public static bool CheckStringIsRange(string pStr, string pRange)
        {
            List<string> list = ConvertSameListString(pRange);
            foreach (char ch in pStr)
            {
                string item = ch.ToString();
                if (!list.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CheckStrInList(string pStr, List<string> pList)
        {
            foreach (string str in pList)
            {
                if (pStr.Contains(str))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckTextBoxIsNull(TextBox pTextBox, string pHint)
        {
            bool flag = false;
            if (pTextBox.Text == "")
            {
                pHint = pHint.Replace("：", "").Replace(":", "");
                PublicMessageAll($"【{pHint}】 不能为空！", true, MessageBoxIcon.Asterisk, "");
                pTextBox.Focus();
                flag = true;
            }
            return flag;
        }

        public static bool CheckUpdateAppl()
        {
            string cServerUpdateUrl = AppInfo.cServerUpdateUrl;
            string strB = "1.0.4";
            return (string.Compare(GetAppServerVersion(cServerUpdateUrl, AppInfo.Account.AppPerName), strB) > 0);
        }

        public static bool CheckValueContainsList(DataGridView pList, string pValue, int pIndex = 0)
        {
            foreach (DataGridViewRow row in (IEnumerable)pList.Rows)
            {
                if (row.Cells[pIndex].Value.ToString() == pValue)
                {
                    return true;
                }
            }
            return false;
        }

        public static double ChinaRound(double pNum, int pDigits = 2)
        {
            if (pNum < 0.0)
            {
                return Math.Round((double)(pNum + (5.0 / Math.Pow(10.0, (double)(pDigits + 1)))), pDigits, MidpointRounding.AwayFromZero);
            }
            return Math.Round(pNum, pDigits, MidpointRounding.AwayFromZero);
        }

        public static void ClearObejct()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            {
            }
        }

        public static void ClearSubKey(string regPath)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(regPath);
            try
            {
                key.DeleteSubKey("");
                key.Close();
            }
            catch
            {
            }
        }

        public static string ClearWrap(string pStr, string pChar = "") =>
            pStr.Replace("\r\n", pChar).Replace("\r", pChar).Replace("\n", pChar);

        public static void ClickALT8()
        {
            SendKeys.Send("%+{8}");
        }

        public static string ClickALT8(string pText, string pName)
        {
            string str = "";
            IntPtr hWnd = FindWindow(null, pName);
            if (hWnd != IntPtr.Zero)
            {
                if (GetControlText(GetParent(hWnd)) != pText)
                {
                    return str;
                }
                List<IntPtr> hWndList = new List<IntPtr>();
                EnumChildWindows(hWnd, delegate (IntPtr hwnd, int lParam) {
                    hWndList.Add(hwnd);
                    return true;
                }, 0);
                foreach (IntPtr ptr3 in hWndList)
                {
                    string controlText = GetControlText(ptr3);
                    if (controlText.Length > 2)
                    {
                        str = controlText;
                        break;
                    }
                }
                if (str == "")
                {
                    str = "未知错误";
                }
                SetForegroundWindow(hWnd);
                ClickEnter();
            }
            return str;
        }

        public static void ClickEnter()
        {
            SendKeys.SendWait("{Enter}");
        }

        public static void ClickEnter(IntPtr pHWnd)
        {
            SetForegroundWindow(pHWnd);
            ClickEnter();
        }

        public static string ClickEnter(string pText, string pName, List<string> pTextList = null, bool pIsException = true)
        {
            string str = "";
            IntPtr hWnd = FindWindow(null, pName);
            if (hWnd != IntPtr.Zero)
            {
                if (GetControlText(GetParent(hWnd)) != pText)
                {
                    return str;
                }
                List<IntPtr> hWndList = new List<IntPtr>();
                EnumChildWindows(hWnd, delegate (IntPtr hwnd, int lParam) {
                    hWndList.Add(hwnd);
                    return true;
                }, 0);
                foreach (IntPtr ptr3 in hWndList)
                {
                    string controlText = GetControlText(ptr3);
                    if (controlText.Length > 2)
                    {
                        str = controlText;
                        break;
                    }
                }
                if (str == "")
                {
                    str = "未知错误";
                }
                bool flag = !pIsException;
                if (pTextList != null)
                {
                    foreach (string str4 in pTextList)
                    {
                        if (str.Contains(str4))
                        {
                            flag = !flag;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    SetForegroundWindow(hWnd);
                    ClickEnter();
                }
                else
                {
                    str = "";
                }
            }
            return str;
        }

        public static void CloseProgress()
        {
            try
            {
                if (AppInfo.PregressHint != null)
                {
                    try
                    {
                        AppInfo.PregressHint.ParentWindow.Enabled = true;
                    }
                    catch
                    {
                    }
                    AppInfo.PregressHint.Close();
                    AppInfo.PregressHint = null;
                }
            }
            catch
            {
            }
        }

        public static string CombinaBetsCode(List<string> pNumberList, string playName)
        {
            string playChar = GetPlayChar(playName);
            return Join(pNumberList, playChar);
        }

        public static List<T> CombinaList<T>(List<T> pList1, List<T> pList2)
        {
            List<T> list = pList1;
            foreach (T local in pList2)
            {
                list.Add(local);
            }
            return list;
        }

        public static string Compress(string pSource)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pSource);
            MemoryStream stream = new MemoryStream();
            using (DeflateStream stream2 = new DeflateStream(stream, CompressionMode.Compress, true))
            {
                stream2.Write(bytes, 0, bytes.Length);
            }
            stream.Position = 0L;
            MemoryStream stream3 = new MemoryStream();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            byte[] dst = new byte[buffer.Length + 4];
            Buffer.BlockCopy(buffer, 0, dst, 4, buffer.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, dst, 0, 4);
            return Convert.ToBase64String(dst);
        }

        public static List<string> Convert11X5Code(List<string> pCodeList)
        {
            List<string> list = new List<string>();
            foreach (string str in pCodeList)
            {
                string item = "";
                if (str.Length == 1)
                {
                    item = Convert11X5Code(str);
                }
                else
                {
                    List<string> pList = new List<string>();
                    for (int i = 0; i < str.Length; i += 2)
                    {
                        string str3 = str.Substring(i, 2);
                        pList.Add(str3);
                    }
                    item = Join(pList, " ");
                }
                list.Add(item);
            }
            return list;
        }

        public static string Convert11X5Code(object pCode) =>
            pCode.ToString().PadLeft(2, '0');

        public static string Convert11X5CodeByString(string pCode)
        {
            List<string> pList = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string item = pCode.Substring(i * 2, 2);
                pList.Add(item);
            }
            return Join(pList, ",");
        }

        public static string Convert11X5CodeByString(string pCode, string pChar)
        {
            List<string> list = SplitString(pCode, pChar, -1);
            List<string> pList = new List<string>();
            foreach (string str2 in list)
            {
                string item = Convert11X5Code(str2);
                pList.Add(item);
            }
            return Join(pList, pChar);
        }

        public static string ConvertBetsCode(string pValue, string playName, string pSkip = "")
        {
            string str = "";
            if ((AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10) && CheckPlayIsFS(playName))
            {
                return pValue;
            }
            if ((pValue == null) || (pValue.ToString() == ""))
            {
                return str;
            }
            string item = ConvertNumberString(pValue.Trim(), " ", pSkip).Trim();
            ConfigurationStatus.PlayBase playInfo = GetPlayInfo(playName);
            string playChar = playInfo.PlayChar;
            if (!item.Contains(" "))
            {
                if (CheckPlayIsFS(playName))
                {
                    List<string> pList = new List<string>();
                    for (int i = 0; i < playInfo.IndexList.Count; i++)
                    {
                        pList.Add(item);
                    }
                    item = Join(pList, " ");
                }
                else
                {
                    item = Join(item, " ", -1);
                }
            }
            return CombinaBetsCode(SplitString(item, " ", -1), playName);
        }

        public static string ConvertBetsExpect(string pExpect, string pLotteryID, bool pDelChar = false, bool p11x5 = false, bool pXJSSC = false)
        {
            string str = pExpect;
            if ((pLotteryID == "CQSSC") || (pLotteryID == "TJSSC"))
            {
                str = str.Insert(6, "-");
            }
            else if (((((((pLotteryID == "XJSSC") || (pLotteryID == "GD11X5")) || ((pLotteryID == "SD11X5") || (pLotteryID == "JX11X5"))) || (((pLotteryID == "AH11X5") || (pLotteryID == "SH11X5")) || ((pLotteryID == "JL11X5") || (pLotteryID == "TJ11X5")))) || ((((pLotteryID == "BJ11X5") || (pLotteryID == "FJ11X5")) || ((pLotteryID == "GS11X5") || (pLotteryID == "GX11X5"))) || (((pLotteryID == "GZ11X5") || (pLotteryID == "HEB11X5")) || ((pLotteryID == "HLJ11X5") || (pLotteryID == "HUB11X5"))))) || (((((pLotteryID == "JS11X5") || (pLotteryID == "LN11X5")) || ((pLotteryID == "NMG11X5") || (pLotteryID == "SXL11X5"))) || (((pLotteryID == "SXR11X5") || (pLotteryID == "XJ11X5")) || ((pLotteryID == "YN11X5") || (pLotteryID == "ZJ11X5")))) || ((pLotteryID == "HB11X5") || (pLotteryID == "QH11X5")))) || (pLotteryID == "HN11X5"))
            {
                str = str.Insert(6, "-0");
            }
            if (!CheckIsSkipLottery(pLotteryID))
            {
                str = "20" + str;
            }
            if (!pDelChar)
            {
                return str;
            }
            if (pLotteryID == "XJSSC")
            {
                if (pXJSSC)
                {
                    return str.Replace("-0", "");
                }
            }
            else if ((((((pLotteryID == "GD11X5") || (pLotteryID == "SD11X5")) || ((pLotteryID == "SH11X5") || (pLotteryID == "JX11X5"))) || ((pLotteryID == "AH11X5") || (pLotteryID == "JS11X5"))) || (pLotteryID == "LN11X5")) && p11x5)
            {
                return str.Replace("-0", "");
            }
            return str.Replace("-", "");
        }

        public static List<double> ConvertBetTimes(List<string> pTimesList)
        {
            List<double> list = new List<double>();
            for (int i = 0; i < pTimesList.Count; i++)
            {
                string pTimes = pTimesList[i];
                if (pTimes != "")
                {
                    list.Add(ConvertBetTimes(pTimes));
                }
            }
            return list;
        }

        public static double ConvertBetTimes(string pTimes)
        {
            if ((pTimes == "x") || (pTimes == "X"))
            {
                return -1.0;
            }
            return Convert.ToDouble(pTimes);
        }

        public static string ConvertCode(string pCode, ConfigurationStatus.LotteryType pType)
        {
            string lotteryName = GetLotteryName(pType);
            return ConvertCode(pCode, lotteryName);
        }

        public static string ConvertCode(string pCode, string pLotteryName)
        {
            string str = pCode;
            if (pLotteryName.Contains("11选5") || pLotteryName.Contains("自行车"))
            {
                return str.Replace(" ", ",");
            }
            return str.Replace(",", "").Replace(" ", "");
        }

        public static Dictionary<string, string> ConvertConfiguration(List<string> pList, char pChar = '=')
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string str in pList)
            {
                string[] strArray = str.Split(new char[] { pChar });
                if (strArray.Length >= 2)
                {
                    string str2 = strArray[0];
                    string str3 = str.Substring(str.IndexOf(pChar) + 1);
                    dictionary[str2] = str3;
                }
            }
            return dictionary;
        }

        public static Dictionary<string, string> ConvertConfiguration(string pStr, char pChar = '=')
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] strArray = pStr.Split(new char[] { pChar });
            if (strArray.Length >= 2)
            {
                string str = strArray[0];
                string str2 = pStr.Substring(pStr.IndexOf(pChar) + 1);
                dictionary[str] = str2;
            }
            return dictionary;
        }

        public static string ConvertExpect(string pExpect, ConfigurationStatus.LotteryType pType)
        {
            string lotteryName = GetLotteryName(pType);
            string lotteryID = GetLotteryID(pType);
            return ConvertExpect(pExpect, lotteryName, lotteryID);
        }

        public static string ConvertExpect(string pExpect, string pLotteryName, string pLotteryID)
        {
            string str = pExpect;
            try
            {
                if (!CheckIsSkipLottery(pLotteryID))
                {
                    int lotteryExpectLen;
                    str = str.Substring(2);
                    if (((((pLotteryID == "CQSSC") || (pLotteryID == "TJSSC")) || ((pLotteryID == "JLSSC") || (pLotteryID == "NMGSSC"))) || (pLotteryID == "HLJSSC")) || (pLotteryID == "YNSSC"))
                    {
                        str = str.Replace("-", "");
                    }
                    else if (((((((pLotteryID == "XJSSC") || (pLotteryID == "GD11X5")) || ((pLotteryID == "SD11X5") || (pLotteryID == "JX11X5"))) || (((pLotteryID == "AH11X5") || (pLotteryID == "SH11X5")) || ((pLotteryID == "JL11X5") || (pLotteryID == "TJ11X5")))) || ((((pLotteryID == "BJ11X5") || (pLotteryID == "FJ11X5")) || ((pLotteryID == "GS11X5") || (pLotteryID == "GX11X5"))) || (((pLotteryID == "GZ11X5") || (pLotteryID == "HEB11X5")) || ((pLotteryID == "HLJ11X5") || (pLotteryID == "HUB11X5"))))) || (((((pLotteryID == "JS11X5") || (pLotteryID == "LN11X5")) || ((pLotteryID == "NMG11X5") || (pLotteryID == "SXL11X5"))) || (((pLotteryID == "SXR11X5") || (pLotteryID == "XJ11X5")) || ((pLotteryID == "YN11X5") || (pLotteryID == "ZJ11X5")))) || ((pLotteryID == "HB11X5") || (pLotteryID == "QH11X5")))) || (pLotteryID == "HN11X5"))
                    {
                        if (str.Length == 9)
                        {
                            str = str.Replace("-", "");
                        }
                        else
                        {
                            str = str.Replace("-0", "");
                        }
                        if (str.Length == 9)
                        {
                            str = str.Substring(0, 6) + str.Substring(7, 2);
                        }
                    }
                    if (((((((((pLotteryName.Contains("尚8") || pLotteryName.Contains("博猫")) || (pLotteryName.Contains("大丰") || pLotteryName.Contains("悠彩"))) || ((pLotteryName.Contains("梦之城") || pLotteryName.Contains("梦时代")) || (pLotteryName.Contains("M5") || pLotteryName.Contains("吉米")))) || (((pLotteryName.Contains("大发") || pLotteryName.Contains("广发")) || (pLotteryName.Contains("印尼") || pLotteryName.Contains("河内"))) || ((pLotteryName.Contains("澳门") || pLotteryName.Contains("悉尼三分彩")) || (pLotteryName.Contains("吉利") || pLotteryName.Contains("博牛"))))) || ((((pLotteryName.Contains("香港三分彩") || pLotteryName.Contains("台湾分分彩")) || (pLotteryName.Contains("纽约") || pLotteryName.Contains("香港分分彩"))) || ((pLotteryName.Contains("香港二分彩") || pLotteryName.Contains("创利")) || (pLotteryName.Contains("迪拜") || pLotteryName.Contains("大圣")))) || (((pLotteryName.Contains("万焰") || pLotteryName.Contains("纬度")) || (pLotteryName.Contains("新宝") || pLotteryName.Contains("广东分分彩"))) || ((pLotteryName.Contains("欧亿") || pLotteryName.Contains("K5")) || (pLotteryName.Contains("必火") || pLotteryName.Contains("极速")))))) || (((((pLotteryName.Contains("欢乐") || pLotteryName.Contains("幸运五分彩")) || (pLotteryName.Contains("大极乐") || pLotteryName.Contains("A6"))) || ((pLotteryName.Contains("新潮") || pLotteryName.Contains("印度五分彩")) || (pLotteryName.Contains("亿丰") || pLotteryName.Contains("微博")))) || (((pLotteryName.Contains("中胜") || pLotteryName.Contains("易发")) || (pLotteryName.Contains("台湾11选5") || pLotteryName.Contains("天音"))) || ((pLotteryName.Contains("瑞士分分彩") || pLotteryName.Contains("星娱")) || (pLotteryName.Contains("香港五分彩") || pLotteryName.Contains("天地"))))) || ((((pLotteryName.Contains("南下分分彩") || pLotteryName.Contains("亿博")) || (pLotteryName.Contains("越南") || pLotteryName.Contains("酷彩1.5分PK10"))) || ((pLotteryName.Contains("SKY") || pLotteryName.Contains("乐趣")) || (pLotteryName.Contains("新德里") || pLotteryName.Contains("瑞典")))) || (((pLotteryName.Contains("传奇") || pLotteryName.Contains("亿游")) || (pLotteryName.Contains("汇丰") || pLotteryName.Contains("九龙城"))) || ((pLotteryName.Contains("迪拜城") || pLotteryName.Contains("经典")) || (pLotteryName.Contains("俄罗斯") || pLotteryName.Contains("鹿鼎"))))))) || ((((((pLotteryName.Contains("新西兰") || pLotteryName.Contains("马来三分彩")) || (pLotteryName.Contains("久赢") || pLotteryName.Contains("玖富"))) || ((pLotteryName.Contains("菲律宾") || pLotteryName.Contains("腾讯")) || (pLotteryName.Contains("QQ") || pLotteryName.Contains("久发")))) || (((pLotteryName.Contains("巴西") || pLotteryName.Contains("乐利")) || (pLotteryName.Contains("风彩") || pLotteryName.Contains("赢財"))) || ((pLotteryName.Contains("百度") || pLotteryName.Contains("谷歌")) || (pLotteryName.Contains("VR") || pLotteryName.Contains("嗨网"))))) || ((((pLotteryName.Contains("吉祥") || pLotteryName.Contains("优盛")) || (pLotteryName.Contains("大阪") || pLotteryName.Contains("台北"))) || ((pLotteryName.Contains("吉隆坡") || pLotteryName.Contains("极速赛车PK10")) || (pLotteryName.Contains("马来") || pLotteryName.Contains("快乐")))) || (((pLotteryName.Contains("新德里1.5") || pLotteryName.Contains("红馆迪拜")) || (pLotteryName.Contains("加纳") || pLotteryName.Contains("台湾"))) || ((pLotteryName.Contains("济州岛") || pLotteryName.Contains("伦敦")) || (pLotteryName.Contains("GG") || pLotteryName.Contains("博美")))))) || (((((pLotteryName.Contains("帝都") || pLotteryName.Contains("日本")) || (pLotteryName.Contains("首尔") || pLotteryName.Contains("泰国"))) || ((pLotteryName.Contains("北京") || pLotteryName.Contains("电信")) || (pLotteryName.Contains("莫斯科") || pLotteryName.Contains("韩国")))) || (((pLotteryName.Contains("南宁") || pLotteryName.Contains("旺旺")) || (pLotteryName.Contains("兰博") || pLotteryName.Contains("新腾讯"))) || ((pLotteryName.Contains("LK") || pLotteryName.Contains("比利时")) || (pLotteryName.Contains("幸运飞艇") || pLotteryName.Contains("高速"))))) || ((((pLotteryName.Contains("富博") || pLotteryName.Contains("新火")) || (pLotteryName.Contains("新加坡") || pLotteryName.Contains("加拿大"))) || ((pLotteryName.Contains("恒发") || pLotteryName.Contains("多伦")) || (pLotteryName.Contains("联通") || pLotteryName.Contains("加州")))) || (((pLotteryName.Contains("香港") || pLotteryName.Contains("大马")) || (pLotteryName.Contains("推特") || pLotteryName.Contains("Skype"))) || ((pLotteryName.Contains("邮件") || pLotteryName.Contains("互联网流量")) || (pLotteryName.Contains("Youtube") || pLotteryName.Contains("时时彩一分")))))))) || (((((((pLotteryName.Contains("时时彩两分") || pLotteryName.Contains("赛车PK10一分")) || (pLotteryName.Contains("11选5一分") || pLotteryName.Contains("万美"))) || ((pLotteryName.Contains("TA") || pLotteryName.Contains("四季")) || (pLotteryName.Contains("古巴") || pLotteryName.Contains("巴西")))) || (((pLotteryName.Contains("UT8") || pLotteryName.Contains("好运")) || (pLotteryName.Contains("幸运") || pLotteryName.Contains("斯洛伐克"))) || ((pLotteryName.Contains("澳洲") || pLotteryName.Contains("赛车")) || (pLotteryName.Contains("翡翠") || pLotteryName.Contains("美福"))))) || ((((pLotteryName.Contains("柬埔寨") || pLotteryName.Contains("缅甸")) || (pLotteryName.Contains("微信") || pLotteryName.Contains("巴黎"))) || ((pLotteryName.Contains("汇盛") || pLotteryName.Contains("官方")) || (pLotteryName.Contains("大众") || pLotteryName.Contains("十里")))) || (((pLotteryName.Contains("HK") || pLotteryName.Contains("东京")) || (pLotteryName.Contains("美国") || pLotteryName.Contains("北海道"))) || ((pLotteryName.Contains("欧洲") || pLotteryName.Contains("金")) || (pLotteryName.Contains("万达") || pLotteryName.Contains("万和城")))))) || (((((pLotteryName.Contains("高雄") || pLotteryName.Contains("长隆")) || (pLotteryName.Contains("数亿") || pLotteryName.Contains("琥珀"))) || ((pLotteryName.Contains("德州") || pLotteryName.Contains("米兰")) || (pLotteryName.Contains("恒瑞") || pLotteryName.Contains("蓝冠")))) || (((pLotteryName.Contains("风云") || pLotteryName.Contains("汇众")) || (pLotteryName.Contains("澳利") || pLotteryName.Contains("公爵"))) || ((pLotteryName.Contains("德国") || pLotteryName.Contains("西贡")) || (pLotteryName.Contains("凯鑫") || pLotteryName.Contains("侏罗纪"))))) || ((((pLotteryName.Contains("盛世") || pLotteryName.Contains("西班牙")) || (pLotteryName.Contains("阿里") || pLotteryName.Contains("亚洲"))) || ((pLotteryName.Contains("强力球") || pLotteryName.Contains("宝汇")) || (pLotteryName.Contains("星多宝") || pLotteryName.Contains("斗鱼")))) || (((pLotteryName.Contains("土耳其") || pLotteryName.Contains("巴登")) || (pLotteryName.Contains("大版") || pLotteryName.Contains("英国"))) || ((pLotteryName.Contains("幻影") || pLotteryName.Contains("芝加哥")) || (pLotteryName.Contains("汶莱") || pLotteryName.Contains("众博"))))))) || (pLotteryName.Contains("天辰") || pLotteryName.Contains("中呗")))) || pLotteryName.Contains("秒乐彩"))
                    {
                        lotteryExpectLen = GetLotteryExpectLen(pLotteryName, pLotteryID);
                        str = str.Substring(0, 6) + "-" + str.Substring(6).PadLeft(lotteryExpectLen, '0');
                    }
                    else if (pLotteryID == "UCFFC")
                    {
                        lotteryExpectLen = GetLotteryExpectLen(pLotteryName, pLotteryID);
                        str = str.Split(new char[] { '-' })[0] + "-" + str.Split(new char[] { '-' })[1].PadLeft(lotteryExpectLen, '0');
                    }
                }
            }
            catch
            {
            }
            return str.Replace("--", "-");
        }

        public static string ConvertHGSSCCode(string pCode)
        {
            List<int> list = SplitInt(pCode, ",");
            List<string> pList = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                int num2 = 0;
                for (int j = 0; j < 4; j++)
                {
                    int num4 = (i * 4) + j;
                    num2 += list[num4];
                }
                pList.Add(Strings.Right(num2.ToString(), 1));
            }
            return Join(pList);
        }

        public static Image ConvertImageBy64String(string pImageString)
        {
            Image image = null;
            byte[] buffer = Convert.FromBase64String(pImageString);
            if (buffer.Length > 0)
            {
                MemoryStream stream = new MemoryStream(buffer, true);
                stream.Write(buffer, 0, buffer.Length);
                image = new Bitmap(stream);
            }
            return image;
        }

        public static void ConvertInputText(RichTextBox pTextBox, ConfigurationStatus.PlayBase playInfo)
        {
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CheckPlayIsDS(playInfo.Play))
                {
                    pTextBox.Text = pTextBox.Text.Replace(playInfo.PlayChar, "\r\n");
                }
            }
            else if ((AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10) && CheckPlayIsDS(playInfo.Play))
            {
                pTextBox.Text = pTextBox.Text.Replace(playInfo.PlayChar, "\r\n");
            }
        }

        public static List<int> ConvertIntList(string pStr) =>
            SplitInt(LottrySplitStr(pStr, ','), ",");

        public static IntPtr ConvertIntPtr(string pStr) =>
            ((pStr == "") ? IntPtr.Zero : new IntPtr(int.Parse(pStr)));

        public static List<string> ConvertIntToStrList(List<int> pList)
        {
            List<string> list = new List<string>();
            foreach (int num in pList)
            {
                list.Add(num.ToString());
            }
            return list;
        }

        public static List<string> ConvertList(Dictionary<string, string> pDic)
        {
            List<string> list = new List<string>();
            foreach (string str in pDic.Keys)
            {
                list.Add(str);
            }
            return list;
        }

        public static List<int> ConvertList(int[] pList)
        {
            List<int> list = new List<int>();
            foreach (int num in pList)
            {
                list.Add(num);
            }
            return list;
        }

        public static List<string> ConvertList(string[] pList)
        {
            List<string> list = new List<string>();
            foreach (string str in pList)
            {
                list.Add(str);
            }
            return list;
        }

        public static List<string> ConvertList(int pMin, int pMax) =>
            ConvertStringList(pMin + "-" + pMax);

        public static Dictionary<string, string> ConvertListToDic(List<string> pList)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string str in pList)
            {
                dictionary[str] = "";
            }
            return dictionary;
        }

        public static List<string> ConvertLotteryNameList(List<string> pLotteryIDList)
        {
            AppInfo.LotterNameDic.Clear();
            List<string> list = new List<string>();
            foreach (string str in pLotteryIDList)
            {
                ConfigurationStatus.LotteryConfig config = AppInfo.Current.LotteryDic[str];
                string name = config.Name;
                AppInfo.LotterNameDic[name] = str;
                list.Add(name);
            }
            return list;
        }

        public static string ConvertNumberString(string pStr, string pChar, string pSkip = "")
        {
            if (pStr.Length > 0x186a0)
            {
                return pStr;
            }
            string str = "";
            for (int i = 0; i < pStr.Length; i++)
            {
                char ch = pStr[i];
                if ((ch >= '0') && (ch <= '9'))
                {
                    str = str + ch;
                }
                else if ((pSkip != "") && pSkip.Contains(ch.ToString()))
                {
                    str = str + ch;
                }
                else
                {
                    str = str + pChar;
                }
            }
            return str;
        }

        public static string ConvertPK10CodeByString(string pCode)
        {
            List<string> pList = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                string item = Convert.ToInt32(pCode.Substring(i * 2, 2)).ToString();
                pList.Add(item);
            }
            return Join(pList, ",");
        }

        public static List<string> ConvertPK10CodeToBets(List<string> pList, int pChangeInt = -1)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < pList.Count; i++)
            {
                string pStr = pList[i];
                List<string> list2 = SplitString(pStr, " ", -1);
                List<string> list3 = new List<string>();
                foreach (string str2 in list2)
                {
                    int num2 = Convert.ToInt32(str2);
                    if (pChangeInt != -1)
                    {
                        num2 -= pChangeInt;
                    }
                    list3.Add(num2.ToString());
                }
                list.Add(Join(list3, " "));
            }
            return list;
        }

        public static List<string> ConvertSameList(string pStr)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < pStr.Length; i++)
            {
                list.Add(pStr[i].ToString());
            }
            return list;
        }

        public static List<int> ConvertSameListInt(string pStr)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < pStr.Length; i++)
            {
                char ch = pStr[i];
                list.Add(Convert.ToInt32(ch.ToString()));
            }
            return list;
        }

        public static List<string> ConvertSameListString(string pStr)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < pStr.Length; i++)
            {
                list.Add(pStr[i].ToString());
            }
            return list;
        }

        public static List<string> ConvertStringList(string pStr) =>
            SplitString(LottrySplitStr(pStr, ','), ",", -1);

        public static List<int> ConvertStrToIntList(List<string> pList)
        {
            List<int> list = new List<int>();
            foreach (string str in pList)
            {
                list.Add(Convert.ToInt32(str));
            }
            return list;
        }

        public static List<int> ConvertStrToIntList(string[] pList)
        {
            List<int> list = new List<int>();
            foreach (string str in pList)
            {
                list.Add(Convert.ToInt32(str));
            }
            return list;
        }

        public static string ConvertTXFFCCode(string pCode)
        {
            List<int> pList = SplitInt(pCode, ",");
            List<string> list2 = new List<string> {
                Strings.Right(CountAnd(pList).ToString(), 1)
            };
            list2.Add(pList[5].ToString());
            list2.Add(pList[6].ToString());
            list2.Add(pList[7].ToString());
            int num = ((pList[5] + pList[6]) + pList[7]) + pList[8];
            list2.Add(Strings.Right(num.ToString(), 1));
            return Join(list2);
        }

        public static string ConvertTXFFCCode1(string pCode)
        {
            List<int> list = SplitInt(pCode, ",");
            List<int> pList = new List<int> {
                list[4],
                list[5],
                list[6],
                list[7],
                list[8]
            };
            List<int> list3 = new List<int> {
                list[5],
                list[6],
                list[7],
                list[8]
            };
            List<int> list4 = new List<int> {
                list[6]
            };
            List<int> list5 = new List<int> {
                list[6],
                list[7]
            };
            List<int> list6 = new List<int> {
                list[7],
                list[8]
            };
            List<string> list7 = new List<string>();
            int num = (CountAnd(pList) + list[8]) + list[7];
            list7.Add(Strings.Right(num.ToString(), 1));
            num = (CountAnd(list3) + list[8]) + list[6];
            list7.Add(Strings.Right(num.ToString(), 1));
            list7.Add(Strings.Right(CountAnd(list4).ToString(), 1));
            list7.Add(Strings.Right(CountAnd(list5).ToString(), 1));
            list7.Add(Strings.Right(CountAnd(list6).ToString(), 1));
            return Join(list7);
        }

        public static string ConvertZWString(string pStr, string pChar, List<string> pSikpList = null)
        {
            string str = "";
            for (int i = 0; i < pStr.Length; i++)
            {
                string str2 = pChar;
                char pChr = pStr[i];
                string item = pChr.ToString();
                if (CheckIsZW(pChr))
                {
                    str2 = item;
                }
                else if (pSikpList != null)
                {
                    char ch2;
                    bool flag = pSikpList.Contains(item);
                    bool flag2 = (i < (pStr.Length - 1)) && pSikpList.Contains((ch2 = pStr[i + 1]).ToString());
                    bool flag3 = (i > 0) && pSikpList.Contains((ch2 = pStr[i - 1]).ToString());
                    if ((flag || flag2) || flag3)
                    {
                        str2 = item;
                    }
                }
                str = str + str2;
            }
            return str;
        }

        public static List<string> CopyList(List<string> pList)
        {
            List<string> list = new List<string>();
            foreach (string str in pList)
            {
                list.Add(str);
            }
            return list;
        }

        public static List<T> CopyList<T>(List<T> pList)
        {
            List<T> list = new List<T>();
            foreach (T local in pList)
            {
                list.Add(local);
            }
            return list;
        }

        public static void CopyText(string pText)
        {
            Clipboard.Clear();
            Clipboard.SetDataObject(pText);
        }

        public static string CountAC(string pStr)
        {
            List<string> list = ConvertSameList(pStr);
            List<int> list2 = new List<int>();
            for (int i = 0; i < (list.Count - 1); i++)
            {
                int num2 = Convert.ToInt32(list[i]);
                for (int j = i + 1; j < list.Count; j++)
                {
                    int item = Math.Abs((int)(Convert.ToInt32(list[j]) - num2));
                    if (!list2.Contains(item))
                    {
                        list2.Add(item);
                    }
                }
            }
            return list2.Count.ToString();
        }

        public static int CountAmp(object pNum1, object pNum2)
        {
            int num = Convert.ToInt32(pNum1);
            int num2 = Convert.ToInt32(pNum2);
            return Math.Abs((int)(num - num2));
        }

        public static int CountAnd(List<int> pList)
        {
            int num = 0;
            for (int i = 0; i < pList.Count; i++)
            {
                int num3 = pList[i];
                num += num3;
            }
            return num;
        }

        public static int CountAnd(List<string> pList)
        {
            int num = 0;
            for (int i = 0; i < pList.Count; i++)
            {
                int num3 = Convert.ToInt32(pList[i]);
                num += num3;
            }
            return num;
        }

        public static string CountAnd(string pStr)
        {
            int num = 0;
            foreach (char ch in pStr)
            {
                num += Convert.ToInt32(ch.ToString());
            }
            return num.ToString();
        }

        public static int CountAndEnd(List<int> pList) =>
            Convert.ToInt32(Strings.Right(CountAnd(pList).ToString(), 1));

        public static string CountAndEnd(string pStr) =>
            Strings.Right(CountAnd(pStr).ToString(), 1);

        public static string CountAverage(string pStr)
        {
            double num = 0.0;
            List<string> list = ConvertSameList(pStr);
            foreach (string str in list)
            {
                num += Convert.ToDouble(str);
            }
            return Math.Round((double)(num / ((double)list.Count)), 0).ToString();
        }

        public static string CountChu3He(string pStr)
        {
            int num = 0;
            List<string> list = ConvertSameList(pStr);
            foreach (string str in list)
            {
                num += Convert.ToInt32(str) % 3;
            }
            return num.ToString();
        }

        public static string CountContinuous(string pStr)
        {
            int num = 0;
            List<string> list = ConvertSameList(pStr);
            for (int i = 0; i < (list.Count - 1); i++)
            {
                int num3 = Convert.ToInt32(list[i]);
                if (Math.Abs((int)(Convert.ToInt32(list[i + 1]) - num3)) == 1)
                {
                    num++;
                }
            }
            return num.ToString();
        }

        public static string CountDaZhongXiao(string pStr)
        {
            string str = "";
            List<string> list = ConvertSameList(pStr);
            foreach (string str2 in list)
            {
                int num = Convert.ToInt32(str2);
                if (num >= 7)
                {
                    str = str + "大";
                }
                else if (num <= 2)
                {
                    str = str + "小";
                }
                else
                {
                    str = str + "中";
                }
            }
            return str;
        }

        public static string CountDS(string pNum)
        {
            if ((Convert.ToInt32(pNum) % 2) == 1)
            {
                return "单";
            }
            return "双";
        }

        public static string CountDX(List<int> pList, int pView = 4)
        {
            string str = "";
            foreach (int num in pList)
            {
                string str2 = (num > pView) ? "大" : "小";
                str = str + str2;
            }
            return str;
        }

        public static string CountDX(string pNum, int pView = 4)
        {
            if (Convert.ToInt32(pNum) > pView)
            {
                return "大";
            }
            return "小";
        }

        public static string CountDZX(List<int> pList)
        {
            string str = "";
            foreach (int num in pList)
            {
                string str2 = "";
                if (num >= 7)
                {
                    str2 = "大";
                }
                else if ((num >= 3) && (num <= 6))
                {
                    str2 = "中";
                }
                else if ((num >= 0) && (num <= 2))
                {
                    str2 = "小";
                }
                str = str + str2;
            }
            return str;
        }

        public static string CountJianGeRatio(string pStr)
        {
            List<string> list = ConvertSameList(pStr);
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < list.Count; i++)
            {
                bool flag = false;
                int num4 = Convert.ToInt32(list[i]);
                if ((i > 0) && (Math.Abs((int)(num4 - Convert.ToInt32(list[i - 1]))) == 2))
                {
                    flag = true;
                }
                if ((i < (list.Count - 1)) && (Math.Abs((int)(num4 - Convert.ToInt32(list[i + 1]))) == 2))
                {
                    flag = true;
                }
                if (flag)
                {
                    num++;
                }
                else
                {
                    num2++;
                }
            }
            return (num.ToString() + ":" + num2.ToString());
        }

        public static string CountJiOuRatio(string pStr)
        {
            string str = "";
            List<string> list = ConvertSameList(pStr);
            foreach (string str2 in list)
            {
                if ((Convert.ToInt32(str2) % 2) == 1)
                {
                    str = str + "奇";
                }
                else
                {
                    str = str + "偶";
                }
            }
            return str;
        }

        public static string CountJO(List<int> pList)
        {
            string str = "";
            foreach (int num in pList)
            {
                string str2 = ((num % 2) == 1) ? "奇" : "偶";
                str = str + str2;
            }
            return str;
        }

        public static string CountL012(List<int> pList)
        {
            string str = "";
            foreach (int num in pList)
            {
                str = str + ((num % 3)).ToString();
            }
            return str;
        }

        public static string CountLH(string pNum1, string pNum2)
        {
            int num = Convert.ToInt32(pNum1);
            int num2 = Convert.ToInt32(pNum2);
            if (num == num2)
            {
                return "和";
            }
            if (num > num2)
            {
                return "龙";
            }
            return "虎";
        }

        public static string CountMainInterval(string pStr)
        {
            int num = 0;
            List<string> list = ConvertSameList(pStr);
            for (int i = 0; i < (list.Count - 1); i++)
            {
                int num3 = Convert.ToInt32(list[i]);
                int num5 = Math.Abs((int)(Convert.ToInt32(list[i + 1]) - num3));
                if (num5 > num)
                {
                    num = num5;
                }
            }
            return num.ToString();
        }

        public static string CountNextExpect(string pExpect, string pLotteryID = "")
        {
            if (pExpect == "")
            {
                return "";
            }
            if (pLotteryID == "")
            {
                pLotteryID = AppInfo.Current.Lottery.ID;
            }
            ConfigurationStatus.LotteryConfig config = AppInfo.Current.LotteryDic[pLotteryID];
            string number = GetNumber(pExpect);
            if (CheckIsSkipLottery(pLotteryID))
            {
                number = (Convert.ToInt64(number) + 1L).ToString();
                if (((pLotteryID == "WBJOMPK10") || (pLotteryID == "YSENTBPK10")) || (pLotteryID == "HENDOMPK10"))
                {
                    number = "0" + number;
                }
                else if (pLotteryID == "HENDFFPK10")
                {
                    number = "00" + number;
                }
                return number;
            }
            DateTime time = new DateTime(Convert.ToInt32("20" + number.Substring(0, 2)), Convert.ToInt32(number.Substring(2, 2)), Convert.ToInt32(number.Substring(4, 2)));
            int length = number.Length - 6;
            int num3 = Convert.ToInt32(number.Substring(6, length)) + 1;
            if (num3 == (config.Expect + 1))
            {
                time = time.AddDays(1.0);
                num3 = 1;
            }
            string str2 = CheckLotteryIsBD(config.ID) ? "-" : "";
            return (time.ToString("yyMMdd") + str2 + num3.ToString().PadLeft(length, '0'));
        }

        public static int CountOut(List<int> pList)
        {
            int num = pList[0];
            int num2 = pList[0];
            for (int i = 0; i < pList.Count; i++)
            {
                int num4 = pList[i];
                if (num4 > num)
                {
                    num = num4;
                }
                if (num4 < num2)
                {
                    num2 = num4;
                }
            }
            return (num - num2);
        }

        public static string CountOut(string pStr) =>
            CountOut(ConvertStrToIntList(ConvertSameList(pStr))).ToString();

        public static bool CountQ(int pNum)
        {
            bool flag = false;
            try
            {
                List<string> list2 = new List<string> {
                    "1",
                    "2",
                    "3",
                    "5",
                    "7",
                    "11",
                    "13",
                    "17",
                    "19",
                    "23",
                    "29",
                    "31",
                    "37",
                    "41",
                    "43",
                    "47",
                    "53",
                    "59",
                    "61",
                    "67",
                    "71",
                    "73",
                    "79",
                    "83",
                    "89",
                    "97",
                    "101",
                    "103",
                    "107",
                    "109",
                    "113",
                    "127",
                    "131",
                    "137",
                    "139",
                    "149",
                    "151",
                    "157",
                    "163",
                    "167",
                    "173",
                    "179",
                    "181",
                    "191",
                    "193",
                    "197",
                    "199"
                };
                flag = list2.Contains(pNum.ToString());
            }
            catch
            {
            }
            return flag;
        }

        public static string CountRatio(object num1, object num2, string pAddStr = "%", int pCount = 2)
        {
            if (Convert.ToDouble(num2) == 0.0)
            {
                return ("0" + pAddStr);
            }
            double num = Convert.ToDouble(num1) / Convert.ToDouble(num2);
            return (Math.Round((double)(num * 100.0), pCount) + pAddStr);
        }

        public static int CountRepeat(string pStr1, string pStr2)
        {
            int num = 0;
            List<string> list = ConvertSameList(pStr1);
            List<string> list2 = ConvertSameList(pStr2);
            foreach (string str in list)
            {
                if (list2.Contains(str))
                {
                    num++;
                }
            }
            return num;
        }

        public static int CountRepeatByBlue(string pStr1, string pStr2)
        {
            int num = 0;
            List<string> list = ConvertSameList(pStr1);
            List<string> list2 = ConvertSameList(pStr2);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == list2[i])
                {
                    num++;
                }
            }
            return num;
        }

        public static string CountShouWeiCha(string pStr) =>
            Math.Abs((int)(Convert.ToInt32(pStr[0]) - Convert.ToInt32(pStr[pStr.Length - 1]))).ToString();

        public static string CountSizeRatio(string pStr)
        {
            string str = "";
            List<string> list = ConvertSameList(pStr);
            foreach (string str2 in list)
            {
                if (Convert.ToInt32(str2) >= 5)
                {
                    str = str + "大";
                }
                else
                {
                    str = str + "小";
                }
            }
            return str;
        }

        public static string CountWeiZhiJianGe(string pStr)
        {
            int num = 0;
            List<string> list = ConvertSameList(pStr);
            for (int i = 0; i < (list.Count - 1); i++)
            {
                int num3 = Convert.ToInt32(list[i]);
                int num4 = Convert.ToInt32(list[i + 1]);
                num = Math.Abs((int)(num - Math.Abs((int)(num3 - num4))));
            }
            return num.ToString();
        }

        public static string CountZH(List<int> pList)
        {
            string str = "";
            foreach (int num in pList)
            {
                string str2 = CountQ(num) ? "质" : "合";
                str = str + str2;
            }
            return str;
        }

        public static string CountZH(string pNum)
        {
            if (CountQ(Convert.ToInt32(pNum)))
            {
                return "质";
            }
            return "合";
        }

        public static string CountZiHeRatio(string pStr)
        {
            string str = "";
            List<string> list = ConvertSameList(pStr);
            foreach (string str2 in list)
            {
                if (CountQ(Convert.ToInt32(str2)))
                {
                    str = str + "质";
                }
                else
                {
                    str = str + "合";
                }
            }
            return str;
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string CreatPTUserString(string pName, Dictionary<string, ConfigurationStatus.PTLine> PTLineDic)
        {
            List<string> pList = new List<string>();
            List<string> list2 = new List<string>();
            foreach (string str2 in PTLineDic.Keys)
            {
                list2.Add(PTLineDic[str2].ID);
            }
            foreach (string str3 in list2)
            {
                string item = str3 + "=" + pName;
                pList.Add(item);
            }
            return Join(pList, ";");
        }

        public static void CutPathFile(string path1, string path2)
        {
            DirectoryInfo info = new DirectoryInfo(path1);
            foreach (FileInfo info2 in info.GetFiles("*.txt"))
            {
                string name = info2.Name;
                string path = path2 + @"\" + name;
                if (!System.IO.File.Exists(path))
                {
                    info2.MoveTo(path);
                }
            }
        }

        public static string Decode(string pSource, string pKey)
        {
            string str = "";
            try
            {
                byte[] buffer = Convert.FromBase64String(pSource);
                pKey = FillMdkey(pKey);
                using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
                {
                    provider.Key = Encoding.ASCII.GetBytes(pKey);
                    provider.IV = Encoding.ASCII.GetBytes(pKey);
                    MemoryStream stream = new MemoryStream();
                    using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        stream2.Write(buffer, 0, buffer.Length);
                        stream2.FlushFinalBlock();
                        stream2.Close();
                    }
                    str = Encoding.UTF8.GetString(stream.ToArray());
                    stream.Close();
                }
            }
            catch
            {
            }
            return str;
        }

        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static void DeleteFile(string pFile)
        {
            if (System.IO.File.Exists(pFile))
            {
                System.IO.File.Delete(pFile);
            }
        }

        public static bool Detect3264()
        {
            string str = "";
            ConnectionOptions options = new ConnectionOptions();
            ManagementScope scope = new ManagementScope(@"\\localhost", options);
            ObjectQuery query = new ObjectQuery("select AddressWidth from Win32_Processor");
            ManagementObjectCollection objects = new ManagementObjectSearcher(scope, query).Get();
            foreach (ManagementObject obj2 in objects)
            {
                str = obj2["AddressWidth"].ToString();
            }
            return (str == "64");
        }

        public static void DownloadFile(string pUrl, string pFile)
        {
            try
            {
                new WebClient().DownloadFile(pUrl, pFile);
            }
            catch
            {
            }
        }

        public static bool DownUpdateList(string pUrl, ref XmlDocument pXmlDoc)
        {
            bool flag = false;
            try
            {
                string pReferer = pUrl;
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
                pXmlDoc = new XmlDocument();
                pXmlDoc.LoadXml(pResponsetext);
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        public static string Encode(string pSource, string pKey)
        {
            StringBuilder builder = new StringBuilder();
            pKey = FillMdkey(pKey);
            using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(pKey);
                byte[] buffer2 = Encoding.ASCII.GetBytes(pKey);
                byte[] buffer = Encoding.UTF8.GetBytes(pSource);
                provider.Mode = CipherMode.CBC;
                provider.Key = bytes;
                provider.IV = buffer2;
                string str = "";
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        stream2.Write(buffer, 0, buffer.Length);
                        stream2.FlushFinalBlock();
                        str = Convert.ToBase64String(stream.ToArray());
                    }
                }
                return str;
            }
        }

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, WNDENUMPROC lpEnumFunc, int lParam);
        public static void FillComboBoxItem(ComboBox pCbb, int pMin, int pMax, int pStep = 1)
        {
            pCbb.Items.Clear();
            for (int i = pMin; i <= pMax; i += pStep)
            {
                pCbb.Items.Add(i.ToString());
            }
        }

        public static string FillMdkey(string pMdkey)
        {
            string str = pMdkey;
            str = str.Split(new char[] { '-' })[0];
            return str.PadRight(8, '8').Substring(0, 8);
        }

        public static List<string> FilterNumber(string pInput)
        {
            List<string> list = new List<string>();
            string[] strArray = pInput.Split(new char[] { ' ' });
            foreach (string str in strArray)
            {
                if (str != "")
                {
                    list.Add(str);
                }
            }
            return list;
        }

        public static List<string> FilterNumber(string pInput, int pCodeLen, string playName, ref string pErrorHint)
        {
            List<string> list2;
            List<string> list = new List<string>();
            ConfigurationStatus.PlayBase playInfo = GetPlayInfo(playName);
            string playChar = playInfo.PlayChar;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                pInput = pInput.Replace("\r\n", " ");
                pInput = pInput.Replace("\n", " ");
                list2 = SplitString(pInput, playChar, -1);
                if (CheckPlayIsFS(playName))
                {
                    if (CheckPlayIsRXFS(playName))
                    {
                        List<string> rXFSCodeList = playInfo.GetRXFSCodeList(list2);
                        if ((list2.Count != 5) || (rXFSCodeList.Count < pCodeLen))
                        {
                            pErrorHint = "投注号码和当前玩法长度不匹配！";
                        }
                    }
                    else if (list2.Count != pCodeLen)
                    {
                        pErrorHint = "投注号码和当前玩法长度不匹配！";
                    }
                    foreach (string str2 in list2)
                    {
                        if (str2 != "")
                        {
                            if (!((str2 == "*") || IsIntNumber(str2)))
                            {
                                pErrorHint = "发现非数字的号码！";
                            }
                            else if ((AppInfo.PTInfo != AppInfo.MZCInfo) && CheckRepeat(str2))
                            {
                                pErrorHint = "发现重复的号码！";
                            }
                            else
                            {
                                list.Add(str2);
                            }
                        }
                    }
                    return list;
                }
                if (CheckPlayIsLH(playName))
                {
                    foreach (string str2 in list2)
                    {
                        if (str2 != "")
                        {
                            if (!"龙虎和".Contains(str2))
                            {
                                pErrorHint = "发现非龙虎和的号码！";
                            }
                            else if (dictionary.ContainsKey(str2))
                            {
                                pErrorHint = "发现重复的号码！";
                            }
                            else
                            {
                                dictionary[str2] = "";
                                list.Add(str2);
                            }
                        }
                    }
                    return list;
                }
                if (CheckPlayIsZuXFS(playName))
                {
                    if (CheckPlayIsZu3(playName))
                    {
                        if (list2.Count < 2)
                        {
                            pErrorHint = "组三号码不完整！";
                            return list;
                        }
                    }
                    else if (CheckPlayIsZu6(playName) && (list2.Count < 3))
                    {
                        pErrorHint = "组六号码不完整！";
                        return list;
                    }
                }
                foreach (string str2 in list2)
                {
                    if (str2 != "")
                    {
                        if (str2.Length != pCodeLen)
                        {
                            pErrorHint = "发现长度不匹配的号码！";
                        }
                        else if (!IsIntNumber(str2))
                        {
                            pErrorHint = "发现非数字的号码！";
                        }
                        else if ((AppInfo.PTInfo != AppInfo.MZCInfo) && dictionary.ContainsKey(str2))
                        {
                            pErrorHint = "发现重复的号码！";
                        }
                        else
                        {
                            dictionary[str2] = "";
                            list.Add(str2);
                        }
                    }
                }
                return list;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                list2 = SplitString(pInput, playChar, -1);
                if (CheckPlayIsDS(playName))
                {
                    foreach (string str2 in list2)
                    {
                        if (str2 != "")
                        {
                            if (str2.Replace(" ", "").Length != (pCodeLen * 2))
                            {
                                pErrorHint = "投注号码和当前玩法长度不匹配！";
                            }
                            else if (!IsIntNumber(str2.Replace(" ", "")))
                            {
                                pErrorHint = "发现非数字的号码！";
                            }
                            else if (dictionary.ContainsKey(str2))
                            {
                                pErrorHint = "发现重复的号码！";
                            }
                            else
                            {
                                dictionary[str2] = "";
                                list.Add(str2);
                            }
                        }
                    }
                    return list;
                }
                if (list2.Count < pCodeLen)
                {
                    pErrorHint = "投注号码和当前玩法长度不匹配！";
                }
                foreach (string str2 in list2)
                {
                    if (str2 != "")
                    {
                        if (!IsIntNumber(str2))
                        {
                            pErrorHint = "发现非数字的号码！";
                        }
                        else if (dictionary.ContainsKey(str2))
                        {
                            pErrorHint = "发现重复的号码！";
                        }
                        else
                        {
                            dictionary[str2] = "";
                            list.Add(str2);
                        }
                    }
                }
                return list;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                list2 = SplitString(pInput, playChar, -1);
                if (CheckPlayIsDS(playName))
                {
                    foreach (string str2 in list2)
                    {
                        if (str2 != "")
                        {
                            if (str2.Replace(" ", "").Length != (pCodeLen * 2))
                            {
                                pErrorHint = "投注号码和当前玩法长度不匹配！";
                            }
                            else if (!IsIntNumber(str2.Replace(" ", "")))
                            {
                                pErrorHint = "发现非数字的号码！";
                            }
                            else if (dictionary.ContainsKey(str2))
                            {
                                pErrorHint = "发现重复的号码！";
                            }
                            else
                            {
                                dictionary[str2] = "";
                                list.Add(str2);
                            }
                        }
                    }
                    return list;
                }
                if (CheckPlayIsFS(playName))
                {
                    if (list2.Count < pCodeLen)
                    {
                        pErrorHint = "投注号码和当前玩法长度不匹配！";
                    }
                    foreach (string str2 in list2)
                    {
                        if (str2 != "")
                        {
                            if (str2 != "*")
                            {
                                List<string> list4 = SplitString(str2, " ", -1);
                                dictionary.Clear();
                                foreach (string str3 in list4)
                                {
                                    if (!IsIntNumber(str3))
                                    {
                                        pErrorHint = "发现非数字的号码！";
                                    }
                                    else
                                    {
                                        int num = Convert.ToInt32(str3);
                                        if ((num < AppInfo.Current.Lottery.Min) || (num > AppInfo.Current.Lottery.Max))
                                        {
                                            pErrorHint = "输入的号码不正确！";
                                        }
                                        else if (dictionary.ContainsKey(str3))
                                        {
                                            pErrorHint = "发现重复的号码！";
                                        }
                                        else
                                        {
                                            dictionary[str3] = "";
                                        }
                                    }
                                }
                            }
                            list.Add(str2);
                        }
                    }
                    return list;
                }
                if (CheckPlayIsHZ(playName))
                {
                    foreach (string str2 in list2)
                    {
                        if (str2 != "")
                        {
                            if (!IsIntNumber(str2))
                            {
                                pErrorHint = "发现非数字的号码！";
                            }
                            else if (dictionary.ContainsKey(str2))
                            {
                                pErrorHint = "发现重复的号码！";
                            }
                            else
                            {
                                string item = Convert.ToInt32(str2).ToString();
                                List<string> list5 = AppInfo.CombinaDicPK10HZ["和值" + playInfo.IndexList.Count];
                                if (!list5.Contains(item))
                                {
                                    pErrorHint = $"发现不符合的号码【{item}】！";
                                }
                                else
                                {
                                    dictionary[str2] = "";
                                    list.Add(str2);
                                }
                            }
                        }
                    }
                    return list;
                }
                if (list2.Count < pCodeLen)
                {
                    pErrorHint = "投注号码和当前玩法长度不匹配！";
                }
                foreach (string str2 in list2)
                {
                    if (str2 != "")
                    {
                        if (!IsIntNumber(str2))
                        {
                            pErrorHint = "发现非数字的号码！";
                        }
                        else if (dictionary.ContainsKey(str2))
                        {
                            pErrorHint = "发现重复的号码！";
                        }
                        else
                        {
                            dictionary[str2] = "";
                            list.Add(str2);
                        }
                    }
                }
            }
            return list;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        public static void GetAllOrder(ref List<string> pOutList, List<string> pList, int pBegin, int pEnd)
        {
            if (pBegin == pEnd)
            {
                string item = Join(pList, " ");
                pOutList.Add(item);
            }
            else
            {
                for (int i = pBegin; i <= pEnd; i++)
                {
                    Swap(pList, pBegin, i);
                    GetAllOrder(ref pOutList, pList, pBegin + 1, pEnd);
                    Swap(pList, i, pBegin);
                }
            }
        }

        public static Dictionary<string, string> GetAppConfiguration(string pAppName, string path = "") =>
            GetConfiguration(HttpHelper.GetWebData(pAppName, path), "\r\n");

        public static string getApplicationDataPath() =>
            (getDllPath() + @"\OpenCode\");

        public static string GetAppServerVersion(string pServerUrl, string pAppName)
        {
            string str = "";
            try
            {
                string pUrl = pServerUrl + GetWebUpdateListFile(pAppName);
                XmlDocument pXmlDoc = null;
                if (!DownUpdateList(pUrl, ref pXmlDoc))
                {
                    return str;
                }
                str = pXmlDoc.SelectSingleNode("AutoUpdater").SelectSingleNode(pAppName).Attributes["Version"].Value;
            }
            catch
            {
            }
            return str;
        }

        public static bool GetAttributeBoolean(XmlNode pNode, string pKey, bool pDefult = false)
        {
            bool flag = pDefult;
            try
            {
                flag = Convert.ToBoolean(((XmlElement)pNode).GetAttribute(pKey));
            }
            catch
            {
            }
            return flag;
        }

        public static double GetAttributeDouble(XmlNode pNode, string pKey, int pDefult = 0)
        {
            double num = pDefult;
            try
            {
                num = Convert.ToDouble(((XmlElement)pNode).GetAttribute(pKey));
            }
            catch
            {
            }
            return num;
        }

        public static int GetAttributeInt(XmlNode pNode, string pKey, int pDefult = 0)
        {
            int num = pDefult;
            try
            {
                num = Convert.ToInt32(((XmlElement)pNode).GetAttribute(pKey));
            }
            catch
            {
            }
            return num;
        }

        public static string GetAttributeString(XmlNode pNode, string pKey, string pDefult = "")
        {
            string attribute = pDefult;
            try
            {
                attribute = ((XmlElement)pNode).GetAttribute(pKey);
            }
            catch
            {
            }
            return attribute;
        }

        public static string GetAttributeString(XmlNodeList pNodeList, string pKey1, string pValue1, string pKey2, string pDefult = "")
        {
            string str = pDefult;
            try
            {
                for (int i = 0; i < pNodeList.Count; i++)
                {
                    XmlNode pNode = pNodeList[i];
                    if (GetAttributeString(pNode, pKey1, "") == pValue1)
                    {
                        return GetAttributeString(pNode, pKey2, "");
                    }
                }
            }
            catch
            {
            }
            return str;
        }

        public static int GetBetsCodeCount(List<string> pCodeList, string playName, List<int> pRXWZ)
        {
            int count;
            ConfigurationStatus.PlayBase playInfo = GetPlayInfo(playName);
            int num = -1;
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CheckPlayIsFS(playName))
                {
                    if (pCodeList.Count == 0)
                    {
                        num = 0;
                    }
                    else if (CheckPlayIsDWD(playName))
                    {
                        num = 0;
                        foreach (string str in pCodeList)
                        {
                            if (str != "*")
                            {
                                num += str.Length;
                            }
                        }
                    }
                    else
                    {
                        List<List<string>> list = new List<List<string>>();
                        count = playInfo.IndexList.Count;
                        if (CheckPlayIsRXFS(playName))
                        {
                            if (pCodeList.Count != 5)
                            {
                                return 0;
                            }
                            pCodeList = playInfo.GetRXFSCodeList(pCodeList);
                            list = playInfo.ConvertRXFSCodeList(pCodeList);
                        }
                        else
                        {
                            list.Add(pCodeList);
                        }
                        if (pCodeList.Count < count)
                        {
                            return 0;
                        }
                        num = 0;
                        foreach (List<string> list2 in list)
                        {
                            int num3 = 1;
                            foreach (string str2 in list2)
                            {
                                num3 *= str2.Length;
                            }
                            num += num3;
                        }
                    }
                }
                else if (CheckPlayIsZuXFS(playName))
                {
                    Dictionary<int, int> dictionary;
                    if (CheckPlayIsZu3(playName))
                    {
                        dictionary = new Dictionary<int, int> {
                            [2] = 2,
                            [3] = 6,
                            [4] = 12,
                            [5] = 20,
                            [6] = 30,
                            [7] = 0x2a,
                            [8] = 0x38,
                            [9] = 0x48,
                            [10] = 90
                        };
                        if (dictionary.ContainsKey(pCodeList.Count))
                        {
                            num = dictionary[pCodeList.Count];
                        }
                    }
                    else if (CheckPlayIsZu6(playName))
                    {
                        dictionary = new Dictionary<int, int> {
                            [3] = 1,
                            [4] = 4,
                            [5] = 10,
                            [6] = 20,
                            [7] = 0x23,
                            [8] = 0x38,
                            [9] = 0x54,
                            [10] = 120
                        };
                        if (dictionary.ContainsKey(pCodeList.Count))
                        {
                            num = dictionary[pCodeList.Count];
                        }
                    }
                }
                else if (CheckPlayIsLH(playName))
                {
                    num = pCodeList.Count;
                }
                else
                {
                    num = pCodeList.Count;
                }
                if (!CheckPlayIsRXDS(playName))
                {
                    return num;
                }
                if ((pRXWZ == null) || (pRXWZ.Count <= 0))
                {
                    return num;
                }
                List<List<int>> list3 = playInfo.ConvertRXDSWZList(pRXWZ);
                if (list3 == null)
                {
                    return 0;
                }
                return (num * list3.Count);
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CheckPlayIsDS(playName))
                {
                    return pCodeList.Count;
                }
                if (!CheckPlayIsFS(playName))
                {
                    return num;
                }
                if (playInfo.CodeCount > pCodeList.Count)
                {
                    return 0;
                }
                return GetCombinations(pCodeList, playInfo.CodeCount, "").Count;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return num;
            }
            if (!CheckPlayIsDS(playName))
            {
                if (CheckPlayIsFS(playName))
                {
                    if (pCodeList.Count == 0)
                    {
                        return 0;
                    }
                    if (CheckPlayIsDWD(playName))
                    {
                        num = 0;
                        foreach (string str in pCodeList)
                        {
                            if (str != "*")
                            {
                                num += str.Trim().Split(new char[] { ' ' }).Length;
                            }
                        }
                        return num;
                    }
                    count = playInfo.IndexList.Count;
                    if (pCodeList.Count >= count)
                    {
                        List<List<string>> pList = new List<List<string>>();
                        foreach (string str in pCodeList)
                        {
                            pList.Add(SplitString(str, " ", -1));
                        }
                        List<string> pOutList = new List<string>();
                        MultipleCombination(ref pOutList, pList);
                        num = 0;
                        foreach (string str2 in pOutList)
                        {
                            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
                            for (int i = 0; i < str2.Length; i += 2)
                            {
                                string key = ((i + 2) <= str2.Length) ? str2.Substring(i, 2) : str2.Substring(i);
                                if (!dictionary2.ContainsKey(key))
                                {
                                    dictionary2[key] = "";
                                }
                            }
                            num++;
                        }
                    }
                    return num;
                }
                if (CheckPlayIsHZ(playName))
                {
                    return pCodeList.Count;
                }
            }
            return pCodeList.Count;
        }

        public static string GetBlankString(int pCount) =>
            " ".PadLeft(pCount, ' ');

        public static int GetChildStrCount(string str1, string str2) =>
            Regex.Matches(str1, str2).Count;

        public static string GetCodeByPlay(string pCode, List<int> pCodeList)
        {
            string str = "";
            foreach (int num in pCodeList)
            {
                str = str + pCode[num - 1];
            }
            return str;
        }

        public static List<string> GetCodeList(string pCode)
        {
            List<string> list = new List<string>();
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                return ConvertSameListString(pCode);
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                return SplitString(pCode, ",", -1);
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return Convert11X5Code(SplitString(pCode, ",", -1));
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP3D)
            {
                list = ConvertSameListString(pCode);
            }
            return list;
        }

        public static List<List<int>> GetCodeListByPlay(string playType, string playName, List<int> RXWZ, List<string> pBetsNumberList)
        {
            ConfigurationStatus.PlayBase playInfo = GetPlayInfo(playType, playName);
            string play = playInfo.Play;
            List<List<int>> list = new List<List<int>>();
            if (CheckPlayIsRXDS(play))
            {
                if ((RXWZ != null) && (RXWZ.Count > 0))
                {
                    list = playInfo.ConvertRXDSWZList(RXWZ);
                }
                return list;
            }
            if (CheckPlayIsRXFS(play))
            {
                if (pBetsNumberList != null)
                {
                    list = playInfo.ConvertRXFSWZList(pBetsNumberList);
                }
                return list;
            }
            list.Add(playInfo.IndexList);
            return list;
        }

        public static System.Drawing.Color GetColorByRGB(string pColor)
        {
            List<int> list = SplitInt(pColor, ",");
            return System.Drawing.Color.FromArgb(list[0], list[1], list[2]);
        }

        public static List<string> GetCombinaList(ConfigurationStatus.CombinaType pType, int pCount, int pMin = -1, int pMax = -1)
        {
            int num;
            List<string> list3;
            List<string> list4;
            string str2;
            if (pMin == -1)
            {
                pMin = AppInfo.Current.Lottery.Min;
            }
            if (pMax == -1)
            {
                pMax = AppInfo.Current.Lottery.Max;
            }
            List<string> pOutList = new List<string>();
            if (pType == ConfigurationStatus.CombinaType.ZX)
            {
                List<List<string>> pList = new List<List<string>>();
                for (num = 0; num < pCount; num++)
                {
                    list3 = new List<string>();
                    for (int i = pMin; i <= pMax; i++)
                    {
                        list3.Add(i.ToString());
                    }
                    pList.Add(list3);
                }
                MultipleCombination(ref pOutList, pList);
                return pOutList;
            }
            if (pType == ConfigurationStatus.CombinaType.Z3)
            {
                list4 = GetCombinaList(ConfigurationStatus.CombinaType.ZX, pCount, pMin, pMax);
                foreach (string str in list4)
                {
                    str2 = SortString(str);
                    if (!(pOutList.Contains(str2) || !CheckCodeIsZu3(str2)))
                    {
                        pOutList.Add(str2);
                    }
                }
                return pOutList;
            }
            if (pType == ConfigurationStatus.CombinaType.Z6)
            {
                list4 = GetCombinaList(ConfigurationStatus.CombinaType.ZX, pCount, pMin, pMax);
                foreach (string str in list4)
                {
                    str2 = SortString(str);
                    if (!(pOutList.Contains(str2) || !CheckCodeIsZu6(str2)))
                    {
                        pOutList.Add(str2);
                    }
                }
                return pOutList;
            }
            if (pType != ConfigurationStatus.CombinaType.PX)
            {
                return pOutList;
            }
            list3 = new List<string>();
            for (num = pMin; num <= pMax; num++)
            {
                list3.Add(num.ToString());
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                list3 = Convert11X5Code(list3);
            }
            else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                list3 = Convert11X5Code(list3);
            }
            return GetCombinations(list3, pCount, "");
        }

        public static List<string> GetCombinations(List<string> itemList, int pLength, string pChar = "")
        {
            int num2;
            List<string> list = new List<string>();
            int[] pEndIndices = new int[pLength];
            int index = pLength - 1;
            for (num2 = itemList.Count - 1; num2 > ((itemList.Count - 1) - pLength); num2--)
            {
                pEndIndices[index] = num2;
                index--;
            }
            int[] pIndices = new int[pLength];
            for (num2 = 0; num2 < pLength; num2++)
            {
                pIndices[num2] = num2;
            }
            do
            {
                string item = "";
                for (int i = 0; i < pLength; i++)
                {
                    if (item == "")
                    {
                        item = itemList[pIndices[i]];
                    }
                    else
                    {
                        item = item + pChar + itemList[pIndices[i]];
                    }
                }
                list.Add(item);
            }
            while (GetNext(ref pIndices, ref pEndIndices));
            return list;
        }

        public static ConfigurationStatus.CombinaType GetCombinaType(string playName)
        {
            ConfigurationStatus.CombinaType zX = ConfigurationStatus.CombinaType.ZX;
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName.Contains("复式"))
                {
                    return ConfigurationStatus.CombinaType.ZX;
                }
                if (CheckPlayIsZu3(playName))
                {
                    return ConfigurationStatus.CombinaType.Z3;
                }
                if (CheckPlayIsZu6(playName))
                {
                    zX = ConfigurationStatus.CombinaType.Z6;
                }
                return zX;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                return ConfigurationStatus.CombinaType.PX;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                zX = ConfigurationStatus.CombinaType.PX;
            }
            return zX;
        }

        public static Dictionary<string, string> GetConfiguration(string pValue, string pChar = "\r\n") =>
            ConvertConfiguration(SplitString(pValue, pChar, -1), '=');

        public static string GetControlText(IntPtr hwnd)
        {
            int capacity = 0x186a0;
            StringBuilder lParam = new StringBuilder(capacity);
            SendMessage(hwnd, 13, capacity, lParam);
            return lParam.ToString();
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);
        public static int GetDataGridViewIndexList(DataGridView pList, string pValue, int pIndex = 0)
        {
            foreach (DataGridViewRow row in (IEnumerable)pList.Rows)
            {
                if (row.Cells[pIndex].Value.ToString() == pValue)
                {
                    return row.Index;
                }
            }
            return -1;
        }

        public static string GetDecodeResponse(string pResponse, string pAppName)
        {
            string pSource = pResponse;
            return Decode(pSource, pAppName);
        }

        public static List<string> GetDicKeyList<T>(Dictionary<string, T> pDic)
        {
            List<string> list = new List<string>();
            foreach (string str in pDic.Keys)
            {
                list.Add(str);
            }
            return list;
        }

        public static List<string> GetDicValueList<T>(Dictionary<string, T> pDic)
        {
            List<string> list = new List<string>();
            foreach (string str in pDic.Keys)
            {
                list.Add(pDic[str].ToString());
            }
            return list;
        }

        public static string GetDiskVolumeSerialNumber()
        {
            ManagementObject obj2 = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            obj2.Get();
            return obj2.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        public static string getDllPath() =>
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString());

        public static bool GetFileNameFromOpen(ref string pFileName, string pFilter)
        {
            bool flag = false;
            try
            {
                OpenFileDialog dialog = new OpenFileDialog {
                    Filter = pFilter
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    flag = true;
                    pFileName = dialog.FileName;
                }
                dialog.Dispose();
                dialog = null;
            }
            catch
            {
            }
            return flag;
        }

        public static bool GetFileNameFromOpenMultiselect(ref List<string> pFileName, string pFilter)
        {
            bool flag = false;
            try
            {
                OpenFileDialog dialog = new OpenFileDialog {
                    Filter = pFilter,
                    Multiselect = true
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    flag = true;
                    foreach (string str in dialog.FileNames)
                    {
                        pFileName.Add(str);
                    }
                }
                dialog.Dispose();
                dialog = null;
            }
            catch
            {
            }
            return flag;
        }

        public static bool GetFileNameFromSave(ref string pFileName, string pFilter)
        {
            bool flag = false;
            try
            {
                SaveFileDialog dialog = new SaveFileDialog {
                    Filter = pFilter,
                    FileName = pFileName
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    flag = true;
                    pFileName = dialog.FileName;
                }
            }
            catch
            {
            }
            return flag;
        }

        public static string GetFirstPath(string path)
        {
            string[] strArray = path.Split(new char[] { '\\' });
            return strArray[strArray.Length - 1];
        }

        public static string GetHostLine(string pUrl)
        {
            string str = "";
            List<string> list = SplitString(pUrl, ":", -1);
            if (list.Count == 3)
            {
                str = list[0] + ":" + list[1];
            }
            return str;
        }

        public static List<string> GetIndexCode(List<string> pCodeList, List<int> pIndexList)
        {
            List<string> list = new List<string>();
            foreach (int num in pIndexList)
            {
                list.Add(pCodeList[num - 1]);
            }
            return list;
        }

        public static string GetIndexString(string pStr, string pFind1, string pFind2, int pIndex = 0)
        {
            string str = "";
            try
            {
                if ((pStr == "") || !pStr.Contains(pFind1))
                {
                    return str;
                }
                int length = pFind1.Replace("\"", "-").Replace(@"\", "-").Length;
                int startIndex = pStr.IndexOf(pFind1, pIndex) + length;
                int index = pStr.IndexOf(pFind2, startIndex);
                if (pFind2 == "")
                {
                    index = pStr.Length;
                }
                str = pStr.Substring(startIndex, index - startIndex);
            }
            catch
            {
            }
            return str;
        }

        public static XmlNode GetItemNode(XmlNode pNode, string pName)
        {
            try
            {
                foreach (XmlNode node2 in pNode.ChildNodes)
                {
                    if (GetAttributeString(node2, "Name", "") == pName)
                    {
                        return node2;
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        public static string GetJoinStr(string pStr)
        {
            string str = "";
            for (int i = 0; i < pStr.Length; i++)
            {
                str = str + "\n" + pStr[i];
            }
            return str.Substring(1);
        }

        public static string GetLastID() =>
            "";

        public static string GetLocalIP()
        {
            string pUrl = "http://www.ip.cn/";
            string pReferer = pUrl;
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return GetIndexString(pResponsetext, "当前 IP：<code>", "<", 0);
        }

        public static int GetLotteryExpectLen(string pLotteryName, string pLotteryID)
        {
            int num = 3;
            if ((((pLotteryName.Contains("分分") || pLotteryName.Contains("一分")) || (pLotteryName.Contains("博猫五分") || pLotteryName.Contains("60"))) || pLotteryName.Contains("30")) || pLotteryName.Contains("1分"))
            {
                return 4;
            }
            if (!((!pLotteryName.Contains("11选5") || pLotteryName.Contains("博猫")) || pLotteryName.Contains("乐利")))
            {
                return 2;
            }
            if (pLotteryID == "XJSSC")
            {
                return 2;
            }
            if ((pLotteryID == "MR11X5") || (pLotteryID == "BD11X5"))
            {
                return 4;
            }
            if (pLotteryID == "XXLSSC")
            {
                num = 4;
            }
            return num;
        }

        public static ConfigurationStatus.LotteryGroup GetLotteryGroup(string pLotteryName, string pLotteryID)
        {
            if ((((pLotteryName.Contains("PK10") || pLotteryName.Contains("赛马")) || (pLotteryName.Contains("快艇") || pLotteryName.Contains("飞艇"))) || ((pLotteryName.Contains("赛车") || pLotteryName.Contains("赛狗")) || (pLotteryID == "THOZPK10"))) || pLotteryName.Contains("游泳"))
            {
                return ConfigurationStatus.LotteryGroup.GPPK10;
            }
            if (pLotteryName.Contains("3D"))
            {
                return ConfigurationStatus.LotteryGroup.GP3D;
            }
            if (pLotteryName.Contains("11选5") || pLotteryName.Contains("自行车"))
            {
                return ConfigurationStatus.LotteryGroup.GP11X5;
            }
            return ConfigurationStatus.LotteryGroup.GPSSC;
        }

        public static string GetLotteryID(ConfigurationStatus.LotteryType pLottery)
        {
            foreach (string str2 in AppInfo.Current.LotteryDic.Keys)
            {
                ConfigurationStatus.LotteryConfig config = AppInfo.Current.LotteryDic[str2];
                if (config.Type == pLottery)
                {
                    return config.ID;
                }
            }
            return "";
        }

        public static string GetLotteryName(ConfigurationStatus.LotteryType pLottery)
        {
            foreach (string str2 in AppInfo.Current.LotteryDic.Keys)
            {
                ConfigurationStatus.LotteryConfig config = AppInfo.Current.LotteryDic[str2];
                if (config.Type == pLottery)
                {
                    return config.Name;
                }
            }
            return "";
        }

        public static string GetMdkey(string pUerID = "")
        {
            if (pUerID == "")
            {
                pUerID = "Administrator";
            }
            return Encode(pUerID, "e8we8w8e");
        }

        public static bool GetNext(ref int[] pIndices, ref int[] pEndIndices)
        {
            bool flag = true;
            for (int i = pEndIndices.Length - 1; i > -1; i--)
            {
                if (pIndices[i] < pEndIndices[i])
                {
                    pIndices[i]++;
                    for (int j = 1; (i + j) < pEndIndices.Length; j++)
                    {
                        pIndices[i + j] = pIndices[i] + j;
                    }
                    return flag;
                }
                if (i == 0)
                {
                    flag = false;
                }
            }
            return flag;
        }

        public static List<string> GetNodeAllLotteryList()
        {
            List<string> list = new List<string>();
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(Resources.LotteryData);
                XmlNodeList list2 = document.SelectSingleNode("Lotterys").SelectNodes("Lottery");
                foreach (XmlNode node in list2)
                {
                    list.Add(GetAttributeString(node, "ID", ""));
                }
            }
            catch
            {
            }
            return list;
        }

        public static XmlNode GetNodeLotteryData(string pValue, string pKey = "Name")
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(Resources.LotteryData);
                XmlNodeList childNodes = document.SelectSingleNode("Lotterys").ChildNodes;
                foreach (XmlNode node2 in childNodes)
                {
                    if (GetAttributeString(node2, pKey, "") == pValue)
                    {
                        return node2;
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        public static XmlNode GetNodeLotteryList(string pValue, string pKey = "Name")
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(Resources.LotteryList);
                XmlNodeList childNodes = document.SelectSingleNode("Lotterys").ChildNodes;
                foreach (XmlNode node2 in childNodes)
                {
                    if (GetAttributeString(node2, pKey, "") == pValue)
                    {
                        return node2;
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        public static string GetNumber(string pStr)
        {
            if (pStr == null)
            {
                return "";
            }
            return Regex.Replace(pStr, @"[^\d.\d]", "");
        }

        public static string GetNumber(string pStr, List<string> pSkipList)
        {
            string str = "";
            foreach (char ch in pStr)
            {
                string str2 = ch.ToString();
                if (IsIntNumber(str2) || pSkipList.Contains(str2))
                {
                    str = str + str2;
                }
            }
            return str;
        }

        public static int GetNumberIndex(string pTemplate, string pFormula, string pExpect, string playName, int pNumber)
        {
            if (pNumber == 1)
            {
                return 0;
            }
            return (Convert.ToInt32(MD5(AppInfo.Current.Lottery.Name + pTemplate + pFormula + pExpect + playName)[0]) % pNumber);
        }

        public static ConfigurationStatus.OpenData GetOpenData(string pData, string pLotteryID, ref Dictionary<string, string> PK10ExpectDic)
        {
            string[] strArray = pData.Split(new char[] { '\t' });
            if (strArray.Length != 2)
            {
                return null;
            }
            ConfigurationStatus.OpenData data = new ConfigurationStatus.OpenData();
            string number = strArray[0];
            if (AppInfo.Current.Lottery.Type == ConfigurationStatus.LotteryType.PK10)
            {
                string[] strArray2 = number.Split(new char[] { ' ' });
                PK10ExpectDic[strArray2[1]] = strArray2[0];
                number = strArray2[1];
            }
            data.Expect = number;
            data.Code = strArray[1];
            ConfigurationStatus.LotteryConfig config = AppInfo.Current.LotteryDic[pLotteryID];
            if (!CheckIsSkipLottery(pLotteryID))
            {
                try
                {
                    number = GetNumber(data.Expect);
                    DateTime time = new DateTime(Convert.ToInt32("20" + number.Substring(0, 2)), Convert.ToInt32(number.Substring(2, 2)), Convert.ToInt32(number.Substring(4, 2)));
                    int lotteryExpectLen = GetLotteryExpectLen(config.Name, config.ID);
                    string str2 = Strings.Right(data.Expect, lotteryExpectLen);
                    DateTime time2 = DateTime.Parse(config.TimeDic[str2]);
                    data.Time = new DateTime(time.Year, time.Month, time.Day, time2.Hour, time2.Minute, time2.Second);
                }
                catch
                {
                }
            }
            data.CodeList = GetCodeList(data.Code);
            return data;
        }

        public static ConfigurationStatus.OpenData GetOpenDataByExpect(string pExpect, List<ConfigurationStatus.OpenData> pDataList)
        {
            foreach (ConfigurationStatus.OpenData data2 in pDataList)
            {
                if (data2.Expect == pExpect)
                {
                    return data2;
                }
            }
            return null;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);
        public static bool GetPathFromDialog(ref string pPath, string pName, string pHint = "")
        {
            bool flag = false;
            pPath = "";
            try
            {
                string regPath = @"software\TUHAOPLUS\YXZXGJ\MainConfig\LastOpenPath\";
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                try
                {
                    dialog.SelectedPath = ReadRegString(regPath, pName, "");
                }
                catch
                {
                }
                dialog.Description = "请选择一个路径来保存文件";
                if (pHint != "")
                {
                    dialog.Description = pHint;
                }
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    flag = true;
                    pPath = dialog.SelectedPath;
                    WriteRegValue(regPath, pName, pPath);
                }
                dialog.Dispose();
                dialog = null;
            }
            catch
            {
            }
            return flag;
        }

        public static List<string> GetPathNameList(string path)
        {
            List<string> list = new List<string>();
            if (Directory.Exists(path))
            {
                DirectoryInfo info = new DirectoryInfo(path);
                foreach (FileInfo info2 in info.GetFiles("*.txt"))
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(info2.FullName);
                    list.Add(fileNameWithoutExtension);
                }
            }
            return list;
        }

        public static string GetPlayChar(string playName)
        {
            string str = " ";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CheckPlayIsFS(playName))
                {
                    str = "-";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CheckPlayIsDS(playName))
                {
                    str = ",";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (CheckPlayIsDS(playName))
                {
                    return ",";
                }
                if (CheckPlayIsHZ(playName))
                {
                    return " ";
                }
                if (CheckPlayIsFS(playName))
                {
                    str = "-";
                }
            }
            return str;
        }

        public static ConfigurationStatus.PlayBase GetPlayInfo(ConfigurationStatus.BetsScheme pScheme)
        {
            string playType = pScheme.SchemeInfo.FNBaseInfo.PlayType;
            string playName = pScheme.SchemeInfo.FNBaseInfo.PlayName;
            return GetPlayInfo(playType, playName);
        }

        public static ConfigurationStatus.PlayBase GetPlayInfo(string playName)
        {
            ConfigurationStatus.PlayBase base2 = null;
            foreach (string str in AppInfo.PlayDic.Keys)
            {
                List<ConfigurationStatus.PlayBase> list = AppInfo.PlayDic[str];
                foreach (ConfigurationStatus.PlayBase base3 in list)
                {
                    if ((str + base3.PlayName) == playName)
                    {
                        base2 = base3;
                        break;
                    }
                }
            }
            return base2;
        }

        public static ConfigurationStatus.PlayBase GetPlayInfo(string playType, string playName)
        {
            if (AppInfo.PlayDic.ContainsKey(playType))
            {
                List<ConfigurationStatus.PlayBase> list = AppInfo.PlayDic[playType];
                foreach (ConfigurationStatus.PlayBase base3 in list)
                {
                    if (base3.PlayName == playName)
                    {
                        return base3;
                    }
                }
            }
            return null;
        }

        public static List<string> GetPlayNameList(string pType)
        {
            List<string> list = new List<string>();
            List<ConfigurationStatus.PlayBase> list2 = AppInfo.PlayDic[pType];
            foreach (ConfigurationStatus.PlayBase base2 in list2)
            {
                list.Add(base2.PlayName);
            }
            return list;
        }

        public static int GetPlayNum(string playName)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int> {
                {
                    "一",
                    1
                },
                {
                    "二",
                    2
                },
                {
                    "三",
                    3
                },
                {
                    "四",
                    4
                },
                {
                    "五",
                    5
                },
                {
                    "六",
                    6
                },
                {
                    "七",
                    7
                },
                {
                    "八",
                    8
                },
                {
                    "九",
                    9
                },
                {
                    "十",
                    10
                },
                {
                    "冠军",
                    1
                },
                {
                    "亚军",
                    2
                }
            };
            foreach (string str in dictionary.Keys)
            {
                if (playName.Contains(str))
                {
                    return dictionary[str];
                }
            }
            return -1;
        }

        public static int GetRadioButtonIndex(List<RadioButton> pRadioButtonList)
        {
            int num = -1;
            for (int i = 0; i < pRadioButtonList.Count; i++)
            {
                if (pRadioButtonList[i].Checked)
                {
                    num = i;
                }
            }
            return num;
        }

        public static List<string> GetRandomByPlay(string playType, string playName, string pTemplate, string pExpect, int pCycle, int pCodeCount)
        {
            List<string> list = new List<string>();
            ConfigurationStatus.PlayBase playInfo = GetPlayInfo(playType, playName);
            int codeCount = playInfo.CodeCount;
            string play = playInfo.Play;
            string playChar = GetPlayChar(play);
            string pFormula = "公式1";
            if (pExpect == "")
            {
                pExpect = AppInfo.Current.Lottery.NextExpect;
            }
            for (int i = 0; i < pCycle; i++)
            {
                List<string> list2;
                int num3;
                string str6;
                List<string> list3;
                string item = "";
                string str5 = (pTemplate == "") ? GetRandomSeed().ToString() : pTemplate;
                str5 = str5 + i.ToString();
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
                {
                    if (CheckPlayIsFS(play))
                    {
                        if (CheckPlayIsRXFS(play))
                        {
                            codeCount = 5;
                        }
                        list2 = new List<string>();
                        if (pCodeCount > 10)
                        {
                            pCodeCount = 10;
                        }
                        num3 = 0;
                        while (num3 < codeCount)
                        {
                            str6 = "";
                            GetRandomNum(ref str6, str5, pFormula, pExpect, play, pCodeCount, num3);
                            list2.Add(str6);
                            num3++;
                        }
                        item = Join(list2, playChar);
                    }
                    else if (CheckPlayIsLH(play))
                    {
                        list3 = ConvertSameList("龙虎和");
                        if (pCodeCount > list3.Count)
                        {
                            pCodeCount = list3.Count;
                        }
                        item = JoinLH(GetRandomNum(list3, str5, pFormula, pExpect, play, pCodeCount), playChar, -1, true);
                    }
                    else
                    {
                        list3 = GetCombinaList(GetCombinaType(play), codeCount, -1, -1);
                        if (pCodeCount > list3.Count)
                        {
                            pCodeCount = list3.Count;
                        }
                        item = Join(GetRandomNum(list3, str5, pFormula, pExpect, play, pCodeCount), playChar, -1, true);
                    }
                }
                else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                {
                    if (CheckPlayIsDS(play))
                    {
                        list3 = Convert11X5Code(GetCombinaList(GetCombinaType(play), codeCount, -1, -1));
                        if (pCodeCount > list3.Count)
                        {
                            pCodeCount = list3.Count;
                        }
                        item = Join(GetRandomNum(list3, str5, pFormula, pExpect, play, pCodeCount), playChar, -1, true);
                    }
                    else
                    {
                        list3 = Convert11X5Code(GetCombinaList(GetCombinaType(play), 1, -1, -1));
                        if (pCodeCount > list3.Count)
                        {
                            pCodeCount = list3.Count;
                        }
                        item = Join(GetRandomNum(list3, str5, pFormula, pExpect, play, pCodeCount), playChar, -1, true);
                    }
                }
                else if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                {
                    if (CheckPlayIsDS(play))
                    {
                        list3 = Convert11X5Code(GetCombinaList(GetCombinaType(play), codeCount, -1, -1));
                        if (pCodeCount > list3.Count)
                        {
                            pCodeCount = list3.Count;
                        }
                        item = Join(GetRandomNum(list3, str5, pFormula, pExpect, play, pCodeCount), playChar, -1, true);
                    }
                    else if (CheckPlayIsHZ(play))
                    {
                        list3 = AppInfo.CombinaDicPK10HZ["和值" + playInfo.IndexList.Count];
                        if (pCodeCount > list3.Count)
                        {
                            pCodeCount = list3.Count;
                        }
                        item = Join(GetRandomNum(list3, str5, pFormula, pExpect, play, pCodeCount), playChar, -1, true);
                    }
                    else
                    {
                        list2 = new List<string>();
                        if (pCodeCount > 10)
                        {
                            pCodeCount = 10;
                        }
                        for (num3 = 0; num3 < codeCount; num3++)
                        {
                            str6 = Join(GetRandomNum(GetCombinaList(GetCombinaType(play), 1, -1, -1), str5 + num3, pFormula, pExpect, play, pCodeCount), " ", -1, true);
                            list2.Add(str6);
                        }
                        item = Join(list2, playChar);
                    }
                }
                list.Add(item);
            }
            return list;
        }

        public static int GetRandomNum(string pFormula, string pTemplate, string pExpect, string playName, int pExtra)
        {
            string str2 = MD5(string.Concat(new object[] { AppInfo.Current.Lottery.Name, pTemplate, pFormula, pExpect, playName, pExtra }));
            string str3 = pExtra.ToString();
            string str4 = "";
            for (int i = 0; i < str3.Length; i++)
            {
                str4 = str4 + ((Convert.ToInt32(str2[i]) % 10)).ToString();
            }
            return (Convert.ToInt32(str4) % pExtra);
        }

        public static Dictionary<string, string> GetRandomNum(List<string> pList, string pTemplate, string pFormula, string pExpect, string playName, int pNumber)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> list = CopyList(pList);
            while (dictionary.Count < pNumber)
            {
                int index = GetRandomNum(pTemplate, pFormula, pExpect, playName, list.Count);
                dictionary[list[index]] = "";
                list.RemoveAt(index);
            }
            return dictionary;
        }

        public static void GetRandomNum(ref string pValue, string pTemplate, string pFormula, string pExpect, string playName, int pNumber)
        {
            if (pNumber != 0)
            {
                string str2 = MD5(AppInfo.Current.Lottery.Name + pTemplate + pFormula + pExpect + playName);
                for (int i = 0; i < pNumber; i++)
                {
                    int num2 = Convert.ToInt32(str2[i]) % 10;
                    while (pValue.Contains(num2.ToString()))
                    {
                        num2 = (num2 + 1) % 10;
                    }
                    pValue = pValue + num2;
                }
                pValue = SortString(pValue);
            }
        }

        public static void GetRandomNum(ref string pValue, string pTemplate, string pFormula, string pExpect, string playName, int pNumber, int pExtra = -1)
        {
            if (pNumber != 0)
            {
                string str2 = MD5(string.Concat(new object[] { AppInfo.Current.Lottery.Name, pTemplate, pFormula, playName, pExpect, pExtra }));
                for (int i = 0; i < pNumber; i++)
                {
                    int num2 = Convert.ToInt32(str2[i]) % 10;
                    while (pValue.Contains(num2.ToString()))
                    {
                        num2 = (num2 + 1) % 10;
                    }
                    pValue = pValue + num2;
                }
                pValue = SortString(pValue);
            }
        }

        public static void GetRandomNumBy11X5(ref string pValue, string pTemplate, string pFormula, string pExpect, string playName, int pNumber)
        {
            if (pNumber != 0)
            {
                string str2 = MD5(AppInfo.Current.Lottery.Name + pTemplate + pFormula + pExpect + playName);
                List<string> pList = new List<string>();
                for (int i = 0; i < pNumber; i++)
                {
                    int pCode = (Convert.ToInt32(str2[i]) % 11) + 1;
                    string item = Convert11X5Code(pCode);
                    while (pList.Contains(item))
                    {
                        pCode++;
                        if (pCode == 12)
                        {
                            pCode = 1;
                        }
                        item = Convert11X5Code(pCode);
                    }
                    pList.Add(item);
                }
                pList.Sort();
                pValue = Join(pList, " ");
            }
        }

        public static void GetRandomNumBy11X5(ref string pValue, string pTemplate, string pFormula, string pExpect, string playName, int pNumber, int pExtra = -1)
        {
            if (pNumber != 0)
            {
                string str2 = MD5(string.Concat(new object[] { AppInfo.Current.Lottery.Name, pTemplate, pFormula, playName, pExpect, pExtra }));
                List<string> pList = new List<string>();
                for (int i = 0; i < pNumber; i++)
                {
                    int pCode = (Convert.ToInt32(str2[i]) % 11) + 1;
                    string item = Convert11X5Code(pCode);
                    while (pList.Contains(item))
                    {
                        pCode++;
                        if (pCode == 12)
                        {
                            pCode = 1;
                        }
                        item = Convert11X5Code(pCode);
                    }
                    pList.Add(item);
                }
                pList.Sort();
                pValue = Join(pList, " ");
            }
        }

        public static void GetRandomNumByPK10(ref string pValue, string pTemplate, string pFormula, string pExpect, string playName, int pNumber)
        {
            if (pNumber != 0)
            {
                string str2 = MD5(AppInfo.Current.Lottery.Name + pTemplate + pFormula + pExpect + playName);
                List<string> pList = new List<string>();
                for (int i = 0; i < pNumber; i++)
                {
                    int pCode = (Convert.ToInt32(str2[i]) % 10) + 1;
                    string item = Convert11X5Code(pCode);
                    while (pList.Contains(item))
                    {
                        pCode++;
                        if (pCode == 11)
                        {
                            pCode = 1;
                        }
                        item = Convert11X5Code(pCode);
                    }
                    pList.Add(item);
                }
                pList.Sort();
                pValue = Join(pList, " ");
            }
        }

        public static void GetRandomNumByPK10(ref string pValue, string pTemplate, string pFormula, string pExpect, string playName, int pNumber, int pExtra = -1)
        {
            if (pNumber != 0)
            {
                string str2 = MD5(string.Concat(new object[] { AppInfo.Current.Lottery.Name, pTemplate, pFormula, playName, pExpect, pExtra }));
                List<string> pList = new List<string>();
                for (int i = 0; i < pNumber; i++)
                {
                    int pCode = (Convert.ToInt32(str2[i]) % 10) + 1;
                    string item = Convert11X5Code(pCode);
                    while (pList.Contains(item))
                    {
                        pCode++;
                        if (pCode == 11)
                        {
                            pCode = 1;
                        }
                        item = Convert11X5Code(pCode);
                    }
                    pList.Add(item);
                }
                pList.Sort();
                pValue = Join(pList, " ");
            }
        }

        public static int GetRandomSeed()
        {
            byte[] data = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(data);
            return BitConverter.ToInt32(data, 0);
        }

        public static string GetShareSchemeHint(string pHint, int pSchemeCount, int pBTFNCount, bool pIsAsk)
        {
            string format = "{0}【{1}】个方案{2}{3}";
            string str2 = pIsAsk ? "？" : "！";
            string str3 = (pBTFNCount == 0) ? "" : $"和【{pBTFNCount}】个高级倍投方案";
            return string.Format(format, new object[] { pHint, pSchemeCount, str3, str2 });
        }

        public static string GetStringFromInputBox(string pText, string pValue = "", string pTitle = "", bool pIsPW = false)
        {
            string outValue = "";
            if (pTitle == "")
            {
                pTitle = "永信在线挂机软件";
            }
            FrmInputBox box = new FrmInputBox(pText, pValue, pTitle, pIsPW);
            if (box.ShowDialog() == DialogResult.Yes)
            {
                outValue = FrmInputBox.OutValue;
            }
            return outValue;
        }

        public static string GetSystemVersion()
        {
            Version version = Environment.OSVersion.Version;
            if ((version.Major == 5) && (version.Minor == 1))
            {
                return "WindowXP";
            }
            if ((version.Major == 5) && (version.Minor == 0))
            {
                return "Window2000";
            }
            if ((version.Major == 6) && (version.Minor == 0))
            {
                return "WindowVista";
            }
            if ((version.Major == 6) && (version.Minor == 1))
            {
                return "Window7";
            }
            if ((version.Major == 6) && (version.Minor == 2))
            {
                return "Window8";
            }
            return "未知";
        }

        public static HtmlElement GetWebHtmlElement(HtmlDocument pDocument, string pID)
        {
            HtmlElement elementById = pDocument.GetElementById(pID);
            if (elementById == null)
            {
                foreach (HtmlWindow window in pDocument.Window.Frames)
                {
                    return GetWebHtmlElement(window.Document, pID);
                }
            }
            return elementById;
        }

        private HtmlElement GetWebHtmlElement(HtmlDocument pDocument, string pInnerText, string pInnerHtml)
        {
            for (int i = 0; i < pDocument.All.Count; i++)
            {
                HtmlElement element2 = pDocument.All[i];
                if ((element2.InnerText == pInnerText) && element2.InnerHtml.Contains(pInnerHtml))
                {
                    return element2;
                }
            }
            return null;
        }

        public static HtmlElement GetWebHtmlElement(HtmlDocument pDocument, string pKey, string pValue, bool pIsEqual = true, bool pClick = false)
        {
            HtmlElement element = null;
            HtmlElementCollection all = pDocument.All;
            List<string> list = new List<string>();
            for (int i = 0; i < all.Count; i++)
            {
                HtmlElement element2 = all[i];
                string item = element2.GetAttribute(pKey).ToString().Trim();
                if (item != "")
                {
                    list.Add(item);
                }
                if (pIsEqual)
                {
                    if (item == pValue)
                    {
                        element = element2;
                        break;
                    }
                }
                else if (item.Contains(pValue))
                {
                    element = element2;
                    break;
                }
            }
            if ((element != null) && pClick)
            {
                element.InvokeMember("click");
            }
            return element;
        }

        public static HtmlElement GetWebHtmlElement(HtmlDocument pDocument, string pName, string pKey, string pValue, bool pIsEqual = true, bool pClick = false)
        {
            HtmlElement element = null;
            HtmlElementCollection elementsByTagName = pDocument.GetElementsByTagName(pName);
            for (int i = 0; i < elementsByTagName.Count; i++)
            {
                HtmlElement element2 = elementsByTagName[i];
                string str = element2.GetAttribute(pKey).ToString().Trim();
                if (pIsEqual)
                {
                    if (str == pValue)
                    {
                        element = element2;
                        break;
                    }
                }
                else if (str.Contains(pValue))
                {
                    element = element2;
                    break;
                }
            }
            if ((element != null) && pClick)
            {
                element.InvokeMember("click");
            }
            return element;
        }

        public static void GetWebImage(PictureBox pPictureBox, string pServer, string path, string pFile, bool pOverwrite = true)
        {
            string str = path + pFile;
            if (!(!pOverwrite && System.IO.File.Exists(str)))
            {
                DownloadFile($"{pServer}//{pFile}", str);
            }
            if (System.IO.File.Exists(str))
            {
                pPictureBox.Image = Image.FromFile(str);
            }
        }

        public static void GetWebImage(ref List<Image> pImageList, string pServer, string path, string pFile, bool pOverwrite = true)
        {
            string str = path + pFile;
            if (!(!pOverwrite && System.IO.File.Exists(str)))
            {
                DownloadFile($"{pServer}//{pFile}", str);
            }
            if (System.IO.File.Exists(str))
            {
                pImageList.Add(Image.FromFile(str));
            }
        }

        public static string GetWebUpdateListFile(string pAppName)
        {
            string str = "UpdateList.xml";
            List<string> list = new List<string> {
                "FNHJ",
                "Manage",
                "TUHAO",
                "WSTD",
                "BFCTGJ",
                "ZZYGJ",
                "TTZGJ"
            };
            if (!list.Contains(pAppName))
            {
                str = pAppName + str;
            }
            return str;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr handle, ref Rectangle lpRect);
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref int lpdwProcessId);
        public static string getWinFormName() =>
            Path.GetFileNameWithoutExtension(getWinFormPath());

        public static string getWinFormPath() =>
            Process.GetCurrentProcess().MainModule.FileName.Replace(".vshost", "");

        public static void InitialProgress(Form pForm, string pHint, float pHandCount, int pMaxCount)
        {
            try
            {
                if (pHandCount >= pMaxCount)
                {
                    AppInfo.PregressHint = new FrmProgress();
                    AppInfo.PregressHint.Show(new WindowWrap(pForm.Handle));
                    AppInfo.PregressHint.HandCount = pHandCount;
                    AppInfo.PregressHint.ParentWindow = pForm;
                    AppInfo.PregressHint.ParentWindow.Enabled = false;
                    AppInfo.PregressHint.NeedShowBar = true;
                    AppInfo.PregressHint.SetHint(pHint);
                }
            }
            catch
            {
            }
        }

        public static bool IsBreakExpect(ref List<string> pList)
        {
            int num2;
            int num = -1;
            string str = @"\BreakExpectList";
            for (num2 = 0; num2 < 0x6fe; num2++)
            {
                string str2 = pList[num2];
                string str3 = pList[num2 + 1];
                int num3 = Convert.ToInt32(str2.Split(new char[] { '\t' })[0].Split(new char[] { ' ' })[1]);
                int num5 = Convert.ToInt32(str3.Split(new char[] { '\t' })[0].Split(new char[] { ' ' })[1]) + 1;
                if (num3 != num5)
                {
                    num = num2;
                    break;
                }
            }
            if (num != -1)
            {
                string item = pList[num].Split(new char[] { '\t' })[0].Split(new char[] { ' ' })[1];
                string regPath = @"software\TUHAOPLUS\YXZXGJ\DlgConfig\永信在线挂机软件" + str;
                List<string> pSearchList = new List<string>();
                ReadRegArrString(regPath, ref pSearchList);
                if (pSearchList.Contains(item))
                {
                    return false;
                }
                for (num2 = num; num2 >= 0; num2--)
                {
                    pList.RemoveAt(num2);
                }
                pSearchList.Add(item);
                ClearSubKey(regPath);
                foreach (string str6 in pSearchList)
                {
                    WriteRegValue(regPath, str6, str6);
                }
                return true;
            }
            return false;
        }

        public static bool IsIntNumber(string pStr)
        {
            int num;
            return int.TryParse(pStr, out num);
        }

        public static bool IsNumeric(string pStr) =>
            Regex.IsMatch(pStr, @"^[+-]?\d*[.]?\d*$");

        public static bool IsNumeric(string pStr, out int pValue) =>
            int.TryParse(pStr, out pValue);

        public static string Join(List<string> pList)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str in pList)
            {
                builder.Append(str);
            }
            return builder.ToString();
        }

        public static string Join<T>(List<T> pList)
        {
            string str = "";
            foreach (T local in pList)
            {
                str = str + local;
            }
            return str;
        }

        public static string Join<T>(T[] pList)
        {
            string str = "";
            foreach (T local in pList)
            {
                str = str + local;
            }
            return str;
        }

        public static string Join(Dictionary<string, string> pDic, string pChar)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str in pDic.Keys)
            {
                string str2 = str + "=" + pDic[str];
                if (builder.Length == 0)
                {
                    builder.Append(str2);
                }
                else
                {
                    builder.Append(pChar + str2);
                }
            }
            return builder.ToString();
        }

        public static string Join(List<double> pList, string pChar)
        {
            StringBuilder builder = new StringBuilder();
            foreach (double num in pList)
            {
                if (builder.Length == 0)
                {
                    builder.Append(num);
                }
                else
                {
                    builder.Append(pChar + num);
                }
            }
            return builder.ToString();
        }

        public static string Join(List<int> pList, string pChar)
        {
            StringBuilder builder = new StringBuilder();
            foreach (int num in pList)
            {
                if (builder.Length == 0)
                {
                    builder.Append(num.ToString());
                }
                else
                {
                    builder.Append(pChar + num.ToString());
                }
            }
            return builder.ToString();
        }

        public static string Join(List<string> pList, string pChar)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str in pList)
            {
                if (builder.Length == 0)
                {
                    builder.Append(str);
                }
                else
                {
                    builder.Append(pChar + str);
                }
            }
            return builder.ToString();
        }

        public static string Join<T>(T[] pList, string pChar)
        {
            string str = "";
            foreach (T local in pList)
            {
                if (str == "")
                {
                    str = local.ToString();
                }
                else
                {
                    str = str + pChar + local;
                }
            }
            return str;
        }

        public static string Join(List<string> pList, string pChar, int pBegin)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = pBegin; i < pList.Count; i++)
            {
                string str = pList[i];
                if (builder.Length == 0)
                {
                    builder.Append(str.ToString());
                }
                else
                {
                    builder.Append(pChar + str.ToString());
                }
            }
            return builder.ToString();
        }

        public static string Join(string pStr, string pChar, int pCount = -1)
        {
            string str = "";
            for (int i = 0; i < pStr.Length; i++)
            {
                char ch = pStr[i];
                if (str == "")
                {
                    str = ch.ToString();
                }
                else if ((pCount != -1) && ((i % pCount) != 0))
                {
                    str = str + ch;
                }
                else
                {
                    str = str + pChar + ch;
                }
            }
            return str;
        }

        public static string Join(Dictionary<string, string> pDic, string pChar, int pNewLine, bool pIsSort)
        {
            StringBuilder builder = new StringBuilder();
            int num = 0;
            List<string> list = new List<string>();
            foreach (string str in pDic.Keys)
            {
                list.Add(str);
            }
            if (pIsSort)
            {
                list.Sort(delegate (string value1, string value2) {
                    int num2 = Convert.ToInt32(CommFunc.GetNumber(value1));
                    int num3 = Convert.ToInt32(CommFunc.GetNumber(value2));
                    return num2 - num3;
                });
            }
            foreach (string str in list)
            {
                string str2 = ((pNewLine != -1) && ((num % pNewLine) == 0)) ? "\r\n" : pChar;
                num++;
                if (builder.Length == 0)
                {
                    builder.Append(str);
                }
                else
                {
                    builder.Append(str2 + str);
                }
            }
            return builder.ToString();
        }

        public static string Join(List<string> pList, string pChar, int pNewLine, bool pIsSort)
        {
            StringBuilder builder = new StringBuilder();
            int num = 0;
            List<string> list = new List<string>();
            foreach (string str in pList)
            {
                list.Add(str);
            }
            if (pIsSort)
            {
                list.Sort(delegate (string value1, string value2) {
                    int num2 = Convert.ToInt32(GetNumber(value1));
                    int num3 = Convert.ToInt32(GetNumber(value2));
                    return num2 - num3;
                });
            }
            foreach (string str in list)
            {
                string str2 = ((pNewLine != -1) && ((num % pNewLine) == 0)) ? "\r\n" : pChar;
                num++;
                if (builder.Length == 0)
                {
                    builder.Append(str);
                }
                else
                {
                    builder.Append(str2 + str);
                }
            }
            return builder.ToString();
        }

        public static string JoinLH(Dictionary<string, string> pDic, string pChar, int pNewLine, bool pIsSort)
        {
            StringBuilder builder = new StringBuilder();
            int num = 0;
            List<string> list = new List<string>();
            foreach (string str in pDic.Keys)
            {
                list.Add(str);
            }
            if (pIsSort)
            {
                list.Sort(delegate (string value1, string value2) {
                    int num2 = AppInfo.LHDic[value1];
                    int num3 = AppInfo.LHDic[value2];
                    return num2 - num3;
                });
            }
            foreach (string str in list)
            {
                string str2 = ((pNewLine != -1) && ((num % pNewLine) == 0)) ? "\r\n" : pChar;
                num++;
                if (builder.Length == 0)
                {
                    builder.Append(str);
                }
                else
                {
                    builder.Append(str2 + str);
                }
            }
            return builder.ToString();
        }

        public static string JoinList(string pStr, string pChar, string pLeftChar, string pRightChar)
        {
            List<string> list = SplitString(pStr, pChar, -1);
            List<string> pList = new List<string>();
            foreach (string str in list)
            {
                string item = $"{pLeftChar}{str}{pRightChar}";
                pList.Add(item);
            }
            return Join(pList, pChar);
        }

        public static string JoinStr(string pStr)
        {
            string str = "";
            string[] strArray = pStr.Split(new char[] { ',' });
            string str2 = strArray[0];
            string str3 = strArray[0];
            for (int i = 1; i < strArray.Length; i++)
            {
                if ((Convert.ToInt32(strArray[i]) - Convert.ToInt32(str3)) == 1)
                {
                    str3 = strArray[i];
                }
                else
                {
                    if ((Convert.ToInt32(str3) - Convert.ToInt32(str2)) > 1)
                    {
                        str = str + "," + str2 + "-" + str3;
                    }
                    else if ((Convert.ToInt32(str3) - Convert.ToInt32(str2)) > 0)
                    {
                        str = str + "," + str2 + "," + str3;
                    }
                    else
                    {
                        str = str + "," + str2;
                    }
                    str2 = str3 = strArray[i];
                }
            }
            if ((Convert.ToInt32(str3) - Convert.ToInt32(str2)) > 1)
            {
                str = str + "," + str2 + "-" + str3;
            }
            else if ((Convert.ToInt32(str3) - Convert.ToInt32(str2)) > 0)
            {
                str = str + "," + str2 + "," + str3;
            }
            else
            {
                str = str + "," + str2;
            }
            return str.Substring(1);
        }

        public static bool LoadConfiguration()
        {
            AppInfo.Account.Configuration = new ConfigurationStatus.AppConfiguration();
            string configurationFile = ConfigurationFile;
            if (System.IO.File.Exists(configurationFile))
            {
                Dictionary<string, string> pDic = ReadTextFileToDic(configurationFile);
                AppInfo.Account.Configuration.LoadAppConfiguration(pDic);
                if ((AppInfo.Account.AppName == AppInfo.Account.Configuration.CompanyKey) && (AppInfo.Account.Configuration.CompanyValue != ""))
                {
                    AppInfo.Account.AppName = AppInfo.Account.AppName + "-" + AppInfo.Account.Configuration.CompanyValue;
                }
            }
            Dictionary<string, string> appConfiguration = GetAppConfiguration(AppInfo.Account.AppPerName, "");
            if (appConfiguration.Count == 0)
            {
            }
            AppInfo.Account.Configuration.LoadAppConfiguration(appConfiguration);
            if (AppInfo.Account.IsDL)
            {
                string pHint = "";
                if (!SQLData.GetDLUser(AppInfo.Account.AppName, AppInfo.Account, ref pHint))
                {
                    PublicMessageAll("加载配置失败，请设置正确的代理ID！", true, MessageBoxIcon.Asterisk, "");
                    return false;
                }
                AppInfo.Account.Configuration.DLConfiguration = AppInfo.Account.ConfigurationString;
            }
            if (AppInfo.Account.Configuration.OnlyLogin)
            {
                AppInfo.Account.Configuration.IsPTLogin = false;
            }
            return true;
        }

        public static string LotteryJoinInt(List<int> pList)
        {
            string str = "";
            int num = pList[0];
            int num2 = pList[0];
            for (int i = 1; i < pList.Count; i++)
            {
                if ((pList[i] - num2) == 1)
                {
                    num2 = pList[i];
                }
                else
                {
                    if ((num2 - num) > 1)
                    {
                        str = string.Concat(new object[] { str, ",", num, "-", num2 });
                    }
                    else if ((num2 - num) > 0)
                    {
                        str = string.Concat(new object[] { str, ",", num, ",", num2 });
                    }
                    else
                    {
                        str = str + "," + num;
                    }
                    num = num2 = pList[i];
                }
            }
            if ((num2 - num) > 1)
            {
                str = string.Concat(new object[] { str, ",", num, "-", num2 });
            }
            else if ((num2 - num) > 0)
            {
                str = string.Concat(new object[] { str, ",", num, ",", num2 });
            }
            else
            {
                str = str + "," + num;
            }
            return str.Substring(1);
        }

        public static List<double> LottrySplitDouble(string pStr)
        {
            List<double> list = new List<double>();
            pStr = pStr.Trim();
            if (pStr == "")
            {
                return list;
            }
            return SplitDouble(LottrySplitStr(pStr, ','), ",");
        }

        public static List<int> LottrySplitInt(string pStr)
        {
            List<int> list = new List<int>();
            pStr = pStr.Trim();
            if (pStr == "")
            {
                return list;
            }
            return SplitInt(LottrySplitStr(pStr, ','), ",");
        }

        public static string LottrySplitStr(string pStr, char pChar = ',')
        {
            string str = "";
            string[] strArray = pStr.Split(new char[] { pChar });
            foreach (string str2 in strArray)
            {
                string str3 = str2;
                string[] strArray2 = str3.Split(new char[] { '-' });
                int index = 0;
                while (index < strArray2.Length)
                {
                    strArray2[index] = GetNumber(strArray2[index]);
                    index++;
                }
                if (strArray2.Length > 1)
                {
                    str3 = "";
                    int num2 = Convert.ToInt32(strArray2[0]);
                    int num3 = Convert.ToInt32(strArray2[1]);
                    for (index = num2; index <= num3; index++)
                    {
                        str3 = str3 + "," + index;
                    }
                    str3 = str3.Substring(1);
                }
                str = str + "," + str3;
            }
            return str.Substring(1);
        }

        public static List<string> LottrySplitString(string pStr)
        {
            List<string> list = new List<string>();
            pStr = pStr.Trim();
            if (pStr == "")
            {
                return list;
            }
            return SplitString(LottrySplitStr(pStr, ','), ",", -1);
        }

        public static string LZMA7zipDecode(string pSource) => SevenZip.LzmaAlone.Decoder(pSource);
        //LzmaHelper.unlzma(pSource);

        public static string LZMA7zipEncode(string pSource) => SevenZip.LzmaAlone.Encoder(pSource);

        //LzmaHelper.lzma(pSource);

        public static string LZMADecode(string pSource) => 
            SevenZip.LzmaAlone.Decoder(pSource);

        public static string LZMAEncode(string pSource) => 
            SevenZip.LzmaAlone.Encoder(pSource);

        public static string MD5(string encryptString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(encryptString + AppInfo.Account.AppName);
            System.Security.Cryptography.MD5 md = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md.ComputeHash(bytes)).Replace("-", "");
        }

        public static string MD52(string ConvertString)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            return BitConverter.ToString(provider.ComputeHash(Encoding.Default.GetBytes(ConvertString)), 4, 8).Replace("-", "").ToLower();
        }

        public static string MixRandom(int pLen)
        {
            string str = "0123456789abcdefghigklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ";
            string str2 = "";
            for (int i = 0; i < pLen; i++)
            {
                System.Random random = new System.Random(GetRandomSeed());
                str2 = str2 + str.Substring(random.Next(0x3d), 1);
            }
            return str2;
        }

        public static void MultipleCombination(ref List<string> pOutList, List<List<string>> pList)
        {
            int[] numArray = new int[pList.Count];
            while (numArray[0] < pList[0].Count)
            {
                string item = "";
                int index = 0;
                while (index < pList.Count)
                {
                    string str2 = pList[index][numArray[index]];
                    item = item + str2;
                    index++;
                }
                pOutList.Add(item);
                numArray[numArray.Length - 1]++;
                for (index = numArray.Length - 1; index > 0; index--)
                {
                    if (numArray[index] == pList[index].Count)
                    {
                        numArray[index] = 0;
                        numArray[index - 1]++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        [DllImport("OLEACC.DLL")]
        public static extern int ObjectFromLresult(int lResult, ref Guid riid, int wParam, [In, Out, MarshalAs(UnmanagedType.Interface)] ref object ppvObject);
        [DllImport("user32.dll")]
        public static extern bool OffsetRect(ref Rectangle lprc, int dx, int dy);
        public static void OpenDirectory(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch
            {
                PublicMessageAll("该目录无效或者已被删除", true, MessageBoxIcon.Asterisk, "");
            }
        }

        public static void OpenFile(string pFile)
        {
            try
            {
                Process.Start(pFile);
            }
            catch
            {
                PublicMessageAll("该文件无效或者已被删除", true, MessageBoxIcon.Asterisk, "");
            }
        }

        public static void OpenQQWeb(string pQQ)
        {
            OpenWeb($"http://wpa.qq.com/msgrd?v=1&uin={pQQ}&Site=ioshenmue&Menu=yes");
        }

        public static void OpenWeb(string pUrl)
        {
            try
            {
                Process.Start(pUrl);
            }
            catch
            {
                try
                {
                    Process.Start("iexplore.exe", pUrl);
                }
                catch
                {
                }
            }
        }

        public static void OpenWebByIE(string pUrl)
        {
            new Process { StartInfo = { 
                FileName = "iexplore.exe",
                Arguments = pUrl
            } }.Start();
        }

        public static void PasteText(RichTextBox pTextBox)
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (dataObject.GetDataPresent(DataFormats.Text))
            {
                pTextBox.Text = dataObject.GetData(DataFormats.Text).ToString();
            }
            pTextBox.SelectAll();
            pTextBox.Focus();
        }

        public static void PasteText(TextBox pTextBox)
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (dataObject.GetDataPresent(DataFormats.Text))
            {
                pTextBox.Text = dataObject.GetData(DataFormats.Text).ToString();
            }
            pTextBox.SelectAll();
            pTextBox.Focus();
        }

        [DllImport("gdi32.dll")]
        public static extern bool PatBlt(IntPtr hdc, int nXLeft, int nYLeft, int nWidth, int nHeight, int dwRop);
        public static void PlaySound(UnmanagedMemoryStream pFile)
        {
            new SoundPlayer(pFile).Play();
        }

        public static void PlaySound(string pName)
        {
            string path = (getDllPath() + @"\Sound\") + pName + ".wav";
            if (System.IO.File.Exists(path) && AppInfo.PlaySound)
            {
                new SoundPlayer(path).Play();
            }
        }

        public static void PublicMessage(string xHintStr, MessageBoxIcon pIcon = MessageBoxIcon.Asterisk, string xTitleId = "")
        {
            if (xTitleId == "")
            {
                xTitleId = "永信在线挂机软件";
            }
            MessageBox.Show(xHintStr, xTitleId, MessageBoxButtons.OK, pIcon);
        }

        public static void PublicMessageAll(string xHintStr, bool pIsAutoColse = true, MessageBoxIcon pIcon = MessageBoxIcon.Asterisk, string xTitleId = "")
        {
            if (xTitleId == "")
            {
                xTitleId = "永信在线挂机软件";
            }
            new FrmMessageBox(xHintStr, xTitleId, false, pIsAutoColse).ShowDialog();
        }

        public static int Random(int pMin, int pMax)
        {
            System.Random random = new System.Random(GetRandomSeed());
            return random.Next(pMin, pMax + 1);
        }

        public static void Random<T>(List<T> pList1, List<T> pList2, int pSelectCount, bool pSort = true)
        {
            if (pList2 != null)
            {
                pList2.Clear();
                System.Random random = new System.Random(GetRandomSeed());
                while (true)
                {
                    int index = random.Next(0, pList1.Count);
                    pList2.Add(pList1[index]);
                    pList1.RemoveAt(index);
                    if (pList2.Count >= pSelectCount)
                    {
                        if (pSort)
                        {
                            pList2.Sort();
                        }
                        return;
                    }
                }
            }
        }

        public static void Random(int pMin, int pMax, Dictionary<int, string> pDic, int pSelectCount)
        {
            if (pDic != null)
            {
                pDic.Clear();
                System.Random random = new System.Random();
                while (true)
                {
                    int num = random.Next(pMin, pMax + 1);
                    pDic[num] = "";
                    if (pDic.Count >= pSelectCount)
                    {
                        return;
                    }
                }
            }
        }

        public static void ReadRegArrString(string regPath, ref List<string> pSearchList)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(regPath);
            try
            {
                foreach (string str in key.GetValueNames())
                {
                    pSearchList.Add(key.GetValue(str).ToString());
                }
                key.Close();
            }
            catch
            {
            }
        }

        public static bool ReadRegBoolean(string regPath, string regKey, string defaultValue)
        {
            bool flag = false;
            if (ReadRegString(regPath, regKey, defaultValue).ToLower() == "true")
            {
                flag = true;
            }
            return flag;
        }

        public static DateTime ReadRegDateTime(string regPath, string regKey, string defaultValue) => 
            Convert.ToDateTime(ReadRegString(regPath, regKey, defaultValue));

        public static decimal ReadRegDecimal(string regPath, string regKey, string defaultValue) => 
            Convert.ToDecimal(ReadRegString(regPath, regKey, defaultValue));

        public static double ReadRegDouble(string regPath, string regKey, string defaultValue) => 
            Convert.ToDouble(ReadRegString(regPath, regKey, defaultValue));

        public static int ReadRegInt(string regPath, string regKey, string defaultValue) => 
            Convert.ToInt32(ReadRegString(regPath, regKey, defaultValue));

        public static string ReadRegString(string regPath, string regKey, string defaultValue)
        {
            string str = defaultValue;
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(regPath, true);
                if (key != null)
                {
                    str = key.GetValue(regKey).ToString();
                }
                key.Close();
            }
            catch
            {
            }
            return str;
        }

        public static void ReadTextFileAddDic(string pFile, ref Dictionary<string, int> pDic)
        {
            FileStream stream = new FileStream(pFile, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream, Encoding.Default);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            for (string str = reader.ReadLine(); str != null; str = reader.ReadLine())
            {
                if (!pDic.ContainsKey(str))
                {
                    pDic[str] = 1;
                }
                else
                {
                    Dictionary<string, int> dictionary;
                    string str2;
                    (dictionary = pDic)[str2 = str] = dictionary[str2] + 1;
                }
            }
            reader.Close();
            stream.Close();
            reader.Dispose();
            stream.Close();
        }

        public static void ReadTextFileAddList(string pFile, ref List<string> pList)
        {
            FileStream stream = new FileStream(pFile, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream, Encoding.Default);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            for (string str = reader.ReadLine(); str != null; str = reader.ReadLine())
            {
                pList.Add(str);
            }
            reader.Close();
            stream.Close();
            reader.Dispose();
            stream.Close();
        }

        public static Dictionary<string, string> ReadTextFileToDic(string pFile) => 
            ConvertConfiguration(ReadTextFileToList(pFile), '=');

        public static List<string> ReadTextFileToList(string pFile)
        {
            List<string> list = new List<string>();
            FileStream stream = new FileStream(pFile, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream, Encoding.Default);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            for (string str = reader.ReadLine(); str != null; str = reader.ReadLine())
            {
                list.Add(str);
            }
            reader.Close();
            stream.Close();
            reader.Dispose();
            stream.Close();
            return list;
        }

        public static string ReadTextFileToStr(string pFile)
        {
            string str = "";
            FileStream stream = new FileStream(pFile, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream, Encoding.Default);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            str = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            reader.Dispose();
            stream.Close();
            return str;
        }

        public static void RefreshDataGridView(DataGridView pDataGridView, int pCount)
        {
            pDataGridView.RowCount = pCount;
            pDataGridView.Refresh();
        }

        public static void RefreshDataGridView(DataGridView pDataGridView, int pCount, bool pFirst)
        {
            RefreshDataGridView(pDataGridView, pCount);
            RefreshSelect(pDataGridView, pFirst);
        }

        public static void RefreshScrollBars(DataGridView pDataGridView, bool pFirst = true)
        {
            ScrollBars scrollBars = pDataGridView.ScrollBars;
            pDataGridView.ScrollBars = ScrollBars.None;
            pDataGridView.ScrollBars = scrollBars;
            if (pFirst)
            {
                pDataGridView.FirstDisplayedScrollingRowIndex = 0;
            }
            else
            {
                pDataGridView.FirstDisplayedScrollingRowIndex = pDataGridView.RowCount - 1;
            }
        }

        public static void RefreshSelect(DataGridView pDataGridView, bool pFirst = true)
        {
            if (pDataGridView.RowCount != 0)
            {
                if (pFirst)
                {
                    pDataGridView.Rows[0].Selected = true;
                    pDataGridView.FirstDisplayedScrollingRowIndex = 0;
                }
                else
                {
                    pDataGridView.Rows[pDataGridView.RowCount - 1].Selected = true;
                    pDataGridView.FirstDisplayedScrollingRowIndex = pDataGridView.RowCount - 1;
                }
            }
        }

        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string lpString);
        public static bool RegPathExists(string regPath) => 
            (Registry.CurrentUser.OpenSubKey(regPath) != null);

        public static List<string> RepeatList(string pStr, int pCount)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < pCount; i++)
            {
                list.Add(pStr);
            }
            return list;
        }

        public static string ReplaceText(string pStr, ConfigurationStatus.PlayBase playInfo, bool pIsReplace = true)
        {
            string str = pStr;
            if (pIsReplace)
            {
                foreach (string str2 in AppInfo.ReplaceList)
                {
                    str = str.Replace(str2, playInfo.PlayChar);
                }
            }
            return str;
        }

        public static void ReplaceTextChar(string pFile, string pStr1, string pStr2)
        {
            List<string> list = ReadTextFileToList(pFile);
            List<string> pList = new List<string>();
            foreach (string str in list)
            {
                pList.Add(str.Replace(pStr1, pStr2));
            }
            WriteTextFile(pFile, pList);
        }

        public static void Reverse(object[,] array)
        {
            int lowerBound = array.GetLowerBound(0);
            for (int i = (lowerBound + array.GetUpperBound(0)) - 1; lowerBound < i; i--)
            {
                for (int j = array.GetLowerBound(1); j <= array.GetUpperBound(1); j++)
                {
                    object obj2 = array[lowerBound, j];
                    array[lowerBound, j] = array[i, j];
                    array[i, j] = obj2;
                }
                lowerBound++;
            }
        }

        public static string ReverseString(string pStr)
        {
            char[] chArray = pStr.ToCharArray();
            int length = chArray.Length;
            for (int i = 0; i < (chArray.Length / 2); i++)
            {
                char ch = chArray[i];
                chArray[i] = chArray[(length - 1) - i];
                chArray[(length - 1) - i] = ch;
            }
            return new string(chArray);
        }

        public static void RunCmd(string cmd)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo {
                CreateNoWindow = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/c " + cmd
            };
            Process.Start(startInfo);
        }

        private static void SaveBetCodeLineInfo(BetCodeLine pBetCodeLine, string pRegConfigPath)
        {
            try
            {
                WriteRegValue(pRegConfigPath, pBetCodeLine.Name + "Value", pBetCodeLine.CodeString);
            }
            catch
            {
            }
        }

        private static void SaveCheckBoxInfo(CheckBox pCheckBox, string pRegConfigPath)
        {
            WriteRegValue(pRegConfigPath, pCheckBox.Name + "Checked", pCheckBox.Checked.ToString());
        }

        private static void SaveComboBoxIndex(ComboBox pComboBox, string pRegConfigPath)
        {
            if (pComboBox.DropDownStyle == ComboBoxStyle.DropDown)
            {
                WriteRegValue(pRegConfigPath, pComboBox.Name + "Value", pComboBox.Text);
            }
            else
            {
                WriteRegValue(pRegConfigPath, pComboBox.Name + "Index", pComboBox.SelectedIndex.ToString());
            }
        }

        private static void SaveComboBoxInfo(ComboBox pComboBox, string pRegConfigPath)
        {
            string regPath = pRegConfigPath + @"\" + pComboBox.Name;
            int num = 0;
            foreach (string str2 in pComboBox.Items)
            {
                WriteRegValue(regPath, num++.ToString(), str2);
            }
        }

        public static void SaveConfiguration()
        {
            string configurationFile = ConfigurationFile;
            if (AppInfo.Account.Configuration != null)
            {
                AppInfo.Account.Configuration.SaveConfiguration(configurationFile);
            }
        }

        private static void SaveDataGridViewIndex(DataGridView pDataGridView, string pRegConfigPath)
        {
            if (pDataGridView.Rows.Count != 0)
            {
                WriteRegValue(pRegConfigPath, pDataGridView.Name + "Value", pDataGridView.SelectedRows[0].Index.ToString());
                WriteRegValue(pRegConfigPath, pDataGridView.Name + "FirstDisplayedScrollingRowIndex", pDataGridView.FirstDisplayedScrollingRowIndex.ToString());
            }
        }

        public static void SaveDataGridViewInfo(DataGridView pDataGridView, string pRegConfigPath)
        {
            try
            {
                pRegConfigPath = pRegConfigPath + @"\" + pDataGridView.Name;
                try
                {
                    Registry.CurrentUser.DeleteSubKey(pRegConfigPath);
                }
                catch
                {
                }
                int count = pDataGridView.Columns.Count;
                string name = pDataGridView.Name;
                int num2 = 0;
                foreach (DataGridViewRow row in (IEnumerable) pDataGridView.Rows)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (row.Cells[i].Value != null)
                        {
                            WriteRegValue(pRegConfigPath, string.Concat(new object[] { name, "Rows", num2, "Cols", i }), row.Cells[i].Value.ToString());
                        }
                        else
                        {
                            WriteRegValue(pRegConfigPath, string.Concat(new object[] { name, "Rows", num2, "Cols", i }), "");
                        }
                    }
                    num2++;
                }
                WriteRegValue(pRegConfigPath, "Rows", num2.ToString());
            }
            catch
            {
            }
        }

        private static void SaveDateTimePickerInfo(DateTimePicker pDateTimePicker, string pRegConfigPath)
        {
            WriteRegValue(pRegConfigPath, pDateTimePicker.Name + "Value", pDateTimePicker.Value.ToString());
        }

        private static void SaveFormInfo(Form pForm, string pRegConfigPath)
        {
            WriteRegValue(pRegConfigPath, "FormTop", pForm.Top.ToString());
            WriteRegValue(pRegConfigPath, "FormLeft", pForm.Left.ToString());
            WriteRegValue(pRegConfigPath, "FormWidth", pForm.Width.ToString());
            WriteRegValue(pRegConfigPath, "FormHeight", pForm.Height.ToString());
            WriteRegValue(pRegConfigPath, "FormWindowState", pForm.WindowState.ToString());
        }

        public static void SaveFormUseingInfo(List<Control> pControlList, string pRegConfigPath)
        {
            try
            {
                if (pControlList.Count != 0)
                {
                    foreach (Control control in pControlList)
                    {
                        if (control is Form)
                        {
                            SaveFormInfo((Form) control, pRegConfigPath);
                        }
                        else if (control is NumericUpDown)
                        {
                            SaveNumericUpDownInfo((NumericUpDown) control, pRegConfigPath);
                        }
                        else if (control is ComboBox)
                        {
                            SaveComboBoxInfo((ComboBox) control, pRegConfigPath);
                        }
                        else if (control is CheckBox)
                        {
                            SaveCheckBoxInfo((CheckBox) control, pRegConfigPath);
                        }
                        else if (control is TabControl)
                        {
                            SaveTabControlInfo((TabControl) control, pRegConfigPath);
                        }
                        else if (control is RadioButton)
                        {
                            SaveRadioButtonInfo((RadioButton) control, pRegConfigPath);
                        }
                        else if (control is DateTimePicker)
                        {
                            SaveDateTimePickerInfo((DateTimePicker) control, pRegConfigPath);
                        }
                        else if (control is TextBox)
                        {
                            SaveTextBoxInfo((TextBox) control, pRegConfigPath);
                        }
                        else if (control is BetCodeLine)
                        {
                            SaveBetCodeLineInfo((BetCodeLine) control, pRegConfigPath);
                        }
                        else if (control is RichTextBox)
                        {
                            SaveRichTextBoxInfo((RichTextBox) control, pRegConfigPath);
                        }
                        else if (((control is DataGridView) || (control is ExpandGirdView)) || (control is TreeGridView))
                        {
                            SaveDataGridViewInfo((DataGridView) control, pRegConfigPath);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private static void SaveNumericUpDownInfo(NumericUpDown pNumericUpDown, string pRegConfigPath)
        {
            WriteRegValue(pRegConfigPath, pNumericUpDown.Name + "Value", pNumericUpDown.Value.ToString());
        }

        private static void SaveRadioButtonInfo(RadioButton pRadioButton, string pRegConfigPath)
        {
            WriteRegValue(pRegConfigPath, pRadioButton.Name + "Checked", pRadioButton.Checked.ToString());
        }

        private static void SaveRichTextBoxInfo(RichTextBox pRichTextBox, string pRegConfigPath)
        {
            try
            {
                WriteRegValue(pRegConfigPath, pRichTextBox.Name + "Value", pRichTextBox.Text);
            }
            catch
            {
            }
        }

        public static void SaveSpecialControlInfo(List<Control> pControlList, string pRegConfigPath)
        {
            if (pControlList != null)
            {
                foreach (Control control in pControlList)
                {
                    if (control is ComboBox)
                    {
                        SaveComboBoxIndex((ComboBox) control, pRegConfigPath);
                    }
                    else if (((control is DataGridView) || (control is ExpandGirdView)) || (control is TreeGridView))
                    {
                        SaveDataGridViewIndex((DataGridView) control, pRegConfigPath);
                    }
                }
            }
        }

        private static void SaveTabControlInfo(TabControl pTabControl, string pRegConfigPath)
        {
            WriteRegValue(pRegConfigPath, pTabControl.Name + "Index", pTabControl.SelectedIndex.ToString());
        }

        private static void SaveTextBoxInfo(TextBox pTextBox, string pRegConfigPath)
        {
            try
            {
                WriteRegValue(pRegConfigPath, pTextBox.Name + "Value", pTextBox.Text);
            }
            catch
            {
            }
        }

        public static void SaveTextToListItems(ComboBox pComboBox)
        {
            string text = pComboBox.Text;
            if (text != "")
            {
                if (pComboBox.Items.Contains(text))
                {
                    pComboBox.Items.Remove(text);
                }
                pComboBox.Items.Insert(0, text);
                pComboBox.Text = text;
            }
        }

        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, ref int lParam);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern int SendMessage(IntPtr HWnd, uint Msg, int WParam, int LParam);
        [DllImport("user32.dll ", EntryPoint="SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, string lParam);
        [DllImport("user32.dll ", EntryPoint="SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll ", EntryPoint="SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, uint wMsg, IntPtr wParam, IntPtr lParam);
        public static bool SetAutomaticStart(string keyName, string filePath, bool enable)
        {
            bool flag = false;
            try
            {
                string str = Detect3264() ? @"software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run" : @"software\Microsoft\Windows\CurrentVersion\Run";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"software\Microsoft\Windows\CurrentVersion\Run", true);
                if (enable)
                {
                    key.SetValue(keyName, filePath);
                }
                else
                {
                    try
                    {
                        key.DeleteValue(keyName);
                    }
                    catch
                    {
                    }
                }
                key.Close();
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        private static void SetBetCodeLineInfo(BetCodeLine pBetCodeLine, string pRegConfigPath)
        {
            try
            {
                pBetCodeLine.CodeString = ReadRegString(pRegConfigPath, pBetCodeLine.Name + "Value", "");
            }
            catch
            {
            }
        }

        public static void SetButtonFormatFlat(List<Button> pButtonList)
        {
            foreach (Button button in pButtonList)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.CheckedBackColor = AppInfo.hotColor;
                button.FlatAppearance.MouseDownBackColor = AppInfo.hotColor;
                button.FlatAppearance.MouseOverBackColor = AppInfo.hotColor;
            }
        }

        public static void SetCheckBoxFormatFlat(List<CheckBox> pCheckBoxList)
        {
            foreach (CheckBox box in pCheckBoxList)
            {
                box.FlatStyle = FlatStyle.Flat;
                box.FlatAppearance.CheckedBackColor = AppInfo.hotColor;
                box.FlatAppearance.MouseDownBackColor = AppInfo.hotColor;
                box.FlatAppearance.MouseOverBackColor = AppInfo.hotColor;
                box.CheckedChanged += new EventHandler(CommFunc.CheckBox_CheckedChanged);
                box.MouseMove += new MouseEventHandler(CommFunc.CheckBox_MouseMove);
                box.MouseLeave += new EventHandler(CommFunc.CheckBox_MouseLeave);
            }
        }

        public static void SetCheckBoxFormatStandard(List<CheckBox> pCheckBoxList)
        {
            foreach (CheckBox box in pCheckBoxList)
            {
                box.FlatStyle = FlatStyle.Standard;
            }
        }

        private static void SetCheckBoxInfo(CheckBox pCheckBox, string pRegConfigPath)
        {
            pCheckBox.Checked = ReadRegBoolean(pRegConfigPath, pCheckBox.Name + "Checked", "False");
        }

        public static void SetComboBoxFormat(List<ComboBox> pComboBoxList, int pMaxItems = 8)
        {
            foreach (ComboBox box in pComboBoxList)
            {
                box.IntegralHeight = false;
                box.MaxDropDownItems = pMaxItems;
            }
        }

        private static void SetComboBoxIndex(ComboBox pComboBox, string pRegConfigPath)
        {
            try
            {
                if (pComboBox.DropDownStyle == ComboBoxStyle.DropDown)
                {
                    pComboBox.Text = ReadRegString(pRegConfigPath, pComboBox.Name + "Value", "");
                    if ((pComboBox.Text == "") && (pComboBox.Items.Count > 0))
                    {
                        pComboBox.Text = pComboBox.Items[0].ToString();
                    }
                }
                else
                {
                    string defaultValue = (pComboBox.Tag != null) ? pComboBox.Tag.ToString() : "0";
                    pComboBox.SelectedIndex = ReadRegInt(pRegConfigPath, pComboBox.Name + "Index", defaultValue);
                }
            }
            catch
            {
            }
        }

        private static void SetComboBoxInfo(ComboBox pComboBox, string pRegConfigPath)
        {
            List<string> pSearchList = new List<string>();
            ReadRegArrString(pRegConfigPath + @"\" + pComboBox.Name, ref pSearchList);
            pComboBox.Items.Clear();
            foreach (string str2 in pSearchList)
            {
                pComboBox.Items.Add(str2);
            }
            if (pSearchList.Count > 0)
            {
                pComboBox.SelectedIndex = 0;
            }
        }

        public static void SetComboBoxList(ComboBox pCbb, List<string> pList)
        {
            string text = pCbb.Text;
            pCbb.Items.Clear();
            foreach (string str2 in pList)
            {
                pCbb.Items.Add(str2);
            }
            if (text != "")
            {
                SetComboBoxSelectedIndex(pCbb, text);
            }
            if ((pCbb.SelectedIndex == -1) && (pCbb.Items.Count > 0))
            {
                pCbb.SelectedIndex = 0;
            }
        }

        public static void SetComboBoxSelectedIndex(ComboBox pCbb, int pIndex)
        {
            try
            {
                if (pCbb.Items.Count != 0)
                {
                    pCbb.SelectedIndex = pIndex;
                }
            }
            catch
            {
                pCbb.SelectedIndex = 0;
            }
        }

        public static void SetComboBoxSelectedIndex(ComboBox pCbb, string pText)
        {
            try
            {
                if (pCbb.Items.Count != 0)
                {
                    int num = pCbb.FindString(pText);
                    pCbb.SelectedIndex = (num >= 0) ? num : 0;
                }
            }
            catch
            {
                pCbb.SelectedIndex = 0;
            }
        }

        public static void SetControlBackColor(List<Control> pControlList, System.Drawing.Color pColor)
        {
            foreach (Control control in pControlList)
            {
                control.BackColor = pColor;
            }
        }

        public static void SetControlForeColor(List<Control> pControlList, System.Drawing.Color pColor)
        {
            foreach (Control control in pControlList)
            {
                control.ForeColor = pColor;
            }
        }

        public static void SetControlHint(ErrorProvider pHintControl, Control pSetControl, string pHint)
        {
            pHintControl.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            pHintControl.SetError(pSetControl, pHint);
        }

        public static void SetDataGridViewAlignment(DataGridView pDataGridView, List<int> pIndexList, DataGridViewContentAlignment pAlignment)
        {
            foreach (int num in pIndexList)
            {
                pDataGridView.Columns[num].HeaderCell.Style.Alignment = pAlignment;
                pDataGridView.Columns[num].DefaultCellStyle.Alignment = pAlignment;
            }
        }

        private static void SetDataGridViewIndex(DataGridView pDataGridView, string pRegConfigPath)
        {
            try
            {
                pDataGridView.Rows[ReadRegInt(pRegConfigPath, pDataGridView.Name + "Value", "0")].Selected = true;
                pDataGridView.FirstDisplayedScrollingRowIndex = ReadRegInt(pRegConfigPath, pDataGridView.Name + "FirstDisplayedScrollingRowIndex", "0");
            }
            catch
            {
            }
        }

        public static void SetDataGridViewInfo(DataGridView pDataGridView, string pRegConfigPath)
        {
            try
            {
                pDataGridView.Rows.Clear();
                pRegConfigPath = pRegConfigPath + @"\" + pDataGridView.Name;
                RegistryKey key = Registry.CurrentUser.OpenSubKey(pRegConfigPath);
                if (key != null)
                {
                    string name = pDataGridView.Name;
                    int count = pDataGridView.Columns.Count;
                    int num2 = ReadRegInt(pRegConfigPath, "Rows", "0");
                    for (int i = 0; i < num2; i++)
                    {
                        object[] values = new object[count];
                        for (int j = 0; j < count; j++)
                        {
                            string str2 = string.Concat(new object[] { name, "Rows", i, "Cols", j });
                            values[j] = key.GetValue(str2).ToString();
                        }
                        pDataGridView.Rows.Add(values);
                    }
                }
            }
            catch
            {
            }
        }

        public static void SetDataGridViewSelected(DataGridView pDataGridView, string pValue, int pIndex = 0)
        {
            if (pValue != "")
            {
                foreach (DataGridViewRow row in (IEnumerable) pDataGridView.Rows)
                {
                    if (row.Cells[pIndex].Value.ToString() == pValue)
                    {
                        row.Selected = true;
                        break;
                    }
                }
            }
        }

        private static void SetDateTimePickerInfo(DateTimePicker pDateTimePicker, string pRegConfigPath)
        {
            pDateTimePicker.Value = ReadRegDateTime(pRegConfigPath, pDateTimePicker.Name + "Value", "1900-1-1 00:00:00");
        }

        public static void SetDoubleBuffered(DataGridView Dgv)
        {
            Dgv.GetType().GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Dgv, true, null);
        }

        public static void SetExpandGirdViewFormat(DataGridView pDataGridView, int pFontSiz = 9)
        {
            pDataGridView.EnableHeadersVisualStyles = false;
            pDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            pDataGridView.ColumnHeadersHeight = 30;
            pDataGridView.ColumnHeadersDefaultCellStyle.BackColor = AppInfo.appBackColor;
            pDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = AppInfo.whiteColor;
            pDataGridView.RowsDefaultCellStyle.SelectionBackColor = AppInfo.hotColor;
            pDataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", (float) pFontSiz);
            if (pFontSiz == 14)
            {
                pDataGridView.RowTemplate.Height = 30;
            }
            if (AppInfo.Account.Configuration.Beautify)
            {
                pDataGridView.BackgroundColor = AppInfo.beaBackColor;
                pDataGridView.RowsDefaultCellStyle.BackColor = AppInfo.beaBackColor;
                pDataGridView.RowsDefaultCellStyle.SelectionBackColor = AppInfo.hotColor;
                pDataGridView.RowsDefaultCellStyle.ForeColor = AppInfo.whiteColor;
                pDataGridView.RowsDefaultCellStyle.SelectionForeColor = AppInfo.blackColor;
            }
        }

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);
        private static void SetFormInfo(Form pForm, string pRegConfigPath)
        {

            int num = CommFunc.ReadRegInt(pRegConfigPath, "FormTop", (-1024).ToString());
            int num2 = CommFunc.ReadRegInt(pRegConfigPath, "FormLeft", (-1024).ToString());
            if (num > 0 && num2 > 0 && num2 + pForm.Width / 2 < Screen.PrimaryScreen.Bounds.Width)
            {
                if (num != -1024)
                {
                    pForm.Top = num;
                }
                if (num2 != -1024)
                {
                    pForm.Left = num2;
                }
            }
        }

        public static void SetFormUseingInfo(List<Control> pControlList, string pRegConfigPath)
        {
            if (RegPathExists(pRegConfigPath) && (pControlList.Count != 0))
            {
                foreach (Control control in pControlList)
                {
                    try
                    {
                        if (control is Form)
                        {
                            SetFormInfo((Form) control, pRegConfigPath);
                        }
                        else if (control is NumericUpDown)
                        {
                            SetNumericUpDownInfo((NumericUpDown) control, pRegConfigPath);
                        }
                        else if (control is ComboBox)
                        {
                            SetComboBoxInfo((ComboBox) control, pRegConfigPath);
                        }
                        else if (control is CheckBox)
                        {
                            SetCheckBoxInfo((CheckBox) control, pRegConfigPath);
                        }
                        else if (control is TabControl)
                        {
                            SetTabControlInfo((TabControl) control, pRegConfigPath);
                        }
                        else if (control is RadioButton)
                        {
                            SetRadioButtonInfo((RadioButton) control, pRegConfigPath);
                        }
                        else if (control is DateTimePicker)
                        {
                            SetDateTimePickerInfo((DateTimePicker) control, pRegConfigPath);
                        }
                        else if (control is TextBox)
                        {
                            SetTextBoxInfo((TextBox) control, pRegConfigPath);
                        }
                        else if (control is BetCodeLine)
                        {
                            SetBetCodeLineInfo((BetCodeLine) control, pRegConfigPath);
                        }
                        else if (control is RichTextBox)
                        {
                            SetRichTextBoxInfo((RichTextBox) control, pRegConfigPath);
                        }
                        else if (((control is DataGridView) || (control is ExpandGirdView)) || (control is TreeGridView))
                        {
                            SetDataGridViewInfo((DataGridView) control, pRegConfigPath);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static void SetHeaderFormat(ExpandGirdView pEgv)
        {
            pEgv.EnableHeadersVisualStyles = false;
            pEgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            pEgv.ColumnHeadersHeight = 30;
        }

        public static void SetHeaderFormat(ExpandGirdView pEgv, System.Drawing.Color pforeColor)
        {
            SetHeaderFormat(pEgv);
            pEgv.ColumnHeadersDefaultCellStyle.ForeColor = pforeColor;
        }

        public static void SetLabelFormat(List<Label> pLabelList)
        {
            foreach (Label label in pLabelList)
            {
                if (label.Dock == DockStyle.None)
                {
                    label.ForeColor = AppInfo.appForeColor;
                }
                else
                {
                    label.BackColor = AppInfo.appBackColor;
                    label.ForeColor = AppInfo.whiteColor;
                }
                if (label.Text == "00")
                {
                    label.Text = "";
                }
            }
        }

        private static void SetNumericUpDownInfo(NumericUpDown pNumericUpDown, string pRegConfigPath)
        {
            if (pNumericUpDown.DecimalPlaces == 0)
            {
                pNumericUpDown.Value = ReadRegInt(pRegConfigPath, pNumericUpDown.Name + "Value", "0");
            }
            else
            {
                pNumericUpDown.Value = ReadRegDecimal(pRegConfigPath, pNumericUpDown.Name + "Value", "0.0");
            }
        }

        public static bool SetProgress(ref bool pBreakOut)
        {
            bool flag = false;
            try
            {
                if ((AppInfo.PregressHint != null) && AppInfo.PregressHint.NeedShowBar)
                {
                    flag = AppInfo.PregressHint.SetValue();
                }
            }
            catch
            {
            }
            pBreakOut = flag;
            return flag;
        }

        public static void SetRadioButtonFormat(List<RadioButton> pRadioButtonList)
        {
            foreach (RadioButton button in pRadioButtonList)
            {
                button.FlatAppearance.CheckedBackColor = AppInfo.appBackColor;
                button.FlatAppearance.MouseDownBackColor = AppInfo.appBackColor;
                button.FlatAppearance.MouseOverBackColor = AppInfo.hotColor;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = AppInfo.darkGrayForeColor;
            }
        }

        private static void SetRadioButtonInfo(RadioButton pRadioButton, string pRegConfigPath)
        {
            pRadioButton.Checked = ReadRegBoolean(pRegConfigPath, pRadioButton.Name + "Checked", "True");
        }

        private static void SetRichTextBoxInfo(RichTextBox pRichTextBox, string pRegConfigPath)
        {
            try
            {
                pRichTextBox.Text = ReadRegString(pRegConfigPath, pRichTextBox.Name + "Value", "");
            }
            catch
            {
            }
        }

        public static void SetSpecialControlInfo(List<Control> pControlList, string pRegConfigPath)
        {
            foreach (Control control in pControlList)
            {
                if (control is ComboBox)
                {
                    SetComboBoxIndex((ComboBox) control, pRegConfigPath);
                }
                else if (((control is DataGridView) || (control is ExpandGirdView)) || (control is TreeGridView))
                {
                    SetDataGridViewIndex((DataGridView) control, pRegConfigPath);
                }
            }
        }

        private static void SetTabControlInfo(TabControl pTabControl, string pRegConfigPath)
        {
            pTabControl.SelectedIndex = ReadRegInt(pRegConfigPath, pTabControl.Name + "Index", "0");
        }

        public static void SetTabSelectIndex(TabControl pTabControl, string pText)
        {
            for (int i = 0; i < pTabControl.TabPages.Count; i++)
            {
                TabPage page = pTabControl.TabPages[i];
                if (page.Text.Contains(pText))
                {
                    pTabControl.SelectedIndex = i;
                    break;
                }
            }
        }

        private static void SetTextBoxInfo(TextBox pTextBox, string pRegConfigPath)
        {
            try
            {
                pTextBox.Text = ReadRegString(pRegConfigPath, pTextBox.Name + "Value", "");
            }
            catch
            {
            }
        }

        public static void SetWebBrowserFormat(List<WebBrowser> pWebBrowserList)
        {
            foreach (WebBrowser browser in pWebBrowserList)
            {
                browser.ScriptErrorsSuppressed = true;
            }
        }

        public static void SetWebHtmlElement(HtmlDocument pDocument, string pKey, string pValue, string pInputKey, string pInputValue, bool pIsEqual = true)
        {
            HtmlElementCollection all = pDocument.All;
            List<string> list = new List<string>();
            for (int i = 0; i < all.Count; i++)
            {
                HtmlElement element = all[i];
                string item = element.GetAttribute(pKey).ToString().Trim();
                list.Add(item);
                if (pIsEqual)
                {
                    if (item == pValue)
                    {
                        element.Focus();
                        element.SetAttribute(pInputKey, pInputValue);
                        break;
                    }
                }
                else if (item.Contains(pValue))
                {
                    element.Focus();
                    element.SetAttribute(pInputKey, pInputValue);
                    break;
                }
            }
        }

        public static string SHA1(string pValue)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pValue);
            return BitConverter.ToString(System.Security.Cryptography.SHA1.Create().ComputeHash(bytes)).Replace("-", "").ToLower();
        }

        public static void SortDataGridView(DataGridView pDataGridView, int pIndex, ListSortDirection pSortDirection)
        {
            pDataGridView.Sort(pDataGridView.Columns[pIndex], pSortDirection);
        }

        public static List<string> SortList(List<string> pList1, List<string> pList2)
        {
            List<string> list = new List<string>();
            foreach (string str in pList1)
            {
                if (pList2.Contains(str))
                {
                    list.Add(str);
                }
            }
            return list;
        }

        public static void SortMaxList(ref List<string> pList)
        {
            int count = pList.Count;
            for (int i = 0; i < (count - 1); i++)
            {
                int num3 = i;
                for (int j = i + 1; j < count; j++)
                {
                    if (Convert.ToInt32(pList[num3]) < Convert.ToInt32(pList[j]))
                    {
                        num3 = j;
                    }
                }
                if (num3 != i)
                {
                    string str = pList[num3];
                    pList[num3] = pList[i];
                    pList[i] = str;
                }
            }
        }

        public static string SortString(string pStr)
        {
            List<string> pList = ConvertSameListString(pStr);
            pList.Sort();
            return Join(pList);
        }

        public static string SortString(string pStr, string pChar)
        {
            List<string> pList = SplitString(pStr, pChar, -1);
            pList.Sort();
            return Join(pList, pChar);
        }

        public static List<string> SplitBetsCode(string pValue, string playName)
        {
            string playChar = GetPlayChar(playName);
            return SplitString(pValue, playChar, -1);
        }

        public static List<double> SplitDouble(string pStr, string pChar = ",")
        {
            List<double> list = new List<double>();
            if (pStr != "")
            {
                string[] strArray = Strings.Split(pStr, pChar, -1, CompareMethod.Binary);
                foreach (string str in strArray)
                {
                    list.Add(Convert.ToDouble(str));
                }
            }
            return list;
        }

        public static List<int> SplitInt(string pStr, string pChar = ",")
        {
            List<int> list = new List<int>();
            if (pStr != "")
            {
                string[] strArray = Strings.Split(pStr, pChar, -1, CompareMethod.Binary);
                foreach (string str in strArray)
                {
                    list.Add(Convert.ToInt32(str));
                }
            }
            return list;
        }

        public static List<string> SplitString(string pStr, string pChar = ",", int pLen = -1)
        {
            List<string> list = new List<string>();
            if (pStr != "")
            {
                string[] strArray = Strings.Split(pStr, pChar, -1, CompareMethod.Binary);
                foreach (string str in strArray)
                {
                    string item = str;
                    if (pLen != -1)
                    {
                        item = item.PadLeft(pLen, '0');
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public static List<string> SplitStringSkipNull(string pStr, string pChar = ",")
        {
            List<string> list = new List<string>();
            if (pStr != "")
            {
                string[] strArray = Strings.Split(pStr, pChar, -1, CompareMethod.Binary);
                foreach (string str in strArray)
                {
                    if (str != "")
                    {
                        list.Add(str);
                    }
                }
            }
            return list;
        }

        public static void StartProcess(string pFile, string pArguments = "")
        {
            Process process = new Process {
                StartInfo = { FileName = pFile }
            };
            if (pArguments != "")
            {
                pArguments = "\"" + pArguments + "\"";
                process.StartInfo.Arguments = pArguments;
            }
            process.Start();
        }

        public static string StringToUnicon(string pStr)
        {
            string str = "";
            for (int i = 0; i < pStr.Length; i++)
            {
                str = str + @"\u" + ((int) pStr[i]).ToString("x");
            }
            return str;
        }

        public static void Swap(List<string> pList, int pFrom, int pTo)
        {
            if (pFrom != pTo)
            {
                string str = pList[pFrom];
                pList[pFrom] = pList[pTo];
                pList[pTo] = str;
            }
        }

        public static void SwapData<T>(ref T pData1, ref T pData2)
        {
            T local = pData1;
            pData1 = pData2;
            pData2 = local;
        }

        public static void SwapValue<T>(ref T pValue1, ref T pValue2)
        {
            T local = pValue1;
            pValue1 = pValue2;
            pValue2 = local;
        }

        public static void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl control = sender as TabControl;
            SolidBrush brush = new SolidBrush(AppInfo.beaBackColor);
            Rectangle rect = new Rectangle(0, 0, control.Width, control.Height);
            e.Graphics.FillRectangle(brush, rect);
            int selectedIndex = control.SelectedIndex;
            for (int i = 0; i < control.TabPages.Count; i++)
            {
                System.Drawing.Color color = (i == selectedIndex) ? AppInfo.hotColor : AppInfo.beaBackColor;
                System.Drawing.Color color2 = (i == selectedIndex) ? AppInfo.defaultForeColor : AppInfo.beaForeColor;
                SolidBrush brush2 = new SolidBrush(color);
                SolidBrush brush3 = new SolidBrush(color2);
                Rectangle tabRect = control.GetTabRect(i);
                e.Graphics.FillRectangle(brush2, tabRect);
                Font font = new Font("微软雅黑", 9f, FontStyle.Bold);
                StringFormat format = new StringFormat {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                string text = control.TabPages[i].Text;
                e.Graphics.DrawString(text, font, brush3, tabRect, format);
            }
        }

        public static string TwoDouble(double pNum, bool pIsTwoNumber = false)
        {
            string str = pNum.ToString();
            try
            {
                if ((pNum != 0.0) && pIsTwoNumber)
                {
                    return pNum.ToString("0.000");
                }
                str = Math.Round(pNum, 0).ToString();
            }
            catch
            {
            }
            return str;
        }

        public static string TwoNumber(double pCode, int pNum = 2)
        {
            if (pCode == 0.0)
            {
                pNum = 2;
            }
            string format = "0." + "0".PadLeft(pNum, '0');
            return pCode.ToString(format);
        }

        public static string UniconToString(string pStr)
        {
            string str = "";
            try
            {
                string[] strArray = pStr.Replace(@"\", "").Split(new char[] { 'u' });
                for (int i = 1; i < strArray.Length; i++)
                {
                    string str2 = strArray[i];
                    if (str2 != "")
                    {
                        string s = str2.Substring(0, 4);
                        string str4 = str2.Substring(4);
                        str2 = ((char) int.Parse(s, NumberStyles.HexNumber)) + str4;
                        str = str + str2;
                    }
                }
            }
            catch
            {
            }
            return str;
        }

        public static void UpdateApp(string pAppName)
        {
            string pFile = getDllPath() + @"\Update\AutoUpdatePlus.exe";
            string str2 = getWinFormPath();
            List<string> pList = new List<string> {
                str2,
                pAppName,
                "1.0.4",
                AppInfo.cServerUpdateUrl
            };
            string pArguments = Join(pList, "|");
            StartProcess(pFile, pArguments);
        }

        public static bool VBLike(string str1, string str2) => 
            LikeOperator.LikeString(str1, str2, CompareMethod.Binary);

        public static bool VerificationCode(ConfigurationStatus.BetsScheme pScheme, ConfigurationStatus.SCPlan plan, List<string> pCodeList)
        {
            bool flag = false;
            try
            {
                if (pScheme != null)
                {
                    foreach (string str in pScheme.FNNumberDic.Keys)
                    {
                        pScheme.FNNumberDic[str].IsWin = false;
                    }
                }
                int pWinCount = 0;
                double num2 = 0.0;
                Dictionary<string, Dictionary<string, List<string>>> fNNumberDic = plan.FNNumberDic;
                List<string> list = new List<string>();
                foreach (string str2 in fNNumberDic.Keys)
                {
                    Dictionary<string, List<string>> dictionary2 = fNNumberDic[str2];
                    foreach (string str in dictionary2.Keys)
                    {
                        List<string> pNumberList = dictionary2[str];
                        List<string> list3 = SplitString(pNumberList[0], "|", -1);
                        List<string> pTNumberList = plan.GetPTNumberList(pNumberList);
                        List<List<int>> indexList = GetCodeListByPlay(plan.PlayType, plan.PlayName, plan.RXWZ, pTNumberList);
                        int num3 = VerificationCode(plan.Play, indexList, pCodeList, pTNumberList);
                        if (num3 > 0)
                        {
                            pWinCount += num3;
                            num2 += Convert.ToDouble(list3[1]) * plan.AutoTimes(str, true);
                            if ((pScheme != null) && pScheme.FNNumberDic.ContainsKey(str))
                            {
                                pScheme.FNNumberDic[str].IsWin = VerificationWinCount(num3, plan.Play, plan.RXZJList);
                            }
                        }
                        foreach (string str3 in pNumberList)
                        {
                            List<string> pList = SplitString(str3, "|", -1);
                            pList.Add(num3.ToString());
                            list.Add(Join(pList, "|"));
                        }
                    }
                }
                plan.NumberList = list;
                plan.AutoWinCount = pWinCount;
                plan.AutoTotalMode = num2;
                flag = VerificationWinCount(pWinCount, plan.Play, plan.RXZJList);
            }
            catch
            {
            }
            return flag;
        }

        public static int VerificationCode(string playName, List<List<int>> indexList, List<string> pCodeList, List<string> pNumberList)
        {
            int num = 0;
            bool flag = false;
            string str = playName;
            foreach (List<int> list in indexList)
            {
                int num2;
                string str3;
                string str4;
                List<string> list3;
                int num5;
                string str8;
                List<string> indexCode = GetIndexCode(pCodeList, list);
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
                {
                    string pCode = Join(indexCode);
                    if (!(!CheckPlayIsZu3(str) || CheckCodeIsZu3(pCode)) || !(!CheckPlayIsZu6(str) || CheckCodeIsZu6(pCode)))
                    {
                        continue;
                    }
                    if (CheckPlayIsFS(str))
                    {
                        if (CheckPlayIsDWD(str))
                        {
                            num2 = 0;
                            while (num2 < indexCode.Count)
                            {
                                str3 = indexCode[num2];
                                str4 = pNumberList[num2];
                                if ((str4 != "*") && str4.Contains(str3))
                                {
                                    flag = true;
                                    num++;
                                }
                                num2++;
                            }
                            continue;
                        }
                        if (CheckPlayIsRXFS(str))
                        {
                            flag = true;
                            num2 = 0;
                            while (num2 < indexCode.Count)
                            {
                                str3 = indexCode[num2];
                                str4 = pNumberList[list[num2] - 1];
                                if (!str4.Contains(str3))
                                {
                                    flag = false;
                                    break;
                                }
                                num2++;
                            }
                            if (flag)
                            {
                                num++;
                            }
                            continue;
                        }
                        flag = true;
                        num2 = 0;
                        while (num2 < indexCode.Count)
                        {
                            str3 = indexCode[num2];
                            str4 = pNumberList[num2];
                            if ((str4 != "*") && !str4.Contains(str3))
                            {
                                flag = false;
                                break;
                            }
                            num2++;
                        }
                        if (flag)
                        {
                            num++;
                        }
                        continue;
                    }
                    if (CheckPlayIsLH(str))
                    {
                        string str5 = indexCode[0];
                        string str6 = indexCode[1];
                        string item = CountLH(str5, str6);
                        if (pNumberList.Contains(item))
                        {
                            num++;
                        }
                        continue;
                    }
                    if (CheckPlayIsZuXFS(str))
                    {
                        flag = true;
                        num2 = 0;
                        while (num2 < indexCode.Count)
                        {
                            str3 = indexCode[num2];
                            if (!pNumberList.Contains(str3))
                            {
                                flag = false;
                                break;
                            }
                            num2++;
                        }
                        if (flag)
                        {
                            num++;
                        }
                        continue;
                    }
                    if (CheckPlayIsZuX(str))
                    {
                        pCode = SortString(pCode);
                        if (pNumberList.Contains(pCode))
                        {
                            num++;
                        }
                    }
                    else if (pNumberList.Contains(pCode))
                    {
                        num++;
                    }
                    continue;
                }
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
                {
                    int num4;
                    int playNum = GetPlayNum(str);
                    if (CheckPlayIsDS(str))
                    {
                        if (str.Contains("任"))
                        {
                            num2 = 0;
                            while (num2 < pNumberList.Count)
                            {
                                str4 = pNumberList[num2];
                                list3 = SplitString(str4, " ", -1);
                                num4 = 0;
                                num5 = 0;
                                while (num5 < list3.Count)
                                {
                                    str8 = list3[num5];
                                    if (indexCode.Contains(str8))
                                    {
                                        num4++;
                                    }
                                    num5++;
                                }
                                if (num4 >= playNum)
                                {
                                    num++;
                                }
                                num2++;
                            }
                        }
                        else
                        {
                            num2 = 0;
                            while (num2 < pNumberList.Count)
                            {
                                str4 = pNumberList[num2];
                                list3 = SplitString(str4, " ", -1);
                                num4 = 0;
                                num5 = 0;
                                while (num5 < list3.Count)
                                {
                                    str8 = list3[num5];
                                    if (indexCode[num5] == str8)
                                    {
                                        num4++;
                                    }
                                    num5++;
                                }
                                if (num4 == playNum)
                                {
                                    num++;
                                }
                                num2++;
                            }
                        }
                    }
                    else
                    {
                        num4 = 0;
                        num2 = 0;
                        while (num2 < pNumberList.Count)
                        {
                            str4 = pNumberList[num2];
                            if (indexCode.Contains(str4))
                            {
                                num4++;
                            }
                            num2++;
                        }
                        if (str.Contains("任"))
                        {
                            if (num4 >= playNum)
                            {
                                num++;
                            }
                        }
                        else if (num4 > 0)
                        {
                            num++;
                        }
                    }
                    continue;
                }
                if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
                {
                    if (CheckPlayIsDS(playName))
                    {
                        num2 = 0;
                        while (num2 < pNumberList.Count)
                        {
                            str4 = pNumberList[num2];
                            list3 = SplitString(str4, " ", -1);
                            bool flag2 = true;
                            for (num5 = 0; num5 < list3.Count; num5++)
                            {
                                str8 = list3[num5];
                                if (indexCode[num5] != str8)
                                {
                                    flag2 = false;
                                    break;
                                }
                            }
                            if (flag2)
                            {
                                num++;
                            }
                            num2++;
                        }
                        continue;
                    }
                    if (CheckPlayIsFS(playName))
                    {
                        if (CheckPlayIsDWD(playName))
                        {
                            num2 = 0;
                            while (num2 < indexCode.Count)
                            {
                                str3 = indexCode[num2];
                                str4 = pNumberList[num2];
                                if ((str4 != "*") && str4.Contains(str3))
                                {
                                    flag = true;
                                    num++;
                                }
                                num2++;
                            }
                            continue;
                        }
                        flag = true;
                        num2 = 0;
                        while (num2 < indexCode.Count)
                        {
                            str3 = indexCode[num2];
                            str4 = pNumberList[num2];
                            if ((str4 != "*") && !str4.Contains(str3))
                            {
                                flag = false;
                                break;
                            }
                            num2++;
                        }
                        if (flag)
                        {
                            num++;
                        }
                        continue;
                    }
                    if (CheckPlayIsHZ(playName))
                    {
                        str4 = CountAnd(indexCode).ToString();
                        if (pNumberList.Contains(str4))
                        {
                            flag = true;
                            num++;
                        }
                    }
                    else
                    {
                        for (num2 = 0; num2 < indexCode.Count; num2++)
                        {
                            str3 = indexCode[num2];
                            if (pNumberList.Contains(str3))
                            {
                                flag = true;
                                num++;
                            }
                        }
                    }
                }
            }
            return num;
        }

        public static bool VerificationWinCount(int pWinCount, string playName, List<int> pRXZJList = null)
        {
            if (CheckPlayIsRX(playName))
            {
                return pRXZJList.Contains(pWinCount);
            }
            return (pWinCount > 0);
        }

        public static string WebMD51(string pValue)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pValue);
            System.Security.Cryptography.MD5 md = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md.ComputeHash(bytes)).Replace("-", "").ToLower();
        }

        public static string WebMD52(string pValue)
        {
            System.Security.Cryptography.MD5 md = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.ASCII.GetBytes(pValue);
            bytes = md.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static string WebMD53(string pValue)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pValue);
            System.Security.Cryptography.MD5 md = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md.ComputeHash(bytes)).Replace("-", "").ToUpper();
        }

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point Point);
        public static void WriteRegValue(string regPath, string regKey, string regValue)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(regPath);
                key.SetValue(regKey, regValue);
                key.Close();
            }
            catch
            {
            }
        }

        public static void WriteRegValue(string regPath, string regKey, string regValue, RegistryValueKind pValueKind)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(regPath);
                key.SetValue(regKey, regValue, pValueKind);
                key.Close();
            }
            catch
            {
            }
        }

        public static void WriteTextFile(string pFile, Dictionary<string, string> pDic)
        {
            FileStream stream = new FileStream(pFile, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("GB2312"));
            writer.Flush();
            writer.BaseStream.Seek(0L, SeekOrigin.Begin);
            foreach (string str in pDic.Keys)
            {
                writer.WriteLine(str + "=" + pDic[str]);
            }
            writer.Close();
            stream.Close();
            writer.Dispose();
            stream.Dispose();
        }

        public static void WriteTextFile(string pFile, List<string> pList)
        {
            FileStream stream = new FileStream(pFile, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("GB2312"));
            writer.Flush();
            writer.BaseStream.Seek(0L, SeekOrigin.Begin);
            foreach (string str in pList)
            {
                writer.WriteLine(str);
            }
            writer.Close();
            stream.Close();
            writer.Dispose();
            stream.Dispose();
        }

        public static void WriteTextFileToStr(string pFile, string pStr)
        {
            FileStream stream = new FileStream(pFile, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("GB2312"));
            writer.Flush();
            writer.BaseStream.Seek(0L, SeekOrigin.Begin);
            writer.Write(pStr);
            writer.Close();
            stream.Close();
            writer.Dispose();
            stream.Dispose();
        }

        public static string ConfigurationFile =>
            (getDllPath() + @"\Configuration.txt");

        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);
    }
}

