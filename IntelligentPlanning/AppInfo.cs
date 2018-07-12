namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;

    internal class AppInfo
    {
        public static A6YL A6YLInfo;
        public static ConfigurationStatus.SCAccountData Account;
        public static ALGJ ALGJInfo;
        public static AMBLR AMBLRInfo;
        public static ConfigurationStatus.AnalysisVerifyCodeDelegate AnalysisVerifyCode;
        public static ConfigurationStatus.AppType App = ConfigurationStatus.AppType.YXZXGJ;
        public static Icon AppIcon16 = Resources.YXZX48;
        public static Icon AppIcon32 = Resources.YXZX48;
        public static List<string> AppPTNameList;
        public static B6YL B6YLInfo;
        public static ConfigurationStatus.BankRefreshDelegate BankControlRefresh;
        public static ConfigurationStatus.BankRefreshDelegate BankRefresh;
        public static BAYL BAYLInfo;
        public static ConfigurationStatus.BetsMainDelegate BetsMain;
        public static ConfigurationStatus.BetsRefreshDelegate BetsRefresh;
        public static BHGJ BHGJInfo;
        public static BHZY BHZYInfo;
        public static BKC BKCInfo;
        public static Color blackColor = Color.Black;
        public static BLGJ BLGJInfo;
        public static Color blueBackColor = Color.SteelBlue;
        public static BMEI BMEIInfo;
        public static BMYX BMYXInfo;
        public static BNGJ BNGJInfo;
        public static Dictionary<string, ConfigurationStatus.GJBTScheme> BTFNDic;
        public static List<string> BTFNList;
        public static ConfigurationStatus.BTImportDelegate BTImport;
        public static BWT BWTInfo;
        public static CAIH CAIHInfo;
        public const string cAppBackGround = "后台";
        public const string cAppCnName = "永信在线挂机软件";
        public const string cAppEnName = "YXZXGJ";
        public const string cAppFont = "微软雅黑";
        public const string cAppHint = "提示";
        public static string cAppName = "CXG";
        public const string cAppVersionInfo = "1.0.4";
        public const string cAuditNo = "审核未过";
        public const string cAuditRun = "审核中...";
        public const string cAuditYes = "审核通过";
        public const string cAutoBets = "自动投注";
        public const string cAutomaticStart = @"software\Microsoft\Windows\CurrentVersion\Run";
        public const string cAutomaticStart64 = @"software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run";
        public const string cAutoUpdate = @"\Update\AutoUpdatePlus.exe";
        public const string cBetsJKHint = "当方案历史中挂情况满足设定条件时开始投注（0表示挂，1表示中），例如设置000就表示连挂3期后开始投注。";
        public const string cBetsNo = "投注失败-";
        public const string cBetsPlanList = @"\BetsPlanList\";
        public const string cBetsYes = "投注成功";
        public static CBL CBLInfo;
        public const string cBTFN = "BTFN";
        public const string cConfiguration = @"\Configuration.txt";
        public const string cCurrentExpect = "{0} 第 {1} 期开奖号码";
        public static CCYL CCYLInfo;
        public const string cDateFormat = "yyyy-MM-dd";
        public const int cDeathFormInfoInt = -1024;
        public const string cDebug = @"\Debug\";
        public const string cDefaultDateTime = "1900-1-1 00:00:00";
        public const string cDefaultDecimal = "0.0";
        public const string cDefaultFalse = "False";
        public const string cDefaultFontSize = "9";
        public const string cDefaultIndex = "0";
        public const string cDefaultInteger = "0";
        public const string cDefaultStr = "";
        public const string cDefaultTime = "00:00:00";
        public const string cDefaultTrue = "True";
        public const string cDeleteHint = "删除成功";
        public const string cE8WKey = "e8we8w8e";
        public const string cErrorList = @"\ErrorList\";
        public const string cErrorString = "【错误】-";
        public const string cExportHint = "导出成功！";
        public const string cFavoriteList = @"\FavoriteList";
        public const string cFNTypeDMLH = "定码轮换";
        public const string cFNTypeGDQM = "固定取码";
        public const string cFNTypeGJDMLH = "高级定码轮换";
        public const string cFNTypeGJKMTM = "高级开某投某";
        public const string cFNTypeKMTM = "开某投某";
        public const string cFNTypeLHKMTM = "龙虎开某投某";
        public const string cFNTypeLRWCH = "冷热温出号";
        public const string cFNTypeSJCH = "随机出号";
        public const string cFNTypeWJJH = "外接计划";
        public const string cFNTypeYLCH = "遗漏出号";
        public const string cFollowBetsString = "跟单投注";
        public const string cFollowSchemeString = "下载";
        public const string cFormMaximized = "Maximized";
        public const string cFormMinimized = "Minimized";
        public const string cFormNormal = "Normal";
        public const string cGJBTScheme = @"\GJBTScheme\";
        public const string cGYBets = "冠亚";
        public static ConfigurationStatus.CheckPTLineDelegate CheckPTLine;
        public static bool CheckPTLogin;
        public const string cHintFileNoFind = "文件已经丢失";
        public const string cHJFG = @"\HJFG\";
        public const string cImageSaveFileFiter = "图片(*.jpg;*.bmp;*png;*gif)|*.jpeg;*.jpg;*.bmp;*.png;*.gif";
        public const string cImportHint = "导入号码成功！共导入 {0} 注号码";
        public const string cIsZB = "IsZB ";
        public const string cLineString = "线路";
        public const string cLineString1 = "线路1";
        public const string cLineString2 = "线路2";
        public const string cLineString3 = "线路3";
        public const string cLineString4 = "线路4";
        public const string cLoading = "加载中...";
        public const string cLoginID = "用户：{0}";
        public const string cLoginOvertime = "登录超时";
        public const string cLogList = @"\LogList\";
        public const string cLogo = @"\Logo\";
        public const string Close = "关闭";
        public static ConfigurationStatus.CloseAppDelegate CloseApp;
        public const string cLotteryIDBy11X5 = "11X5";
        public const string cLotteryIDBy3D = "3D";
        public const string cLotteryIDByA65FC = "A65FC";
        public const string cLotteryIDByA6FFC = "A6FFC";
        public const string cLotteryIDByAH11X5 = "AH11X5";
        public const string cLotteryIDByAHK3 = "AHK3";
        public const string cLotteryIDByALGJ5FC = "ALGJ5FC";
        public const string cLotteryIDByALGJFF11X5 = "ALGJFF11X5";
        public const string cLotteryIDByALGJFFC = "ALGJFFC";
        public const string cLotteryIDByALGJNTXFFC = "ALGJNTXFFC";
        public const string cLotteryIDByALGJOZBWC = "ALGJOZBWC";
        public const string cLotteryIDByALGJRB15C = "ALGJRB15C";
        public const string cLotteryIDByALGJSE15C = "ALGJSE15C";
        public const string cLotteryIDByALGJTXFFC = "ALGJTXFFC";
        public const string cLotteryIDByAMBLRAM11X5 = "AMBLRAM11X5";
        public const string cLotteryIDByAMBLRAMPK10 = "AMBLRAMPK10";
        public const string cLotteryIDByAMBLRAMSSC = "AMBLRAMSSC";
        public const string cLotteryIDByAMBLRBX15F = "AMBLRBX15F";
        public const string cLotteryIDByAMBLRBXKLC = "AMBLRBXKLC";
        public const string cLotteryIDByAMBLRHN2FC = "AMBLRHN2FC";
        public const string cLotteryIDByAMBLRHN5FC = "AMBLRHN5FC";
        public const string cLotteryIDByAMBLRQQFFC = "AMBLRQQFFC";
        public const string cLotteryIDByAMBLRTW11X5 = "AMBLRTW11X5";
        public const string cLotteryIDByAMBLRTWPK10 = "AMBLRTWPK10";
        public const string cLotteryIDByAMBLRTWSSC = "AMBLRTWSSC";
        public const string cLotteryIDByAMBLRTXFFC = "AMBLRTXFFC";
        public const string cLotteryIDByAU3FC = "AU3FC";
        public const string cLotteryIDByAUFFC = "AUFFC";
        public const string cLotteryIDByB6YL3F11X5 = "B6YL3F11X5";
        public const string cLotteryIDByB6YL3FC = "B6YL3FC";
        public const string cLotteryIDByB6YL5FC = "B6YL5FC";
        public const string cLotteryIDByB6YLFFC = "B6YLFFC";
        public const string cLotteryIDByBB3FC = "BB3FC";
        public const string cLotteryIDByBBFFC = "BBFFC";
        public const string cLotteryIDByBD11X5 = "BD11X5";
        public const string cLotteryIDByBD2FC = "BD2FC";
        public const string cLotteryIDByBDFFC = "BDFFC";
        public const string cLotteryIDByBDSSC = "BDSSC";
        public const string cLotteryIDByBHGJ2FC = "BHGJ2FC";
        public const string cLotteryIDByBHGJ5FC = "BHGJ5FC";
        public const string cLotteryIDByBHGJFFC = "BHGJFFC";
        public const string cLotteryIDByBHGJHGSSC = "BHGJHGSSC";
        public const string cLotteryIDByBHGJHN15C = "BHGJHN15C";
        public const string cLotteryIDByBHGJHNFFC = "BHGJHNFFC";
        public const string cLotteryIDByBHGJPK10 = "BHGJPK10";
        public const string cLotteryIDByBHGJXXLSSC = "BHGJXXLSSC";
        public const string cLotteryIDByBHZY5FC = "BHZY5FC";
        public const string cLotteryIDByBHZYDJSSC = "BHZYDJSSC";
        public const string cLotteryIDByBHZYFFC = "BHZYFFC";
        public const string cLotteryIDByBHZYHGSSC = "BHZYHGSSC";
        public const string cLotteryIDByBHZYTXFFC = "BHZYTXFFC";
        public const string cLotteryIDByBJ11X5 = "BJ11X5";
        public const string cLotteryIDByBJ2HGSSC = "BJ2HGSSC";
        public const string cLotteryIDByBJK3 = "BJK3";
        public const string cLotteryIDByBJSSC = "BJSSC";
        public const string cLotteryIDByBKC11X5FFC = "BKC11X5FFC";
        public const string cLotteryIDByBKC2FC = "BKC2FC";
        public const string cLotteryIDByBKC5FC = "BKC5FC";
        public const string cLotteryIDByBKCFFC = "BKCFFC";
        public const string cLotteryIDByBLGJ5FC = "BLGJ5FC";
        public const string cLotteryIDByBLGJFF11X5 = "BLGJFF11X5";
        public const string cLotteryIDByBLGJFFC = "BLGJFFC";
        public const string cLotteryIDByBLGJFLP45M = "BLGJFLP45M";
        public const string cLotteryIDByBLGJNTXFFC = "BLGJNTXFFC";
        public const string cLotteryIDByBLGJTXFFC = "BLGJTXFFC";
        public const string cLotteryIDByBLGJXDL45M = "BLGJXDL45M";
        public const string cLotteryIDByBLGJXDL90M = "BLGJXDL90M";
        public const string cLotteryIDByBLGJYN15C = "BLGJYN15C";
        public const string cLotteryIDByBLSFFC = "BLSFFC";
        public const string cLotteryIDByBM1FC = "BM1FC";
        public const string cLotteryIDByBM2FC = "BM2FC";
        public const string cLotteryIDByBM5FC = "BM5FC";
        public const string cLotteryIDByBMDJSSC = "BMDJSSC";
        public const string cLotteryIDByBMEI5FC = "BMEI5FC";
        public const string cLotteryIDByBMEIFFC = "BMEIFFC";
        public const string cLotteryIDByBMEISE15F = "BMEISE15F";
        public const string cLotteryIDByBMFLBSSC = "BMFLBSSC";
        public const string cLotteryIDByBMHGSSC = "BMHGSSC";
        public const string cLotteryIDByBMQQFFC = "BMQQFFC";
        public const string cLotteryIDByBMTWFFC = "BMTWFFC";
        public const string cLotteryIDByBMTXFFC = "BMTXFFC";
        public const string cLotteryIDByBN5FC = "BN5FC";
        public const string cLotteryIDByBNFFC = "BNFFC";
        public const string cLotteryIDByBNTXFFC = "BNTXFFC";
        public const string cLotteryIDByBWT11X5 = "BWT11X5";
        public const string cLotteryIDByBWT5FC = "BWT5FC";
        public const string cLotteryIDByBWTDJSSC = "BWTDJSSC";
        public const string cLotteryIDByBWTFFC = "BWTFFC";
        public const string cLotteryIDByBWTHGSSC = "BWTHGSSC";
        public const string cLotteryIDByBWTOZBWC = "BWTOZBWC";
        public const string cLotteryIDByBWTTXFFC = "BWTTXFFC";
        public const string cLotteryIDByCAIHDJSSC = "CAIHDJSSC";
        public const string cLotteryIDByCAIHDLD30M = "CAIHDLD30M";
        public const string cLotteryIDByCAIHFLP15C = "CAIHFLP15C";
        public const string cLotteryIDByCAIHFLP2FC = "CAIHFLP2FC";
        public const string cLotteryIDByCAIHFLP5FC = "CAIHFLP5FC";
        public const string cLotteryIDByCAIHHGSSC = "CAIHHGSSC";
        public const string cLotteryIDByCAIHLD2FC = "CAIHLD2FC";
        public const string cLotteryIDByCAIHNY15C = "CAIHNY15C";
        public const string cLotteryIDByCAIHPK10 = "CAIHPK10";
        public const string cLotteryIDByCAIHSE15F = "CAIHSE15F";
        public const string cLotteryIDByCAIHXDLSSC = "CAIHXDLSSC";
        public const string cLotteryIDByCAIHXJPSSC = "CAIHXJPSSC";
        public const string cLotteryIDByCAIHXWYFFC = "CAIHXWYFFC";
        public const string cLotteryIDByCBLDJSSC = "CBLDJSSC";
        public const string cLotteryIDByCBLDLD30M = "CBLDLD30M";
        public const string cLotteryIDByCBLHGSSC = "CBLHGSSC";
        public const string cLotteryIDByCBLLD2FC = "CBLLD2FC";
        public const string cLotteryIDByCBLNY15C = "CBLNY15C";
        public const string cLotteryIDByCBLPK10 = "CBLPK10";
        public const string cLotteryIDByCBLSE15F = "CBLSE15F";
        public const string cLotteryIDByCBLXDLSSC = "CBLXDLSSC";
        public const string cLotteryIDByCBLXJPSSC = "CBLXJPSSC";
        public const string cLotteryIDByCBLXWYFFC = "CBLXWYFFC";
        public const string cLotteryIDByCCAM11X5 = "CCAM11X5";
        public const string cLotteryIDByCCDJSSCGB = "CCDJSSCGB";
        public const string cLotteryIDByCCFFPK10 = "CCFFPK10";
        public const string cLotteryIDByCCFLP15C = "CCFLP15C";
        public const string cLotteryIDByCCHGSSCGB = "CCHGSSCGB";
        public const string cLotteryIDByCCRD2FC = "CCRD2FC";
        public const string cLotteryIDByCCTG60M = "CCTG60M";
        public const string cLotteryIDByCCTJ3FC = "CCTJ3FC";
        public const string cLotteryIDByCCTJ5FC = "CCTJ5FC";
        public const string cLotteryIDByCCTW11X5 = "CCTW11X5";
        public const string cLotteryIDByCCTWSSCGB = "CCTWSSCGB";
        public const string cLotteryIDByCCWXFFC = "CCWXFFC";
        public const string cLotteryIDByCCXG11X5 = "CCXG11X5";
        public const string cLotteryIDByCCXG15C = "CCXG15C";
        public const string cLotteryIDByCLYL2F11X5 = "CLYL2F11X5";
        public const string cLotteryIDByCLYL2FC = "CLYL2FC";
        public const string cLotteryIDByCLYL3F11X5 = "CLYL3F11X5";
        public const string cLotteryIDByCLYL3FC = "CLYL3FC";
        public const string cLotteryIDByCLYL5F11X5 = "CLYL5F11X5";
        public const string cLotteryIDByCLYL5FC = "CLYL5FC";
        public const string cLotteryIDByCLYLAM15C = "CLYLAM15C";
        public const string cLotteryIDByCLYLDJSSC = "CLYLDJSSC";
        public const string cLotteryIDByCLYLFF11X5 = "CLYLFF11X5";
        public const string cLotteryIDByCLYLFFC = "CLYLFFC";
        public const string cLotteryIDByCLYLHGSSC = "CLYLHGSSC";
        public const string cLotteryIDByCLYLTB15C = "CLYLTB15C";
        public const string cLotteryIDByCLYLXDL15F = "CLYLXDL15F";
        public const string cLotteryIDByCQSSC = "CQSSC";
        public const string cLotteryIDByCTT5FC = "CTT5FC";
        public const string cLotteryIDByCTTFFC = "CTTFFC";
        public const string cLotteryIDByCTXDJSSC = "CTXDJSSC";
        public const string cLotteryIDByCTXDLD30M = "CTXDLD30M";
        public const string cLotteryIDByCTXHGSSC = "CTXHGSSC";
        public const string cLotteryIDByCTXLD2FC = "CTXLD2FC";
        public const string cLotteryIDByCTXNY15C = "CTXNY15C";
        public const string cLotteryIDByCTXPK10 = "CTXPK10";
        public const string cLotteryIDByCTXSE15F = "CTXSE15F";
        public const string cLotteryIDByCTXXDLSSC = "CTXXDLSSC";
        public const string cLotteryIDByCTXXJPSSC = "CTXXJPSSC";
        public const string cLotteryIDByCTXXWYFFC = "CTXXWYFFC";
        public const string cLotteryIDByCYYLDJSSC = "CYYLDJSSC";
        public const string cLotteryIDByCYYLDLD30M = "CYYLDLD30M";
        public const string cLotteryIDByCYYLHGSSC = "CYYLHGSSC";
        public const string cLotteryIDByCYYLLD2FC = "CYYLLD2FC";
        public const string cLotteryIDByCYYLNY15C = "CYYLNY15C";
        public const string cLotteryIDByCYYLSE15F = "CYYLSE15F";
        public const string cLotteryIDByCYYLXDLSSC = "CYYLXDLSSC";
        public const string cLotteryIDByCYYLXJPSSC = "CYYLXJPSSC";
        public const string cLotteryIDByCYYLXWYFFC = "CYYLXWYFFC";
        public const string cLotteryIDByDA3FC = "DA3FC";
        public const string cLotteryIDByDAFFC = "DAFFC";
        public const string cLotteryIDByDAZ11X5 = "DAZ11X5";
        public const string cLotteryIDByDAZ5FC = "DAZ5FC";
        public const string cLotteryIDByDAZDJSSC = "DAZDJSSC";
        public const string cLotteryIDByDAZFFC = "DAZFFC";
        public const string cLotteryIDByDAZHGSSC = "DAZHGSSC";
        public const string cLotteryIDByDAZJDSSC = "DAZJDSSC";
        public const string cLotteryIDByDAZRBSSC = "DAZRBSSC";
        public const string cLotteryIDByDAZTG15C = "DAZTG15C";
        public const string cLotteryIDByDB15C = "DB15C";
        public const string cLotteryIDByDEJI5FC = "DEJI5FC";
        public const string cLotteryIDByDEJIFF11X5 = "DEJIFF11X5";
        public const string cLotteryIDByDEJIFFC = "DEJIFFC";
        public const string cLotteryIDByDEJIFLP45M = "DEJIFLP45M";
        public const string cLotteryIDByDEJIHG5FC = "DEJIHG5FC";
        public const string cLotteryIDByDEJIJND4FC = "DEJIJND4FC";
        public const string cLotteryIDByDEJIMG45M = "DEJIMG45M";
        public const string cLotteryIDByDEJIQQXDL45M = "DEJIQQXDL45M";
        public const string cLotteryIDByDEJIQQXDL90M = "DEJIQQXDL90M";
        public const string cLotteryIDByDEJIRB45M = "DEJIRB45M";
        public const string cLotteryIDByDEJITXFFC = "DEJITXFFC";
        public const string cLotteryIDByDF3FC = "DF3FC";
        public const string cLotteryIDByDFFFC = "DFFFC";
        public const string cLotteryIDByDJSSC = "DJSSC";
        public const string cLotteryIDByDPC5FC = "DPC5FC";
        public const string cLotteryIDByDPCDJSSC = "DPCDJSSC";
        public const string cLotteryIDByDPCFFC = "DPCFFC";
        public const string cLotteryIDByDPCOZBWC = "DPCOZBWC";
        public const string cLotteryIDByDPCTXFFC = "DPCTXFFC";
        public const string cLotteryIDByDPSSC = "DPSSC";
        public const string cLotteryIDByDQOZ3FC = "DQOZ3FC";
        public const string cLotteryIDByDQSLFK5FC = "DQSLFK5FC";
        public const string cLotteryIDByDT2FC = "DT2FC";
        public const string cLotteryIDByDTFFC = "DTFFC";
        public const string cLotteryIDByDY2F11X5 = "DY2F11X5";
        public const string cLotteryIDByDY2FC = "DY2FC";
        public const string cLotteryIDByDY2FPK10 = "DY2FPK10";
        public const string cLotteryIDByDY3F11X5 = "DY3F11X5";
        public const string cLotteryIDByDY3FC = "DY3FC";
        public const string cLotteryIDByDY3FPK10 = "DY3FPK10";
        public const string cLotteryIDByDY5F11X5 = "DY5F11X5";
        public const string cLotteryIDByDY5FC = "DY5FC";
        public const string cLotteryIDByDY5FPK10 = "DY5FPK10";
        public const string cLotteryIDByDYDJSSC = "DYDJSSC";
        public const string cLotteryIDByDYFF11X5 = "DYFF11X5";
        public const string cLotteryIDByDYFFC = "DYFFC";
        public const string cLotteryIDByDYFFPK10 = "DYFFPK10";
        public const string cLotteryIDByDYHGSSC = "DYHGSSC";
        public const string cLotteryIDByDYJ2FC = "DYJ2FC";
        public const string cLotteryIDByDYJFFC = "DYJFFC";
        public const string cLotteryIDByELSSSC = "ELSSSC";
        public const string cLotteryIDByFCOZ3FC = "FCOZ3FC";
        public const string cLotteryIDByFCSLFK5FC = "FCSLFK5FC";
        public const string cLotteryIDByFEIC2F11X5 = "FEIC2F11X5";
        public const string cLotteryIDByFEIC2FPK10 = "FEIC2FPK10";
        public const string cLotteryIDByFEIC30M = "FEIC30M";
        public const string cLotteryIDByFEIC3F11X5 = "FEIC3F11X5";
        public const string cLotteryIDByFEIC3FC = "FEIC3FC";
        public const string cLotteryIDByFEIC3FPK10 = "FEIC3FPK10";
        public const string cLotteryIDByFEIC45M = "FEIC45M";
        public const string cLotteryIDByFEIC5F11X5 = "FEIC5F11X5";
        public const string cLotteryIDByFEIC5FPK10 = "FEIC5FPK10";
        public const string cLotteryIDByFEICDJSSC = "FEICDJSSC";
        public const string cLotteryIDByFEICFF11X5 = "FEICFF11X5";
        public const string cLotteryIDByFEICFFC = "FEICFFC";
        public const string cLotteryIDByFEICFFPK10 = "FEICFFPK10";
        public const string cLotteryIDByFEICHGSSC = "FEICHGSSC";
        public const string cLotteryIDByFEICSLFK5FC = "FEICSLFK5FC";
        public const string cLotteryIDByFEICXJP2FC = "FEICXJP2FC";
        public const string cLotteryIDByFJ11X5 = "FJ11X5";
        public const string cLotteryIDByFJK3 = "FJK3";
        public const string cLotteryIDByFLBSSC = "FLBSSC";
        public const string cLotteryIDByFLC2FC = "FLC2FC";
        public const string cLotteryIDByFLC5FC = "FLC5FC";
        public const string cLotteryIDByFLCFFC = "FLCFFC";
        public const string cLotteryIDByFNYXFFC = "FNYXFFC";
        public const string cLotteryIDByFNYXHY11X5 = "FNYXHY11X5";
        public const string cLotteryIDByFS5FC = "FS5FC";
        public const string cLotteryIDByFSFFC = "FSFFC";
        public const string cLotteryIDByGD11X5 = "GD11X5";
        public const string cLotteryIDByGDFFC = "GDFFC";
        public const string cLotteryIDByGF3FC = "GF3FC";
        public const string cLotteryIDByGFFFC = "GFFFC";
        public const string cLotteryIDByGGFFC = "GGFFC";
        public const string cLotteryIDByGGSSC = "GGSSC";
        public const string cLotteryIDByGJ5FC = "GJ5FC";
        public const string cLotteryIDByGJFF11X5 = "GJFF11X5";
        public const string cLotteryIDByGJFFC = "GJFFC";
        public const string cLotteryIDByGJOZBWC = "GJOZBWC";
        public const string cLotteryIDByGJTX60M = "GJTX60M";
        public const string cLotteryIDByGS11X5 = "GS11X5";
        public const string cLotteryIDByGSK3 = "GSK3";
        public const string cLotteryIDByGX11X5 = "GX11X5";
        public const string cLotteryIDByGXK3 = "GXK3";
        public const string cLotteryIDByGZ11X5 = "GZ11X5";
        public const string cLotteryIDByGZK3 = "GZK3";
        public const string cLotteryIDByHANYDJSSC = "HANYDJSSC";
        public const string cLotteryIDByHANYFLP2FC = "HANYFLP2FC";
        public const string cLotteryIDByHANYFLP30M = "HANYFLP30M";
        public const string cLotteryIDByHANYFLPFFC = "HANYFLPFFC";
        public const string cLotteryIDByHANYHGSSC = "HANYHGSSC";
        public const string cLotteryIDByHANYJPZ30M = "HANYJPZ30M";
        public const string cLotteryIDByHANYJPZ5FC = "HANYJPZ5FC";
        public const string cLotteryIDByHANYJPZFFC = "HANYJPZFFC";
        public const string cLotteryIDByHANYMD30M = "HANYMD30M";
        public const string cLotteryIDByHANYMD3FC = "HANYMD3FC";
        public const string cLotteryIDByHANYMDFFC = "HANYMDFFC";
        public const string cLotteryIDByHANYTXFFC = "HANYTXFFC";
        public const string cLotteryIDByHANYXJP30M = "HANYXJP30M";
        public const string cLotteryIDByHB11X5 = "HB11X5";
        public const string cLotteryIDByHCSSC = "HCSSC";
        public const string cLotteryIDByHCYLOZ3FC = "HCYLOZ3FC";
        public const string cLotteryIDByHCYLSLFK5FC = "HCYLSLFK5FC";
        public const string cLotteryIDByHCZX3FPK10 = "HCZX3FPK10";
        public const string cLotteryIDByHCZX5FC = "HCZX5FC";
        public const string cLotteryIDByHCZXDJSSC = "HCZXDJSSC";
        public const string cLotteryIDByHCZXFFC = "HCZXFFC";
        public const string cLotteryIDByHCZXFFPK10 = "HCZXFFPK10";
        public const string cLotteryIDByHCZXJNDSSC = "HCZXJNDSSC";
        public const string cLotteryIDByHCZXMG45M = "HCZXMG45M";
        public const string cLotteryIDByHCZXNTXFFC = "HCZXNTXFFC";
        public const string cLotteryIDByHDYL2F11X5 = "HDYL2F11X5";
        public const string cLotteryIDByHDYL2FC = "HDYL2FC";
        public const string cLotteryIDByHDYL5F11X5 = "HDYL5F11X5";
        public const string cLotteryIDByHDYL5FC = "HDYL5FC";
        public const string cLotteryIDByHDYLASKFFC = "HDYLASKFFC";
        public const string cLotteryIDByHDYLFF11X5 = "HDYLFF11X5";
        public const string cLotteryIDByHDYLFFC = "HDYLFFC";
        public const string cLotteryIDByHDYLFFFT = "HDYLFFFT";
        public const string cLotteryIDByHDYLFFPK10 = "HDYLFFPK10";
        public const string cLotteryIDByHEB11X5 = "HEB11X5";
        public const string cLotteryIDByHEBK3 = "HEBK3";
        public const string cLotteryIDByHEND2FC = "HEND2FC";
        public const string cLotteryIDByHEND5FC = "HEND5FC";
        public const string cLotteryIDByHENDDJSSC = "HENDDJSSC";
        public const string cLotteryIDByHENDDX15C = "HENDDX15C";
        public const string cLotteryIDByHENDFFC = "HENDFFC";
        public const string cLotteryIDByHENDFFPK10 = "HENDFFPK10";
        public const string cLotteryIDByHENDHG1FC = "HENDHG1FC";
        public const string cLotteryIDByHENDHG2FC = "HENDHG2FC";
        public const string cLotteryIDByHENDHGSSC = "HENDHGSSC";
        public const string cLotteryIDByHENDJS11X5 = "HENDJS11X5";
        public const string cLotteryIDByHENDJSPK10 = "HENDJSPK10";
        public const string cLotteryIDByHENDNY15C = "HENDNY15C";
        public const string cLotteryIDByHENDOMPK10 = "HENDOMPK10";
        public const string cLotteryIDByHENDTG30 = "HENDTG30";
        public const string cLotteryIDByHENDTXPK10 = "HENDTXPK10";
        public const string cLotteryIDByHENDXDLSSC = "HENDXDLSSC";
        public const string cLotteryIDByHENDXJPSSC = "HENDXJPSSC";
        public const string cLotteryIDByHENDXXLSSC = "HENDXXLSSC";
        public const string cLotteryIDByHENR2FC = "HENR2FC";
        public const string cLotteryIDByHENR3FC = "HENR3FC";
        public const string cLotteryIDByHENRDJ2FC = "HENRDJ2FC";
        public const string cLotteryIDByHENRFFC = "HENRFFC";
        public const string cLotteryIDByHENROZ15C = "HENROZ15C";
        public const string cLotteryIDByHENRTXFFC = "HENRTXFFC";
        public const string cLotteryIDByHENRXG15C = "HENRXG15C";
        public const string cLotteryIDByHENRXJPSSC = "HENRXJPSSC";
        public const string cLotteryIDByHG1FC = "HG1FC";
        public const string cLotteryIDByHGDB5FC = "HGDB5FC";
        public const string cLotteryIDByHGDBFFC = "HGDBFFC";
        public const string cLotteryIDByHGLTC = "HGLTC";
        public const string cLotteryIDByHGSSC = "HGSSC";
        public const string cLotteryIDByHH3FC = "HH3FC";
        public const string cLotteryIDByHHFFC = "HHFFC";
        public const string cLotteryIDByHKC11X5FFC = "HKC11X5FFC";
        public const string cLotteryIDByHKC2FC = "HKC2FC";
        public const string cLotteryIDByHKC5FC = "HKC5FC";
        public const string cLotteryIDByHKCFFC = "HKCFFC";
        public const string cLotteryIDByHLC2FC = "HLC2FC";
        public const string cLotteryIDByHLC5FC = "HLC5FC";
        public const string cLotteryIDByHLCDJ15F = "HLCDJ15F";
        public const string cLotteryIDByHLCFFC = "HLCFFC";
        public const string cLotteryIDByHLCFLB15C = "HLCFLB15C";
        public const string cLotteryIDByHLCFLB2FC = "HLCFLB2FC";
        public const string cLotteryIDByHLCFLB5FC = "HLCFLB5FC";
        public const string cLotteryIDByHLCHG15F = "HLCHG15F";
        public const string cLotteryIDByHLCNY15C = "HLCNY15C";
        public const string cLotteryIDByHLCSE15F = "HLCSE15F";
        public const string cLotteryIDByHLJ11X5 = "HLJ11X5";
        public const string cLotteryIDByHLJSSC = "HLJSSC";
        public const string cLotteryIDByHN11X5 = "HN11X5";
        public const string cLotteryIDByHNYL11X5 = "HNYL11X5";
        public const string cLotteryIDByHNYL5FC = "HNYL5FC";
        public const string cLotteryIDByHNYLFFC = "HNYLFFC";
        public const string cLotteryIDByHNYLFLP15C = "HNYLFLP15C";
        public const string cLotteryIDByHNYLXJPSSC = "HNYLXJPSSC";
        public const string cLotteryIDByHOND2FC = "HOND2FC";
        public const string cLotteryIDByHOND5FC = "HOND5FC";
        public const string cLotteryIDByHONDDJ15F = "HONDDJ15F";
        public const string cLotteryIDByHONDFFC = "HONDFFC";
        public const string cLotteryIDByHONDHG15F = "HONDHG15F";
        public const string cLotteryIDByHONDNY15C = "HONDNY15C";
        public const string cLotteryIDByHONDSE15F = "HONDSE15F";
        public const string cLotteryIDByHR2FC = "HR2FC";
        public const string cLotteryIDByHR45C = "HR45C";
        public const string cLotteryIDByHRFFC = "HRFFC";
        public const string cLotteryIDByHS5FC = "HS5FC";
        public const string cLotteryIDByHSAZXYC = "HSAZXYC";
        public const string cLotteryIDByHSDJSSC = "HSDJSSC";
        public const string cLotteryIDByHSFFC = "HSFFC";
        public const string cLotteryIDByHSQQFFC = "HSQQFFC";
        public const string cLotteryIDByHSSE15F = "HSSE15F";
        public const string cLotteryIDByHUAY11X5 = "HUAY11X5";
        public const string cLotteryIDByHUAY2FC = "HUAY2FC";
        public const string cLotteryIDByHUAY5FC = "HUAY5FC";
        public const string cLotteryIDByHUAYFFC = "HUAYFFC";
        public const string cLotteryIDByHUB11X5 = "HUB11X5";
        public const string cLotteryIDByHUBK3 = "HUBK3";
        public const string cLotteryIDByHUBO2FC = "HUBO2FC";
        public const string cLotteryIDByHUBO5FC = "HUBO5FC";
        public const string cLotteryIDByHUBODJSSC = "HUBODJSSC";
        public const string cLotteryIDByHUBODZ30M = "HUBODZ30M";
        public const string cLotteryIDByHUBOFFC = "HUBOFFC";
        public const string cLotteryIDByHUBOFLP2FC = "HUBOFLP2FC";
        public const string cLotteryIDByHUBOHGSSC = "HUBOHGSSC";
        public const string cLotteryIDByHUBOMG45M = "HUBOMG45M";
        public const string cLotteryIDByHUBOML20M = "HUBOML20M";
        public const string cLotteryIDByHUBOTXFFC = "HUBOTXFFC";
        public const string cLotteryIDByHUBOWXFFC = "HUBOWXFFC";
        public const string cLotteryIDByHUIZ5FC = "HUIZ5FC";
        public const string cLotteryIDByHUIZFF11X5 = "HUIZFF11X5";
        public const string cLotteryIDByHUIZFFC = "HUIZFFC";
        public const string cLotteryIDByHUIZFFPK10 = "HUIZFFPK10";
        public const string cLotteryIDByHUIZHGSSC = "HUIZHGSSC";
        public const string cLotteryIDByHUIZJN15C = "HUIZJN15C";
        public const string cLotteryIDByHUIZXJPSSC = "HUIZXJPSSC";
        public const string cLotteryIDByHY11X5 = "HY11X5";
        public const string cLotteryIDByHY2FC = "HY2FC";
        public const string cLotteryIDByHYDJSSC = "HYDJSSC";
        public const string cLotteryIDByHYFFC = "HYFFC";
        public const string cLotteryIDByHYFLBSSC = "HYFLBSSC";
        public const string cLotteryIDByHYGGFFC = "HYGGFFC";
        public const string cLotteryIDByHYHGSSC = "HYHGSSC";
        public const string cLotteryIDByHYHLWFFC = "HYHLWFFC";
        public const string cLotteryIDByHYHNFFC = "HYHNFFC";
        public const string cLotteryIDByHYJNDFFC = "HYJNDFFC";
        public const string cLotteryIDByHYPK10 = "HYPK10";
        public const string cLotteryIDByHYSkyFFC = "HYSkyFFC";
        public const string cLotteryIDByHYTTFFC = "HYTTFFC";
        public const string cLotteryIDByHYTXFFC = "HYTXFFC";
        public const string cLotteryIDByHYXDLSSC = "HYXDLSSC";
        public const string cLotteryIDByHYXJPSSC = "HYXJPSSC";
        public const string cLotteryIDByHYYJFFC = "HYYJFFC";
        public const string cLotteryIDByHYYTFFC = "HYYTFFC";
        public const string cLotteryIDByHZ3FC = "HZ3FC";
        public const string cLotteryIDByHZ5FC = "HZ5FC";
        public const string cLotteryIDByHZDJSSC = "HZDJSSC";
        public const string cLotteryIDByHZFF11X5 = "HZFF11X5";
        public const string cLotteryIDByHZFFC = "HZFFC";
        public const string cLotteryIDByHZHG2FC = "HZHG2FC";
        public const string cLotteryIDByHZHGSSC = "HZHGSSC";
        public const string cLotteryIDByHZJNDSSC = "HZJNDSSC";
        public const string cLotteryIDByHZML15F = "HZML15F";
        public const string cLotteryIDByHZQQFFC = "HZQQFFC";
        public const string cLotteryIDByHZTG11X5 = "HZTG11X5";
        public const string cLotteryIDByHZTG15F = "HZTG15F";
        public const string cLotteryIDByHZXDLSSC = "HZXDLSSC";
        public const string cLotteryIDByHZXJP2FC = "HZXJP2FC";
        public const string cLotteryIDByHZXXLSSC = "HZXXLSSC";
        public const string cLotteryIDByJBEITG30 = "JBEITG30";
        public const string cLotteryIDByJCX60MPK10 = "JCX60MPK10";
        public const string cLotteryIDByJCXFLPFFC = "JCXFLPFFC";
        public const string cLotteryIDByJCXHN15C = "JCXHN15C";
        public const string cLotteryIDByJCXJN11X5 = "JCXJN11X5";
        public const string cLotteryIDByJCXMG15C = "JCXMG15C";
        public const string cLotteryIDByJCXMG45M = "JCXMG45M";
        public const string cLotteryIDByJCXMJFFC = "JCXMJFFC";
        public const string cLotteryIDByJCXNDJSSC = "JCXNDJSSC";
        public const string cLotteryIDByJCXNHGSSC = "JCXNHGSSC";
        public const string cLotteryIDByJCXNY15C = "JCXNY15C";
        public const string cLotteryIDByJCXNYFFC = "JCXNYFFC";
        public const string cLotteryIDByJCXQQFFC = "JCXQQFFC";
        public const string cLotteryIDByJCXXDLSSC = "JCXXDLSSC";
        public const string cLotteryIDByJCXXXLSSC = "JCXXXLSSC";
        public const string cLotteryIDByJDSSC = "JDSSC";
        public const string cLotteryIDByJFYL2F11X5 = "JFYL2F11X5";
        public const string cLotteryIDByJFYL2FC = "JFYL2FC";
        public const string cLotteryIDByJFYLBHD15C = "JFYLBHD15C";
        public const string cLotteryIDByJFYLFFC = "JFYLFFC";
        public const string cLotteryIDByJFYLGG15C = "JFYLGG15C";
        public const string cLotteryIDByJFYLGX2FC = "JFYLGX2FC";
        public const string cLotteryIDByJH15C = "JH15C";
        public const string cLotteryIDByJH2FC = "JH2FC";
        public const string cLotteryIDByJH5FC = "JH5FC";
        public const string cLotteryIDByJHC215C = "JHC215C";
        public const string cLotteryIDByJHC23FC = "JHC23FC";
        public const string cLotteryIDByJHC2DPFFC = "JHC2DPFFC";
        public const string cLotteryIDByJHC2FC = "JHC2FC";
        public const string cLotteryIDByJHC2FFC = "JHC2FFC";
        public const string cLotteryIDByJHC2JD15C = "JHC2JD15C";
        public const string cLotteryIDByJHC2JZFFC = "JHC2JZFFC";
        public const string cLotteryIDByJHC2PK10 = "JHC2PK10";
        public const string cLotteryIDByJHC5FC = "JHC5FC";
        public const string cLotteryIDByJHCDBFFC = "JHCDBFFC";
        public const string cLotteryIDByJHCFFC = "JHCFFC";
        public const string cLotteryIDByJHCJDSSC = "JHCJDSSC";
        public const string cLotteryIDByJHCJZFFC = "JHCJZFFC";
        public const string cLotteryIDByJHJPZ15C = "JHJPZ15C";
        public const string cLotteryIDByJHQQFFC = "JHQQFFC";
        public const string cLotteryIDByJHTXFFC = "JHTXFFC";
        public const string cLotteryIDByJL11X5 = "JL11X5";
        public const string cLotteryIDByJLGJ5FC = "JLGJ5FC";
        public const string cLotteryIDByJLGJFF11X5 = "JLGJFF11X5";
        public const string cLotteryIDByJLGJFFC = "JLGJFFC";
        public const string cLotteryIDByJLGJTXFFC = "JLGJTXFFC";
        public const string cLotteryIDByJLK3 = "JLK3";
        public const string cLotteryIDByJLSSC = "JLSSC";
        public const string cLotteryIDByJN15F = "JN15F";
        public const string cLotteryIDByJNDSSC = "JNDSSC";
        public const string cLotteryIDByJS11X5 = "JS11X5";
        public const string cLotteryIDByJSK3 = "JSK3";
        public const string cLotteryIDByJW3FC = "JW3FC";
        public const string cLotteryIDByJWFFC = "JWFFC";
        public const string cLotteryIDByJX11X5 = "JX11X5";
        public const string cLotteryIDByJXFFC = "JXFFC";
        public const string cLotteryIDByJXIN2FC = "JXIN2FC";
        public const string cLotteryIDByJXIN5FC = "JXIN5FC";
        public const string cLotteryIDByJXINDB15C = "JXINDB15C";
        public const string cLotteryIDByJXINDJSSC = "JXINDJSSC";
        public const string cLotteryIDByJXINFFC = "JXINFFC";
        public const string cLotteryIDByJXINFLP15C = "JXINFLP15C";
        public const string cLotteryIDByJXINHGSSC = "JXINHGSSC";
        public const string cLotteryIDByJXINJLP15C = "JXINJLP15C";
        public const string cLotteryIDByJXINMSK15C = "JXINMSK15C";
        public const string cLotteryIDByJXINXJPSSC = "JXINXJPSSC";
        public const string cLotteryIDByJXK3 = "JXK3";
        public const string cLotteryIDByJXYL2FC = "JXYL2FC";
        public const string cLotteryIDByJXYL3FC = "JXYL3FC";
        public const string cLotteryIDByJXYL5FC = "JXYL5FC";
        public const string cLotteryIDByJXYLELSSSC = "JXYLELSSSC";
        public const string cLotteryIDByJXYLFF11X5 = "JXYLFF11X5";
        public const string cLotteryIDByJXYLFFC = "JXYLFFC";
        public const string cLotteryIDByJXYLHG5FC = "JXYLHG5FC";
        public const string cLotteryIDByJXYLXXLSSC = "JXYLXXLSSC";
        public const string cLotteryIDByJY3FC = "JY3FC";
        public const string cLotteryIDByJYFFC = "JYFFC";
        public const string cLotteryIDByJYIN3FC = "JYIN3FC";
        public const string cLotteryIDByJYINFFC = "JYINFFC";
        public const string cLotteryIDByJYYLDJSSC = "JYYLDJSSC";
        public const string cLotteryIDByJYYLHGSSC = "JYYLHGSSC";
        public const string cLotteryIDByJYYLLD2FC = "JYYLLD2FC";
        public const string cLotteryIDByJYYLQQFFC = "JYYLQQFFC";
        public const string cLotteryIDByJYYLTX30M = "JYYLTX30M";
        public const string cLotteryIDByJYYLXDLSSC = "JYYLXDLSSC";
        public const string cLotteryIDByJYYLXJPSSC = "JYYLXJPSSC";
        public const string cLotteryIDByJYYLXWYFFC = "JYYLXWYFFC";
        public const string cLotteryIDByJZD11X5 = "JZD11X5";
        public const string cLotteryIDByJZD15FC = "JZD15FC";
        public const string cLotteryIDByJZDPK10 = "JZDPK10";
        public const string cLotteryIDByK311X5 = "K311X5";
        public const string cLotteryIDByK3DJFFC = "K3DJFFC";
        public const string cLotteryIDByK3DJSSC = "K3DJSSC";
        public const string cLotteryIDByK3HG2FC = "K3HG2FC";
        public const string cLotteryIDByK3HGSSC = "K3HGSSC";
        public const string cLotteryIDByK3MG5FC = "K3MG5FC";
        public const string cLotteryIDByK3NTXFFC = "K3NTXFFC";
        public const string cLotteryIDByK3TXFFC = "K3TXFFC";
        public const string cLotteryIDByK3XXLSSC = "K3XXLSSC";
        public const string cLotteryIDByK55FC = "K55FC";
        public const string cLotteryIDByK5FFC = "K5FFC";
        public const string cLotteryIDByKC2FC = "KC2FC";
        public const string cLotteryIDByKC5FC = "KC5FC";
        public const string cLotteryIDByKCFFC = "KCFFC";
        public const string cLotteryIDByKL2FC = "KL2FC";
        public const string cLotteryIDByKLFFC = "KLFFC";
        public const string cLotteryIDByKX11X53FC = "KX11X53FC";
        public const string cLotteryIDByKX11X5FFC = "KX11X5FFC";
        public const string cLotteryIDByKXBL15C = "KXBL15C";
        public const string cLotteryIDByKXFLBSSC = "KXFLBSSC";
        public const string cLotteryIDByKXHGSSC = "KXHGSSC";
        public const string cLotteryIDByKXTXFFC = "KXTXFFC";
        public const string cLotteryIDByKXWBFFC = "KXWBFFC";
        public const string cLotteryIDByKXWX15C = "KXWX15C";
        public const string cLotteryIDByKXYN3FC = "KXYN3FC";
        public const string cLotteryIDByKXYN5FC = "KXYN5FC";
        public const string cLotteryIDByKXYNFFC = "KXYNFFC";
        public const string cLotteryIDByKY11X53FC = "KY11X53FC";
        public const string cLotteryIDByKY11X5FFC = "KY11X5FFC";
        public const string cLotteryIDByKYBL15C = "KYBL15C";
        public const string cLotteryIDByKYFLBSSC = "KYFLBSSC";
        public const string cLotteryIDByKYHGSSC = "KYHGSSC";
        public const string cLotteryIDByKYWBFFC = "KYWBFFC";
        public const string cLotteryIDByKYWX15C = "KYWX15C";
        public const string cLotteryIDByKYYN3FC = "KYYN3FC";
        public const string cLotteryIDByKYYN5FC = "KYYN5FC";
        public const string cLotteryIDByKYYNFFC = "KYYNFFC";
        public const string cLotteryIDByLD11X5 = "LD11X5";
        public const string cLotteryIDByLD2FC = "LD2FC";
        public const string cLotteryIDByLDPK10 = "LDPK10";
        public const string cLotteryIDByLDYL11X53FC = "LDYL11X53FC";
        public const string cLotteryIDByLDYL11X5FFC = "LDYL11X5FFC";
        public const string cLotteryIDByLDYL3FC = "LDYL3FC";
        public const string cLotteryIDByLDYL5FC = "LDYL5FC";
        public const string cLotteryIDByLDYLBL15C = "LDYLBL15C";
        public const string cLotteryIDByLDYLFFC = "LDYLFFC";
        public const string cLotteryIDByLDYLFLBSSC = "LDYLFLBSSC";
        public const string cLotteryIDByLDYLHGSSC = "LDYLHGSSC";
        public const string cLotteryIDByLDYLWBFFC = "LDYLWBFFC";
        public const string cLotteryIDByLDYLWX15C = "LDYLWX15C";
        public const string cLotteryIDByLEF2FC = "LEF2FC";
        public const string cLotteryIDByLEF5FC = "LEF5FC";
        public const string cLotteryIDByLEFFFC = "LEFFFC";
        public const string cLotteryIDByLF11X5 = "LF11X5";
        public const string cLotteryIDByLF22FC = "LF22FC";
        public const string cLotteryIDByLF25FC = "LF25FC";
        public const string cLotteryIDByLF2FC = "LF2FC";
        public const string cLotteryIDByLF2FFC = "LF2FFC";
        public const string cLotteryIDByLF5FC = "LF5FC";
        public const string cLotteryIDByLFDJSSC = "LFDJSSC";
        public const string cLotteryIDByLFFFC = "LFFFC";
        public const string cLotteryIDByLFHGSSC = "LFHGSSC";
        public const string cLotteryIDByLGZX3F11X5 = "LGZX3F11X5";
        public const string cLotteryIDByLGZX3FC = "LGZX3FC";
        public const string cLotteryIDByLGZXFF11X5 = "LGZXFF11X5";
        public const string cLotteryIDByLGZXFFC = "LGZXFFC";
        public const string cLotteryIDByLGZXPK10 = "LGZXPK10";
        public const string cLotteryIDByLMHDJSSC = "LMHDJSSC";
        public const string cLotteryIDByLMHHGSSC = "LMHHGSSC";
        public const string cLotteryIDByLMHXY45M = "LMHXY45M";
        public const string cLotteryIDByLN11X5 = "LN11X5";
        public const string cLotteryIDByLSWJS5FC = "LSWJS5FC";
        public const string cLotteryIDByLSWJSDJSSC = "LSWJSDJSSC";
        public const string cLotteryIDByLSWJSFF11X5 = "LSWJSFF11X5";
        public const string cLotteryIDByLSWJSFFC = "LSWJSFFC";
        public const string cLotteryIDByLSWJSFFPK10 = "LSWJSFFPK10";
        public const string cLotteryIDByLSWJSHGSSC = "LSWJSHGSSC";
        public const string cLotteryIDByLSWJSMG15C = "LSWJSMG15C";
        public const string cLotteryIDByLSWJSMG45M = "LSWJSMG45M";
        public const string cLotteryIDByLSWJSOTXFFC = "LSWJSOTXFFC";
        public const string cLotteryIDByLSWJSOZ35C = "LSWJSOZ35C";
        public const string cLotteryIDByLSWJSOZFFC = "LSWJSOZFFC";
        public const string cLotteryIDByLSWJSTXFFC = "LSWJSTXFFC";
        public const string cLotteryIDByLT2FC = "LT2FC";
        public const string cLotteryIDByLTFFC = "LTFFC";
        public const string cLotteryIDByLUDI3FC = "LUDI3FC";
        public const string cLotteryIDByLUDIFFC = "LUDIFFC";
        public const string cLotteryIDByLX3FC = "LX3FC";
        public const string cLotteryIDByLX5FC = "LX5FC";
        public const string cLotteryIDByLXFFC = "LXFFC";
        public const string cLotteryIDByLYSSLFK5FC = "LYSSLFK5FC";
        public const string cLotteryIDByM511X5 = "M511X5";
        public const string cLotteryIDByM53FC = "M53FC";
        public const string cLotteryIDByM55FC = "M55FC";
        public const string cLotteryIDByM5FFC = "M5FFC";
        public const string cLotteryIDByMCQQFFC = "MCQQFFC";
        public const string cLotteryIDByMD2FC = "MD2FC";
        public const string cLotteryIDByMDHG90M = "MDHG90M";
        public const string cLotteryIDByMDHGSSC = "MDHGSSC";
        public const string cLotteryIDByMINC2FC = "MINC2FC";
        public const string cLotteryIDByMINC5FC = "MINC5FC";
        public const string cLotteryIDByMINCDJSSC = "MINCDJSSC";
        public const string cLotteryIDByMINCFFC = "MINCFFC";
        public const string cLotteryIDByMINCNY15C = "MINCNY15C";
        public const string cLotteryIDByMINCSE15F = "MINCSE15F";
        public const string cLotteryIDByMINCTXFFC = "MINCTXFFC";
        public const string cLotteryIDByMR11X5 = "MR11X5";
        public const string cLotteryIDByMR2FC = "MR2FC";
        public const string cLotteryIDByMR45C = "MR45C";
        public const string cLotteryIDByMRFFC = "MRFFC";
        public const string cLotteryIDByMRPK10 = "MRPK10";
        public const string cLotteryIDByMTDJSSC = "MTDJSSC";
        public const string cLotteryIDByMTDLD30M = "MTDLD30M";
        public const string cLotteryIDByMTHGSSC = "MTHGSSC";
        public const string cLotteryIDByMTLD2FC = "MTLD2FC";
        public const string cLotteryIDByMTNY15C = "MTNY15C";
        public const string cLotteryIDByMTPK10 = "MTPK10";
        public const string cLotteryIDByMTSE15F = "MTSE15F";
        public const string cLotteryIDByMTXDLSSC = "MTXDLSSC";
        public const string cLotteryIDByMTXJPSSC = "MTXJPSSC";
        public const string cLotteryIDByMTXWYFFC = "MTXWYFFC";
        public const string cLotteryIDByMXYLDJSSC = "MXYLDJSSC";
        public const string cLotteryIDByMXYLHGSSC = "MXYLHGSSC";
        public const string cLotteryIDByMXYLLD2FC = "MXYLLD2FC";
        public const string cLotteryIDByMXYLQQFFC = "MXYLQQFFC";
        public const string cLotteryIDByMXYLTX30M = "MXYLTX30M";
        public const string cLotteryIDByMXYLXDLSSC = "MXYLXDLSSC";
        public const string cLotteryIDByMXYLXJPSSC = "MXYLXJPSSC";
        public const string cLotteryIDByMXYLXWYFFC = "MXYLXWYFFC";
        public const string cLotteryIDByMYOZBWC = "MYOZBWC";
        public const string cLotteryIDByMZC3FC = "MZC3FC";
        public const string cLotteryIDByMZCFFC = "MZCFFC";
        public const string cLotteryIDByNB5FC = "NB5FC";
        public const string cLotteryIDByNBA180MPK10 = "NBA180MPK10";
        public const string cLotteryIDByNBA3F11X5 = "NBA3F11X5";
        public const string cLotteryIDByNBA5F11X5 = "NBA5F11X5";
        public const string cLotteryIDByNBA60MPK10 = "NBA60MPK10";
        public const string cLotteryIDByNBABJSSC = "NBABJSSC";
        public const string cLotteryIDByNBADJSSC = "NBADJSSC";
        public const string cLotteryIDByNBAFF11X5 = "NBAFF11X5";
        public const string cLotteryIDByNBAFLP15C = "NBAFLP15C";
        public const string cLotteryIDByNBAHGSSC = "NBAHGSSC";
        public const string cLotteryIDByNBAJN11X5 = "NBAJN11X5";
        public const string cLotteryIDByNBAJZDPK10 = "NBAJZDPK10";
        public const string cLotteryIDByNBANDJSSC = "NBANDJSSC";
        public const string cLotteryIDByNBANY2FC = "NBANY2FC";
        public const string cLotteryIDByNBANY3FC = "NBANY3FC";
        public const string cLotteryIDByNBANY5FC = "NBANY5FC";
        public const string cLotteryIDByNBAQQFFC = "NBAQQFFC";
        public const string cLotteryIDByNBATWSSC = "NBATWSSC";
        public const string cLotteryIDByNBAXDLSSC = "NBAXDLSSC";
        public const string cLotteryIDByNBAXXLSSC = "NBAXXLSSC";
        public const string cLotteryIDByNBFFC = "NBFFC";
        public const string cLotteryIDByNBRBSSC = "NBRBSSC";
        public const string cLotteryIDByNBTGSSC = "NBTGSSC";
        public const string cLotteryIDByNMG11X5 = "NMG11X5";
        public const string cLotteryIDByNMGK3 = "NMGK3";
        public const string cLotteryIDByNMGSSC = "NMGSSC";
        public const string cLotteryIDByNQQFFC = "NQQFFC";
        public const string cLotteryIDByNY3FC = "NY3FC";
        public const string cLotteryIDByNY5FC = "NY5FC";
        public const string cLotteryIDByNYFFC = "NYFFC";
        public const string cLotteryIDByOE3FC = "OE3FC";
        public const string cLotteryIDByOEFFC = "OEFFC";
        public const string cLotteryIDByPK10 = "PK10";
        public const string cLotteryIDByQFYL11X5 = "QFYL11X5";
        public const string cLotteryIDByQFYL3FC = "QFYL3FC";
        public const string cLotteryIDByQFYL5FC = "QFYL5FC";
        public const string cLotteryIDByQFYLDJSSC = "QFYLDJSSC";
        public const string cLotteryIDByQFYLFFC = "QFYLFFC";
        public const string cLotteryIDByQFYLNY15C = "QFYLNY15C";
        public const string cLotteryIDByQFYLSE15F = "QFYLSE15F";
        public const string cLotteryIDByQFZXDJSSC = "QFZXDJSSC";
        public const string cLotteryIDByQFZXFL30M = "QFZXFL30M";
        public const string cLotteryIDByQFZXHGSSC = "QFZXHGSSC";
        public const string cLotteryIDByQFZXLD2FC = "QFZXLD2FC";
        public const string cLotteryIDByQFZXNY15C = "QFZXNY15C";
        public const string cLotteryIDByQFZXPK10 = "QFZXPK10";
        public const string cLotteryIDByQFZXQQFFC = "QFZXQQFFC";
        public const string cLotteryIDByQFZXSE15F = "QFZXSE15F";
        public const string cLotteryIDByQFZXTX30M = "QFZXTX30M";
        public const string cLotteryIDByQFZXXDLSSC = "QFZXXDLSSC";
        public const string cLotteryIDByQFZXXJPSSC = "QFZXXJPSSC";
        public const string cLotteryIDByQFZXXWYFFC = "QFZXXWYFFC";
        public const string cLotteryIDByQH11X5 = "QH11X5";
        public const string cLotteryIDByQJC11X5 = "QJC11X5";
        public const string cLotteryIDByQJC2FC = "QJC2FC";
        public const string cLotteryIDByQJC45C = "QJC45C";
        public const string cLotteryIDByQJCAM15C = "QJCAM15C";
        public const string cLotteryIDByQJCFFC = "QJCFFC";
        public const string cLotteryIDByQJCHL15C = "QJCHL15C";
        public const string cLotteryIDByQJCPDFFC = "QJCPDFFC";
        public const string cLotteryIDByQJCPK10 = "QJCPK10";
        public const string cLotteryIDByQJCTXFFC = "QJCTXFFC";
        public const string cLotteryIDByQJCXNFFC = "QJCXNFFC";
        public const string cLotteryIDByQQ15F = "QQ15F";
        public const string cLotteryIDByQQ30M = "QQ30M";
        public const string cLotteryIDByQQFFC = "QQFFC";
        public const string cLotteryIDByQQT2FC = "QQT2FC";
        public const string cLotteryIDByQQT5FC = "QQT5FC";
        public const string cLotteryIDByQQTDJSSC = "QQTDJSSC";
        public const string cLotteryIDByQQTFFC = "QQTFFC";
        public const string cLotteryIDByQQTHGPK10 = "QQTHGPK10";
        public const string cLotteryIDByQQTHGSSC = "QQTHGSSC";
        public const string cLotteryIDByQQTJNDSSC = "QQTJNDSSC";
        public const string cLotteryIDByQQTNY15C = "QQTNY15C";
        public const string cLotteryIDByQQTTG15C = "QQTTG15C";
        public const string cLotteryIDByQQTTX2FC = "QQTTX2FC";
        public const string cLotteryIDByQQTTXFFC = "QQTTXFFC";
        public const string cLotteryIDByQQTXDLSSC = "QQTXDLSSC";
        public const string cLotteryIDByQQTXXLSSC = "QQTXXLSSC";
        public const string cLotteryIDByQQYL5FC = "QQYL5FC";
        public const string cLotteryIDByQQYLFF11X5 = "QQYLFF11X5";
        public const string cLotteryIDByQQYLFFC = "QQYLFFC";
        public const string cLotteryIDByQQYLFLP45M = "QQYLFLP45M";
        public const string cLotteryIDByQQYLHG5FC = "QQYLHG5FC";
        public const string cLotteryIDByQQYLJND4FC = "QQYLJND4FC";
        public const string cLotteryIDByQQYLMG45M = "QQYLMG45M";
        public const string cLotteryIDByQQYLQQXDL45M = "QQYLQQXDL45M";
        public const string cLotteryIDByQQYLQQXDL90M = "QQYLQQXDL90M";
        public const string cLotteryIDByQQYLRB45M = "QQYLRB45M";
        public const string cLotteryIDByQYFFC = "QYFFC";
        public const string cLotteryIDByRDYL2FC = "RDYL2FC";
        public const string cLotteryIDByRDYL5FC = "RDYL5FC";
        public const string cLotteryIDByRDYLFFC = "RDYLFFC";
        public const string cLotteryIDBySD11X5 = "SD11X5";
        public const string cLotteryIDBySESSC = "SESSC";
        public const string cLotteryIDBySH11X5 = "SH11X5";
        public const string cLotteryIDBySHK3 = "SHK3";
        public const string cLotteryIDBySIJI3F11X5 = "SIJI3F11X5";
        public const string cLotteryIDBySIJI3FC = "SIJI3FC";
        public const string cLotteryIDBySIJI5F11X5 = "SIJI5F11X5";
        public const string cLotteryIDBySIJI5FC = "SIJI5FC";
        public const string cLotteryIDBySIJIDJSSC = "SIJIDJSSC";
        public const string cLotteryIDBySIJIELSSSC = "SIJIELSSSC";
        public const string cLotteryIDBySIJIFF11X5 = "SIJIFF11X5";
        public const string cLotteryIDBySIJIFFC = "SIJIFFC";
        public const string cLotteryIDBySIJIFLBSSC = "SIJIFLBSSC";
        public const string cLotteryIDBySIJIHGSSC = "SIJIHGSSC";
        public const string cLotteryIDBySIJITXYFC = "SIJITXYFC";
        public const string cLotteryIDBySKY2F11X5 = "SKY2F11X5";
        public const string cLotteryIDBySKY2FC = "SKY2FC";
        public const string cLotteryIDBySKYFFC = "SKYFFC";
        public const string cLotteryIDBySLTH2FC = "SLTH2FC";
        public const string cLotteryIDBySLTHDJSSC = "SLTHDJSSC";
        public const string cLotteryIDBySLTHFFC = "SLTHFFC";
        public const string cLotteryIDBySLTHHGSSC = "SLTHHGSSC";
        public const string cLotteryIDBySLTHNHGSSC = "SLTHNHGSSC";
        public const string cLotteryIDBySLTHTEQ15C = "SLTHTEQ15C";
        public const string cLotteryIDBySSC = "SSC";
        public const string cLotteryIDBySSHC3F11X5 = "SSHC3F11X5";
        public const string cLotteryIDBySSHC3FC = "SSHC3FC";
        public const string cLotteryIDBySSHCFF11X5 = "SSHCFF11X5";
        public const string cLotteryIDBySSHCFFC = "SSHCFFC";
        public const string cLotteryIDBySSHCPK10 = "SSHCPK10";
        public const string cLotteryIDBySSHCTXFFC = "SSHCTXFFC";
        public const string cLotteryIDBySXL11X5 = "SXL11X5";
        public const string cLotteryIDBySXR11X5 = "SXR11X5";
        public const string cLotteryIDBySYYLFFC = "SYYLFFC";
        public const string cLotteryIDByTA11X53FC = "TA11X53FC";
        public const string cLotteryIDByTA11X5FFC = "TA11X5FFC";
        public const string cLotteryIDByTA3FC = "TA3FC";
        public const string cLotteryIDByTA5FC = "TA5FC";
        public const string cLotteryIDByTABL15C = "TABL15C";
        public const string cLotteryIDByTAFFC = "TAFFC";
        public const string cLotteryIDByTAFLBSSC = "TAFLBSSC";
        public const string cLotteryIDByTAHGSSC = "TAHGSSC";
        public const string cLotteryIDByTATXFFC = "TATXFFC";
        public const string cLotteryIDByTAWBFFC = "TAWBFFC";
        public const string cLotteryIDByTAWX15C = "TAWX15C";
        public const string cLotteryIDByTBYFLP15C = "TBYFLP15C";
        public const string cLotteryIDByTBYLJND35C = "TBYLJND35C";
        public const string cLotteryIDByTBYLJND5FC = "TBYLJND5FC";
        public const string cLotteryIDByTBYLJS11X5 = "TBYLJS11X5";
        public const string cLotteryIDByTBYLMG15C = "TBYLMG15C";
        public const string cLotteryIDByTBYLNDJSSC = "TBYLNDJSSC";
        public const string cLotteryIDByTBYLNHGSSC = "TBYLNHGSSC";
        public const string cLotteryIDByTBYLNY15C = "TBYLNY15C";
        public const string cLotteryIDByTBYLQQFFC = "TBYLQQFFC";
        public const string cLotteryIDByTBYLSE15C = "TBYLSE15C";
        public const string cLotteryIDByTBYLXDLSSC = "TBYLXDLSSC";
        public const string cLotteryIDByTCYL2F11X5 = "TCYL2F11X5";
        public const string cLotteryIDByTCYL2FC = "TCYL2FC";
        public const string cLotteryIDByTCYL2FPK10 = "TCYL2FPK10";
        public const string cLotteryIDByTCYLFF11X5 = "TCYLFF11X5";
        public const string cLotteryIDByTCYLFFC = "TCYLFFC";
        public const string cLotteryIDByTCYLFFPK10 = "TCYLFFPK10";
        public const string cLotteryIDByTH5FC = "TH5FC";
        public const string cLotteryIDByTHDJSSC = "THDJSSC";
        public const string cLotteryIDByTHEN120MPK10 = "THEN120MPK10";
        public const string cLotteryIDByTHEN180MPK10 = "THEN180MPK10";
        public const string cLotteryIDByTHENDJSSC = "THENDJSSC";
        public const string cLotteryIDByTHENELSSSC = "THENELSSSC";
        public const string cLotteryIDByTHENHGSSC = "THENHGSSC";
        public const string cLotteryIDByTHENJND11X5 = "THENJND11X5";
        public const string cLotteryIDByTHENMGFFC = "THENMGFFC";
        public const string cLotteryIDByTHENNY11X5 = "THENNY11X5";
        public const string cLotteryIDByTHENXDLSSC = "THENXDLSSC";
        public const string cLotteryIDByTHENXJP30M = "THENXJP30M";
        public const string cLotteryIDByTHENXJPSSC = "THENXJPSSC";
        public const string cLotteryIDByTHENYD15C = "THENYD15C";
        public const string cLotteryIDByTHFFC = "THFFC";
        public const string cLotteryIDByTHHGSSC = "THHGSSC";
        public const string cLotteryIDByTHJDSSC = "THJDSSC";
        public const string cLotteryIDByTHMD2FC = "THMD2FC";
        public const string cLotteryIDByTHOZPK10 = "THOZPK10";
        public const string cLotteryIDByTHPK10 = "THPK10";
        public const string cLotteryIDByTHTGSSC = "THTGSSC";
        public const string cLotteryIDByTIYUFLP15C = "TIYUFLP15C";
        public const string cLotteryIDByTIYUML2FC = "TIYUML2FC";
        public const string cLotteryIDByTIYUNHG15C = "TIYUNHG15C";
        public const string cLotteryIDByTIYUXDL15C = "TIYUXDL15C";
        public const string cLotteryIDByTIYUXJPSSC = "TIYUXJPSSC";
        public const string cLotteryIDByTJ11X5 = "TJ11X5";
        public const string cLotteryIDByTJSSC = "TJSSC";
        public const string cLotteryIDByTR11X5 = "TR11X5";
        public const string cLotteryIDByTR2FC = "TR2FC";
        public const string cLotteryIDByTR45C = "TR45C";
        public const string cLotteryIDByTRFFC = "TRFFC";
        public const string cLotteryIDByTRPK10 = "TRPK10";
        public const string cLotteryIDByTWSSC = "TWSSC";
        public const string cLotteryIDByTXFFC = "TXFFC";
        public const string cLotteryIDByTYYL120MPK10 = "TYYL120MPK10";
        public const string cLotteryIDByTYYL180MPK10 = "TYYL180MPK10";
        public const string cLotteryIDByTYYL30M = "TYYL30M";
        public const string cLotteryIDByTYYL30M11X5 = "TYYL30M11X5";
        public const string cLotteryIDByTYYL90M11X5 = "TYYL90M11X5";
        public const string cLotteryIDByTYYLDJ15F = "TYYLDJ15F";
        public const string cLotteryIDByTYYLELS15F = "TYYLELS15F";
        public const string cLotteryIDByTYYLHS15F = "TYYLHS15F";
        public const string cLotteryIDByTYYLMG60M = "TYYLMG60M";
        public const string cLotteryIDByTYYLXDL15F = "TYYLXDL15F";
        public const string cLotteryIDByTYYLXJPSSC = "TYYLXJPSSC";
        public const string cLotteryIDByTYYLYD15F = "TYYLYD15F";
        public const string cLotteryIDByUC3F11X5 = "UC3F11X5";
        public const string cLotteryIDByUC5FC = "UC5FC";
        public const string cLotteryIDByUCFFC = "UCFFC";
        public const string cLotteryIDByUCHGSSC = "UCHGSSC";
        public const string cLotteryIDByUCHL2FC = "UCHL2FC";
        public const string cLotteryIDByUCRD2FC = "UCRD2FC";
        public const string cLotteryIDByUCRDFFC = "UCRDFFC";
        public const string cLotteryIDByUCTWSSC = "UCTWSSC";
        public const string cLotteryIDByUT83FC = "UT83FC";
        public const string cLotteryIDByUT8DJSSC = "UT8DJSSC";
        public const string cLotteryIDByUT8FFC = "UT8FFC";
        public const string cLotteryIDByUT8HGSSC = "UT8HGSSC";
        public const string cLotteryIDByVR3FC = "VR3FC";
        public const string cLotteryIDByVRHXSSC = "VRHXSSC";
        public const string cLotteryIDByVRKT = "VRKT";
        public const string cLotteryIDByVRMXSC = "VRMXSC";
        public const string cLotteryIDByVRPK10 = "VRPK10";
        public const string cLotteryIDByVRSSC = "VRSSC";
        public const string cLotteryIDByVRSXFFC = "VRSXFFC";
        public const string cLotteryIDByVRTXFFC = "VRTXFFC";
        public const string cLotteryIDByVRYYPK10 = "VRYYPK10";
        public const string cLotteryIDByVRZXC11X5 = "VRZXC11X5";
        public const string cLotteryIDByWBJ2FC = "WBJ2FC";
        public const string cLotteryIDByWBJ5FC = "WBJ5FC";
        public const string cLotteryIDByWBJBJSSC = "WBJBJSSC";
        public const string cLotteryIDByWBJDJSSC = "WBJDJSSC";
        public const string cLotteryIDByWBJDX15C = "WBJDX15C";
        public const string cLotteryIDByWBJFFC = "WBJFFC";
        public const string cLotteryIDByWBJHG2FC = "WBJHG2FC";
        public const string cLotteryIDByWBJHGSSC = "WBJHGSSC";
        public const string cLotteryIDByWBJJNDSSC = "WBJJNDSSC";
        public const string cLotteryIDByWBJMSK35C = "WBJMSK35C";
        public const string cLotteryIDByWBJNNPK10 = "WBJNNPK10";
        public const string cLotteryIDByWBJNY15C = "WBJNY15C";
        public const string cLotteryIDByWBJOM11X5 = "WBJOM11X5";
        public const string cLotteryIDByWBJOMPK10 = "WBJOMPK10";
        public const string cLotteryIDByWBJTG30 = "WBJTG30";
        public const string cLotteryIDByWBJXDLSSC = "WBJXDLSSC";
        public const string cLotteryIDByWBJXJPSSC = "WBJXJPSSC";
        public const string cLotteryIDByWBJXXLSSC = "WBJXXLSSC";
        public const string cLotteryIDByWCAIDJSSC = "WCAIDJSSC";
        public const string cLotteryIDByWCAIHGSSC = "WCAIHGSSC";
        public const string cLotteryIDByWCAILD2FC = "WCAILD2FC";
        public const string cLotteryIDByWCAIMBE30M = "WCAIMBE30M";
        public const string cLotteryIDByWCAITWSSC = "WCAITWSSC";
        public const string cLotteryIDByWCAIXDLSSC = "WCAIXDLSSC";
        public const string cLotteryIDByWCAIXJPSSC = "WCAIXJPSSC";
        public const string cLotteryIDByWCAIXWYFFC = "WCAIXWYFFC";
        public const string cLotteryIDByWCDBFFC = "WCDBFFC";
        public const string cLotteryIDByWCFLPFFC = "WCFLPFFC";
        public const string cLotteryIDByWCJPZFFC = "WCJPZFFC";
        public const string cLotteryIDByWCMDJB15C = "WCMDJB15C";
        public const string cLotteryIDByWCMG45M = "WCMG45M";
        public const string cLotteryIDByWCMGDZ2FC = "WCMGDZ2FC";
        public const string cLotteryIDByWCMLXY3FC = "WCMLXY3FC";
        public const string cLotteryIDByWCTWFFC = "WCTWFFC";
        public const string cLotteryIDByWCXBYFFC = "WCXBYFFC";
        public const string cLotteryIDByWCXDL15C = "WCXDL15C";
        public const string cLotteryIDByWCYGFFC = "WCYGFFC";
        public const string cLotteryIDByWDYL11X5 = "WDYL11X5";
        public const string cLotteryIDByWDYL2FC = "WDYL2FC";
        public const string cLotteryIDByWDYL5FC = "WDYL5FC";
        public const string cLotteryIDByWDYLFFC = "WDYLFFC";
        public const string cLotteryIDByWE11X5 = "WE11X5";
        public const string cLotteryIDByWE2FC = "WE2FC";
        public const string cLotteryIDByWE45C = "WE45C";
        public const string cLotteryIDByWEAM15C = "WEAM15C";
        public const string cLotteryIDByWEFFC = "WEFFC";
        public const string cLotteryIDByWEPK10 = "WEPK10";
        public const string cLotteryIDByWETXFFC = "WETXFFC";
        public const string cLotteryIDByWHC3FC = "WHC3FC";
        public const string cLotteryIDByWHCFFC = "WHCFFC";
        public const string cLotteryIDByWHDJSSC = "WHDJSSC";
        public const string cLotteryIDByWHEN11X5 = "WHEN11X5";
        public const string cLotteryIDByWHEN120MPK10 = "WHEN120MPK10";
        public const string cLotteryIDByWHEN180MPK10 = "WHEN180MPK10";
        public const string cLotteryIDByWHEN30M = "WHEN30M";
        public const string cLotteryIDByWHENBLS60M = "WHENBLS60M";
        public const string cLotteryIDByWHENDJSSC = "WHENDJSSC";
        public const string cLotteryIDByWHENELSSSC = "WHENELSSSC";
        public const string cLotteryIDByWHENFLP15C = "WHENFLP15C";
        public const string cLotteryIDByWHENFS15F = "WHENFS15F";
        public const string cLotteryIDByWHENHGSSC = "WHENHGSSC";
        public const string cLotteryIDByWHENHS15F = "WHENHS15F";
        public const string cLotteryIDByWHENNDJSSC = "WHENNDJSSC";
        public const string cLotteryIDByWHENQQFFC = "WHENQQFFC";
        public const string cLotteryIDByWHENXDL15F = "WHENXDL15F";
        public const string cLotteryIDByWHENXJPSSC = "WHENXJPSSC";
        public const string cLotteryIDByWHENYG60M = "WHENYG60M";
        public const string cLotteryIDByWHXJPSSC = "WHXJPSSC";
        public const string cLotteryIDByWJH5FC = "WJH5FC";
        public const string cLotteryIDByWM2FC = "WM2FC";
        public const string cLotteryIDByWMFFC = "WMFFC";
        public const string cLotteryIDByWMHGSSC = "WMHGSSC";
        public const string cLotteryIDByWMPK10 = "WMPK10";
        public const string cLotteryIDByWMTWSSC = "WMTWSSC";
        public const string cLotteryIDByWMXXLSSC = "WMXXLSSC";
        public const string cLotteryIDByWS11X5 = "WS11X5";
        public const string cLotteryIDByWS120MPK10 = "WS120MPK10";
        public const string cLotteryIDByWS180MPK10 = "WS180MPK10";
        public const string cLotteryIDByWS30M = "WS30M";
        public const string cLotteryIDByWSBLS60M = "WSBLS60M";
        public const string cLotteryIDByWSELSSSC = "WSELSSSC";
        public const string cLotteryIDByWSFLP15C = "WSFLP15C";
        public const string cLotteryIDByWSFS15F = "WSFS15F";
        public const string cLotteryIDByWSHS15F = "WSHS15F";
        public const string cLotteryIDByWSNDJSSC = "WSNDJSSC";
        public const string cLotteryIDByWSQQFFC = "WSQQFFC";
        public const string cLotteryIDByWSXDL15F = "WSXDL15F";
        public const string cLotteryIDByWTYLAZ5FC = "WTYLAZ5FC";
        public const string cLotteryIDByWTYLHG15C = "WTYLHG15C";
        public const string cLotteryIDByWTYLJNDSSC = "WTYLJNDSSC";
        public const string cLotteryIDByWTYLNY2FC = "WTYLNY2FC";
        public const string cLotteryIDByWTYLTXFFC = "WTYLTXFFC";
        public const string cLotteryIDByWTYLXDL35C = "WTYLXDL35C";
        public const string cLotteryIDByWTYLXXL15C = "WTYLXXL15C";
        public const string cLotteryIDByWX11X5 = "WX11X5";
        public const string cLotteryIDByWX15F = "WX15F";
        public const string cLotteryIDByWX3FC = "WX3FC";
        public const string cLotteryIDByWX5FC = "WX5FC";
        public const string cLotteryIDByWXFFC = "WXFFC";
        public const string cLotteryIDByWY2F11X5 = "WY2F11X5";
        public const string cLotteryIDByWY2FC = "WY2FC";
        public const string cLotteryIDByWY2FPK10 = "WY2FPK10";
        public const string cLotteryIDByWY3F11X5 = "WY3F11X5";
        public const string cLotteryIDByWY3FC = "WY3FC";
        public const string cLotteryIDByWY3FPK10 = "WY3FPK10";
        public const string cLotteryIDByWYFF11X5 = "WYFF11X5";
        public const string cLotteryIDByWYFFC = "WYFFC";
        public const string cLotteryIDByWYFFPK10 = "WYFFPK10";
        public const string cLotteryIDByWZYLBL11X5 = "WZYLBL11X5";
        public const string cLotteryIDByWZYLBL1FC = "WZYLBL1FC";
        public const string cLotteryIDByWZYLDJSSC = "WZYLDJSSC";
        public const string cLotteryIDByWZYLHG1FC = "WZYLHG1FC";
        public const string cLotteryIDByWZYLHGSSC = "WZYLHGSSC";
        public const string cLotteryIDByWZYLJDSSC = "WZYLJDSSC";
        public const string cLotteryIDByWZYLJSPK10 = "WZYLJSPK10";
        public const string cLotteryIDByWZYLLSWJS15C = "WZYLLSWJS15C";
        public const string cLotteryIDByWZYLMG15C = "WZYLMG15C";
        public const string cLotteryIDByWZYLMG35C = "WZYLMG35C";
        public const string cLotteryIDByWZYLTG11X5 = "WZYLTG11X5";
        public const string cLotteryIDByWZYLTXFFC = "WZYLTXFFC";
        public const string cLotteryIDByWZYLXGSSC = "WZYLXGSSC";
        public const string cLotteryIDByWZYLXJPSSC = "WZYLXJPSSC";
        public const string cLotteryIDByXB3FC = "XB3FC";
        public const string cLotteryIDByXB5FC = "XB5FC";
        public const string cLotteryIDByXBFFC = "XBFFC";
        public const string cLotteryIDByXC11X53FC = "XC11X53FC";
        public const string cLotteryIDByXC11X5FFC = "XC11X5FFC";
        public const string cLotteryIDByXCBL15C = "XCBL15C";
        public const string cLotteryIDByXCFLBSSC = "XCFLBSSC";
        public const string cLotteryIDByXCHGSSC = "XCHGSSC";
        public const string cLotteryIDByXCQQFFC = "XCQQFFC";
        public const string cLotteryIDByXCTXFFC = "XCTXFFC";
        public const string cLotteryIDByXCWX15C = "XCWX15C";
        public const string cLotteryIDByXCYN3FC = "XCYN3FC";
        public const string cLotteryIDByXCYN5FC = "XCYN5FC";
        public const string cLotteryIDByXCYNFFC = "XCYNFFC";
        public const string cLotteryIDByXDB2F11X5 = "XDB2F11X5";
        public const string cLotteryIDByXDB2FC = "XDB2FC";
        public const string cLotteryIDByXDB2FPK10 = "XDB2FPK10";
        public const string cLotteryIDByXDB3F11X5 = "XDB3F11X5";
        public const string cLotteryIDByXDB3FC = "XDB3FC";
        public const string cLotteryIDByXDB3FPK10 = "XDB3FPK10";
        public const string cLotteryIDByXDB5F11X5 = "XDB5F11X5";
        public const string cLotteryIDByXDB5FC = "XDB5FC";
        public const string cLotteryIDByXDB5FPK10 = "XDB5FPK10";
        public const string cLotteryIDByXDBDJSSC = "XDBDJSSC";
        public const string cLotteryIDByXDBFF11X5 = "XDBFF11X5";
        public const string cLotteryIDByXDBFFC = "XDBFFC";
        public const string cLotteryIDByXDBFFPK10 = "XDBFFPK10";
        public const string cLotteryIDByXDBHGSSC = "XDBHGSSC";
        public const string cLotteryIDByXDL11X5 = "XDL11X5";
        public const string cLotteryIDByXDL90M = "XDL90M";
        public const string cLotteryIDByXDLPK10 = "XDLPK10";
        public const string cLotteryIDByXDLSSC = "XDLSSC";
        public const string cLotteryIDByXGLLDJSSC = "XGLLDJSSC";
        public const string cLotteryIDByXGLLFLP15C = "XGLLFLP15C";
        public const string cLotteryIDByXGLLL2FC = "XGLLL2FC";
        public const string cLotteryIDByXGLLLDPK10 = "XGLLLDPK10";
        public const string cLotteryIDByXGLLLFFC = "XGLLLFFC";
        public const string cLotteryIDByXGLLLSJ2FC = "XGLLLSJ2FC";
        public const string cLotteryIDByXGLLLSY11X5 = "XGLLLSY11X5";
        public const string cLotteryIDByXGLLSSPK10 = "XGLLSSPK10";
        public const string cLotteryIDByXGLLWNS15C = "XGLLWNS15C";
        public const string cLotteryIDByXGLLWYN30M = "XGLLWYN30M";
        public const string cLotteryIDByXGSM = "XGSM";
        public const string cLotteryIDByXHDF5FC = "XHDF5FC";
        public const string cLotteryIDByXHDFDJSSC = "XHDFDJSSC";
        public const string cLotteryIDByXHDFFFC = "XHDFFFC";
        public const string cLotteryIDByXHDFHGSSC = "XHDFHGSSC";
        public const string cLotteryIDByXHDFTXFFC = "XHDFTXFFC";
        public const string cLotteryIDByXHHCOZ3FC = "XHHCOZ3FC";
        public const string cLotteryIDByXHHCSLFK5FC = "XHHCSLFK5FC";
        public const string cLotteryIDByXHSD5FC = "XHSD5FC";
        public const string cLotteryIDByXHSDFF11X5 = "XHSDFF11X5";
        public const string cLotteryIDByXHSDFFC = "XHSDFFC";
        public const string cLotteryIDByXHSDTXFFC = "XHSDTXFFC";
        public const string cLotteryIDByXJ11X5 = "XJ11X5";
        public const string cLotteryIDByXJP120M = "XJP120M";
        public const string cLotteryIDByXJP15F = "XJP15F";
        public const string cLotteryIDByXJPSSC = "XJPSSC";
        public const string cLotteryIDByXJSSC = "XJSSC";
        public const string cLotteryIDByXQYL11X5 = "XQYL11X5";
        public const string cLotteryIDByXQYL3FC = "XQYL3FC";
        public const string cLotteryIDByXQYL5FC = "XQYL5FC";
        public const string cLotteryIDByXQYLBDFFC = "XQYLBDFFC";
        public const string cLotteryIDByXQYLDJSSC = "XQYLDJSSC";
        public const string cLotteryIDByXQYLFFC = "XQYLFFC";
        public const string cLotteryIDByXQYLHGSSC = "XQYLHGSSC";
        public const string cLotteryIDByXQYLJS11X5 = "XQYLJS11X5";
        public const string cLotteryIDByXQYLJSPK10 = "XQYLJSPK10";
        public const string cLotteryIDByXQYLNY15C = "XQYLNY15C";
        public const string cLotteryIDByXQYLQQFFC = "XQYLQQFFC";
        public const string cLotteryIDByXQYLSE15F = "XQYLSE15F";
        public const string cLotteryIDByXQYLWBFFC = "XQYLWBFFC";
        public const string cLotteryIDByXTYLDJSSC = "XTYLDJSSC";
        public const string cLotteryIDByXTYLDLD30M = "XTYLDLD30M";
        public const string cLotteryIDByXTYLHGSSC = "XTYLHGSSC";
        public const string cLotteryIDByXTYLLD2FC = "XTYLLD2FC";
        public const string cLotteryIDByXTYLPK10 = "XTYLPK10";
        public const string cLotteryIDByXTYLXDLSSC = "XTYLXDLSSC";
        public const string cLotteryIDByXTYLXJPSSC = "XTYLXJPSSC";
        public const string cLotteryIDByXTYLXWYFFC = "XTYLXWYFFC";
        public const string cLotteryIDByXWYLDJSSC = "XWYLDJSSC";
        public const string cLotteryIDByXWYLFL30M = "XWYLFL30M";
        public const string cLotteryIDByXWYLHGSSC = "XWYLHGSSC";
        public const string cLotteryIDByXWYLLD2FC = "XWYLLD2FC";
        public const string cLotteryIDByXWYLNY15C = "XWYLNY15C";
        public const string cLotteryIDByXWYLQQFFC = "XWYLQQFFC";
        public const string cLotteryIDByXWYLSE15F = "XWYLSE15F";
        public const string cLotteryIDByXWYLTX30M = "XWYLTX30M";
        public const string cLotteryIDByXWYLXDLSSC = "XWYLXDLSSC";
        public const string cLotteryIDByXWYLXJPSSC = "XWYLXJPSSC";
        public const string cLotteryIDByXWYLXWYFFC = "XWYLXWYFFC";
        public const string cLotteryIDByXXLSSC = "XXLSSC";
        public const string cLotteryIDByXYFTPK10 = "XYFTPK10";
        public const string cLotteryIDByYB3FC = "YB3FC";
        public const string cLotteryIDByYBAO2FC = "YBAO2FC";
        public const string cLotteryIDByYBAO45C = "YBAO45C";
        public const string cLotteryIDByYBAOFFC = "YBAOFFC";
        public const string cLotteryIDByYBBLSFFC = "YBBLSFFC";
        public const string cLotteryIDByYBDJSSC = "YBDJSSC";
        public const string cLotteryIDByYBFFC = "YBFFC";
        public const string cLotteryIDByYBHG15F = "YBHG15F";
        public const string cLotteryIDByYBNHG15F = "YBNHG15F";
        public const string cLotteryIDByYBTXFFC = "YBTXFFC";
        public const string cLotteryIDByYBXDL15F = "YBXDL15F";
        public const string cLotteryIDByYC2FC = "YC2FC";
        public const string cLotteryIDByYCFFC = "YCFFC";
        public const string cLotteryIDByYCYL5FC = "YCYL5FC";
        public const string cLotteryIDByYCYLFF11X5 = "YCYLFF11X5";
        public const string cLotteryIDByYCYLFFC = "YCYLFFC";
        public const string cLotteryIDByYCYLTXFFC = "YCYLTXFFC";
        public const string cLotteryIDByYD2FC = "YD2FC";
        public const string cLotteryIDByYDFFC = "YDFFC";
        public const string cLotteryIDByYDFFPK10 = "YDFFPK10";
        public const string cLotteryIDByYDHGSSC = "YDHGSSC";
        public const string cLotteryIDByYDTXFFC = "YDTXFFC";
        public const string cLotteryIDByYDXXLSSC = "YDXXLSSC";
        public const string cLotteryIDByYHSGDJSSC = "YHSGDJSSC";
        public const string cLotteryIDByYHSGHGSSC = "YHSGHGSSC";
        public const string cLotteryIDByYHSGLD2FC = "YHSGLD2FC";
        public const string cLotteryIDByYHSGXDLSSC = "YHSGXDLSSC";
        public const string cLotteryIDByYHSGXJPSSC = "YHSGXJPSSC";
        public const string cLotteryIDByYHSGXWYFFC = "YHSGXWYFFC";
        public const string cLotteryIDByYHYLBD45M = "YHYLBD45M";
        public const string cLotteryIDByYHYLBLSFFC = "YHYLBLSFFC";
        public const string cLotteryIDByYHYLFLP2FC = "YHYLFLP2FC";
        public const string cLotteryIDByYHYLFLP45M = "YHYLFLP45M";
        public const string cLotteryIDByYHYLFLPFFC = "YHYLFLPFFC";
        public const string cLotteryIDByYHYLJNDFFC = "YHYLJNDFFC";
        public const string cLotteryIDByYHYLQQFFC = "YHYLQQFFC";
        public const string cLotteryIDByYHYLTXFFC = "YHYLTXFFC";
        public const string cLotteryIDByYHYLWXFFC = "YHYLWXFFC";
        public const string cLotteryIDByYHYLXDL15F = "YHYLXDL15F";
        public const string cLotteryIDByYHYLXHG15F = "YHYLXHG15F";
        public const string cLotteryIDByYHYLXHG45M = "YHYLXHG45M";
        public const string cLotteryIDByYIFA5FC = "YIFA5FC";
        public const string cLotteryIDByYIFAFFC = "YIFAFFC";
        public const string cLotteryIDByYINH2FC = "YINH2FC";
        public const string cLotteryIDByYINH5FC = "YINH5FC";
        public const string cLotteryIDByYINHDJSSC = "YINHDJSSC";
        public const string cLotteryIDByYINHHGSSC = "YINHHGSSC";
        public const string cLotteryIDByYINHQQFFC = "YINHQQFFC";
        public const string cLotteryIDByYINHTXFFC = "YINHTXFFC";
        public const string cLotteryIDByYL2028DJSSC = "YL2028DJSSC";
        public const string cLotteryIDByYL2028DZ30M = "YL2028DZ30M";
        public const string cLotteryIDByYL2028FFPK10 = "YL2028FFPK10";
        public const string cLotteryIDByYL2028FLP2FC = "YL2028FLP2FC";
        public const string cLotteryIDByYL2028HGSSC = "YL2028HGSSC";
        public const string cLotteryIDByYL2028MG45M = "YL2028MG45M";
        public const string cLotteryIDByYL2028ML20M = "YL2028ML20M";
        public const string cLotteryIDByYL2028PK10 = "YL2028PK10";
        public const string cLotteryIDByYL2028WXFFC = "YL2028WXFFC";
        public const string cLotteryIDByYN11X5 = "YN11X5";
        public const string cLotteryIDByYNSSC = "YNSSC";
        public const string cLotteryIDByYR2FC = "YR2FC";
        public const string cLotteryIDByYR5FC = "YR5FC";
        public const string cLotteryIDByYRALFFC = "YRALFFC";
        public const string cLotteryIDByYRFFC = "YRFFC";
        public const string cLotteryIDByYRHG15C = "YRHG15C";
        public const string cLotteryIDByYRTXFFC = "YRTXFFC";
        public const string cLotteryIDByYSEN2FC = "YSEN2FC";
        public const string cLotteryIDByYSEN5FC = "YSEN5FC";
        public const string cLotteryIDByYSENBJSSC = "YSENBJSSC";
        public const string cLotteryIDByYSENDJ35C = "YSENDJ35C";
        public const string cLotteryIDByYSENDJSSC = "YSENDJSSC";
        public const string cLotteryIDByYSENDL2FC = "YSENDL2FC";
        public const string cLotteryIDByYSENDM45C = "YSENDM45C";
        public const string cLotteryIDByYSENFFC = "YSENFFC";
        public const string cLotteryIDByYSENHGSSC = "YSENHGSSC";
        public const string cLotteryIDByYSENJNDSSC = "YSENJNDSSC";
        public const string cLotteryIDByYSENJZ11X5 = "YSENJZ11X5";
        public const string cLotteryIDByYSENJZ15C = "YSENJZ15C";
        public const string cLotteryIDByYSENLT15C = "YSENLT15C";
        public const string cLotteryIDByYSENTBPK10 = "YSENTBPK10";
        public const string cLotteryIDByYSENXDLSSC = "YSENXDLSSC";
        public const string cLotteryIDByYSENXGBFC = "YSENXGBFC";
        public const string cLotteryIDByYSENXGPK10 = "YSENXGPK10";
        public const string cLotteryIDByYSENXGSSC = "YSENXGSSC";
        public const string cLotteryIDByYSENXJPSSC = "YSENXJPSSC";
        public const string cLotteryIDByYTXJPSSC = "YTXJPSSC";
        public const string cLotteryIDByYXZXFLP15C = "YXZXFLP15C";
        public const string cLotteryIDByYXZXHN45M = "YXZXHN45M";
        public const string cLotteryIDByYXZXJND35C = "YXZXJND35C";
        public const string cLotteryIDByYXZXJND5FC = "YXZXJND5FC";
        public const string cLotteryIDByYXZXMG15C = "YXZXMG15C";
        public const string cLotteryIDByYXZXMG45M = "YXZXMG45M";
        public const string cLotteryIDByYXZXNDJSSC = "YXZXNDJSSC";
        public const string cLotteryIDByYXZXNHGSSC = "YXZXNHGSSC";
        public const string cLotteryIDByYXZXNY15C = "YXZXNY15C";
        public const string cLotteryIDByYXZXQQFFC = "YXZXQQFFC";
        public const string cLotteryIDByYXZXSE15C = "YXZXSE15C";
        public const string cLotteryIDByYXZXTW11X5 = "YXZXTW11X5";
        public const string cLotteryIDByYXZXTWPK10 = "YXZXTWPK10";
        public const string cLotteryIDByYXZXXDLSSC = "YXZXXDLSSC";
        public const string cLotteryIDByYXZXXXLSSC = "YXZXXXLSSC";
        public const string cLotteryIDByYZCPBL1FC = "YZCPBL1FC";
        public const string cLotteryIDByYZCPHG1FC = "YZCPHG1FC";
        public const string cLotteryIDByYZCPHGSSC = "YZCPHGSSC";
        public const string cLotteryIDByYZCPJDSSC = "YZCPJDSSC";
        public const string cLotteryIDByYZCPJSFFC = "YZCPJSFFC";
        public const string cLotteryIDByYZCPJSPK10 = "YZCPJSPK10";
        public const string cLotteryIDByYZCPLSWJS15C = "YZCPLSWJS15C";
        public const string cLotteryIDByYZCPMG15C = "YZCPMG15C";
        public const string cLotteryIDByYZCPMG35C = "YZCPMG35C";
        public const string cLotteryIDByYZCPRBSSC = "YZCPRBSSC";
        public const string cLotteryIDByYZCPTG11X5 = "YZCPTG11X5";
        public const string cLotteryIDByYZCPTG15C = "YZCPTG15C";
        public const string cLotteryIDByYZCPTG30M = "YZCPTG30M";
        public const string cLotteryIDByYZCPXG11X5 = "YZCPXG11X5";
        public const string cLotteryIDByYZCPXJPSSC = "YZCPXJPSSC";
        public const string cLotteryIDByYZCPYN5FC = "YZCPYN5FC";
        public const string cLotteryIDByZBEI2F11X5 = "ZBEI2F11X5";
        public const string cLotteryIDByZBEI2FPK10 = "ZBEI2FPK10";
        public const string cLotteryIDByZBEI30M = "ZBEI30M";
        public const string cLotteryIDByZBEI3F11X5 = "ZBEI3F11X5";
        public const string cLotteryIDByZBEI3FC = "ZBEI3FC";
        public const string cLotteryIDByZBEI3FPK10 = "ZBEI3FPK10";
        public const string cLotteryIDByZBEI45M = "ZBEI45M";
        public const string cLotteryIDByZBEI5F11X5 = "ZBEI5F11X5";
        public const string cLotteryIDByZBEI5FPK10 = "ZBEI5FPK10";
        public const string cLotteryIDByZBEIDJSSC = "ZBEIDJSSC";
        public const string cLotteryIDByZBEIFF11X5 = "ZBEIFF11X5";
        public const string cLotteryIDByZBEIFFC = "ZBEIFFC";
        public const string cLotteryIDByZBEIFFPK10 = "ZBEIFFPK10";
        public const string cLotteryIDByZBEIGGFFC = "ZBEIGGFFC";
        public const string cLotteryIDByZBEIHGSSC = "ZBEIHGSSC";
        public const string cLotteryIDByZBEIJNDSSC = "ZBEIJNDSSC";
        public const string cLotteryIDByZBEIQQFFC = "ZBEIQQFFC";
        public const string cLotteryIDByZBEISLFK5FC = "ZBEISLFK5FC";
        public const string cLotteryIDByZBEIWXFFC = "ZBEIWXFFC";
        public const string cLotteryIDByZBEIXDL15C = "ZBEIXDL15C";
        public const string cLotteryIDByZBEIXJP15C = "ZBEIXJP15C";
        public const string cLotteryIDByZBEIXJP2FC = "ZBEIXJP2FC";
        public const string cLotteryIDByZBYL2F11X5 = "ZBYL2F11X5";
        public const string cLotteryIDByZBYL2FC = "ZBYL2FC";
        public const string cLotteryIDByZBYL3F11X5 = "ZBYL3F11X5";
        public const string cLotteryIDByZBYL3FC = "ZBYL3FC";
        public const string cLotteryIDByZBYL5F11X5 = "ZBYL5F11X5";
        public const string cLotteryIDByZBYL5FC = "ZBYL5FC";
        public const string cLotteryIDByZBYLFF11X5 = "ZBYLFF11X5";
        public const string cLotteryIDByZBYLFFC = "ZBYLFFC";
        public const string cLotteryIDByZBYLGGFFC = "ZBYLGGFFC";
        public const string cLotteryIDByZBYLTX2FC = "ZBYLTX2FC";
        public const string cLotteryIDByZBYZCP15C = "ZBYZCP15C";
        public const string cLotteryIDByZD5FC = "ZD5FC";
        public const string cLotteryIDByZDDJSSC = "ZDDJSSC";
        public const string cLotteryIDByZDFFC = "ZDFFC";
        public const string cLotteryIDByZDHS15F = "ZDHS15F";
        public const string cLotteryIDByZDRBSSC = "ZDRBSSC";
        public const string cLotteryIDByZDTGSSC = "ZDTGSSC";
        public const string cLotteryIDByZJ11X5 = "ZJ11X5";
        public const string cLotteryIDByZLJDJSSC = "ZLJDJSSC";
        public const string cLotteryIDByZLJELS5FC = "ZLJELS5FC";
        public const string cLotteryIDByZLJFF11X5 = "ZLJFF11X5";
        public const string cLotteryIDByZLJFFPK10 = "ZLJFFPK10";
        public const string cLotteryIDByZLJFLP2FC = "ZLJFLP2FC";
        public const string cLotteryIDByZLJMGFFC = "ZLJMGFFC";
        public const string cLotteryIDByZLJSE15C = "ZLJSE15C";
        public const string cLotteryIDByZXBX3FC = "ZXBX3FC";
        public const string cLotteryIDByZXBX5FC = "ZXBX5FC";
        public const string cLotteryIDByZXDJSSC = "ZXDJSSC";
        public const string cLotteryIDByZXFF11X5 = "ZXFF11X5";
        public const string cLotteryIDByZXGBFFC = "ZXGBFFC";
        public const string cLotteryIDByZXHG2FC = "ZXHG2FC";
        public const string cLotteryIDByZXHGSSC = "ZXHGSSC";
        public const string cLotteryIDByZXJNDSSC = "ZXJNDSSC";
        public const string cLotteryIDByZXML15F = "ZXML15F";
        public const string cLotteryIDByZXTG15F = "ZXTG15F";
        public const string cLotteryIDByZXXDLSSC = "ZXXDLSSC";
        public const string cLotteryIDByZXXJP2FC = "ZXXJP2FC";
        public const string cLotteryIDByZXXXLSSC = "ZXXXLSSC";
        public const string cLotteryIDByZYL5FC = "ZYL5FC";
        public const string cLotteryIDByZYLDJSSC = "ZYLDJSSC";
        public const string cLotteryIDByZYLFFC = "ZYLFFC";
        public const string cLotteryIDByZYLHS15F = "ZYLHS15F";
        public const string cLotteryIDByZYLRBSSC = "ZYLRBSSC";
        public const string cLotteryIDByZYLTGSSC = "ZYLTGSSC";
        public const string cLotteryNameBy11X5 = "11选5";
        public const string cLotteryNameBy3D = "3D";
        public const string cLotteryNameByFT = "飞艇";
        public const string cLotteryNameByKT = "快艇";
        public const string cLotteryNameByPK = "PK10";
        public const string cLotteryNameByPK10 = "北京赛车PK10";
        public const string cLotteryNameBySC = "赛车";
        public const string cLotteryNameBySG = "赛狗";
        public const string cLotteryNameBySM = "赛马";
        public const string cLotteryNameBySSC = "时时彩";
        public const string cLotteryNameByYY = "游泳";
        public const string cLotteryNameByZXC = "自行车";
        public const string cLSData = @"\LSData\";
        public static CLYL CLYLInfo;
        public const int cMaxLen = 0x10;
        public const string cNextExpect = "距第 {0} 期开奖：";
        public const string cNoError = "无";
        public const string cNoFindFile = "该文件无效或者已被删除";
        public const string cNoFindPath = "该目录无效或者已被删除";
        public const string cNotLogin = "未登录";
        public static Dictionary<string, List<string>> CombinaDicPK10HZ;
        public const string cOpenCode = @"\OpenCode\";
        public const string cOpenRightDLL = @"\MdiTabControl.dll\";
        public static CP361 CP361Info;
        public const string cPlanScheme = @"\PlanScheme\";
        public const string cPlanTimesErrorHint = "该计划不适合投注！";
        public const string cPlayCGJ = "猜冠军猜冠军";
        public const string cPlayCQ2DS = "猜前二单式";
        public const string cPlayCQ2FS = "猜前二复式";
        public const string cPlayCQ2HZ = "猜前二和值";
        public const string cPlayCQ3DS = "猜前三单式";
        public const string cPlayCQ3FS = "猜前三复式";
        public const string cPlayCQ3HZ = "猜前三和值";
        public const string cPlayCQ4DS = "猜前四单式";
        public const string cPlayCQ4FS = "猜前四复式";
        public const string cPlayCQ5DS = "猜前五单式";
        public const string cPlayCQ5FS = "猜前五复式";
        public const string cPlayDa = "大";
        public const string cPlayDan = "单";
        public const string cPlayDanSi = "单式";
        public const string cPlayDS = "单双";
        public const string cPlayDW1 = "定位胆万位";
        public const string cPlayDW2 = "定位胆千位";
        public const string cPlayDW3 = "定位胆百位";
        public const string cPlayDW4 = "定位胆十位";
        public const string cPlayDW5 = "定位胆个位";
        public const string cPlayDWD = "定位胆";
        public const string cPlayDWD1 = "定位胆冠军";
        public const string cPlayDWD10 = "定位胆第十名";
        public const string cPlayDWD2 = "定位胆亚军";
        public const string cPlayDWD3 = "定位胆第三名";
        public const string cPlayDWD4 = "定位胆第四名";
        public const string cPlayDWD5 = "定位胆第五名";
        public const string cPlayDWD6 = "定位胆第六名";
        public const string cPlayDWD7 = "定位胆第七名";
        public const string cPlayDWD8 = "定位胆第八名";
        public const string cPlayDWD9 = "定位胆第九名";
        public const string cPlayDX = "大小";
        public const string cPlayFuSi = "复式";
        public const string cPlayGJ = "冠军";
        public const string cPlayH2ZXDS = "后二直选单式";
        public const string cPlayH2ZXFS = "后二直选复式";
        public const string cPlayH3Z3DS = "后三组三单式";
        public const string cPlayH3Z3FS = "后三组三复式";
        public const string cPlayH3Z6DS = "后三组六单式";
        public const string cPlayH3Z6FS = "后三组六复式";
        public const string cPlayH3ZXDS = "后三直选单式";
        public const string cPlayH3ZXFS = "后三直选复式";
        public const string cPlayH4ZXDS = "后四直选单式";
        public const string cPlayH4ZXFS = "后四直选复式";
        public const string cPlayHe = "和";
        public const string cPlayHu = "虎";
        public const string cPlayHX = "后四";
        public const string cPlayHZ = "和值";
        public const string cPlayLH = "龙虎";
        public const string cPlayLH12 = "龙虎万千";
        public const string cPlayLH13 = "龙虎万百";
        public const string cPlayLH14 = "龙虎万十";
        public const string cPlayLH15 = "龙虎万个";
        public const string cPlayLH23 = "龙虎千百";
        public const string cPlayLH24 = "龙虎千十";
        public const string cPlayLH25 = "龙虎千个";
        public const string cPlayLH34 = "龙虎百十";
        public const string cPlayLH35 = "龙虎百个";
        public const string cPlayLH45 = "龙虎十个";
        public const string cPlayLHString = "龙虎和";
        public const string cPlayLong = "龙";
        public const string cPlayQ2ZXDS = "前二直选单式";
        public const string cPlayQ2ZXFS = "前二直选复式";
        public const string cPlayQ3Z3DS = "前三组三单式";
        public const string cPlayQ3Z3FS = "前三组三复式";
        public const string cPlayQ3Z6DS = "前三组六单式";
        public const string cPlayQ3Z6FS = "前三组六复式";
        public const string cPlayQ3ZXDS = "前三直选单式";
        public const string cPlayQ3ZXFS = "前三直选复式";
        public const string cPlayQ4ZXDS = "前四直选单式";
        public const string cPlayQ4ZXFS = "前四直选复式";
        public const string cPlayQX = "前四";
        public const string cPlayR2ZXDS = "任二直选单式";
        public const string cPlayR2ZXFS = "任二直选复式";
        public const string cPlayR3Z3DS = "任三组三单式";
        public const string cPlayR3Z3FS = "任三组三复式";
        public const string cPlayR3Z6DS = "任三组六单式";
        public const string cPlayR3Z6FS = "任三组六复式";
        public const string cPlayR3ZXDS = "任三直选单式";
        public const string cPlayR3ZXFS = "任三直选复式";
        public const string cPlayR4ZXDS = "任四直选单式";
        public const string cPlayR4ZXFS = "任四直选复式";
        public const string cPlayRen = "任";
        public const string cPlayRenX = "任选";
        public const string cPlayRX = "任四";
        public const string cPlayRX1DS = "任选单式一中一";
        public const string cPlayRX1FS = "任选复式一中一";
        public const string cPlayRX2DS = "任选单式二中二";
        public const string cPlayRX2FS = "任选复式二中二";
        public const string cPlayRX3DS = "任选单式三中三";
        public const string cPlayRX3FS = "任选复式三中三";
        public const string cPlayRX4DS = "任选单式四中四";
        public const string cPlayRX4FS = "任选复式四中四";
        public const string cPlayRX5DS = "任选单式五中五";
        public const string cPlayRX5FS = "任选复式五中五";
        public const string cPlayShuang = "双";
        public const string cPlaySX = "四星";
        public const string cPlayWX = "五星";
        public const string cPlayWXZXDS = "五星直选单式";
        public const string cPlayWXZXFS = "五星直选复式";
        public const string cPlayXiao = "小";
        public const string cPlayYJ = "亚军";
        public const string cPlayZ3 = "组三";
        public const string cPlayZ3Z3DS = "中三组三单式";
        public const string cPlayZ3Z3FS = "中三组三复式";
        public const string cPlayZ3Z6DS = "中三组六单式";
        public const string cPlayZ3Z6FS = "中三组六复式";
        public const string cPlayZ3ZXDS = "中三直选单式";
        public const string cPlayZ3ZXFS = "中三直选复式";
        public const string cPlayZ6 = "组六";
        public const string cPlayZXDS = "直选单式";
        public const string cPlayZXFS = "直选复式";
        public const string cPTIDByA6YL = "A6YL";
        public const string cPTIDByALGJ = "ALGJ";
        public const string cPTIDByAMBLR = "AMBLR";
        public const string cPTIDByB6YL = "B6YL";
        public const string cPTIDByBAYL = "BAYL";
        public const string cPTIDByBHGJ = "BHGJ";
        public const string cPTIDByBHZY = "BHZY";
        public const string cPTIDByBKC = "BKC";
        public const string cPTIDByBLGJ = "BLGJ";
        public const string cPTIDByBMEI = "BMEI";
        public const string cPTIDByBMYX = "BMYX";
        public const string cPTIDByBNGJ = "BNGJ";
        public const string cPTIDByBWT = "BWT";
        public const string cPTIDByCAIH = "CAIH";
        public const string cPTIDByCBL = "CBL";
        public const string cPTIDByCCYL = "CCYL";
        public const string cPTIDByCLYL = "CLYL";
        public const string cPTIDByCP361 = "CP361";
        public const string cPTIDByCTT = "CTT";
        public const string cPTIDByCTX = "CTX";
        public const string cPTIDByCYYL = "CYYL";
        public const string cPTIDByDACP = "DACP";
        public const string cPTIDByDAZYL = "DAZYL";
        public const string cPTIDByDEJI = "DEJI";
        public const string cPTIDByDJYL = "DJYL";
        public const string cPTIDByDPC = "DPC";
        public const string cPTIDByDQYL = "DQYL";
        public const string cPTIDByDTCP = "DTCP";
        public const string cPTIDByDYYL = "DYYL";
        public const string cPTIDByFCYL = "FCYL";
        public const string cPTIDByFEIC = "FEIC";
        public const string cPTIDByFLC = "FLC";
        public const string cPTIDByFNYX = "FNYX";
        public const string cPTIDByFSYL = "FSYL";
        public const string cPTIDByGJYL = "GJYL";
        public const string cPTIDByHANY = "HANY";
        public const string cPTIDByHBS = "HBS";
        public const string cPTIDByHCYL = "HCYL";
        public const string cPTIDByHCZX = "HCZX";
        public const string cPTIDByHDYL = "HDYL";
        public const string cPTIDByHEND = "HEND";
        public const string cPTIDByHENR = "HENR";
        public const string cPTIDByHGDB = "HGDB";
        public const string cPTIDByHKC = "HKC";
        public const string cPTIDByHLC = "HLC";
        public const string cPTIDByHNYL = "HNYL";
        public const string cPTIDByHOND = "HOND";
        public const string cPTIDByHRCP = "HRCP";
        public const string cPTIDByHSGJ = "HSGJ";
        public const string cPTIDByHSYL = "HSYL";
        public const string cPTIDByHUAY = "HUAY";
        public const string cPTIDByHUBO = "HUBO";
        public const string cPTIDByHUIZ = "HUIZ";
        public const string cPTIDByHYYL = "HYYL";
        public const string cPTIDByHZYL = "HZYL";
        public const string cPTIDByJCX = "JCX";
        public const string cPTIDByJFYL = "JFYL";
        public const string cPTIDByJHC = "JHC";
        public const string cPTIDByJHC2 = "JHC2";
        public const string cPTIDByJHYL = "JHYL";
        public const string cPTIDByJLGJ = "JLGJ";
        public const string cPTIDByJXIN = "JXIN";
        public const string cPTIDByJXYL = "JXYL";
        public const string cPTIDByJYGJ = "JYGJ";
        public const string cPTIDByJYYL = "JYYL";
        public const string cPTIDByK3YL = "K3YL";
        public const string cPTIDByK5YL = "K5YL";
        public const string cPTIDByKSYL = "KSYL";
        public const string cPTIDByKXYL = "KXYL";
        public const string cPTIDByKYYL = "KYYL";
        public const string cPTIDByLDYL = "LDYL";
        public const string cPTIDByLF2 = "LF2";
        public const string cPTIDByLFGJ = "LFGJ";
        public const string cPTIDByLFYL = "LFYL";
        public const string cPTIDByLGZX = "LGZX";
        public const string cPTIDByLMH = "LMH";
        public const string cPTIDByLSWJS = "LSWJS";
        public const string cPTIDByLUDI = "LUDI";
        public const string cPTIDByLXYL = "LXYL";
        public const string cPTIDByLYS = "LYS";
        public const string cPTIDByM5CP = "M5CP";
        public const string cPTIDByMCYL = "MCYL";
        public const string cPTIDByMINC = "MINC";
        public const string cPTIDByMRYL = "MRYL";
        public const string cPTIDByMTYL = "MTYL";
        public const string cPTIDByMXYL = "MXYL";
        public const string cPTIDByMYGJ = "MYGJ";
        public const string cPTIDByMZC = "MZC";
        public const string cPTIDByNBAYL = "NBAYL";
        public const string cPTIDByNBYL = "NBYL";
        public const string cPTIDByOEYL = "OEYL";
        public const string cPTIDByQFYL = "QFYL";
        public const string cPTIDByQFZX = "QFZX";
        public const string cPTIDByQJC = "QJC";
        public const string cPTIDByQQT2 = "QQT2";
        public const string cPTIDByQQYL = "QQYL";
        public const string cPTIDByRDYL = "RDYL";
        public const string cPTIDBySIJI = "SIJI";
        public const string cPTIDBySKYYL = "SKYYL";
        public const string cPTIDBySLTH = "SLTH";
        public const string cPTIDBySSHC = "SSHC";
        public const string cPTIDBySYYL = "SYYL";
        public const string cPTIDByTAYL = "TAYL";
        public const string cPTIDByTBYL = "TBYL";
        public const string cPTIDByTCYL = "TCYL";
        public const string cPTIDByTHDYL = "THDYL";
        public const string cPTIDByTHEN = "THEN";
        public const string cPTIDByTHYL = "THYL";
        public const string cPTIDByTIYU = "TIYU";
        public const string cPTIDByTRYL = "TRYL";
        public const string cPTIDByTYYL = "TYYL";
        public const string cPTIDByUCYL = "UCYL";
        public const string cPTIDByUT8 = "UT8";
        public const string cPTIDByWBJ = "WBJ";
        public const string cPTIDByWCAI = "WCAI";
        public const string cPTIDByWCYL = "WCYL";
        public const string cPTIDByWDYL = "WDYL";
        public const string cPTIDByWEYL = "WEYL";
        public const string cPTIDByWHC = "WHC";
        public const string cPTIDByWHEN = "WHEN";
        public const string cPTIDByWJSJ = "WJSJ";
        public const string cPTIDByWMYL = "WMYL";
        public const string cPTIDByWSYL = "WSYL";
        public const string cPTIDByWTYL = "WTYL";
        public const string cPTIDByWXYL = "WXYL";
        public const string cPTIDByWYYL = "WYYL";
        public const string cPTIDByWZYL = "WZYL";
        public const string cPTIDByXB3 = "XB3";
        public const string cPTIDByXCAI = "XCAI";
        public const string cPTIDByXCYL = "XCYL";
        public const string cPTIDByXDB = "XDB";
        public const string cPTIDByXGLL = "XGLL";
        public const string cPTIDByXHDF = "XHDF";
        public const string cPTIDByXHHC = "XHHC";
        public const string cPTIDByXHSD = "XHSD";
        public const string cPTIDByXINC = "XINC";
        public const string cPTIDByXQYL = "XQYL";
        public const string cPTIDByXTYL = "XTYL";
        public const string cPTIDByXWYL = "XWYL";
        public const string cPTIDByYBAO = "YBAO";
        public const string cPTIDByYBYL = "YBYL";
        public const string cPTIDByYCYL = "YCYL";
        public const string cPTIDByYDYL = "YDYL";
        public const string cPTIDByYHSG = "YHSG";
        public const string cPTIDByYHYL = "YHYL";
        public const string cPTIDByYIFA = "YIFA";
        public const string cPTIDByYINH = "YINH";
        public const string cPTIDByYL2028 = "YL2028";
        public const string cPTIDByYRYL = "YRYL";
        public const string cPTIDByYSEN = "YSEN";
        public const string cPTIDByYXZX = "YXZX";
        public const string cPTIDByYYZX = "YYZX";
        public const string cPTIDByYZCP = "YZCP";
        public const string cPTIDByZBEI = "ZBEI";
        public const string cPTIDByZBYL = "ZBYL";
        public const string cPTIDByZDYL = "ZDYL";
        public const string cPTIDByZLJ = "ZLJ";
        public const string cPTIDByZXYL = "ZXYL";
        public const string cPTIDByZYL = "ZYL";
        public const string cPTLine = @"\PTLine\";
        public const string cPTNameByA6YL = "A6娱乐";
        public const string cPTNameByALGJ = "澳利国际";
        public const string cPTNameByAMBLR = "澳门巴黎人";
        public const string cPTNameByB6YL = "B6娱乐城";
        public const string cPTNameByBAYL = "BA娱乐";
        public const string cPTNameByBHGJ = "宝汇国际";
        public const string cPTNameByBHZY = "必火直营";
        public const string cPTNameByBKC = "博客彩";
        public const string cPTNameByBLGJ = "博乐国际";
        public const string cPTNameByBMEI = "博美娱乐";
        public const string cPTNameByBMYX = "博猫平台";
        public const string cPTNameByBNGJ = "博牛国际";
        public const string cPTNameByBWT = "博万通";
        public const string cPTNameByCAIH = "彩宏娱乐";
        public const string cPTNameByCBL = "彩部落";
        public const string cPTNameByCCYL = "长城娱乐";
        public const string cPTNameByCLYL = "长隆娱乐";
        public const string cPTNameByCP361 = "361彩票";
        public const string cPTNameByCTT = "彩天堂";
        public const string cPTNameByCTX = "彩天下";
        public const string cPTNameByCYYL = "彩云娱乐";
        public const string cPTNameByDACP = "大发彩票";
        public const string cPTNameByDAZYL = "大众娱乐";
        public const string cPTNameByDEJI = "德晋娱乐";
        public const string cPTNameByDJYL = "鼎尖娱乐";
        public const string cPTNameByDPC = "迪拜城";
        public const string cPTNameByDQYL = "地球娱乐";
        public const string cPTNameByDTCP = "大唐彩票";
        public const string cPTNameByDYYL = "斗鱼娱乐";
        public const string cPTNameByFCYL = "疯彩娱乐";
        public const string cPTNameByFEIC = "翡翠娱乐";
        public const string cPTNameByFLC = "菲洛城";
        public const string cPTNameByFNYX = "飞牛游戏";
        public const string cPTNameByFSYL = "丰尚娱乐";
        public const string cPTNameByGJYL = "公爵娱乐";
        public const string cPTNameByHANY = "航洋国际";
        public const string cPTNameByHBS = "红宝石";
        public const string cPTNameByHCYL = "豪彩娱乐";
        public const string cPTNameByHCZX = "宏川在线";
        public const string cPTNameByHDYL = "宏达娱乐";
        public const string cPTNameByHEND = "恒达娱乐";
        public const string cPTNameByHENR = "恒瑞";
        public const string cPTNameByHGDB = "红馆迪拜";
        public const string cPTNameByHKC = "豪客彩";
        public const string cPTNameByHLC = "欢乐城";
        public const string cPTNameByHNYL = "华纳娱乐";
        public const string cPTNameByHOND = "宏達娱乐";
        public const string cPTNameByHRCP = "华人彩票";
        public const string cPTNameByHSGJ = "汇盛国际";
        public const string cPTNameByHSYL = "弘尚娱乐";
        public const string cPTNameByHUAY = "华宇娱乐";
        public const string cPTNameByHUBO = "琥珀游戏";
        public const string cPTNameByHUIZ = "汇众娱乐";
        public const string cPTNameByHYYL = "好友";
        public const string cPTNameByHZYL = "华众娱乐";
        public const string cPTNameByJCX = "金诚信";
        public const string cPTNameByJFYL = "玖富娱乐";
        public const string cPTNameByJHC = "金皇朝";
        public const string cPTNameByJHC2 = "金皇朝2";
        public const string cPTNameByJHYL = "金狐娱乐";
        public const string cPTNameByJLGJ = "巨龙国际";
        public const string cPTNameByJXIN = "聚鑫娱乐";
        public const string cPTNameByJXYL = "锦绣娱乐";
        public const string cPTNameByJYGJ = "久赢国际";
        public const string cPTNameByJYYL = "鲸鱼娱乐";
        public const string cPTNameByK3YL = "K3娱乐";
        public const string cPTNameByK5YL = "K5娱乐";
        public const string cPTNameByKSYL = "凯萨娱乐";
        public const string cPTNameByKXYL = "凯鑫娱乐";
        public const string cPTNameByKYYL = "开元娱乐";
        public const string cPTNameByLDYL = "立鼎娱乐";
        public const string cPTNameByLF2 = "拉菲II";
        public const string cPTNameByLFGJ = "乐丰国际";
        public const string cPTNameByLFYL = "拉菲娱乐";
        public const string cPTNameByLGZX = "蓝冠在线";
        public const string cPTNameByLMH = "乐美汇";
        public const string cPTNameByLSWJS = "拉斯维加斯";
        public const string cPTNameByLUDI = "鹿鼎娱乐";
        public const string cPTNameByLXYL = "利信娱乐";
        public const string cPTNameByLYS = "路易斯";
        public const string cPTNameByM5CP = "M5彩票";
        public const string cPTNameByMCYL = "迷彩娱乐";
        public const string cPTNameByMINC = "名城娱乐";
        public const string cPTNameByMRYL = "名人娱乐";
        public const string cPTNameByMTYL = "马泰娱乐";
        public const string cPTNameByMXYL = "梦想娱乐";
        public const string cPTNameByMYGJ = "美娱国际";
        public const string cPTNameByMZC = "梦之城";
        public const string cPTNameByNBAYL = "NBA娱乐";
        public const string cPTNameByNBYL = "NB娱乐";
        public const string cPTNameByOEYL = "欧亿娱乐";
        public const string cPTNameByQFYL = "起凡娱乐";
        public const string cPTNameByQFZX = "青蜂在线";
        public const string cPTNameByQJC = "千金城";
        public const string cPTNameByQQT2 = "全球通2";
        public const string cPTNameByQQYL = "奇趣娱乐";
        public const string cPTNameByRDYL = "仁鼎娱乐";
        public const string cPTNameBySIJI = "四季娱乐";
        public const string cPTNameBySKYYL = "SKY娱乐";
        public const string cPTNameBySLTH = "十里桃花";
        public const string cPTNameBySSHC = "盛世皇朝";
        public const string cPTNameBySYYL = "数亿娱乐";
        public const string cPTNameByTAYL = "TA娱乐";
        public const string cPTNameByTBYL = "天博娱乐";
        public const string cPTNameByTCYL = "天辰娱乐";
        public const string cPTNameByTHDYL = "桃花岛娱乐";
        public const string cPTNameByTHEN = "天恒娱乐";
        public const string cPTNameByTHYL = "天豪娱乐";
        public const string cPTNameByTIYU = "天宇娱乐";
        public const string cPTNameByTRYL = "唐人娱乐";
        public const string cPTNameByTYYL = "天易娱乐";
        public const string cPTNameByUCYL = "众赢娱乐";
        public const string cPTNameByUT8 = "游艇会(UT8)";
        public const string cPTNameByWBJ = "旺百家";
        public const string cPTNameByWCAI = "万彩";
        public const string cPTNameByWCYL = "万彩娱乐";
        public const string cPTNameByWDYL = "万达娱乐";
        public const string cPTNameByWEYL = "WE娱乐";
        public const string cPTNameByWHC = "万和城";
        public const string cPTNameByWHEN = "万恒娱乐";
        public const string cPTNameByWJSJ = "玩家世界";
        public const string cPTNameByWMYL = "万美娱乐";
        public const string cPTNameByWSYL = "万森娱乐";
        public const string cPTNameByWTYL = "威霆娱乐";
        public const string cPTNameByWXYL = "无限娱乐";
        public const string cPTNameByWYYL = "幻影娱乐";
        public const string cPTNameByWZYL = "网赚娱乐";
        public const string cPTNameByXB3 = "新宝3";
        public const string cPTNameByXCAI = "杏彩娱乐";
        public const string cPTNameByXCYL = "轩彩娱乐";
        public const string cPTNameByXDB = "星多宝";
        public const string cPTNameByXGLL = "香格里拉";
        public const string cPTNameByXHDF = "新火巅峰";
        public const string cPTNameByXHHC = "新濠环彩";
        public const string cPTNameByXHSD = "新火大时代";
        public const string cPTNameByXINC = "星彩娱乐";
        public const string cPTNameByXQYL = "喜鹊娱乐";
        public const string cPTNameByXTYL = "新泰娱乐";
        public const string cPTNameByXWYL = "鑫旺娱乐";
        public const string cPTNameByYBAO = "亿宝娱乐";
        public const string cPTNameByYBYL = "亿博娱乐";
        public const string cPTNameByYCYL = "亿城娱乐";
        public const string cPTNameByYDYL = "万兴国际";
        public const string cPTNameByYHSG = "永恒时光";
        public const string cPTNameByYHYL = "亿皇娱乐";
        public const string cPTNameByYIFA = "易发彩票";
        public const string cPTNameByYINH = "银河";
        public const string cPTNameByYL2028 = "2028娱乐";
        public const string cPTNameByYRYL = "亿人娱乐";
        public const string cPTNameByYSEN = "亿昇娱乐";
        public const string cPTNameByYXZX = "永信在线";
        public const string cPTNameByYYZX = "易赢在线";
        public const string cPTNameByYZCP = "亚洲彩票";
        public const string cPTNameByZBEI = "中呗娱乐";
        public const string cPTNameByZBYL = "众博娱乐";
        public const string cPTNameByZDYL = "纵达娱乐";
        public const string cPTNameByZLJ = "侏罗纪";
        public const string cPTNameByZXYL = "在线娱乐";
        public const string cPTNameByZYL = "Z娱乐";
        public const string cPTNameValue = "PTNameValue";
        public const string cQQSend = "QQSend ";
        public const string cQQSendPlan = @"\QQSendPlan";
        public const string cQQSendPlay = @"\QQSendPlay";
        public const string cQuitLogin = "退出登录";
        public const string cRefreshTimeFormat = "HH:mm:ss";
        public const string cRegAppRoot = @"software\TUHAOPLUS\YXZXGJ\";
        public const string cRegCheckedKey = "Checked";
        public const string cRegControl = @"Control\";
        public const string cRegDialogConfigPath = @"DlgConfig\";
        public const string cRegFIPS = @"SYSTEM\CurrentControlSet\Control\Lsa\FipsAlgorithmPolicy";
        public const string cRegFirstDisplayedScrollingRowIndexKey = "FirstDisplayedScrollingRowIndex";
        public const string cRegFormHeightKey = "FormHeight";
        public const string cRegFormLeftKey = "FormLeft";
        public const string cRegFormTopKey = "FormTop";
        public const string cRegFormWidthKey = "FormWidth";
        public const string cRegFormWindowState = "FormWindowState";
        public const string cRegIndex1Key = "Index1";
        public const string cRegIndex2Key = "Index2";
        public const string cRegIndexKey = "Index";
        public const string cRegItemsKey = "Items";
        public const string cRegLastOpenPath = @"LastOpenPath\";
        public const string cRegLotteryConfig = @"LotteryConfig\";
        public const string cRegMainConfig = @"MainConfig\";
        public const string cRegScenarionList = @"\ScenarionList\";
        public const string cRegTextKey = "Text";
        public const string cRegValueKey = "Value";
        public const string cRGPlan = "RGPlan";
        public const string cScheme = @"\Scheme\";
        public const string cSelectPlan = "SelectPlan";
        public const string cSendBetsString = "共享投注";
        public const string cSendSchemeString = "上传";
        public const string cShareCode = "ShareCode";
        public const string cShowTapData = "ShowTapData";
        public const string cSlashSymbol = @"\";
        public const string cSound = @"\Sound\";
        public const string cSoundBetsNo = "投注失败";
        public const string cSoundBetsYes = "投注成功";
        public const string cSoundLoginNo = "登录失败";
        public const string cSoundLoginYes = "登录成功";
        public const string cSoundLoseHint = "掉线提示";
        public const string cSoundOpenData = "开奖号码";
        public const string cStartBets = "开启\r\n自动投注";
        public const string cStopBets = "关闭\r\n自动投注";
        public const string cTemplate = @"\Template\";
        public const string cTemplateName = "系统默认";
        public const string cTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string cTimesType1 = "收益率";
        public const string cTimesType2 = "固定利润";
        public const string cTimesType3 = "累加利润";
        public const string cTimesType4 = "自由倍数";
        public static CTT CTTInfo;
        public const string cTwoPoint = "0.00";
        public static CTX CTXInfo;
        public const string cTxtSaveFileFiter = "文本文档(*.txt)|*.txt";
        public const string cTypeName = "TypeName";
        public static ConfigurationStatus Current;
        public const string cUserExport = @"\UserExport\";
        public const string cUserLogin = "平台登录";
        public const string cUserLoginHint = "请先登录后再使用该功能！";
        public const string cUserLoginLTHint = "请先登录论坛后再使用该功能！";
        public const string cUserLoginPTHint = "请先登录平台后再使用该功能！";
        public const string cVerifyCode = @"\VerifyCode\";
        public const string cVerifyCodeLib = @"VerifyCode\Lib\";
        public const string cVersionWindow2000 = "Window2000";
        public const string cVersionWindow7 = "Window7";
        public const string cVersionWindow8 = "Window8";
        public const string cVersionWindowUnknown = "未知";
        public const string cVersionWindowVista = "WindowVista";
        public const string cVersionWindowXP = "WindowXP";
        public const string cWaitOpen = "等待";
        public const string cWrap = "\r\n";
        public const string cWrap2 = "\r\n\r\n";
        public const string cWrap3 = "\r\n\r\n\r\n";
        public const string cXing = "*";
        public const string cYJDate = "9999-12-31";
        public static CYYL CYYLInfo;
        public const string cZuKey = "0";
        public static DACP DACPInfo;
        public static Color darkCyanBackColor = Color.DarkCyan;
        public static Color darkGrayBackColor = Color.DarkGray;
        public static Color darkGrayForeColor = Color.DarkGray;
        public const int DataFontSize1 = 9;
        public const int DataFontSize2 = 11;
        public const int DataFontSize3 = 14;
        public static List<ConfigurationStatus.OpenData> DataList = new List<ConfigurationStatus.OpenData>();
        public static DAZYL DAZYLInfo;
        public const string DefaultMoney = "50000";
        public static DEJI DEJIInfo;
        public static DJYL DJYLInfo;
        public static DPC DPCInfo;
        public static DQYL DQYLInfo;
        public static DTCP DTCPInfo;
        public static DYYL DYYLInfo;
        public static FCYL FCYLInfo;
        public static FEIC FEICInfo;
        public static List<ConfigurationStatus.OpenData> FilterList = new List<ConfigurationStatus.OpenData>();
        public static Dictionary<string, int> FiveDic;
        public static FLC FLCInfo;
        public static FNYX FNYXInfo;
        public static FSYL FSYLInfo;
        public static Color gainsboroColor = Color.Gainsboro;
        public static ConfigurationStatus.GetHtmlDocumentDelegate GetHtmlDocument;
        public static ConfigurationStatus.GetLoginUrlDelegate GetLoginUrl;
        public static GJYL GJYLInfo;
        public static Color grayColor = Color.DimGray;
        public static Color greenBackColor = Color.DarkGreen;
        public static HANY HANYInfo;
        public static HBS HBSInfo;
        public static HCYL HCYLInfo;
        public static HCZX HCZXInfo;
        public static HDYL HDYLInfo;
        public static HEND HENDInfo;
        public static HENR HENRInfo;
        public static HGDB HGDBInfo;
        public static HKC HKCInfo;
        public static HLC HLCInfo;
        public static HNYL HNYLInfo;
        public static HOND HONDInfo;
        public static HRCP HRCPInfo;
        public static HSGJ HSGJInfo;
        public static HSYL HSYLInfo;
        public static HUAY HUAYInfo;
        public static HUBO HUBOInfo;
        public static HUIZ HUIZInfo;
        public static HYYL HYYLInfo;
        public static HZYL HZYLInfo;
        public static Dictionary<int, string> Index1Dic;
        public static Dictionary<int, string> IndexDic;
        public static bool IsAppLoaded;
        public static bool IsCheckTXFFC;
        public static bool IsCXG;
        public static bool IsDQCT;
        public static bool IsPassApp;
        public static bool IsViewLogin;
        public static bool IsViewPeople;
        public static JCX JCXInfo;
        public static JFYL JFYLInfo;
        public static JHC2 JHC2Info;
        public static JHC JHCInfo;
        public static JHYL JHYLInfo;
        public static JLGJ JLGJInfo;
        public static JXIN JXINInfo;
        public static JXYL JXYLInfo;
        public static JYGJ JYGJInfo;
        public static JYYL JYYLInfo;
        public static K3YL K3YLInfo;
        public static K5YL K5YLInfo;
        public static KSYL KSYLInfo;
        public static KXYL KXYLInfo;
        public static KYYL KYYLInfo;
        public static LDYL LDYLInfo;
        public static LF2 LF2Info;
        public static LFGJ LFGJInfo;
        public static LFYL LFYLInfo;
        public static LGZX LGZXInfo;
        public static Dictionary<string, int> LHDic;
        public static Color lightGreenColor = Color.FromArgb(0xdb, 0xff, 0xdb);
        public static Color lightRedColor = Color.MistyRose;
        public static Color lightYellowColor = Color.LightGoldenrodYellow;
        public static Color lineColor = Color.DarkOrange;
        public static LMH LMHInfo;
        public static ConfigurationStatus.LoadConfigurationDelegate LoadConfiguration;
        public static ConfigurationStatus.LoadConfigurationLaterDelegate LoadConfigurationLater;
        public static ConfigurationStatus.LoadDelegate LoadEnd;
        public static ConfigurationStatus.LoadDelegate LoadStart;
        public static bool LoginCancel;
        public static ConfigurationStatus.LoadDelegate LoginIPVerify;
        public static ConfigurationStatus.LoginMainDelegate LoginMain;
        public static ConfigurationStatus.LoginLotteryDelegate LoginPTLottery;
        public static ConfigurationStatus.LoadDelegate LoginVerify;
        public static Dictionary<string, string> LotterNameDic;
        public static LSWJS LSWJSInfo;
        public static LUDI LUDIInfo;
        public static LXYL LXYLInfo;
        public static LYS LYSInfo;
        public static M5CP M5CPInfo;
        public static MCYL MCYLInfo;
        public static Color mediumBlueBackColor = Color.MediumSlateBlue;
        public static MINC MINCInfo;
        public static MRYL MRYLInfo;
        public static MTYL MTYLInfo;
        public static MXYL MXYLInfo;
        public static MYGJ MYGJInfo;
        public static MZC MZCInfo;
        public static NBAYL NBAYLInfo;
        public static NBYL NBYLInfo;
        public static DateTime NextTime;
        public static OEYL OEYLInfo;
        public static Color omissionColor = Color.LightYellow;
        public const string Open = "打开";
        public static Dictionary<string, string> OpenDataLoginLotteryDic;
        public static Color orangeBackColor = Color.DarkOrange;
        public static Color orangeForeColor = Color.Yellow;
        public static Dictionary<string, string> PK10ExpectDic;
        public static Dictionary<string, List<ConfigurationStatus.PlayBase>> PlayDic;
        public static bool PlaySound;
        public static FrmProgress PregressHint;
        public static Color pressedColor = Color.SteelBlue;
        public static ConfigurationStatus.PTIndexMainDelegate PTIndexMain;
        public static PTBase PTInfo;
        public static Dictionary<string, int> PTVerifyCodeDic;
        public static QFYL QFYLInfo;
        public static QFZX QFZXInfo;
        public static QJC QJCInfo;
        public static QQT2 QQT2Info;
        public static QQYL QQYLInfo;
        public static RDYL RDYLInfo;
        public static Color redBackColor = Color.IndianRed;
        public static ConfigurationStatus.RefreshListDelegate RefreshList;
        public static ConfigurationStatus.RefreshLoginHintDelegate RefreshLoginHint;
        public static ConfigurationStatus.RefreshLSDataDelegate RefreshLSData;
        public static ConfigurationStatus.RefreshLSDataLaterDelegate RefreshLSDataLater;
        public static ConfigurationStatus.RefreshTJDataDelegate RefreshTJData;
        public static ConfigurationStatus.RefreshTJDataLaterDelegate RefreshTJDataLater;
        public static ConfigurationStatus.RefreshUserMainDelegate RefreshUserMain;
        public static ConfigurationStatus.LoadDelegate RefreshVerifyCode;
        public static string RemoteUrl = "";
        public static ConfigurationStatus.RemoveLoginLockDelegate RemoveLoginLock;
        public static List<string> ReplaceList;
        public static List<string> SchemeList;
        public static SIJI SIJIInfo;
        public static SKYYL SKYYLInfo;
        public static SLTH SLTHInfo;
        public static SSHC SSHCInfo;
        public static SYYL SYYLInfo;
        public static TAYL TAYLInfo;
        public static TBYL TBYLInfo;
        public static TCYL TCYLInfo;
        public static THDYL THDYLInfo;
        public static THEN THENInfo;
        public static THYL THYLInfo;
        public static int TimeInterval;
        public static TIYU TIYUInfo;
        public static TRYL TRYLInfo;
        public static Color turquoiseBackColor = Color.DarkTurquoise;
        public static TYYL TYYLInfo;
        public static UCYL UCYLInfo;
        public static UT8 UT8Info;
        public static WBJ WBJInfo;
        public static WCAI WCAIInfo;
        public static WCYL WCYLInfo;
        public static WDYL WDYLInfo;
        public static ConfigurationStatus.WebDataDelegate WebData;
        public static WEYL WEYLInfo;
        public static WHC WHCInfo;
        public static WHEN WHENInfo;
        public static Color whiteColor = Color.White;
        public static WJSJ WJSJInfo;
        public static WMYL WMYLInfo;
        public static WSYL WSYLInfo;
        public static WTYL WTYLInfo;
        public static WXYL WXYLInfo;
        public static WYYL WYYLInfo;
        public static WZYL WZYLInfo;
        public static XB3 XB3Info;
        public static XCAI XCAIInfo;
        public static XCYL XCYLInfo;
        public static XDB XDBInfo;
        public static XGLL XGLLInfo;
        public static XHDF XHDFInfo;
        public static XHHC XHHCInfo;
        public static XHSD XHSDInfo;
        public static XINC XINCInfo;
        public static XQYL XQYLInfo;
        public static XTYL XTYLInfo;
        public static XWYL XWYLInfo;
        public static YBAO YBAOInfo;
        public static YBYL YBYLInfo;
        public static YCYL YCYLInfo;
        public static YDYL YDYLInfo;
        public static Color yellowBackColor = Color.Yellow;
        public static YHSG YHSGInfo;
        public static YHYL YHYLInfo;
        public static YIFA YIFAInfo;
        public static YINH YINHInfo;
        public static YL2028 YL2028Info;
        public static YRYL YRYLInfo;
        public static YSEN YSENInfo;
        public static YXZX YXZXInfo;
        public static YYZX YYZXInfo;
        public static YZCP YZCPInfo;
        public static ZBEI ZBEIInfo;
        public static ZBYL ZBYLInfo;
        public static ZDYL ZDYLInfo;
        public static ZLJ ZLJInfo;
        public static ZXYL ZXYLInfo;
        public static ZYL ZYLInfo;

        static AppInfo()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int> {
                { 
                    "万",
                    0
                },
                { 
                    "千",
                    1
                },
                { 
                    "百",
                    2
                },
                { 
                    "十",
                    3
                },
                { 
                    "个",
                    4
                }
            };
            FiveDic = dictionary;
            Dictionary<int, string> dictionary2 = new Dictionary<int, string> {
                { 
                    0,
                    "万"
                },
                { 
                    1,
                    "千"
                },
                { 
                    2,
                    "百"
                },
                { 
                    3,
                    "十"
                },
                { 
                    4,
                    "个"
                }
            };
            IndexDic = dictionary2;
            Dictionary<int, string> dictionary3 = new Dictionary<int, string> {
                { 
                    0,
                    "w"
                },
                { 
                    1,
                    "q"
                },
                { 
                    2,
                    "b"
                },
                { 
                    3,
                    "s"
                },
                { 
                    4,
                    "g"
                }
            };
            Index1Dic = dictionary3;
            Dictionary<string, int> dictionary4 = new Dictionary<string, int> {
                { 
                    "龙",
                    1
                },
                { 
                    "虎",
                    2
                },
                { 
                    "和",
                    3
                }
            };
            LHDic = dictionary4;
            PK10ExpectDic = new Dictionary<string, string>();
            List<string> list = new List<string> { 
                "，",
                ";",
                "\r\n",
                "\n",
                "\r"
            };
            ReplaceList = list;
            CombinaDicPK10HZ = new Dictionary<string, List<string>>();
            LotterNameDic = new Dictionary<string, string>();
            OpenDataLoginLotteryDic = new Dictionary<string, string>();
            CP361Info = new CP361();
            MRYLInfo = new MRYL();
            HRCPInfo = new HRCP();
            DTCPInfo = new DTCP();
            BMYXInfo = new BMYX();
            WJSJInfo = new WJSJ();
            OEYLInfo = new OEYL();
            MZCInfo = new MZC();
            UT8Info = new UT8();
            M5CPInfo = new M5CP();
            DACPInfo = new DACP();
            UCYLInfo = new UCYL();
            LFYLInfo = new LFYL();
            LFGJInfo = new LFGJ();
            BNGJInfo = new BNGJ();
            WXYLInfo = new WXYL();
            LXYLInfo = new LXYL();
            BAYLInfo = new BAYL();
            YYZXInfo = new YYZX();
            JYGJInfo = new JYGJ();
            HBSInfo = new HBS();
            XB3Info = new XB3();
            K5YLInfo = new K5YL();
            FSYLInfo = new FSYL();
            BHZYInfo = new BHZY();
            A6YLInfo = new A6YL();
            YIFAInfo = new YIFA();
            ZDYLInfo = new ZDYL();
            SKYYLInfo = new SKYYL();
            DPCInfo = new DPC();
            LUDIInfo = new LUDI();
            LF2Info = new LF2();
            CTTInfo = new CTT();
            YBAOInfo = new YBAO();
            YRYLInfo = new YRYL();
            TRYLInfo = new TRYL();
            HGDBInfo = new HGDB();
            WSYLInfo = new WSYL();
            BMEIInfo = new BMEI();
            QQT2Info = new QQT2();
            WBJInfo = new WBJ();
            THDYLInfo = new THDYL();
            XHDFInfo = new XHDF();
            NBYLInfo = new NBYL();
            YSENInfo = new YSEN();
            WMYLInfo = new WMYL();
            TAYLInfo = new TAYL();
            YDYLInfo = new YDYL();
            BKCInfo = new BKC();
            MINCInfo = new MINC();
            SIJIInfo = new SIJI();
            HZYLInfo = new HZYL();
            ZXYLInfo = new ZXYL();
            DQYLInfo = new DQYL();
            XCYLInfo = new XCYL();
            FCYLInfo = new FCYL();
            FEICInfo = new FEIC();
            LMHInfo = new LMH();
            HANYInfo = new HANY();
            CAIHInfo = new CAIH();
            LYSInfo = new LYS();
            THYLInfo = new THYL();
            HSGJInfo = new HSGJ();
            DAZYLInfo = new DAZYL();
            SLTHInfo = new SLTH();
            RDYLInfo = new RDYL();
            K3YLInfo = new K3YL();
            JFYLInfo = new JFYL();
            FLCInfo = new FLC();
            HENRInfo = new HENR();
            JHCInfo = new JHC();
            KSYLInfo = new KSYL();
            WDYLInfo = new WDYL();
            QJCInfo = new QJC();
            HNYLInfo = new HNYL();
            WHCInfo = new WHC();
            BWTInfo = new BWT();
            ZYLInfo = new ZYL();
            CLYLInfo = new CLYL();
            YL2028Info = new YL2028();
            THENInfo = new THEN();
            CBLInfo = new CBL();
            LDYLInfo = new LDYL();
            HKCInfo = new HKC();
            SYYLInfo = new SYYL();
            MTYLInfo = new MTYL();
            HUBOInfo = new HUBO();
            XCAIInfo = new XCAI();
            LGZXInfo = new LGZX();
            WHENInfo = new WHEN();
            JHYLInfo = new JHYL();
            MYGJInfo = new MYGJ();
            HLCInfo = new HLC();
            HYYLInfo = new HYYL();
            JHC2Info = new JHC2();
            HUIZInfo = new HUIZ();
            HDYLInfo = new HDYL();
            ALGJInfo = new ALGJ();
            KYYLInfo = new KYYL();
            CCYLInfo = new CCYL();
            GJYLInfo = new GJYL();
            CTXInfo = new CTX();
            JCXInfo = new JCX();
            KXYLInfo = new KXYL();
            ZLJInfo = new ZLJ();
            LSWJSInfo = new LSWJS();
            HCYLInfo = new HCYL();
            SSHCInfo = new SSHC();
            XHSDInfo = new XHSD();
            HCZXInfo = new HCZX();
            BHGJInfo = new BHGJ();
            XDBInfo = new XDB();
            DJYLInfo = new DJYL();
            DYYLInfo = new DYYL();
            HONDInfo = new HOND();
            QFYLInfo = new QFYL();
            TYYLInfo = new TYYL();
            AMBLRInfo = new AMBLR();
            JXYLInfo = new JXYL();
            XINCInfo = new XINC();
            YHYLInfo = new YHYL();
            CYYLInfo = new CYYL();
            BLGJInfo = new BLGJ();
            YBYLInfo = new YBYL();
            JYYLInfo = new JYYL();
            WCYLInfo = new WCYL();
            WYYLInfo = new WYYL();
            XHHCInfo = new XHHC();
            NBAYLInfo = new NBAYL();
            WEYLInfo = new WEYL();
            MCYLInfo = new MCYL();
            MXYLInfo = new MXYL();
            WCAIInfo = new WCAI();
            QQYLInfo = new QQYL();
            YHSGInfo = new YHSG();
            YINHInfo = new YINH();
            HENDInfo = new HEND();
            XGLLInfo = new XGLL();
            DEJIInfo = new DEJI();
            JLGJInfo = new JLGJ();
            XTYLInfo = new XTYL();
            XWYLInfo = new XWYL();
            B6YLInfo = new B6YL();
            TBYLInfo = new TBYL();
            WZYLInfo = new WZYL();
            YZCPInfo = new YZCP();
            TIYUInfo = new TIYU();
            YCYLInfo = new YCYL();
            ZBYLInfo = new ZBYL();
            FNYXInfo = new FNYX();
            HUAYInfo = new HUAY();
            YXZXInfo = new YXZX();
            WTYLInfo = new WTYL();
            TCYLInfo = new TCYL();
            QFZXInfo = new QFZX();
            JXINInfo = new JXIN();
            ZBEIInfo = new ZBEI();
            HSYLInfo = new HSYL();
            XQYLInfo = new XQYL();
            PTInfo = null;
            PlaySound = true;
            IsDQCT = false;
            TimeInterval = 60;
            LoginCancel = false;
            CheckPTLogin = false;
            IsPassApp = false;
            IsViewLogin = true;
            IsViewPeople = false;
            IsCheckTXFFC = true;
            IsCXG = false;
            IsAppLoaded = false;
            Account = new ConfigurationStatus.SCAccountData();
            PlayDic = new Dictionary<string, List<ConfigurationStatus.PlayBase>>();
            BTFNDic = new Dictionary<string, ConfigurationStatus.GJBTScheme>();
            PTVerifyCodeDic = new Dictionary<string, int>();
            AppPTNameList = new List<string>();
            BTFNList = new List<string>();
            SchemeList = new List<string>();
        }

        public static string cPTLineFile(string pAppName = "")
        {
            if (pAppName == "")
            {
                pAppName = Account.AppPerName;
            }
            string str = pAppName;
            switch (str)
            {
                case "FNHJ":
                case "PTOpenData":
                    str = "";
                    break;
            }
            return (str + "PTLine");
        }

        public static Color appBackColor
        {
            get
            {
                Color colorByRGB = Color.FromArgb(0x2b, 0x3a, 0x5d);
                if ((Account.Configuration != null) && (Account.Configuration.BackColor != ""))
                {
                    colorByRGB = CommFunc.GetColorByRGB(Account.Configuration.BackColor);
                }
                return colorByRGB;
            }
        }

        public static Color appForeColor
        {
            get
            {
                Color steelBlue = Color.SteelBlue;
                if ((Account.Configuration != null) && (Account.Configuration.ForeColor != ""))
                {
                    steelBlue = CommFunc.GetColorByRGB(Account.Configuration.ForeColor);
                }
                return steelBlue;
            }
        }

        public static Color beaBackColor
        {
            get
            {
                Color control = SystemColors.Control;
                if ((Account.Configuration != null) && (Account.Configuration.BeaBackColor != ""))
                {
                    control = CommFunc.GetColorByRGB(Account.Configuration.BeaBackColor);
                }
                return control;
            }
        }

        public static Color beaForeColor
        {
            get
            {
                Color blackColor = AppInfo.blackColor;
                if ((Account.Configuration != null) && (Account.Configuration.BeaForeColor != ""))
                {
                    blackColor = CommFunc.GetColorByRGB(Account.Configuration.BeaForeColor);
                }
                return blackColor;
            }
        }

        public static Color beaHotColor
        {
            get
            {
                Color colorByRGB = Color.FromArgb(0x9e, 0xc9, 0xfb);
                if ((Account.Configuration != null) && (Account.Configuration.BeaHotColor != ""))
                {
                    colorByRGB = CommFunc.GetColorByRGB(Account.Configuration.BeaHotColor);
                }
                return colorByRGB;
            }
        }

        public static Color blueForeColor
        {
            get
            {
                Color blue = Color.Blue;
                if (Account.Configuration.Beautify)
                {
                    blue = Color.LightSkyBlue;
                }
                return blue;
            }
        }

        public static Image CodeImage =>
            Resources.Code55;

        public static Image CodeImage1
        {
            get
            {
                Image image = Resources.Code40;
                if (App == ConfigurationStatus.AppType.TCYLGJ)
                {
                    return Resources.Code40TCYL;
                }
                if (App == ConfigurationStatus.AppType.JFYLGJ)
                {
                    image = Resources.Code40JFYL;
                }
                return image;
            }
        }

        public static int cRefreshTime =>
            0x1388;

        public static string cServerGGUrl =>
            (cServerUrl + "GG/TUHAOPLUS");

        public static string cServerSqlUrl =>
            (cServerUrl + "TUHAOPLUS.aspx");

        public static string cServerUpdateUrl =>
            (cServerUrl + "Update/TUHAOPLUS/");

        public static string cServerUrl
        {
            get
            {
                string remoteUrl = "http://183.60.110.151:2570/";
                if ((((((((App == ConfigurationStatus.AppType.QQTGJ) || (App == ConfigurationStatus.AppType.WBJGJ)) || ((App == ConfigurationStatus.AppType.THDGJ) || (App == ConfigurationStatus.AppType.XH3GJ))) || (((App == ConfigurationStatus.AppType.NBGJ) || (App == ConfigurationStatus.AppType.YSENGJ)) || ((App == ConfigurationStatus.AppType.YRYLGJ) || (App == ConfigurationStatus.AppType.TAGJ)))) || ((((App == ConfigurationStatus.AppType.BKCGJ) || (App == ConfigurationStatus.AppType.MINCGJ)) || ((App == ConfigurationStatus.AppType.HZGJ) || (App == ConfigurationStatus.AppType.ZXGJ))) || (((App == ConfigurationStatus.AppType.UT8GJ) || (App == ConfigurationStatus.AppType.OEGJ)) || ((App == ConfigurationStatus.AppType.DQGJ) || (App == ConfigurationStatus.AppType.XCGJ))))) || (((((App == ConfigurationStatus.AppType.SKYGJ) || (App == ConfigurationStatus.AppType.FCGJ)) || ((App == ConfigurationStatus.AppType.FEICGJ) || (App == ConfigurationStatus.AppType.LDGJ))) || (((App == ConfigurationStatus.AppType.MZCGJ) || (App == ConfigurationStatus.AppType.LMHGJ)) || ((App == ConfigurationStatus.AppType.HANYGJ) || (App == ConfigurationStatus.AppType.CAIHGJ)))) || ((((App == ConfigurationStatus.AppType.LYSGJ) || (App == ConfigurationStatus.AppType.THGJ)) || ((App == ConfigurationStatus.AppType.HSGJ) || (App == ConfigurationStatus.AppType.DAZGJ))) || (((App == ConfigurationStatus.AppType.SLTHGJ) || (App == ConfigurationStatus.AppType.JFYLGJ)) || ((App == ConfigurationStatus.AppType.JHCGJ) || (App == ConfigurationStatus.AppType.KSGJ)))))) || ((((((App == ConfigurationStatus.AppType.WHCGJ) || (App == ConfigurationStatus.AppType.ZYLGJ)) || ((App == ConfigurationStatus.AppType.CBLGJ) || (App == ConfigurationStatus.AppType.HKCGJ))) || (((App == ConfigurationStatus.AppType.MTYLGJ) || (App == ConfigurationStatus.AppType.LGZXGJ)) || ((App == ConfigurationStatus.AppType.JHC2GJ) || (App == ConfigurationStatus.AppType.HUIZGJ)))) || ((((App == ConfigurationStatus.AppType.KYYLGJ) || (App == ConfigurationStatus.AppType.CTXGJ)) || ((App == ConfigurationStatus.AppType.KXYLGJ) || (App == ConfigurationStatus.AppType.ZLJGJ))) || (((App == ConfigurationStatus.AppType.HCYLGJ) || (App == ConfigurationStatus.AppType.SSHCGJ)) || ((App == ConfigurationStatus.AppType.DYCT) || (App == ConfigurationStatus.AppType.XHSD))))) || (((((App == ConfigurationStatus.AppType.DJGJ) || (App == ConfigurationStatus.AppType.JXGJ)) || ((App == ConfigurationStatus.AppType.XINCGJ) || (App == ConfigurationStatus.AppType.YHGJ))) || (((App == ConfigurationStatus.AppType.CYYLGJ) || (App == ConfigurationStatus.AppType.BLGJ)) || ((App == ConfigurationStatus.AppType.YBGJ) || (App == ConfigurationStatus.AppType.JYGJ)))) || ((((App == ConfigurationStatus.AppType.WCGJ) || (App == ConfigurationStatus.AppType.WYGJ)) || ((App == ConfigurationStatus.AppType.XHHCGJ) || (App == ConfigurationStatus.AppType.QQGJ))) || ((App == ConfigurationStatus.AppType.HENDGJ) || (App == ConfigurationStatus.AppType.XTYLGJ)))))) || (App == ConfigurationStatus.AppType.TCYLGJ))
                {
                    remoteUrl = "http://183.60.110.151:3570/";
                }
                else if ((((((((App == ConfigurationStatus.AppType.ZYIN) || (App == ConfigurationStatus.AppType.XHGJ)) || ((App == ConfigurationStatus.AppType.YBAOGJ) || (App == ConfigurationStatus.AppType.RDYLGJ))) || (((App == ConfigurationStatus.AppType.K3GJ) || (App == ConfigurationStatus.AppType.WSGJ)) || ((App == ConfigurationStatus.AppType.FLCGJ) || (App == ConfigurationStatus.AppType.CAITTGJ)))) || ((((App == ConfigurationStatus.AppType.WDCD) || (App == ConfigurationStatus.AppType.QJCGJ)) || ((App == ConfigurationStatus.AppType.HNYLGJ) || (App == ConfigurationStatus.AppType.BWTGJ))) || (((App == ConfigurationStatus.AppType.YL28) || (App == ConfigurationStatus.AppType.CLYLGJ)) || ((App == ConfigurationStatus.AppType.THEN) || (App == ConfigurationStatus.AppType.LDYLGJ))))) || (((((App == ConfigurationStatus.AppType.SYYLGJ) || (App == ConfigurationStatus.AppType.HENRGJ)) || ((App == ConfigurationStatus.AppType.WHEN) || (App == ConfigurationStatus.AppType.HDYLGJ))) || (((App == ConfigurationStatus.AppType.ALGJGJ) || (App == ConfigurationStatus.AppType.HDGJ)) || ((App == ConfigurationStatus.AppType.CCGJ) || (App == ConfigurationStatus.AppType.JCXGJ)))) || ((((App == ConfigurationStatus.AppType.LSWJSGJ) || (App == ConfigurationStatus.AppType.TRGJ)) || ((App == ConfigurationStatus.AppType.HCZXGJ) || (App == ConfigurationStatus.AppType.BHGJ))) || (((App == ConfigurationStatus.AppType.XDBGJ) || (App == ConfigurationStatus.AppType.DYGJ)) || ((App == ConfigurationStatus.AppType.WDGJ) || (App == ConfigurationStatus.AppType.HONDGJ)))))) || ((((App == ConfigurationStatus.AppType.QFGJ) || (App == ConfigurationStatus.AppType.TYGJ)) || ((App == ConfigurationStatus.AppType.AMGJ) || (App == ConfigurationStatus.AppType.NBAGJ))) || (((App == ConfigurationStatus.AppType.WEGJ) || (App == ConfigurationStatus.AppType.B6YLGJ)) || (App == ConfigurationStatus.AppType.ZBYLGJ)))) || (App == ConfigurationStatus.AppType.HUAYGJ))
                {
                    remoteUrl = "http://116.31.99.59:3570/";
                }
                else if (((((((App == ConfigurationStatus.AppType.MCGJ) || (App == ConfigurationStatus.AppType.MXGJ)) || ((App == ConfigurationStatus.AppType.WCAIGJ) || (App == ConfigurationStatus.AppType.YHSGGJ))) || (((App == ConfigurationStatus.AppType.YINHGJ) || (App == ConfigurationStatus.AppType.XGLLGJ)) || ((App == ConfigurationStatus.AppType.XWYLGJ) || (App == ConfigurationStatus.AppType.TBYLGJ)))) || ((((App == ConfigurationStatus.AppType.WZGJ) || (App == ConfigurationStatus.AppType.YZCPGJ)) || ((App == ConfigurationStatus.AppType.TIYUGJ) || (App == ConfigurationStatus.AppType.YCYLGJ))) || (((App == ConfigurationStatus.AppType.FNYXGJ) || (App == ConfigurationStatus.AppType.YXZXGJ)) || ((App == ConfigurationStatus.AppType.WTYLGJ) || (App == ConfigurationStatus.AppType.QFZXGJ))))) || ((App == ConfigurationStatus.AppType.ZBEIGJ) || (App == ConfigurationStatus.AppType.JXINGJ))) || (App == ConfigurationStatus.AppType.XQYLGJ))
                {
                    remoteUrl = "http://183.60.233.191:3570/";
                }
                else if (((App == ConfigurationStatus.AppType.QIQUGJ) || (App == ConfigurationStatus.AppType.DEJIGJ)) || (App == ConfigurationStatus.AppType.JLGJ))
                {
                    remoteUrl = "http://59.37.85.99:3570/";
                }
                else if (App == ConfigurationStatus.AppType.WMGJ)
                {
                    remoteUrl = "http://119.63.32.43:3570/";
                }
                else if (((((App == ConfigurationStatus.AppType.YFENG) || (App == ConfigurationStatus.AppType.BNGJ)) || ((App == ConfigurationStatus.AppType.BMGJ) || (App == ConfigurationStatus.AppType.DPCGJ))) || (((App == ConfigurationStatus.AppType.MDLM) || (App == ConfigurationStatus.AppType.YDGJ)) || ((App == ConfigurationStatus.AppType.SIJIGJ) || (App == ConfigurationStatus.AppType.JCLM3)))) || (App == ConfigurationStatus.AppType.GJYLGJ))
                {
                    remoteUrl = "http://183.60.110.151:2570/";
                }
                else if (App == ConfigurationStatus.AppType.CXG3)
                {
                    remoteUrl = "http://43.229.7.80/";
                }
                else if (App == ConfigurationStatus.AppType.CSCGJ)
                {
                    remoteUrl = "http://103.230.218.202:3570/";
                }
                else if (((App == ConfigurationStatus.AppType.LHLM) || (App == ConfigurationStatus.AppType.NRLM)) || (App == ConfigurationStatus.AppType.HRCP))
                {
                    remoteUrl = "http://183.60.110.151:2570/";
                }
                else if (App == ConfigurationStatus.AppType.Manage)
                {
                    remoteUrl = "http://59.37.85.99:3570/";
                }
                if (RemoteUrl != "")
                {
                    remoteUrl = RemoteUrl;
                }
                return remoteUrl;
            }
        }

        public static Color defaultForeColor
        {
            get
            {
                Color black = Color.Black;
                if ((Account.Configuration != null) && (Account.Configuration.DefaultFore != ""))
                {
                    black = CommFunc.GetColorByRGB(Account.Configuration.DefaultFore);
                }
                return black;
            }
        }

        public static Color greenForeColor
        {
            get
            {
                Color darkGreen = Color.DarkGreen;
                if (Account.Configuration.Beautify)
                {
                    darkGreen = Color.LimeGreen;
                }
                return darkGreen;
            }
        }

        public static Color hotColor
        {
            get
            {
                Color colorByRGB = Color.FromArgb(0x9e, 0xc9, 0xfb);
                if ((Account.Configuration != null) && (Account.Configuration.HotColor != ""))
                {
                    colorByRGB = CommFunc.GetColorByRGB(Account.Configuration.HotColor);
                }
                return colorByRGB;
            }
        }

        public static Image NextTimeImage =>
            Resources.Img_NextTime;

        public static string PTLineFile =>
            (PTLinePath + cPTLineFile("") + ".txt");

        public static string PTLinePath =>
            (CommFunc.getDllPath() + @"\PTLine\");

        public static Color redForeColor
        {
            get
            {
                Color red = Color.Red;
                if (Account.Configuration.Beautify)
                {
                    red = Color.Tomato;
                }
                return red;
            }
        }
    }
}

