using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MDCTest2016
{
    class Cmd
    {
        public static Queue<byte[]> SendQueue=new Queue<byte[]>();


        //将String转换为byte数组
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
            {
                hexString += " ";
            }
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }


        /// <summary>
        /// 举例：
        /// int i = 0;
        /// byte[] sendbyte = new byte[5];
        /// MiniPort.SetLast4Byte(byte,i.toString("x"));
        /// sendbyte[0]=108;
        /// </summary>
        /// <param name="b">要发送的数组</param>
        /// <param name="s">字符串</param>
        public static void SetLast4Byte(byte[] b, string s)
        {
            s = ExpandStringTo8(s);
            byte[] a = strToToHexByte(s);
            b[1] = a[0];
            b[2] = a[1];
            b[3] = a[2];
            b[4] = a[3];
        }

        /// <summary>
        /// 将指定字符串长度扩展为8
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ExpandStringTo8(string s)
        {
            while (s.Length < 8)
            {
                s = "0" + s;
            }
            return s;
        }
        
        public static void StopRun()
        {
            byte[] b = new byte[5];
            b[0] = 0xcc;
            b[1] = 0x23;
            b[2] = 0x45;
            b[3] = 0x67;
            b[4] = 0x89;
            SendQueue.Enqueue(b);
            b = new byte[5];
            b[0] = 0xcc;
            b[1] = 0x23;
            b[2] = 0x45;
            b[3] = 0x67;
            b[4] = 0x89;
            SendQueue.Enqueue(b);
            b = new byte[5];
            b[0] = 0xcc;
            b[1] = 0x23;
            b[2] = 0x45;
            b[3] = 0x67;
            b[4] = 0x89;
            SendQueue.Enqueue(b);
        }

        public static void ReadAllParam(object o)
        {
            SendReadParamCmd(130);
            SendReadParamCmd(160);
            SendReadParamCmd(128);
            SendReadParamCmd(129);
            SendReadParamCmd(161);
            SendReadParamCmd(162);
            SendReadParamCmd(169);
            isReadedAllParam = true;
        }

        public static bool isReadedAllParam = false;

        /// <summary>
        /// 设置参数写使能
        /// </summary>
        public static void EnterParamSetMode()
        {
            byte[] b = new byte[5];
            b[0] = 194;
            b[1] = 0x12;
            b[2] = 0x34;
            b[3] = 0x56;
            b[4] = 0x78;
            SendQueue.Enqueue(b);
        }

        /// <summary>
        /// 退出参数写使能
        /// </summary>
        public static void ExitParamSetMode()
        {
            byte[] b = new byte[5];
            b[0] = 195;
            b[1] = 0x12;
            b[2] = 0x34;
            b[3] = 0x56;
            b[4] = 0x78;
            SendQueue.Enqueue(b);
        }

        /// <summary>
        /// 设置出厂日期
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="d">日</param>
        public static void SetDateOfProduction(int y, int m, int d)
        {
            int p = 0;
            p = (y << 15) | (m << 8) | d;
            byte[] o = new byte[5];
            EnterParamSetMode();
            o[0] = 140;
            SetLast4Byte(o, p.ToString("x"));
            SendQueue.Enqueue(o);
            ExitParamSetMode();
        }

        /// <summary>
        /// 设置上次使用日期
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="d">日</param>
        public static void SetLastUsedData(object l)
        {
            Thread.Sleep(8000);
            int y = DateTime.Now.Year;// 获取年  
            int m = DateTime.Now.Month; //获取月   /
            int d = DateTime.Now.Day;// 获取日
            int p = 0;
            p = (y << 16) | (m << 8) | d;
            byte[] o = new byte[5];
            EnterParamSetMode();
            o[0] = 136;
            SetLast4Byte(o, p.ToString("x"));
            SendQueue.Enqueue(o);
            ExitParamSetMode();
        }

        //进入校准状态
        public static void EnterCaliMode()
        {
            byte[] b = new byte[5];
            b[0] = 190;
            b[1] = 0x12;
            b[2] = 0x34;
            b[3] = 0x56;
            b[4] = 0x78;
            SendQueue.Enqueue(b);
            Datas.IsCali = true;
        }

        //退出校准状态
        public static void ExitCaliMode()
        {
            byte[] b = new byte[5];
            b[0] = 191;
            b[1] = 0x12;
            b[2] = 0x34;
            b[3] = 0x56;
            b[4] = 0x78;
            SendQueue.Enqueue(b);
            Datas.IsCali = false;
        }

        //进入试验状态
        public static void EnterTestMode()
        {
            byte[] b = new byte[5];
            b[0] = 207;
            b[1] = 0xff;
            b[2] = 0xff;
            b[3] = 0xff;
            b[4] = 0xff;
            SendQueue.Enqueue(b);
        }

        //退出试验状态
        public static void ExitTestMode()
        {
            byte[] b = new byte[5];
            b[0] = 207;
            b[1] = 0;
            b[2] = 0;
            b[3] = 0;
            b[4] = 0;
            SendQueue.Enqueue(b);
        }
        
        /// <summary>
        /// 发送参数号的读取命令
        /// </summary>
        /// <param name="i">参数号</param>
        public static void SendReadParamCmd(int i)
        {
            if (i < 128 || i > 175)
            {
                System.Windows.MessageBox.Show("参数范围为128~175！", "警告");
                return;
            }
            byte[] b = new byte[5];
            b[0] = 196;
            SetLast4Byte(b, i.ToString("x"));
            SendQueue.Enqueue(b);
            Thread.Sleep(200);
        }
    }

    class CmdClass
    {
        //运行方向：0方向自判，1向上，-1向下
        public int dir = 0;
        //命令号：0为立即执行，1-15为链接命令
        public int CmdNum = 0;
        //控制方式：1位移控，2力控，3变形控,4应力控，5应变控
        public int CtrlMode = 0;
        //对应控制方式的速度（DSP浮点）
        public double CtrlSpeed = 0;
        //结束方式：0时间，1位移，2力，3变形,4应力，5应变
        public int EndMode = 0;
        //结束方式对应的值
        public double EndValue = 0;
    }
}
