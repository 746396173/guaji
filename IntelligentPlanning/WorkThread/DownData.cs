namespace IntelligentPlanning.WorkThread
{
    using IntelligentPlanning;
    using Microsoft.VisualBasic;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Xml;

    internal class DownData
    {
        public List<string> DataList = new List<string>();
        public bool IsRefreshOpenData = false;
        private MainThread mainThread;
        public string OpenDataNextExpect = "";
        public OpenDataClass OpenDataOption;
        public PTBase PTInfo;
        public int RefeshExpect = 0;
        public int SaveExpect = 0;
        public int SQLRowCount = -1;
        public List<string> TempAddList = new List<string>();
        public ConfigurationStatus.LotteryType type;

        public DownData(MainThread pThread, ConfigurationStatus.LotteryType pType, PTBase pInfo, int pRefreshExpect, int pSaveExpect)
        {
            this.mainThread = pThread;
            this.type = pType;
            this.RefeshExpect = pRefreshExpect;
            this.SaveExpect = pSaveExpect;
            this.PTInfo = pInfo;
            this.OpenDataOption = new OpenDataClass();
            this.LoadParameter();
        }

        private bool CheckCodeLen(string pCode)
        {
            int length = pCode.Length;
            switch (CommFunc.GetLotteryGroup(this.OpenDataOption.Name, this.OpenDataOption.ID))
            {
                case ConfigurationStatus.LotteryGroup.GPSSC:
                    return ((length == 5) && CommFunc.CheckIsNumber(pCode));

                case ConfigurationStatus.LotteryGroup.GP11X5:
                    return (length == 14);

                case ConfigurationStatus.LotteryGroup.GPPK10:
                    return (length == 20);

                case ConfigurationStatus.LotteryGroup.GP3D:
                    return (length == 3);
            }
            return false;
        }

        private bool CheckExpect(Dictionary<string, string> pAddData, ref bool xReturn)
        {
            if (this.RefeshExpect != 0)
            {
                bool flag = pAddData.Count >= this.RefeshExpect;
                bool flag2 = false;
                if (pAddData.Count == 1)
                {
                    string str = CommFunc.GetDicKeyList<string>(pAddData)[0].Split(new char[] { '\t' })[0];
                    if (CommFunc.CountNextExpect(this.GetOpenDataFirstExpect, "") != str)
                    {
                        flag2 = true;
                    }
                    else
                    {
                        xReturn = true;
                    }
                }
                if (flag || flag2)
                {
                    this.DataList.Clear();
                    SQLServer.DeleteSQLData(this.OpenDataOption.ID);
                    this.SQLRowCount = 0;
                    xReturn = true;
                    return true;
                }
            }
            return false;
        }

        private bool CheckExpectLen(string pExpect)
        {
            int length = pExpect.Length;
            if (((this.type == ConfigurationStatus.LotteryType.HGSSC) || (this.type == ConfigurationStatus.LotteryType.XJPSSC)) || (this.type == ConfigurationStatus.LotteryType.JNDSSC))
            {
                return ((length == 7) && CommFunc.CheckIsNumber(pExpect));
            }
            if (this.type == ConfigurationStatus.LotteryType.TWSSC)
            {
                return ((length == 9) && CommFunc.CheckIsNumber(pExpect));
            }
            return true;
        }

        private bool CheckIsPTLottery(ConfigurationStatus.LotteryType pType)
        {
            if (((pType == ConfigurationStatus.LotteryType.CQSSC) || (pType == ConfigurationStatus.LotteryType.XJSSC)) || (pType == ConfigurationStatus.LotteryType.TJSSC))
            {
                return false;
            }
            if (((pType == ConfigurationStatus.LotteryType.GD11X5) || (pType == ConfigurationStatus.LotteryType.SD11X5)) || (pType == ConfigurationStatus.LotteryType.JX11X5))
            {
                return false;
            }
            return ((pType == ConfigurationStatus.LotteryType.PK10) || true);
        }

        private void CommonLate(bool pStatus, Dictionary<string, string> pAddData = null)
        {
            if (pStatus && (pAddData != null))
            {
                foreach (string str in pAddData.Keys)
                {
                    this.DataList.Add(str);
                    this.TempAddList.Add(str);
                    this.SQLRowCount++;
                }
            }
        }

        private string ConverCheckData(string pCheckData)
        {
            string str = pCheckData;
            if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
            {
                return str;
            }
            string key = str.Split(new char[] { '\t' })[0];
            string str3 = str.Split(new char[] { '\t' })[1];
            string str4 = DateTime.Now.ToString("yyyy-MM-dd");
            if (AppInfo.PK10ExpectDic.ContainsKey(key))
            {
                str4 = AppInfo.PK10ExpectDic[key];
            }
            return (str4 + " " + key + "\t" + str3);
        }

        private void CountNextExpect()
        {
            try
            {
                if ((AppInfo.App == ConfigurationStatus.AppType.OpenData) && (this.DataList.Count > 0))
                {
                    string str = this.DataList[0];
                    string pExpect = str.Split(new char[] { '\t' })[0];
                    if (this.type == ConfigurationStatus.LotteryType.PK10)
                    {
                        pExpect = pExpect.Split(new char[] { ' ' })[1];
                    }
                    this.OpenDataNextExpect = CommFunc.CountNextExpect(pExpect, this.OpenDataOption.ID);
                }
            }
            catch
            {
            }
        }

        public void DefaultOption()
        {
            this.IsRefreshOpenData = false;
            this.OpenDataNextExpect = "";
        }

        private int GetDXLoginCount(int pMaxNum = 3)
        {
            int num = pMaxNum;
            if (AppInfo.App == ConfigurationStatus.AppType.OpenData)
            {
                num = 100;
            }
            return num;
        }

        private string GetNextExpect() => 
            ((AppInfo.App == ConfigurationStatus.AppType.OpenData) ? this.OpenDataNextExpect : AppInfo.Current.Lottery.NextExpect);

        private bool GetWeb11X5ByCaiLeLe1(string pUrl)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            try
            {
                string pReferer = pUrl;
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, 0x2710, "UTF-8", false);
                if (pResponsetext == "")
                {
                    return false;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(pResponsetext);
                XmlNodeList list = document.SelectNodes("xml/row");
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                for (int i = 0; i < list.Count; i++)
                {
                    XmlNode node = list[i];
                    string str3 = node.Attributes["expect"].Value;
                    if (str3.Substring(0, 2) == "20")
                    {
                        str3 = str3.Substring(2);
                    }
                    DateTime time = Convert.ToDateTime(node.Attributes["opentime"].Value);
                    string str4 = node.Attributes["opencode"].Value;
                    string key = str3 + "\t" + str4;
                    this.OpenDataOption.GetData++;
                    if (!dictionary2.ContainsKey(key))
                    {
                        if (!pAddData.ContainsKey(key))
                        {
                            pAddData[key] = "";
                            this.OpenDataOption.GetNewData++;
                        }
                    }
                    else
                    {
                        xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                        goto Label_01D3;
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_01D3:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebA6YL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"content=23%7C1%7C{pID}%7C7%7C&rand={DateTime.Now.ToOADate()}&command=lottery_request_transmit";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = (pResponsetext != "") ? pResponsetext.Substring(pResponsetext.IndexOf("^!^7|") + 5) : "";
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "|||", -1, CompareMethod.Binary))
                {
                    if (str6 != "")
                    {
                        List<string> list = CommFunc.SplitString(str6, "|", -1);
                        string pExpect = list[1];
                        pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                        string pCode = list[2];
                        pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                        if (this.CheckCodeLen(pCode))
                        {
                            string key = pExpect + "\t" + pCode;
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(key))
                            {
                                if (!pAddData.ContainsKey(key))
                                {
                                    pAddData[key] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_027C;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_027C:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebALGJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"content=23%7C1%7C{pID}%7C7%7C&rand={DateTime.Now.ToOADate()}&command=lottery_request_transmit";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = (pResponsetext != "") ? pResponsetext.Substring(pResponsetext.IndexOf("^!^7|") + 5) : "";
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "|||", -1, CompareMethod.Binary))
                {
                    if (str6 != "")
                    {
                        List<string> list = CommFunc.SplitString(str6, "|", -1);
                        string pExpect = list[1];
                        pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                        string pCode = list[2];
                        pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("北京赛车PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02E2;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02E2:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebAMBLR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "<tbody>", "</table>", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</tr>", -1, CompareMethod.Binary);
                int length = strArray.Length;
                if (length > 50)
                {
                    length = 50;
                }
                for (int i = 0; i < length; i++)
                {
                    string pStr = strArray[i];
                    List<string> list = CommFunc.SplitString(pStr, "</td>", -1);
                    string pExpect = CommFunc.GetIndexString(list[1], "<td>", "", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(list[2], "<td>", "", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02B6;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02B6:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebAMBLR1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = $"{this.PTInfo.GetLine()}/index.php/index/getLastKjData/{pID}";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string str4 = pResponsetext;
                if (!str4.Contains("\"actionNo\""))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str4;
                string pExpect = CommFunc.GetIndexString(pStr, "\"actionNo\":\"", "\"", 0);
                if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                {
                    pExpect = CommFunc.GetIndexString(pStr, "\"actionNo\":", ",", 0);
                }
                pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                if (!this.CheckExpectLen(pExpect))
                {
                    return false;
                }
                string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"data\":\"", "\"", 0), this.type);
                if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                {
                    pCode = CommFunc.ConvertPK10CodeByString(pCode);
                }
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string pCheckData = pExpect + "\t" + pCode;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebB6YL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "历史开奖", "<script>", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</td></tr>", -1, CompareMethod.Binary);
                if (this.OpenDataOption.Name.Contains("PK10"))
                {
                    strArray = Strings.Split(expression, "<ul>", -1, CompareMethod.Binary);
                }
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string pExpect = CommFunc.GetIndexString(pStr, "<tr><td>", "<", 0);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        if (i == 0)
                        {
                            continue;
                        }
                        pExpect = CommFunc.GetIndexString(pStr, "<li>", "<", 0);
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    string[] strArray2 = Strings.Filter(Strings.Split(pStr, "</span>", -1, CompareMethod.Binary), "<span>", true, CompareMethod.Binary);
                    List<string> pList = new List<string>();
                    for (int j = 0; j < strArray2.Length; j++)
                    {
                        string str7 = strArray2[j];
                        pList.Add(CommFunc.GetIndexString(str7, "<span>", "", 0));
                    }
                    string pCode = CommFunc.ConvertCode(CommFunc.Join(pList), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(CommFunc.Join(pList, ","), ",");
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0359;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0359:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebBAYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "{", "}}]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = strArray.Length - 1; i >= 0; i--)
                {
                    string pStr = strArray[i];
                    string pExpect = CommFunc.GetIndexString(pStr, "\"period\":\"", "\"", 0);
                    if (!(CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID) || (this.type == ConfigurationStatus.LotteryType.PK10)))
                    {
                        pExpect = "20" + pExpect;
                    }
                    else if ((this.type == ConfigurationStatus.LotteryType.HGSSC) && (pExpect.Length == 5))
                    {
                        pExpect = "16" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    if (this.type == ConfigurationStatus.LotteryType.TJSSC)
                    {
                        pExpect = pExpect.Insert(6, "0");
                    }
                    string pCode = CommFunc.GetIndexString(pStr, "\"Number\":\"", "\"", 0);
                    if (this.type != ConfigurationStatus.LotteryType.PK10)
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02CA;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02CA:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebBAYL1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = $"{this.PTInfo.GetLine()}/client/do.php?a=do&m=lotnum&play={this.PTInfo.GetPTLotteryName(this.type)}&t={DateTime.Now.ToOADate()}";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string pStr = CommFunc.GetIndexString(pResponsetext, "\"list\":[[\"", "]]}", 0);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                List<string> list = CommFunc.SplitString(pStr, "\",\"", -1);
                if ((((pResponsetext == "") || (pStr == "")) || (list.Count != 4)) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                string pExpect = list[1];
                pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                string pCode = list[2];
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string pCheckData = pExpect + "\t" + pCode;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebBHGJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{", "}]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "'expect': '", "'", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "'opencode': '", "'", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0253;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0253:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebBHZY(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_027A;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_027A:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebBKC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string indexLine = this.PTInfo.GetIndexLine();
                string pData = $"gameID={this.PTInfo.GetBetsLotteryID(this.type)}";
                HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "history", "\"user\"", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                if (this.PTInfo.Prize == "")
                {
                    this.PTInfo.Rebate = CommFunc.GetIndexString(pResponsetext, "\"user\":", ",", 0);
                    this.PTInfo.Prize = (1800.0 + ((Convert.ToDouble(this.PTInfo.Rebate) * 2.0) * 10.0)).ToString();
                }
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "\"token_tz\":\"", "\"", 0);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = strArray.Length - 1; i >= 0; i--)
                {
                    string pStr = strArray[i];
                    string pExpect = CommFunc.GetIndexString(pStr, "\"issueno\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"nums\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0332;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0332:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebBKC1(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"type=1&nums=30&gametype={this.PTInfo.GetBetsLotteryID(this.type)}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "}},", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issueno\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"nums\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_029E;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_029E:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebBKC2()
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = "http://lottery90s.com?time=" + DateTime.Now.ToOADate();
                string pReferer = pUrl;
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, 0x4e20, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "<tr><td", "</table>", 0);
                if ((pResponsetext == "") || (expression == ""))
                {
                    return false;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</tr>", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "; '>", "<", 0).Trim(), this.type).Replace("-0", "-");
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "; '>", "<", pStr.IndexOf(" </td>")).Trim(), CommFunc.GetLotteryName(this.type));
                    if (this.CheckCodeLen(pCode))
                    {
                        string key = str6 + "\t" + pCode;
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(key))
                        {
                            if (!pAddData.ContainsKey(key))
                            {
                                pAddData[key] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0203;
                        }
                    }
                }
            }
            catch
            {
            }
        Label_0203:
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebBLGJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02AF;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02AF:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebBMEI(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"content=23%7C1%7C{pID}%7C7%7C&rand={DateTime.Now.ToOADate()}&command=lottery_request_transmit";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = (pResponsetext != "") ? pResponsetext.Substring(pResponsetext.IndexOf("^!^7|") + 5) : "";
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "|||", -1, CompareMethod.Binary))
                {
                    if (str6 != "")
                    {
                        List<string> list = CommFunc.SplitString(str6, "|", -1);
                        string pExpect = list[1];
                        pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                        string pCode = list[2];
                        pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02C8;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02C8:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebBMYX(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID, DateTime.Now.ToOADate());
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "J-textarea-historys-balls-data", "textarea", 0);
                if (((pResponsetext == "") || (str4 == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                string str5 = CommFunc.GetIndexString(pResponsetext, "\"_token\":\"", "\"", 0);
                if (str5 != "")
                {
                    this.PTInfo.Token = str5;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(str4.Substring(20), ",", -1, CompareMethod.Binary);
                for (int i = 0; i < strArray.Length; i++)
                {
                    List<string> pSkipList = new List<string> { 
                        " ",
                        "="
                    };
                    string number = CommFunc.GetNumber(strArray[i], pSkipList);
                    string str7 = CommFunc.ConvertExpect("20" + number.Split(new char[] { '=' })[0], this.type);
                    string pCode = number.Split(new char[] { '=' })[1];
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.CheckCodeLen(pCode))
                    {
                        string key = str7 + "\t" + pCode;
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(key))
                        {
                            if (!pAddData.ContainsKey(key))
                            {
                                pAddData[key] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02B4;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02B4:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebBNGJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"content=23%7C1%7C{pID}%7C7%7C&rand={DateTime.Now.ToOADate()}&command=lottery_request_transmit";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = (pResponsetext != "") ? pResponsetext.Substring(pResponsetext.IndexOf("^!^7|") + 5) : "";
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "|||", -1, CompareMethod.Binary))
                {
                    if (str6 != "")
                    {
                        List<string> list = CommFunc.SplitString(str6, "|", -1);
                        string pExpect = list[1];
                        pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                        string pCode = list[2];
                        pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02C8;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02C8:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebBWT(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0295;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0295:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebByServer(string pServerUrl, string pServerSqlUrl, string pExpect)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string pReferer = pServerUrl;
                string pUrl = $"{pServerSqlUrl}?action=getOpenData&App={AppInfo.cAppName}&LotteryID={this.OpenDataOption.ID}&LastExpect={pExpect}&ExpectCount={this.RefeshExpect}&Mkey={mdkey}";
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
                switch (pResponsetext)
                {
                    case "":
                    case "-1":
                        if (pResponsetext == "-1")
                        {
                            AppInfo.IsPassApp = true;
                        }
                        return false;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(pResponsetext, "\r\n", -1, CompareMethod.Binary))
                {
                    if (str5 != "")
                    {
                        string[] strArray2 = str5.Split(new char[] { '\t' });
                        string str6 = strArray2[0];
                        string pCode = strArray2[1];
                        if (this.CheckCodeLen(pCode))
                        {
                            string key = str6 + "\t" + pCode;
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(key))
                            {
                                if (!pAddData.ContainsKey(key))
                                {
                                    pAddData[key] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_0224;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0224:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCAIH(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.type == ConfigurationStatus.LotteryType.CAIHNY15C)
                    {
                        pCode = pCode.Substring(0, 5);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EE;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EE:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCBL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.type == ConfigurationStatus.LotteryType.CAIHNY15C)
                    {
                        pCode = pCode.Substring(0, 5);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EE;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EE:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCBLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCCYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse8(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"data\":[", "\"code\":1000", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "\"}},{\"", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"issue\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"result\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_025B;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_025B:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCCYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02DD;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02DD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCLYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "<div class=\"row five-ball-open five-ball-mobile\">", "<div class=\"col-lg-6 col-md-6 col-sm-12 col-xs-12 lottery-mobile-open\">", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 10, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "cycle_value = '", "'", 0);
                this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.ExpectID = CommFunc.GetIndexString(pResponsetext, "game_cycle_id = '", "'", 0);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "<div class=\"row five-ball-open five-ball-mobile\">", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\">", "<", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string[] strArray2 = Strings.Filter(Strings.Split(str5, "</span>", -1, CompareMethod.Binary), "ball-base-five ball03", true, CompareMethod.Binary);
                    List<string> pList = new List<string>();
                    foreach (string str7 in strArray2)
                    {
                        pList.Add(Strings.Split(str7, "\"ball-base-five ball03\">", -1, CompareMethod.Binary)[1]);
                    }
                    string pCode = CommFunc.ConvertCode(CommFunc.Join(pList), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Join(pList, ",");
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_036F;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_036F:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCP361(int pID, int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID, pCount) + "&time=" + DateTime.Now.ToOADate();
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "table", "table", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</tr>", -1, CompareMethod.Binary);
                DateTime now = DateTime.Now;
                if (CommFunc.CheckLotteryIsBD(this.OpenDataOption.ID) && ((now.Hour >= 0) && (now.Hour <= 7)))
                {
                    now = now.AddDays(-1.0);
                }
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    int startIndex = pStr.IndexOf("#FFFFFF;\">") + 10;
                    int index = pStr.IndexOf("&#", startIndex);
                    string str6 = pStr.Substring(startIndex, index - startIndex);
                    if (((i == 0) && (now.Hour == 8)) && ((str6 == "1380") || (str6 == "690")))
                    {
                        xReturn = !this.mainThread.SingleMode;
                        goto Label_0398;
                    }
                    string str7 = CommFunc.ConvertExpect(now.ToString("yyyyMMdd") + "-" + str6, this.type);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        str7 = str7.Split(new char[] { '-' })[1];
                    }
                    switch (str6)
                    {
                        case "0001":
                        case "001":
                            now = now.AddDays(-1.0);
                            break;
                    }
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "#90ff00;\">", "\n", 0), this.type);
                    if (this.CheckCodeLen(pCode))
                    {
                        string key = str7 + "\t" + pCode;
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(key))
                        {
                            if (!pAddData.ContainsKey(key))
                            {
                                pAddData[key] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0398;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0398:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCP3611(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/LotteryService.aspx";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(this.GetNextExpect(), this.OpenDataOption.ID);
                string pData = $"lotteryid={pID}&flag=gethistory&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("'code':["))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "'issue':'", "'", 0), this.type);
                string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "'code':[", "]", 0).Replace("'", ""), this.type);
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string key = str8 + "\t" + pCode;
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(key))
                {
                    if (!pAddData.ContainsKey(key))
                    {
                        pAddData[key] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebCTT(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"id={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = strArray.Length - 1; i >= 0; i--)
                {
                    string pStr = strArray[i];
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_025D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_025D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCTT1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/lottery/get_last_win_code";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(AppInfo.Current.Lottery.NextExpect, "");
                string pData = $"id={pID}&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"issue\":\""))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                if (!this.CheckExpectLen(pExpect))
                {
                    return false;
                }
                string expression = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                if (Strings.Split(expression, ",", -1, CompareMethod.Binary).Length == 20)
                {
                    expression = CommFunc.ConvertHGSSCCode(expression);
                }
                else if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                {
                    expression = CommFunc.ConvertCode(expression, this.type);
                }
                if (!this.CheckCodeLen(expression))
                {
                    return false;
                }
                string pCheckData = pExpect + "\t" + expression;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebCTTVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02DD;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02DD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCTX(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02CD;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02CD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCTXVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCYYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.type == ConfigurationStatus.LotteryType.CAIHNY15C)
                    {
                        pCode = pCode.Substring(0, 5);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EE;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EE:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebCYYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebDACP(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"round\"", "]}]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = 1; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"round\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(pStr, "\"numbers\":[", "]", 0).Replace("\"", "");
                    if (!pCode.Contains("?"))
                    {
                        if (!this.OpenDataOption.Name.Contains("PK10"))
                        {
                            pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str6 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_026B;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_026B:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        public bool GetWebDate()
        {
            string getOpenDataFirstExpect;
            if (AppInfo.App == ConfigurationStatus.AppType.OpenData)
            {
                while (!this.PTInfo.PTLoginStatus)
                {
                    Thread.Sleep(0x3e8);
                }
            }
            if (this.PTInfo.PTLoginStatus)
            {
                List<int> list;
                bool flag;
                List<string> list2;
                int pID = Convert.ToInt32(this.PTInfo.GetBetsLotteryID(this.type));
                if (this.PTInfo == AppInfo.CP361Info)
                {
                    list = new List<int> { 
                        0x17,
                        0,
                        1
                    };
                    flag = list.Contains(DateTime.Now.Hour);
                    list2 = new List<string> { 
                        "CQSSC",
                        "FLBSSC"
                    };
                    if ((((AppInfo.App == ConfigurationStatus.AppType.OpenData) || !list2.Contains(this.OpenDataOption.ID)) || !flag) && this.GetWebCP361(pID, 60))
                    {
                        return true;
                    }
                    if (this.GetWebCP3611(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.MRYLInfo)
                {
                    list = new List<int> { 
                        0x17,
                        0,
                        1
                    };
                    flag = list.Contains(DateTime.Now.Hour);
                    list2 = new List<string> { 
                        "CQSSC",
                        "FLBSSC"
                    };
                    if ((!list2.Contains(this.OpenDataOption.ID) || !flag) && this.GetWebMRYL(pID, 60))
                    {
                        return true;
                    }
                    if (this.GetWebMRYL1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HRCPInfo)
                {
                    list = new List<int> { 
                        0x17,
                        0,
                        1
                    };
                    flag = list.Contains(DateTime.Now.Hour);
                    list2 = new List<string> { 
                        "CQSSC",
                        "FLBSSC",
                        "QJCTXFFC"
                    };
                    if ((!list2.Contains(this.OpenDataOption.ID) || !flag) && this.GetWebHRCP(pID, 60))
                    {
                        return true;
                    }
                    if (this.GetWebHRCP1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.DTCPInfo)
                {
                    list = new List<int> { 
                        0x17,
                        0,
                        1
                    };
                    flag = list.Contains(DateTime.Now.Hour);
                    list2 = new List<string> { 
                        "CQSSC",
                        "FLBSSC"
                    };
                    if ((((AppInfo.App == ConfigurationStatus.AppType.OpenData) || !list2.Contains(this.OpenDataOption.ID)) || !flag) && this.GetWebDTCP(pID, 60))
                    {
                        return true;
                    }
                    if (this.GetWebDTCP1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.TRYLInfo)
                {
                    List<int> list11 = new List<int> { 
                        0x17,
                        0,
                        1
                    };
                    flag = list11.Contains(DateTime.Now.Hour);
                    list2 = new List<string> { 
                        "CQSSC",
                        "FLBSSC",
                        "QJCTXFFC"
                    };
                    if ((((AppInfo.App == ConfigurationStatus.AppType.OpenData) || !list2.Contains(this.OpenDataOption.ID)) || !flag) && this.GetWebTRYL(pID, 60))
                    {
                        return true;
                    }
                    if (this.GetWebTRYL1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YBAOInfo)
                {
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        if (this.GetWebYBAOByPK10(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebYBAO(pID))
                    {
                        return true;
                    }
                    if (this.GetWebYBAO1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.BMYXInfo)
                {
                    if (this.GetWebBMYX(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.OEYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebOEYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebOEYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.MZCInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebMZCVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebMZC(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.UT8Info)
                {
                    if (this.GetWebUT8(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.M5CPInfo)
                {
                    if (this.GetWebM5CP(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.DACPInfo)
                {
                    if (this.GetWebDACP(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WJSJInfo)
                {
                    if (this.GetWebWJSJ(pID))
                    {
                        return true;
                    }
                    if (this.GetWebWJSJ1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.UCYLInfo)
                {
                    if (this.GetWebUCYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.LFYLInfo)
                {
                    if (this.GetWebLFYL(pID))
                    {
                        return true;
                    }
                    if (this.GetWebLFYL1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.LFGJInfo)
                {
                    if (this.GetWebLFGJ(pID))
                    {
                        return true;
                    }
                    if (this.GetWebLFGJ1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.BNGJInfo)
                {
                    if (this.GetWebBNGJ(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WXYLInfo)
                {
                    if (this.GetWebWXYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.LXYLInfo)
                {
                    if (this.GetWebLXYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.BAYLInfo)
                {
                    if (this.GetWebBAYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.SIJIInfo)
                {
                    if (this.GetWebSIJI(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YYZXInfo)
                {
                    if (this.GetWebYYZX(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.JYGJInfo)
                {
                    if (this.GetWebJYGJ(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HBSInfo)
                {
                    if (this.GetWebHBS(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.XB3Info)
                {
                    if (this.GetWebXB3(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.K5YLInfo)
                {
                    if (this.GetWebK5YL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.FSYLInfo)
                {
                    if (this.GetWebFSYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.BHZYInfo)
                {
                    if (this.GetWebBHZY(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.A6YLInfo)
                {
                    if (this.GetWebA6YL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.JHYLInfo)
                {
                    if (this.GetWebJHYL(pID))
                    {
                        return true;
                    }
                    if ((((this.type != ConfigurationStatus.LotteryType.JHTXFFC) && (this.type != ConfigurationStatus.LotteryType.JHQQFFC)) && (this.type != ConfigurationStatus.LotteryType.PK10)) && this.GetWebJHYL1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.ZDYLInfo)
                {
                    if (this.GetWebZDYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.SKYYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebSKYYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebSKYYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.DPCInfo)
                {
                    if (this.GetWebDPC(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.LUDIInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebLUDIVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebLUDI(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.LF2Info)
                {
                    if (this.GetWebLF2(pID))
                    {
                        return true;
                    }
                    if (this.GetWebLF21(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YRYLInfo)
                {
                    if (this.GetWebYRYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HLCInfo)
                {
                    if (this.GetWebHLC(pID))
                    {
                        return true;
                    }
                    if (this.GetWebHLC1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HGDBInfo)
                {
                    if (this.GetWebHGDB(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WSYLInfo)
                {
                    if (this.GetWebWSYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.BMEIInfo)
                {
                    if (this.GetWebBMEI(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.QQT2Info)
                {
                    if (this.GetWebQQT2(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WBJInfo)
                {
                    if (this.GetWebWBJ(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.THDYLInfo)
                {
                    if (this.GetWebTHDYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.XHDFInfo)
                {
                    if (this.GetWebXHDF(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.NBYLInfo)
                {
                    if (this.GetWebNBYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YSENInfo)
                {
                    if (this.GetWebYSEN(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HYYLInfo)
                {
                    if (this.GetWebHYYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WMYLInfo)
                {
                    if (this.GetWebWMYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.TAYLInfo)
                {
                    if (this.GetWebTAYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YDYLInfo)
                {
                    if (this.GetWebYDYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.BKCInfo)
                {
                    if (this.PTInfo.Token == "")
                    {
                        if (this.GetWebBKC(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.type == ConfigurationStatus.LotteryType.JN15F)
                    {
                        if (this.GetWebBKC2())
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebBKC1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.MINCInfo)
                {
                    if (this.GetWebMINC(pID))
                    {
                        return true;
                    }
                    if (this.GetWebMINC1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HZYLInfo)
                {
                    if (this.GetWebHZYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.ZXYLInfo)
                {
                    if (this.GetWebZXYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.DQYLInfo)
                {
                    if (this.GetWebDQYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.XCYLInfo)
                {
                    if (this.GetWebXCYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.FCYLInfo)
                {
                    if (this.GetWebFCYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.FEICInfo)
                {
                    if (this.GetWebFEIC(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.LMHInfo)
                {
                    if (this.GetWebLMH(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HANYInfo)
                {
                    if (this.GetWebHANY(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.CAIHInfo)
                {
                    if (this.GetWebCAIH(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.LYSInfo)
                {
                    if (this.GetWebLYS(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HCYLInfo)
                {
                    if (this.GetWebHCYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.THYLInfo)
                {
                    if (this.GetWebTHYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HSGJInfo)
                {
                    if (this.GetWebHSGJ(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.DAZYLInfo)
                {
                    if (this.GetWebDAZYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.SLTHInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebSLTHVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebSLTH(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.RDYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebRDYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (this.GetWebRDYL(pID))
                        {
                            return true;
                        }
                        if (this.GetWebRDYL1(pID))
                        {
                            return true;
                        }
                    }
                }
                else if (this.PTInfo == AppInfo.K3YLInfo)
                {
                    if (this.GetWebK3YL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.JFYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebJFYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebJFYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.FLCInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebFLCVR(pID))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (this.GetWebFLC(pID))
                        {
                            return true;
                        }
                        if (this.GetWebFLC1(pID))
                        {
                            return true;
                        }
                    }
                }
                else if (this.PTInfo == AppInfo.MYGJInfo)
                {
                    if (this.GetWebMYGJ(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.CTTInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebCTTVR(pID))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (this.GetWebCTT(pID))
                        {
                            return true;
                        }
                        if (this.GetWebCTT1(pID))
                        {
                            return true;
                        }
                    }
                }
                else if (this.PTInfo == AppInfo.JHCInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebJHCVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebJHC(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.KSYLInfo)
                {
                    if (this.GetWebKSYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WDYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebWDYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebWDYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.QJCInfo)
                {
                    if (this.GetWebQJC(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HNYLInfo)
                {
                    if (this.GetWebHNYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WHCInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebWHCVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebWHC(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.BWTInfo)
                {
                    if (this.GetWebBWT(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.ZYLInfo)
                {
                    if (this.GetWebZYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.CLYLInfo)
                {
                    if (this.GetWebCLYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.CBLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebCBLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebCBL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YL2028Info)
                {
                    if (this.GetWebYL2028(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.THENInfo)
                {
                    if (this.GetWebTHEN(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.LDYLInfo)
                {
                    if (this.GetWebLDYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HKCInfo)
                {
                    if (this.PTInfo.Token == "")
                    {
                        if (this.GetWebHKC(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebHKC1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.SYYLInfo)
                {
                    if (this.GetWebSYYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.MTYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebMTYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebMTYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HUBOInfo)
                {
                    if (this.GetWebHUBO(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.XCAIInfo)
                {
                    if (this.GetWebXCAI(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HENRInfo)
                {
                    if (this.GetWebHENR(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.LGZXInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebLGZXVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebLGZX(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WHENInfo)
                {
                    if (this.GetWebWHEN(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.JHC2Info)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebJHC2VR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebJHC2(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HUIZInfo)
                {
                    if (this.GetWebHUIZ(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HDYLInfo)
                {
                    if (this.GetWebHDYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.ALGJInfo)
                {
                    if (this.GetWebALGJ(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.KYYLInfo)
                {
                    if (this.GetWebKYYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.CCYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebCCYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebCCYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.GJYLInfo)
                {
                    if (this.GetWebGJYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.LSWJSInfo)
                {
                    if (this.GetWebLSWJS(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.CTXInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebCTXVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebCTX(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.JCXInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebJCXVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebJCX(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.KXYLInfo)
                {
                    if (this.GetWebKXYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.ZLJInfo)
                {
                    if (this.GetWebZLJ(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.SSHCInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebSSHCVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebSSHC(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.XHSDInfo)
                {
                    if (this.GetWebXHSD(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HCZXInfo)
                {
                    if (this.GetWebHCZX(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.BHGJInfo)
                {
                    if (this.GetWebBHGJ(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.XDBInfo)
                {
                    if (this.GetWebXDB(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.DJYLInfo)
                {
                    if (this.GetWebDJYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.DYYLInfo)
                {
                    if (this.GetWebDYYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HONDInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebHONDVR(pID))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (this.GetWebHOND(pID))
                        {
                            return true;
                        }
                        if (this.GetWebHOND1(pID))
                        {
                            return true;
                        }
                    }
                }
                else if (this.PTInfo == AppInfo.QFYLInfo)
                {
                    if (this.GetWebQFYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.TYYLInfo)
                {
                    if (this.GetWebTYYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.AMBLRInfo)
                {
                    if (this.GetWebAMBLR1(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.JXYLInfo)
                {
                    if (this.GetWebJXYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.XINCInfo)
                {
                    if (this.GetWebXINC(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YHYLInfo)
                {
                    if (this.GetWebYHYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.CYYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebCYYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebCYYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.BLGJInfo)
                {
                    if (this.GetWebBLGJ(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YBYLInfo)
                {
                    if (this.GetWebYBYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.JYYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebJYYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebJYYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WCYLInfo)
                {
                    if (this.GetWebWCYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WYYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebWYYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebWYYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.XHHCInfo)
                {
                    if (this.GetWebXHHC(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.NBAYLInfo)
                {
                    if (this.GetWebNBAYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WEYLInfo)
                {
                    if (this.GetWebWEYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.MCYLInfo)
                {
                    if (this.GetWebMCYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.MXYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebMXYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebMXYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WCAIInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebWCAIVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebWCAI(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.QQYLInfo)
                {
                    if (this.GetWebQQYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YHSGInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebYHSGVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebYHSG(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YINHInfo)
                {
                    if (this.OpenDataOption.Name == "北京赛车PK10")
                    {
                        getOpenDataFirstExpect = this.GetOpenDataFirstExpect;
                        if (this.GetWebByServer(ServerUrl, ServerSqlUrl, getOpenDataFirstExpect))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebYINH(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HENDInfo)
                {
                    if (this.GetWebHEND(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.XGLLInfo)
                {
                    if (this.GetWebXGLL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.DEJIInfo)
                {
                    if (this.GetWebDEJI(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.JLGJInfo)
                {
                    if (this.GetWebJLGJ(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.XTYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebXTYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebXTYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.XWYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebXWYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebXWYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.B6YLInfo)
                {
                    if (this.OpenDataOption.ID == "TXFFC")
                    {
                        getOpenDataFirstExpect = this.GetOpenDataFirstExpect;
                        if (this.GetWebByServer(ServerUrl, ServerSqlUrl, getOpenDataFirstExpect))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebB6YL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.TBYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebTBYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebTBYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WZYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsZB(this.type))
                    {
                        if (this.GetWebWZYLZB(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebWZYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YZCPInfo)
                {
                    if (this.PTInfo.CheckLotteryIsZB(this.type))
                    {
                        if (this.GetWebYZCPZB(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebYZCP(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.TIYUInfo)
                {
                    if (this.GetWebTIYU(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YCYLInfo)
                {
                    if (this.GetWebYCYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.ZBYLInfo)
                {
                    if (this.GetWebZBYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.FNYXInfo)
                {
                    if (this.GetWebFNYX(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.HUAYInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebHUAYVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebHUAY(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.YXZXInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebYXZXVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebYXZX(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.WTYLInfo)
                {
                    if (this.GetWebWTYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.TCYLInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebTCYLVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebTCYL(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.QFZXInfo)
                {
                    if (this.PTInfo.CheckLotteryIsVR(this.type))
                    {
                        if (this.GetWebQFZXVR(pID))
                        {
                            return true;
                        }
                    }
                    else if (this.GetWebQFZX(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.JXINInfo)
                {
                    if (this.GetWebJXIN(pID))
                    {
                        return true;
                    }
                }
                else if (this.PTInfo == AppInfo.ZBEIInfo)
                {
                    if (this.GetWebZBEI(pID))
                    {
                        return true;
                    }
                }
                else if ((this.PTInfo == AppInfo.XQYLInfo) && this.GetWebXQYL(pID))
                {
                    return true;
                }
            }
            else if (AppInfo.App != ConfigurationStatus.AppType.OpenData)
            {
                AppInfo.IsPassApp = false;
                if (!AppInfo.Current.Lottery.IsLoadServerData || this.CheckIsPTLottery(this.type))
                {
                    getOpenDataFirstExpect = this.GetOpenDataFirstExpect;
                    if (this.GetWebByServer(ServerUrl, ServerSqlUrl, getOpenDataFirstExpect))
                    {
                        return true;
                    }
                }
                else
                {
                    string str2;
                    string str3;
                    if (this.type == ConfigurationStatus.LotteryType.CQSSC)
                    {
                        str2 = "http://www.cailele.com/static/ssc/newlyopenlist.xml";
                        if (this.GetWebSSCByCaiLeLe(str2))
                        {
                            return true;
                        }
                        str3 = "http://caipiao.163.com/award/cqssc/{0}.html";
                        if (this.GetWebSSCBy163(str3))
                        {
                            return true;
                        }
                    }
                    else if (this.type == ConfigurationStatus.LotteryType.XJSSC)
                    {
                        str3 = "http://www.xjflcp.com/game/SelectDate";
                        if (this.GetWebSSCByXJFLCP(str3))
                        {
                            return true;
                        }
                    }
                    else if (this.type == ConfigurationStatus.LotteryType.TJSSC)
                    {
                        string pUrl = "http://zst.cjcp.com.cn/cjwssc/view/ssc_hezhi-hezhi-5-tianjinssc-1-{0}001-{1}084-9.html";
                        if (this.GetWebSSCByCaiJing(pUrl))
                        {
                            return true;
                        }
                    }
                    else if (this.type == ConfigurationStatus.LotteryType.GD11X5)
                    {
                        str2 = "http://www.cailele.com/static/gd11x5/newlyopenlist.xml";
                        if (this.GetWeb11X5ByCaiLeLe1(str2))
                        {
                            return true;
                        }
                    }
                    else if (this.type == ConfigurationStatus.LotteryType.SD11X5)
                    {
                        str2 = "http://www.cailele.com/static/11yun/newlyopenlist.xml";
                        if (this.GetWeb11X5ByCaiLeLe1(str2))
                        {
                            return true;
                        }
                    }
                    else if (this.type == ConfigurationStatus.LotteryType.JX11X5)
                    {
                        str2 = "http://www.cailele.com/static/jx11x5/newlyopenlist.xml";
                        if (this.GetWeb11X5ByCaiLeLe1(str2))
                        {
                            return true;
                        }
                    }
                }
                bool flag2 = !this.mainThread.SingleMode;
                if (AppInfo.IsPassApp)
                {
                    flag2 = false;
                }
                return flag2;
            }
            return false;
        }

        private bool GetWebDAZYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse2(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "{\n", "\n}", 0);
                int dXLoginCount = this.GetDXLoginCount(5);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\" : \"", "\"", 0);
                this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"wn_number\" : \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (pCode != "")
                    {
                        if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_031F;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_031F:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebDEJI(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02AF;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02AF:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebDJYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"period\":", ",", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str5, "\"result\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("PK10") && !this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        if (this.OpenDataOption.Name.Contains("11选5"))
                        {
                            pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                        }
                        else
                        {
                            pCode = CommFunc.ConvertCode(pCode, this.type);
                        }
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A4;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A4:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebDPC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A2;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A2:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebDQYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"period\":", ",", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str5, "\"result\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("PK10") && !this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        if (this.OpenDataOption.Name.Contains("11选5"))
                        {
                            pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                        }
                        else
                        {
                            pCode = CommFunc.ConvertCode(pCode, this.type);
                        }
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A4;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A4:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebDTCP(int pID, int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID, pCount) + "&time=" + DateTime.Now.ToOADate();
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "ewinnumber", "</tbody>", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</tr>", -1, CompareMethod.Binary);
                DateTime now = DateTime.Now;
                if ((CommFunc.CheckLotteryIsBD(this.OpenDataOption.ID) && (this.type != ConfigurationStatus.LotteryType.FLBSSC)) && ((now.Hour >= 0) && (now.Hour <= 7)))
                {
                    now = now.AddDays(-1.0);
                }
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.GetIndexString(pStr, "<td>", "&", 0);
                    if (((i == 0) && (now.Hour == 8)) && ((str6 == "1380") || (str6 == "690")))
                    {
                        xReturn = !this.mainThread.SingleMode;
                        goto Label_040A;
                    }
                    string pExpect = str6;
                    if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pExpect = now.ToString("yyyyMMdd") + "-" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pExpect = pExpect.Split(new char[] { '-' })[1];
                    }
                    switch (str6)
                    {
                        case "0001":
                        case "001":
                            now = now.AddDays(-1.0);
                            break;
                    }
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"color01\">", "<", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_040A;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_040A:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebDTCP1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/LotteryService.aspx";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(this.GetNextExpect(), this.OpenDataOption.ID);
                string pData = $"lotteryid={pID}&flag=gethistory&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("'code':["))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "'issue':'", "'", 0), this.type);
                string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "'code':[", "]", 0).Replace("'", ""), this.type);
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string key = str8 + "\t" + pCode;
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(key))
                {
                    if (!pAddData.ContainsKey(key))
                    {
                        pAddData[key] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebDYYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"issues\":", "\"last_number\"", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                string pExpect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\":\"", "\"", 0);
                if ((pExpect != "") && !CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                {
                    pExpect = this.PTInfo.GetAppExpect(this.type, pExpect, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        pExpect = "20" + pExpect;
                    }
                    this.PTInfo.Expect = CommFunc.ConvertExpect(pExpect, this.type);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    str7 = this.PTInfo.GetAppExpect(this.type, str7, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        str7 = "20" + str7;
                    }
                    str7 = CommFunc.ConvertExpect(str7, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"wn_number\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0336;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0336:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebFCYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"period\":", ",", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str5, "\"result\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("PK10") && !this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        if (this.OpenDataOption.Name.Contains("11选5"))
                        {
                            pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                        }
                        else
                        {
                            pCode = CommFunc.ConvertCode(pCode, this.type);
                        }
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A4;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A4:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebFEIC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"issues\":", "\"last_number\"", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                {
                    string pExpect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\":\"", "\"", 0);
                    if (this.type != ConfigurationStatus.LotteryType.FEICDJSSC)
                    {
                        pExpect = "20" + pExpect;
                    }
                    this.PTInfo.Expect = CommFunc.ConvertExpect(pExpect, this.type);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    if (!(CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID) || (this.type == ConfigurationStatus.LotteryType.FEICDJSSC)))
                    {
                        str7 = "20" + str7;
                    }
                    str7 = CommFunc.ConvertExpect(str7, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"wn_number\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0306;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0306:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebFLC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"id={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = strArray.Length - 1; i >= 0; i--)
                {
                    string pStr = strArray[i];
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_025D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_025D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebFLC1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/lottery/get_last_win_code";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(AppInfo.Current.Lottery.NextExpect, "");
                string pData = $"id={pID}&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"issue\":\""))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                if (!this.CheckExpectLen(pExpect))
                {
                    return false;
                }
                string expression = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                if (Strings.Split(expression, ",", -1, CompareMethod.Binary).Length == 20)
                {
                    expression = CommFunc.ConvertHGSSCCode(expression);
                }
                else if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                {
                    expression = CommFunc.ConvertCode(expression, this.type);
                }
                if (!this.CheckCodeLen(expression))
                {
                    return false;
                }
                string pCheckData = pExpect + "\t" + expression;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebFLCVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02DD;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02DD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebFNYX(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"data\":[", "}]}", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"issue\":\"", "\"", 0);
                    if ((!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID) && (this.OpenDataOption.ID != "TXFFC")) && (this.OpenDataOption.ID != "FNYXHY11X5"))
                    {
                        pExpect = "20" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"code\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A6;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A6:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebFSYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x4e20, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"issues\" : [ {\n", "}\n}", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\" : \"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"issue\" : \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"wn_number\" : \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.CheckCodeLen(pCode))
                    {
                        string key = str6 + "\t" + pCode;
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(key))
                        {
                            if (!pAddData.ContainsKey(key))
                            {
                                pAddData[key] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_025F;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_025F:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebGJYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0295;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0295:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHANY(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"shortName={this.PTInfo.GetPTLotteryName(this.type)}&num=20";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"expect\":\"", "\"", 0), this.type);
                    if (str7 != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"openCode\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str7 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_0281;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0281:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHBS(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"shortName={this.PTInfo.GetPTLotteryName(this.type)}&num=20";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"expect\":\"", "\"", 0), this.type);
                    if (str7 != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"openCode\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str7 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_0277;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0277:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHCYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"period\":", ",", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str5, "\"result\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("PK10") && !this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        if (this.OpenDataOption.Name.Contains("11选5"))
                        {
                            pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                        }
                        else
                        {
                            pCode = CommFunc.ConvertCode(pCode, this.type);
                        }
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A4;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A4:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHCZX(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = "";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{\"", "\"}]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"IssueNo\":\"", "\"", 0), this.type);
                    if (str7 != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"LotteryOpenNo\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛狗"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str7 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_028B;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_028B:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHDYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse1(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"content\":[{", "}}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"numero\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"winNo\":\"", "\"", 0), this.type);
                    if ((this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("飞艇")) || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A9;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A9:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHEND(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"historyNumbers\":[{", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"currentNumber\":\"", "\"", 0);
                if (this.PTInfo.Expect != "")
                {
                    this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        this.PTInfo.Expect = "20" + this.PTInfo.Expect;
                    }
                    this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"number\":\"", "\"", 0);
                    pExpect = this.PTInfo.GetAppExpect(this.type, pExpect, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        pExpect = "20" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"code\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_036C;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_036C:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHENR(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[\"", "\"]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "\",\"", -1, CompareMethod.Binary))
                {
                    string[] strArray2 = str5.Split(new char[] { '|' });
                    string pExpect = strArray2[0];
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        pExpect = "20" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    string pCode = strArray2[1];
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_026F;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_026F:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHGDB(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A2;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A2:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHKC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string indexLine = this.PTInfo.GetIndexLine();
                string pData = $"gameID={this.PTInfo.GetBetsLotteryID(this.type)}";
                HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "history", "\"user\"", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                if (this.PTInfo.Prize == "")
                {
                    this.PTInfo.Rebate = CommFunc.GetIndexString(pResponsetext, "\"user\":", ",", 0);
                    this.PTInfo.Prize = (1800.0 + ((Convert.ToDouble(this.PTInfo.Rebate) * 2.0) * 10.0)).ToString();
                }
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "\"token_tz\":\"", "\"", 0);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = strArray.Length - 1; i >= 0; i--)
                {
                    string pStr = strArray[i];
                    string pExpect = CommFunc.GetIndexString(pStr, "\"issueno\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"nums\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0332;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0332:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHKC1(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"type=1&nums=30&gametype={this.PTInfo.GetBetsLotteryID(this.type)}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "}},", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issueno\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"nums\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_029E;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_029E:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHLC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"id={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = strArray.Length - 1; i >= 0; i--)
                {
                    string pStr = strArray[i];
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0250;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0250:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHLC1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/lottery/get_last_win_code";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(AppInfo.Current.Lottery.NextExpect, "");
                string pData = $"id={pID}&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"issue\":\""))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                if (!this.CheckExpectLen(pExpect))
                {
                    return false;
                }
                string expression = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                if (Strings.Split(expression, ",", -1, CompareMethod.Binary).Length == 20)
                {
                    expression = CommFunc.ConvertHGSSCCode(expression);
                }
                else if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                {
                    expression = CommFunc.ConvertCode(expression, this.type);
                }
                if (!this.CheckCodeLen(expression))
                {
                    return false;
                }
                string pCheckData = pExpect + "\t" + expression;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebHNYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{\"", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{\"", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.ConvertLotteryExpectChar(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\":\"", "\"", 0);
                    if (pCode.Contains("|"))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02F0;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02F0:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHOND(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"id={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = strArray.Length - 1; i >= 0; i--)
                {
                    string pStr = strArray[i];
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_025D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_025D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHOND1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/lottery/get_last_win_code";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(AppInfo.Current.Lottery.NextExpect, "");
                string pData = $"id={pID}&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"issue\":\""))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                if (!this.CheckExpectLen(pExpect))
                {
                    return false;
                }
                string expression = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                if (Strings.Split(expression, ",", -1, CompareMethod.Binary).Length == 20)
                {
                    expression = CommFunc.ConvertHGSSCCode(expression);
                }
                else if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                {
                    expression = CommFunc.ConvertCode(expression, this.type);
                }
                if (!this.CheckCodeLen(expression))
                {
                    return false;
                }
                string pCheckData = pExpect + "\t" + expression;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebHONDVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02DD;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02DD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHRCP(int pID, int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID, pCount) + "&time=" + DateTime.Now.ToOADate();
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "body", "body", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</tr>", -1, CompareMethod.Binary);
                DateTime now = DateTime.Now;
                if (CommFunc.CheckLotteryIsBD(this.OpenDataOption.ID) && ((now.Hour >= 0) && (now.Hour <= 7)))
                {
                    now = now.AddDays(-1.0);
                }
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.GetIndexString(pStr, "\"center\">", "&", 0);
                    if (((i == 0) && (now.Hour == 8)) && ((str6 == "1380") || (str6 == "690")))
                    {
                        xReturn = !this.mainThread.SingleMode;
                        goto Label_037F;
                    }
                    string str7 = CommFunc.ConvertExpect(now.ToString("yyyyMMdd") + "-" + str6, this.type);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        str7 = str7.Split(new char[] { '-' })[1];
                    }
                    switch (str6)
                    {
                        case "0001":
                        case "001":
                            now = now.AddDays(-1.0);
                            break;
                    }
                    string number = CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "5e4919;\">", "<", 0));
                    if (this.CheckCodeLen(number))
                    {
                        string key = str7 + "\t" + number;
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(key))
                        {
                            if (!pAddData.ContainsKey(key))
                            {
                                pAddData[key] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_037F;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_037F:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHRCP1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/LotteryService.aspx";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(this.GetNextExpect(), this.OpenDataOption.ID);
                string pData = $"lotteryid={pID}&flag=gethistory&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("'code':["))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "'issue':'", "'", 0), this.type);
                string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "'code':[", "]", 0).Replace("'", ""), this.type);
                if (this.OpenDataOption.Name.Contains("PK10"))
                {
                    pCode = CommFunc.ConvertPK10CodeByString(pCode);
                }
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string pCheckData = str8 + "\t" + pCode;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebHSGJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"content=23%7C1%7C{pID}%7C7%7C&rand={DateTime.Now.ToOADate()}&command=lottery_request_transmit";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = (pResponsetext != "") ? pResponsetext.Substring(pResponsetext.IndexOf("^!^7|") + 5) : "";
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "|||", -1, CompareMethod.Binary))
                {
                    if (str6 != "")
                    {
                        List<string> list = CommFunc.SplitString(str6, "|", -1);
                        string pExpect = list[1];
                        pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                        string pCode = list[2];
                        pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("北京赛车PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02E2;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02E2:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHUAY(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                string pData = $"(data:{pID})".Replace("(", "{").Replace(")", "}");
                HttpHelper.GetResponse1(ref pResponsetext, indexCode, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"data\":[{", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"n\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"o\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02AA;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02AA:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHUAYVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02DD;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02DD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHUBO(int pID)
        {
            if (this.PTInfo.WebCookie == "")
            {
                return false;
            }
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lid={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"recentBit\":[{", "}],", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.ExpectID = CommFunc.GetIndexString(pResponsetext, "\"id\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"series_number\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    this.PTInfo.Rebate = CommFunc.GetIndexString(pResponsetext, "\"bonuslevel\":\"", "\"", 0);
                    this.PTInfo.Prize = (1800.0 + ((Convert.ToDouble(this.PTInfo.Rebate) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"series_number\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"lottery_number\":\"", "\"", 0), this.type);
                    if (this.CheckCodeLen(pCode))
                    {
                        string key = str7 + "\t" + pCode;
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(key))
                        {
                            if (!pAddData.ContainsKey(key))
                            {
                                pAddData[key] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0329;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0329:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHUIZ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0);
                    if (this.type != ConfigurationStatus.LotteryType.HUIZFFPK10)
                    {
                        pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                    }
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02C4;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02C4:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHYYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"historys\":[{", "}]}", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"number\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"balls\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_026E;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_026E:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebHZYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"lotteryId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CzPeriod\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"CzNum\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_026D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_026D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJCX(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                string pData = $"type={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"Data\":[{", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"Issue\":", ",", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"Codes\":\"", "\"", 0);
                    if (!(this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车")))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02C3;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02C3:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJCXVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02DD;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02DD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJFYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1700.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0351;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0351:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJFYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0), this.type);
                    if (((this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车")) || this.OpenDataOption.Name.Contains("游泳")) || this.OpenDataOption.Name.Contains("快艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0330;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0330:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJHC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1700.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0351;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0351:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJHC2(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 10, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1700.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0353;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0353:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJHC2VR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJHCVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("快艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0307;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0307:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJHYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"action=get_lottery_open&lottery_code={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue_no\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"lotteryopen_no\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A8;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A8:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJHYL1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/tools/ssc_ajax.ashx";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(AppInfo.Current.Lottery.NextExpect, "");
                string pData = $"action=get_hg90open&lotterycode={pID}&issue_no={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"Code\":1"))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string pExpect = betsExpect;
                pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                string expression = CommFunc.GetIndexString(pStr, "\"StrCode\":\"", "\"", 0);
                if (Strings.Split(expression, ",", -1, CompareMethod.Binary).Length == 20)
                {
                    expression = CommFunc.ConvertHGSSCCode(expression);
                }
                else
                {
                    expression = CommFunc.ConvertCode(expression, CommFunc.GetLotteryName(this.type));
                }
                if (!this.CheckCodeLen(expression))
                {
                    return false;
                }
                string pCheckData = pExpect + "\t" + expression;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebJLGJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02AF;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02AF:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJXIN(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"num=20&lottery={this.PTInfo.GetPTLotteryName(this.type)}&token={this.PTInfo.Token}&user_id={this.PTInfo.VerifyCodeToken}&sign=";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"0\":{", "},\"code\":1", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "\":{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"no\":\"", "\"", 0), this.type);
                    if (str7 != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"number\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if (pCode != "")
                        {
                            if (this.OpenDataOption.Name.Contains("PK10"))
                            {
                                pCode = CommFunc.ConvertPK10CodeByString(pCode);
                            }
                            if (this.CheckCodeLen(pCode))
                            {
                                string pCheckData = str7 + "\t" + pCode;
                                pCheckData = this.ConverCheckData(pCheckData);
                                this.OpenDataOption.GetData++;
                                if (!dictionary2.ContainsKey(pCheckData))
                                {
                                    if (!pAddData.ContainsKey(pCheckData))
                                    {
                                        pAddData[pCheckData] = "";
                                        this.OpenDataOption.GetNewData++;
                                    }
                                }
                                else
                                {
                                    xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                    goto Label_02B7;
                                }
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02B7:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJXYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02AF;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02AF:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJYGJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"shortName={this.PTInfo.GetPTLotteryName(this.type)}&num=20";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"expect\":\"", "\"", 0), this.type);
                    if (str7 != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"openCode\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str7 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_0277;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0277:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJYYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.type == ConfigurationStatus.LotteryType.CAIHNY15C)
                    {
                        pCode = pCode.Substring(0, 5);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EE;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EE:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebJYYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebK3YL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"issues\":", "\"last_number\"", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                string pExpect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\":\"", "\"", 0);
                if ((pExpect != "") && !CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                {
                    pExpect = this.PTInfo.GetAppExpect(this.type, pExpect, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        pExpect = "20" + pExpect;
                    }
                    this.PTInfo.Expect = CommFunc.ConvertExpect(pExpect, this.type);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    str7 = this.PTInfo.GetAppExpect(this.type, str7, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        str7 = "20" + str7;
                    }
                    str7 = CommFunc.ConvertExpect(str7, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"wn_number\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_031A;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_031A:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebK5YL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"content=23%7C1%7C{pID}%7C7%7C&rand={DateTime.Now.ToOADate()}&command=lottery_request_transmit";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = (pResponsetext != "") ? pResponsetext.Substring(pResponsetext.IndexOf("^!^7|") + 5) : "";
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "|||", -1, CompareMethod.Binary))
                {
                    if (str6 != "")
                    {
                        List<string> list = CommFunc.SplitString(str6, "|", -1);
                        string pExpect = list[1];
                        pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                        string pCode = list[2];
                        pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02AD;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02AD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebKSYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"period\":", ",", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str5, "\"result\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("PK10") && !this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        if (this.OpenDataOption.Name.Contains("11选5"))
                        {
                            pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                        }
                        else
                        {
                            pCode = CommFunc.ConvertCode(pCode, this.type);
                        }
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A4;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A4:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebKXYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"lotteryId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CzPeriod\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"CzNum\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_026D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_026D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebKYYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"lotteryId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CzPeriod\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"CzNum\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_026D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_026D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLDYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"lotteryId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CzPeriod\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"CzNum\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_026D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_026D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLF2(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "中奖号码", "第", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext != "") && (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</tr>", -1, CompareMethod.Binary);
                for (int i = 1; i < (strArray.Length - 1); i++)
                {
                    string str5 = strArray[i];
                    string[] strArray2 = Strings.Split(str5, "\r\n", -1, CompareMethod.Binary);
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(strArray2[2], "<td>", "<", 0), this.type);
                    string number = CommFunc.GetNumber(strArray2[4]);
                    if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        number = CommFunc.Convert11X5CodeByString(number);
                    }
                    else if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        number = CommFunc.ConvertPK10CodeByString(number);
                    }
                    if (this.CheckCodeLen(number))
                    {
                        string pCheckData = str6 + "\t" + number;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0276;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0276:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLF21(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/lottery/getopencode";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(AppInfo.Current.Lottery.NextExpect, "");
                string pData = $"id={pID}&issuo={betsExpect}&__RequestVerificationToken=undefined";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"statuscode\":\"2\""))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                string pCode = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID) || (this.type == ConfigurationStatus.LotteryType.DJSSC))
                {
                    pCode = CommFunc.ConvertHGSSCCode(pCode);
                }
                pCode = CommFunc.ConvertCode(pCode, this.type);
                if (this.OpenDataOption.Name.Contains("PK10"))
                {
                    pCode = CommFunc.ConvertPK10CodeByString(pCode);
                }
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string pCheckData = str8 + "\t" + pCode;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebLFGJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"Model=Game&Action=GetHistoryWinCode&Id={pID}&Num={5}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                if ((((pResponsetext == "") || (expression == "")) || (this.PTInfo.CheckBreakConnect(pResponsetext) || this.PTInfo.PTIsBreak)) || (strArray.Length != 5))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, true);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                for (int i = 0; i < strArray.Length; i++)
                {
                    string pStr = strArray[i];
                    string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"W_Issue\":\"", "\"", 0), this.type);
                    if (!this.CheckExpectLen(pExpect))
                    {
                        return false;
                    }
                    string pCode = CommFunc.GetIndexString(pStr, "\"W_Code\":\"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID) || (this.type == ConfigurationStatus.LotteryType.WHDJSSC))
                    {
                        pCode = CommFunc.ConvertHGSSCCode(pCode.Replace(" ", ","));
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.CheckCodeLen(pCode))
                    {
                        string key = pExpect + "\t" + pCode;
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(key))
                        {
                            if (!pAddData.ContainsKey(key))
                            {
                                pAddData[key] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_028E;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_028E:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLFGJ1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(AppInfo.Current.Lottery.NextExpect, "");
                string pData = $"Model=Game&Action=GetLastWinCode&Id={pID}&Issue={betsExpect}&Sleep=10";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("win_issue"))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"win_issue\":\"", "\"", 0), this.type);
                if (!this.CheckExpectLen(pExpect))
                {
                    return false;
                }
                string pCode = CommFunc.GetIndexString(pStr, "\"win_code\":\"", "\"", 0);
                if ((this.type == ConfigurationStatus.LotteryType.HGSSC) || (this.type == ConfigurationStatus.LotteryType.WHDJSSC))
                {
                    pCode = CommFunc.ConvertHGSSCCode(pCode);
                }
                pCode = CommFunc.ConvertCode(pCode, this.type);
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string key = pExpect + "\t" + pCode;
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(key))
                {
                    if (!pAddData.ContainsKey(key))
                    {
                        pAddData[key] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebLFYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "中奖号码", "第", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext != "") && (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</tr>", -1, CompareMethod.Binary);
                for (int i = 1; i < (strArray.Length - 1); i++)
                {
                    string str5 = strArray[i];
                    string[] strArray2 = Strings.Split(str5, "\r\n", -1, CompareMethod.Binary);
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(strArray2[2], "<td>", "<", 0), this.type);
                    string number = CommFunc.GetNumber(strArray2[4]);
                    if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        number = CommFunc.Convert11X5CodeByString(number);
                    }
                    else if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        number = CommFunc.ConvertPK10CodeByString(number);
                    }
                    if (this.CheckCodeLen(number))
                    {
                        string pCheckData = str6 + "\t" + number;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0276;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0276:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLFYL1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/lottery/getopencode";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(AppInfo.Current.Lottery.NextExpect, "");
                string pData = $"id={pID}&issuo={betsExpect}&__RequestVerificationToken=undefined";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"statuscode\":\"2\""))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                string pCode = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID) || (this.type == ConfigurationStatus.LotteryType.DJSSC))
                {
                    pCode = CommFunc.ConvertHGSSCCode(pCode);
                }
                pCode = CommFunc.ConvertCode(pCode, this.type);
                if (this.OpenDataOption.Name.Contains("PK10"))
                {
                    pCode = CommFunc.ConvertPK10CodeByString(pCode);
                }
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string pCheckData = str8 + "\t" + pCode;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebLGZX(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 10, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1800.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0353;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0353:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLGZXVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLMH(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"pageindex=1&Game={this.PTInfo.GetPTLotteryName(this.type)}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"list\":[{", "]} }", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"Index\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"Number\":\"", "\"", 0), this.type);
                    if ((this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛马")) || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0299;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0299:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLSWJS(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0);
                    if (this.type != ConfigurationStatus.LotteryType.LSWJSFFPK10)
                    {
                        pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                    }
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02B5;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02B5:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLUDI(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 10, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1700.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0337;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0337:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLUDIVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02D0;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02D0:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLXYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{\"", "}]", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 10, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{\"", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\":\"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.CheckCodeLen(pCode))
                    {
                        string key = str7 + "\t" + pCode;
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(key))
                        {
                            if (!pAddData.ContainsKey(key))
                            {
                                pAddData[key] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0279;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0279:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebLYS(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"period\":", ",", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str5, "\"result\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("PK10") && !this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        if (this.OpenDataOption.Name.Contains("11选5"))
                        {
                            pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                        }
                        else
                        {
                            pCode = CommFunc.ConvertCode(pCode, this.type);
                        }
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A4;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A4:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebM5CP(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"round\"", "]}]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = 1; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"round\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(pStr, "\"numbers\":[", "]", 0).Replace("\"", "");
                    if (!pCode.Contains("?"))
                    {
                        if (!(this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("快艇")))
                        {
                            pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str6 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_0287;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0287:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebMCYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"period\":", ",", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str5, "\"result\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("PK10") && !this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        if (this.OpenDataOption.Name.Contains("11选5"))
                        {
                            pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                        }
                        else
                        {
                            pCode = CommFunc.ConvertCode(pCode, this.type);
                        }
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A4;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A4:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebMINC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"id={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = strArray.Length - 1; i >= 0; i--)
                {
                    string pStr = strArray[i];
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0250;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0250:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebMINC1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/lottery/get_last_win_code";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(AppInfo.Current.Lottery.NextExpect, "");
                string pData = $"id={pID}&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"issue\":\""))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                if (!this.CheckExpectLen(pExpect))
                {
                    return false;
                }
                string expression = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                if (Strings.Split(expression, ",", -1, CompareMethod.Binary).Length == 20)
                {
                    expression = CommFunc.ConvertHGSSCCode(expression);
                }
                else if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                {
                    expression = CommFunc.ConvertCode(expression, this.type);
                }
                if (!this.CheckCodeLen(expression))
                {
                    return false;
                }
                string pCheckData = pExpect + "\t" + expression;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebMRYL(int pID, int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID, pCount) + "&time=" + DateTime.Now.ToOADate();
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "table-border text-center", "table", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</tr>", -1, CompareMethod.Binary);
                DateTime now = DateTime.Now;
                if ((CommFunc.CheckLotteryIsBD(this.OpenDataOption.ID) && (this.type != ConfigurationStatus.LotteryType.FLBSSC)) && ((now.Hour >= 0) && (now.Hour <= 7)))
                {
                    now = now.AddDays(-1.0);
                }
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.GetIndexString(pStr, "\"50%\">", "&", 0);
                    if (((i == 0) && (now.Hour == 8)) && ((str6 == "1380") || (str6 == "690")))
                    {
                        xReturn = !this.mainThread.SingleMode;
                        goto Label_03E1;
                    }
                    string pExpect = str6;
                    if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pExpect = now.ToString("yyyyMMdd") + "-" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pExpect = pExpect.Split(new char[] { '-' })[1];
                    }
                    switch (str6)
                    {
                        case "0001":
                        case "001":
                            now = now.AddDays(-1.0);
                            break;
                    }
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "<td>", "\n", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_03E1;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_03E1:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebMRYL1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/LotteryService.aspx";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(this.GetNextExpect(), this.OpenDataOption.ID);
                string pData = $"lotteryid={pID}&flag=gethistory&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("'code':["))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "'issue':'", "'", 0), this.type);
                string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "'code':[", "]", 0).Replace("'", ""), this.type);
                if (this.OpenDataOption.Name.Contains("PK10"))
                {
                    pCode = CommFunc.ConvertPK10CodeByString(pCode);
                }
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string pCheckData = str8 + "\t" + pCode;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebMTYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.type == ConfigurationStatus.LotteryType.CAIHNY15C)
                    {
                        pCode = pCode.Substring(0, 5);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EE;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EE:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebMTYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebMXYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.type == ConfigurationStatus.LotteryType.CAIHNY15C)
                    {
                        pCode = pCode.Substring(0, 5);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EE;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EE:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebMXYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebMYGJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"content=23%7C1%7C{pID}%7C7%7C&rand={DateTime.Now.ToOADate()}&command=lottery_request_transmit";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = (pResponsetext != "") ? pResponsetext.Substring(pResponsetext.IndexOf("^!^7|") + 5) : "";
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "|||", -1, CompareMethod.Binary))
                {
                    if (str6 != "")
                    {
                        List<string> list = CommFunc.SplitString(str6, "|", -1);
                        string pExpect = list[1];
                        pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                        string pCode = list[2];
                        pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02C8;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02C8:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebMZC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 10, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1800.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0337;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0337:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebMZCVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebNBAYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"type={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"Data\":[{", "}]}", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"Issue\":\"", "\"", 0);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        pExpect = "20" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"Code\":\"", "\"", 0);
                    if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                    }
                    if (!(this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车")))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02DA;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02DA:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebNBYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "{\n", "\n}", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\" : \"", "\"", 0);
                this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"wn_number\" : \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (pCode != "")
                    {
                        if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02E8;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02E8:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebNewMRYL(int pID, int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID, pCount) + "&time=" + DateTime.Now.ToOADate();
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "<ul id=\"ewinnumber\">", "</ul>", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</li>", -1, CompareMethod.Binary);
                DateTime now = DateTime.Now;
                if ((CommFunc.CheckLotteryIsBD(this.OpenDataOption.ID) && (this.type != ConfigurationStatus.LotteryType.FLBSSC)) && ((now.Hour >= 0) && (now.Hour <= 7)))
                {
                    now = now.AddDays(-1.0);
                }
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.GetIndexString(pStr, "\"float:left;\">", "&", 0);
                    if (((i == 0) && (now.Hour == 8)) && ((str6 == "1380") || (str6 == "690")))
                    {
                        xReturn = !this.mainThread.SingleMode;
                        goto Label_043B;
                    }
                    string pExpect = str6;
                    if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pExpect = now.ToString("yyyyMMdd") + "-" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pExpect = pExpect.Split(new char[] { '-' })[1];
                    }
                    switch (str6)
                    {
                        case "0001":
                        case "001":
                            now = now.AddDays(-1.0);
                            break;
                    }
                    string pCode = CommFunc.GetIndexString(pStr, "\"float:left;\">", "<", pStr.IndexOf("bet-results"));
                    if (this.OpenDataOption.Name.Contains("11选5") || this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.GetIndexString(pStr, "\"float:left;letter-spacing:1px;\">", "<", pStr.IndexOf("bet-results"));
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_043B;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_043B:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebNewMRYL1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/LotteryService.aspx";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(this.GetNextExpect(), this.OpenDataOption.ID);
                string pData = $"lotteryid={pID}&flag=gethistory&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"code\""))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"code\":[", "]", 0).Replace("'", ""), this.type);
                if (this.OpenDataOption.Name.Contains("PK10"))
                {
                    pCode = CommFunc.ConvertPK10CodeByString(pCode);
                }
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string pCheckData = str8 + "\t" + pCode;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebOEYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 10, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1800.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0337;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0337:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebOEYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebQFYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{\"", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{\"", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.ConvertLotteryExpectChar(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\":\"", "\"", 0);
                    if (pCode.Contains("|"))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02F0;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02F0:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebQFZX(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.type == ConfigurationStatus.LotteryType.CAIHNY15C)
                    {
                        pCode = pCode.Substring(0, 5);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_030A;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_030A:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebQFZXVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebQJC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                string pData = $"flag=YLFXBean&id={pID}&num=15";
                HttpHelper.GetResponse1(ref pResponsetext, indexCode, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"reslist\":[", "}]}", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0), this.type);
                    if (this.CheckExpectLen(pExpect))
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"winnumber\":\"", "\"", 0), this.type);
                        if (this.OpenDataOption.Name.Contains("PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_0273;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0273:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebQQT2(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"historyNumbers\":[{", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"currentNumber\":\"", "\"", 0);
                if (this.PTInfo.Expect != "")
                {
                    this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        this.PTInfo.Expect = "20" + this.PTInfo.Expect;
                    }
                    this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"number\":\"", "\"", 0);
                    pExpect = this.PTInfo.GetAppExpect(this.type, pExpect, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        pExpect = "20" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"code\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0350;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0350:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebQQYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02AF;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02AF:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebRDYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"id={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = strArray.Length - 1; i >= 0; i--)
                {
                    string pStr = strArray[i];
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_025D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_025D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebRDYL1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/lottery/get_last_win_code";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(AppInfo.Current.Lottery.NextExpect, "");
                string pData = $"id={pID}&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"issue\":\""))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                if (!this.CheckExpectLen(pExpect))
                {
                    return false;
                }
                string expression = CommFunc.GetIndexString(pStr, "\"code\":\"", "\"", 0);
                if (Strings.Split(expression, ",", -1, CompareMethod.Binary).Length == 20)
                {
                    expression = CommFunc.ConvertHGSSCCode(expression);
                }
                else if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                {
                    expression = CommFunc.ConvertCode(expression, this.type);
                }
                if (!this.CheckCodeLen(expression))
                {
                    return false;
                }
                string pCheckData = pExpect + "\t" + expression;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebRDYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02DD;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02DD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSIJI(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "{", "}}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                string str5 = "";
                if (AppInfo.App == ConfigurationStatus.AppType.OpenData)
                {
                    str5 = "18";
                }
                else
                {
                    if (this.PTInfo.Expect == "")
                    {
                        return false;
                    }
                    str5 = this.PTInfo.Expect.Substring(0, 2);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = strArray.Length - 1; i >= 0; i--)
                {
                    string pStr = strArray[i];
                    string pExpect = CommFunc.GetIndexString(pStr, "\"period\":\"", "\"", 0);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        pExpect = "20" + pExpect;
                    }
                    else if (((this.type == ConfigurationStatus.LotteryType.SIJIHGSSC) || (this.type == ConfigurationStatus.LotteryType.SIJIELSSSC)) && (pExpect.Length == 5))
                    {
                        pExpect = str5 + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(pStr, "\"Number\":\"", "\"", 0);
                    if (this.type != ConfigurationStatus.LotteryType.PK10)
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_033A;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_033A:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSKYYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1700.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0351;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0351:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSKYYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0), this.type);
                    if (((this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车")) || this.OpenDataOption.Name.Contains("游泳")) || this.OpenDataOption.Name.Contains("快艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0330;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0330:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSLTH(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 10, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1800.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0337;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0337:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSLTHVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSSCBy163(string pUrl)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
            int num = 0;
            try
            {
                string str;
                int num4;
                bool flag3;
                goto Label_02E9;
            Label_0035:
                str = DateTime.Now.AddDays((double) (num * -1)).ToString("yyyyMMdd");
                string str2 = string.Format(pUrl, str) + "?time=" + DateTime.Now.ToOADate();
                string pReferer = pUrl;
                string pResponsetext = "";
                HttpHelper.GetResponse3(ref pResponsetext, str2, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
                if (pResponsetext == "")
                {
                    return false;
                }
                int index = pResponsetext.IndexOf("start", 0);
                int num3 = pResponsetext.IndexOf("</table>", index);
                string[] strArray = Strings.Split(pResponsetext.Substring(index, num3 - index), "</tr>", -1, CompareMethod.Binary);
                List<string> list = new List<string>();
                for (num4 = 1; num4 < (strArray.Length - 1); num4++)
                {
                    string[] strArray2 = Strings.Split(strArray[num4], "</td>", -1, CompareMethod.Binary);
                    for (int i = 0; i < 3; i++)
                    {
                        string str6 = strArray2[i * 6];
                        string str7 = str6.Substring(str6.IndexOf("data-period=\"") + 13, 11).Replace("\">", "");
                        if (str7.Substring(0, 2) == "20")
                        {
                            str7 = str7.Substring(2);
                        }
                        int num6 = str6.IndexOf("data-win-number='");
                        if (num6 != -1)
                        {
                            string str8 = str6.Substring(num6 + 0x11, 9).Replace(" ", "");
                            if (str8.Length == 5)
                            {
                                list.Add(str7 + "\t" + str8);
                            }
                        }
                    }
                }
                list.Sort();
                list.Reverse();
                for (num4 = 0; num4 < list.Count; num4++)
                {
                    string key = list[num4];
                    this.OpenDataOption.GetData++;
                    if (!dictionary2.ContainsKey(key))
                    {
                        if (!pAddData.ContainsKey(key))
                        {
                            pAddData[key] = "";
                            this.OpenDataOption.GetNewData++;
                        }
                    }
                    else
                    {
                        xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                        goto Label_02F7;
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                    goto Label_02F7;
                }
                num++;
            Label_02E9:
                flag3 = true;
                goto Label_0035;
            }
            catch
            {
            }
        Label_02F7:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSSCByCaiJing(string pUrl)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
            try
            {
                string str;
                bool flag3;
                int num = 0;
                goto Label_020E;
            Label_0035:
                str = DateTime.Now.AddDays((double) (num * -1)).ToString("yyyyMMdd");
                string str2 = string.Format(pUrl, str, str);
                string pReferer = str2;
                str2 = str2 + "?time=" + DateTime.Now.ToOADate();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, str2, "GET", string.Empty, pReferer, 0x4e20, "UTF-8", true);
                if (pResponsetext == "")
                {
                    return false;
                }
                int index = pResponsetext.IndexOf("id=\"pagedata\"", 0);
                int num3 = pResponsetext.IndexOf("</tbody>", index);
                string[] source = Strings.Split(pResponsetext.Substring(index, num3 - index), "</td>", -1, CompareMethod.Binary);
                string[] strArray2 = Strings.Filter(source, "bg_05", true, CompareMethod.Binary);
                string[] strArray3 = Strings.Filter(source, "bg_13", true, CompareMethod.Binary);
                for (int i = strArray3.Length - 1; i >= 0; i--)
                {
                    string str6 = Strings.Right(strArray2[(i * 3) + 1], 9);
                    string str7 = Strings.Right(strArray3[i], 5);
                    string key = str6 + "\t" + str7;
                    this.OpenDataOption.GetData++;
                    if (!dictionary2.ContainsKey(key))
                    {
                        if (!pAddData.ContainsKey(key))
                        {
                            pAddData[key] = "";
                            this.OpenDataOption.GetNewData++;
                        }
                    }
                    else
                    {
                        xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                        goto Label_021C;
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                    goto Label_021C;
                }
                num++;
            Label_020E:
                flag3 = true;
                goto Label_0035;
            }
            catch
            {
            }
        Label_021C:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSSCByCaiLeLe(string pUrl)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            try
            {
                string pReferer = pUrl;
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, pReferer, 0x2710, "UTF-8", false);
                if (pResponsetext == "")
                {
                    return false;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(pResponsetext);
                XmlNodeList list = document.SelectNodes("xml/row");
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                for (int i = 0; i < list.Count; i++)
                {
                    XmlNode node = list[i];
                    string str3 = node.Attributes["expect"].Value;
                    if (str3.Substring(0, 2) == "20")
                    {
                        str3 = str3.Substring(2);
                    }
                    DateTime time = Convert.ToDateTime(node.Attributes["opentime"].Value);
                    string str4 = Strings.Replace(node.Attributes["opencode"].Value, ",", "", 1, -1, CompareMethod.Binary).TrimEnd(new char[0]);
                    if (str4.Length == 5)
                    {
                        string key = str3 + "\t" + str4;
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(key))
                        {
                            if (!pAddData.ContainsKey(key))
                            {
                                pAddData[key] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0205;
                        }
                        if (this.CheckExpect(pAddData, ref xReturn))
                        {
                            goto Label_0205;
                        }
                    }
                }
            }
            catch
            {
            }
        Label_0205:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSSCByTJFlCPW(string pUrl)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
            try
            {
                string str;
                bool flag3;
                int num = 0;
                goto Label_0237;
            Label_0035:
                str = pUrl;
                string str2 = pUrl;
                string pResponsetext = "";
                string str4 = DateTime.Now.AddDays((double) (num * -1)).ToString("yyyy-MM-dd");
                int num2 = this.mainThread.SingleMode ? 13 : 0x54;
                string pData = $"currentPage=1&pageSize={num2}&playType=4&startTime={str4} 00:00:00&endTime={str4} 23:59:59";
                HttpHelper.GetResponse(ref pResponsetext, str2, "POST", pData, str, 0x2710, "UTF-8", true);
                if (pResponsetext == "")
                {
                    return false;
                }
                string[] source = Strings.Split(pResponsetext, ",", -1, CompareMethod.Binary);
                string[] strArray2 = Strings.Filter(source, "TermCode", true, CompareMethod.Binary);
                string[] strArray3 = Strings.Filter(source, "BasicCode", true, CompareMethod.Binary);
                for (int i = 0; i < strArray2.Length; i++)
                {
                    string number = CommFunc.GetNumber(strArray2[i]);
                    if (number.Substring(0, 2) == "20")
                    {
                        number = number.Substring(2);
                    }
                    string[] strArray4 = Strings.Split(strArray3[i], "u003e", -1, CompareMethod.Binary);
                    string str7 = "";
                    for (int j = 1; j <= 5; j++)
                    {
                        str7 = str7 + strArray4[j * 2].Substring(0, 1);
                    }
                    string key = number + "\t" + str7;
                    this.OpenDataOption.GetData++;
                    if (!dictionary2.ContainsKey(key))
                    {
                        if (!pAddData.ContainsKey(key))
                        {
                            pAddData[key] = "";
                            this.OpenDataOption.GetNewData++;
                        }
                    }
                    else
                    {
                        xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                        goto Label_0245;
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                    goto Label_0245;
                }
                num++;
            Label_0237:
                flag3 = true;
                goto Label_0035;
            }
            catch
            {
            }
        Label_0245:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSSCByXJFLCP(string pUrl)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
            DateTime now = DateTime.Now;
            try
            {
                string str;
                bool flag3;
                goto Label_01E7;
            Label_0039:
                str = "";
                string pReferer = "";
                string str3 = pUrl;
                string pData = $"selectDate={now.ToString("yyyyMMdd")}";
                HttpHelper.GetResponse(ref str, str3, "POST", pData, pReferer, 0x2710, "UTF-8", true);
                if (str == "")
                {
                    return false;
                }
                now = now.AddDays(-1.0);
                string[] strArray = Strings.Split(str, "\"createTime\"", -1, CompareMethod.Binary);
                for (int i = 1; i < strArray.Length; i++)
                {
                    string expression = strArray[i];
                    string[] strArray2 = Strings.Split(expression, ",\"", -1, CompareMethod.Binary);
                    string str6 = CommFunc.GetIndexString(strArray2[13], "\":\"", "\"", 0).Substring(2);
                    string str7 = CommFunc.GetIndexString(strArray2[14], "\":\"", "\"", 0).Replace(",", "");
                    if (str7.Length == 5)
                    {
                        string key = str6 + "\t" + str7;
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(key))
                        {
                            if (!pAddData.ContainsKey(key))
                            {
                                pAddData[key] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_01F5;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                    goto Label_01F5;
                }
            Label_01E7:
                flag3 = true;
                goto Label_0039;
            }
            catch
            {
            }
        Label_01F5:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSSHC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1850.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("快艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_036D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_036D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSSHCVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("快艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0307;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0307:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebSYYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"resultdata\":[{", "],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"officialissue\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"resultdata\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0284;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0284:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebTAYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"lotteryId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CzPeriod\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"CzNum\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_026D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_026D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebTBYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                string pData = $"type={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                pResponsetext = this.PTInfo.DecryptString(pResponsetext);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"Data\":[{", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"Issue\":", ",", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"Codes\":\"", "\"", 0);
                    if (!(this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车")))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02D0;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02D0:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebTBYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02DD;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02DD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebTCYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 10, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1800.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0353;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0353:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebTCYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebTHDYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"shortName={this.PTInfo.GetPTLotteryName(this.type)}&num=20";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"expect\":\"", "\"", 0), this.type);
                    if (str7 != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"openCode\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str7 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_0281;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0281:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebTHEN(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{\"", "}]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{\"", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\":\"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02CE;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02CE:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebTHYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"(lotteryCode:{this.PTInfo.GetPTLotteryName(this.type)},pageNum:1,pageSize:8,token:{this.PTInfo.Token})".Replace("(", "{").Replace(")", "}");
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{", "}]}", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"drawPeriod\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"drawNumber\":\"", "\"", 0), this.type);
                    if ((this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车")) || (this.OpenDataOption.ID == "THOZPK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0304;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0304:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebTIYU(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"lssue\":\"", "\"", 0), this.type);
                    if (str6 != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"lottery_numbers\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str6 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_025D;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_025D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebTRYL(int pID, int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID, pCount) + "&time=" + DateTime.Now.ToOADate();
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "ewinnumber", "</tbody>", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</tr>", -1, CompareMethod.Binary);
                DateTime now = DateTime.Now;
                if ((CommFunc.CheckLotteryIsBD(this.OpenDataOption.ID) && (this.type != ConfigurationStatus.LotteryType.FLBSSC)) && ((now.Hour >= 0) && (now.Hour <= 7)))
                {
                    now = now.AddDays(-1.0);
                }
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.GetIndexString(pStr, "<td>", "&", 0);
                    if (((i == 0) && (now.Hour == 8)) && ((str6 == "1380") || (str6 == "690")))
                    {
                        xReturn = !this.mainThread.SingleMode;
                        goto Label_040A;
                    }
                    string pExpect = str6;
                    if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pExpect = now.ToString("yyyyMMdd") + "-" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pExpect = pExpect.Split(new char[] { '-' })[1];
                    }
                    switch (str6)
                    {
                        case "0001":
                        case "001":
                            now = now.AddDays(-1.0);
                            break;
                    }
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"color01\">", "<", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_040A;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_040A:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebTRYL1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/LotteryService.aspx";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(this.GetNextExpect(), this.OpenDataOption.ID);
                string pData = $"lotteryid={pID}&flag=gethistory&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"code\":["))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"code\":[", "]", 0).Replace("\",", "").Replace("\"", ""), this.type);
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string key = str8 + "\t" + pCode;
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(key))
                {
                    if (!pAddData.ContainsKey(key))
                    {
                        pAddData[key] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebTYYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{\"", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{\"", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.ConvertLotteryExpectChar(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"code\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02B3;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02B3:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebUCYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"historynumtable\"", "</table>", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</tr>", -1, CompareMethod.Binary);
                for (int i = 0; i < 10; i++)
                {
                    string str5 = strArray[i];
                    string[] strArray2 = Strings.Split(str5, "</td>", -1, CompareMethod.Binary);
                    string[] strArray3 = Strings.Split(strArray2[0], "\r\n", -1, CompareMethod.Binary);
                    string pExpect = strArray3[strArray3.Length - 1];
                    pExpect = pExpect.Substring(pExpect.IndexOf("width=\"25%\">") + 12);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pExpect = pExpect.Substring(pExpect.IndexOf("width=\"5%\">") + 11);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pExpect = pExpect.Substring(pExpect.IndexOf("width=\"21%\">") + 12);
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    if (!this.CheckExpectLen(pExpect))
                    {
                        return false;
                    }
                    string pCode = strArray2[1];
                    pCode = CommFunc.ConvertCode(CommFunc.GetNumber(pCode.Substring(pCode.IndexOf("<span>"))), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0336;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0336:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebUT8(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"shortName={this.PTInfo.GetPTLotteryName(this.type)}&num=20";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"expect\":\"", "\"", 0), this.type);
                    if (str7 != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"openCode\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str7 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_0292;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0292:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWBJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"historyNumbers\":[{", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"currentNumber\":\"", "\"", 0);
                if (this.PTInfo.Expect != "")
                {
                    this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        this.PTInfo.Expect = "20" + this.PTInfo.Expect;
                    }
                    this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"number\":\"", "\"", 0);
                    pExpect = this.PTInfo.GetAppExpect(this.type, pExpect, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        pExpect = "20" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"code\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0350;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0350:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWCAI(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.type == ConfigurationStatus.LotteryType.CAIHNY15C)
                    {
                        pCode = pCode.Substring(0, 5);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EE;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EE:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWCAIVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWCYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"openCodes\":", "}]}}", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"issueNO\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                string str5 = DateTime.Now.ToString("yyyy");
                for (int i = 0; i < strArray.Length; i++)
                {
                    string pStr = strArray[i];
                    string pExpect = CommFunc.GetIndexString(pStr, "\"issueNO\":\"", "\"", 0);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        pExpect = str5 + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    if (pExpect != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"openCode\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02E0;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02E0:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWDYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                string pData = $"(data:{pID})".Replace("(", "{").Replace(")", "}");
                HttpHelper.GetResponse1(ref pResponsetext, indexCode, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"data\":[{", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"n\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"o\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02AA;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02AA:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWDYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02DD;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02DD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWEYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                string pData = $"flag=YLFXBean&id={pID}&num=15";
                HttpHelper.GetResponse1(ref pResponsetext, indexCode, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"reslist\":[", "}]}", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0), this.type);
                    if (this.CheckExpectLen(pExpect))
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"winnumber\":\"", "\"", 0), this.type);
                        if (this.OpenDataOption.Name.Contains("PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_0273;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0273:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWHC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1800.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("快艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_036D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_036D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWHCVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("快艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0307;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0307:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWHEN(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{\"", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{\"", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.ConvertLotteryExpectChar(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"code\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02B3;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02B3:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWJSJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"Model=Game&Action=GetHistoryWinCode&Id={pID}&Num={5}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                if ((((pResponsetext == "") || (expression == "")) || (this.PTInfo.CheckBreakConnect(pResponsetext) || this.PTInfo.PTIsBreak)) || (strArray.Length != 5))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, true);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                for (int i = 0; i < strArray.Length; i++)
                {
                    string pStr = strArray[i];
                    string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"W_Issue\":\"", "\"", 0), this.type);
                    if (!this.CheckExpectLen(pExpect))
                    {
                        return false;
                    }
                    string pCode = CommFunc.GetIndexString(pStr, "\"W_Code\":\"", "\"", 0);
                    if (((this.type == ConfigurationStatus.LotteryType.HGSSC) || (this.type == ConfigurationStatus.LotteryType.WHDJSSC)) || (this.type == ConfigurationStatus.LotteryType.JH15C))
                    {
                        pCode = CommFunc.ConvertHGSSCCode(pCode);
                    }
                    if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A6;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A6:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWJSJ1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(AppInfo.Current.Lottery.NextExpect, "");
                string pData = $"Model=Game&Action=GetLastWinCode&Id={pID}&Issue={betsExpect}&Sleep=10";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("win_issue"))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"win_issue\":\"", "\"", 0), this.type);
                if (!this.CheckExpectLen(pExpect))
                {
                    return false;
                }
                string pCode = CommFunc.GetIndexString(pStr, "\"win_code\":\"", "\"", 0);
                if (((this.type == ConfigurationStatus.LotteryType.HGSSC) || (this.type == ConfigurationStatus.LotteryType.WHDJSSC)) || (this.type == ConfigurationStatus.LotteryType.JH15C))
                {
                    pCode = CommFunc.ConvertHGSSCCode(pCode);
                }
                if (!this.OpenDataOption.Name.Contains("北京赛车PK10"))
                {
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                }
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string pCheckData = pExpect + "\t" + pCode;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebWMYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"action=get_lottery_open&lottery_code={this.PTInfo.GetBetsLotteryID(this.type)}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\\\"issue_no\\\":\\\"", "\\\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\\\"lotteryopen_no\\\":\\\"", "\\\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_025D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_025D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWSYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{\"", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{\"", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.ConvertLotteryExpectChar(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"code\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02B3;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02B3:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWTYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"expect\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"code\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_025A;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_025A:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWXYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                string pData = $"token={this.PTInfo.Token}&pageSize={30}&ticketId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"openedList\":[{\"", "}]}}", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "\"},{\"", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "ticketPlanId\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ticketOpenNum\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0268;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0268:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWYYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"lotteryGameId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":[", "]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                if (this.PTInfo.Prize == "")
                {
                    string str6 = CommFunc.GetIndexString(pResponsetext, "\"UserReturnPoint\":", ",", 0);
                    this.PTInfo.Prize = (1700.0 + ((Convert.ToDouble(str6) * 2.0) * 10.0)).ToString();
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("快艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_036D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_036D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWYYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("快艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0307;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0307:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWZYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "{\n", "\n}", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\" : \"", "\"", 0);
                this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"wn_number\" : \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (pCode != "")
                    {
                        if (this.OpenDataOption.Name.Contains("赛车"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02E8;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02E8:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebWZYLZB(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.ZBWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                string zBString = this.PTInfo.ZBString;
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", zBString, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"list\":[{", "}]}}", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"RoundFullStr\":\"", "\"", 0), this.type);
                    string[] strArray2 = Strings.Filter(Strings.Split(pStr, ",", -1, CompareMethod.Binary), "Ball_", true, CompareMethod.Binary);
                    List<string> pList = new List<string>();
                    foreach (string str8 in strArray2)
                    {
                        pList.Add(CommFunc.GetIndexString(str8, "\":", "", 0));
                    }
                    string pCode = CommFunc.ConvertCode(CommFunc.Join(pList), this.type);
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02BF;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02BF:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXB3(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse1(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[", "]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "\r\n  {\r\n", -1, CompareMethod.Binary);
                for (int i = 1; i < strArray.Length; i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"IssueNo\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"LotteryOpenNo\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0268;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0268:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXCAI(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "<div class=\"recentCon re\">", "</div>", 0);
                string pExpect = CommFunc.GetIndexString(pResponsetext, "issue:'", "'", 0);
                if (this.type == ConfigurationStatus.LotteryType.XJSSC)
                {
                    pExpect = pExpect.Replace("-", "-0");
                }
                pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                this.PTInfo.Expect = pExpect;
                if (this.PTInfo.PrizeDic.Count == 0)
                {
                    this.PTInfo.CountPrizeDic(pResponsetext);
                    string pResponseText = this.PTInfo.LoginLotteryWeb(this.type, "");
                    this.PTInfo.CountPrizeDic(pResponseText);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</li>", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str8 = CommFunc.GetIndexString(pStr, "\"issue\">", "期", 0);
                    if (this.type == ConfigurationStatus.LotteryType.XJSSC)
                    {
                        str8 = str8.Replace("-", "-0");
                    }
                    str8 = CommFunc.ConvertExpect(str8, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"num\">", "<", 0), this.type);
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str8 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0296;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0296:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXCYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"lotteryId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CzPeriod\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"CzNum\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_026D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_026D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXDB(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"issues\":", "\"last_number\"", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                string pExpect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\":\"", "\"", 0);
                if (((pExpect != "") && !CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID)) && (this.type != ConfigurationStatus.LotteryType.XDBDJSSC))
                {
                    pExpect = this.PTInfo.GetAppExpect(this.type, pExpect, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        pExpect = "20" + pExpect;
                    }
                    this.PTInfo.Expect = CommFunc.ConvertExpect(pExpect, this.type);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    str7 = this.PTInfo.GetAppExpect(this.type, str7, false);
                    if (!(CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID) || (this.type == ConfigurationStatus.LotteryType.XDBDJSSC)))
                    {
                        str7 = "20" + str7;
                    }
                    str7 = CommFunc.ConvertExpect(str7, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"wn_number\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0357;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0357:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXGLL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"content\":[{", "}}]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"numero\":\"", "\"", 0), this.type);
                    if (str6 != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"winNo\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if ((this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("飞艇")) || this.OpenDataOption.Name.Contains("赛车"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str6 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02A0;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A0:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXHDF(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0295;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0295:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXHHC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"period\":", ",", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str5, "\"result\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("PK10") && !this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        if (this.OpenDataOption.Name.Contains("11选5"))
                        {
                            pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                        }
                        else
                        {
                            pCode = CommFunc.ConvertCode(pCode, this.type);
                        }
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A4;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A4:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXHSD(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02AF;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02AF:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXINC(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str5, "\"period\":", ",", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str5, "\"result\":\"", "\"", 0);
                    if (!this.OpenDataOption.Name.Contains("PK10") && !this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        if (this.OpenDataOption.Name.Contains("11选5"))
                        {
                            pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                        }
                        else
                        {
                            pCode = CommFunc.ConvertCode(pCode, this.type);
                        }
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02A4;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02A4:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXQYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[{\"", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{\"", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.ConvertLotteryExpectChar(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\":\"", "\"", 0);
                    if (pCode.Contains("|"))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if ((this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车")) || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0307;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0307:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXTYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.type == ConfigurationStatus.LotteryType.CAIHNY15C)
                    {
                        pCode = pCode.Substring(0, 5);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EE;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EE:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXTYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXWYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.type == ConfigurationStatus.LotteryType.CAIHNY15C)
                    {
                        pCode = pCode.Substring(0, 5);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_030A;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_030A:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebXWYLVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYBAO(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x4e20, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "<tr class=\"ylfx_content\">", "text-align:center;background", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "</tr>", -1, CompareMethod.Binary);
                for (int i = strArray.Length - 2; i >= 0; i--)
                {
                    string pStr = strArray[i];
                    string pExpect = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "<b>", "</b>", 0), this.type);
                    if (this.CheckExpectLen(pExpect))
                    {
                        string[] strArray2 = Strings.Filter(Strings.Split(pStr, "</td>", -1, CompareMethod.Binary), "ylfx_kjhm", true, CompareMethod.Binary);
                        List<string> pList = new List<string>();
                        for (int j = 0; j < strArray2.Length; j++)
                        {
                            string str7 = strArray2[j];
                            pList.Add(CommFunc.GetIndexString(str7, "\"ball_color3\">", "<", 0).Replace("\r", "").Replace("\n", "").Trim());
                        }
                        string pCode = CommFunc.Join(pList);
                        if (this.OpenDataOption.Name.Contains("11选5"))
                        {
                            pCode = CommFunc.Join(pList, ",");
                        }
                        else
                        {
                            pCode = CommFunc.ConvertCode(pCode, CommFunc.GetLotteryName(this.type));
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string key = pExpect + "\t" + pCode;
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(key))
                            {
                                if (!pAddData.ContainsKey(key))
                                {
                                    pAddData[key] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02E2;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02E2:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYBAO1(int pID)
        {
            bool pStatus = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = this.PTInfo.GetLine() + "/LotteryService.aspx";
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string betsExpect = this.PTInfo.GetBetsExpect(this.GetNextExpect(), this.OpenDataOption.ID);
                string pData = $"lotteryid={pID}&flag=gethistory&issue={betsExpect}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string str6 = pResponsetext;
                if (!str6.Contains("\"code\""))
                {
                    return pStatus;
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string pStr = str6;
                string str8 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"issue\":\"", "\"", 0), this.type);
                string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(pStr, "\"code\":[", "]", 0).Replace("\"", "").Replace("\",", ""), this.type);
                if (this.OpenDataOption.Name.Contains("11选5"))
                {
                    pCode = CommFunc.Convert11X5CodeByString(pCode.Replace(",", ""));
                }
                else if (this.OpenDataOption.Name.Contains("PK10"))
                {
                    pCode = CommFunc.ConvertPK10CodeByString(pCode);
                }
                if (!this.CheckCodeLen(pCode))
                {
                    return false;
                }
                string pCheckData = str8 + "\t" + pCode;
                pCheckData = this.ConverCheckData(pCheckData);
                this.OpenDataOption.GetData++;
                if (!dictionary2.ContainsKey(pCheckData))
                {
                    if (!pAddData.ContainsKey(pCheckData))
                    {
                        pAddData[pCheckData] = "";
                        this.OpenDataOption.GetNewData++;
                    }
                }
                else
                {
                    pStatus = !this.mainThread.SingleMode || (pAddData.Count > 0);
                }
            }
            catch
            {
            }
            pStatus = pAddData.Count > 0;
            this.CommonLate(pStatus, pAddData);
            return pStatus;
        }

        private bool GetWebYBAOByPK10(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"id={pID}&num=5&flag=openrecord";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"openrecord\":[", "}]}", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"issue\":\"", "期", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"winnumber\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0267;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0267:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYBYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"shortName={this.PTInfo.GetPTLotteryName(this.type)}&num=20";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"expect\":\"", "\"", 0), this.type);
                    if (str7 != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"openCode\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str7 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_0281;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0281:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYCYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"command=lottery_request_transmit_v2&param=%7B%22lottery_id%22%3A%22{pID}%22%2C%22count%22%3A{"7"}%2C%22rand_key%22%3A{DateTime.Now.ToOADate()}%2C%22command_id%22%3A23%7D";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"LIST\": [{", "}],", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CP_QS\": \"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"ZJHM\": \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02AF;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02AF:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYDYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"shortName={this.PTInfo.GetPTLotteryName(this.type)}&num=10";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"winnumber\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_025D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_025D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYHSG(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}&history=true";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "[ {", "} ]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\" : \"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.type == ConfigurationStatus.LotteryType.CAIHNY15C)
                    {
                        pCode = pCode.Substring(0, 5);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_030A;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_030A:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYHSGVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02EB;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02EB:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYHYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"shortName={this.PTInfo.GetPTLotteryName(this.type)}&num=20";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"expect\":\"", "\"", 0), this.type);
                    if (str7 != "")
                    {
                        string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"openCode\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                        if (this.OpenDataOption.Name.Contains("PK10"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = str7 + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_0281;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0281:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYINH(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "<td class=\"issue\">", "<tr class=tb>", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "<td class=\"issue\">", -1, CompareMethod.Binary);
                string str5 = DateTime.Now.ToString("yyyy");
                for (int i = 0; i < strArray.Length; i++)
                {
                    string str6 = strArray[i];
                    string str7 = CommFunc.ConvertExpect(str5 + Strings.Split(str6, "</td>", -1, CompareMethod.Binary)[0], this.type);
                    string[] strArray2 = Strings.Filter(Strings.Split(str6, "</td>", -1, CompareMethod.Binary), "<td align=\"center\" width=\"28\">", true, CompareMethod.Binary);
                    List<string> pList = new List<string>();
                    for (int j = 0; j < strArray2.Length; j++)
                    {
                        string pStr = strArray2[j];
                        pList.Add(CommFunc.GetIndexString(pStr, "<div class=\"wth\">", "<", 0));
                    }
                    string pCode = CommFunc.ConvertCode(CommFunc.Join(pList), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Join(pList, ",");
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02C6;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02C6:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYL2028(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                string pData = $"name={this.PTInfo.GetPTLotteryName(this.type)}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"openList\":[", "]", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"expect\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"expect\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"code\":\"", "\"", 0);
                    if (CommFunc.CheckIsKLCLottery(this.OpenDataOption.ID))
                    {
                        pCode = pCode.Split(new char[] { '|' })[0];
                    }
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("飞艇"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02F5;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02F5:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYRYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"Game={this.PTInfo.GetPTLotteryName(this.type)}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"info\" : [{", "]}] }", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"Index\":\"", "\"", 0).Trim();
                    if (this.type == ConfigurationStatus.LotteryType.YRHG15C)
                    {
                        pExpect = "20" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"Number\":[", "]", 0).Replace("\"", ""), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        List<string> list = CommFunc.SplitString(pCode, ",", -1);
                        List<string> pList = new List<string>();
                        foreach (string str9 in list)
                        {
                            pList.Add(CommFunc.Convert11X5Code(str9));
                        }
                        pCode = CommFunc.Join(pList, ",");
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_033D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_033D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYSEN(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"historyNumbers\":[{", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"currentNumber\":\"", "\"", 0);
                if (this.PTInfo.Expect != "")
                {
                    this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        this.PTInfo.Expect = "20" + this.PTInfo.Expect;
                    }
                    this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"number\":\"", "\"", 0);
                    pExpect = this.PTInfo.GetAppExpect(this.type, pExpect, false);
                    if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                    {
                        pExpect = "20" + pExpect;
                    }
                    pExpect = CommFunc.ConvertExpect(pExpect, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"code\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0350;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0350:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYXZX(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                string pData = $"type={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, indexLine, 0x2710, "UTF-8", true);
                pResponsetext = this.PTInfo.DecryptString(pResponsetext);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"Data\":[{", "}]", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str6, "\"Issue\":", ",", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.GetIndexString(str6, "\"Codes\":\"", "\"", 0);
                    if (!(this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车")))
                    {
                        pCode = CommFunc.ConvertCode(pCode, this.type);
                    }
                    if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Convert11X5CodeByString(pCode, ",");
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02D0;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02D0:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYXZXVR(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.VRWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"OldIssues\":", "}]", 0);
                if ((((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext)) || this.PTInfo.PTIsBreak)
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "GameSerialNumber\":\"", "\"", 0);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("__RequestVerificationToken"));
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str6 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "GameSerialNumber\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetNumber(CommFunc.GetIndexString(pStr, "GameWinningNumber\":\"", "\"", 0)), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str6 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02DD;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02DD:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYYZX(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"shortName={this.PTInfo.GetPTLotteryName(this.type)}&num=20";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"expect\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"openCode\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_025D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_025D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYZCP(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "{\n", "\n}", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\" : \"", "\"", 0);
                this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"wn_number\" : \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (pCode != "")
                    {
                        if (this.OpenDataOption.Name.Contains("赛车"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_02E8;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02E8:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebYZCPZB(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                if (!this.PTInfo.ZBWebLoginMain(this.type))
                {
                    return false;
                }
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string indexLine = this.PTInfo.GetIndexLine();
                string zBString = this.PTInfo.ZBString;
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", zBString, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"list\":[{", "}]}}", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string[] strArray = Strings.Split(expression, "},{", -1, CompareMethod.Binary);
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    string pStr = strArray[i];
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(pStr, "\"RoundFullStr\":\"", "\"", 0), this.type);
                    string[] strArray2 = Strings.Filter(Strings.Split(pStr, ",", -1, CompareMethod.Binary), "Ball_", true, CompareMethod.Binary);
                    List<string> pList = new List<string>();
                    foreach (string str8 in strArray2)
                    {
                        pList.Add(CommFunc.GetIndexString(str8, "\":", "", 0));
                    }
                    string pCode = CommFunc.ConvertCode(CommFunc.Join(pList), this.type);
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_02BF;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_02BF:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebZBEI(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string pUrl = string.Format(this.PTInfo.GetIndexCode(this.type), pID);
                string indexLine = this.PTInfo.GetIndexLine();
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "\"issues\":", "\"last_number\"", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 3, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                if (!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID))
                {
                    string pExpect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\":\"", "\"", 0);
                    if (((!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID) && (this.type != ConfigurationStatus.LotteryType.ZBEIDJSSC)) && (this.type != ConfigurationStatus.LotteryType.AH11X5)) && (this.type != ConfigurationStatus.LotteryType.FJ11X5))
                    {
                        pExpect = "20" + pExpect;
                    }
                    this.PTInfo.Expect = CommFunc.ConvertExpect(pExpect, this.type);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.GetIndexString(str6, "\"issue\":\"", "\"", 0);
                    if (((!CommFunc.CheckIsSkipLottery(this.OpenDataOption.ID) && (this.type != ConfigurationStatus.LotteryType.ZBEIDJSSC)) && (this.type != ConfigurationStatus.LotteryType.AH11X5)) && (this.type != ConfigurationStatus.LotteryType.FJ11X5))
                    {
                        str7 = "20" + str7;
                    }
                    str7 = CommFunc.ConvertExpect(str7, this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"wn_number\":\"", "\"", 0), this.type);
                    if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0347;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0347:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebZBYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "<div class=\"row five-ball-open five-ball-pc\">", "<script>", 0);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, 10, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "cycle_value = '", "'", 0);
                this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                this.PTInfo.ExpectID = CommFunc.GetIndexString(pResponsetext, "game_cycle_id = '", "'", 0);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "<div class=\"row five-ball-open five-ball-pc\">", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\">", "<", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string[] strArray2 = Strings.Filter(Strings.Split(str5, "</span>", -1, CompareMethod.Binary), "ball-base-five ball03", true, CompareMethod.Binary);
                    List<string> pList = new List<string>();
                    foreach (string str7 in strArray2)
                    {
                        pList.Add(Strings.Split(str7, "\"ball-base-five ball03\">", -1, CompareMethod.Binary)[1]);
                    }
                    string pCode = CommFunc.ConvertCode(CommFunc.Join(pList), this.type);
                    if (this.OpenDataOption.Name.Contains("北京赛车PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    else if (this.OpenDataOption.Name.Contains("11选5"))
                    {
                        pCode = CommFunc.Join(pList, ",");
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_036F;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_036F:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebZDYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "{\n", "\n}", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\" : \"", "\"", 0);
                this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"wn_number\" : \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (pCode != "")
                    {
                        if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_031F;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_031F:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebZLJ(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, false);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "<textarea id=\"J-textarea-historys-balls-data\" style=\"display:none;\">", "<", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"currentNumber\":\"", "\"", 0);
                if (this.PTInfo.Expect != "")
                {
                    this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                    this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                }
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                string delimiter = ",";
                if (this.OpenDataOption.Name.Contains("PK10"))
                {
                    delimiter = "|";
                }
                string[] strArray = Strings.Split(expression, delimiter, -1, CompareMethod.Binary);
                for (int i = 0; i < strArray.Length; i++)
                {
                    string[] strArray2 = strArray[i].Replace("\r\n", "").Replace("\t", "").Split(new char[] { '=' });
                    string pExpect = strArray2[0];
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = strArray2[1];
                    pCode = CommFunc.ConvertCode(pCode, this.type);
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = pExpect + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_0337;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_0337:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebZXYL(int pID)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                string pData = $"lotteryId={pID}";
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "POST", pData, lotteryLine, 0x2710, "UTF-8", true);
                string expression = pResponsetext;
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str6 in Strings.Split(expression, "},{", -1, CompareMethod.Binary))
                {
                    string str7 = CommFunc.ConvertExpect(CommFunc.GetIndexString(str6, "\"CzPeriod\":\"", "\"", 0), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str6, "\"CzNum\":\"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (this.OpenDataOption.Name.Contains("PK10"))
                    {
                        pCode = CommFunc.ConvertPK10CodeByString(pCode);
                    }
                    if (this.CheckCodeLen(pCode))
                    {
                        string pCheckData = str7 + "\t" + pCode;
                        pCheckData = this.ConverCheckData(pCheckData);
                        this.OpenDataOption.GetData++;
                        if (!dictionary2.ContainsKey(pCheckData))
                        {
                            if (!pAddData.ContainsKey(pCheckData))
                            {
                                pAddData[pCheckData] = "";
                                this.OpenDataOption.GetNewData++;
                            }
                        }
                        else
                        {
                            xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                            goto Label_026D;
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_026D:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool GetWebZYL(int pCount)
        {
            bool xReturn = false;
            this.OpenDataOption.GetNewData = 0;
            this.OpenDataOption.GetData = 0;
            Dictionary<string, string> pAddData = new Dictionary<string, string>();
            string pResponsetext = "";
            try
            {
                string indexCode = this.PTInfo.GetIndexCode(this.type);
                string lotteryLine = this.PTInfo.GetLotteryLine(this.type, true);
                HttpHelper.GetResponse(ref pResponsetext, indexCode, "GET", string.Empty, lotteryLine, 0x2710, "UTF-8", true);
                string expression = CommFunc.GetIndexString(pResponsetext, "{\n", "\n}", 0);
                int dXLoginCount = this.GetDXLoginCount(3);
                if (((pResponsetext == "") || (expression == "")) || this.PTInfo.CheckBreakConnect(pResponsetext))
                {
                    if (this.IsNeedLogin())
                    {
                        this.PTInfo.AgainLoginMain(this.type, pResponsetext, dXLoginCount, false);
                    }
                    return false;
                }
                this.PTInfo.PTDXCount = 0;
                this.PTInfo.Expect = CommFunc.GetIndexString(pResponsetext, "\"current_issue\" : \"", "\"", 0);
                this.PTInfo.Expect = this.PTInfo.GetAppExpect(this.type, this.PTInfo.Expect, false);
                this.PTInfo.Expect = CommFunc.ConvertExpect(this.PTInfo.Expect, this.type);
                Dictionary<string, string> dictionary2 = CommFunc.ConvertListToDic(this.DataList);
                foreach (string str5 in Strings.Split(expression, "}, {", -1, CompareMethod.Binary))
                {
                    string pExpect = CommFunc.GetIndexString(str5, "\"issue\" : \"", "\"", 0);
                    pExpect = CommFunc.ConvertExpect(this.PTInfo.GetAppExpect(this.type, pExpect, false), this.type);
                    string pCode = CommFunc.ConvertCode(CommFunc.GetIndexString(str5, "\"wn_number\" : \"", "\"", 0), CommFunc.GetLotteryName(this.type));
                    if (pCode != "")
                    {
                        if (this.OpenDataOption.Name.Contains("PK10") || this.OpenDataOption.Name.Contains("赛车"))
                        {
                            pCode = CommFunc.ConvertPK10CodeByString(pCode);
                        }
                        if (this.CheckCodeLen(pCode))
                        {
                            string pCheckData = pExpect + "\t" + pCode;
                            pCheckData = this.ConverCheckData(pCheckData);
                            this.OpenDataOption.GetData++;
                            if (!dictionary2.ContainsKey(pCheckData))
                            {
                                if (!pAddData.ContainsKey(pCheckData))
                                {
                                    pAddData[pCheckData] = "";
                                    this.OpenDataOption.GetNewData++;
                                }
                            }
                            else
                            {
                                xReturn = !this.mainThread.SingleMode || (pAddData.Count > 0);
                                goto Label_031F;
                            }
                        }
                    }
                }
                if (this.CheckExpect(pAddData, ref xReturn))
                {
                }
            }
            catch
            {
            }
        Label_031F:
            this.CommonLate(xReturn, pAddData);
            return xReturn;
        }

        private bool IsNeedLogin()
        {
            bool flag = false;
            if ((AppInfo.App != ConfigurationStatus.AppType.OpenData) || (AppInfo.OpenDataLoginLotteryDic[this.PTInfo.PTName] == this.OpenDataOption.ID))
            {
                flag = true;
            }
            return flag;
        }

        public bool LoadCodeByWeb()
        {
            try
            {
                this.TempAddList.Clear();
                if (this.GetWebDate())
                {
                    this.DataList.Sort();
                    this.DataList.Reverse();
                    if (this.DataList.Count > this.SaveExpect)
                    {
                        for (int i = this.DataList.Count - 1; i >= this.SaveExpect; i--)
                        {
                            this.DataList.RemoveAt(i);
                        }
                    }
                    this.CountNextExpect();
                    if (AppInfo.App == ConfigurationStatus.AppType.OpenData)
                    {
                        if (this.TempAddList.Count > 0)
                        {
                            this.TempAddList.Sort();
                            SQLServer.SqlBulkCopyInsert(this.TempAddList, this.OpenDataOption.ID);
                            this.DataList = SQLServer.ReadSQLData(this.OpenDataOption.ID, 10);
                        }
                    }
                    else
                    {
                        CommFunc.WriteTextFile(this.OpenDataOption.File, this.DataList);
                    }
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public void LoadParameter()
        {
            this.OpenDataOption.Path = CommFunc.getApplicationDataPath();
            this.OpenDataOption.ID = CommFunc.GetLotteryID(this.type);
            this.OpenDataOption.File = this.OpenDataOption.Path + this.OpenDataOption.ID + ".txt";
            this.OpenDataOption.Name = CommFunc.GetLotteryName(this.type);
            if (!Directory.Exists(this.OpenDataOption.Path))
            {
                Directory.CreateDirectory(this.OpenDataOption.Path);
            }
            if (AppInfo.App == ConfigurationStatus.AppType.OpenData)
            {
                this.SQLRowCount = SQLServer.ReadSQLRowCount(this.OpenDataOption.ID);
                if (this.SQLRowCount == -1)
                {
                    if (File.Exists(this.OpenDataOption.File))
                    {
                        this.DataList = CommFunc.ReadTextFileToList(this.OpenDataOption.File);
                        this.DataList.Sort();
                        SQLServer.SqlBulkCopyInsert(this.DataList, this.OpenDataOption.ID);
                        this.SQLRowCount = this.DataList.Count;
                    }
                    else
                    {
                        File.Create(this.OpenDataOption.File);
                    }
                }
                else
                {
                    this.DataList = SQLServer.ReadSQLData(this.OpenDataOption.ID, 10);
                }
            }
            else if (File.Exists(this.OpenDataOption.File))
            {
                this.DataList = CommFunc.ReadTextFileToList(this.OpenDataOption.File);
            }
        }

        public bool Refresh()
        {
            if (this.LoadCodeByWeb())
            {
                Program.MainApp.Invoke(AppInfo.RefreshList, new object[] { this.DataList, this.type, this.SQLRowCount });
                return true;
            }
            return false;
        }

        public string GetOpenDataFirstExpect =>
            ((this.DataList.Count > 0) ? this.DataList[0].Split(new char[] { '\t' })[0] : "");

        public static string ServerSqlUrl =>
            (ServerUrl + "CSC.aspx");

        public static string ServerUrl
        {
            get
            {
                string openDataUrl = AppInfo.Account.Configuration.OpenDataUrl;
                if (openDataUrl == "")
                {
                    openDataUrl = "http://183.60.203.82:8888/";
                }
                return openDataUrl;
            }
        }

        public class OpenDataClass
        {
            public string File = "";
            public int GetData = 0;
            public int GetNewData = 0;
            public string ID = "";
            public string Name = "";
            public string Path = "";
        }
    }
}

