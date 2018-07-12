namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Xml;

    internal class SQLData
    {
        public static string ServerSqlUrl = AppInfo.cServerSqlUrl;
        public static string ServerUrl = AppInfo.cServerUrl;

        public static string AddCZListRow(string password, string pCreatTime, string pDay, string pAppName, string pAppID)
        {
            string str = "";
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=addCZList";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"Password={HttpUtility.UrlEncode(password)}&CreatTime={HttpUtility.UrlEncode(pCreatTime)}&Day={HttpUtility.UrlEncode(pDay)}&AppName={HttpUtility.UrlEncode(pAppName)}&CreatID={HttpUtility.UrlEncode(pAppID)}&&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
                str = pResponsetext;
            }
            catch
            {
            }
            return str;
        }

        public static string AddDLUserRow(ConfigurationStatus.SCAccountData pAccountData, ConfigurationStatus.SCAccountData pDLAccountData)
        {
            string str = "";
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=addDLUser";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.ID)}&Password={HttpUtility.UrlEncode(pAccountData.PW)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&ActiveTime={HttpUtility.UrlEncode(pAccountData.ActiveTime)}&AllowCZ={HttpUtility.UrlEncode(pAccountData.AllowCZ.ToString())}&AllowDelete={HttpUtility.UrlEncode(pAccountData.AllowDelete.ToString())}&AllowClear={HttpUtility.UrlEncode(pAccountData.AllowClear.ToString())}&AllowDK={HttpUtility.UrlEncode(pAccountData.AllowDK.ToString())}&Configuration={HttpUtility.UrlEncode(pAccountData.ConfigurationString)}&GGImage={HttpUtility.UrlEncode(pAccountData.GGImageString)}&DLUserID={HttpUtility.UrlEncode(pDLAccountData.ID)}&DLPassword={HttpUtility.UrlEncode(pDLAccountData.PW)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0xea60, "UTF-8", true);
                str = pResponsetext;
            }
            catch
            {
            }
            return str;
        }

        public static string AddPTUserRow(ConfigurationStatus.SCAccountData pAccountData)
        {
            string str = "";
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=addPTUser";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string diskVolumeSerialNumber = CommFunc.GetDiskVolumeSerialNumber();
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.PTID)}&MachineCode={HttpUtility.UrlEncode(diskVolumeSerialNumber)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&PTName={HttpUtility.UrlEncode(pAccountData.PTName)}&Audit={HttpUtility.UrlEncode(pAccountData.PTLoginAudit)}&Hint={HttpUtility.UrlEncode(pAccountData.PTLoginHint)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
                str = pResponsetext;
            }
            catch
            {
            }
            return str;
        }

        public static bool AddSharePlanRow(ConfigurationStatus.SCPlan plan)
        {
            bool flag = false;
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=addSharePlan";
                string pResponsetext = "";
                string pData = $"UserID={HttpUtility.UrlEncode(AppInfo.Account.SendUserID)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}&PTID={HttpUtility.UrlEncode(AppInfo.PTInfo.PTID)}&Lottery={HttpUtility.UrlEncode(plan.LotteryID)}&FNCHType={HttpUtility.UrlEncode(plan.FNCHType)}&FNPlay={HttpUtility.UrlEncode(plan.Play)}&Expect={HttpUtility.UrlEncode(plan.CurrentExpect)}&Value={HttpUtility.UrlEncode(plan.SharePlanListData)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                flag = CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return flag;
        }

        public static bool AddSharePlanStateRow(ConfigurationStatus.AutoBets pBets, int pSendCount, bool pIsBetsYes)
        {
            bool flag = false;
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=addSharePlanState";
                string pResponsetext = "";
                string pData = $"UserID={HttpUtility.UrlEncode(AppInfo.Account.SendUserID)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}&ShareCode={HttpUtility.UrlEncode(ConfigurationStatus.ShareBets.GetEncodeShareCode(""))}&PTID={HttpUtility.UrlEncode(AppInfo.PTInfo.PTID)}&Lottery={HttpUtility.UrlEncode(pBets.LotteryID)}&Expect={HttpUtility.UrlEncode(pBets.Expect)}&PlanCount={pSendCount}&IsBetsYes={pIsBetsYes}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                flag = CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return flag;
        }

        public static bool AddShareSchemeRow(string pLottery, string pName, string pCHType, string playName, string pValue)
        {
            bool flag = false;
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=addShareScheme";
                string pResponsetext = "";
                string pData = $"UserID={HttpUtility.UrlEncode(AppInfo.Account.SendUserID)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}&PTID={HttpUtility.UrlEncode(AppInfo.PTInfo.PTID)}&Lottery={HttpUtility.UrlEncode(pLottery)}&FNName={HttpUtility.UrlEncode(pName)}&FNCHType={HttpUtility.UrlEncode(pCHType)}&FNPlay={HttpUtility.UrlEncode(playName)}&Value={HttpUtility.UrlEncode(pValue)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                flag = CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return flag;
        }

        public static bool AddShareSchemeStateRow(int pSchemeCount, int pBTFNCount)
        {
            bool flag = false;
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=addShareSchemeState";
                string pResponsetext = "";
                string pData = $"UserID={HttpUtility.UrlEncode(AppInfo.Account.SendUserID)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}&ShareCode={HttpUtility.UrlEncode(ConfigurationStatus.ShareBets.GetEncodeShareCode(""))}&PTID={HttpUtility.UrlEncode(AppInfo.PTInfo.PTID)}&Lottery={HttpUtility.UrlEncode(AppInfo.Current.Lottery.GroupString)}&SchemeCount={pSchemeCount}&BTFNCount={pBTFNCount}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                flag = CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return flag;
        }

        public static string AddUserRow(ConfigurationStatus.SCAccountData pAccountData)
        {
            string str = "";
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=addUser";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.ID)}&Password={HttpUtility.UrlEncode(pAccountData.PW)}&MachineCode={HttpUtility.UrlEncode(pAccountData.MachineCode)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&QQ={HttpUtility.UrlEncode(pAccountData.QQ)}&Phone={HttpUtility.UrlEncode(pAccountData.Phone)}&Type={HttpUtility.UrlEncode(pAccountData.TypeString)}&ActiveTime={HttpUtility.UrlEncode(pAccountData.ActiveTime)}&LastTime={HttpUtility.UrlEncode(pAccountData.LastTime)}&LastIP={HttpUtility.UrlEncode(pAccountData.LastIP)}&PTUser={HttpUtility.UrlEncode(pAccountData.PTUser)}&FreeDay={pAccountData.Configuration.FreeDay}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
                str = pResponsetext;
            }
            catch
            {
            }
            return str;
        }

        public static string CheckUserState(string pID)
        {
            string str = "";
            try
            {
                string pUrl = $"{ServerSqlUrl}?action=checkUserState&UserID={HttpUtility.UrlEncode(pID)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}";
                string serverUrl = ServerUrl;
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, serverUrl, 0x2710, "UTF-8", true);
                switch (pResponsetext)
                {
                    case "":
                    case "-1":
                        return str;
                }
                if (pResponsetext.Contains("错误"))
                {
                    str = pResponsetext.Split(new char[] { '-' })[1];
                }
            }
            catch
            {
            }
            return str;
        }

        public static string CZUser(string pUserID, string password, string pAppName, string pUsedTime, string pUsedIP)
        {
            string str = "";
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=cZUser";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UsedID={HttpUtility.UrlEncode(pUserID)}&Password={HttpUtility.UrlEncode(password)}&AppName={HttpUtility.UrlEncode(pAppName)}&UsedTime={HttpUtility.UrlEncode(pUsedTime)}&UsedIP={HttpUtility.UrlEncode(pUsedIP)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
                str = pResponsetext;
            }
            catch
            {
            }
            return str;
        }

        public static bool DeleteCZState(string pUserID, string pAppName, string pUsedTime)
        {
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=delCZState";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UsedID={HttpUtility.UrlEncode(pUserID)}&AppName={HttpUtility.UrlEncode(pAppName)}&UsedTime={HttpUtility.UrlEncode(pUsedTime)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
                return CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return false;
        }

        public static bool DeleteDLUserRow(ConfigurationStatus.SCAccountData pAccountData, ConfigurationStatus.SCAccountData pDLAccountData)
        {
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string cServerUrl = AppInfo.cServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=delDLUser";
                string pData = $"AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&DLUserID={HttpUtility.UrlEncode(pDLAccountData.ID)}&DLPassword={HttpUtility.UrlEncode(pDLAccountData.PW)}&Mkey={mdkey}";
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, cServerUrl, 0x2710, "UTF-8", true);
                return CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return false;
        }

        public static bool DeletePTUserRow(ConfigurationStatus.SCAccountData pAccountData)
        {
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string cServerUrl = AppInfo.cServerUrl;
                string pUrl = $"{AppInfo.cServerSqlUrl}?action=delPTUser&UserID={HttpUtility.UrlEncode(pAccountData.PTID)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&PTName={HttpUtility.UrlEncode(pAccountData.PTName)}&Mkey={mdkey}";
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, cServerUrl, 0x2710, "UTF-8", true);
                return CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return false;
        }

        public static bool DeleteUserRow(ConfigurationStatus.SCAccountData pAccountData, ConfigurationStatus.SCAccountData pDLAccountData)
        {
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string cServerUrl = AppInfo.cServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=delUser";
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.ID)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&DLUserID={HttpUtility.UrlEncode(pDLAccountData.ID)}&DLPassword={HttpUtility.UrlEncode(pDLAccountData.PW)}&Mkey={mdkey}";
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, cServerUrl, 0x2710, "UTF-8", true);
                return CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return false;
        }

        public static bool DelShareSchemeRow()
        {
            bool flag = false;
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=delShareSchemeRow";
                string pResponsetext = "";
                string pData = $"UserID={HttpUtility.UrlEncode(AppInfo.Account.SendUserID)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}&Lottery={HttpUtility.UrlEncode(AppInfo.Current.Lottery.GroupString)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                flag = pResponsetext != "";
            }
            catch
            {
            }
            return flag;
        }

        public static bool DLUserLogin(string pID, string pW, ConfigurationStatus.SCAccountData pAccountData, ref string pHint)
        {
            try
            {
                pW = CommFunc.Encode(pW, "e8we8w8e");
                string pUrl = $"{ServerSqlUrl}?action=DLLogin&UserID={HttpUtility.UrlEncode(pID)}&Password={HttpUtility.UrlEncode(pW)}";
                string serverUrl = ServerUrl;
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, serverUrl, 0x4e20, "UTF-8", true);
                switch (pResponsetext)
                {
                    case "":
                    case "-1":
                        return false;
                }
                if (pResponsetext.Contains("错误"))
                {
                    pHint = pResponsetext.Split(new char[] { '-' })[1];
                    return false;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(pResponsetext);
                XmlNode node = document.SelectNodes("Account/Item")[0];
                pAccountData.ID = node.SelectSingleNode("UserID").InnerText;
                pAccountData.PW = node.SelectSingleNode("Password").InnerText;
                pAccountData.AppName = node.SelectSingleNode("AppName").InnerText;
                pAccountData.ActiveTime = node.SelectSingleNode("ActiveTime").InnerText;
                pAccountData.AllowDK = Convert.ToBoolean(node.SelectSingleNode("AllowDK").InnerText);
                pAccountData.AllowCZ = Convert.ToBoolean(node.SelectSingleNode("AllowCZ").InnerText);
                pAccountData.AllowDelete = Convert.ToBoolean(node.SelectSingleNode("AllowDelete").InnerText);
                pAccountData.AllowClear = Convert.ToBoolean(node.SelectSingleNode("AllowClear").InnerText);
                pHint = $"登录成功！当前版本为【{pAccountData.TypeString}】，有效期为【{pAccountData.ActiveTime}】";
                return true;
            }
            catch
            {
            }
            return false;
        }

        public static bool EditUserRow(ConfigurationStatus.SCAccountData pAccountData)
        {
            bool flag = false;
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=editUser";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.ID)}&Password={HttpUtility.UrlEncode(pAccountData.PW)}&MachineCode={HttpUtility.UrlEncode(pAccountData.MachineCode)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
                flag = CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return flag;
        }

        public static List<ConfigurationStatus.SCAccountData> GetAllDLUserList(string pAppName, ConfigurationStatus.SCAccountData pDLAccountData)
        {
            List<ConfigurationStatus.SCAccountData> list = new List<ConfigurationStatus.SCAccountData>();
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string cServerUrl = AppInfo.cServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=getallDLUser";
                string pData = $"AppName={HttpUtility.UrlEncode(pAppName)}&DLUserID={HttpUtility.UrlEncode(pDLAccountData.ID)}&DLPassword={HttpUtility.UrlEncode(pDLAccountData.PW)}&Mkey={mdkey}";
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, cServerUrl, 0x2710, "UTF-8", true);
                switch (pResponsetext)
                {
                    case "":
                    case "-1":
                        return list;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(pResponsetext);
                XmlNodeList list2 = document.SelectNodes("Account/Item");
                for (int i = 0; i < list2.Count; i++)
                {
                    ConfigurationStatus.SCAccountData item = new ConfigurationStatus.SCAccountData();
                    XmlNode node = list2[i];
                    item.ID = node.SelectSingleNode("UserID").InnerText;
                    item.PW = node.SelectSingleNode("Password").InnerText;
                    item.AppName = node.SelectSingleNode("AppName").InnerText;
                    item.ActiveTime = node.SelectSingleNode("ActiveTime").InnerText;
                    item.AllowCZ = Convert.ToBoolean(node.SelectSingleNode("AllowCZ").InnerText);
                    item.AllowDelete = Convert.ToBoolean(node.SelectSingleNode("AllowDelete").InnerText);
                    item.AllowClear = Convert.ToBoolean(node.SelectSingleNode("AllowClear").InnerText);
                    item.AllowDK = Convert.ToBoolean(node.SelectSingleNode("AllowDK").InnerText);
                    item.IsStop = Convert.ToBoolean(node.SelectSingleNode("Stop").InnerText);
                    item.ConfigurationString = node.SelectSingleNode("Configuration").InnerText;
                    item.Configuration.DLConfiguration = item.ConfigurationString;
                    list.Add(item);
                }
            }
            catch
            {
            }
            return list;
        }

        public static List<ConfigurationStatus.SCAccountData> GetAllPTUserList(string pAppName)
        {
            List<ConfigurationStatus.SCAccountData> list = new List<ConfigurationStatus.SCAccountData>();
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string cServerUrl = AppInfo.cServerUrl;
                string pUrl = $"{AppInfo.cServerSqlUrl}?action=getallPTUser&AppName={HttpUtility.UrlEncode(pAppName)}&Mkey={mdkey}";
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, cServerUrl, 0x2710, "UTF-8", true);
                switch (pResponsetext)
                {
                    case "":
                    case "-1":
                        return list;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(pResponsetext);
                XmlNodeList list2 = document.SelectNodes("Account/Item");
                for (int i = 0; i < list2.Count; i++)
                {
                    ConfigurationStatus.SCAccountData item = new ConfigurationStatus.SCAccountData();
                    XmlNode node = list2[i];
                    item.ID = node.SelectSingleNode("UserID").InnerText;
                    item.AppName = node.SelectSingleNode("AppName").InnerText;
                    item.PTName = node.SelectSingleNode("PTName").InnerText;
                    item.MachineCode = node.SelectSingleNode("MachineCode").InnerText;
                    item.PTLoginAudit = node.SelectSingleNode("Audit").InnerText;
                    item.PTLoginHint = node.SelectSingleNode("Hint").InnerText;
                    item.OnLineTime = node.SelectSingleNode("OnLineTime").InnerText;
                    list.Add(item);
                }
            }
            catch
            {
            }
            return list;
        }

        public static List<ConfigurationStatus.SCAccountData> GetAllUserList(string pAppName, string pID, string pW)
        {
            List<ConfigurationStatus.SCAccountData> list = new List<ConfigurationStatus.SCAccountData>();
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string cServerUrl = AppInfo.cServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=getallUser";
                string pData = $"AppName={HttpUtility.UrlEncode(pAppName)}&DLUserID={HttpUtility.UrlEncode(pID)}&DLPassword={HttpUtility.UrlEncode(pW)}&Mkey={mdkey}";
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, cServerUrl, 0x4e20, "UTF-8", true);
                switch (pResponsetext)
                {
                    case "":
                    case "-1":
                        return list;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(pResponsetext);
                XmlNodeList list2 = document.SelectNodes("Account/Item");
                for (int i = 0; i < list2.Count; i++)
                {
                    ConfigurationStatus.SCAccountData item = new ConfigurationStatus.SCAccountData();
                    XmlNode node = list2[i];
                    item.ID = node.SelectSingleNode("UserID").InnerText;
                    item.PW = node.SelectSingleNode("Password").InnerText;
                    item.MachineCode = node.SelectSingleNode("MachineCode").InnerText;
                    item.AppName = node.SelectSingleNode("AppName").InnerText;
                    item.QQ = node.SelectSingleNode("QQ").InnerText;
                    item.Phone = node.SelectSingleNode("Phone").InnerText;
                    item.TypeString = node.SelectSingleNode("Type").InnerText;
                    item.ActiveTime = node.SelectSingleNode("ActiveTime").InnerText;
                    item.IsStop = Convert.ToBoolean(node.SelectSingleNode("Stop").InnerText);
                    item.OnLineTime = node.SelectSingleNode("OnLineTime").InnerText;
                    item.State = CommFunc.Decode(node.SelectSingleNode("State").InnerText, item.AppName);
                    item.PTUser = node.SelectSingleNode("PTUser").InnerText;
                    item.Remark = node.SelectSingleNode("Remark").InnerText;
                    item.AnalysisState(item.State);
                    item.LastTime = node.SelectSingleNode("LastTime").InnerText;
                    item.LastIP = node.SelectSingleNode("LastIP").InnerText;
                    list.Add(item);
                }
            }
            catch
            {
            }
            return list;
        }

        public static bool GetDLUser(string pAppName, ConfigurationStatus.SCAccountData pAccountData, ref string pHint)
        {
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string pUrl = $"{AppInfo.cServerSqlUrl}?action=getDLUser&AppName={pAppName}&Mkey={mdkey}";
                string serverUrl = ServerUrl;
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, serverUrl, 0x4e20, "UTF-8", true);
                switch (pResponsetext)
                {
                    case "":
                    case "-1":
                        return false;
                }
                if (pResponsetext.Contains("错误"))
                {
                    pHint = pResponsetext.Split(new char[] { '-' })[1];
                    return false;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(pResponsetext);
                XmlNode node = document.SelectNodes("Account/Item")[0];
                pAccountData.ConfigurationString = node.SelectSingleNode("Configuration").InnerText;
                pAccountData.GGImageString = node.SelectSingleNode("GGImage").InnerText;
                return true;
            }
            catch
            {
            }
            return false;
        }

        public static string GetFollowUser()
        {
            string str = "";
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=getFollowUser";
                string pResponsetext = "";
                string pData = $"UserID={HttpUtility.UrlEncode(AppInfo.Account.SendUserID)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                str = pResponsetext;
            }
            catch
            {
            }
            return str;
        }

        public static string GetFormText()
        {
            string serverUrl = ServerUrl;
            string pUrl = string.Format($"{ServerUrl}/FormText.txt", new object[0]);
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, serverUrl, 0x2710, "UTF-8", true);
            return pResponsetext;
        }

        public static string GetRandomNum()
        {
            string str = "0";
            string serverUrl = ServerUrl;
            string pUrl = string.Format($"{ServerSqlUrl}?action=getRandom", new object[0]);
            string pResponsetext = "";
            HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, serverUrl, 0x2710, "UTF-8", true);
            if (CommFunc.CheckIsNumber(pResponsetext))
            {
                str = pResponsetext;
            }
            return str;
        }

        public static bool GetSharePlan(ConfigurationStatus.AutoBets pBets, ref int pCount, ref string pHint)
        {
            bool flag = false;
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=getSharePlan";
                string pResponsetext = "";
                string pData = $"UserID={HttpUtility.UrlEncode(pBets.ShareBetsInfo.ShareUser)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}&ShareCode={HttpUtility.UrlEncode(pBets.ShareBetsInfo.ShareCode)}&PTID={HttpUtility.UrlEncode(AppInfo.PTInfo.PTID)}&Lottery={HttpUtility.UrlEncode(pBets.LotteryID)}&Expect={HttpUtility.UrlEncode(pBets.Expect)}&FollowUser={HttpUtility.UrlEncode(AppInfo.Account.SendUserID)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                if (pResponsetext == "")
                {
                    return flag;
                }
                if (pResponsetext.Contains("【错误】-"))
                {
                    pHint = pResponsetext.Replace("【错误】-", "");
                    return flag;
                }
                string[] strArray = pResponsetext.Split(new char[] { '-' });
                pCount = Convert.ToInt32(strArray[0]);
                pBets.ShareBetsInfo.FollowYes = Convert.ToBoolean(strArray[1]);
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        public static bool GetSharePlanList(ConfigurationStatus.AutoBets pBets, int pIndex, ref ConfigurationStatus.SCPlan plan, ref string pHint)
        {
            bool flag = false;
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=getSharePlanList";
                string pResponsetext = "";
                string pData = $"UserID={HttpUtility.UrlEncode(pBets.ShareBetsInfo.ShareUser)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}&Index={pIndex}&PTID={HttpUtility.UrlEncode(AppInfo.PTInfo.PTID)}&Lottery={HttpUtility.UrlEncode(pBets.LotteryID)}&Expect={HttpUtility.UrlEncode(pBets.Expect)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                if (pResponsetext == "")
                {
                    return flag;
                }
                if (pResponsetext.Contains("【错误】-"))
                {
                    pHint = pResponsetext.Replace("【错误】-", "");
                    return flag;
                }
                plan = new ConfigurationStatus.SCPlan();
                XmlDocument document = new XmlDocument();
                document.LoadXml(pResponsetext);
                XmlNode node = document.SelectNodes("Plan/Item")[0];
                plan.SharePlanListData = node.SelectSingleNode("Value").InnerText;
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        public static bool GetShareSchemeCount(string pShareUser, string pShareCode, ref string pHint, ref int pSchemeCount, ref int pBTFNCount)
        {
            bool flag = false;
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=getShareSchemeCount";
                string pResponsetext = "";
                string pData = $"UserID={HttpUtility.UrlEncode(pShareUser)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}&ShareCode={HttpUtility.UrlEncode(pShareCode)}&PTID={HttpUtility.UrlEncode(AppInfo.PTInfo.PTID)}&Lottery={HttpUtility.UrlEncode(AppInfo.Current.Lottery.GroupString)}&FollowUser={HttpUtility.UrlEncode(AppInfo.Account.SendUserID)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                if (pResponsetext != "")
                {
                    if (pResponsetext.Contains("【错误】-"))
                    {
                        pHint = pResponsetext.Replace("【错误】-", "");
                    }
                    else
                    {
                        string[] strArray = pResponsetext.Split(new char[] { ';' });
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            string[] strArray2 = strArray[i].Split(new char[] { '-' });
                            string str6 = strArray2[0];
                            int num2 = Convert.ToInt32(strArray2[1]);
                            if (str6 == AppInfo.Current.Lottery.GroupString)
                            {
                                pSchemeCount = num2;
                            }
                            else if (str6 == "BTFN")
                            {
                                pBTFNCount = num2;
                            }
                        }
                        flag = true;
                    }
                }
            }
            catch
            {
            }
            if (!(flag || (pHint != "")))
            {
                pHint = "没有获取到上级共享的方案个数！";
            }
            return flag;
        }

        public static bool GetShareSchemeList(string pLottery, string pShareUser, int pIndex, ref string pHint, ref string pResponseText)
        {
            bool flag = false;
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=getShareSchemeList";
                string pResponsetext = "";
                string pData = $"UserID={HttpUtility.UrlEncode(pShareUser)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}&Index={pIndex}&PTID={HttpUtility.UrlEncode(AppInfo.PTInfo.PTID)}&Lottery={HttpUtility.UrlEncode(pLottery)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                if (pResponsetext != "")
                {
                    if (pResponsetext.Contains("【错误】-"))
                    {
                        pHint = pResponsetext.Replace("【错误】-", "");
                    }
                    else
                    {
                        pResponseText = pResponsetext;
                        flag = true;
                    }
                }
            }
            catch
            {
            }
            if (!(flag || (pHint != "")))
            {
                pHint = "下载上级共享的方案失败！";
            }
            return flag;
        }

        public static bool PTUserLogin(ConfigurationStatus.SCAccountData pAccountData, ref string pHint)
        {
            try
            {
                string pUrl = $"{ServerSqlUrl}?action=PTLogin&UserID={HttpUtility.UrlEncode(pAccountData.PTID)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&PTName={HttpUtility.UrlEncode(pAccountData.PTName)}";
                string serverUrl = ServerUrl;
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, serverUrl, 0x4e20, "UTF-8", true);
                switch (pResponsetext)
                {
                    case "":
                    case "-1":
                        return false;
                }
                if (pResponsetext.Contains("错误"))
                {
                    pHint = pResponsetext.Split(new char[] { '-' })[1];
                    return false;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(pResponsetext);
                XmlNode node = document.SelectNodes("Account/Item")[0];
                pAccountData.MachineCode = node.SelectSingleNode("MachineCode").InnerText;
                pAccountData.PTLoginAudit = node.SelectSingleNode("Audit").InnerText;
                pAccountData.PTLoginHint = node.SelectSingleNode("Hint").InnerText;
                return true;
            }
            catch
            {
            }
            return false;
        }

        public static bool SaveFNEncrypt(ConfigurationStatus.SCAccountData pDLAccountData)
        {
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string cServerUrl = AppInfo.cServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=saveFNEncrypt";
                string str = "";
                if (pDLAccountData.Configuration.FNEncrypIDList.Count > 0)
                {
                    string fNEncrypt = pDLAccountData.Configuration.FNEncrypt;
                    if (fNEncrypt == "")
                    {
                        fNEncrypt = pDLAccountData.AppName.PadRight(8, '6');
                    }
                    str = fNEncrypt + "-" + CommFunc.Join(pDLAccountData.Configuration.FNEncrypIDList, ";");
                }
                string pData = $"FNEncryptID={HttpUtility.UrlEncode(str)}&AppName={HttpUtility.UrlEncode(pDLAccountData.AppName)}&DLUserID={HttpUtility.UrlEncode(pDLAccountData.ID)}&DLPassword={HttpUtility.UrlEncode(pDLAccountData.PW)}&Mkey={mdkey}";
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, cServerUrl, 0x2710, "UTF-8", true);
                return CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return false;
        }

        public static bool UpdataDLUserRow(ConfigurationStatus.SCAccountData pAccountData)
        {
            bool flag = false;
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=updateDLUser";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.ID)}&Password={HttpUtility.UrlEncode(pAccountData.PW)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&AllowCZ={HttpUtility.UrlEncode(pAccountData.AllowCZ.ToString())}&AllowDelete={HttpUtility.UrlEncode(pAccountData.AllowDelete.ToString())}&AllowClear={HttpUtility.UrlEncode(pAccountData.AllowClear.ToString())}&AllowDK={HttpUtility.UrlEncode(pAccountData.AllowDK.ToString())}&Configuration={HttpUtility.UrlEncode(pAccountData.ConfigurationString)}&GGImage={HttpUtility.UrlEncode(pAccountData.GGImageString)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0xea60, "UTF-8", true);
                flag = CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return flag;
        }

        public static void UpdataPTUserOnLine(ConfigurationStatus.SCAccountData pAccountData)
        {
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=updatePTUserOnLine";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.PTID)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&PTName={HttpUtility.UrlEncode(pAccountData.PTName)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
            }
            catch
            {
            }
        }

        public static void UpdataPTUserRow(ConfigurationStatus.SCAccountData pAccountData)
        {
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=updatePTUserAudit";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.ID)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&PTName={HttpUtility.UrlEncode(pAccountData.PTName)}&Audit={HttpUtility.UrlEncode(pAccountData.PTLoginAudit)}&Hint={HttpUtility.UrlEncode(pAccountData.PTLoginHint)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
            }
            catch
            {
            }
        }

        public static void UpdataUserOnLine(ConfigurationStatus.SCAccountData pAccountData)
        {
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=updateUserOnLine";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.ID)}&MachineCode={HttpUtility.UrlEncode(pAccountData.MachineCode)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
            }
            catch
            {
            }
        }

        public static void UpdataUserRow(ConfigurationStatus.SCAccountData pAccountData)
        {
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=updateUser";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.ID)}&Password={HttpUtility.UrlEncode(pAccountData.PW)}&MachineCode={HttpUtility.UrlEncode(pAccountData.MachineCode)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&LastTime={HttpUtility.UrlEncode(pAccountData.LastTime)}&LastIP={HttpUtility.UrlEncode(pAccountData.LastIP)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
            }
            catch
            {
            }
        }

        public static bool UpdateDLUserStop(ConfigurationStatus.SCAccountData pAccountData)
        {
            bool flag = false;
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=updateDLUserStop";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&Stop={HttpUtility.UrlEncode(pAccountData.IsStop.ToString())}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
                flag = CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return flag;
        }

        public static bool UpdateFollowUser(string pFollowUser)
        {
            bool flag = false;
            try
            {
                string mdkey = CommFunc.GetMdkey("");
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=updateFollowUser";
                string pResponsetext = "";
                string pData = $"UserID={HttpUtility.UrlEncode(AppInfo.Account.SendUserID)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}&FollowUser={HttpUtility.UrlEncode(pFollowUser)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, AppInfo.PTInfo.BetsTime1, "UTF-8", true);
                flag = CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return flag;
        }

        public static bool UpdateMachineCode(string pID, string pW, string pAppEnName, string pMachineCodea)
        {
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=updateMachineCode";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pID)}&Password={HttpUtility.UrlEncode(pW)}&MachineCode={HttpUtility.UrlEncode(pMachineCodea)}&AppName={HttpUtility.UrlEncode(pAppEnName)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
                return CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return false;
        }

        public static bool UpdatePTuser(ConfigurationStatus.SCAccountData pAccountData)
        {
            bool flag = false;
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=updatePTUser";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.ID)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&PTUser={HttpUtility.UrlEncode(pAccountData.PTUser)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
                flag = CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return flag;
        }

        public static bool UpdateUserRemark(ConfigurationStatus.SCAccountData pAccountData)
        {
            bool flag = false;
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=updateUserRemark";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.ID)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&Remark={HttpUtility.UrlEncode(pAccountData.Remark)}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
                flag = CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return flag;
        }

        public static bool UpdateUserStop(ConfigurationStatus.SCAccountData pAccountData)
        {
            bool flag = false;
            try
            {
                string serverUrl = ServerUrl;
                string pUrl = $"{ServerSqlUrl}?action=updateUserStop";
                string pResponsetext = "";
                string mdkey = CommFunc.GetMdkey("");
                string pData = $"UserID={HttpUtility.UrlEncode(pAccountData.ID)}&MachineCode={HttpUtility.UrlEncode(pAccountData.MachineCode)}&AppName={HttpUtility.UrlEncode(pAccountData.AppName)}&Stop={HttpUtility.UrlEncode(pAccountData.IsStop.ToString())}&Mkey={mdkey}";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "POST", pData, serverUrl, 0x2710, "UTF-8", true);
                flag = CommFunc.CheckResponseText(pResponsetext);
            }
            catch
            {
            }
            return flag;
        }

        public static bool UserLogin(string pID, string pW, string pMachineCode, ConfigurationStatus.SCAccountData pAccountData, ref string pHint)
        {
            try
            {
                pW = CommFunc.Encode(pW, "e8we8w8e");
                string pUrl = $"{ServerSqlUrl}?action=Login&UserID={HttpUtility.UrlEncode(pID)}&Password={HttpUtility.UrlEncode(pW)}&MachineCode={HttpUtility.UrlEncode(pMachineCode)}&AppName={HttpUtility.UrlEncode(AppInfo.Account.AppName)}";
                string serverUrl = ServerUrl;
                string pResponsetext = "";
                HttpHelper.GetResponse(ref pResponsetext, pUrl, "GET", string.Empty, serverUrl, 0x2710, "UTF-8", true);
                switch (pResponsetext)
                {
                    case "":
                    case "-1":
                        return false;
                }
                if (pResponsetext.Contains("错误"))
                {
                    pHint = pResponsetext.Split(new char[] { '-' })[1];
                    return false;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(pResponsetext);
                XmlNode node = document.SelectNodes("Account/Item")[0];
                pAccountData.ID = node.SelectSingleNode("UserID").InnerText;
                pAccountData.PW = node.SelectSingleNode("Password").InnerText;
                pAccountData.MachineCode = node.SelectSingleNode("MachineCode").InnerText;
                pAccountData.AppName = node.SelectSingleNode("AppName").InnerText;
                pAccountData.QQ = node.SelectSingleNode("QQ").InnerText;
                pAccountData.Phone = node.SelectSingleNode("Phone").InnerText;
                pAccountData.TypeString = node.SelectSingleNode("Type").InnerText;
                pAccountData.ActiveTime = node.SelectSingleNode("ActiveTime").InnerText;
                pAccountData.State = node.SelectSingleNode("State").InnerText;
                pAccountData.LastTime = node.SelectSingleNode("LastTime").InnerText;
                pAccountData.LastIP = node.SelectSingleNode("LastIP").InnerText;
                pAccountData.PTUser = node.SelectSingleNode("PTUser").InnerText;
                pAccountData.Remark = node.SelectSingleNode("Remark").InnerText;
                pHint = $"登录成功！当前版本为【{pAccountData.TypeString}】，有效期为【{pAccountData.ActiveTime}】";
                return true;
            }
            catch
            {
            }
            return false;
        }
    }
}

