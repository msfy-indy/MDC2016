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
            Thread.Sleep(300);
        }

        /// <summary>
        /// 清零相关通道
        /// </summary>
        /// <param name="channel">清零相关通道,从0开始，分别是力，位移，变形，大变形上，大变形下，扩展1-6</param>
        public static void SendZero(int channel)
        {
            if (channel < 0 || channel > 10)
            {
                return;
            }
            byte[] b = new byte[5];
            SetLast4Byte(b, Datas.Code[channel].ToString("x"));
            switch (channel)
            {
                case 0:
                    {
                        b[0] = 111;
                        break;
                    }
                case 1:
                    {
                        b[0] = 108;
                        break;
                    }
                case 2:
                    {
                        b[0] = 112;
                        break;
                    }
                case 3:
                    {
                        b[0] = 109;
                        break;
                    }
                case 4:
                    {
                        b[0] = 110;
                        break;
                    }
                case 5:
                    {
                        b[0] = 113;
                        break;
                    }
                case 6:
                    {
                        b[0] = 114;
                        break;
                    }
                case 7:
                    {
                        b[0] = 115;
                        break;
                    }
                case 8:
                    {
                        b[0] = 116;
                        break;
                    }
                case 9:
                    {
                        b[0] = 117;
                        break;
                    }
                case 10:
                    {
                        b[0] = 118;
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            SendQueue.Enqueue(b);
            Datas.Zero[channel] = Datas.Code[channel];
        }

        /// <summary>
        /// 统一发送200号命令的位置,CtrlSpeed=0时，为保载试验
        /// </summary>
        /// <param name="dir">运行方向：0方向自判，1向上，-1向下</param>
        /// <param name="CmdNum">命令号：0为立即执行，1-15为链接命令</param>
        /// <param name="CtrlMode">控制方式：1位移控，2力控，3变形控</param>
        /// <param name="CtrlSpeed">对应控制方式的速度（DSP浮点）</param>
        /// <param name="EndMode">结束方式：0时间，1位移，2力，3变形</param>
        /// <param name="EndValue">结束方式对应的值</param>
        /// <returns></returns>
        public static bool Send200Cmd(int dir, int CmdNum, int CtrlMode, double CtrlSpeed_, int EndMode, double EndValue_)
        {
            //安全性检查
            if (CmdNum < 0 || CmdNum > 15 || CtrlMode < 1 || CtrlMode > 3 || EndMode < 0 || EndMode > 3)
            {
                return false;
            }
            int CtrlSpeed = 0, EndValue = 0;
            byte[] b = new byte[5];
            switch (CtrlMode)
            {
                case 1:
                    {
                        CtrlSpeed = (int)(Communication.FLTtoDSP((float)(CtrlSpeed_ * Math.Pow(-1, Datas.Symbol[1]) / (600000 * Sensor.ScaleValue[1]))));
                        b[0] = 101;
                        break;
                    }
                case 2:
                    {
                        CtrlSpeed = (int)(Communication.FLTtoDSP((float)(Sensor.GetCode(CtrlSpeed_, 0) / 600000)));
                        b[0] = 102;
                        break;
                    }
                case 3:
                    {
                        CtrlSpeed = (int)(Communication.FLTtoDSP((float)(Sensor.GetCode(CtrlSpeed_, 2) / 600000)));
                        b[0] = 103;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            SetLast4Byte(b, CtrlSpeed.ToString("x"));
            SendQueue.Enqueue(b);

            b = new byte[5];

            switch (EndMode)
            {
                case 0:
                    {
                        EndValue = (int)(EndValue_ * 100);
                        b[0] = 100;
                        break;
                    }
                case 1:
                    {
                        EndValue = (int)(EndValue_ * Math.Pow(-1, Datas.Symbol[1]) / Sensor.ScaleValue[1]);
                        b[0] = 104;
                        break;
                    }
                case 2:
                    {
                        EndValue = Sensor.GetCode(EndValue_, 0);
                        b[0] = 105;
                        break;
                    }
                case 3:
                    {
                        EndValue = Sensor.GetCode(EndValue_, 2);
                        b[0] = 106;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            if (CtrlSpeed_ == 0)
            {
                EndValue = 1;
                SetLast4Byte(b, EndValue.ToString("x"));
                SendQueue.Enqueue(b);

                b = new byte[5];
                EndValue = (int)(EndValue_ * 100);
                b[0] = 100;
                SetLast4Byte(b, EndValue.ToString("x"));
                SendQueue.Enqueue(b);
            }
            else
            {
                SetLast4Byte(b, EndValue.ToString("x"));
                SendQueue.Enqueue(b);
            }

            b = new byte[5];

            b[0] = 200;
            switch (dir)
            {
                case 0:
                    {
                        b[1] = 0x80;
                        break;
                    }
                case 1:
                    {
                        b[1] = 0x40;
                        break;
                    }
                case -1:
                    {
                        b[1] = 0x00;
                        break;
                    }
                default:
                    break;
            }
            if (CmdNum == 0)
            {
                b[2] = 0x01;
            }
            else
            {
                b[2] = (byte)(CmdNum << 2);
            }
            //CtrlMode:控制方式：1位移控，2力控，3变形控
            switch (CtrlMode)
            {
                case 1:
                    {
                        b[3] = 2;
                        break;
                    }
                case 2:
                    {
                        b[3] = 0;
                        break;
                    }
                case 3:
                    {
                        b[3] = 1;
                        break;
                    }
                default:
                    break;
            }
            //EndMode:结束方式：0时间，1位移，2力，3变形
            switch (EndMode)
            {
                case 0:
                    {
                        b[4] = 3;
                        break;
                    }
                case 1:
                    {
                        b[4] = 2;
                        break;
                    }
                case 2:
                    {
                        b[4] = 0;
                        break;
                    }
                case 3:
                    {
                        b[4] = 1;
                        break;
                    }
                default:
                    break;
            }
            if (CtrlSpeed_ == 0)
            {
                b[4] = 3;
            }
            SendQueue.Enqueue(b);
            return true;
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
