namespace IntelligentPlanning.WorkThread
{
    using IntelligentPlanning;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal class MainThread
    {
        public Dictionary<string, Thread> BetsThreadDic = new Dictionary<string, Thread>();
        public Thread checkPTLineThread;
        public Thread cyclicThread;
        public DownData downCode;
        public Thread loadConfigurationThread;
        public Thread loadThread;
        public bool openPlanSingle = false;
        public bool openSingle = false;
        public string PTName = "";
        public Thread refreshLSDataThread;
        public Thread refreshTJDataThread;
        public bool StopIt = false;
        private List<Thread> ThreadList = new List<Thread>();
        public Thread verifyCodeThread;
        public Thread webDataThread;

        public MainThread(string pID, PTBase pInfo)
        {
            this.PTName = pInfo.PTName;
            ConfigurationStatus.LotteryConfig config = AppInfo.Current.LotteryDic[pID];
            this.downCode = new DownData(this, config.Type, pInfo, config.RefreshExpect, config.SaveExpect);
            this.loadThread = new Thread(new ParameterizedThreadStart(this.LoadThread));
            this.loadThread.IsBackground = true;
            this.loadThread.Start(true);
            this.cyclicThread = new Thread(new ThreadStart(this.CyclicThread));
            this.cyclicThread.IsBackground = true;
            this.cyclicThread.Start();
            this.verifyCodeThread = new Thread(new ThreadStart(this.VerifyCodeThread));
            this.verifyCodeThread.IsBackground = true;
            this.verifyCodeThread.Start();
            this.checkPTLineThread = new Thread(new ThreadStart(this.CheckPTLineThread));
            this.checkPTLineThread.IsBackground = true;
            this.checkPTLineThread.Start();
            this.webDataThread = new Thread(new ThreadStart(this.WebDataThread));
            this.webDataThread.IsBackground = true;
            this.webDataThread.Start();
            this.ThreadList.Add(this.loadThread);
            this.ThreadList.Add(this.cyclicThread);
            this.ThreadList.Add(this.verifyCodeThread);
            this.ThreadList.Add(this.checkPTLineThread);
            this.ThreadList.Add(this.webDataThread);
            this.ThreadList.Add(this.loadConfigurationThread);
            this.ThreadList.Add(this.refreshLSDataThread);
            this.ThreadList.Add(this.refreshTJDataThread);
        }

        public void CheckPTLineThread()
        {
            if (AppInfo.CheckPTLine != null)
            {
                while (!this.StopIt)
                {
                    AppInfo.CheckPTLine();
                    Thread.Sleep(AppInfo.cRefreshTime);
                }
            }
        }

        public void CloseThreadList()
        {
            this.StopIt = true;
            if (AppInfo.PTVerifyCodeDic.ContainsKey(this.PTName))
            {
                Dictionary<string, int> dictionary;
                string str2;
                (dictionary = AppInfo.PTVerifyCodeDic)[str2 = this.PTName] = dictionary[str2] - 1;
                if (AppInfo.PTVerifyCodeDic[this.PTName] == 0)
                {
                    AppInfo.PTVerifyCodeDic.Remove(this.PTName);
                }
            }
            for (int i = 0; i < this.ThreadList.Count; i++)
            {
                if (this.ThreadList[i] != null)
                {
                    this.ThreadList[i].Abort();
                }
            }
            foreach (string str in this.BetsThreadDic.Keys)
            {
                this.BetsThreadDic[str].Abort();
            }
        }

        public void CyclicThread()
        {
            while (!this.StopIt)
            {
                if (this.loadThread.ThreadState != ThreadState.Stopped)
                {
                    Thread.Sleep(0x3e8);
                }
                else
                {
                    try
                    {
                        if (this.SingleMode)
                        {
                            this.MainRefresh(true);
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(AppInfo.cRefreshTime);
                }
            }
        }

        public void LoadThread(object RefreshData)
        {
            this.MainRefresh(Convert.ToBoolean(RefreshData));
            this.openSingle = true;
        }

        private bool MainRefresh(bool RefreshData)
        {
            bool flag = true;
            try
            {
                if (!this.openSingle && (AppInfo.LoadStart != null))
                {
                    Program.MainApp.Invoke(AppInfo.LoadStart);
                }
                if (RefreshData)
                {
                    flag = this.downCode.Refresh();
                }
                if (this.openSingle)
                {
                    return flag;
                }
                if (AppInfo.LoadEnd != null)
                {
                    Program.MainApp.Invoke(AppInfo.LoadEnd);
                }
            }
            catch
            {
            }
            return flag;
        }

        public void MainVerifyCodeRefresh(string PTName, bool pRefresh)
        {
            try
            {
                if (AppInfo.AnalysisVerifyCode != null)
                {
                    int pCount = 0;
                    while (!AppInfo.AnalysisVerifyCode(PTName, ref pCount))
                    {
                        Thread.Sleep(0x3e8);
                    }
                }
            }
            catch
            {
            }
        }

        public void VerifyCodeThread()
        {
            if (AppInfo.AnalysisVerifyCode != null)
            {
                if (AppInfo.PTVerifyCodeDic.ContainsKey(this.PTName))
                {
                    Dictionary<string, int> dictionary;
                    string str;
                    (dictionary = AppInfo.PTVerifyCodeDic)[str = this.PTName] = dictionary[str] + 1;
                }
                else
                {
                    AppInfo.PTVerifyCodeDic[this.PTName] = 1;
                    while (!this.StopIt)
                    {
                        this.MainVerifyCodeRefresh(this.PTName, true);
                        Thread.Sleep(0x7d0);
                    }
                }
            }
        }

        public void WebDataThread()
        {
            if (AppInfo.WebData != null)
            {
                while (!this.StopIt)
                {
                    AppInfo.WebData();
                    Thread.Sleep(0x1d4c0);
                }
            }
        }

        public bool SingleMode
        {
            get
            {
                bool flag = true;
                return (flag && this.openSingle);
            }
        }
    }
}

