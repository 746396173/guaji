namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class DebugLog
    {
        public static void ClearDebug()
        {
            try
            {
                CommFunc.DeleteDirectory(DebugPath);
            }
            catch
            {
            }
        }

        public static void ClearLogList()
        {
            try
            {
                CommFunc.DeleteDirectory(LogListPath);
            }
            catch
            {
            }
        }

        public static void SaveDebug(Exception pException, string pExtraString = "")
        {
            try
            {
                if (AppInfo.App != ConfigurationStatus.AppType.OpenData)
                {
                    string debugPath = DebugPath;
                    CommFunc.CreateDirectory(debugPath);
                    string pStr = (pException != null) ? pException.ToString() : "";
                    if (pExtraString != "")
                    {
                        pStr = pStr + "\r\n\r\n" + pExtraString;
                    }
                    string[] strArray = pException.StackTrace.Split(new char[] { ' ' });
                    CommFunc.WriteTextFileToStr($"{debugPath}{DateTime.Now.ToString("HH时mm分ss秒")}-{strArray[strArray.Length - 1]}.txt", pStr);
                }
            }
            catch
            {
            }
        }

        public static void SaveDebug(string pExtraString, string pTitle)
        {
            try
            {
                if (AppInfo.App != ConfigurationStatus.AppType.OpenData)
                {
                    string debugPath = DebugPath;
                    CommFunc.CreateDirectory(debugPath);
                    string pStr = pExtraString;
                    CommFunc.WriteTextFileToStr($"{debugPath}{DateTime.Now.ToString("HH时mm分ss秒")}-{pTitle}.txt", pStr);
                }
            }
            catch
            {
            }
        }

        public static void SaveLogList(ConfigurationStatus.AutoBets pBets)
        {
            try
            {
                if (AppInfo.App != ConfigurationStatus.AppType.OpenData)
                {
                    string logListPath = LogListPath;
                    CommFunc.CreateDirectory(logListPath);
                    string str2 = (pBets.ErrorState != "") ? pBets.ErrorState : pBets.PTState;
                    string pFile = $"{logListPath}{pBets.Name}-{DateTime.Now.ToString("HH时mm分ss秒")}-{pBets.ErrorState}.txt";
                    List<string> currentState = pBets.GetCurrentState();
                    CommFunc.WriteTextFile(pFile, currentState);
                }
            }
            catch
            {
            }
        }

        public static string DebugPath =>
            (CommFunc.getDllPath() + @"\Debug\");

        public static string LogListPath =>
            (CommFunc.getDllPath() + @"\LogList\");
    }
}

