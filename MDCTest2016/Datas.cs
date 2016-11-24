using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDCTest2016
{
    class Datas
    {
        #region 原始数据、码值及采样值
        public static byte[] Original=new byte[64];
        public static long Dwtime;//运行时间
        public static int Day;//可运行天数
        public static int Status0;//状态0
        public static byte FeedBackNum;//返回的参数号
        public static int FeedBack;//返回的参数内容
        public static int OpenValue;//开度
        public static int Status1;//状态1

        //从0开始分别是力，位移，变形的当前速度
        //以DSP浮点数保存，使用前需用miniport中的DSPtoFLT方法转换为普通浮点数使用
        public static int[] SpeedCode = new int[3];

        //各通道原始码
        //从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6
        public static int[] Code = new int[11];

        //各个通道符号位
        //从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6
        public static int[] Symbol = new int[11];

        //130参数中，三个码与方向的异或值,从0开始分别是力，位移，变形
        public static int[] XORNum = new int[3];

        //11个值
        //从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6
        public static double[] Value = new double[11];

        //11个峰值
        //从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6,
        public static double[] MaxValue = new double[11];

        //11个谷值
        //从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6,
        //后来为了兼容画图，第11个为命令号，第12个为时间
        public static double[] MiniValue = new double[11];

        //11个零点码
        //从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6
        public static int[] Zero = new int[11];

        //128~175参数的本地保存,直接用脚标表示，其他的内容不作处理
        //注意，需要初始化！！！
        public static int[] FeedBackParam = new int[176];
        #endregion

        #region 状态位
        public static double[] RealTimeSpeed = new double[3];
        //是否在线
        public static bool IsOnlie = false;
        //是否断线重连，优化用
        public static bool ReConnectSign = false;
        //是否在运行中
        public static bool IsRunning = false;
        //当前控制方式 1-力，2-位移，3-变形,0-没有
        public static int CtrlMode = 0;
        //运行方向  1为向上，-1为向下，0为没有
        public static int rundir = 0;
        //上下限位
        public static bool UpLimit = false;
        public static bool LowLimit = false;
        //是否超载
        public static bool IsOverLoad = false;
        //急停
        public static bool EmergencyStop = false;
        //试验运行->即运行的出发条件是否是进行实验
        public static bool IsOnTestingRunning = false;
        //位移码和方向的异或值
        public static int PosiCodeXorDir = 0;
        //力码和方向值异或值
        public static int LoadCodeXorDir = 0;
        //变形码和方向异或值
        public static int ExtnCodeXorDir = 0;
        //是否处于校准状态
        public static bool IsCali = false;
        //测控器使能
        public static bool MDCEnable = true;
        //是否为管理员
        public static bool IsRoot = false;
        #endregion

        #region 示值栏
        //public static ShowValuePanel[] sp;
        //public static int NumOfShowValuePanel;
        #endregion

        #region 试验相关
        public static bool NeedStartTest = false;
        #endregion
    }
}
