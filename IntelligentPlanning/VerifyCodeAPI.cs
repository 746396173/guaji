namespace IntelligentPlanning
{
    using MSScriptControl;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    internal class VerifyCodeAPI
    {
        public static Dictionary<string, int> OCRDic = new Dictionary<string, int>();

        public static int CheckVerifyCodeMain(string pName, string pFile)
        {
            int num = -1;
            for (int i = 1; i <= 0x2d; i++)
            {
                try
                {
                    if (GetVerifyCodeByOCR(pName, pFile, i).Length == 4)
                    {
                        num = i;
                    }
                }
                catch
                {
                }
            }
            return num;
        }

        [DllImport("OCR.dll")]
        public static extern int CNN_INIT(byte[] FileBuffer, int ImgBufLen, string pwd, int xcs);
        [DllImport("OCR.dll")]
        public static extern string CNN_OCR(int iIndex, byte[] FileBuffer, int ImgBufLen, int zxd);
        [DllImport("decode.dll")]
        public static extern string getCode(byte[] photo_byte);
        [DllImport("Sunday.dll")]
        public static extern bool GetCodeFromBuffer(int LibFileIndex, byte[] FileBuffer, int ImgBufLen, StringBuilder Code);
        [DllImport("Sunday.dll")]
        public static extern bool GetCodeFromFile(int LibFileIndex, string FilePath, StringBuilder Code);
        public static int GetIndexByOCR(string pName)
        {
            int num = -1;
            if ((pName == "SYYL") || (pName == "FNYX"))
            {
                return 2;
            }
            if (((((pName == "UT8") || (pName == "YYZX")) || ((pName == "HBS") || (pName == "JYGJ"))) || (((pName == "THDYL") || (pName == "HANY")) || (pName == "YHYL"))) || (pName == "YBYL"))
            {
                return 6;
            }
            if ((pName == "M5CP") || (pName == "DACP"))
            {
                return 7;
            }
            if ((pName == "LFYL") || (pName == "LF2"))
            {
                return 9;
            }
            if (pName == "HENR")
            {
                return 11;
            }
            if (((((((pName == "BNGJ") || (pName == "K5YL")) || ((pName == "A6YL") || (pName == "YIFA"))) || (((pName == "DPC") || (pName == "BHZY")) || ((pName == "HGDB") || (pName == "BMEI")))) || ((((pName == "XHDF") || (pName == "HSGJ")) || ((pName == "MYGJ") || (pName == "BWT"))) || (((pName == "CLYL") || (pName == "HUIZ")) || ((pName == "ALGJ") || (pName == "GJYL"))))) || ((((pName == "LSWJS") || (pName == "HCYL")) || ((pName == "XHSD") || (pName == "JXYL"))) || (pName == "BLGJ"))) || (pName == "ZBYL"))
            {
                return 12;
            }
            if ((((pName == "LXYL") || (pName == "HNYL")) || ((pName == "QFYL") || (pName == "WTYL"))) || (pName == "XQYL"))
            {
                return 15;
            }
            if ((pName == "WMYL") || (pName == "BHGJ"))
            {
                return 0x10;
            }
            if ((pName == "BAYL") || (pName == "SIJI"))
            {
                return 0x13;
            }
            if ((pName == "WJSJ") || (pName == "LFGJ"))
            {
                return 0x17;
            }
            if (pName == "JHYL")
            {
                return 0x19;
            }
            if (pName == "XCAI")
            {
                return 0x1c;
            }
            if (((((pName == "SKYYL") || (pName == "OEYL")) || ((pName == "MZC") || (pName == "WHC"))) || (pName == "LGZX")) || (pName == "SSHC"))
            {
                return 0x1d;
            }
            if (((((pName == "LUDI") || (pName == "SLTH")) || ((pName == "JFYL") || (pName == "JHC"))) || ((pName == "JHC2") || (pName == "WYYL"))) || (pName == "TCYL"))
            {
                return 0x1f;
            }
            if (((((pName == "WSYL") || (pName == "TAYL")) || ((pName == "XCYL") || (pName == "THEN"))) || (((pName == "LDYL") || (pName == "WHEN")) || ((pName == "KYYL") || (pName == "KXYL")))) || (pName == "TYYL"))
            {
                return 0x20;
            }
            if (pName == "NBAYL")
            {
                return 0x21;
            }
            if (pName == "YRYL")
            {
                return 0x23;
            }
            if (((((pName == "FCYL") || (pName == "LYS")) || ((pName == "KSYL") || (pName == "DQYL"))) || (pName == "DJYL")) || (pName == "XINC"))
            {
                return 0x24;
            }
            if (((((pName == "HLC") || (pName == "CTT")) || ((pName == "MINC") || (pName == "RDYL"))) || (pName == "FLC")) || (pName == "HOND"))
            {
                return 0x25;
            }
            if ((pName == "LMH") || (pName == "WCYL"))
            {
                return 0x26;
            }
            if (((((pName == "QQT2") || (pName == "WBJ")) || ((pName == "FEIC") || (pName == "ZLJ"))) || (pName == "XDB")) || (pName == "ZBEI"))
            {
                return 0x27;
            }
            if (pName == "THYL")
            {
                return 0x2b;
            }
            if ((pName == "BKC") || (pName == "HKC"))
            {
                return 0x2c;
            }
            if ((pName == "JCX") || (pName == "TBYL"))
            {
                num = 0x2d;
            }
            return num;
        }

        public static string GetIndexByOCRPlus(string pName)
        {
            string str = "";
            if (pName == "SSHC")
            {
                return "sshc";
            }
            if (pName == "JHC")
            {
                return "jhc";
            }
            if (pName == "HOND")
            {
                return "hond";
            }
            if ((((pName == "BLGJ") || (pName == "QQYL")) || ((pName == "DEJI") || (pName == "JLGJ"))) || (pName == "YCYL"))
            {
                return "blgj";
            }
            if ((pName == "XHHC") || (pName == "MCYL"))
            {
                return "xhhc";
            }
            if (pName == "YINH")
            {
                return "yinh";
            }
            if (pName == "B6YL")
            {
                return "b6yl";
            }
            if (pName == "TIYU")
            {
                return "tiyu";
            }
            if (pName == "ZBYL")
            {
                return "zbyl";
            }
            if (pName == "FNYX")
            {
                return "fnyx";
            }
            if (pName == "JXIN")
            {
                str = "jxin";
            }
            return str;
        }

        public static string GetJScript(string pText, string pName)
        {
            string code = CommFunc.ReadTextFileToStr((CommFunc.getDllPath() + @"\VerifyCode\Lib\") + pName + ".js");
            ScriptControl control = new ScriptControlClass {
                UseSafeSubset = true,
                Language = "JScript"
            };
            control.AddCode(code);
            return control.Eval(pText).ToString();
        }

        public static string GetJScript(string pMethod, List<string> pTextList, string pName)
        {
            string code = CommFunc.ReadTextFileToStr((CommFunc.getDllPath() + @"\VerifyCode\Lib\") + pName + ".js");
            ScriptControl control = new ScriptControlClass {
                UseSafeSubset = true,
                Language = "JScript"
            };
            control.AddCode(code);
            object[] objArray = new object[pTextList.Count];
            for (int i = 0; i < pTextList.Count; i++)
            {
                objArray[i] = pTextList[i];
            }
            object[] parameters = objArray;
            return control.Run(pMethod, ref parameters).ToString();
        }

        public static string GetJScript(string pMethod, string pText, string pName)
        {
            string code = CommFunc.ReadTextFileToStr((CommFunc.getDllPath() + @"\VerifyCode\Lib\") + pName + ".js");
            ScriptControl control = new ScriptControlClass {
                UseSafeSubset = true,
                Language = "JScript"
            };
            control.AddCode(code);
            object[] parameters = new object[] { pText };
            return control.Run(pMethod, ref parameters).ToString();
        }

        public static string GetJScriptByPT(string pText, string pName = "Common")
        {
            string code = CommFunc.ReadTextFileToStr((CommFunc.getDllPath() + @"\VerifyCode\Lib\") + pName + ".js");
            ScriptControl control = new ScriptControlClass {
                UseSafeSubset = true,
                Language = "JScript"
            };
            control.AddCode(code);
            pText = $"getpass('{pText}')";
            return control.Eval(pText).ToString();
        }

        [DllImport("CaptchaOCR.dll")]
        public static extern bool GetVcode(int Index, byte[] ImageBuffer, int ImgBufLen, StringBuilder Vcode);
        public static string GetVerifyCodeByDC(string pName, string pFile)
        {
            FileStream input = new FileStream(pFile, FileMode.Open, FileAccess.Read);
            byte[] buffer = new BinaryReader(input).ReadBytes((int) input.Length);
            return "GYYQS";
        }

        public static string GetVerifyCodeByLZ(string pFile, string pID, string pW)
        {
            Bitmap bmp = new Bitmap(pFile);
            return GetVerifyCodeByLZ(bmp, 0, 5, 5, pID, pW, "!cpfsy").Split(new char[] { '|' })[0];
        }

        public static string GetVerifyCodeByLZ(Bitmap bmp, int codeType, int codeMinLen, int codeMaxLen, string strVcodeUser, string strVcodePass, string strSoftkey)
        {
            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = new byte[stream.Length];
            stream.Position = 0L;
            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
            bmp.Dispose();
            return LianZhong.recCode(buffer, buffer.Length, codeType, codeMinLen, codeMaxLen, strVcodeUser, strVcodePass, strSoftkey);
        }

        public static string GetVerifyCodeByOCR(string pName, string pFile, int pIndex = -1)
        {
            StringBuilder vcode = new StringBuilder(0, 20);
            VcodeInit("DABADD13CE58C3EC923DC91E8DBC589C");
            FileStream stream = File.OpenRead(pFile);
            int length = (int) stream.Length;
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            stream.Close();
            int index = pIndex;
            if (index == -1)
            {
                index = GetIndexByOCR(pName);
            }
            GetVcode(index, buffer, buffer.Length, vcode);
            return vcode.ToString();
        }

        public static string GetVerifyCodeByOCRPlus(string pName, string pFile, int pIndex = -1)
        {
            int iIndex = -1;
            pName = GetIndexByOCRPlus(pName);
            if (OCRDic.ContainsKey(pName))
            {
                iIndex = OCRDic[pName];
            }
            else
            {
                iIndex = LCNN_INIT(@"VerifyCode\Lib\" + pName + ".cnn", "", 10);
                OCRDic[pName] = iIndex;
            }
            if (iIndex == -1)
            {
                return "";
            }
            FileStream stream = File.OpenRead(pFile);
            int length = (int) stream.Length;
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            stream.Close();
            return CNN_OCR(iIndex, buffer, buffer.Length, 0);
        }

        public static string GetVerifyCodeBySD(string pName, string pFile)
        {
            string str = "";
            StringBuilder code = new StringBuilder();
            FileStream stream = File.OpenRead(pFile);
            int length = (int) stream.Length;
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            stream.Close();
            if (GetCodeFromBuffer(LoadLibFromFile(@"VerifyCode\Lib\" + pName + ".lib", "123"), buffer, length, code))
            {
                str = code.ToString();
            }
            return str;
        }

        [DllImport("OCR.dll")]
        public static extern int LCNN_INIT(string filePath, string pwd, int xcs);
        [DllImport("OCR.dll")]
        public static extern string LCNN_OCR(int iIndex, string filePath, int zxd);
        [DllImport("Sunday.dll")]
        public static extern int LoadLibFromFile(string LibFilePath, string nSecret);
        [DllImport("urlmon.dll", EntryPoint="URLDownloadToFileA")]
        public static extern int URLDownloadToFile(int pCaller, string szURL, string szFileName, int dwReserved, int lpfnCB);
        [DllImport("CaptchaOCR.dll")]
        public static extern int VcodeInit(string PassWord);
        public static string VerifyCodeMain(string pName, string pFile)
        {
            if (pName == "AMBLR")
            {
                return GetVerifyCodeBySD(pName, pFile);
            }
            if ((((((pName == "SSHC") || (pName == "BLGJ")) || ((pName == "XHHC") || (pName == "MCYL"))) || (((pName == "QQYL") || (pName == "YINH")) || ((pName == "DEJI") || (pName == "JLGJ")))) || ((((pName == "B6YL") || (pName == "TIYU")) || ((pName == "YCYL") || (pName == "ZBYL"))) || (pName == "FNYX"))) || (pName == "JXIN"))
            {
                return GetVerifyCodeByOCRPlus(pName, pFile, -1);
            }
            return GetVerifyCodeByOCR(pName, pFile, -1);
        }
    }
}

