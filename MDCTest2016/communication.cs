using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MDCTest2016
{
    class Communication
    {
        /// <summary>
        /// 当前通讯方式，0-USB2.0通讯，1-HID通讯，2-WIFI通讯
        /// </summary>
        public static int CommunicationWay = 0;

        public static void EstablishCommunication(int i)
        {
            CommunicationWay = i;
            Init();
            Open();
        }

        public static void Close()
        {
            switch (CommunicationWay)
            {
                case 0:
                    {
                        UsbCommunication.Close();
                        break;
                    }
                default:
                    {
                        UsbCommunication.Close();
                        break;
                    }
            }
        }

        public static void Exit()
        {
            switch (CommunicationWay)
            {
                case 0:
                    {
                        UsbCommunication.Exit();
                        break;
                    }
                default:
                    {
                        UsbCommunication.Exit();
                        break;
                    }
            }
        }

        public static void Init()
        {
            switch (CommunicationWay)
            {
                case 0:
                    {
                        UsbCommunication.Init();
                        break;
                    }
                default:
                    {
                        UsbCommunication.Init();
                        break;
                    }
            }
        }

        public static void Open()
        {
            switch (CommunicationWay)
            {
                case 0:
                    {
                        UsbCommunication.OpenPort();
                        break;
                    }
                default:
                    {
                        UsbCommunication.OpenPort();
                        break;
                    }
            }
        }

        public static bool IsOpen()
        {
            switch (CommunicationWay)
            {
                case 0:
                    {
                        if (UsbCommunication.IsOpen()==1)
                        {
                            return true;
                        }
                        break;
                    }
                default:
                    {
                        if (UsbCommunication.IsOpen() == 1)
                        {
                            return true;
                        }
                        break;
                    }
            }
            return false;
        }

        public static void Write(byte[] SendByte)
        {
            switch (CommunicationWay)
            {
                case 0:
                    {
                        UsbCommunication.Write(SendByte);
                        break;
                    }
                default:
                    {
                        UsbCommunication.Write(SendByte);
                        break;
                    }
            }
        }

        public static void Read(byte[] GetByte)
        {
            switch (CommunicationWay)
            {
                case 0:
                    {
                        UsbCommunication.Read(GetByte);
                        break;
                    }
                default:
                    {
                        UsbCommunication.Read(GetByte);
                        break;
                    }
            }
        }

        #region DSP浮点数转换DLL的调用

        /// <summary>
        /// 将普通浮点数转化为dsp浮点
        /// </summary>
        /// <param name="i">普通浮点数</param>
        /// <returns>DSP浮点数，暂时用4Byte大小的int来保存此数据</returns>
        [DllImport("DSPFLT.dll")]
        public static extern int FLTtoDSP(float i);

        /// <summary>
        /// 将dsp浮点转化为普通浮点
        /// </summary>
        /// <param name="i">DSP浮点数，用int类型的保存的</param>
        /// <returns>普通浮点</returns>
        [DllImport("DSPFLT.dll")]
        public static extern float DSPtoFLT(int i);
        #endregion
    }

    class UsbCommunication
    {

        /// <summary>
        /// 初始化并打开miniport的连接
        /// </summary>
        /// <returns>-1失败，0-USB通讯，1-HID通讯，2-TCP通讯</returns>
        [DllImport("miniport.dll")]
        public static extern int Init();

        /// <summary>
        /// 判断连接是否开着
        /// </summary>
        /// <returns>1-开，0-关</returns>
        [DllImport("miniport.dll")]
        public static extern int IsOpen();

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns>1-成功，0-失败</returns>
        [DllImport("miniport.dll")]
        public static extern int Close();

        /// <summary>
        /// 打开miniport连接
        /// </summary>
        /// <returns>1-成功，0-失败</returns>
        [DllImport("miniport.dll")]
        public static extern int OpenPort();

        /// <summary>
        /// 从miniport读取数据
        /// </summary>
        /// <param name="GetByte">大小至少为64的byte数组</param>
        /// <returns></returns>
        [DllImport("miniport.dll")]
        public static extern int Read(byte[] GetByte);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="SendByte">大小必须为5的byte数组</param>
        /// <returns></returns>
        [DllImport("miniport.dll")]
        public static extern int Write(byte[] SendByte);

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns>1-成功，2-失败</returns>
        [DllImport("miniport.dll")]
        public static extern int Exit();

    }
}
