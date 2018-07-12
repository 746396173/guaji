namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web;

    public class K3YL : PTBase
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
                            string play = plan.Play;
                            ConfigurationStatus.PlayBase playInfo = CommFunc.GetPlayInfo(play);
                            int num = plan.FNNumber(str4);
                            int num2 = 2;
                            string prize = base.Prize;
                            string format = "";
                            Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
                            if (CommFunc.CheckPlayIsRX(play))
                            {
                                string current;
                                string str10;
                                string str11;
                                int num3;
                                int count;
                                List<string> list5;
                                List<string> pList = new List<string>();
                                Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
                                List<string> list3 = CommFunc.SplitString(this.GetPlayMethodID(plan.Type, plan.Play), ";", -1);
                                using (List<string>.Enumerator enumerator3 = list3.GetEnumerator())
                                {
                                    while (enumerator3.MoveNext())
                                    {
                                        current = enumerator3.Current;
                                        str10 = CommFunc.Join(current.Split(new char[] { '-' })[0], ",", -1);
                                        str11 = current.Split(new char[] { '-' })[1];
                                        dictionary4[str10] = str11;
                                    }
                                }
                                if (CommFunc.CheckPlayIsRXFS(play))
                                {
                                    List<string> itemList = new List<string>();
                                    num3 = 0;
                                    while (num3 < pTNumberList.Count)
                                    {
                                        string item = $"{num3}-{pTNumberList[num3]}";
                                        itemList.Add(item);
                                        num3++;
                                    }
                                    count = playInfo.IndexList.Count;
                                    list5 = CommFunc.GetCombinations(itemList, count, "|");
                                    num3 = 0;
                                    while (num3 < list5.Count)
                                    {
                                        string pStr = list5[num3];
                                        if (!pStr.Contains("*"))
                                        {
                                            List<string> list6 = new List<string>();
                                            List<string> list7 = new List<string>();
                                            List<string> list8 = CommFunc.SplitString(pStr, "|", -1);
                                            for (int i = 0; i < list8.Count; i++)
                                            {
                                                string str14 = list8[i];
                                                list6.Add(str14.Split(new char[] { '-' })[0]);
                                                list7.Add(str14.Split(new char[] { '-' })[1]);
                                            }
                                            str10 = CommFunc.Join(list6, ",");
                                            str11 = CommFunc.Join(list7, "|");
                                            dictionary3[str10] = str11;
                                        }
                                        num3++;
                                    }
                                }
                                else if (CommFunc.CheckPlayIsRXDS(play))
                                {
                                    List<string> list9 = new List<string>();
                                    num3 = 0;
                                    while (num3 < plan.RXWZ.Count)
                                    {
                                        string str15 = plan.RXWZ[num3].ToString();
                                        list9.Add(str15);
                                        num3++;
                                    }
                                    count = playInfo.IndexList.Count;
                                    list5 = CommFunc.GetCombinations(list9, count, ",");
                                    for (num3 = 0; num3 < list5.Count; num3++)
                                    {
                                        str10 = list5[num3];
                                        str11 = this.GetNumberList1(pTNumberList, play, null);
                                        if (CommFunc.CheckPlayIsZuX(play) && play.Contains("复式"))
                                        {
                                            str11 = CommFunc.Join(str11, "|", -1);
                                        }
                                        dictionary3[str10] = str11;
                                    }
                                }
                                int num6 = 1;
                                foreach (string str100 in dictionary3.Keys)
                                {
                                    current = dictionary4[str100];
                                    str11 = dictionary3[str100];
                                    List<string> list10 = CommFunc.SplitString(str100, ",", -1);
                                    List<string> list11 = new List<string>();
                                    foreach (string str16 in list10)
                                    {
                                        list11.Add((Convert.ToInt32(str16) + 1).ToString());
                                    }
                                    string str17 = CommFunc.Join(list11);
                                    int num8 = num;
                                    if (CommFunc.CheckPlayIsRXFS(play))
                                    {
                                        count = playInfo.IndexList.Count;
                                        string playName = "";
                                        switch (count)
                                        {
                                            case 2:
                                                playName = "前二直选复式";
                                                break;

                                            case 3:
                                                playName = "前三直选复式";
                                                break;

                                            case 4:
                                                playName = "前四直选复式";
                                                break;
                                        }
                                        num8 = CommFunc.GetBetsCodeCount(CommFunc.SplitString(str11, "|", -1), playName, null);
                                    }
                                    else if (CommFunc.CheckPlayIsRXDS(play))
                                    {
                                        num8 = CommFunc.GetBetsCodeCount(pTNumberList, play, null);
                                    }
                                    string str19 = "balls%5B{0}%5D%5Bball%5D={1}&balls%5B{0}%5D%5BviewBalls%5D=&balls%5B{0}%5D%5Bnum%5D={2}&balls%5B{0}%5D%5Btype%5D={3}&balls%5B{0}%5D%5BonePrice%5D={4}&balls%5B{0}%5D%5Bmoneyunit%5D={5}&balls%5B{0}%5D%5Bmultiple%5D={6}&balls%5B{0}%5D%5BprizeGroup%5D={7}&balls%5B{0}%5D%5Bindex%5D={8}&balls%5B{0}%5D%5BjsId%5D={9}&balls%5B{0}%5D%5BwayId%5D={10}";
                                    str19 = string.Format(str19, new object[] { num6 - 1, HttpUtility.UrlEncode(str11), num8, this.GetPlayString(play), num2, plan.Money / ((double) num2), Convert.ToInt32(plan.AutoTimes(str4, true)), prize, HttpUtility.UrlEncode(str100), num6, current });
                                    pList.Add(str19);
                                    num6++;
                                }
                                format = CommFunc.Join(pList, "&");
                            }
                            else
                            {
                                format = "balls%5B0%5D%5BjsId%5D={0}&balls%5B0%5D%5BwayId%5D={1}&balls%5B0%5D%5Bball%5D={2}&balls%5B0%5D%5BviewBalls%5D=&balls%5B0%5D%5Bnum%5D={3}&balls%5B0%5D%5Btype%5D={4}&balls%5B0%5D%5BonePrice%5D={5}&balls%5B0%5D%5BprizeGroup%5D={6}&balls%5B0%5D%5Bmoneyunit%5D={7}&balls%5B0%5D%5Bmultiple%5D={8}";
                                format = string.Format(format, new object[] { "1", this.GetPlayMethodID(plan.Type, plan.Play), HttpUtility.UrlEncode(this.GetNumberList1(pTNumberList, plan.Play, null)), num, this.GetPlayString(plan.Play), num2, prize, plan.Money / ((double) num2), Convert.ToInt32(plan.AutoTimes(str4, true)) });
                            }
                            string str20 = "gameId={0}&isTrace=0&traceWinStop=1&traceStopValue=1&{1}&orders%5B{2}%5D={3}&amount={4}";
                            str20 = string.Format(str20, new object[] { this.GetBetsLotteryID(plan.Type), format, this.GetBetsExpect(plan.CurrentExpect, ""), "1", plan.AutoTotalMoney(str4, true) });
                            HttpHelper.GetResponse(ref pResponsetext, betsLine, "POST", str20, lotteryLine, base.BetsTime3, "UTF-8", true);
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
            pHint.Contains("重新登录");

        public override bool CheckReturn(string pResponseText, bool pIsChange) => 
            (pResponseText.Contains("\"isSuccess\":1") || (pResponseText == "投注成功"));

        public override void GetAccountsMem(ConfigurationStatus.LotteryType pType, ConfigurationStatus.SCAccountData pInfo)
        {
            try
            {
                string accountsMemLine = this.GetAccountsMemLine(pType);
                string indexLine = this.GetIndexLine();
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, accountsMemLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
                string str4 = CommFunc.GetIndexString(pResponsetext, "\"available\":\"", "\"", 0);
                AppInfo.Account.BankBalance = Convert.ToDouble(str4);
            }
            catch
            {
            }
        }

        public override string GetAccountsMemLine(ConfigurationStatus.LotteryType pType) => 
            (this.GetLine() + "/users/user-monetary-info");

        public override string GetAppExpect(ConfigurationStatus.LotteryType pType, string pExpect, bool pIsBets = false)
        {
            string str = pExpect;
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                if (pIsBets)
                {
                    str = str.Replace("-", "");
                }
                return str;
            }
            if ((((pType == ConfigurationStatus.LotteryType.K3HGSSC) || (pType == ConfigurationStatus.LotteryType.K3DJSSC)) || (pType == ConfigurationStatus.LotteryType.K3TXFFC)) || (pType == ConfigurationStatus.LotteryType.K3NTXFFC))
            {
                if (pIsBets)
                {
                    return ("20" + str.Replace("-", ""));
                }
                return str.Substring(2);
            }
            if ((pType == ConfigurationStatus.LotteryType.K3HG2FC) || (pType == ConfigurationStatus.LotteryType.K3MG5FC))
            {
                if (pIsBets)
                {
                    return str.Insert(6, "0").Replace("-", "");
                }
                return str.Remove(6, 1);
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
            return this.GetAppExpect(AppInfo.Current.Lottery.Type, pExpect, true);
        }

        public override string GetBetsLine(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/bets/bet/{this.GetBetsLotteryID(pType)}";

        public override string GetBetsLotteryID(ConfigurationStatus.LotteryType pType)
        {
            string str = "";
            if (pType == ConfigurationStatus.LotteryType.CQSSC)
            {
                return "1";
            }
            if (pType == ConfigurationStatus.LotteryType.XJSSC)
            {
                return "6";
            }
            if (pType == ConfigurationStatus.LotteryType.TJSSC)
            {
                return "7";
            }
            if (pType == ConfigurationStatus.LotteryType.BJSSC)
            {
                return "53";
            }
            if (pType == ConfigurationStatus.LotteryType.TWSSC)
            {
                return "39";
            }
            if (pType == ConfigurationStatus.LotteryType.K3TXFFC)
            {
                return "43";
            }
            if (pType == ConfigurationStatus.LotteryType.K3NTXFFC)
            {
                return "42";
            }
            if (pType == ConfigurationStatus.LotteryType.K3XXLSSC)
            {
                return "54";
            }
            if (pType == ConfigurationStatus.LotteryType.K3DJFFC)
            {
                return "44";
            }
            if (pType == ConfigurationStatus.LotteryType.K3HG2FC)
            {
                return "45";
            }
            if (pType == ConfigurationStatus.LotteryType.K3MG5FC)
            {
                return "46";
            }
            if (pType == ConfigurationStatus.LotteryType.K3HGSSC)
            {
                return "41";
            }
            if (pType == ConfigurationStatus.LotteryType.K3DJSSC)
            {
                return "23";
            }
            if (pType == ConfigurationStatus.LotteryType.GD11X5)
            {
                return "9";
            }
            if (pType == ConfigurationStatus.LotteryType.SD11X5)
            {
                return "2";
            }
            if (pType == ConfigurationStatus.LotteryType.LN11X5)
            {
                return "30";
            }
            if (pType == ConfigurationStatus.LotteryType.JX11X5)
            {
                return "8";
            }
            if (pType == ConfigurationStatus.LotteryType.K311X5)
            {
                return "47";
            }
            if (pType == ConfigurationStatus.LotteryType.PK10)
            {
                str = "22";
            }
            return str;
        }

        public override string GetIndexCode(ConfigurationStatus.LotteryType pType) => 
            $"{this.GetLine()}/buy/load-numbers/{this.GetBetsLotteryID(pType)}";

        public override string GetIndexLine() => 
            (this.GetLine() + "/");

        public override string GetLoginLine() => 
            (this.GetLine() + "/auth/signin");

        public override string GetLotteryLine(ConfigurationStatus.LotteryType pType, bool pAll = false) => 
            $"{this.GetLine()}/buy/bet/{this.GetBetsLotteryID(pType)}";

        public override string GetNumberList1(List<string> pNumberList, string playName, List<int> pRXWZ = null)
        {
            int num;
            List<string> list;
            int num2;
            string str2;
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    return CommFunc.Join(pNumberList, "|").Replace("*", "");
                }
                if (playName.Contains("定位胆"))
                {
                    char ch = playName[3];
                    num = AppInfo.FiveDic[ch.ToString()];
                    list = new List<string>();
                    for (num2 = 0; num2 < 5; num2++)
                    {
                        str2 = "*";
                        if (num2 == num)
                        {
                            str2 = CommFunc.Join(pNumberList);
                        }
                        list.Add(str2);
                    }
                    return CommFunc.Join(list, "|").Replace("*", "");
                }
                if (CommFunc.CheckPlayIsZuX(playName))
                {
                    if (playName.Contains("复式"))
                    {
                        return CommFunc.Join(pNumberList);
                    }
                    return CommFunc.Join(pNumberList, "|");
                }
                return CommFunc.Join(pNumberList, "|");
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (CommFunc.CheckPlayIsDS(playName))
                {
                    return CommFunc.Join(pNumberList, "|");
                }
                if (CommFunc.CheckPlayIsFS(playName))
                {
                    str = CommFunc.Join(pNumberList, " ");
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group != ConfigurationStatus.LotteryGroup.GPPK10)
            {
                return str;
            }
            if (CommFunc.CheckPlayIsDS(playName))
            {
                return CommFunc.Join(pNumberList, "|");
            }
            if (CommFunc.CheckPlayIsFS(playName))
            {
                return CommFunc.Join(pNumberList, "|").Replace("*", "");
            }
            num = -1;
            if (playName.Contains("冠军"))
            {
                num = 0;
            }
            else if (playName.Contains("亚军"))
            {
                num = 1;
            }
            else
            {
                num = CommFunc.GetPlayNum(playName) - 1;
            }
            list = new List<string>();
            int num3 = (playName == "猜冠军猜冠军") ? 1 : 10;
            pNumberList = CommFunc.ConvertPK10CodeToBets(pNumberList, -1);
            for (num2 = 0; num2 < num3; num2++)
            {
                str2 = "*";
                if (num2 == num)
                {
                    str2 = CommFunc.Join(pNumberList, " ");
                }
                list.Add(str2);
            }
            return CommFunc.Join(list, "|").Replace("*", "");
        }

        public override string GetPlayMethodID(ConfigurationStatus.LotteryType pType, string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "1";
                }
                if (playName == "前三直选复式")
                {
                    return "65";
                }
                if (playName == "前三组三复式")
                {
                    return "16";
                }
                if (playName == "前三组三单式")
                {
                    return "2";
                }
                if (playName == "前三组六复式")
                {
                    return "17";
                }
                if (playName == "前三组六单式")
                {
                    return "3";
                }
                if (playName == "后三直选单式")
                {
                    return "8";
                }
                if (playName == "后三直选复式")
                {
                    return "69";
                }
                if (playName == "后三组三复式")
                {
                    return "49";
                }
                if (playName == "后三组三单式")
                {
                    return "9";
                }
                if (playName == "后三组六复式")
                {
                    return "50";
                }
                if (playName == "后三组六单式")
                {
                    return "10";
                }
                if (playName == "中三直选单式")
                {
                    return "142";
                }
                if (playName == "中三直选复式")
                {
                    return "150";
                }
                if (playName == "中三组三复式")
                {
                    return "145";
                }
                if (playName == "中三组三单式")
                {
                    return "143";
                }
                if (playName == "中三组六复式")
                {
                    return "146";
                }
                if (playName == "中三组六单式")
                {
                    return "144";
                }
                if (playName == "前二直选单式")
                {
                    return "4";
                }
                if (playName == "前二直选复式")
                {
                    return "66";
                }
                if (playName == "后二直选单式")
                {
                    return "11";
                }
                if (playName == "后二直选复式")
                {
                    return "70";
                }
                if (playName == "后四直选单式")
                {
                    return "6";
                }
                if (playName == "后四直选复式")
                {
                    return "67";
                }
                if (playName == "五星直选单式")
                {
                    return "7";
                }
                if (playName == "五星直选复式")
                {
                    return "68";
                }
                if (playName == "任三直选单式")
                {
                    return "012-259;013-246;014-227;023-293;024-273;034-214;123-344;124-325;134-312;234-357";
                }
                if (playName == "任三直选复式")
                {
                    return "012-258;013-245;014-226;023-292;024-272;034-213;123-343;124-324;134-311;234-356";
                }
                if (playName == "任三组三复式")
                {
                    return "012-261;013-248;014-229;023-295;024-275;034-216;123-346;124-327;134-314;234-359";
                }
                if (playName == "任三组三单式")
                {
                    return "012-262;013-249;014-230;023-296;024-276;034-217;123-347;124-328;134-315;234-360";
                }
                if (playName == "任三组六复式")
                {
                    return "012-263;013-250;014-231;023-297;024-277;034-218;123-348;124-329;134-316;234-361";
                }
                if (playName == "任三组六单式")
                {
                    return "012-264;013-251;014-232;023-298;024-279;034-219;123-349;124-330;134-317;234-362";
                }
                if (playName == "任二直选单式")
                {
                    return "01-234;02-281;03-221;04-208;12-332;13-319;14-306;23-364;24-351;34-300";
                }
                if (playName == "任二直选复式")
                {
                    return "01-233;02-280;03-220;04-207;12-331;13-318;14-305;23-363;24-350;34-299";
                }
                if (playName == "任四直选单式")
                {
                    return "0123-267;0124-253;0134-240;0234-287;1234-338";
                }
                if (playName == "任四直选复式")
                {
                    return "0123-266;0124-252;0134-239;0234-286;1234-337";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "78";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "95";
                }
                if (playName == "前二直选单式")
                {
                    return "94";
                }
                if (playName == "任选复式一中一")
                {
                    return "98";
                }
                if (playName == "任选复式二中二")
                {
                    return "99";
                }
                if (playName == "任选复式三中三")
                {
                    return "100";
                }
                if (playName == "任选复式四中四")
                {
                    return "101";
                }
                if (playName == "任选复式五中五")
                {
                    return "102";
                }
                if (playName == "任选单式一中一")
                {
                    return "86";
                }
                if (playName == "任选单式二中二")
                {
                    return "87";
                }
                if (playName == "任选单式三中三")
                {
                    return "88";
                }
                if (playName == "任选单式四中四")
                {
                    return "89";
                }
                if (playName == "任选单式五中五")
                {
                    str = "90";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "";
                }
                if (playName == "猜前三复式")
                {
                    return "515";
                }
                if (playName == "猜前二单式")
                {
                    return "";
                }
                if (playName == "猜前二复式")
                {
                    return "514";
                }
                if (playName == "猜前四单式")
                {
                    return "";
                }
                if (playName == "猜前四复式")
                {
                    return "";
                }
                if (playName == "猜前五单式")
                {
                    return "";
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
                    str = "513";
                }
            }
            return str;
        }

        public override string GetPlayString(string playName)
        {
            string str = "";
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPSSC)
            {
                if (playName == "前三直选单式")
                {
                    return "qiansan.zhixuan.danshi";
                }
                if (playName == "前三直选复式")
                {
                    return "qiansan.zhixuan.fushi";
                }
                if (playName == "前三组三复式")
                {
                    return "qiansan.zuxuan.zusan";
                }
                if (playName == "前三组三单式")
                {
                    return "qiansan.zuxuan.zusandanshi";
                }
                if (playName == "前三组六复式")
                {
                    return "qiansan.zuxuan.zuliu";
                }
                if (playName == "前三组六单式")
                {
                    return "qiansan.zuxuan.zuliudanshi";
                }
                if (playName == "后三直选单式")
                {
                    return "housan.zhixuan.danshi";
                }
                if (playName == "后三直选复式")
                {
                    return "housan.zhixuan.fushi";
                }
                if (playName == "后三组三复式")
                {
                    return "housan.zuxuan.zusan";
                }
                if (playName == "后三组三单式")
                {
                    return "housan.zuxuan.zusandanshi";
                }
                if (playName == "后三组六复式")
                {
                    return "housan.zuxuan.zuliu";
                }
                if (playName == "后三组六单式")
                {
                    return "housan.zuxuan.zuliudanshi";
                }
                if (playName == "中三直选单式")
                {
                    return "zhongsan.zhixuan.danshi";
                }
                if (playName == "中三直选复式")
                {
                    return "zhongsan.zhixuan.fushi";
                }
                if (playName == "中三组三复式")
                {
                    return "zhongsan.zuxuan.zusan";
                }
                if (playName == "中三组三单式")
                {
                    return "zhongsan.zuxuan.zusandanshi";
                }
                if (playName == "中三组六复式")
                {
                    return "zhongsan.zuxuan.zuliu";
                }
                if (playName == "中三组六单式")
                {
                    return "zhongsan.zuxuan.zuliudanshi";
                }
                if (playName == "前二直选单式")
                {
                    return "erxing.zhixuan.qianerdanshi";
                }
                if (playName == "前二直选复式")
                {
                    return "erxing.zhixuan.qianerfushi";
                }
                if (playName == "后二直选单式")
                {
                    return "erxing.zhixuan.houerdanshi";
                }
                if (playName == "后二直选复式")
                {
                    return "erxing.zhixuan.houerfushi";
                }
                if (playName == "后四直选单式")
                {
                    return "sixing.zhixuan.danshi";
                }
                if (playName == "后四直选复式")
                {
                    return "sixing.zhixuan.fushi";
                }
                if (playName == "五星直选单式")
                {
                    return "wuxing.zhixuan.danshi";
                }
                if (playName == "五星直选复式")
                {
                    return "wuxing.zhixuan.fushi";
                }
                if (playName == "任三直选单式")
                {
                    return "renxuansan.zhixuan.zhixuandanshi";
                }
                if (playName == "任三直选复式")
                {
                    return "renxuansan.zhixuan.zhixuanfushi";
                }
                if (playName == "任三组三复式")
                {
                    return "renxuansan.zuxuan.zusanfushi";
                }
                if (playName == "任三组三单式")
                {
                    return "renxuansan.zuxuan.zusandanshi";
                }
                if (playName == "任三组六复式")
                {
                    return "renxuansan.zuxuan.zuliufushi";
                }
                if (playName == "任三组六单式")
                {
                    return "renxuansan.zuxuan.zuliudanshi";
                }
                if (playName == "任二直选单式")
                {
                    return "renxuaner.zhixuan.zhixuandanshi";
                }
                if (playName == "任二直选复式")
                {
                    return "renxuaner.zhixuan.zhixuanfushi";
                }
                if (playName == "任四直选单式")
                {
                    return "renxuansi.zhixuan.zhixuandanshi";
                }
                if (playName == "任四直选复式")
                {
                    return "renxuansi.zhixuan.zhixuanfushi";
                }
                if (playName.Contains("定位胆"))
                {
                    str = "yixing.dingweidan.fushi";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GP11X5)
            {
                if (playName == "前三直选单式")
                {
                    return "sanma.zhixuan.danshi";
                }
                if (playName == "前二直选单式")
                {
                    return "erma.zhixuan.danshi";
                }
                if (playName == "任选复式一中一")
                {
                    return "renxuanfushi.renxuanfushi.renxuanyi";
                }
                if (playName == "任选复式二中二")
                {
                    return "renxuanfushi.renxuanfushi.renxuaner";
                }
                if (playName == "任选复式三中三")
                {
                    return "renxuanfushi.renxuanfushi.renxuansan";
                }
                if (playName == "任选复式四中四")
                {
                    return "renxuanfushi.renxuanfushi.renxuansi";
                }
                if (playName == "任选复式五中五")
                {
                    return "renxuanfushi.renxuanfushi.renxuanwu";
                }
                if (playName == "任选单式一中一")
                {
                    return "renxuandanshi.renxuandanshi.renxuanyi";
                }
                if (playName == "任选单式二中二")
                {
                    return "renxuandanshi.renxuandanshi.renxuaner";
                }
                if (playName == "任选单式三中三")
                {
                    return "renxuandanshi.renxuandanshi.renxuansan";
                }
                if (playName == "任选单式四中四")
                {
                    return "renxuandanshi.renxuandanshi.renxuansi";
                }
                if (playName == "任选单式五中五")
                {
                    str = "renxuandanshi.renxuandanshi.renxuanwu";
                }
                return str;
            }
            if (AppInfo.Current.Lottery.Group == ConfigurationStatus.LotteryGroup.GPPK10)
            {
                if (playName == "猜前三单式")
                {
                    return "";
                }
                if (playName == "猜前三复式")
                {
                    return "qiansan.qiansan.fushi";
                }
                if (playName == "猜前二单式")
                {
                    return "";
                }
                if (playName == "猜前二复式")
                {
                    return "qianer.qianer.fushi";
                }
                if (playName == "猜前四单式")
                {
                    return "";
                }
                if (playName == "猜前四复式")
                {
                    return "";
                }
                if (playName == "猜前五单式")
                {
                    return "";
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
                    str = "caimingci.caimingci.caimingci";
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
            return CommFunc.UniconToString(CommFunc.GetIndexString(pResponseText, "\"Msg\":\"", "\"", 0).Replace("-", ""));
        }

        public override string GetPTLotteryName(ConfigurationStatus.LotteryType pType) => 
            "";

        public override string GetQuitPTLine() => 
            (this.GetLine() + "/auth/logout");

        public override string GetReturn(string pResponseText) => 
            pResponseText;

        public override void GetSite(ConfigurationStatus.LotteryType pType, string playName = "")
        {
            if (base.Prize == "")
            {
                string pStr = this.LoginLotteryWeb(pType, "");
                if (pStr != "")
                {
                    base.Prize = CommFunc.GetIndexString(pStr, "\"defaultPrizeGroup\":\"", "\"", 0);
                }
            }
        }

        public bool InputWeb(string pID, string pW, ref string pHint)
        {
            bool flag = false;
            string loginLine = this.GetLoginLine();
            string pUrl = this.GetLoginLine();
            string pResponsetext = "";
            string str4 = HttpUtility.UrlEncode(pW);
            string str5 = HttpUtility.UrlEncode(CommFunc.WebMD51(CommFunc.WebMD51(CommFunc.WebMD51(pID + pW))));
            string pData = $"_token={base.Token}&_random={base.Random}&username={pID}&password={str5}";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, loginLine, 0x2710, "UTF-8", true);
            flag = pResponsetext.Contains("彩票大厅");
            if (!flag)
            {
                pHint = CommFunc.GetIndexString(pResponsetext, "\"pop-content\">", "<", 0);
            }
            return flag;
        }

        public override string LoginLotteryWeb(ConfigurationStatus.LotteryType pType, string pInfo = "")
        {
            string lotteryLine = this.GetLotteryLine(pType, false);
            string indexLine = this.GetIndexLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, lotteryLine, "GET", string.Empty, indexLine, 0x2710, "UTF-8", true);
            return pResponsetext;
        }

        public bool LoginWeb()
        {
            string pReferer = "";
            string loginLine = this.GetLoginLine();
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, loginLine, "GET", string.Empty, pReferer, 0x2710, "UTF-8", true);
            bool flag = pResponsetext.Contains("K3游戏");
            if (flag)
            {
                base.Token = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("_token"));
                base.Random = CommFunc.GetIndexString(pResponsetext, "value=\"", "\"", pResponsetext.IndexOf("_random"));
            }
            return flag;
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
            if (!this.LoginWeb())
            {
                return false;
            }
            if (!this.InputWeb(pID, pW, ref pHint))
            {
                return false;
            }
            return true;
        }
    }
}

