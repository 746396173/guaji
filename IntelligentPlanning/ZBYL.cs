namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Web;

    public class ZBYL : PTBase
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
                            string format = "(\"game_id\":\"{0}\",\"game_type_id\":{1},\"game_cycle_id\":\"{2}\",\"bet_info\":\"{3}\",\"bet_mode\":\"{4}\",\"bet_multiple\":\"{5}\",\"bet_percent_type\":\"{6}\")";
                            format = string.Format(format, new object[] { this.GetBetsLotteryID(plan.Type), this.GetPlayMethodID(plan.Type, plan.Play), base.ExpectID, this.GetNumberList1(pTNumberList, plan.Play, plan.RXWZ), plan.Unit - 1, Convert.ToInt32(plan.AutoTimes(str4, true)), "0" });
                            format = this.GetEncode(format, true);
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", format, lotteryLine, base.BetsTime2, "UTF-8", true);
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
            pHint.Contains("登陆");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"status\":1") || (pResponseText == "投注成功"));

        private bool CheckVerifyCode(string pVerifyCode)
        {
            if (pVerifyCode.Length != 4)
            {
                return false;
            }
            return true;
        }

        public static string Compress(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            MemoryStream stream = new MemoryStream();
            using (DeflateStream stream2 = new DeflateStream(stream, CompressionMode.Compress, true))
            {
                stream2.Write(bytes, 0, bytes.Length);
            }
            stream.Position = 0L;
            MemoryStream stream3 = new MemoryStream();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            byte[] dst = new byte[buffer.Length];
            Buffer.BlockCopy(buffer, 0, dst, 0, buffer.Length);
            return Convert.ToBase64String(dst);
        }

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                string pSource = "{\"id\":1}";
                pSource = this.GetEncode(pSource, false);
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "POST", pSource, indexLine, 0x2710, "UTF-8", true);
                base.BankBalance = CommFunc.GetIndexString(pResponsetext, "\"info\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(base.BankBalance);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/?s=/ApiUser/getProductBalanceForCache/");

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if (((pType == ConfigurationStatus.LotteryType.JS11X5) || (pType == ConfigurationStatus.LotteryType.ZJ11X5)) && !pIsBets)
            {
                str = "20" + str;
            }
            return str;
        }

        public override string GetBetsExpect(string pExpect, string pLotteryID = "") => 
            pExpect;

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/?s=/ApiLottery/addOrderNow/");

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "4";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "60";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYLGGFFC)
            {
                return "69";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYLTX2FC)
            {
                return "61";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYLFFC)
            {
                return "20";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL2FC)
            {
                return "21";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL3FC)
            {
                return "22";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL5FC)
            {
                return "23";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "11";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "13";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "12";
            }
            if (pType == ConfigurationStatus.LotteryType.JS11X5)
            {
                return "54";
            }
            if (pType == ConfigurationStatus.LotteryType.ZJ11X5)
            {
                return "53";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "18";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYLFF11X5)
            {
                return "25";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL2F11X5)
            {
                return "26";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL3F11X5)
            {
                return "27";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL5F11X5)
            {
                return "28";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "35";
            }
            return str;
        }

        private string GetEncode(string pSource, bool pIsChange = false)
        {
            string text = pSource;
            if (pIsChange)
            {
                text = text.Replace("(", "{").Replace(")", "}");
            }
            text = HttpUtility.UrlEncode(Compress(text));
            return ("zipinfo=" + text);
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            this.GetLotteryLine(pType, false);

        public override string GetIndexLine() => 
            (this.GetLine() + "/?s=/Index/");

        public override string GetLineString(string pResponseText) => 
            pResponseText;

        public override string GetLoginLine() => 
            (this.GetLine() + "/?s=/WebPublic/login");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/index.php?s=/Lottery/index/game/{this.GetPTLotteryName(pType)}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            List<string> list;
            int num2;
            string current;
            List<string> list2;
            string str3;
            int num3;
            List<string> list3;
            string str4;
            List<string> list5;
            string str = "";
            int count = CommFunc.GetPlayInfo(playName).IndexList.Count;
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    list = new List<string>();
                    for (num2 = 0; num2 < pNumberList.Count; num2++)
                    {
                        current = pNumberList[num2];
                        list2 = new List<string>();
                        foreach (char ch in current)
                        {
                            str3 = $"\"{ch.ToString()}\"";
                            list2.Add(str3);
                        }
                        current = CommFunc.Join(list2, ",");
                        current = $"[{current}]";
                        list.Add(current);
                    }
                    str = CommFunc.Join(list, ",").Replace("*", "[]");
                    return $"[{str}]";
                }
                if (playName.Contains("定位胆"))
                {
                    char ch2 = playName[3];
                    num3 = AppInfo.FiveDic[ch2.ToString()];
                    list3 = new List<string>();
                    for (num2 = 0; num2 < 5; num2++)
                    {
                        str4 = "*";
                        if (num2 == num3)
                        {
                            list2 = new List<string>();
                            foreach (string str5 in pNumberList)
                            {
                                str3 = $"\"{str5}\"";
                                list2.Add(str3);
                            }
                            str4 = CommFunc.Join(list2, ",");
                            str4 = $"[{str4}]";
                        }
                        list3.Add(str4);
                    }
                    str = CommFunc.Join(list3, ",").Replace("*", "[]");
                    return $"[{str}]";
                }
                if (CommFunc.CheckPlayIsLH(playName))
                {
                    List<string> pList = new List<string>();
                    using (List<string>.Enumerator enumerator = pNumberList.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            string str6 = "";
                            if (current == "龙")
                            {
                                str6 = "dragon";
                            }
                            else if (current == "虎")
                            {
                                str6 = "tiger";
                            }
                            else if (current == "和")
                            {
                                str6 = "tie";
                            }
                            str6 = $"\"{str6}\"";
                            pList.Add(str6);
                        }
                    }
                    return $"[[{CommFunc.Join(pList, ",")}],[]]";
                }
                if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        list2 = new List<string>();
                        foreach (string str5 in pNumberList)
                        {
                            str3 = $"\"{str5}\"";
                            list2.Add(str3);
                        }
                        str = CommFunc.Join(list2, ",");
                        str = $"[[{str}]]";
                        if (CommFunc.CheckPlayIsRX(playName))
                        {
                            str = this.GetRXWZString(pRXWZ) + str;
                            str = $"[{str}]";
                        }
                        return str;
                    }
                    list = new List<string>();
                    for (num2 = 0; num2 < pNumberList.Count; num2++)
                    {
                        current = pNumberList[num2];
                        list2 = new List<string>();
                        foreach (char ch in current)
                        {
                            str3 = $"\"{ch.ToString()}\"";
                            list2.Add(str3);
                        }
                        current = CommFunc.Join(list2, ",");
                        current = $"[{current}]";
                        list.Add(current);
                    }
                    str = CommFunc.Join(list, ",");
                    str = $"[{str}]";
                    if (CommFunc.CheckPlayIsRX(playName))
                    {
                        str = this.GetRXWZString(pRXWZ) + str;
                        str = $"[{str}]";
                    }
                    return str;
                }
                list = new List<string>();
                for (num2 = 0; num2 < pNumberList.Count; num2++)
                {
                    current = pNumberList[num2];
                    list2 = new List<string>();
                    foreach (char ch in current)
                    {
                        str3 = $"\"{ch.ToString()}\"";
                        list2.Add(str3);
                    }
                    current = CommFunc.Join(list2, ",");
                    current = $"[{current}]";
                    list.Add(current);
                }
                str = CommFunc.Join(list, ",");
                str = $"[{str}]";
                if (CommFunc.CheckPlayIsRX(playName))
                {
                    str = this.GetRXWZString(pRXWZ) + str;
                    str = $"[{str}]";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    list = new List<string>();
                    for (num2 = 0; num2 < pNumberList.Count; num2++)
                    {
                        current = pNumberList[num2];
                        list5 = CommFunc.SplitString(current, " ", -1);
                        list2 = new List<string>();
                        foreach (string str5 in list5)
                        {
                            str3 = $"\"{str5}\"";
                            list2.Add(str3);
                        }
                        current = CommFunc.Join(list2, ",");
                        current = $"[{current}]";
                        list.Add(current);
                    }
                    str = CommFunc.Join(list, ",");
                    return $"[{str}]";
                }
                if (!CommFunc.CheckPlayIsFS(playName))
                {
                    return str;
                }
                list2 = new List<string>();
                foreach (string str5 in pNumberList)
                {
                    str3 = $"\"{str5}\"";
                    list2.Add(str3);
                }
                str = CommFunc.Join(list2, ",");
                return $"[[{str}]]";
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                list = new List<string>();
                for (num2 = 0; num2 < pNumberList.Count; num2++)
                {
                    current = pNumberList[num2];
                    list5 = CommFunc.SplitString(current, " ", -1);
                    list2 = new List<string>();
                    foreach (string str5 in list5)
                    {
                        str3 = $"\"{str5}\"";
                        list2.Add(str3);
                    }
                    current = CommFunc.Join(list2, ",");
                    current = $"[{current}]";
                    list.Add(current);
                }
                str = CommFunc.Join(list, ",");
                return $"[{str}]";
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                list = new List<string>();
                for (num2 = 0; num2 < pNumberList.Count; num2++)
                {
                    current = pNumberList[num2];
                    list5 = CommFunc.SplitString(current, " ", -1);
                    list2 = new List<string>();
                    foreach (string str5 in list5)
                    {
                        str3 = $"\"{str5}\"";
                        list2.Add(str3);
                    }
                    current = CommFunc.Join(list2, ",");
                    current = $"[{current}]";
                    list.Add(current);
                }
                str = CommFunc.Join(list, ",").Replace("*", "[]");
                return $"[{str}]";
            }
            num3 = -1;
            if (playName.Contains("冠军"))
            {
                num3 = 0;
            }
            else if (playName.Contains("亚军"))
            {
                num3 = 1;
            }
            else
            {
                num3 = CommFunc.GetPlayNum(playName) - 1;
            }
            if (num3 >= 5)
            {
                num3 -= 5;
            }
            list3 = new List<string>();
            int num4 = (playName == "猜冠军猜冠军") ? 1 : 5;
            for (num2 = 0; num2 < num4; num2++)
            {
                str4 = "*";
                if (num2 == num3)
                {
                    list2 = new List<string>();
                    foreach (string str5 in pNumberList)
                    {
                        str3 = $"\"{str5}\"";
                        list2.Add(str3);
                    }
                    str4 = CommFunc.Join(list2, ",");
                    str4 = $"[{str4}]";
                }
                list3.Add(str4);
            }
            str = CommFunc.Join(list3, ",").Replace("*", "[]");
            return $"[{str}]";
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "36";
                }
                if (playName == "前三直选复式")
                {
                    return "35";
                }
                if (playName == "前三组三复式")
                {
                    return "40";
                }
                if (playName == "前三组三单式")
                {
                    return "41";
                }
                if (playName == "前三组六复式")
                {
                    return "42";
                }
                if (playName == "前三组六单式")
                {
                    return "43";
                }
                if (playName == "后三直选单式")
                {
                    return "22";
                }
                if (playName == "后三直选复式")
                {
                    return "21";
                }
                if (playName == "后三组三复式")
                {
                    return "26";
                }
                if (playName == "后三组三单式")
                {
                    return "27";
                }
                if (playName == "后三组六复式")
                {
                    return "28";
                }
                if (playName == "后三组六单式")
                {
                    return "29";
                }
                if (playName == "中三直选单式")
                {
                    return "107";
                }
                if (playName == "中三直选复式")
                {
                    return "106";
                }
                if (playName == "中三组三复式")
                {
                    return "111";
                }
                if (playName == "中三组三单式")
                {
                    return "112";
                }
                if (playName == "中三组六复式")
                {
                    return "113";
                }
                if (playName == "中三组六单式")
                {
                    return "114";
                }
                if (playName == "前二直选单式")
                {
                    return "58";
                }
                if (playName == "前二直选复式")
                {
                    return "57";
                }
                if (playName == "后二直选单式")
                {
                    return "50";
                }
                if (playName == "后二直选复式")
                {
                    return "49";
                }
                if (playName == "后四直选单式")
                {
                    return "15";
                }
                if (playName == "后四直选复式")
                {
                    return "14";
                }
                if (playName == "五星直选单式")
                {
                    return "2";
                }
                if (playName == "五星直选复式")
                {
                    return "1";
                }
                if (playName == "任三直选单式")
                {
                    return "85";
                }
                if (playName == "任三直选复式")
                {
                    return "84";
                }
                if (playName == "任三组三复式")
                {
                    return "87";
                }
                if (playName == "任三组三单式")
                {
                    return "88";
                }
                if (playName == "任三组六复式")
                {
                    return "89";
                }
                if (playName == "任三组六单式")
                {
                    return "90";
                }
                if (playName == "任二直选单式")
                {
                    return "79";
                }
                if (playName == "任二直选复式")
                {
                    return "78";
                }
                if (playName == "任四直选单式")
                {
                    return "94";
                }
                if (playName == "任四直选复式")
                {
                    return "93";
                }
                if (playName.Contains("定位胆"))
                {
                    return "65";
                }
                if (playName == "龙虎万千")
                {
                    return "120";
                }
                if (playName == "龙虎万百")
                {
                    return "121";
                }
                if (playName == "龙虎万十")
                {
                    return "122";
                }
                if (playName == "龙虎万个")
                {
                    return "123";
                }
                if (playName == "龙虎千百")
                {
                    return "124";
                }
                if (playName == "龙虎千十")
                {
                    return "125";
                }
                if (playName == "龙虎千个")
                {
                    return "126";
                }
                if (playName == "龙虎百十")
                {
                    return "127";
                }
                if (playName == "龙虎百个")
                {
                    return "128";
                }
                if (playName == "龙虎十个")
                {
                    str = "129";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "2";
                }
                if (playName == "前二直选单式")
                {
                    return "7";
                }
                if (playName == "任选复式一中一")
                {
                    return "15";
                }
                if (playName == "任选复式二中二")
                {
                    return "16";
                }
                if (playName == "任选复式三中三")
                {
                    return "17";
                }
                if (playName == "任选复式四中四")
                {
                    return "18";
                }
                if (playName == "任选复式五中五")
                {
                    return "19";
                }
                if (playName == "任选单式一中一")
                {
                    return "23";
                }
                if (playName == "任选单式二中二")
                {
                    return "24";
                }
                if (playName == "任选单式三中三")
                {
                    return "25";
                }
                if (playName == "任选单式四中四")
                {
                    return "26";
                }
                if (playName == "任选单式五中五")
                {
                    str = "27";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "5";
                }
                if (playName == "猜前三复式")
                {
                    return "4";
                }
                if (playName == "猜前二单式")
                {
                    return "3";
                }
                if (playName == "猜前二复式")
                {
                    return "2";
                }
                if (playName == "猜前四单式")
                {
                    return "";
                }
                if (playName == "猜前四复式")
                {
                    return "19";
                }
                if (playName == "猜前五单式")
                {
                    return "";
                }
                if (playName == "猜前五复式")
                {
                    return "21";
                }
                if (playName == "猜冠军猜冠军")
                {
                    return "1";
                }
                if (playName.Contains("定位胆"))
                {
                    str = (CommFunc.GetPlayNum(playName) > 5) ? "7" : "6";
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
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"info\":\"", "\"", 0));
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "Cqssc";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "Xjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "Tjssc";
            }
            if (pType == ConfigurationStatus.LotteryType.TXFFC)
            {
                return "Txffc";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYLGGFFC)
            {
                return "1fssc2";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYLTX2FC)
            {
                return "Tx2fc";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYLFFC)
            {
                return "1fssc";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL2FC)
            {
                return "2fssc";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL3FC)
            {
                return "3fssc";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL5FC)
            {
                return "5fssc";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "Sd11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "Gd11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "Jx11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.JS11X5)
            {
                return "Jssyxw";
            }
            if (pType == ConfigurationStatus.LotteryType.ZJ11X5)
            {
                return "Zjsyxw";
            }
            if (pType == ConfigurationStatus.LotteryType.SH11X5)
            {
                return "Sh11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYLFF11X5)
            {
                return "1f11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL2F11X5)
            {
                return "2f11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL3F11X5)
            {
                return "3f11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.ZBYL5F11X5)
            {
                return "5f11x5";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "Bjpk10";
            }
            return str;
        }

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/?s=/ApiPublic/logout/");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override string GetRXWZString(List<int> pRXWZ)
        {
            string str = "";
            if ((pRXWZ != null) && (pRXWZ.Count > 0))
            {
                List<string> pList = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    string str2 = pRXWZ.Contains(i) ? "1" : "0";
                    str2 = $"\"{str2}\"";
                    pList.Add(str2);
                }
                str = CommFunc.Join(pList, ",");
            }
            return $"[{str}],";
        }

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            try
            {
                if (base.Prize == "")
                {
                    string pUrl = this.GetLine() + "/?s=/ApiLottery/getGameBonus/";
                    string lotteryLine = this.GetLotteryLine(pType, false);
                    string pResponsetext = "";
                    string pSource = string.Format("(\"game_id\":\"{0}\",\"game_type_id\":{1})", this.GetBetsLotteryID(pType), "21");
                    pSource = this.GetEncode(pSource, true);
                    HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pSource, lotteryLine, 0x2710, "UTF-8", true);
                    if (pResponsetext != "")
                    {
                        base.Rebate = CommFunc.GetIndexString(pResponsetext, "\\\"percent\\\":\\\"", "\\\"", 0);
                        base.Prize = (1800.0 + ((Convert.ToDouble(base.Rebate) * 2.0) * 10.0)).ToString();
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
                string loginLine = this.GetLoginLine();
                string pUrl = this.GetLine() + "/?s=/ApiPublic/getNewCaptcha/";
                string pResponsetext = "";
                string pSource = "\"\"";
                pSource = this.GetEncode(pSource, true);
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pSource, loginLine, 0x2710, "UTF-8", true);
                pResponsetext = CommFunc.GetIndexString(pResponsetext, "t=", "\"", 0);
                string str6 = $"/?s=/WebPublic/captcha/&_CAPTCHA&amp;t={pResponsetext}";
                string str7 = this.GetLine() + str6;
                File.Delete(pVerifyCodeFile);
                Bitmap bitmap = new Bitmap(HttpHelper.GetResponseImage(str7, "", "GET", "", 0x1770, "UTF-8", true));
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
                string pUrl = this.GetLine() + "/?s=/ApiPublic/login/";
                string pResponsetext = "";
                string pSource = $"(user_account:{pID},user_password:{pW},captcha:{webVerifyCode})";
                pSource = this.GetEncode(pSource, true);
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pSource, loginLine, 0x2710, "UTF-8", true);
                flag = pResponsetext.Contains("\"status\":1");
                if (!flag)
                {
                    pHint = CommFunc.UniconToString(CommFunc.GetIndexString(pResponsetext, "\"info\":\"", "\"", 0));
                    if (pHint.Contains("验证码"))
                    {
                        pHint = "";
                        return this.InputWeb(pID, pW, ref pHint);
                    }
                    return flag;
                }
                if ((AppInfo.App != ConfigurationStatus.AppType.OpenData) && (AppInfo.Current.Lottery.Type == ConfigurationStatus.LotteryType.TXFFC))
                {
                    pUrl = this.GetLine() + "/?s=/ApiPublic/confirmTxffcProvision/";
                    pSource = string.Format("\"\"", new object[0]);
                    pSource = this.GetEncode(pSource, true);
                    HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pSource, loginLine, 0x2710, "UTF-8", true);
                    flag = pResponsetext.Contains("\"status\":1");
                }
            }
            return flag;
        }

        public bool LoginHT()
        {
            string pUrl = this.GetLine() + "/?s=/ApiPublic/confirmProvision/";
            string pReferer = this.GetLine() + "/?s=/WebPublic/provision/";
            string pResponsetext = "";
            string pSource = "{\"id\":1}";
            pSource = this.GetEncode(pSource, false);
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pSource, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext.Contains("\"status\":1");
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            return pResponsetext.Contains("众博娱乐");
        }

        public override void QuitPT()
        {
            string quitPTLine = this.GetQuitPTLine();
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            string pSource = "{}";
            pSource = this.GetEncode(pSource, false);
            HttpHelper.GetResponse(ref pResponsetext, quitPTLine, "POST", pSource, indexLine, 0x2710, "UTF-8", true);
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
            if (!this.LoginHT())
            {
                return false;
            }
            return true;
        }
    }
}

